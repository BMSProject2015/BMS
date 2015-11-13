using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.Sql;
using System.Data.SqlClient;

namespace VIETTEL.Models
{
    public class ThuNopModels
    {
        public static int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeThuNopNganSach;

        public static DataTable getThongTinCotBaoCao(String iLoai)
        {
            DataTable dt;
            String SQL = "SELECT * FROM TN_CauHinhBaoCao WHERE iID_MaBaoCao=@iLoai";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable getThongTinBaoCao(String iLoai, String iID_MaCot)
        {
            DataTable dt;
            String SQL = "SELECT * FROM TN_CauHinhBaoCao WHERE iID_MaBaoCao=@iLoai AND iID_MaCot=@iID_MaCot";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            cmd.Parameters.AddWithValue("@iID_MaCot", iID_MaCot);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static String DKDonVi(String MaND, SqlCommand cmd)
        {
            String DKDonVi = "";
            DataTable dtNĐonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            for (int i = 0; i < dtNĐonVi.Rows.Count; i++)
            {
                DKDonVi += "iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < dtNĐonVi.Rows.Count - 1)
                    DKDonVi += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtNĐonVi.Rows[i]["iID_MaDonVi"]);
            }
            if (String.IsNullOrEmpty(DKDonVi)) DKDonVi = " AND 0=1";
            else
            {
                DKDonVi = "AND (" + DKDonVi + ")";
            }
            return DKDonVi;
        }

        public static String DKPhongBan(String MaND, SqlCommand cmd, String iID_MaPhongBan)
        {
            String DKPhongBan = "";
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            if (iID_MaPhongBan == "-1" || iID_MaPhongBan == "" || iID_MaPhongBan == "02" || iID_MaPhongBan == "11")
            {
                if (sTenPB == "02" || sTenPB == "2" || sTenPB == "11")
                    DKPhongBan = " AND 1=1";
                else
                {
                    DKPhongBan = " AND iID_MaPhongBan=@iID_MaPhongBan";
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", sTenPB);
                }
            }
            else
            {
                DKPhongBan = " AND iID_MaPhongBan=@iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            }
            return DKPhongBan;
        }

        public static String DKPhongBan_QuyetToan(String MaND, SqlCommand cmd)
        {
            String DKPhongBan = "";
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            if (sTenPB != "02")
            {
                DKPhongBan = " AND iID_MaPhongBan=@iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", sTenPB);
            }
            return DKPhongBan;
        }

        public static String DKLNS(String MaND, SqlCommand cmd, String iID_MaPhongBan)
        {
            String DKLNS = "";
            DataTable dtLNS = DanhMucModels.NS_LoaiNganSachFull(iID_MaPhongBan);
            for (int i = 0; i < dtLNS.Rows.Count; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < dtLNS.Rows.Count - 1)
                    DKLNS += " OR ";
                cmd.Parameters.AddWithValue("@sLNS" + i, dtLNS.Rows[i]["sLNS"]);
            }
            if (String.IsNullOrEmpty(DKLNS)) DKLNS = " AND 0=1";
            else
            {
                DKLNS = "AND (" + DKLNS + ")";
            }
            return DKLNS;
        }

        public static DataTable getDSPhongBan(String iNamLamViec, String MaND)
        {
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            String DK = "";


            if (sTenPB == "02" || sTenPB == "2")
            {

            }
            else
            {
                DK = " AND iID_MaPhongBan=@iID_MaPhongBan";
            }

            String SQL = String.Format(@"SELECT DISTINCT iID_MaPhongBan,sTenPhongBan
FROM TN_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0}
AND iID_MaPhongBan NOT IN (02)
", DK);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", sTenPB);
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow dr = dt.NewRow();
            dr["iID_MaPhongBan"] = "-1";
            dr["sTenPhongBan"] = "--Chọn tất cả các B--";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        public static DataTable getDSLoaiBaoCao()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof (String));
            dt.Columns.Add("sTen", typeof (String));
            DataRow dr = dt.NewRow();
            dr["MaLoai"] = "1";
            dr["sTen"] = "Loại hình--Đơn vị";
            dt.Rows.InsertAt(dr, 0);

