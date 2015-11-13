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
    public class TCDN_HoSoDoanhNghiep_ChiTieuModels
    {
        public static String LayXauChiTieu(String sTaiKhoan, String sKyHieu, String Path, String XauHanhDong,
            String XauSapXep, String MaChiTieuCha, int Cap, ref int ThuTu)
        {
            String vR = "";
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            if (MaChiTieuCha != null && MaChiTieuCha != "")
            {
                SQL = string.Format("SELECT * FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE iTrangThai=1 AND iID_MaChiTieuHoSo_Cha = '{0}'", MaChiTieuCha);
            }
            else
            {
                SQL = "SELECT * FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE iTrangThai=1 AND iID_MaChiTieuHoSo_Cha = 0";
            }
            //SQL = string.Format("SELECT * FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE 1=1");
            if (String.IsNullOrEmpty(sTaiKhoan) == false && sTaiKhoan != "")
            {
                //SQL += " AND sTen like N'%' +  @sTen + '%'";
                SQL += " AND sTen= @sTen";
                cmd.Parameters.AddWithValue("@sTen", sTaiKhoan);
            }
            if (String.IsNullOrEmpty(sKyHieu) == false && sKyHieu != "")
            {
                SQL += " AND sKyHieu= @sKyHieu";
                cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            }
            SQL += " ORDER BY iLoai DESC,iSTT";
            cmd.CommandText = SQL;
            //DataTable dt = CommonFunction.dtData(cmd, "", Trang, SoBanGhi);
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
                    // int STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String strLoai = "";
                    switch (Convert.ToInt32(Row["iLoai"]))
                    {
                        case 0:
                            strLoai = "Chi tiêt thu nộp";
                            break;
                        case 1:
                            strLoai = "Chỉ tiêu tài chính";
                            break;
                        case 2:
                            strLoai = "Chỉ tiêu hồ sơ doanh nghiệp";
                            break;
                        default:
                            strLoai = "Chi tiêt thu nộp";
                            break;
                    }

                    String strChiTieu = "";
                    DataTable dtChiTieu = Get_MucLucChiTieu_ChiTieuHoSo(Convert.ToString(Row["iID_MaChiTieuHoSo"]));
                    for (int j = 0; j < dtChiTieu.Rows.Count; j++) {
                        DataTable dtChiTieuChiTiet = TCDN_ChiTieuModels.Get_ChiTietChiTieu_Row(Convert.ToString(dtChiTieu.Rows[j]["iID_MaChiTieu"]));
                        strChiTieu += Convert.ToString(dtChiTieuChiTiet.Rows[0]["sKyHieu"]) + " - " + Convert.ToString(dtChiTieuChiTiet.Rows[0]["sTen"]) + "<br/>";
                        dtChiTieuChiTiet.Dispose();
                    }
                    dtChiTieu.Dispose();

                    if (Convert.ToInt32(Row["iLoai"]) == 0){}
                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaChiTieuHoSo"].ToString());
                    strXauMucLucQuanSoCon = LayXauChiTieu(sTaiKhoan, sKyHieu, Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaChiTieuHoSo"]), Cap + 1, ref ThuTu);

                    if (strXauMucLucQuanSoCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaChiTieuHoSo"].ToString());
                    }
                    strPG += string.Format("<tr>");
                    if (Cap == 0)
                    {
                        // strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\"><b>{0}</b></td>", ThuTu);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sKyHieu"]);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sTen"]);
                        strPG += string.Format("<td align=\"center\" style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}</td>", Row["sDonViTinh"]);
                        strPG += string.Format("<td align=\"center\" style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}</td>", strLoai);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}</td>", strChiTieu);
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {
                            // strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\"><b>{0}</b></td>", ThuTu);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                            strPG += string.Format("<td align=\"center\" style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}</td>", Row["sDonViTinh"]);
                            strPG += string.Format("<td align=\"center\" style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}</td>", strLoai);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}</td>", strChiTieu);
                        }
                        else
                        {
                            // strPG += string.Format("<td style=\"padding: 3px 3px;\"><b>{0}</b></td>", ThuTu);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                            strPG += string.Format("<td align=\"center\" style=\"padding: 3px 3px;\">{0}</td>", Row["sDonViTinh"]);
                            strPG += string.Format("<td align=\"center\" style=\"padding: 3px 3px;\">{0}</td>", strLoai);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", strChiTieu);
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
        public static DataTable Get_ChiTietChiTieu_Row(String iID_MaChiTieuHoSo)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE iTrangThai=1 AND iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo");
            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static int Get_So_KyHieuChiTieu(String sKyHieu)
        {
            int vR = 0;
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE iTrangThai=1 AND sKyHieu=@sKyHieu");
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
            Row[1] = "Chỉ tiêu hồ sơ doanh nghiệp";
            vR.Rows.InsertAt(Row, 0);

            Row = vR.NewRow();
            Row[0] = 1;
            Row[1] = "Chỉ tiêu tài chính";
            vR.Rows.Add(Row);

            Row = vR.NewRow();
            Row[0] = 0;
            Row[1] = "Chi tiêt thu nộp";
            vR.Rows.Add(Row);

            return vR;
        }
        public static Boolean Delete(String iID_MaChiTieuHoSo)
        {
            Boolean vR = false;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM TCDN_HoSoDoanhNghiepChiTieu_ChiTieuCanDoi WHERE iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo";
            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo);
            Connection.GetDataTable(cmd);
            cmd.Dispose();

            Bang bang = new Bang("TCDN_HoSoDoanhNghiep_ChiTieu");
            bang.GiaTriKhoa = iID_MaChiTieuHoSo;
            bang.Delete();
            vR = true;
            return vR;
        }
        public static DataTable DT_MucLucChiTieu(String iLoai)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE iLoai=@iLoai AND iTrangThai = 1 ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_MucLucChiTieu(String iLoai)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE iLoai=@iLoai AND iTrangThai = 1 ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_MucLucChiTieu_ChiTieuHoSo(String iID_MaChiTieuHoSo)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_HoSoDoanhNghiepChiTieu_ChiTieuCanDoi WHERE iTrangThai = 1 AND iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable Get_MucLucChiTieuHoSo_BaoCaoTaiChinh(String iID_MaChiTieuHoSo)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_BaoCaoTaiChinhTruongLayDuLieu_ChiTieuHoSo WHERE iTrangThai = 1 AND iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static String LayXauChiTieuCanDoi(String iID_MaChiTieuHoSo, String MaChiTieuCha, int Cap, ref int ThuTu)
        {
            String vR = "";
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            if (MaChiTieuCha != null && MaChiTieuCha != "")
            {
                SQL = string.Format("SELECT * FROM TCDN_ChiTieu WHERE iTrangThai=1 AND iID_MaChiTieu_Cha = '{0}'", MaChiTieuCha);
            }
            else
            {
                SQL = "SELECT * FROM TCDN_ChiTieu WHERE iTrangThai=1 AND iID_MaChiTieu_Cha = 0";
            }
            SQL += " ORDER BY iSTT";
            cmd.CommandText = SQL;
            //DataTable dt = CommonFunction.dtData(cmd, "", Trang, SoBanGhi);
            DataTable dt = Connection.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                int i,j, tgThuTu;

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

                    strXauMucLucQuanSoCon = LayXauChiTieuCanDoi(iID_MaChiTieuHoSo, Convert.ToString(Row["iID_MaChiTieu"]), Cap + 1, ref ThuTu);

                    DataTable dtChiTieu = Get_MucLucChiTieu_ChiTieuHoSo(iID_MaChiTieuHoSo);
                    
                    String _Checked = "";

                    for (j = 0; j < dtChiTieu.Rows.Count; j++)
                    {
                        if (Convert.ToString(Row["iID_MaChiTieu"]) == Convert.ToString(dtChiTieu.Rows[j]["iID_MaChiTieu"]))
                        {
                            _Checked = "checked=\"checked\"";
                            break;
                        }
                    }    

                    strPG += string.Format("<tr>");
                    if (Cap == 0)
                    {
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\"><b>{0}</b></td>", "<input type=\"checkbox\" value=\"" + Convert.ToString(Row["iID_MaChiTieu"]) + "\" " + _Checked + " check-group=\"ChiTieuCanDoi\" id=\"chkChiTieuCanDoi\" name=\"iID_MaChiTieu\" />");
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sKyHieu"]);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sTen"]);
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\"><b>{0}</b></td>", "<input type=\"checkbox\" value=\"" + Convert.ToString(Row["iID_MaChiTieu"]) + "\" " + _Checked + " check-group=\"ChiTieuCanDoi\" id=\"chkChiTieuCanDoi\" name=\"iID_MaChiTieu\" />");
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                        }
                        else
                        {
                            strPG += string.Format("<td style=\"padding: 3px 3px;\"><b>{0}</b></td>", "<input type=\"checkbox\" value=\"" + Convert.ToString(Row["iID_MaChiTieu"]) + "\" " + _Checked + " check-group=\"ChiTieuCanDoi\" id=\"chkChiTieuCanDoi\" name=\"iID_MaChiTieu\" />");
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                        }
                    }

                    strPG += string.Format("</tr>");
                    strPG += strXauMucLucQuanSoCon;
                }
                vR = String.Format("{0}", strPG);
            }
            dt.Dispose();
            return vR;
        }
        public static String LayXauChuyenSoLieuBaoCaoTaiChinh(String iID_MaChiTieuHoSo)
        { 
            String vR = "";
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            SQL = "SELECT * FROM TCDN_BaoCaoTaiChinhTruongLayDuLieu WHERE iTrangThai=1 ORDER BY iSTT";
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(SQL);
            DataRow Row;
            if (dt.Rows.Count > 0)
            {
                int i, j, tgThuTu;
                string strPG = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    tgThuTu = i;
                    Row = dt.Rows[i];

                    DataTable dtChiTieu = Get_MucLucChiTieuHoSo_BaoCaoTaiChinh(iID_MaChiTieuHoSo);

                    String _Checked = "";

                    for (j = 0; j < dtChiTieu.Rows.Count; j++)
                    {
                        if (Convert.ToString(Row["iID_MaTruongKhoa"]) == Convert.ToString(dtChiTieu.Rows[j]["iID_MaTruongKhoa"]))
                        {
                            _Checked = "checked=\"checked\"";
                            break;
                        }
                    }    

                    strPG += string.Format("<tr>");
                    if (tgThuTu % 2 == 0)
                    {
                        strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\"><b>{0}</b></td>", "<input type=\"checkbox\" value=\"" + Convert.ToString(Row["iID_MaTruongKhoa"]) + "\" " + _Checked + " check-group=\"ChiTieuBaoCaoTaiChinh\" id=\"chkChiTieuBaoCaoTaiChinh\" name=\"iID_MaChiTieuBaoCaoTaiChinh\" />");
                        strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}</td>", Row["sTen"]);
                    }
                    else
                    {
                        strPG += string.Format("<td style=\"padding: 3px 3px; width: 10%;\"><b>{0}</b></td>", "<input type=\"checkbox\" value=\"" + Convert.ToString(Row["iID_MaTruongKhoa"]) + "\" " + _Checked + " check-group=\"ChiTieuBaoCaoTaiChinh\" id=\"chkChiTieuBaoCaoTaiChinh\" name=\"iID_MaChiTieuBaoCaoTaiChinh\" />");
                        strPG += string.Format("<td style=\"padding: 3px 3px; width: 90%\">{0}</td>", Row["sTen"]);
                    }
                    
                    strPG += string.Format("</tr>");
                }
                vR = String.Format("{0}", strPG);
            }

            return vR;
        }
    }
}