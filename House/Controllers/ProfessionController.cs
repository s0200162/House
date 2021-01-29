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
    public class ProfessionController : Controller
    {
        private readonly HouseContext _context;

        public ProfessionController(HouseContext context)
        {
            _context = context;
        }

        // GET: Profession
        public async Task<IActionResult> Index()
        {
            List<Profession> professions = await _context.Profession.ToListAsync();
            return View(professions);
        }

        // GET: Profession/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profession = await _context.Profession
                .FirstOrDefaultAsync(m => m.ProfessionID == id);
            if (profession == null)
            {
                return NotFound();
            }

            return View(profession);
        }

        // GET: Profession/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profession/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfessionID,Description")] Profession profession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profession);
        }

        // GET: Profession/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profession = await _context.Profession.FindAsync(id);
            if (profession == null)
            {
                return NotFound();
            }
            return View(profession);
        }

        // POST: Profession/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProfessionID,Description")] Profession profession)
        {
            if (id != profession.ProfessionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessionExists(profession.ProfessionID))
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
            return View(profession);
        }

        // GET: Profession/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profession = await _context.Profession
                .FirstOrDefaultAsync(m => m.ProfessionID == id);
            if (profession == null)
            {
                return NotFound();
            }

            return View(profession);
        }

        // POST: Profession/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profession = await _context.Profession.FindAsync(id);
            _context.Profession.Remove(profession);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessionExists(int id)
        {
            return _context.Profession.Any(e => e.ProfessionID == id);
        }
    }
}
