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
    public class BaoHiem_PhaiThuModels
    {
        /// <summary>
        /// Lấy thông tin của một chứng từ quyết toán ngân sách
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTin(String iID_MaBaoHiemPhaiThu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetChungTu(iID_MaBaoHiemPhaiThu);
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
        public static DataTable GetChungTu(String iID_MaBaoHiemPhaiThu)
        {
            DataTable vR;
            String query = String.Format(@"SELECT BH_PhaiThuChungTu.*
                                           FROM BH_PhaiThuChungTu WHERE iTrangThai=1 AND iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu ");
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", iID_MaBaoHiemPhaiThu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static int GetMaxChungTu()
        {
            int vR;
            SqlCommand cmd = new SqlCommand("SELECT MAX(iSoChungTu) FROM BH_PhaiThuChungTu WHERE iTrangThai=1");
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
            Bang bang = new Bang("BH_PhaiThuChungTu");
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
        public static Boolean UpdateRecord(String iID_MaBaoHiemPhaiThu, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("BH_PhaiThuChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaBaoHiemPhaiThu;
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
        public static int Delete_ChungTu(String iID_MaBaoHiemPhaiThu, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng BH_PhaiThuChungTuTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM BH_PhaiThuChungTuChiTiet WHERE iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu");
            cmd.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", iID_MaBaoHiemPhaiThu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng BH_PhaiThuChungTu
            Bang bang = new Bang("BH_PhaiThuChungTu");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaBaoHiemPhaiThu;
            bang.Delete();
            return 1;
        }

        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng BH_ChungTu, BH_PhaiThuChungTuTiet
        /// </summary>
        /// <param name="iID_MaBaoHiemPhaiThu"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TrangThaiTrinhDuyet"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaBaoHiemPhaiThu, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            UpdateRecord(iID_MaBaoHiemPhaiThu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng BH_PhaiThuChungTuTiet            
            String SQL = "UPDATE BH_PhaiThuChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE BH_PhaiThuChungTuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaBaoHiemPhaiThu=@iID_MaBaoHiemPhaiThu";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", iID_MaBaoHiemPhaiThu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }

        public static String InsertDuyetChungTu(String iID_MaBaoHiemPhaiThu, String NoiDung, String MaND, String IPSua)
        {
            String iID_MaDuyetChungTu;
            Bang bang = new Bang("BH_DuyetPhaiThu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaBaoHiemPhaiThu", iID_MaBaoHiemPhaiThu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetChungTu = Convert.ToString(bang.Save());
            return iID_MaDuyetChungTu;
        }

        public static DataTable Get_DanhSachChungTu(String MaND, String SoChungTu, String iThang_Quy, String iID_MaTrangThaiDuyet,String iID_MaDonVi, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeBaoHiem, MaND);

            DK += " AND iTrangThai = 1";
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(iThang_Quy) == false && iThang_Quy != "" && iThang_Quy !="-1")
            {
                DK += " AND iThang_Quy = @iThang_Quy";
                cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            }

            String SQL = String.Format("SELECT BH_PhaiThuChungTu.* FROM BH_PhaiThuChungTu WHERE  {0} {1} ", DK, ReportModels.DieuKien_NganSach(MaND));
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,iSoChungTu DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
            //,(SELECT sTen FROM NS_DonVi WHERE NS_DonVi.iID_MaDonVi = BH_PhaiThuChungTu.iID_MaDonVi) AS sTen_DonVi
        }

        public static int Get_DanhSachChungTu_Count(String MaND, String SoChungTu, String iThang_Quy, String iID_MaTrangThaiDuyet, String iID_MaDonVi)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeBaoHiem, MaND);

            DK += " AND iTrangThai = 1";

            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaDonVi)==false)
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (String.IsNullOrEmpty(iThang_Quy) == false && iThang_Quy != "")
            {
                DK += " AND iThang_Quy >= @iThang_Quy";
                cmd.Parameters.AddWithValue("@iThang_Quy", iThang_Quy);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
          
            String SQL = String.Format("SELECT Count(*) FROM BH_PhaiThuChungTu WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
    }
}