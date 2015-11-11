using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class DanhMucThuBaoHiemModels
    {
        public static String arrTruongTienBaoHiem = "rBHXH_CN,rBHYT_CN,rBHTN_CN,rBHXH_DV,rBHYT_DV,rBHTN_DV,rBHXH_CS,rLuongToiThieu";

        public static String strDSCot = "rBHXH_CN,rBHYT_CN,rBHTN_CN,rBHXH_DV,rBHYT_DV,rBHTN_DV,rBHXH_CS,rLuongToiThieu";
        public static String strDSTruongTien = "rBHXH_CN,rBHYT_CN,rBHTN_CN,rBHXH_DV,rBHYT_DV,rBHTN_DV,rBHXH_CS,rLuongToiThieu";
        public static String strDSTruongTienTieuDe = "% BHXH-CN,% BHYT-CN,% BHTN-CN,%BHXH-DV,%BHYT-DV,%BHTN-DV,%BHXH-CS,Lương TT";
        public static String strDSTruongTienDoRong = "100,100,100,100,100,100,100,100";

        public static String strDSTruongTieuDe = "Mã,Nội dung";
        public static String strDSTruong = "sKyHieu,sMoTa";
        public static String strDSTruongDoRong = "50,200";  

      public static NameValueCollection LayThongTin(String iNamLamViec, String iThang)
      {
          NameValueCollection Data = new NameValueCollection();
          DataTable dt = Get_DTDanhMucThuBaoHiem(iNamLamViec,iThang);
          String colName = "";
          for (int i = 0; i < dt.Columns.Count; i++)
          {
              colName = dt.Columns[i].ColumnName;
              Data[colName] = Convert.ToString(dt.Rows[0][i]);
          }
          dt.Dispose();
          return Data;
      }
      public static DataTable Get_DTDanhMucThuBaoHiem(String iNamLamViec, String iThang)
      {
          DataTable dt = null;
          String SQL = "SELECT * FROM BH_DanhMucThuBaoHiem WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang ORDER BY sKyHieu";
          SqlCommand cmd = new SqlCommand();
          cmd.CommandText = SQL;
          cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
          cmd.Parameters.AddWithValue("@iThang", iThang);
          dt = Connection.GetDataTable(cmd);
          cmd.Dispose();
          return dt;
      }
      public static DataTable Get_DTDanhMucThuBaoHiemCopy(String iNamLamViec, String iThang,String iThangCopy)
        {
            if (iThang == "1")
            {
                iThangCopy = "12";
                iNamLamViec = Convert.ToString(Convert.ToInt16(iNamLamViec) - 1);
            }
            
            DataTable dt = null;
            String SQL = "SELECT * FROM BH_DanhMucThuBaoHiem WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang ORDER BY sKyHieu";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec",iNamLamViec);

            cmd.Parameters.AddWithValue("@iThang", iThangCopy);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }


      public static DataTable GET_DanhSachThuBaoHiem(String iNamLamViec="")
      {
          DataTable dt = null;
          String DK="";
          if (String.IsNullOrEmpty(iNamLamViec) == false)
          {
              DK = " WHERE iNamLamViec=@iNamLamViec ";
          }
          String SQL = "SELECT Distinct iNamLamViec,iThang FROM BH_DanhMucThuBaoHiem {0}";
          SQL = String.Format(SQL, DK);
          SqlCommand cmd = new SqlCommand();
          cmd.CommandText = SQL;
          if (String.IsNullOrEmpty(iNamLamViec) == false)
          {
              cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
          }
          dt = Connection.GetDataTable(cmd);
          cmd.Dispose();
          return dt;
      }

      public static Boolean Check_Trung(String iNamLamViec, String iThang)
      {
          String SQL = "SELECT Count(*) FROM BH_DanhMucThuBaoHiem WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang";
          SqlCommand cmd = new SqlCommand(SQL);
          cmd.Parameters.AddWithValue("@iThang",iThang);
          cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
          Int32 vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
          cmd.Dispose();
          if (vR > 0)
              return true;
          return false;

      }
/// <summary>
      /// Lấy dt đã right join với bảng NS_MucLucDoiTuong
/// </summary>
/// <param name="iNamLamViec"></param>
/// <param name="iThang"></param>
/// <returns></returns>
      public static DataTable Get_DTChiTietDanhMuc(String iNamLamViec, String iThang)
      {
          DataTable dt = null;//ORDER BY sKyHieu
          String SQL = String.Format(@"SELECT iID_MaDanhMucThuBaoHiem
                    ,ML.iID_MaMucLucDoiTuong,ML.iID_MaMucLucDoiTuong_Cha
                    ,ML.sKyHieu,ML.sMoTa,ML.bLaHangCha,DM.iThang,DM.iNamLamViec
                    ,DM.rBHXH_CN,rBHXH_DV,rBHXH_CS,rBHYT_CN,rBHYT_DV,rBHTN_CN,rBHTN_DV,rLuongToiThieu
                    FROM(
                    (SELECT * FROM BH_DanhMucThuBaoHiem WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang) DM 
                    RIGHT JOIN NS_MucLucDoiTuong AS ML ON DM.sKyHieu=ML.sKyHieu)
                    ORDER BY ML.sKyHieu");
           SqlCommand cmd = new SqlCommand();
          cmd.CommandText = SQL;
          cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
          cmd.Parameters.AddWithValue("@iThang", iThang);
          dt = Connection.GetDataTable(cmd);
          cmd.Dispose();
          return dt;
      }
    }
}