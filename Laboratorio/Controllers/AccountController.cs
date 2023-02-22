using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Laboratorio.Models.DataAccess;

namespace Laboratorio.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogIn(string usuario, string contrasena)
        {
            List<string> l = DataAccess.GetUserPrivileges(usuario, contrasena);

            if (l.Count == 0)
            {
                return RedirectToAction("Index", "ToolTypes", new { validation = "Usuario/Constraseña incorrecta." });
            }
            else
            {
                foreach (string s in l)
                {
                    if (s.Equals("Instrumentos"))
                    {
                        FormsAuthentication.SetAuthCookie(usuario, false /* createPersistentCookie */);
                        return RedirectToAction("Index", "ToolTypes");
                    }
                }

                return RedirectToAction("Index", "ToolTypes", new { validation = "No tiene los privilegios para usar esta aplicación." });
            }

            
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "ToolTypes");
        }

    }
}
