using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public Customer customer { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public bool Paid { get; set; }
        public List<ReservationInvoice> ReservationInvoices { get; set; }
    }
}
