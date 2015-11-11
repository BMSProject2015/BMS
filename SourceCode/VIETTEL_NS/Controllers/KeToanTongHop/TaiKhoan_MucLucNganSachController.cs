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

namespace VIETTEL.Controllers.KeToanTongHop
{
    public class TaiKhoan_MucLucNganSachController : Controller
    {
        //
        // GET: /TaiKhoan_MucLucNganSach/
        public string sViewPath = "~/Views/KeToanTongHop/DanhMuc/TaiKhoanMucLucNganSach/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoan_MucLucNganSach", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            
            return View(sViewPath + "TaiKhoan_MucLucNganSach_Index.aspx");
        }
    }
}
