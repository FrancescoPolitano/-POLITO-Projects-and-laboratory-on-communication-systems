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
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;

namespace GUI
{
    /// <summary>
    /// Interaction logic for VisitorCreation.xaml
    /// </summary>
    public partial class VisitorCreation : MetroWindow
    {
        public VisitorCreation()
        {
            InitializeComponent();
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
            if (String.IsNullOrEmpty(ExpiryDate.Text))
            {
                //errore data
                this.ShowModalMessageExternal("Ops", "Insert a valid expiryDate");
                return;
            }
            //TODO condizione per verificare se la data è già passata
            //else if ()

            Visitor v = new Visitor(Name.Text, Surname.Text, Motivation.Text, ExpiryDate.Text);
            if (await RestClient.CreateVisitor(v))
            {
                //stampare successo
            }
            else
            {
                //stampare fallimento
            }


        }

        private void Cancel_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
