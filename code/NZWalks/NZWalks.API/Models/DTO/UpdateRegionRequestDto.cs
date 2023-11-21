using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Max of 100 characters are allowed")]
        public string Name { get; set; }
        [Required]
        [MaxLength(3, ErrorMessage = "Max of 3 characters are allowed")]
        [MinLength(3, ErrorMessage = "Min of 3 characters are allowed")]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
