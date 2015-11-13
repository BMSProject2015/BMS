using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.Sql;
using System.Data.SqlClient;

namespace VIETTEL.Models
{
    public class DuToanBSListModels
    {
        
    }

    public class DuToanBSModels
    {
        public static int iID_MaPhanHeQuyetToan = PhanHeModels.iID_MaPhanHeQuyetToan;
        public const int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeDuToan;
        public static String strDSTruongTienTieuDe =
            "Tên công trình,Ngày,Người,Chi tại kho bạc,Tồn kho,Tự chi,Chi tập trung,Hàng nhập ,Hàng mua,Hiện vật,Dự phòng,Phân cấp";

        public static String strDSTruongTienTieuDe_ThuChi =
            "Tên công trình,Ngày,Người,Chi tại kho bạc,Tồn kho,Số tiền,Chi tập trung,Hàng nhập ,Hàng mua,Hiện vật,Dự phòng,Phân cấp";

        public static String strDSTruongTien_So =
            "rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap";

        public static String strDSTruongTien_Xau = "sTenCongTrinh";
        public static String strDSTruongTien = strDSTruongTien_Xau + "," + strDSTruongTien_So;
        public static String strDSDuocNhapTruongTien = "b" + strDSTruongTien.Replace(",", ",b");

        public static String strDSTruongTienDoRong_So = "130,130,130,130,130,130,130,130,130,130,130";
        public static String strDSTruongTienDoRong_Xau = "150";
        public static String strDSTruongTienDoRong = strDSTruongTienDoRong_Xau + "," + strDSTruongTienDoRong_So;

        public static String strDSTruongTien_Full = strDSTruongTien;
        public static String strDSTruongTienDoRong_Full = strDSTruongTienDoRong;
        public static String strDSTruongTienTieuDe_Full = strDSTruongTienTieuDe;

        public static DataTable getDSPhongBan(String iNamLamViec, String MaND)
        {
            String sTenPB = NguoiDung_PhongBanModels.getTenPhongBan_NguoiDung(MaND);
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            if (sTenPB == "02" || sTenPB == "2")
            {

            }
            else
            {
                DK = " AND iID_MaPhongBan=@iID_MaPhongBan";
            }
            
            String SQL = String.Format(@"SELECT DISTINCT iID_MaPhongBan,sTenPhongBan
FROM DT_ChungTuChiTiet
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} AND iID_MaPhongBan NOT IN (02)
", DK);
            
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", sTenPB);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            DataRow dr = dt.NewRow();
            dr["iID_MaPhongBan"] = "-1";
            dr["sTenPhongBan"] = "--Chọn tất cả các B--";

            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        //lấy danh sách đợt
        public static DataTable getDSDot(String iNamLamViec, String MaND)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            dt.Columns.Add("MaDot", typeof(String));
            dt.Columns.Add("iDotCap", typeof(String));
            DataRow dr = dt.NewRow();
            String SQL = String.Format(@"Select DISTINCT CONVERT(VARCHAR(24),dNgayChungTu,105) as iDotCap,dNgayChungTu
FROM DT_ChungTu
WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec
    AND iID_MaChungTu IN (
                        SELECT iID_MaChungTu FROM DT_ChungTuChiTiet WHERE iTrangThai=1 AND rTuChi<>0 AND iID_MaDonVi<>''
) 
ORDER BY dNgayChungTu
");
            
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            //cmd.Parameters.AddWithValue("");
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }

}