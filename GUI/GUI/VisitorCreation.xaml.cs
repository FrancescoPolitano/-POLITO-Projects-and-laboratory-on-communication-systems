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
        private Visitor myVisitor;

        public Visitor MyVisitor { get => myVisitor; set => myVisitor = value; }

        public VisitorCreation()
        {
            MyVisitor = new Visitor();
            InitializeComponent();
            myGrid.DataContext = MyVisitor;
            CalendarDateRange cdr = new CalendarDateRange(DateTime.MinValue, DateTime.Today);
            DatePick.BlackoutDates.Add(cdr);
            VisitorLevels.Items.Add("1");
            VisitorLevels.Items.Add("2");
            VisitorLevels.Items.Add("3");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            //take fields and trigger something to do post
            if (String.IsNullOrEmpty(NameBox.Text))
            {
                //errore Nome
                this.ShowModalMessageExternal("Ops", "Insert a valid name");
                return;
            }
            if (String.IsNullOrEmpty(SurnameBox.Text))
            {
                //errore Cognome
                this.ShowModalMessageExternal("Ops", "Insert a valid surname");
                return;
            }
            if (String.IsNullOrEmpty(MotivationBox.Text))
            {
                //errore Cognome
                this.ShowModalMessageExternal("Ops", "Insert a valid motivation");
                return;
            }
            if (String.IsNullOrEmpty(DatePick.Text))
            {
                //errore data
                this.ShowModalMessageExternal("Ops", "Insert a valid expiration date");
                return;
            }
            if (String.IsNullOrEmpty(VisitorLevels.Text))
            {
                //errore AuthLevel
                this.ShowModalMessageExternal("Ops", "Insert a valid authentication level");
                return;
            }


            
            myVisitor.Expiration = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DatePick.SelectedDate);
            myVisitor.AuthLevel = VisitorLevels.Text;

            
            VisitorResponseClass vrc =  RestClient.CreateVisitor(myVisitor);
            if (vrc != null)
            {
                QRCode qr = new QRCode(Constants.IPREMOTE + vrc.QrCodeURL);
                qr.ShowDialog();
                App.visitorList.Add(vrc.Visitor);
                BigWindow.VisitorList.Add(vrc.Visitor);
                Close();
            }
            else
            {
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
