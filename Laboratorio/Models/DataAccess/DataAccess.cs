using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenaris.Library.DbClient;
using System.Data;
using System.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

namespace Laboratorio.Models.DataAccess
{
    public class DataAccess
    {
        private static DbClient dbClientGrana;

        private static string storeProcedureQryToolTypes = ConfigurationManager.AppSettings["SP_qryToolTypes"].ToString();
        private static string storeProcedureInsToolType = ConfigurationManager.AppSettings["SP_insToolType"].ToString();
        private static string storeProcedureDelToolType = ConfigurationManager.AppSettings["SP_delToolType"].ToString();
        private static string storeProcedureQryTools = ConfigurationManager.AppSettings["SP_qryTools"].ToString();
        private static string storeProcedureInsTool = ConfigurationManager.AppSettings["SP_insTool"].ToString();
        private static string storeProcedureDelTool = ConfigurationManager.AppSettings["SP_delTool"].ToString();
        private static string storeProcedureUpdTool = ConfigurationManager.AppSettings["SP_updTool"].ToString();
        private static string storeProcedureQryToolsByMachineCode = ConfigurationManager.AppSettings["SP_qryToolsByMachineCode"].ToString();
        private static string storeProcedureQryAvailableTools = ConfigurationManager.AppSettings["SP_qryAvailableTools"].ToString();
        private static string storeProcedureInsMachineTool = ConfigurationManager.AppSettings["SP_insMachineTool"].ToString();
        private static string storeProcedureDelMachineTool = ConfigurationManager.AppSettings["SP_delMachineTool"].ToString();
        private static string storeProcedureQryUserPrivileges = ConfigurationManager.AppSettings["SP_qryUserPrivileges"].ToString();
        private static string storeProcedureInsLog = ConfigurationManager.AppSettings["SP_insLog"].ToString();
        private static string storeProcedureShareMachineTool = ConfigurationManager.AppSettings["SP_shaMachineTool"].ToString();
        
        private static string storeProcedureQryDeactivatedTools = ConfigurationManager.AppSettings["SP_GetDeactivatedTools"].ToString();
        private static string storeProcedureActivateTool = ConfigurationManager.AppSettings["SP_ActivateTool"].ToString();
        private static string storeProcedureGetToolKit = ConfigurationManager.AppSettings["SP_GetToolKit"].ToString();
        private static string storeProcedureGetToolKitCodesByIdMachine = ConfigurationManager.AppSettings["SP_GetToolKitCodesByIdMachine"].ToString();
        private static string storeProcedureInsToolKitCode = ConfigurationManager.AppSettings["SP_InsToolKitCode"].ToString();
        private static string storeProcedureInsToolKit = ConfigurationManager.AppSettings["SP_InsToolKit"].ToString();
        private static string storeProcedureDeactiveToolFromToolkitCatalog = ConfigurationManager.AppSettings["SP_DeactiveToolFromToolkitCatalog"].ToString();
        private static string storeProcedureDeleteToolKit = ConfigurationManager.AppSettings["SP_DeleteToolKit"].ToString();
        private static string storeProcedureGetToolKitCodesPerTool = ConfigurationManager.AppSettings["SP_GetToolKitCodesPerTool"].ToString();
        //private static string storeProcedureDeleteDeactivatedTool = ConfigurationManager.AppSettings["SP_DeleteDeactivatedTool"].ToString();
        private static string storeProcedureDeleteToolInToolkit = ConfigurationManager.AppSettings["SP_DeleteToolInToolkit"].ToString();
        private static string storeProcedureqryToolsWithToolkitList = ConfigurationManager.AppSettings["SP_qryToolsWithToolkitList"].ToString();
        private static string storeProcedureqryAvailableToolsForToolKit = ConfigurationManager.AppSettings["SP_qryAvailableToolsForToolKit"].ToString();

