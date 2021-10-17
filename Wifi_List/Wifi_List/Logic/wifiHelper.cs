using Sandwych.SmartConfig;
using Sandwych.SmartConfig.Esptouch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Wifi_List
{
   public class WifiHelper
    {
        internal async Task<List<string>> GetWifiListAsync()
        {

            List<string> wifiNetworks = new List<string>();
            Sandwych.SmartConfig.SmartConfigArguments args = new Sandwych.SmartConfig.SmartConfigArguments();
           await SmartConfigStarter.StartAsync<EspSmartConfigProvider>(args, onDeviceDiscovered: (s, e) => 
           wifiNetworks.Add(string.Format("Found device: IP={0}    MAC={1}", e.Device.IPAddress, e.Device.MacAddress)));
           
           return wifiNetworks;
        }
    }
}