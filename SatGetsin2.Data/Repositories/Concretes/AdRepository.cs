using SatGetsin2.Core.Entities;
using SatGetsin2.Core.Repositories.Abstractions;
using SatGetsin2.Data.Context;

namespace SatGetsin2.Data.Repositories.Concretes
{
    public class AdRepository : Repository<Ad>, IAdRepository
    {
        public AdRepository(AppDbContext context) : base(context)
        {
        }
    }
}
