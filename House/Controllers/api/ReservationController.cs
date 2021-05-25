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
    public class ReservationController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly HouseContext _context;

        public ReservationController(IUnitOfWork uow, HouseContext context)
        {
            _uow = uow;
            _context = context;
        }

        // GET: api/Reservation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservation()
        {
            return await _uow.ReservationRepository.GetAll()
                .Include(x => x.room)
                .Include(x => x.customer)
                .Include(x => x.period)
                .ToListAsync();
        }

        // GET: api/Reservation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            //Reservation reservation = await _uow.ReservationRepository.GetById(id);

            //if (reservation == null)
            //{
            //    return NotFound();
            //}

            //return reservation;
            var reservation = await _context.Reservation
                .Include(r => r.customer)
                .Include(r => r.room)
                .Include(r => r.period)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return reservation;
        }

        // PUT: api/Reservation/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return BadRequest();
            }

            _uow.ReservationRepository.Update(reservation);

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

        // POST: api/Reservation
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            _uow.ReservationRepository.Create(reservation);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {

                throw;
            }

            return CreatedAtAction("GetReservation", new { id = reservation.ReservationID }, reservation);
        }

        // DELETE: api/Reservation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reservation>> DeleteReservation(int id)
        {
            Reservation reservation = await _uow.ReservationRepository.GetById(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _uow.ReservationRepository.Delete(reservation);

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
