using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Toplivo.Web.Common;
using Toplivo.Web.Models.Toplivo;
using Toplivo.Web.Repositories;

namespace Toplivo.Web.Services
{
    //Обычно сюда добавляется какая то бизнес логика
    //выборки, правила и т.д.
    public class TankService : IToplivoService<Tank>
    {
        private readonly IToplivoRepository<Tank> _tankRepository;

        public TankService(IToplivoRepository<Tank> tankRepository)
        {
            _tankRepository = tankRepository;
        }

        public async Task<Tank> CreateAsync(Tank item)
        {
            return await _tankRepository.CreateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _tankRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Tank>> GetAllAsync()
        {
            return await _tankRepository.GetAllAsync();
        }

        public async Task<Tank> GetAsync(int id)
        {
            return await _tankRepository.GetAsync(id);
        }

        public async Task<Tank> UpdateAsync(Tank item)
        {
            return await _tankRepository.UpdateAsync(item);
        }

        
        public async Task<string> CreateWithPicture(Tank tank, HttpPostedFileBase upload)
        {
            string fileName = "";
            if (upload != null)
            {
                // формируем имя файла
                fileName = "/Images/" + tank.TankID.ToString() + System.IO.Path.GetExtension(upload.FileName);
                // сохраняем файл в папку Images в приложении
                upload.SaveAs(HttpContext.Current.Server.MapPath(fileName));
            }
            tank.TankPicture = fileName;
            await _tankRepository.CreateAsync(tank); //CreateAsync(tank);
            return fileName;

        }
        public async Task<string> UpdateWithPicture(Tank tank, HttpPostedFileBase upload)
        {
            string fileName = "";
            if (upload != null)
            {
                // формируем имя файла
                fileName = "/Images/" + tank.TankID.ToString() + System.IO.Path.GetExtension(upload.FileName);
                // сохраняем файл в папку Images в приложении
                upload.SaveAs(HttpContext.Current.Server.MapPath(fileName));
                tank.TankPicture = fileName;
            }
            await _tankRepository.UpdateAsync(tank);
            return fileName;
        }

        public async Task<PagedCollections<Tank>> GetNumberItems(Func<Tank, bool> predicate, int page = 1, string sort = "", int pageSize = 18)
        {
            var tanks = await _tankRepository.GetAllAsync();

            int totalitems = tanks.Where(predicate).Count();
            if ((int)Math.Ceiling((decimal)totalitems / pageSize) < page) { page = 1; };
            switch (sort)
            {
                case "TankType":
                    tanks = tanks.Where(predicate).OrderByDescending(s => s.TankType).Skip((page - 1) * pageSize).Take(pageSize);
                    break;

                case "TankMaterial":
                    tanks = tanks.Where(predicate).OrderBy(s => s.TankMaterial).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case "TankMaterial_desc":
                    tanks = tanks.Where(predicate).OrderByDescending(s => s.TankMaterial).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                default:
                    tanks = tanks.Where(predicate).OrderBy(s => s.TankType).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
            }

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalitems };
            PagedCollections<Tank> viewtanks = new PagedCollections<Tank> { PageInfo = pageInfo, PagedItems = tanks };
            return viewtanks;
        }

        public IEnumerable<Tank> GetAllAsync1()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tank>> Find(Func<Tank, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}