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
    public class QuyetToan_QuanSo_MucLucModels
    {
        /// <summary>
        /// Lấy thông tin của một mục lục quân số
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTin(String iID_MaMucLucQuanSo)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetMucLucQuanSo(iID_MaMucLucQuanSo);
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
        /// Lấy thông tin một bản ghi trong bảng mục lục quân số
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo"></param>
        /// <returns></returns>
        public static DataTable GetMucLucQuanSo(String iID_MaMucLucQuanSo)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_MucLucQuanSo WHERE iTrangThai=1 AND iID_MaMucLucQuanSo=@iID_MaMucLucQuanSo");
            cmd.Parameters.AddWithValue("@iID_MaMucLucQuanSo", iID_MaMucLucQuanSo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy thông tin một bản ghi trong bảng mục lục quân số
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo"></param>
        /// <returns></returns>
        public static DataTable GetChiTietMucLucQuanSo(String sKyHieu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_MucLucQuanSo WHERE iTrangThai=1 AND sKyHieu=@sKyHieu");
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách mục lục quân số trong bảng NS_MucLucQuanSo theo mã cha
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        public static DataTable List_MucLucQuanSo(String iID_MaMucLucQuanSo_Cha)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            if (iID_MaMucLucQuanSo_Cha != null && iID_MaMucLucQuanSo_Cha != "")
            {
                cmd.CommandText = "SELECT * FROM NS_MucLucQuanSo WHERE iID_MaMucLucQuanSo_Cha=@iID_MaMucLucQuanSo_Cha AND iTrangThai = 1  ORDER BY iSTT";
                cmd.Parameters.AddWithValue("@iID_MaMucLucQuanSo_Cha", iID_MaMucLucQuanSo_Cha);
            }
            else
            {
                cmd.CommandText = "SELECT * FROM NS_MucLucQuanSo WHERE iID_MaMucLucQuanSo_Cha is Null AND iTrangThai = 1 ORDER BY iSTT";
            }
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        
        public static DataTable DT_MucLucQuanSo()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM NS_MucLucQuanSo WHERE iTrangThai = 1 ORDER BY iSTT";
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable DT_MucLucQuanSo(String sKyHieu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM NS_MucLucQuanSo WHERE iTrangThai = 1 AND sKyHieu=@sKyHieu ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Xóa một mục lục quân số
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo"></param>
        /// <returns></returns>
        public static Boolean DeleteMucLucQuanSo(String iID_MaMucLucQuanSo)
        {
            Boolean vR = false;
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "DELETE NS_MucLucQuanSo WHERE iID_MaMucLucQuanSo_Cha=@iID_MaMucLucQuanSo";
            cmd.Parameters.AddWithValue("@iID_MaMucLucQuanSo", iID_MaMucLucQuanSo);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            Bang bang = new Bang("NS_MucLucQuanSo");
            bang.GiaTriKhoa = iID_MaMucLucQuanSo;
            bang.Delete();
            vR = true;

            return vR;
        }
        /// <summary>
        /// Tính số con của loại mục lục quân số cha
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo"></param>
        /// <returns></returns>
        public static int SoHangCon(String iID_MaMucLucQuanSo) { 
            int vR = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT count(*) FROM NS_MucLucQuanSo WHERE iID_MaMucLucQuanSo_Cha=@iID_MaMucLucQuanSo");
            cmd.Parameters.AddWithValue("@iID_MaMucLucQuanSo", iID_MaMucLucQuanSo);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy hình cây mục lục quân số
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="XauHanhDong"></param>
        /// <param name="XauSapXep"></param>
        /// <param name="MaMucLucQuanSoCha"></param>
        /// <param name="Cap"></param>
        /// <param name="ThuTu"></param>
        /// <returns></returns>
        public static String LayXauMucLucQuanSo(string Path, string XauHanhDong, string XauSapXep, String MaMucLucQuanSoCha, int Cap, ref int ThuTu)
        {
            String vR = "";
            String SQL = "";
            if (MaMucLucQuanSoCha != null && MaMucLucQuanSoCha != "")
            {
                SQL = string.Format("SELECT * FROM NS_MucLucQuanSo WHERE iTrangThai = 1 AND iID_MaMucLucQuanSo_Cha = '{0}' ORDER BY iSTT", MaMucLucQuanSoCha);
            }
            else {
                SQL = string.Format("SELECT * FROM NS_MucLucQuanSo WHERE iTrangThai = 1 AND iID_MaMucLucQuanSo_Cha is null ORDER BY iSTT", MaMucLucQuanSoCha);
            }            
            DataTable dt = Connection.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;

                string strPG = "", strXauMucLucQuanSoCon, strDoanTrang = "";

                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaMucLucQuanSo"].ToString());
                    strXauMucLucQuanSoCon = LayXauMucLucQuanSo(Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaMucLucQuanSo"]), Cap + 1, ref ThuTu);

                    if (strXauMucLucQuanSoCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaMucLucQuanSo"].ToString());
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
                    strPG += strXauMucLucQuanSoCon;
                }
                vR = String.Format("{0}", strPG);
            }
            dt.Dispose();
            return vR;
        }
    }
}