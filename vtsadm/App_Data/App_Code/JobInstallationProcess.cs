using System;
using System.Collections.Generic;
using System.Linq;

namespace vtsadm.App_Code
{
    public class JobTypeCategory
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public string[] ProcessKeys { get; set; }

        public bool HasMaintSubType
        {
            get { return ProcessKeys != null && ProcessKeys.Length > 1; }
        }

        public bool HasAccess(string accessMenu)
        {
            if (string.IsNullOrEmpty(accessMenu)) return false;
            accessMenu = accessMenu.ToUpper();
            if (Key == "new_install")
            {
                var p = JobInstallationProcess.Get("new_install");
                return p != null && accessMenu.Contains(p.MenuCode);
            }
            return ProcessKeys.Any(k =>
            {
                var proc = JobInstallationProcess.Get(k);
                return proc != null && accessMenu.Contains(proc.MenuCode);
            });
        }

        public IEnumerable<JobInstallationProcess> GetAccessibleProcesses(string accessMenu)
        {
            if (string.IsNullOrEmpty(accessMenu)) yield break;
            accessMenu = accessMenu.ToUpper();
            foreach (var key in ProcessKeys)
            {
                var proc = JobInstallationProcess.Get(key);
                if (proc != null && accessMenu.Contains(proc.MenuCode))
                    yield return proc;
            }
        }
    }

    public class JobInstallationProcess
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public string MenuCode { get; set; }
        public bool IsMaintenance { get; set; }
        public string Prefix { get; set; }
        public string PictureSessionKey { get; set; }
        public string UploadPage { get; set; }
        public MaintUiFlags Ui { get; set; }

        public class MaintUiFlags
        {
            public bool DeviceFirstFlow { get; set; }
            public bool ShowNewDevice { get; set; }
            public bool ShowNewGsm { get; set; }
            public bool ShowNewGsmSuspend { get; set; }
            public bool ShowNewVehicle { get; set; }
            public bool ShowNewCustomer { get; set; }
            public bool ShowNewServer { get; set; }
            public bool ShowStatusOldDevice { get; set; }
            public bool ShowStatusOldGsm { get; set; }
            public bool ShowStatusOldVehicle { get; set; }
            public bool ShowAccessoriesMaint { get; set; }
            public bool ShowUserAccess { get; set; }
            public bool ShowWarranty { get; set; }
            public bool ShowEmailNotif { get; set; }
            public bool SinglePicture { get; set; }
        }

        private static readonly JobTypeCategory[] Categories =
        {
            new JobTypeCategory { Key = "accessories_maint", Label = "Accessories", ProcessKeys = new[] { "accessories_maint" } },
            new JobTypeCategory { Key = "others_maint", Label = "Others", ProcessKeys = new[] { "others_maint" } },
            new JobTypeCategory { Key = "uninstall_maint", Label = "Uninstalling", ProcessKeys = new[] { "uninstall_maint" } },
            new JobTypeCategory { Key = "sb_maint", Label = "Soft Blocked", ProcessKeys = new[] { "sb_maint" } },
            new JobTypeCategory { Key = "ub_maint", Label = "Unblocked", ProcessKeys = new[] { "ub_maint" } },
            new JobTypeCategory { Key = "sp_maint", Label = "Suspended", ProcessKeys = new[] { "sp_maint" } },
            new JobTypeCategory { Key = "re_maint", Label = "Reactivated", ProcessKeys = new[] { "re_maint" } },
            new JobTypeCategory { Key = "device_maint", Label = "Device", ProcessKeys = new[] { "device_maint" } },
            new JobTypeCategory { Key = "gsm_maint", Label = "Gsm", ProcessKeys = new[] { "gsm_maint" } },
            new JobTypeCategory { Key = "gsm_maint_suspend", Label = "Reactivated Gsm", ProcessKeys = new[] { "gsm_maint_suspend" } },
            new JobTypeCategory { Key = "customer_maint", Label = "Customer", ProcessKeys = new[] { "customer_maint" } },
            new JobTypeCategory { Key = "server_maint", Label = "Server", ProcessKeys = new[] { "server_maint" } },
            new JobTypeCategory { Key = "vehicle_maint", Label = "Vehicle", ProcessKeys = new[] { "vehicle_maint" } }
        };

        private static readonly HashSet<string> GeneralJobMaintenanceKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "accessories_maint", "others_maint", "uninstall_maint", "sb_maint", "ub_maint", "sp_maint", "re_maint"
        };

        private static readonly JobInstallationProcess[] All =
        {
            New("new_install", "New Installation", "MNUINSTALLNEW", null, "ClsTypeNewPicture", new MaintUiFlags()),
            Maint("device_maint", "Device Maintenance", "MNUINSTALLMAINTDEV", "device_maint", "ClsTypeDeviceMaintPicture",
                f => { f.DeviceFirstFlow = true; f.ShowNewDevice = true; f.ShowNewServer = true; f.ShowStatusOldDevice = true; f.ShowUserAccess = true; f.ShowWarranty = true; f.ShowEmailNotif = true; f.SinglePicture = true; }),
            Maint("gsm_maint", "GSM Maintenance", "MNUINSTALLMAINTGSM", "gsm_maint", "ClsTypeGsmMaintPicture",
                f => { f.DeviceFirstFlow = true; f.ShowNewGsm = true; f.ShowStatusOldGsm = true; f.SinglePicture = true; }),
            Maint("gsm_maint_suspend", "GSM Suspend Maintenance", "MNUINSTALLMAINTGSM", "gsm_maint_suspend", "ClsTypeGsmMaintPicture",
                f => { f.DeviceFirstFlow = true; f.ShowNewGsmSuspend = true; f.ShowStatusOldGsm = true; f.SinglePicture = true; }),
            Maint("vehicle_maint", "Vehicle Maintenance", "MNUINSTALLMAINTVEH", "vehicle_maint", "ClsTypeVehicleMaintPicture",
                f => { f.DeviceFirstFlow = true; f.ShowNewVehicle = true; f.ShowStatusOldVehicle = true; f.SinglePicture = true; }),
            Maint("others_maint", "Others Maintenance", "MNUINSTALLMAINTOTH", "others_maint", "ClsTypeOthersMaintPicture",
                f => { f.DeviceFirstFlow = true; f.SinglePicture = true; }),
            Maint("uninstall_maint", "Uninstall Maintenance", "MNUINSTALLMAINTUNI", "uninstall_maint", "ClsTypeUninstallMaintPicture",
                f => { f.DeviceFirstFlow = true; f.SinglePicture = true; }),
            Maint("server_maint", "Server Maintenance", "MNUINSTALLMAINTSVR", "server_maint", "ClsTypeServerMaintPicture",
                f => { f.DeviceFirstFlow = true; f.ShowNewServer = true; f.ShowUserAccess = true; f.SinglePicture = true; }),
            Maint("customer_maint", "Customer Maintenance", "MNUINSTALLMAINTCUST", "customer_maint", null,
                f => { f.DeviceFirstFlow = true; f.ShowNewCustomer = true; f.ShowNewServer = true; f.SinglePicture = false; }),
            Maint("sb_maint", "Soft Blocked Maintenance", "MNUINSTALLMAINTSB", "sb_maint", "ClsTypeSBMaintPicture",
                f => { f.DeviceFirstFlow = true; f.SinglePicture = true; }),
            Maint("ub_maint", "Unblocked Maintenance", "MNUINSTALLMAINTUB", "ub_maint", "ClsTypeUBMaintPicture",
                f => { f.DeviceFirstFlow = true; f.SinglePicture = true; }),
            Maint("sp_maint", "Suspended Maintenance", "MNUINSTALLMAINTSP", "sp_maint", "ClsTypeSPMaintPicture",
                f => { f.DeviceFirstFlow = true; f.SinglePicture = true; }),
            Maint("re_maint", "Reactivated Maintenance", "MNUINSTALLMAINTRE", "re_maint", "ClsTypeReMaintPicture",
                f => { f.DeviceFirstFlow = true; f.SinglePicture = true; }),
            Maint("accessories_maint", "Accessories Maintenance", "MNUINSTALLMAINTACC", "accessories_maint", "ClsTypeAccessoriesMaintPicture",
                f => { f.DeviceFirstFlow = true; f.ShowAccessoriesMaint = true; f.SinglePicture = true; })
        };

        private static JobInstallationProcess New(string key, string label, string menu, string prefix, string pictureKey, MaintUiFlags flags)
        {
            return new JobInstallationProcess { Key = key, Label = label, MenuCode = menu, IsMaintenance = false, Prefix = prefix, PictureSessionKey = pictureKey, UploadPage = prefix != null ? prefix + "_upload.aspx" : null, Ui = flags };
        }

        private static JobInstallationProcess Maint(string key, string label, string menu, string prefix, string pictureKey, Action<MaintUiFlags> configure)
        {
            var flags = new MaintUiFlags { DeviceFirstFlow = true, SinglePicture = true };
            configure(flags);
            return new JobInstallationProcess { Key = key, Label = label, MenuCode = menu, IsMaintenance = true, Prefix = prefix, PictureSessionKey = pictureKey, UploadPage = prefix + "_upload.aspx", Ui = flags };
        }

        public static IEnumerable<JobTypeCategory> GetAllCategories()
        {
            return Categories;
        }

        public static JobTypeCategory GetCategory(string key)
        {
            return Categories.FirstOrDefault(c => c.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<JobTypeCategory> GetAccessibleCategories(string accessMenu)
        {
            return Categories.Where(c => c.HasAccess(accessMenu));
        }

        public static IEnumerable<JobInstallationProcess> GetAll()
        {
            return All;
        }

        public static JobInstallationProcess Get(string key)
        {
            return All.FirstOrDefault(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public static bool HasHubAccess(string accessMenu)
        {
            return GetAccessibleCategories(accessMenu).Any();
        }

        public static bool IsGeneralJobMaintenance(string categoryKey)
        {
            return !string.IsNullOrEmpty(categoryKey) && GeneralJobMaintenanceKeys.Contains(categoryKey);
        }

        public static IEnumerable<JobInstallationProcess> GetAccessible(string accessMenu, bool maintenanceOnly)
        {
            if (string.IsNullOrEmpty(accessMenu)) yield break;
            accessMenu = accessMenu.ToUpper();
            foreach (var p in All.Where(x => x.IsMaintenance == maintenanceOnly))
            {
                if (accessMenu.Contains(p.MenuCode))
                    yield return p;
            }
        }

        public string Sp(string suffix)
        {
            return "sp_" + Prefix + "_" + suffix;
        }

        public string ListSp(string suffix)
        {
            return "sp_list_" + Prefix + "_" + suffix;
        }
    }
}
