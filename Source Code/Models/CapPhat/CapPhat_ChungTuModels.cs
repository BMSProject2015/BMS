using System;
using System.Data.SqlClient;
using System.Data;
using DomainModel;
using System.Collections.Specialized;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class CapPhat_ChungTuModels
    {
        public static NameValueCollection LayThongTin(String iID_MaCapPhat)
        {         
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = LayChungTuCapPhat(iID_MaCapPhat);
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
        /// Lấy thông tin chứng từ theo mã chứng từ
        /// </summary>
        /// <param name="siID_MaCapPhat"></param>
        /// <returns></returns>
        public static DataTable LayToanBoThongTinChungTu(String siID_MaCapPhat)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CP_CapPhat WHERE iTrangThai=1 AND iID_MaCapPhat=@iID_MaCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", siID_MaCapPhat);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Hàm nằm ngoài scope phân hệ
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static DataTable GetDanhSachCapPhat(String iNamLamViec)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaCapPhat, sTienToChungTu + '' + Convert(nvarchar, iSoCapPhat) + ' - ' + sNoiDung AS TENHT  FROM CP_CapPhat WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec");
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Hàm lấy các thông tin cơ bản của chứng từ cấp phát
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <returns></returns>
        public static DataTable LayChungTuCapPhat(String iID_MaCapPhat)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT *,Day(dNgayCapPhat) as Ngay,MONTH(dNgayCapPhat) as Thang,YEAR(dNgayCapPhat) as Nam FROM CP_CapPhat WHERE iTrangThai=1 AND iID_MaCapPhat=@iID_MaCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách chứng từ trong danh sách chứng từ cục dựa theo các tham số của 
        /// thông tin tìm kiếm
        /// </summary>
        /// <param name="sMaPhongBan"></param>
        /// <param name="sMaND"></param>
        /// <param name="sSoChungTu"></param>
        /// <param name="sTuNgay"></param>
        /// <param name="sDenNgay"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="bLayTheoMaNDTao"></param>
        /// <param name="iTrang"></param>
        /// <param name="iSoBanGhi"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuCuc(String sMaPhongBan, String sMaND, String sSoChungTu, String sTuNgay, String sDenNgay, String iID_MaTrangThaiDuyet, String iDM_MaLoaiCapPhat, Boolean bLayTheoMaNDTao = false, int iTrang = 1, int iSoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeCapPhat, sMaND);

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(sMaND);
            DK += " AND iNamLamViec=@iNamLamViec";
            DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
            DK += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            cmd.Parameters.AddWithValue("@iNamLamViec", dtCauHinh.Rows[0]["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            DK += " AND iID_MaDonVi is NULL";
            if (sMaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(sMaPhongBan) == false && sMaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", sMaPhongBan);
            }
            if (bLayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(sMaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sMaND);
            }
            if (iDM_MaLoaiCapPhat != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iDM_MaLoaiCapPhat) == false && iDM_MaLoaiCapPhat != "")
            {
                DK += " AND iDM_MaLoaiCapPhat = @iDM_MaLoaiCapPhat";
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
            if (CommonFunction.IsNumeric(sSoChungTu))
            {
                DK += " AND iSoCapPhat = @iSoCapPhat";
                cmd.Parameters.AddWithValue("@iSoCapPhat", sSoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(sTuNgay) == false && sTuNgay != "")
            {
                DK += " AND dNgayCapPhat >= @dTuNgayCapPhat";
                cmd.Parameters.AddWithValue("@dTuNgayCapPhat", CommonFunction.LayNgayTuXau(sTuNgay));
            }
            if (String.IsNullOrEmpty(sDenNgay) == false && sDenNgay != "")
            {
                DK += " AND dNgayCapPhat <= @dDenNgayCapPhat";
                cmd.Parameters.AddWithValue("@dDenNgayCapPhat", CommonFunction.LayNgayTuXau(sDenNgay));
            }

            String SQL = String.Format("SELECT * FROM CP_CapPhat WHERE iTrangThai=1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,iSoCapPhat DESC", iTrang, iSoBanGhi);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy danh sách chứng từ trong danh sách chứng từ cục dựa theo các tham số của 
        /// thông tin tìm kiếm
        /// </summary>
        /// <param name="sMaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sMaND"></param>
        /// <param name="sSoChungTu"></param>
        /// <param name="sTuNgay"></param>
        /// <param name="sDenNgay"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="bLayTheoMaNDTao"></param>
        /// <param name="iTrang"></param>
        /// <param name="iSoBanGhi"></param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuDonVi(String sMaPhongBan, String iID_MaDonVi, String sMaND, String sSoChungTu, String sTuNgay, String sDenNgay, String iID_MaTrangThaiDuyet, String iDM_MaLoaiCapPhat, Boolean bLayTheoMaNDTao = false, int iTrang = 1, int iSoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeCapPhat, sMaND);

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(sMaND);
            DK += " AND iNamLamViec=@iNamLamViec";
            DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
            DK += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            cmd.Parameters.AddWithValue("@iNamLamViec", dtCauHinh.Rows[0]["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);

            if (sMaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(sMaPhongBan) == false && sMaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", sMaPhongBan);
            }
            if (String.IsNullOrEmpty(iID_MaDonVi) == false && iID_MaDonVi != "")
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            else
            {
                DK += " AND iID_MaDonVi IS NOT NULL";
            }
            if (bLayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(sMaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sMaND);
            }
            if (iDM_MaLoaiCapPhat != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iDM_MaLoaiCapPhat) == false && iDM_MaLoaiCapPhat != "")
            {
                DK += " AND iDM_MaLoaiCapPhat = @iDM_MaLoaiCapPhat";
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
            if (CommonFunction.IsNumeric(sSoChungTu))
            {
                DK += " AND iSoCapPhat = @iSoCapPhat";
                cmd.Parameters.AddWithValue("@iSoCapPhat", sSoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(sTuNgay) == false && sTuNgay != "")
            {
                DK += " AND dNgayCapPhat >= @dTuNgayCapPhat";
                cmd.Parameters.AddWithValue("@dTuNgayCapPhat", CommonFunction.LayNgayTuXau(sTuNgay));
            }
            if (String.IsNullOrEmpty(sDenNgay) == false && sDenNgay != "")
            {
                DK += " AND dNgayCapPhat <= @dDenNgayCapPhat";
                cmd.Parameters.AddWithValue("@dDenNgayCapPhat", CommonFunction.LayNgayTuXau(sDenNgay));
            }

            String SQL = String.Format("SELECT * FROM CP_CapPhat WHERE iTrangThai=1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iID_MaTrangThaiDuyet,iSoCapPhat DESC", iTrang, iSoBanGhi);
            cmd.Dispose();
            return vR;
        }
        
        /// <summary>
        /// Hàm trả về số bản ghi lấy từ bảng cấp phát trong danh sách
        /// chứng từ cấp phát cục thỏa mãn điều kiện cấp phát
        /// </summary>
        /// <param name="sMaPhongBan"></param>
        /// <param name="sMaND"></param>
        /// <param name="sSoChungTu"></param>
        /// <param name="TuNgay"></param>
        /// <param name="sDenNgay"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="bLayTheoMaNDTao"></param>
        /// <returns></returns>
        public static int LayDanhSachChungTuCapPhatCucCount(String sMaPhongBan = "", String sMaND = "", String sSoChungTu = "", String TuNgay = "", String sDenNgay = "", String iID_MaTrangThaiDuyet = "", String iDM_MaLoaiCapPhat = "", Boolean bLayTheoMaNDTao = false)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeCapPhat, sMaND);

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(sMaND);
            DK += " AND iNamLamViec=@iNamLamViec";
            DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
            DK += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            cmd.Parameters.AddWithValue("@iNamLamViec", dtCauHinh.Rows[0]["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);

            if (sMaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(sMaPhongBan) == false && sMaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", sMaPhongBan);
            }
            if (bLayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(sMaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sMaND);
            }
            if (iDM_MaLoaiCapPhat != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iDM_MaLoaiCapPhat) == false && iDM_MaLoaiCapPhat != "")
            {
                DK += " AND iDM_MaLoaiCapPhat = @iDM_MaLoaiCapPhat";
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
            if (CommonFunction.IsNumeric(sSoChungTu))
            {
                DK += " AND iSoCapPhat = @iSoCapPhat";
                cmd.Parameters.AddWithValue("@iSoCapPhat", sSoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(TuNgay) == false && TuNgay != "")
            {
                DK += " AND dNgayCapPhat >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(TuNgay));
            }
            if (String.IsNullOrEmpty(sDenNgay) == false && sDenNgay != "")
            {
                DK += " AND dNgayCapPhat <= @dDenNgayCapPhat";
                cmd.Parameters.AddWithValue("@dDenNgayCapPhat", CommonFunction.LayNgayTuXau(sDenNgay));
            }

            String SQL = String.Format("SELECT COUNT(*) FROM CP_CapPhat WHERE iTrangThai=1 AND {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }


        /// <summary>
        /// Hàm trả về số bản ghi lấy từ bảng cấp phát trong danh sách
        /// chứng từ cấp phát đơn vị thỏa mãn điều kiện cấp phát
        /// </summary>
        /// <param name="sMaPhongBan"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="sMaND"></param>
        /// <param name="SoChungTu"></param>
        /// <param name="sTuNgay"></param>
        /// <param name="sDenNgay"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="iDM_MaLoaiCapPhat"></param>
        /// <param name="bLayTheoMaNDTao"></param>
        /// <returns></returns>
        public static int LayDanhSachChungTuCapPhatDonViCount(String sMaPhongBan = "", String iID_MaDonVi = "", String sMaND = "", String SoChungTu = "", String sTuNgay = "", String sDenNgay = "", String iID_MaTrangThaiDuyet = "", String iDM_MaLoaiCapPhat = "", Boolean bLayTheoMaNDTao = false)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeCapPhat, sMaND);

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(sMaND);
            DK += " AND iNamLamViec=@iNamLamViec";
            DK += " AND iID_MaNamNganSach=@iID_MaNamNganSach";
            DK += " AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            cmd.Parameters.AddWithValue("@iNamLamViec", dtCauHinh.Rows[0]["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);

            if (sMaPhongBan != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(sMaPhongBan) == false && sMaPhongBan != "")
            {
                DK += " AND iID_MaPhongBan = @iID_MaPhongBan";
                cmd.Parameters.AddWithValue("@iID_MaPhongBan", sMaPhongBan);
            }
            if (String.IsNullOrEmpty(iID_MaDonVi) == false && iID_MaDonVi != "")
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            else
            {
                DK += " AND iID_MaDonVi IS NOT NULL";
            }
            if (bLayTheoMaNDTao && BaoMat.KiemTraNguoiDungQuanTri(sMaND) == false)
            {
                DK += " AND sID_MaNguoiDungTao = @sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", sMaND);
            }
            if (iDM_MaLoaiCapPhat != Convert.ToString(Guid.Empty) && String.IsNullOrEmpty(iDM_MaLoaiCapPhat) == false && iDM_MaLoaiCapPhat != "")
            {
                DK += " AND iDM_MaLoaiCapPhat = @iDM_MaLoaiCapPhat";
                cmd.Parameters.AddWithValue("@iDM_MaLoaiCapPhat", iDM_MaLoaiCapPhat);
            }
            if (CommonFunction.IsNumeric(SoChungTu))
            {
                DK += " AND iSoCapPhat = @iSoCapPhat";
                cmd.Parameters.AddWithValue("@iSoCapPhat", SoChungTu);
            }
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) == false && iID_MaTrangThaiDuyet != "" && iID_MaTrangThaiDuyet != "-1")
            {
                DK += " AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            if (String.IsNullOrEmpty(sTuNgay) == false && sTuNgay != "")
            {
                DK += " AND dNgayCapPhat >= @dTuNgayChungTu";
                cmd.Parameters.AddWithValue("@dTuNgayChungTu", CommonFunction.LayNgayTuXau(sTuNgay));
            }
            if (String.IsNullOrEmpty(sDenNgay) == false && sDenNgay != "")
            {
                DK += " AND dNgayCapPhat <= @dDenNgayCapPhat";
                cmd.Parameters.AddWithValue("@dDenNgayCapPhat", CommonFunction.LayNgayTuXau(sDenNgay));
            }

            String SQL = String.Format("SELECT COUNT(*) FROM CP_CapPhat WHERE iTrangThai=1 AND iID_MaDonVi IS NOT NULL AND {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Xóa chứng từ
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="sIPSua"></param>
        /// <param name="sMaNguoiDungSua"></param>
        public static void XoaChungTu(String iID_MaCapPhat, String sIPSua, String sMaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng CP_CapPhatChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("UPDATE CP_CapPhatChiTiet SET iTrangThai=0 WHERE iID_MaCapPhat=@iID_MaCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng CP_CapPhat
            Bang bang = new Bang("CP_CapPhat");
            bang.MaNguoiDungSua = sMaNguoiDungSua;
            bang.IPSua = sIPSua;
            bang.GiaTriKhoa = iID_MaCapPhat;
            bang.Delete();

        }
        /// <summary>
        /// cập nhật chứng từ
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="Params"></param>
        /// <param name="sMaND"></param>
        /// <param name="sIPSua"></param>
        /// <returns></returns>
        public static Boolean CapNhatBanGhi(String iID_MaCapPhat, SqlParameterCollection Params, String sMaND, String sIPSua)
        {
            Bang bang = new Bang("CP_CapPhat");
            bang.MaNguoiDungSua = sMaND;
            bang.IPSua = sIPSua;
            bang.GiaTriKhoa = iID_MaCapPhat;
            bang.DuLieuMoi = false;
            for (int i = 0; i < Params.Count; i++)
            {
                bang.CmdParams.Parameters.AddWithValue(Params[i].ParameterName, Params[i].Value);
            }
            bang.Save();
            return false;
        }
        /// <summary>
        /// Ham cập nhật mã trạng thái duyệt của chứng từ sau khi chứng từ
        /// được duyệt hoặc bị từ chối
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="bTrangThaiTrinhDuyet"></param>
        /// <param name="sMaND"></param>
        /// <param name="sIPSua"></param>
        /// <returns></returns>
        public static Boolean CapNhatMaTrangThaiDuyet(String iID_MaCapPhat, int iID_MaTrangThaiDuyet, Boolean bTrangThaiTrinhDuyet, String sMaND, String sIPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            CapNhatBanGhi(iID_MaCapPhat, cmd.Parameters, sMaND, sIPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng CP_CapPhatChiTiet            
            String SQL = "UPDATE CP_CapPhatChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaCapPhat=@iID_MaCapPhat";
            if (bTrangThaiTrinhDuyet)
            {
                SQL = "UPDATE CP_CapPhatChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaCapPhat=@iID_MaCapPhat";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }
        /// <summary>
        /// Hàm lưu lần duyệt/từ chối của chứng từ
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="sNoiDung"></param>
        /// <param name="sMaND"></param>
        /// <param name="sIPSua"></param>
        /// <returns></returns>
        public static String CapNhatBangDuyetChungTu(String iID_MaCapPhat, String sNoiDung, String sMaND, String sIPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("CP_DuyetCapPhat");
            bang.MaNguoiDungSua = sMaND;
            bang.IPSua = sIPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", sNoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }

        /// <summary>
        /// Tổng hợp lại các ghi chú từ chối cần sửa trong chứng từ
        /// Hàm này chỉ được gọi khi chứng từ bị từ chối
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        public static void CapNhapLaiTruong_sSua(String iID_MaCapPhat)
        {
            String iID_MaDuyetCapPhat;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetCapPhatCuoiCung FROM CP_CapPhat WHERE iID_MaCapPhat=@iID_MaCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            iID_MaDuyetCapPhat = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            cmd = new SqlCommand("SELECT * FROM CP_CapPhatChiTiet WHERE iID_MaCapPhat=@iID_MaCapPhat AND bDongY=1");
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sSua = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sSua += String.Format("Mục {0}: {1}<br/>", dt.Rows[i]["sXauNoiMa"], dt.Rows[i]["sLyDo"]);
            }
            dt.Dispose();

            cmd = new SqlCommand("UPDATE CP_DuyetCapPhat SET sSua=@sSua WHERE iID_MaDuyetCapPhat=@iID_MaDuyetCapPhat");
            cmd.Parameters.AddWithValue("@iID_MaDuyetCapPhat", iID_MaDuyetCapPhat);
            cmd.Parameters.AddWithValue("@sSua", sSua);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        //Lấy danh sách đơn vị cấp phát dùng cho thông tri cấp phát
        public static DataTable LayDtDonViCapPhat(String iID_MaCapPhat)
        {
            String SQL = "SELECT sTen,CP.iID_MaDonVi FROM (SELECT Distinct(iID_MaDonVi) FROM CP_CapPhatChiTiet ";
            SQL += " WHERE iID_MaCapPhat=@iID_MaCapPhat";
            SQL += " AND iID_MaDonVi <> '99' ";
            SQL += " AND iID_MaDonVi<>'') CP";
            SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec_DonVi ) NS_DonVi ON CP.iID_MaDonVi=NS_DonVi.iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", iID_MaCapPhat);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Hàm lấy danh sách lựa chọn loại con chi tiết nhất
        /// được nhập của chứng từ cấp phát
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable LayLoaiNganSachCon()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_Loai");
            dt.Columns.Add("TenHT");
            DataRow r = dt.NewRow();

            //Thêm Mục
            r["iID_Loai"] = "sM";
            r["TenHT"] = "Mục";
            dt.Rows.Add(r);

            //Thêm Tiểu Mục
            r = dt.NewRow();
            r["iID_Loai"] = "sTM";
            r["TenHT"] = "Tiểu Mục";
            dt.Rows.Add(r);

            //Thêm Ngành
            r = dt.NewRow();
            r["iID_Loai"] = "sNG";
            r["TenHT"] = "Ngành";
            dt.Rows.Add(r);

            return dt;
        }

        //VungNV: 2015/10/21 get max value of iSoCapPhat
        public static int GetMaxSoCapPhat()
        {
            SqlCommand cmd = new SqlCommand();
            int iSoCapPhat = 0;
            String SQL = "SELECT MAX(iSoCapPhat) FROM CP_CapPhat";
            cmd.CommandText = SQL;
            String sSoCapPhat = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            if (sSoCapPhat != "")
            {
                iSoCapPhat = Int32.Parse(sSoCapPhat);
            }

            return iSoCapPhat;
        }
        
    }
}