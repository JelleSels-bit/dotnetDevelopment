using StartSpelerAPI.Dto.Event;

namespace StartSpelerAPI.Dto.Community
{
    public class CommunityWithEventsDto
    {
        public string Naam { get; set; }
        public List<EventDto>? Events { get; set; }
    }
}
