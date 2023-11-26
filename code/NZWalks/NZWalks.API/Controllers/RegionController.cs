using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System;
using System.Data;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper autoMapper;
        public RegionController(NZWalksDbContext _dbContext, IRegionRepository _regionRepository, IMapper _autoMapper)
        {
            dbContext = _dbContext;
            regionRepository = _regionRepository;
            autoMapper = _autoMapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll() 
        {
            var regionsDomainModel = await regionRepository.GetAllAsync();
            return Ok(autoMapper.Map<List<RegionDto>>(regionsDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            var regionDomainModel = await regionRepository.GetByIdAsync(id);
            if (regionDomainModel == null) return NotFound(); 
            return Ok(autoMapper.Map<RegionDto>(regionDomainModel));
        }

        [HttpPost]
        [ValidateModel]
        [Route("Create")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto regionRequest) {
                var regionDomainModel = autoMapper.Map<Region>(regionRequest);
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
                var regionResponseDto = autoMapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionResponseDto);   
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto regionRequest) {

            if (ModelState.IsValid)
            {
                var regionDomainModel = autoMapper.Map<Region>(regionRequest);
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null) return NotFound();
                var regionResponseDto = autoMapper.Map<RegionDto>(regionDomainModel);
                return Ok(regionResponseDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
          
        }
        
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if ( regionDomainModel == null ) return NotFound();
            return Ok(autoMapper.Map<RegionDto>(regionDomainModel));
        }
        

    }
}
