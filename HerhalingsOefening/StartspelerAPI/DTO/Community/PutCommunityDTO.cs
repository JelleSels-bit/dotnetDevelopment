using System.ComponentModel.DataAnnotations;
using StartspelerAPI.DTO.Event;

namespace StartspelerAPI.DTO.Community
{
    public class PutCommunityDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "max 50 char long")]
        public string Naam { get; set; }
        public List<EventDTO>? Events { get; set; }

    }
}
