using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace VIETTEL.Models
{
    public class DanhMuc_BaoCao_ChuKyModels
    {
        #region Danh mục chữ ký

        public static NameValueCollection LayThongTinBaoCaoChuKy(String iID_MaBaoCao_ChuKy)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietDanhMucBaoCaoChuKy(iID_MaBaoCao_ChuKy);
            String colName = "";
            
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_ChiTietDanhMucBaoCaoChuKy(String iID_MaBaoCao_ChuKy)
        {
            DataTable dt = new DataTable();
            String DK = "";

            if (String.IsNullOrEmpty(iID_MaBaoCao_ChuKy) == false)
            {
                SqlCommand cmd = new SqlCommand();
                DK = " iID_MaBaoCao_ChuKy=@iID_MaBaoCao_ChuKy";
                cmd.Parameters.AddWithValue("@iID_MaBaoCao_ChuKy", iID_MaBaoCao_ChuKy);
                String SQL = "SELECT * FROM NS_DanhMuc_BaoCao_ChuKy WHERE iTrangThai=1 AND {0}";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            return dt;
        }
        public static DataTable Get_dtLayThongTinChuKy(String ControllerName)
        {
            String SQL = @"SELECT
            sTenThuaLenh1=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaThuaLenh1)
            ,sTenThuaLenh2=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaThuaLenh2)
            ,sTenThuaLenh3=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaThuaLenh3)
            ,sTenThuaLenh4=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaThuaLenh4)
            ,sTenThuaLenh5=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaThuaLenh5)
            ,sTenChucDanh1=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaChucDanh1)
            ,sTenChucDanh2=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaChucDanh2)
            ,sTenChucDanh3=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaChucDanh3)
            ,sTenChucDanh4=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaChucDanh4)
            ,sTenChucDanh5=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaChucDanh5)
            ,sTen1=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaTen1)
            ,sTen2=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaTen2)
            ,sTen3=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaTen3)
            ,sTen4=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaTen4)
            ,sTen5=(SELECT sTen FROM NS_DanhMucChuKy WHERE iID_MaChuKy=NS_DanhMuc_BaoCao_ChuKy.iID_MaTen5)
            FROM NS_DanhMuc_BaoCao_ChuKy WHERE sController=@sController AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sController", ControllerName);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", NguoiDungCauHinhModels.MaNguoiDung);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_TaiKhoan(String iID_MaPhanHe)
        {
            String SQL = "SELECT DISTINCT sID_MaNguoiDungTao FROM NS_DanhMuc_BaoCao_ChuKy WHERE iID_MaPhanHe='" + iID_MaPhanHe + "'";
            return Connection.GetDataTable(SQL);
        }
        /// <summary>
        /// Lấy datatable Danh mục chữ ký
        /// </summary>
        /// <param name="iID_MaBaoCao_ChuKy">Mã chữ ký  nếu bằng "" lấy tất cả danh mục chữ ký</param>
        /// <returns></returns>
        public static DataTable Get_dtDanhMucBaoCaoChuKy(String iID_MaBaoCao_ChuKy = "")
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaBaoCao_ChuKy) == false)
            {
                DK = " AND iID_MaBaoCao_ChuKy=@iID_MaBaoCao_ChuKy";
                cmd.Parameters.AddWithValue("@iID_MaBaoCao_ChuKy", iID_MaBaoCao_ChuKy);
            }
            String SQL = "SELECT * FROM NS_DanhMuc_BaoCao_ChuKy WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        /// <summary>
        /// Lấy datatable Danh mục báo chữ ký theo phân hệ
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ  nếu bằng "" lấy tất cả danh mục chữ ký</param>
        /// <returns></returns>
        public static DataTable Get_dtDanhMucBaoCaoChuKyTheoPhanHe(String iID_MaPhanHe = "",String MaND="")
        {
            DataTable dt = null;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaPhanHe) == false)
            {
                DK = " AND iID_MaPhanHe=@iID_MaPhanHe";
                cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            }
            if (String.IsNullOrEmpty(MaND) == false)
            {
                DK = DK+ " AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            String SQL = "SELECT * FROM NS_DanhMuc_BaoCao_ChuKy WHERE iTrangThai=1 {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public static DataTable Get_dtDanhBaoCaoMucChuKy(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM NS_DanhMuc_BaoCao_ChuKy WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "sKyHieu", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountDanhMucBaoCaoChuKy()
        {

            String SQL = "SELECT COUNT(*) FROM NS_DanhMuc_BaoCao_ChuKy WHERE iTrangThai=1";
            return Convert.ToInt16(Connection.GetValue(SQL, 0));
        }

        #endregion

    }
}