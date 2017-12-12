using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        public static List<User> userList = new List<User>();
        public static List<Room> roomList = new List<Room>();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            BigWindow.CreateUser += CreateUser;
            LoginWindow.PageChange += LoginWindow_PageChange;
            //userList = RestClient.GetAllUsers();
            //roomList = RestClient.GetAllRooms();
            //if (userList != null)
            //    foreach (User user in userList)
            //        Console.WriteLine("nome {0} cognome {1} ruolo {2} serial {3} currentPosition {4}", user.Name, user.Surname, user.AuthLevel, user.Serial, user.CurrentPosition);
        }

        private void LoginWindow_PageChange()
        {
            mw = new BigWindow();
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
                //do post here

            }
        }
    }
}
