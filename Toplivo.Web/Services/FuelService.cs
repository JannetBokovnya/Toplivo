using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Toplivo.Web.Common;

using Toplivo.Web.Models.Toplivo;
using Toplivo.Web.Repositories;
using System.Data.Entity;



namespace Toplivo.Web.Services
{
    public class FuelService : IToplivoService<Fuel>
    {
        private readonly IToplivoRepository<Fuel> _fuelRepository;

        public FuelService(IToplivoRepository<Fuel> fuelRepository)
        {
            _fuelRepository = fuelRepository;
        }


        public async Task<Fuel> CreateAsync(Fuel item)
        {
            return await _fuelRepository.CreateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _fuelRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<Fuel>> Find(Func<Fuel, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Fuel>> GetAllAsync()
        {
            return await _fuelRepository.GetAllAsync(); ;
           
            //using (var toplivoContext = new ToplivoContext())
            //{
            //    var result = toplivoContext.Fuels.SqlQuery("Select * from dbo.Fuel").ToList();
            //    return await Task.FromResult(result);
            //}
        }

        public IEnumerable<Fuel> GetAllAsync1()
        {
            throw new NotImplementedException();
        }

        public async Task<Fuel> GetAsync(int id)
        {
            return await _fuelRepository.GetAsync(id);
        }

        public async Task<PagedCollections<Fuel>> GetNumberItems(Func<Fuel, bool> predicate, int page = 1, string sort = "", int pageSize = 18)
        {
            IEnumerable<Fuel> fuels;
            using (var toplivoContext = new ToplivoContext())
            {
                fuels = await Task.FromResult(toplivoContext.Fuels.SqlQuery("Select * from Fuels").ToList());
                
            }

            //var fuels = await _fuelRepository.GetAllAsync();
            fuels = fuels.Where(predicate).OrderBy(o => o.FuelType);
            int totalitems = fuels.Count();
            //int totalitems = fuels.Where(predicate).Count();
            if ((int)Math.Ceiling((decimal)totalitems / pageSize) < page) { page = 1; };
            if (page != 0)
            {
                fuels = fuels.Skip((page - 1) * pageSize).Take(pageSize);
            };
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalitems };
            PagedCollections<Fuel> viewtanks = new PagedCollections<Fuel> { PageInfo = pageInfo, PagedItems = fuels };
            return viewtanks;
        }

        public async Task<Fuel> UpdateAsync(Fuel item)
        {
            return await _fuelRepository.UpdateAsync(item);
        }
    }
}