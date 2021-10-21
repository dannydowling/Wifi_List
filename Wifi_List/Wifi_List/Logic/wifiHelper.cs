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
        internal async Task<List<WifiNetwork>> GetWifiListAsync()
        {

            List<WifiNetwork> wifiNetworks = new List<WifiNetwork>();
            Sandwych.SmartConfig.SmartConfigArguments args = new Sandwych.SmartConfig.SmartConfigArguments();
           await SmartConfigStarter.StartAsync<EspSmartConfigProvider>(args, onDeviceDiscovered: (s, e) => 
           wifiNetworks.Add(new WifiNetwork{Name = e.Device.IPAddress.ToString(), MacAddress = e.Device.MacAddress.ToString()}));
           
           return wifiNetworks;
        }
    }
}