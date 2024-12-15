using SatGetsin2.Core.Entities.Base;
using SatGetsin2.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SatGetsin2.Core.Entities
{
    public partial class Ad : BaseEntity
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Whatsapp { get; set; }
    }
    public partial class Ad
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public ProductState State { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public List<Image> Images { get; set; }
        public AdLevel Level { get; set; }
        public DateTime ExpireDate { get; set; }
        public ProductStatus Status { get; set; }
        public List<Favorite> FavoritedByUsers { get; set; }
    }
}
