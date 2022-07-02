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
        bool AddItem(Item item);
        bool ChangeItem(Item item);
        /// <summary>
        /// Получить предмет по Id.
        /// </summary>
        /// <param name="id">Id предмета.</param>
        /// <returns></returns>
        Item GetItem(Guid id);
        /// <summary>
        /// Удаление предмета из БД.
        /// </summary>
        /// <param name="id">Id предмета для удаления.</param>
        /// <returns>True - если предмет удалён, false - если предмет с таким Id не найден.</returns>
        bool RemoveItem(Guid id);
    }
}