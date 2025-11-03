using DemoIdentity.Data;
using DemoIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoIdentity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KlantController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public KlantController(DatabaseContext context)
        {
            _context = context;
        }

        [Route("GeefALleUsersOpLijst")]
        [HttpGet]
        public async Task<ActionResult> GetLijst()
        {
            return Ok(await _context.Klanten.ToListAsync());
        }

        [Authorize(Roles = "SuperAdmin, Admin, Webmaster")]
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            Klant? klant = await _context.Klanten.Where(u => u.KlantID == id).FirstOrDefaultAsync();

            if (klant != null)
                return Ok(klant);
            else return Ok(null);
        }

        // Enkel superadmin
        [Authorize(Roles = "Superadmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Klant? klant = await _context.Klanten.FindAsync(id);
            if (klant != null)
            {
                _context.Klanten.Remove(klant);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [Authorize(Roles = "Superadmin, Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Klant klant)
        {
            if (klant == null || id != (klant.KlantID.ToString())) return BadRequest();

            Klant? klantdb = await _context.Klanten.FindAsync(id);

            if (klantdb == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Klanten.Update(klant);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return BadRequest();
        }

    }
}
