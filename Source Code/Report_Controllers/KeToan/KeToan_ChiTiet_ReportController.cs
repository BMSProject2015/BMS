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

namespace VIETTEL.Report_Controllers.KeToan
{
    public class KeToan_ChiTiet_ReportController : Controller
    {
        public string sViewPath = "~/Report_Views/KeToan/";
        public ActionResult Index()
        {
            return View(sViewPath + "Report_ChiTiet_Index.aspx");
        }

    }
}
