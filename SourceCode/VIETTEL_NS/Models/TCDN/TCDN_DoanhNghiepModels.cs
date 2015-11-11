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
    public class TCSN_DoanhNghiepModels
    {
        /// <summary>
        /// Lấy danh sách đơn vị theo tham số tìm
        /// </summary>
        /// <param name="sTenDoanhNghiep"></param>
        /// <param name="sTenThuongGoi"></param>
        /// <param name="sTenGiaoDich"></param>
        /// <param name="sTenTheoQuocPhong"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="iID_MaLoaiDoanhNghiep"></param>
        /// <param name="iID_MaNhomDoanhNghiep"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable getList(String sTenDoanhNghiep = "", String sTenThuongGoi = "", String sTenGiaoDich = "", String sTenTheoQuocPhong = "", 
            String iID_MaDonVi = "", String iNamLamViec = "", String iID_MaLoaiDoanhNghiep = "", String iID_MaNhomDoanhNghiep = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1";
            if (String.IsNullOrEmpty(sTenDoanhNghiep)==false)
            {
                DK += " AND sTenDoanhNghiep LIKE @sTenDoanhNghiep";
                cmd.Parameters.AddWithValue("@sTenDoanhNghiep", sTenDoanhNghiep+"%");
            }
            if (String.IsNullOrEmpty(sTenThuongGoi) == false) 
            {
                DK += " AND sTenThuongGoi LIKE @sTenThuongGoi";
                cmd.Parameters.AddWithValue("@sTenThuongGoi", sTenThuongGoi + "%");
            }
            if (String.IsNullOrEmpty(sTenGiaoDich) == false) 
            {
                DK += " AND sTenGiaoDich LIKE @sTenGiaoDich";
                cmd.Parameters.AddWithValue("@sTenGiaoDich", sTenGiaoDich + "%");
            }
            if (String.IsNullOrEmpty(sTenTheoQuocPhong) == false) 
            {
                DK += " AND sTenTheoQuocPhong LIKE @sTenTheoQuocPhong";
                cmd.Parameters.AddWithValue("@sTenTheoQuocPhong", sTenTheoQuocPhong + "%");
            }
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi );
            }
            if (String.IsNullOrEmpty(iNamLamViec) == false) 
            {
                DK += " AND iNamLamViec = @iNamLamViec";
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            }
            if (iID_MaLoaiDoanhNghiep != Guid.Empty.ToString() && String.IsNullOrEmpty(iID_MaLoaiDoanhNghiep) == false)
            {
                DK += " AND iID_MaLoaiDoanhNghiep= @iID_MaLoaiDoanhNghiep";
                cmd.Parameters.AddWithValue("@iID_MaLoaiDoanhNghiep", iID_MaLoaiDoanhNghiep);
            }
            if (iID_MaNhomDoanhNghiep != Guid.Empty.ToString() && String.IsNullOrEmpty(iID_MaNhomDoanhNghiep) == false)
            {
                DK += " AND iID_MaNhomDoanhNghiep= @iID_MaNhomDoanhNghiep";
                cmd.Parameters.AddWithValue("@iID_MaNhomDoanhNghiep", iID_MaNhomDoanhNghiep);
            }
            String SQL = String.Format("SELECT  * FROM TCDN_DoanhNghiep WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaLoaiDoanhNghiep,iID_MaNhomDoanhNghiep,sTenDoanhNghiep ", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số danh sách đơn vị theo tham số tìm
        /// </summary>
        /// <param name="sTenDoanhNghiep"></param>
        /// <param name="sTenThuongGoi"></param>
        /// <param name="sTenGiaoDich"></param>
        /// <param name="sTenTheoQuocPhong"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iNamLamViec"></param>
        /// <param name="iID_MaLoaiDoanhNghiep"></param>
        /// <param name="iID_MaNhomDoanhNghiep"></param>
        /// <returns></returns>
        public static int getList_Count(String sTenDoanhNghiep = "", String sTenThuongGoi = "", String sTenGiaoDich = "", String sTenTheoQuocPhong = "", String
            iID_MaDonVi = "", String iNamLamViec = "", String iID_MaLoaiDoanhNghiep = "", String iID_MaNhomDoanhNghiep = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1";
            if (String.IsNullOrEmpty(sTenDoanhNghiep) == false)
            {
                DK += " AND sTenDoanhNghiep LIKE @sTenDoanhNghiep";
                cmd.Parameters.AddWithValue("@sTenDoanhNghiep", sTenDoanhNghiep + "%");
            }
            if (String.IsNullOrEmpty(sTenThuongGoi) == false)
            {
                DK += " AND sTenThuongGoi LIKE @sTenThuongGoi";
                cmd.Parameters.AddWithValue("@sTenThuongGoi", sTenThuongGoi + "%");
            }
            if (String.IsNullOrEmpty(sTenGiaoDich) == false)
            {
                DK += " AND sTenGiaoDich LIKE @sTenGiaoDich";
                cmd.Parameters.AddWithValue("@sTenGiaoDich", sTenGiaoDich + "%");
            }
            if (String.IsNullOrEmpty(sTenTheoQuocPhong) == false)
            {
                DK += " AND sTenTheoQuocPhong LIKE @sTenTheoQuocPhong";
                cmd.Parameters.AddWithValue("@sTenTheoQuocPhong", sTenTheoQuocPhong + "%");
            }
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (String.IsNullOrEmpty(iNamLamViec) == false)
            {
                DK += " AND iNamLamViec = @iNamLamViec";
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            }
            if (iID_MaLoaiDoanhNghiep != Guid.Empty.ToString() && String.IsNullOrEmpty(iID_MaLoaiDoanhNghiep) == false)
            {
                DK += " AND iID_MaLoaiDoanhNghiep= @iID_MaLoaiDoanhNghiep";
                cmd.Parameters.AddWithValue("@iID_MaLoaiDoanhNghiep", iID_MaLoaiDoanhNghiep);
            }
            if (iID_MaNhomDoanhNghiep != Guid.Empty.ToString() && String.IsNullOrEmpty(iID_MaNhomDoanhNghiep) == false)
            {
                DK += " AND iID_MaNhomDoanhNghiep= @iID_MaNhomDoanhNghiep";
                cmd.Parameters.AddWithValue("@iID_MaNhomDoanhNghiep", iID_MaNhomDoanhNghiep);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM TCDN_DoanhNghiep WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy thông tin doanh nghiệp
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static String Get_TenDonVi(String iID_MaDoanhNghiep)
        {
            String vR = "";
            SqlCommand cmd = new SqlCommand("SELECT sTenDoanhNghiep FROM TCDN_DoanhNghiep WHERE iTrangThai=1 AND iID_MaDoanhNghiep = @iID_MaDoanhNghiep");
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy ra danh sách đơn vị
        /// </summary>
        /// <param name="MaND"></param>
        /// <returns></returns>
        public static DataTable Get_ListDoanhNghiep(Boolean bCoHangTieuDe = false)
        {
            DataTable vR;
            String SQL = "SELECT iID_MaDoanhNghiep, sTenDoanhNghiep FROM TCDN_DoanhNghiep WHERE iTrangThai=1 ORDER BY sTenDoanhNghiep";
            SqlCommand cmd = new SqlCommand(SQL);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (bCoHangTieuDe == true)
            {
                vR.Rows.InsertAt(vR.NewRow(), 0);
                vR.Rows[0]["iID_MaDoanhNghiep"] = Guid.Empty;
                vR.Rows[0]["sTenDoanhNghiep"] = "-- Doanh nghiệp --";
            }
            return vR;
        }
        public static DataTable Get_ListDoanhNghiep()
        {
            DataTable vR;
            String SQL = "SELECT iID_MaDoanhNghiep as iID_MaDonVi, sTenDoanhNghiep as TenHT FROM TCDN_DoanhNghiep WHERE iTrangThai=1 ORDER BY sTenDoanhNghiep";
            SqlCommand cmd = new SqlCommand(SQL);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy thông tin chi tiết của một doanh nghiệp
        /// </summary>
        /// <param name="iID_MaDoanhNghiep"></param>
        /// <returns></returns>
        public static DataTable Get_DoanhNghiep_Row(String iID_MaDoanhNghiep) {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM TCDN_DoanhNghiep WHERE iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep");
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Update trường iTrangThai=0  cho bản ghi
        /// </summary>
        /// <param name="iID_MaDoanhNghiep"></param>
        /// <returns></returns>
        public static int Delete(String iID_MaDoanhNghiep) {
            int vR = 0;
            SqlCommand cmd = new SqlCommand("UPDATE TCDN_DoanhNghiep SET iTrangThai = 0 WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep");
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            vR = 1;
            return vR;
        }
    }
}