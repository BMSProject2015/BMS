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
namespace VIETTEL.Report_Controllers.GIA
{
    public class GIA_ReportController : Controller
    {
        //
        // GET: /GIA_Report/
        public string sViewPath = "~/Report_Views/GIA/";
        public ActionResult Index()
        {
            return View(sViewPath + "Report_Index.aspx");
        }
        public ActionResult List()
        {
            return View(sViewPath + "List.aspx");
        }
        public static DataTable getDtDonVi(String MaND, String iID_LoaiDonVi)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT NS_DonVi.* FROM NS_DonVi INNER JOIN NS_NguoiDung_DonVi ON (NS_NguoiDung_DonVi.iID_MaDonVi=NS_DonVi.iID_MaDonVi) "
                       + " WHERE NS_NguoiDung_DonVi.iTrangThai=1 AND NS_DonVi.iTrangThai=1 AND NS_NguoiDung_DonVi.sMaNguoiDung=@sMaNguoiDung AND NS_DonVi.iNamLamViec_DonVi=@iNamLamViec"
                       + " AND NS_DonVi.iID_MaDonVi IN ( "
                       + " SELECT iID_MaDonVi FROM DM_SanPham_ChiTietGia WHERE iTrangThai = 1 AND iID_MaSanPham IN (SELECT DM_SanPham.iID_MaSanPham FROM DM_SanPham WHERE DM_SanPham.iTrangThai = 1) "
                       + " AND iID_LoaiDonVi = @iID_LoaiDonVi "
                       + ") "
                       + " ORDER BY NS_DonVi.iID_MaDonVi";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_LoaiDonVi", iID_LoaiDonVi);
            DataTable dtDonVi = Connection.GetDataTable(cmd);
            return dtDonVi;
        }
    }
}