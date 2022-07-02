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

        public void CreateItem(Item item)
        {
            db.AddItem(item);
        }

        public void DeleteItem(Guid id)
        {
            db.RemoveItem(id);
        }

        public Item GetItem(Guid id)
        {
            var item = db.GetItem(id);
            return item;
        }

        public IEnumerable<Item> GetItems()
        {
            return db.Items;
        }

        public void UpdateItem(Item item)
        {
            db.ChangeItem(item);
        }
    }
}
