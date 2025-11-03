using System.ComponentModel.DataAnnotations;

namespace StartSpelerAPI.Models
{
    public class Community
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "De naam is verplicht.")]
        [StringLength(50, ErrorMessage = "De naam mag maximaal 50 karakters zijn.")]
        public string Naam { get; set; }

        public List<Event>? Events { get; set; }
    }
}
