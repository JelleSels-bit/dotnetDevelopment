using System.ComponentModel.DataAnnotations;

namespace DogRescue.Models
{
    public class HondModel
    {
        [Key]
        public int HondId { get; set; }
        public DateTime? GeboorteDatum { get; set; }
        public Geslacht Geslacht { get; set; }
        public string Naam { get; set; }
        public string Opmerkingen { get; set; }


    }
}
