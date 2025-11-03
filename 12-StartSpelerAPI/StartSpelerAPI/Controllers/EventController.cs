using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StartSpelerAPI.Data.UnitOfWork;
using StartSpelerAPI.Dto.Community;
using StartSpelerAPI.Dto.Event;

namespace StartSpelerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await _unitOfWork.EventRepository.GetEventsWithCommunity();

            List<EventWithCommunityDto> dto = _mapper.Map<List<EventWithCommunityDto>>(events);

            return Ok(dto.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<EventWithCommunityDto>> GetEvent(int id)
        {
            Event? ev = await _unitOfWork.EventRepository.GetEventWithCommunity(id);

            if (ev == null)
            {
                return NotFound($"Event {id} kan niet worden gevonden in de database");
            }

            EventWithCommunityDto dto = _mapper.Map<EventWithCommunityDto>(ev);

            return Ok(dto);
        }


        [Authorize(Roles = "admin, communitymanager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, UpdateEventDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            Community community = _mapper.Map<Community>(dto);

            _unitOfWork.CommunityRepository.Update(community);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (EventExists(id).Result == false)
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

        [Authorize(Roles = "admin, communitymanager")]
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(CreateEventDto dto)
        {
            Event ev = _mapper.Map<Event>(dto);

            await _unitOfWork.EventRepository.AddAsync(ev);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = ev.Id }, ev);
        }

        [Authorize(Roles = "admin, communitymanager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _unitOfWork.EventRepository.GetByIdAsync(id);

            if (ev == null)
            {
                return NotFound();
            }

            _unitOfWork.EventRepository.Delete(ev);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> EventExists(int id)
        {
            var gevonden = await _unitOfWork.EventRepository.GetByIdAsync(id);

            if (gevonden == null)
                return false;
            else
                return true;
        }

    }
}
