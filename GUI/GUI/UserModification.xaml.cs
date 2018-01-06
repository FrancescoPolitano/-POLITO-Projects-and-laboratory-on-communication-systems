using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace GUI
{
    public partial class UserModification : MetroWindow
    {
        private Employee user;

        public Employee User { get => user; set => user = value; }

        public UserModification(Employee u)
        {
            InitializeComponent();
            User = u;
            myGrd.DataContext = User;
        }

        private async void RoleChange_Click(object sender, RoutedEventArgs e)
        {
            string result = await this.ShowInputAsync("Changing Authorization level", "Please insert a new Authorhization Level", new MetroDialogSettings());
            if (!string.IsNullOrEmpty(result))
            {
                AuthLevelClass auth = new AuthLevelClass(result, User.Serial);
                //TODO test this
                if (!RestClient.RoleChange(auth))
                    this.ShowModalMessageExternal("Ops", "Cannot change authorization level right now.");
                else User.AuthLevel = result;
                Close();
            }

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
            //TODO blocco permanente dell'utente
            if (RestClient.BlockAccess(User.Serial))
                User.AuthLevel = "0";
            else
                this.ShowModalMessageExternal("Ops", "Cannot block the user right now");
            Close();
        }

    }
}
