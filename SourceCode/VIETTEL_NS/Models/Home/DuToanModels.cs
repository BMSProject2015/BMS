using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace VIETTEL.Models
{
    public class DuToanModels
    {
        public const int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeDuToan;

        public static String strDSTruongTienTieuDe =
            "Tên công trình,Ngày,Người,Chi tại kho bạc,Tồn kho,Tự chi,Chi tập trung,Hàng nhập ,Hàng mua,Hiện vật,Phân cấp,Dự phòng";

        public static String strDSTruongTienTieuDe_ThuChi =
            "Tên công trình,Ngày,Người,Chi tại kho bạc,Tồn kho,Số tiền,Chi tập trung,Hàng nhập ,Hàng mua,Hiện vật,Phân cấp,Dự phòng";

        public static String strDSTruongTien_So =
            "rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rPhanCap,rDuPhong";

        public static String strDSTruongTien_Xau = "sTenCongTrinh";
        public static String strDSTruongTien = strDSTruongTien_Xau + "," + strDSTruongTien_So;
        public static String strDSDuocNhapTruongTien = "b" + strDSTruongTien.Replace(",", ",b");

        public static String strDSTruongTienDoRong_So = "130,130,130,130,130,130,130,130,130,130,130";
        public static String strDSTruongTienDoRong_Xau = "300";
        public static String strDSTruongTienDoRong = strDSTruongTienDoRong_Xau + "," + strDSTruongTienDoRong_So;

        public static String strDSTruongTien_Full = strDSTruongTien;
        public static String strDSTruongTienDoRong_Full = strDSTruongTienDoRong;
        public static String strDSTruongTienTieuDe_Full = strDSTruongTienTieuDe;
    }
}