        static DataAccess()
        {
            dbClientGrana = new DbClient("GEC_DB");

            dbClientGrana.AddCommand(storeProcedureQryToolTypes);
            dbClientGrana.AddCommand(storeProcedureInsToolType);
            dbClientGrana.AddCommand(storeProcedureDelToolType);
            dbClientGrana.AddCommand(storeProcedureQryTools);
            dbClientGrana.AddCommand(storeProcedureInsTool);
            dbClientGrana.AddCommand(storeProcedureDelTool);
            dbClientGrana.AddCommand(storeProcedureUpdTool);
            dbClientGrana.AddCommand(storeProcedureQryToolsByMachineCode);
            dbClientGrana.AddCommand(storeProcedureQryAvailableTools);
            dbClientGrana.AddCommand(storeProcedureInsMachineTool);
            dbClientGrana.AddCommand(storeProcedureDelMachineTool);
            dbClientGrana.AddCommand(storeProcedureQryUserPrivileges);
            dbClientGrana.AddCommand(storeProcedureInsLog);
            dbClientGrana.AddCommand(storeProcedureShareMachineTool);
            


            dbClientGrana.AddCommand(storeProcedureQryDeactivatedTools);
            dbClientGrana.AddCommand(storeProcedureActivateTool);
            dbClientGrana.AddCommand(storeProcedureGetToolKit);
            dbClientGrana.AddCommand(storeProcedureGetToolKitCodesByIdMachine);
            dbClientGrana.AddCommand(storeProcedureInsToolKitCode);
            dbClientGrana.AddCommand(storeProcedureInsToolKit);
            dbClientGrana.AddCommand(storeProcedureDeactiveToolFromToolkitCatalog);
            dbClientGrana.AddCommand(storeProcedureDeleteToolKit);
            dbClientGrana.AddCommand(storeProcedureGetToolKitCodesPerTool);
           // dbClientGrana.AddCommand(storeProcedureDeleteDeactivatedTool);
            dbClientGrana.AddCommand(storeProcedureDeleteToolInToolkit);
            dbClientGrana.AddCommand(storeProcedureqryToolsWithToolkitList);
            dbClientGrana.AddCommand(storeProcedureqryAvailableToolsForToolKit);

            dbClientGrana.Activate();
        }




        public static List<ToolModel> GetAvailableToolsForToolKit()
        {
            List<ToolModel> l = new List<ToolModel>();

            using (var reader = dbClientGrana.GetCommand(storeProcedureqryAvailableToolsForToolKit).ExecuteReader())
            {
                while (reader.Read())
                {
                    ToolModel tm = new ToolModel();

                    tm.Code = reader["Code"].ToString();
                    tm.Type = reader["TypeName"].ToString();
                    tm.CalibrationDate = DateTime.Parse(reader["CalibrationDate"].ToString());
                    tm.ExpirationDate = DateTime.Parse(reader["ExpirationDate"].ToString());

                    l.Add(tm);
                }

            }

            return l;
        }






        public static List<ToolModel> GetToolsWithToolKitList()
        {
            List<ToolModel> l = new List<ToolModel>();

            using (var reader = dbClientGrana.GetCommand(storeProcedureqryToolsWithToolkitList).ExecuteReader())
            {
                while (reader.Read())
                {
                    ToolModel tm = new ToolModel();

                    tm.Code = reader["Code"].ToString();
                    tm.Type = reader["TypeName"].ToString();
                    tm.CalibrationDate = DateTime.Parse(reader["CalibrationDate"].ToString());
                    tm.ExpirationDate = DateTime.Parse(reader["ExpirationDate"].ToString());
                    tm.Machine = reader["MachineName"].ToString();
                    tm.Plantilla = reader["Plantilla"].ToString();
                    bool active = Boolean.Parse(reader["Active"].ToString());
                    if (tm.Machine.Equals(String.Empty))
                    {
                        if (active)
                        {
                            tm.Available = 1; //disponible
                        }
                        else
                        {
                            tm.Available = 3; //de baja
                        }
                    }
                    else
                    {
                        tm.Available = 2; //no disponible porque esta asignado
                    }

                    l.Add(tm);
                }
            }

            return l;
        }












        public static void DeleteToolByToolkit( string ToolKitCode, string ToolCode)
        {
            dbClientGrana.GetCommand(storeProcedureDeleteToolInToolkit).Parameters["@ToolkitCode"].Value = ToolKitCode;
            dbClientGrana.GetCommand(storeProcedureDeleteToolInToolkit).Parameters["@ToolCode"].Value = ToolCode;         
            //dbClientGrana.GetCommand(storeProcedureDeleteToolInToolkit).Parameters["@Measure"].Value = Measure;         
            dbClientGrana.GetCommand(storeProcedureDeleteToolInToolkit).ExecuteNonQuery();
        }





