using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartSpelerAPI.Data;
using StartSpelerAPI.Data.UnitOfWork;
using StartSpelerAPI.Dto.Community;
using StartSpelerAPI.Models;

namespace StartSpelerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommunityController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin, communitymanager")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunityWithEventsDto>>> GetCommunities()
        {
            var communities = await _unitOfWork.CommunityRepository.GetCommunitiesWithEvents();

            List<CommunityWithEventsDto> dto = _mapper.Map<List<CommunityWithEventsDto>>(communities);

            return Ok(dto.ToList());
        }

        [Authorize(Roles = "admin, communitymanager")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CommunityWithEventsDto>> GetCommunity(int id)
        {
            Community? community = await _unitOfWork.CommunityRepository.GetCommunityWithEvents(id);

            if (community == null)
            {
                return NotFound($"Community {id} kan niet worden gevonden in de database");
            }

            CommunityWithEventsDto dto = _mapper.Map<CommunityWithEventsDto>(community);

            return Ok(dto);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunity(int id, UpdateCommunityDto dto)
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
                if (CommunityExists(id).Result == false)
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Community>> PostCommunity(CreateCommunityDto dto)
        {
            Community community = _mapper.Map<Community>(dto);

            await _unitOfWork.CommunityRepository.AddAsync(community);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetCommunity", new { id = community.Id }, community);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            var community = await _unitOfWork.CommunityRepository.GetByIdAsync(id);

            if (community == null)
            {
                return NotFound();
            }

            _unitOfWork.CommunityRepository.Delete(community);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> CommunityExists(int id)
        {
            var gevonden = await _unitOfWork.CommunityRepository.GetByIdAsync(id);

            if (gevonden == null)
                return false;
            else
                return true;
        }
    }
}
