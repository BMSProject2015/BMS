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
    public class VatTu_SoSanhGiaController : Controller
    {
        //
        // GET: /SanPham/
        public string sViewPath = "~/Views/SanPham/SoSanhGia/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_SanPham_ChiTietGia", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["sTen"] = Request.Form[ParentID + "_sTen"];
            ViewData["sTenSanPham"] = Request.Form[ParentID + "_sTenSanPham"];
            ViewData["KieuSoSanh"] = Request.Form[ParentID + "_KieuSoSanh"];
            ViewData["Nam"] = Request.Form[ParentID + "_Nam"];
            ViewData["NamSoSanh"] = Request.Form[ParentID + "_NamSoSanh"];
            ViewData["LoaiNganSach"] = Request.Form[ParentID + "_LoaiNganSach"];
            return View(sViewPath + "VatTu.aspx");
        }
        public static DataTable Get_DanhSachVatTu(String sTen, String sTenSanPham, int Nam, int NamSoSanh, int KieuSoSanh, String LoaiNganSach, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "";
            if (!String.IsNullOrEmpty(sTen))
            {
                DK += " AND DM_VatTu.sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", "%" + sTen + "%");
            }
            if (Nam > 0)
            {
                DK += " AND YEAR(ISNULL(DM_SanPham_DanhMucGia.dNgayTao,GETDATE())) <= @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }
            if (!String.IsNullOrEmpty(sTenSanPham))
            {
                DK += " AND DM_SanPham.sTen LIKE @sTenSanPham";
                cmd.Parameters.AddWithValue("@sTenSanPham", "%" + sTenSanPham + "%");
            }
            String SQL = String.Format(@"SELECT tbl.*, Nam1 = 0, Nam2 = 0, Nam3 = 0, Nam4 = 0, Nam5 = 0, SoSanh = 0 
                                        FROM (SELECT DISTINCT DM_VatTu.iID_MaVatTu, DM_VatTu.sTen, DM_SanPham_DanhMucGia.iDM_MaDonViTinh, DM_SanPham_DanhMucGia.iID_MaSanPham
                                                        , DM_SanPham.sTen AS sTenSanPham
                                                        ,(SELECT DC_DanhMuc.sTen FROM DC_DanhMuc WHERE DM_SanPham_DanhMucGia.iDM_MaDonViTinh = DC_DanhMuc.iID_MaDanhMuc) AS sTen_DonViTinh
                                                        
                                            FROM DM_SanPham_DanhMucGia INNER JOIN DM_SanPham_ChiTietGia ON DM_SanPham_ChiTietGia.iTrangThai = 1
                                            AND DM_SanPham_DanhMucGia.iID_MaChiTietGia = DM_SanPham_ChiTietGia.iID_MaChiTietGia AND DM_SanPham_ChiTietGia.iID_LoaiDonVi = 3
                                            INNER JOIN DM_SanPham ON DM_SanPham_ChiTietGia.iID_MaSanPham = DM_SanPham.iID_MaSanPham AND DM_SanPham.iTrangThai= 1
                                            INNER JOIN DM_VatTu ON DM_VatTu.iID_MaVatTu = DM_SanPham_DanhMucGia.iID_MaVatTu AND DM_VatTu.iTrangThai = 1 
                                            WHERE DM_SanPham_DanhMucGia.iTrangThai = 1 {0}) AS tbl", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sTen", Trang, SoBanGhi);
            cmd.Dispose();
            foreach (DataRow r in vR.Rows)
            {
                String iID_MaSanPham = Convert.ToString(r["iID_MaSanPham"]);
                String iID_MaVatTu = Convert.ToString(r["iID_MaVatTu"]);
                DataTable dtGia = getGiaVatTu(iID_MaVatTu, iID_MaSanPham, Nam, NamSoSanh, KieuSoSanh, LoaiNganSach);
                if (dtGia.Rows.Count > 0)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        r["Nam" + i.ToString()] = dtGia.Rows[0]["Nam" + i.ToString()];
                    }
                    r["SoSanh"] = dtGia.Rows[0]["SoSanh"];
                }
            }
            return vR;
        }

        public static int Get_DanhSachVatTu_Count(String sTen, String sTenSanPham, int Nam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "";
            if (!String.IsNullOrEmpty(sTen))
            {
                DK += " AND DM_VatTu.sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", "%" + sTen + "%");
            }
            if (Nam > 0)
            {
                DK += " AND YEAR(ISNULL(DM_SanPham_DanhMucGia.dNgayTao,GETDATE())) <= @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }
            if (!String.IsNullOrEmpty(sTenSanPham))
            {
                DK += " AND DM_SanPham.sTen LIKE @sTenSanPham";
                cmd.Parameters.AddWithValue("@sTenSanPham", "%" + sTenSanPham + "%");
            }
            String SQL = String.Format(@"SELECT * FROM (SELECT DISTINCT DM_VatTu.iID_MaVatTu, DM_VatTu.sTen, DM_SanPham_DanhMucGia.iDM_MaDonViTinh, DM_SanPham_DanhMucGia.iID_MaSanPham
                                            FROM DM_SanPham_DanhMucGia INNER JOIN DM_SanPham_ChiTietGia ON DM_SanPham_ChiTietGia.iTrangThai = 1
                                            AND DM_SanPham_DanhMucGia.iID_MaChiTietGia = DM_SanPham_ChiTietGia.iID_MaChiTietGia AND DM_SanPham_ChiTietGia.iID_LoaiDonVi = 3
                                            INNER JOIN DM_SanPham ON DM_SanPham_ChiTietGia.iID_MaSanPham = DM_SanPham.iID_MaSanPham AND DM_SanPham.iTrangThai= 1
                                            INNER JOIN DM_VatTu ON DM_VatTu.iID_MaVatTu = DM_SanPham_DanhMucGia.iID_MaVatTu AND DM_VatTu.iTrangThai = 1 
                                            WHERE DM_SanPham_DanhMucGia.iTrangThai = 1 {0}) AS tbl", DK);
            cmd.CommandText = SQL;  
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR.Rows.Count;
        }
        private static DataTable getGiaVatTu(String iID_MaVatTu, String iID_MaSanPham, int Nam, int NamSoSanh, int KieuSoSanh, String LoaiNganSach)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String HamSQL = "";
            if (KieuSoSanh == 1) HamSQL = "MAX"; else HamSQL = "AVG";
            if (String.IsNullOrEmpty(LoaiNganSach)) LoaiNganSach = "0";
            String queryNam = "";
            for (int i = 1; i <= 5; i++)
            {
                String DauPhay = ",";
                if (i == 1) DauPhay = "";
                queryNam += DauPhay + "Nam" + i.ToString() + "= (SELECT " + HamSQL + "(rTien_CTC_DeNghi) FROM DM_SanPham_DanhMucGia INNER JOIN DM_SanPham_ChiTietGia ON "
                             + "DM_SanPham_ChiTietGia.iTrangThai = 1 AND DM_SanPham_ChiTietGia.iID_MaChiTietGia = DM_SanPham_DanhMucGia.iID_MaChiTietGia "
                             + "WHERE DM_SanPham_DanhMucGia.iTrangThai = 1 AND DM_SanPham_DanhMucGia.bNganSach = @LoaiNganSach "
                             + "AND DM_SanPham_ChiTietGia.iID_LoaiDonVi = 3 AND DM_SanPham_ChiTietGia.iID_MaSanPham = @iID_MaSanPham  AND DM_SanPham_DanhMucGia.iID_MaVatTu = @iID_MaVatTu "
                             + "AND YEAR(ISNULL(DM_SanPham_ChiTietGia.dNgayTao,GETDATE())) = @Nam" + i.ToString() + " )";
                cmd.Parameters.AddWithValue("@Nam" + i.ToString(), Nam - 5 + i);
            }
            int iSoSanh = 5 + NamSoSanh - Nam;
            String SQL = String.Format(@"SELECT tbl.*, SoSanh = CASE WHEN Nam{1} <> 0 THEN (Nam5/Nam{1})*100 ELSE 0 END FROM (SELECT {0}) AS tbl", queryNam, iSoSanh);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
            cmd.Parameters.AddWithValue("@LoaiNganSach", LoaiNganSach);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
    }
}
