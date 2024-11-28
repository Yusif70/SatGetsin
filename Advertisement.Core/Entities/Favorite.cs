using Advertisement.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Advertisement.Core.Entities
{
    public class Favorite : BaseEntity
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Guid AdId { get; set; }
        public Ad Ad { get; set; }
    }
}
