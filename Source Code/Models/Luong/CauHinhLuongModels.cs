using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using DomainModel.Abstract;
using DomainModel;

namespace VIETTEL.Models
{
    public class CauHinhLuongModels
    {
        public static Boolean SuaCauHinh(String sID_MaNguoiDung, Object options)
        {
            Boolean vR = false;
            Bang bang = new Bang("L_CauHinhLuong");
            bang.MaNguoiDungSua = sID_MaNguoiDung;

            Boolean okUpdate = false;
            PropertyInfo[] properties = options.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                bang.CmdParams.Parameters.AddWithValue("@" + properties[i].Name, properties[i].GetValue(options, null));
                okUpdate = true;
            }
            if (okUpdate)
            {
                SqlCommand cmd = new SqlCommand("SELECT iID_CauHinhLuong FROM L_CauHinhLuong WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sID_MaNguoiDung);
                int iID_CauHinhLuong = Convert.ToInt32(Connection.GetValue(cmd, -1));
                cmd.Dispose();
                if (iID_CauHinhLuong >= 0)
                {
                    bang.DuLieuMoi = false;
                    bang.GiaTriKhoa = iID_CauHinhLuong;
                }
                bang.Save();
                vR = true;
            }
            return vR;
        }

        public static DataTable LayCauHinh(String sID_MaNguoiDung)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM L_CauHinhLuong WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sID_MaNguoiDung);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tháng làm việc của ngừơi dùng
        /// </summary>
        /// <param name="MaND">Tên đăng nhập của người dùng</param>
        /// <returns>Tháng làm việc</returns>
        public static int LayThangLamViec(String MaND)
        {

            int vR;
            SqlCommand cmd = new SqlCommand("SELECT iThangLamViec FROM L_CauHinhLuong WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            vR = Convert.ToInt16(Connection.GetValue(cmd,0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy Năm làm việc của người dùng
        /// </summary>
        /// <param name="MaND">Tên đăng nhập của người dùng</param>
        /// <returns>Năm làm việc</returns>
        public static int LayNamLamViec(String MaND)
        {
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT iNamLamViec FROM L_CauHinhLuong WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao");
            cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            vR = Convert.ToInt16(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy lương tối thiểu
        /// </summary>
        /// <returns></returns>
        /// 

        public static int LayLuongToiThieu()
        {
            int vR = 0;
            SqlCommand cmd = new SqlCommand("SELECT TOP (1) sThamSo  FROM L_DanhMucThamSo WHERE (sKyHieu = N'rLuongToiThieu') AND (iTrangThai=1) AND (bConSuDung=1) ORDER BY dThoiGianApDung_BatDau DESC");
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Tính thuế TNCN
        /// </summary>
        /// <param name="SoTien"></param>
        /// <returns></returns>
        public static double TinhThueTNCN(double SoTien)
        {
            double vR = 0;
            vR = 0.05 * SoTien;
            //if (SoTien <= 0)
            //    vR = 0;
            //else if (SoTien > 0 && SoTien <= 5000000)
            //    vR = 0.05 * SoTien;
            //else if (SoTien > 5000000 && SoTien <= 10000000)
            //    vR = 250000 + 0.1 * (SoTien - 5000000);
            //else if (SoTien > 10000000 && SoTien <= 18000000)
            //    vR = 750000 + 0.15 * (SoTien - 10000000);
            //else if (SoTien > 18000000 && SoTien <= 32000000)
            //    vR = 1950000 + 0.2 * (SoTien - 18000000);
            //else if (SoTien > 32000000 && SoTien <= 52000000)
            //    vR = 5450000 + 0.25 * (SoTien - 32000000);
            //else if (SoTien > 52000000 && SoTien <= 80000000)
            //    vR = 10450000 + 0.3 * (SoTien - 52000000);
            //else
            //    vR = 18850000 + 0.35 * (SoTien - 80000000);
            return vR;
        }
    }
}