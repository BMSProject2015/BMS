using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.Sql;
using System.Data.SqlClient;

namespace VIETTEL.Models.DuToanBS
{
    public class DuToanBSModels
    {
        public static int iID_MaPhanHeQuyetToan = PhanHeModels.iID_MaPhanHeQuyetToan;
        public const int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeDuToan;
        public static string strDSTruongTienTieuDe = "Tên công trình,Ngày,Người,Chi tại kho bạc,Tồn kho,Tự chi,Chi tập trung,Hàng nhập ,Hàng mua,Hiện vật,Dự phòng,Phân cấp";
        public static string strDSTruongTienTieuDe_ThuChi ="Tên công trình,Ngày,Người,Chi tại kho bạc,Tồn kho,Số tiền,Chi tập trung,Hàng nhập ,Hàng mua,Hiện vật,Dự phòng,Phân cấp";
        public static string strDSTruongTien_So ="rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap";
        public static string strDSTruongTien_Xau = "sTenCongTrinh";
        public static string strDSTruongTien = strDSTruongTien_Xau + "," + strDSTruongTien_So;
        public static string strDSDuocNhapTruongTien = "b" + strDSTruongTien.Replace(",", ",b");
        public static string strDSTruongTienDoRong_So = "130,130,130,130,130,130,130,130,130,130,130";
        public static string strDSTruongTienDoRong_Xau = "150";
        public static string strDSTruongTienDoRong = strDSTruongTienDoRong_Xau + "," + strDSTruongTienDoRong_So;
        public static string strDSTruongTien_Full = strDSTruongTien;
        public static string strDSTruongTienDoRong_Full = strDSTruongTienDoRong;
        public static string strDSTruongTienTieuDe_Full = strDSTruongTienTieuDe;
    }

}