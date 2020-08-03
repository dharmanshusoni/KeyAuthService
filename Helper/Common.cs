using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace DemoService.Helper
{
    public class Common
    {
        #region Declare
        public static string _BaseURL = "https://unicrm.biz/abcon/api/";
        public static string _keyCheckURL = "keycheck.php?";
        public static string _keyUpdateURL = "updatekey.php";
        #endregion

        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        public static string GetLocationProperty()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
                return "lat:"+coord.Latitude + "lon:" + coord.Longitude;
            }
            else
            {
                return "Unknown";
            }
        }

        public static bool SystemLogout()
        {
            Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
            return true;
        }

        public static string getMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        public static string AddParameter(string url, string paramName, string paramValue)
        {
            if (url.Substring(url.Length-1) == "?")
            {
                url = url + paramName + "=" + paramValue;
            }
            else
            {
                url = url + "&" + paramName + "=" + paramValue;
            }

            return url;
        }
        
        public static string GetKeyCheckURL()
        {
            return _BaseURL + _keyCheckURL;
        }
        
        public static string GetKeyUpdateURL()
        {
            return _BaseURL + _keyUpdateURL;
        }
    
    }
}
