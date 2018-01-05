namespace GUI
{
    class AuthLevelClass
    {
        public AuthLevelClass(string authLevel, int serialNumber)
        {
            AuthLevel = authLevel;
            SerialNumber = serialNumber;
        }
        private string authLevel;
        private int serialNumber;

        public string AuthLevel { get => authLevel; set => authLevel = value; }
        public int SerialNumber { get => serialNumber; set => serialNumber = value; }
    }
}