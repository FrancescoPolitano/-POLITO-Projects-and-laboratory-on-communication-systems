﻿
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
        Employee user = new Employee();

        public Employee User { get => user; set => user = value; }

        public UserCreation()
        {
            InitializeComponent();
            myGrid.DataContext = User;
            Role.Items.Add("1");
            Role.Items.Add("2");
            Role.Items.Add("3");
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
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
                this.ShowModalMessageExternal("Ops", "Insert a valid Authorization Level");
                return;
            }
            if (String.IsNullOrEmpty(user.PathPhoto))
            {
                //errore photo
                this.ShowModalMessageExternal("Ops", "Insert a valid photo");
                return;
            }
            if (String.IsNullOrEmpty(Email.Text) ||!ValidatorExtensions.IsValidEmailAddress(Email.Text))
            {
                //errore ruolo
                this.ShowModalMessageExternal("Ops", "Insert a valid Email");
                return;
            }
            EmployeeResponseClass erc=null;
            try
            {
                 erc = RestClient.CreateUser(user);
            }
            catch { }
            if (erc == null)
                this.ShowModalMessageExternal("Ops", "Error creating the user");
            else
            {
                App.userList.Add(erc.Employee);
                BigWindow.UserList.Add(erc.Employee);
                BigWindow.users.Add(erc.Employee.Name + " " + erc.Employee.Surname + " " + erc.Employee.Serial);
                QRCode qr = new QRCode(Constants.IPREMOTE + erc.QrCodeURL);
                qr.ShowDialog();
            }
            Close();
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
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                User.PathPhoto = openFileDialog1.FileName;
                imageBox.Source = new ImageSourceConverter().ConvertFromString(openFileDialog1.FileName) as ImageSource;

            }
        }

        private void Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string auth = Role.SelectedItem as string;
            User.AuthLevel = Role.SelectedItem as string;
        }
    }
}
