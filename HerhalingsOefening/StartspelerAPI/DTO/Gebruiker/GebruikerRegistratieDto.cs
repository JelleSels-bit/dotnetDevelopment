using System.ComponentModel.DataAnnotations;

namespace StartspelerAPI.DTO.Gebruiker
{
    public class GebruikerRegistratieDto
    {
        [Required(ErrorMessage = "De voornaam is verplicht.")]
        [StringLength(50, ErrorMessage = "De voornaam mag maximaal 50 karakters zijn.")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "De familienaam is verplicht.")]
        [StringLength(50, ErrorMessage = "De familienaam mag maximaal 50 karakters zijn.")]
        public string Familienaam { get; set; }

        [Required(ErrorMessage = "De geboortedatum is verplicht.")]
        public DateTime Geboortedatum { get; set; }

        [EmailAddress(ErrorMessage = "Ongeldig emailadres")]
        [Required(ErrorMessage = "Email is verplicht!")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is verplicht!")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Tweede wachtwoord is verplicht in te vullen.")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; } = "";

        public string PhoneNumber { get; set; } = "";
    }
}
