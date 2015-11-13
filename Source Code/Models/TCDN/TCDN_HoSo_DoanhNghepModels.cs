using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DomainModel;

namespace VIETTEL.Models
{
    public class TCDN_HoSo_DoanhNghepModels
    {
        public static DataTable GetList(string sTenDoanhNghiep, string sTenThuongGoi, string sTenGiaoDich, string iIdMaLoaiHinhDoanhNghiep, string iIdMaHinhThucHoatDong, string iIdMaKhoi, string iIdMaNhom, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR = new DataTable();
            SqlCommand cmd = new SqlCommand();
            string DieuKien = "iTrangThai=1";
            if (!string.IsNullOrEmpty(sTenDoanhNghiep))
            {
                DieuKien += " AND sTenDoanhNghiep LIKE @sTenDoanhNghiep";
                cmd.Parameters.AddWithValue("@sTenDoanhNghiep", "%"+sTenDoanhNghiep+"%");
            }
            if (!string.IsNullOrEmpty(sTenThuongGoi))
            {
                DieuKien += " AND sTenThuongGoi LIKE @sTenThuongGoi";
                cmd.Parameters.AddWithValue("@sTenThuongGoi", "%" + sTenThuongGoi+"%");
            }
            if (!string.IsNullOrEmpty(sTenGiaoDich))
            {
                DieuKien += " AND sTenGiaoDich LIKE @sTenGiaoDich";
                cmd.Parameters.AddWithValue("@sTenGiaoDich", "%" + sTenGiaoDich+"%");
            }
            if (!string.IsNullOrEmpty(iIdMaLoaiHinhDoanhNghiep) && iIdMaLoaiHinhDoanhNghiep != Guid.Empty.ToString())
            {
                DieuKien += " AND iIdMaLoaiHinhDoanhNghiep = @iIdMaLoaiHinhDoanhNghiep";
                cmd.Parameters.AddWithValue("@iIdMaLoaiHinhDoanhNghiep", iIdMaLoaiHinhDoanhNghiep);
            }
            if (!string.IsNullOrEmpty(iIdMaHinhThucHoatDong) && iIdMaHinhThucHoatDong != Guid.Empty.ToString())
            {
                DieuKien += " AND iIdMaHinhThucHoatDong = @iIdMaHinhThucHoatDong";
                cmd.Parameters.AddWithValue("@iIdMaHinhThucHoatDong", iIdMaHinhThucHoatDong);
            }
            if (!string.IsNullOrEmpty(iIdMaKhoi) && iIdMaKhoi != Guid.Empty.ToString())
            {
                DieuKien += " AND iIdMaKhoi = @iIdMaKhoi";
                cmd.Parameters.AddWithValue("@iIdMaKhoi", iIdMaKhoi);
            }
            if (!string.IsNullOrEmpty(iIdMaNhom) && iIdMaNhom != Guid.Empty.ToString())
            {
                DieuKien += " AND iIdMaNhom LIKE @iIdMaNhom";
                cmd.Parameters.AddWithValue("@iIdMaNhom", iIdMaNhom);
            }
            string sql = string.Format("SELECT * FROM TCDN_DoanhNghiep WHERE {0}", DieuKien);
            cmd.CommandText = sql;
            vR = CommonFunction.dtData(cmd, "sTenDoanhNghiep", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static double getList_Count(string sTenDoanhNghiep, string sTenThuongGoi, string sTenGiaoDich, string iIdMaLoaiHinhDoanhNghiep, string iIdMaHinhThucHoatDong, string iIdMaKhoi, string iIdMaNhom)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            string DieuKien = "iTrangThai=1";
            if (!string.IsNullOrEmpty(sTenDoanhNghiep))
            {
                DieuKien += " AND sTenDoanhNghiep LIKE @sTenDoanhNghiep";
                cmd.Parameters.AddWithValue("@sTenDoanhNghiep", sTenDoanhNghiep);
            }
            if (!string.IsNullOrEmpty(sTenThuongGoi))
            {
                DieuKien += " AND sTenThuongGoi LIKE @sTenThuongGoi";
                cmd.Parameters.AddWithValue("@sTenThuongGoi", sTenThuongGoi);
            }
            if (!string.IsNullOrEmpty(sTenGiaoDich))
            {
                DieuKien += " AND sTenGiaoDich LIKE @sTenGiaoDich";
                cmd.Parameters.AddWithValue("@sTenGiaoDich", sTenGiaoDich);
            }
            if (!string.IsNullOrEmpty(iIdMaLoaiHinhDoanhNghiep) && iIdMaLoaiHinhDoanhNghiep != Guid.Empty.ToString())
            {
                DieuKien += " AND iIdMaLoaiHinhDoanhNghiep = @iIdMaLoaiHinhDoanhNghiep";
                cmd.Parameters.AddWithValue("@iIdMaLoaiHinhDoanhNghiep", iIdMaLoaiHinhDoanhNghiep);
            }
            if (!string.IsNullOrEmpty(iIdMaHinhThucHoatDong) && iIdMaHinhThucHoatDong != Guid.Empty.ToString())
            {
                DieuKien += " AND iIdMaHinhThucHoatDong = @iIdMaHinhThucHoatDong";
                cmd.Parameters.AddWithValue("@iIdMaHinhThucHoatDong", iIdMaHinhThucHoatDong);
            }
            if (!string.IsNullOrEmpty(iIdMaKhoi) && iIdMaKhoi != Guid.Empty.ToString())
            {
                DieuKien += " AND iIdMaKhoi = @iIdMaKhoi";
                cmd.Parameters.AddWithValue("@iIdMaKhoi", iIdMaKhoi);
            }
            if (!string.IsNullOrEmpty(iIdMaNhom) && iIdMaNhom != Guid.Empty.ToString())
            {
                DieuKien += " AND iIdMaNhom LIKE @iIdMaNhom";
                cmd.Parameters.AddWithValue("@iIdMaNhom", iIdMaNhom);
            }
            string sql = string.Format("SELECT COUNT(iID_MaDoanhNghiep) FROM TCDN_DoanhNghiep WHERE {0}", DieuKien);
            cmd.CommandText = sql;
            result = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return result;
        }

        internal static DataTable GetDonViThanhVien(string iID_MaDoanhNghiep)
        {
            DataTable result = new DataTable();
            SqlCommand cmd = new SqlCommand();
            string sql = string.Format(@"SELECT * FROM TCDN_DVTV WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iTrangThai=1");
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", new Guid(iID_MaDoanhNghiep));
            result = Connection.GetDataTable(cmd);
            if (result!=null)
            {
                DataRow row = result.NewRow();
                row["iID_MaDV"] = Guid.Empty;
                row["iID_MaDoanhNghiep"] = iID_MaDoanhNghiep;
                row["iTrangThai"] =true;
                result.Rows.Add(row);
            }
            return result;
        }

        public static DataTable GetChiTiet(string iID_MaDoanhNghiep)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM TCDN_DoanhNghiep WHERE iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep");
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable GetChucDanh(string iiD_MaDanhMuc, string iID_MaDoanhNghiep)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM TCDN_DN_Quanly
                                              WHERE iID_MaChucDanh=@iID_MaChucDanh AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep and iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChucDanh", iiD_MaDanhMuc);
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }



        internal static DataTable GetDuAn(string iID_MaDoanhNghiep)
        {
            DataTable result = new DataTable();
            SqlCommand cmd = new SqlCommand();
            string sql = string.Format(@"SELECT * FROM TCDN_DuAn WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iTrangThai=1");
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", new Guid(iID_MaDoanhNghiep));
            result = Connection.GetDataTable(cmd);
            if (result != null)
            {
                DataRow row = result.NewRow();
                row["iID_MaDuAn"] = Guid.Empty;
                row["iID_MaDoanhNghiep"] = iID_MaDoanhNghiep;
                row["iTrangThai"] = true;
                result.Rows.Add(row);
            }
            return result;
        }
    }
}