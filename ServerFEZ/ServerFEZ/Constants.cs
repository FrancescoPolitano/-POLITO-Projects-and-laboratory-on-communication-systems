using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerFEZ
{
    public static class Constants
    {
        public const int PACKET_SIZE = 1400;
        public const int PORT_TCP = 8989;
        public const string IP_LOCAL = "192.168.100.1";
        public const string DOOR_ID = "ABCDEFGH";
        public enum EVALUATION { ACCEPT, REJECT, NOCODE }
    }
}
