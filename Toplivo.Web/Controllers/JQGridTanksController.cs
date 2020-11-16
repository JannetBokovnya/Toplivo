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
    public class JQGridTanksController : Controller
    {
        private readonly IToplivoService<Tank> _tankService;
        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata;
        private ToplivoContext toplivoContext = new ToplivoContext();
        // GET: JQGridTanks

        public JQGridTanksController(IToplivoService<Tank> tankService)
        {
            _tankService = tankService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetTanks(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            transferdata.FuelPage = page;
            transferdata.strFuelTypeFind = searchString;
            Session["TransferData"] = transferdata;
            sord = (sord == null) ? "" : sord;

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            IEnumerable<Tank> tanks1;
            tanks1 = await _tankService.GetAllAsync();

            var tanks = tanks1.Select(
                    t => new
                    {
                        t.TankID,
                        t.TankType,
                        t.TankVolume,
                        t.TankWeight,
                        t.TankMaterial,
                        t.TankPicture
                    });
            if (_search)
            {
                switch (searchField)
                {
                    case "TankType":
                        tanks = tanks.Where(t => t.TankType.Contains(searchString));
                        break;
                    case "TankMaterial":
                        tanks = tanks.Where(t => t.TankMaterial.Contains(searchString));
                        break;
                }
            }
            int totalRecords = tanks.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            string sortOrder = sidx + " " + sord.ToUpper();
            switch (sortOrder)
            {
                case "TankMaterial ASC":
                    tanks = tanks.OrderBy(s => s.TankMaterial);
                    break;
                case "TankType ASC":
                    tanks = tanks.OrderBy(s => s.TankType);
                    break;
                case "TankMaterial DESC":
                    tanks = tanks.OrderByDescending(s => s.TankMaterial);
                    break;
                default:
                    tanks = tanks.OrderByDescending(s => s.TankType);
                    break;
            }

            tanks = tanks.Skip(pageIndex * pageSize).Take(pageSize);


            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = tanks
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> Edit(Tank Model)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    await _tankService.UpdateAsync(Model);
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


        [HttpPost]
        public async Task<string> Upload(Tank tank)
        {
            string fileName = "";

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {

                    // формируем имя файла
                    fileName = await UpdateWithPicture(tank, upload);
                    return "файл загружен";

                }

            }
            tank.TankPicture = fileName;
            await _tankService.UpdateAsync(tank); //CreateAsync(tank);
            return "данные обновлены";
        }

        //[HttpPost]
        //public string Upload(Tank tank)
        //{
        //    string fileName = "";

        //    foreach (string file in Request.Files)
        //    {
        //        var upload = Request.Files[file];
        //        if (upload != null)
        //        {

        //            fileName = unitOfWork.Tanks.UpdateWithPicture(tank, upload);
        //            unitOfWork.Tanks.Save();
        //            return "файл загружен";

        //        }

        //    }
        //    unitOfWork.Tanks.Update(tank);
        //    unitOfWork.Tanks.Save();
        //    return "данные обновлены";
        //}

        //загрузка картинки из папки на форму
        public async Task<string> UpdateWithPicture(Tank tank, HttpPostedFileBase upload)
        {
            string fileName = "";
            if (upload != null)
            {
                // формируем имя файла
                fileName = "/Images/" + tank.TankID.ToString() + System.IO.Path.GetExtension(upload.FileName);

                // сохраняем файл в папку Images в приложении
                upload.SaveAs(System.Web.HttpContext.Current.Server.MapPath(fileName));
                tank.TankPicture = fileName;
            }
            await _tankService.UpdateAsync(tank);
            return fileName;
        }
        public async Task<string> Delete(string id)
        {
            await _tankService.DeleteAsync(Convert.ToInt32(id));
            return "Запись удалена";
        }

        //public string UpdateWithPicture(Tank tank, HttpPostedFileBase upload)
        //{
        //    string fileName = "";
        //    if (upload != null)
        //    {
        //        // формируем имя файла
        //        fileName = "/Images/" + tank.TankID.ToString() + System.IO.Path.GetExtension(upload.FileName);
        //        // сохраняем файл в папку Images в приложении
        //        upload.SaveAs(HttpContext.Current.Server.MapPath(fileName));
        //        tank.TankPicture = fileName;
        //    }
        //    Update(tank);
        //    return fileName;
        //}

        ////сохранение картинки в папке Image в приложении
        //public async Task<string> CreateWithPicture(Tank tank, HttpPostedFileBase upload)
        //{
        //    string fileName = "";
        //    if (upload != null)
        //    {
        //        // формируем имя файла
        //        fileName = "/Images/" + tank.TankID.ToString() + System.IO.Path.GetExtension(upload.FileName);

        //        // сохраняем файл в папку Images в приложении
        //        upload.SaveAs(System.Web.HttpContext.Current.Server.MapPath(fileName));
        //    }
        //    tank.TankPicture = fileName;
        //    await _tankService.CreateAsync(tank); //CreateAsync(tank);
        //    return fileName;

        //}
    }
}