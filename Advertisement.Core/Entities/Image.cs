using Advertisement.Core.Entities.Base;

namespace Advertisement.Core.Entities
{
    public class Image : BaseEntity
    {
        public string Name {  get; set; }
        public string Url { get; set; }
        public Guid AdId { get; set; }
        public Ad Ad { get; set; }
    }
}
