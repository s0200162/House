using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using House.Data;
using House.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using House.Data.Repository;
using House.Data.UnitOfWork;

namespace House.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public LocationController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Location
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocation()
        {
            return await _uow.LocationRepository.GetAll().Include(x => x.Rooms).ToListAsync();
        }

        // GET: api/Location/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            Location location = await _uow.LocationRepository.GetById(id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        // PUT: api/Location/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.LocationID)
            {
                return BadRequest();
            }

            _uow.LocationRepository.Update(location);

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

        // POST: api/Location
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            _uow.LocationRepository.Create(location);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {
                //foutmelding loggen
                // return Http response in 500 reeks + custom foutmelding
            }
            
            return CreatedAtAction("GetLocation", new { id = location.LocationID }, location);
        }

        // DELETE: api/Location/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Location>> DeleteLocation(int id)
        {
            Location location = await _uow.LocationRepository.GetById(id);
            if (location == null)
            {
                return NotFound();
            }

            _uow.LocationRepository.Delete(location);

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
