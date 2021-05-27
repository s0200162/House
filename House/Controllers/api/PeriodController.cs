using House.Data;
using House.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodController : ControllerBase
    {
        private readonly HouseContext _context;

        public PeriodController(HouseContext context)
        {
            _context = context;
        }

        // GET: api/Period
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Period>>> GetPeriod()
        {
            List<Period> periods = _context.Period.ToList();
            return periods;
        }
    }
}
