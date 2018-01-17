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
            AuthLevel.Items.Add("1");
            AuthLevel.Items.Add("2");
            AuthLevel.Items.Add("3");
            User = u;
            name.Text = User.Name;
            surname.Text = User.Surname;
            email.Text = User.Email;
            switch (User.AuthLevel)
            {
                case "1": AuthLevel.SelectedIndex = 0; break;
                case "2": AuthLevel.SelectedIndex = 1; break;
                case "3": AuthLevel.SelectedIndex = 2; break;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(name.Text))
            {
                this.ShowModalMessageExternal("Ops", "Insert a name");
                return;
            }
            if (string.IsNullOrEmpty(surname.Text))
            {
                this.ShowModalMessageExternal("Ops", "Insert a surname");
                return;
            }
            if (string.IsNullOrEmpty(email.Text))
            {
                this.ShowModalMessageExternal("Ops", "Insert a email");
                return;
            }

            User.Name = name.Text;
            User.Surname = surname.Text;
            User.Email = email.Text;
            User.AuthLevel = AuthLevel.SelectedItem as string;

            if (!RestClient.ModifyUser(User))
                this.ShowModalMessageExternal("Ops", "Something went wrong");
            Close();

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
