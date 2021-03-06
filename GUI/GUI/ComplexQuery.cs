﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class ComplexQuery
    {
        private List<string> employees = new List<string>();
        private List<string> rooms = new List<string>();
        private DateTime? initial, end;

        public ComplexQuery() { }

        public ComplexQuery(List<string> users, List<string> rooms, DateTime? initial, DateTime? end)
        {
            Employees = users;
            Rooms = rooms;
            Initial = initial;
            End = end;
        }

        public List<string> Employees { get => employees; set => employees = value; }
        public List<string> Rooms { get => rooms; set => rooms = value; }
        public DateTime? Initial { get => initial; set => initial = value; }
        public DateTime? End { get => end; set => end = value; }
    }
}
