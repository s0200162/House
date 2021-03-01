using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using House.Data;
using House.Models;

namespace House.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionController : ControllerBase
    {
        private readonly HouseContext _context;

        public ProfessionController(HouseContext context)
        {
            _context = context;
        }

        // GET: api/Profession
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profession>>> GetProfession()
        {
            return await _context.Profession.ToListAsync();
        }

        // GET: api/Profession/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profession>> GetProfession(int id)
        {
            var profession = await _context.Profession.FindAsync(id);

            if (profession == null)
            {
                return NotFound();
            }

            return profession;
        }

        // PUT: api/Profession/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfession(int id, Profession profession)
        {
            if (id != profession.ProfessionID)
            {
                return BadRequest();
            }

            _context.Entry(profession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfessionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Profession
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Profession>> PostProfession(Profession profession)
        {
            _context.Profession.Add(profession);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfession", new { id = profession.ProfessionID }, profession);
        }

        // DELETE: api/Profession/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Profession>> DeleteProfession(int id)
        {
            var profession = await _context.Profession.FindAsync(id);
            if (profession == null)
            {
                return NotFound();
            }

            _context.Profession.Remove(profession);
            await _context.SaveChangesAsync();

            return profession;
        }

        private bool ProfessionExists(int id)
        {
            return _context.Profession.Any(e => e.ProfessionID == id);
        }
    }
}
