using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using VIETTEL.Models;
//using DomainModel.Controls;
//using DomainModel.Abstract;
//using System.Collections.Specialized;
//using VIETTEL.Models;

namespace VIETTEL.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            String MaND = User.Identity.Name;
            NguoiDungCauHinhModels.MaNguoiDung = MaND;
            NguoiDungCauHinhModels.iNamLamViec = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Blank()
        {
            return View();
        }
    }
}
