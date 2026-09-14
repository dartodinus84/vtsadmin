using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using vtsadm.App_Code;

namespace vtsadm
{
    /// <summary>
    /// IT Support data access for dashboard_assign_job_itsupport (mst_itsupport + ref_support_area).
    /// </summary>
    public static class ItsSupportAssignData
    {
        private const string AreaGroupWestValue = "ARG0000001";
        private const string AreaGroupEastValue = "ARG0000002";

        public static DataTable LoadAuthUsers(string filterAreaGroupId = "")
        {
            string requiredCanon = ResolveCanonicalAreaGroupId(filterAreaGroupId);
            if (!string.IsNullOrWhiteSpace(requiredCanon))
            {
                DataTable sqlFiltered = LoadAuthUsersFromSql(requiredCanon);
                if (sqlFiltered != null && sqlFiltered.Rows.Count > 0)
                {
                    return sqlFiltered;
                }
            }

            DataTable allUsers = LoadAuthUsersUnfiltered();
            if (allUsers == null || allUsers.Rows.Count == 0)
            {
                return allUsers ?? new DataTable();
            }

            if (string.IsNullOrWhiteSpace(requiredCanon))
            {
                return allUsers;
            }

            Dictionary<string, string> supAreaToGroup = LoadSupAreaToAreaGroupMap();
            Dictionary<string, string> areaGroupNames = LoadAreaGroupNameMap();
            HashSet<string> allowedSupAreaIds = LoadSupAreaIdsForCanonicalGroup(requiredCanon, supAreaToGroup, areaGroupNames);
            DataTable filtered = allUsers.Clone();
            EnsureAreaColumns(filtered);
            if (!filtered.Columns.Contains("AreaGroupName"))
            {
                filtered.Columns.Add("AreaGroupName", typeof(string));
            }

            foreach (DataRow row in allUsers.Rows)
            {
                if (RowMatchesAreaGroup(row, requiredCanon, supAreaToGroup, areaGroupNames, allowedSupAreaIds))
                {
                    CopyAuthUserRow(row, filtered);
                }
            }

            return filtered;
        }

