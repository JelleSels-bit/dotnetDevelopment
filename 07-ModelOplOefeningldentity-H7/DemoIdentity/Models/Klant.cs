using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoIdentity.Models
{
    public class Klant
    {
        public int KlantID { get; set; }
        [Required]
        public string Naam { get; set; } = default!;
        [Required]
        public string Voornaam { get; set; } = default!;
        [DataType(DataType.Date), Display(Name = "Datum Aangemaakt")]

        public DateTime AangemaaktDatum { get; set; }
        [JsonIgnore]
        public IList<Bestelling> Bestellinging { get; set; } = default!;
    }
}
