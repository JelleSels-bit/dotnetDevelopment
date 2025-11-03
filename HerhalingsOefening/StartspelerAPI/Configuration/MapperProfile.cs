using AutoMapper;
using StartspelerAPI.DTO.Community;
using StartspelerAPI.DTO.Event;

namespace StartspelerAPI.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<Community, CommunityWithEventsDTO>().ReverseMap();
            CreateMap<Community, PutCommunityDTO>().ReverseMap();
            CreateMap<Community, CreateCommunityDTO>().ReverseMap();
            
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<Event,CreateEventDTO>().ReverseMap();
            CreateMap<Event, EventWithCommunityDto>()
                .ForMember(dest => dest.Community, opt => opt.MapFrom(src => src.Community.Naam));
            CreateMap<Event, PutEventDTO>().ReverseMap();
                        

        }

    }
}
