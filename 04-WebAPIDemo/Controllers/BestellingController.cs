using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Data;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestellingController : ControllerBase
    {
        private readonly WebAPIDemoContext _context;

        public BestellingController(WebAPIDemoContext context)
        {
            _context = context;
        }

        // GET: api/Bestelling
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bestelling>>> AlleBestellingenOphalen()
        {
            return await _context.Bestellingen.Include(x=>x.Klant).Include(x=>x.Orderlijnen).ThenInclude(x=>x.Product).ToListAsync();
        }

        // GET: api/Bestelling/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bestelling>> BestellingOphalen(int id)
        {
            var bestelling = await _context.Bestellingen.FindAsync(id);

            if (bestelling == null)
            {
                return NotFound();
            }

            return bestelling;
        }

        // PUT: api/Bestelling/5
        [HttpPut("{id}")]
        public async Task<IActionResult> BestellingWijzigen(int id, Bestelling bestelling)
        {
            if (id != bestelling.Id)
            {
                return BadRequest("De id's van de bestelling die je wil wijzigen komen niet overeen.");
            }
            var existingBestelling = await _context.Bestellingen
                .Include(b => b.Orderlijnen)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existingBestelling == null)
            {
                return NotFound("De bestelling die je wil wijzigen, komt niet voor in de database.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existingBestelling.KlantId = bestelling.KlantId;

            foreach (var updatedOrderlijn in bestelling.Orderlijnen)
            {
                var existingOrderlijn = existingBestelling.Orderlijnen.FirstOrDefault(ol => ol.Id == updatedOrderlijn.Id);

                if (existingOrderlijn != null)
                {
                    existingOrderlijn.Aantal = updatedOrderlijn.Aantal;
                    existingOrderlijn.ProductId = updatedOrderlijn.ProductId;
                }
                else
                {
                    existingBestelling.Orderlijnen.Add(new Orderlijn
                    {
                        Aantal = updatedOrderlijn.Aantal,
                        BestellingId = existingBestelling.Id, 
                        ProductId = updatedOrderlijn.ProductId

                    });
                }
            }
            var orderlijnenToRemove = existingBestelling.Orderlijnen.Where(ol => !bestelling.Orderlijnen.Any(uol => uol.Id == ol.Id)).ToList();

            _context.Orderlijnen.RemoveRange(orderlijnenToRemove);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BestellingExists(id))
                {
                    return NotFound("Er is een probleem opgetreden bij het opslaan in de database.");
                }
                else
                {
                    throw;
                }
            }

            return Ok($"De bestelling is gewijzigd.");
        }

            // POST: api/Bestelling
        [HttpPost]
        public async Task<ActionResult<Bestelling>> BestellingToevoegen(Bestelling bestelling)
        {
            if (_context.Bestellingen == null)
                return NotFound("De tabel Bestellingen bestaat niet in de database.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Bestellingen.Add(bestelling);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }

            return CreatedAtAction("BestellingOphalen", new { id = bestelling.Id }, bestelling);
        }

        // DELETE: api/Bestelling/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> BestellingVerwijderen(int id)
        {
             var bestelling = await _context.Bestellingen
                    .Include(b => b.Orderlijnen)
                    .FirstOrDefaultAsync(b => b.Id == id);

            if (bestelling == null)
            {
                return NotFound("De bestelling werd niet gevonden.");
            }

            foreach (var orderlijn in bestelling.Orderlijnen)
            {
                _context.Orderlijnen.Remove(orderlijn);
            }

            _context.Bestellingen.Remove(bestelling);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log de fout of geef een aangepaste foutmelding terug
                return StatusCode(StatusCodes.Status500InternalServerError, "Er is een fout opgetreden bij het verwijderen van de bestelling.");
            }

            return Ok($"De bestelling met id {id} is verwijderd.");
        }

        private bool BestellingExists(int id)
        {
            return _context.Bestellingen.Any(e => e.Id == id);
        }
    }
}
