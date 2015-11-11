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
using System.Web.Mvc;

namespace VIETTEL.Models
{
    public class DonViModels
    {
        public static NameValueCollection LayThongTinDonVi(String iID_MaDonVi)
        {

            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_dtDonVi(iID_MaDonVi);
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
        /// Lấy danh sách trong bảng đon vị
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_dtDonVi()
        {
            String SQL = "SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi ORDER BY iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_DanhSachDonVi()
        {
            String SQL = "SELECT iID_MaDonVi, iID_MaDonVi + '-' +sTen as sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi ORDER BY iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        //lay danh sach don vi theo cap thu
        public static DataTable Get_DanhSachDonVi_CapThu(String iID_MaCapPhat)
        {

            String SQL = "SELECT dv.iID_MaDonVi, dv.iID_MaDonVi + '-' + dv.sTen as sTen FROM NS_DonVi dv WHERE dv.iTrangThai=1 AND dv.iNamLamViec_DonVi=@iNamLamViec_DonVi" +
            " and  EXISTS (SELECT iID_MaChungTuChiTiet FROM KTTG_ChungTuChiTietCapThu c where c.iTrangThai=1  AND c.bDuyet = 0 and dv.iID_MaDonVi=c.iID_MaDonVi and iID_MaCapPhat=@iID_MaCapPhat) ORDER BY dv.iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy thông tin một đơn vị
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static DataTable Get_dtDonVi(String iID_MaDonVi)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND iID_MaDonVi = @iID_MaDonVi");
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy thông tin tên đơn vị
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public static String Get_TenDonVi(String iID_MaDonVi, String MaND)
        {
            String vR = "";
            SqlCommand cmd = new SqlCommand("SELECT sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND iID_MaDonVi = @iID_MaDonVi");
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", ReportModels.LayNamLamViec(MaND));
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
        public static String Get_TenDonVi(String iID_MaDonVi)
        {
            String vR = "";
            SqlCommand cmd = new SqlCommand("SELECT sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND iID_MaDonVi = @iID_MaDonVi");
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }


        /// <summary>
        /// Hiển thị danh sách đơn vị (iID_MaDonVi,sTen,TenHT)
        /// </summary>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi(String MaND)
        {
            String SQL = String.Format(@"SELECT DISTINCT iID_MaDonVi,sTen,iID_MaDonVi+'-'+sTen AS TenHT FROM NS_DONVI WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND iID_MaDonVi<>'' ORDER BY iID_MaDonVi ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách đơn vị nợ có trong chứng từ kế toán tổng hợp
        /// </summary>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi_ChungTu_KeToan(String iID_MaChungTu)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT  DISTINCT    'C-' + iID_MaDonVi AS iID_MaDonVi, 'C - ' +  iID_MaDonVi + ' - ' + sTen as sTen
            FROM NS_DonVi where iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND
            iID_MaDonVi IN (SELECT iID_MaDonVi_Co from KT_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu) 
            UNION
            SELECT    DISTINCT  'N-' + iID_MaDonVi AS iID_MaDonVi, 'N - ' +  iID_MaDonVi + ' - ' + sTen as sTen
            FROM  NS_DonVi where iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND
            iID_MaDonVi IN (SELECT iID_MaDonVi_No from KT_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// Danh sach don vi trong chung tu thu nop
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi_ThuNop(String iID_MaChungTu, String sSoCT)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"select distinct iID_MaDonVi, RTRIM(LTRIM(iID_MaDonVi)) + ' - ' + sTenDonVi as sTen from TN_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu AND iID_MaDonVi<>'' AND sSoCT=@sSoCT ORDER BY iID_MaDonVi");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@sSoCT", sSoCT);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            return dt;
        }
        public static DataTable DanhSach_DonVi_ThuNop_PhongBan(String iID_MaPhongBan, String MaND)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            String DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            String DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            String SQL = String.Format(@"select distinct iID_MaDonVi, RTRIM(LTRIM(iID_MaDonVi)) + ' - ' + sTenDonVi as sTen from TN_ChungTuChiTiet where iTrangThai=1 {0} {1} {2} AND iID_MaDonVi<>'' AND iNamLamViec=@iNamLamViec ORDER BY iID_MaDonVi", DK, DKPhongBan, DKDonVi);

            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow dr = dt.NewRow();
            dr[0] = "-1";
            dr[1] = "--Chọn tất cả đơn vị--";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        public static DataTable DanhSach_DonVi_QuyetToan_PhongBan(String iID_MaPhongBan, String MaND)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            String DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            String DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            String SQL = String.Format(@"select distinct iID_MaDonVi, RTRIM(LTRIM(iID_MaDonVi)) + ' - ' + sTenDonVi as sTen from QTA_ChungTuChiTiet where iTrangThai=1 {0} {1} {2} AND iID_MaDonVi<>'' AND iNamLamViec=@iNamLamViec ORDER BY iID_MaDonVi", DK, DKPhongBan, DKDonVi);

            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow dr = dt.NewRow();
            dr[0] = "-1";
            dr[1] = "--Chọn tất cả đơn vị--";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        public static DataTable DanhSach_DonVi_QuyetToan_PhongBan(String iID_MaPhongBan, String iThang_Quy, String iID_MaNamNganSach, String MaND)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (!String.IsNullOrEmpty(iID_MaNamNganSach) && iID_MaNamNganSach != "0")
            {
                DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            }
            String DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            String DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            String SQL = String.Format(@"select distinct iID_MaDonVi, RTRIM(LTRIM(iID_MaDonVi)) + ' - ' + sTenDonVi as sTen from QTA_ChungTuChiTiet where iTrangThai=1 {0} {1} {2} AND iID_MaDonVi<>''
AND iNamLamViec=@iNamLamViec AND iThang_Quy=@iThang_Quy ORDER BY iID_MaDonVi", DK, DKPhongBan, DKDonVi);

            cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow dr = dt.NewRow();
            dr[0] = "-1";
            dr[1] = "--Chọn tất cả đơn vị--";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }
        /// <summary>
        /// Lấy danh sách đơn vị nợ có trong chứng từ tiền gửi
        /// </summary>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi_ChungTu_KeToan_TienGui(String iID_MaChungTu)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT  DISTINCT    'C-' + iID_MaDonVi AS iID_MaDonVi, 'C - ' +  iID_MaDonVi + ' - ' + sTen as sTen
            FROM NS_DonVi where iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND
            iID_MaDonVi IN (SELECT iID_MaDonVi_Co from KTTG_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu) 
            UNION
            SELECT    DISTINCT  'N-' + iID_MaDonVi AS iID_MaDonVi, 'N - ' +  iID_MaDonVi + ' - ' + sTen as sTen
            FROM  NS_DonVi where iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND
            iID_MaDonVi IN (SELECT iID_MaDonVi_No from KTTG_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// Lấy danh sách đơn vị nợ có trong chứng từ tiền mat
        /// </summary>
        /// <returns></returns>
        public static DataTable DanhSach_DonVi_ChungTu_KeToan_TienMat(String iID_MaChungTu)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT  DISTINCT    'C-' + iID_MaDonVi AS iID_MaDonVi, 'C - ' +  iID_MaDonVi + ' - ' + sTen as sTen
            FROM NS_DonVi where iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND
            iID_MaDonVi IN (SELECT iID_MaDonVi_Co from KTTM_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu) 
            UNION
            SELECT    DISTINCT  'N-' + iID_MaDonVi AS iID_MaDonVi, 'N - ' +  iID_MaDonVi + ' - ' + sTen as sTen
            FROM  NS_DonVi where iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi AND
            iID_MaDonVi IN (SELECT iID_MaDonVi_No from KTTM_ChungTuChiTiet where iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu)");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            return dt;
        }

        /// <summary>
        /// Lấy Mã phòng ban của đơn vị 06,07,10
        /// </summary>
        /// <returns></returns>
        public static String getPhongBanCuaDonVi(String iID_MaDonVi)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT  DISTINCT TOP 1 sKyHieu FROM 

(SELECT * FROM NS_PhongBan_DonVi
WHERE iNamLamViec=@iNamLamViec AND iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi) a
INNER JOIN (
SELECT  * FROM NS_PhongBan WHERE iTrangThai=1 AND sKyHieu IN (06,07,10))b
ON a.iID_MaPhongBan=b.iID_MaPhongBan");
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.CommandText = SQL;
            String iID_MaPhongBan = Connection.GetValueString(cmd, "");
            return iID_MaPhongBan;
        }

        public static String LayXauDanhSachDonVi(string Path, string XauHanhDong, string XauSapXep, String MaDonViCha, int Cap, ref int ThuTu)
        {
            String vR = "";
            String SQL = "";
            if (MaDonViCha != null && MaDonViCha != "")
            {
                SQL = string.Format("SELECT * FROM NS_DonVi WHERE iTrangThai = 1 AND iNamLamViec_DonVi='{1}' AND iID_MaDonViCha= '{0}' ORDER BY iID_MaDonVi", MaDonViCha, NguoiDungCauHinhModels.iNamLamViec);
            }
            else
            {
                SQL = string.Format("SELECT * FROM NS_DonVi WHERE iTrangThai = 1 AND iNamLamViec_DonVi='{1}' AND (iID_MaDonViCha is null OR iID_MaDonViCha='0') ORDER BY iID_MaDonVi", MaDonViCha, NguoiDungCauHinhModels.iNamLamViec);
            }
            DataTable dt = Connection.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;

                string strPG = "", strXauCon, strDoanTrang = "";

                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    String classtr = "";
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String iID_MaKhoiDonVi, iID_MaLoaiDonVi, iID_MaNhomDonVi;
                    iID_MaKhoiDonVi = Convert.ToString(Row["iID_MaKhoiDonVi"]);
                    iID_MaLoaiDonVi = Convert.ToString(Row["iID_MaLoaiDonVi"]);
                    iID_MaNhomDonVi = Convert.ToString(Row["iID_MaNhomDonVi"]);

                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaDonVi"].ToString());
                    strXauCon = LayXauDanhSachDonVi(Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaDonVi"]), Cap + 1, ref ThuTu);

                    if (strXauCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaDonVi"].ToString());
                    }
                    strPG += string.Format("<tr " + classtr + ">");
                    if (Cap == 0)
                    {
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, tgThuTu);
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["iID_MaDonVi"]);
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sTen"]);
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sMoTa"]);
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, CommonFunction.LayTenDanhMuc(iID_MaKhoiDonVi));
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, CommonFunction.LayTenDanhMuc(iID_MaNhomDonVi));
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, CommonFunction.LayTenDanhMuc(iID_MaLoaiDonVi));
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, tgThuTu);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["iID_MaDonVi"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sMoTa"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, CommonFunction.LayTenDanhMuc(iID_MaKhoiDonVi));
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, CommonFunction.LayTenDanhMuc(iID_MaNhomDonVi));
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, CommonFunction.LayTenDanhMuc(iID_MaLoaiDonVi));
                        }
                        else
                        {
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, tgThuTu);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["iID_MaDonVi"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sMoTa"]);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, CommonFunction.LayTenDanhMuc(iID_MaKhoiDonVi));
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, CommonFunction.LayTenDanhMuc(iID_MaNhomDonVi));
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, CommonFunction.LayTenDanhMuc(iID_MaLoaiDonVi));
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
                    strPG += strXauCon;
                }
                vR = String.Format("{0}", strPG);
            }
            dt.Dispose();
            return vR;
        }
    }
}