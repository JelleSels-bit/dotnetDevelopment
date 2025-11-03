using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterimkantoorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlantController : ControllerBase
    {
        private readonly APIinterimkantoorContext _context;

        public KlantController(APIinterimkantoorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Klant>> GetKlant(string id)
        {
            Klant klant = await _context.Klanten.FirstOrDefaultAsync(x => x.Id == id);
            if (klant == null)
                return NotFound($"Er is geen product gevonden met id {id}");

            return Ok(klant);

        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Klant>>> Search(string zoekwaarde)
        {
            var klanten = await _context.Klanten.Where(x => x.Naam.Contains(zoekwaarde)).OrderBy(x => x.Naam).ToListAsync();
            if (klanten == null)
            {
                return NotFound($"Er zijn geen producten in de database waar {zoekwaarde} voorkomt in de naam");
            }
            return Ok(klanten);
        }

        [HttpPost]
        public async Task<ActionResult<Klant>> KlantToevoegen(Klant klant)
        {
            //Validatie
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.Klanten.Any(k => k.Id == klant.Id))
                return BadRequest("Klant bestaat al");
            //Product toevoegen aan de DbSet
            await _context.Klanten.AddAsync(klant);
            try
            {
                //Product wegschrijven naar de database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }
            return CreatedAtAction("GetKlant", new { id = klant.Id }, klant);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> KlantWijzigen(string id, Klant klant)
        {
            if (id != klant.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen.");
            }
            //Hier kan nog andere validatie komen.

            _context.Klanten.Update(klant);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Klanten.AnyAsync(x => x.Id == id))
                {
                    return NotFound("Er is geen Klant met dit id gevonden");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> KlantVerwijderen(string id)
        {
            if (_context.Klanten == null)
            {
                return NotFound("De tabel producten bestaat niet.");
            }
            Klant Klant = await _context.Klanten.FirstOrDefaultAsync(x => x.Id == id);
            if (Klant == null)
            {
                return NotFound("Het product met deze id is niet gevonden.");
            }

            _context.Klanten.Remove(Klant);
            await _context.SaveChangesAsync();

            return Ok($"Product met id {id} is verwijderd");
        }


      
    }
}
