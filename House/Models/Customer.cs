using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int ProfessionID { get; set; }
        public Profession profession { get; set; }
        public bool Admin { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
