using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class EmployeeResponseClass
    {
        private Employee employee;
        private string qrCodeURL;

        public EmployeeResponseClass() { }

        public EmployeeResponseClass(Employee u , String url)
        {
            Employee = u;
            QrCodeURL = url;
        }

        public string QrCodeURL { get => qrCodeURL; set => qrCodeURL = value; }
        public Employee Employee { get => employee; set => employee = value; }
    }
}
