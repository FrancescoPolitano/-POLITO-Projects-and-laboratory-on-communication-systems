namespace ServerFEZ
{
    public static class Constants
    {
        public const int PORT_TCP = 8989;
        public const string DOOR_ID = "OFFICE4H";
        //public const string STATIC_IP_SERVER = "192.168.1.133";
        public const string URI = "http://13.59.45.212:8080/RestfulService/resources/auth/";

        public const string STATIC_IP_SERVER = "172.20.10.4";
        //public const string URI = "http://172.20.10.3:8082/RestfulService/resources/auth/";
        public enum EVALUATION { ACCEPT, REJECT, NOCODE }
    }
}