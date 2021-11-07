using System;
using System.Collections.Generic;
using System.Linq;

namespace Wifi_List
{
    internal class FlagNetwork
    {
        internal static void CreateNewNotification(string selectedWifiNetwork)
        {



        }
    

    public void CheckNetworkNamesForMatches()
    {
        UI_Networks_Scanner wifiHelper = new UI_Networks_Scanner();
        var networkNames = new List<WifiNetwork>();
            networkNames = wifiHelper.UI_List_Visible_Networks().Result;

        SaveState saveState = new SaveState();
        var flaggedNetworks = saveState.retrieveAllFlaggedNetworks();


        foreach (var item in networkNames)
        {
            if (flaggedNetworks.Contains(item))
            {
                Alarm(item);
            }
        } 
    }

        public void Alarm(WifiNetwork item)
        {
            MainPage.Alarm(item.Message);
        }

    }
}