        public static void DeleteDeactivatedTool(string ToolCode)
        {
            dbClientGrana.GetCommand(storeProcedureDeleteToolKit).Parameters["@ToolCode"].Value = ToolCode;

            dbClientGrana.GetCommand(storeProcedureDeleteToolKit).ExecuteNonQuery();
        }





        public static string GetToolKitCodesPerTool(string ToolCode)
        {
            dbClientGrana.GetCommand(storeProcedureGetToolKitCodesPerTool).Parameters["@ToolCode"].Value = ToolCode;

            string listCodes = "";

            using (var reader = dbClientGrana.GetCommand(storeProcedureGetToolKitCodesPerTool).ExecuteReader())
            {
                while (reader.Read())
                {
                    string Code = "";

                    Code = reader["ToolKitCode"].ToString();
                    listCodes = $"{listCodes},{Code}";
                }

            }

            return listCodes;


        }






        public static void DeleteToolkit(string ToolkitCode)
        {
            dbClientGrana.GetCommand(storeProcedureDeleteToolKit).Parameters["@ToolKitCode"].Value = ToolkitCode;

            dbClientGrana.GetCommand(storeProcedureDeleteToolKit).ExecuteNonQuery();
            
        }



        public static void DeactivateToolFromToolKitCatalog(string code)
        {
            dbClientGrana.GetCommand(storeProcedureDeactiveToolFromToolkitCatalog).Parameters["@ToolCode"].Value = code;

            dbClientGrana.GetCommand(storeProcedureDeactiveToolFromToolkitCatalog).ExecuteNonQuery();
            
        }



        //Valor default de prueba DWF
        public static void InsToolKit(string ToolKitCode, string ToolCode, int Use=1)
        {
            dbClientGrana.GetCommand(storeProcedureInsToolKit).Parameters["@ToolKitCode"].Value = ToolKitCode;
            dbClientGrana.GetCommand(storeProcedureInsToolKit).Parameters["@ToolCode"].Value = ToolCode;
            dbClientGrana.GetCommand(storeProcedureInsToolKit).Parameters["@Measure"].Value = Use;
            dbClientGrana.GetCommand(storeProcedureInsToolKit).ExecuteNonQuery();          
        }


        public static bool InsToolKitCatalog(string ToolKitCode, string MachineCode)
        {
            dbClientGrana.GetCommand(storeProcedureInsToolKitCode).Parameters["@ToolKitCode"].Value = ToolKitCode;
            dbClientGrana.GetCommand(storeProcedureInsToolKitCode).Parameters["@MachineCode"].Value = MachineCode;

            var result = dbClientGrana.GetCommand(storeProcedureInsToolKitCode).ExecuteScalar().ToString();

            return (result.ToString() == "1") ? true : false;


        }


        public static List<ToolModel> GetToolsFromToolKit(string toolKitCode)
        {
            List<ToolModel> ToolsFromToolKit = new List<ToolModel>();

            dbClientGrana.GetCommand(storeProcedureGetToolKit).Parameters["@ToolKitCode"].Value = toolKitCode;

            using (var reader = dbClientGrana.GetCommand(storeProcedureGetToolKit).ExecuteReader())
            {
                while (reader.Read())
                {
                    ToolModel tm = new ToolModel();
                    //Aqui se cambio --tener cuidado en proudctivo DWF
                    tm.Code = reader["Code"].ToString();
                    tm.Type = reader["Name"].ToString();
                    tm.CalibrationDate = DateTime.Parse(reader["CalibrationDate"].ToString());
                    tm.ExpirationDate = DateTime.Parse(reader["ExpirationDate"].ToString());
                    tm.Measure =  reader.GetSchemaTable().Select("ColumnName='Measure'") == null ? Boolean.Parse(reader["Measure"].ToString()) : false;
                    tm.Shared = reader["Shared"].ToString();
                    ToolsFromToolKit.Add(tm);
                }

            }

            return ToolsFromToolKit;




        }


