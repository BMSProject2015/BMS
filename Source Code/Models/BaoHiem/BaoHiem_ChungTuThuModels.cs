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
    public class BaoHiem_ChungTuThuModels
    {
        /// <summary>
        /// Lấy thông tin của một chứng từ quyết toán ngân sách
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTin(String iID_MaChungTuThu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetChungTu(iID_MaChungTuThu);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }

        /// <summary>
        /// Lấy DataTable thông tin của một chứng từ chỉ tiêu phân bổ
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable GetChungTu(String iID_MaChungTuThu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM BH_ChungTuThu WHERE iTrangThai=1 AND iID_MaChungTuThu=@iID_MaChungTuThu");
            cmd.Parameters.AddWithValue("@iID_MaChungTuThu", iID_MaChungTuThu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static int GetMaxChungTu()
        {
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT MAX(iSoChungTu) FROM BH_ChungTuThu WHERE iTrangThai=1");
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Thêm một hàng dữ liệu vào bảng BH_ChungTu
        /// </summary>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="Params"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertRecord(String iID_MaDotPhanBo, SqlParameterCollection Params, String MaND, String IPSua)
        {
            String MaChungTu = "";
            Bang bang = new Bang("BH_ChungTuThu");
            DataTable dtDotPhanBo = PhanBo_DotPhanBoModels.GetDotPhanBo(iID_MaDotPhanBo);
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }            
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtDotPhanBo.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtDotPhanBo.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtDotPhanBo.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeBaoHiem));
            bang.DuLieuMoi = true;
            String MaChungTuAddNew = Convert.ToString(bang.Save());


            return MaChungTu;
        }

        /// <summary>
        /// Cập nhập dữ liệu 1 Record của Chỉ tiêu
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="Params">Params là của cmd.Parameters</param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean UpdateRecord(String iID_MaChungTuThu, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("BH_ChungTuThu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTuThu;
            bang.DuLieuMoi = false;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.Save();
            return false;
        }

        /// <summary>
        /// Xóa dữ liệu chỉ tiêu
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="IPSua"></param>
        /// <param name="MaNguoiDungSua"></param>
        /// <returns></returns>
        public static int Delete_ChungTu(String iID_MaChungTuThu, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng BH_ChungTuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM BH_ChungTuThuChiTiet WHERE iID_MaChungTuThu=@iID_MaChungTuThu");
            cmd.Parameters.AddWithValue("@iID_MaChungTuThu", iID_MaChungTuThu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng BH_ChungTuThu
            Bang bang = new Bang("BH_ChungTuThu");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTuThu;
            bang.Delete();
            return 1;
        }

        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng BH_ChungTu, BH_ChungTuChiTiet
        /// </summary>
        /// <param name="iID_MaChungTuThu"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TrangThaiTrinhDuyet"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaChungTuThu, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            UpdateRecord(iID_MaChungTuThu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng BH_ChungTuChiTiet            
            String SQL = "UPDATE BH_ChungTuThuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTuThu=@iID_MaChungTuThu";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE BH_ChungTuThuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaChungTuThu=@iID_MaChungTuThu";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTuThu", iID_MaChungTuThu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }

        public static String InsertDuyetChungTu(String iID_MaChungTuThu, String NoiDung, String MaND, String IPSua)
        {
            String iID_MaDuyetChungTu;
            Bang bang = new Bang("BH_DuyetChungTuThu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuThu", iID_MaChungTuThu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetChungTu = Convert.ToString(bang.Save());
            return iID_MaDuyetChungTu;
        }

        public static DataTable Get_DanhSachChungTu(String MaND, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeBaoHiem, MaND);
            DK += " AND iTrangThai=1 ";
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
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
            String SQL = String.Format("SELECT * FROM BH_ChungTuThu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayChungTu DESC, iID_MaTrangThaiDuyet", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Count(String MaND, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeBaoHiem, MaND);
            DK += " AND iTrangThai=1 ";
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
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
            String SQL = String.Format("SELECT Count(*) FROM BH_ChungTuThu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}