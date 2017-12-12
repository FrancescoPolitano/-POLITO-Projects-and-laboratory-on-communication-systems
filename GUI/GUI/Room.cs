using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
   public class Room
    {
        private string name, idRoom,authGrade;

        public string Name { get => name; set => name = value; }
        public string IdRoom { get => idRoom; set => idRoom = value; }
        public string AuthGrade { get => authGrade; set => authGrade = value; }

        public Room(string name, string idRoom, string authGrade)
        {
            Name = name;
            IdRoom = idRoom;
            AuthGrade = authGrade;
        }

        public Room() { }
    }
}
