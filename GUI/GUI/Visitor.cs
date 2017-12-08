using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class Visitor
    {
        string name, surname, motivation, expiryDate;

        public Visitor(string name, string surname, string motivation, string expiryDate)
        {
            Name = name;
            Surname = surname;
            Motivation = motivation;
            ExpiryDate = expiryDate;
        }

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Motivation { get => motivation; set => motivation = value; }
        public string ExpiryDate { get => expiryDate; set => expiryDate = value; }
    }
}
