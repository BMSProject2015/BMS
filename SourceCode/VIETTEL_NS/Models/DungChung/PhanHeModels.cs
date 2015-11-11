using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class PhanHeModels
    {
        public const int iID_MaPhanHeDuToan = 1;
        public const int iID_MaPhanHeChiTieu = 2;
        public const int iID_MaPhanHePhanBo = 3;
        public const int iID_MaPhanHeCapPhat = 4;
        public const int iID_MaPhanHeQuyetToan = 5;
        public const int iID_MaPhanHeTinDung = 6;
        public const int iID_MaPhanHeKeToanTongHop = 7;
        public const int iID_MaPhanHeThuNopNganSach = 8;
        public const int iID_MaPhanHeNguoiCoCong = 9;
        public const int iID_MaPhanHeBaoHiem = 10;
        public const int iID_MaPhanHeCongSan = 11;
        public const int iID_MaPhanHeThongKeTCDN = 12;
        public const int iID_MaPhanHeLuong = 13;
        public const int iID_MaPhanHeVonDauTu = 14;
        public const int iID_MaPhanHeGia = 15;
        public const int iID_MaPhanHeNhanSu = 10;
        public const int iID_MaPhanHeKeToanChiTiet = 16;
        public const int iID_MaPhanHeSanPham = 17;
        public const int iID_MaPhanHeTuLieuLichSu = 20;
        public static String LayTienToChungTu(int iID_MaPhanHe){
            String vR = "";
            SqlCommand cmd = new SqlCommand("SELECT sTienToChungTu FROM NS_PhanHe WHERE iID_MaPhanHe = @iID_MaPhanHe");
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", iID_MaPhanHe);
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            return vR;
        }
    }
}