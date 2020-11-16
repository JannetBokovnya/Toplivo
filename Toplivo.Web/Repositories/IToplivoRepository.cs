using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toplivo.Web.Repositories
{
    public interface IToplivoRepository<T> where T : class
    {
        //получение объекта по индексу
        Task<T> GetAsync(int id);
        //получить коллекцию из всех объектов
        Task<IEnumerable<T>> GetAllAsync();
        //получить коллекцию из всех объектов
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
        //создание объекта
        Task<T> CreateAsync(T item);
        //обновление объекта
        Task<T> UpdateAsync(T item);
        //удалить объект
        Task DeleteAsync(int id);

        //получить коллекцию pageSize объектов расположенных на page странице и удовлетворяющих заданному условияю
        Task<Services.PagedCollections<T>> GetNumberItems(Func<T, bool> predicate, int page = 1, string sort = "", int pageSize = 18);

    }
}
