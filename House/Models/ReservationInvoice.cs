using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Models
{
    public class ReservationInvoice
    {
        public int ReservationInvoiceID { get; set; }
        public int ReservationID { get; set; }
        public Reservation reservation { get; set; }
        public int InvoiceID { get; set; }
        public Invoice invoice { get; set; }
    }
}
