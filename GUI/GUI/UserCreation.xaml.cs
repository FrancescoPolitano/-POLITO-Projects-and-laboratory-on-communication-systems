
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace GUI
{
    /// <summary>
    /// Interaction logic for UserCreation.xaml
    /// </summary>
    public partial class UserCreation : MetroWindow
    {
        User user = new User();

        public User User { get => user; set => user = value; }

        public UserCreation()
        {
            InitializeComponent();
            User.PathPhoto = "F:\\Downloads\\farmer.png";
            myGrid.DataContext = User;
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            //take fields and trigger something to do post
            if (String.IsNullOrEmpty(Name.Text))
            {
                this.ShowModalMessageExternal("Ops", "Insert a valid name");
                return;
            }
            if (String.IsNullOrEmpty(Surname.Text))
            {
                //errore Cognome
                this.ShowModalMessageExternal("Ops", "Insert a valid surname");
                return;
            }
            if (String.IsNullOrEmpty(Role.Text))
            {
                //errore ruolo
                this.ShowModalMessageExternal("Ops", "Insert a valid role");
                return;
            }

            BigWindow.UserList.Add(user);
            if (await RestClient.CreateUser(user))
            {
                //stampare successo
                System.Windows.MessageBox.Show("Successo");
                Close();

            }
            else
            {
                //stampare fallimento
                System.Windows.MessageBox.Show("fallimento");
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChoosePic_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Images (*.png,*.jpg,*.jpeg) | *.png;*.jpg;*.jpeg |All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            if ( openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                User.PathPhoto = openFileDialog1.FileName;
            }
        }
    }
}
