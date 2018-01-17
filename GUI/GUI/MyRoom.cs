using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
  public  class MyRoom : INotifyPropertyChanged
    {
        string name;
        List<Employee> employees;

        public MyRoom()
        {
        }

        public MyRoom(string name, List<Employee> employees)
        {
            Name = name;
            Employees = employees;
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public List<Employee> Employees
        {
            get => employees;
            set
            {
                employees = value;
                NotifyPropertyChanged("Employees");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
