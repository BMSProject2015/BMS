using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
using DomainModel.Controls;
using System.Collections.Specialized;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class DuToan_DotNganSachModels
    {
        public static NameValueCollection LayThongTin(String MaDotNganSach)
        {
            NameValueCollection Data = new NameValueCollection();
            String SQL = "SELECT * FROM DT_DotNganSach WHERE iID_MaDotNganSach=@iID_MaDotNganSach";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDotNganSach", MaDotNganSach);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt != null && dt.Rows.Count > 0)
            {
                String colName = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colName = dt.Columns[i].ColumnName;
                    Data[colName] = Convert.ToString(dt.Rows[0][i]);
                }
            }
            return Data;
        }

        public static DataTable GetDotNganSach(String MaDotNganSach, String SoChungTu = "", String TuNgay = "", String DenNgay = "", String iID_MaTrangThaiDuyet = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 AND iID_MaDotNganSach=@iID_MaDotNganSach";
            cmd.Parameters.AddWithValue("@iID_MaDotNganSach", MaDotNganSach);
        
            if (String.IsNullOrEmpty(SoChungTu) == false && SoChungTu != "") {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayChungTu >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayChungTu <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }

            String SQL = String.Format("SELECT * FROM DT_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayChungTu DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int GetDotNganSach_Count(String MaDotNganSach, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1 AND iID_MaDotNganSach=@iID_MaDotNganSach";
            cmd.Parameters.AddWithValue("@iID_MaDotNganSach", MaDotNganSach);
            if (String.IsNullOrEmpty(SoChungTu) == false && SoChungTu != "")
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayChungTu >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(DenNgay) == false && DenNgay != "")
            {
                DK += " AND dNgayChungTu <= @dDenNgayChungTu";
                cmd.Parameters.AddWithValue("@dDenNgayChungTu", CommonFunction.LayNgayTuXau(DenNgay));
            }

            String SQL = String.Format("SELECT COUNT(*) FROM DT_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable GetDotNganSach(String MaDotNganSach)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM DT_DotNganSach WHERE iID_MaDotNganSach=@iID_MaDotNganSach");
            cmd.Parameters.AddWithValue("@iID_MaDotNganSach", MaDotNganSach);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static void UpdateDotNganSach(String MaDotNganSach, String NgayDotNganSach)
        {
            SqlCommand cmd = new SqlCommand("UPDATE DT_DotNganSach SET dNgayDotNganSach = @dNgayDotNganSach WHERE iID_MaDotNganSach=@iID_MaDotNganSach");
            cmd.Parameters.AddWithValue("@iID_MaDotNganSach", MaDotNganSach);
            cmd.Parameters.AddWithValue("@dNgayDotNganSach", CommonFunction.LayNgayTuXau(NgayDotNganSach));
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static Boolean UpdateRecord(String iID_MaDotNganSach, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("DT_DotNganSach");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaDotNganSach;
            bang.DuLieuMoi = false;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.Save();
            return false;
        }

        public static int Delete_DotNganSach(String MaDotNganSach, String IPSua, String MaNguoiDungSua)
        {
            int vR = 0;
            try {
                if (CheckChungTuDuyetTheoDot(MaDotNganSach) == true)
                {
                    SqlCommand cmd;

                    //Xóa dữ liệu trong bảng DT_ChungTu                
                    cmd = new SqlCommand("UPDATE DT_ChungTu SET iTrangThai = 0 WHERE iID_MaDotNganSach=@iID_MaDotNganSach");
                    cmd.Parameters.AddWithValue("@iID_MaDotNganSach", MaDotNganSach);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();

                    //Xóa dữ liệu trong bảng DT_ChungTuChiTiet                
                    cmd = new SqlCommand("UPDATE DT_ChungTuChiTiet SET iTrangThai = 0 WHERE iID_MaDotNganSach=@iID_MaDotNganSach");
                    cmd.Parameters.AddWithValue("@iID_MaDotNganSach", MaDotNganSach);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();

                    //Xóa dữ liệu trong bảng DT_DotNganSach
                    Bang bang = new Bang("DT_DotNganSach");
                    bang.MaNguoiDungSua = MaNguoiDungSua;
                    bang.IPSua = IPSua;
                    bang.GiaTriKhoa = MaDotNganSach;
                    bang.Delete();
                    vR = 1;
                }
                else
                {
                    vR = -1;
                }
            }
            catch
            {
                vR = 0;
            } 
            return vR;
        }

        public static Boolean CheckChungTuDuyetTheoDot(String MaDotNganSach)
        {
            Boolean vR = true;
            SqlCommand cmd;
            cmd = new SqlCommand("SELECT COUNT(*) FROM DT_ChungTu WHERE iID_MaDotNganSach=@iID_MaDotNganSach AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet");
            cmd.Parameters.AddWithValue("@iID_MaDotNganSach", MaDotNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(DuToanModels.iID_MaPhanHe));
            int rGT = Convert.ToInt32( Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (rGT > 0) {
                vR = false;
            }
            return vR;
        }
    }
}