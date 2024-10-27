using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Data.Context;

namespace Advertisement.Data.Repositories.Concretes
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(AppDbContext context) : base(context) { }
    }
}
