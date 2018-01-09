using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
   public class Room
    {
        private string name, idLocal,authGrade;

        public string Name { get => name; set => name = value; }
        public string IdLocal { get => idLocal; set => idLocal = value; }
        public string AuthGrade { get => authGrade; set => authGrade = value; }

        public Room(string name, string idRoom, string authGrade)
        {
            Name = name;
            IdLocal = idRoom;
            AuthGrade = authGrade;
        }

        public Room() { }
    }
}
