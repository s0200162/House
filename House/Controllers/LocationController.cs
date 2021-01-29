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

namespace House.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {
        private readonly HouseContext _context;

        public LocationController(HouseContext context)
        {
            _context = context;
        }

        // GET: Location
        public async Task<IActionResult> Index()
        {
            List<Location> locations = await _context.Location.ToListAsync();
            return View(locations);
        }

        // GET: Location/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("CustomNotFound");
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.LocationID == id);
            if (location == null)
            {
                return View("CustomNotFound");
            }

            return View(location);
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationID,Name,Place,Adress")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Location/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("CustomNotFound");
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return View("CustomNotFound");
            }
            return View(location);
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationID,Name,Place,Adress")] Location location)
        {
            if (id != location.LocationID)
            {
                return View("~/Home/CustomNotFound");
            }

            if (ModelState.IsValid)
            {
                _context.Update(location);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Location/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("CustomNotFound");
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.LocationID == id);
            if (location == null)
            {
                return View("CustomNotFound");
            }

            return View(location);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Location.FindAsync(id);
            _context.Location.Remove(location);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Location.Any(e => e.LocationID == id);
        }
    }
}
