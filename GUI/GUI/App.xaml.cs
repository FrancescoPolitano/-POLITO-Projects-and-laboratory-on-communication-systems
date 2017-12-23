using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        BigWindow mw;
        UserCreation uc;
        VisitorCreation vc;
        private string userType = String.Empty;
        public static List<Employee> userList;
        public static List<Room> roomList;
        public static List<Visitor> visitorList;
        private Thread t;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            BigWindow.CreateUser += CreateUser;
            LoginWindow.PageChange += LoginWindow_PageChange;
            SystemEvents.SessionEnded += SystemEvents_SessionEnded;
            //TODO mettere qualcosa per il get del portinaio ( atm se non sei loggato restituisce null)

            t = new Thread(UserListUpdate)
            {
                Name = "thread Aggiornamenti",
                IsBackground = true
            };
        }

        private void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e)
        {
            if (String.Compare(userType, Constants.ADMIN) == 0)
                RestClient.Logout();
        }

        private void LoginWindow_PageChange(string UserType)
        {
            //chiamate ridondate per non prendere i null
            if (userList == null)
            {
                userList = RestClient.GetAllUsers();
                if (userList == null)
                    userList = new List<Employee>();
            }
            if (roomList == null)
            {
                roomList = RestClient.GetAllRooms();
                if (roomList == null)
                    roomList = new List<Room>();
            }
            if (visitorList == null)
            {
                visitorList = RestClient.GetAllVisitors();
                if (visitorList == null)
                    visitorList = new List<Visitor>();
            }
            if (t != null)
                t.Start();
            userType = UserType;
            mw = new BigWindow(UserType);
            mw.Show();
        }

        private void CreateUser(bool temporary)
        {
            if (temporary)
            {
                vc = new VisitorCreation();
                vc.ShowDialog();
            }
            else
            {
                uc = new UserCreation();
                uc.ShowDialog();
            }
        }

        private void UserListUpdate()
        {
            while (true)
            {
                List<Employee> temp = RestClient.GetAllUsers();
                foreach (Employee e in temp)
                {
                    foreach (Employee user in userList)
                        if (e.Serial == user.Serial)
                        {
                            if (String.Compare(e.CurrentPosition, user.CurrentPosition) != 0)
                                user.CurrentPosition = e.CurrentPosition;
                            break;
                        }
                }
                Thread.Sleep(60000);
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (String.Compare(userType, Constants.ADMIN) == 0)
                RestClient.Logout();
        }
    }
}
