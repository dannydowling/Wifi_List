using Sandwych.SmartConfig;
using Sandwych.SmartConfig.Esptouch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Wifi_List
{
   public class UI_Networks_Scanner
    {
        internal async Task<List<partialNetworkInfo>> UI_List_Visible_Networks()
        {
            List<partialNetworkInfo> wifiNetworks = new List<partialNetworkInfo>();
            Sandwych.SmartConfig.SmartConfigArguments args = new Sandwych.SmartConfig.SmartConfigArguments();

           await SmartConfigStarter.StartAsync<EspSmartConfigProvider>(args, onDeviceDiscovered: (s, e) => 
           wifiNetworks.Add(new partialNetworkInfo{IPAddress = e.Device.IPAddress.ToString(), MACaddress = e.Device.MacAddress.ToString()}));
           
           return wifiNetworks;
        }

        internal class partialNetworkInfo
        {
            public string IPAddress { get; set; }
            public string MACaddress { get; set; }
        }
    }
}