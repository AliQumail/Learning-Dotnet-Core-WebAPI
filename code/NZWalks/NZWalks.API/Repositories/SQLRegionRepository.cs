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
        public async Task<List<Region>> GetAllAsync() 
        {
            var regions = await dbContext.Regions.ToListAsync();
            return regions;
        }

        public async Task<Region?> GetByIdAsync(Guid id) 
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(e => e.Id == id);
            return region; 
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region) {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(e => e.Id == id);
            if (existingRegion == null) return null;

            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            return existingRegion;

        }

        public async Task<Region?> DeleteAsync(Guid id) 
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(e => e.Id == id);
            if (regionDomainModel == null) return null;

            dbContext.Regions.Remove(regionDomainModel);
            await dbContext.SaveChangesAsync();
            return regionDomainModel; 

        }
    }
}
