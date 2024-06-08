using Microsoft.AspNetCore.Identity;

namespace Ostrovskyi.UI.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public byte[] Avatar { get; set; }
        public string MimeType { get; set; } = string.Empty;
    }
}
