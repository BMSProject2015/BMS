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
using System.Text;

namespace VIETTEL.Models
{
    public class TuLieuLichSuModels
    {
        /// <summary>
        /// Lấy ra danh sách tài khoản kế toán theo hình cây
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="XauHanhDong"></param>
        /// <param name="XauSapXep"></param>
        /// <param name="LayXauTaiKhoanKeToan"></param>
        /// <param name="Cap"></param>
        /// <param name="ThuTu"></param>
        /// <returns></returns>
        public static String LayXauDanhMucTaiLieu(string Path, string XauHanhDong,
            string XauSapXep, String iID_MaKieuTaiLieu_Cha, int Cap, ref int ThuTu, string UserName,int STT)
        {
            String vR = "";
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            if (iID_MaKieuTaiLieu_Cha != null && iID_MaKieuTaiLieu_Cha != "")
            {
                SQL = string.Format("SELECT * FROM TL_DanhMucTaiLieu WHERE iTrangThai = 1 AND iID_MaKieuTaiLieu_Cha = '{0}'", iID_MaKieuTaiLieu_Cha);

            }
            else
            {
                SQL = "SELECT * FROM TL_DanhMucTaiLieu WHERE iTrangThai = 1 AND iID_MaKieuTaiLieu_Cha = ''";

            }
            SQL += " ORDER BY iSTT";
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
                     STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaKieuTaiLieu"].ToString());
                    strXauMucLucQuanSoCon = LayXauDanhMucTaiLieu(Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaKieuTaiLieu"]), Cap + 1, ref ThuTu, UserName, STT);

                    if (strXauMucLucQuanSoCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaKieuTaiLieu"].ToString());
                    }
                    strPG += string.Format("<tr>");
                    if (Cap == 0)
                    {
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\"><b>{0}</b></td>", strDoanTrang+STT);
                        //strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["iID_MaKieuTaiLieu"]);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sTen"]);
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\"><b>{0}</b></td>", strDoanTrang +STT);
                            //strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["iID_MaKieuTaiLieu"]);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                        }
                        else
                        {
                            strPG += string.Format("<td style=\"padding: 3px 3px;\"><b>{0}</b></td>", strDoanTrang+STT);
                            //strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["iID_MaKieuTaiLieu"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
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

        public static Boolean Delete(String iID_MaChiTieuHoSo)
        {
            Boolean vR = false;

            Bang bang = new Bang("TL_DanhMucTaiLieu");
            bang.GiaTriKhoa = iID_MaChiTieuHoSo;
            bang.Delete();
            vR = true;
            return vR;
        }

        public static DataTable DT_DanhMucTaiLieu_Cay()
        {
            DataTable vR;

            SqlCommand cmd = new SqlCommand("SELECT * FROM TL_DanhMucTaiLieu WHERE iTrangThai = 1");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            vR = dt.Clone();
            DataRow Row;
            int i, j, k;
            String strNoi = "---";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt.Rows[i]["iID_MaKieuTaiLieu_Cha"]) == 0)
                {
                    Row = vR.NewRow();
                    for (k = 0; k < dt.Rows.Count; k++)
                    {
                        Row[k] = dt.Rows[i][k];
                    }
                    vR.Rows.Add(Row);
                    for (j = 0; j < dt.Rows.Count; j++)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["iID_MaKieuTaiLieu"]) == Convert.ToInt32(dt.Rows[j]["iID_MaKieuTaiLieu_Cha"]))
                        {
                            Row = vR.NewRow();
                            for (k = 0; k < dt.Rows.Count; k++)
                            {
                                Row[k] = dt.Rows[j][k];
                            }
                            Row["sTen"] = strNoi + Convert.ToString(Row["sTen"]);
                            vR.Rows.Add(Row);
                        }
                    }
                }
            }

            return vR;
        }

        public static bool FindChildNode(int iID_MaKieuTaiLieu, DataTable dt)
        {
            bool Flag = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((int)iID_MaKieuTaiLieu == int.Parse(dt.Rows[i]["iID_MaKieuTaiLieu_Cha"].ToString()))
                    Flag = true;
            }
            return Flag;
        }
        public static string Create_Select_Tree(int iID_MaKieuTaiLieu, DataTable dt, int iID_MaKieuTaiLieu_Cha_Id, string varSpace)
        {
            varSpace += "&nbsp;&nbsp;&nbsp;&nbsp;";
            StringBuilder tbl = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int intiID_MaKieuTaiLieu_Cha = int.Parse(dt.Rows[i]["iID_MaKieuTaiLieu_Cha"].ToString());
                if (intiID_MaKieuTaiLieu_Cha == iID_MaKieuTaiLieu)
                {
                    int intiID_MaKieuTaiLieu = int.Parse(dt.Rows[i]["iID_MaKieuTaiLieu"].ToString());
                    string strCatName = dt.Rows[i]["sTen"].ToString();
                    string strSelected = string.Empty;
                    if (iID_MaKieuTaiLieu_Cha_Id == intiID_MaKieuTaiLieu)
                        strSelected = " selected ";
                    tbl.Append("<option value='" + intiID_MaKieuTaiLieu + "'" + strSelected + ">" + varSpace + "-&nbsp;" + strCatName + "</option>");
                    if (FindChildNode(intiID_MaKieuTaiLieu, dt))
                    {
                        tbl.Append(Create_Select_Tree(intiID_MaKieuTaiLieu, dt, iID_MaKieuTaiLieu_Cha_Id, varSpace));
                    }
                }
            }
            return tbl.ToString();
        }

        public static string wrDanhMucTaiLieu_DDL(String ParentID, int intiID_MaKieuTaiLieu)
        {
            StringBuilder tbl = new StringBuilder();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TL_DanhMucTaiLieu WHERE iTrangThai = 1 ORDER BY iSTT");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            string varSpace = string.Empty;
            tbl.Append("            <select id='" + ParentID + "_iID_MaKieuTaiLieu' name='" + ParentID + "_iID_MaKieuTaiLieu' style='width:100%;'>");
            tbl.Append("                <option value='0'>-- Chọn lĩnh vực --</option>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (int.Parse(dt.Rows[i]["iID_MaKieuTaiLieu_Cha"].ToString()) == 0)
                {
                    string strCatName = dt.Rows[i]["sTen"].ToString();
                    int iID_MaKieuTaiLieu = int.Parse(dt.Rows[i]["iID_MaKieuTaiLieu"].ToString());
                    string strSelected = string.Empty;
                    if (intiID_MaKieuTaiLieu == iID_MaKieuTaiLieu)
                        strSelected = " selected ";
                    tbl.Append("        <option value='" + iID_MaKieuTaiLieu + "'" + strSelected + ">" + strCatName + "</option>");
                    if (FindChildNode(iID_MaKieuTaiLieu, dt))
                    {
                        varSpace = "";
                        tbl.Append(Create_Select_Tree(iID_MaKieuTaiLieu, dt, intiID_MaKieuTaiLieu, varSpace));
                    }
                }
            }
            tbl.Append("            </select>");

            return tbl.ToString();
        }
        public static String LayDSMaTaiLieuCon(String MaKieuTaiLieu)
        {
            String sDanhSach = "";
            String SQL = "SELECT iID_MaKieuTaiLieu FROM TL_DanhMucTaiLieu WHERE iTrangThai=1 AND iID_MaKieuTaiLieu_Cha=@MaKieuTaiLieu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@MaKieuTaiLieu", MaKieuTaiLieu);
            DataTable dt = Connection.GetDataTable(cmd);
            String tg = "";
            int i = 0;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                sDanhSach += Convert.ToString(dt.Rows[i]["iID_MaKieuTaiLieu"]) + ",";
                tg = Convert.ToString(dt.Rows[i]["iID_MaKieuTaiLieu"]);
                String sDanhSachCon = LayDSMaTaiLieuCon(tg);
                sDanhSach += sDanhSachCon;
            }
            return sDanhSach;
        }

        public static DataTable Get_DanhSachVanBan(String MaKieuTaiLieu, String sTen,String sSoHieu,String iDM_LoaiVanBan,String iDM_NoiBanHanh,String sNguoiKy, String TuNgay, String DenNgay, String sTuKhoa,String sHieuLuc,int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            String DK = "iTrangThai=1";
            SqlCommand cmd;
            cmd = new SqlCommand();
            String sDanhSach = "";
            sDanhSach = LayDSMaTaiLieuCon(MaKieuTaiLieu);
            sDanhSach += MaKieuTaiLieu;
            if (MaKieuTaiLieu != "0" && MaKieuTaiLieu!="-1")
            {
                if (DK != "") DK += " AND ";
                DK += "TL_VanBan.iID_MaKieuTaiLieu IN(" + sDanhSach + ")  ";

            }//lấy thông tin 10 van ban gan nhat
            else if(MaKieuTaiLieu=="-1")
            {
                DK += " AND TL_VanBan.dNgayTao>=GETDATE()-10";
            }

            if (String.IsNullOrEmpty(sTen) == false)
            {
                DK += " AND TL_VanBan.sTen LIKE N'%"+sTen+"%'";
            }
            if (String.IsNullOrEmpty(sTuKhoa) == false && String.IsNullOrEmpty(sTen) == false)
            {
                DK += " AND TL_VanBan.sTuKhoaLQ LIKE N'%" + sTuKhoa + "%'";
            }
            if (String.IsNullOrEmpty(sTuKhoa) == false && String.IsNullOrEmpty(sTen) == true)
            {
                DK += " AND (TL_VanBan.sTuKhoaLQ LIKE N'%" + sTuKhoa + "%'" ;
                DK += " OR TL_VanBan.sTen LIKE N'%" + sTuKhoa + "%')";
            }
            if (String.IsNullOrEmpty(sSoHieu) == false)
            {
                DK += " AND TL_VanBan.sSoHieu LIKE N'%" + sSoHieu + "%'";
            }
            if (String.IsNullOrEmpty(iDM_LoaiVanBan) == false && iDM_LoaiVanBan !=Guid.Empty.ToString())
            {
                DK += " AND TL_VanBan.iDM_LoaiVanBan =@iDM_LoaiVanBan";
                cmd.Parameters.AddWithValue("@iDM_LoaiVanBan", iDM_LoaiVanBan);
            }
            if (String.IsNullOrEmpty(iDM_NoiBanHanh) == false && iDM_NoiBanHanh != Guid.Empty.ToString())
            {
                DK += " AND TL_VanBan.iDM_NoiBanHanh =@iDM_NoiBanHanh";
                cmd.Parameters.AddWithValue("@iDM_NoiBanHanh", iDM_NoiBanHanh);
            }
            if (String.IsNullOrEmpty(sNguoiKy) == false && sNguoiKy != "--Chọn người ký--") 
            {
                DK += " AND TL_VanBan.sNguoiKy =@sNguoiKy";
                cmd.Parameters.AddWithValue("@sNguoiKy", sNguoiKy);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "Từ Ngày")
            {
                DK += " AND dNgayBanHanh >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "Đến Ngày")
            {
                DK += " AND dNgayBanHanh <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            if (sHieuLuc != "0" && !String.IsNullOrEmpty(sHieuLuc))
            {
                DK += " AND iHieuLuc=@iHieuLuc";
                cmd.Parameters.AddWithValue("@iHieuLuc",sHieuLuc);
            }
            String SQL = String.Format("SELECT * FROM TL_VanBan WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayBanHanh DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        public static int Get_DanhSachVanBan_Count(String MaKieuTaiLieu, String sTen, String sSoHieu, String iDM_LoaiVanBan, String iDM_NoiBanHanh, String sNguoiKy, String TuNgay, String DenNgay, String sTuKhoa, String sHieuLuc)
        {
            int vR;
            String DK = "iTrangThai=1";
            SqlCommand cmd;
            cmd = new SqlCommand();
            String sDanhSach = "";
            sDanhSach = LayDSMaTaiLieuCon(MaKieuTaiLieu);
            sDanhSach += MaKieuTaiLieu;
            if (MaKieuTaiLieu != "0" && MaKieuTaiLieu != "-1")
            {
                if (DK != "") DK += " AND ";
                DK += "TL_VanBan.iID_MaKieuTaiLieu IN(" + sDanhSach + ")  ";

            }//lấy thông tin 10 van ban gan nhat
            else if (MaKieuTaiLieu == "-1")
            {
                DK += " AND TL_VanBan.dNgayTao>=GETDATE()-10";
            }

            if (String.IsNullOrEmpty(sTen) == false)
            {
                DK += " AND TL_VanBan.sTen LIKE N'%" + sTen + "%'";
            }
            if (String.IsNullOrEmpty(sTuKhoa) == false && String.IsNullOrEmpty(sTen) == false)
            {
                DK += " AND TL_VanBan.sTuKhoaLQ LIKE N'%" + sTuKhoa + "%'";
            }
            if (String.IsNullOrEmpty(sTuKhoa) == false && String.IsNullOrEmpty(sTen) == true)
            {
                DK += " AND (TL_VanBan.sTuKhoaLQ LIKE N'%" + sTuKhoa + "%'";
                DK += " OR TL_VanBan.sTen LIKE N'%" + sTuKhoa + "%')";
            }
            if (String.IsNullOrEmpty(sSoHieu) == false)
            {
                DK += " AND TL_VanBan.sSoHieu LIKE N'%" + sSoHieu + "%'";
            }
            if (String.IsNullOrEmpty(iDM_LoaiVanBan) == false && iDM_LoaiVanBan != Guid.Empty.ToString())
            {
                DK += " AND TL_VanBan.iDM_LoaiVanBan =@iDM_LoaiVanBan";
                cmd.Parameters.AddWithValue("@iDM_LoaiVanBan", iDM_LoaiVanBan);
            }
            if (String.IsNullOrEmpty(iDM_NoiBanHanh) == false && iDM_NoiBanHanh != Guid.Empty.ToString())
            {
                DK += " AND TL_VanBan.iDM_NoiBanHanh =@iDM_NoiBanHanh";
                cmd.Parameters.AddWithValue("@iDM_NoiBanHanh", iDM_NoiBanHanh);
            }
            if (String.IsNullOrEmpty(sNguoiKy) == false && sNguoiKy != "--Chọn người ký--")
            {
                DK += " AND TL_VanBan.sNguoiKy =@sNguoiKy";
                cmd.Parameters.AddWithValue("@sNguoiKy", sNguoiKy);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "Từ Ngày")
            {
                DK += " AND dNgayBanHanh >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "Đến Ngày")
            {
                DK += " AND dNgayBanHanh <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            if (sHieuLuc == "2")
            {
                DK += " AND DATEDIFF(dd,dNgayHetHan,getDate())>0";
            }
            if (sHieuLuc == "1")
            {
                DK += " AND( DATEDIFF(dd,dNgayHetHan,getDate())<0  OR DATEDIFF(dd,dNgayHetHan,getDate()) is null )";
            }
            String SQL = String.Format("SELECT COUNT(*) FROM TL_VanBan WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachTaiLieu(String MaKieuTaiLieu, String MaDonVi, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            String DK = "iTrangThai=1";
            SqlCommand cmd;
            cmd = new SqlCommand();
            String sDanhSach = "";
            sDanhSach = LayDSMaTaiLieuCon(MaKieuTaiLieu);
                sDanhSach += MaKieuTaiLieu;
            if (MaKieuTaiLieu != "0")
            {
                if (DK != "") DK += " AND ";
                DK = "TL_TaiLieu.iID_MaKieuTaiLieu IN("+sDanhSach+")  ";
             
            }

            if (String.IsNullOrEmpty(MaDonVi) == false)
            {
                if (DK != "") DK += " AND ";
                DK += String.Format("(sMaDonVi='' OR sMaDonVi LIKE '%{0}%')", MaDonVi);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "Từ Ngày")
            {
                DK += " AND dNgayUpload >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "Đến Ngày")
            {
                DK += " AND dNgayUpload <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }

            String SQL = String.Format("SELECT * FROM TL_TaiLieu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaKieuTaiLieu,dNgayUpload DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachTaiLieu_Count(String MaKieuTaiLieu, String MaDonVi, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd;
            String DK = "iTrangThai=1";
            cmd = new SqlCommand();
            String sDanhSach = "";
            sDanhSach=LayDSMaTaiLieuCon(MaKieuTaiLieu);
            if (MaKieuTaiLieu != "0")
            {
                if (DK != "") DK += " AND ";
                DK = "TL_TaiLieu.iID_MaKieuTaiLieu IN(@sDanhSach)";
                cmd.Parameters.AddWithValue("@sDanhSach", sDanhSach);
            }

            if (String.IsNullOrEmpty(MaDonVi) == false)
            {
                if (DK != "") DK += " AND ";
                DK += String.Format("(sMaDonVi='' OR sMaDonVi LIKE '%{0}%')", MaDonVi);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "Từ Ngày")
            {
                DK += " AND dNgayUpload >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "Đến Ngày")
            {
                DK += " AND dNgayUpload <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT Count(*) FROM TL_TaiLieu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachVideo(String MaKieuTaiLieu, String MaDonVi, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            String DK = "iTrangThai=1";
            SqlCommand cmd;
            cmd = new SqlCommand();
            String sDanhSach = "";
            sDanhSach = LayDSMaTaiLieuCon(MaKieuTaiLieu);
            sDanhSach += MaKieuTaiLieu;
            if (MaKieuTaiLieu != "0")
            {
                if (DK != "") DK += " AND ";
                DK = "TL_Video.iID_MaKieuTaiLieu IN(" + sDanhSach + ")  ";

            }

            if (String.IsNullOrEmpty(MaDonVi) == false)
            {
                if (DK != "") DK += " AND ";
                DK += String.Format("(sMaDonVi='' OR sMaDonVi LIKE '%{0}%')", MaDonVi);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "Từ Ngày")
            {
                DK += " AND dNgayUpload >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "Đến Ngày")
            {
                DK += " AND dNgayUpload <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }

            String SQL = String.Format("SELECT * FROM TL_Video WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaKieuTaiLieu,dNgayUpload DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachVideo_Count(String MaKieuTaiLieu, String MaDonVi, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd;
            String DK = "iTrangThai=1";
            cmd = new SqlCommand();
            String sDanhSach = "";
            sDanhSach = LayDSMaTaiLieuCon(MaKieuTaiLieu);
            sDanhSach += MaKieuTaiLieu;
            if (MaKieuTaiLieu != "0")
            {
                if (DK != "") DK += " AND ";
                DK = "TL_Video.iID_MaKieuTaiLieu IN(" + sDanhSach + ")  ";

            }

            if (String.IsNullOrEmpty(MaDonVi) == false)
            {
                if (DK != "") DK += " AND ";
                DK += String.Format("(sMaDonVi='' OR sMaDonVi LIKE '%{0}%')", MaDonVi);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "Từ Ngày")
            {
                DK += " AND dNgayUpload >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "Đến Ngày")
            {
                DK += " AND dNgayUpload <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT Count(*) FROM TL_Video WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static DataTable Get_DanhSachHinhAnh(String MaKieuTaiLieu, String MaDonVi, String TuNgay, String DenNgay, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            String DK = "iTrangThai=1";
            SqlCommand cmd;
            cmd = new SqlCommand();
            String sDanhSach = "";
            sDanhSach = LayDSMaTaiLieuCon(MaKieuTaiLieu);
            sDanhSach += MaKieuTaiLieu;
            if (MaKieuTaiLieu != "0")
            {
                if (DK != "") DK += " AND ";
                DK = "TL_Anh.iID_MaKieuTaiLieu IN(" + sDanhSach + ")  ";

            }

            if (String.IsNullOrEmpty(MaDonVi) == false)
            {
                if (DK != "") DK += " AND ";
                DK += String.Format("(sMaDonVi='' OR sMaDonVi LIKE '%{0}%')", MaDonVi);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "Từ Ngày")
            {
                DK += " AND dNgayUpload >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "Đến Ngày")
            {
                DK += " AND dNgayUpload <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }

            String SQL = String.Format("SELECT * FROM TL_Anh WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaKieuTaiLieu,dNgayUpload DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachHinhAnh_Count(String MaKieuTaiLieu, String MaDonVi, String TuNgay, String DenNgay)
        {
            int vR;
            SqlCommand cmd;
            String DK = "iTrangThai=1";
            cmd = new SqlCommand();
            String sDanhSach = "";
            sDanhSach = LayDSMaTaiLieuCon(MaKieuTaiLieu);
            sDanhSach += MaKieuTaiLieu;
            if (MaKieuTaiLieu != "0")
            {
                if (DK != "") DK += " AND ";
                DK = "TL_Anh.iID_MaKieuTaiLieu IN(" + sDanhSach + ")  ";

            }

            if (String.IsNullOrEmpty(MaDonVi) == false)
            {
                if (DK != "") DK += " AND ";
                DK += String.Format("(sMaDonVi='' OR sMaDonVi LIKE '%{0}%')", MaDonVi);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "Từ Ngày")
            {
                DK += " AND dNgayUpload >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "Đến Ngày")
            {
                DK += " AND dNgayUpload <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }
            String SQL = String.Format("SELECT Count(*) FROM TL_Anh WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static string wrDanhMucTaiLieu_Cay(String ParentID, int intiID_MaKieuTaiLieu)
        {
            StringBuilder tbl = new StringBuilder();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TL_DanhMucTaiLieu WHERE iTrangThai = 1 ORDER BY iSTT");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            string varSpace = string.Empty;
            tbl.Append("            <table cellpadding='0' cellspacing='0' border='0' width='100%' class='table_form3'");
            tbl.Append("<tr><td class = 'td_form2_td1_left' style = 'height:24px'><a style='color:Black' href = 'javascript:timTheoMuc(0);'><b>Lĩnh vực</b></a></td></tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (int.Parse(dt.Rows[i]["iID_MaKieuTaiLieu_Cha"].ToString()) == 0)
                {
                   
                    
                    int iID_MaKieuTaiLieu = int.Parse(dt.Rows[i]["iID_MaKieuTaiLieu"].ToString());
                    String sDanhSach = "";
                    sDanhSach = LayDSMaTaiLieuCon(iID_MaKieuTaiLieu+"");
                    sDanhSach += iID_MaKieuTaiLieu;
                    String SQL = String.Format(@"SELECT COUNT(*) FROM TL_VanBan WHERE iTrangThai=1 AND iID_MaKieuTaiLieu IN(" + sDanhSach + ")");
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iID_MaKieuTaiLieu", iID_MaKieuTaiLieu);
                    String count = Connection.GetValueString(cmd, "");
                    string strCatName = "";
                    if (!String.IsNullOrEmpty(count))
                        strCatName = dt.Rows[i]["sTen"].ToString() + "("+count+")";
                    else
                        strCatName = dt.Rows[i]["sTen"].ToString();
                    string strSelected = string.Empty;
                    bool hasChild = FindChildNode(iID_MaKieuTaiLieu, dt);
                    tbl.Append("<tr id = 'tree_row" + iID_MaKieuTaiLieu + "'>");
                    if (intiID_MaKieuTaiLieu == iID_MaKieuTaiLieu)
                        strSelected = " selected ";
                    tbl.Append("        <td  class = 'td_form2_td4' style = 'height:24px'>");
                    if (hasChild)
                    {
                        tbl.Append("<a href = 'javascript:showChild(" + iID_MaKieuTaiLieu + ");'><img width='10px' height='10px' id='tree_img" + iID_MaKieuTaiLieu + "' src = '/Content/Themes/images/minus.gif' alt = 'collapse' /></a>");
                    }
                    tbl.Append("<a style='color:Black'  href = 'javascript:timTheoMuc(" + iID_MaKieuTaiLieu + ");'>" + strCatName + "</a>");
                    tbl.Append("        <input type = 'hidden' name = 'parentIdStr' id='parent_id" + iID_MaKieuTaiLieu + "' value = '0' />");
                    tbl.Append("        </td>");
                    tbl.Append("</tr>");
                    if (hasChild)
                    {
                        varSpace = "";
                        tbl.Append(Create_Menu_Tree(iID_MaKieuTaiLieu, dt, intiID_MaKieuTaiLieu, varSpace, iID_MaKieuTaiLieu.ToString()));
                    }
                }
            }
            tbl.Append("            </table>");

            return tbl.ToString();
        }
        public static string Create_Menu_Tree(int iID_MaKieuTaiLieu, DataTable dt, int iID_MaKieuTaiLieu_Cha_Id, string varSpace, string parentIdStr)
        {
            varSpace += "&nbsp;&nbsp;&nbsp;&nbsp;";
            StringBuilder tbl = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int intiID_MaKieuTaiLieu_Cha = int.Parse(dt.Rows[i]["iID_MaKieuTaiLieu_Cha"].ToString());
                if (intiID_MaKieuTaiLieu_Cha == iID_MaKieuTaiLieu)
                {
                    int intiID_MaKieuTaiLieu = int.Parse(dt.Rows[i]["iID_MaKieuTaiLieu"].ToString());
                    string strCatName = dt.Rows[i]["sTen"].ToString();
                    string strSelected = string.Empty;
                    if (iID_MaKieuTaiLieu_Cha_Id == intiID_MaKieuTaiLieu)
                        strSelected = " selected ";
                    bool hasChild = FindChildNode(intiID_MaKieuTaiLieu, dt);
                    tbl.Append("<tr id = 'tree_row" + intiID_MaKieuTaiLieu + "'>");
                    tbl.Append("        <td class = 'td_form2_td4' style = 'height:24px'>");
                    tbl.Append(varSpace);
                    if (hasChild)
                    {
                        tbl.Append("<a  href = 'javascript:showChild(" + intiID_MaKieuTaiLieu + ");'><img width='10px' height='10px' id='tree_img" + intiID_MaKieuTaiLieu + "' src = '/Content/Themes/images/minus.gif' alt = 'collapse' /></a>");
                    }
                    else
                    {
                        tbl.Append("&nbsp;-&nbsp;");
                    }
                    tbl.Append("<a style='color:Black'  href = 'javascript:timTheoMuc(" + intiID_MaKieuTaiLieu + ");'>" + strCatName + "</a>");
                    tbl.Append("        <input type = 'hidden' name = 'parentIdStr' id='parent_id" + intiID_MaKieuTaiLieu + "' value = '" + parentIdStr + "' />");
                    tbl.Append("        </td>");
                    tbl.Append("</tr>");
                    if (hasChild)
                    {
                        tbl.Append(Create_Menu_Tree(intiID_MaKieuTaiLieu, dt, iID_MaKieuTaiLieu_Cha_Id, varSpace, parentIdStr + "_" + intiID_MaKieuTaiLieu));
                    }
                }
            }
            return tbl.ToString();
        }

        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String MaTuLieu, string tableName, string tableIdName)
        {
            int vR = -1;
            DataTable dt = getDetail(MaTuLieu, tableName, tableIdName);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            if (dt != null) dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeTuLieuLichSu, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                vR = iID_MaTrangThaiDuyet_TuChoi;
            }
            return vR;
        }

        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String MaTuLieu, string tableName, string tableIdName)
        {
            int vR = -1;
            DataTable dt = getDetail(MaTuLieu, tableName, tableIdName);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            if (dt != null) dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeTuLieuLichSu, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }

        public static DataTable getDetail(String iID_MaTuLieu, string tableName, string tableIdName)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            string SQL = string.Format(@"SELECT * FROM {0} WHERE {1}=@iID_MaTuLieu", tableName, tableIdName);
            cmd.Parameters.AddWithValue("@iID_MaTuLieu", iID_MaTuLieu);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaTuLieu, int iID_MaTrangThaiDuyet, string tableName, string tableIdName)
        {
            SqlCommand cmd;
            String SQL = string.Format("UPDATE {0} SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE {1}=@iID_MaTuLieu", tableName, tableIdName);
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaTuLieu", iID_MaTuLieu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }
        public static String InsertDuyetTuLieu(String iID_MaTuLieu, String NoiDung, String MaND, String IPSua, string tableName)
        {
            String MaDuyetTuLieu;
            Bang bang = new Bang(tableName);
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTuLieu", iID_MaTuLieu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetTuLieu = Convert.ToString(bang.Save());
            return MaDuyetTuLieu;
        }

        public static DataTable LayDanhSachThuMuc()
        {
            String SQL = String.Format(@"select  tm.iID_MaThuMucTaiLieu, tm.sTen,dm.sTen as loaidanhmuc, tm.dNgayTao, tm.bHoatDong  from TL_ThuMucTaiLieu tm
                                            join  DC_DanhMuc dm on dm.iID_MaDanhMuc = tm.iID_MaLoaiThuMuc where tm.iTrangThai = 1");
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static int Count_DanhSachNganh()
        {
            int vR;
            String SQL = String.Format(@"SELECT COUNT(*)
                                        FROM TL_ThuMucTaiLieu ");
            SqlCommand cmd = new SqlCommand(SQL);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static int XoaThuMuc(string ID)
        {
            return Connection.DeleteRecord("TL_ThuMucTaiLieu", "iID_MaThuMucTaiLieu", ID);
        }

        public static DataTable LayDanhSachThuMuc(string iID_MaLoaiThuMuc)
        {
            SqlCommand cmd;
            if (string.IsNullOrEmpty(iID_MaLoaiThuMuc) || iID_MaLoaiThuMuc.Equals(Guid.Empty.ToString()))
            {
                String SQL = String.Format(@"select  tm.iID_MaThuMucTaiLieu, tm.sTen, tm.dNgayTao, tm.bHoatDong,tm.iID_MaLoaiThuMuc  from TL_ThuMucTaiLieu tm ");
                cmd = new SqlCommand(SQL);
            }
            else
            {
                String SQL = String.Format(@"select  tm.iID_MaThuMucTaiLieu, tm.sTen, tm.dNgayTao, tm.bHoatDong, tm.iID_MaLoaiThuMuc  from TL_ThuMucTaiLieu tm
                            where tm.iID_MaLoaiThuMuc = @iID_MaLoaiThuMuc ");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaLoaiThuMuc", iID_MaLoaiThuMuc);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static void CapNhatThuMucLuu(string iID_MaLoaiThuMuc, int iID_MaThuMucTaiLieu)
        {
            String SQL = String.Format(@"update TL_ThuMucTaiLieu 
                                        set bHoatDong = 'false'
                                        where iID_MaLoaiThuMuc = @iID_MaLoaiThuMuc ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaLoaiThuMuc", iID_MaLoaiThuMuc);
            Connection.UpdateDatabase(cmd);

            SQL = String.Format(@"update TL_ThuMucTaiLieu 
                                        set bHoatDong = 'true'
                                        where iID_MaLoaiThuMuc = @iID_MaLoaiThuMuc  and iID_MaThuMucTaiLieu = @iID_MaThuMucTaiLieu");

            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaLoaiThuMuc", iID_MaLoaiThuMuc);
            cmd.Parameters.AddWithValue("@iID_MaThuMucTaiLieu", iID_MaThuMucTaiLieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

        }

        public static String ThuMucLuu(string sTenKhoa)
        {
            String SQL = String.Format(@"select tl.sTen from DC_DanhMuc dm
                                        join TL_ThuMucTaiLieu tl on tl.iID_MaLoaiThuMuc = dm.iID_MaDanhMuc
                                        where dm.sTenKhoa = @sTenKhoa and tl.bHoatDong = 'true'");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", sTenKhoa);
            DataTable dt = Connection.GetDataTable(cmd);
            string sPath = string.Empty;
            if (dt != null && dt.Rows.Count != 0)
            {
                sPath = Convert.ToString(dt.Rows[0]["sTen"]);
            }
            return sPath;
        }
        public static DataTable GetThuMuc(string ID)
        {
            DataTable dt;
            if (string.IsNullOrEmpty(ID))
            {
                return null;
            }
            try
            {
                string sqlQuery =
                    string.Format(
                        @"select * from TL_ThuMucTaiLieu
                                                where iID_MaThuMucTaiLieu = @iID_MaThuMucTaiLieu");
                SqlCommand cmd = new SqlCommand(sqlQuery);
                cmd.Parameters.AddWithValue("@iID_MaThuMucTaiLieu", ID);
                dt = Connection.GetDataTable(cmd);
            }
            catch (Exception)
            {
                dt = null;
            }
            return dt;
        }
        public static Boolean CheckEditOrDelete(String sMaND, String iID_MaTaiLieu)
        {
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(sMaND);
            if (sTenPB == "02" || sTenPB == "2" || sTenPB == "11")
                return true;
            else
            {
                String SQL = String.Format(@"SELECT COUNT(*) FROM TL_VanBan
WHERE iTrangThai=1 AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao
AND iID_MaTaiLieu=@iID_MaTaiLieu");
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaTaiLieu", iID_MaTaiLieu);
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sMaND);
                int count =Convert.ToInt32(Connection.GetValue(cmd, 0));
                if (count > 0) return true;
                else return false;
            }
        }

    }
}