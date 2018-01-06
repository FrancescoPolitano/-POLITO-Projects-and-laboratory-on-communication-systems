using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
   public class Access
    {
        private string employeeID, employeeName, employeeSurname, localName;
        private DateTime time;
        private bool result;

        public string EmployeeID { get => employeeID; set => employeeID = value; }
        public string EmployeeName { get => employeeName; set => employeeName = value; }
        public string EmployeeSurname { get => employeeSurname; set => employeeSurname = value; }
        public string LocalName { get => localName; set => localName = value; }
        public DateTime Time { get => time; set => time = value; }
        public bool Result { get => result; set => result = value; }

        public Access() { }

        public Access(string employeeID, string employeeName, string employeeSurname, string roomName, DateTime time, bool result)
        {
            EmployeeID = employeeID;
            EmployeeName = employeeName;
            EmployeeSurname = employeeSurname;
            LocalName = roomName;
            Time = time;
            Result = result;
        }
    }
}
