using SatGetsin2.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SatGetsin2.Core.Entities
{
    public class Image : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(Ad))]
        public Guid AdId { get; set; }
        [JsonIgnore]
        public Ad Ad { get; set; }
        public bool IsMain { get; set; }
    }
}
