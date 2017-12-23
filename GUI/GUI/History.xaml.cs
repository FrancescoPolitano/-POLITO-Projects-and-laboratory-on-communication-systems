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
        private ObservableCollection<string> users = new ObservableCollection<string>();
        private ObservableCollection<string> rooms = new ObservableCollection<string>();
        private ObservableCollection<string> listUsers = new ObservableCollection<string>();
        private ObservableCollection<string> listRooms = new ObservableCollection<string>();
        private ObservableCollection<Access> contenuto = new ObservableCollection<Access>();

        public ObservableCollection<Access> Contenuto { get => contenuto; set => contenuto = value; }

        public History()
        {
            InitializeComponent();
            CalendarDateRange cdr = new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue);
            fromDate.BlackoutDates.Add(cdr);
            toDate.BlackoutDates.Add(cdr);
            if (App.userList != null)
                foreach(Employee user in App.userList)
                    users.Add(user.Name + " " + user.Surname + " " + user.Serial);
            if (App.roomList != null)
                foreach (Room room in App.roomList)
                    rooms.Add(room.Name);
            users.Add("Tutti");
            users.Add("Gianmaria Tremigliozzi 2");
            users.Add("Cristiano Palazzi 4");
            users.Add("Francesco Politano 1");
            users.Add("Alfredo Nazzaro 5");
            users.Add("Gabriele Basile 9");
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
            listContent.ItemsSource = Contenuto;
        }

        private void Users_CLICK(object sender, MouseButtonEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            string utente = sp.DataContext.ToString();
            if (listUsers.Contains(utente))
                return;
            if (String.Compare(utente, "Tutti") == 0)
            {
                listUsers.Clear();
                listUsers.Add(utente);

            }
            else
            {
                if (listUsers.Contains("Tutti"))
                    listUsers.Remove("Tutti");
                listUsers.Add(sp.DataContext.ToString());
            }
            requestData();

        }

        private void Room_click(object sender, MouseButtonEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            string stanza = sp.DataContext.ToString();
            if (listRooms.Contains(stanza))
                return;
            if (String.Compare(stanza, "Tutte") == 0)
            {
                listRooms.Clear();
                listRooms.Add(stanza);
            }

            else
            {
                if (listRooms.Contains("Tutte"))
                    listRooms.Remove("Tutte");
                listRooms.Add(sp.DataContext.ToString());
            }
            requestData();

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
            DateTime t = DateTime.MaxValue;
            t = toDate.SelectedDate ?? t;
            DateTime from = DateTime.MaxValue;
            from = fromDate.SelectedDate ?? from;
            if (DateTime.Compare(t, from) < 0)
                this.ShowModalMessageExternal("Ops", "The \"from\" date cannot be after the \"to\" date");
            else
                requestData();


        }

        private void toDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime t = DateTime.MaxValue;
            t = toDate.SelectedDate ?? t;
            DateTime from = DateTime.MinValue;
            from = fromDate.SelectedDate ?? from;
            if (DateTime.Compare(t, from) < 0)
                this.ShowModalMessageExternal("Ops", "The \"to\" date cannot be before the \"from\" date");
            else
                requestData();

        }


        private async void requestData()
        {
            List<Access> result;
            if (listRooms.Count == 0)
                listRooms.Add("Tutte");
            if (listUsers.Count == 0)
                listUsers.Add("Tutti");
          
            result =  RestClient.GetHistory(createArrays(fromDate.SelectedDate,toDate.SelectedDate));
            if (result != null)
                foreach (Access acc in result)
                    Contenuto.Add(acc);
        }

        private ComplexQuery createArrays(DateTime? t, DateTime? t1)
        {
            ComplexQuery qr = new ComplexQuery();
            List<string> temp = new List<string>();
            foreach (string st in listUsers)
            {
                if (String.IsNullOrEmpty(st))
                    continue;
                if (String.Compare(st, "Tutti") == 0)
                {
                    temp.Add(st);
                    continue;
                }
                temp.Add(st.Substring(st.LastIndexOf(" ")));
            }
            qr.Employees = temp;
            qr.Rooms = new List<string>(listRooms);
            if (t != null)
                qr.Initial = t ?? DateTime.MinValue;
            if (t1 != null)
                qr.End = t1 ?? DateTime.MaxValue;
            return qr;
        }
    }
}
