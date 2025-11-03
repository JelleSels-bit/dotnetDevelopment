using Microsoft.AspNetCore.Identity;

namespace DemoIdentity.Models
{
    public class CustomUser : IdentityUser
    {
        [PersonalData]
        public string? Naam { get; set; }
        [PersonalData]
        public DateTime? Geboortedatum { get; set; }

        public static implicit operator CustomUser(BeperkteUser v)
        {
            throw new NotImplementedException();
        }
    }
}
