using AutoMapper;


namespace WebAPIDemo.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<AddProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

            //Bij get ga je gegevens ophalen
            CreateMap<Klant, GetKlantBestellingDTO>()
                .ForMember(dest => dest.KlantNaam, opt => opt.MapFrom(src => $"{src.Voornaam} {src.Naam}"));

            CreateMap<Bestelling, GetBestellingDTO>()
                .ForMember(dest => dest.Producten, opt => opt.MapFrom(src => src.Orderlijnen.Select(ol => new GetProductDTO
                {
                    Naam = ol.Product.Naam,
                    Beschrijving = ol.Product.Beschrijving,
                    Prijs = ol.Product.Prijs,
                    Aantal = ol.Aantal,
                    Totaal = ol.Aantal * Convert.ToDouble(ol.Product.Prijs)
                }))); 
        }
    }
}
