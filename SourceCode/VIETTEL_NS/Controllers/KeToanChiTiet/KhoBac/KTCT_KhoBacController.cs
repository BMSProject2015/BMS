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

namespace VIETTEL.Controllers.KeToanChiTiet.KhoBac
{
    public class KTCT_KhoBacController : Controller
    {
        //
        // GET: /KTCT_KhoBac/
        public string sViewPath = "~/Views/KeToanChiTiet/KhoBac/";
        public ActionResult Index()
        {
            return View(sViewPath + "KTCT_KhoBac_Index.aspx");
        }
    }
}
