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
    public class HomeController : Controller
    {//Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata = new TransferData { TankPage = 1, strTankTypeFind = "", FuelPage = 1, strFuelTypeFind = "",  OperationPage = 1 };
        private readonly IToplivoService<Operation> _operationService;
        private ToplivoContext db = new ToplivoContext();

        public HomeController(IToplivoService<Operation> operationService)
        {
            _operationService = operationService;
        }

        public async Task<ActionResult> Index(int pagesize = 10)
        {
            //Инициализация временных переменных сессии для использования разными объектами
            Session["TransferData"] = transferdata;
            int page = transferdata.OperationPage;
            ViewBag.NumberOperations = pagesize;
            //Получаем из БД  pagesize объектов Operation, при этом будут подгружаться данные из Tank и Fuel
            PagedCollections<Operation> pagedcollection = await _operationService.GetNumberItems(t => true, page, "", pagesize);
            // передаем все объекты в динамическое свойство Operations в ViewBag
            ViewBag.Operations = pagedcollection.PagedItems;
            ViewBag.PageInfo = pagedcollection.PageInfo;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Топливная база";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}