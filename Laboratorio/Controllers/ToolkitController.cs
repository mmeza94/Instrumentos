﻿using Laboratorio.Models.DataAccess;
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

        public List<SelectListItem> toolkits { get; set; }
        public List<ToolModel> toolsFromToolKits { get; set; }
        public List<ToolModel> AvailableTools { get; set; }

        public ToolkitController()
        {
            FillToolKitViewBag();
            FillAvailableToolsViewBag();
            FillToolkitsViewBag();
            
        }





        public ActionResult Index(string ToolKitCode, string validation)
        {

                
            ViewBag.ValidationMessage = validation;


            if(ToolKitCodeExists(ToolKitCode))
                FillToolkitsViewBag();


            //Validamos que las dos tablas tenga informacion
            NoRecordsViewBag();
            NoRecords2ViewBag();
            return View();
        }



        public ActionResult AddTool(string toolCode, int use)
        {

            ToolModel SelectedTool = GetSelectedToolFromAvailableTools(toolCode, use);

            toolsFromToolKits.Add(SelectedTool);
            ViewBag.ToolKit = toolsFromToolKits;
            NoRecordsViewBag();
            NoRecords2ViewBag();
            return View("Index");
        }



        public ActionResult RemoveTool(string toolCode)
        {
            ToolModel SelectedTool = toolsFromToolKits.FirstOrDefault(tool => tool.Code == toolCode);
            toolsFromToolKits.Remove(SelectedTool);
            ViewBag.ToolKit = toolsFromToolKits;
            NoRecordsViewBag();
            NoRecords2ViewBag();
            return View("Index");


        }






        private void NoRecordsViewBag()
        {
            if (toolkits.Count == 0)
            {
                ViewBag.NoRecords = true;
            }
            else
            {
                ViewBag.NoRecords = false;
            }
        }

        private void NoRecords2ViewBag()
        {
            if (AvailableTools.Count == 0)
            {
                ViewBag.NoRecords2 = true;
            }
            else
            {
                ViewBag.NoRecords2 = false;
            }
        }

        private void FillToolKitViewBag()
        {
            toolkits = GetToolkitCodes();
            ViewBag.toolkitCodes = toolkits;
        }

        private void FillAvailableToolsViewBag()
        {
            AvailableTools = DataAccess.GetAvailableTools();
            ViewBag.AvailableTools = AvailableTools;
        }

        private void FillToolkitsViewBag(string ToolKitCode = "Abc") //Valor por default para prueba, esto se va a poner en BD
        {
            toolsFromToolKits = GetToolsFromToolKit(ToolKitCode);
            ViewBag.ToolKit = toolsFromToolKits;
        }

        private bool ToolKitCodeExists(string ToolKitCode)
        {
            if (ToolKitCode is null)
                return false;
            return true;
        }

        public List<ToolModel> GetToolsFromToolKit(string ToolKitCode)
        {
            List<ToolModel> Tools = DataAccess.GetToolsFromToolKit(ToolKitCode);

            return Tools;
        }

        public List<SelectListItem> GetToolkitCodes()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            lista = DataAccess.GetToolKitCodes();

            return lista;
        }

        private ToolModel GetSelectedToolFromAvailableTools(string toolCode, int use)
        {
            ToolModel SelectedTool = AvailableTools.FirstOrDefault(tool => tool.Code == toolCode);
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
