using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Web.Routing;

namespace Oneres.Controllers.Shared
{
    public class ChiaSeThongTinController : Controller
    {
        public string sViewPath = "~/Views/Shared/ChiaSeThongTin/";


        [Authorize]
        public ActionResult Index(String TenBang, String TenTruongKhoa, String GiaTriKhoa)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, TenBang, "Share") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.KiemTraQuyenXemTin(User.Identity.Name, TenBang, TenTruongKhoa, GiaTriKhoa))
            {
                return View(sViewPath + "Edit.aspx");
            }
            return null;
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(String TenBang, String TenTruongKhoa, String GiaTriKhoa, String MaNhomNguoiDung_Public, string returnUrl)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, TenBang, "Share") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.KiemTraQuyenXemTin(User.Identity.Name, TenBang, TenTruongKhoa, GiaTriKhoa))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = String.Format("UPDATE {0} SET bPublic=1, iID_MaNhomNguoiDung_Public=@iID_MaNhomNguoiDung_Public WHERE {1}=@{1}", TenBang, TenTruongKhoa);
                cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung_Public", MaNhomNguoiDung_Public);
                cmd.Parameters.AddWithValue("@" + TenTruongKhoa, GiaTriKhoa);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            return Redirect(returnUrl);
        }
    }
}
