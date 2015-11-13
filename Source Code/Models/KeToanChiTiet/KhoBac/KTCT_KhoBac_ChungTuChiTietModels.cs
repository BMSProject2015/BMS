using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class KTCT_KhoBac_ChungTuChiTietModels
    {
        /// <summary>
        /// Thêm dữ liệu vào bảng KTKB_ChungTuChiTiet
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        public static void ThemChiTiet(String iID_MaChungTu, String MaND, String IPSua)
        {
            DataTable dtChungTu = KTCT_KhoBac_ChungTuModels.GetChungTu(iID_MaChungTu);

            String iID_MaChiTieu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);


            Bang bang = new Bang("KTKB_ChungTuChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);

            dtChungTu.Dispose();
        }
        /// <summary>
        /// Thêm dữ liệu vào bảng KTKB_DuyetChungTu
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="NoiDung"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertDuyetQuyetToan(String iID_MaChungTu, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("KTKB_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
        }
        /// <summary>
        /// Lấy DataTable thông tin của một chứng từ chi tiết
        /// </summary>
        /// <param name="iID_MaChungTuChiTiet"></param>
        /// <returns></returns>
        public static DataTable GetChungTuChiTiet(String iID_MaChungTuChiTiet)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTKB_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet");
            cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy trạng thái từ chối của bảng KTTG_ChungTuChiTiet
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable Get_dtChungTuChiTietTuChoi(int iThang, int iNam, Dictionary<String, String> arrGiaTriTimKiem, String MaND)
        {
            Boolean LaTroLyPhongBan = LuongCongViecModel.KiemTra_TroLyPhongBan(MaND);
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();

            DK = "iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iThangCT=@iThangCT AND bDongY=0 AND (NOT sLyDo IS NULL AND sLyDo<>'')";
            cmd.Parameters.AddWithValue("@iThangCT", iThang);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            //cmd.Parameters.AddWithValue("@sLyDo", "");

            if (arrGiaTriTimKiem != null)
            {
            }

            if (LaTroLyPhongBan)
            {
                DK += " AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            SQL = String.Format("SELECT * FROM KTKB_ChungTuChiTiet WHERE {0} ORDER BY sSoChungTuGhiSo, iSTT, dNgayTao", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Lấy trạng thái từ chối
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static int Get_iID_MaTrangThaiDuyet_TuChoi(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = KTCT_KhoBac_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM KTKB_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
                    {
                        vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TuChoi);
                    }
                    cmd.Dispose();
                }
            }
            return vR;
        }
        /// <summary>
        /// Lấy trạng thái trình duyệt
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static int Get_iID_MaTrangThaiDuyet_TrinhDuyet(String MaND, String iID_MaChungTu)
        {
            int vR = -1;
            DataTable dt = KTCT_KhoBac_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    vR = Convert.ToInt32(iID_MaTrangThaiDuyet_TrinhDuyet);
                }
            }
            return vR;
        }
        /// <summary>
        /// Lấy danh sách trong bảng KTKB_ChungTuChiTiet theo mã chứng từ
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable Get_dtChungTuChiTiet(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND)
        {
            DataTable vR;
            String SQL, DK;
            SqlCommand cmd = new SqlCommand();
            Boolean LaTroLyPhongBan = LuongCongViecModel.KiemTra_TroLyPhongBan(MaND);
            DK = "iTrangThai=1";
            if (String.IsNullOrEmpty(iID_MaChungTu))
            {
                DK += " AND iID_MaChungTu IS NULL";
            }
            else
            {
                DK += " AND iID_MaChungTu=@iID_MaChungTu";
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            }

            if (arrGiaTriTimKiem != null)
            {
                if (String.IsNullOrEmpty(arrGiaTriTimKiem["MaND"]) == false)
                {
                    if (LaTroLyPhongBan == false)
                    {
                        DK += " AND sID_MaNguoiDungTao=@MaND";
                        cmd.Parameters.AddWithValue("@MaND", arrGiaTriTimKiem["MaND"]);
                    }
                }
                if (String.IsNullOrEmpty(arrGiaTriTimKiem["TrangThai"]) == false)
                {
                    if (arrGiaTriTimKiem["TrangThai"] == "1")
                    {
                        DK += " AND bDongY=@TrangThai";
                        cmd.Parameters.AddWithValue("@TrangThai", true);
                    }
                    else
                    {
                        DK += " AND bDongY=@TrangThai";
                        cmd.Parameters.AddWithValue("@TrangThai", false);
                    }
                }
            }

            if (LaTroLyPhongBan)
            {
                DK += " AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            SQL = String.Format("SELECT * FROM KTKB_ChungTuChiTiet WHERE {0} ORDER BY iSTT, dNgayTao", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Lấy chi tiết bản ghi trong bảng KTKB_ChungTuChiTiet với mã chứng từ chi tiết
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public static DataTable Get_dtChungTuChiTiet_Row(String iID_MaChungTuChiTiet)
        {
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTKB_ChungTuChiTiet WHERE iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet");
            cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách chứng từ chi tiết với các tham số tìm kiếm
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="SoChungTu"></param>
        /// <param name="iNgayCT"></param>
        /// <param name="iThangCT"></param>
        /// <param name="iNgay"></param>
        /// <param name="iThang"></param>
        /// <param name="sSoTienTu"></param>
        /// <param name="sSoTienDen"></param>
        /// <param name="sTaiKhoanNo"></param>
        /// <param name="sTaiKhoanCo"></param>
        /// <param name="sDonViNo"></param>
        /// <param name="sDonViCo"></param>
        /// <param name="Trang"></param>
        /// <param name="SoBanGhi"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSachChungTuChiTiet(String MaND, String SoChungTu, String iNgayCT, String iThangCT,
            String iNgay, String iThang, String sSoTienTu, String sSoTienDen, String sTaiKhoanNo, String sTaiKhoanCo,
            String sDonViNo, String sDonViCo, int Trang = 1, int SoBanGhi = 0)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iNamLamViec = @iNamLamViec ";
            cmd.Parameters.AddWithValue("@iNamLamViec", dtCauHinh.Rows[0]["iNamLamViec"]);
            if (String.IsNullOrEmpty(SoChungTu) == false && SoChungTu != "")
            {
                DK += " AND sSoChungTuChiTiet LIKE '%" + SoChungTu + "%'";
            }
            if (String.IsNullOrEmpty(iNgayCT) == false && iNgayCT != "")
            {
                DK += " AND iNgayCT = @iNgayCT";
                cmd.Parameters.AddWithValue("@iNgayCT", iNgayCT);
            }
            if (String.IsNullOrEmpty(iThangCT) == false && iThangCT != "")
            {
                DK += " AND iThangCT = @iThangCT";
                cmd.Parameters.AddWithValue("@iThangCT", iThangCT);
            }
            if (String.IsNullOrEmpty(iNgay) == false && iNgay != "")
            {
                DK += " AND iNgay = @iNgay";
                cmd.Parameters.AddWithValue("@iNgay", iNgay);
            }
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }
            if (String.IsNullOrEmpty(sSoTienTu) == false && sSoTienTu != "")
            {
                DK += " AND rSoTien >= @sSoTienTu";
                cmd.Parameters.AddWithValue("@sSoTienTu", sSoTienTu);
            }
            if (String.IsNullOrEmpty(sSoTienDen) == false && sSoTienDen != "")
            {
                DK += " AND rSoTien <= @sSoTienDen";
                cmd.Parameters.AddWithValue("@sSoTienDen", sSoTienDen);
            }
            if (String.IsNullOrEmpty(sTaiKhoanNo) == false && sTaiKhoanNo != "")
            {
                DK += " AND iID_MaTaiKhoan_No = @iID_MaTaiKhoan_No";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_No", sTaiKhoanNo);
            }
            if (String.IsNullOrEmpty(sTaiKhoanCo) == false && sTaiKhoanCo != "")
            {
                DK += " AND iID_MaTaiKhoan_Co = @iID_MaTaiKhoan_Co";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", sTaiKhoanCo);
            }
            if (String.IsNullOrEmpty(sDonViNo) == false && sDonViNo != "")
            {
                DK += " AND iID_MaDonVi_No = @iID_MaDonVi_No";
                cmd.Parameters.AddWithValue("@iID_MaDonVi_No", sDonViNo);
            }
            if (String.IsNullOrEmpty(sDonViCo) == false && sDonViCo != "")
            {
                DK += " AND iID_MaDonVi_Co = @iID_MaDonVi_Co";
                cmd.Parameters.AddWithValue("@iID_MaDonVi_Co", sDonViCo);
            }
            String SQL = String.Format("SELECT * FROM KTKB_ChungTuChiTiet WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số chứng từ chi tiết với các tham số tìm kiếm
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="SoChungTu"></param>
        /// <param name="iNgayCT"></param>
        /// <param name="iThangCT"></param>
        /// <param name="iNgay"></param>
        /// <param name="iThang"></param>
        /// <param name="sSoTienTu"></param>
        /// <param name="sSoTienDen"></param>
        /// <param name="sTaiKhoanNo"></param>
        /// <param name="sTaiKhoanCo"></param>
        /// <param name="sDonViNo"></param>
        /// <param name="sDonViCo"></param>
        /// <returns></returns>
        public static int Get_DanhSachChungTuChiTiet_Count(String MaND, String SoChungTu, String iNgayCT, String iThangCT,
            String iNgay, String iThang, String sSoTienTu, String sSoTienDen, String sTaiKhoanNo, String sTaiKhoanCo,
            String sDonViNo, String sDonViCo)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "iNamLamViec = @iNamLamViec ";
            cmd.Parameters.AddWithValue("@iNamLamViec", dtCauHinh.Rows[0]["iNamLamViec"]);
            if (String.IsNullOrEmpty(SoChungTu) == false && SoChungTu != "")
            {
                DK += " AND sSoChungTuChiTiet LIKE '%" + SoChungTu + "%'";
            }
            if (String.IsNullOrEmpty(iNgayCT) == false && iNgayCT != "")
            {
                DK += " AND iNgayCT = @iNgayCT";
                cmd.Parameters.AddWithValue("@iNgayCT", iNgayCT);
            }
            if (String.IsNullOrEmpty(iThangCT) == false && iThangCT != "")
            {
                DK += " AND iThangCT = @iThangCT";
                cmd.Parameters.AddWithValue("@iThangCT", iThangCT);
            }
            if (String.IsNullOrEmpty(iNgay) == false && iNgay != "")
            {
                DK += " AND iNgay = @iNgay";
                cmd.Parameters.AddWithValue("@iNgay", iNgay);
            }
            if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            {
                DK += " AND iThang = @iThang";
                cmd.Parameters.AddWithValue("@iThang", iThang);
            }
            if (String.IsNullOrEmpty(sSoTienTu) == false && sSoTienTu != "")
            {
                DK += " AND rSoTien >= @sSoTienTu";
                cmd.Parameters.AddWithValue("@sSoTienTu", sSoTienTu);
            }
            if (String.IsNullOrEmpty(sSoTienDen) == false && sSoTienDen != "")
            {
                DK += " AND rSoTien <= @sSoTienDen";
                cmd.Parameters.AddWithValue("@sSoTienDen", sSoTienDen);
            }
            if (String.IsNullOrEmpty(sTaiKhoanNo) == false && sTaiKhoanNo != "")
            {
                DK += " AND iID_MaTaiKhoan_No = @iID_MaTaiKhoan_No";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_No", sTaiKhoanNo);
            }
            if (String.IsNullOrEmpty(sTaiKhoanCo) == false && sTaiKhoanCo != "")
            {
                DK += " AND iID_MaTaiKhoan_Co = @iID_MaTaiKhoan_Co";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", sTaiKhoanCo);
            }
            if (String.IsNullOrEmpty(sDonViNo) == false && sDonViNo != "")
            {
                DK += " AND iID_MaDonVi_No = @iID_MaDonVi_No";
                cmd.Parameters.AddWithValue("@iID_MaDonVi_No", sDonViNo);
            }
            if (String.IsNullOrEmpty(sDonViCo) == false && sDonViCo != "")
            {
                DK += " AND iID_MaDonVi_Co = @iID_MaDonVi_Co";
                cmd.Parameters.AddWithValue("@iID_MaDonVi_Co", sDonViCo);
            }
            String SQL = String.Format("SELECT Count(*) FROM KTKB_ChungTuChiTiet WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Kiểm tra có được cập nhập thông tin trong 'bang' hay không
        /// - Trường hợp thêm mới: số tiền = 0 thì không được thêm
        /// - Trường hợp sửa thông tin cũ:
        ///     + Trường hợp sửa thông tin chi tiết: Tài khoản nào tạo thì tài khoản đó mới được sửa
        /// </summary>
        /// <param name="bang"></param>
        /// <returns></returns>
        public static Boolean KiemTraCoDuocCapNhapBang(Bang bang, int iLoai)
        {
            Boolean vR = true;
            if (bang.DuLieuMoi)
            {
                switch (iLoai)
                {
                    case 1:
                        //Rút dự toán
                        if (bang.CmdParams.Parameters.IndexOf("@rDTRut") < 0 || Convert.ToDouble(bang.CmdParams.Parameters["@rDTRut"].Value) == 0)
                        {
                            //Trường hợp thêm mới và số tiền =0 thì không thêm
                            vR = false;
                        }
                        break;
                    case 2:
                        //Nhập số duyệt tạm ứng
                        if (bang.CmdParams.Parameters.IndexOf("@rSoTien") < 0 || Convert.ToDouble(bang.CmdParams.Parameters["@rSoTien"].Value) == 0)
                        {
                            //Trường hợp thêm mới và số tiền =0 thì không thêm
                            vR = false;
                        }
                        break;
                    case 3:
                        //Khôi phục dự toán
                        if (bang.CmdParams.Parameters.IndexOf("@rDTKhoiPhuc") < 0 || Convert.ToDouble(bang.CmdParams.Parameters["@rDTKhoiPhuc"].Value) == 0)
                        {
                            //Trường hợp thêm mới và số tiền =0 thì không thêm
                            vR = false;
                        }
                        break;
                    case 4:
                        //Hủy dự toán
                        if (bang.CmdParams.Parameters.IndexOf("@rDTHuy") < 0 || Convert.ToDouble(bang.CmdParams.Parameters["@rDTHuy"].Value) == 0)
                        {
                            //Trường hợp thêm mới và số tiền =0 thì không thêm
                            vR = false;
                        }
                        break;
                }
                
            }
            else
            {
                Boolean okSuaChiTiet = false;
                for (int i = 0; i < bang.CmdParams.Parameters.Count; i++)
                {
                    if (bang.CmdParams.Parameters[i].ParameterName != "@iSTT" &&
                        bang.CmdParams.Parameters[i].ParameterName != "@bDongY" &&
                        bang.CmdParams.Parameters[i].ParameterName != "@sLyDo" &&
                        bang.CmdParams.Parameters[i].ParameterName != "@sMauSac")
                    {
                        okSuaChiTiet = true;
                        break;
                    }
                }
                if (okSuaChiTiet)
                {
                    String iID_MaChungTuChiTiet = Convert.ToString(bang.GiaTriKhoa);
                    DataTable dt = GetChungTuChiTiet(iID_MaChungTuChiTiet);
                    if (dt.Rows.Count > 0)
                    {
                        if (bang.MaNguoiDungSua != Convert.ToString(dt.Rows[0]["sID_MaNguoiDungTao"]) && LuongCongViecModel.KiemTra_TroLyTongHop(bang.MaNguoiDungSua) == false)
                        {
                            vR = false;
                        }
                    }
                    dt.Dispose();
                }
            }
            return vR;
        }
        /// <summary>
        /// Lấy danh sách chứng từ chi tiết theo tháng
        /// </summary>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static DataTable Lay_DanhSachChungTuChiTiet_TheoThang(String iThang)
        {
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTKB_ChungTuChiTiet WHERE iThang=@iThang ORDER BY iSTT, dNgayTao");
            cmd.Parameters.AddWithValue("@iThang", iThang);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
    }
}