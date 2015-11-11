using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;

namespace VIETTEL.Report_Controllers.KeToan
{
    public class rptKeToan_DanhMucTaiKhoanGiaiThichController : Controller
    {
        //
        // GET: /rptKeToan_DanhMucTaiKhoan/
        public string sViewPath = "~/Report_Views/KeToan/rptKeToan_DanhMucTaiKhoanGiaiThich.aspx";

        public ActionResult Index()
        {
            return View(sViewPath);
        }
        public ActionResult EditSubmit(String sKyHieu, String ControllerName)
        {
            String iID_MaTaiKhoan = Convert.ToString(Request.Form["MaTK"]);
            if (String.IsNullOrEmpty(iID_MaTaiKhoan))
            {
                iID_MaTaiKhoan = "001";
            }
            UPDATETK(sKyHieu, iID_MaTaiKhoan);
            return RedirectToAction("Index", ControllerName);
        }
        public static DataTable DanhSachTaiKhoan()
        {
            DataTable dt;
            SqlCommand cmd= new SqlCommand();
            String SQL = String.Format(@"   SELECT * FROM KT_TaiKhoanDanhMucChiTiet
   WHERE iTrangThai=1 AND SUBSTRING(sKyHieu,1,3)='313' AND bLaHangCha=0");
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static String sThamSo(String sKyHieu)
        {
            String SQL = @"SELECT sThamSo FROM KT_DanhMucThamSo WHERE sKyHieu=@sKyHieu AND iTrangThai=1 AND iNamLamViec=@iNamLamViec";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            String sThamSo = Connection.GetValueString(cmd, "").ToString();
            cmd.Dispose();
            return sThamSo;
        }
        public void UPDATETK(String sKyHieu,String iID_MaTaiKhoan)
        {
            String SQL = @"UPDATE KT_DanhMucThamSo SET sThamSo=@iID_MaTaiKhoan WHERE sKyHieu=@sKyHieu AND iNamLamViec=@iNamLamViec AND iTrangThai=1";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

    }
}
