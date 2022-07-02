using System.ComponentModel.DataAnnotations;

namespace MyWebApi.DTOs
{
    public record UpdateItemDTO
    {
        [Required]
        public string? Name { get; init; }

        [Required]
        [Range(0, 10000000)]
        public decimal Price { get; init; }
    }
}
