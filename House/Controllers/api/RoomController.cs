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
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public RoomController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoom()
        {
            return await _uow.RoomRepository.GetAll().ToListAsync();
        }

        // GET: api/Room/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            Room room = await _uow.RoomRepository.GetById(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        // PUT: api/Room/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.RoomID)
            {
                return BadRequest();
            }

            _uow.RoomRepository.Update(room);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {
                //foutmelding loggen, eventueel rollback doen als dit geïmplementeerd moet worden
                //return iets in 400 reeks?
            }

            return NoContent();
        }

        // POST: api/Room
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            _uow.RoomRepository.Create(room);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {
                //foutmelding loggen
                // return Http response in 500 reeks + custom foutmelding
            }

            return CreatedAtAction("GetRoom", new { id = room.RoomID }, room);
        }

        // DELETE: api/Room/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Room>> DeleteRoom(int id)
        {
            Room room = await _uow.RoomRepository.GetById(id);
            if (room == null)
            {
                return NotFound();
            }

            _uow.RoomRepository.Delete(room);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {
                //foutmelding, zie vorige
            }

            return NoContent();
        }
    }
}
