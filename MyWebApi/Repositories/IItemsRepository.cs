using MyWebApi.Entities;

namespace MyWebApi.Repositories
{
    public interface IItemsRepository
    {
        Task<Item>? GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<bool> CreateItemAsync(Item item);
        Task<bool> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(Guid id);
    }
}