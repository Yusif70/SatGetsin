using Advertisement.Core.Entities.Base;
using Advertisement.Core.Enums;
using System.Text.Json.Serialization;

namespace Advertisement.Core.Entities
{
    public partial class Ad : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Whatsapp { get; set; }
    }
    public partial class Ad
    {
        public string Name { get; set; }
        [JsonIgnore]
        public Guid CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        //public Guid SubCategoryId { get; set; }
        //public Category SubCategory { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public ProductState State { get; set; }
        //public bool IsByAgreement { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public string VideoLink { get; set; }
        public List<Image> Images { get; set; }
        public AdLevel Level { get; set; }
    }
}
