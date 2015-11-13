using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
using DomainModel.Abstract;
namespace VIETTEL.Models
{
    public class KTCS_TaiSan_DonViModels
    {
        public static void ThemDieuChuyenTaiSan(String iID_MaDonVi,String iID_MaTaiSan,String UserName,String IPSua)
        {
            if (CheckDonViSuDungTaiSan(iID_MaDonVi, iID_MaTaiSan) == false)
            {
                //update lại trường đang sử dụng=false của tài sản với các đơn vị khác
                SuaLaiTruong_DangSuDung(iID_MaTaiSan, false);
                //thêm mới
                Bang bang = new Bang("KTCS_TaiSan_DonVi");
                bang.DuLieuMoi = true;
                bang.MaNguoiDungSua = UserName;
                bang.IPSua = IPSua;
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                bang.CmdParams.Parameters.AddWithValue("@bDangSuDung", 1);
                bang.Save();
            }
        }

        public static void SuaLaiTruong_DangSuDung(String iID_MaTaiSan,Boolean bDangSuDung)
        {
            SqlCommand cmd = new SqlCommand("UPDATE KTCS_TaiSan_DonVi SET bDangSuDung=@bDangSuDung WHERE iID_MaTaiSan=@iID_MaTaiSan");
            cmd.Parameters.AddWithValue("@bDangSuDung", bDangSuDung);
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        /// <summary>
        /// Kiểm tra với tài sản có đang được sử dụng bởi đơn vị không? nếu đang được sử dụng trả về true và ngược lại
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTaiSan"></param>
        /// <returns></returns>
        public static Boolean CheckDonViSuDungTaiSan(String iID_MaDonVi,String iID_MaTaiSan)
        {
            String sGiaTri,sTruong;
            sTruong="iID_MaDonVi,iID_MaTaiSan,bDangSuDung";
            sGiaTri=String.Format("{0},{1},{2}",iID_MaDonVi,iID_MaTaiSan,1);
            return HamChung.Check_Trung("KTCS_TaiSan_DonVi","iID_MaTaiSanDonVi","",sTruong,sGiaTri,true);

        }

        public static DataTable DanhSachDieuChuyen(String iID_MaTaiSan)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = @"SELECT KTCS_TaiSan_DonVi.*,KTCS_TaiSan.* FROM ((SELECT * FROM KTCS_TaiSan {0}) AS KTCS_TaiSan
                    INNER JOIN ( SELECT * FROM KTCS_TaiSan_DonVi {0}) AS KTCS_TaiSan_DonVi
                     ON KTCS_TaiSan_DonVi.iID_MaTaiSan=KTCS_TaiSan.iID_MaTaiSan
                    ) ORDER BY KTCS_TaiSan_DonVi.iID_MaTaiSan,KTCS_TaiSan_DonVi.dNgayTao";
            if (String.IsNullOrEmpty(iID_MaTaiSan) == false)
            {
               DK+=" WHERE iID_MaTaiSan=@iID_MaTaiSan";
               cmd.Parameters.AddWithValue("@iID_MaTaiSan",iID_MaTaiSan);
            }
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }
    }
}