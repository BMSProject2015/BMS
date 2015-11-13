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
    public class PhanBo_DotPhanBoModels
    {
        public static NameValueCollection LayThongTin(String MaDotPhanBo)
        {
            NameValueCollection Data = new NameValueCollection();
            String SQL = "SELECT * FROM PB_DotPhanBo WHERE iTrangThai = 1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
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

        public static DataTable GetDotPhanBo(String MaDotPhanBo, String SoChungTu = "", String TuNgay = "", String DenNgay = "", String iID_MaTrangThaiDuyet = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo";
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);

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

            String SQL = String.Format("SELECT * FROM DT_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayChungTu DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int GetDotPhanBo_Count(String MaDotPhanBo, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo";
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
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

        public static DataTable GetDotPhanBo(String MaDotPhanBo)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM PB_DotPhanBo WHERE iTrangThai = 1 AND iID_MaDotPhanBo=@iID_MaDotPhanBo");
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static DataTable DT_DotPhanBo()
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM PB_DotPhanBo WHERE iTrangThai = 1");
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }

        public static void UpdateDotPhanBo(String MaDotPhanBo, String NgayDotPhanBo)
        {
            SqlCommand cmd = new SqlCommand("UPDATE PB_DotPhanBo SET dNgayDotPhanBo = @dNgayDotPhanBo WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo");
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
            cmd.Parameters.AddWithValue("@dNgayDotPhanBo", CommonFunction.LayNgayTuXau(NgayDotPhanBo));
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static Boolean UpdateRecord(String iID_MaDotPhanBo, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("PB_DotPhanBo");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaDotPhanBo;
            bang.DuLieuMoi = false;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.Save();
            return false;
        }

        public static int Delete_DotPhanBo(String MaDotPhanBo, String IPSua, String MaNguoiDungSua)
        {
            int vR = 0;
            try
            {
                SqlCommand cmd;
                //Xóa dữ liệu trong bảng DT_ChungTuChiTiet
                cmd = new SqlCommand("DELETE FROM PB_PhanBoChiTiet WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo");
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                //Xóa dữ liệu trong bảng DT_ChungTu                
                cmd = new SqlCommand("DELETE FROM PB_PhanBo WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo");
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                //Xóa dữ liệu trong bảng DT_ChungTuChiTiet
                cmd = new SqlCommand("DELETE FROM PB_ChiTieuChiTiet WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo");
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                //Xóa dữ liệu trong bảng DT_ChungTu                
                cmd = new SqlCommand("DELETE FROM PB_ChiTieu WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo");
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                //Xóa dữ liệu trong bảng PB_DotPhanBo
                Bang bang = new Bang("PB_DotPhanBo");
                bang.MaNguoiDungSua = MaNguoiDungSua;
                bang.IPSua = IPSua;
                bang.GiaTriKhoa = MaDotPhanBo;
                bang.Delete();
                vR = 1;
            }
            catch
            {
                vR = 0;
            }
            return vR;
        }
    }
}