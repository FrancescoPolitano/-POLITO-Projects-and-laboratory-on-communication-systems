using MahApps.Metro.Controls;
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
    /// Interaction logic for QRCode.xaml
    /// </summary>
    public partial class QRCode : MetroWindow
    {
        private string path = String.Empty;

        public string Path { get => path; set => path = value; }

        public QRCode(string path)
        {
            InitializeComponent();
            //TODO capire se funziona uguale senza la variabile qua
            Path = path;
            myGrid.DataContext = Path;
        }

        private void Stampa_Click(object sender, RoutedEventArgs e)
        {
            this.ShowModalMessageExternal("Printing", "Your code is being printed");
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
