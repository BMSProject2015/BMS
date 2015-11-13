using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Controllers.CongSan
{
    public class KTCS_TaiSan_DonViController : Controller
    {
        //
        // GET: /KTCS_TaiSan_DonVi/
        public string sViewPath = "~/Views/CongSan/TaiSan_DonVi/";
        public ActionResult Index(String iID_MaTaiSan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_TaiSan", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaTaiSan"] = iID_MaTaiSan;
            return View(sViewPath + "KTCS_TaiSan_DonVi_Index.aspx");


        }

    }
}
