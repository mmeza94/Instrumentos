using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio.Models.DataAccess;
using Laboratorio.Models;
using System.Resources;
using System.Globalization;

namespace Laboratorio.Controllers
{
    public class ToolController : Controller
    {
        //
        // GET: /Tool/

        public ActionResult Index(string validation, string tableError)
        {
            List<ToolModel> tm = DataAccess.GetToolsWithToolKitList();
            if (tm.Count == 0)
            {
                ViewBag.NoRecords = true;
            }
            else
            {
                ViewBag.NoRecords = false;
            }
            
            List<ToolTypeModel> ttm = DataAccess.GetToolTypes();
            List<SelectListItem> sli = new List<SelectListItem>();

            foreach (ToolTypeModel t in ttm)
            {
                SelectListItem s = new SelectListItem();
                s.Text = t.Name;
                s.Value = t.Name;
                sli.Add(s);
            }

            ViewBag.Types = sli;

            ViewBag.ValidationMessage = validation;
            ViewBag.TableError = tableError;
            FillFlag(tm);
            return View(tm);
        }


        private void FillFlag(List<ToolModel> tm)
        {
            foreach (var item in tm)
            {

                


                var flag = item.ExpirationDate.Date.Subtract(DateTime.Now.Date).Days;
                //var flag = DateTime.Now.Subtract(item.ExpirationDate).Days;

                if (flag <= 0)
                    item.ExpirationFlag = "0";//expirado
                if (flag <= 7 && flag >0)
                    item.ExpirationFlag = "1";//proximo a expirar
                if (flag > 7)
                    item.ExpirationFlag = "2";//todo bien

            }
        }


        //
        // POST: /Tool/InsTool
        public ActionResult InsertTool(string code, string typeName, DateTime expirationDate,  DateTime calibrationDate)
        {
            ResourceManager rs = new ResourceManager(typeof(Laboratorio.Messages));
            string msg = String.Empty;

            if (code.Equals(String.Empty))
            {
                msg += rs.GetString("ValidationToolCodeMissing");
            }
            else if (code.Length > 20)
            {
                msg += rs.GetString("ValidationToolCodeTooLong");
            }

            if (typeName.Equals(string.Empty))
            {
                msg += "\n" + rs.GetString("ValidationTypeMissing");
            }

            if (DateTime.Compare(expirationDate, calibrationDate) < 0)
            {
                msg += rs.GetString("ValidationExpirationDate");
            }

            if (!msg.Equals(String.Empty))
            {
                return RedirectToAction("Index", "Tool", new { validation=msg });
            }

            int id = DataAccess.InsertTool(code, typeName, expirationDate, calibrationDate);

            if (id < 0)
            {
                ViewBag.ErrorCode = id;
                return View("Error");
            }
            else
            {
                DataAccess.InsertLogEntry(User.Identity.Name,
                    String.Format("Instrumento creado: \"{0}\" de tipo \"{1}\" con fecha de calibración \"{2}\" y fecha de expiración \"{3}\"",
                    code, typeName, calibrationDate.ToShortDateString(), expirationDate.ToShortDateString()));
                return RedirectToAction("Index");
            }
        }

        public ActionResult DeleteTool(string code)
        {
            int result = DataAccess.DeleteTool(code);

            if (result < 0)
            {
                ViewBag.ErrorCode = result;
                return View("Error");
            }
            else
            {
                DataAccess.DeactivateToolFromToolKitCatalog(code);
                DataAccess.InsertLogEntry(User.Identity.Name,
                    String.Format("Instrumento dado de baja: \"{0}\"", code));
                return RedirectToAction("Index");
            }
                
        }

        public ActionResult UpdateToolPage(string code, string validation)
        {
            List<ToolModel> tm = DataAccess.GetToolsWithToolKitList();

            ViewBag.ValidationMessage = validation;

            return View(tm.Single(s => s.Code.Equals(code)));
        }

        public ActionResult UpdateTool(string code, DateTime calibrationDate, DateTime expirationDate, string previousCalibration, string previousExpiration)
        {
            
            if (DateTime.Compare(expirationDate, calibrationDate) < 0)
            {
                ResourceManager rs = new ResourceManager(typeof(Laboratorio.Messages));
                string msg = rs.GetString("ValidationExpirationDate");
                return RedirectToAction("UpdateToolPage", "Tool", new { validation = msg, code=code });
            }

            int result = DataAccess.UpdateTool(code, calibrationDate, expirationDate);

            if (result < 0)
            {
                return RedirectToAction("UpdateToolPage", "Tool" ,new { code= code});
            }
            else
            {
                DataAccess.InsertLogEntry(User.Identity.Name,
                    String.Format("Instrumento recalibrado: \"{0}\". Fecha previa de calibracion \"{1}\", fecha previa de expiración \"{2}\". Fecha de calibración nueva \"{3}\", fecha de expiración nueva \"{4}\"",
                    code, previousCalibration, previousExpiration, calibrationDate.ToShortDateString(), expirationDate.ToShortDateString()));
                return RedirectToAction("Index", "Tool");
            }
        }
    }
}
