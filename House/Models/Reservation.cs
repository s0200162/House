using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int CustomerID { get; set; }
        public Customer customer { get; set; }
        public int RoomID { get; set; }
        public Room room { get; set; }
        public DateTime Date { get; set; }
        public int PeriodID { get; set; }
        public Period period { get; set; }
        public double Price { get; set; }
    }
}
