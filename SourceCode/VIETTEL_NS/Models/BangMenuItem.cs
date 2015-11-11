using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;

namespace VIETTEL.CacBang
{
    public class BangMenuItem:Bang
    {
        public BangMenuItem()
            : base("MENU_MenuItem")
        {
            GiaTriKhoa = null;
        }
        

        override public Dictionary<string, object> LayGoiDuLieu(NameValueCollection dataIn, Boolean DuLieuSua)
        {
            Dictionary<string, object> dicData = null;
            NameValueCollection data = null;

            if (dataIn != null)
            {
                data = new NameValueCollection();
                data.Add(dataIn);
            }
            else
            {
                data = this.dataTheoGiaTriKhoa();
            }

            dicData = new Dictionary<string, object>();
            dicData["TenBang"] = this.TenBang;
            dicData["TruongKhoa"] = this.TruongKhoa;
            if (this.GiaTriKhoa != null)
            {
                data[this.TruongKhoa] = this.GiaTriKhoa.ToString();
            }

            dicData["data"] = data;
            return dicData;
        }

        override public Boolean IsValid(string TenTruong, ref object GiaTri, NameValueCollection arrLoi)
        {


            if (TenTruong.ToUpper() == "sTen")
            {
                arrLoi.Add("err_sTen", NgonNgu.LayXau("Phải teen ."));
            }
          
            return true;
        }

        public string LayXauMenu(string Path, string XauHanhDong, string XauSapXep, int MaMenuItemCha, int Cap, ref int ThuTu)
        {
            string vR = "";
            String SQL = string.Format("SELECT * FROM MENU_MenuItem WHERE iID_MaMenuItemCha={0} ORDER BY tThuTu, iID_MaMenuItem", MaMenuItemCha);
            DataTable dt = Connection.GetDataTable(SQL);
      
            if (dt.Rows.Count > 0)
            {
                int i,tgThuTu;
               
                string strPG = "", url, strXauMenuCon, strDoanTrang="";
                
                for (i=1;i<=Cap;i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaMenuItem"].ToString());
                    strXauMenuCon = LayXauMenu(Path, XauHanhDong, XauSapXep, (int)(Row["iID_MaMenuItem"]), Cap + 1, ref ThuTu);

                    if (strXauMenuCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaMenuItem"].ToString());
                    }
                    url = "#";
                    if (dt.Rows[i]["sURL"] != DBNull.Value && string.IsNullOrEmpty((string)(Row["sURL"])) == false && (string)(Row["sURL"]) != "#")
                    {
                        url = Path + Row["sURL"];
                    }
                    strPG += string.Format("<tr>");
                    if (Cap==0)
                    {
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;\">{1}<b><a href=\"{0}\">{2}</a></b></td>", url, strDoanTrang, Row["sTen"]);
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {
                            strPG += string.Format("<td style=\"background-color:#dff0fb;\">{1}<a href=\"{0}\">{2}</a></td>", url, strDoanTrang, Row["sTen"]);
                        }
                        else
                        {
                            strPG += string.Format("<td>{1}<a href=\"{0}\">{2}</a></td>", url, strDoanTrang, Row["sTen"]);
                        }
                          
                    }
                    if (tgThuTu % 2 == 0)
                    {
                        strPG += string.Format("<td style=\"background-color:#dff0fb;\">{0}</td>", strHanhDong);
                    }
                    else
                    {
                        strPG += string.Format("<td>{0}</td>", strHanhDong);
                    }
                   
                    strPG += string.Format("</tr>"); 
                    strPG += strXauMenuCon;
                }
                vR = String.Format("{0}", strPG);
            }
            dt.Dispose();
            return vR;
        }

        public string LayXauMenuItemCam(string MaLuat, int MaMenuItemCha, int Cap, ref int ThuTu)
        {
            int MaMenuItem;
            string vR = "";
            String SQL = string.Format("SELECT * FROM MENU_MenuItem WHERE bHoatDong=1 AND iID_MaMenuItemCha={0} ORDER BY tThuTu, iID_MaMenuItem", MaMenuItemCha);
            DataTable dt = Connection.GetDataTable(SQL);
            SqlCommand cmd;

            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;

                string strPG = "", strXauMenuCon, strDoanTrang = "";

                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String strTG;

                    MaMenuItem = Convert.ToInt32(Row["iID_MaMenuItem"]);
                    strXauMenuCon = LayXauMenuItemCam(MaLuat, MaMenuItem, Cap + 1, ref ThuTu);
                    cmd = new SqlCommand("SELECT Count(*) FROM PQ_MenuItem_Cam WHERE iID_MaMenuItem=@iID_MaMenuItem AND iID_MaLuat=@iID_MaLuat");
                    cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
                    cmd.Parameters.AddWithValue("@iID_MaMenuItem", MaMenuItem);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        strTG = String.Format("<input name=\"{0}\" value=\"{1}\" type=\"checkbox\" checked=\"checked\" >", "MenuItem_Cam", MaMenuItem);
                    }
                    else
                    {
                        strTG = String.Format("<input name=\"{0}\" value=\"{1}\" type=\"checkbox\" >", "MenuItem_Cam", MaMenuItem);
                    }
                    cmd.Dispose();
                    String classtr = "";
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    strPG += string.Format("<tr " + classtr + ">");
                    if (tgThuTu % 2 == 0)
                    {
                        strPG += string.Format("<td style=\"padding: 3px 2px;\">{1}{0}</td>", Row["sTen"], strDoanTrang);
                    }
                    else
                    {
                        strPG += string.Format("<td style=\"padding: 3px 2px;\">{1}{0}</td>", Row["sTen"], strDoanTrang);
                    }
                    if (tgThuTu % 2 == 0)
                    {
                        strPG += string.Format("<td style=\"padding: 3px 2px;\" align=\"center\">{0}</td>", strTG);
                    }
                    else
                    {
                        strPG += string.Format("<td style=\"padding: 3px 2px;\" align=\"center\">{0}</td>", strTG);
                    }

                    strPG += string.Format("</tr>");
                    strPG += strXauMenuCon;
                }
                vR = String.Format("{0}", strPG);
            }
            dt.Dispose();
            return vR;
        }
    }
}
