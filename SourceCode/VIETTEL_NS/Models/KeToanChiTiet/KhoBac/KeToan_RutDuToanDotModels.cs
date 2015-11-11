using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;

namespace VIETTEL.Models
{
    public class KeToan_RutDuToanDotModels
    {
        /// <summary>
        /// Lấy thông tin của một chứng từ thu nộp ngân sách
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTin(String iID_MaDotRutDuToan)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietDotRutDuToan(iID_MaDotRutDuToan);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        public static DataTable Get_dtDotRutDuToan(String iID_MaDotRutDuToan)
        {
            String SQL = "SELECT * FROM KT_RutDuToanDot WHERE iID_MaDotRutDuToan=@iID_MaDotRutDuToan";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDotRutDuToan", iID_MaDotRutDuToan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }

        public static DataTable Get_ChiTietDotRutDuToan(String iID_MaDotRutDuToan)
        {
            String SQL = "SELECT * FROM KT_RutDuToanChiTiet WHERE iTrangThai=1 AND iID_MaDotRutDuToan=@iID_MaDotRutDuToan ORDER BY sLNS,sL,sK,sM,sTM";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDotRutDuToan", iID_MaDotRutDuToan);
            DataTable dt = Connection.GetDataTable(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToString(dt.Rows[i]["sTM"]) != "")
                {
                    dt.Rows[i]["bLaHangCha"] = false;
                }
            }
                cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtChiTieu(String iNamLamViec, String iID_MaNamNganSach, String iID_MaNguonNganSach, String dNgayDotPhanBo)
        {
            String SQL = "SELECT sLNS,sL,sK,sM,sTM,sMoTa,SUM(rChiTaiKhoBac) as rSoTien";
            SQL += " FROM PB_ChiTieuChiTiet ";
            SQL += " WHERE iID_MaDotPhanBo IN (SELECT iID_MaDotPhanBo FROM PB_DotPhanBo";
            SQL += " WHERE iNamLamViec=@iNamLamViec ";
            SQL += " AND iID_MaNamNganSach=@iID_MaNamNganSach ";
            SQL += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            SQL += " AND dNgayDotPhanBo=@dNgayDotPhanBo";
            SQL += " )";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sMoTa";
            SQL += " HAVING SUM(rChiTaiKhoBac)>0 ";
            SQL += " ORDER BY sLNS,sL,sK,sM,sTM";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@dNgayDotPhanBo", dNgayDotPhanBo);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Get_dtMucLucNganSach()
        {
            String SQL = "SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha, sLNS,sL,SK,sM,sTM,sMoTa,sChuong,bLaHangCha,rSoTien=0";
            SQL +=" FROM NS_MucLucNganSach";
            SQL += " WHERE iTrangThai=1  AND (sTM<>'' AND sTTM='') OR(sTTM='00' AND sNG='00') OR bLaHangCha=1";
            SQL +=" ORDER BY sLNS,sL,sK,sM,sTM";
            return Connection.GetDataTable(SQL);
        }
        public static void Save(String iID_MaDotRutDuToan,String IP,String User)
        {
            String iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach, dNgayDotRutDuToan;
            DataTable dtDotRutDuToan = Get_dtDotRutDuToan(iID_MaDotRutDuToan);
            iNamLamViec = Convert.ToString(dtDotRutDuToan.Rows[0]["iNamLamViec"]);
            iID_MaNamNganSach = Convert.ToString(dtDotRutDuToan.Rows[0]["iID_MaNamNganSach"]);
            iID_MaNguonNganSach = Convert.ToString(dtDotRutDuToan.Rows[0]["iID_MaNguonNganSach"]);
            dNgayDotRutDuToan = Convert.ToString(dtDotRutDuToan.Rows[0]["dNgayDotRutDuToan"]);

            DataTable dtChiTieu = Get_dtChiTieu(iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach, dNgayDotRutDuToan);

            DataTable dtMucLucNganSach = Get_dtMucLucNganSach();            
            String sXauNoiMa,sXauNoiMa1;
            if (dtChiTieu.Rows.Count > 0)
            {
                for (int i = 0; i < dtChiTieu.Rows.Count; i++)
                {
                    sXauNoiMa = Convert.ToString(dtChiTieu.Rows[i]["sLNS"]).Trim();
                    if (String.IsNullOrEmpty(Convert.ToString(dtChiTieu.Rows[i]["sL"])) == false) sXauNoiMa += "-" + Convert.ToString(dtChiTieu.Rows[i]["sL"]).Trim();
                    if (String.IsNullOrEmpty(Convert.ToString(dtChiTieu.Rows[i]["sK"])) == false) sXauNoiMa += "-" + Convert.ToString(dtChiTieu.Rows[i]["sK"]).Trim();
                    if (String.IsNullOrEmpty(Convert.ToString(dtChiTieu.Rows[i]["sM"])) == false) sXauNoiMa += "-" + Convert.ToString(dtChiTieu.Rows[i]["sM"]).Trim();
                    if (String.IsNullOrEmpty(Convert.ToString(dtChiTieu.Rows[i]["sTM"])) == false) sXauNoiMa += "-" + Convert.ToString(dtChiTieu.Rows[i]["sTM"]).Trim();

                    for (int j = i; j < dtMucLucNganSach.Rows.Count; j++)
                    {
                        sXauNoiMa1 = Convert.ToString(dtMucLucNganSach.Rows[j]["sLNS"]).Trim();
                        if (String.IsNullOrEmpty(Convert.ToString(dtMucLucNganSach.Rows[j]["sL"])) == false) sXauNoiMa1 += "-" + Convert.ToString(dtMucLucNganSach.Rows[j]["sL"]).Trim();
                        if (String.IsNullOrEmpty(Convert.ToString(dtMucLucNganSach.Rows[j]["sK"])) == false) sXauNoiMa1 += "-" + Convert.ToString(dtMucLucNganSach.Rows[j]["sK"]).Trim();
                        if (String.IsNullOrEmpty(Convert.ToString(dtMucLucNganSach.Rows[j]["sM"])) == false) sXauNoiMa1 += "-" + Convert.ToString(dtMucLucNganSach.Rows[j]["sM"]).Trim();
                        if (String.IsNullOrEmpty(Convert.ToString(dtMucLucNganSach.Rows[j]["sTM"])) == false) sXauNoiMa1 += "-" + Convert.ToString(dtMucLucNganSach.Rows[j]["sTM"]).Trim();
                        if (sXauNoiMa == sXauNoiMa1)
                        {
                            dtMucLucNganSach.Rows[j]["rSoTien"] = dtChiTieu.Rows[i]["rSoTien"];
                            break;
                        }
                    }
                }
            }
            Bang bang = new Bang("KT_RutDuToanChiTiet");
            bang.MaNguoiDungSua = User;
            bang.IPSua = IP;
            for (int j = 0; j < dtMucLucNganSach.Rows.Count; j++)
            {
                sXauNoiMa1 = Convert.ToString(dtMucLucNganSach.Rows[j]["sLNS"]).Trim();
                if (String.IsNullOrEmpty(Convert.ToString(dtMucLucNganSach.Rows[j]["sL"])) == false) sXauNoiMa1 += "-" + Convert.ToString(dtMucLucNganSach.Rows[j]["sL"]).Trim();
                if (String.IsNullOrEmpty(Convert.ToString(dtMucLucNganSach.Rows[j]["sK"])) == false) sXauNoiMa1 += "-" + Convert.ToString(dtMucLucNganSach.Rows[j]["sK"]).Trim();
                if (String.IsNullOrEmpty(Convert.ToString(dtMucLucNganSach.Rows[j]["sM"])) == false) sXauNoiMa1 += "-" + Convert.ToString(dtMucLucNganSach.Rows[j]["sM"]).Trim();
                if (String.IsNullOrEmpty(Convert.ToString(dtMucLucNganSach.Rows[j]["sTM"])) == false) sXauNoiMa1 += "-" + Convert.ToString(dtMucLucNganSach.Rows[j]["sTM"]).Trim();
                if (j == 0)
                {
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                    bang.CmdParams.Parameters.AddWithValue("@dNgayDotRutDuToan", dNgayDotRutDuToan);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDotRutDuToan", iID_MaDotRutDuToan);

                    bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach", dtMucLucNganSach.Rows[j]["iID_MaMucLucNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", dtMucLucNganSach.Rows[j]["iID_MaMucLucNganSach_Cha"]);
                    bang.CmdParams.Parameters.AddWithValue("@sLNS", dtMucLucNganSach.Rows[j]["sLNS"]);
                    bang.CmdParams.Parameters.AddWithValue("@sL", dtMucLucNganSach.Rows[j]["sL"]);
                    bang.CmdParams.Parameters.AddWithValue("@sK", dtMucLucNganSach.Rows[j]["sK"]);
                    bang.CmdParams.Parameters.AddWithValue("@sM", dtMucLucNganSach.Rows[j]["sM"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTM", dtMucLucNganSach.Rows[j]["sTM"]);
                    bang.CmdParams.Parameters.AddWithValue("@sMoTa", dtMucLucNganSach.Rows[j]["sMoTa"]);
                    bang.CmdParams.Parameters.AddWithValue("@sChuong", dtMucLucNganSach.Rows[j]["sChuong"]);
                    bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", dtMucLucNganSach.Rows[j]["bLaHangCha"]);
                    bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa1);
                    bang.CmdParams.Parameters.AddWithValue("@rSoTien", dtMucLucNganSach.Rows[j]["rSoTien"]);
                }
                else
                {

                    bang.CmdParams.Parameters["@" + bang.TruongKhoa].Value = Guid.NewGuid();
                    bang.CmdParams.Parameters["@iID_MaMucLucNganSach"].Value= dtMucLucNganSach.Rows[j]["iID_MaMucLucNganSach"];
                    bang.CmdParams.Parameters["@iID_MaMucLucNganSach_Cha"].Value= dtMucLucNganSach.Rows[j]["iID_MaMucLucNganSach_Cha"];
                    bang.CmdParams.Parameters["@sLNS"].Value= dtMucLucNganSach.Rows[j]["sLNS"];
                    bang.CmdParams.Parameters["@sL"].Value=dtMucLucNganSach.Rows[j]["sL"];
                    bang.CmdParams.Parameters["@sK"].Value=dtMucLucNganSach.Rows[j]["sK"];
                    bang.CmdParams.Parameters["@sM"].Value=dtMucLucNganSach.Rows[j]["sM"];
                    bang.CmdParams.Parameters["@sTM"].Value= dtMucLucNganSach.Rows[j]["sTM"];
                    bang.CmdParams.Parameters["@sMoTa"].Value= dtMucLucNganSach.Rows[j]["sMoTa"];
                    bang.CmdParams.Parameters["@sChuong"].Value = dtMucLucNganSach.Rows[j]["sChuong"];
                    bang.CmdParams.Parameters["@bLaHangCha"].Value = dtMucLucNganSach.Rows[j]["bLaHangCha"];
                    bang.CmdParams.Parameters["@sXauNoiMa"].Value= sXauNoiMa1;
                    bang.CmdParams.Parameters["@rSoTien"].Value= dtMucLucNganSach.Rows[j]["rSoTien"];
                }
                bang.Save();
            }

        }
        public static DataTable GetDotRutDuToan(String iNamLamViec="", String iID_MaNamNganSach = "", String iID_MaNguonNganSach = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1";
            
            if (String.IsNullOrEmpty(iNamLamViec) == false)
            {
                DK += " AND iNamLamViec=@iNamLamViec";
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            }
            
            if (String.IsNullOrEmpty(iID_MaNamNganSach) == false)
            {
                DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            }
            
            if (String.IsNullOrEmpty(iID_MaNguonNganSach) == false)
            {
                DK += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            }

            String SQL = String.Format("SELECT * FROM KT_RutDuToanDot WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayDotRutDuToan DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int GetDotRutDuToan_Count(String iNamLamViec="", String iID_MaNamNganSach = "", String iID_MaNguonNganSach = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
             String DK = "iTrangThai=1";
            
            if (String.IsNullOrEmpty(iNamLamViec) == false)
            {
                DK += " AND iNamLamViec=@iNamLamViec";
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            }
            
            if (String.IsNullOrEmpty(iID_MaNamNganSach) == false)
            {
                DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            }
            
            if (String.IsNullOrEmpty(iID_MaNguonNganSach) == false)
            {
                DK += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            }

            String SQL = String.Format("SELECT COUNT(*) FROM KT_RutDuToanDot WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

    }
}