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
        public LoginWindow()
        {
            InitializeComponent();
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

            //TODO temporary, while fixing login
            PageChange("ADMIN");
            Close();

            //if (RestClient.Login(username.Text, password.Password).Result)
            //{
            //    PageChange();
            //    Close();
            //}
            //else
            //{
            //    errors.Text = "Something is wrong, retry";
            //    return;
            //}
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public delegate void myDelegate(string UserType);
        public static event myDelegate PageChange;
    }
}
