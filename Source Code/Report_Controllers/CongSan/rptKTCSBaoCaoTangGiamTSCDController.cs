using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Globalization;
namespace VIETTEL.Report_Controllers.CongSan
{
    public class rptKTCSBaoCaoTangGiamTSCDController : Controller
    {
        //
        // GET: /rptKTCSBaoCaoTangGiamTSCD/

        public string sViewPath = "~/Report_Views/";
        String sFilePath = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD.xls";
        String sFilePath1 = "/Report_ExcelFrom/CongSan/rptKTCS_BaoCaoTangGiamTSCD_A3.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/CongSan/rptKTCS_BaoCaoTangGiamTSCD.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

    }
}