        public static List<ToolModel> GetToolsFromToolKit123(string toolKitCode)
        {
            List<ToolModel> ToolsFromToolKit = new List<ToolModel>();

            dbClientGrana.GetCommand(storeProcedureGetToolKit).Parameters["@ToolKitCode"].Value = toolKitCode;

            using (var reader = dbClientGrana.GetCommand(storeProcedureGetToolKit).ExecuteReader())
            {
                while (reader.Read())
                {
                    ToolModel tm = new ToolModel();
                    //Aqui se cambio --tener cuidado en proudctivo DWF
                    tm.Code = reader["Code"].ToString();
                    tm.Type = reader["Name"].ToString();
                    tm.CalibrationDate = DateTime.Parse(reader["CalibrationDate"].ToString());
                    tm.ExpirationDate = DateTime.Parse(reader["ExpirationDate"].ToString());
                    tm.Measure = reader["Measure"].ToString() == "1" ? true : false;
                    tm.Shared = reader["Shared"].ToString();
                    ToolsFromToolKit.Add(tm);
                }

            }

            return ToolsFromToolKit;




        }


        public static List<SelectListItem> GetToolKitCodes(string MachineCode)
        {
            var list = new List<SelectListItem>();
            dbClientGrana.GetCommand(storeProcedureGetToolKitCodesByIdMachine).Parameters["@MachineCode"].Value = MachineCode;

            using (var reader = dbClientGrana.GetCommand(storeProcedureGetToolKitCodesByIdMachine).ExecuteReader())
            {
                while (reader.Read())
                {
                    SelectListItem ttm = new SelectListItem();

                    ttm.Text = reader["ToolKitCode"].ToString();
                    ttm.Value = reader["ToolKitCode"].ToString();

                    list.Add(ttm);

                }


            }

            return list;
        }


        public static int ActivateTool(string code)
        {
            dbClientGrana.GetCommand(storeProcedureActivateTool).Parameters["@Code"].Value = code;

            dbClientGrana.GetCommand(storeProcedureActivateTool).ExecuteNonQuery();

            return Int32.Parse(dbClientGrana.GetCommand(storeProcedureActivateTool).Parameters["@Success"].Value.ToString());
        }


        public static List<ToolModel> GetDeactivatedTools()
        {
            List<ToolModel> DeactivatedTools = new List<ToolModel>();

            using (var reader = dbClientGrana.GetCommand(storeProcedureQryDeactivatedTools).ExecuteReader())
            {
                while (reader.Read())
                {
                    ToolModel tm = new ToolModel();

                    tm.Code = reader["Code"].ToString();
                    tm.Type = reader["TypeName"].ToString();
                    tm.CalibrationDate = DateTime.Parse(reader["CalibrationDate"].ToString());
                    tm.ExpirationDate = DateTime.Parse(reader["ExpirationDate"].ToString());
                    tm.Machine = reader["MachineName"].ToString();

                    bool active = Boolean.Parse(reader["Active"].ToString());
                    if (tm.Machine.Equals(String.Empty))
                    {
                        if (active)
                        {
                            tm.Available = 1; //disponible
                        }
                        else
                        {
                            tm.Available = 3; //de baja
                        }
                    }
                    else
                    {
                        tm.Available = 2; //no disponible porque esta asignado
                    }

                    DeactivatedTools.Add(tm);
                }
            }

            return DeactivatedTools;

        }








        public static List<ToolTypeModel> GetToolTypes()
        {
            List<ToolTypeModel> l = new List<ToolTypeModel>();

            using (var reader = dbClientGrana.GetCommand(storeProcedureQryToolTypes).ExecuteReader())
            {
                while (reader.Read())
                {
                    ToolTypeModel ttm = new ToolTypeModel();

                    ttm.ID = Int32.Parse(reader["idType"].ToString());
                    ttm.Name = reader["Name"].ToString();
                    ttm.Active = Boolean.Parse(reader["Active"].ToString());

                    l.Add(ttm);
                }
            }

            return l;
        }

