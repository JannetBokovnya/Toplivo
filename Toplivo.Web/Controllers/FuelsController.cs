using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using Toplivo.Web.Models.Toplivo;
using Toplivo.Web.Services;
using System.Net;
using Toplivo.Web.PL;

namespace Toplivo.Web.Controllers
{
    public class FuelsController : Controller
    {
        private readonly IToplivoService<Fuel> _fuelService;
        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata;

        public FuelsController(IToplivoService<Fuel> fuelService)
        {
            _fuelService = fuelService;

        }

        // GET: Fuels
        public async Task<ActionResult> Index(PageInfo pageinfo)
        {
            int page = pageinfo.PageNumber;
            string searchString = pageinfo.SearchString ?? "";
            
            transferdata = (TransferData)Session["TransferData"];
            transferdata.FuelPage = page; transferdata.strFuelTypeFind = searchString;
            Session["TransferData"] = transferdata;

           
            PagedCollections<Fuel> pagedcollection = await _fuelService.GetNumberItems(t => (t.FuelType.Contains(searchString)), page);
            pagedcollection.PageInfo.SearchString = searchString;
           
            return View(pagedcollection);
        }

        // GET: Fuels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return await RedirectToIndex();
            Fuel fuel = await _fuelService.GetAsync((int)id);
            if (fuel == null)
            {
                return HttpNotFound();
            }
            return View(fuel);
        }

        public async Task<ActionResult> RedirectToIndex()
        {
            transferdata = (TransferData)Session["TransferData"];
            int page = transferdata.FuelPage;
            string searchstring = transferdata.strFuelTypeFind ?? "";

            PagedCollections<Fuel> pagedcollection = await _fuelService.GetNumberItems(t => (t.FuelType.Contains(searchstring)), page, "");
            pagedcollection.PageInfo.SearchString = searchstring;

            return View("Index", pagedcollection);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Fuels");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Fuel fuel, string redirectUrl, HttpPostedFileBase upload)
        {
            if (!ModelState.IsValid)
            {
                return View(fuel);
            }
            await _fuelService.CreateAsync(fuel);
            //return RedirectToAction("Edit", new { id = fuel.FuelID });
            return RedirectToLocal(redirectUrl);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return await RedirectToIndex();
            Fuel fuel = await _fuelService.GetAsync((int)id);
            if (fuel == null)
            {
                return HttpNotFound();
            }
            return View(fuel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Fuel fuel, string redirectUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(fuel);
            }
          
            var fuel1 = await _fuelService.GetAsync(fuel.FuelID);
            if (fuel != null)
            {

                await _fuelService.UpdateAsync(fuel);
                
            }

            //return RedirectToLocal(redirectUrl);
            return  await RedirectToIndex();
        }

        public async Task<ActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fuel fuel = await _fuelService.GetAsync((int)id);
            if (fuel == null)
            {
                return HttpNotFound();
            }
            return View(fuel);
        }

        // POST: Tanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _fuelService.DeleteAsync(id);
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { FuelID = id, saveChangesError = true });
            }
            return RedirectToAction("Index");

        }
    }
}