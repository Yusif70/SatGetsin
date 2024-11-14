using Advertisement.Core.Entities.Base;
using System.Text.Json.Serialization;

namespace Advertisement.Core.Entities
{
    public class Image : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        [JsonIgnore]
        public Guid AdId { get; set; }
        [JsonIgnore]
        public Ad Ad { get; set; }
    }
}
