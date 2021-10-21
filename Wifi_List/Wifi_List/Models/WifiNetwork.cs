using System;
using System.Collections.Generic;
using System.Text;

namespace Wifi_List
{
    public class WifiNetwork
    {
        public string Name { get; set; }
        public string MacAddress {  get; set; }
        public bool Flagged { get; set; }
        public string Message { get; set; }

    }
}
