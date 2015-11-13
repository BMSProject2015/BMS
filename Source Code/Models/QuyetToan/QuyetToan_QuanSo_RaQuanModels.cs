using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
namespace VIETTEL.Models
{
    public class QuyetToan_QuanSo_RaQuanModels
    {
        

        /// <summary>
        /// Lấy DataTable thông tin của một tháng quyết toán ra quân
        /// </summary>
        /// <param name="iID_MaQuanSoRaQuan"></param>
        /// <returns></returns>
        public static DataTable GetRaQuan(String iThang,String iNamLamViec)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM QTQS_QuyetToanRaQuan WHERE iTrangThai=1 AND iThang=@iThang AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang",iThang);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static int GetMaxRaQuan()
        {
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT MAX(iSoRaQuan) FROM QTQS_QuyetToanRaQuan WHERE iTrangThai=1");
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }


        /// <summary>
        /// Xóa dữ liệu chỉ tiêu
        /// </summary>
        /// <param name="iID_MaQuanSoRaQuan"></param>
        /// <param name="IPSua"></param>
        /// <param name="MaNguoiDungSua"></param>
        /// <returns></returns>
        public static int Delete_RaQuan(String iThang, String IPSua, String MaND)
        {
            //Xóa dữ liệu trong bảng QTQS_QuyetToanRaQuanChiTiet
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM QTQS_QuyetToanRaQuan WHERE iThang=@iThang AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNamLamViec", dtCauHinh.Rows[0]["iNamLamViec"]);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            dtCauHinh.Dispose();
            return 1;
        }

        public static String InsertDuyetRaQuan(String iID_MaQuanSoRaQuan, String NoiDung, String MaND, String IPSua)
        {
            String iID_MaDuyetRaQuan;
            Bang bang = new Bang("QTA_DuyetRaQuan");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaQuanSoRaQuan", iID_MaQuanSoRaQuan);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetRaQuan = Convert.ToString(bang.Save());
            return iID_MaDuyetRaQuan;
        }

        public static DataTable Get_DanhSachRaQuan(String iThang,String MaND, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];

            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            
            String DK= " iNamLamViec=@iNamLamViec";

            cmd.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);            

            
            if (CommonFunction.IsNumeric(iThang))
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }

            
            String SQL = String.Format("SELECT distinct iThang,iNamLamViec FROM QTQS_QuyetToanRaQuan WHERE iTrangThai = 1 AND {0} ORDER BY iThang", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachRaQuan_Count(String iThang, String MaND)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];

            Int32 vR;
            SqlCommand cmd = new SqlCommand();

            String DK = " iNamLamViec=@iNamLamViec";

            cmd.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);


            if (CommonFunction.IsNumeric(iThang))
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }


            String SQL = String.Format("SELECT COUNT(distinct iThang) FROM QTQS_QuyetToanRaQuan WHERE iTrangThai = 1 AND {0}", DK);
                        
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable LayDuLieuTuQuanSo(String iThang,String iNamLamViec)
        {

            String SQL = "SELECT iID_MaDonVi,ISNULL(SUM(rHuu),0) AS rHuu,ISNULL(SUM(rXuatNgu),0) AS rXuatNgu,ISNULL(SUM(rPhucVien),0) AS rPhucVien,ISNULL(SUM(rThoiViec),0) AS rThoiViec";
            SQL += " FROM( ";
            SQL += " SELECT iID_MaDonVi";
            SQL += " ,rHuu=Case When sKyHieu='310' Then SUM(rTongSo) Else 0 end";
            SQL += " ,rXuatNgu=Case When sKyHieu='320' Then SUM(rTongSo) Else 0 end";
            SQL += " ,rPhucVien=Case When sKyHieu='331' Then SUM(rTongSo) Else 0 end";
            SQL += " ,rThoiViec=Case When sKyHieu='330' Then SUM(rTongSo) Else 0 end";
            SQL += " FROM QTQS_ChungTuChiTiet";
            SQL += " WHERE iThang_Quy=@iThang_Quy AND iNamLamViec=@iNamLamViec";
            SQL += " GROUP BY iID_MaDonVi,sKyHieu) RaQuan";
            SQL += " GROUP BY iID_MaDonVi";
            SQL += " ORDER BY iID_MaDonVi";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}