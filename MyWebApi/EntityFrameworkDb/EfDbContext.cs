using Microsoft.EntityFrameworkCore;
using MyWebApi.Entities;

namespace MyWebApi.EntityFrameworkDb
{
    public class EfDbContext : DbContext, IDbContext
    {
        public DbSet<Item> Items => Set<Item>();
        public EfDbContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MyWebApi.db");
        }

        /// <summary>
        /// Получить предмет по Id.
        /// </summary>
        /// <param name="id">Id предмета.</param>
        /// <returns></returns>
        public Item GetItem(Guid id)
        {
            var item = Items.FirstOrDefault(x => x.Id == id);
            return item;
        }

        /// <summary>
        /// Добавление предмета.
        /// </summary>
        /// <param name="item">Предмет для добавления.</param>
        /// <returns>True - если удалось добавить, false - если предмет с таким Id уже существует.</returns>
        public bool AddItem(Item item)
        {
            var existingItem = GetItem(item.Id);
            if(existingItem != null)
                return false;

            Items.Add(item);
            SaveChanges();
            return true;
        }

        /// <summary>
        /// Удаление предмета.
        /// </summary>
        /// <param name="id">Id предмета для удаления.</param>
        /// <returns>True - если предмет удалён, false - если предмет с таким Id не найден.</returns>
        public bool RemoveItem(Guid id)
        {
            var existingItem = GetItem(id);
            if (existingItem is null)
                return false;

            Items.Remove(existingItem);
            SaveChanges();
            return true;
        }

        /// <summary>
        /// Изменить предмет с соответствующим Id на новый.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item">Предмет на который будет изменён существующий предмет.</param>
        /// <returns>True - если предмет успешно изменён, false - если предмет с таким Id не найден.</returns>
        public bool ChangeItem(Item item)
        {
            var existingItem = GetItem(item.Id);
            if (existingItem is null)
                return false;

            existingItem = existingItem with
            {
                Name = item.Name,
                Price = item.Price
            };
            SaveChanges();
            return true;
        }
    }
}
