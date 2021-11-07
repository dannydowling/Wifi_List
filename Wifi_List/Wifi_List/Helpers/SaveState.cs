using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wifi_List
{
    internal class SaveState
    {
        internal void Internal_Save_Network_As_Flagged(string filename, Network[] networks)
        {

            EnumeratedNetworks networksFromFile;

            foreach (Network network in networks)
            {
                if (File.Exists(filename))
                {
                    using (Stream file = File.OpenRead(filename))
                    {
                        networksFromFile = EnumeratedNetworks.Parser.ParseFrom(file);
                    }
                }
                else
                {
                    Console.WriteLine("{0}: File not found. Creating a new file.", filename);
                    networksFromFile = new EnumeratedNetworks();
                }

                networksFromFile.MergeFrom(EnumeratedNetworks.Parser.ParseFrom(network.ToByteArray()));


                if (filename == String.Empty)
                {
                    string flaggedNetworksFile = "networks.data";

                    using (Stream output = File.OpenWrite(flaggedNetworksFile))
                    {
                        networksFromFile.WriteTo(output);
                    }
                }

            }
        }
        public List<Network> Internal_Get_All_Flagged_Networks(string filename)
        {
            if (filename == null)
                filename = "networks.data";

            if (!File.Exists(filename))
            {
                Console.WriteLine("{0} doesn't exist. Add a person to create the file first.", filename);
            }

            List<Network> result = new List<Network>();

            using (Stream stream = File.OpenRead(filename))
            {
                EnumeratedNetworks networks = EnumeratedNetworks.Parser.ParseFrom(stream);
                result.AddRange(networks.Networks);
            }
            return result;
        }

        internal List<string> UI_Update_Flagged_Network_Names()
        {
            var networkList = Internal_Get_All_Flagged_Networks("networks.data");
            List<string> result = new List<string>();
            foreach (Network network in networkList)
            {
                result.Add(network.Name.ToString());
            }
            return result;
        }

        internal Network Internal_Get_Single_Flagged_Network(string Name)
        {
            var networkList = Internal_Get_All_Flagged_Networks("networks.data");
            Network result = networkList.Single(x => x.Name == Name);

            return result;
        }

        internal void Internal_Remove_Network_As_Flagged(string Name)
        {
            if (Name == null)
                Console.WriteLine("{0} doesn't exist.", Name);

            var networks = Internal_Get_All_Flagged_Networks("networks.data");
            Network network = networks.Single(x => x.Name == Name);
            networks.Remove(network);
        }

        internal void Internal_Update_Single_Network_In_Flagged(Network network)
        {
            Internal_Remove_Network_As_Flagged(network.Name);
            Network[] networkArray = new Network[0];
            networkArray[0] = network;
            Internal_Save_Network_As_Flagged("networks.data", networkArray);
        }

        internal void Internal_Update_Many_Networks_In_Flagged(Network[] networks)
        {
            foreach (Network network in networks)
            {
                Internal_Remove_Network_As_Flagged(network.Name);
                Internal_Save_Network_As_Flagged("networks.data", networks);
            }            
        }
    }
}
