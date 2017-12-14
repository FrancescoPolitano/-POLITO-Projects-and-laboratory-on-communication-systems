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
        private string qrCodeString;

        public EmployeeResponseClass() { }

        public EmployeeResponseClass(Employee u , String url)
        {
            Employee = u;
            QrCodeString = url;
        }

        public string QrCodeString { get => qrCodeString; set => qrCodeString = value; }
        public Employee Employee { get => employee; set => employee = value; }
    }
}
