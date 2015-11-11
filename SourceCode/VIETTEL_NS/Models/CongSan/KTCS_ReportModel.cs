using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;

namespace VIETTEL.Models
{
    public class KTCS_ReportModel
    {
        public static DataTable ListDonVi()
        {
            DataTable dt;
            String SQL = String.Format(@"SELECT DISTINCT NS_DonVi.iID_MaDonVi,sTen, NS_DonVi.iID_MaDonVi + '-' + sTen as TenHT
                                        FROM KTCS_TaiSan_DonVi
                                        INNER JOIN (SELECT iID_MaDonVi,sTen FROM NS_DonVi WHERE iNamLamViec_DonVi=@iNamLamViec) as NS_DonVi 
                                        ON KTCS_TaiSan_DonVi.iID_MaDonVi=NS_DonVi.iID_MaDonVi
                                        ORDER BY NS_DonVi.iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            return dt = Connection.GetDataTable(cmd);
        }
        public static DataTable DT_LoaiTS(Boolean All = false, String TieuDe = "")
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand(@"SELECT iID_MaNhomTaiSan,iID_MaNhomTaiSan +' - '+  sTen AS TenHT
                                                FROM KTCS_NhomTaiSan
                                                WHERE iTrangThai=1
                                                ORDER BY iSTT,iID_MaNhomTaiSan ");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["iID_MaLoaiTaiSan"] = Guid.Empty;
                R["TenHT"] = TieuDe;
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
    }
}