using Google.Protobuf.Collections;
using Sandwych.SmartConfig;
using Sandwych.SmartConfig.Esptouch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Wifi_List
{
   public class Internal_Networks_Scanner
    {
        internal async Task<RepeatedField<Network>> Internal_List_Visible_Networks()
        {
            //so we start out by seeing what we can see
            List<UI_Network_Info> network_Mac_and_IPs = new List<UI_Network_Info>();
            Sandwych.SmartConfig.SmartConfigArguments args = new Sandwych.SmartConfig.SmartConfigArguments();

           await SmartConfigStarter.StartAsync<EspSmartConfigProvider>(args, onDeviceDiscovered: (s, e) => 
           network_Mac_and_IPs.Add(new UI_Network_Info{
               IPAddress = e.Device.IPAddress.ToString(), 
               MACaddress = e.Device.MacAddress.ToString()}));

            RepeatedField<Network> scannedNetworks = new RepeatedField<Network>();
            NetworksCRUD networksCRUD = new NetworksCRUD();

            var flaggedNetworks = networksCRUD.Internal_Get_All_Flagged_Networks("networks.data");

            List<Network> networks = new List<Network>();

            //then for each thing that we can see, we look for what we already know about it.
            foreach (UI_Network_Info visibleDevices in network_Mac_and_IPs)
            {
                Internal_Builder_Pattern_Create_Network(visibleDevices);
                foreach (Network network in networks)
                {
                    if (flaggedNetworks.Contains(network))
                {
                        Alarm();

                    } 
                }
            }
        }

        private void Internal_Builder_Pattern_Create_Network(UI_Network_Info visibleDevice)
        {
                Director director = new Director();
                BuilderAbstract builderAbstract = new Builder();                

                director.Construct(builderAbstract, visibleDevice);
                Network networkConstruction = builderAbstract.GetResult();                              
            }
        }

        class Director
        {
            public void Construct(BuilderAbstract builder, UI_Network_Info visibleDevice)
            {
                builder.Add_Network_Info(visibleDevice);
                builder.Add_Other_Info();
            }
        }

        abstract class BuilderAbstract
        {
            public abstract void Add_Network_Info(UI_Network_Info visibleDevice);
            public abstract void Add_Other_Info();
            public abstract Network GetResult();
        }

        class Builder : BuilderAbstract
        {
            private Network network = new Network();

            public override void Add_Network_Info(UI_Network_Info visibleDevice)
            {
                network.Macaddresses.Add(visibleDevice.MACaddress);
                network.IPAddress.Add(visibleDevice.IPAddress);
            }

            public override void Add_Other_Info()
            {
            network.Name = ("Name"); //get the name from the UI
            network.Id = Convert.ToInt32(Guid.NewGuid());
            network.Warning = ("Warning") //get the warning message from the UI
            }

        public override Network GetResult()
        {
            return network;
        }
    }



       

        internal class UI_Network_Info
        {
            public string IPAddress { get; set; }
            public string MACaddress { get; set; }
        }
    }
}