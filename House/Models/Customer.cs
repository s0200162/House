using House.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string UserID { get; set; }

        public CustomUser CustomUser { get; set; }

        public override string ToString()
        {
            StringBuilder stringbuilder = new StringBuilder();
            stringbuilder.Append($"CustomerID: {CustomerID}; ");
            stringbuilder.Append($"Firstname: {Firstname}; ");
            stringbuilder.Append($"Lastname: {Lastname}; ");

            return stringbuilder.ToString();
        }
    }
}
