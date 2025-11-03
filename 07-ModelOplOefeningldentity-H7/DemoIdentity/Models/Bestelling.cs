using System.Numerics;

namespace DemoIdentity.Models
{
    public class Bestelling
    {
        public int BestellingID { get; set; }

        public int KlantID { get; set; }
        public Klant Klant { get; set; } = default!;

        //public List<OrderLijn> orderlijnen { get; set; } = default!;
    }

}
