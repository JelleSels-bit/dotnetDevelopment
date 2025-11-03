



using StartSpelerAPI.Dto.Community;
using StartSpelerAPI.Dto.Event;
using StartSpelerAPI.Dto.Inschrijving;

namespace StartSpelerAPI.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<Community, CommunityWithEventsDto>().ReverseMap();

            CreateMap<Event, EventWithCommunityDto>()
                .ForMember(dest => dest.Community, opt => opt.MapFrom(src => src.Community.Naam));

            CreateMap<Inschrijving, InschrijvingenPerEventDto>()
                .ForMember(dest => dest.Gebruiker, opt => opt.MapFrom(src => $"{src.Gebruiker.Voornaam} {src.Gebruiker.Familienaam}"));

            CreateMap<InschrijvenDto, Inschrijving>();

            CreateMap<UitschrijvenDto, Inschrijving>();
        }
    }
}
