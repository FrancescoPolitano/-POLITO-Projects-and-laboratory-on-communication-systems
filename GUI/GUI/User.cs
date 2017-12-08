using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GUI
{
   public class User : INotifyPropertyChanged
    {
        String name, surname, role,currentPosition,imagePath;
        private int serial;


        public User(string name, string surname, string role, string curPos,string imgPath) 
        {
            Name = name;
            Surname = surname;
            Role = role;
            ImagePath = imgPath;
            CurrentPosition = curPos;
        }

        public User(string name,string surname,string role,string curPos,string imgPath,int serial):this(name,surname,role,curPos,imgPath)
        {
            Serial = serial;
        }

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Role { get => role; set => role = value; }
        public string CurrentPosition { get => currentPosition; set => currentPosition = value; }
        public string ImagePath { get => imagePath; set => imagePath = value; }
        public int Serial { get => serial; set => serial = value; }

        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
