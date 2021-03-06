﻿using System;
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
    public class CustomerController : Controller
    {
        private readonly HouseContext _context;

        public CustomerController(HouseContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            List<Customer> customers = await _context.Customer.Include(c => c.CustomUser).Include(c => c.Profession).ToListAsync();
            return View(customers);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("CustomNotFound");
            }

            var customer = await _context.Customer
                .Include(c => c.CustomUser)
                .Include(c => c.Profession)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return View("CustomNotFound");
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProfessionID"] = new SelectList(_context.Profession, "ProfessionID", "Description");
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,Firstname,Lastname,ProfessionID,UserID")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", customer.UserID);
            ViewData["ProfessionID"] = new SelectList(_context.Profession, "ProfessionID", "Description", customer.ProfessionID);
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("CustomNotFound");
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return View("CustomNotFound");
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", customer.UserID);
            ViewData["ProfessionID"] = new SelectList(_context.Profession, "ProfessionID", "Description", customer.ProfessionID);
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,Firstname,Lastname,ProfessionID,UserID")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return View("CustomNotFound");
            }

            if (ModelState.IsValid)
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", customer.UserID);
            ViewData["ProfessionID"] = new SelectList(_context.Profession, "ProfessionID", "Description", customer.ProfessionID);
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("CustomNotFound");
            }

            var customer = await _context.Customer
                .Include(c => c.CustomUser)
                .Include(c => c.Profession)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return View("CustomNotFound");
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerID == id);
        }
    }
}
