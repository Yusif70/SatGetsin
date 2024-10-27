using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Data.Context;

namespace Advertisement.Data.Repositories.Concretes
{
    public class AdRepository : Repository<Ad>, IAdRepository
    {
        public AdRepository(AppDbContext context) : base(context) { }
    }
}
