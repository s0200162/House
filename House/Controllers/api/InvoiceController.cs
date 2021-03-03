using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using House.Data;
using House.Models;
using House.Data.UnitOfWork;

namespace House.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public InvoiceController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Invoice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoice()
        {
            return await _uow.InvoiceRepository.GetAll().ToListAsync();
        }

        // GET: api/Invoice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            Invoice invoice = await _uow.InvoiceRepository.GetById(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/Invoice/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.InvoiceID)
            {
                return BadRequest();
            }

            _uow.InvoiceRepository.Update(invoice);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {
                //
            }

            return NoContent();
        }

        // POST: api/Invoice
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            _uow.InvoiceRepository.Create(invoice);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {

                throw;
            }

            return CreatedAtAction("GetInvoice", new { id = invoice.InvoiceID }, invoice);
        }

        // DELETE: api/Invoice/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Invoice>> DeleteInvoice(int id)
        {
            Invoice invoice = await _uow.InvoiceRepository.GetById(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _uow.InvoiceRepository.Delete(invoice);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {

                throw;
            }

            return NoContent();
        }
    }
}
