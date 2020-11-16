using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Toplivo.Web.Common;
using Toplivo.Web.Models.Toplivo;
using Toplivo.Web.PL;
using Toplivo.Web.Services;
using Toplivo.Web.ViewModels;

namespace Toplivo.Web.Controllers
{
    public class TanksController : Controller
    {
        private readonly IToplivoService<Tank> _tankService;
        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata; 

        public TanksController(IToplivoService<Tank> tankService)
        {
            _tankService = tankService;
            
        }

        public async Task<ActionResult> Index(string sortOrder, PageInfo pageinfo)
        {
           
            int page = pageinfo.PageNumber;
            string searchString = pageinfo.SearchString ?? "";
            string nameSortParm = String.IsNullOrEmpty(sortOrder) ? "TankType" : ""; 
            string name2SortParm = sortOrder == "TankMaterial" ? "TankMaterial_desc" : "TankMaterial";

            transferdata = (TransferData)Session["TransferData"];
            transferdata.TankPage = page; transferdata.strTankTypeFind = searchString;
            Session["TransferData"] = transferdata;

            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "TankType" : "";
            //ViewBag.Name2SortParm = sortOrder == "TankMaterial" ? "TankMaterial_desc" : "TankMaterial";

            PagedCollections<Tank> pagedcollection =  await _tankService.GetNumberItems(t => (t.TankType.Contains(searchString)), page, sortOrder);
            pagedcollection.PageInfo.SearchString = searchString;
            pagedcollection.PageInfo.NameSortParm = nameSortParm;
            pagedcollection.PageInfo.Name2SortParm = name2SortParm;

            return View(pagedcollection);
            //return View(tanks.ToList());

        }

        // GET: Tanks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return await RedirectToIndex();
            Tank tank = await _tankService.GetAsync((int)id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }


        public ActionResult Create()
        {
            //ViewBag.RedurectUrl = Url.Action("Index", "Tanks");
            return View();
        }


        // POST: Tanks/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        //как пример
        //public ActionResult Create([Bind(Include = "LastName, FirstMidName, EnrollmentDate")]Student student)
        //сво-во Bind какие св-ва нужно исключить и метод Includ показывает какие поля из модели нуждаются в привязке
        //UpdateModel - явная привязка моделей
        //Предупреждение безопасности — ValidateAntiForgeryToken атрибут позволяет запретить подделки запросов между сайтами атак.
        //Он требует соответствующего Html.AntiForgeryToken()
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tank tank, string redirectUrl, HttpPostedFileBase upload)
        {
            if (!ModelState.IsValid)
            {
                return View(tank);
            }

            //string filename= await CreateWithPicture(tank, upload);
            await _tankService.CreateAsync(tank);
            //return RedirectToAction("Edit", new { id = tank.TankID });
            return RedirectToLocal(redirectUrl);
        }

        public async Task<ActionResult> Delete(int? id)
        {
     
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tank tank = await _tankService.GetAsync((int)id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }

        // POST: Tanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _tankService.DeleteAsync(id);
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { TankID = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
            
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Tanks");
        }


        public async Task<ActionResult> RedirectToIndex()
        {
            transferdata = (TransferData)Session["TransferData"];
            int page = transferdata.TankPage;
            string searchstring = transferdata.strTankTypeFind ?? "";

            PagedCollections<Tank> pagedcollection = await _tankService.GetNumberItems(t => (t.TankType.Contains(searchstring)), page, "");
            pagedcollection.PageInfo.SearchString = searchstring;
 
            return View("Index", pagedcollection);
        }

        //сохранение картинки в папке Image в приложении
        public async Task<string> CreateWithPicture(Tank tank, HttpPostedFileBase upload)
        {
            string fileName = "";
            if (upload != null)
            {
                // формируем имя файла
                fileName = "/Images/" + tank.TankID.ToString() + System.IO.Path.GetExtension(upload.FileName);
                
                // сохраняем файл в папку Images в приложении
                upload.SaveAs(System.Web.HttpContext.Current.Server.MapPath(fileName));
            }
            tank.TankPicture = fileName;
            await _tankService.CreateAsync(tank); //CreateAsync(tank);
            return fileName;

        }
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
        // GET: Tanks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return await RedirectToIndex();
            Tank tank = await _tankService.GetAsync((int)id);
            if (tank == null)
            {
                return HttpNotFound();
            }
            return View(tank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Tank tank, HttpPostedFileBase upload, string redirectUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(tank);
            }

            var tank1 = await _tankService.GetAsync(tank.TankID);
            if (tank != null)
            {

                //await _tankService.UpdateAsync(tank);
                string filename = await UpdateWithPicture(tank, upload);
            }

            return RedirectToLocal(redirectUrl);
        }

    }
}