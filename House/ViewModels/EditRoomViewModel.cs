using House.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.ViewModels
{
    public class EditRoomViewModel
    {
        public Room Room { get; set; }
        public SelectList Locations { get; set; }
    }
}
