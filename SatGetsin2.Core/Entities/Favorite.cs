using SatGetsin2.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SatGetsin2.Core.Entities
{
    public class Favorite : BaseEntity
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        [ForeignKey(nameof(Ad))]
        public Guid AdId { get; set; }
        public Ad Ad { get; set; }
    }
}
