using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class BangLuong_TruyLinhModels
    {
        public static void CapNhapBangLuongTruyLinh(String iID_MaBangLuongChiTiet, String UserID, String IPSua)
        {
            DataTable dtChiTiet = LuongModels.Get_ChiTietBangLuongChiTiet(iID_MaBangLuongChiTiet);

            DataRow Row = dtChiTiet.Rows[0];
            SqlCommand cmd;
            if (Convert.ToBoolean(Row["bTruyLinh"]))
            {
                TinhTruyLinh(dtChiTiet, UserID, IPSua);
            }
            else
            {
                if (Convert.ToBoolean(Row["bCoPhanTruyLinh"]))
                {
                    //Xóa trong bảng chi tiết lương
                    cmd = new SqlCommand("UPDATE L_BangLuongChiTiet SET iTrangThai=0 WHERE iID_MaBangLuong=@iID_MaBangLuong AND iID_MaCanBo=@iID_MaCanBo AND bPhanTruyLinh=1");
                    cmd.Parameters.AddWithValue("iID_MaBangLuong", Row["iID_MaBangLuong"]);
                    cmd.Parameters.AddWithValue("iID_MaCanBo", Row["iID_MaCanBo"]);
                    Connection.UpdateDatabase(cmd, UserID, IPSua);

                    //Cập nhập lại bảng chi tiết
                    cmd = new SqlCommand("UPDATE L_BangLuongChiTiet SET bCoPhanTruyLinh=0 WHERE iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet");
                    cmd.Parameters.AddWithValue("iID_MaBangLuongChiTiet", Row["iID_MaBangLuongChiTiet"]);
                    Connection.UpdateDatabase(cmd, UserID, IPSua);
                }
            }

            dtChiTiet.Dispose();
        }

        private static void TinhTruyLinh(DataTable dtChiTiet, String MaND, String IPSua)
        {
            SqlCommand cmd = new SqlCommand("SELECT iID_MaBangLuongChiTiet FROM L_BangLuongChiTiet WHERE bPhanTruyLinh=1 AND iID_MaBangLuong=@iID_MaBangLuong AND iID_MaCanBo=@iID_MaCanBo");
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", dtChiTiet.Rows[0]["iID_MaBangLuong"]);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", dtChiTiet.Rows[0]["iID_MaCanBo"]);
            String iID_MaBangLuongChiTiet = Convert.ToString(Connection.GetValue(cmd,""));
            cmd.Dispose();

            DateTime dTruyLinh_TuNgay = Convert.ToDateTime(dtChiTiet.Rows[0]["dTruyLinh_TuNgay"]);
            DateTime dTruyLinh_DenNgay = Convert.ToDateTime(dtChiTiet.Rows[0]["dTruyLinh_DenNgay"]);
            String iID_MaCanBo=Convert.ToString(dtChiTiet.Rows[0]["iID_MaCanBo"]);

            //DataTable dtChiTiet_PhuCap = LuongModels.Get_dtLuongPhuCap(iID_MaBangLuongChiTiet);
            DataTable dtChiTiet_TruyLinh = Get_dtChiTietThang(dTruyLinh_TuNgay, dTruyLinh_DenNgay, iID_MaCanBo);
            //DataTable dtChiTiet_PhuCap_TruyLinh = Get_dtChiTietThang(dTruyLinh_TuNgay, dTruyLinh_DenNgay, iID_MaCanBo);

            DataRow Row_TruyLinh, Row;
            //for (int iPhuCap_TruyLinh = 0; iPhuCap_TruyLinh < dtChiTiet_PhuCap_TruyLinh.Rows.Count; iPhuCap_TruyLinh++)
            //{
            //    Row_TruyLinh = dtChiTiet_PhuCap_TruyLinh.Rows[iPhuCap_TruyLinh];
            //    for (int iPhuCap = 0; iPhuCap < dtChiTiet_PhuCap.Rows.Count; iPhuCap++)
            //    {
            //        Row = dtChiTiet_PhuCap.Rows[iPhuCap];
            //        if (Convert.ToString(Row["iID_MaPhuCap"]) == Convert.ToString(Row_TruyLinh["iID_MaPhuCap"]))
            //        {
            //            Row_TruyLinh["rSoTien"] = Convert.ToDouble(Row["rSoTien"]) - Convert.ToDouble(Row_TruyLinh["rSoTien"]);
            //            break;
            //        }
            //    }
            //}

            String strTruongTru = "rLuongCoBan,rTienAn1Ngay,";
            String strTruong0 = "rGiamTruGiaCanh,rGiamTruBanThan,rGiamTruKhac,rTienOmNganNgay,rDuocGiamThue,rThuong,rLoiIchKhac,rDaNopThue,rNopThueDauVao,rDieuChinhThuNhap,rDieuChinhThueDaNop,rDieuChinhThueGiamTru,rTrichLuong_SoLuong,rTrichLuong,rPhuCap_ThamNien,";
            String strTruongCongThuc = "sLuongCoBan_NganSach_CongThuc,sKhoanTru_TienAn_CongThuc,sTienTinhThueTNCN_CongThuc,sThueTNCN_CongThuc";
            String strTruongGiaTriCongThuc = "rLuongCoBan_NganSach,rKhoanTru_TienAn,rTienTinhThueTNCN,rThueTNCN";
            String[] arrTruongCongThuc = strTruongCongThuc.Split(',');
            String[] arrTruongGiaTriCongThuc = strTruongGiaTriCongThuc.Split(',');

            Row = dtChiTiet.Rows[0];

            for (int j = 0; j < dtChiTiet.Columns.Count; j++)
            {
                string TenCot = Convert.ToString(dtChiTiet.Columns[j].ColumnName);
                if (TenCot.StartsWith("r") && TenCot.IndexOf("HeSo") == -1)
                {
                    if (strTruong0.IndexOf(TenCot + ",") >= 0 ||
                        TenCot.StartsWith("rKhoanTru_") ||
                        TenCot.StartsWith("rBaoHiemChi_"))
                    {
                        Row[TenCot] = 0;
                    }
                    else if (strTruongTru.IndexOf(TenCot + ",") >= 0 ||
                        TenCot.StartsWith("rPhuCap_") ||
                        TenCot.StartsWith("rBaoHiem_"))
                    {
                        double S = 0;
                        for (int iChiTiet_TruyLinh = 0; iChiTiet_TruyLinh < dtChiTiet_TruyLinh.Rows.Count; iChiTiet_TruyLinh++)
                        {
                            Row_TruyLinh = dtChiTiet_TruyLinh.Rows[iChiTiet_TruyLinh];

                            Double S1 = Convert.ToDouble(Row[TenCot]) - Convert.ToDouble(Row_TruyLinh[TenCot]);
                            if (dTruyLinh_TuNgay.Year == Convert.ToInt32(Row_TruyLinh["iNamBangLuong"]) && dTruyLinh_TuNgay.Month == Convert.ToInt32(Row_TruyLinh["iThangBangLuong"]) && dTruyLinh_TuNgay.Day > 15)
                            {
                                S1 = S1 / 2;
                            }
                            else if (dTruyLinh_DenNgay.Year == Convert.ToInt32(Row_TruyLinh["iNamBangLuong"]) && dTruyLinh_TuNgay.Month == Convert.ToInt32(Row_TruyLinh["iThangBangLuong"]) && dTruyLinh_DenNgay.Day <= 15)
                            {
                                S1 = S1 / 2;
                            }
                            S += S1;
                        }
                        Row[TenCot] = S;
                    }   
                }
            }
            DataTable dtDanhMucLoaiCongThuc = LuongModels.Get_dtDanhMucLoaiCongThuc();
            for (int j = 0; j < arrTruongCongThuc.Length; j++)
            {
                string TenCot = arrTruongCongThuc[j];
                string Truong = arrTruongGiaTriCongThuc[j];
                String sCongThuc = Convert.ToString(Row[TenCot]);
                Boolean okGiaTriCoThayDoi = false;
                Double rGiaTri = BangLuongChiTiet_CaNhanModels.TinhTienTheoCongThuc(sCongThuc, Row, dtDanhMucLoaiCongThuc, null, null, ref okGiaTriCoThayDoi);
                if (okGiaTriCoThayDoi)
                {
                    Row[Truong] = rGiaTri;
                }
            }


            Bang bang = new Bang("L_BangLuongChiTiet");
            if (String.IsNullOrEmpty(iID_MaBangLuongChiTiet) == false)
            {
                bang.GiaTriKhoa = iID_MaBangLuongChiTiet;
                bang.DuLieuMoi = false;
            }
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;

            int cs1 = dtChiTiet.Columns.IndexOf("iSTT");
            for (int i = 1; i < cs1; i++)
            {
                bang.CmdParams.Parameters.AddWithValue("@" + dtChiTiet.Columns[i].ColumnName, Row[i]);
            }
            bang.CmdParams.Parameters["@bTruyLinh"].Value = false;
            bang.CmdParams.Parameters["@bPhanTruyLinh"].Value = true;
            bang.Save();

            //dtChiTiet_PhuCap.Dispose();
            dtChiTiet_TruyLinh.Dispose();
            //dtChiTiet_PhuCap_TruyLinh.Dispose();
        }

        private static DataTable Get_dtChiTietThang(DateTime dTruyLinh_TuNgay, DateTime dTruyLinh_DenNgay, String iID_MaCanBo)
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=0";
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);

            DateTime dDau = dTruyLinh_DenNgay;
            int d = 0;
            while (String.Compare(dDau.ToString("yyyyMMdd"), dTruyLinh_DenNgay.ToString("yyyyMMdd")) <= 0)
            {
                DK += String.Format(" OR (iThangBangLuong=@iThangBangLuong{0} AND iNamBangLuong=@iNamBangLuong{0})", d);
                cmd.Parameters.AddWithValue("@iThangBangLuong" + d, dDau.Month);
                cmd.Parameters.AddWithValue("@iNamBangLuong" + d, dDau.Year);
                d = d + 1;
                dDau = dDau.AddMonths(1);
            }
            cmd.CommandText = String.Format("SELECT * FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND bPhanTruyLinh=0 AND iID_MaCanBo=@iID_MaCanBo AND ({0}) ORDER BY iID_MaCanBo, iNamBangLuong, iThangBangLuong", DK);

            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        private static DataTable Get_dtChiTietThang_PhuCap(DateTime dTruyLinh_TuNgay, DateTime dTruyLinh_DenNgay, String iID_MaCanBo)
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=0";
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);

            DateTime dDau = dTruyLinh_DenNgay;
            int d = 0;
            while (dDau < dTruyLinh_DenNgay)
            {
                DK = String.Format(" OR (iThangBangLuong=@iThangBangLuong{0} AND iNamBangLuong=@iNamBangLuong{0})", d);
                cmd.Parameters.AddWithValue("@iThangBangLuong" + d, dDau.Month);
                cmd.Parameters.AddWithValue("@iNamBangLuong" + d, dDau.Year);
                d = d + 1;
                dDau = dDau.AddMonths(1);
            }
            cmd.CommandText = String.Format("SELECT * FROM L_BangLuongChiTiet_PhuCap WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND ({0}) ORDER BY iID_MaCanBo, iNamBangLuong, iThangBangLuong", DK);

            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
    }
}