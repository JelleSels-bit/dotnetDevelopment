using Microsoft.EntityFrameworkCore;

namespace StartSpelerAPI.Models
{
    public class Inschrijving
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "De EventId is verplicht.")]
        public int EventId { get; set; }
        [Required(ErrorMessage = "De GebruikerId is verplicht.")]
        public string GebruikerId { get; set; }
        [Required(ErrorMessage = "De TimeStamp voor de inschrijving is verplicht.")]
        public DateTime TimeStampInschrijving { get; set; } = DateTime.Now;
        public Event? Event { get; set; }
        public Gebruiker? Gebruiker { get; set; }
    }
}
