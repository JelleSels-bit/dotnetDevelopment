using System.ComponentModel.DataAnnotations;

namespace StartSpelerAPI.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "De naam is verplicht.")]
        [StringLength(50, ErrorMessage = "De naam mag maximaal 50 karakters zijn.")]
        [MinLength(5, ErrorMessage = "De naam moet minstens 5 karakters bevatten.")]
        public string Naam { get; set; }
        [StringLength(200, ErrorMessage = "De beschrijving mag maximaal 200 karakters bevatten.")]
        public string? Beschrijving { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartMoment { get; set; } = DateTime.Now.AddDays(1);

        public decimal? Prijs { get; set; }
        [Range(4, 32, ErrorMessage = "Het aantal deelnemers moet tussen de 4 en 32 liggen. ")]
        public int? MaxDeelnemers { get; set; }
        public int? CommunityId { get; set; }
        public Community? Community { get; set; }

        public List<Inschrijving>? Inschrijvingen { get; set; }
    }
}
