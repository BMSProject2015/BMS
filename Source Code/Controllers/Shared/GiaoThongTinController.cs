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
    public class GiaoThongTinController : Controller
    {
        public string sViewPath = "~/Views/Shared/GiaoThongTin/";

        
        [Authorize]
        public ActionResult Index(String TenBang, String TenTruongKhoa, String GiaTriKhoa, String MaNhomNguoiDung_DuocGiao)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, TenBang, "Responsibility") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.KiemTraQuyenXemTin(User.Identity.Name, TenBang, TenTruongKhoa, GiaTriKhoa))
            {
                String MaNhomNguoiDung_DuocGiao_Tin = "";
                String MaNguoiDung_DuocGiao = "";
                if (String.IsNullOrEmpty(MaNhomNguoiDung_DuocGiao))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = String.Format("SELECT iID_MaNhomNguoiDung_DuocGiao, sID_MaNguoiDung_DuocGiao FROM {0} WHERE {1}=@{1}", TenBang, TenTruongKhoa);
                    cmd.Parameters.AddWithValue("@" + TenTruongKhoa, GiaTriKhoa);
                    DataTable dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                    MaNhomNguoiDung_DuocGiao_Tin = Convert.ToString(dt.Rows[0]["iID_MaNhomNguoiDung_DuocGiao"]);
                    MaNhomNguoiDung_DuocGiao = Convert.ToString(dt.Rows[0]["iID_MaNhomNguoiDung_DuocGiao"]);
                    MaNguoiDung_DuocGiao = Convert.ToString(dt.Rows[0]["sID_MaNguoiDung_DuocGiao"]);
                    dt.Dispose();
                }
                ViewData["MaNhomNguoiDung_DuocGiao_Tin"] = MaNhomNguoiDung_DuocGiao_Tin;
                ViewData["MaNhomNguoiDung_DuocGiao"] = MaNhomNguoiDung_DuocGiao;
                ViewData["MaNguoiDung_DuocGiao"] = MaNguoiDung_DuocGiao;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(String TenBang, String TenTruongKhoa, String GiaTriKhoa, String MaNhomNguoiDung_DuocGiao, String MaNguoiDung_DuocGiao, String returnUrl)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, TenBang, "Responsibility") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.KiemTraQuyenXemTin(User.Identity.Name, TenBang, TenTruongKhoa, GiaTriKhoa))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = String.Format("UPDATE {0} SET iID_MaNhomNguoiDung_DuocGiao=@iID_MaNhomNguoiDung_DuocGiao, sID_MaNguoiDung_DuocGiao=@sID_MaNguoiDung_DuocGiao WHERE {1}=@{1}", TenBang, TenTruongKhoa);
                cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung_DuocGiao", MaNhomNguoiDung_DuocGiao);
                if (String.IsNullOrEmpty(MaNguoiDung_DuocGiao))
                {
                    cmd.Parameters.AddWithValue("@sID_MaNguoiDung_DuocGiao", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@sID_MaNguoiDung_DuocGiao", MaNguoiDung_DuocGiao);
                }
                cmd.Parameters.AddWithValue("@" + TenTruongKhoa, GiaTriKhoa);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            return Redirect(returnUrl);
        }
    }
}
