using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toplivo.Web.Services
{
    public interface IToplivoService<T> where T:class
    {
        //получение объекта по индексу
        Task<T> GetAsync(int id);
        //получить коллекцию из всех объектов
        Task<IEnumerable<T>> GetAllAsync();
        //создание объекта
        Task<T> CreateAsync(T item);
        //обновление объекта
        Task<T> UpdateAsync(T item);
        //удалить объект
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> Find(Func<T, bool> predicate);//получить коллекцию объектов, удовлетворяющих заданному условию

        //получить коллекцию pageSize объектов расположенных на page странице и удовлетворяющих заданному условияю
        Task<PagedCollections<T>> GetNumberItems(Func<T, bool> predicate, int page = 1, string sort = "", int pageSize = 18);
    }
}
