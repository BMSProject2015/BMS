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
    public class TCDN_ChiTieuModels
    {
        public static String LayXauChiTieu(String sTenChiTieu, String sKyHieu, String Path, String XauHanhDong,
            String XauSapXep, String MaChiTieuCha, int Cap, ref int ThuTu)
        {
            String vR = "";
            String SQL = "";
            SqlCommand cmd1 = new SqlCommand();
            String strSQL = "SELECT Top 1 * FROM TCDN_ChiTieu WHERE iTrangThai=1 ";
            Boolean Ok = false;
            if (String.IsNullOrEmpty(sTenChiTieu) == false)
            {
                Ok = true;
                strSQL += " AND sTen LIKE @sTen";
                cmd1.Parameters.AddWithValue("@sTen", sTenChiTieu + "%");
            }
            if (String.IsNullOrEmpty(sKyHieu) == false && sKyHieu != "")
            {
                Ok = true;
                strSQL += " AND sKyHieu= @sKyHieu";
                cmd1.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            }
            strSQL += " ORDER BY iLoai,iSTT";
            cmd1.CommandText = strSQL;
            if (Ok)
            {
                DataTable dt1 = Connection.GetDataTable(cmd1);
                cmd1.Dispose();

                if (dt1.Rows.Count > 0)
                {
                    MaChiTieuCha = Convert.ToString(dt1.Rows[0]["iID_MaChiTieu_Cha"]);
                    sKyHieu = "";
                    sTenChiTieu = "";
                }
            }

            SqlCommand cmd = new SqlCommand();
            if (MaChiTieuCha != null && MaChiTieuCha != "")
            {
                SQL = string.Format("SELECT * FROM TCDN_ChiTieu WHERE iTrangThai=1 AND iID_MaChiTieu_Cha = '{0}'", MaChiTieuCha);
            }
            else
            {
                SQL = "SELECT * FROM TCDN_ChiTieu WHERE iTrangThai=1 AND iID_MaChiTieu_Cha = 0";
            }
            //SQL = string.Format("SELECT * FROM TCDN_ChiTieu WHERE 1=1");
            //if (String.IsNullOrEmpty(sTenChiTieu) == false && sTenChiTieu != "")
            //{
            //    //SQL += " AND sTen like N'%' +  @sTen + '%'";
            //    SQL += " AND sTen LIKE @sTen";
            //    cmd.Parameters.AddWithValue("@sTen", sTenChiTieu +"%");
            //}
            //if (String.IsNullOrEmpty(sKyHieu) == false && sKyHieu != "")
            //{
            //    SQL += "  sKyHieu= @sKyHieu)";
            //    cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            //}
            SQL += " ORDER BY iLoai,iSTT";
            cmd.CommandText = SQL;            
            DataTable dt = Connection.GetDataTable(cmd);
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
                    // int STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String strLoai = "";
                    switch (Convert.ToInt32(Row["iLoai"])) { 
                        case 0:
                            strLoai = "Thuộc bảng cân đối kế toán";
                            break;
                        case 1:
                            strLoai = "Chỉ tiêu ngoài bảng";
                            break;
                        case 2:
                            strLoai = "Chỉ tiêu báo cáo kết quả kinh doanh";
                            break;
                        default:
                            strLoai = "Thuộc bảng cân đối kế toán";
                            break;
                    }
                    if (Convert.ToInt32(Row["iLoai"]) == 0) { 
                        
                    }
                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaChiTieu"].ToString());
                    strXauMucLucQuanSoCon = LayXauChiTieu(sTenChiTieu, sKyHieu, Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaChiTieu"]), Cap + 1, ref ThuTu);

                    if (strXauMucLucQuanSoCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaChiTieu"].ToString());
                    }
                    strPG += string.Format("<tr>");
                    if (Cap == 0)
                    {
                        // strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\"><b>{0}</b></td>", ThuTu);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, HttpUtility.HtmlEncode(Row["sKyHieu"]));
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, HttpUtility.HtmlEncode(Row["sTen"]));
                      //  strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}</td>", HttpUtility.HtmlEncode(Row["sThuyetMinh"]));
                        //strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}</td>", strLoai);
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {
                            // strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\"><b>{0}</b></td>", ThuTu);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, HttpUtility.HtmlEncode(Row["sKyHieu"]));
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, HttpUtility.HtmlEncode(Row["sTen"]));
                          //  strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}</td>", HttpUtility.HtmlEncode(Row["sThuyetMinh"]));
                          //  strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}</td>", HttpUtility.HtmlEncode(strLoai));
                        }
                        else
                        {
                            // strPG += string.Format("<td style=\"padding: 3px 3px;\"><b>{0}</b></td>", ThuTu);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, HttpUtility.HtmlEncode(Row["sKyHieu"]));
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, HttpUtility.HtmlEncode(Row["sTen"]));
                           // strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", HttpUtility.HtmlEncode(Row["sThuyetMinh"]));
                           // strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", HttpUtility.HtmlEncode(strLoai));
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
        public static int CheckLoaiChiTieu(String iID_MaChiTieu)
        {
            Int32 vR;
            SqlCommand cmd = new SqlCommand("SELECT iLoai FROM TCDN_ChiTieu WHERE iID_MaChiTieu=@iID_MaChiTieu AND iTrangThai=1");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_ChiTietChiTieu_Row(String iID_MaChiTieu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM TCDN_ChiTieu WHERE iTrangThai=1 AND iID_MaChiTieu=@iID_MaChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            
            return vR;
        }
        public static int Get_So_KyHieuChiTieu(String sKyHieu)
        {
            int vR = 0;
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM TCDN_ChiTieu WHERE iTrangThai=1 AND sKyHieu=@sKyHieu");
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            return vR;
        }
        public static DataTable DT_LoaiChiTieu()
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("ID", typeof(int));
            vR.Columns.Add("sTen", typeof(String));
            DataRow Row;

            Row = vR.NewRow();
            Row[0] = 2;
            Row[1] = "Chỉ tiêu báo cáo kết quả kinh doanh";
            vR.Rows.InsertAt(Row, 0);

            Row = vR.NewRow();
            Row[0] = 1;
            Row[1] = "Chỉ tiêu ngoài bảng";
            vR.Rows.Add(Row);

            Row = vR.NewRow();
            Row[0] = 0;
            Row[1] = "Thuộc bảng cân đối kế toán";
            vR.Rows.Add(Row);
            
            return vR;
        }
        public static Boolean Delete(String iID_MaChiTieu)
        {
            Boolean vR = false;
            Bang bang = new Bang("TCDN_ChiTieu");
            bang.GiaTriKhoa = iID_MaChiTieu;
            bang.Delete();
            vR = true;
            return vR;
        }
        public static DataTable DT_MucLucChiTieu(String iLoai)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_ChiTieu WHERE iTrangThai = 1 AND iLoai=@iLoai ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            
            return vR;
        }
        public static DataTable Get_MucLucChiTieu()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_ChiTieu WHERE iTrangThai = 1 ORDER BY iSTT";
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_ChiTieu_Theo_KyHieu(String sKyHieu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT TOP 1 TCDN_ChiTieu.* FROM TCDN_ChiTieu WHERE iTrangThai = 1 AND sKyHieu=@sKyHieu ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
    }
}