using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Toplivo.Web.Common;
using Toplivo.Web.Services;

namespace Toplivo.Web.Repositories
{
    public class ToplivoRepository<T> : IToplivoRepository<T> where T : class
    {
       public ToplivoRepository()
        {

        }

        //сохранение
        public async Task<T> CreateAsync(T item)
        {
            T result = null;

            using (var toplivoContext = new ToplivoContext())
            {
                result = toplivoContext.Set<T>().Add(item);
                await toplivoContext.SaveChangesAsync();
            }

            return result;
        }
        //удаление
        public async Task DeleteAsync(int id)
        {

            using (var toplivoContext = new ToplivoContext())
            {
                var t = await toplivoContext.Set<T>().FindAsync(id);
                if (t != null)
                {
                    toplivoContext.Entry(t).State = EntityState.Deleted;
                    //вариант
                    //toplivoContext.Set<T>().Remove(t);

                    await toplivoContext.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = new List<T>();

            using (var toplivoContext = new ToplivoContext())
            {
                result = await toplivoContext.Set<T>().ToListAsync();
            }

            return result;
        }


        public async Task<T> GetAsync(int id)
        {
            T result = null;
            //когда есть using до вызывается метод dispoze
            using (var toplivoContext = new ToplivoContext())
            {
                //result =  (toplivoContext.Set<T>().Find(id));
                result = await toplivoContext.Set<T>().FindAsync(id);
            }
            return result;
        }

        public async Task<T> UpdateAsync(T item)
        {
            using (var toplivoContext = new ToplivoContext())
            {
                toplivoContext.Entry(item).State= EntityState.Modified;
                await toplivoContext.SaveChangesAsync();
            }

            return item;
           
        }

        public Task<PagedCollections<T>> GetNumberItems(Func<T, bool> predicate, int page = 1, string sort = "", int pageSize = 18)
        {
            throw new NotImplementedException();
        }


        //Делегат Predicate<T>, как правило, используется для сравнения, 
        //сопоставления некоторого объекта T определенному условию. 
        //В качестве выходного результата возвращается значение true, если условие соблюдено, и false,
        //если не соблюдено
        //Еще одним распространенным делегатом является Func. Он возвращает результат действия и может принимать 
        //параметры. Он также имеет различные формы: 
        //от Func<out T>(), где T - тип возвращаемого значения, до Func<in T1, in T2,...in T16, out TResult>(), 
        //то есть может принимать до 16 параметров.
        //Возвращает коллекцию объектов, удовлетворяющих заданному условию
        public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
        {
            IEnumerable<T> result = null;

            using (var toplivoContext = new ToplivoContext())
            {
                result = toplivoContext.Set<T>().Where(predicate);
            }
            return await Task.FromResult(result);
        }

        ////Возвращает коллекцию объектов, удовлетворяющих заданному условию
        //public async Task<IEnumerable<T>> Find(Func<T, bool> predicate)
        //{
        //    using (var toplivoContext = new ToplivoContext())
        //    {
        //        var result = toplivoContext.Set<T>().Where(predicate);

               
        //        return await Task.FromResult(result);
        //    }
                
        //}
    }
}