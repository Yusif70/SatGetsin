using Advertisement.Core.Entities.Base;

namespace Advertisement.Core.Entities
{
    public class Favorites : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Guid AdId { get; set; }
        public Ad Ad { get; set; }
    }
}
