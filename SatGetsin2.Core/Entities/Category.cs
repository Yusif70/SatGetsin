using SatGetsin2.Core.Entities.Base;
using System.Text.Json.Serialization;

namespace SatGetsin2.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        [JsonIgnore]
        public Guid? ParentCategoryId { get; set; }
        [JsonIgnore]
        public Category ParentCategory { get; set; }
        public List<Category> ChildCategories { get; set; }
        [JsonIgnore]
        public List<Ad> Ads { get; set; }
    }
}
