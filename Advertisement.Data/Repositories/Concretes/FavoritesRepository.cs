﻿using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Data.Context;

namespace Advertisement.Data.Repositories.Concretes
{
    public class FavoritesRepository : Repository<Favorites>, IFavoritesRepository
    {
        public FavoritesRepository(AppDbContext context) : base(context) { }
    }
}
