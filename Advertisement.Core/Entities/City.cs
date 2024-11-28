using Advertisement.Core.Entities.Base;
using System.Text.Json.Serialization;

namespace Advertisement.Core.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        [JsonIgnore]
        public List<Ad> Ads { get; set; }
    }
}
