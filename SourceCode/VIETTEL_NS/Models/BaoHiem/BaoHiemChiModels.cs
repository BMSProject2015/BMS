using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VIETTEL.Models
{
    public class BaoHiemChiModels
    {
        public static String strDSCot = "rSLSQ,rTienSQ,rSLQNCN,rTienQNCN,rSLCNV,rTienCNV,rSLHD,rTienHD,rSLHD_Khac,rTienHD_Khac,rSLHSQ_CS,rTienHSQ_CS";
        public static String strDSTruongTien = "rSLSQ,rTienSQ,rSLQNCN,rTienQNCN,rSLCNV,rTienCNV,rSLHD,rTienHD,rSLHD_Khac,rTienHD_Khac,rSLHSQ_CS,rTienHSQ_CS";
        public static String strDSTruongTienTieuDe = "SL SQ,Tiền SQ,SL QNCN,Tiền QNCN,SL CNV,Tiền CNV,SL HĐ,Tiền HĐ,SL HĐKhac,Tiền HĐ Khác,SL HSQCS,Tiền HSQCS";
        public static String strDSTruongTienDoRong = "100,100,100,100,100,100,100,100,100,100,100,100";

        public static String strDSTruongTieuDe ="Nội dung,Tổng số";// "LNS,L,K,M,TM,TTM,NG,TNG,Nội dung,rTongSo";
        public static String strDSTruong = "sMoTa,rTongSo";//"sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,rTongSo";
        public static String strDSTruongDoRong = "250,120";  //"60,30,30,30,30,30,30,30,200,120";  


        public static String strDSCot_Mua = "rSoNguoi,rSoTien";
        public static String strDSTruongTien_Mua = "rSoNguoi,rSoTien";
        public static String strDSTruongTienTieuDe_Mua = "Số người, Số tiền";
        public static String strDSTruongTienDoRong_Mua = "100,100";

        public static String strDSTruongTieuDe_Mua = "LNS,L,K,M,TM,TTM,NG,TNG,Nội dung";
        public static String strDSTruong_Mua = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
        public static String strDSTruongDoRong_Mua ="60,30,30,30,30,30,30,30,200";

        public static String strDSHangChiTiet = "Trợ cấp ốm đau,Bản thân ốm,Con ốm,Dưỡng sức - phục hồi SK sau ốm,"
                                                 + "Trợ cấp thai sản,Sinh con - nuôi con,Trợ cấp 1 lần,Khám thai,Dưỡng sức - phục hồi SK sau thai sản,"
                                                 + "Tai nạn lao động - bệnh nghề nghiệp,Trợ cấp 1 lần,Trợ cấp hàng tháng,Trợ cấp phục hồi chức năng,Trợ cấp người phục vụ,Trợ cấp chết do TNLD - BNN,Dưỡng sức - PHSK sau TNLD - BNN,"
                                                 + "Hưu trí,Trợ cấp 1 lần,Phục viên,Trợ cấp 1 lần,Xuất ngũ,Trợ cấp 1 lần,Thôi việc,Trợ cấp 1 lần,Tử tuất,Mai táng phí";
        public static String strDSTruong_HangChiTiet = ",rBaoHiemChi_OmDau_BanThanOm,rBaoHiemChi_OmDau_ConOm,rBaoHiemChi_OmDau_DuongSuc,"
                                                 + ",rBaoHiemChi_ThaiSan_SinhCon,rBaoHiemChi_ThaiSan_1Lan,rBaoHiemChi_ThaiSan_KhamThai,rBaoHiemChi_ThaiSan_DuongSuc,"
                                                 + ",rBaoHiemChi_TaiNan_1Lan,rBaoHiemChi_TaiNan_HangThang,rBaoHiemChi_TaiNan_PhucHoi,rBaoHiemChi_TaiNan_NguoiPhucVu,rBaoHiemChi_TaiNan_Chet,rBaoHiemChi_TaiNan_DuongSuc,"
                                                 + ",rBaoHiemChi_HuuTri,,rBaoHiemChi_PhucVien,,rBaoHiemChi_XuatNgu,,rBaoHiemChi_ThoiViec,,rBaoHiemChi_TuTuat";
        public static String strDSLaCha_HangChiTiet = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
    }
}