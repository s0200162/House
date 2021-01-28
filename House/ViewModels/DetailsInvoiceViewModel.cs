using House.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.ViewModels
{
    public class DetailsInvoiceViewModel
    {
        public Invoice Invoice { get; set; }
        public IEnumerable<SelectListItem> ReservationList { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
