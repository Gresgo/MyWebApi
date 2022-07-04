using MyWebApi.Dtos;
using MyWebApi.Entities;

namespace MyWebApi
{
    public static class Extentions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}
