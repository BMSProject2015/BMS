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
    public class KTCS_KhauHaoModels
    {
        public static void TinhKhauHaoTaiSan(String iID_MaTaiSan, int iNamTinh, String MaND, String sIP) {
            Double dSoNamKhauHao = 0, dNguyenGiaBanDau = 0, dNguyenGia = 0, dGiam = 0, dTongTang = 0, dTongGiam = 0;
            DataTable dt;
            SqlCommand cmd;
            String SQL;

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
                    //Xoa hang du lieu trong bang KTCS_KhauHaoHangNam neu nam da có
                    cmd = new SqlCommand("DELETE FROM KTCS_KhauHaoHangNam WHERE iID_MaTaiSan=@iID_MaTaiSan AND iNamLamViec=@iNamLamViec AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();

                    //Lay thong tin trong bang KTCS_KhauHaoHangNam
                    cmd = new SqlCommand("SELECT * FROM KTCS_KhauHaoHangNam WHERE iID_MaTaiSan=@iID_MaTaiSan AND iNamLamViec=@iNamLamViec AND iTrangThai = 1");
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

                        cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec AND iTrangThai = 1 AND sTinhChat IN ('1','T')");
                        cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                        cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                        dTongTang = Convert.ToDouble(Connection.GetValue(cmd, 0));
                        cmd.Dispose();

                        cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec AND iTrangThai = 1 AND sTinhChat='2'");
                        cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                        cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                        dTongGiam = Convert.ToDouble(Connection.GetValue(cmd, 0));
                        cmd.Dispose();

                        cmd = new SqlCommand("SELECT TOP 1 rNamKhauHao FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec AND iTrangThai = 1 ORDER BY iThang DESC, iNgay DESC");
                        cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                        cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                        Double iNamKhauHaoCSDL = Convert.ToDouble(Connection.GetValue(cmd, -1));
                        cmd.Dispose();
                        if (iNamKhauHaoCSDL > 0) {
                            dSoNamKhauHao = iNamKhauHaoCSDL;
                        }

                        
                        Double dNguyenGiaMoi = 0;
                        Double dMucHaoMonNam = 0;
                        Double dSoNamHaoMonMoi = 0;
                        Double dSoHaoMonLuyKe = 0;
                        Double dGiaTriConLaiTruocKhauHao = 0;
                        Double dGiaTriConLai = 0;

                        dNguyenGiaBanDau = dNguyenGiaBanDau + (dTongTang - dTongGiam);

                        dNguyenGiaMoi = dNguyenGia + (dTongTang - dTongGiam);
                        if (dNguyenGiaMoi != 0 && dSoNamKhauHao != 0)
                        {
                            dMucHaoMonNam = dNguyenGiaMoi / dSoNamKhauHao;
                            dSoNamHaoMonMoi = dSoNamKhauHao - 1;
                        }
                        else {
                            dMucHaoMonNam = 0;
                            dSoNamHaoMonMoi = 0;
                        }
                        dGiaTriConLaiTruocKhauHao = dNguyenGiaBanDau - rSoTienKhaoHao_LuyKe;
                        dSoHaoMonLuyKe = rSoTienKhaoHao_LuyKe + dMucHaoMonNam;
                        dGiaTriConLai = dNguyenGiaBanDau - dSoHaoMonLuyKe;

                        ThemVaoBangKhauHaoTaiSanNam(iID_MaTaiSan, iNam, dNguyenGiaBanDau, dTongTang, dTongGiam, dMucHaoMonNam, dSoHaoMonLuyKe, dGiaTriConLaiTruocKhauHao, dGiaTriConLai, dSoNamHaoMonMoi, MaND, sIP);
                    }
                    else
                    {
                        if (dtTaiSan.Rows.Count > 0)
                        {
                            dNguyenGiaBanDau = Convert.ToDouble(dtTaiSan.Rows[0]["rNguyenGia"]);
                            dNguyenGia = Convert.ToDouble(dtTaiSan.Rows[0]["rNguyenGia"]);
                            dSoNamKhauHao = Convert.ToDouble(dtTaiSan.Rows[0]["rSoNamKhauHao"]);
                            dtTaiSan.Dispose();

                            cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND  YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec AND iTrangThai = 1  AND sTinhChat IN ('1','T')");
                            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                            dTongTang = Convert.ToDouble(Connection.GetValue(cmd, 0));
                            cmd.Dispose();

                            cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND  YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec AND iTrangThai = 1 AND sTinhChat='2'");
                            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                            dTongGiam = Convert.ToDouble(Connection.GetValue(cmd, 0));
                            cmd.Dispose();

                            cmd = new SqlCommand("SELECT TOP 1 rNamKhauHao FROM KTCS_ChungTuChiTiet WHERE iID_MaTaiSan=@iID_MaTaiSan AND  YEAR(dThoiGianDuaVaoSuDung)=@iNamLamViec AND iTrangThai = 1 ORDER BY iThang DESC, iNgay DESC");
                            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
                            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                            Double iNamKhauHaoCSDL = Convert.ToDouble(Connection.GetValue(cmd, -1));
                            cmd.Dispose();
                            if (iNamKhauHaoCSDL > 0)
                            {
                                dSoNamKhauHao = iNamKhauHaoCSDL;
                            }

                            Double dNguyenGiaMoi = 0;
                            Double dMucHaoMonNam = 0;
                            Double dSoNamHaoMonMoi = 0;
                            Double dSoHaoMonLuyKe = 0;
                            Double dGiaTriConLaiTruocKhauHao = 0;
                            Double dGiaTriConLai = 0;

                            dNguyenGiaBanDau = dNguyenGiaBanDau + (dTongTang - dTongGiam);

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

                            ThemVaoBangKhauHaoTaiSanNam(iID_MaTaiSan, iNam, dNguyenGiaBanDau, dTongTang, dTongGiam, dMucHaoMonNam, dSoHaoMonLuyKe, dGiaTriConLaiTruocKhauHao, dGiaTriConLai, dSoNamHaoMonMoi, MaND, sIP);
                        }
                    }
                    dt.Dispose();
                }
            }
        }

        public static void ThemVaoBangKhauHaoTaiSanNam(String iID_MaTaiSan, int iNam, Double rNguyenGia
            , Double rSoTienTang, Double rSoTienGiam, Double rSoTienKhauHao, Double rSoTienKhaoHao_LuyKe
            , Double rGiaTriConLaiTruocKhauHao, Double rGiaTriConLai, Double iSoNamKhaoHao, String MaND, String sIP)
        {
            Bang bang = new Bang("KTCS_KhauHaoHangNam");
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNam);
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

        public static DataTable Get_Table_TinhHaoMonTaiSan(int iNam) {
            DataTable vR;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT * FROM KTCS_KhauHaoHangNam WHERE iTrangThai = 1 AND iNamLamViec = @iNamLamViec");
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static DataTable Get_Table_TinhHaoMonTaiSan_LichSu(int iNam, String iID_MaTaiSan)
        {
            DataTable vR;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT * FROM KTCS_KhauHaoHangNam WHERE iTrangThai = 1 AND iID_MaTaiSan=@iID_MaTaiSan AND iNamLamViec <= @iNamLamViec ORDER BY iNamLamViec ASC");
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", iID_MaTaiSan);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
    }
}