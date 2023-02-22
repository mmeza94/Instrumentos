using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio.Models.DataAccess;
using Laboratorio.Models;
using System.Resources;

namespace Laboratorio.Controllers
{
    public class ToolTypesController : Controller
    {
        //
        // GET: /ToolType/
        public ActionResult Index(string validation, string tableError)
        {
            List<ToolTypeModel> l;
            l = DataAccess.GetToolTypes();

            if (l.Count == 0)
            {
                ViewBag.NoRecords = true; 
            }
            else 
            {
                ViewBag.NoRecords = false; 
            }

            ViewBag.ValidationMessage = validation;
            ViewBag.TableError = tableError;

            return View(l);
        }

        //
        // POST: /InsertToolType/ HOLA MUNDO
        public ActionResult InsertToolType(string toolTypeName)
        {
            ResourceManager rs = new ResourceManager(typeof(Laboratorio.Messages));
            if (toolTypeName.Equals(String.Empty))
            {
                ViewBag.ValidationMessage = rs.GetString("ValidationToolTypeNameMissing");
                return RedirectToAction("Index", new { validation=ViewBag.ValidationMessage});
            }
            else if (toolTypeName.Length > 50)
            {
                ViewBag.ValidationMessage = rs.GetString("ValidationToolTypeNameTooLong");
                return RedirectToAction("Index", new { validation = ViewBag.ValidationMessage });
            }

            int id = DataAccess.InsertToolType(toolTypeName);

            if (id < 0)
            {
                ViewBag.ValidationMessage = rs.GetString("ErrorInsToolType");
                return RedirectToAction("Index", new { validation = ViewBag.ValidationMessage });
            }
            else
            {
                DataAccess.InsertLogEntry(User.Identity.Name,
                    String.Format("Tipo de instrumento creado: \"{0}\"", toolTypeName));
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /DeleteToolType/#id
        public ActionResult DeleteToolType(int id, string tipo)
        {
            int result = DataAccess.DeleteToolType(id);

            if (result < 0)
            {
                ResourceManager rs = new ResourceManager(typeof(Laboratorio.Messages));
                ViewBag.TableError = rs.GetString("ErrorDelToolType");
                return RedirectToAction("Index", new { tableError = ViewBag.TableError });
            }
            else
            {
                DataAccess.InsertLogEntry(User.Identity.Name,
                    String.Format("Tipo de instrumento eliminado: \"{0}\"", tipo));
                return RedirectToAction("Index");
            }
        }

    }
}
