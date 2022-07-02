using Microsoft.EntityFrameworkCore;
using MyWebApi.Entities;
using MyWebApi.EntityFrameworkDb;

namespace MyWebApi.Repositories
{
    public class DbItemsRepository : IItemsRepository
    {
        IDbContext db;

        public DbItemsRepository()
        {
            db = new EfDbContext();
        }

        public async Task<bool> CreateItemAsync(Item item)
        {
            return await db.AddItemAsync(item);
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            return await db.RemoveItemAsync(id);
        }

        public async Task<Item?> GetItemAsync(Guid id)
        {
            var item = await db.GetItemAsync(id);
            return item;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            var items = await db.GetAllItemsAsync();
            return await Task.FromResult(items);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            return await db.ChangeItemAsync(item);
        }
    }
}
