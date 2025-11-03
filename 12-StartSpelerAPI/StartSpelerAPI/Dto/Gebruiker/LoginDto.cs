namespace StartSpelerAPI.Dto.Gebruiker
{
    public class LoginDto
    {
        [EmailAddress(ErrorMessage = "Ongeldig emailadres")]
        [Required(ErrorMessage = "Email is verplicht!")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is verplicht!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
