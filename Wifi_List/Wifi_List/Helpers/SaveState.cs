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
            //make a new place to store the list of blocked networks
            List<string> blockedList = new List<string>();

            //bring down the list as one big horrible string
            //ssid's are allowed to be 32 bytes long. 
            //So we count to 33 to avoid that bug of naming an ssid as the list name.
            string addedBlock = Preferences.Get("123456789101112131415161718192021222324252627282930313233", "");

            //if it's not empty
            if (addedBlock != null)
            {
                //split it up into columns
                var columns = addedBlock.Split(',');

                //go over the columns, adding to the list.
                for (int i = 0; i < columns.Length; i++)
                {
                    blockedList.Add(i.ToString());
                }

                //remove the key from the blocked list
                blockedList.Remove(key);

                //re-write the preference file
                foreach (var item in blockedList)
                {
                    Preferences.Set("123456789101112131415161718192021222324252627282930313233", item.ToString());
                }
               
            }            

            //outside of the list of network names only, keyed by "blockedList", retrieve the actual blocked network
            retrieveBlockedNetwork(key, "");

            //and clear that from preferences
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
