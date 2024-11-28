using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Data.Context;

namespace Advertisement.Data.Repositories.Concretes
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(AppDbContext context) : base(context) { }
    }
}
