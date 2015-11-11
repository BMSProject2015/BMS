﻿using System;
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

namespace VIETTEL.Report_Controllers.QLDA
{
    public class QLDA_ReportController : Controller
    {
        //
        // GET: /QLDA_Report/
        public string sViewPath = "~/Report_Views/QLDA/";
        public ActionResult Index()
        {
            return View(sViewPath + "Report_Index.aspx");
        }
    }
}
