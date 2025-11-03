using System.ComponentModel.DataAnnotations;

namespace StartspelerAPI.DTO.Community
{
    public class CreateCommunityDTO
    {
        [Required(ErrorMessage = "De naam is verplicht.")]
        [StringLength(50, ErrorMessage = "De naam mag maximaal 50 karakters zijn.")]
        public string Naam { get; set; }

    }
}
