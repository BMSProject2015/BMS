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
    public class KTCS_NhomTaiSanModels
    {
        /// <summary>
        /// lấy ra danh sách tài khoản kế toán
        /// </summary>
        /// <param name="sTaiKhoan">Tên tài khoản</param>
        /// <param name="sKyHieu">Ký hiệu tài khoản</param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable getList(String sTaiKhoan = "", String sKyHieu = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();

            string SQL = string.Format(@"SELECT  * FROM  KTCS_NhomTaiSan WHERE iTrangThai = 1");

            if (String.IsNullOrEmpty(sTaiKhoan) == false && sTaiKhoan != "")
            {
                SQL += " AND sTen= @sTen";
                cmd.Parameters.AddWithValue("@sTen", sTaiKhoan);
            }
            if (String.IsNullOrEmpty(sKyHieu) == false && sKyHieu != "")
            {
                SQL += " AND sKyHieu= @sKyHieu";
                cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            }

            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Đếm số bản ghi của tài khoản kế toán
        /// </summary>
        /// <param name="sTaiKhoan">Tên tài khoản</param>
        /// <param name="sKyHieu">Ký hiệu tài khoản</param>
        /// <returns></returns>
        public static int getList_Count(String sTaiKhoan = "", String sKyHieu = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = " iTrangThai=1";
            if (String.IsNullOrEmpty(sTaiKhoan) == false && sTaiKhoan != "")
            {
                DK += " AND sTen= @sTen";
                cmd.Parameters.AddWithValue("@sTen", sTaiKhoan);
            }
            if (String.IsNullOrEmpty(sKyHieu) == false && sKyHieu != "")
            {
                DK += " AND sKyHieu= @sKyHieu";
                cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            }
            String SQL = String.Format("SELECT COUNT(*) FROM KTCS_NhomTaiSan WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

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
        public static String LayXauTaiKhoanKeToan(string sTaiKhoan, string sKyHieu, string Path, string XauHanhDong,
            string XauSapXep, String MaMucLucQuanSoCha, int Cap, ref int ThuTu, string UserName)
        {
            String vR = "";
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            if (MaMucLucQuanSoCha != null && MaMucLucQuanSoCha != "")
            {
                SQL = string.Format("SELECT * FROM KTCS_NhomTaiSan WHERE iID_MaNhomTaiSan_Cha = '{0}'", MaMucLucQuanSoCha);

            }
            else
            {
                SQL = "SELECT * FROM KTCS_NhomTaiSan WHERE iID_MaNhomTaiSan_Cha = ''";

            }
            if (String.IsNullOrEmpty(sTaiKhoan) == false && sTaiKhoan != "")
            {
                //SQL += " AND sTen like N'%' +  @sTen + '%'";
                SQL += " AND sTen= @sTen";
                cmd.Parameters.AddWithValue("@sTen", sTaiKhoan);
            }
            if (String.IsNullOrEmpty(sKyHieu) == false && sKyHieu != "")
            {
                SQL += " AND iID_MaNhomTaiSan= @iID_MaNhomTaiSan";
                cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", sKyHieu);
            }

            SQL += " AND iTrangThai=1 ORDER BY iSTT ASC";
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
                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaNhomTaiSan"].ToString());
                    strXauMucLucQuanSoCon = LayXauTaiKhoanKeToan(sTaiKhoan, sKyHieu, Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaNhomTaiSan"]), Cap + 1, ref ThuTu, UserName);

                    if (strXauMucLucQuanSoCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaNhomTaiSan"].ToString());
                    }
                    strPG += string.Format("<tr>");
                    if (Cap == 0)
                    {
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;text-align: center;\"><b>{0}</b></td>", Row["iID_MaLoaiTaiSan"]);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["iID_MaNhomTaiSan"]);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sTen"]);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}</td>", Row["sMoTa"]);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;text-align: center\">{0}</td>", Row["rSoNamKhauHao"]);
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;text-align: center;\"><b>{0}</b></td>", Row["iID_MaLoaiTaiSan"]);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["iID_MaNhomTaiSan"]);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}</td>", Row["sMoTa"]);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;text-align: center\">{0}</td>", Row["rSoNamKhauHao"]);
                        }
                        else
                        {
                            strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center;\"><b>{0}</b></td>", Row["iID_MaLoaiTaiSan"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["iID_MaNhomTaiSan"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", Row["sMoTa"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", Row["rSoNamKhauHao"]);
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
        /// <summary>
        /// Lấy thông tin một bản ghi trong tài khoản
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo"></param>
        /// <returns></returns>
        public static DataTable getChiTietTK(String iID_MaNhomTaiSan)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTCS_NhomTaiSan WHERE iTrangThai=1 AND iID_MaNhomTaiSan=@iID_MaNhomTaiSan");
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// loại tài khoản
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_LoaiTK()
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("ID", typeof(int));
            vR.Columns.Add("sTen", typeof(String));
            DataRow Row;

            Row = vR.NewRow();
            Row[0] = 0;
            Row[1] = "Dư nợ";
            vR.Rows.Add(Row);


            Row = vR.NewRow();
            Row[0] = 0;
            Row[1] = "Dư có";
            vR.Rows.Add(Row);


            Row = vR.NewRow();
            Row[0] = 0;
            Row[1] = "Lưỡng tính";
            vR.Rows.InsertAt(Row, 0);
            return vR;
        }
        /// <summary>
        /// Cấp độ tài khoản
        /// </summary>
        /// <returns></returns>
        public static DataTable DT_CapDoTK(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable vR = new DataTable();
            vR.Columns.Add("iID_MaDanhMuc", typeof(int));
            vR.Columns.Add("sTen", typeof(String));
            DataRow Row;
            for (int i = 1; i <= 4; i++)
            {
                Row = vR.NewRow();
                Row[0] = i;
                Row[1] = "Cấp " + Convert.ToString(i);
                vR.Rows.Add(Row);
            }
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaDanhMuc"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        /// <summary>
        /// Xóa một tài khoản
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo"></param>
        /// <returns></returns>
        public static Boolean DeleteTaiKhoan(String iID_MaNhomTaiSan)
        {
            Boolean vR = false;
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "DELETE KTCS_NhomTaiSan WHERE iID_MaNhomTaiSan_Cha=@iID_MaNhomTaiSan";
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            cmd = new SqlCommand();
            cmd.CommandText = "DELETE KTCS_NhomTaiSan WHERE iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            vR = true;
            return vR;
        }
        /// <summary>
        /// Kiểm tra tài khoản có nằm trong bảng chứng từ chi tiết không
        /// </summary>
        /// <param name="iID_MaNhomTaiSan"></param>
        /// <returns>True: tồn tại, False: không tồn tại</returns>
        public static Boolean CheckTaiKhoan(String iID_MaNhomTaiSan)
        {
            Boolean vR = false;
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT iID_MaBangTaiSan FROM KTCS_TaiSan WHERE iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            var dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0) vR = true;
            if (dt != null) dt.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy cấp của tài khoản theo mã tài khoản
        /// </summary>
        /// <param name="iID_MaNhomTaiSan"></param>
        /// <returns></returns>
        public static int getCapTK(string iID_MaNhomTaiSan)
        {
            string SQL = "SELECT  rSoNamKhauHao FROM KTCS_NhomTaiSan WHERE 1=1";
            SqlCommand cmd = new SqlCommand();
            if (iID_MaNhomTaiSan != null || iID_MaNhomTaiSan != "")
            {
                SQL += " AND iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
                cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            }
            cmd.CommandText = SQL;
            int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return SoHangMauCon;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iID_MaNhomTaiSan"></param>
        /// <returns></returns>
        public static int UpdateTaiKhoan_LaHangCha(string iID_MaNhomTaiSan)
        {
            string SQL = "UPDATE KTCS_NhomTaiSan SET bLaHangCha=1 WHERE iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            cmd.CommandText = SQL;
            int vR = Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy ra danh sách tài khoản kế toán
        /// </summary>
        /// <param name="ThemDongTieuDe"></param>
        /// <param name="sDongTieuDe"></param>
        /// <returns></returns>

        public static DataTable DT_DSTaiKhoan(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "")
        {
            DataTable vR = new DataTable();
            String SQL = "SELECT iID_MaNhomTaiSan, iID_MaNhomTaiSan + '-' +sTen as sTen FROM KTCS_NhomTaiSan WHERE iTrangThai=1 AND iNam=@iNam";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaNhomTaiSan"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        public static DataTable DT_DSTaiKhoan_NgoaiBang(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "")
        {
            DataTable vR = new DataTable();
            String SQL = "SELECT iID_MaNhomTaiSan, iID_MaNhomTaiSan + '-' +sTen as sTen FROM KTCS_NhomTaiSan WHERE iTrangThai=1 AND iNam=@iNam AND substring(iID_MaNhomTaiSan,1,1)=0";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaNhomTaiSan"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        /// <summary>
        /// Danh sách tài khoản theo phương án
        /// </summary>
        /// <param name="ThemDongTieuDe"></param>
        /// <param name="sDongTieuDe"></param>
        /// <param name="UserName"></param>
        /// <param name="KyHieu"></param>
        /// <returns></returns>
        public static DataTable DT_DSTaiKhoan_PhuongAn(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "", String KyHieu = "")
        {
            //Lấy ra tham số của phương án
            string mySQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE (sKyHieu = @KyHieu)";
            SqlCommand cmdSQL = new SqlCommand();
            cmdSQL.Parameters.AddWithValue("@KyHieu", KyHieu);
            cmdSQL.CommandText = mySQL;
            String iID_MaNhomTaiSan = Convert.ToString(Connection.GetValue(cmdSQL, 0));
            cmdSQL.Dispose();
            //tài khoản
            String[] arrTaiKhoan = iID_MaNhomTaiSan.Split(',');
            //trả về danh sách tài khoản theo phương án
            DataTable vR = new DataTable();
            String SQL = "SELECT iID_MaNhomTaiSan, iID_MaNhomTaiSan + '-' +sTen as sTen FROM KTCS_NhomTaiSan WHERE (iTrangThai=1) AND (iNam=@iNam)";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            if (arrTaiKhoan.Length > 0)
            {
                for (int i = 0; i < arrTaiKhoan.Length; i++)
                {
                    if (i == 0)
                        SQL += " AND (iID_MaNhomTaiSan=@iID_MaNhomTaiSan" + i;
                    else
                        SQL += " OR iID_MaNhomTaiSan=@iID_MaNhomTaiSan" + i;

                    cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan" + i, arrTaiKhoan[i]);
                }
                SQL += ")";
            }
            SQL += " ORDER BY iID_MaNhomTaiSan ASC";
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaNhomTaiSan"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        /// <summary>
        /// lấy tên TK theo mã TK
        /// </summary>
        /// <param name="iID_MaNhomTaiSan"></param>
        /// <returns></returns>
        public static string getTenTK(string iID_MaNhomTaiSan)
        {
            string SQL = "SELECT iID_MaNhomTaiSan + ' - ' + sTen AS TenTK FROM KTCS_NhomTaiSan WHERE 1=1 AND iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            cmd.CommandText = SQL;
            String SoHangMauCon = Convert.ToString(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return SoHangMauCon;
        }
        /// <summary>
        /// Lấy tên tài khoản không ghép mã
        /// </summary>
        /// <param name="iID_MaNhomTaiSan"></param>
        /// <returns></returns>
        public static string LayTenTaiKhoanKhongGhepMa(string iID_MaNhomTaiSan)
        {
            String vR = "";
            string SQL = "SELECT sTen FROM KTCS_NhomTaiSan WHERE iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tài khoản cấp cha
        /// </summary>
        /// <param name="iID_MaNhomTaiSan"></param>
        /// <returns></returns>
        public static string LayTaiKhoanCha(string iID_MaNhomTaiSan)
        {
            String vR = "";
            string SQL = "SELECT iID_MaNhomTaiSan_Cha FROM KTCS_NhomTaiSan WHERE iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public static string LayTenTaiKhoan(String iID_MaNhomTaiSan)
        {
            String vR = "";
            string SQL = "SELECT sTen FROM KTCS_NhomTaiSan WHERE iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", iID_MaNhomTaiSan);
            cmd.CommandText = SQL;
            vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }
    }
}