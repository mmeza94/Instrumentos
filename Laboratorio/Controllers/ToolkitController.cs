using Laboratorio.Models.DataAccess;
using Laboratorio.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web.Mvc;

namespace Laboratorio.Controllers
{
    public class ToolkitController: Controller
    {
        // GET: /Assignment/





        private bool ValidateInformation()
        {
            //if (toolkits == null)
            //    return true;
            //if (toolsFromToolKits == null)
            //    return true;
            //if (AvailableTools == null)
            //    return true;

            return false;
        }




        public ActionResult Index(string ToolKitCode, string validation)
        {

                
            ViewBag.ValidationMessage = validation;

            FillToolKitViewBag();
            FillAvailableToolsViewBag();
            FillToolkitsViewBag(ToolKitCode);


            //Validamos que las dos tablas tenga informacion
            NoRecordsViewBag();
            NoRecords2ViewBag();
            return View();
        }


        private void UpdateToolsFromToolkitData(ToolModel SelectedTool)
        {
            var UpdatedToolsFromToolskit = this.Session["ToolsFromToolKit"];
            List<ToolModel>  toolsFromToolKits = (List<ToolModel>)UpdatedToolsFromToolskit;
            toolsFromToolKits.Add(SelectedTool);
            this.Session["ToolsFromToolKit"] = toolsFromToolKits;
        }


        private void UpdateViewBags()
        {
            ViewBag.ToolKit = this.Session["ToolsFromToolKit"];
            ViewBag.toolkitCodes = this.Session["ToolKitCatalog"];
            ViewBag.AvailableTools = this.Session["AvailableTools"];
            NoRecordsViewBag();
            NoRecords2ViewBag();
        }




        public ActionResult AddTool(string toolCode, int use)
        {
            ToolModel SelectedTool = GetSelectedToolFromAvailableTools(toolCode, use);

            UpdateToolsFromToolkitData(SelectedTool);

            UpdateViewBags();

            return View("Index");
        }







        public ActionResult InsertNewToolKitCatalog(string KitCode)
        {

            var result = DataAccess.InsToolKitCatalog(KitCode);

            if (result)
            {
                InsToolKit(KitCode);
                NoRecordsViewBag();
                NoRecords2ViewBag();
                return View("Index");// "OperacionExitosa";
            }

            NoRecordsViewBag();
            NoRecords2ViewBag();

            return View("Index"); // Operacion no exitosa
        }


        public void InsToolKit(string kitCode)
        {
            var toolsFromToolKits = (List<ToolModel>)this.Session["ToolsFromToolKit"];
            foreach (var item in toolsFromToolKits)
            {
                DataAccess.InsToolKit(kitCode, item.Code);
            }
      
        }





        public ActionResult RemoveTool(string toolCode)
        {
            List<ToolModel> toolsFromToolKits = (List<ToolModel>)this.Session["ToolsFromToolKit"];
            ToolModel SelectedTool = toolsFromToolKits.FirstOrDefault(tool => tool.Code == toolCode);
            toolsFromToolKits.Remove(SelectedTool);
            ViewBag.ToolKit = toolsFromToolKits;
            NoRecordsViewBag();
            NoRecords2ViewBag();
            return View("Index");


        }






        private void NoRecordsViewBag()
        {
            //if (toolkits.Count == 0)
            //{
            //    ViewBag.NoRecords = true;
            //}
            //else
            //{
            //    ViewBag.NoRecords = false;
            //}
            ViewBag.NoRecords = false;

        }

        private void NoRecords2ViewBag()
        {
            //if (AvailableTools.Count == 0)
            //{
            //    ViewBag.NoRecords2 = true;
            //}
            //else
            //{
            //    ViewBag.NoRecords2 = false;
            //}
            ViewBag.NoRecords2 = false;
        }

        private void FillToolKitViewBag()
        {
            List<SelectListItem> ToolKitCatalog = GetToolkitCodesFromCatalog();
            this.Session["ToolKitCatalog"] = ToolKitCatalog;
            ViewBag.toolkitCodes = this.Session["ToolKitCatalog"];
        }

        private void FillAvailableToolsViewBag()
        {
            List<ToolModel> AvailableTools = DataAccess.GetAvailableTools();
            this.Session["AvailableTools"] = AvailableTools;
            ViewBag.AvailableTools = this.Session["AvailableTools"];
        }

        private void FillToolkitsViewBag(string ToolKitCode) //Valor por default para prueba, esto se va a poner en BD
        {
            List<ToolModel> toolsFromToolKit = GetToolsFromSelectedToolKit(ToolKitCode);
            this.Session["ToolsFromToolKit"] = toolsFromToolKit;
            ViewBag.ToolKit = this.Session["ToolsFromToolKit"];
            
        }


        private List<ToolModel> GetToolsFromSelectedToolKit(string ToolKitCode)
        {
            List<ToolModel> toolsFromToolKit = DataAccess.GetToolsFromToolKit(ToolKitCode);
            return toolsFromToolKit;
        }

        private List<SelectListItem> GetToolkitCodesFromCatalog()
        {
            List<SelectListItem> ToolKitCatalog = new List<SelectListItem>();
            ToolKitCatalog = DataAccess.GetToolKitCodes();
            return ToolKitCatalog;
        }




