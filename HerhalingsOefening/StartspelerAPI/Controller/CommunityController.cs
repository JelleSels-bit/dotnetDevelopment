using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Any;
using StartspelerAPI.DTO.Community;

namespace StartspelerAPI.Controller
{
    
    [Route("api/[Controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {

        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public CommunityController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin, communitymanager")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunityWithEventsDTO>>> GetAllCommunitysWithEvents()
        {
            var communitys = await _context.CommunityRepository.GetAllCommunityAsync();

            List<CommunityWithEventsDTO> dtos = _mapper.Map<List<CommunityWithEventsDTO>>(communitys);
            return Ok(dtos.ToList());
        }

        [Authorize(Roles = "admin, communitymanager")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CommunityWithEventsDTO>> GetCommunityById(int id)
        {
            var community = await _context.CommunityRepository.GetCommunityByIdAsync(id);

            if (community == null)
            {
                return NotFound("Er is geen job gevonden met dit id");
            }

            CommunityWithEventsDTO dto = _mapper.Map<CommunityWithEventsDTO>(community);
            return Ok(dto);

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Community>> PostCommunity(CreateCommunityDTO createCommunityDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Community community = _mapper.Map<Community>(createCommunityDTO);
            await _context.CommunityRepository.AddAsync(community);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }

            return CreatedAtAction(nameof(GetCommunityById), new { id = community.Id }, community);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCommunity(int id, PutCommunityDTO PutCommunityDTO)
        {
            if (id != PutCommunityDTO.Id)
            {
                return BadRequest("De opgegeven id's komen niet overeen");

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Community? community = await _context.CommunityRepository.GetByIdAsync(id);
            if (community == null)
                return BadRequest("Er is geen record gevonden met dit id");

            _mapper.Map(PutCommunityDTO, community);
            _context.CommunityRepository.Update(community);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var communitys = await _context.CommunityRepository.GetAllAsync();
                if (communitys.Any(x => x.Id == id))
                {
                    return NotFound("Er is geen product gevonden met dit Id");
                }else
                {
                    throw;
                }
            }
            return NoContent();

        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommunity(int id)
        {
            if (_context.CommunityRepository == null)
            {
                return BadRequest("De tabel Community bestaat niet");
            }
            Community? community = await _context.CommunityRepository.GetByIdAsync(id);
            if (community == null)
            {
                return NotFound("De klant met deze id is niet gevonden");
            }

            _context.CommunityRepository.Delete(community);
            await _context.SaveChangesAsync();
            return Ok($"De Community {community.Naam} met id ${id} is succesvol verwijderdt");


        }
        


    }
}
