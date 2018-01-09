using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        private LoginData myLoginData = new LoginData();
        public LoginWindow()
        {
            InitializeComponent();
            myGrid.DataContext = myLoginData;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(username.Text))
            {
                errors.Text = "Username is empty";
                return;
            }
            if (String.IsNullOrEmpty(password.Password))
            {
                errors.Text = "Password is empty";
                return;
            }

            myLoginData.Password = password.Password;
            if (RestClient.Login(myLoginData))
            {
                PageChange(Constants.ADMIN);
                Close();
            }
            else
            {
                errors.Text = "Something is wrong, retry";
                username.Text = "";
                password.Password = "";
                return;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            //TODO non è proprio cosi nella versione finale, #imbroglio
            myLoginData.Username = "admin";
            myLoginData.Password = "admin";
            if (RestClient.Login(myLoginData))
                PageChange(Constants.ADMIN);
            Close();
        }


        public delegate void myDelegate(string UserType);
        public static event myDelegate PageChange;
    }
}
