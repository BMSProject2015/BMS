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
    public class ChangeDirectoryV2Controller : Controller
    {
        //
        // GET: /ChangeDirectory/

        public string sViewPath = "~/Views/TuLieu/";

        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "ChangeDirectoryV1.aspx");
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult submit(string ParentID, string contronllerName)
        {
            try
            {
                string iID_MaLoaiThuMuc = Request.Form[ParentID + "_iID_MaLoaiThuMuc"];
                return RedirectToAction("Index", "ChangeDirectoryV2", new { iID_MaLoaiThuMuc = iID_MaLoaiThuMuc, contronllerName = contronllerName });

            }
            catch
            {
                return View(sViewPath + "ChangeDirectoryV1.aspx");
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
        [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult SaveSubmit(string ParentID, string contronllerName)
        {
            try
            {
                string iID_MaThuMucTaiLieu = Convert.ToString(Request.Form[ParentID + "_iID_ThuMucChon"]);
                if (!string.IsNullOrEmpty(iID_MaThuMucTaiLieu))
                {
                    char[] spliter = new char[] { '_' };
                    string[] ids = iID_MaThuMucTaiLieu.Split(spliter, StringSplitOptions.None);
                    if (ids != null && ids.Length != 0)
                    {
                        int mathumuc = Convert.ToInt32(ids[0]);
                        string maloaithumuc = ids[1];
                        TuLieuLichSuModels.CapNhatThuMucLuu(maloaithumuc, mathumuc);
                       
                    }
                }
                return RedirectToAction("Edit", contronllerName);
            }
            catch
            {
                return View(sViewPath + "ChangeDirectoryV1.aspx");
            }
        }
        
    }
}
