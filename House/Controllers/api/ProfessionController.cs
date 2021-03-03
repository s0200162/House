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
    public class ProfessionController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ProfessionController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Profession
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profession>>> GetProfession()
        {
            return await _uow.ProfessionRepository.GetAll().ToListAsync();
        }

        // GET: api/Profession/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profession>> GetProfession(int id)
        {
            Profession profession = await _uow.ProfessionRepository.GetById(id);

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

            _uow.ProfessionRepository.Update(profession);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {
                //nog één en dezelfde exception maken voor alle API-controllers
            }

            return NoContent();
        }

        // POST: api/Profession
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Profession>> PostProfession(Profession profession)
        {
            _uow.ProfessionRepository.Create(profession);

            try
            {
                await _uow.SaveAsync();
            }
            catch (Exception exception)
            {

                throw;
            }

            return CreatedAtAction("GetProfession", new { id = profession.ProfessionID }, profession);
        }

        // DELETE: api/Profession/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Profession>> DeleteProfession(int id)
        {
            Profession profession = await _uow.ProfessionRepository.GetById(id);
            if (profession == null)
            {
                return NotFound();
            }

            _uow.ProfessionRepository.Delete(profession);

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
