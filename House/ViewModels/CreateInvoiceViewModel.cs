using House.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.ViewModels
{
    public class CreateInvoiceViewModel
    {
        public Invoice Invoice { get; set; }
        public SelectList Customers { get; set; }
        public int? SelectedCustomer { get; set; }
        public IEnumerable<SelectListItem> ReservationList { get; set; }
        public IEnumerable<int> SelectedReservations { get; set; }
    }
}
