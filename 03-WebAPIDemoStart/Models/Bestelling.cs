

namespace WebAPIDemo.Models
{
    public class Bestelling
    {
        public int Id { get; set; }

        public int KlantId{ get; set; }

        [JsonIgnore]
        public Klant? Klant { get; set; }

        public List<Orderlijn> Orderlijnen { get; set; }
    }
}
