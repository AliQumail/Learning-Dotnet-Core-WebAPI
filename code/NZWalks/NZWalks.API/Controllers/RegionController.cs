using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        public RegionController(NZWalksDbContext _dbContext, IRegionRepository _regionRepository)
        {
            dbContext = _dbContext;
            regionRepository = _regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var regions = await regionRepository.GetAll();
            var regionsResponse = new List<RegionDto>(); 
            foreach (var region in regions)
            {
                regionsResponse.Add(
                    new RegionDto()
                    {
                        Id = region.Id,
                        Name = region.Name,
                        Code = region.Code,
                        RegionImageUrl = region.RegionImageUrl,
                    }
                );
            }
            return Ok(regionsResponse);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(e => e.Id == id);
            if (region == null) return NotFound(); 

            var regionResponse = new RegionDto() {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
            };
            return Ok(regionResponse);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto regionRequest) {
            var regionDomainModel = new Region()
            {
                Name = regionRequest.Name,
                Code = regionRequest.Code,
                RegionImageUrl =regionRequest.RegionImageUrl,
            };
            await dbContext.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

            var regionResponseDto = new RegionDto() 
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name, 
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionResponseDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto regionRequest) {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(data => data.Id == id);

            if (regionDomainModel == null) return NotFound();

            regionDomainModel.Name = regionRequest.Name;
            regionDomainModel.Code = regionRequest.Code;
            regionDomainModel.RegionImageUrl = regionRequest.RegionImageUrl;

           await dbContext.SaveChangesAsync();


            var regionResponseDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionResponseDto);

        }
        
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(e => e.Id == id);
            if (regionDomainModel == null) return NotFound();
            
            dbContext.Regions.Remove(regionDomainModel);
            await dbContext.SaveChangesAsync();
            
            var regionResponseDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            
            return Ok(regionResponseDto);
        }
        

    }
}
