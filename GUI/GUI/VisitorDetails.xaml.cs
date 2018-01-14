
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
    /// Interaction logic for VisitorDetails.xaml
    /// </summary>
    public partial class VisitorDetails : MetroWindow
    {
        private Visitor myVisitor = new Visitor();

        public VisitorDetails(Visitor visitor)
        {
            InitializeComponent();
            MyVisitor = visitor;
            MyGrid.DataContext = MyVisitor;

        }

        public Visitor MyVisitor { get => myVisitor; set => myVisitor = value; }
    }
}
