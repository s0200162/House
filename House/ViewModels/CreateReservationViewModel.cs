using House.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace House.ViewModels
{
    public class CreateReservationViewModel
    {
        public Reservation Reservation { get; set; }

        [Required(ErrorMessage = "Selecteer een locatie")]
        [Display(Name = "Locatie")]
        public int? SelectedLocation { get; set; }

        [Required(ErrorMessage = "Selecteer een kamer")]
        [Display(Name = "Kamer")]
        public int? SelectedRoom { get; set; }

        [Required(ErrorMessage = "Selecteer een Datum")]
        [Display(Name = "Datum")]
        public DateTime? SelectedDate { get; set; }

        [Required(ErrorMessage = "Selecteer een periode")]
        [Display(Name = "Periode")]
        public int? SelectedPeriod { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList RoomList { get; set; }
        public SelectList PeriodList { get; set; }
    }
}
