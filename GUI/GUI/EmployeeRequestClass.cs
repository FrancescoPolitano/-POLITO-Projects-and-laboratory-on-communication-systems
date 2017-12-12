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
        private User user;

        public EmployeeRequestClass() { }

        public EmployeeRequestClass(User u, byte[] p)
        {
            User = u;
            Photo = p;
        }

        public byte[] Photo { get => photo; set => photo = value; }
        public User User { get => user; set => user = value; }
    }
}
