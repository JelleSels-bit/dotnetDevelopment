namespace StartSpelerAPI.Dto.Community
{
    public class UpdateCommunityDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "De naam is verplicht.")]
        [StringLength(50, ErrorMessage = "De naam mag maximaal 50 karakters zijn.")]
        public string Naam { get; set; }
    }
}
