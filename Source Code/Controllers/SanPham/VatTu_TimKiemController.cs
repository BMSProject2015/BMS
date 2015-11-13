using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace VIETTEL.Controllers
{
    public class VatTu_TimKiemController : Controller
    {
        //
        // GET: /TimKiemVatTu/
        public string sViewPath = "~/Views/SanPham/TimKiemVatTu/";
        [Authorize]
        public ActionResult Index(String ParentID)
        {
            ViewData["sTen"] = Request.Form[ParentID + "_sTen"];
            ViewData["Nam"] = Request.Form[ParentID + "_Nam"];
            return View(sViewPath + "List.aspx");
        }

        public static DataTable Get_DanhSachVatTu(String sTen, int Nam, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "DM_VatTu.iTrangThai = 1 ";
            if (!String.IsNullOrEmpty(sTen))
            {
                DK += " AND DM_VatTu.sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", "%" + sTen + "%");
            }
            if (Nam > 0)
            {
                DK += " AND YEAR(ISNULL(DM_SanPham_DanhMucGia.dNgayTao,GETDATE())) = @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }
            String SQL = String.Format(@"SELECT * FROM (SELECT DISTINCT DM_VatTu.iID_MaVatTu, DM_VatTu.sTen, DM_VatTu.iDM_MaDonViTinh, DM_SanPham_DanhMucGia.iID_MaSanPham
                                            FROM DM_VatTu LEFT JOIN DM_SanPham_DanhMucGia ON DM_VatTu.iID_MaVatTu = DM_SanPham_DanhMucGia.iID_MaVatTu
                                            AND DM_SanPham_DanhMucGia.iTrangThai = 1 
                                            AND DM_SanPham_DanhMucGia.iID_MaSanPham IN (SELECT iID_MaSanPham FROM DM_SanPham WHERE DM_SanPham.iTrangThai = 1)
                                            AND DM_SanPham_DanhMucGia.iID_MaChiTietGia IN (SELECT iID_MaChiTietGia FROM DM_SanPham_ChiTietGia WHERE DM_SanPham_ChiTietGia.iTrangThai = 1)
                                            WHERE {0}) AS tbl", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sTen", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachVatTu_Count(String sTen, int Nam)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "DM_VatTu.iTrangThai = 1 ";
            if (!String.IsNullOrEmpty(sTen))
            {
                DK += " AND DM_VatTu.sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", "%" + sTen + "%");
            }
            if (Nam > 0)
            {
                DK += " AND YEAR(ISNULL(DM_SanPham_DanhMucGia.dNgayTao,GETDATE())) = @Nam";
                cmd.Parameters.AddWithValue("@Nam", Nam);
            }
            String SQL = String.Format(@"SELECT DISTINCT DM_VatTu.iID_MaVatTu, DM_VatTu.sTen, DM_SanPham_DanhMucGia.iID_MaSanPham, DM_SanPham_DanhMucGia.dNgayTao
                                            ,(SELECT DC_DanhMuc.sTen FROM DC_DanhMuc WHERE DM_VatTu.iDM_MaDonViTinh = DC_DanhMuc.iID_MaDanhMuc) AS sTen_DonViTinh
                                            ,(SELECT DM_SanPham.sTen FROM DM_SanPham WHERE DM_SanPham_DanhMucGia.iID_MaSanPham = DM_SanPham.iID_MaSanPham) AS sTen_SanPham
                                            FROM DM_VatTu LEFT JOIN DM_SanPham_DanhMucGia ON DM_VatTu.iID_MaVatTu = DM_SanPham_DanhMucGia.iID_MaVatTu
                                            AND DM_SanPham_DanhMucGia.iTrangThai = 1 
                                            AND DM_SanPham_DanhMucGia.iID_MaSanPham IN (SELECT iID_MaSanPham FROM DM_SanPham WHERE DM_SanPham.iTrangThai = 1)
                                            AND DM_SanPham_DanhMucGia.iID_MaChiTietGia IN (SELECT iID_MaChiTietGia FROM DM_SanPham_ChiTietGia WHERE DM_SanPham_ChiTietGia.iTrangThai = 1)
                                            WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR.Rows.Count;
        }
        public static DataTable getGiaVatTu(String iID_MaVatTu, String iID_MaSanPham, int Nam)
        {
            String[] arrTruongGiaTri = "rDonGia_DangThucHien,rDonGia_DV_DeNghi,rDonGia_DatHang_DeNghi,rDonGia_CTC_DeNghi".Split(',');
            DataTable dtGia = new DataTable();
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
            if (String.IsNullOrEmpty(iID_MaSanPham))
            {
                SQL = "SELECT TOP 1 rGia, rGia_NS FROM DM_VatTu_Gia WHERE iID_MaVatTu=@iID_MaVatTu";
                if (Nam > 0)
                {
                    SQL += " AND YEAR(dTuNgay) <= @Nam";
                    cmd.Parameters.AddWithValue("@Nam", Nam);
                }
                SQL += " ORDER BY dTuNgay, dNgayTao DESC";
                cmd.CommandText = SQL;
                dtGia = Connection.GetDataTable(cmd);
            }
            else
            {
                DataTable dtChiTiet = getChiTietGiaGanNhat(iID_MaVatTu, iID_MaSanPham, Nam);
                if (dtChiTiet.Rows.Count > 0)
                {
                    int LoaiDonVi = (int)dtChiTiet.Rows[0]["iID_LoaiDonVi"];
                    String iID_MaChiTietGia = Convert.ToString(dtChiTiet.Rows[0]["iID_MaChiTietGia"]);
                    String TruongGiaTri = arrTruongGiaTri[LoaiDonVi];
                    SQL = String.Format(@"SELECT 
                                          (SELECT TOP 1 {0} FROM DM_SanPham_DanhMucGia AS tbl1 
                                                WHERE iTrangThai = 1 AND iID_MaVatTu=@iID_MaVatTu AND iID_MaChiTietGia = @iID_MaChiTietGia AND bNganSach = 0) AS rGia
                                          ,(SELECT TOP 1 {0} FROM DM_SanPham_DanhMucGia AS tbl2 
                                                WHERE iTrangThai = 1 AND iID_MaVatTu=@iID_MaVatTu AND iID_MaChiTietGia = @iID_MaChiTietGia AND bNganSach = 1) AS rGia_NS",TruongGiaTri);
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
                    dtGia = Connection.GetDataTable(cmd);
                }
            }
            return dtGia;
        }
        private static DataTable getChiTietGiaGanNhat(String iID_MaVatTu, String iID_MaSanPham, int Nam)
        {
            String DK_Nam = "";
            if (Nam > 0)
            {
                DK_Nam = " AND YEAR(dNgayTao) <= @Nam";
            }
            String SQL = String.Format(@"SELECT TOP 1 * FROM DM_SanPham_ChiTietGia
                                         WHERE iTrangThai = 1 AND iID_MaSanPham = @iID_MaSanPham {0}
                                         AND iID_MaChiTietGia IN (
                                            SELECT DISTINCT iID_MaChiTietGia FROM DM_SanPham_DanhMucGia 
                                            WHERE iTrangThai = 1 AND iID_MaSanPham = @iID_MaSanPham
                                            AND iID_MaVatTu=@iID_MaVatTu {0})
                                         ORDER BY dNgayTao DESC", DK_Nam);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
            cmd.Parameters.AddWithValue("@Nam", Nam);
            cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            return Connection.GetDataTable(cmd);
        }
    }
}
