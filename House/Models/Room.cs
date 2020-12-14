using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public int LocationID { get; set; }
        public Location location { get; set; }
        public string Description { get; set; }
        public double PriceHour { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
