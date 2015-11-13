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
    public class TCDN_ChungTuModels
    {
        /// <summary>
        /// Lấy thông tin của một chứng từ thu nộp ngân sách
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTin(String iID_MaChungTu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetChungTu(iID_MaChungTu);
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
        /// Lấy DataTable thông tin của một chứng từ thu nộp ngân sách
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable GetChungTu(String iID_MaChungTu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM TCDN_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy số chứng từ lớn nhất trong bảng TCDN_ChungTu
        /// </summary>
        /// <returns></returns>
        public static int GetMaxChungTu()
        {
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT MAX(iSoChungTu) FROM TCDN_ChungTu WHERE iTrangThai=1");
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Thêm một hàng dữ liệu vào bảng TCDN_ChungTu
        /// </summary>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="Params"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertRecord(String iID_MaDotPhanBo, SqlParameterCollection Params, String MaND, String IPSua)
        {
            String MaChungTu = "";
            Bang bang = new Bang("TCDN_ChungTu");
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
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(QuyetToanModels.iID_MaPhanHeQuyetToan));
            bang.DuLieuMoi = true;
            String MaChungTuAddNew = Convert.ToString(bang.Save());

            //Thêm chi tiết chỉ tiêu
            //PhanBo_ChungTuChiTietModels.ThemChiTiet(MaChungTuAddNew, MaND, IPSua);

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
        public static Boolean UpdateRecord(String iID_MaChungTu, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("TCDN_ChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChungTu;
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
        public static int Delete_ChungTu(String iID_MaChungTu, String IPSua, String MaNguoiDungSua)
        {
            SqlCommand cmd;
            //Xóa dữ liệu trong bảng TCDN_ChungTuChiTiet
            //kiem tra du lieu bang TCDN_ChungTuChiTiet
            String SQL = "SELECT COUNT(*) FROM TCDN_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            int check =Convert.ToInt32(Connection.GetValue(cmd, 1));
            if (check == 0)
            {
                cmd = new SqlCommand("DELETE FROM TCDN_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu");
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                //Xóa dữ liệu trong bảng TCDN_ChungTu
                Bang bang = new Bang("TCDN_ChungTu");
                bang.MaNguoiDungSua = MaNguoiDungSua;
                bang.IPSua = IPSua;
                bang.GiaTriKhoa = iID_MaChungTu;
                bang.Delete();
                return 1;
            }
            else 
                return 0;
        }

        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng TCDN_ChungTu, TCDN_ChungTuChiTiet
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TrangThaiTrinhDuyet"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaChungTu, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng TCDN_ChungTuChiTiet            
            String SQL = "UPDATE TCDN_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChungTu=@iID_MaChungTu";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE TCDN_ChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaChungTu=@iID_MaChungTu";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }

        public static String InsertDuyetChungTu(String iID_MaChungTu, String NoiDung, String MaND, String IPSua)
        {
            String iID_MaDuyetChungTu;
            Bang bang = new Bang("TCDN_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetChungTu = Convert.ToString(bang.Save());
            return iID_MaDuyetChungTu;
        }

        public static DataTable Get_DanhSachChungTu(String MaND, String SoChungTu, String iQuy, String iID_MaDoanhNghiep, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(TCDNModels.iID_MaPhanHe, MaND);
            DK += " AND iTrangThai = 1 AND iNamLamViec=@iNamLamViec ";
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iQuy) == false && iQuy != "" && iQuy != "-1")
            {
                DK += " AND iQuy = @iQuy";
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
            }
            if (String.IsNullOrEmpty(iID_MaDoanhNghiep) == false && iID_MaDoanhNghiep != Guid.Empty.ToString())
            {
                DK += " AND iID_MaDoanhNghiep = @iID_MaDoanhNghiep";
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
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
            String SQL = String.Format("SELECT * FROM TCDN_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iQuy DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChungTu_Count(String MaND, String SoChungTu, String iQuy, String iID_MaDoanhNghiep, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(TCDNModels.iID_MaPhanHe, MaND);
            DK += " AND iTrangThai = 1 ";
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iQuy) == false && iQuy != "" && iQuy != "-1")
            {
                DK += " AND iQuy = @iQuy";
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
            }
            if (String.IsNullOrEmpty(iID_MaDoanhNghiep) == false && iID_MaDoanhNghiep != Guid.Empty.ToString())
            {
                DK += " AND iID_MaDoanhNghiep = @iID_MaDoanhNghiep";
                cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
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
            String SQL = String.Format("SELECT Count(*) FROM TCDN_ChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}