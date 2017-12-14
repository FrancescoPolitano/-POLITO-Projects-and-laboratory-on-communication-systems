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
    public partial class UserDetails : Window
    {
        private Employee user;
        public UserDetails(Employee u)
        {
            InitializeComponent();
            User = u;
            OuterGrid.DataContext = User;
            InnerGrid.DataContext = User;
        }

        public Employee User { get => user; set => user = value; }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Employee u = b.DataContext as Employee;
            UserModification uw = new UserModification(u);
            uw.ShowDialog();
        }
    }
}
