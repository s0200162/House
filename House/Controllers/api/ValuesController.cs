using House.Data;
using House.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly HouseContext _context;

        public ValuesController(HouseContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetValues(int id)
        {
            return await _context.Room.Where(x => x.LocationID == id).ToListAsync();
        }

        [HttpGet("{id}/{date}")]
        public async Task<ActionResult<IEnumerable<Period>>> GetValues(int id, DateTime date)
        {
            var reservations = _context.Reservation.ToList()
                .Where(x => x.Date.Date == date.Date)
                .Where(x => x.RoomID == id);

            List<Period> periods = _context.Period.ToList();

            foreach (Reservation reservation in reservations)
            {
                Period period = reservation.period;
                periods.Remove(period);
            }

            return periods;
        }

        //[HttpGet]
        //public JsonResult FetchPeriods(int ID, DateTime Date, CreateReservationViewModel viewModel)
        //{
        //    var reservations = _context.Reservation.ToList()
        //        .Where(x => x.Date.Date == Date.Date)
        //        .Where(x => x.RoomID == ID);

        //    List<Period> periods = _context.Period.ToList();

        //    foreach (Reservation reservation in reservations)
        //    {
        //        Period period = reservation.period;
        //        periods.Remove(period);
        //    }

        //    var data = periods
        //        .Select(x =>
        //        new { Value = x.PeriodID, Text = x.Hour });
        //    return Json(data);
        //}
    }
}
