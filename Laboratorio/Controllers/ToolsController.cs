using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio.Models.DataAccess;
using Laboratorio.Models;


using Laboratorio.Models;namespace Laboratorio.Controllers
{
    public class ToolsController : Controller
    {
        //
        // GET: /Tool/

        public ActionResult Index()
        {
            List<ToolsModel> l = DataAccess.GetTools();

            return View(l);
        }

    }
}
