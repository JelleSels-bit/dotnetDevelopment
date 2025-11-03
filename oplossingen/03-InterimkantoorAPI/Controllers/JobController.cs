using System.Threading.Tasks;
using InterimkantoorAPI.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterimkantoorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IUnitOfWork _context;

        public JobController(IUnitOfWork context)
        {
            _context = context ;
        }

        // GET: api/Job
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {

            return Ok(_context.JobRepository.GetJobs());
        }

        // GET: api/Job/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = _context.JobRepository.GetJob(id);

            if (job == null)
            {
                return NotFound("Er is geen Job gevonden met deze id");
            }

            return Ok(job);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Job>>> Search(string zoekwaarde)
        {
            var jobs = await _context.JobRepository.SearchAsync(zoekwaarde);

            if (!jobs.Any())
            {
                return NotFound($"Er zijn geen jobs in de database waar '{zoekwaarde}' voorkomt in de omschrijving of locatie.");
            }
            return Ok(jobs);
        }


        //POST
        [HttpPost("AssignAPI")]
        public async Task<ActionResult<KlantJob>> Assign(KlantJob klantjob)
        {
            
            var result = _context.KlantJobs.Where(x => x.JobId == klantjob.JobId && x.KlantId == klantjob.KlantId).ToList();

            if (result.Count != 0)
            {
                return Problem("Klant is al toegewezen aan deze job.");
            }

            await _context.KlantJobs.AddAsync(klantjob);
            _context.SaveChanges();

            return CreatedAtAction("GetJob", new { id = klantjob.Id }, klantjob);

        }

        [HttpPost]
        public async Task<ActionResult<Job>> JobToevoegen(Job job)
        {
            //Validatie
            if (_context.JobRepository == null)
                return NotFound("De tabel Job bestaat niet in de database.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(job.EindDatum<job.StartDatum)
            {
                return BadRequest("De einddatum is kleiner dan de startdatum");
            }

            //Job toevoegen aan de DbSet
            await _context.JobRepository.AddAsync(job);
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

            var existingJob = await _context.Jobs.Include(b => b.KlantJobs)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (existingJob == null)
            {
                return NotFound("De Job die je wil wijzigen, komt niet voor in de database.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existingJob.Omschrijving = job.Omschrijving;
            existingJob.StartDatum = job.StartDatum;
            existingJob.EindDatum = job.EindDatum;
            existingJob.Locatie = job.Locatie;
            existingJob.IsWerkschoenen = job.IsWerkschoenen;
            existingJob.IsBadge = job.IsBadge;
            existingJob.IsKleding = job.IsKleding;
            existingJob.AantalPlaatsen = job.AantalPlaatsen;

            foreach (var updatedKlantJob in job.KlantJobs)
            {
                var existingKlantJob = existingJob.KlantJobs.FirstOrDefault(ol => ol.Id == updatedKlantJob.Id);

                if (existingKlantJob != null)
                {
                    existingKlantJob.KlantId = updatedKlantJob.KlantId;
                }
                else
                {
                    existingJob.KlantJobs.Add(new KlantJob
                    {
                        JobId = existingJob.Id,
                        KlantId = updatedKlantJob.KlantId

                    });
                }
            }
            var klantJobsToRemove = existingJob.KlantJobs.Where(ol => !job.KlantJobs.Any(uol => uol.Id == ol.Id)).ToList();

            _context.KlantJobs.RemoveRange(klantJobsToRemove);



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
            if (_context.JobRepository == null)
            {
                return NotFound("De tabel Job bestaat niet.");
            }

            var job = await _context.JobRepository.GetJob(id);
            if (job == null)
            {
                return NotFound("De job met deze id is niet gevonden.");
            }

            foreach (var klantJob in job.KlantJobs)
            {
                _context.Klant;
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return Ok($"Job met id {id} is verwijderd");
        }
    }
}