        public static int InsertToolType(string name)
        {
            dbClientGrana.GetCommand(storeProcedureInsToolType).Parameters["@Name"].Value = name;

            dbClientGrana.GetCommand(storeProcedureInsToolType).ExecuteNonQuery();

            return Int32.Parse(dbClientGrana.GetCommand(storeProcedureInsToolType).Parameters["@Success"].Value.ToString());
        }

        public static int DeleteToolType(int id)
        {
            dbClientGrana.GetCommand(storeProcedureDelToolType).Parameters["@Id"].Value = id.ToString();

            dbClientGrana.GetCommand(storeProcedureDelToolType).ExecuteNonQuery();

            return Int32.Parse(dbClientGrana.GetCommand(storeProcedureDelToolType).Parameters["@Success"].Value.ToString());
        }








        public static List<ToolModel> GetTools()
        {
            List<ToolModel> l = new List<ToolModel>();

            using (var reader = dbClientGrana.GetCommand(storeProcedureQryTools).ExecuteReader())
            {
                while (reader.Read())
                {
                    ToolModel tm = new ToolModel();

                    tm.Code = reader["Code"].ToString();
                    tm.Type = reader["TypeName"].ToString();
                    tm.CalibrationDate = DateTime.Parse(reader["CalibrationDate"].ToString());
                    tm.ExpirationDate = DateTime.Parse(reader["ExpirationDate"].ToString());
                    tm.Machine = reader["MachineName"].ToString();
                    tm.Plantilla = reader["Plantilla"].ToString();
                    bool active = Boolean.Parse(reader["Active"].ToString());
                    if (tm.Machine.Equals(String.Empty))
                    {
                        if (active)
                        {
                            tm.Available = 1; //disponible
                        }
                        else
                        {
                            tm.Available = 3; //de baja
                        }
                    }
                    else
                    {
                        tm.Available = 2; //no disponible porque esta asignado
                    }

                    l.Add(tm);
                }
            }

            return l;
        }

        public static int InsertTool(string code, string typeName, DateTime expirationDate, DateTime calibrationDate)
        {
            dbClientGrana.GetCommand(storeProcedureInsTool).Parameters["@Code"].Value = code;
            dbClientGrana.GetCommand(storeProcedureInsTool).Parameters["@TypeName"].Value = typeName;
            dbClientGrana.GetCommand(storeProcedureInsTool).Parameters["@ExpirationDate"].Value = expirationDate;
            dbClientGrana.GetCommand(storeProcedureInsTool).Parameters["@CalibrationDate"].Value = calibrationDate;

            dbClientGrana.GetCommand(storeProcedureInsTool).ExecuteNonQuery();

            return Int32.Parse(dbClientGrana.GetCommand(storeProcedureInsTool).Parameters["@Success"].Value.ToString());
        }

        public static int DeleteTool(string code)
        {
            dbClientGrana.GetCommand(storeProcedureDelTool).Parameters["@Code"].Value = code;

            dbClientGrana.GetCommand(storeProcedureDelTool).ExecuteNonQuery();

            return Int32.Parse(dbClientGrana.GetCommand(storeProcedureDelTool).Parameters["@Success"].Value.ToString());
        }

        public static List<ToolModel> GetToolsByMachineCode(string code)
        {
            List<ToolModel> l = new List<ToolModel>();

            dbClientGrana.GetCommand(storeProcedureQryToolsByMachineCode).Parameters["@MachineCode"].Value = code;

            using (var reader = dbClientGrana.GetCommand(storeProcedureQryToolsByMachineCode).ExecuteReader())
            {
                while (reader.Read())
                {
                    ToolModel tm = new ToolModel();

                    tm.Code = reader["Code"].ToString();
                    tm.Type = reader["Name"].ToString();
                    tm.CalibrationDate = DateTime.Parse(reader["CalibrationDate"].ToString());
                    tm.ExpirationDate = DateTime.Parse(reader["ExpirationDate"].ToString());
                    tm.Measure = Boolean.Parse(reader["Measure"].ToString());
                    tm.Shared = reader["Shared"].ToString();
                    l.Add(tm);
                }

            }

            return l;
        }



