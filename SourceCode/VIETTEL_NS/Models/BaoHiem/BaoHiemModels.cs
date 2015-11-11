using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VIETTEL.Models
{
    public class BaoHiemModels
    {
        public static int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeBaoHiem;

        public static String strDSCot = "rKhoiDT,rKhoiDN";
        public static String strDSTruongTien = "rKhoiDT,rKhoiDN";
        public static String strDSTruongTienTieuDe = "Khối Dự toán,Khối Doanh nghiệp";
        public static String strDSTruongTienDoRong = "150,150";

        public static String strDSTruongTieuDe = "Mã Đơn Vị,Đơn Vị";
        public static String strDSTruong = "iID_MaDonVi,sTenDonVi";
        public static String strDSTruongDoRong = "50,150";  
    }
}