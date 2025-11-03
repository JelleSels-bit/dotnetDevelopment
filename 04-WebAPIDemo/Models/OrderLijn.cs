

namespace WebAPIDemo.Models
{
    public class Orderlijn
    {
        public int Id { get; set; }

        [Required]
        public double Aantal { get; set; }
        public int BestellingId { get; set; }


        public int ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; } = default!;
        [JsonIgnore]
        public Bestelling? Bestelling { get; set; } = default!;
    }
}
