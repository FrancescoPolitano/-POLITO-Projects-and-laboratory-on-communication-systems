using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class EmployeeResponseClass
    {
        private User user;
        private string qrCodeString;

        public EmployeeResponseClass() { }

        public EmployeeResponseClass(User u , String url)
        {
            User = u;
            QrCodeString = url;
        }

        public User User { get => user; set => user = value; }
        public string QrCodeString { get => qrCodeString; set => qrCodeString = value; }
    }
}
