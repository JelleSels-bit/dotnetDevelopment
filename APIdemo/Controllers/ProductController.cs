using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIdemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly APIdemoContext _context;

        public ProductController(APIdemoContext context)
        {
            _context = context;
        }

        [HttpGet("Producten ophalen")]
        public async Task<ActionResult<List<Product>>> ProductenOphalen() 
        {
            List<Product> producten = await _context.Producten.ToListAsync();
            if (producten.Count == 0) 
            return NotFound("Er zijn geen producten gevonden");

            return Ok(producten);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> ProductOphalen(int id)
        {
            Product product = await _context.Producten.FirstOrDefaultAsync(x => x.Id == id);  
            if (product == null)
                return NotFound($"Er is geen product gevonden met id {id}");
           
            return Ok(product);

        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Product>>> Search(string zoekwaarde)
        {
            var producten = await _context.Producten.Where(x => x.Naam.Contains(zoekwaarde)).OrderBy(x => x.Naam).ToListAsync();
            if (producten == null)
            {
                return NotFound($"Er zijn geen producten in de database waar {zoekwaarde} voorkomt in de naam");
            }
            return Ok(producten);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> ProductToevoegen(Product product)
        {
            //Validatie
            if (_context.Producten == null)
                return NotFound("De tabel Producten bestaat niet in de database.");
            if (product.Naam == "")
                return BadRequest("Gelieve een naam in te geven");
            if (product.Prijs == 0)
                return BadRequest("Gelieve een prijs in te geven");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Product toevoegen aan de DbSet
            await _context.Producten.AddAsync(product);
            try
            {
                //Product wegschrijven naar de database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ProductWijzigen(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen.");
            }
            //Hier kan nog andere validatie komen.

            _context.Producten.Update(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Producten.AnyAsync(x => x.Id == id))
                {
                    return NotFound("Er is geen product met dit id gevonden");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ProductVerwijderen(int id)
        {
            if (_context.Producten == null)
            {
                return NotFound("De tabel producten bestaat niet.");
            }
            Product product = await _context.Producten.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound("Het product met deze id is niet gevonden.");
            }

            _context.Producten.Remove(product);
            await _context.SaveChangesAsync();

            return Ok($"Product met id {id} is verwijderd");
        }
    }
}
