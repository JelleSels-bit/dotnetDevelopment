using InterimkantoorAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterimkantoorAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly APIinterimkantoorContext _context;

        public JobController(APIinterimkantoorContext context)
        {
            _context = context;
        }

        // GET: api/Job
        [HttpGet]
        public async Task<ActionResult<List<Job>>> GetJobs()
        {
            var jobs = await _context.Jobs.Include(x => x.KlantJobs).ThenInclude(x => x.Klant).ToListAsync();
            if (!jobs.Any())
                return BadRequest("Er zijn geen jobs gevonden.");

            return jobs;
        }

        // POST: api/Bestelling
        [HttpPost]
        public async Task<ActionResult<KlantJob>> JobToewijzen(KlantJob klantjob)
        {
            if (_context.KlantJob == null)
                return Problem("De tabel Jobs bestaat niet in de database.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _context.KlantJob.Where(x => x.JobId == klantjob.JobId && x.KlantId == klantjob.KlantId).ToList();

            if (result.Count != 0)
            {
                return Problem("Klant is al toegewezen aan deze job.");
            }

            await _context.KlantJob.AddAsync(klantjob);
            _context.SaveChanges();

            return CreatedAtAction("GetJob", new { id = klantjob.Id }, klantjob);
        }

        // GET: api/Job/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id);

            if (job == null)
            {
                return NotFound("Er is geen Job gevonden met deze id.");
            }

            return Ok(job);
        }

        [HttpGet("Search")]
        public ActionResult<List<Job>> Search(string zoekwaarde)
        {
            var jobs = _context.Jobs.Where(x => x.Omschrijving.Contains(zoekwaarde)).OrderBy(x => x.Omschrijving).ToList();

            if (!jobs.Any())
            {
                return NotFound($"Er zijn geen jobs in de database waar '{zoekwaarde}' voorkomt in de omschrijving of locatie.");
            }
            return Ok(jobs);
        }

        [HttpPost]
        public async Task<ActionResult<Job>> JobToevoegen(Job job)
        {
            //Validatie
            if (_context.Jobs == null)
                return NotFound("De tabel Job bestaat niet in de database.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (job.EindDatum < job.StartDatum)
            {
                return BadRequest("De einddatum is kleiner dan de startdatum");
            }

            //Job toevoegen aan de DbSet
            await _context.Jobs.AddAsync(job);
            try
            {
                //Job wegschrijven naar de database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }

            return CreatedAtAction(nameof(GetJob), new { id = job.Id }, job);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> JobWijzigen(int id, Job job)
        {
            if (id != job.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Jobs.Update(job);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Jobs.Any(x => x.Id == id))
                {
                    return NotFound("Er is geen job met dit id gevonden.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> JobVerwijderen(int id)
        {
            if (_context.Jobs == null)
            {
                return NotFound("De tabel Job bestaat niet.");
            }

            var job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id);
            if (job == null)
            {
                return NotFound("De job met deze id is niet gevonden.");
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return Ok($"Job met id {id} is verwijderd");
        }
    }
}