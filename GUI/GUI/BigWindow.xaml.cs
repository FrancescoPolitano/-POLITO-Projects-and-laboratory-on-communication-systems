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
using System.Windows.Interop;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BigWindow : MetroWindow
    {
        public static ObservableCollection<string> users = new ObservableCollection<string>();
        private ObservableCollection<Room> rooms = new ObservableCollection<Room>();
        private ObservableCollection<string> listUsers = new ObservableCollection<string>();
        private ObservableCollection<string> listRooms = new ObservableCollection<string>();
        private ObservableCollection<Access> contenuto = new ObservableCollection<Access>();
        private static ObservableCollection<Employee> userList;
        private static ObservableCollection<Visitor> visitorList;
        private static ObservableCollection<Employee> room1;
        private static ObservableCollection<Employee> room2;
        private static ObservableCollection<Employee> room3;
        private static ObservableCollection<Employee> room4;


        public static ObservableCollection<Employee> UserList { get => userList; set => userList = value; }
        internal static ObservableCollection<Visitor> VisitorList { get => visitorList; set => visitorList = value; }
        public ObservableCollection<Access> Contenuto { get => contenuto; set => contenuto = value; }

        public static ObservableCollection<Employee> Room1 { get => room1; set => room1 = value; }
        public static ObservableCollection<Employee> Room2 { get => room2; set => room2 = value; }
        public static ObservableCollection<Employee> Room3 { get => room3; set => room3 = value; }
        public static ObservableCollection<Employee> Room4 { get => room4; set => room4 = value; }

        public BigWindow(string UserType)
        {
            InitializeComponent();
            this.SourceInitialized += Window1_SourceInitialized;
            if (String.Compare(UserType, Constants.ADMIN) != 0)
            {
                History.Visibility = Visibility.Collapsed;
            }
            else
                History.Visibility = Visibility.Visible;

            users.Add("Tutti");
            rooms.Add(new Room
            {
                Name = "Tutte"
            });

            if (App.userList != null)
            {
                UserList = new ObservableCollection<Employee>(App.userList);
                foreach (Employee user in UserList)
                    users.Add(user.Name + " " + user.Surname + " " + user.Serial);
            }
            else
                UserList = new ObservableCollection<Employee>();
            if (App.visitorList != null)
                VisitorList = new ObservableCollection<Visitor>(App.visitorList);
            else
                VisitorList = new ObservableCollection<Visitor>();
            CalendarDateRange cdr = new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue);
            fromDate.BlackoutDates.Add(cdr);
            toDate.BlackoutDates.Add(cdr);
            if (App.roomList != null)
                foreach (Room room in App.roomList)
                    rooms.Add(room);

            Users.ItemsSource = UserList;
            Visitors.ItemsSource = VisitorList;

            Room1 = new ObservableCollection<Employee>();
            Room2 = new ObservableCollection<Employee>();
            Room3 = new ObservableCollection<Employee>();
            Room4 = new ObservableCollection<Employee>();

            ListaStanza1.ItemsSource = Room1;
            ListaStanza2.ItemsSource = Room2;
            ListaStanza3.ItemsSource = Room3;
            ListaStanza4.ItemsSource = Room4;

            Utenti.ItemsSource = users;
            Rooms.ItemsSource = rooms;
            SelectedEmployees.ItemsSource = listUsers;
            SelectedRooms.ItemsSource = listRooms;
            listContent.ItemsSource = Contenuto;
        }

        private void Window1_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }

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
            Employee u = b.DataContext as Employee;
            UserModification uw = new UserModification(u);
            uw.Show();

        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                Employee user = item.DataContext as Employee;
                UserDetails uD = new UserDetails(user);
                uD.ShowDialog();


            }


        }

        private void PlanimetryClick(object sender, MouseButtonEventArgs e)
        {
            //TODO TESTARE
            var item = sender as ListViewItem;
            if (item != null)
            {
                if (item.DataContext.GetType() == typeof(Employee))
                {
                    Employee user = item.DataContext as Employee;
                    UserDetails uD = new UserDetails(user);
                    uD.ShowDialog();
                }
                else
                {
                    Visitor visitor = item.DataContext as Visitor;
                    VisitorDetails vD = new VisitorDetails(visitor);
                    vD.ShowDialog();
                }
            }
        }

        private void Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            Room stanza = sp.DataContext as Room;
            if (listRooms.Contains(stanza.Name))
                return;
            if (String.Compare(stanza.Name, "Tutte") == 0)
            {
                listRooms.Clear();
                listRooms.Add(stanza.Name);
            }

            else
            {
                if (listRooms.Contains("Tutte"))
                    listRooms.Remove("Tutte");
                listRooms.Add(stanza.Name);
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
                if (SelectedEmployees.Items.Count != 0)
                    requestData();
                else
                    Contenuto.Clear();
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
                if (SelectedRooms.Items.Count != 0)
                    requestData();
                else
                    Contenuto.Clear();
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


        private void requestData()
        {
            List<Access> result;
            if (listRooms.Count == 0)
                listRooms.Add("Tutte");
            if (listUsers.Count == 0)
                listUsers.Add("Tutti");

            result = RestClient.GetHistory(createArrays(fromDate.SelectedDate, toDate.SelectedDate));
            if (result != null)
            {
                Contenuto.Clear();
                foreach (Access acc in result)
                    Contenuto.Add(acc);
            }
            else
                this.ShowModalMessageExternal("Ops", "Qualcosa non è andato a buon fine");
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
                    //qua ho cambiato, ora si manda vuoto se c'è solo TUTTI
                    continue;
                }
                temp.Add(st.Substring(st.LastIndexOf(" ") + 1));
            }
            qr.Employees = temp;
            List<String> temp2 = new List<string>();
            foreach (string st in listRooms)
            {
                //uguale a quello degli utenti, per non mandare TUTTE
                if (String.Compare("Tutte", st) == 0)
                    continue;
                else
                {
                    foreach (Room r in rooms)
                        if (String.Compare(r.Name, st) == 0)
                            temp2.Add(r.IdLocal);
                }
                //  temp2.Add(st);
            }
            qr.Rooms = temp2;
            //if (t != null)
            //    qr.Initial = t ?? DateTime.MinValue;
            //if (t1 != null)
            //    qr.End = t1 ?? DateTime.MaxValue;
            qr.Initial = t;
            qr.End = t1;
            return qr;
        }

        private void ListViewItem_VisitorClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                Visitor user = item.DataContext as Visitor;
                //todo here
                VisitorDetails vd = new VisitorDetails(user);
                vd.ShowDialog();
            }
        }
    }
}
