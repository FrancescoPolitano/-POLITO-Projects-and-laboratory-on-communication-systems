namespace ServerFEZ
{
    public static class Constants
    {
        public const int PACKET_SIZE = 10000;
        public const int PORT_TCP = 8989;
        public const string IP_LOCAL = "192.168.100.1";
        public const string DOOR_ID = "OFFICE4H";
        public const string STATIC_IP_SERVER = "192.168.1.2";
        public const string URI = "http://18.218.18.37:8080/RestfulService/resources/auth/";
        public enum EVALUATION { ACCEPT, REJECT, NOCODE }
    }
}
