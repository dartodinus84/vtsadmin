using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
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

        public sealed class CustomerDeviceCounts
        {
            public int TotalGps { get; set; }
            public int TotalAcs { get; set; }
        }

        public static Dictionary<string, string> LoadItSupportNameMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            DataTable table = ExecuteQuery(
                "SELECT LTRIM(RTRIM(ISNULL(ITID, ''))) AS ITID, "
                + "LTRIM(RTRIM(ISNULL(UserID, ''))) AS UserID, "
                + "LTRIM(RTRIM(ISNULL(Name, ''))) AS Name "
                + "FROM mst_itsupport WITH (NOLOCK) "
                + "WHERE ISNULL(Status, '') NOT IN ('DE', 'BL')");
            if (table == null || table.Rows.Count == 0)
            {
                return map;
            }

            foreach (DataRow row in table.Rows)
            {
                string name = FirstNonEmpty(GetRowString(row, "Name"), GetRowString(row, "ITID"));
                string itId = GetRowString(row, "ITID");
                string userId = GetRowString(row, "UserID");
                if (!string.IsNullOrWhiteSpace(itId))
                {
                    map[itId] = name;
                }

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    map[userId] = name;
                }
            }

            return map;
        }

        public static string ResolveItSupportName(string technicianId, Dictionary<string, string> nameMap)
        {
            string key = (technicianId ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(key) || nameMap == null || nameMap.Count == 0)
            {
                return key;
            }

            string name;
            if (nameMap.TryGetValue(key, out name) && !string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            return key;
        }

        public static Dictionary<string, CustomerDeviceCounts> LoadCustomerGpsCountMap()
        {
            Dictionary<string, CustomerDeviceCounts> map =
                new Dictionary<string, CustomerDeviceCounts>(StringComparer.OrdinalIgnoreCase);
            DataTable table = ExecuteQuery("sp_get_customer_gps_acs_counts ''");
            if (table == null || table.Rows.Count == 0)
            {
                return map;
            }

            foreach (DataRow row in table.Rows)
            {
                string custId = GetRowString(row, "CustID");
                if (string.IsNullOrWhiteSpace(custId))
                {
                    continue;
                }

                map[custId] = new CustomerDeviceCounts
                {
                    TotalGps = ParseIntColumnAny(row, "TotUnit", "TotalGps", "TotalGPS"),
                    TotalAcs = 0
                };
            }

            return map;
        }

        private static int ParseIntColumn(DataRow row, string columnName)
        {
            return ParseIntColumnAny(row, columnName);
        }

        private static int ParseIntColumnAny(DataRow row, params string[] columnNames)
        {
            if (row == null || row.Table == null || columnNames == null || columnNames.Length == 0)
            {
                return 0;
            }

            foreach (string columnName in columnNames)
            {
                if (string.IsNullOrWhiteSpace(columnName) || !row.Table.Columns.Contains(columnName))
                {
                    continue;
                }

                object value = row[columnName];
                if (value == null || value == DBNull.Value)
                {
                    continue;
                }

                int parsed;
                if (int.TryParse(Convert.ToString(value), out parsed))
                {
                    return parsed;
                }
            }

            return 0;
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
            return QueryDataTable(sql);
        }

        public static bool IsOpenItSupportAssignStatus(string status)
        {
            string normalized = (status ?? string.Empty).Trim().ToUpperInvariant();
            return normalized == "RG";
        }

        public static bool IsValidItSupportTechnicianId(string technicianId)
        {
            string normalized = (technicianId ?? string.Empty).Trim().ToUpperInvariant();
            if (normalized.Length < 3 || !normalized.StartsWith("IT", StringComparison.Ordinal))
            {
                return false;
            }

            for (int i = 2; i < normalized.Length; i++)
            {
                if (!char.IsDigit(normalized[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsAssignedItSupportTechnician(
            string technicianId,
            Dictionary<string, string> itSupportNames)
        {
            string raw = (technicianId ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(raw)
                || raw.Equals("UNASSIGNED", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (IsValidItSupportTechnicianId(raw))
            {
                return true;
            }

            if (itSupportNames != null && itSupportNames.ContainsKey(raw))
            {
                return true;
            }

            string resolvedItId = LookupItId(raw);
            return IsValidItSupportTechnicianId(resolvedItId);
        }

        private static string BuildAssignedItSupportTechnicianSqlFilter(string detailAlias)
        {
            string alias = string.IsNullOrWhiteSpace(detailAlias) ? "d" : detailAlias.Trim();
            return "AND LTRIM(RTRIM(ISNULL(" + alias + ".TechnicianID, ''))) <> '' "
                + "AND UPPER(LTRIM(RTRIM(ISNULL(" + alias + ".TechnicianID, '')))) <> 'UNASSIGNED' "
                + "AND EXISTS ("
                + "SELECT 1 FROM mst_itsupport it WITH (NOLOCK) "
                + "WHERE ("
                + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) = LTRIM(RTRIM(ISNULL(" + alias + ".TechnicianID, ''))) "
                + "OR LTRIM(RTRIM(ISNULL(it.UserID, ''))) = LTRIM(RTRIM(ISNULL(" + alias + ".TechnicianID, '')))"
                + ") AND ISNULL(it.Status, '') NOT IN ('DE', 'BL')"
                + ") ";
        }

        private static string BuildItSupportTrainingOrderSqlFilter(string detailAlias)
        {
            string alias = string.IsNullOrWhiteSpace(detailAlias) ? "d" : detailAlias.Trim();
            return "AND EXISTS ("
                + "SELECT 1 FROM trx_training_order t WITH (NOLOCK) "
                + "WHERE LTRIM(RTRIM(ISNULL(t.TrainingID, ''))) = LTRIM(RTRIM(ISNULL(" + alias + ".JobID, ''))) "
                + "AND ISNULL(t.Status, '') NOT IN ('DE')"
                + ") ";
        }

        private static string BuildOpenItSupportAssignStatusSqlFilter(string detailAlias)
        {
            string alias = string.IsNullOrWhiteSpace(detailAlias) ? "d" : detailAlias.Trim();
            return "WHERE UPPER(LTRIM(RTRIM(ISNULL(" + alias + ".Status, '')))) = 'RG' "
                + "AND LTRIM(RTRIM(ISNULL(" + alias + ".JobID, ''))) <> '' ";
        }

        public static string BuildOpenItSupportAssignListSql(int maxRows)
        {
            int safeMaxRows = maxRows < 1 ? 15 : Math.Min(maxRows, 100);
            return "SELECT TOP " + safeMaxRows.ToString()
                + " LTRIM(RTRIM(ISNULL(d.AssignID, ''))) AS AssignID, "
                + "ISNULL(d.Seq, 0) AS Seq, "
                + "LTRIM(RTRIM(ISNULL(d.JobID, ''))) AS JobID, "
                + "d.SchDate, "
                + "LTRIM(RTRIM(ISNULL(d.TechnicianID, ''))) AS TechnicianID, "
                + "LTRIM(RTRIM(ISNULL(d.Status, ''))) AS Status, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS TechnicianName, "
                + "LTRIM(RTRIM(ISNULL(c.FullName, ''))) AS CustomerName "
                + "FROM trx_job_assign_detail d WITH (NOLOCK) "
                + "LEFT JOIN mst_itsupport it WITH (NOLOCK) ON ("
                + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) = LTRIM(RTRIM(ISNULL(d.TechnicianID, ''))) "
                + "OR LTRIM(RTRIM(ISNULL(it.UserID, ''))) = LTRIM(RTRIM(ISNULL(d.TechnicianID, '')))"
                + ") AND ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                + "LEFT JOIN trx_job_assign_header h WITH (NOLOCK) "
                + "ON h.AssignID = d.AssignID AND ISNULL(h.Status, '') NOT IN ('DE') "
                + "LEFT JOIN trx_training_order t WITH (NOLOCK) "
                + "ON LTRIM(RTRIM(ISNULL(t.TrainingID, ''))) = LTRIM(RTRIM(ISNULL(d.JobID, ''))) "
                + "AND ISNULL(t.Status, '') NOT IN ('DE') "
                + "LEFT JOIN mst_customer c WITH (NOLOCK) "
                + "ON c.CustID = COALESCE(NULLIF(LTRIM(RTRIM(h.CustID)), ''), NULLIF(LTRIM(RTRIM(t.CustID)), '')) "
                + BuildOpenItSupportAssignStatusSqlFilter("d")
                + BuildAssignedItSupportTechnicianSqlFilter("d")
                + BuildItSupportTrainingOrderSqlFilter("d")
                + "ORDER BY d.SchDate DESC, d.JobID ASC";
        }

        public static string BuildOpenItSupportAssignBareSql(int maxRows)
        {
            int safeMaxRows = maxRows < 1 ? 15 : Math.Min(maxRows, 100);
            return "SELECT TOP " + safeMaxRows.ToString()
                + " LTRIM(RTRIM(ISNULL(d.AssignID, ''))) AS AssignID, "
                + "ISNULL(d.Seq, 0) AS Seq, "
                + "LTRIM(RTRIM(ISNULL(d.JobID, ''))) AS JobID, "
                + "d.SchDate, "
                + "LTRIM(RTRIM(ISNULL(d.TechnicianID, ''))) AS TechnicianID, "
                + "LTRIM(RTRIM(ISNULL(d.Status, ''))) AS Status, "
                + "LTRIM(RTRIM(ISNULL(it.Name, ''))) AS TechnicianName, "
                + "'' AS CustomerName "
                + "FROM trx_job_assign_detail d WITH (NOLOCK) "
                + "INNER JOIN mst_itsupport it WITH (NOLOCK) ON ("
                + "LTRIM(RTRIM(ISNULL(it.ITID, ''))) = LTRIM(RTRIM(ISNULL(d.TechnicianID, ''))) "
                + "OR LTRIM(RTRIM(ISNULL(it.UserID, ''))) = LTRIM(RTRIM(ISNULL(d.TechnicianID, '')))"
                + ") AND ISNULL(it.Status, '') NOT IN ('DE', 'BL') "
                + BuildOpenItSupportAssignStatusSqlFilter("d")
                + BuildItSupportTrainingOrderSqlFilter("d")
                + "ORDER BY d.SchDate DESC, d.JobID ASC";
        }

        public static string BuildOpenItSupportAssignListFallbackSql(int maxRows)
        {
            int safeMaxRows = maxRows < 1 ? 15 : Math.Min(maxRows, 100);
            return "SELECT TOP " + safeMaxRows.ToString()
                + " LTRIM(RTRIM(ISNULL(d.AssignID, ''))) AS AssignID, "
                + "ISNULL(d.Seq, 0) AS Seq, "
                + "LTRIM(RTRIM(ISNULL(d.JobID, ''))) AS JobID, "
                + "d.SchDate, "
                + "LTRIM(RTRIM(ISNULL(d.TechnicianID, ''))) AS TechnicianID, "
                + "LTRIM(RTRIM(ISNULL(d.Status, ''))) AS Status, "
                + "'' AS TechnicianName, "
                + "'' AS CustomerName "
                + "FROM trx_job_assign_detail d WITH (NOLOCK) "
                + BuildOpenItSupportAssignStatusSqlFilter("d")
                + BuildAssignedItSupportTechnicianSqlFilter("d")
                + BuildItSupportTrainingOrderSqlFilter("d")
                + "ORDER BY d.SchDate DESC, d.JobID ASC";
        }

        public static DataTable LoadOpenItSupportAssignRows(int maxRows)
        {
            return LoadOpenItSupportAssignRows(maxRows, null);
        }

        public static DataTable LoadOpenItSupportAssignRows(int maxRows, string connString)
        {
            string[] queries =
            {
                BuildOpenItSupportAssignListSql(maxRows),
                BuildOpenItSupportAssignBareSql(maxRows),
                BuildOpenItSupportAssignListFallbackSql(maxRows)
            };

            foreach (string sql in queries)
            {
                DataTable table = QueryDataTable(sql, connString);
                if (table != null && table.Rows.Count > 0)
                {
                    return table;
                }
            }

            return new DataTable();
        }

        private static bool IsOleDbConnectionString(string connString)
        {
            return !string.IsNullOrWhiteSpace(connString)
                && connString.IndexOf("provider=", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static DataTable TrySqlClientQuery(string sql, string connString)
        {
            string sqlConn = ResolveSqlClientConnectionString(connString);
            if (string.IsNullOrWhiteSpace(sqlConn))
            {
                return null;
            }

            try
            {
                DataTable table = new DataTable();
                using (SqlConnection conn = new SqlConnection(sqlConn))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 120;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                        }
                    }
                }

                return table;
            }
            catch
            {
                return null;
            }
        }

        private static DataTable TryRecordsetQuery(string sql, string connString)
        {
            string recordsetConn = ResolveRecordsetConnectionString(connString);
            if (string.IsNullOrWhiteSpace(recordsetConn) || !IsOleDbConnectionString(recordsetConn))
            {
                return null;
            }

            try
            {
                Recordset rec = new Recordset();
                string openError = string.Empty;
                rec.Open(sql, recordsetConn.Trim(), ref openError);
                if (!string.IsNullOrWhiteSpace(openError))
                {
                    return null;
                }

                return rec.DataRecord() ?? new DataTable();
            }
            catch
            {
                return null;
            }
        }

        public static DataTable QueryDataTable(string sql)
        {
            return QueryDataTable(sql, null);
        }

        public static DataTable QueryDataTable(string sql, string connString)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new DataTable();
            }

            string trimmed = sql.Trim();
            string recordsetConn = ResolveRecordsetConnectionString(connString);
            bool useRecordsetFirst = IsOleDbConnectionString(recordsetConn);

            DataTable recordsetTable = null;
            DataTable sqlTable = null;
            if (useRecordsetFirst)
            {
                recordsetTable = TryRecordsetQuery(trimmed, connString);
                if (recordsetTable != null && recordsetTable.Rows.Count > 0)
                {
                    return recordsetTable;
                }

                sqlTable = TrySqlClientQuery(trimmed, connString);
                if (sqlTable != null && sqlTable.Rows.Count > 0)
                {
                    return sqlTable;
                }
            }
            else
            {
                sqlTable = TrySqlClientQuery(trimmed, connString);
                if (sqlTable != null && sqlTable.Rows.Count > 0)
                {
                    return sqlTable;
                }

                recordsetTable = TryRecordsetQuery(trimmed, connString);
                if (recordsetTable != null && recordsetTable.Rows.Count > 0)
                {
                    return recordsetTable;
                }
            }

            if (recordsetTable != null)
            {
                return recordsetTable;
            }

            if (sqlTable != null)
            {
                return sqlTable;
            }

            return new DataTable();
        }

        public static string ResolveSqlClientConnectionString()
        {
            return ResolveSqlClientConnectionString(null);
        }

        public static string ResolveSqlClientConnectionString(string rawConnectionString)
        {
            try
            {
                ConnectionStringSettings sqlSettings = ConfigurationManager.ConnectionStrings["VTSADMIN"];
                if (sqlSettings != null && !string.IsNullOrWhiteSpace(sqlSettings.ConnectionString))
                {
                    return sqlSettings.ConnectionString.Trim();
                }
            }
            catch
            {
            }

            string trimmed = (rawConnectionString ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                HttpContext context = HttpContext.Current;
                if (context != null && context.Session != null)
                {
                    trimmed = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
                }
            }

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                ConnectionStringSettings oleDbSettings = ConfigurationManager.ConnectionStrings["VTSAdminDB"];
                if (oleDbSettings != null && !string.IsNullOrWhiteSpace(oleDbSettings.ConnectionString))
                {
                    trimmed = oleDbSettings.ConnectionString.Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return string.Empty;
            }

            trimmed = trimmed.Trim();
            if (trimmed.IndexOf("provider=", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                try
                {
                    string[] parts = trimmed.Split(';');
                    StringBuilder builder = new StringBuilder();
                    foreach (string part in parts)
                    {
                        string item = (part ?? string.Empty).Trim();
                        if (item.Length == 0
                            || item.StartsWith("Provider=", StringComparison.OrdinalIgnoreCase)
                            || item.StartsWith("Persist Security Info", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        if (builder.Length > 0)
                        {
                            builder.Append(';');
                        }

                        builder.Append(item);
                    }

                    return builder.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }

            try
            {
                return new SqlConnectionStringBuilder(trimmed).ConnectionString;
            }
            catch
            {
                return trimmed;
            }
        }

        public static string ResolveRecordsetConnectionString()
        {
            return ResolveRecordsetConnectionString(null);
        }

        public static string ResolveRecordsetConnectionString(string rawConnectionString)
        {
            string trimmed = (rawConnectionString ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                HttpContext context = HttpContext.Current;
                if (context != null && context.Session != null)
                {
                    trimmed = Convert.ToString(context.Session["ClsTypeDBConnStringSQL"]);
                }
            }

            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                return trimmed.Trim();
            }

            ConnectionStringSettings oleDbSettings = ConfigurationManager.ConnectionStrings["VTSAdminDB"];
            if (oleDbSettings != null && !string.IsNullOrWhiteSpace(oleDbSettings.ConnectionString))
            {
                return oleDbSettings.ConnectionString.Trim();
            }

            return ResolveSqlClientConnectionString();
        }

        public static DataTable LoadAssignDetailRow(string assignId, int seq)
        {
            return LoadAssignDetailRow(assignId, seq, null);
        }

        public static DataTable LoadAssignDetailRow(string assignId, int seq, string connString)
        {
            if (string.IsNullOrWhiteSpace(assignId))
            {
                return new DataTable();
            }

            string safeAssignId = EscapeSqlLiteral(assignId.Trim());
            int safeSeq = Math.Max(1, seq);
            string[] queries =
            {
                "SELECT TOP 1 "
                    + "LTRIM(RTRIM(ISNULL(AssignID, ''))) AS AssignID, "
                    + "ISNULL(Seq, 0) AS Seq, "
                    + "LTRIM(RTRIM(ISNULL(JobID, ''))) AS JobID, "
                    + "LTRIM(RTRIM(ISNULL(TechnicianID, ''))) AS TechnicianID, "
                    + "SchDate, "
                    + "LTRIM(RTRIM(ISNULL(Remark, ''))) AS Remark, "
                    + "LTRIM(RTRIM(ISNULL(Status, ''))) AS Status, "
                    + "DtmUpd "
                    + "FROM trx_job_assign_detail WITH (NOLOCK) "
                    + "WHERE AssignID = '" + safeAssignId + "' "
                    + "AND Seq = " + safeSeq.ToString(),
                "SELECT TOP 1 "
                    + "LTRIM(RTRIM(ISNULL(AssignID, ''))) AS AssignID, "
                    + "ISNULL(Seq, 0) AS Seq, "
                    + "LTRIM(RTRIM(ISNULL(JobID, ''))) AS JobID, "
                    + "LTRIM(RTRIM(ISNULL(TechnicianID, ''))) AS TechnicianID, "
                    + "SchDate, "
                    + "LTRIM(RTRIM(ISNULL(Remark, ''))) AS Remark, "
                    + "LTRIM(RTRIM(ISNULL(Status, ''))) AS Status, "
                    + "DtmUpd "
                    + "FROM trx_job_assign_detail WITH (NOLOCK) "
                    + "WHERE AssignID = '" + safeAssignId + "' "
                    + "AND ISNULL(Status, '') NOT IN ('DE') "
                    + "ORDER BY Seq DESC"
            };

            foreach (string sql in queries)
            {
                DataTable table = QueryDataTable(sql, connString);
                if (table != null && table.Rows.Count > 0)
                {
                    return table;
                }
            }

            return new DataTable();
        }

        public static DataTable FindAssignDetailKeyByJobId(string jobId)
        {
            return FindAssignDetailKeyByJobId(jobId, null);
        }

        public static DataTable FindAssignDetailKeyByJobId(string jobId, string connString)
        {
            return FindAssignDetailKeyByJobId(jobId, 0, connString);
        }

        public static DataTable FindAssignDetailKeyByJobId(string jobId, int seq, string connString)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return new DataTable();
            }

            string safeJobId = EscapeSqlLiteral(jobId.Trim());
            string seqFilter = seq > 0 ? "AND ISNULL(Seq, 0) = " + seq.ToString() + " " : string.Empty;
            string activeFilter = "AND ISNULL(Status, '') NOT IN ('DE') ";
            string[] queries =
            {
                "SELECT TOP 1 "
                + "LTRIM(RTRIM(ISNULL(AssignID, ''))) AS AssignID, "
                + "ISNULL(Seq, 0) AS Seq, "
                + "LTRIM(RTRIM(ISNULL(JobID, ''))) AS JobID "
                + "FROM trx_job_assign_detail WITH (NOLOCK) "
                + "WHERE JobID = '" + safeJobId + "' "
                + activeFilter
                + seqFilter
                + "ORDER BY SchDate DESC, Seq DESC",
                "SELECT TOP 1 "
                + "LTRIM(RTRIM(ISNULL(AssignID, ''))) AS AssignID, "
                + "ISNULL(Seq, 0) AS Seq, "
                + "LTRIM(RTRIM(ISNULL(JobID, ''))) AS JobID "
                + "FROM trx_job_assign_detail WITH (NOLOCK) "
                + "WHERE LTRIM(RTRIM(ISNULL(JobID, ''))) = '" + safeJobId + "' "
                + activeFilter
                + seqFilter
                + "ORDER BY SchDate DESC, Seq DESC"
            };

            foreach (string sql in queries)
            {
                DataTable table = QueryDataTable(sql, connString);
                if (table != null && table.Rows.Count > 0)
                {
                    return table;
                }
            }

            return new DataTable();
        }

        public static DataTable FindAssignDetailKeyByAssignId(string assignId)
        {
            return FindAssignDetailKeyByAssignId(assignId, null);
        }

        public static DataTable FindAssignDetailKeyByAssignId(string assignId, string connString)
        {
            if (string.IsNullOrWhiteSpace(assignId))
            {
                return new DataTable();
            }

            string safeAssignId = EscapeSqlLiteral(assignId.Trim());
            return QueryDataTable(
                "SELECT TOP 1 "
                + "LTRIM(RTRIM(ISNULL(AssignID, ''))) AS AssignID, "
                + "ISNULL(Seq, 0) AS Seq "
                + "FROM trx_job_assign_detail WITH (NOLOCK) "
                + "WHERE AssignID = '" + safeAssignId + "' "
                + "AND ISNULL(Status, '') NOT IN ('DE') "
                + "ORDER BY Seq DESC",
                connString);
        }

        private static string EscapeSqlLiteral(string value)
        {
            return (value ?? string.Empty).Replace("'", "''");
        }

        private static string GetRowString(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || string.IsNullOrWhiteSpace(columnName))
            {
                return string.Empty;
            }

            if (row.Table.Columns.Contains(columnName))
            {
                object value = row[columnName];
                return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value).Trim();
            }

            foreach (DataColumn column in row.Table.Columns)
            {
                if (column.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    object value = row[column];
                    return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value).Trim();
                }
            }

            return string.Empty;
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

        public sealed class CustomerContact
        {
            public string CustId { get; set; }
            public string FullName { get; set; }
            public string Address { get; set; }
            public string PicName { get; set; }
            public string MobilePhone1 { get; set; }
            public string OfficePhone1 { get; set; }
            public string BranchName { get; set; }
            public string MarketingName { get; set; }
            public string Lat { get; set; }
            public string Long { get; set; }
        }

        public static bool TryGetValidCustomerCoordinates(string latText, string longText, out double lat, out double lng)
        {
            lat = 0;
            lng = 0;
            if (!double.TryParse((latText ?? string.Empty).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out lat))
            {
                return false;
            }

            if (!double.TryParse((longText ?? string.Empty).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out lng))
            {
                return false;
            }

            if (Math.Abs(lat) < 0.000001 && Math.Abs(lng) < 0.000001)
            {
                return false;
            }

            if (lat < -90 || lat > 90 || lng < -180 || lng > 180)
            {
                return false;
            }

            return true;
        }

        public static string BuildGoogleMapsUrl(string latText, string longText)
        {
            double lat;
            double lng;
            if (!TryGetValidCustomerCoordinates(latText, longText, out lat, out lng))
            {
                return string.Empty;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "https://www.google.com/maps?q={0},{1}",
                lat,
                lng);
        }

        public static CustomerContact LoadCustomerContact(string custId, string jobId, string connString)
        {
            string trimmedCustId = (custId ?? string.Empty).Trim();
            string trimmedJobId = (jobId ?? string.Empty).Trim();
            List<string> queries = new List<string>();

            if (!string.IsNullOrWhiteSpace(trimmedCustId))
            {
                string safeCustId = EscapeSqlLiteral(trimmedCustId);
                queries.Add(
                    "SELECT TOP 1 "
                    + "LTRIM(RTRIM(ISNULL(c.CustID, ''))) AS CustID, "
                    + "LTRIM(RTRIM(ISNULL(c.FullName, ''))) AS FullName, "
                    + "LTRIM(RTRIM(ISNULL(c.Address, ''))) AS Address, "
                    + "LTRIM(RTRIM(ISNULL(c.PICName1, ''))) AS PICName1, "
                    + "LTRIM(RTRIM(ISNULL(c.PICName2, ''))) AS PICName2, "
                    + "LTRIM(RTRIM(ISNULL(c.MobilePhone1, ''))) AS MobilePhone1, "
                    + "LTRIM(RTRIM(ISNULL(c.OfficePhone1, ''))) AS OfficePhone1, "
                    + "LTRIM(RTRIM(ISNULL(c.BranchName, ''))) AS BranchName, "
                    + "LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) AS MarketingID, "
                    + "LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName, "
                    + "LTRIM(RTRIM(ISNULL(CONVERT(varchar(50), c.Lat), ''))) AS Lat, "
                    + "LTRIM(RTRIM(ISNULL(CONVERT(varchar(50), c.Long), ''))) AS Long "
                    + "FROM mst_customer c WITH (NOLOCK) "
                    + "LEFT JOIN mst_marketing m WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(m.MarketingID, ''))) = LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) "
                    + "WHERE LTRIM(RTRIM(ISNULL(c.CustID, ''))) = '" + safeCustId + "'");
                queries.Add(
                    "SELECT TOP 1 * "
                    + "FROM mst_customer WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(CustID, ''))) = '" + safeCustId + "'");
            }

            if (!string.IsNullOrWhiteSpace(trimmedJobId))
            {
                string safeJobId = EscapeSqlLiteral(trimmedJobId);
                queries.Add(
                    "SELECT TOP 1 "
                    + "LTRIM(RTRIM(ISNULL(c.CustID, ''))) AS CustID, "
                    + "LTRIM(RTRIM(ISNULL(c.FullName, ''))) AS FullName, "
                    + "LTRIM(RTRIM(ISNULL(c.Address, ''))) AS Address, "
                    + "LTRIM(RTRIM(ISNULL(c.PICName1, ''))) AS PICName1, "
                    + "LTRIM(RTRIM(ISNULL(c.PICName2, ''))) AS PICName2, "
                    + "LTRIM(RTRIM(ISNULL(c.MobilePhone1, ''))) AS MobilePhone1, "
                    + "LTRIM(RTRIM(ISNULL(c.OfficePhone1, ''))) AS OfficePhone1, "
                    + "LTRIM(RTRIM(ISNULL(c.BranchName, ''))) AS BranchName, "
                    + "LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) AS MarketingID, "
                    + "LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName, "
                    + "LTRIM(RTRIM(ISNULL(CONVERT(varchar(50), c.Lat), ''))) AS Lat, "
                    + "LTRIM(RTRIM(ISNULL(CONVERT(varchar(50), c.Long), ''))) AS Long "
                    + "FROM trx_training_order t WITH (NOLOCK) "
                    + "INNER JOIN mst_customer c WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(c.CustID, ''))) = LTRIM(RTRIM(ISNULL(t.CustID, ''))) "
                    + "LEFT JOIN mst_marketing m WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(m.MarketingID, ''))) = LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) "
                    + "WHERE LTRIM(RTRIM(ISNULL(t.TrainingID, ''))) = '" + safeJobId + "' "
                    + "AND ISNULL(t.Status, '') NOT IN ('DE')");
                queries.Add(
                    "SELECT TOP 1 c.* "
                    + "FROM trx_training_order t WITH (NOLOCK) "
                    + "INNER JOIN mst_customer c WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(c.CustID, ''))) = LTRIM(RTRIM(ISNULL(t.CustID, ''))) "
                    + "WHERE LTRIM(RTRIM(ISNULL(t.TrainingID, ''))) = '" + safeJobId + "' "
                    + "AND ISNULL(t.Status, '') NOT IN ('DE')");
            }

            CustomerContact bestWithoutPhone = null;
            foreach (string sql in queries)
            {
                DataTable table = QueryDataTable(sql, connString);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                CustomerContact contact = MapCustomerContact(table.Rows[0]);
                if (contact == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(contact.MarketingName))
                {
                    contact.MarketingName = LookupMarketingNameByMarketingId(
                        GetRowString(table.Rows[0], "MarketingID"));
                }

                if (string.IsNullOrWhiteSpace(contact.MarketingName))
                {
                    contact.MarketingName = LookupMarketingNameByCustId(contact.CustId);
                }

                bool hasIdentity = !string.IsNullOrWhiteSpace(contact.Address)
                    || !string.IsNullOrWhiteSpace(contact.PicName)
                    || !string.IsNullOrWhiteSpace(contact.FullName)
                    || !string.IsNullOrWhiteSpace(contact.CustId);
                bool hasPhone = !string.IsNullOrWhiteSpace(contact.MobilePhone1)
                    || !string.IsNullOrWhiteSpace(contact.OfficePhone1);
                if (!hasIdentity && !hasPhone)
                {
                    continue;
                }

                if (hasPhone)
                {
                    return contact;
                }

                if (bestWithoutPhone == null)
                {
                    bestWithoutPhone = contact;
                }
            }

            return bestWithoutPhone;
        }

        private static CustomerContact MapCustomerContact(DataRow row)
        {
            if (row == null)
            {
                return null;
            }

            return new CustomerContact
            {
                CustId = FirstNonEmpty(
                    GetRowString(row, "CustID"),
                    GetRowString(row, "CustomerID")),
                FullName = CleanCustomerField(FirstNonEmpty(
                    GetRowString(row, "FullName"),
                    GetRowString(row, "CustomerName"),
                    GetRowString(row, "CustName"))),
                Address = CleanCustomerField(FirstNonEmpty(
                    GetRowString(row, "Address"),
                    GetRowString(row, "CustAddress"),
                    GetRowString(row, "BillingAddr"),
                    GetRowString(row, "BillingAddress"),
                    GetRowString(row, "TaxAddr"),
                    GetRowString(row, "ShippingAddr"),
                    GetRowString(row, "BranchAddress"))),
                PicName = CleanCustomerField(FirstNonEmpty(
                    GetRowString(row, "PICName1"),
                    GetRowString(row, "PicName"),
                    GetRowString(row, "PICName"),
                    GetRowString(row, "PICName2"))),
                MobilePhone1 = CleanCustomerField(FirstNonEmpty(
                    GetRowString(row, "MobilePhone1"),
                    GetRowString(row, "Mobile Phone 1"))),
                OfficePhone1 = CleanCustomerField(FirstNonEmpty(
                    GetRowString(row, "OfficePhone1"),
                    GetRowString(row, "Office Phone 1"))),
                BranchName = CleanCustomerField(GetRowString(row, "BranchName")),
                Lat = CleanCustomerField(GetRowString(row, "Lat")),
                Long = CleanCustomerField(GetRowString(row, "Long")),
                MarketingName = CleanCustomerField(FirstNonEmpty(
                    GetRowString(row, "MarketingName"),
                    GetRowString(row, "Marketing")))
            };
        }

        public static string LookupMarketingNameByCustId(string custId)
        {
            string trimmed = (custId ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmed)
                || trimmed.Equals("[SELECT]", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            string safeCustId = EscapeSqlLiteral(trimmed);
            string[] queries =
            {
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName "
                    + "FROM mst_customer c WITH (NOLOCK) "
                    + "LEFT JOIN mst_marketing m WITH (NOLOCK) "
                    + "ON LTRIM(RTRIM(ISNULL(m.MarketingID, ''))) = LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) "
                    + "WHERE LTRIM(RTRIM(ISNULL(c.CustID, ''))) = '" + safeCustId + "' "
                    + "AND LTRIM(RTRIM(ISNULL(c.MarketingID, ''))) NOT IN ('', '[SELECT]')",
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(m.MarketingName, ''))) AS MarketingName "
                    + "FROM mst_customer c WITH (NOLOCK) "
                    + "LEFT JOIN mst_marketing m WITH (NOLOCK) "
                    + "ON m.MarketingID = c.MarketingID "
                    + "WHERE c.CustID = '" + safeCustId + "'"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(sql);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                string marketingName = CleanCustomerField(GetRowString(table.Rows[0], "MarketingName"));
                if (!string.IsNullOrWhiteSpace(marketingName))
                {
                    return marketingName;
                }
            }

            return string.Empty;
        }

        public static string LookupMarketingNameByMarketingId(string marketingId)
        {
            string trimmed = (marketingId ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmed)
                || trimmed.Equals("[SELECT]", StringComparison.OrdinalIgnoreCase)
                || trimmed.Equals("-", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            string safeId = EscapeSqlLiteral(trimmed);
            string[] queries =
            {
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(MarketingName, ''))) AS MarketingName "
                    + "FROM mst_marketing WITH (NOLOCK) "
                    + "WHERE LTRIM(RTRIM(ISNULL(MarketingID, ''))) = '" + safeId + "'",
                "SELECT TOP 1 LTRIM(RTRIM(ISNULL(MarketingName, ''))) AS MarketingName "
                    + "FROM mst_marketing WITH (NOLOCK) "
                    + "WHERE MarketingID = '" + safeId + "'"
            };

            foreach (string sql in queries)
            {
                DataTable table = ExecuteQuery(sql);
                if (table == null || table.Rows.Count == 0)
                {
                    continue;
                }

                string marketingName = CleanCustomerField(GetRowString(table.Rows[0], "MarketingName"));
                if (!string.IsNullOrWhiteSpace(marketingName))
                {
                    return marketingName;
                }
            }

            return string.Empty;
        }

        private static string CleanCustomerField(string value)
        {
            string cleaned = (value ?? string.Empty)
                .Replace("&nbsp;", " ")
                .Replace("&NBSP;", " ")
                .Trim();
            if (string.IsNullOrWhiteSpace(cleaned)
                || cleaned == "-"
                || cleaned.Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return cleaned;
        }

        public static string ResolveItSupportAssignDate(
            string connString,
            string assignId,
            int seq,
            string jobId = null)
        {
            if (!string.IsNullOrWhiteSpace(assignId))
            {
                DataTable table = LoadAssignDetailRow(assignId, Math.Max(1, seq));
                if (table != null && table.Rows.Count > 0)
                {
                    string formatted = FormatAssignDateForDisplay(GetRowString(table.Rows[0], "DtmUpd"));
                    if (!string.IsNullOrWhiteSpace(formatted))
                    {
                        return formatted;
                    }
                }
            }

            string trimmedJobId = (jobId ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(trimmedJobId))
            {
                return string.Empty;
            }

            DataTable keyTable = FindAssignDetailKeyByJobId(trimmedJobId, connString);
            if (keyTable == null || keyTable.Rows.Count == 0)
            {
                return string.Empty;
            }

            string resolvedAssignId = GetRowString(keyTable.Rows[0], "AssignID");
            int resolvedSeq = 1;
            int parsedSeq;
            if (int.TryParse(GetRowString(keyTable.Rows[0], "Seq"), out parsedSeq) && parsedSeq > 0)
            {
                resolvedSeq = parsedSeq;
            }

            if (string.IsNullOrWhiteSpace(resolvedAssignId)
                || resolvedAssignId.Equals((assignId ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return ResolveItSupportAssignDate(connString, resolvedAssignId, resolvedSeq, null);
        }

        public static string ResolveJobTrainingAssignDate(string connString, string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return string.Empty;
            }

            string effectiveConn = ResolveRecordsetConnectionString(connString);
            if (string.IsNullOrWhiteSpace(effectiveConn))
            {
                effectiveConn = ResolveSqlClientConnectionString(connString);
            }

            string trimmedJobId = jobId.Trim();
            string safeJobId = EscapeSqlLiteral(trimmedJobId);

            if (!string.IsNullOrWhiteSpace(effectiveConn))
            {
                try
                {
                    Recordset rec = new Recordset();
                    string openError = string.Empty;
                    rec.Open(
                        "sp_list_header_job_training_itsupport '" + safeJobId + "'",
                        effectiveConn.Trim(),
                        ref openError);
                    if (string.IsNullOrWhiteSpace(openError))
                    {
                        DataTable headerRows = rec.DataRecord() ?? new DataTable();
                        foreach (DataRow row in headerRows.Rows)
                        {
                            string trainingId = FirstNonEmpty(
                                GetRowString(row, "TrainingID"),
                                GetRowString(row, "JobID")).Trim();
                            if (!trainingId.Equals(trimmedJobId, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            string assignDate = FirstNonEmpty(
                                GetRowString(row, "sReqDate"),
                                GetRowString(row, "ReqDate"));
                            if (!string.IsNullOrWhiteSpace(assignDate))
                            {
                                return FormatAssignDateForDisplay(assignDate);
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            DataTable direct = ExecuteQuery(
                "SELECT TOP 1 "
                + "CONVERT(varchar(10), ReqDate, 120) AS ReqDateIso, "
                + "CONVERT(varchar(11), ReqDate, 106) AS ReqDateText "
                + "FROM trx_training_order WITH (NOLOCK) "
                + "WHERE TrainingID = '" + safeJobId + "' "
                + "AND ISNULL(Status, '') NOT IN ('DE')");
            if (direct != null && direct.Rows.Count > 0)
            {
                DataRow row = direct.Rows[0];
                string assignDate = FirstNonEmpty(
                    GetRowString(row, "ReqDateIso"),
                    GetRowString(row, "ReqDateText"));
                if (!string.IsNullOrWhiteSpace(assignDate))
                {
                    return FormatAssignDateForDisplay(assignDate);
                }
            }

            return string.Empty;
        }

        private static string FormatAssignDateForDisplay(string source)
        {
            DateTime parsedDate;
            if (DateTime.TryParse(source, out parsedDate))
            {
                return parsedDate.ToString("dd MMM yyyy");
            }

            return (source ?? string.Empty).Trim();
        }
    }
}