        public static bool RowMatchesAreaGroup(
            DataRow userRow,
            string requiredAreaGroupId,
            Dictionary<string, string> supAreaToGroup = null,
            Dictionary<string, string> areaGroupNames = null,
            HashSet<string> allowedSupAreaIds = null)
        {
            string requiredCanon = ResolveCanonicalAreaGroupId(requiredAreaGroupId);
            if (string.IsNullOrWhiteSpace(requiredCanon))
            {
                return true;
            }

            if (userRow == null)
            {
                return false;
            }

            if (supAreaToGroup == null)
            {
                supAreaToGroup = LoadSupAreaToAreaGroupMap();
            }

            if (areaGroupNames == null)
            {
                areaGroupNames = LoadAreaGroupNameMap();
            }

            if (allowedSupAreaIds == null)
            {
                allowedSupAreaIds = LoadSupAreaIdsForCanonicalGroup(requiredCanon, supAreaToGroup, areaGroupNames);
            }

            string rowGroupId = GetRowString(userRow, "AreaGroupID");
            string rowGroupName = GetRowString(userRow, "AreaGroupName");
            if (AreaGroupIdsEquivalent(requiredCanon, rowGroupId, rowGroupName))
            {
                return true;
            }

            string resolvedGroup = ResolveCanonicalAreaGroupId(rowGroupId, rowGroupName);
            if (!string.IsNullOrWhiteSpace(resolvedGroup)
                && requiredCanon.Equals(resolvedGroup, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string supAreaId = NormalizeSupAreaId(
                FirstNonEmpty(GetRowString(userRow, "SupAreaID"), GetRowString(userRow, "SupportAreaID")));
            if (!string.IsNullOrWhiteSpace(supAreaId))
            {
                if (allowedSupAreaIds.Contains(supAreaId))
                {
                    return true;
                }

                string mappedGroupId;
                if (supAreaToGroup.TryGetValue(supAreaId, out mappedGroupId))
                {
                    string mappedGroupName;
                    areaGroupNames.TryGetValue(mappedGroupId, out mappedGroupName);
                    if (AreaGroupIdsEquivalent(requiredCanon, mappedGroupId, mappedGroupName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static DataTable LoadAuthUsersFromSql(string requiredCanon)
        {
            string areaPredicate = BuildAreaGroupSqlPredicate(requiredCanon);
            if (string.IsNullOrWhiteSpace(areaPredicate))
            {
                return new DataTable();
            }

            string[] queries =
            {
                BuildPrimaryAuthUsersSql(areaPredicate),
                BuildPrimaryAuthUsersSql(areaPredicate, includeAreaGroupStatus: true)
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(sql);
                if (table != null && table.Rows.Count > 0)
                {
                    PrepareAuthUsersTable(table);
                    return table;
                }
            }

            return new DataTable();
        }

        private static DataTable LoadAuthUsersUnfiltered()
        {
            string[] queries =
            {
                BuildPrimaryAuthUsersSql(string.Empty),
                BuildPrimaryAuthUsersSql(string.Empty, includeAreaGroupStatus: true),
                BuildAuthUsersQuery(includeSupportAreaColumn: false, includeAreaGroupJoin: true, statusOnJoin: false),
                BuildAuthUsersQuery(includeSupportAreaColumn: false, includeAreaGroupJoin: false, statusOnJoin: false)
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(sql);
                if (table != null && table.Rows.Count > 0)
                {
                    PrepareAuthUsersTable(table);
                    return table;
                }
            }

            return new DataTable();
        }

        private static void PrepareAuthUsersTable(DataTable table)
        {
            EnsureAreaColumns(table);
            if (!table.Columns.Contains("AreaGroupName"))
            {
                table.Columns.Add("AreaGroupName", typeof(string));
            }

            EnrichAuthUsersAreaGroup(table);
        }

        private static string GetItSupportSupAreaSqlExpression()
        {
            return "LTRIM(RTRIM(COALESCE("
                + "NULLIF(LTRIM(RTRIM(ISNULL(it.SupAreaID, ''))), ''), "
                + "NULLIF(LTRIM(RTRIM(ISNULL(it.SupportAreaID, ''))), ''))))";
        }

        private static string BuildPrimaryAuthUsersSql(string areaPredicate, bool includeAreaGroupStatus = false)
        {
            string supAreaExpr = GetItSupportSupAreaSqlExpression();
            string areaGroupJoin = "LEFT JOIN ref_area_group ag WITH (NOLOCK) "
                + "ON LTRIM(RTRIM(ISNULL(ag.AreaGroupID, ''))) = LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) ";
            if (includeAreaGroupStatus)
            {
                areaGroupJoin += "AND ISNULL(ag.Status, '') IN ('RG', '') ";
            }

            return "SELECT "
                + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                + "'ITS' AS GroupID, "
                + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                + supAreaExpr + " AS SupAreaID, "
                + supAreaExpr + " AS SupportAreaID, "
                + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID, "
                + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName "
                + "FROM mst_itsupport it WITH (NOLOCK) "
                + "LEFT JOIN ref_support_area sa WITH (NOLOCK) "
                + "ON LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) = " + supAreaExpr + " "
                + areaGroupJoin
                + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                + areaPredicate
                + "ORDER BY ISNULL(it.Name, it.UserID)";
        }

        private static string BuildAreaGroupSqlPredicate(string requiredCanon)
        {
            if (string.IsNullOrWhiteSpace(requiredCanon))
            {
                return string.Empty;
            }

            if (requiredCanon.Equals(AreaGroupWestValue, StringComparison.OrdinalIgnoreCase))
            {
                return "AND (LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) = '" + AreaGroupWestValue + "' "
                    + "OR UPPER(LTRIM(RTRIM(ISNULL(ag.AreaGroupName, '')))) LIKE '%WEST%') ";
            }

            if (requiredCanon.Equals(AreaGroupEastValue, StringComparison.OrdinalIgnoreCase))
            {
                return "AND (LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) = '" + AreaGroupEastValue + "' "
                    + "OR UPPER(LTRIM(RTRIM(ISNULL(ag.AreaGroupName, '')))) LIKE '%EAST%') ";
            }

            string safeCanon = EscapeSqlLiteral(requiredCanon);
            return "AND LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) = '" + safeCanon + "' ";
        }

        private static string BuildAuthUsersQuery(bool includeSupportAreaColumn, bool includeAreaGroupJoin, bool statusOnJoin)
        {
            string supAreaExpr = includeSupportAreaColumn
                ? "LTRIM(RTRIM(COALESCE(NULLIF(LTRIM(RTRIM(ISNULL(it.SupAreaID, ''))), ''), NULLIF(LTRIM(RTRIM(ISNULL(it.SupportAreaID, ''))), ''))))"
                : "LTRIM(RTRIM(ISNULL(it.SupAreaID, '')))";

            string joinOn = "ON LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) = " + supAreaExpr;
            if (statusOnJoin)
            {
                joinOn += " AND ISNULL(sa.Status, '') IN ('RG', '')";
            }

            string areaGroupSelect = includeAreaGroupJoin
                ? "LTRIM(RTRIM(COALESCE(NULLIF(LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))), ''), NULLIF(LTRIM(RTRIM(ISNULL(ag.AreaGroupID, ''))), '')))) AS AreaGroupID, "
                    + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName "
                : "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID, "
                    + "'' AS AreaGroupName ";

            string areaGroupJoin = includeAreaGroupJoin
                ? "LEFT JOIN ref_area_group ag WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(ag.AreaGroupID, ''))) = LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) "
                : string.Empty;

            return "SELECT "
                + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) AS ITID, "
                + "LTRIM(RTRIM(ISNULL(it.UserID, ''))) AS UserID, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS FullName, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS Name, "
                + "'ITS' AS GroupID, "
                + "LTRIM(RTRIM(ISNULL(it.Status, ''))) AS Status, "
                + supAreaExpr + " AS SupAreaID, "
                + supAreaExpr + " AS SupportAreaID, "
                + areaGroupSelect
                + "FROM mst_itsupport it WITH (NOLOCK) "
                + "LEFT JOIN ref_support_area sa WITH (NOLOCK) "
                + joinOn + " "
                + areaGroupJoin
                + "WHERE ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                + "ORDER BY ISNULL(it.Name, it.UserID)";
        }

        private static HashSet<string> LoadSupAreaIdsForCanonicalGroup(
            string requiredCanon,
            Dictionary<string, string> supAreaToGroup,
            Dictionary<string, string> areaGroupNames)
        {
            HashSet<string> ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(requiredCanon) || supAreaToGroup == null)
            {
                return ids;
            }

            foreach (KeyValuePair<string, string> pair in supAreaToGroup)
            {
                string mappedName;
                areaGroupNames.TryGetValue(pair.Value, out mappedName);
                if (ResolveCanonicalAreaGroupId(pair.Value, mappedName)
                    .Equals(requiredCanon, StringComparison.OrdinalIgnoreCase))
                {
                    ids.Add(pair.Key);
                }
            }

            return ids;
        }

        private static void CopyAuthUserRow(DataRow source, DataTable target)
        {
            DataRow targetRow = target.NewRow();
            foreach (DataColumn column in source.Table.Columns)
            {
                if (!target.Columns.Contains(column.ColumnName))
                {
                    continue;
                }

                targetRow[column.ColumnName] = source[column.ColumnName];
            }

            target.Rows.Add(targetRow);
        }

        private static string NormalizeSupAreaId(string supAreaId)
        {
            string normalized = (supAreaId ?? string.Empty).Trim();
            if (normalized.Equals("[SELECT]", StringComparison.OrdinalIgnoreCase)
                || normalized.Equals("-", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return normalized;
        }

        private static void EnrichAuthUsersAreaGroup(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            Dictionary<string, string> supAreaToGroup = LoadSupAreaToAreaGroupMap();
            Dictionary<string, string> areaGroupNames = LoadAreaGroupNameMap();
            foreach (DataRow row in table.Rows)
            {
                string areaGroupId = GetRowString(row, "AreaGroupID").Trim();
                string areaGroupName = GetRowString(row, "AreaGroupName").Trim();
                if (string.IsNullOrWhiteSpace(areaGroupName)
                    && !string.IsNullOrWhiteSpace(areaGroupId)
                    && areaGroupNames.TryGetValue(areaGroupId, out areaGroupName)
                    && !string.IsNullOrWhiteSpace(areaGroupName))
                {
                    row["AreaGroupName"] = areaGroupName.Trim();
                }

                if (!string.IsNullOrWhiteSpace(areaGroupId))
                {
                    continue;
                }

                string supAreaId = NormalizeSupAreaId(
                    FirstNonEmpty(GetRowString(row, "SupAreaID"), GetRowString(row, "SupportAreaID")));
                if (string.IsNullOrWhiteSpace(supAreaId))
                {
                    continue;
                }

                string mappedGroup;
                if (supAreaToGroup.TryGetValue(supAreaId, out mappedGroup)
                    && !string.IsNullOrWhiteSpace(mappedGroup))
                {
                    row["AreaGroupID"] = mappedGroup.Trim();
                    string mappedName;
                    if (areaGroupNames.TryGetValue(mappedGroup.Trim(), out mappedName)
                        && !string.IsNullOrWhiteSpace(mappedName))
                    {
                        row["AreaGroupName"] = mappedName.Trim();
                    }
                }
            }
        }

        public static string LookupItId(string userIdOrItId)
        {
            string safeValue = EscapeSqlLiteral((userIdOrItId ?? string.Empty).Trim());
            if (string.IsNullOrWhiteSpace(safeValue))
            {
                return string.Empty;
            }

            string[] queries =
            {
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(ITID, ''))) AS ITID "
                    + "FROM mst_itsupport WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(UserID, ''))) = '" + safeValue + "' "
                    + "AND ISNULL(Status, '') NOT IN ('DE', 'BL') "
                    + "ORDER BY DtmUpd DESC",
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(ITID, ''))) AS ITID "
                    + "FROM mst_itsupport WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(ITID, ''))) = '" + safeValue + "' "
                    + "AND ISNULL(Status, '') NOT IN ('DE', 'BL') "
                    + "ORDER BY DtmUpd DESC",
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(ITID, ''))) AS ITID "
                    + "FROM mst_itsupport WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(UserID, ''))) = '" + safeValue + "' "
                    + "ORDER BY DtmUpd DESC",
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(ITID, ''))) AS ITID "
                    + "FROM mst_itsupport WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(ITID, ''))) = '" + safeValue + "' "
                    + "ORDER BY DtmUpd DESC"
            };

