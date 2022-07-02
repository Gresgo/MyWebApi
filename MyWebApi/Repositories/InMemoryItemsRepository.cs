using MyWebApi.Entities;

namespace MyWebApi.Repositories
{
    public class InMemoryItemsRepository : IItemsRepository
    {
        private readonly List<Item> items = new()
        {
            new Item() {Id = Guid.NewGuid(), Name = "Potion", Price = 10, CreatedDate = DateTime.UtcNow},
            new Item() {Id = Guid.NewGuid(), Name = "Big Potion", Price = 25, CreatedDate = DateTime.UtcNow},
            new Item() {Id = Guid.NewGuid(), Name = "Iron Shield", Price = 50, CreatedDate = DateTime.UtcNow},
            new Item() {Id = Guid.NewGuid(), Name = "Iron Sword", Price = 55, CreatedDate = DateTime.UtcNow}
        };

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(items);
        }

        public async Task<Item>? GetItemAsync(Guid id)
        {
            var item = items.Where(x => x.Id == id).SingleOrDefault();
            return await Task.FromResult(item);
        }

        public async Task<bool> CreateItemAsync(Item item)
        {
            if (items.SingleOrDefault(x => x.Id == item.Id || x.Name == item.Name) != null)
                return await Task.FromResult(false);

            items.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var index = items.FindIndex(x => x.Id == item.Id);
            if (index == -1)
                return await Task.FromResult(false);

            items[index] = item;
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            var index = items.FindIndex(x => x.Id == id);
            if (index == -1)
                return await Task.FromResult(false);

            items.RemoveAt(index);
            return await Task.FromResult(true);
        }
    }
}
