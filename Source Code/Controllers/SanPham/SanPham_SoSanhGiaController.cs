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
    public class SanPham_SoSanhGiaController : Controller
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
            ViewData["sMa"] = Request.Form[ParentID + "_sMa"];
            ViewData["Nam"] = Request.Form[ParentID + "_Nam"];
            ViewData["NamSoSanh"] = Request.Form[ParentID + "_NamSoSanh"];
            return View(sViewPath + "SanPham.aspx");
        }
        public static DataTable Get_DanhSachSanPham(String sTen, int Nam, int NamSoSanh, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "";
            if (!String.IsNullOrEmpty(sTen))
            {
                DK += " AND DM_SanPham.sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", "%" + sTen + "%");
            }
            if (Nam > 0)
            {
                DK += " AND YEAR(ISNULL(DM_SanPham_ChiTietGia.dNgayTao,GETDATE())) <= @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }
            String SQL = String.Format(@"SELECT tbl.*, Nam1 = 0, Nam2 = 0, Nam3 = 0, Nam4 = 0, Nam5 = 0, SoSanh = 0
                                         FROM (SELECT DISTINCT DM_SanPham.sTen, DM_SanPham.iDM_MaDonViTinh, DM_SanPham.iID_MaSanPham
                                            ,(SELECT DC_DanhMuc.sTen FROM DC_DanhMuc WHERE DM_SanPham.iDM_MaDonViTinh = DC_DanhMuc.iID_MaDanhMuc) AS sTen_DonViTinh
                                            FROM DM_SanPham_ChiTietGia INNER JOIN DM_SanPham ON DM_SanPham_ChiTietGia.iID_MaSanPham = DM_SanPham.iID_MaSanPham
                                            AND DM_SanPham_ChiTietGia.iTrangThai = 1 AND DM_SanPham.iTrangThai = 1
                                            WHERE DM_SanPham_ChiTietGia.iID_LoaiDonVi = 3 {0}) AS tbl", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sTen", Trang, SoBanGhi);
            cmd.Dispose();
            foreach (DataRow r in vR.Rows)
            {
                String iID_MaSanPham = Convert.ToString(r["iID_MaSanPham"]);
                DataTable dtGia = getGiaSanPham(iID_MaSanPham, Nam, NamSoSanh);
                if (dtGia.Rows.Count>0)
                {
                    for (int i = 1; i <= 5; i ++ )
                    {
                        r["Nam" + i.ToString()] = dtGia.Rows[0]["Nam" + i.ToString()];
                    }
                    r["SoSanh"] = dtGia.Rows[0]["SoSanh"];
                }
            }
            return vR;
        }

        public static int Get_DanhSachSanPham_Count(String sTen, int Nam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "";
            if (!String.IsNullOrEmpty(sTen))
            {
                DK += " AND DM_SanPham.sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", "%" + sTen + "%");
            }
            if (Nam > 0)
            {
                DK += " AND YEAR(ISNULL(DM_SanPham_ChiTietGia.dNgayTao,GETDATE())) <= @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }
            String SQL = String.Format(@"SELECT DISTINCT DM_SanPham.sTen, DM_SanPham.iDM_MaDonViTinh, DM_SanPham.iID_MaSanPham
                                            FROM DM_SanPham_ChiTietGia INNER JOIN DM_SanPham ON DM_SanPham_ChiTietGia.iID_MaSanPham = DM_SanPham.iID_MaSanPham
                                            AND DM_SanPham_ChiTietGia.iTrangThai = 1 AND DM_SanPham.iTrangThai = 1
                                            WHERE DM_SanPham_ChiTietGia.iID_LoaiDonVi = 3 {0}", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR.Rows.Count;
        }
        private static DataTable getGiaSanPham(String iID_MaSanPham, int Nam, int NamSoSanh)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String queryNam = "";
            for (int i = 1; i <= 5; i++)
            {
                String DauPhay = ",";
                if (i == 1) DauPhay = "";
                queryNam += DauPhay + "Nam" + i.ToString() + "= (SELECT TOP 1 rTien_CTC_DeNghi FROM DM_SanPham_DanhMucGia INNER JOIN DM_SanPham_ChiTietGia ON "
                             + "DM_SanPham_ChiTietGia.iTrangThai = 1 AND DM_SanPham_ChiTietGia.iID_MaChiTietGia = DM_SanPham_DanhMucGia.iID_MaChiTietGia "
                             + "WHERE DM_SanPham_DanhMucGia.iTrangThai = 1 AND LTRIM(RTRIM(DM_SanPham_DanhMucGia.sKyHieu)) = '10' "
                             + "AND DM_SanPham_ChiTietGia.iID_LoaiDonVi = 3 AND DM_SanPham_ChiTietGia.iID_MaSanPham = @iID_MaSanPham "
                             + "AND YEAR(ISNULL(DM_SanPham_ChiTietGia.dNgayTao,GETDATE())) = @Nam" + i.ToString() + " ORDER BY DM_SanPham_ChiTietGia.dNgayTao DESC)";
                cmd.Parameters.AddWithValue("@Nam" + i.ToString(), Nam - 5 + i);
            }
            int iSoSanh = 5  + NamSoSanh - Nam;
            String SQL = String.Format(@"SELECT tbl.*, SoSanh = CASE WHEN Nam{1} <> 0 THEN (Nam5/Nam{1})*100 ELSE 0 END FROM (SELECT {0}) AS tbl", queryNam, iSoSanh);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
    }
}
