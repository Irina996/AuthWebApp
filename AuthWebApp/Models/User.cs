using Microsoft.AspNetCore.Identity;

namespace AuthWebApp.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastLogIn { get; set; } = DateTime.Now;
        public bool IsBlocked { get; set; } = false;
    }
}
