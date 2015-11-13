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
    public class PhanBo_ChiTieuModels
    {
        /// <summary>
        /// Lấy thông tin của một chứng từ chỉ tiêu phân bổ
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTin(String iID_MaChiTieu)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetChiTieu(iID_MaChiTieu);
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
        /// <param name="iID_MaChiTieu"></param>
        /// <returns></returns>
        public static DataTable GetChiTieu(String iID_MaChiTieu)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM PB_ChiTieu WHERE iTrangThai=1 AND iID_MaChiTieu=@iID_MaChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static String InsertRecord(String iID_MaDotPhanBo, SqlParameterCollection Params, String MaND, String IPSua)
        {
            String MaChiTieu = "";
            Bang bang = new Bang("PB_ChiTieu");
            DataTable dtDotPhanBo = PhanBo_DotPhanBoModels.GetDotPhanBo(iID_MaDotPhanBo);
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.CmdParams.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            bang.CmdParams.Parameters.AddWithValue("@dNgayDotPhanBo", dtDotPhanBo.Rows[0]["dNgayDotPhanBo"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtDotPhanBo.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtDotPhanBo.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtDotPhanBo.Rows[0]["iID_MaNamNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanBoModels.iID_MaPhanHeChiTieu));
            bang.DuLieuMoi = true;
            String MaChiTieuAddNew = Convert.ToString(bang.Save());

            //Thêm chi tiết chỉ tiêu
            PhanBo_ChiTieuChiTietModels.ThemChiTiet(MaChiTieuAddNew, MaND, IPSua);

            return MaChiTieu;
        }
        
        /// <summary>
        /// Cập nhập dữ liệu 1 Record của Chỉ tiêu
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        /// <param name="Params">Params là của cmd.Parameters</param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean UpdateRecord(String iID_MaChiTieu, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("PB_ChiTieu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChiTieu;
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
        /// <param name="iID_MaChiTieu"></param>
        /// <param name="IPSua"></param>
        /// <param name="MaNguoiDungSua"></param>
        /// <returns></returns>
        public static int Delete_ChiTieu(String iID_MaChiTieu, String IPSua, String MaNguoiDungSua)
        {
            
            //Xóa dữ liệu trong bảng PB_ChiTieuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM PB_ChiTieuChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            //Xóa dữ liệu trong bảng PB_ChiTieu_DuToan
            cmd = new SqlCommand("DELETE FROM PB_ChiTieu_DuToan WHERE iID_MaChiTieu=@iID_MaChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

          //  Xóa dữ liệu trong bảng DT_DotNganSach
            Bang bang = new Bang("PB_ChiTieu");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaChiTieu;
            bang.Delete();
            return 1;
        }
        /// <summary>
        /// Xóa dữ liệu chỉ tiêu
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        /// <param name="IPSua"></param>
        /// <param name="MaNguoiDungSua"></param>
        /// <returns></returns>
        public static int Delete_ChiTieuChiTiet(String iID_MaChiTieu, String IPSua, String MaNguoiDungSua)
        {
            
            //Xóa dữ liệu trong bảng PB_ChiTieuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM PB_ChiTieuChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return 1;
        }
        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng PB_ChiTieu, PB_ChiTieuChiTiet
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TrangThaiTrinhDuyet"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaChiTieu, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            UpdateRecord(iID_MaChiTieu, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng PB_ChiTieuChiTiet            
            String SQL = "UPDATE PB_ChiTieuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaChiTieu=@iID_MaChiTieu";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE PB_ChiTieuChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaChiTieu=@iID_MaChiTieu";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }

        public static String InsertDuyetChiTieu(String iID_MaChiTieu, String NoiDung, String MaND, String IPSua)
        {
            String iID_MaDuyetChiTieu;
            Bang bang = new Bang("PB_DuyetChiTieu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetChiTieu = Convert.ToString(bang.Save());
            return iID_MaDuyetChiTieu;
        }

        public static DataTable Get_DanhSachChiTieu(String iID_MaDotPhanBo, String MaND, String MaPhongBan, String NgayDotPhanBo, String SoChiTieu, String iID_MaTrangThaiDuyet, String TuNgay, String DenNgay, Boolean LayTheoMaNDTao=false, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];

            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanBoModels.iID_MaPhanHeChiTieu, MaND);
            DK += " AND iNamLamViec=@iNamLamViec AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND iID_MaNamNganSach=@iID_MaNamNganSach ";

            cmd.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);

            if (iID_MaDotPhanBo != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iID_MaDotPhanBo) == false && iID_MaDotPhanBo != "")
            {
                DK += " AND iID_MaDotPhanBo = @iID_MaDotPhanBo";
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            }
            if (LayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(MaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            if (MaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(MaPhongBan) == false && MaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            }
            if (String.IsNullOrEmpty(NgayDotPhanBo) == false && NgayDotPhanBo != "")
            {
                DK += " AND dNgayDotNganSach = @dNgayDotNganSach";
                cmd.Parameters.AddWithValue("@dNgayDotNganSach", CommonFunction.LayNgayTuXau(NgayDotPhanBo));
            }
            if (CommonFunction.IsNumeric(SoChiTieu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChiTieu);
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
            String SQL = String.Format("SELECT * FROM PB_ChiTieu WHERE iTrangThai=1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,iSoChungTu ASC, dNgayChungTu DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachChiTieu_Count(String iID_MaDotPhanBo, String MaND, String MaPhongBan, String NgayDotPhanBo, String SoChiTieu, String iID_MaTrangThaiDuyet, String TuNgay, String DenNgay, Boolean LayTheoMaNDTao=false)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanBoModels.iID_MaPhanHeChiTieu, MaND);
            DK += " AND iNamLamViec=@iNamLamViec AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND iID_MaNamNganSach=@iID_MaNamNganSach ";

            cmd.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);

            if (iID_MaDotPhanBo != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iID_MaDotPhanBo) == false && iID_MaDotPhanBo != "")
            {
                DK += " AND iID_MaDotPhanBo = @iID_MaDotPhanBo";
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            }
            if (MaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(MaPhongBan) == false && MaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", MaPhongBan);
            }
            if (LayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(MaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            if (String.IsNullOrEmpty(NgayDotPhanBo) == false && NgayDotPhanBo != "")
            {
                DK += " AND dNgayDotNganSach = @dNgayDotNganSach";
                cmd.Parameters.AddWithValue("@dNgayDotNganSach", CommonFunction.LayNgayTuXau(NgayDotPhanBo));
            }
            if (CommonFunction.IsNumeric(SoChiTieu))
            {
                DK += " AND iSoChungTu = @iSoChungTu";
                cmd.Parameters.AddWithValue("@iSoChungTu", SoChiTieu);
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
            String SQL = String.Format("SELECT Count(*) FROM PB_ChiTieu WHERE iTrangThai=1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        #region Thêm chỉ tiêu có chọn chứng từ dự toán
        public static DataTable GET_DanhSachDuToan(String MaND,String iID_MaChiTieu, Boolean bChiNganSach)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            int iNamLamViec = Convert.ToInt16(dt.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach= Convert.ToInt16(dt.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt16(dt.Rows[0]["iID_MaNamNganSach"]);
            return GET_DanhSachDuToan(iNamLamViec, iID_MaNguonNganSach, iID_MaNamNganSach,iID_MaChiTieu, bChiNganSach);
        }
        public static DataTable GET_DanhSachDuToan(int iNamLamViec,
                                              int iID_MaNguonNganSach,
                                              int iID_MaNamNganSach,String iID_MaChiTieu,
                                              Boolean bChiNganSach)
        {
            DataTable vR;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (String.IsNullOrEmpty(iID_MaChiTieu) == false)
            {
                DK = " WHERE iID_MaChiTieu<>@iID_MaChiTieu ";
                cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            }
            String SQL = String.Format(@"SELECT iID_MaChungTu,sTienToChungTu+''+convert(varchar,iSoChungTu) as sSoChungTu,dNgayChungTu FROM DT_ChungTu WHERE iTrangThai=1 AND 
                 iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet_DaDuyet AND iNamLamViec=@iNamLamViec AND
                 iID_MaNguonNganSach=@iID_MaNguonNganSach AND iID_MaNamNganSach=@iID_MaNamNganSach AND bChiNganSach=@bChiNganSach
                 AND iID_MaChungTu NOT IN (SELECT iID_MaDuToan FROM PB_ChiTieu_DuToan {0})",DK);
            int iID_MaTrangThaiDuyet_DaDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeDuToan);
           
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet_DaDuyet", iID_MaTrangThaiDuyet_DaDuyet);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@bChiNganSach", bChiNganSach);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable GET_DanhSachDuToanDuocChon(String iID_MaChiTieu)
        {
            String SQL = @"SELECT iID_MaDuToan FROM PB_ChiTieu_DuToan WHERE iID_MaChiTieu=@iID_MaChiTieu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Update vào bảng chi tiêu dự toán
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        /// <param name="siID_MaDuToan"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        public static void Update_ChiTieu_DuToan(String iID_MaChiTieu, String siID_MaDuToan, String MaND, String IPSua)
        {
            if (String.IsNullOrEmpty(siID_MaDuToan) == false)
            {
                String[] arrMaDuToan = siID_MaDuToan.Split(',');
                for (int i = 0; i < arrMaDuToan.Length; i++)
                {
                    Bang bangCTDT = new Bang("PB_ChiTieu_DuToan");
                    bangCTDT.MaNguoiDungSua = MaND;
                    bangCTDT.IPSua = IPSua;
                    bangCTDT.DuLieuMoi = true;
                    bangCTDT.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
                    bangCTDT.CmdParams.Parameters.AddWithValue("@iID_MaDuToan", arrMaDuToan[i]);
                    bangCTDT.Save();
                }
            }
        }
        /// <summary>
        /// Xóa bảng chỉ tiêu dự toán
        /// </summary>
        /// <param name="iID_MaChiTieu"></param>
        public static void Delete_ChiTieu_DuToan(String iID_MaChiTieu)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM PB_ChiTieu_DuToan WHERE iID_MaChiTieu=@iID_MaChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static void Delete_PhanBo(String iID_MaChiTieu)
        {
            //Xóa dữ liệu trong bảng PB_PhanBoChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM PB_PhanBoChiTiet WHERE iID_MaChiTieu=@iID_MaChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng DT_DotNganSach
            cmd = new SqlCommand("DELETE FROM PB_PhanBo WHERE iID_MaChiTieu=@iID_MaChiTieu");
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        #endregion
    }
}