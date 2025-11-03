using DemoIdentity.Models;

namespace DemoIdentity.Helper
{
    public class UserWithRoles
    {
        public BeperkteUser User { get; set; }
        public IList<string> Roles { get; set; }
    }

}
