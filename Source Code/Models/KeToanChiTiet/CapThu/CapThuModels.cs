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
    public class CapThuModels
    {
        public  static  string getNgayCapPhat(string iID_MaCapPhat)
        {
            SqlCommand cmd;
           
            String SQL_CT = "SELECT dNgayCapPhat FROM CP_CapPhat WHERE iID_MaCapPhat=@iID_MaCapPhat";
            cmd = new SqlCommand();
            cmd.CommandText = String.Format(SQL_CT, 0);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            DateTime dNgayCapPhat = Convert.ToDateTime(Connection.GetValue(cmd, DateTime.Now));
            cmd.Dispose();
            return dNgayCapPhat.ToString("dd/MM/yyyy");

        }
        /// <summary>
        /// Delete_CapThu_Duyet
        /// </summary>
        /// <param name="iID_MaChungTu_Duyet"></param>
        public static void Delete_CapThu_Duyet(String iID_MaChungTu_Duyet)
        {
            String SQL, DK;
            SqlCommand cmd;

            cmd = new SqlCommand("DELETE FROM KTTG_ChungTuCapThu_Duyet_ChiTiet WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet");
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            if (Get_RowCapThu_Duyet(iID_MaChungTu_Duyet).Rows.Count>0)
            {
                String iID_MaCapPhat = Convert.ToString(Get_RowCapThu_Duyet(iID_MaChungTu_Duyet).Rows[0]["iID_MaCapPhat"]);
                cmd = new SqlCommand("DELETE FROM KTTG_ChungTuChiTietCapThu WHERE iID_MaCapPhat=@iID_MaCapPhat");
                cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
                cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
                
            }
            cmd = new SqlCommand();
            DK = "iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet";
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            SQL = String.Format("DELETE FROM KTTG_ChungTuCapThu_Duyet WHERE {0}", DK);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();            
        }
        /// <summary>
        /// Get_RowCapThu_Duyet
        /// </summary>
        /// <param name="iID_MaChungTu_Duyet"></param>
        /// <returns></returns>
        public static DataTable Get_RowCapThu_Duyet(String iID_MaChungTu_Duyet)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai = 1 AND iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet";
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            SQL = String.Format("SELECT * FROM KTTG_ChungTuCapThu_Duyet WHERE {0}", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Get_Grid_dtChungTu_ChiTiet_Duyet
        /// </summary>
        /// <param name="iID_MaChungTu_Duyet"></param>
        /// <param name="arrGiaTriTimKiem"></param>
        /// <returns></returns>
        public static DataTable Get_Grid_dtChungTu_ChiTiet_Duyet(String iID_MaChungTu_Duyet, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "A.iTrangThai = 1 AND  A.iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet ";
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            if (arrGiaTriTimKiem != null)
            {
                String DSTruong = MucLucNganSachModels.strDSTruong;
                String[] arrDSTruong = DSTruong.Split(',');
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
                    }
                }
            }

            SQL = String.Format("SELECT A.iID_MaChungTuChiTiet_Duyet, A.iID_MaChungTu_Duyet, B.* FROM KTTG_ChungTuCapThu_Duyet_ChiTiet AS A INNER JOIN KTTG_ChungTuChiTietCapThu AS B ON A.iID_MaChungTuChiTiet = B.iID_MaChungTuChiTiet WHERE {0} ORDER BY dNgayTao DESC", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Get_RowCapThuRutDuToan
        /// </summary>
        /// <param name="iID_MaChungTu_Duyet"></param>
        /// <returns></returns>
        public static DataTable Get_RowCapThuRutDuToan(String iID_MaChungTu_Duyet)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "B.iTrangThai = 1 AND A.iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet AND B.bRutDuToan = 1";
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            SQL = String.Format("SELECT B.sLNS,B.sL,B.sK,B.sM,B.sTM ,SUM(B.rSoTienDuyet) as rSoTien FROM KTTG_ChungTuCapThu_Duyet_ChiTiet AS A INNER JOIN KTTG_ChungTuChiTietCapThu AS B ON A.iID_MaChungTuChiTiet = B.iID_MaChungTuChiTiet WHERE {0} GROUP BY B.sLNS,B.sL,B.sK,B.sM,B.sTM  ", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// CheckChungTuChiTiet_ChuyenRutDuToan
        /// </summary>
        /// <param name="iID_MaChungTu_Duyet"></param>
        /// <returns></returns>
        public static Boolean CheckChungTuChiTiet_ChuyenRutDuToan(String iID_MaChungTu_Duyet){
            Boolean vR = false;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "B.iTrangThai = 1 AND A.iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet AND B.bRutDuToan = 1";
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            SQL = String.Format("SELECT COUNT(*) FROM KTTG_ChungTuCapThu_Duyet_ChiTiet AS A INNER JOIN KTTG_ChungTuChiTietCapThu AS B ON A.iID_MaChungTuChiTiet = B.iID_MaChungTuChiTiet WHERE {0} ", DK);
            cmd.CommandText = SQL;
            int iCount = Convert.ToInt32(Connection.GetValue(cmd, 0));            
            cmd.Dispose();
            if(iCount > 0){
                vR = true;
            }
            return vR;
        }
        public static void CapNhapTruongTongCapTongThu(String iID_MaChungTu_Duyet)
        {
            SqlCommand cmd;
            //String mySQL = "UPDATE KTTG_ChungTuChiTietCapThu SET rSoTienDuyet=(select sum( WHERE iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet";
            //cmd = new SqlCommand(mySQL);
            //cmd.Parameters.AddWithValue("@rSoTienDuyet", rTongCap);
            //Connection.UpdateDatabase(cmd);
            //cmd.Dispose();

            Double rTongCap = 0, rTongThu = 0, rTien = 0;
            String SQL_CT = "SELECT isNULL(SUM(rSoTienDuyet),0) FROM KTTG_ChungTuChiTietCapThu WHERE bLoai = {0} AND bRutDuToan = 0 AND iTrangThai = 1 AND iID_MaChungTuChiTiet IN (SELECT iID_MaChungTuChiTiet FROM KTTG_ChungTuCapThu_Duyet_ChiTiet WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet)";
            SQL_CT = @"SELECT rSoTienDuyet
FROM(
SELECT iID_MaTinhChatCapThu,isNULL(SUM(rSoTienDuyet),0) as rSoTienDuyet
FROM KTTG_ChungTuChiTietCapThu
 WHERE  bRutDuToan = 0 AND iTrangThai = 1 
 AND iID_MaChungTuChiTiet IN (SELECT iID_MaChungTuChiTiet FROM KTTG_ChungTuCapThu_Duyet_ChiTiet WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet)
 GROUP BY iID_MaTinhChatCapThu
 ) as KTTG_ChungTuChiTietCapThu
INNER JOIN(
SELECT * FROM KTTG_TinhChatCapThu WHERE bLoai={0}) as KTTG_TinhChatCapThu
ON KTTG_ChungTuChiTietCapThu.iID_MaTinhChatCapThu=KTTG_TinhChatCapThu.iID_MaTinhChatCapThu
";

            //cmd = new SqlCommand("SELECT isNULL(SUM(B.rSoTienDuyet),0)FROM KTTG_ChungTuCapThu_Duyet_ChiTiet AS A INNER JOIN KTTG_ChungTuChiTietCapThu AS B ON A.iID_MaChungTuChiTiet = B.iID_MaChungTuChiTiet WHERE A.iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet AND  B.bLoai = 0 AND B.iTrangThai = 1");
            cmd = new SqlCommand();
            cmd.CommandText = String.Format(SQL_CT, 0);
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            rTongCap = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            //cmd = new SqlCommand("SELECT isNULL(SUM(B.rSoTienDuyet),0)FROM KTTG_ChungTuCapThu_Duyet_ChiTiet AS A INNER JOIN KTTG_ChungTuChiTietCapThu AS B ON A.iID_MaChungTuChiTiet = B.iID_MaChungTuChiTiet WHERE A.iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet AND B.bLoai = 1 AND B.iTrangThai = 1");
            cmd = new SqlCommand();
            cmd.CommandText = String.Format(SQL_CT, 1);
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            rTongThu = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            rTien = rTongCap - rTongThu;

            String SQL = "UPDATE KTTG_ChungTuCapThu_Duyet SET rTongCap=@rTongCap, rTongThu=@rTongThu, rSoTien=@rSoTien WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@rTongCap", rTongCap);
            cmd.Parameters.AddWithValue("@rTongThu", rTongThu);
            cmd.Parameters.AddWithValue("@rSoTien", rTien);
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        /// <summary>
        /// CapNhapTruongTongCapTongThu
        /// </summary>
        /// <param name="iID_MaChungTu_Duyet"></param>
        public static void CapNhapTruongTongCapTongThu_Duyet(String iID_MaChungTu_Duyet)
        {
            SqlCommand cmd;
            Double rTongCap = 0, rTongThu = 0, rTien = 0;
            String SQL_CT = "SELECT isNULL(SUM(rSoTienDuyet),0) FROM KTTG_ChungTuChiTietCapThu WHERE bLoai = {0} AND bRutDuToan = 0 AND iTrangThai = 1 AND iID_MaChungTuChiTiet IN (SELECT iID_MaChungTuChiTiet FROM KTTG_ChungTuCapThu_Duyet_ChiTiet WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet)";
            SQL_CT= @"SELECT rSoTienDuyet
FROM(
SELECT iID_MaTinhChatCapThu,isNULL(SUM(rSoTienDuyet),0) as rSoTienDuyet
FROM KTTG_ChungTuChiTietCapThu
 WHERE  bRutDuToan = 0 AND iTrangThai = 1 
 AND iID_MaChungTuChiTiet IN (SELECT iID_MaChungTuChiTiet FROM KTTG_ChungTuCapThu_Duyet_ChiTiet WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet)
 GROUP BY iID_MaTinhChatCapThu
 ) as KTTG_ChungTuChiTietCapThu
INNER JOIN(
SELECT * FROM KTTG_TinhChatCapThu WHERE bLoai={0}) as KTTG_TinhChatCapThu
ON KTTG_ChungTuChiTietCapThu.iID_MaTinhChatCapThu=KTTG_TinhChatCapThu.iID_MaTinhChatCapThu
";
            
            //cmd = new SqlCommand("SELECT isNULL(SUM(B.rSoTienDuyet),0)FROM KTTG_ChungTuCapThu_Duyet_ChiTiet AS A INNER JOIN KTTG_ChungTuChiTietCapThu AS B ON A.iID_MaChungTuChiTiet = B.iID_MaChungTuChiTiet WHERE A.iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet AND  B.bLoai = 0 AND B.iTrangThai = 1");
            cmd = new SqlCommand();
            cmd.CommandText = String.Format(SQL_CT, 0);
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            rTongCap = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            //cmd = new SqlCommand("SELECT isNULL(SUM(B.rSoTienDuyet),0)FROM KTTG_ChungTuCapThu_Duyet_ChiTiet AS A INNER JOIN KTTG_ChungTuChiTietCapThu AS B ON A.iID_MaChungTuChiTiet = B.iID_MaChungTuChiTiet WHERE A.iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet AND B.bLoai = 1 AND B.iTrangThai = 1");
            cmd = new SqlCommand();
            cmd.CommandText = String.Format(SQL_CT, 1);
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            rTongThu = Convert.ToDouble(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            rTien = rTongCap - rTongThu;

            //String SQL = "UPDATE KTTG_ChungTuCapThu_Duyet SET rTongCap=@rTongCap, rTongThu=@rTongThu, rSoTien=@rSoTien WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet";
            String SQL = "UPDATE KTTG_ChungTuCapThu_Duyet SET rSoTien=@rSoTien WHERE iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet";
            cmd = new SqlCommand(SQL);
            //cmd.Parameters.AddWithValue("@rTongCap", rTongCap);
            cmd.Parameters.AddWithValue("@rTongThu", rTongThu);
            cmd.Parameters.AddWithValue("@rSoTien", rTien);
            cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", iID_MaChungTu_Duyet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        /// <summary>
        /// Update_rSoTien_KTTG_ChungTuChiTietCapThu
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="iID_MaCapPhatChiTiet"></param>
        /// <param name="rTien"></param>
        public static void Update_rSoTien_KTTG_ChungTuChiTietCapThu(String iID_MaCapPhat, String iID_MaCapPhatChiTiet, Double rTien)
        {
            SqlCommand cmd;
            String SQL = "UPDATE KTTG_ChungTuChiTietCapThu SET rSoTien=@rSoTien WHERE iID_MaCapPhat=@iID_MaCapPhat AND iID_MaCapPhatChiTiet=@iID_MaCapPhatChiTiet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@rSoTien", rTien);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            cmd.Parameters.AddWithValue("@iID_MaCapPhatChiTiet", iID_MaCapPhatChiTiet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
    }
}