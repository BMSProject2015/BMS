using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel;
using System.Web.Script.Serialization;

namespace VIETTEL.Controllers.Shared
{
    public class NhapNhanhController : Controller
    {
        //
        // GET: /NhapNhanh/

        [AcceptVerbs(HttpVerbs.Post)]
        public JavaScriptResult Index(string id, string OnSuccess, string OnLoad)
        {
            // Điền dữ liệu vào bảng
            ViewData["ControlID"] = id;
            ViewData["OnSuccess"] = OnSuccess;

            switch (id.ToUpper())
            {
                case "COTMAU":
                    ViewData["Partial_View"] = "~/Views/CotMau/Dialog.aspx";
                    break;
                case "HANGMAU":
                    ViewData["Partial_View"] = "~/Views/HangMau/Dialog.aspx";
                    break;
                case "BANG_CHITIEU":
                    ViewData["Partial_View"] = "~/Views/Bang/Dialog.aspx";
                    break;
                case "BANGMAU_COTMAU_SUATEN":
                    ViewData["Partial_View"] = "~/Views/BangMauCotMauDonViTenMoi/Dialog.aspx";
                    break;
                case "BANGLUONGCHITIET":
                    ViewData["Partial_View"] = "~/Views/Luong/BangLuongChiTiet/Luong_BangLuongChiTiet_Dialog.aspx";
                    break;

                case "CAPPHAT_THONGTRI":
                    ViewData["Partial_View"] = "~/Views/CapPhat/ChungTu/CapPhat_ThongTri_Dialog.aspx";
                    break;
                case "CHUNGTU_THEMMOI":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTu/ChungTu_Dialog.aspx";
                    break;
                case "CHITIETVATTU":
                    ViewData["Partial_View"] = "~/Views/MaVatTu/Dialog.aspx";
                    break;
                case "CHITIETTAISAN":
                    ViewData["Partial_View"] = "~/Views/CongSan/ChiTietTaiSan/Dat_ChiTietTaiSan.aspx";
                    break;
                case "KTTH_NHANRUTDUTOAN":
                    ViewData["Partial_View"] = "~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_NhanRutDuToan_Dialog.aspx";
                    break;
                case "KTTH_NHANUYNHIEMCHI":
                    ViewData["Partial_View"] = "~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_NhanUyNhiemChi_Dialog.aspx";
                    break;
                case "KTTH_NHANPHIEUTHUCHI":
                    ViewData["Partial_View"] = "~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_NhanPhieuThuPhieuChi_Dialog.aspx";
                    break;
                case "KTTH_NHANQUYETTOAN":
                    ViewData["Partial_View"] = "~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_NhanQuyetToan_Dialog.aspx";
                    break;

                case "KTTH_LICHSUCHUNGTU":
                    ViewData["Partial_View"] = "~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_ChungTu_LichSu.aspx";
                    break;

                case "KTTH_NHANCONGSAN":
                    ViewData["Partial_View"] = "~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_NhanCongSan_Dialog.aspx";
                    break;
                case "KTTH_THEMCHUNGTU":
                    ViewData["Partial_View"] = "~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_ThemChungTu_Dialog.aspx";
                    break;
                case "KTTH_CHITIETCHUNGTUCHOSOTAIKHOAN":
                    ViewData["Partial_View"] = "~/Views/KeToanTongHop/KhoaSoKeToan/KeToanTongHop_KhoaSoKeToan_Dialog.aspx";
                    break;

                case "KTTH_THCTGS":
                    ViewData["Partial_View"] = "~/Report_Views/KeToan/TongHop/rptBangTongHopChungTuGoc.aspx";
                    break;
                case "KTTH_THONGTRI":
                    ViewData["Partial_View"] = "~/Report_Views/KeToan/TongHop/rptKTTH_ThongTri.aspx";
                    break;
                case "KTTH_TAOCTGS":
                    ViewData["Partial_View"] = "~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_ChungTu_Dialog.aspx";
                    break;
                case "KTTH_THONGTRINEW":
                    ViewData["Partial_View"] = "~/Report_Views/KeToan/TongHop/rptKTTH_ThongTri_PhongBan.aspx";
                    break;
                //KE TOAN CHI TIET
                case "KTCT_PHIEUTHU":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/TienMat/ChungTu/KTCT_TienMat_ChungTuPhieuThu_Dialog.aspx";
                    break;
                case "KTCT_PHIEUCHI":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/TienMat/ChungTu/KTCT_TienMat_ChungTuPhieuChi_Dialog.aspx";
                    break;
                case "KTTH_TIENMAT_LICHSUCHUNGTU":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/TienMat/KeToanChiTiet_TienMat_ChungTu_LichSu.aspx";
                    break;
                case "KTTH_TIENGUI_LICHSUCHUNGTU":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/TienGui/KeToanChiTiet_TienGui_ChungTu_LichSu.aspx";
                    break;
                case "KTTH_KHOBAC_LICHSUCHUNGTU":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/KhoBac/KeToanChiTiet_KhoBac_ChungTu_LichSu.aspx";
                    break;
                ///
                case "KTCT_UYNHIEMCHI":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/TienGui/ChungTuChiTiet/KTCT_TienGui_ChungTu_UyNhiemChi_Dialog.aspx";
                    break;
                case "KTCT_GIAYRUTTIEN":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/KhoBac/ChungTuChiTiet/KTCT_KhoBac_ChungTuChiTiet_RutDuToan_Dialog.aspx";
                    break;

                case "KTCT_TTCHUYENTIEN":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/KhoBac/ChungTuChiTiet/KTCT_KhoBac_ThongTriChuyenTien_Dialog.aspx";
                    break;
                case "KTCT_TTLOAI":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/KhoBac/ChungTuChiTiet/KTCT_ChungTu_ThongTriLoai_Dialog.aspx";
                    break;

                case "KTCT_TTTONGHOP":
                    ViewData["Partial_View"] = "~/Report_Views/KeToan/TongHop/rptKTTH_ThongTri_PhongBan.aspx";
                    break;
                case "KTCT_TTLOAINS":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/KhoBac/ChungTuChiTiet/KTCT_ChungTuChiTiet_ThongTriLoaiNS_Dialog.aspx";
                    break;

                case "LUONGPHUCAP":
                    ViewData["Partial_View"] = "~/Views/Luong/BangLuongChiTiet/Luong_PhuCap_Dialog.aspx";
                    break;

                case "NS_QUATRINHDAOTAO":
                    ViewData["Partial_View"] = "~/Views/CanBo/Pages/CanBo_DaoTaoBoiDuong_Dialog.aspx";
                    break;
                case "NS_QUATRINHCONGTAC":
                    ViewData["Partial_View"] = "~/Views/CanBo/Pages/CanBo_QuaTrinhCongTacDialog.aspx";
                    break;
                case "NS_NGUOIPHUTHUOC":
                    ViewData["Partial_View"] = "~/Views/CanBo/Pages/CanBo_NguoiPhuThuocDialog.aspx";
                    break;
                case "NS_DINUOCNGOAI":
                    ViewData["Partial_View"] = "~/Views/CanBo/Pages/CanBo_DiNuocNgoaiDialog.aspx";
                    break;

                case "NS_KHENTHUONG":
                    ViewData["Partial_View"] = "~/Views/CanBo/Pages/CanBo_KhenThuongDialog.aspx";
                    break;
                case "NS_KYLUAT":
                    ViewData["Partial_View"] = "~/Views/CanBo/Pages/CanBo_KyLuatDialog.aspx";
                    break;

                case "SP_DANHMUCGIA":
                    ViewData["Partial_View"] = "~/Views/SanPham/DanhMucGia/DanhMuc_Dialog.aspx";
                    break;
                case "SP_DANHMUCGIA_VT":
                    ViewData["Partial_View"] = "~/Views/SanPham/DanhMucGia/ChonVatTu_Dialog.aspx";
                    break;
                case "TONKHOVATTU":
                    ViewData["Partial_View"] = "~/Views/SanPham/TonKhoVatTu/Dialog.aspx";
                    break;

                ///TIEN GUI
                case "KT_TIENGUI_THONGTRI":
                    ViewData["Partial_View"] = "~/Report_Views/KeToan/TienGui/rptKTTG_ThongTri.aspx";
                    break;
                case "KT_TIENGUI_TONGHOP_NGAY":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/KhoBac/ChungTuChiTiet/KTCT_KhoBac_ThongTriChuyenTien_Dialog.aspx";
                    break;
                case "KT_TIENGUI_TONGHOP_CTGS":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/TienGui/ChungTuChiTiet/KTCT_TienGui_ChungTu_TongHopCTGS_Dialog.aspx";
                    break;
                case "KT_TIENMAT_TONGHOP_CTGS":
                    ViewData["Partial_View"] = "~/Report_Views/KeToan/TienMat/rptKTTienMatTongHopCTGS.aspx";
                    break;
                case "KT_TIENGUI_KIEMTRASOLIEUUNC":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/TienGui/ChungTuChiTiet/KTCT_TienGui_ChungTu_KiemTraSoLieuUNC_Dialog.aspx";
                    break;
                case "KTTH_TIENMAT_THONGTRI":
                    ViewData["Partial_View"] = "~/Report_Views/KeToan/TienMat/rptKTTM_ThongTri.aspx";
                    break;
                case "KTTH_TIENMAT_TONGHOP_NGAY":
                    ViewData["Partial_View"] = "~/Report_Views/KeToan/TienMat/rptKTTM_TongHopNgay.aspx";
                    break;
                case "KTTM_KIEMTRASOLIEU":
                    ViewData["Partial_View"] = "~/Views/KeToanChiTiet/TienMat/ChungTuChiTiet/KTTM_KiemTraSoLieu_Dialog.aspx";
                    break;
                //QLDA
                case "QLDA_THONGTRI_CP":
                    ViewData["Partial_View"] = "~/Views/QLDA/CapPhat/rptQLDA_ThongTri_CP.aspx";
                    break;
                case "QLDA_THONGTRI_CP_1":
                    ViewData["Partial_View"] = "~/Views/QLDA/CapPhat/rptQLDA_ThongTri_CP_1.aspx";
                    break;
                //Quyet toan
                case "QUYETTOAN_TUYCHINH":
                    ViewData["Partial_View"] = "~/Views/QuyetToan/ChungTuChiTiet/QuyetToan_TuyChinh.aspx";
                    break;
                case "QUYETTOAN_BAOCAOINKIEM":
                    ViewData["Partial_View"] = "~/Views/QuyetToan/ChungTuChiTiet/QuyetToan_BaoCaoInKiem.aspx";
                    break;
                case "QUYETTOAN_BAOCAOTHONGTRI":
                    ViewData["Partial_View"] = "~/Views/QuyetToan/ChungTuChiTiet/QuyetToan_BaoCaoThongTri.aspx";
                    break;
                //Du Toan
                case "DUTOAN_TRINHDUYETCHITIET":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_TrinhDuyetChiTiet.aspx";
                    break;
                case "DUTOAN_TUCHOICHITIET":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_TuChoiChiTiet.aspx";
                    break;
                case "DUTOAN_TRINHDUYETCHUNGTU":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_TrinhDuyetChungTu.aspx";
                    break;
                case "DUTOAN_TUCHOICHUNGTU":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_TuChoiChungTu.aspx";
                    break;
                case "DUTOAN_TRINHDUYETCHUNGTU_GOM":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_TrinhDuyetChungTu_Gom.aspx";
                    break;
                case "DUTOAN_TUCHOICHUNGTU_GOM":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_TuChoiChungTu_Gom.aspx";
                    break;
                case "DUTOAN_TRINHDUYETCHUNGTU_GOM_THCUC":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_TrinhDuyetChungTu_Gom_THCuc.aspx";
                    break;
                case "DUTOAN_TUCHOICHUNGTU_GOM_THCUC":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_TuChoiChungTu_Gom_THCuc.aspx";
                    break;
                case "DUTOAN_TUYCHINH":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_TuyChinh.aspx";
                    break;
                case "DUTOAN_PHANCAP_TUYCHINH":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_PhanCap_TuyChinh.aspx";
                    break;
                case "DUTOAN_BAOCAOINKIEM":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_BaoCaoInKiem.aspx";
                    break;
                case "DUTOAN_NHAPEXCEL":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTuChiTiet/DuToan_Nhapexcel.aspx";
                    break;
                // Dự toán bổ sung
                case "DUTOANBS_TRINHDUYETCHUNGTU":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_TrinhDuyetChungTu.aspx";
                    break;
                case "DUTOANBS_TUCHOICHUNGTU":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_TuChoiChungTu.aspx";
                    break;
                case "DUTOANBS_BAOCAOINKIEM_GOM":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_BaoCaoInKiem_gom.aspx";
                    break;
                case "DUTOANBS_TRINHDUYETCHUNGTU_GOM":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_TrinhDuyetChungTu_Gom.aspx";
                    break;
                case "DUTOANBS_TUCHOICHUNGTU_GOM":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_TuChoiChungTu_Gom.aspx";
                    break;
                case "DUTOANBS_NHAPEXCEL":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_Nhapexcel.aspx";
                    break;
                case "DUTOANBS_TUCHOICHITIET":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_TuChoiChiTiet.aspx";
                    break;
                case "DUTOANBS_TRINHDUYETCHITIET":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_TrinhDuyetChiTiet.aspx";
                    break;
                case "DUTOANBS_BAOCAOINKIEM":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_BaoCaoInKiem.aspx";
                    break;
                case "DUTOANBS_TUYCHINH":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_TuyChinh.aspx";
                    break;
                case "DUTOANBS_TRINHDUYETCHUNGTU_GOM_THCUC":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_TrinhDuyetChungTu_Gom_THCuc.aspx";
                    break;
                case "DUTOANBS_TUCHOICHUNGTU_GOM_THCUC":
                    ViewData["Partial_View"] = "~/Views/DuToanBS/ChungTuChiTiet/DuToan_TuChoiChungTu_Gom_THCuc.aspx";
                    break;
                // Tiêu đề báo cáo phân bổ 19
                case "PB_19":
                    ViewData["Partial_View"] = "~/Report_Views/PhanBo/rptPhanBo_19P_dialog.aspx";
                    break;
                case "TRANGTHAI_TAISAN":
                    ViewData["Partial_View"] = "~/Views/CongSan/TaiSan/KTCS_TaiSan_TrangThai_Dialog.aspx";
                    break;
                case "THUNOP_THONGTRI":
                    ViewData["Partial_View"] = "~/Views/ThuNop/rptThuNop_ThongTri.aspx";
                    break;
                //TCDN
                case "TCDN_DUANDANGDAUTU":
                    ViewData["Partial_View"] = "~/Views/TCDN/DoanhNghiep/DuAnDangDauTu/TCDN_DuAnDangDauTu_dialog.aspx";
                    break;
                case "TCDN_CONGTYLDLK":
                    ViewData["Partial_View"] = "~/Views/TCDN/DoanhNghiep/CongTyLDLK/TCDN_CongTyLDLK_dialog.aspx";
                    break;
                case "TCDN_DONVITHANHVIEN":
                    ViewData["Partial_View"] = "~/Views/TCDN/DoanhNghiep/DonViThanhVien/TCDN_DonViThanhVien_dialog.aspx";
                    break;
                case "TCDN_LINHVUC":
                    ViewData["Partial_View"] = "~/Views/TCDN/DoanhNghiep/LinhVuc/TCDN_LinhVuc_dialog.aspx";
                    break;
            }
            //return CommonFunction.RenderPartialViewToString("~/Views/shared/HoTro/NhapNhanh.ascx", this);
            String tg = CommonFunction.RenderPartialViewToString("~/Views/Shared/NhapNhanh.ascx", this);
            String strJ = "";
            BocTachDuLieu(ref tg, ref strJ);
            tg = tg.Trim();
            strJ = strJ.Trim();
            if (String.IsNullOrEmpty(OnLoad) == false)
            {
                strJ = JavaScriptEncode(strJ);
                strJ = String.Format("{0}({1});ImportJavascript({2});", OnLoad, JavaScriptEncode(tg), strJ);
                //strJ = "alert(\"1\");";
            }
            if (strJ == "")
            {
                return null;
            }
            return JavaScript(strJ);
        }
        /// <summary>
        /// Hien thi Dialog
        /// </summary>
        /// <param name="id"></param>
        /// <param name="OnSuccess"></param>
        /// <param name="OnLoad"></param>
        /// <param name="LoaiTS"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JavaScriptResult Edit(string id, string OnSuccess, string OnLoad, int LoaiTS)
        {
            // Điền dữ liệu vào bảng
            ViewData["ControlID"] = id;
            ViewData["OnSuccess"] = OnSuccess;

            switch (id.ToUpper())
            {
                case "COTMAU":
                    ViewData["Partial_View"] = "~/Views/CotMau/Dialog.aspx";
                    break;
                case "HANGMAU":
                    ViewData["Partial_View"] = "~/Views/HangMau/Dialog.aspx";
                    break;
                case "BANG_CHITIEU":
                    ViewData["Partial_View"] = "~/Views/Bang/Dialog.aspx";
                    break;
                case "BANGMAU_COTMAU_SUATEN":
                    ViewData["Partial_View"] = "~/Views/BangMauCotMauDonViTenMoi/Dialog.aspx";
                    break;
                case "CHUNGTU_THEMMOI":
                    ViewData["Partial_View"] = "~/Views/DuToan/ChungTu/ChungTu_Dialog.aspx";
                    break;
                case "CHITIETVATTU":
                    ViewData["Partial_View"] = "~/Views/MaVatTu/Dialog.aspx";
                    break;
                //case "CHITIETTAISAN":
                //    if (LoaiTS == 1)
                //    {
                //        ViewData["Partial_View"] = "~/Views/CongSan/ChiTietTaiSan/Dat_ChiTietTaiSan.aspx";
                //    }
                //    else if (LoaiTS == 2)
                //    {
                //        ViewData["Partial_View"] = "~/Views/CongSan/ChiTietTaiSan/Nha_ChiTietTaiSan.aspx";
                //    }
                //    else if (LoaiTS == 3)
                //    {
                //        ViewData["Partial_View"] = "~/Views/CongSan/ChiTietTaiSan/OTo_ChiTiet.aspx";
                //    }
                //    else if (LoaiTS == 4)
                //    {
                //        ViewData["Partial_View"] = "~/Views/CongSan/ChiTietTaiSan/500_ChiTiet.aspx";
                //    }
                //    break;
            }
            //return CommonFunction.RenderPartialViewToString("~/Views/shared/HoTro/NhapNhanh.ascx", this);
            String tg = CommonFunction.RenderPartialViewToString("~/Views/Shared/NhapNhanh.ascx", this);
            String strJ = "";
            BocTachDuLieu(ref tg, ref strJ);
            tg = tg.Trim();
            strJ = strJ.Trim();
            if (String.IsNullOrEmpty(OnLoad) == false)
            {
                strJ = JavaScriptEncode(strJ);
                strJ = String.Format("{0}({1});ImportJavascript({2});", OnLoad, JavaScriptEncode(tg), strJ);
                //strJ = "alert(\"1\");";
            }
            if (strJ == "")
            {
                return null;
            }
            return JavaScript(strJ);
        }
        private static string JavaScriptEncode(string str)
        {
            // Encode certain characters, or the JavaScript expression could be invalid

            return new JavaScriptSerializer().Serialize(str);
            //return "";
        }

        public void BocTachDuLieu(ref string str1, ref string str2)
        {
            Boolean ok = true;
            str1 = str1.Replace("\r", "");
            str1 = str1.Replace("\n", "");

            while (ok)
            {
                int cs1 = str1.IndexOf("<script");

                if (cs1 >= 0)
                {
                    int cs2 = str1.IndexOf("</script>");
                    string tg = str1.Substring(cs1, cs2 - cs1 + 9);
                    str1 = str1.Remove(cs1, cs2 - cs1 + 9);
                    cs1 = tg.IndexOf(">");
                    tg = tg.Substring(cs1 + 1, tg.Length - cs1 - 10);
                    //tg = tg.Replace("\r", "");
                    //tg = tg.Replace("\n", "");
                    str2 += tg;
                }
                else
                {
                    ok = false;
                }
            }
        }

    }
}
