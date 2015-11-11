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
    public class TaiKhoanModels
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

            string SQL = string.Format(@"SELECT  * FROM  KT_TaiKhoan WHERE 1 = 1");

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
            String DK = " 1=1";
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
            String SQL = String.Format("SELECT COUNT(*) FROM KT_TaiKhoan WHERE {0}", DK);
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
            string XauSapXep,string XauTaiKhoanChiTiet, String MaMucLucQuanSoCha, int Cap, ref int ThuTu, string UserName)
        {
            String vR = "";
            String SQL = "";
            SqlCommand cmd = new SqlCommand();
            if (MaMucLucQuanSoCha != null && MaMucLucQuanSoCha != "")
            {
                SQL = string.Format("SELECT * FROM KT_TaiKhoan WHERE iNam = @NamLamViec AND iID_MaTaiKhoan_Cha = '{0}'", MaMucLucQuanSoCha);
                
            }
            else
            {
                SQL = "SELECT * FROM KT_TaiKhoan WHERE iNam = @NamLamViec AND iID_MaTaiKhoan_Cha = ''";
               
            }
            if (String.IsNullOrEmpty(sTaiKhoan) == false && sTaiKhoan != "")
            {
                //SQL += " AND sTen like N'%' +  @sTen + '%'";
                SQL += " AND sTen= @sTen";
                cmd.Parameters.AddWithValue("@sTen", sTaiKhoan);
            }
            if (String.IsNullOrEmpty(sKyHieu) == false && sKyHieu != "")
            {
                SQL += " AND iID_MaTaiKhoan= @iID_MaTaiKhoan";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", sKyHieu);
            }

            SQL += " AND iTrangThai=1 ORDER BY iID_MaTaiKhoan ASC";           
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@NamLamViec", DanhMucModels.NamLamViec(UserName));
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
                    String sClasstr = "";
                    if (i % 2 == 0) sClasstr = "alt";
                    // int STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaTaiKhoan"].ToString());
                    strXauMucLucQuanSoCon = LayXauTaiKhoanKeToan(sTaiKhoan, sKyHieu, Path, XauHanhDong, XauSapXep,
                                                                 XauTaiKhoanChiTiet,
                                                                 Convert.ToString(Row["iID_MaTaiKhoan"]), Cap + 1,
                                                                 ref ThuTu, UserName);

                    if (strXauMucLucQuanSoCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaTaiKhoan"].ToString());
                    }
                    else
                    {
                        strHanhDong += XauTaiKhoanChiTiet.Replace("%23%23", Row["iID_MaTaiKhoan"].ToString());
                    }
                    strPG += string.Format("<tr class=" + sClasstr + ">");
                    string bHienThi = HamChung.ConvertToString(Row["bHienThi"]);
                    if (Cap == 0)
                    {
                        // strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\"><b>{0}</b></td>", ThuTu);
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["iID_MaTaiKhoan"]);
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sTen"]);
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", Row["sMoTa"]);
                        strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", Row["iCapTaiKhoan"]);
                        if (bHienThi == "True")
                        {
                            strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", "X");
                        }
                        else
                            strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", "");
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {
                            // strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\"><b>{0}</b></td>", ThuTu);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["iID_MaTaiKhoan"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", Row["sMoTa"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", Row["iCapTaiKhoan"]);
                            if (bHienThi == "True")
                            {
                                strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", "X");
                            }
                            else
                                strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", "");
                        }
                        else
                        {
                            // strPG += string.Format("<td style=\"padding: 3px 3px;\"><b>{0}</b></td>", ThuTu);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["iID_MaTaiKhoan"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", Row["sMoTa"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", Row["iCapTaiKhoan"]);
                            if (bHienThi == "True")
                            {
                                strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", "X");
                            }
                            else
                                strPG += string.Format("<td style=\"padding: 3px 3px;text-align: center\">{0}</td>", "");
                        }
                    }
                    if (tgThuTu % 2 == 0)
                    {
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", strHanhDong);
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
        public static DataTable getChiTietTK(String iID_MaTaiKhoan)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KT_TaiKhoan WHERE iTrangThai=1 AND iID_MaTaiKhoan=@iID_MaTaiKhoan");
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
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
        public static Boolean DeleteTaiKhoan(String iID_MaTaiKhoan)
        {
            Boolean vR = false;
            if (CheckTaiKhoan(iID_MaTaiKhoan))
            {
                SqlCommand cmd;
                cmd = new SqlCommand();
                cmd.CommandText = "DELETE KT_TaiKhoan WHERE iID_MaTaiKhoan_Cha=@iID_MaTaiKhoan";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                Bang bang = new Bang("KT_TaiKhoan");
                bang.GiaTriKhoa = iID_MaTaiKhoan;
                bang.Delete();
                vR = true;
            }
            return vR;
        }
        /// <summary>
        /// Kiểm tra tài khoản có nằm trong bảng chứng từ chi tiết không
        /// </summary>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns>True: tồn tại, False: không tồn tại</returns>
        public static Boolean CheckTaiKhoan(String iID_MaTaiKhoan)
        {
            Boolean vR = false;
            DataTable dt;
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT iID_MaChungTuChiTiet FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND (iID_MaTaiKhoan_No=@iID_MaTaiKhoan OR iID_MaTaiKhoan_Co=@iID_MaTaiKhoan)";
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
             dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0) vR = true;
            if (dt != null) dt.Dispose();
            cmd.Dispose();

            cmd = new SqlCommand();
            cmd.CommandText = "SELECT iID_MaChungTuChiTiet FROM KTTG_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND (iID_MaTaiKhoan_No=@iID_MaTaiKhoan OR iID_MaTaiKhoan_Co=@iID_MaTaiKhoan)";
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
             dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0) vR = true;
            if (dt != null) dt.Dispose();
            cmd.Dispose();

            cmd = new SqlCommand();
            cmd.CommandText = "SELECT iID_MaChungTuChiTiet FROM KTTM_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND (iID_MaTaiKhoan_No=@iID_MaTaiKhoan OR iID_MaTaiKhoan_Co=@iID_MaTaiKhoan)";
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0) vR = true;
            if (dt != null) dt.Dispose();
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Lấy cấp của tài khoản theo mã tài khoản
        /// </summary>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static int getCapTK(string iID_MaTaiKhoan)
        {
            string SQL = "SELECT  iCapTaiKhoan FROM KT_TaiKhoan WHERE 1=1";
            SqlCommand cmd = new SqlCommand();
            if (iID_MaTaiKhoan != null || iID_MaTaiKhoan != "")
            {
                SQL += " AND iID_MaTaiKhoan=@iID_MaTaiKhoan";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            }
            cmd.CommandText = SQL;
            int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return SoHangMauCon;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static int UpdateTaiKhoan_LaHangCha(string iID_MaTaiKhoan)
        {
            string SQL = "UPDATE KT_TaiKhoan SET bLaHangCha=1 WHERE iID_MaTaiKhoan=@iID_MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
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

        public static DataTable DT_DSTaiKhoan(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName="")
        {
            DataTable vR = new DataTable();
            String SQL = "SELECT iID_MaTaiKhoan, iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNam";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTaiKhoan"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        public static DataTable DT_DSTaiKhoanCha(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "",Boolean TaiKhoanCha=false)
        {
            DataTable vR = new DataTable();
            String SQL = "SELECT iID_MaTaiKhoan, iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNam AND bLaHangCha=@bLaHangCha";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            cmd.Parameters.AddWithValue("@bLaHangCha", TaiKhoanCha);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTaiKhoan"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        public static DataTable DT_DSTaiKhoanCha_SoDuGiaiThich(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "", Boolean TaiKhoanCha = false)
        {
            DataTable vR = new DataTable();
            String SQL =
                "SELECT DISTINCT iID_MaTaiKhoan, iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNam AND bLaHangCha=@bLaHangCha " +
                "AND exists (select iID_MaChungTuChiTiet from KT_SoDuTaiKhoanGiaiThich ct where ct.iTrangThai=1 AND ct.iNamLamViec=@iNam AND (ct.iID_MaTaiKhoan_No=KT_TaiKhoan.iID_MaTaiKhoan OR ct.iID_MaTaiKhoan_Co=KT_TaiKhoan.iID_MaTaiKhoan))" +
                " ORDER BY iID_MaTaiKhoan";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            cmd.Parameters.AddWithValue("@bLaHangCha", TaiKhoanCha);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTaiKhoan"] = "-1";
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        public static DataTable DT_DSTaiKhoan_Exits_KT_TaiKhoanGiaiThich(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "", Boolean TaiKhoanCha = false)
        {
            DataTable vR = new DataTable();
            String SQL = "SELECT kt.iID_MaTaiKhoan, kt.iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan kt WHERE kt.iTrangThai=1 AND kt.iNam=@iNam AND kt.bLaHangCha=@bLaHangCha AND exists(select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanGiaiThich ct where ct.iTrangThai=1 and ct.iID_MaTaiKhoan=kt.iID_MaTaiKhoan)";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            cmd.Parameters.AddWithValue("@bLaHangCha", TaiKhoanCha);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTaiKhoan"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ThemDongTieuDe"></param>
        /// <param name="sDongTieuDe"></param>
        /// <param name="UserName"></param>
        /// <param name="MaTaiKhoanCha"></param>
        /// <returns></returns>
        public static DataTable DT_DSTaiKhoan_TheoTaiKhoanCha(String UserName = "", String MaTaiKhoanCha = "")
        {
            DataTable vR = new DataTable();
            String SQL = "SELECT iID_MaTaiKhoan, iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNam";
            if (String.IsNullOrEmpty(MaTaiKhoanCha) == false && MaTaiKhoanCha != "")
            {
                SQL += " AND iID_MaTaiKhoan_Cha=@iID_MaTaiKhoan";
               
            }
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            if (String.IsNullOrEmpty(MaTaiKhoanCha) == false && MaTaiKhoanCha!="")
            { 
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", MaTaiKhoanCha);
            }
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (vR != null || vR.Rows.Count == 0)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTaiKhoan"] = "-1";
                R["sTen"] = "Không có tài khoản con";
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        public static DataTable DT_DSTaiKhoan_NgoaiBang(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "")
        {
            DataTable vR = new DataTable();
            String SQL = "SELECT iID_MaTaiKhoan, iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNam AND substring(iID_MaTaiKhoan,1,1)=0";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTaiKhoan"] = Guid.Empty;
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
        public static DataTable DT_DSTaiKhoan_PhuongAn(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", String UserName = "", String KyHieu="")
        {
            //Lấy ra tham số của phương án
            string mySQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE (sKyHieu = @KyHieu)";
            SqlCommand cmdSQL = new SqlCommand();
            cmdSQL.Parameters.AddWithValue("@KyHieu", KyHieu);
            cmdSQL.CommandText = mySQL;
            String iID_MaTaiKhoan = Convert.ToString(Connection.GetValue(cmdSQL, 0));
            cmdSQL.Dispose();
            //tài khoản
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //trả về danh sách tài khoản theo phương án
            DataTable vR = new DataTable();
            String SQL = "SELECT iID_MaTaiKhoan, iID_MaTaiKhoan + '-' +sTen as sTen FROM KT_TaiKhoan WHERE (iTrangThai=1) AND (iNam=@iNam)";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            if (arrTaiKhoan.Length > 0)
            {
                for (int i = 0; i < arrTaiKhoan.Length; i++)
                {
                    if (i == 0)
                        SQL += " AND (iID_MaTaiKhoan=@iID_MaTaiKhoan" + i;
                    else
                        SQL += " OR iID_MaTaiKhoan=@iID_MaTaiKhoan" + i;
                  
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrTaiKhoan[i]);
                }
                SQL += ")";
            }
            SQL += " ORDER BY iID_MaTaiKhoan ASC";
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTaiKhoan"] = Guid.Empty;
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        public static DataTable DT_DSTaiKhoanGiaiThich_PhuongAn(String UserName, String KyHieu)
        {
            //Lấy ra tham số của phương án
            string mySQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE (sKyHieu = @KyHieu) AND iNamLamViec=@iNamLamViec";
            SqlCommand cmdSQL = new SqlCommand();
            cmdSQL.Parameters.AddWithValue("@KyHieu", KyHieu);
            cmdSQL.Parameters.AddWithValue("@iNamLamViec", DanhMucModels.NamLamViec(UserName));
            cmdSQL.CommandText = mySQL;
            String iID_MaTaiKhoan = Convert.ToString(Connection.GetValue(cmdSQL, 0));
            cmdSQL.Dispose();
            //tài khoản
            String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');
            //trả về danh sách tài khoản theo phương án
            DataTable vR = new DataTable();
            String SQL = "SELECT sKyHieu, sKyHieu + '-' +sTen as sTen FROM KT_TaiKhoanDanhMucChiTiet WHERE (iTrangThai=1) ";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(UserName));
            if (arrTaiKhoan.Length > 0)
            {
                for (int i = 0; i < arrTaiKhoan.Length; i++)
                {
                    if (i == 0)
                        SQL += " AND (sKyHieu=@sKyHieu" + i;
                    else
                        SQL += " OR sKyHieu=@sKyHieu" + i;

                    cmd.Parameters.AddWithValue("@sKyHieu" + i, arrTaiKhoan[i]);
                }
                SQL += ")";
            }
            SQL += " ORDER BY sKyHieu ASC";
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// lấy tên TK theo mã TK
        /// </summary>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static string getTenTK(string iID_MaTaiKhoan)
        {
            string SQL = "SELECT iID_MaTaiKhoan + ' - ' + sTen AS TenTK FROM KT_TaiKhoan WHERE 1=1 AND iID_MaTaiKhoan=@iID_MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.CommandText = SQL;
            String SoHangMauCon = Convert.ToString(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return SoHangMauCon;
        }

        /// <summary>
        /// Lấy tên tài khoản không ghép mã
        /// </summary>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static string LayTenTaiKhoanKhongGhepMa(string iID_MaTaiKhoan)
        {
            String vR = "";
            string SQL = "SELECT sTen FROM KT_TaiKhoan WHERE iTrangThai=1 AND iID_MaTaiKhoan=@iID_MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public static string LayTenTaiKhoanKhongGhepMa(string iID_MaTaiKhoan,int iNam)
        {
            String vR = "";
            string SQL = "SELECT sTen FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam=@iNam AND iID_MaTaiKhoan=@iID_MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.Parameters.AddWithValue("@iNam", iNam);            
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tài khoản cấp cha
        /// </summary>
        /// <param name="iID_MaTaiKhoan"></param>
        /// <returns></returns>
        public static string LayTaiKhoanCha(string iID_MaTaiKhoan)
        {
            String vR = "";
            string SQL = "SELECT iID_MaTaiKhoan_Cha FROM KT_TaiKhoan WHERE iID_MaTaiKhoan=@iID_MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public static string LayTenTaiKhoan(String iID_MaTaiKhoan)
        {
            String vR = "";
            string SQL = "SELECT sTen FROM KT_TaiKhoan WHERE iID_MaTaiKhoan=@iID_MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            cmd.CommandText = SQL;
            vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lay danh sach tai khoan theo BQL, DV (03/10/2013 - phuonglt15)
        /// </summary>
        /// <param name="iID_MaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable DSTaiKhoan_BQL_DVi(string iID_MaPhongBan, string iID_MaDonVi, string iID_MaTrangThai, string iThang, Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---", string UserName="")
        {
            DataTable vR = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            if (iID_MaTrangThai=="0")
            {
                SQL =String.Format(@"select iID_MaTaiKhoan_No as iID_MaTaiKhoan,sTenTaiKhoan_No as sTenTaiKhoan  from
(SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,iID_MaPhongBan_No
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
            AND sTenTaiKhoan_No <>''
            AND sTenTaiKhoan_No IS NOT NULL
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL
             -- AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
           --  AND iThangCT=@iThangCT
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,iID_MaPhongBan_No
       
       UNION 
       
        SELECT iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_Co
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
 AND sTenTaiKhoan_Co <>''
            AND sTenTaiKhoan_Co IS NOT NULL
			 AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL
           --  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
           --  AND iThangCT=@iThangCT
       GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_Co
       
       Union
       
        SELECT iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_Co
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
             AND sTenTaiKhoan_Co <>''
            AND sTenTaiKhoan_Co IS NOT NULL
			 AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL
               --AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             --AND iThangCT<@iThangCT
       GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_Co
             
       union
       
       SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,iID_MaPhongBan_No
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
             AND sTenTaiKhoan_No <>''
            AND sTenTaiKhoan_No IS NOT NULL
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL
            --  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
            -- AND iThangCT<@iThangCT
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,iID_MaPhongBan_No) a 
--where iID_MaDonVi_No IN ({0})  and     iID_MaPhongBan_No=(select top 1 sKyHieu from NS_PhongBan  where iID_MaPhongBan =@iID_MaPhongBan) 
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No ORDER BY iID_MaTaiKhoan_No", iID_MaDonVi);  
            }
            else
            {
                SQL =String.Format(@"select iID_MaTaiKhoan_No as iID_MaTaiKhoan,sTenTaiKhoan_No as sTenTaiKhoan  from
(SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,iID_MaPhongBan_No
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
            AND sTenTaiKhoan_No <>''
            AND sTenTaiKhoan_No IS NOT NULL
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL             
             AND iNamLamViec=@iNamLamViec
             AND iThangCT=@iThangCT
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,iID_MaPhongBan_No
       
       UNION 
       
        SELECT iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_Co
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
 AND sTenTaiKhoan_Co <>''
            AND sTenTaiKhoan_Co IS NOT NULL
			 AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL           
             AND iNamLamViec=@iNamLamViec
             AND iThangCT=@iThangCT
       GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_Co
       
       Union
       
        SELECT iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_Co
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1 
               AND sTenTaiKhoan_Co <>''
            AND sTenTaiKhoan_Co IS NOT NULL 
			 AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL             
             AND iNamLamViec=@iNamLamViec
            -- AND iThangCT<@iThangCT
       GROUP BY iID_MaTaiKhoan_Co,sTenTaiKhoan_Co,iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_Co
             
       union
       
       SELECT iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,iID_MaPhongBan_No
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
             AND sTenTaiKhoan_No <>''
            AND sTenTaiKhoan_No IS NOT NULL
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL       
             AND iNamLamViec=@iNamLamViec
           --  AND iThangCT<@iThangCT
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No,iID_MaDonVi_No,sTenDonVi_No,iID_MaPhongBan_No) a
--where iID_MaDonVi_No IN ({0})  and  iID_MaPhongBan_No=(select top 1 sKyHieu from NS_PhongBan  where iID_MaPhongBan =@iID_MaPhongBan) 
       GROUP BY iID_MaTaiKhoan_No,sTenTaiKhoan_No ORDER BY iID_MaTaiKhoan_No", iID_MaDonVi);   
            }
            cmd.CommandText = SQL;
            if (iID_MaTrangThai == "0")
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet",
                                            LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(
                                                PhanHeModels.iID_MaPhanHeKeToanTongHop));
            cmd.Parameters.AddWithValue("@iNamLamViec", DanhMucModels.NamLamViec(UserName));
            cmd.Parameters.AddWithValue("@iThangCT", iThang);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTaiKhoan"] = Guid.Empty;
                R["sTenTaiKhoan"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }

            return vR; 
        }
    }

}