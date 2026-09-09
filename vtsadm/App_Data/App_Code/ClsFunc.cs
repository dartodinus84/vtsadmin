using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vtsadm.App_Code
{
    public static class ClsFunc
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern int WritePrivateProfileString(string lpApplication, string lpKeyName, string lpString, string lpFileName);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);

        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }
        public static string Left(this string value, int length)
        {
            return value.Substring(0, length);
        }
    }
}