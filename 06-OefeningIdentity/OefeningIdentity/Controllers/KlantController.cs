using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OefeningIdentity.Models;

namespace OefeningIdentity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KlantController : ControllerBase
    {
        private UserManager<CustomUser> _userManager;

        public KlantController(UserManager<CustomUser> userManager) 
        {
            _userManager = userManager;
        }

        [Authorize]
        [Route("Lijst")]
        [HttpGet]
        public async Task<ActionResult> GetLijst()
        {
            return Ok(await _userManager.Users.ToListAsync());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(string id)
        {
            CustomUser? gebruiker = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (gebruiker != null)
                return Ok(gebruiker);
            else return Ok(null);
        }
    }
}
