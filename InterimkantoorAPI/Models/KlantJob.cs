namespace InterimkantoorAPI.Models
{
    public class KlantJob
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string KlantId { get; set; }

        public Klant Klant { get; set; }
        public Job Job { get; set; }




    }
}
