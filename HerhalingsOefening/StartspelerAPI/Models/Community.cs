using System.ComponentModel.DataAnnotations;

namespace StartspelerAPI.Models
{
    public class Community
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "max 50 char long")]
        public string Naam { get; set; }
        public List<Event>? Events { get; set; }
        
    }
}
