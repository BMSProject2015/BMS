using System;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Web.Routing;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;
namespace VIETTEL.Models
{
    public class SanPham_VatTuModels
    {
        /// <summary>
        /// Lấy một giá trị giá gần nhất
        /// </summary>
        /// <param name="iID_MaVatTu"></param>
        /// <returns></returns>
        /// 
        public static DataTable Get_SanPham(String iID_MaSanPham)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 DM_SanPham.* FROM DM_SanPham WHERE iTrangThai=1 AND iID_MaSanPham=@iID_MaSanPham");
            cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static String Get_TenSanPham(String iID_MaSanPham)
        {
            String vR;
            String Query = String.Format(@"SELECT TOP 1 DM_SanPham.sTen FROM DM_SanPham WHERE DM_SanPham.iID_MaSanPham = @iID_MaSanPham");
            SqlCommand cmd = new SqlCommand(Query);
            cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
            vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_GiaVatTu_Row(String iID_MaVatTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM DM_VatTu_Gia WHERE iID_MaVatTu=@iID_MaVatTu ORDER BY dTuNgay, dNgayTao DESC");
            cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_DanhSachSanPham(String MaND, String sTen, String sMa, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "iTrangThai = 1 ";
            if (!String.IsNullOrEmpty(sTen))
            {
                DK += " AND sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", "%" + sTen + "%");
            }
            if (!String.IsNullOrEmpty(sMa))
            {
                DK += " AND sMa LIKE @sMa";
                cmd.Parameters.AddWithValue("@sMa", "%" + sMa + "%");
            }
            //if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            //{
            //    DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
            //    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //}
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayLap >= @dTuNgay";
                cmd.Parameters.AddWithValue("@dTuNgay", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayLap <= @dDenNgay";
                cmd.Parameters.AddWithValue("@dDenNgay", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT * FROM DM_SanPham WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet, dNgayLap DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachSanPham_Count(String MaND, String sTen, String sMa, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            //String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
            String DK = "iTrangThai = 1 ";
            if (!String.IsNullOrEmpty(sTen))
            {
                DK += " AND sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", "%" + sTen + "%");
            }
            if (!String.IsNullOrEmpty(sMa))
            {
                DK += " AND sMa LIKE @sMa";
                cmd.Parameters.AddWithValue("@sMa", "%" + sMa + "%");
            }
            //if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            //{
            //    DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
            //    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            //}
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayLap >= @dTuNgay";
                cmd.Parameters.AddWithValue("@dTuNgay", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayLap <= @dDenNgay";
                cmd.Parameters.AddWithValue("@dDenNgay", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT Count(*) FROM DM_SanPham WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_VatTu(String iID_MaVatTu)
        {
            DataTable vR;
            String Query = String.Format(@"SELECT TOP 1 DM_VatTu.* 
                                            ,(SELECT DC_DanhMuc.sTen FROM DC_DanhMuc WHERE DM_VatTu.iDM_MaDonViTinh = DC_DanhMuc.iID_MaDanhMuc) AS sTen_DonVi
                                            FROM DM_VatTu WHERE iID_MaVatTu  = @iID_MaVatTu");
            SqlCommand cmd = new SqlCommand(Query);
            cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_VatTuChuaChon(String iID_MaDanhMucGia)
        {
            DataTable vR;
            String Query = String.Format(@"SELECT DM_VatTu.* 
                                         ,(SELECT DC_DanhMuc.sTen FROM DC_DanhMuc WHERE DM_VatTu.iDM_MaDonViTinh = DC_DanhMuc.iID_MaDanhMuc) AS sTen_DonVi
                                         FROM DM_VatTu
                                         WHERE iTrangThai = 1 AND iID_MaVatTu NOT IN (
	                                            SELECT tblDM.iID_MaVatTu 
                                                FROM DM_SanPham_DanhMucGia AS tblDM WHERE tblDM.iID_MaDanhMucGia_Cha = @iID_MaDanhMucGia
                                                AND tblDM.iTrangThai = 1 AND tblDM.bNganSach = 0 AND tblDM.iID_MaVatTu IN (
                                                    SELECT tblDM2.iID_MaVatTu
                                                    FROM DM_SanPham_DanhMucGia AS tblDM2 WHERE tblDM2.iTrangThai = 1 AND tblDM2.bNganSach = 1 AND tblDM2.iID_MaDanhMucGia_Cha = @iID_MaDanhMucGia
                                                )
                                            )
                                         ORDER BY DM_VatTu.sTen");
            SqlCommand cmd = new SqlCommand(Query);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucGia", iID_MaDanhMucGia);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static int Check_VatTu_NganSach(String iID_MaDanhMucGia, String iID_MaVatTu, String bNganSach)
        {
            int vR;
            String Query = String.Format(@"SELECT COUNT(tblDM.iID_MaVatTu) AS iSoLuong 
                                                FROM DM_SanPham_DanhMucGia AS tblDM WHERE tblDM.iID_MaDanhMucGia_Cha = @iID_MaDanhMucGia
                                                AND tblDM.iTrangThai = 1 AND tblDM.bNganSach = @bNganSach AND tblDM.iID_MaVatTu = @iID_MaVatTu");
            SqlCommand cmd = new SqlCommand(Query);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucGia", iID_MaDanhMucGia);
            cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
            cmd.Parameters.AddWithValue("@bNganSach", bNganSach);
            vR =Convert.ToInt16(Connection.GetValue(cmd,0));
            cmd.Dispose();
            return vR;
        }
        public static String Get_TenDonViTinh(String iDM_MaDonViTinh)
        {
            String vR;
            String Query = String.Format(@"SELECT DC_DanhMuc.sTen AS sTen_DonVi FROM DC_DanhMuc WHERE DC_DanhMuc.iID_MaDanhMuc = @iDM_MaDonViTinh");
            SqlCommand cmd = new SqlCommand(Query);
            cmd.Parameters.AddWithValue("@iDM_MaDonViTinh", iDM_MaDonViTinh);
            vR = Connection.GetValueString(cmd,"");
            cmd.Dispose();
            return vR;
        }
        public static int KiemTraTrungMaVatTu(String sTenKhoa, String LoaiDanhMuc)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(sTenKhoa) FROM DC_DanhMuc " +
                    "WHERE bHoatDong = 1 AND sTenKhoa = @sTenKhoa " +
                        "AND iID_MaLoaiDanhMuc = (SELECT TOP 1 iID_MaLoaiDanhMuc " +
                                                    "FROM DC_LoaiDanhMuc " +
                                                    "WHERE sTenBang = @sTenBang)");
            cmd.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
            cmd.Parameters.AddWithValue("@sTenBang", LoaiDanhMuc);
            return Convert.ToInt32(Connection.GetValue(cmd, 0));
        }
    }
}