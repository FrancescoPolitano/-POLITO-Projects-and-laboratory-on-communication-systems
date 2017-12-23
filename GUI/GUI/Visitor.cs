using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public class Visitor : INotifyPropertyChanged
    {
        string name, surname, causal, expiration;

        public Visitor() { }

        public Visitor(string name, string surname, string motivation, string expiryDate)
        {
            Name = name;
            Surname = surname;
            Causal = motivation;
            Expiration = expiryDate;
        }

        public string Name { get => name; set { name = value; NotifyPropertyChanged("Name"); } }
        public string Surname { get => surname; set { surname = value; NotifyPropertyChanged("Surname"); } }
        public string Causal { get => causal; set { causal = value; NotifyPropertyChanged("Causal"); } }
        public string Expiration { get => expiration; set { expiration = value; NotifyPropertyChanged("Expiration"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
