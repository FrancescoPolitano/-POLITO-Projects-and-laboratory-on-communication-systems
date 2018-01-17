using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
    /// Interaction logic for UserDetails.xaml
    /// </summary>
    public partial class UserDetails : MetroWindow
    {
        private Employee user;
        public UserDetails(Employee u)
        {
            InitializeComponent();
            User = u;
            OuterGrid.DataContext = User;
            InnerGrid.DataContext = User;
            if (String.Compare(User.AuthLevel, "0") == 0)
                Role.Text = "AuthLevel: Access Blocked";
        }

        public Employee User { get => user; set => user = value; }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Employee u = b.DataContext as Employee;
            UserModification uw = new UserModification(u);
            uw.ShowDialog();
            if (String.Compare(User.AuthLevel, "0") == 0)
                Role.Text = "AuthLevel: Access Blocked";
            else
                Role.Text = "AuthLevel: " + User.AuthLevel;
        }

        private void QRChange_Click(object sender, RoutedEventArgs e)
        {
            this.ShowModalMessageExternal("ChangingQRCode", "A new QRCode is being generated");
            string result = RestClient.QRCodeChange(user.Serial.ToString());
            if (!string.IsNullOrEmpty(result))
            {
                QRCode qr = new QRCode(Constants.IPREMOTE + result);
                qr.ShowDialog();
            }
            else
                this.ShowModalMessageExternal("Ops", "An error occurred while changing the code");
        }

        private void BlockUser_Click(object sender, RoutedEventArgs e)
        {
            if (RestClient.BlockAccess(User.Serial))
            {
                User.AuthLevel = "0";
                Role.Text = "AuthLevel: Access Blocked";
            }
            else
                this.ShowModalMessageExternal("Ops", "Cannot block the user right now");
        }
    }
}
