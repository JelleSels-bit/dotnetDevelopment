using System.ComponentModel.DataAnnotations;

namespace StartspelerAPI.DTO.Event
{
    public class PutEventDTO
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Min length is 5 char")]
        [MaxLength(50, ErrorMessage = "Max length is 50 char")]
        public string Naam { get; set; }
        [MaxLength(200, ErrorMessage = "Max length is 200 char")]
        public string? Beschrijving { get; set; }
        [Required]
        public DateTime Startmoment { get; set; }
        public int? Prijs { get; set; }
        [Range(4, 32, ErrorMessage = "Het aantal deelnemers moet tussen de 4 en 32 liggen. ")]
        public int? MaxDeelnemers { get; set; }
        public int? CommunityId { get; set; }
    }
}
