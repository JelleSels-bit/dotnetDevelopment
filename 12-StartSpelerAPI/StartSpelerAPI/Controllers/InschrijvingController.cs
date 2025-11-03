using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using StartSpelerAPI.Data.UnitOfWork;
using StartSpelerAPI.Dto.Community;
using StartSpelerAPI.Dto.Inschrijving;
using StartSpelerAPI.Models;

namespace StartSpelerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InschrijvingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Gebruiker> _userManager;
        private readonly IMapper _mapper;

        public InschrijvingController(IUnitOfWork unitOfWork, UserManager<Gebruiker> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost(Name = "Inschrijven")]
        public async Task<ActionResult> Inschrijven(InschrijvenDto dto)
        {
            if (!await BestaatEvent(dto.EventId))
                return BadRequest("Dit event bestaat niet!");

            if (!await BestaatGebruiker(dto.GebruikerId))
                return BadRequest("Deze gebruiker bestaat niet!");

            if (await BestaatInschrijving(dto.EventId, dto.GebruikerId))
                return BadRequest("De gebruiker is reeds ingeschreven voor dit event!");

            Inschrijving inschrijving = _mapper.Map<Inschrijving>(dto);

            await _unitOfWork.InschrijvingRepository.AddAsync(inschrijving);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Uitschrijven(UitschrijvenDto dto)
        {
            if (!await BestaatEvent(dto.EventId))
                return BadRequest("Dit event bestaat niet!");

            if (!await BestaatGebruiker(dto.GebruikerId))
                return BadRequest("Deze gebruiker bestaat niet!");

            Inschrijving inschrijving = await _unitOfWork.InschrijvingRepository
                .GetInschrijving(dto.EventId, dto.GebruikerId);
            if (inschrijving == null)
                return NotFound();

            _unitOfWork.InschrijvingRepository.Delete(inschrijving);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InschrijvingenPerEventDto>>> GetInschrijvingen()
        {
            var inschrijvingen = await _unitOfWork.InschrijvingRepository.GetVolledigeInschrijvingen();

            List<InschrijvingenPerEventDto> dto = _mapper.Map<List<InschrijvingenPerEventDto>>(inschrijvingen);

            return Ok(dto.ToList());
        }

        private async Task<bool> BestaatGebruiker(string id)
        {
            Gebruiker gebruiker = await _userManager.FindByIdAsync(id);
            if (gebruiker == null)
                return false;
            return true;
        }

        private async Task<bool> BestaatEvent(int id)
        {
            Event ev = await _unitOfWork.EventRepository.GetByIdAsync(id);
            if (ev == null)
                return false;
            return true;
        }

        private async Task<bool> BestaatInschrijving(int eventId, string gebruikerId)
        {
            Inschrijving inschrijving = await _unitOfWork.InschrijvingRepository
                .GetInschrijving(eventId, gebruikerId);
            if (inschrijving == null)
                return false;
            return true;
        }
    }
}
