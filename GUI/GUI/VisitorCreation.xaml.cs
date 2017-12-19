﻿using MahApps.Metro.Controls;
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
using MahApps.Metro.Controls.Dialogs;

namespace GUI
{

    /// <summary>
    /// Interaction logic for VisitorCreation.xaml
    /// </summary>
    public partial class VisitorCreation : MetroWindow
    {
        private Visitor myVisitor = new Visitor();

        internal Visitor MyVisitor { get => myVisitor; set => myVisitor = value; }

        public VisitorCreation()
        {
            InitializeComponent();
            myGrid.DataContext = MyVisitor;
            CalendarDateRange cdr = new CalendarDateRange(DateTime.MinValue, DateTime.Today);
            DatePick.BlackoutDates.Add(cdr);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            //take fields and trigger something to do post
            if (String.IsNullOrEmpty(Name.Text))
            {
                //errore Nome
                this.ShowModalMessageExternal("Ops", "Insert a valid name");
                return;
            }
            if (String.IsNullOrEmpty(Surname.Text))
            {
                //errore Cognome
                this.ShowModalMessageExternal("Ops", "Insert a valid surname");
                return;
            }
            if (String.IsNullOrEmpty(DatePick.Text))
            {
                //errore data
                this.ShowModalMessageExternal("Ops", "Insert a valid expiryDate");
                return;
            }


            
            myVisitor.Expiration = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DatePick.SelectedDate);
            Console.WriteLine("Puppe");
            //TODO CHANGE : cambiare valore di ritorno, per prendere il path del QR code nuovo
            string QRcodeNew = await RestClient.CreateVisitor(myVisitor);
            if (!String.IsNullOrEmpty(QRcodeNew))
            {
                QRCode qr = new QRCode(Constants.IPREMOTE + QRcodeNew);
                qr.ShowDialog();
                Close();
            }
            else
            {
                //stampare fallimento
                this.ShowModalMessageExternal("Ops", "Error while creating this visitor");
                Close();
            }
        }

        private void Cancel_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
