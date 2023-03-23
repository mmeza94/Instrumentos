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

            ToolKitModel model = new ToolKitModel();




            //Estas no dependen de nada
            FillMachineIdViewBag();
            FillAvailableToolsSession();
            ViewBag.ValidationMessage = validation;




            MachineCode = SetDefaultMachineCodeIfNull(MachineCode);
            this.Session["MachineCode"] = MachineCode;
            //this.Session["idSelectedMachine"] = ConfigurationManager.AppSettings[MachineCode].ToString();


            //Llenamos el combobox de codigos de plantillas
            FillToolKitCatalogSession();


            //Llenamos la tabla de instrumentos que contiene la plantilla
            FillToolkitsViewBag(ToolKitCode);




            FillModel(model);



            //Validamos que las dos tablas tenga informacion
            UpdateViewBags();



            return View(model);
        }


        


        public ActionResult InsertNewToolKitCatalog(string KitCode, string MachineCode)
        {

            var cleanedToolkitCode = KitCode.Trim();

            //int idMachine = Convert.ToInt32(ConfigurationManager.AppSettings[MachineCode].ToString());
            bool IsInsertionSuccesful = false;
            ToolKitModel model = new ToolKitModel();

            if (!string.IsNullOrEmpty(cleanedToolkitCode))
            {
                IsInsertionSuccesful = DataAccess.InsToolKitCatalog(cleanedToolkitCode, MachineCode);               
            }
            


            if (IsInsertionSuccesful)
            {
                //Rellenamos todos los viewbags con los session actualizados y el model y mandamos a llamar a la vista
                FillToolKitCatalogSession();
                FillToolkitsViewBag();
                FillModel(model);
                UpdateViewBags();
                return View("Index",model);// "OperacionExitosa";
            }

            //Rellenamos el modelo solamente y los viewbags
            FillModel(model);
            UpdateViewBags();
            return View("Index", model); // Operacion no exitosa
        }



        public ActionResult DeleteToolkit()
        {
            ToolKitModel model = new ToolKitModel();
            string ToolkitCode = this.Session["SelectedToolKitCode123"].ToString();
            DataAccess.DeleteToolkit(ToolkitCode);
            FillToolKitCatalogSession();
            FillToolkitsViewBag(ToolkitCode);
            FillModel(model);
            UpdateViewBags();
            return View("Index", model);
        }



        //[HttpPost]
        //public ActionResult ToolMassiveAction(string activos, string activosCodeToolkit, string MeasureUse, string Action)
        //{

        //    if (MeasureUse == "" || activos == "" || activosCodeToolkit == "")
        //    {
        //        ToolKitModel model = new ToolKitModel();
        //        FillToolkitsViewBag();
        //        UpdateViewBags();
        //        FillModel(model);

        //        return View("Index", model);
        //    }
        //    else
        //    {
        //        if (IsActionInsert(Action))
        //        {

        //            MassiveInsertTools(activos, activosCodeToolkit, MeasureUse);
        //        }
        //        else
        //        {
        //            MassiveDeleteTools(activos, activosCodeToolkit, MeasureUse);
        //        }



        //        ToolKitModel model = new ToolKitModel();
        //        FillToolkitsViewBag();
        //        UpdateViewBags();
        //        FillModel(model);

        //        return View("Index", model);
        //    }




        //}




        [HttpPost]
        public ActionResult ToolMassiveAction(string activos, string activosCodeToolkit, string MeasureUse, string Action)
        {
            ToolKitModel model = new ToolKitModel();

            if (IsActionInsert(Action))
            {
                if (MeasureUse == "" || activos == "" || activosCodeToolkit == "")
                {

                    FillToolkitsViewBag();
                    UpdateViewBags();
                    FillModel(model);

                    return View("Index", model);
                }
                else
                {

                    MassiveInsertTools(activos, activosCodeToolkit, MeasureUse);
                    FillToolkitsViewBag();
                    UpdateViewBags();
                    FillModel(model);

                    return View("Index", model);
                }

            }
            else
            {

                if (activos == "" || activosCodeToolkit == "")
                {

                    FillToolkitsViewBag();
                    UpdateViewBags();
                    FillModel(model);

                    return View("Index", model);
                }

                MassiveDeleteTools(activos, activosCodeToolkit);
                FillToolkitsViewBag();
                UpdateViewBags();
                FillModel(model);

                return View("Index", model);
            }


        }







        public ActionResult AddTool(string toolCode, int use)
        {
            //ToolModel SelectedTool = GetSelectedToolFromAvailableTools(toolCode, use);

            //UpdateToolsFromToolkitData(SelectedTool);
            var toolkitcode = this.Session["SelectedToolKitCode123"].ToString();
            DataAccess.InsToolKit(toolkitcode, toolCode, use);
            ToolKitModel model = new ToolKitModel();
            FillToolkitsViewBag(toolkitcode);
            UpdateViewBags();
            FillModel(model);
            return View("Index",model);
        }



        public ActionResult RemoveTool(string toolCode, bool Measure)
        {
            var measure = (Measure == true) ? 1 : 0;
            List<ToolModel> toolsFromToolKits = (List<ToolModel>)this.Session["ToolsFromToolKit"];
            ToolModel SelectedTool = toolsFromToolKits.FirstOrDefault(tool => tool.Code == toolCode);
            var toolkitcode = this.Session["SelectedToolKitCode123"].ToString();
            DataAccess.DeleteToolByToolkit(toolkitcode, toolCode);
            //toolsFromToolKits.Remove(SelectedTool);
            ToolKitModel model = new ToolKitModel();
            FillToolkitsViewBag(toolkitcode);
            UpdateViewBags();
            FillModel(model);
            return View("Index",model);


        }




        public ActionResult UpdateToolPage(string code, string validation)
        {
            List<ToolModel> tm = DataAccess.GetAvailableToolsForToolKit();

            ViewBag.ValidationMessage = validation;

            return View(tm.Single(s => s.Code.Equals(code)));
        }


        public ActionResult UpdateTool(string code, DateTime calibrationDate, DateTime expirationDate, string previousCalibration, string previousExpiration)
        {

            if (DateTime.Compare(expirationDate, calibrationDate) < 0)
            {
                ResourceManager rs = new ResourceManager(typeof(Laboratorio.Messages));
                string msg = rs.GetString("ValidationExpirationDate");
                return RedirectToAction("UpdateToolPage", "Toolkit", new { validation = msg, code = code });
            }

            int result = DataAccess.UpdateTool(code, calibrationDate, expirationDate);

            if (result < 0)
            {
                return RedirectToAction("UpdateToolPage", "Toolkit", new { code = code });
            }
            else
            {
                DataAccess.InsertLogEntry(User.Identity.Name,
                    String.Format("Instrumento recalibrado: \"{0}\". Fecha previa de calibracion \"{1}\", fecha previa de expiración \"{2}\". Fecha de calibración nueva \"{3}\", fecha de expiración nueva \"{4}\"",
                    code, previousCalibration, previousExpiration, calibrationDate.ToShortDateString(), expirationDate.ToShortDateString()));
                return RedirectToAction("Index", "Toolkit");
            }
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






        #region Utils


        private List<string> GetCodesList(string ConcatenatedCodes)
        {
            List<string> CleanedCodes = new List<string>();
            CleanedCodes = ConcatenatedCodes.Split(',').Where(x => x != "").ToList();
            return CleanedCodes;
        }


        private bool IsActionInsert(string Action)
        {
            return (Action == "Insertar") ? true : false;
        }


        private void MassiveDeleteTools(string activos, string activosCodeToolkit)
        {
            List<string> codigosInstrumentos = GetCodesList(activos);
            List<string> codigosPlantillas = GetCodesList(activosCodeToolkit);
            //var measure = Convert.ToInt32(MeasureUse);

            foreach (string Plantilla in codigosPlantillas)
            {
                foreach (string Instrumento in codigosInstrumentos)
                {
                    DataAccess.DeleteToolByToolkit(Plantilla, Instrumento);
                }
            }

            //DeleteToolByToolkit
        }


        private void MassiveInsertTools(string activos, string activosCodeToolkit, string MeasureUse)
        {
            //TODO Validar el valor del agregar para saber si borro o agrego
            List<string> codigosInstrumentos = GetCodesList(activos);
            List<string> codigosPlantillas = GetCodesList(activosCodeToolkit);
            var measure = Convert.ToInt32(MeasureUse);




            foreach (string Plantilla in codigosPlantillas)
            {
                foreach (string Instrumento in codigosInstrumentos)
                {
                    DataAccess.InsToolKit(Plantilla, Instrumento, measure);
                }
            }
        }



        private void FillModel(ToolKitModel model)
        {
            var lista = ((List<SelectListItem>)this.Session["ToolKitCatalog"]);
            model.ToolKitCodes = lista.Select(x => x.Value).ToList();
            model.Tools = (List<ToolModel>)this.Session["AvailableTools"];
        }


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
            List<ToolModel> AvailableTools = DataAccess.GetAvailableToolsForToolKit();
            FillFlag(AvailableTools);   
            this.Session["AvailableTools"] = AvailableTools;

        }


        private void FillFlag(List<ToolModel> tm)
        {
            foreach (var item in tm)
            {
                var flag = item.ExpirationDate.Date.Subtract(DateTime.Now.Date).Days;

                if (flag <= 0)
                    item.ExpirationFlag = "0";//expirado
                if (flag <= 7 && flag > 0)
                    item.ExpirationFlag = "1";//proximo a expirar
                if (flag > 7)
                    item.ExpirationFlag = "2";//todo bien

            }
        }


        private void FillToolkitsViewBag(string ToolKitCode = null) //Valor por default para prueba, esto se va a poner en BD
        {
            //Validamos que la plantilla si exista en el catalogo de la maquina, si no, seleccionamos la primer plantilla que traiga.
            List<SelectListItem> codes = (List<SelectListItem>)this.Session["ToolKitCatalog"];
            List<ToolModel> toolsFromToolKit = new List<ToolModel>();

            if (ExistingCodes())
            {
                var IsToolkitMachineValid = codes.Any(c => c.Value == ToolKitCode);
                ToolKitCode = IsToolkitMachineValid ? ToolKitCode : codes.FirstOrDefault().Value;
                toolsFromToolKit = GetToolsFromSelectedToolKit(ToolKitCode);
                FillFlag(toolsFromToolKit);

                this.Session["SelectedToolKitCode123"] = ToolKitCode;
                this.Session["ToolsFromToolKit"] = toolsFromToolKit;
                ViewBag.ToolKit = this.Session["ToolsFromToolKit"];
            }
            else
            {

                //toolsFromToolKit = GetToolsFromSelectedToolKit(ToolKitCode);
                //FillFlag(toolsFromToolKit);

                this.Session["SelectedToolKitCode123"] = ToolKitCode;
                this.Session["ToolsFromToolKit"] = toolsFromToolKit;
                ViewBag.ToolKit = this.Session["ToolsFromToolKit"];
            }




        }


        private bool ExistingCodes()
        {
            List<SelectListItem> codes = (List<SelectListItem>)this.Session["ToolKitCatalog"];
            return (codes == null || codes.Count == 0) ? false : true;            

        }


        private List<ToolModel> GetToolsFromSelectedToolKit(string ToolKitCode)
        {
            List<ToolModel> toolsFromToolKit = DataAccess.GetToolsFromToolKit123(ToolKitCode);
            return toolsFromToolKit;
        }


        private List<SelectListItem> GetToolkitCodesFromCatalog()
        {
            string MachineCode = this.Session["MachineCode"].ToString();
            List<SelectListItem> ToolKitCatalog = new List<SelectListItem>();
            ToolKitCatalog = DataAccess.GetToolKitCodes(MachineCode);
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

        #endregion



    }
}