        private ToolModel GetSelectedToolFromAvailableTools(string toolCode, int use)
        {
            var availableTools = this.Session["AvailableTools"];
            ToolModel SelectedTool = ((List<ToolModel>)availableTools).FirstOrDefault(tool => tool.Code == toolCode);
            var valorMeasure = (use == 1) ? true : false;
            SelectedTool.Measure = valorMeasure;

            return SelectedTool;
        }




        public ActionResult UpdateToolPage(string code, string validation)
        {
            List<ToolModel> tm = DataAccess.GetTools();

            ViewBag.ValidationMessage = validation;

            return View(tm.Single(s => s.Code.Equals(code)));
        }

        private List<SelectListItem> LoadMachines()
        {
            List<SelectListItem> sli = new List<SelectListItem>();

            SelectListItem gmp = new SelectListItem();
            gmp.Text = "Granalladora MP";
            gmp.Value = ConfigurationManager.AppSettings["GranalladoraCode"].ToString();

            SelectListItem end = new SelectListItem();
            end.Text = "Enderezadora";
            end.Value = ConfigurationManager.AppSettings["EnderezadoraCode"].ToString();

            SelectListItem cnd = new SelectListItem();
            cnd.Text = "Control no destructivo";
            cnd.Value = ConfigurationManager.AppSettings["CNDCode"].ToString();

            SelectListItem forj0 = new SelectListItem();
            forj0.Text = "Forjadora 0";
            forj0.Value = ConfigurationManager.AppSettings["Forjadora0Code"].ToString();

            SelectListItem forj = new SelectListItem();
            forj.Text = "Forjadora 1";
            forj.Value = ConfigurationManager.AppSettings["ForjadoraCode"].ToString();

            SelectListItem nor = new SelectListItem();
            nor.Text = "Horno de normalizado";
            nor.Value = ConfigurationManager.AppSettings["NormalizadoCode"].ToString();

            SelectListItem rev = new SelectListItem();
            rev.Text = "Horno de revenido";
            rev.Value = ConfigurationManager.AppSettings["RevenidoCode"].ToString();

            SelectListItem gra2 = new SelectListItem();
            gra2.Text = "Granalladora";
            gra2.Value = ConfigurationManager.AppSettings["Granalladora2Code"].ToString();

            SelectListItem mec1 = new SelectListItem();
            mec1.Text = "Roscadora 1";
            mec1.Value = ConfigurationManager.AppSettings["Roscadora1Code"].ToString();

            SelectListItem mec2 = new SelectListItem();
            mec2.Text = "Roscadora 2";
            mec2.Value = ConfigurationManager.AppSettings["Roscadora2Code"].ToString();

            SelectListItem mec3 = new SelectListItem();
            mec3.Text = "Roscadora 3";
            mec3.Value = ConfigurationManager.AppSettings["Roscadora3Code"].ToString();

            SelectListItem mec4 = new SelectListItem();
            mec4.Text = "Roscadora 4";
            mec4.Value = ConfigurationManager.AppSettings["Roscadora4Code"].ToString();

            SelectListItem rosCop = new SelectListItem();
            rosCop.Text = "Roscadora Coples";
            rosCop.Value = ConfigurationManager.AppSettings["RoscadoraCoplesCode"].ToString();

            SelectListItem fosCop = new SelectListItem();
            fosCop.Text = "Fosfatizado Coples";
            fosCop.Value = ConfigurationManager.AppSettings["FosfatizadoCoplesCode"].ToString();

            SelectListItem Embalado1 = new SelectListItem();
            Embalado1.Text = "Pintado Tina A";
            Embalado1.Value = ConfigurationManager.AppSettings["Embalado1Code"].ToString();
            SelectListItem Embalado2 = new SelectListItem();
            Embalado2.Text = "Pintado Tina B";
            Embalado2.Value = ConfigurationManager.AppSettings["Embalado2Code"].ToString();

            sli.Add(gmp);
            sli.Add(end);
            sli.Add(cnd);
            sli.Add(forj0);
            sli.Add(forj);
            sli.Add(nor);
            sli.Add(rev);
            sli.Add(gra2);
            sli.Add(mec1);
            sli.Add(mec2);
            sli.Add(Embalado1);
            sli.Add(mec3);
            sli.Add(mec4);
            sli.Add(Embalado2);
            sli.Add(rosCop);
            sli.Add(fosCop);

            return sli;
        }

        public ActionResult ShareTool(string toolCode, string machineCode, bool meassure)
        {
            int result = DataAccess.ShareMachineTool(toolCode, machineCode, meassure);

            if (result < 0)
            {
                ViewBag.ValidationMessage = "Error al compartir instrumento, podría deberse a que ya existe en la otra máquina.";
                return RedirectToAction("Index", "Assignment", new { machineCode = machineCode, validation = ViewBag.ValidationMessage });
                //return View("Error");
            }
            else
            {
                DataAccess.InsertLogEntry(User.Identity.Name,
                   String.Format("Instrumento Compartido : \"{0}\"  a la máquina \"{1}\"", toolCode, machineCode));
                ViewBag.MachineCode = machineCode;
                return RedirectToAction("Index", "Assignment", new { machineCode = machineCode });
            }
        }


    }
}
