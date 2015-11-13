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
    public class TCDN_BaoCaoTaiChinhModels
    {
        public static void ThemChiTiet(String iLoaiDN,String iQuy, String iNam, String MaND, String IPSua)
        {
            int i, j, k;
            DataTable dt = DT_DanhMucDoanhNghiep(iLoaiDN);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

            Bang bang = new Bang("TCDN_BaoCaoTaiChinh");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iQuy", iQuy);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNam);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtCauHinh.Rows[0]["iID_MaNamNganSach"]);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                //Thêm thông tin vào bảng TCDN_ChungTuChiTiet
                ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(dt.Rows[i], bang.CmdParams.Parameters);
                bang.Save();
            }
            dt.Dispose();

            //Thêm dữ liệu cho các trường khác            
            DataTable dtDanhSachBCTC = LayDanhSachBaoCaoTaiChinh(iNam, iQuy);
            DataRow R;
            for (i = 0; i < dtDanhSachBCTC.Rows.Count; i++)
            {
                R = dtDanhSachBCTC.Rows[i];

                //Update vốn điều lệ và vốn nhà nước
                Double rVongDieuLe = 0, rVonNhaNuoc = 0, rTyLe = 0;
                Double rTongVon_ChuSoHuu = 0;
                Double rVonDauTu_ChuSoHuu = 0;
                Double rThangDu_ChuSoHuu = 0;
                Double rQuyDTPT_ChuSoHuu = 0;
                Double rQuyDPPT_ChuSoHuu = 0;
                Double rChenhLechTyGia_ChuSoHuu = 0;
                Double rLoiNhuanChuaPP_ChuSoHuu = 0;
                Double rVonKhac_ChuSoHuu = 0;
                Double rDoanhThu = 0;
                Double rLoiNhuanTruocThue = 0;
                Double rLoiNhuanSauThue = 0;
                Double rBangTien_VonNhaNuoc = 0;
                Double rBangCoPhieu_VonNhaNuoc = 0;
                Double rCong_VonNhaNuoc = 0;
                Double rNopNganSach = 0;
                Double rLaoDongBinhQuan = 0;
                Double rTongQuyLuong = 0;
                Double rThuNhapBinhQuan = 0;
                Double rPhaiNop_VonNhaNuocKhiCoPhanHoa = 0;
                Double rDaNop_VonNhaNuocKhiCoPhanHoa = 0;
                Double rConPhaiNop_VonNhaNuocKhiCoPhanHoa = 0;
                Double rCoTucNamTruoc_CoTuc = 0;
                Double rCoTucNamNay_CoTuc = 0;
                Double rDaNop_CoTuc = 0;
                Double rConPhaiNop_CoTuc = 0;
                Double rTienDatChuaNop_TienThueDat = 0;
                Double rTienDatNamNay_TienThueDat = 0;
                Double rDaNop_TienThueDat = 0;
                Double rConPhaiNop_TienThueDat = 0;
                Double rTongSoConPhaiNop = 0;
                Double rTongSoDaNop = 0;

                DataTable dtThongTinDN = LayVonDoanhNghiep_KhaiBao(R["iID_MaDoanhNghiep"].ToString());
                if (dtThongTinDN.Rows.Count > 0)
                {
                    for (j = 0; j < dtThongTinDN.Rows.Count; j++)
                    {
                        if (Convert.ToString(dtThongTinDN.Rows[0]["rVonDieuLe"]) != null && Convert.ToString(dtThongTinDN.Rows[0]["rVonDieuLe"]) != "")
                        {
                            rVongDieuLe += Convert.ToDouble(dtThongTinDN.Rows[0]["rVonDieuLe"]);
                        }
                        if (Convert.ToString(dtThongTinDN.Rows[0]["rVonNhaNuoc"]) != null && Convert.ToString(dtThongTinDN.Rows[0]["rVonNhaNuoc"]) != "")
                        {
                            rVonNhaNuoc += Convert.ToDouble(dtThongTinDN.Rows[0]["rVonNhaNuoc"]);
                        }
                    }
                }
                dtThongTinDN.Dispose();
                if (rVonNhaNuoc != 0 && rVonNhaNuoc != 0)
                {
                    rTyLe = rVongDieuLe / rVonNhaNuoc * 100;
                }

                //Lấy thông tin chi tiết hồ sơ doanh nghiệp
                DataTable dtHoSoDoanhNghiepChiTiet = LayDanhSachHoSoDoanhNghiep(R["iID_MaDoanhNghiep"].ToString(), iNam, iQuy);
                for (j = 0; j < dtHoSoDoanhNghiepChiTiet.Rows.Count; j++) {
                    switch (LayTruongDuLieuBaoCaoTaiChinh(Convert.ToString(dtHoSoDoanhNghiepChiTiet.Rows[j]["iID_MaChiTieuHoSo"]))) {
                        case "rTongVon_ChuSoHuu":
                            rTongVon_ChuSoHuu = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rVonDauTu_ChuSoHuu":
                            rVonDauTu_ChuSoHuu = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rThangDu_ChuSoHuu":
                            rThangDu_ChuSoHuu = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rQuyDTPT_ChuSoHuu":
                            rQuyDTPT_ChuSoHuu = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rQuyDPPT_ChuSoHuu":
                            rQuyDPPT_ChuSoHuu = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rChenhLechTyGia_ChuSoHuu":
                            rChenhLechTyGia_ChuSoHuu = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rLoiNhuanChuaPP_ChuSoHuu":
                            rLoiNhuanChuaPP_ChuSoHuu = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rVonKhac_ChuSoHuu":
                            rVonKhac_ChuSoHuu = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rDoanhThu":
                            rDoanhThu = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rLoiNhuanTruocThue":
                            rLoiNhuanTruocThue = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rLoiNhuanSauThue":
                            rLoiNhuanSauThue = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rBangTien_VonNhaNuoc":
                            rBangTien_VonNhaNuoc = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rBangCoPhieu_VonNhaNuoc":
                            rBangCoPhieu_VonNhaNuoc = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rCong_VonNhaNuoc":
                            rCong_VonNhaNuoc = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rNopNganSach":
                            rNopNganSach = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rLaoDongBinhQuan":
                            rLaoDongBinhQuan = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rTongQuyLuong":
                            rTongQuyLuong = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rThuNhapBinhQuan":
                            rThuNhapBinhQuan = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rPhaiNop_VonNhaNuocKhiCoPhanHoa":
                            rPhaiNop_VonNhaNuocKhiCoPhanHoa = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rDaNop_VonNhaNuocKhiCoPhanHoa":
                            rDaNop_VonNhaNuocKhiCoPhanHoa = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rConPhaiNop_VonNhaNuocKhiCoPhanHoa":
                            rConPhaiNop_VonNhaNuocKhiCoPhanHoa = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rCoTucNamTruoc_CoTuc":
                            rCoTucNamTruoc_CoTuc = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rCoTucNamNay_CoTuc":
                            rCoTucNamNay_CoTuc = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rDaNop_CoTuc":
                            rDaNop_CoTuc = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rConPhaiNop_CoTuc":
                            rConPhaiNop_CoTuc = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rTienDatChuaNop_TienThueDat":
                            rTienDatChuaNop_TienThueDat = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rTienDatNamNay_TienThueDat":
                            rTienDatNamNay_TienThueDat = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rDaNop_TienThueDat":
                            rDaNop_TienThueDat = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        case "rConPhaiNop_TienThueDat":
                            rConPhaiNop_TienThueDat = Convert.ToDouble(dtHoSoDoanhNghiepChiTiet.Rows[j]["rNamBaoCao"]);
                            break;
                        default:
                            break;
                    }
                }

                //Lấy thông tin thu nộp của doanh nghiệp
                DataTable dtThuNopDoanhNghiep = LayDanhSachTinhHinhThuNop(R["iID_MaDoanhNghiep"].ToString(), iNam, iQuy);
                for (j = 0; j < dtThuNopDoanhNghiep.Rows.Count; j++)
                {
                    switch (LayTruongDuLieuBaoCaoTaiChinh(Convert.ToString(dtThuNopDoanhNghiep.Rows[j]["iID_MaChiTieuHoSo"])))
                    {
                        case "rPhaiNop_VonNhaNuocKhiCoPhanHoa":
                            rPhaiNop_VonNhaNuocKhiCoPhanHoa = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rDaNop_VonNhaNuocKhiCoPhanHoa":
                            rDaNop_VonNhaNuocKhiCoPhanHoa = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rConPhaiNop_VonNhaNuocKhiCoPhanHoa":
                            rConPhaiNop_VonNhaNuocKhiCoPhanHoa = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rCoTucNamTruoc_CoTuc":
                            rCoTucNamTruoc_CoTuc = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rCoTucNamNay_CoTuc":
                            rCoTucNamNay_CoTuc = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rDaNop_CoTuc":
                            rDaNop_CoTuc = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rConPhaiNop_CoTuc":
                            rConPhaiNop_CoTuc = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rTienDatChuaNop_TienThueDat":
                            rTienDatChuaNop_TienThueDat = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rTienDatNamNay_TienThueDat":
                            rTienDatNamNay_TienThueDat = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rDaNop_TienThueDat":
                            rDaNop_TienThueDat = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        case "rConPhaiNop_TienThueDat":
                            rConPhaiNop_TienThueDat = Convert.ToDouble(dtThuNopDoanhNghiep.Rows[j]["rTrongKyDaNop"]);
                            break;
                        default:
                            break;
                    }
                }

                rTongVon_ChuSoHuu = rVonDauTu_ChuSoHuu + rThangDu_ChuSoHuu + rQuyDTPT_ChuSoHuu + rQuyDPPT_ChuSoHuu + rChenhLechTyGia_ChuSoHuu + rLoiNhuanChuaPP_ChuSoHuu + rVonKhac_ChuSoHuu;
                rCong_VonNhaNuoc = rBangTien_VonNhaNuoc + rBangCoPhieu_VonNhaNuoc;
                rConPhaiNop_VonNhaNuocKhiCoPhanHoa = rPhaiNop_VonNhaNuocKhiCoPhanHoa - rPhaiNop_VonNhaNuocKhiCoPhanHoa;
                rConPhaiNop_CoTuc = rCoTucNamTruoc_CoTuc + rCoTucNamNay_CoTuc - rDaNop_CoTuc;
                rConPhaiNop_TienThueDat = rTienDatChuaNop_TienThueDat + rTienDatNamNay_TienThueDat - rDaNop_TienThueDat;

                //Update thông tin khác trên bảng hồ sơ doanh nghiệp      
                String SQL = "UPDATE TCDN_BaoCaoTaiChinh SET rVongDieuLe=@rVongDieuLe, rVonNhaNuoc=@rVonNhaNuoc, rTyLe=@rTyLe," +
                    "rTongVon_ChuSoHuu=@rTongVon_ChuSoHuu, rVonDauTu_ChuSoHuu=@rVonDauTu_ChuSoHuu, rThangDu_ChuSoHuu=@rThangDu_ChuSoHuu," +
                    "rQuyDTPT_ChuSoHuu=@rQuyDTPT_ChuSoHuu,rQuyDPPT_ChuSoHuu=@rQuyDPPT_ChuSoHuu,rChenhLechTyGia_ChuSoHuu=@rChenhLechTyGia_ChuSoHuu," +
                    "rLoiNhuanChuaPP_ChuSoHuu=@rLoiNhuanChuaPP_ChuSoHuu,rVonKhac_ChuSoHuu=@rVonKhac_ChuSoHuu,rDoanhThu=@rDoanhThu," +
                    "rLoiNhuanTruocThue=@rLoiNhuanTruocThue,rLoiNhuanSauThue=@rLoiNhuanSauThue,rBangTien_VonNhaNuoc=@rBangTien_VonNhaNuoc," +
                    "rBangCoPhieu_VonNhaNuoc=@rBangCoPhieu_VonNhaNuoc,rCong_VonNhaNuoc=@rCong_VonNhaNuoc,rNopNganSach=@rNopNganSach," +
                    "rLaoDongBinhQuan=@rLaoDongBinhQuan,rTongQuyLuong=@rTongQuyLuong,rThuNhapBinhQuan=@rThuNhapBinhQuan," +
                    "rPhaiNop_VonNhaNuocKhiCoPhanHoa=@rPhaiNop_VonNhaNuocKhiCoPhanHoa,rDaNop_VonNhaNuocKhiCoPhanHoa=@rDaNop_VonNhaNuocKhiCoPhanHoa,rConPhaiNop_VonNhaNuocKhiCoPhanHoa=@rConPhaiNop_VonNhaNuocKhiCoPhanHoa," +
                    "rCoTucNamTruoc_CoTuc=@rCoTucNamTruoc_CoTuc,rCoTucNamNay_CoTuc=@rCoTucNamNay_CoTuc,rDaNop_CoTuc=@rDaNop_CoTuc," +
                    "rConPhaiNop_CoTuc=@rConPhaiNop_CoTuc,rTienDatChuaNop_TienThueDat=@rTienDatChuaNop_TienThueDat,rTienDatNamNay_TienThueDat=@rTienDatNamNay_TienThueDat," +
                    "rDaNop_TienThueDat=@rDaNop_TienThueDat,rConPhaiNop_TienThueDat=@rConPhaiNop_TienThueDat,rTongSoConPhaiNop=@rTongSoConPhaiNop,rTongSoDaNop=@rTongSoDaNop " +   
                    "WHERE iID_MaDoanhNghiep=@iID_MaDoanhNghiep AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@rVongDieuLe", rVongDieuLe);
                cmd.Parameters.AddWithValue("@rVonNhaNuoc", rVonNhaNuoc);
                cmd.Parameters.AddWithValue("@rTyLe", rTyLe);
                cmd.Parameters.AddWithValue("@rTongVon_ChuSoHuu", rTongVon_ChuSoHuu);
                cmd.Parameters.AddWithValue("@rVonDauTu_ChuSoHuu", rVonDauTu_ChuSoHuu);
                cmd.Parameters.AddWithValue("@rThangDu_ChuSoHuu", rThangDu_ChuSoHuu);
                cmd.Parameters.AddWithValue("@rQuyDTPT_ChuSoHuu", rQuyDTPT_ChuSoHuu);
                cmd.Parameters.AddWithValue("@rQuyDPPT_ChuSoHuu", rQuyDPPT_ChuSoHuu);
                cmd.Parameters.AddWithValue("@rChenhLechTyGia_ChuSoHuu", rChenhLechTyGia_ChuSoHuu);
                cmd.Parameters.AddWithValue("@rLoiNhuanChuaPP_ChuSoHuu", rLoiNhuanChuaPP_ChuSoHuu);
                cmd.Parameters.AddWithValue("@rVonKhac_ChuSoHuu", rVonKhac_ChuSoHuu);
                cmd.Parameters.AddWithValue("@rDoanhThu", rDoanhThu);
                cmd.Parameters.AddWithValue("@rLoiNhuanTruocThue", rLoiNhuanTruocThue);
                cmd.Parameters.AddWithValue("@rLoiNhuanSauThue", rLoiNhuanSauThue);
                cmd.Parameters.AddWithValue("@rBangTien_VonNhaNuoc", rBangTien_VonNhaNuoc);
                cmd.Parameters.AddWithValue("@rBangCoPhieu_VonNhaNuoc", rBangCoPhieu_VonNhaNuoc);
                cmd.Parameters.AddWithValue("@rCong_VonNhaNuoc", rCong_VonNhaNuoc);
                cmd.Parameters.AddWithValue("@rNopNganSach", rNopNganSach);
                cmd.Parameters.AddWithValue("@rLaoDongBinhQuan", rLaoDongBinhQuan);
                cmd.Parameters.AddWithValue("@rTongQuyLuong", rTongQuyLuong);
                cmd.Parameters.AddWithValue("@rThuNhapBinhQuan", rThuNhapBinhQuan);
                cmd.Parameters.AddWithValue("@rPhaiNop_VonNhaNuocKhiCoPhanHoa", rPhaiNop_VonNhaNuocKhiCoPhanHoa);
                cmd.Parameters.AddWithValue("@rDaNop_VonNhaNuocKhiCoPhanHoa", rDaNop_VonNhaNuocKhiCoPhanHoa);
                cmd.Parameters.AddWithValue("@rConPhaiNop_VonNhaNuocKhiCoPhanHoa", rConPhaiNop_VonNhaNuocKhiCoPhanHoa);
                cmd.Parameters.AddWithValue("@rCoTucNamTruoc_CoTuc", rCoTucNamTruoc_CoTuc);
                cmd.Parameters.AddWithValue("@rCoTucNamNay_CoTuc", rCoTucNamNay_CoTuc);
                cmd.Parameters.AddWithValue("@rDaNop_CoTuc", rDaNop_CoTuc);
                cmd.Parameters.AddWithValue("@rConPhaiNop_CoTuc", rConPhaiNop_CoTuc);
                cmd.Parameters.AddWithValue("@rTienDatChuaNop_TienThueDat", rTienDatChuaNop_TienThueDat);
                cmd.Parameters.AddWithValue("@rTienDatNamNay_TienThueDat", rTienDatNamNay_TienThueDat);
                cmd.Parameters.AddWithValue("@rDaNop_TienThueDat", rDaNop_TienThueDat);
                cmd.Parameters.AddWithValue("@rConPhaiNop_TienThueDat", rConPhaiNop_TienThueDat);
                cmd.Parameters.AddWithValue("@rTongSoConPhaiNop", rTongSoConPhaiNop);
                cmd.Parameters.AddWithValue("@rTongSoDaNop", rTongSoDaNop);
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", R["iID_MaDoanhNghiep"].ToString());
                cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            dtDanhSachBCTC.Dispose();
        }

        public static void ThemThongTinChiTietTaiChinhDoanhNghiepLayTruongTien(DataRow RMucLucChiTieu, SqlParameterCollection Params)
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
        }
        public static DataTable Get_dtBaoCaoChiTiet(String iNam, String iQuy, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);

            SQL = String.Format("SELECT * FROM TCDN_BaoCaoTaiChinh WHERE {0} ORDER BY iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        public static Boolean CheckData(String iQuy, String iNam) {

            Boolean vR = false;

            SqlCommand cmd;
            cmd = new SqlCommand("SELECT COUNT(*) FROM TCDN_BaoCaoTaiChinh WHERE iTrangThai=1 AND iQuy=@iQuy AND iNamLamViec=@iNamLamViec");
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
            cmd.CommandText = "SELECT * FROM TCDN_BaoCaoTaiChinh WHERE iTrangThai = 1 AND iNamLamViec=@iNamLamViec AND iQuy=@iQuy ORDER BY iSTT";
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
            cmd.CommandText = "SELECT iID_MaTruongKhoa FROM TCDN_BaoCaoTaiChinhTruongLayDuLieu_ChiTieuHoSo WHERE iTrangThai = 1 AND iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo";
            cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo);
            vR = Convert.ToString(Connection.GetValue(cmd,""));
            cmd.Dispose();

            return vR;
        }
    }
}