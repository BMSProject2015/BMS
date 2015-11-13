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
    public class NganSach_DoiTuongModels
    {
        /// <summary>
        /// Lấy thông tin của một mục lục đối tượng
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTin(String iID_MaMucLucDoiTuong)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetMucLucDoiTuong(iID_MaMucLucDoiTuong);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        /// <summary>
        /// Lấy thông tin một bản ghi trong bảng mục lục đối tượng
        /// </summary>
        /// <param name="iID_MaMucLucDoiTuong"></param>
        /// <returns></returns>
        public static DataTable GetMucLucDoiTuong(String iID_MaMucLucDoiTuong)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_MucLucDoiTuong WHERE iTrangThai=1 AND iID_MaMucLucDoiTuong=@iID_MaMucLucDoiTuong");
            cmd.Parameters.AddWithValue("@iID_MaMucLucDoiTuong", iID_MaMucLucDoiTuong);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy thông tin ký hiệu của đối tượng
        /// </summary>
        /// <param name="sKyHieu"></param>
        /// <returns></returns>
        public static int GetKyHieuDoiTuong(String sKyHieu)
        {
            int vR = 0;
            SqlCommand cmd = new SqlCommand("SELECT count(*) FROM NS_MucLucDoiTuong WHERE iTrangThai=1 AND sKyHieu=@sKyHieu");
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            vR = Convert.ToInt16(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách mục lục đối tượng trong bảng NS_MucLucDoiTuong theo mã cha
        /// </summary>
        /// <param name="iID_MaMucLucDoiTuong_Cha"></param>
        /// <returns></returns>
        public static DataTable List_MucLucDoiTuong(String iID_MaMucLucDoiTuong_Cha)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            if (iID_MaMucLucDoiTuong_Cha != null && iID_MaMucLucDoiTuong_Cha != "")
            {
                cmd.CommandText = "SELECT * FROM NS_MucLucDoiTuong WHERE iID_MaMucLucDoiTuong_Cha=@iID_MaMucLucDoiTuong_Cha AND iTrangThai = 1  ORDER BY iSTT";
                cmd.Parameters.AddWithValue("@iID_MaMucLucDoiTuong_Cha", iID_MaMucLucDoiTuong_Cha);
            }
            else
            {
                cmd.CommandText = "SELECT * FROM NS_MucLucDoiTuong WHERE iID_MaMucLucDoiTuong_Cha is Null AND iTrangThai = 1 ORDER BY iSTT";
            }
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable DT_MucLucDoiTuong()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM NS_MucLucDoiTuong WHERE iTrangThai = 1 ORDER BY iSTT";
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Xóa một mục lục đối tượng
        /// </summary>
        /// <param name="iID_MaMucLucDoiTuong"></param>
        /// <returns></returns>
        public static Boolean DeleteMucLucDoiTuong(String iID_MaMucLucDoiTuong)
        {
            Boolean vR = false;
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "DELETE NS_MucLucDoiTuong WHERE iID_MaMucLucDoiTuong_Cha=@iID_MaMucLucDoiTuong";
            cmd.Parameters.AddWithValue("@iID_MaMucLucDoiTuong", iID_MaMucLucDoiTuong);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            Bang bang = new Bang("NS_MucLucDoiTuong");
            bang.GiaTriKhoa = iID_MaMucLucDoiTuong;
            bang.Delete();
            vR = true;

            return vR;
        }
        /// <summary>
        /// Tính số con của loại mục lục đối tượng cha
        /// </summary>
        /// <param name="iID_MaMucLucDoiTuong"></param>
        /// <returns></returns>
        public static int SoHangCon(String iID_MaMucLucDoiTuong)
        {
            int vR = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT count(*) FROM NS_MucLucDoiTuong WHERE iID_MaMucLucDoiTuong_Cha=@iID_MaMucLucDoiTuong");
            cmd.Parameters.AddWithValue("@iID_MaMucLucDoiTuong", iID_MaMucLucDoiTuong);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy hình cây mục lục
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="XauHanhDong"></param>
        /// <param name="XauSapXep"></param>
        /// <param name="MaMucLucDoiTuongCha"></param>
        /// <param name="Cap"></param>
        /// <param name="ThuTu"></param>
        /// <returns></returns>
        public static String LayXauMucLuc(string Path, string XauHanhDong, string XauSapXep, String MaMucLucDoiTuongCha, int Cap, ref int ThuTu)
        {
            String vR = "";
            String SQL = "";
            if (MaMucLucDoiTuongCha != null && MaMucLucDoiTuongCha != "")
            {
                SQL = string.Format("SELECT * FROM NS_MucLucDoiTuong WHERE iID_MaMucLucDoiTuong_Cha = '{0}' ORDER BY iSTT", MaMucLucDoiTuongCha);
            }
            else
            {
                SQL = string.Format("SELECT * FROM NS_MucLucDoiTuong WHERE iID_MaMucLucDoiTuong_Cha is null ORDER BY iSTT", MaMucLucDoiTuongCha);
            }
            DataTable dt = Connection.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;

                string strPG = "", strXauMucLucDoiTuongCon, strDoanTrang = "";

                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaMucLucDoiTuong"].ToString());
                    strXauMucLucDoiTuongCon = LayXauMucLuc(Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaMucLucDoiTuong"]), Cap + 1, ref ThuTu);

                    if (strXauMucLucDoiTuongCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaMucLucDoiTuong"].ToString());
                    }
                    strPG += string.Format("<tr>");
                    if (Cap == 0)
                    {
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sKyHieu"]);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sMoTa"]);
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sMoTa"]);
                        }
                        else
                        {
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sMoTa"]);
                        }
                    }
                    if (tgThuTu % 2 == 0)
                    {
                        strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}</td>", strHanhDong);
                    }
                    else
                    {
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", strHanhDong);
                    }

                    strPG += string.Format("</tr>");
                    strPG += strXauMucLucDoiTuongCon;
                }
                vR = String.Format("{0}", strPG);
            }
            dt.Dispose();
            return vR;
        }
    }
}