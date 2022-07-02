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

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItem(Guid id)
        {
            return items.Where(x => x.Id == id).SingleOrDefault();
        }

        public void CreateItem(Item item)
        {
            items.Add(item);
        }

        public void UpdateItem(Item item)
        {
            var index = items.FindIndex(x => x.Id == item.Id);
            items[index] = item;
        }

        public void DeleteItem(Guid id)
        {
            var index = items.FindIndex(x => x.Id == id);
            items.RemoveAt(index);
        }
    }
}
