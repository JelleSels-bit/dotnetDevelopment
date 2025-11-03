using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartspelerAPI.DTO.Event;
using StartspelerAPI.Models;

namespace StartspelerAPI.Controller
{
    [Route("api/[Controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public EventController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventWithCommunityDto>>> GetAllEvents()
        {
            var evs = await _context.EventRepository.GetAllEventsAsync();

            List<EventWithCommunityDto> dto = _mapper.Map<List<EventWithCommunityDto>>(evs);
            return Ok(dto.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventWithCommunityDto>> GetEventById(int id)
        {
            var ev = await _context.EventRepository.GetEventByIdAsync(id);
            if (ev == null)
            {
                return BadRequest("Er is geen Event gevonden");
            }

            EventWithCommunityDto dto = _mapper.Map<EventWithCommunityDto>(ev);
            return Ok(dto);

        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(CreateEventDTO createEventDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event ev = _mapper.Map<Event>(createEventDTO);
            await _context.EventRepository.AddAsync(ev);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException dbError)
            {
                return BadRequest(dbError);
            }

            return CreatedAtAction(nameof(GetEventById), new { id = ev.Id }, ev);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutEvent(int id, PutEventDTO putEventDTO)
        {
            if (id != putEventDTO.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event? ev = await _context.EventRepository.GetEventByIdAsync(id);
            if (ev == null)
            {
                return BadRequest("Er is geen record gevonden met id");
            }

            _mapper.Map(putEventDTO, ev);
            _context.EventRepository.Update(ev);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (EventExists(id).Result == false)
                {
                    return NotFound("Er is geen Event gevonden met dit Id");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("${id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            Event? ev = await _context.EventRepository.GetByIdAsync(id);
            if (ev == null)
                return NotFound("Kan event met dit id niet vinden");

            _context.EventRepository.Delete(ev);
            await _context.SaveChangesAsync();
            return Ok($"Het event {ev.Naam} met id ${id} is succesvol verwijderd");

        }

        private async Task<bool> EventExists(int id)
        {
            var gevonden = await _context.EventRepository.GetByIdAsync(id);

            if (gevonden == null)
                return false;
            else
                return true;
        }
    }
}
