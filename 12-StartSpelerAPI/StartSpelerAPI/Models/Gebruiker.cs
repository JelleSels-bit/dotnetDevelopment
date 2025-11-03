

namespace StartSpelerAPI.Models
{
    public class Gebruiker : IdentityUser
    {
        [Required(ErrorMessage = "De voornaam is verplicht.")]
        [StringLength(50, ErrorMessage = "De voornaam mag maximaal 50 karakters zijn.")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "De familienaam is verplicht.")]
        [StringLength(50, ErrorMessage = "De familienaam mag maximaal 50 karakters zijn.")]
        public string Familienaam { get; set; }

        [Required(ErrorMessage = "De geboortedatum is verplicht.")]
        public DateTime Geboortedatum { get; set; }

        public List<Inschrijving>? Inschrijvingen { get; set; }
    }
}
