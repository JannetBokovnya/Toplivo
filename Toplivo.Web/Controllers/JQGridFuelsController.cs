using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Toplivo.Web.Common;
using Toplivo.Web.Models.Toplivo;
using Toplivo.Web.PL;
using Toplivo.Web.Services;

namespace Toplivo.Web.Controllers
{
    public class JQGridFuelsController : Controller
    {
        private readonly IToplivoService<Fuel> _fuelService;
        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata;
        private ToplivoContext toplivoContext = new ToplivoContext();

        public JQGridFuelsController(IToplivoService<Fuel> fuelService)
        {
            _fuelService = fuelService;
        }

        // GET: JQGridFuels
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetFuels(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            transferdata.FuelPage = page;
            transferdata.strFuelTypeFind = searchString;
            Session["TransferData"] = transferdata;
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            IEnumerable<Fuel> fuels3;
            fuels3 = await _fuelService.GetAllAsync();

            var fuels = fuels3.Select(
                        t => new
                        {
                            t.FuelID,
                            t.FuelType,
                            t.FuelDensity,
                        });
            if (_search)
            {
                switch (searchField)
                {
                    case "FuelType":
                        fuels = fuels.Where(t => t.FuelType.Contains(searchString));
                        break;

                }
            }

            int totalRecords = fuels.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                fuels = fuels.OrderByDescending(t => t.FuelType);
                fuels = fuels.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                fuels = fuels.OrderBy(t => t.FuelType);
                fuels = fuels.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = fuels
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<string> Create([Bind(Exclude = "FuelID")] Fuel Model)
        {

            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    await _fuelService.CreateAsync(Model);
                    msg = "Сохранено";
                }
                else
                {
                    msg = "Модель не прошла валидацию";
                }
            }
            catch (Exception ex)
            {
                msg = "Ошибка:" + ex.Message;
            }
            return msg;
        }

        public async Task<string> Edit(Fuel Model)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    await _fuelService.UpdateAsync(Model);
                    msg = "Сохранено";
                }
                else
                {
                    msg = "Модель не прошла валидацию";
                }
            }
            catch (Exception ex)
            {
                msg = "Ошибка:" + ex.Message;
            }
            return msg;
        }
        public async Task<string> Delete(string id)
        {
            await _fuelService.DeleteAsync(Convert.ToInt32(id));
            return "Запись удалена";
        }
    }
}