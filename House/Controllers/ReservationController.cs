using House.Areas.Identity.Data;
using House.Data;
using House.Models;
using House.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace House.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HouseContext _context;

        public ReservationController(HouseContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            var houseContext = _context.Reservation.Include(r => r.customer).Include(r => r.room).Include(r => r.period);
            return View(await houseContext.ToListAsync());
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.customer)
                .Include(r => r.room)
                .Include(r => r.period)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        public IActionResult Create()
        {
            CreateReservationViewModel model = new CreateReservationViewModel();
            ConfigureViewModel(model);
            return View(model);
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel viewModel)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Customer> customers = _context.Customer.ToList();
            Customer currentCustomer = new Customer();
            foreach (Customer customer in customers)
            {
                if (customer.UserID == currentUserID)
                {
                    currentCustomer = customer;
                }
            }

            viewModel.Reservation.CustomerID = currentCustomer.CustomerID;
            viewModel.Reservation.RoomID = viewModel.SelectedRoom ?? -1;
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            viewModel.LocationList = new SelectList(_context.Location, "LocationID", "NameAndPlace");
            viewModel.RoomList = new SelectList(_context.Room, "RoomID", "Description");
            viewModel.PeriodList = new SelectList(_context.Period, "PeriodID", "Hour");
            return View(viewModel);
        }

        [HttpGet]
        public JsonResult FetchRooms(int ID)
        {
            var data = _context.Room.ToList()
                .Where(x => x.LocationID == ID)
                .Select(x => new { Value = x.RoomID, Text = x.Description });
            return Json(data);
        }

        [HttpGet]
        public JsonResult FetchPeriods(int ID, DateTime Date)
        {
            List<Reservation> reservations = new List<Reservation>();
            reservations = (List<Reservation>)_context.Reservation.ToList();
                //.Where(x => x.Date == Date)
                //.Where(x => x.RoomID == ID);

            List<Period> periods = _context.Period.ToList();

            foreach (Reservation reservation in reservations)
            {
                Period period = reservation.period;
                periods.Remove(period);
            }

            var data = periods
                .Select(x => new { Value = x.PeriodID, Text = x.Hour });
            return Json(data);
        }

        private void ConfigureViewModel (CreateReservationViewModel model)
        {
            model.Reservation = new Reservation();
            model.SelectedDate = DateTime.Now;
            List<Location> locations = _context.Location.ToList();
            model.LocationList = new SelectList(locations, "LocationID", "NameAndPlace");
            if (model.SelectedLocation.HasValue)
            {
                IEnumerable<Room> rooms = _context.Room.ToList()
                    .Where(x => x.LocationID == model.SelectedLocation.Value);
                model.RoomList = new SelectList(rooms, "RoomID", "Description");
            } else
            {
                model.RoomList = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            if (model.SelectedRoom.HasValue && model.SelectedDate.HasValue)
            {
                List<Period> hours = _context.Period.ToList();
                model.PeriodList = new SelectList(hours, "HourID", "Period");
            }
            else
            {
                model.PeriodList = new SelectList(Enumerable.Empty<SelectListItem>());
            }

        }


        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customer, "CustomerID", "Firstname", reservation.CustomerID);
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "Description", reservation.RoomID);
            ViewData["PeriodID"] = new SelectList(_context.Period, "PeriodID", "Hour", reservation.PeriodID);
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,CustomerID,RoomID,Date,PeriodID,Price")] Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customer, "CustomerID", "Firstname", reservation.CustomerID);
            ViewData["RoomID"] = new SelectList(_context.Room, "RoomID", "Description", reservation.RoomID);
            ViewData["PeriodID"] = new SelectList(_context.Period, "PeriodID", "Hour", reservation.PeriodID);
            return View(reservation);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.customer)
                .Include(r => r.room)
                .Include(r => r.period)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ReservationID == id);
        }
    }
}
