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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            BigWindow.CreateUser += CreateUser;
            LoginWindow.PageChange += LoginWindow_PageChange;
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
