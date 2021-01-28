using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace House.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public Customer customer { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public bool Paid { get; set; }
        public double TotalPrice { get; set; }
        public List<ReservationInvoice> ReservationInvoices { get; set; }
    }
}
