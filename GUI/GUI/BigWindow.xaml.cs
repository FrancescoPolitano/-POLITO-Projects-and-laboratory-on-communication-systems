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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BigWindow : MetroWindow
    {
      
        public BigWindow()
        {
            InitializeComponent();
            UserList = new ObservableCollection<User>();

            UserList.Add(new User("Giorgio", "Mastrota", "ADMIN", "ACASA", "F:\\Downloads\\Farmer.png", 0));
            UserList.Add(new User("Giorgio", "Mastrota", "DOORMAN", "ACASA", "F:\\Downloads\\Farmer2.png", 9));
            UserList.Add(new User("Giorgio", "Mastrota", "USER", "ACASA", "F:\\Downloads\\Farmer.png", 11));
            UserList.Add(new User("Giorgio", "Mastrota", "USER", "ACASA", "F:\\Downloads\\Farmer2.png", 12));
            UserList.Add(new User("Giorgio", "Mastrota", "USER", "ACASA", "F:\\Downloads\\Farmer2.png", 13));
            //foreach (User user in App.userList)
            //{
            //    if (String.IsNullOrEmpty(user.PathPhoto))
            //        user.PathPhoto = "F:\\Downloads\\Farmer.png";
            //    UserList.Add(user);
            //}

            Users.ItemsSource = UserList;
        }

        public static ObservableCollection<User> UserList { get => userList; set => userList = value; }
        private static ObservableCollection<User> userList;

        private void newUser_Click(object sender, RoutedEventArgs e)
        {
            CreateUser(false);
        }

        private void newVisitor_Click(object sender, RoutedEventArgs e)
        {
            CreateUser(true);
        }


        public delegate void myDelegate(bool temporary);
        public static event myDelegate CreateUser;

        private void Modify_User(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            User u = b.DataContext as User;
            UserModification uw = new UserModification(u);
            uw.Show();

        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                User user = item.DataContext as User;
                UserDetails uD = new UserDetails(user);
                uD.ShowDialog();


            }


        }

        private void Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
