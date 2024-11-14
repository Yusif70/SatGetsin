using Microsoft.AspNetCore.Identity;

namespace Advertisement.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? Pfp { get; set; }
        public double Balance { get; set; }
        public List<Ad> Ads { get; set; }
        public List<Favorites> Favorites { get; set; }
    }
}

