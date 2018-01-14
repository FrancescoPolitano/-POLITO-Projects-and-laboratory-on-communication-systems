using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class VisitorResponseClass
    {
        private Visitor visitor;
        private string qrCodeURL;

        public VisitorResponseClass()
        {
        }

        public VisitorResponseClass(Visitor visitor, string qrCodeURL)
        {
            Visitor = visitor;
            QrCodeURL = qrCodeURL;
        }

        public Visitor Visitor { get => visitor; set => visitor = value; }
        public string QrCodeURL { get => qrCodeURL; set => qrCodeURL = value; }
    }
}
