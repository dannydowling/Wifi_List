using System;
using System.Collections.Generic;
using System.Text;
using Wifi_List.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wifi_List.Helpers
{
    internal class SaveState
    {       
       //add the network names and mac addresses to the preferences file 
        internal void saveBlockedNetwork(Dictionary<string, object> values)
        {
            foreach (var item in values)
            {
                Preferences.Set(item.Key, item.Value.ToString());
            }
        }

        //remove one Network by name from preferences
        internal List<string> clearBlockedNetwork(string key)
        {
            List<string> blockedList = new List<string>();
            string addedBlock = Preferences.Get("blockedList", "");

            if (addedBlock != null)
            {
                var columns = addedBlock.Split(',');
                blockedList.Add(columns[0]);
            }            

            retrieveBlockedNetwork(key, "");
            Preferences.Clear(key);

            return blockedList;
        }

        internal WifiNetwork retrieveBlockedNetwork(string key, string valueString)
        {
            string mess = Preferences.Get(key,valueString);
            if (mess == null) { Console.WriteLine("key not found"); }

            var columns = mess.Split(',');
            var result = new WifiNetwork 
            {
                Name = columns[0],
                MacAddress = columns[1]  
            };

            if (result != null)
            {
                result.Flagged = true;
            }

            return result;

        }
    }
}
