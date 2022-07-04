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
        public async Task<Item?> GetItemAsync(Guid id)
        {
            var item = await Items.SingleOrDefaultAsync(x => x.Id == id);
            return item;
        }

        /// <summary>
        /// Добавление предмета.
        /// </summary>
        /// <param name="item">Предмет для добавления.</param>
        /// <returns>True - если удалось добавить, false - если предмет с таким Id уже существует.</returns>
        public async Task<bool> AddItemAsync(Item item)
        {
            var existingItem = await Items.SingleOrDefaultAsync(x => x.Id == item.Id || x.Name == item.Name);
            if (existingItem is not null)
                return await Task.FromResult(false);

            await Items.AddAsync(item);
            await SaveChangesAsync();
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Удаление предмета.
        /// </summary>
        /// <param name="id">Id предмета для удаления.</param>
        /// <returns>True - если предмет удалён, false - если предмет с таким Id не найден.</returns>
        public async Task<bool> RemoveItemAsync(Guid id)
        {
            var existingItem = await GetItemAsync(id);
            if (existingItem is null)
                return await Task.FromResult(false);

            Items.Remove(existingItem);
            await SaveChangesAsync();
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Изменить предмет с соответствующим Id на новый.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item">Предмет на который будет изменён существующий предмет.</param>
        /// <returns>True - если предмет успешно изменён, false - если предмет с таким Id не найден.</returns>
        public async Task<bool> ChangeItemAsync(Item item)
        {
            var existingItem = await GetItemAsync(item.Id);
            if (existingItem is null)
                return await Task.FromResult(false);

            Items.Remove(existingItem);
            await Items.AddAsync(item);

            await SaveChangesAsync();
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await Task.FromResult(Items);
        }
    }
}
