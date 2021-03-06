﻿using House.Areas.Identity.Data;
using House.Data;
using House.Models;
using House.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly HouseContext _context;

        public ReservationController(HouseContext context)
        {
            _context = context;
        }

        // GET: Reservation
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<Reservation> reservations = await _context.Reservation.Include(r => r.customer).Include(r => r.room).Include(r => r.period).ToListAsync();
            return View(reservations);
        }

        // GET: Reservation/Own
        public async Task<IActionResult> Own()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Customer> customers = await _context.Customer.ToListAsync();
            Customer currentCustomer = new Customer();
            foreach (Customer customer in customers)
            {
                if (customer.UserID == currentUserID)
                {
                    currentCustomer = customer;
                }
            }

            var houseContext = _context.Reservation.Include(r => r.customer).Include(r => r.room).Include(r => r.period)
                .Where(x => x.CustomerID == currentCustomer.CustomerID);
            return View(await houseContext.ToListAsync());
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("CustomNotFound");
            }

            var reservation = await _context.Reservation
                .Include(r => r.customer)
                .Include(r => r.room)
                .Include(r => r.period)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return View("CustomNotFound");
            }

            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUser.IsInRole("Admin"))
            {
                return View(reservation);
            }
            else
            {
                List<Customer> customers = await _context.Customer.ToListAsync();
                Customer currentCustomer = new Customer();
                foreach (Customer customer in customers)
                {
                    if (customer.UserID == currentUserID)
                    {
                        currentCustomer = customer;
                    }
                }
                if (currentCustomer.CustomerID == reservation.CustomerID)
                {
                    return View(reservation);
                }
                else
                {
                    return View("CustomNotFound");
                }
            }
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
            viewModel.Reservation = new Reservation();

            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Customer> customers = await _context.Customer.ToListAsync();
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
            viewModel.Reservation.Date = viewModel.SelectedDate ?? default;
            viewModel.Reservation.PeriodID = viewModel.SelectedPeriod ?? -1;

            Room room = _context.Room.Find(viewModel.SelectedRoom);

            if (ModelState.IsValid)
            {
                viewModel.Reservation.Price = room.PriceHour * 2;
                _context.Add(viewModel.Reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Own));
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
        public JsonResult FetchPeriods(int ID, DateTime Date, CreateReservationViewModel viewModel)
        {
            var reservations = _context.Reservation.ToList()
                .Where(x => x.Date.Date == Date.Date)
                .Where(x => x.RoomID == ID);

            List<Period> periods = _context.Period.ToList();

            foreach (Reservation reservation in reservations)
            {
                Period period = reservation.period;
                periods.Remove(period);
            }

            var data = periods
                .Select(x => 
                new { Value = x.PeriodID, Text = x.Hour });
            return Json(data);
        }

        private void ConfigureViewModel (CreateReservationViewModel viewModel)
        {
            viewModel.Reservation = new Reservation();
            viewModel.SelectedDate = DateTime.Now;
            List<Location> locations = _context.Location.ToList();
            viewModel.LocationList = new SelectList(locations, "LocationID", "NameAndPlace");
            if (viewModel.SelectedLocation.HasValue)
            {
                IEnumerable<Room> rooms = _context.Room.ToList()
                    .Where(x => x.LocationID == viewModel.SelectedLocation.Value);
                viewModel.RoomList = new SelectList(rooms, "RoomID", "Description");
            } else
            {
                viewModel.RoomList = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            if (viewModel.SelectedRoom.HasValue && viewModel.SelectedDate.HasValue)
            {
                IEnumerable<Period> hours = _context.Period.ToList();
                viewModel.PeriodList = new SelectList(hours, "HourID", "Period");
            }
            else
            {
                viewModel.PeriodList = new SelectList(Enumerable.Empty<SelectListItem>());
            }

        }


        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id, EditReservationViewModel viewModel)
        {
            if (id == null)
            {
                return View("CustomNotFound");
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return View("CustomNotFound");
            }

            viewModel.Reservation = reservation;

            //Location location = await _context.Location.FindAsync(reservation.room.LocationID);

            Room room = await _context.Room.FindAsync(reservation.RoomID);

            viewModel.SelectedLocation = room.LocationID;

            viewModel.SelectedDate = reservation.Date;
            viewModel.SelectedRoom = reservation.RoomID;
            viewModel.SelectedPeriod = reservation.PeriodID;
            List<Location> locations = await _context.Location.ToListAsync();
            viewModel.LocationList = new SelectList(locations, "LocationID", "NameAndPlace", room.LocationID);
            if (viewModel.SelectedLocation.HasValue)
            {
                IEnumerable<Room> rooms = _context.Room.ToList()
                    .Where(x => x.LocationID == viewModel.SelectedLocation.Value);
                viewModel.RoomList = new SelectList(rooms, "RoomID", "Description", reservation.RoomID);
            }
            else
            {
                viewModel.RoomList = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            if (viewModel.SelectedRoom.HasValue && viewModel.SelectedDate.HasValue)
            {
                IEnumerable<Period> hours = await _context.Period.ToListAsync();
                viewModel.PeriodList = new SelectList(hours, "PeriodID", "Hour", reservation.PeriodID);
            }
            else
            {
                viewModel.PeriodList = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            
            return View(viewModel);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditReservationViewModel viewModel)
        {
            if (id != viewModel.Reservation.ReservationID)
            {
                return View("CustomNotFound");
            }

            if (ModelState.IsValid)
            {
                _context.Update(viewModel.Reservation);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(viewModel);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("CustomNotFound");
            }

            var reservation = await _context.Reservation
                .Include(r => r.customer)
                .Include(r => r.room)
                .Include(r => r.period)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return View("CustomNotFound");
            }

            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUser.IsInRole("Admin"))
            {
                return View(reservation);
            }
            else
            {
                List<Customer> customers = await _context.Customer.ToListAsync();
                Customer currentCustomer = new Customer();
                foreach (Customer customer in customers)
                {
                    if (customer.UserID == currentUserID)
                    {
                        currentCustomer = customer;
                    }
                }
                if (currentCustomer.CustomerID == reservation.CustomerID)
                {
                    return View(reservation);
                }
                else
                {
                    return View("CustomNotFound");
                }
            }

        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var reservation = await _context.Reservation.FindAsync(id);
                _context.Reservation.Remove(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Own));
            }
            catch (Exception)
            {

                return RedirectToAction(nameof(FailedDelete));
            }

        }

        // GET: Reservation/FailedDelete
        public IActionResult FailedDelete()
        {
            return View();
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ReservationID == id);
        }
    }
}
