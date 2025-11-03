
using InterimkantoorAPI.Data.UnitOfWork;

namespace InterimkantoorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlantController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public KlantController(IUnitOfWork context)
        {
            _unitOfWork = context;
        }

        // GET: api/Klant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Klant>>> GetKlanten()
        {
            var klanten = _unitOfWork.KlantRepository.GetAllAsync();
            return Ok(await klanten);
        }

        // GET: api/Klant/
        [HttpGet("{id}")]
        public async Task<ActionResult<Klant>> GetKlant(string id)
        {
            //var klant = await _context.Klanten.FirstOrDefaultAsync(x=>x.Id == id);
            var klant = await _unitOfWork.KlantRepository.GetByIdAsync(id);
            if (klant == null)
            {
                return NotFound("Er is geen klant gevonden met deze Id");
            }

            return Ok(klant);
        }

        [HttpGet("Search")]
        public ActionResult<List<Klant>> Search(string zoekwaarde)
        {
            //var product = _context.Klanten.Where(x => x.Naam.Contains(zoekwaarde) || x.Voornaam.Contains(zoekwaarde)).OrderBy(x => x.Naam);
            var klant = _unitOfWork.KlantRepository.SearchAsync(zoekwaarde);
            if (klant == null)
            {
                return NotFound($"Er zijn geen klanten in de database waar {zoekwaarde} voorkomt in de naam of voornaam");
            }
            return Ok(klant);
        }

        [HttpPost]
        public async Task<ActionResult<Klant>> KlantToevoegen(Klant klant)
        {
            //Validatie
            if (_unitOfWork.KlantRepository == null)
                return NotFound("De tabel Klanten bestaat niet in de database.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Klant toevoegen aan de DbSet
            await _unitOfWork.KlantRepository.AddAsync(klant);
            try
            {
                //Klant wegschrijven naar de database
                _unitOfWork.SaveChangesAsync();
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.KlantRepository.Update(klant);

            try
            {
                _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_unitOfWork.KlantRepository.FindAsync(id) == null)
                {
                    return NotFound("Er is geen klant met dit id gevonden");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> KlantVerwijdern(string id)
        {
            if (_unitOfWork.KlantRepository == null)
            {
                return NotFound("De tabel klanten bestaat niet.");
            }
            Klant klant = await _unitOfWork.KlantRepository.GetByIdAsync(id);
            if (klant == null)
            {
                return NotFound("De klant met deze id is niet gevonden.");
            }

            _unitOfWork.KlantRepository.Delete(klant);
            await _unitOfWork.SaveChangesAsync();
            

            return Ok($"Klant met id {id} is verwijderd");
        }
    }
}
