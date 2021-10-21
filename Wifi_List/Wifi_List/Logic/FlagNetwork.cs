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
        WifiHelper wifiHelper = new WifiHelper();
        var networkNames = new List<WifiNetwork>();
            networkNames = wifiHelper.GetWifiListAsync().Result;

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