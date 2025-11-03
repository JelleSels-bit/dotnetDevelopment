using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InterimkantoorAPI.Models
{
    public class Job
    {
    public int Id { get; set; }
        [Required(ErrorMessage ="De naam moet ingevuld worden.")]
        [MaxLength(200, ErrorMessage ="De omschrijving mag maar 200 karakters bevatten")]
    public string Omschrijving { get; set; }
        [MaxLength(100, ErrorMessage = "De omschrijving mag maar 100 karakters bevatten")]
    public string Locatie { get; set; }
        [Required()]
        [DataType(DataType.Date)]
        public DateTime StartDatum { get; set; } = DateTime.Now;
        [Required()]
        [DataType(DataType.Date)]
        public DateTime EindDatum { get; set; } = DateTime.Now.AddDays(1);

        [DefaultValue(false)]
    public bool IsWerkschoenen { get; set; } 
        [DefaultValue(false)]
    public bool IsBadge { get; set; }
        [DefaultValue(false)]
    public bool IsKleding { get; set; }
        [MaxLength(100,ErrorMessage ="100 max")]
        [MinLength(1,ErrorMessage ="min 1")]
    public int AantalPlaatsen { get; set; }

    public List<KlantJob>? KlantJobs { get; set; }

    }
}

