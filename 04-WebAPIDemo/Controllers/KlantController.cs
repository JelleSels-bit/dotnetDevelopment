using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Data;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlantController : ControllerBase
    {
        private readonly WebAPIDemoContext _context;
        private readonly IMapper _mapper;

        public KlantController(WebAPIDemoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Klant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetKlantBestellingDTO>>> GetKlantenMetBestellingen()
        {
            var klanten = await _context.Klanten.Include(x => x.Bestellingen).ThenInclude(x => x.Orderlijnen).ThenInclude(x => x.Product).ToListAsync();

            if (klanten == null)
            {
                return NotFound("Er zijn geen klanten gevonden");
            }
            List<GetKlantBestellingDTO> klantenDTO = _mapper.Map<List<GetKlantBestellingDTO>>(klanten);
            return klantenDTO;
        }

        // GET: api/Klant/5
        [HttpGet("KlantMetBestelling{id}")]
        public async Task<ActionResult<GetKlantBestellingDTO>> GetKlant(int id)
        {
            var klant = await _context.Klanten
                .Include(x => x.Bestellingen)
                .ThenInclude(x => x.Orderlijnen)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (klant == null)
            {
                return NotFound($"Er is geen klant gevonen met id: {id}");
            }

            GetKlantBestellingDTO klantDTO = _mapper.Map<GetKlantBestellingDTO>(klant);

            return klantDTO;
        }

        // PUT: api/Klant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKlant(int id, Klant klant)
        {
            if (id != klant.Id)
            {
                return BadRequest();
            }

            _context.Entry(klant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KlantExists(id))
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

        // POST: api/Klant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Klant>> PostKlant(Klant klant)
        {
            _context.Klanten.Add(klant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKlant", new { id = klant.Id }, klant);
        }

        // DELETE: api/Klant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKlant(int id)
        {
            var klant = await _context.Klanten.FindAsync(id);
            if (klant == null)
            {
                return NotFound();
            }

            _context.Klanten.Remove(klant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KlantExists(int id)
        {
            return _context.Klanten.Any(e => e.Id == id);
        }
    }
}
