using Microsoft.AspNetCore.Identity;

namespace BlueBerry_API.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
