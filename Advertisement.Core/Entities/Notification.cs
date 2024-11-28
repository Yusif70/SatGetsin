using Advertisement.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Advertisement.Core.Entities
{
    public class Notification : BaseEntity
    {
        [ForeignKey(nameof(Ad))]
        public Guid AdId { get; set; }
        public Ad Ad { get; set; }
        public string Message { get; set; }
    }
}
