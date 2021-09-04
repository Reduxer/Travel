using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Data.Contexts;
using Travel.Domain.Entities;

namespace Travel.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourPackagesController : ControllerBase
    {
        private readonly TravelDbContext _dbContext;

        public TourPackagesController(TravelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TourPackage>>> Get()
        {
            return await _dbContext.TourPackages.ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TourPackage>> GetById(int id)
        {
            var tp = await _dbContext.TourPackages.FirstOrDefaultAsync(tp => tp.Id == id);

            if(tp is null)
            {
                return NotFound();
            }

            return tp;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TourPackage tourPackage)
        {
            await _dbContext.TourPackages.AddAsync(tourPackage);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = tourPackage.Id }, tourPackage);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TourPackage>> Delete([FromRoute] int id)
        {
            var tp = _dbContext.TourPackages.SingleOrDefault(tp => tp.Id == id);

            if(tp is null)
            {
                return NotFound();
            }

            _dbContext.TourPackages.Remove(tp);
            await _dbContext.SaveChangesAsync();

            return tp;
        }
    }
}
