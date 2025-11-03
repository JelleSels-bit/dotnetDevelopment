namespace StartSpelerAPI.Dto.Inschrijving
{
    public class InschrijvenDto
    {
        [Required]
        public string GebruikerId { get; set; }
        [Required]
        public int EventId { get; set; }
    }
}
