using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int PeriodID { get; set; }
        public Period period { get; set; }
        public double Price { get; set; }
        public List<ReservationInvoice> ReservationInvoices { get; set; }
        public String InvoiceView => $"{Date.ToString("dd/MM/yyyy")}, {period.Hour}, {room.Description}, {Price}";
        public override string ToString()
        {
            return Date.ToString("dd/MM/yyyy") + ", " + period.Hour + ", " + room.Description + ", " + Price + "euro" ;
        }
    }
}
