using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StartspelerAPI.Models
{
    public class Gebruiker : IdentityUser
    {
        [Required]
        public string Voornaam { get; set; }
        [Required]
        public string Familienaam { get; set; }
        [Required]
        public DateTime Geboortedatum { get; set; }

        public List<Inschrijving>? Inschrijvingen { get; set; }


    }
}
