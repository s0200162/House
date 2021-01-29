using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using House.Data;
using House.Models;
using Microsoft.AspNetCore.Authorization;
using House.ViewModels;

namespace House.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomController : Controller
    {
        private readonly HouseContext _context;

        public RoomController(HouseContext context)
        {
            _context = context;
        }

        // GET: Room
        public async Task<IActionResult> Index()
        {
            List<Room> rooms = await _context.Room.Include(r => r.location).ToListAsync();
            return View(rooms);
        }

        // GET: Room/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.location)
                .FirstOrDefaultAsync(m => m.RoomID == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Room/Create
        public IActionResult Create()
        {
            CreateRoomViewModel viewModel = new CreateRoomViewModel();
            viewModel.Room = new Room();
            viewModel.Locations = new SelectList(_context.Location, "LocationID", "NameAndPlace");
            
            return View(viewModel);
        }

        // POST: Room/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            viewModel.Locations = new SelectList(_context.Location, "LocationID", "NameAndPlace", viewModel.Room.LocationID);
            return View(viewModel);
        }

        // GET: Room/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EditRoomViewModel viewModel = new EditRoomViewModel();
            viewModel.Room = await _context.Room.FindAsync(id);
            if (viewModel.Room == null)
            {
                return NotFound();
            }
            viewModel.Locations = new SelectList(_context.Location, "LocationID", "NameAndPlace", viewModel.Room.LocationID);
            return View(viewModel);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditRoomViewModel viewModel)
        {
            if (id != viewModel.Room.RoomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(viewModel.Room);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            viewModel.Locations = new SelectList(_context.Location, "LocationID", "NameAndPlace", viewModel.Room.LocationID);
            return View(viewModel);
        }

        // GET: Room/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.location)
                .FirstOrDefaultAsync(m => m.RoomID == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.FindAsync(id);
            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.RoomID == id);
        }
    }
}
