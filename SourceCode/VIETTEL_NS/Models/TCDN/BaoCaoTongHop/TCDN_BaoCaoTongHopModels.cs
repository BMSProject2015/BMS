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
    public class TCDN_BaoCaoTongHopModels
    {
        public static String sDanhSachTruongTien = "rDoanhThu,rLoiNhuanTT,rNopNganSach,rChiaCoTuc,rLaoDong,rLuongBQ";
        public static String sDanhSachTruongTien_U="rDoanhThu_U,rLoiNhuanTT_U,rNopNganSach_U,rLaoDong_U,rLuongBQ_U";
        public static String sDanhSachTruongTien_NT = "rDoanhThu_NT,rNopNganSach_NT,rLoiNhuanTT_NT,rChiaCoTuc_NT,rLaoDong_NT,rLuongBQ_NT";
        public static String sDanhSachTruong = sDanhSachTruongTien_NT + "," + sDanhSachTruongTien + "," + sDanhSachTruongTien_U;
        public static String sDanhSachTruongTieuDe = "DoanhThu,Nộp ngân sách,Lợi nhận TT,Chia cổ tức %,Lao động,Lương BQ,DoanhThu,Lợi nhận TT,Nộp ngân sách,Chia cổ tức %,Lao động,Lương BQ,DoanhThu,Lợi nhận TT,Nộp ngân sách,Lao động,Lương BQ";
        public static String sDanhSachTruongDoRong = "100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100";
        public static String[] arrDanhSachTruongTien_NT = sDanhSachTruongTien_NT.Split(',');

        public static void ThemChiTiet(String iID_MaLoaiDoanhNghiep, String iQuy, String iNam, String MaND, String IPSua)
        {
            int i;
            DataTable dt = DT_DanhMucDoanhNghiep(iID_MaLoaiDoanhNghiep);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            int iNamLamViec = Convert.ToInt32(iNam);
            DataTable dtNamTruoc = Get_dtQuyBonNamTruoc(iNamLamViec - 1, iID_MaLoaiDoanhNghiep);
            Bang bang = new Bang("TCDN_BaoCaoTongHop");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iQuy", iQuy);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNam);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDoanhNghiep", iID_MaLoaiDoanhNghiep);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                //Thêm thông tin vào bảng TCDN_BaoCaoTongHop
                ThemThongTinChiTiet(dt.Rows[i],dtNamTruoc, bang.CmdParams.Parameters);
                bang.Save();
            }
            dt.Dispose();
               
             
        }

        public static DataTable Get_dtQuyBonNamTruoc(int iNamLamViec,String iID_MaLoaiDoanhNghiep)
        {
            String SQL = "SELECT * FROM TCDN_BaoCaoTongHop WHERE iTrangThai=1 AND iQuy=4 AND iNamLamViec=@iNamLamViec AND iID_MaLoaiDoanhNghiep=@iID_MaLoaiDoanhNghiep";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec",iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaLoaiDoanhNghiep", iID_MaLoaiDoanhNghiep);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static void ThemThongTinChiTiet(DataRow RMucLucChiTieu,DataTable dtNamTruoc, SqlParameterCollection Params)
        {
            //<--Thêm tham số từ bảng MucLucNganSach
            String strDSTruong = "iID_MaDoanhNghiep,sTenDoanhNghiep";
            String[] arrDSTruong = strDSTruong.Split(',');

            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                if (Params.IndexOf("@" + arrDSTruong[i]) >= 0)
                {
                    Params["@" + arrDSTruong[i]].Value = RMucLucChiTieu[arrDSTruong[i]];
                }
                else
                {
                    Params.AddWithValue("@" + arrDSTruong[i], RMucLucChiTieu[arrDSTruong[i]]);
                }
            }
            String iID_MaDoanhNghiep = Convert.ToString(RMucLucChiTieu["iID_MaDoanhNghiep"]);
            String iID_MaDoanhNghiep1 = "";
            for (int i = 0; i < dtNamTruoc.Rows.Count; i++)
            {
                iID_MaDoanhNghiep1 =Convert.ToString(dtNamTruoc.Rows[i]["iID_MaDoanhNghiep"]);
                if (iID_MaDoanhNghiep.Equals(iID_MaDoanhNghiep1))
                {
                    for (int j = 0; j < arrDanhSachTruongTien_NT.Length; j++)
                    {
                        if (Params.IndexOf("@" + arrDanhSachTruongTien_NT[i]) >= 0)
                        {
                            Params["@" + arrDanhSachTruongTien_NT[i]].Value = dtNamTruoc.Rows[i][arrDSTruong[j]];
                        }
                        else
                        {
                            Params.AddWithValue("@" + arrDanhSachTruongTien_NT[i], dtNamTruoc.Rows[i][arrDSTruong[j]]);
                        }
                    }
                    dtNamTruoc.Rows.RemoveAt(i);
                    break;
                }
            }
        }
        public static DataTable Get_dtBaoCaoTongHop(String iNam, String iQuy, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);

            SQL = String.Format("SELECT * FROM TCDN_BaoCaoTongHop WHERE {0} ORDER BY sTenDoanhNghiep", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static Boolean CheckData(String iQuy, String iNam) {

            Boolean vR = false;

            SqlCommand cmd;
            cmd = new SqlCommand("SELECT COUNT(*) FROM TCDN_BaoCaoTongHop WHERE iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            Int32 iCount = Convert.ToInt32(Connection.GetValue(cmd,0));

            if (iCount > 0) {
                vR = true;
            }
            return vR;
        }
        public static DataTable DT_DanhMucDoanhNghiep(String iLoai)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_DoanhNghiep WHERE iTrangThai = 1 AND iID_MaLoaiDoanhNghiep=@iID_MaLoaiDoanhNghiep ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iID_MaLoaiDoanhNghiep", iLoai);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable LayDanhSachBaoCaoTaiChinh(String iNam, String iQuy) {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_BaoCaoTongHop WHERE iTrangThai = 1 AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy ORDER BY iSTT";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable LayVonDoanhNghiep_KhaiBao(String iID_MaDoanhNghiep)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_DoanhNghiep WHERE iTrangThai = 1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep";
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable LayDanhSachHoSoDoanhNghiep(String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_HoSoDoanhNghiepChiTiet WHERE iTrangThai = 1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep " +
                                "AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy";
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static DataTable LayDanhSachTinhHinhThuNop(String iID_MaDoanhNghiep, String iNam, String iQuy)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM TCDN_ChiTieuThuNop WHERE iTrangThai = 1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep " +
                                "AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy";
            cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static String LayTruongDuLieuBaoCaoTaiChinh(String iID_MaChiTieuHoSo)
        {
            String vR;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT iID_MaTruongKhoa FROM TCDN_BaoCaoTongHopTruongLayDuLieu_ChiTieuHoSo WHERE iTrangThai = 1 AND iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo";
            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo);
            vR = Convert.ToString(Connection.GetValue(cmd,""));
            cmd.Dispose();

            return vR;
        }
    }
}