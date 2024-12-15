using SatGetsin2.Core.Entities;
using SatGetsin2.Core.Repositories.Abstractions;
using SatGetsin2.Data.Context;

namespace SatGetsin2.Data.Repositories.Concretes
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
