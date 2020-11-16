using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Toplivo.Web.Common;
using Toplivo.Web.Models.Toplivo;
using Toplivo.Web.Repositories;
using System.Data.Entity;

namespace Toplivo.Web.Services
{
    public class OperationService : IToplivoService<Operation>
    {
        private readonly IToplivoRepository<Operation> _operationRepository;
        private ToplivoContext db = new ToplivoContext();

        public OperationService(IToplivoRepository<Operation> operationRepository)
        {
            _operationRepository = operationRepository;
        }

        public async Task<Operation> CreateAsync(Operation item)
        {
            return await _operationRepository.CreateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
             await _operationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Operation>> Find(Func<Operation, bool> predicate)
        {
            
                var result = db.Set<Operation>().Include(o => o.Fuel).Include(o => o.Tank).Where(predicate).ToList();
                return await Task.FromResult(result);
            
        }

        public async Task<IEnumerable<Operation>> GetAllAsync()
        {

           
                var result = db.Set<Operation>().Include(o => o.Fuel).Include(o => o.Tank).OrderBy(o => o.OperationID).ToList(); 
                return await Task.FromResult(result);
           

 
        }

        public async Task<Operation> GetAsync(int id)
        {
            Operation result;
            //using (var toplivoContext = new ToplivoContext())
            //{

                // result = toplivoContext.Set<Operation>().Find(id);
                result = await Task.FromResult(db.Operations.FirstOrDefault(a => a.OperationID == id));

            //}

            return result;
        }

 

        public async Task<PagedCollections<Operation>> GetNumberItems(Func<Operation, bool> predicate, int page = 1, string sort = "", int pageSize = 18)
        {
            var operations =  await GetAllAsync();
            operations = operations.Where(predicate);
            int totalitems = operations.Count();
            if ((int)Math.Ceiling((decimal)totalitems / pageSize) < page) { page = 1; };
            if (page != 0)
            {
                operations = operations.Skip((page - 1) * pageSize).Take(pageSize);
            };
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalitems };
            PagedCollections<Operation> viewoperations = new PagedCollections<Operation> { PageInfo = pageInfo, PagedItems = operations };
            return viewoperations;

        }

        public async Task<Operation> UpdateAsync(Operation item)
        {
            return await _operationRepository.UpdateAsync(item);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



    }
}