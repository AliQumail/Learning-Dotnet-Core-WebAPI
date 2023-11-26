using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository iWalkRepository; 
        public WalksController(IMapper _mapper, IWalkRepository _walkRepository)
        {
            mapper = _mapper;
            iWalkRepository = _walkRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateAsync([FromBody] AddWalkRequestDto addWalkRequestDto) 
        {
            if (ModelState.IsValid)
            {
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                walkDomainModel = await iWalkRepository.CreateAsync(walkDomainModel);
                var walkDto = mapper.Map<WalkDto>(walkDomainModel);
                return Ok(walkDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending, [FromQuery] int pageNumber, [FromQuery] int pageSize) 
        {
            var walksDomainModel = await iWalkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            var walksDto = mapper.Map<List<WalkDto>>(walksDomainModel);
            return Ok(walksDto); 
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var walkDomainModel = await iWalkRepository.GetById(id);
            if (walkDomainModel == null) return NotFound(); 
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto) 
        {
            if (ModelState.IsValid)
            {
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
                walkDomainModel = await iWalkRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null) return NotFound();
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
           var walkDomainModel = await iWalkRepository.DeleteAsync(id);
            if (walkDomainModel == null) return NotFound();
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

       
    }
}