            foreach (string sql in queries)
            {
                DataTable rows = ExecuteQuery(sql);
                if (rows != null && rows.Rows.Count > 0)
                {
                    string itId = GetRowString(rows.Rows[0], "ITID");
                    if (!string.IsNullOrWhiteSpace(itId))
                    {
                        return itId.Trim();
                    }
                }
            }

            return string.Empty;
        }

        public static Dictionary<string, string> LoadTrainingMarketingMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string[] queries =
            {
                "SELECT LTRIM(RTRIM(t.TrainingID)) AS TrainingID, "
                    + "LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName "
                    + "FROM trx_training_order t WITH (NOLOCK) "
                    + "LEFT JOIN mst_customer c WITH (NOLOCK) ON c.CustID = t.CustID "
                    + "LEFT JOIN mst_marketing m WITH (NOLOCK) ON m.MarketingID = c.MarketingID "
                    + "WHERE LTRIM(RTRIM(ISNULL(t.TrainingID, ''))) <> ''",
                "SELECT LTRIM(RTRIM(t.TrainingID)) AS TrainingID, "
                    + "LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName "
                    + "FROM trx_training_order t WITH (NOLOCK) "
                    + "LEFT JOIN mst_customer c WITH (NOLOCK) ON LTRIM(RTRIM(c.CustID)) = LTRIM(RTRIM(t.CustID)) "
                    + "LEFT JOIN mst_marketing m WITH (NOLOCK) ON m.MarketingID = c.MarketingID "
                    + "WHERE LTRIM(RTRIM(ISNULL(t.TrainingID, ''))) <> ''"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(sql);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                foreach (DataRow row in table.Rows)
                {
                    string trainingId = GetRowString(row, "TrainingID");
                    string marketingName = GetRowString(row, "MarketingName");
                    if (string.IsNullOrWhiteSpace(trainingId) || string.IsNullOrWhiteSpace(marketingName))
                    {
                        continue;
                    }

                    if (!map.ContainsKey(trainingId))
                    {
                        map[trainingId] = marketingName;
                    }
                }

                if (map.Count > 0)
                {
                    break;
                }
            }

