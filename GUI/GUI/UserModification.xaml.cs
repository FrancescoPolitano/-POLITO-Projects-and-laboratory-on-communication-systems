using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace GUI
{
    public partial class UserModification : MetroWindow
    {
        private User user;

        public User User { get => user; set => user = value; }

        public UserModification(User u)
        {
            InitializeComponent();
            User = u;
            myGrd.DataContext = User;
        }

        private async void RoleChange_Click(object sender, RoutedEventArgs e)
        {
            string result = await this.ShowInputAsync("Changing Role", "Please choose a new role", new MetroDialogSettings());
            if (!string.IsNullOrEmpty(result))
            {
                User.AuthLevel = result;
                text.Text = result;
                //    bool change = RestClient.RoleChange(User).Result;
                //TODO aggiungere qualche cosa qua 
                Close();
            }

        }

        private void QRChange_Click(object sender, RoutedEventArgs e)
        {
            this.ShowModalMessageExternal("ChangingQRCode", "A new QRCode is being generated");
        }

        private void BlockUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Niente ancora non l'ho fatto");
        }

    }
}
