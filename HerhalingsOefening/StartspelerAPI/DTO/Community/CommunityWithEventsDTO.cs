using StartspelerAPI.DTO.Event;

namespace StartspelerAPI.DTO.Community
{
    public class CommunityWithEventsDTO
    {
        public string Naam { get; set; }
        public List<EventDTO>? Events { get; set; }
    }
}
