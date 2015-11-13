using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;

namespace VIETTEL.Report_Controllers.NhanSu
{
    public class rptNhanSu_DanhSachCapBacCanBo : Controller
    {
     
        public ActionResult Index()
        {
            return View();
        }
       // public DataTable NhanSu_DanhSachCapBacCanBo(String TuTuoi,String DenTuoi,String DoiTuong,String ChucVu)

    }
}
