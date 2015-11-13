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
    public class KTCS_HachToanModels
    {
        public static NameValueCollection LayThongTinLoaiTaiSan(String iID_Ma)
        {
            NameValueCollection data = new NameValueCollection();
            
            DataTable dt = Get_dtChiTietLoaiTaiSan(iID_Ma);
            String ColName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ColName = dt.Columns[i].ColumnName;
                    data.Add(ColName, Convert.ToString(dt.Rows[0][ColName]));
                }
            }
            return data;
        }

        public static DataTable Get_dtDSLoaiTaiSan()
        {            
            String SQL = "SELECT * FROM KTCS_LoaiTaiSan WHERE iTrangThai=1";
            return Connection.GetDataTable(SQL);
        }

        public static DataTable Get_dtChiTietLoaiTaiSan(String iID_Ma)
        {
            DataTable dt;
            String SQL = "SELECT * FROM KTCS_LoaiTaiSan WHERE iTrangThai=1 AND iID_Ma=@iID_Ma";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_Ma", iID_Ma);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static void ThemMoiHachToan(String iID_MaChungTuChiTiet, String iID_MaKyHieuHachToan, String NGIA, String TANG, String GIAM, String HMON, String LKHM, String CONLAI, String CONLAITKH, String NAMKHCL, String UserName, String IPSua)
        {
            DataTable dtCauHinhCT = KTCS_CauHinhHachToanModels.Get_dtCauHinhHachToanChiTiet(iID_MaKyHieuHachToan);

            String rSoTien = "0";
            String iID_MaTaiKhoan_No, iID_MaTaiKhoan_Co, iID_MaKyHieuHachToanChiTiet;
            DataTable  dtCTChiTiet = KTCS_ChungTuChiTietModels.Get_dtChungTuChiTiet_Row(iID_MaChungTuChiTiet);
            for (int i = 0; i < dtCauHinhCT.Rows.Count; i++)
            {
                String sGiaTri = Convert.ToString(dtCauHinhCT.Rows[i]["sGiaTri"]);
                switch (sGiaTri)
                {
                    case "NGIA":
                        rSoTien = NGIA;
                        break;
                    case "TANG":
                        rSoTien = TANG;
                        break;
                    case "GIAM":
                        rSoTien = GIAM;
                        break;
                    case "HMON":
                        rSoTien = HMON;
                        break;
                    case "LKHM":
                        rSoTien = LKHM;
                        break;
                    case "CONLAI":
                        rSoTien = CONLAI;
                        break;
                    case "CONLAITKH":
                        rSoTien = CONLAITKH;
                        break;
                    case "NAMKHCL":
                        rSoTien = NAMKHCL;
                        break;
                }
                //NGIA#TANG#GIAM#HMON#LKHM#CONLAI#CONLAITKH#NAMKHCL
                iID_MaTaiKhoan_No = Convert.ToString(dtCauHinhCT.Rows[i]["iID_MaTaiKhoan_No"]);
                iID_MaTaiKhoan_Co = Convert.ToString(dtCauHinhCT.Rows[i]["iID_MaTaiKhoan_Co"]);
                iID_MaKyHieuHachToanChiTiet = Convert.ToString(dtCauHinhCT.Rows[i]["iID_MaKyHieuHachToanChiTiet"]);
                String sTenTaiKhoan_No = TaiKhoanModels.LayTenTaiKhoan(iID_MaTaiKhoan_No);
                String sTenTaiKhoan_Co = TaiKhoanModels.LayTenTaiKhoan(iID_MaTaiKhoan_Co);


                Bang bang = new Bang("KTCS_ChungTuChiTietHachToan");
                bang.MaNguoiDungSua = UserName;
                bang.IPSua = IPSua;
                bang.CmdParams.Parameters.AddWithValue("@iNgay", dtCTChiTiet.Rows[0]["iNgay"]);
                bang.CmdParams.Parameters.AddWithValue("@iThang", dtCTChiTiet.Rows[0]["iThang"]);
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtCTChiTiet.Rows[0]["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiSan", dtCTChiTiet.Rows[0]["iID_MaTaiSan"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaKyHieuHachToan", iID_MaKyHieuHachToan);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaKyHieuHachToanChiTiet", iID_MaKyHieuHachToanChiTiet);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", iID_MaTaiKhoan_No);
                bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", sTenTaiKhoan_No);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", iID_MaTaiKhoan_Co);
                bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", sTenTaiKhoan_Co);
                bang.CmdParams.Parameters.AddWithValue("@rSoTien", rSoTien);
                bang.Save();
            }
        }

        public static void XoaHachToan(String iID_MaChungTuChiTiet)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM KTCS_ChungTuChiTietHachToan WHERE iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet");
            cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        /// <summary>
        /// String: NGIA#TANG#GIAM#HMON#LKHM#CONLAI#CONLAITKH#NAMKHCL
        /// </summary>
        /// <param name="iID_MaTaiSan"></param>
        /// <param name="iNamTinh"></param>
        /// <param name="iThangTinh"></param>
        /// <param name="MaND"></param>
        /// <param name="sIP"></param>
        /// <returns></returns>
        public static String TinhHachToanTaiSan(String iID_MaTaiSan, int iNamTinh, int iThangTinh, String MaND, String sIP)
        {
            String vR = "";
            Double dSoNamKhauHao = 0, dNguyenGiaBanDau = 0, dNguyenGia = 0, dGiam = 0, dTongTang = 0, dTongGiam = 0;
            DataTable dt;
            SqlCommand cmd;
            String SQL;

            //Gia trị tra ve
            Double dNguyenGiaMoi = 0;
            Double dMucHaoMonNam = 0;
            Double dSoNamHaoMonMoi = 0;
            Double dSoHaoMonLuyKe = 0;
            Double dGiaTriConLaiTruocKhauHao = 0;
            Double dGiaTriConLai = 0;
            Double dTongTangMoi = 0;
            Double dTongGiamMoi = 0;

            //Lay nam dau tien tinh khau hao
            cmd = new SqlCommand("SELECT *, YEAR(dNgayDuaVaoKhauHao) AS NamKhauHao FROM KTCS_TaiSan WHERE iID_MaTaiSan=@iID_MaTaiSan AND iTrangThai = 1");
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            DataTable dtTaiSan = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int iNamBatDauKhauHao = Convert.ToInt32(dtTaiSan.Rows[0]["NamKhauHao"]);

            if (iNamBatDauKhauHao > 0 && iNamTinh >= iNamBatDauKhauHao)
            {
                for (int i = iNamBatDauKhauHao; i <= iNamTinh; i++)
                {
                    int iNam = i;
                    //Xoa hang du lieu trong bang KTCS_HachToanTaiSan neu nam da có
                    cmd = new SqlCommand("DELETE FROM KTCS_HachToanTaiSan WHERE iID_MaTaiSan=@iID_MaTaiSan AND iNamLamViec=@iNamLamViec AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();

                    //Lay thong tin trong bang KTCS_HachToanTaiSan
                    cmd = new SqlCommand("SELECT * FROM KTCS_HachToanTaiSan WHERE iID_MaTaiSan=@iID_MaTaiSan AND iNamLamViec=@iNamLamViec AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam - 1);
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                    if (dt.Rows.Count > 0)
                    {
                        dNguyenGiaBanDau = Convert.ToDouble(dt.Rows[0]["rNguyenGia"]);
                        dNguyenGia = Convert.ToDouble(dt.Rows[0]["rGiaTriConLai"]);
                        dSoNamKhauHao = Convert.ToDouble(dt.Rows[0]["iSoNamKhauHaoThayDoi"]);
                        Double rSoTienKhaoHao_LuyKe = Convert.ToDouble(dt.Rows[0]["rSoTienKhauHao_LuyKe"]);

                        String DK = "";

                        if (iNam == iNamTinh) {
                            DK += "AND iThang<=@iThang";
                        }

                        cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec " + DK + " AND iTrangThai = 1 AND sTinhChat IN ('1','T')");
                        cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                        cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                        if (iNam == iNamTinh)
                        {
                            cmd.Parameters.AddWithValue("@iThang", iThangTinh);
                        }
                        dTongTang = Convert.ToDouble(Connection.GetValue(cmd, 0));
                        cmd.Dispose();

                        cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec  " + DK + " AND iTrangThai = 1 AND sTinhChat='2'");
                        cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                        cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                        if (iNam == iNamTinh)
                        {
                            cmd.Parameters.AddWithValue("@iThang", iThangTinh);
                        }
                        dTongGiam = Convert.ToDouble(Connection.GetValue(cmd, 0));
                        cmd.Dispose();

                        cmd = new SqlCommand("SELECT TOP 1 rNamKhauHao FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec " + DK + " AND iTrangThai = 1 ORDER BY iThang DESC, iNgay DESC");
                        cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                        cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                        if (iNam == iNamTinh)
                        {
                            cmd.Parameters.AddWithValue("@iThang", iThangTinh);
                        }
                        Double iNamKhauHaoCSDL = Convert.ToDouble(Connection.GetValue(cmd, -1));
                        cmd.Dispose();
                        if (iNamKhauHaoCSDL > 0)
                        {
                            dSoNamKhauHao = iNamKhauHaoCSDL;
                        }

                        dNguyenGiaBanDau = dNguyenGiaBanDau + (dTongTang - dTongGiam);
                        dTongTangMoi = dTongTang;
                        dTongGiamMoi = dTongGiam;

                        dNguyenGiaMoi = dNguyenGia + (dTongTang - dTongGiam);
                        if (dNguyenGiaMoi != 0 || dSoNamKhauHao != 0)
                        {
                            dMucHaoMonNam = dNguyenGiaMoi / dSoNamKhauHao;
                            dSoNamHaoMonMoi = dSoNamKhauHao - 1;
                        }
                        else
                        {
                            dMucHaoMonNam = 0;
                            dSoNamHaoMonMoi = 0;
                        }
                        dGiaTriConLaiTruocKhauHao = dNguyenGiaBanDau - rSoTienKhaoHao_LuyKe;
                        dSoHaoMonLuyKe = rSoTienKhaoHao_LuyKe + dMucHaoMonNam;
                        dGiaTriConLai = dNguyenGiaBanDau - dSoHaoMonLuyKe;

                        ThemVaoBangHachToanTaiSanNam(iID_MaTaiSan, iNam, iThangTinh, dNguyenGiaBanDau, dTongTang, dTongGiam, dMucHaoMonNam, dSoHaoMonLuyKe, dGiaTriConLaiTruocKhauHao, dGiaTriConLai, dSoNamHaoMonMoi, MaND, sIP);
                    }
                    else
                    {
                        if (dtTaiSan.Rows.Count > 0)
                        {
                            dNguyenGiaBanDau = Convert.ToDouble(dtTaiSan.Rows[0]["rNguyenGia"]);
                            dNguyenGia = Convert.ToDouble(dtTaiSan.Rows[0]["rNguyenGia"]);
                            dSoNamKhauHao = Convert.ToDouble(dtTaiSan.Rows[0]["rSoNamKhauHao"]);
                            dtTaiSan.Dispose();

                            String DK = "";

                            if (iNam == iNamTinh)
                            {
                                DK += "AND iThang<=@iThang";
                            }

                            cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec " + DK + " AND iTrangThai = 1 AND sTinhChat IN ('1','T')");
                            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                            if (iNam == iNamTinh)
                            {
                                cmd.Parameters.AddWithValue("@iThang", iThangTinh);
                            }
                            dTongTang = Convert.ToDouble(Connection.GetValue(cmd, 0));
                            cmd.Dispose();

                            cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec " + DK + " AND iTrangThai = 1 AND sTinhChat='2'");
                            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                            if (iNam == iNamTinh)
                            {
                                cmd.Parameters.AddWithValue("@iThang", iThangTinh);
                            }
                            dTongGiam = Convert.ToDouble(Connection.GetValue(cmd, 0));
                            cmd.Dispose();

                            cmd = new SqlCommand("SELECT TOP 1 rNamKhauHao FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec " + DK + " AND iTrangThai = 1 ORDER BY iThang, iNgay DESC");
                            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                            if (iNam == iNamTinh)
                            {
                                cmd.Parameters.AddWithValue("@iThang", iThangTinh);
                            }
                            Double iNamKhauHaoCSDL = Convert.ToDouble(Connection.GetValue(cmd, -1));
                            cmd.Dispose();
                            if (iNamKhauHaoCSDL > 0)
                            {
                                dSoNamKhauHao = iNamKhauHaoCSDL;
                            }
                            
                            dNguyenGiaBanDau = dNguyenGiaBanDau + (dTongTang - dTongGiam);
                            dTongTangMoi = dTongTang;
                            dTongGiamMoi = dTongGiam;

                            dNguyenGiaMoi = dNguyenGia + (dTongTang - dTongGiam);
                            if (dNguyenGiaMoi != 0 || dSoNamKhauHao != 0)
                            {
                                dMucHaoMonNam = dNguyenGiaMoi / dSoNamKhauHao;
                                dSoNamHaoMonMoi = dSoNamKhauHao - 1;
                            }
                            else
                            {
                                dMucHaoMonNam = 0;
                                dSoNamHaoMonMoi = 0;
                            }
                            dGiaTriConLaiTruocKhauHao = dNguyenGiaMoi;
                            dSoHaoMonLuyKe = dMucHaoMonNam;
                            dGiaTriConLai = dNguyenGiaBanDau - dSoHaoMonLuyKe;

                            ThemVaoBangHachToanTaiSanNam(iID_MaTaiSan, iNam, iThangTinh, dNguyenGiaBanDau, dTongTang, dTongGiam, dMucHaoMonNam, dSoHaoMonLuyKe, dGiaTriConLaiTruocKhauHao, dGiaTriConLai, dSoNamHaoMonMoi, MaND, sIP);
                        }
                    }
                    dt.Dispose();
                }
            }
            vR = String.Format("{0}#{1}#{2}#{3}#{4}#{5}#{6}#{7}", dNguyenGiaBanDau, dTongTangMoi, dTongGiamMoi, dMucHaoMonNam, dSoHaoMonLuyKe, dGiaTriConLai, dGiaTriConLaiTruocKhauHao, dSoNamHaoMonMoi);
            return vR;
        }

        public static DataTable Get_Table_TinhHachToanTaiSan(int iNam)
        {
            DataTable vR;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT * FROM KTCS_HachToanTaiSan WHERE iTrangThai = 1 AND iNamLamViec = @iNamLamViec");
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_Table_ChiTietHachToanTaiSan(int iNam, String iID_MaTaiSan)
        {
            DataTable vR;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT * FROM KTCS_ChungTuChiTietHachToan WHERE iTrangThai = 1 AND iID_MaTaiSan = @iID_MaTaiSan AND iNamLamViec = @iNamLamViec ORDER BY iThang");
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static void ThemVaoBangHachToanTaiSanNam(String iID_MaTaiSan, int iNam,int iThang, Double rNguyenGia
            , Double rSoTienTang, Double rSoTienGiam, Double rSoTienKhauHao, Double rSoTienKhaoHao_LuyKe
            , Double rGiaTriConLaiTruocKhauHao, Double rGiaTriConLai, Double iSoNamKhaoHao, String MaND, String sIP)
        {
            Bang bang = new Bang("KTCS_HachToanTaiSan");
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNam);
            bang.CmdParams.Parameters.AddWithValue("@iThang", iThang);
            bang.CmdParams.Parameters.AddWithValue("@rNguyenGia", rNguyenGia);
            bang.CmdParams.Parameters.AddWithValue("@rSoTienTang", rSoTienTang);
            bang.CmdParams.Parameters.AddWithValue("@rSoTienGiam", rSoTienGiam);
            bang.CmdParams.Parameters.AddWithValue("@rSoTienKhauHao", rSoTienKhauHao);
            bang.CmdParams.Parameters.AddWithValue("@rSoTienKhauHao_LuyKe", rSoTienKhaoHao_LuyKe);
            bang.CmdParams.Parameters.AddWithValue("@rGiaTriConLaiTruocKhauHao", rGiaTriConLaiTruocKhauHao);
            bang.CmdParams.Parameters.AddWithValue("@rGiaTriConLai", rGiaTriConLai);
            bang.CmdParams.Parameters.AddWithValue("@iSoNamKhauHao", iSoNamKhaoHao);
            bang.CmdParams.Parameters.AddWithValue("@iSoNamKhauHaoThayDoi", iSoNamKhaoHao);
            bang.CmdParams.Parameters.AddWithValue("@dNgayKhauHao", DateTime.Now);
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = sIP;
            bang.Save();
        }
    }
}