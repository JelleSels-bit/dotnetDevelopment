using StartSpelerAPI.Dto.Event;
using StartSpelerAPI.Dto.Gebruiker;

namespace StartSpelerAPI.Dto.Inschrijving
{
    public class InschrijvingenPerEventDto
    {
        public string Gebruiker { get; set; }
        public DateTime TimeStampInschrijving { get; set; } = DateTime.Now;
    }
}
