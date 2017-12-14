using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class EmployeeRequestClass
    {
        
        private byte[] photo;

        private Employee employee;

        public EmployeeRequestClass() { }

        public EmployeeRequestClass(Employee u, byte[] p)
        {
            Employee = u;
            Photo = p;
        }

        [JsonConverter(typeof(ByteArrayConverter))]
        public byte[] Photo { get => photo; set => photo = value; }

        public Employee Employee { get => employee; set => employee = value; }
    }
}
