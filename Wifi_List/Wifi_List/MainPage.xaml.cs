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

        public ListView FlaggedList { get; set; }
        public string Observed_WifiNetwork_Selected_String { get; set; }
            public string Flagged_WifiNetwork_Selected_String { get; set; }

        public MainPage()
        {
            //wifiHelper goes out and gets the info
            var wifiHelper = new WifiHelper();

            //saveState stores information in preferences cache
            var saveState = new SaveState();

            //loads up a library and gets a list of networks that can be seen
            var wifiSource = wifiHelper.GetWifiListAsync();

            //checks preferences to retrieve the list of flagged networks store
            var flaggedListSource = saveState.retrieveAllFlaggedNetworkNames();

            //creates an object to store a list of networks currently being seen
            WifiList = new ListView();
            //sets the source to be the result of checking for wifi Networks
            List<WifiNetwork> wifiNetworkList = wifiSource.Result;
            List<string> wifiNetworkNames = new List<string>();
            foreach (var item in wifiNetworkList)
            {
                wifiNetworkNames.Add(item.Name);
            }

            WifiList.ItemsSource = wifiNetworkNames;
            //Sets the dimensions of the list
            WifiList.WidthRequest = 150;
            WifiList.HeightRequest = 300;
            //triggers an event when an item in the list is selected
            WifiList.ItemTapped += WifiPicker_SelectedIndexChanged;

            //creates an object to store the cached list of flagged network names
            FlaggedList = new ListView();
            //sets the list object to use the list from the preferences cache
            FlaggedList.ItemsSource = flaggedListSource;
            //sets the dimensions of the list
            FlaggedList.WidthRequest = 150;
            FlaggedList.HeightRequest = 300;
            //triggers an event for when an item in the flagged network list is selected
            FlaggedList.ItemTapped += FlaggedNetworkPicker_SelectedIndexChanged;

            var flagWifi = new SpringBoardButton();
            flagWifi.Icon = "blockWifi.png";
            flagWifi.Label = "Flag Network";
            var flaggedtapGestureRecognizer = new TapGestureRecognizer();
            flaggedtapGestureRecognizer.Tapped += (s, e) =>
            {

                var flaggedNetworkToSave = wifiNetworkList.Single(x => x.Name == Observed_WifiNetwork_Selected_String);
                //add the observed network to the flagged networks preferences cache
                saveState.saveFlaggedNetwork(flaggedNetworkToSave);
                FlagNetwork.CreateNewNotification(Observed_WifiNetwork_Selected_String);                
            };
            flagWifi.GestureRecognizers.Add(flaggedtapGestureRecognizer);

        }
        private void WifiPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
           Observed_WifiNetwork_Selected_String = WifiList.SelectedItem as string;
        }

          private void FlaggedNetworkPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
           Flagged_WifiNetwork_Selected_String = FlaggedList.SelectedItem as string;
        }
    }
}
