using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using House.Data;
using House.Models;
using House.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace House.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly HouseContext _context;

        public InvoiceController(HouseContext context)
        {
            _context = context;
        }

        // GET: Invoice
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var houseContext = _context.Invoice.Include(i => i.customer);
            return View(await houseContext.ToListAsync());
        }

        // GET: Reservation/Own
        public async Task<IActionResult> Own()
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

            var houseContext = _context.Invoice.Include(r => r.customer)
                .Where(x => x.CustomerID == currentCustomer.CustomerID);
            return View(await houseContext.ToListAsync());
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Invoice invoice = await _context.Invoice
                .Include(x => x.customer)
                .Include(x => x.ReservationInvoices)
                .SingleOrDefaultAsync(x => x.InvoiceID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            List<ReservationInvoice> reservationInvoices = await _context.ReservationInvoice
                .Include(x => x.reservation)
                .ThenInclude(reservation => reservation.period)
                .Include(x => x.reservation)
                .ThenInclude(reservation => reservation.room)
                .Where(x => x.InvoiceID == id).ToListAsync();

            DetailsInvoiceViewModel viewModel = new DetailsInvoiceViewModel();
            viewModel.Invoice = invoice;
            List<Reservation> reservations = new List<Reservation>();

            foreach (ReservationInvoice reservationInvoice in reservationInvoices)
            {
                reservations.Add(reservationInvoice.reservation);
            }

            viewModel.Reservations = new List<Reservation>(reservations);

            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUser.IsInRole("Admin"))
            {
                return View(viewModel);
            }
            else
            {
                List<Customer> customers = _context.Customer.ToList();
                Customer currentCustomer = new Customer();
                foreach (Customer customer in customers)
                {
                    if (customer.UserID == currentUserID)
                    {
                        currentCustomer = customer;
                    }
                }
                if (currentCustomer.CustomerID == invoice.CustomerID)
                {
                    return View(viewModel);
                }
                else
                {
                    return NotFound();
                }
            }

            return View(viewModel);
        }

        // GET: Invoice/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            CreateInvoiceViewModel viewModel = new CreateInvoiceViewModel();

            viewModel.Invoice = new Invoice();
            List<Customer> customers = _context.Customer.ToList();
            viewModel.Customers = new SelectList(customers, "CustomerID", "Fullname");
            viewModel.ReservationList = new SelectList(Enumerable.Empty<SelectListItem>());

            //new SelectList(_context.Reservation, "ReservationID", "Date");
            viewModel.SelectedReservations = new List<int>();
            
            return View(viewModel);
        }

        // GET: Reservation filtered on customerID
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(CreateInvoiceViewModel viewModel)
        {
            int CusID = viewModel.Invoice.CustomerID;
            if (CusID>0)
            {
                List<Reservation> reservations = await _context.Reservation
                    .Include(x => x.room)
                    .Include(x => x.period)
                    .Where(x => x.CustomerID == CusID).ToListAsync();

                viewModel.ReservationList = new SelectList(reservations, "ReservationID", "InvoiceView");

            }
            else
            {
                viewModel.ReservationList = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            viewModel.Invoice = new Invoice();
            List<Customer> customers = _context.Customer.ToList();
            viewModel.Customers = new SelectList(customers, "CustomerID", "Firstname", CusID) ;
            viewModel.SelectedCustomer = CusID;
            viewModel.SelectedReservations = new List<int>();

            return View("Create", viewModel);
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateInvoiceViewModel viewModel)
        {
            viewModel.Invoice = new Invoice();

            viewModel.Invoice.Date = DateTime.Today;
            viewModel.Invoice.EndDate = DateTime.Today.AddDays(14);
            viewModel.Invoice.Paid = false;
            viewModel.Invoice.CustomerID = viewModel.SelectedCustomer ?? -1;

            double Total = 0;

            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Invoice);
                await _context.SaveChangesAsync();

                List<ReservationInvoice> newInvoices = new List<ReservationInvoice>();
                foreach (int reservationID in viewModel.SelectedReservations)
                {
                    Reservation reservation = _context.Reservation.Find(reservationID);
                    Total = Total + reservation.Price;
                    newInvoices.Add(new ReservationInvoice
                    {
                        ReservationID = reservationID,
                        InvoiceID = viewModel.Invoice.InvoiceID
                    });
                }

                viewModel.Invoice.TotalPrice = Total;
                _context.Update(viewModel.Invoice);
                await _context.SaveChangesAsync();

                Invoice invoice = await _context.Invoice.Include(x => x.ReservationInvoices)
                    .SingleOrDefaultAsync(x => x.InvoiceID == viewModel.Invoice.InvoiceID);
                invoice.ReservationInvoices.AddRange(newInvoices);
                _context.Update(invoice);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            //zaken terug opvullen
            return View(viewModel);
        }

        // GET: Invoice/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customer, "CustomerID", "Fullname", invoice.CustomerID);
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceID,CustomerID,Date,EndDate,Paid,TotalPrice")] Invoice invoice)
        {
            if (id != invoice.InvoiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customer, "CustomerID", "Firstname", invoice.CustomerID);
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.customer)
                .FirstOrDefaultAsync(m => m.InvoiceID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Invoice invoice = await _context.Invoice.FindAsync(id);

            List<ReservationInvoice> reservationInvoices = await _context.ReservationInvoice.ToListAsync();

            foreach (ReservationInvoice resin in reservationInvoices)
            {
                if (resin.InvoiceID == id)
                {
                    _context.ReservationInvoice.Remove(resin);
                    await _context.SaveChangesAsync();
                }
            }

            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.InvoiceID == id);
        }
    }
}
