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
        string name, surname, causal, expiration,authLevel,currentPosition;

        public Visitor() { }

        public Visitor(string name, string surname, string motivation, string expiryDate, string level)
        {
            Name = name;
            Surname = surname;
            Causal = motivation;
            Expiration = expiryDate;
            AuthLevel = level;
        }

        public Visitor(string name, string surname, string motivation, string expiryDate,string level,string currentPos) : this(name, surname, motivation, expiryDate,level)
        {
            CurrentPosition = currentPos;
        }


        public string Name { get => name; set { name = value; NotifyPropertyChanged("Name"); } }
        public string Surname { get => surname; set { surname = value; NotifyPropertyChanged("Surname"); } }
        public string Causal { get => causal; set { causal = value; NotifyPropertyChanged("Causal"); } }
        public string Expiration { get => expiration; set { expiration = value; NotifyPropertyChanged("Expiration"); } }

        public string AuthLevel { get => authLevel; set { authLevel = value; NotifyPropertyChanged("AuthLevel"); } }
        public string CurrentPosition { get => currentPosition; set { currentPosition = value; NotifyPropertyChanged("CurrentPosition"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
