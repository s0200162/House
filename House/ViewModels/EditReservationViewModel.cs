using House.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.ViewModels
{
    public class EditReservationViewModel
    {
        public Reservation Reservation { get; set; }

        public int? SelectedLocation { get; set; }
        public int? SelectedRoom { get; set; }
        public DateTime? SelectedDate { get; set; }
        public int? SelectedPeriod { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList RoomList { get; set; }
        public SelectList PeriodList { get; set; }
    }
}
