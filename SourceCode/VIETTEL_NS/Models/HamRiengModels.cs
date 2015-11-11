using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
namespace VIETTEL.Models
{
    public class HamRiengModels
    {
        public static String SSODomain = "";
        public static int SSOTimeout = 1;

        public static int MaDonViCuaNhomNguoiDung(String MaNguoiDung)
        {
            String SQL = String.Format("SELECT iID_MaDonVi " +
                                       "FROM QT_NguoiDung INNER JOIN " +
                                            "QT_NhomNguoiDung ON QT_NhomNguoiDung.iID_MaNhomNguoiDung=QT_NguoiDung.iID_MaNhomNguoiDung " +
                                       "WHERE sID_MaNguoiDung='{0}'", MaNguoiDung);
            SqlCommand cmd = new SqlCommand(SQL);
            int MaDonVi = Convert.ToInt16(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return MaDonVi;
        }
        public static void UserDefault(String MaND)
        {
            NguoiDungCauHinhModels.MaNguoiDung = MaND;
            NguoiDungCauHinhModels.iNamLamViec = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
        }
        public static int ChuyenThoiGianSangSo(String ThoiGian)
        {
            int vR=0;
            int Gio = 0;
            int Phut = 0;
            String[] arr = ThoiGian.Split('h');
            if (CommonFunction.IsNumeric(arr[0]))
            {
                Gio = Convert.ToInt16(arr[0]);
            }
            if (arr.Length>1 && CommonFunction.IsNumeric(arr[1]))
            {
                Phut = Convert.ToInt16(arr[1]);
            }
            vR = Gio * 60 + Phut;
            return vR;
        }

        public static String ChuyenSoSangThoiGian(int ThoiGian)
        {
            String vR="";
            if (ThoiGian > 0)
            {
                int Gio = ThoiGian / 60;
                int Phut = ThoiGian % 60;
                vR = String.Format("{0}h{1}", Gio, Phut);
            }
            return vR;
        }

        public static object LayGiaTri(DataTable dt, string TenTruongTim, object GiaTriTim, string TenTruongLayGiaTri)
        {
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToString(dt.Rows[i][TenTruongTim]) == Convert.ToString(GiaTriTim))
                {
                    return dt.Rows[i][TenTruongLayGiaTri];
                }
            }
            return null;
        }

        public static String LayXauDinhDangSo(int SoSauDauPhay)
        {
            String vR="0";
            if (SoSauDauPhay > 0)
            {
                vR += ".";
                for (int i = 1; i <= SoSauDauPhay; i++)
                {
                    vR += "0";
                }
            }
            return vR;
        }

        /// <summary>
        /// method for validating a guid using regular expressions
        /// </summary>
        /// <param name="guid">value to be validated</param>
        /// <returns>true/false</returns>
        public static bool IsValidGuid(string guid)
        {
            if (!(string.IsNullOrEmpty(guid)))
            {
                Regex reg = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

                return reg.IsMatch(guid);
            }

            return false;
        }
    }
}
