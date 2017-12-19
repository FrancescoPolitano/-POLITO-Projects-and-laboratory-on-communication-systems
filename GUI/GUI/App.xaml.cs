﻿using System;
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
        public static List<Employee> userList = new List<Employee>();
        public static List<Room> roomList = new List<Room>();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            BigWindow.CreateUser += CreateUser;
            LoginWindow.PageChange += LoginWindow_PageChange;
            userList = RestClient.GetAllUsers();
            Thread t = new Thread(UserListUpdate)
            {
                Name="thread Aggiornamenti",
                IsBackground = true
            };
            t.Start();
            roomList = RestClient.GetAllRooms();
        }

        private void LoginWindow_PageChange(string UserType)
        {
            mw = new BigWindow( UserType);
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

        private void UserListUpdate()
        {
            while (true)
            {
                Thread.Sleep(60000);
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
                
            }
        }
    }
}
