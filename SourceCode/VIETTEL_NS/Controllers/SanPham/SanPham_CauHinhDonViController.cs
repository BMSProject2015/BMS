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
using System.Collections;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Controllers.SanPham
{
    public class SanPham_CauHinhDonViController : Controller
    {
        //
        // GET: /SanPham/
        public string sViewPath = "~/Views/SanPham/CauHinhDonVi/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_SanPham_CauHinhDonVi", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "Index.aspx");
        }
        /// <summary>
        /// Lưu cấu hình
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, " DM_SanPham_CauHinhDonVi", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String SQL = "SELECT COUNT(iID_MaDonVi) AS SoBanGhi FROM DM_SanPham_CauHinhDonVi WHERE iID_MaDonVi = @iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            int SoBanGhi = (int)Connection.GetValue(cmd, -1);
            if (SoBanGhi > 0)
            {
                String DelSQL = "DELETE DM_SanPham_CauHinhDonVi WHERE iID_MaDonVi = @iID_MaDonVi";
                cmd.CommandText = DelSQL;
                SqlConnection conn = new SqlConnection(Connection.ConnectionString);
                conn.Open();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                conn.Dispose();
            }
            
            Bang bang = new Bang("DM_SanPham_CauHinhDonVi");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            bang.DuLieuMoi = true;
            bang.Save();
            return RedirectToAction("Index", new { iID_MaDonVi = iID_MaDonVi });
        }
        public static DataTable LayCauHinh(String iID_MaDonVi)
        {
            String SQL = "SELECT * FROM DM_SanPham_CauHinhDonVi WHERE iID_MaDonVi = @iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            DataTable dt = Connection.GetDataTable(cmd);
            return dt;
        }
    }
}
