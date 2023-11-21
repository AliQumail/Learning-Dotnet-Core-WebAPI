using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage ="Max of 100 characters is allowed")]
        public string Name { get; set; }
        
        [MaxLength(1000, ErrorMessage = "Max of 1000 characters is allowed")]
        public string Description { get; set; }
        
        [Required]
        [Range(0,50)]
        public double LengthInKm { get; set; }
        
        public string? WalkImageUrl { get; set; }
        
        [Required]
        public Guid DifficultyId { get; set; }
        
        [Required]
        public Guid RegionId { get; set; }
    }
}
