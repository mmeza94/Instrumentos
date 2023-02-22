using Laboratorio.Models;
using Laboratorio.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratorio.Controllers
{
    public class DischargedToolsController: Controller
    {
        public ActionResult Index(string validation, string tableError)
        {
            List<ToolModel> tm = DataAccess.GetDeactivatedTools();
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

            return View(tm);
        }


        public ActionResult ActivateTool(string code)
        {
            int result = DataAccess.ActivateTool(code);

            if (result < 0)
            {
                ViewBag.ErrorCode = result;
                return View("Error");
            }
            else
                DataAccess.InsertLogEntry(User.Identity.Name,
                    String.Format("Instrumento dado de alta: \"{0}\"", code));
            return RedirectToAction("Index");
        }






    }
}