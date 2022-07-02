using MyWebApi.DTOs;
using MyWebApi.Entities;

namespace MyWebApi
{
    public static class Extentions
    {
        public static ItemDTO AsDto(this Item item)
        {
            return new ItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate,
            };
        }
    }
}
