using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Adress { get; set; }
        public string NameAndPlace => $"{Name}, {Place}";
        public ICollection<Room> Rooms { get; set; }
    }
}
