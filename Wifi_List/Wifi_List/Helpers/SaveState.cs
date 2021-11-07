using Google.Protobuf;
using Google.Protobuf.Collections;
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
            EnumeratedNetworks openedFileStream;

            if (filename == String.Empty)
            {
               filename = "networks.data";
            }

            //This is the protobuf concrete class of repeated network classes
            using (Stream file = File.OpenRead(filename))
            {
                openedFileStream = EnumeratedNetworks.Parser.ParseFrom(file);
            }
            
            RepeatedField<Network> additions = new RepeatedField<Network>();
            additions.AddRange(networks);

            RepeatedField<Network> existingNetworks = new RepeatedField<Network>();
            var networkList = Internal_Get_All_Flagged_Networks(filename);
            existingNetworks.AddRange(networkList);

            //we now have a set of old and a set of new.

            existingNetworks.AddRange(additions);


            using (Stream output = File.OpenWrite(filename))
            {
                openedFileStream.WriteTo(output);
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
