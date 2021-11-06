using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wifi_List
{
    internal class SaveState
    {       
       //add the network names and mac addresses to the preferences file 
        internal void saveFlaggedNetwork(string filename, WifiNetwork[] networks)
        {

            EnumeratedNetworks networksFromFile;

            foreach (var network in networks)
            {
                if (File.Exists(filename))
                {
                    using (Stream file = File.OpenRead(filename))
                    {
                        networks = EnumeratedNetworks.Parser.ParseFrom(file);
                        
                    }
                }
                else
                {
                    Console.WriteLine("{0}: File not found. Creating a new file.", filename);
                    networksFromFile = new EnumeratedNetworks();
                }

                networksFromFile.Add(networks);
            }

            if (filename == String.Empty)
            {
                string flaggedNetworksFile = "networks.data";
                // Write the new network enumeration back to disk.
                using (Stream output = File.OpenWrite(flaggedNetworksFile))
                {
                    networksFromFile.WriteTo(output);
                }
            } 
           
        }



        //remove one Network by name from preferences
        internal List<string> removeOneFlaggedNetwork(string key)
        {
            //make a new place to store the list of blocked networks
            List<string> result = new List<string>();

            //bring down the list as one big horrible string
            //ssid's are allowed to be 32 bytes long. 
            //So we count to 33 to avoid that bug of naming an ssid as the list name.
            string addedFlaggedNetwork = Preferences.Get("123456789101112131415161718192021222324252627282930313233", "");

            //if it's not empty
            if (addedFlaggedNetwork != null)
            {
                //split it up into columns
                var columns = addedFlaggedNetwork.Split(',');

                //go over the columns, adding to the list.
                for (int i = 0; i < columns.Length; i++)
                {
                    result.Add(i.ToString());
                }

                //remove the key from the flagged list
                result.Remove(key);

                //re-write the preference file
                foreach (var item in result)
                {
                    Preferences.Set("123456789101112131415161718192021222324252627282930313233", item.ToString());
                }
               
            }            

            //this is getting the actual network information and not just the name of one.
            retrieveSingleFlaggedNetwork(key, "");

            //clear that network from preferences cache
            Preferences.Clear(key);

            //return the new list of network names without the one that was cleared.
            return result;
        }

        //get all the networks added as flagged
        internal List<WifiNetwork> retrieveAllFlaggedNetworks()
        {
            //first create an array of information
            List<WifiNetwork []> result = new List<WifiNetwork[]>();
              var columns =  Preferences.Get("123456789101112131415161718192021222324252627282930313233", "").Split(',');
           
            //go through the comma delimited values and create new wifi networks of them
           foreach (var item in columns)
            {
                result.Add(new WifiNetwork{
                    Name = columns[0],
                    MacAddress = columns[1],
                    Flagged = columns[2]
                });
            }              
            
           //return that list of networks
            return result
        }

        //get a list of the network names, for use in the UI
        internal List<string> retrieveAllFlaggedNetworkNames()
        {
            var networkList = retrieveAllFlaggedNetworks();
            List<string> result = new List<string>();
            foreach (var item in networkList)
	        {
                result.Add(item.Name.ToString());
	        }

            return result;
        }

        internal WifiNetwork retrieveSingleFlaggedNetwork(string key, string valueString)
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
