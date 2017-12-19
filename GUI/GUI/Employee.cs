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
        String name, surname, authLevel, currentPosition, pathPhoto, email;
        private int serial;

        public Employee() { }

        public Employee(string name, string surname, string level, string email)
        {
            Name = name;
            Surname = surname;
            AuthLevel = level;
            Email = email;
        }

        public Employee(string name, string surname, string level, string email, string curPos, string imgPath) : this(name, surname, level, email)
        {
            if (!String.IsNullOrEmpty(imgPath))
                PathPhoto = imgPath;
            CurrentPosition = curPos;
        }

        public Employee(string name, string surname, string level, string email, string curPos, string imgPath, int serial) : this(name, surname, level, email, curPos, imgPath)
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
        public string CurrentPosition
        {
            get => currentPosition;
            set
            {
                currentPosition = value;
                NotifyPropertyChanged("CurrentPosition");
            }
        }
        public int Serial { get => serial; set => serial = value; }
        public string PathPhoto
        {
            get => pathPhoto;
            set
            {
                pathPhoto = value;
                NotifyPropertyChanged("PathPhoto");
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                NotifyPropertyChanged("Email");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
