using Laboratorio.Models.DataAccess;
using Laboratorio.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web.Mvc;
using System.CodeDom;
using System.Web.UI.WebControls.WebParts;

namespace Laboratorio.Controllers
{
    public class ToolkitController: Controller
    {



        // this.Session["SelectedToolKitCode123"] -- El toolkitcode seleccionado
        // this.Session["MachineIds"] - la lista de machineId's
        // this.Session["ToolsFromToolKit"] -- Tabla superior --instrumentos de la plantilla
        //this.Session["AvailableTools"]; -- Tabla inferior -- instrumentos disponibles
        //this.Session["ToolKitCatalog"] -- Codigos de plantillas



        // GET: /Assignment/

        #region EndPoints


        public ActionResult Index(string MachineCode,string ToolKitCode, string validation)
        {
            //Estas no dependen de nada
            FillMachineIdViewBag();
            FillAvailableToolsSession();
            ViewBag.ValidationMessage = validation;




            MachineCode = SetDefaultMachineCodeIfNull(MachineCode);
            this.Session["idSelectedMachine"] = ConfigurationManager.AppSettings[MachineCode].ToString();


            //Llenamos el combobox de codigos de plantillas
            FillToolKitCatalogSession();
            //Llenamos la tabla de instrumentos que contiene la plantilla
            FillToolkitsViewBag(ToolKitCode);



            //Validamos que las dos tablas tenga informacion
            UpdateViewBags();
            return View();
        }




        public ActionResult AddToolMassive(List<string> Code)
        {
            List<string> list = new List<string>();
            foreach (var item in Code)
            {
                int use = 1;
                AddTool(item, use);
            }
            return View("Index");
        }




        public ActionResult AddTool(string toolCode, int use)
        {
            ToolModel SelectedTool = GetSelectedToolFromAvailableTools(toolCode, use);

            UpdateToolsFromToolkitData(SelectedTool);

            UpdateViewBags();

            return View("Index");
        }




        public ActionResult InsertNewToolKitCatalog(string KitCode, string MachineCode)
        {

            int idMachine = Convert.ToInt32(ConfigurationManager.AppSettings[MachineCode].ToString());


            bool IsInsertionSuccesful = DataAccess.InsToolKitCatalog(KitCode, idMachine);

            if (IsInsertionSuccesful)
            {
                InsToolKit(KitCode);
                FillToolKitCatalogSession();
                FillToolkitsViewBag();
                UpdateViewBags();
                return View("Index");// "OperacionExitosa";
            }

            UpdateViewBags();
            return View("Index"); // Operacion no exitosa
        }



        public ActionResult DeleteToolkit()
        {
            string ToolkitCode = this.Session["SelectedToolKitCode123"].ToString();
            DataAccess.DeleteToolkit(ToolkitCode);
            FillToolKitCatalogSession();
            FillToolkitsViewBag(ToolkitCode);
            UpdateViewBags();
            return View("Index");
        }


        public ActionResult RemoveTool(string toolCode)
        {

            List<ToolModel> toolsFromToolKits = (List<ToolModel>)this.Session["ToolsFromToolKit"];
            ToolModel SelectedTool = toolsFromToolKits.FirstOrDefault(tool => tool.Code == toolCode);
            toolsFromToolKits.Remove(SelectedTool);
            UpdateViewBags();
            return View("Index");


        }



        public ActionResult UpdateToolPage(string code, string validation)
        {
            List<ToolModel> tm = DataAccess.GetTools();

            ViewBag.ValidationMessage = validation;

            return View(tm.Single(s => s.Code.Equals(code)));
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



        #endregion







        private string SetDefaultMachineCodeIfNull(string MachineCode)
        {
            if (MachineCode == null)
            {
                MachineCode = ConfigurationManager.AppSettings["GranalladoraCode"].ToString();
            }

            return MachineCode;

        }


        private void FillMachineIdViewBag()
        {
            //Se llena el primer comboBox
            List<SelectListItem> sli = LoadMachines();
            this.Session["MachineIds"]= sli;
            ViewBag.Sli = sli;
        }


        public void InsToolKit(string kitCode)
        {
            var toolsFromToolKits = (List<ToolModel>)this.Session["ToolsFromToolKit"];
            foreach (var item in toolsFromToolKits)
            {
                DataAccess.InsToolKit(kitCode, item.Code);
            }
      
        }

       
        private void UpdateToolsFromToolkitData(ToolModel SelectedTool)
        {
            var UpdatedToolsFromToolskit = this.Session["ToolsFromToolKit"];
            List<ToolModel> toolsFromToolKits = (List<ToolModel>)UpdatedToolsFromToolskit;
            toolsFromToolKits.Add(SelectedTool);
            this.Session["ToolsFromToolKit"] = toolsFromToolKits;
        }


        private void UpdateViewBags()
        {
            ViewBag.ToolKit = this.Session["ToolsFromToolKit"];
            ViewBag.toolkitCodes = this.Session["ToolKitCatalog"];
            ViewBag.AvailableTools = this.Session["AvailableTools"];
            ViewBag.Sli = this.Session["MachineIds"];
            NoRecordsViewBag();
            NoRecords2ViewBag();
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


        private void FillToolKitCatalogSession()
        {

            List<SelectListItem> ToolKitCatalog = GetToolkitCodesFromCatalog();
            this.Session["ToolKitCatalog"] = ToolKitCatalog;         

        }


        private void FillAvailableToolsSession()
        {
            List<ToolModel> AvailableTools = DataAccess.GetAvailableTools();
            FillFlag(AvailableTools);
            this.Session["AvailableTools"] = AvailableTools;

        }

        private void FillFlag(List<ToolModel> tm)
        {
            foreach (var item in tm)
            {
                var flag = item.ExpirationDate.Subtract(DateTime.Now).Days;

                if (flag <= 0)
                    item.ExpirationFlag = "0";//expirado
                if (flag <= 10 && flag > 0)
                    item.ExpirationFlag = "1";//proximo a expirar
                if (flag > 10)
                    item.ExpirationFlag = "2";//todo bien

            }
        }


        private void FillToolkitsViewBag(string ToolKitCode = null) //Valor por default para prueba, esto se va a poner en BD
        {
            //Validamos que la plantilla si exista en el catalogo de la maquina, si no, seleccionamos la primer plantilla que traiga.
            List<SelectListItem> codes = (List<SelectListItem>)this.Session["ToolKitCatalog"];


            //TO-DO : Validar si Codes esta vacio, esto para en caso qeu no haya una plantilla definida para la maquina
            var validacion = codes.Any(c => c.Value == ToolKitCode);
            ToolKitCode = validacion ? ToolKitCode : codes.FirstOrDefault().Value;


            List <ToolModel> toolsFromToolKit = GetToolsFromSelectedToolKit(ToolKitCode);
            this.Session["SelectedToolKitCode123"] = ToolKitCode;
            FillFlag(toolsFromToolKit);
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
            int idMachine = Convert.ToInt32(this.Session["idSelectedMachine"].ToString());
            List<SelectListItem> ToolKitCatalog = new List<SelectListItem>();
            ToolKitCatalog = DataAccess.GetToolKitCodes(idMachine);
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





    }
}
