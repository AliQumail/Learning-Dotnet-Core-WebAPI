using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() 
        {
            // Region mappings
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Region, AddRegionRequestDto>().ReverseMap();
            CreateMap<Region, UpdateRegionRequestDto>().ReverseMap();

            // Walk mappings 
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Walk, AddWalkRequestDto>().ReverseMap();

            // Difficulty mappings
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            
        }
    }
}
