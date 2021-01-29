using House.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace House.ViewModels
{
    public class CreateInvoiceViewModel
    {
        public Invoice Invoice { get; set; }
        public SelectList Customers { get; set; }
        public int? SelectedCustomer { get; set; }
        public IEnumerable<SelectListItem> ReservationList { get; set; }

        [Required(ErrorMessage = "Selecteer ten minste één reservatie")]
        [Display(Name = "Reservaties")]
        public IEnumerable<int> SelectedReservations { get; set; }
    }
}
