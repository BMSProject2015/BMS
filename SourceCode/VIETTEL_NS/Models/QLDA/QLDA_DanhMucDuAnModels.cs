using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
namespace VIETTEL.Models
{
    public class QLDA_DanhMucDuAnModels
    {
        public static String sDSTruongTimKiem = "sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sDeAn,sDuAn";
        public static String sDSTruong = "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sTenDonVi,sTenChuDauTu,sTenBanQuanLy,sTienDo";
        public static String sDSTruongTieuDe = "Đề án,Dự án,DA thành phần,Công trình, HM công trình, HM chi tiết,Mô tả,Đơn vị,Chủ đầu tư,Tên ban quản lý,Tiến độ";
        public static String DSTruongDoRong = "60,60,80,80,80,60,250,100,200,200,150";
        public static String[] arrDSTruongDoRong = DSTruongDoRong.Split(',');
        public static String[] arrDSTruongTieuDe = sDSTruongTieuDe.Split(',');
        public static String[] arrDSTruong = sDSTruong.Split(',');
        public static String sDSTruongXauNoiMa = "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet";
        public static String[] arrDSTruongXauNoiMa = sDSTruongXauNoiMa.Split(',');
        public static DataTable ddl_DanhMucDuAn(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDanhMucDuAn, sDeAn +' - '+ sDuAn +' - '+ sDuAnThanhPhan +' - '+ sCongTrinh +' - '+ sHangMucCongTrinh +' - '+ sHangMucChiTiet +' - '+ sTenDuAn AS TenHT " +
                    "FROM QLDA_DanhMucDuAn WHERE iTrangThai = 1 ORDER By sXauNoiMa_DuAn");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDanhMucDuAn"] = Guid.Empty;
                R["TenHT"] = "--- Danh sách công trình dự án ---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static bool CheckHopDong(string iID_MaDanhMucDuAn)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  iID_MaHopDongChiTiet FROM QLDA_HopDongChiTiet WHERE (iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn OR iID_MaDanhMucDuAn_Cha=@iID_MaDanhMucDuAn) AND iTrangThai = 1 ORDER By sXauNoiMa_DuAn");
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            int count = dt.Rows.Count;
            if (dt != null) dt.Dispose();
            if (count > 0)
            {
                return true;
            }
            else return false;
        }

        public static DataTable Row_DanhMucDuAn(String iID_MaDanhMucDuAn)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT *, sDeAn +' - '+ sDuAn +' - '+ sDuAnThanhPhan +' - '+ sCongTrinh +' - '+ sHangMucCongTrinh +' - '+ sHangMucChiTiet +' - '+ sTenDuAn AS TenHT " +
                    "FROM QLDA_DanhMucDuAn WHERE iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn AND iTrangThai = 1 ORDER By sXauNoiMa_DuAn");
            cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtDanhMucDuAn(Dictionary<String, String> arrGiaTriTimKiem)
        {
            String DK = "";
            String SQL = "SELECT * FROM QLDA_DanhMucDuAn WHERE iTrangThai=1 {0}  ORDER BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet";
            SqlCommand cmd = new SqlCommand();
            #region Điều kiện
            String[] arrDSTruong = sDSTruongTimKiem.Split(',');
            if (arrGiaTriTimKiem != null)
            {
                for (int i = 0; i < arrDSTruong.Length; i++)
                {
                    if (String.IsNullOrEmpty(arrGiaTriTimKiem[arrDSTruong[i]]) == false)
                    {
                        DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrGiaTriTimKiem[arrDSTruong[i]]);
                    }
                }
            }
            #endregion Điều kiện
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static void CapNhapLai()
        {
            SqlCommand cmd;

            //Cap nhap lai xau ma
            String DK = "";
            String SQL = String.Format("SELECT * FROM QLDA_DanhMucDuAn {0} ORDER BY sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet", DK);
            DataTable dt = Connection.GetDataTable(SQL);
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow R = dt.Rows[i];
                string sXauNoiMa = "";
                Boolean bLaHangCha = false;
                for (int j = 0; j < arrDSTruongXauNoiMa.Length; j++)
                {

                    /* --------code cũ 05/09/2012-------*/
                    if (String.IsNullOrEmpty(Convert.ToString(R[arrDSTruongXauNoiMa[j]])) == false)
                    {
                        if (sXauNoiMa != "") sXauNoiMa += "-";
                        string sGiaTriTruong = Convert.ToString(R[arrDSTruongXauNoiMa[j]]);
                        sXauNoiMa += sGiaTriTruong.Trim().Equals("") ? "00" : sGiaTriTruong;
                    }
                    else
                    {

                        if (j < arrDSTruongXauNoiMa.Length - 2)
                        {
                            bLaHangCha = true;
                        }
                    }
                     
                    // quangvv4: 06/09/2012
                    /* 
                   if (sXauNoiMa != "") sXauNoiMa += "-";
                   string sGiaTriTruong = Convert.ToString(R[arrDSTruongXauNoiMa[j]]);
                   sXauNoiMa += sGiaTriTruong.Trim().Equals("") ? "00" : sGiaTriTruong;
                   if (String.IsNullOrEmpty(Convert.ToString(R[arrDSTruongXauNoiMa[j]])))
                   {

                       if (j < arrDSTruongXauNoiMa.Length - 2)
                       {
                           bLaHangCha = true;
                       }
                   }
                      */
                    // quangvv : 06/09/2012
                }
                cmd = new SqlCommand("UPDATE QLDA_DanhMucDuAn SET sXauNoiMa_DuAn=@sXauNoiMa, bLaHangCha_DuAn=@bLaHangCha " +
                                     " WHERE iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn");

                cmd.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                cmd.Parameters.AddWithValue("@bLaHangCha", bLaHangCha);

                /*  //quangvv : 06/09/2012
                cmd = new SqlCommand("UPDATE QLDA_DanhMucDuAn SET sXauNoiMa_DuAn=@sXauNoiMa, bLaHangCha_DuAn=@bLaHangCha " +
                                     ",sDeAn=@sDeAn,sDuAn=@sDuAn,sDuAnThanhPhan=@sDuAnThanhPhan,sCongTrinh=@sCongTrinh" +
                                     ",sHangMucCongTrinh=@sHangMucCongTrinh,sHangMucChiTiet=@sHangMucChiTiet" +
                                     " WHERE iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn");
                cmd.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                cmd.Parameters.AddWithValue("@bLaHangCha", bLaHangCha);
                
                 //Danh mục dự án phải nhập đầy đủ từ đề án, dự án, DA thành phần, Công trình, hạ mục công trình, HM chi tiết; nếu mục nào không có thì nhập “00”
                 //update gia tri các cot từ sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet
                 string[] sGiaTriMa = sXauNoiMa.Split('-');
                 string sDeAn = "00";
                 string sDuAn = "00";
                 string sDuAnThanhPhan = "00";
                 string sCongTrinh = "00";
                 string sHangMucCongTrinh = "00";
                 string sHangMucChiTiet = "00";
                 if (sGiaTriMa.Length >0)
                 {
                     sDeAn = sGiaTriMa[0];
                     sDuAn = sGiaTriMa[1];
                     sDuAnThanhPhan = sGiaTriMa[2];
                     sCongTrinh = sGiaTriMa[3];
                     sHangMucCongTrinh = sGiaTriMa[4];
                     sHangMucChiTiet = sGiaTriMa[5];
                 }
                 cmd.Parameters.AddWithValue("@sDeAn", sDeAn);
                 cmd.Parameters.AddWithValue("@sDuAn", sDuAn);
                 cmd.Parameters.AddWithValue("@sDuAnThanhPhan", sDuAnThanhPhan);
                 cmd.Parameters.AddWithValue("@sCongTrinh", sCongTrinh);
                 cmd.Parameters.AddWithValue("@sHangMucCongTrinh", sHangMucCongTrinh);
                 cmd.Parameters.AddWithValue("@sHangMucChiTiet", sHangMucChiTiet);
                 //quangvv4 end
                 * */
                cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", R["iID_MaDanhMucDuAn"]);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            dt.Dispose();
            //Cap nhap lai ma cha            
            dt = Connection.GetDataTable(SQL);
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow R = dt.Rows[i];
                Object iID_MaMucLucNganSachCha = DBNull.Value;

                string sXauNoiMa = Convert.ToString(R["sXauNoiMa_DuAn"]);

                for (int j = i - 1; j >= 0; j--)
                {
                    String tg_sXauNoiMa = Convert.ToString(dt.Rows[j]["sXauNoiMa_DuAn"]);
                    if (sXauNoiMa.StartsWith(tg_sXauNoiMa))
                    {
                        cmd = new SqlCommand("UPDATE QLDA_DanhMucDuAn SET iID_MaDanhMucDuAn_Cha=@iID_MaDanhMucDuAn_Cha WHERE iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn");
                        cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", R["iID_MaDanhMucDuAn"]);
                        cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn_Cha", dt.Rows[j]["iID_MaDanhMucDuAn"]);
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();
                        break;
                    }
                }
            }
            dt.Dispose();
        }
    }
}