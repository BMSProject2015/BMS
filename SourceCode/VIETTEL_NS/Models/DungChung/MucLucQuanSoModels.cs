using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;
namespace VIETTEL.Models
{
    public class MucLucQuanSoModels
    {
        //Danh sách trường của mục lục ngân sách
        public static String strDSTruongTienTieuDe = "Thiếu úy,Trung úy,Thượng úy,Đại úy,Thiếu tá,Trung tá,Thượng tá,Đại tá,Tướng,Thiếu SQ,Binh nhì,Binh nhất,Hạ sỹ,Trung sỹ,Thượng sỹ,QNCN,CNVQP,Lao động HĐ,Ghi chú";
        public static String strDSTruongTien_So = "rThieuUy,rTrungUy,rThuongUy,rDaiUy,rThieuTa,rTrungTa,rThuongTa,rDaiTa,rTuong,rTSQ,rBinhNhi,rBinhNhat,rHaSi,rTrungSi,rThuongSi,rQNCN,rCNVQP,rLDHD";
        public static String strDSTruongTien_Xau = "sGhiChu";
        public static String strDSTruongTien = strDSTruongTien_So + "," + strDSTruongTien_Xau;
        public static String strDSTruongTien_rTongSo = strDSTruongTien_Xau + ",rTongSo";

        public static String strDSTruongTienDoRong = "70,70,70,70,70,70,70,70,70,70,70,70,70,70,70,70,70,70,200";
        public static String strDSDuocNhapTruongTien = "b" + strDSTruongTien.Replace(",", ",b");
        public static String strDSTruongTieuDe = "Ký hiệu,Mô tả";
        public static String strDSTruong = "sKyHieu,sMoTa";
        public static String strDSTruongDoRong = "60,200";

        public static String Get_Mota(String sKyHieu)
        {
            String SQL = String.Format("SELECT sMoTa FROM NS_MucLucQuanSo WHERE sKyHieu='{0}'", sKyHieu);
            return Connection.GetValueString(SQL, "");
        }
    }
}