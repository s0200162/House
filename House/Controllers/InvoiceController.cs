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

namespace House.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly HouseContext _context;

        public InvoiceController(HouseContext context)
        {
            _context = context;
        }

        // GET: Invoice
        public async Task<IActionResult> Index()
        {
            var houseContext = _context.Invoice.Include(i => i.customer);
            return View(await houseContext.ToListAsync());
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Invoice/Create
        public IActionResult Create()
        {
            CreateInvoiceViewModel viewModel = new CreateInvoiceViewModel();

            viewModel.Invoice = new Invoice();
            List<Customer> customers = _context.Customer.ToList();
            viewModel.Customers = new SelectList(customers, "CustomerID", "Firstname");
            viewModel.ReservationList = new SelectList(Enumerable.Empty<SelectListItem>());

            //new SelectList(_context.Reservation, "ReservationID", "Date");
            viewModel.SelectedReservations = new List<int>();
            
            return View(viewModel);
        }

        public JsonResult FetchReservations(int ID)
        {
            var data = _context.Reservation.ToList()
                .Where(x => x.CustomerID == ID)
                .Select(x => new { Value = x.ReservationID, Text = x.Date });
            return Json(data);
        }

        // GET: Reservation filtered on customerID
        public async Task<IActionResult> Search(CreateInvoiceViewModel viewModel)
        {
            int CusID = viewModel.Invoice.CustomerID;
            if (CusID>0)
            {
                List<Reservation> reservations = await _context.Reservation
                    .Where(x => x.CustomerID == CusID).ToListAsync();

                viewModel.ReservationList = new SelectList(reservations, "ReservationID", "Date");

            }
            else
            {
                viewModel.ReservationList = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            viewModel.Invoice = new Invoice();
            List<Customer> customers = _context.Customer.ToList();
            viewModel.Customers = new SelectList(customers, "CustomerID", "Firstname", CusID) ;
            viewModel.SelectedReservations = new List<int>();

            return View("Create", viewModel);
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvoiceViewModel viewModel)
        {
            viewModel.Invoice = new Invoice();

            viewModel.Invoice.Date = DateTime.Today;
            viewModel.Invoice.EndDate = DateTime.Today.AddDays(14);
            viewModel.Invoice.Paid = false;

            //customerID toevoegen, wordt niet meer meegegeven?

            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Invoice);
                await _context.SaveChangesAsync();

                List<ReservationInvoice> newInvoices = new List<ReservationInvoice>();
                foreach (int reservationID in viewModel.SelectedReservations)
                {
                    newInvoices.Add(new ReservationInvoice
                    {
                        ReservationID = reservationID,
                        InvoiceID = viewModel.Invoice.InvoiceID
                    });
                }



                return RedirectToAction(nameof(Index));
            }
            //zaken terug opvullen
            return View(viewModel);
        }

        // GET: Invoice/Edit/5
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
            ViewData["CustomerID"] = new SelectList(_context.Customer, "CustomerID", "Firstname", invoice.CustomerID);
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceID,CustomerID,Date,EndDate,Paid")] Invoice invoice)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
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
