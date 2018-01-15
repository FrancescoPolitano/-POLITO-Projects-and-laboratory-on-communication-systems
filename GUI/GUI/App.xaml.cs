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
                if (!t.IsAlive)
                    t.Start();
            userType = UserType;
            if (mw == null)
                mw = new BigWindow(UserType);
            createRooms();

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
                Thread.Sleep(1000);
                List<Employee> temp = RestClient.GetAllUsers();
                //TODO TESTARE BENE
                List<Visitor> temp2 = RestClient.GetAllVisitors();
                foreach (Visitor v in temp2)
                    temp.Add(v);
                foreach (Employee e in temp)
                {
                    bool exists = false;
                    if (e.GetType() == typeof(Employee))
                    {
                        foreach (Employee user in userList)
                            if (String.Compare(e.Serial, user.Serial) == 0)
                            {
                                exists = true;
                                if (String.Compare(e.CurrentPosition, user.CurrentPosition) != 0)
                                {
                                    changeRooms(user, e.CurrentPosition);
                                    user.CurrentPosition = e.CurrentPosition;
                                }
                                break;
                            }
                        if (!exists)
                        {
                            userList.Add(e);
                            addToRooms(e);
                            BigWindow.UserList.Add(e);
                            BigWindow.users.Add(e.Name + " " + e.Surname + " " + e.Serial);

                        }
                    }
                    else if (e.GetType() == typeof(Visitor))
                    {
                        Visitor visT = (Visitor)e;
                        foreach (Visitor visitor in visitorList)
                        {
                            if (String.Compare(visT.Serial, visitor.Serial) == 0)
                            {
                                exists = true;
                                if (String.Compare(visT.CurrentPosition, visitor.CurrentPosition) != 0)
                                {
                                    changeRooms(visitor, visT.CurrentPosition);
                                    visitor.CurrentPosition = visT.CurrentPosition;
                                }
                                break;
                            }
                        }
                        if (!exists)
                        {
                            visitorList.Add(visT);
                            addToRooms(visT);
                            BigWindow.VisitorList.Add(visT);
                        }
                    }
                }
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (String.Compare(userType, Constants.ADMIN) == 0)
                RestClient.Logout();
        }


        private void createRooms()
        {

            foreach (Employee e in userList)
            {
                switch (e.CurrentPosition)
                {
                    case Constants.stanza1:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room1.Add(e);
                        }));
                        break;
                    case Constants.stanza2:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room2.Add(e);
                        }));
                        break;
                    case Constants.stanza3:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room3.Add(e);
                        }));
                        break;
                    case Constants.stanza4:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room4.Add(e);
                        }));
                        break;

                }
            }

            foreach (Visitor v in visitorList)
            {
                switch (v.CurrentPosition)
                {
                    case Constants.stanza1:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room1.Add(v);
                        }));
                        break;
                    case Constants.stanza2:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room2.Add(v);
                        }));
                        break;
                    case Constants.stanza3:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room3.Add(v);
                        }));
                        break;
                    case Constants.stanza4:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room4.Add(v);
                        }));
                        break;
                }
            }

            mw.NomeStanza1.Text = Constants.stanza1;
            mw.NomeStanza2.Text = Constants.stanza2;
            mw.NomeStanza3.Text = Constants.stanza3;
            mw.NomeStanza4.Text = Constants.stanza4;
        }

        private void addToRooms(Employee user)
        {
            if(user.GetType() == typeof(Employee))
            {
                switch (user.CurrentPosition)
                {
                    case Constants.stanza1:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room1.Add(user);
                        }));
                        break;
                    case Constants.stanza2:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room2.Add(user);
                        }));
                        break;
                    case Constants.stanza3:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room3.Add(user);
                        }));
                        break;
                    case Constants.stanza4:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room4.Add(user);
                        }));
                        break;

                }
            }
            else if(user.GetType()== typeof(Visitor))
            {
                Visitor visitor = (Visitor)user;
                switch (visitor.CurrentPosition)
                {
                    case Constants.stanza1:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room1.Add(visitor);
                        }));
                        break;
                    case Constants.stanza2:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room2.Add(visitor);
                        }));
                        break;
                    case Constants.stanza3:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room3.Add(visitor);
                        }));
                        break;
                    case Constants.stanza4:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room4.Add(visitor);
                        }));
                        break;
                }
            }
        }
        private void changeRooms(Employee user, string arrivo)
        {

            if (user.GetType() == typeof(Employee))
            {

                switch (user.CurrentPosition)
                {
                    case Constants.stanza1:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room1.Remove(user);
                        }));
                        break;
                    case Constants.stanza2:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room2.Remove(user);
                        }));
                        break;
                    case Constants.stanza3:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room3.Remove(user);
                        }));
                        break;
                    case Constants.stanza4:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room4.Remove(user);
                        }));
                        break;

                }

                user.CurrentPosition = arrivo;
                switch (arrivo)
                {
                    case Constants.stanza1:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room1.Add(user);
                        }));
                        break;
                    case Constants.stanza2:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room2.Add(user);
                        }));
                        break;
                    case Constants.stanza3:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room3.Add(user);
                        }));
                        break;
                    case Constants.stanza4:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room4.Add(user);
                        }));
                        break;

                }
            }
            if (user.GetType() == typeof(Visitor))
            {
                Visitor visitor = (Visitor)user;

                switch (visitor.CurrentPosition)
                {
                    case Constants.stanza1:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room1.Remove(visitor);
                        }));
                        break;
                    case Constants.stanza2:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room2.Remove(visitor);
                        }));
                        break;
                    case Constants.stanza3:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room3.Remove(visitor);
                        }));
                        break;
                    case Constants.stanza4:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room4.Remove(visitor);
                        }));
                        break;

                }

                visitor.CurrentPosition = arrivo;
                switch (arrivo)
                {
                    case Constants.stanza1:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room1.Add(visitor);
                        }));
                        break;
                    case Constants.stanza2:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room2.Add(visitor);
                        }));
                        break;
                    case Constants.stanza3:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room3.Add(visitor);
                        }));
                        break;
                    case Constants.stanza4:
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            BigWindow.Room4.Add(visitor);
                        }));
                        break;

                }
            }



        }

    }
}
