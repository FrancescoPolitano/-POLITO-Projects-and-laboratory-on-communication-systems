using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GUI
{
    public class Employee : INotifyPropertyChanged
    {
        String name, surname, authLevel, currentPosition, pathPhoto;
        private int serial;

        public Employee() { }

        public Employee (string name,string surname,string level)
        {
            Name = name;
            Surname = surname;
            AuthLevel = level;
        }

        public Employee(string name, string surname, string level, string curPos, string imgPath) : this(name,surname,level)
        {
            if (!String.IsNullOrEmpty(imgPath))
                PathPhoto = imgPath;
            CurrentPosition = curPos;
        }

        public Employee(string name, string surname, string level, string curPos, string imgPath, int serial) : this(name, surname, level, curPos, imgPath)
        {
            Serial = serial;
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
        public string Surname
        {
            get => surname;
            set
            {
                surname = value;
                NotifyPropertyChanged("Surname");
            }
        }
        public string AuthLevel
        {
            get => authLevel;
            set
            {
                authLevel = value;
                NotifyPropertyChanged("AuthLevel");
            }
        }
        public string CurrentPosition { get => currentPosition; set => currentPosition = value; }
        public int Serial { get => serial; set => serial = value; }
        public string PathPhoto {
            get => pathPhoto;
            set
            {
                pathPhoto = value;
                NotifyPropertyChanged("PathPhoto");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
