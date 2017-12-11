using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : MetroWindow
    {
        private ObservableCollection<String> users = new ObservableCollection<string>();
        private ObservableCollection<String> rooms = new ObservableCollection<string>();
        private ObservableCollection<String> listUsers = new ObservableCollection<string>();
        private ObservableCollection<String> listRooms = new ObservableCollection<string>();

        public History()
        {
            InitializeComponent();
            CalendarDateRange cdr = new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue);
            fromDate.BlackoutDates.Add(cdr);
            toDate.BlackoutDates.Add(cdr);
            users.Add("Tutti");
            users.Add("Tremigliozzi 2");
            users.Add("Palazzi 4");
            users.Add("Politano 1");
            users.Add("Nazzaro 5");
            users.Add("Basile 9");
            rooms.Add("Tutte");
            rooms.Add("Laboratorio");
            rooms.Add("Cancello");
            rooms.Add("CEO ");
            rooms.Add("Batalfonso");
            rooms.Add("Ingresso");
            Users.ItemsSource = users;
            Rooms.ItemsSource = rooms;
            SelectedEmployees.ItemsSource = listUsers;
            SelectedRooms.ItemsSource = listRooms;
        }

        private void Users_CLICK(object sender, MouseButtonEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            string utente = sp.DataContext.ToString();
            if (String.Compare(utente, "Tutti") == 0)
            {
                listUsers.Clear();
                listUsers.Add(utente);
            }
            if (listUsers.Contains(utente))
                return;
            else
            {
                if (listUsers.Contains("Tutti"))
                    listUsers.Remove("Tutti");
                listUsers.Add(sp.DataContext.ToString());
            }
        }

        private void Room_click(object sender, MouseButtonEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            string stanza = sp.DataContext.ToString();
            if (String.Compare(stanza, "Tutte") == 0)
            {
                listRooms.Clear();
                listRooms.Add(stanza);
            }
            if (listRooms.Contains(stanza))
                return;
            else
            {
                if (listRooms.Contains("Tutte"))
                    listRooms.Remove("Tutte");
                listRooms.Add(sp.DataContext.ToString());
            }
        }

        private void MenuItem_Click_Employees(object sender, RoutedEventArgs e)
        {
            if (SelectedEmployees.SelectedIndex == -1)
                return;
            else
            {
                string employee = SelectedEmployees.SelectedItem as string;
                listUsers.Remove(employee);
            }
        }

        private void MenuItem_Click_Rooms(object sender, RoutedEventArgs e)
        {
            if (SelectedRooms.SelectedIndex == -1)
                return;
            else
            {
                string room = SelectedRooms.SelectedItem as string;
                listRooms.Remove(room);
            }
        }

        private void fromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime t  = DateTime.MaxValue;
            t = toDate.SelectedDate ?? t;
            DateTime from = DateTime.MaxValue;
            from = fromDate.SelectedDate ?? from;
            if (DateTime.Compare(t, from) < 0)
            {
                this.ShowModalMessageExternal("Ops", "The \"from\" date cannot be after the \"to\" date");
                if (DateTime.Compare(t, DateTime.MaxValue) == 0)
                    fromDate.SelectedDate = DateTime.Now;
                else
                    fromDate.SelectedDate = t;
            }

        }

        private void toDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime t = DateTime.MaxValue;
            t = toDate.SelectedDate ?? t;
            DateTime from = DateTime.MinValue;
            from = fromDate.SelectedDate ?? from;
            if (DateTime.Compare(t, from) < 0)
            {
                this.ShowModalMessageExternal("Ops", "The \"to\" date cannot be before the \"from\" date");
                toDate.SelectedDate = DateTime.Now;
            }
        }
    }
}
