using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel;
using DomainModel.Abstract;
using VIETTEL.Models;

namespace VIETTEL.Controllers.TuLieu
{
    public class ChangeDirectoryController : Controller
    {
        //
        // GET: /ChangeDirectory/

        public string sViewPath = "~/Views/TuLieu/";

        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "ChangeDirectory.aspx");
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult submit(string ParentID, string sessionName, string contronllerName )
        {
            try
            {
                // TODO: Add delete logic here
                NameValueCollection arrLoi = new NameValueCollection();
                String path = Convert.ToString(Request.Form[ParentID + "_path"]);
                string rootPath = System.Configuration.ConfigurationManager.AppSettings["RootPath"];
                string flask1 = @"\";
                string flask2 = @"\\";
                string newPath = path.Replace(flask1, flask2);
                if (newPath.Contains(rootPath))
                {
                    Session[sessionName] = path;
                }
                else
                {
                    arrLoi.Add("err_path", "Không cho phép truy cập thu mục!");

                }

                if (arrLoi.Count > 0)
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    return View(sViewPath + "ChangeDirectory.aspx", new { sessionName = sessionName, contronllerName = contronllerName });
                }
                else
                {
                    return RedirectToAction("Index", "ChangeDirectory", new { path = path, sessionName = sessionName, contronllerName = contronllerName });
                }
            }
            catch
            {
                return View(sViewPath + "ChangeDirectory.aspx", new {sessionName = sessionName, contronllerName = contronllerName });
            }
    }

    [Authorize]
        public ActionResult SubFoldersubmit( string path, string sessionName, string contronllerName)
        {
            try
            {
                // TODO: Add delete logic here
                Session[sessionName] = path;
                return RedirectToAction("Index", "ChangeDirectory", new { path = path, sessionName = sessionName, contronllerName = contronllerName });
            }
            catch
            {
                return View(sViewPath + "ChangeDirectory.aspx", new { sessionName = sessionName, contronllerName = contronllerName });
            }
        }

        [Authorize]
    public ActionResult SaveSubmit(string path, string sessionName, string contronllerName)
        {
            try
            {
                // TODO: Add delete logic here
                string flask1 = @"\";
                string flask2 = @"\\";
                string newPath = path.Replace(flask1, flask2);
                Session[sessionName] = newPath;
                return RedirectToAction("Edit", contronllerName, new { path = newPath });
            }
            catch
            {
                return View(sViewPath + "ChangeDirectory.aspx", new { path = path, sessionName = sessionName, contronllerName = contronllerName });
            }
        }

        
    }
}
