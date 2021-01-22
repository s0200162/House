using House.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.ViewModels
{
    public class CreateReservationViewModel
    {
        public Reservation Reservation { get; set; }

        public int? SelectedLocation { get; set; }
        public int? SelectedRoom { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList RoomList { get; set; }
    }
}
