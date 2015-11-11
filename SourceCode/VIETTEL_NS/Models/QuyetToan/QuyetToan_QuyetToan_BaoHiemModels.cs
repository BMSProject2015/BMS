using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class QuyetToan_QuyetToan_BaoHiemModels
    {
        
        /// <summary>
        /// Lấy DataTable 
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable Get_QuyetToanBaoHiem(String iID_MaChungTu)
        {
            DataTable dt= QuyetToan_ChungTuModels.GetChungTu(iID_MaChungTu);
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM QTA_QuyetToanBaoHiem WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iThang_Quy=@iThang_Quy  AND iID_MaDonVi=@iID_MaDonVi ORDER By sKyHieuDoiTuong");
            cmd.Parameters.AddWithValue("@iID_MaDonVi", dt.Rows[0]["iID_MaDonVi"]);
            cmd.Parameters.AddWithValue("@iThang_Quy", dt.Rows[0]["iThang_Quy"]);
            cmd.Parameters.AddWithValue("@iNamLamViec", dt.Rows[0]["iNamLamViec"]);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            dt.Dispose();
            return vR;
        }
       
    }
}