            return map;
        }

        public static Dictionary<string, string> LoadCustomerMarketingMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string[] queries =
            {
                "SELECT LTRIM(RTRIM(c.CustID)) AS CustID, "
                    + "LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName "
                    + "FROM mst_customer c WITH (NOLOCK) "
                    + "LEFT JOIN mst_marketing m WITH (NOLOCK) ON m.MarketingID = c.MarketingID "
                    + "WHERE LTRIM(RTRIM(ISNULL(c.CustID, ''))) <> '' "
                    + "AND LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) NOT IN ('', '[SELECT]')",
                "SELECT LTRIM(RTRIM(c.CustID)) AS CustID, "
                    + "LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName "
                    + "FROM mst_customer c WITH (NOLOCK) "
                    + "LEFT JOIN mst_marketing m WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(m.MarketingID, ''))) = LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) "
                    + "WHERE LTRIM(RTRIM(ISNULL(c.CustID, ''))) <> '' "
                    + "AND LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) NOT IN ('', '[SELECT]')"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(sql);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                foreach (DataRow row in table.Rows)
                {
                    string custId = GetRowString(row, "CustID");
                    string marketingName = GetRowString(row, "MarketingName");
                    if (string.IsNullOrWhiteSpace(custId) || string.IsNullOrWhiteSpace(marketingName))
                    {
                        continue;
                    }

                    if (!map.ContainsKey(custId))
                    {
                        map[custId] = marketingName;
                    }
                }

                if (map.Count > 0)
                {
                    break;
                }
            }

            return map;
        }

        public static string ResolveMarketingName(string custId, Dictionary<string, string> marketingByCustId)
        {
            if (string.IsNullOrWhiteSpace(custId) || marketingByCustId == null || marketingByCustId.Count == 0)
            {
                return string.Empty;
            }

            string marketingName;
            if (marketingByCustId.TryGetValue(custId.Trim(), out marketingName))
            {
                return marketingName;
            }

            return string.Empty;
        }

        public sealed class CustomerDeviceCounts
        {
            public int Gps { get; set; }
            public int Acs { get; set; }
        }

        public static Dictionary<string, CustomerDeviceCounts> LoadCustomerGpsAcsCountMap()
        {
            Dictionary<string, CustomerDeviceCounts> map =
                new Dictionary<string, CustomerDeviceCounts>(StringComparer.OrdinalIgnoreCase);

            string[] queries =
            {
                "sp_get_customer_gps_acs_counts ''",
                "SELECT "
                    + "LTRIM(RTRIM(c.CustID)) AS CustID, "
                    + "ISNULL(gps.TotalGps, 0) AS TotalGps, "
                    + "0 AS TotalAcs "
                    + "FROM mst_customer c WITH (NOLOCK) "
                    + "LEFT JOIN mst_customer_server cs WITH (NOLOCK) "
                    + "ON cs.CustID = c.CustID "
                    + "AND cs.ServerID = 'SVR0000001' "
                    + "AND ISNULL(cs.Status, '') <> 'DE' "
                    + "LEFT JOIN gpsb.dbo.customer g WITH (NOLOCK) "
                    + "ON g.company_id = cs.MIS_CustID "
                    + "OUTER APPLY ("
                    + "SELECT COUNT(*) AS TotalGps "
                    + "FROM gpsb.dbo.car_master cm WITH (NOLOCK) "
                    + "WHERE cm.company_id = g.company_id"
                    + ") gps "
                    + "WHERE ISNULL(c.Status, '') NOT IN ('DE', 'BL')"
            };

            foreach (string sql in queries)
            {
                try
                {
                    DataTable table = ExecuteQuery(sql);
                    if (table == null || table.Rows.Count == 0)
                    {
                        continue;
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        string custId = GetRowString(row, "CustID");
                        if (string.IsNullOrWhiteSpace(custId))
                        {
                            continue;
                        }

                        int gpsCount = ParseCount(row, "TotalGps", "CustomerGps", "GpsCount");
                        int acsCount = ParseCount(row, "TotalAcs", "CustomerAcs", "AcsCount");
                        map[custId] = new CustomerDeviceCounts
                        {
                            Gps = gpsCount,
                            Acs = acsCount
                        };
                    }

                    if (map.Count > 0)
                    {
                        break;
                    }
                }
                catch
                {
                    // Try next query source.
                }
            }

            return map;
        }

        private static int ParseCount(DataRow row, params string[] columnNames)
        {
            if (row == null || row.Table == null || columnNames == null)
            {
                return 0;
            }

            foreach (string columnName in columnNames)
            {
                if (string.IsNullOrWhiteSpace(columnName) || !row.Table.Columns.Contains(columnName))
                {
                    continue;
                }

                object raw = row[columnName];
                if (raw == null || raw == DBNull.Value)
                {
                    continue;
                }

                int parsed;
                if (int.TryParse(Convert.ToString(raw), out parsed))
                {
                    return Math.Max(0, parsed);
                }
            }

            return 0;
        }

        public static Dictionary<string, string> LoadSupAreaToAreaGroupMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string[] queries =
            {
                "SELECT "
                + "LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) AS SupAreaID, "
                + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID "
                + "FROM ref_support_area sa WITH (NOLOCK) "
                + "WHERE LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) <> '' "
                + "AND LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) <> ''",
                "SELECT "
                + "LTRIM(RTRIM(ISNULL(sa.SupAreaID, ''))) AS SupAreaID, "
                + "LTRIM(RTRIM(ISNULL(sa.AreaGroupID, ''))) AS AreaGroupID "
                + "FROM ref_support_area sa WITH (NOLOCK) "
                + "WHERE ISNULL(sa.Status, '') IN ('RG', '')"
            };

            foreach (string sql in queries)
            {
                DataTable refAreas = ExecuteQuery(sql);
                if (refAreas == null || refAreas.Rows.Count == 0)
                {
                    continue;
                }

                foreach (DataRow row in refAreas.Rows)
                {
                    string supAreaId = FirstNonEmpty(GetRowString(row, "SupAreaID"), GetRowString(row, "SupportAreaID")).Trim();
                    string areaGroupId = FirstNonEmpty(
                        GetRowString(row, "AreaGroupID"),
                        GetRowString(row, "GroupSupportAreaID"),
                        GetRowString(row, "GroupID")).Trim();
                    if (string.IsNullOrWhiteSpace(supAreaId) || string.IsNullOrWhiteSpace(areaGroupId) || map.ContainsKey(supAreaId))
                    {
                        continue;
                    }

                    map[supAreaId] = areaGroupId;
                }

                if (map.Count > 0)
                {
                    break;
                }
            }

            return map;
        }

        public static Dictionary<string, string> LoadAreaGroupNameMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            DataTable groups = ExecuteQuery(
                "SELECT "
                + "LTRIM(RTRIM(ISNULL(ag.AreaGroupID, ''))) AS AreaGroupID, "
                + "LTRIM(RTRIM(ISNULL(ag.AreaGroupName, ''))) AS AreaGroupName "
                + "FROM ref_area_group ag WITH (NOLOCK) "
                + "WHERE LTRIM(RTRIM(ISNULL(ag.AreaGroupID, ''))) <> ''");
            if (groups == null || groups.Rows.Count == 0)
            {
                return map;
            }

            foreach (DataRow row in groups.Rows)
            {
                string areaGroupId = GetRowString(row, "AreaGroupID");
                string areaGroupName = GetRowString(row, "AreaGroupName");
                if (string.IsNullOrWhiteSpace(areaGroupId) || map.ContainsKey(areaGroupId))
                {
                    continue;
                }

                map[areaGroupId] = areaGroupName;
            }

            return map;
        }

        public static string ResolveCanonicalAreaGroupId(string areaGroupId, string areaGroupName = "")
        {
            string normalizedId = (areaGroupId ?? string.Empty).Trim();
            string normalizedName = (areaGroupName ?? string.Empty).Trim().ToUpperInvariant();
            string upperId = normalizedId.ToUpperInvariant();

            if (upperId.Equals(AreaGroupWestValue, StringComparison.OrdinalIgnoreCase)
                || upperId.Contains("WEST")
                || normalizedName.Contains("WEST"))
            {
                return AreaGroupWestValue;
            }

            if (upperId.Equals(AreaGroupEastValue, StringComparison.OrdinalIgnoreCase)
                || upperId.Contains("EAST")
                || normalizedName.Contains("EAST"))
            {
                return AreaGroupEastValue;
            }

            return normalizedId;
        }

        private static bool AreaGroupIdsEquivalent(string requiredCanon, string candidateId, string candidateName)
        {
            string candidateCanon = ResolveCanonicalAreaGroupId(candidateId, candidateName);
            return !string.IsNullOrWhiteSpace(requiredCanon)
                && !string.IsNullOrWhiteSpace(candidateCanon)
                && requiredCanon.Equals(candidateCanon, StringComparison.OrdinalIgnoreCase);
        }

        private static void EnsureAreaColumns(DataTable table)
        {
            if (table == null)
            {
                return;
            }

            if (!table.Columns.Contains("SupAreaID"))
            {
                table.Columns.Add("SupAreaID", typeof(string));
            }
            if (!table.Columns.Contains("SupportAreaID"))
            {
                table.Columns.Add("SupportAreaID", typeof(string));
            }
            if (!table.Columns.Contains("AreaGroupID"))
            {
                table.Columns.Add("AreaGroupID", typeof(string));
            }
        }

        private static DataTable ExecuteQuery(string sql)
        {
            return dashboard_assign_job.ExecuteJobTrainingQuery(sql);
        }

        private static string EscapeSqlLiteral(string value)
        {
            return (value ?? string.Empty).Replace("'", "''");
        }

        private static string GetRowString(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
            {
                return string.Empty;
            }

            object value = row[columnName];
            return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value).Trim();
        }

        private static string FirstNonEmpty(params string[] values)
        {
            if (values == null)
            {
                return string.Empty;
            }

            foreach (string value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }

            return string.Empty;
        }
    }
}
