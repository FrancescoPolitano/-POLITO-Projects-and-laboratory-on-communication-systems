using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class Visitor : INotifyPropertyChanged
    {
        string name, surname, motivation, expiryDate;

        public Visitor() { }

        public Visitor(string name, string surname, string motivation, string expiryDate)
        {
            Name = name;
            Surname = surname;
            Motivation = motivation;
            ExpiryDate = expiryDate;
        }

        public string Name { get => name; set { name = value; NotifyPropertyChanged("Name"); } }
        public string Surname { get => surname; set { surname = value; NotifyPropertyChanged("Surname"); } }
        public string Motivation { get => motivation; set { motivation = value; NotifyPropertyChanged("Motivation"); } }
        public string ExpiryDate { get => expiryDate; set { expiryDate = value; NotifyPropertyChanged("ExpiryDate"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
