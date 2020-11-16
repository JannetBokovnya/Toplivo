using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toplivo.Web.Services;
using Toplivo.Web.Models.Toplivo;
using System.Threading.Tasks;
using Toplivo.Web.PL;
using PagedList;
using System.Net;
using System.Data.Entity.Infrastructure;
using Toplivo.Web.Common;

namespace Toplivo.Web.Controllers
{
    public class OperationsController : Controller
    {
        private readonly IToplivoService<Operation> _operationService;

        private readonly IToplivoService<Fuel> _fuelService;

        private readonly IToplivoService<Tank> _tankServise;

        private ToplivoContext db = new ToplivoContext();

        //Объект для передачи данных, отражающих выбор пользователя
        TransferData transferdata;
        int pageSize = 15;

        public OperationsController(IToplivoService<Operation> operationService, IToplivoService<Fuel> fuelService, IToplivoService<Tank> tankServise)
        {
            _operationService = operationService;
            _fuelService = fuelService;
            _tankServise = tankServise;

        }
        
        // GET: Operations
        public async Task<ActionResult> Index(string strTankTypeFind, string strFuelTypeFind, int page = 1)
        {
            transferdata = (TransferData)Session["TransferData"];


            if (strTankTypeFind == null) { strTankTypeFind = transferdata.strTankTypeFind ?? ""; };
            if (strFuelTypeFind == null) { strFuelTypeFind = transferdata.strFuelTypeFind ?? ""; };

            transferdata.OperationPage = page;
            transferdata.strTankTypeFind = strTankTypeFind;
            transferdata.strFuelTypeFind = strFuelTypeFind;
            Session["TransferData"] = transferdata;

            IEnumerable<Operation> operations =  await _operationService.Find(t => ((t.Tank.TankType.Contains(strTankTypeFind))) & (t.Fuel.FuelType.Contains(strFuelTypeFind)));
            return View(operations.ToPagedList(page, pageSize));
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return await RedirectToIndex();
            Operation operation = await _operationService.GetAsync((int)id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }


        // GET: Operations/Create
        public async Task<ActionResult> Create()
        {
            
            ViewBag.FuelID = new SelectList(await _fuelService.GetAllAsync(), "FuelID", "FuelType");
            ViewBag.TankID = new SelectList(await _tankServise.GetAllAsync(), "TankID", "TankType");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Operation operation, string redirectUrl, HttpPostedFileBase upload)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.FuelID = new SelectList(await _fuelService.GetAllAsync(), "FuelID", "FuelType");
                ViewBag.TankID = new SelectList(await _tankServise.GetAllAsync(), "TankID", "TankType");
                return View(operation);
            }
            await _operationService.CreateAsync(operation);
            return RedirectToAction("Index", "Operations");
        }

        public async Task<ActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operation operation = await _operationService.GetAsync((int)id);
            if (operation == null)
            {
                return HttpNotFound();
            }
            return View(operation);
        }


        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _operationService.DeleteAsync(id);
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { OperationID = id, saveChangesError = true });
            }
            return RedirectToAction("Index");

        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == -1) return await RedirectToIndex();
            Operation operation = await _operationService.GetAsync((int)id);
            if (operation == null)
            {
                return HttpNotFound();
            }

            ViewBag.FuelID = new SelectList(await _fuelService.GetAllAsync(), "FuelID", "FuelType", operation.FuelID);
            ViewBag.TankID = new SelectList(await _tankServise.GetAllAsync(), "TankID", "TankType", operation.TankID);
            return View(operation);
        }

 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Operation operation, string redirectUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(operation);
            }

            var operation1 = await _operationService.GetAsync(operation.FuelID);
            if (operation != null)
            {

                await _operationService.UpdateAsync(operation);

            }
            ViewBag.FuelID = new SelectList(await _fuelService.GetAllAsync(), "FuelID", "FuelType", operation.FuelID);
            ViewBag.TankID = new SelectList(await _tankServise.GetAllAsync(), "TankID", "TankType", operation.TankID);
            //return RedirectToLocal(redirectUrl);
            return await RedirectToIndex();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> RedirectToIndex()
        {
            transferdata = (TransferData)Session["TransferData"];
            int page = transferdata.OperationPage;
            string strTankTypeFind = transferdata.strTankTypeFind;
            string strFuelTypeFind = transferdata.strFuelTypeFind;
            IEnumerable<Operation> operations = await _operationService.Find(t => ((t.Tank.TankType.Contains(strTankTypeFind))) & (t.Fuel.FuelType.Contains(strFuelTypeFind)));

            return View("Index", operations.ToPagedList(page, pageSize));
        }
    }
}