using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository: IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<Region>> GetAll() 
        {
            var regions = await dbContext.Regions.ToListAsync();
            return regions;
        }
    }
}