            dr = dt.NewRow();
            dr["MaLoai"] = "2";
            dr["sTen"] = "Đơn vị--Loại hình";
            dt.Rows.InsertAt(dr, 1);
            return dt;
        }

        public static DataTable getDSTO()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof (String));
            dt.Columns.Add("sTen", typeof (String));
            DataRow dr = dt.NewRow();
            dr["MaLoai"] = "1";
            dr["sTen"] = "Tờ 1";
            dt.Rows.InsertAt(dr, 0);

            dr = dt.NewRow();
            dr["MaLoai"] = "2";
            dr["sTen"] = "Tờ 2";
            dt.Rows.InsertAt(dr, 1);
            return dt;
        }

        public static DataTable getDSLoaiThongTri()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoai", typeof (String));
            dt.Columns.Add("sTen", typeof (String));
            DataRow dr = dt.NewRow();
            dr["MaLoai"] = "1";
            dr["sTen"] = "Nộp NSQP";
            dt.Rows.InsertAt(dr, 0);

            dr = dt.NewRow();
            dr["MaLoai"] = "2";
            dr["sTen"] = "Thuế TNDN qua BQP";
            dt.Rows.InsertAt(dr, 1);

            dr = dt.NewRow();
            dr["MaLoai"] = "3";
            dr["sTen"] = "Thuế TNCN";
            dt.Rows.InsertAt(dr, 2);
            return dt;
        }

        public static DataTable getKeHoach(String MaND, String iID_MaPhongBan)
        {
            String DKDonVi = "", DKPhongBan = "";
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan(MaND, cmd, iID_MaPhongBan);
            String SQL = "";
            if (iID_MaPhongBan == "-1")
            {
                SQL = String.Format(@"SELECT iID_MaPhongBan,
KeHoachNSQP=SUM(CASE WHEN sLNS like ('801%') THEN rTuChi ELSE 0 END)
,KeHoachNSNN=SUM(CASE WHEN sLNS like ('802%') THEN rTuChi ELSE 0 END)
FROM DT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1}
GROUP BY iID_MaPhongBan", DKDonVi, DKPhongBan);
            }
            else
            {
                SQL =
                    String.Format(@"SELECT iID_MaPhongBan,iID_MaDonVi,REPLACE(sTenDonVi,iID_MaDonVi+' - ','') as sTenDonVi,
KeHoachNSQP=SUM(CASE WHEN sLNS like ('801%') THEN rTuChi ELSE 0 END)
,KeHoachNSNN=SUM(CASE WHEN sLNS like ('802%') THEN rTuChi ELSE 0 END)
FROM DT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1}
GROUP BY iID_MaPhongBan,iID_MaDonVi,sTenDonVi", DKDonVi, DKPhongBan);
            }

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable getdtLoaiHinh()
        {
            String SQL = "";
            SQL =
                String.Format(
                    @"SELECT DC_DanhMuc.sTen as  sMoTa,DC_DanhMuc.sTenKhoa as sNG FROM DC_LoaiDanhMuc INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc WHERE DC_DanhMuc.bHoatDong=1 AND DC_LoaiDanhMuc.sTenBang=N'TN_LoaiHinh' ORDER BY iSTT");
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }

        public static bool checkXoaThuNop(String iID_MaChungTu)
        {
            String SQL =
                "SELECT COUNT(iID_MaChungTu) FROM TN_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            int a = Convert.ToInt32(Connection.GetValue(cmd, 0));
            if (a > 0) return false;
            else
            {
                return true;
            }
        }
        #region duonglh3
        public static String DKDonVi(String MaND, SqlCommand cmd, String tblAlias)
        {
            String DKDonVi = "";
            DataTable dtNĐonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
            for (int i = 0; i < dtNĐonVi.Rows.Count; i++)
            {
                DKDonVi += tblAlias + ".iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < dtNĐonVi.Rows.Count - 1)
                    DKDonVi += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtNĐonVi.Rows[i]["iID_MaDonVi"]);
            }
            if (String.IsNullOrEmpty(DKDonVi)) DKDonVi = " AND 0=1";
            else
            {
                DKDonVi = "AND (" + DKDonVi + ")";
            }
            return DKDonVi;
        }
        public static String DKPhongBan(String MaND, SqlCommand cmd, String iID_MaPhongBan, String tblAlias)
        {
            String DKPhongBan = "";
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            if (iID_MaPhongBan == "-1" || iID_MaPhongBan == "")
            {
                if (sTenPB == "02" || sTenPB == "2")
                    DKPhongBan = " AND 1=1";
                else
                {
                    DKPhongBan = string.Format(" AND {0}.iID_MaPhongBan=@iID_MaPhongBan", tblAlias);
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", sTenPB);
                }
            }
            else
            {
                DKPhongBan = string.Format(" AND {0}.iID_MaPhongBan=@iID_MaPhongBan", tblAlias);
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
            }
            return DKPhongBan;
        }
        public static String DKPhongBan_QuyetToan(String MaND, SqlCommand cmd, string tblAlias)
        {
            String DKPhongBan = "";
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            if (sTenPB != "02")
                DKPhongBan = string.Format(" AND {0}.iID_MaPhongBan=@iID_MaPhongBan", tblAlias);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", sTenPB);

            return DKPhongBan;
        }
        #endregion

    }
}