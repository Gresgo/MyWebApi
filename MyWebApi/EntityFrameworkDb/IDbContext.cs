using Microsoft.EntityFrameworkCore;
using MyWebApi.Entities;

namespace MyWebApi.EntityFrameworkDb
{
    public interface IDbContext
    {
        DbSet<Item> Items { get; }

        /// <summary>
        /// Добавление предмета в БД.
        /// </summary>
        /// <param name="item">Предмет для добавления.</param>
        /// <returns>True - если удалось добавить, false - если предмет с таким Id уже существует.</returns>
        Task<bool> AddItemAsync(Item item);
        Task<bool> ChangeItemAsync(Item item);
        /// <summary>
        /// Получить предмет по Id.
        /// </summary>
        /// <param name="id">Id предмета.</param>
        /// <returns></returns>
        Task<Item?> GetItemAsync(Guid id);
        /// <summary>
        /// Удаление предмета из БД.
        /// </summary>
        /// <param name="id">Id предмета для удаления.</param>
        /// <returns>True - если предмет удалён, false - если предмет с таким Id не найден.</returns>
        Task<bool> RemoveItemAsync(Guid id);
        /// <summary>
        /// Возвращает все предметы из таблицы Items.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Item>> GetAllItemsAsync();
    }
}