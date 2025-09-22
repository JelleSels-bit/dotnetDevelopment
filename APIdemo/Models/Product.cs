using System.ComponentModel.DataAnnotations;

namespace APIdemo.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="De naam moet ingevuld worden.")]
        [MaxLength(50,ErrorMessage ="De naam mag maar 50 karakters bevatten")]
        public string Naam { get; set; }
        [Required(ErrorMessage ="De Prijs moet ingevuld worden")]
        public decimal Prijs { get; set; }

    }
}
