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
    public class PhanBo_PhanBoModels
    {
        /// <summary>
        /// Lấy thông tin của một chứng từ chỉ tiêu phân bổ
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        /// <returns></returns>
        public static NameValueCollection LayThongTin(String iID_MaPhanBo)
        {
            NameValueCollection Data = new NameValueCollection();
            DataTable dt = GetPhanBo(iID_MaPhanBo);
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
        /// Lay danh sách phân bổ để chọn tổng hợp
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        /// <returns></returns>
        public static DataTable Get_dtChungTuPhanBo(String iID_MaPhanBo,String username)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(username);
            String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            DataTable dtPB = GetPhanBo(iID_MaPhanBo);
            int TrangThaiTaoMoi = LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHePhanBo);
            int TrangThaiTroLyPhongBanTrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(TrangThaiTaoMoi);

            String SQL = @"SELECT * FROM PB_PhanBo WHERE iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND iID_MaDonVi <> '99' AND iNamLamViec=@iNamLamViec 
                        AND iID_MaNamNganSach=@iID_MaNamNganSach AND iID_MaNguonNganSach=@iID_MaNguonNganSach 
                        AND  iID_MaPhanBo <> @iID_MaPhanBo AND  bPhanBoTong=0 
                        AND iID_MaPhanBo NOT IN (SELECT iID_MaPhanBo FROM PB_PhanBo_PhanBo WHERE iID_MaPhanBoTong <> @iID_MaPhanBo)";
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanBo",iID_MaPhanBo);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", NamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", NguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", TrangThaiTroLyPhongBanTrinhDuyet);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy danh sách phần bổ đã được chọn
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        /// <returns></returns>
        public static DataTable DanhSachPhanBoDuocChon(String iID_MaPhanBo)
        {
            String SQL = @"SELECT iID_MaPhanBo FROM PB_PhanBo_PhanBo WHERE iID_MaPhanBoTong=@iID_MaPhanBoTong";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanBoTong", iID_MaPhanBo);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Lấy DataTable thông tin của một chứng từ chỉ tiêu phân bổ
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        /// <returns></returns>
        public static DataTable GetPhanBo(String iID_MaPhanBo)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM PB_PhanBo WHERE iTrangThai=1 AND iID_MaPhanBo=@iID_MaPhanBo");
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static String InsertRecord(String iID_MaDotPhanBo, SqlParameterCollection Params, String MaND, String IPSua)
        {
            String MaPhanBo = "";
            Bang bang = new Bang("PB_PhanBo");
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
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanBoModels.iID_MaPhanHePhanBo));
            bang.DuLieuMoi = true;
            String MaPhanBoAddNew = Convert.ToString(bang.Save());

            //Thêm chi tiết chỉ tiêu
            PhanBo_PhanBoChiTietModels.ThemChiTiet(MaPhanBoAddNew, MaND, IPSua);

            return MaPhanBo;
        }

        /// <summary>
        /// Cập nhập dữ liệu 1 Record của Chỉ tiêu
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        /// <param name="Params">Params là của cmd.Parameters</param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean UpdateRecord(String iID_MaPhanBo, SqlParameterCollection Params, String MaND, String IPSua)
        {
            Bang bang = new Bang("PB_PhanBo");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaPhanBo;
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
        /// <param name="iID_MaPhanBo"></param>
        /// <param name="IPSua"></param>
        /// <param name="MaNguoiDungSua"></param>
        /// <returns></returns>
        public static int Delete_PhanBo(String iID_MaPhanBo, String IPSua, String MaNguoiDungSua)
        {
            //Xóa dữ liệu trong bảng PB_PhanBoChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM PB_PhanBoChiTiet WHERE iID_MaPhanBo=@iID_MaPhanBo");
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            //Xóa dữ liệu trong bảng DT_DotNganSach
            Bang bang = new Bang("PB_PhanBo");
            bang.MaNguoiDungSua = MaNguoiDungSua;
            bang.IPSua = IPSua;
            bang.GiaTriKhoa = iID_MaPhanBo;
            bang.Delete();
            return 1;
        }

        /// <summary>
        /// Cập nhập trường iID_MaTrangThaiDuyet của bảng PB_PhanBo, PB_PhanBoChiTiet
        /// </summary>
        /// <param name="iID_MaPhanBo"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="TrangThaiTrinhDuyet"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static Boolean Update_iID_MaTrangThaiDuyet(String iID_MaPhanBo, int iID_MaTrangThaiDuyet, Boolean TrangThaiTrinhDuyet, String MaND, String IPSua)
        {
            SqlCommand cmd;

            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            UpdateRecord(iID_MaPhanBo, cmd.Parameters, MaND, IPSua);
            cmd.Dispose();

            //Sửa dữ liệu trong bảng PB_PhanBoChiTiet            
            String SQL = "UPDATE PB_PhanBoChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet WHERE iID_MaPhanBo=@iID_MaPhanBo";
            if (TrangThaiTrinhDuyet)
            {
                SQL = "UPDATE PB_PhanBoChiTiet SET iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet, bDongY=0, sLyDo='' WHERE iID_MaPhanBo=@iID_MaPhanBo";
            }
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            cmd.CommandText = SQL;
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return false;
        }

        public static String InsertDuyetPhanBo(String iID_MaPhanBo, String NoiDung, String MaND, String IPSua)
        {
            String iID_MaDuyetPhanBo;
            Bang bang = new Bang("PB_DuyetPhanBo");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhanBo", iID_MaPhanBo);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            iID_MaDuyetPhanBo = Convert.ToString(bang.Save());
            return iID_MaDuyetPhanBo;
        }

        public static DataTable Get_DanhSachPhanBo_TheoChiTieu(String iID_MaChiTieu, String MaND)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanBoModels.iID_MaPhanHePhanBo, MaND);
            DK = " iID_MaChiTieu = @iID_MaChiTieu";
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);

            String SQL = String.Format("SELECT * FROM PB_PhanBo WHERE {0} ORDER BY iSoChungTu DESC", DK);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachPhanBo_TheoChiTieu_Count(String iID_MaChiTieu, String MaND)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanBoModels.iID_MaPhanHePhanBo, MaND);
            DK = " iID_MaChiTieu = @iID_MaChiTieu";
            cmd.Parameters.AddWithValue("@iID_MaChiTieu", iID_MaChiTieu);

            String SQL = String.Format("SELECT COUNT(*) FROM PB_PhanBo WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd,0));
            cmd.Dispose();
            return vR;
        }

        public static DataTable Get_DanhSachPhanBo(String MaPhongBan, String NgayDotNganSach, String MaDotNganSach, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, String MaND, Boolean LayTheoMaNDTao = false, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];

            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanBoModels.iID_MaPhanHePhanBo, MaND);
            DK += "  "+ ReportModels.DieuKien_NganSach(MaND) +"  ";

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
            if (String.IsNullOrEmpty(NgayDotNganSach) == false && NgayDotNganSach != "")
            {
                DK += " AND CONVERT(nvarchar, dNgayDotPhanBo, 103) = @dNgayDotPhanBo";
                cmd.Parameters.AddWithValue("@dNgayDotPhanBo", NgayDotNganSach);
            }
            if (String.IsNullOrEmpty(MaDotNganSach) == false && MaDotNganSach != "")
            {
                DK += " AND iID_MaDotPhanBo = @iID_MaDotPhanBo";
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotNganSach);
            }
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

            String SQL = String.Format("SELECT * FROM PB_PhanBo WHERE {0} ", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "iSoChungTu DESC, iID_MaTrangThaiDuyet", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSachPhanBo_Count(String MaPhongBan, String NgayDotNganSach, String MaDotNganSach, String SoChungTu, String TuNgay, String DenNgay, String iID_MaTrangThaiDuyet, String MaND, Boolean LayTheoMaNDTao=false)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            DataRow R = dtCauHinh.Rows[0];

            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = LuongCongViecModel.Get_DieuKien_TrangThaiDuyet_DuocXem(PhanBoModels.iID_MaPhanHeChiTieu, MaND);
            DK += "  " + ReportModels.DieuKien_NganSach(MaND) + "  ";

            cmd.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);

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
            if (String.IsNullOrEmpty(NgayDotNganSach) == false && NgayDotNganSach != "")
            {
                DK += " AND CONVERT(nvarchar, dNgayDotPhanBo, 103) = @dNgayDotPhanBo";
                cmd.Parameters.AddWithValue("@dNgayDotPhanBo", NgayDotNganSach);
            }
            if (String.IsNullOrEmpty(MaDotNganSach) == false && MaDotNganSach != "")
            {
                DK += " AND iID_MaDotPhanBo = @iID_MaDotPhanBo";
                cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotNganSach);
            }
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

            String SQL = String.Format("SELECT Count(*) FROM PB_PhanBo WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

     
    }
}