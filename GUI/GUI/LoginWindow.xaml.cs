﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        private static bool restart = false;
        private LoginData myLoginData = new LoginData();
        public LoginWindow()
        {
            InitializeComponent();
            myGrid.DataContext = myLoginData;

        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(username.Text))
            {
                errors.Text = "Username is empty";
                return;
            }
            if (String.IsNullOrEmpty(password.Password))
            {
                errors.Text = "Password is empty";
                return;
            }

            myLoginData.Password = password.Password;
            if (RestClient.Login(myLoginData))
            {
                PageChange(Constants.ADMIN);
                Close();
            }
            else
            {
                errors.Text = "Something is wrong, retry";
                username.Text = "";
                password.Password = "";
                return;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            PageChange(Constants.DOORMAN);
            Close();
        }


        public delegate void myDelegate(string UserType);
        public static event myDelegate PageChange;

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!RestClient.loggedIn && restart)
            {
                MessageDialogResult mr = this.ShowModalMessageExternal("Ops", "è necessario effettuare l'accesso per eseguire questa operazione", MessageDialogStyle.AffirmativeAndNegative);
                if (mr != MessageDialogResult.Affirmative)
                    e.Cancel = false;
                else
                    e.Cancel = true;
            }
            else
                restart = true;
        }
    }
}
