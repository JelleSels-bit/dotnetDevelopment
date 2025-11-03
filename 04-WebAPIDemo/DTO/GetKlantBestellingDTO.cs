namespace WebAPIDemo.DTO
{
    public class GetKlantBestellingDTO
    {
        public string KlantNaam { get; set; }
        public List<GetBestellingDTO> Bestellingen { get; set; }
    }
}