        //Se modifico este SP en BD
        public static List<ToolModel> GetAvailableTools()
        {
            List<ToolModel> l = new List<ToolModel>();

            using (var reader = dbClientGrana.GetCommand(storeProcedureQryAvailableTools).ExecuteReader())
            {
                while (reader.Read())
                {
                    ToolModel tm = new ToolModel();

                    tm.Code = reader["Code"].ToString();
                    tm.Type = reader["TypeName"].ToString();
                    tm.CalibrationDate = DateTime.Parse(reader["CalibrationDate"].ToString());
                    tm.ExpirationDate = DateTime.Parse(reader["ExpirationDate"].ToString());

                    l.Add(tm);
                }

            }

            return l;
        }

        public static int InsertMachineTool(string toolCode, string machineCode, int measure)
        {
            dbClientGrana.GetCommand(storeProcedureInsMachineTool).Parameters["@ToolCode"].Value = toolCode;
            dbClientGrana.GetCommand(storeProcedureInsMachineTool).Parameters["@MachineCode"].Value = machineCode;
            dbClientGrana.GetCommand(storeProcedureInsMachineTool).Parameters["@Measure"].Value = measure;

            dbClientGrana.GetCommand(storeProcedureInsMachineTool).ExecuteNonQuery();

            return Int32.Parse(dbClientGrana.GetCommand(storeProcedureInsMachineTool).Parameters["@Success"].Value.ToString());
        }

        public static int DeleteMachineTool(string toolCode, string machineCode)
        {
            dbClientGrana.GetCommand(storeProcedureDelMachineTool).Parameters["@ToolCode"].Value = toolCode;
            dbClientGrana.GetCommand(storeProcedureDelMachineTool).Parameters["@MachineCode"].Value = machineCode;

            dbClientGrana.GetCommand(storeProcedureDelMachineTool).ExecuteNonQuery();

            return Int32.Parse(dbClientGrana.GetCommand(storeProcedureDelMachineTool).Parameters["@Success"].Value.ToString());
        }

        public static int UpdateTool(string code, DateTime calibrationDate, DateTime expirationDate)
        {
            dbClientGrana.GetCommand(storeProcedureUpdTool).Parameters["@Code"].Value = code;
            dbClientGrana.GetCommand(storeProcedureUpdTool).Parameters["@CalibrationDate"].Value = calibrationDate;
            dbClientGrana.GetCommand(storeProcedureUpdTool).Parameters["@ExpirationDate"].Value = expirationDate;

            dbClientGrana.GetCommand(storeProcedureUpdTool).ExecuteNonQuery();

            return Int32.Parse(dbClientGrana.GetCommand(storeProcedureUpdTool).Parameters["@Success"].Value.ToString());
        }

        public static List<string> GetUserPrivileges(string user, string pass)
        {
            List<string> l = new List<string>();

            dbClientGrana.GetCommand(storeProcedureQryUserPrivileges).Parameters["@User"].Value = user;
            dbClientGrana.GetCommand(storeProcedureQryUserPrivileges).Parameters["@Pass"].Value = pass;

            using (var reader = dbClientGrana.GetCommand(storeProcedureQryUserPrivileges).ExecuteReader())
            {
                while (reader.Read())
                {
                    string s = reader["Name"].ToString();
                    l.Add(s);
                }
            }
            return l;
        }

        public static void InsertLogEntry(string user, string movement)
        {
            dbClientGrana.GetCommand(storeProcedureInsLog).Parameters["@User"].Value = user;
            dbClientGrana.GetCommand(storeProcedureInsLog).Parameters["@Movement"].Value = movement;

            dbClientGrana.GetCommand(storeProcedureInsLog).ExecuteNonQuery();
        }

        public static int ShareMachineTool(string toolCode, string machineCode,bool meassure)
        {
            dbClientGrana.GetCommand(storeProcedureShareMachineTool).Parameters["@ToolCode"].Value = toolCode;
            dbClientGrana.GetCommand(storeProcedureShareMachineTool).Parameters["@MachineCode"].Value = machineCode;
            dbClientGrana.GetCommand(storeProcedureShareMachineTool).Parameters["@Measure"].Value = meassure;

            dbClientGrana.GetCommand(storeProcedureShareMachineTool).ExecuteNonQuery();

            return Int32.Parse(dbClientGrana.GetCommand(storeProcedureShareMachineTool).Parameters["@Success"].Value.ToString());
        }







    }
}