using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Wifi_List
{
    public partial class MainPage : ContentPage
    {       
            public ListView WifiList { get; set; }
        public string SelectedWifiNetwork { get; set; }

        public MainPage()
        {
            var wifiHelper = new WifiHelper();
            var wifiSource = wifiHelper.GetWifiListAsync();

            WifiList = new ListView();
            WifiList.ItemsSource = wifiSource.Result;
            WifiList.WidthRequest = 150;
            WifiList.ItemTapped += WifiPicker_SelectedIndexChanged;

            var blockWifi = new SpringBoardButton();
            blockWifi.Icon = "blockWifi.png";
            blockWifi.Label = "Flag Network";
            var blocktapGestureRecognizer = new TapGestureRecognizer();
            blocktapGestureRecognizer.Tapped += (s, e) =>
            {
                BlockNetwork.AddToBlockList(SelectedWifiNetwork);
                
            };
            blockWifi.GestureRecognizers.Add(blocktapGestureRecognizer);

        }
        private void WifiPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
           SelectedWifiNetwork = WifiList.SelectedItem as string;
        }
    }
}
