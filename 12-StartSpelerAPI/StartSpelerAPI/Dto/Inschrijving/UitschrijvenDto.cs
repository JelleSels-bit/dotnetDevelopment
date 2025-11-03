namespace StartSpelerAPI.Dto.Inschrijving
{
    public class UitschrijvenDto
    {
        [Required]
        public string GebruikerId { get; set; }
        [Required]
        public int EventId { get; set; }
    }
}
