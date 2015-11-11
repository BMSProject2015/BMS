using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Collections.Specialized;

namespace VIETTEL.Models
{
    public class KeToanTongHop_ChungTuChiTietModels
    {
        /// <summary>
        /// Thêm dữ liệu vào bảng KT_ChungTuChiTiet
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        public static void ThemChiTiet(String iID_MaChungTu, String MaND, String IPSua)
        {
            DataTable dtChungTu = KeToanTongHop_ChungTuModels.GetChungTu(iID_MaChungTu);

            String iID_MaChiTieu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);


            Bang bang = new Bang("KT_ChungTuChiTiet");
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
        /// Thêm dữ liệu cho bảng KT_DuyetChungTu
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="NoiDung"></param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String InsertDuyetQuyetToan(String iID_MaChungTu, String NoiDung, String MaND, String IPSua)
        {
            String MaDuyetChungTu;
            Bang bang = new Bang("KT_DuyetChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            bang.DuLieuMoi = true;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", NoiDung);
            MaDuyetChungTu = Convert.ToString(bang.Save());
            return MaDuyetChungTu;
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
            DataTable dt = KeToanTongHop_ChungTuModels.GetChungTu(iID_MaChungTu);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(dt.Rows[0]["iID_MaTrangThaiDuyet"]);
            dt.Dispose();
            if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet))
            {
                int iID_MaTrangThaiDuyet_TuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(iID_MaTrangThaiDuyet);
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM KT_ChungTuChiTiet WHERE iID_MaChungTu=@iID_MaChungTu AND bDongY=0");
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
            DataTable dt = KeToanTongHop_ChungTuModels.GetChungTu(iID_MaChungTu);
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
        /// lấy thông tin chi tiết trong bảng KT_CanDoiThuChiTaiChinh 
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// lấy thông tin chi tiết một row trogn bảng KT_ChungTuChiTiet 
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
                    if (LaTroLyPhongBan==false)
                    {
                        DK += " AND sID_MaNguoiDungTao=@MaND";
                        cmd.Parameters.AddWithValue("@MaND", arrGiaTriTimKiem["MaND"]);
                    }
                }
                if (String.IsNullOrEmpty(arrGiaTriTimKiem["TrangThai"]) == false)
                {
                    if(arrGiaTriTimKiem["TrangThai"]=="1")
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
            SQL = String.Format("SELECT * FROM KT_ChungTuChiTiet WHERE {0} ORDER BY iSTT, dNgayTao", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Lấy thông tin các chứng từ chi tiết bị từ chối trong tháng
        /// </summary>
        /// <param name="iThang"></param>
        /// <param name="iNam"></param>
        /// <param name="arrGiaTriTimKiem"></param>
        /// <param name="MaND"></param>
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
            SQL = String.Format("SELECT * FROM KT_ChungTuChiTiet WHERE {0} ORDER BY sSoChungTuGhiSo, iSTT, dNgayTao", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return vR;
        }
        /// <summary>
        /// Lấy DataTable thông tin của một chứng từ chi tiết
        /// </summary>
        /// <param name="iID_MaChungTuChiTiet"></param>
        /// <returns></returns>
        public static DataTable GetChungTuChiTiet(String iID_MaChungTuChiTiet)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet");
            cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách dữ liệu trong bảng KT_ChungTuChiTiet với các tham số tìm
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
            String sDonViNo, String sDonViCo,String sNoiDung,
            String iDenNgayCT, String iDenThangCT,String iDenNgay,String iDenThang, String sNguoiTao,String sChiTietCo,String sChiTietNo,
            String sBNo, String sBCo,
            int Trang = 1, int SoBanGhi = 0)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = HamChung.ConvertToString(dtCauHinh.Rows[0]["iNamLamViec"].ToString());
            if (String.IsNullOrEmpty(iNamLamViec) == false)
            {
                iNamLamViec = DateTime.Now.Year.ToString();
            }
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai=1 AND iNamLamViec = @iNamLamViec ";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (String.IsNullOrEmpty(SoChungTu) == false && SoChungTu != "")
            {
                DK += " AND sSoChungTuChiTiet LIKE '%" + SoChungTu + "%'";
            }


            String iTuNgayCT_Search = iNamLamViec + "/" + iThangCT + "/" + iNgayCT;
            String iDenNgayCT_Search = iNamLamViec + "/" + iDenThangCT + "/" + iDenNgayCT;
            if (String.IsNullOrEmpty(iNgayCT) == false && iNgayCT != "" && String.IsNullOrEmpty(iThangCT) == false && iThangCT != ""
                    && String.IsNullOrEmpty(iDenNgayCT) == false && iDenNgayCT != "" && String.IsNullOrEmpty(iDenThangCT) == false && iDenThangCT != "")
            {

                DK += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar,iNgayCT), 111) >= @TuNgayCT)";
                DK += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @DenNgayCT) AND (iThang<>0)";
                cmd.Parameters.AddWithValue("@TuNgayCT", Convert.ToString(iTuNgayCT_Search));
                cmd.Parameters.AddWithValue("@DenNgayCT", Convert.ToString(iDenNgayCT_Search));

            }

            String iTuNgay_Search = iNamLamViec + "/" + iThang + "/" + iNgay;
            String iDenNgay_Search = iNamLamViec + "/" + iDenThang + "/" + iDenNgay;
            if (String.IsNullOrEmpty(iNgay) == false && iNgay != "" && String.IsNullOrEmpty(iThang) == false && iThang != ""
                    && String.IsNullOrEmpty(iDenNgay) == false && iDenNgay != "" && String.IsNullOrEmpty(iDenThang) == false && iDenThang != "")
            {

                DK += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar,iNgay), 111) >= @TuNgay)";
                DK += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) <= @DenNgay) AND (iThang<>0)";
                cmd.Parameters.AddWithValue("@TuNgay", Convert.ToString(iTuNgay_Search));
                cmd.Parameters.AddWithValue("@DenNgay", Convert.ToString(iDenNgay_Search));

            }

            if (String.IsNullOrEmpty(sNguoiTao) == false && sNguoiTao != "")
            {
                DK += " AND sID_MaNguoiDungTao LIKE @sNguoiTao";
                cmd.Parameters.AddWithValue("@sNguoiTao", "%" + sNguoiTao + "%");
            }
            if (String.IsNullOrEmpty(sBNo) == false && sBNo != "")
            {
                DK += " AND sTenPhongBan_No LIKE @sTenPhongBan_No OR iID_MaPhongBan_No LIKE @sTenPhongBan_No";
                cmd.Parameters.AddWithValue("@sTenPhongBan_No", "%" + sBNo + "%");
            }
            if (String.IsNullOrEmpty(sBCo) == false && sBCo != "")
            {
                DK += " AND sTenPhongBan_Co LIKE @sTenPhongBan_Co OR iID_MaPhongBan_Co LIKE @sTenPhongBan_Co";
                cmd.Parameters.AddWithValue("@sTenPhongBan_Co", "%" + sBCo + "%");
            }

            if (String.IsNullOrEmpty(sChiTietCo) == false && sChiTietCo != "")
            {
                DK += " AND sTenTaiKhoanGiaiThich_Co LIKE @sTenTaiKhoanGiaiThich_Co OR iID_MaTaiKhoanGiaiThich_Co LIKE @sTenTaiKhoanGiaiThich_Co";
                cmd.Parameters.AddWithValue("@sTenTaiKhoanGiaiThich_Co", "%" + sChiTietCo + "%");
            }
            if (String.IsNullOrEmpty(sChiTietNo) == false && sChiTietNo != "")
            {
                DK += " AND sTenTaiKhoanGiaiThich_No LIKE @sTenTaiKhoanGiaiThich_No OR iID_MaTaiKhoanGiaiThich_No LIKE @sTenTaiKhoanGiaiThich_No";
                cmd.Parameters.AddWithValue("@sTenTaiKhoanGiaiThich_No", "%" + sChiTietNo + "%");
            }
            //if (String.IsNullOrEmpty(iNgayCT) == false && iNgayCT != "")
            //{
            //    DK += " AND iNgayCT = @iNgayCT";
            //    cmd.Parameters.AddWithValue("@iNgayCT", iNgayCT);
            //}
            //if (String.IsNullOrEmpty(iThangCT) == false && iThangCT != "")
            //{
            //    DK += " AND iThangCT = @iThangCT";
            //    cmd.Parameters.AddWithValue("@iThangCT", iThangCT);
            //}
            //if (String.IsNullOrEmpty(iNgay) == false && iNgay != "")
            //{
            //    DK += " AND iNgay = @iNgay";
            //    cmd.Parameters.AddWithValue("@iNgay", iNgay);
            //}
            //if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            //{
            //    DK += " AND iThang = @iThang";
            //    cmd.Parameters.AddWithValue("@iThang", iThang);
            //}
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
                DK += " AND iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan_No";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_No", sTaiKhoanNo + "%");
            }
            if (String.IsNullOrEmpty(sTaiKhoanCo) == false && sTaiKhoanCo != "")
            {
                DK += " AND iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan_Co";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", sTaiKhoanCo + "%");
            }
            if (String.IsNullOrEmpty(sDonViNo) == false && sDonViNo != "")
            {
                DK += " AND sTenDonVi_No LIKE @sTenDonVi_No";
                cmd.Parameters.AddWithValue("@sTenDonVi_No", "%" + sDonViNo + "%");
            }
            if (String.IsNullOrEmpty(sDonViCo) == false && sDonViCo != "")
            {
                DK += " AND sTenDonVi_Co LIKE @sTenDonVi_Co";
                cmd.Parameters.AddWithValue("@sTenDonVi_Co", "%" + sDonViCo + "%");
            }
            if (String.IsNullOrEmpty(sNoiDung) == false)
            {
                DK += " AND sNoiDung LIKE @sNoiDung";
                cmd.Parameters.AddWithValue("@sNoiDung","%"+ sNoiDung + "%");
            }
            String SQL = String.Format("SELECT * FROM KT_ChungTuChiTiet WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy tổng số bản ghi trong bảng KT_ChungTuChiTiet với các tham số truyền vào
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
            String sDonViNo, String sDonViCo,String sNoiDung, String iDenNgayCT, String iDenThangCT,String iDenNgay,String iDenThang, String sNguoiTao,String sChiTietCo,String sChiTietNo,
            String sBNo, String sBCo)
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = HamChung.ConvertToString(dtCauHinh.Rows[0]["iNamLamViec"].ToString());
            if (String.IsNullOrEmpty(iNamLamViec) == false)
            {
                iNamLamViec = DateTime.Now.Year.ToString();
            }
            String DK = "iTrangThai=1 AND iNamLamViec = @iNamLamViec ";
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            if (String.IsNullOrEmpty(SoChungTu) == false && SoChungTu != "")
            {
                DK += " AND sSoChungTuChiTiet LIKE '%" + SoChungTu + "%'";
            }

            String iTuNgayCT_Search = iNamLamViec + "/" + iThangCT + "/" + iNgayCT;
            String iDenNgayCT_Search = iNamLamViec + "/" + iDenThangCT + "/" + iDenNgayCT;
            if (String.IsNullOrEmpty(iNgayCT) == false && iNgayCT != "" && String.IsNullOrEmpty(iThangCT) == false && iThangCT != ""
                    && String.IsNullOrEmpty(iDenNgayCT) == false && iDenNgayCT != "" && String.IsNullOrEmpty(iDenThangCT) == false && iDenThangCT != "")
            {

                DK += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar,iNgayCT), 111) >= @TuNgayCT)";
                DK += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= @DenNgayCT) AND (iThang<>0)";
                cmd.Parameters.AddWithValue("@TuNgayCT", Convert.ToString(iTuNgayCT_Search));
                cmd.Parameters.AddWithValue("@DenNgayCT", Convert.ToString(iDenNgayCT_Search));

            }

            String iTuNgay_Search = iNamLamViec + "/" + iThang + "/" + iNgay;
            String iDenNgay_Search = iNamLamViec + "/" + iDenThang + "/" + iDenNgay;
            if (String.IsNullOrEmpty(iNgay) == false && iNgay != "" && String.IsNullOrEmpty(iThang) == false && iThang != ""
                    && String.IsNullOrEmpty(iDenNgay) == false && iDenNgay != "" && String.IsNullOrEmpty(iDenThang) == false && iDenThang != "")
            {

                DK += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar,iNgay), 111) >= @TuNgay)";
                DK += " AND (CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThang) + '/' + CONVERT(varchar, iNgay), 111) <= @DenNgay) AND (iThang<>0)";
                cmd.Parameters.AddWithValue("@TuNgay", Convert.ToString(iTuNgay_Search));
                cmd.Parameters.AddWithValue("@DenNgay", Convert.ToString(iDenNgay_Search));

            }
            if (String.IsNullOrEmpty(sNguoiTao) == false && sNguoiTao != "")
            {
                DK += " AND sID_MaNguoiDungTao LIKE @sNguoiTao";
                cmd.Parameters.AddWithValue("@sNguoiTao", "%" + sNguoiTao + "%");
            }
            if (String.IsNullOrEmpty(sBNo) == false && sBNo != "")
            {
                DK += " AND sTenPhongBan_No LIKE @sTenPhongBan_No OR iID_MaPhongBan_No LIKE @sTenPhongBan_No";
                cmd.Parameters.AddWithValue("@sTenPhongBan_No", "%" + sBNo + "%");
            }
            if (String.IsNullOrEmpty(sBCo) == false && sBCo != "")
            {
                DK += " AND sTenPhongBan_Co LIKE @sTenPhongBan_Co OR iID_MaPhongBan_Co LIKE @sTenPhongBan_Co";
                cmd.Parameters.AddWithValue("@sTenPhongBan_Co", "%" + sBCo + "%");
            }

            if (String.IsNullOrEmpty(sChiTietCo) == false && sChiTietCo != "")
            {
                DK += " AND sTenTaiKhoanGiaiThich_Co LIKE @sTenTaiKhoanGiaiThich_Co OR iID_MaTaiKhoanGiaiThich_Co LIKE @sTenTaiKhoanGiaiThich_Co";
                cmd.Parameters.AddWithValue("@sTenTaiKhoanGiaiThich_Co", "%" + sChiTietCo + "%");
            }
            if (String.IsNullOrEmpty(sChiTietNo) == false && sChiTietNo != "")
            {
                DK += " AND sTenTaiKhoanGiaiThich_No LIKE @sTenTaiKhoanGiaiThich_No OR iID_MaTaiKhoanGiaiThich_No LIKE @sTenTaiKhoanGiaiThich_No";
                cmd.Parameters.AddWithValue("@sTenTaiKhoanGiaiThich_No", "%" + sChiTietNo + "%");
            }
            //if (String.IsNullOrEmpty(iNgayCT) == false && iNgayCT != "")
            //{
            //    DK += " AND iNgayCT = @iNgayCT";
            //    cmd.Parameters.AddWithValue("@iNgayCT", iNgayCT);
            //}
            //if (String.IsNullOrEmpty(iThangCT) == false && iThangCT != "")
            //{
            //    DK += " AND iThangCT = @iThangCT";
            //    cmd.Parameters.AddWithValue("@iThangCT", iThangCT);
            //}
            //if (String.IsNullOrEmpty(iNgay) == false && iNgay != "")
            //{
            //    DK += " AND iNgay = @iNgay";
            //    cmd.Parameters.AddWithValue("@iNgay", iNgay);
            //}
            //if (String.IsNullOrEmpty(iThang) == false && iThang != "")
            //{
            //    DK += " AND iThang = @iThang";
            //    cmd.Parameters.AddWithValue("@iThang", iThang);
            //}
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
                DK += " AND iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan_No";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_No", sTaiKhoanNo + "%");
            }
            if (String.IsNullOrEmpty(sTaiKhoanCo) == false && sTaiKhoanCo != "")
            {
                DK += " AND iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan_Co";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", sTaiKhoanCo + "%");
            }
            if (String.IsNullOrEmpty(sDonViNo) == false && sDonViNo != "")
            {
                DK += " AND sTenDonVi_No LIKE @sTenDonVi_No";
                cmd.Parameters.AddWithValue("@sTenDonVi_No", "%" + sDonViNo + "%");
            }
            if (String.IsNullOrEmpty(sDonViCo) == false && sDonViCo != "")
            {
                DK += " AND sTenDonVi_Co LIKE @sTenDonVi_Co";
                cmd.Parameters.AddWithValue("@sTenDonVi_Co", "%" + sDonViCo + "%");
            }
            if (String.IsNullOrEmpty(sNoiDung) == false)
            {
                DK += " AND sNoiDung LIKE @sNoiDung";
                cmd.Parameters.AddWithValue("@sNoiDung", "%" + sNoiDung + "%");
            }
            String SQL = String.Format("SELECT Count(*) FROM KT_ChungTuChiTiet WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        public static int Lay_DanhSachChungTuChiTiet_TheoThang(String iID_MaChungTu, String iThang, String iNam, String iID_MaChungTuChiTiet, int iLoai)
        {
            int vR = 0;
            SqlCommand cmd;
            switch (iLoai) {
                case 1: //Danh sách giá trị kho bạc
                   // cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iID_MaChungTu=@iID_MaChungTu AND iThang=@iThang AND iID_MaChungTuChiTiet_KhoBac = @iID_MaChungTuChiTiet_KhoBac AND iTrangThai = 1");
                    cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaChungTuChiTiet_KhoBac = @iID_MaChungTuChiTiet_KhoBac AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    //cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_KhoBac", iID_MaChungTuChiTiet);
                    break;
                case 2: //Danh sách giá trị tiền gửi
                    //cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iID_MaChungTu=@iID_MaChungTu AND iThang=@iThang AND iID_MaChungTuChiTiet_TienGui = @iID_MaChungTuChiTiet_TienGui AND iTrangThai = 1");
                    cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec  AND iThang=@iThang AND iID_MaChungTuChiTiet_TienGui = @iID_MaChungTuChiTiet_TienGui AND iTrangThai = 1");
                   
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    //cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienGui", iID_MaChungTuChiTiet);
                    break;
                case 3: //Danh sách giá trị  tiền mặt
                    //cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iID_MaChungTu=@iID_MaChungTu AND iThang=@iThang AND iID_MaChungTuChiTiet_TienMat = @iID_MaChungTuChiTiet_TienMat AND iTrangThai = 1");
                    cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaChungTuChiTiet_TienMat = @iID_MaChungTuChiTiet_TienMat AND iTrangThai = 1");
                   
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    //cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienMat", iID_MaChungTuChiTiet);
                    break;
                case 4: //Danh sách giá trị quyết toán
                   // cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iID_MaChungTu=@iID_MaChungTu AND iThang=@iThang AND iID_MaChungTuChiTiet_QuyetToan = @iID_MaChungTuChiTiet_QuyetToan AND iTrangThai = 1");
                    cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec  AND iThang=@iThang AND iID_MaChungTuChiTiet_QuyetToan = @iID_MaChungTuChiTiet_QuyetToan AND iTrangThai = 1");
                    
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    //cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_QuyetToan", iID_MaChungTuChiTiet);
                    break;
                default: //Danh sách giá trị  tiền mặt
                    //cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iID_MaChungTu=@iID_MaChungTu AND iThang=@iThang AND iID_MaChungTuChiTiet_TienMat = @iID_MaChungTuChiTiet_TienMat AND iTrangThai = 1");
                    cmd = new SqlCommand("SELECT COUNT(*) FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaChungTuChiTiet_TienMat = @iID_MaChungTuChiTiet_TienMat AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    //cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienMat", iID_MaChungTuChiTiet);
                    break;
            }
            vR = Convert.ToInt32( Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách chứng từ chi tiết tiền gửi theo tháng
        /// </summary>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static DataTable Lay_DanhSachChungTuChiTietTienGui_TheoThang(String iNam, String iThang)
        {
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTTG_ChungTuChiTiet WHERE iNamLamViec = @iNamLamViec AND iThangCT=@iThang AND iTrangThai = 1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ORDER BY iSTT, dNgayTao");
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        //public static DataTable Lay_DanhSachChungTuChiTietTienGui_TheoThang(String iNam, String iThang, String Loai)
        //{
        //    string SQL = "SELECT * FROM KTTG_ChungTuChiTiet WHERE iNamLamViec = @iNamLamViec AND iThangCT=@iThang AND iTrangThai = 1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
        //    if (Loai == "true") SQL += " AND iID_MaChungTuChiTiet NOT IN (select iID_MaChungTuChiTiet_TienGui from KT_ChungTuChiTiet WHERE iTrangThai = 1)";
        //    SQL += "  ORDER BY iSTT, dNgayTao";
        //    SqlCommand cmd = new SqlCommand(SQL);
        //    DataTable vR = null;           
        //    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
        //    cmd.Parameters.AddWithValue("@iThang", iThang);
        //    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
        //    vR = Connection.GetDataTable(cmd);
        //    cmd.Dispose();
        //    return vR;
        //}

        public static DataTable Lay_DanhSachChungTuChiTietTienGui_TheoThang(String iNam, String iThang, String Loai)
        {
            string SQL = "SELECT * FROM KTTG_ChungTu WHERE iNamLamViec = @iNamLamViec AND iThang=@iThang AND iTrangThai = 1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            if (Loai == "true") SQL += " AND iID_MaChungTu NOT IN (select iID_MaChungTu_TienGui from KT_ChungTuChiTiet WHERE iTrangThai = 1)";
            SQL += "  ORDER BY iSTT, dNgayTao";
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable vR = null;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách chứng từ chi tiết tiền mặt theo tháng
        /// </summary>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static DataTable Lay_DanhSachChungTuChiTietTienMat_TheoThang(String iNam, String iThang)
        {
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTTM_ChungTuChiTiet WHERE iNamLamViec = @iNamLamViec AND iThangCT=@iThang AND iTrangThai = 1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ORDER BY iSTT, dNgayTao");
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }


        public static DataTable Lay_DanhSachChungTuChiTietTienMat_TheoThang(String iNam, String iThang, String Loai)
        {
            DataTable vR = null;
            string SQL = "SELECT * FROM KTTM_ChungTuChiTiet WHERE iNamLamViec = @iNamLamViec AND iThangCT=@iThang AND iTrangThai = 1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            if (Loai == "true") SQL += " AND iID_MaChungTuChiTiet NOT IN (select iID_MaChungTuChiTiet_TienMat from KT_ChungTuChiTiet WHERE iTrangThai = 1)";
            SQL += "  ORDER BY iSTT, dNgayTao";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách chứng từ chi tiết kho bạc theo tháng
        /// </summary>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static DataTable Lay_DanhSachChungTuChiTietKhoBac_TheoThang(String iNam, String iThang)
        {
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM KTKB_ChungTuChiTiet WHERE iNamLamViec = @iNamLamViec AND iThangCT=@iThang AND iTrangThai = 1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ORDER BY iSTT, dNgayTao");
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        public static DataTable Lay_DanhSachChungTuChiTietKhoBac_TheoThang(String iNam, String iThang, String Loai)
        {
            string SQL = "SELECT * FROM KTKB_ChungTuChiTiet WHERE iNamLamViec = @iNamLamViec AND iThangCT=@iThang AND iTrangThai = 1 AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            if (Loai == "true") SQL += " AND iID_MaChungTuChiTiet NOT IN (select iID_MaChungTuChiTiet_KhoBac from KT_ChungTuChiTiet WHERE iTrangThai = 1)";
            SQL += "  ORDER BY iSTT, dNgayTao";
            DataTable vR = null;
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách loại ngân sách chi tiết và giá trị tổng tiền của từng loại theo tháng
        /// </summary>
        /// <param name="iThang"></param>
        /// <returns></returns>
        public static DataTable Lay_DanhSachChungTuChiTietQuyetToan_TheoThang(String iNam, String iThang)
        {
            DataTable vR = null;
            SqlCommand cmd;
            String SQL = "";
            SQL += "SELECT CTCT.iID_MaChungTuChiTiet,CT.sTienToChungTu,CT.iSoChungTu,CTCT.iID_MaMucLucNganSach,CTCT.sLNS,CTCT.sMoTa,CTCT.iThang_Quy,CTCT.bLoaiThang_Quy,CTCT.rTongSo AS rSoTien " +
                    "FROM QTA_ChungTuChiTiet AS CTCT INNER JOIN QTA_ChungTu AS CT ON CTCT.iID_MaChungTu = CT.iID_MaChungTu " +
                    "WHERE LEN(CTCT.sLNS)=7 AND sL='' AND CTCT.iNamLamViec = @iNamLamViec AND CTCT.iThang_Quy = @iThang_Quy AND CTCT.bLoaiThang_Quy = 0 " +  //AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
                    "ORDER BY CTCT.sLNS";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
            cmd.Parameters.AddWithValue("@iThang_Quy", iThang);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeQuyetToan));
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            vR.Columns.Add("iID_MaTaiKhoan_No");
            vR.Columns.Add("iID_MaTaiKhoan_Co");
            vR.Columns.Add("sTenTaiKhoan_No");
            vR.Columns.Add("sTenTaiKhoan_Co");

            DataRow R;
            for (int i = 0; i < vR.Rows.Count; i++) {
                R = vR.Rows[i];
                DataTable dt;
                cmd = new SqlCommand("SELECT iID_MaTaiKhoan_No, iID_MaTaiKhoan_Co FROM NS_MucLucNganSach WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach ");
                cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", Convert.ToString(R["iID_MaMucLucNganSach"]));
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();

                R["iID_MaTaiKhoan_No"] = dt.Rows[0]["iID_MaTaiKhoan_No"];
                R["iID_MaTaiKhoan_Co"] = dt.Rows[0]["iID_MaTaiKhoan_Co"];

                R["sTenTaiKhoan_No"] = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(Convert.ToString(dt.Rows[0]["iID_MaTaiKhoan_No"]));
                R["sTenTaiKhoan_Co"] = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(Convert.ToString(dt.Rows[0]["iID_MaTaiKhoan_Co"]));

                dt.Dispose();
            }
            return vR;
        }
        /// <summary>
        /// Lấy thông tin chi tiết một chứng từ chi tiết quyết toán để thêm vào bảng kế toán tổng hợp
        /// </summary>
        /// <param name="iID_MaChungTuChiTiet"></param>
        /// <returns></returns>
        public static DataTable Lay_DanhSachChungTuChiTietQuyetToan_TheoMaQuyetToan(String iID_MaChungTuChiTiet)
        {
            DataTable vR = null;
            SqlCommand cmd;
            String SQL = "";
            SQL += "SELECT CTCT.*,CT.sTienToChungTu,CT.iSoChungTu " +
                    "FROM QTA_ChungTuChiTiet AS CTCT INNER JOIN QTA_ChungTu AS CT ON CTCT.iID_MaChungTu = CT.iID_MaChungTu " +
                    "WHERE CTCT.iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet ";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", iID_MaChungTuChiTiet);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            vR.Columns.Add("iID_MaTaiKhoan_No");
            vR.Columns.Add("iID_MaTaiKhoan_Co");
            vR.Columns.Add("sTenTaiKhoan_No");
            vR.Columns.Add("sTenTaiKhoan_Co");

            DataRow R;
            for (int i = 0; i < vR.Rows.Count; i++)
            {
                R = vR.Rows[i];
                DataTable dt;
                cmd = new SqlCommand("SELECT iID_MaTaiKhoan_No, iID_MaTaiKhoan_Co FROM NS_MucLucNganSach WHERE iID_MaMucLucNganSach=@iID_MaMucLucNganSach ");
                cmd.Parameters.AddWithValue("@iID_MaMucLucNganSach", Convert.ToString(R["iID_MaMucLucNganSach"]));
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();

                R["iID_MaTaiKhoan_No"] = dt.Rows[0]["iID_MaTaiKhoan_No"];
                R["iID_MaTaiKhoan_Co"] = dt.Rows[0]["iID_MaTaiKhoan_Co"];

                R["sTenTaiKhoan_No"] = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(Convert.ToString(dt.Rows[0]["iID_MaTaiKhoan_No"]));
                R["sTenTaiKhoan_Co"] = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(Convert.ToString(dt.Rows[0]["iID_MaTaiKhoan_Co"]));

                dt.Dispose();
            }
            return vR;
        }
        /// <summary>
        /// Gép chuỗi giá trị chứng từ ghi sổ trong bảng KT_ChungTuChiTiet
        /// </summary>
        /// <param name="iID_MaChungTuChiTiet"></param>
        /// <param name="iThang"></param>
        /// <param name="iNam"></param>
        /// <param name="iLoai"></param>
        /// <returns></returns>
        public static String LayChuoiChungTuGhiSoCuaChungTuChiTiet(String iID_MaChungTuChiTiet, String iThang, String iNam, int iLoai)
        {
            String vR = "";
            SqlCommand cmd;
            switch (iLoai)
            {
                case 1: //Danh sách giá trị kho bạc
                    cmd = new SqlCommand("SELECT iID_MaChungTu FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaChungTuChiTiet_KhoBac = @iID_MaChungTuChiTiet_KhoBac AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_KhoBac", iID_MaChungTuChiTiet);
                    break;
                case 2: //Danh sách giá trị tiền gửi
                    cmd = new SqlCommand("SELECT iID_MaChungTu FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaChungTuChiTiet_TienGui = @iID_MaChungTuChiTiet_TienGui AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienGui", iID_MaChungTuChiTiet);
                    break;
                case 3: //Danh sách giá trị  tiền mặt
                    cmd = new SqlCommand("SELECT iID_MaChungTu FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaChungTuChiTiet_TienMat = @iID_MaChungTuChiTiet_TienMat AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienMat", iID_MaChungTuChiTiet);
                    break;
                case 4: //Danh sách giá trị quyết toán
                    cmd = new SqlCommand("SELECT iID_MaChungTu FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaChungTuChiTiet_QuyetToan = @iID_MaChungTuChiTiet_QuyetToan AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_QuyetToan", iID_MaChungTuChiTiet);
                    break;
                case 5: //Danh sách giá trị công sản
                    cmd = new SqlCommand("SELECT iID_MaChungTu FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaChungTuChiTiet_CongSan = @iID_MaChungTuChiTiet_CongSan AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_CongSan", iID_MaChungTuChiTiet);
                    break;
                default: //Danh sách giá trị  tiền mặt
                    cmd = new SqlCommand("SELECT iID_MaChungTu FROM KT_ChungTuChiTiet WHERE iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaChungTuChiTiet_TienMat = @iID_MaChungTuChiTiet_TienMat AND iTrangThai = 1");
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNam);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienMat", iID_MaChungTuChiTiet);
                    break;
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            for (int i = 0; i < dt.Rows.Count; i++) {
                NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(Convert.ToString(dt.Rows[i]["iID_MaChungTu"]));
                String strTenChungTu = data["sSoChungTu"];
                vR += ";" + strTenChungTu;
            }
            if (vR.Length > 1) vR = vR.Substring(1, vR.Length - 1);
            if (vR.Length > 0) vR = "[" + vR + "]";
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
        public static Boolean KiemTraCoDuocCapNhapBang(Bang bang)
        {
            Boolean vR = true;
            if (bang.DuLieuMoi)
            {
                if (bang.CmdParams.Parameters.IndexOf("@rSoTien")<0 ||Convert.ToDouble(bang.CmdParams.Parameters["@rSoTien"].Value) == 0)
                {
                    //Trường hợp thêm mới và số tiền =0 thì không thêm
                    vR = false;
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


        //tuannn
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="iThang"></param>
        /// <param name="iLoai">Loai =0 kế toán tổng hợp,1 kế toán kho bạc,2 Tiền gửi,3 tiền mặt </param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuKeToan(String iNamLamViec, String iThang, int iLoai=0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM {0} WHERE iTrangThai = 1 AND iNamLamViec=@iNamLamViec AND iThang=@iThang AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            switch (iLoai)
            {
                case 0: //Danh sách chứng từ KTTH KT_ChungTu
                    SQL = String.Format(SQL, "KT_ChungTu");
                    break;
                case 1: //Danh sách chứng từ kho bạc KTKB_ChungTu
                    SQL = String.Format(SQL, "KTKB_ChungTu");
                    break;
                case 2: //Danh sách chứng từ tiền gửi KTTG_ChungTu
                    SQL = String.Format(SQL, "KTTG_ChungTu");
                    break;
                case 3: //Danh sách chứng từ  tiền mặt KTTM_ChungTu
                    SQL = String.Format(SQL, "KTTM_ChungTu");
                    break;
                
            }            
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <param name="iThang"></param>
        /// <param name="iLoai">Loai =0 kế toán tổng hợp,1 kế toán kho bạc,2 Tiền gửi,3 tiền mặt </param>
        /// <returns></returns>
        public static DataTable LayDanhSachChungTuDaNhan(String iNamLamViec, String iThang, int iLoai = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT {0} FROM KT_ChungTu WHERE iTrangThai = 1 AND {1} AND iNamLamViec=@iNamLamViec AND iThang=@iThang ";
            switch (iLoai)
            {
                case 0: //Danh sách chứng từ KTTH KT_ChungTu
                    SQL = String.Format(SQL, "iID_MaChungTu,sSoChungTu", " iID_MaChungTu IS NOT Null ");
                    break;
                case 1: //Danh sách chứng từ kho bạc KTKB_ChungTu
                    SQL = String.Format(SQL, "iID_MaChungTu_KhoBac,sSoChungTu", " iID_MaChungTu_KhoBac IS NOT Null ");
                    break;
                case 2: //Danh sách chứng từ tiền gửi KTTG_ChungTu
                    SQL = String.Format(SQL, "iID_MaChungTu_TienGui,sSoChungTu", " iID_MaChungTu_TienGui IS NOT Null ");
                    break;
                case 3: //Danh sách chứng từ  tiền mặt KTTM_ChungTu
                    SQL = String.Format(SQL, "iID_MaChungTu_TienMat,sSoChungTu", " iID_MaChungTu_TienMat IS NOT Null ");
                    break;

            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iThang);            

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        public static DataTable LayDanhSachChiTietChungTuNhan(String iID_MaChungTu, int iLoai)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM {0} WHERE iTrangThai = 1 AND iID_MaChungTu=@iID_MaChungTu AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            switch (iLoai)
            {
                case 0: //Danh sách chứng từ KTTH KT_ChungTu
                    SQL = String.Format(SQL, "KT_ChungTuChiTiet");
                    break;
                case 1: //Danh sách chứng từ kho bạc KTKB_ChungTu
                    SQL = String.Format(SQL, "KTKB_ChungTuChiTiet");
                    break;
                case 2: //Danh sách chứng từ tiền gửi KTTG_ChungTu
                    SQL = String.Format(SQL, "KTTG_ChungTuChiTiet");
                    break;
                case 3: //Danh sách chứng từ  tiền mặt KTTM_ChungTu
                    SQL = String.Format(SQL, "KTTM_ChungTuChiTiet");
                    break;

            }
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

    }
}