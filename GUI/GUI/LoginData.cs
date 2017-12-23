using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class LoginData : INotifyPropertyChanged
    {
        private string username, password;

        public LoginData() { }

        public LoginData(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                NotifyPropertyChanged("Email");
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                NotifyPropertyChanged("Password");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
