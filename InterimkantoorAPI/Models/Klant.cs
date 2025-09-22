using System.ComponentModel.DataAnnotations;

namespace InterimkantoorAPI.Models
{
    public class Klant
    {
            [Required()]
            [RegularExpression("^[A-Z]{1,4}$", ErrorMessage ="4 hoofdletters")]
        public string Id { get; set; }
            [Required()]
            [MaxLength(100)]
        public string Naam { get; set; }
            [Required()]
            [MaxLength(100)]
        public string Voornaam { get; set; }
            [Required()]
            [MaxLength(100)]
        public string Gemeente { get; set; }
            [Required()]        
        public int Postcode { get; set; }
            [Required()]
            [MaxLength(100)]
        public string Straat { get; set; }
        public int Huisnummer { get; set; }
            [Required()]
        public string Bankrekeningnummer { get; set; }
    }
}
