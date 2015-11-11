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
    public class CanBo_HoSoNhanSuModels
    {

        public static DataTable Get_DanhSach(String iID_MaDonVi = "", String iID_MaTinhTrangCanBo = "", String sMaHoSo = "", String sHoTen = "", String sSoHieuCBCC = "",
            String iID_MaChucVu = "", String iID_MaDT = "", String iID_MaTrinhDo = "", String iTuoiTu = "", String iDenTuoi = "", String iID_MaNgach = "", String iID_MaTrinhDoLyLuanCT = "", int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT NS.iID_MaCanBo, NS.sHoDem + ' ' + NS.sTen AS HoTen, NS.sSoHieuCBCC, NS.dNgaySinh," +
               " NS.iID_MaPhongBan, NS.sID_ChucVuHienTai, NS.sID_MaTrinhDoChuyenMonCaoNhat, NS.iID_MaNgachLuong, NS.iID_MaDoiTuong, DV.sTen AS TenDonVi, L.sTenNgachLuong, NS.bGioiTinh" +
               " FROM CB_CanBo AS NS, NS_DonVi AS DV, L_DanhMucNgachLuong AS L WHERE NS.iTrangThai=1 AND NS.iID_MaDonVi = DV.iID_MaDonVi AND NS.iID_MaNgachLuong = L.iID_MaNgachLuong");

            if (String.IsNullOrEmpty(iID_MaDonVi) == false && iID_MaDonVi != "")
            {
                SQL += " AND NS.iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (String.IsNullOrEmpty(iID_MaTinhTrangCanBo) == false && iID_MaTinhTrangCanBo != "-1")
            {
                SQL += " AND NS.iID_MaTinhTrangCanBo = @iID_MaTinhTrangCanBo";
                cmd.Parameters.AddWithValue("@iID_MaTinhTrangCanBo", iID_MaTinhTrangCanBo);
            }
            if (String.IsNullOrEmpty(sMaHoSo) == false && sMaHoSo != "")
            {
                SQL += " AND NS.sMaSoHoSo like N'%' +  @sMaSoHoSo + '%'";
                cmd.Parameters.AddWithValue("@sMaSoHoSo", sMaHoSo);
            }
            if (String.IsNullOrEmpty(sHoTen) == false && sHoTen != "")
            {
                SQL += " AND NS.sTuKhoa like N'%' +  @sHoTen + '%'";
                cmd.Parameters.AddWithValue("@sHoTen", sHoTen);
            }
            if (String.IsNullOrEmpty(sSoHieuCBCC) == false && sSoHieuCBCC != "")
            {
                SQL += " AND NS.sSoHieuCBCC like N'%' +  @sSoHieuCBCC + '%'";
                cmd.Parameters.AddWithValue("@sSoHieuCBCC", sSoHieuCBCC);
            }

            if (String.IsNullOrEmpty(iID_MaChucVu) == false && iID_MaChucVu != Convert.ToString(Guid.Empty))
            {
                SQL += " AND NS.sID_ChucVuHienTai = @iID_ChucVuHienTai";
                cmd.Parameters.AddWithValue("@iID_ChucVuHienTai", iID_MaChucVu);
            }
            if (String.IsNullOrEmpty(iID_MaDT) == false && iID_MaDT != Convert.ToString(Guid.Empty))
            {
                SQL += " AND NS.iID_MaDoiTuong = @iID_MaDT";
                cmd.Parameters.AddWithValue("@iID_MaDT", iID_MaDT);
            }

            if (String.IsNullOrEmpty(iID_MaTrinhDo) == false && iID_MaTrinhDo != Convert.ToString(Guid.Empty))
            {
                SQL += " AND NS.sID_MaTrinhDoChuyenMonCaoNhat = @iID_MaTrinhDo";
                cmd.Parameters.AddWithValue("@iID_MaTrinhDo", iID_MaTrinhDo);
            }
            if (String.IsNullOrEmpty(iID_MaNgach) == false && iID_MaNgach != Convert.ToString(Guid.Empty))
            {
                SQL += " AND NS.iID_MaNgachLuong = @iID_MaNgach";
                cmd.Parameters.AddWithValue("@iID_MaNgach", iID_MaNgach);
            }

            if (String.IsNullOrEmpty(iID_MaTrinhDoLyLuanCT) == false && iID_MaTrinhDoLyLuanCT != Convert.ToString(Guid.Empty))
            {
                SQL += " AND NS.iID_MaTrinhDoLyLuanChinhTri = @iID_MaTrinhDoLyLuanCT";
                cmd.Parameters.AddWithValue("@iID_MaTrinhDoLyLuanCT", iID_MaTrinhDoLyLuanCT);
            }
            if (String.IsNullOrEmpty(iTuoiTu) == false && iTuoiTu != "")
            {
                SQL += " AND YEAR(getdate()) - YEAR(NS.dNgaySinh) >=@iTuoiTu";
                cmd.Parameters.AddWithValue("@iTuoiTu", iTuoiTu);
            }
            if (String.IsNullOrEmpty(iDenTuoi) == false && iDenTuoi != "")
            {
                SQL += " AND YEAR(getdate()) - YEAR(NS.dNgaySinh) <=@iDenTuoi";
                cmd.Parameters.AddWithValue("@iDenTuoi", iDenTuoi);
            }

            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "NS.sTen, NS.sHoDem DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_DanhSach_Count(String iID_MaDonVi = "", String iID_MaTinhTrangCanBo = "", String sMaHoSo = "", String sHoTen = "", String sSoHieuCBCC = "",
            String iID_MaChucVu = "", String iID_MaDT = "", String iID_MaTrinhDo = "", String iTuoiTu = "", String iDenTuoi = "", String iID_MaNgach = "", String iID_MaTrinhDoLyLuanCT = "")
        {
            int vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "1=1";
            if (String.IsNullOrEmpty(iID_MaDonVi) == false && iID_MaDonVi != "")
            {
                DK += " AND iID_MaDonVi = @iID_MaDonVi";
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            if (String.IsNullOrEmpty(iID_MaTinhTrangCanBo) == false && iID_MaTinhTrangCanBo != "-1")
            {
                DK += " AND iID_MaTinhTrangCanBo = @iID_MaTinhTrangCanBo";
                cmd.Parameters.AddWithValue("@iID_MaTinhTrangCanBo", iID_MaTinhTrangCanBo);
            }
            if (String.IsNullOrEmpty(sMaHoSo) == false && sMaHoSo != "")
            {
                DK += " AND sMaSoHoSo like N'%' +  @sMaSoHoSo + '%'";
                cmd.Parameters.AddWithValue("@sMaSoHoSo", sMaHoSo);
            }
            if (String.IsNullOrEmpty(sHoTen) == false && sHoTen != "")
            {
                DK += " AND sTuKhoa like N'%' +  @sHoTen + '%'";
                cmd.Parameters.AddWithValue("@sHoTen", sHoTen);
            }
            if (String.IsNullOrEmpty(sSoHieuCBCC) == false && sSoHieuCBCC != "")
            {
                DK += " AND sSoHieuCBCC like N'%' +  @sSoHieuCBCC + '%'";
                cmd.Parameters.AddWithValue("@sSoHieuCBCC", sSoHieuCBCC);
            }

            if (String.IsNullOrEmpty(iID_MaChucVu) == false && iID_MaChucVu != Convert.ToString(Guid.Empty))
            {
                DK += " AND sID_ChucVuHienTai = @iID_ChucVuHienTai";
                cmd.Parameters.AddWithValue("@iID_ChucVuHienTai", iID_MaChucVu);
            }
            if (String.IsNullOrEmpty(iID_MaDT) == false && iID_MaDT != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaDoiTuong = @iID_MaDT";
                cmd.Parameters.AddWithValue("@iID_MaDT", iID_MaDT);
            }

            if (String.IsNullOrEmpty(iID_MaTrinhDo) == false && iID_MaTrinhDo != Convert.ToString(Guid.Empty))
            {
                DK += " AND sID_MaTrinhDoChuyenMonCaoNhat = @iID_MaTrinhDo";
                cmd.Parameters.AddWithValue("@iID_MaTrinhDo", iID_MaTrinhDo);
            }
            if (String.IsNullOrEmpty(iID_MaNgach) == false && iID_MaNgach != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaNgachLuong = @iID_MaNgach";
                cmd.Parameters.AddWithValue("@iID_MaNgach", iID_MaNgach);
            }

            if (String.IsNullOrEmpty(iID_MaTrinhDoLyLuanCT) == false && iID_MaTrinhDoLyLuanCT != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaTrinhDoLyLuanChinhTri = @iID_MaTrinhDoLyLuanCT";
                cmd.Parameters.AddWithValue("@iID_MaTrinhDoLyLuanCT", iID_MaTrinhDoLyLuanCT);
            }
            if (String.IsNullOrEmpty(iTuoiTu) == false && iTuoiTu != "")
            {
                DK += " AND YEAR(getdate()) - YEAR(dNgaySinh) >=@iTuoiTu";
                cmd.Parameters.AddWithValue("@iTuoiTu", iTuoiTu);
            }
            if (String.IsNullOrEmpty(iDenTuoi) == false && iDenTuoi != "")
            {
                DK += " AND YEAR(getdate()) - YEAR(dNgaySinh) <=@iDenTuoi";
                cmd.Parameters.AddWithValue("@iDenTuoi", iDenTuoi);
            }

            String SQL = String.Format("SELECT Count(*) FROM CB_CanBo WHERE {0}", DK);
            cmd.CommandText = SQL;
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy chi tiết thông tin cán bộ
        /// </summary>
        /// <param name="iID_MaCanBo">Mã cán bộ</param>
        /// <returns></returns>
        public static DataTable GetChiTiet(string iID_MaCanBo)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CB_CanBo WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo");
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Kiem tra ma nhan su
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static Boolean CheckMaNhanSu(string iID_MaCanBo)
        {
            Boolean vR = false;
            string SQL = "SELECT Count(*) FROM L_BangLuongChiTiet WHERE iID_MaCanBo=@iID_MaCanBo";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            cmd.CommandText = SQL;
            if (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0)
            {
                vR = true;
            }
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Kiểm tra có trùng số sổ lương hay không>
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <param name="sSoSoLuong_CanBo"></param>
        /// <returns>
        ///     + True: Nếu trùng số sổ lương
        ///     + False: Nếu không trùng số sổ lương
        /// </returns>
        public static Boolean KiemTraTrung_sSoSoLuong(String iID_MaCanBo, String sSoSoLuong_CanBo)
        {
            Boolean vR = false;
            if (String.IsNullOrEmpty(sSoSoLuong_CanBo) == false)
            {
                SqlCommand cmd = new SqlCommand();
                String DK = "";

                DK += " AND sSoSoLuong_CanBo=@sSoSoLuong_CanBo";
                cmd.Parameters.AddWithValue("@sSoSoLuong_CanBo", sSoSoLuong_CanBo);

                if (String.IsNullOrEmpty(iID_MaCanBo) == false)
                {
                    DK += " AND iID_MaCanBo<>@iID_MaCanBo";
                    cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
                }

                cmd.CommandText = String.Format("SELECT COUNT(*) FROM L_BangLuongChiTiet WHERE iTrangThai=1 {0}", DK);
                vR = Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0;
                cmd.Dispose();
            }
            return vR;
        }
        public static string GhepNgayThangNam(String strNgay)
        {
            String HaiSoDau, HaiSoCuoi;
            HaiSoDau = strNgay.Substring(0, 2);
            HaiSoCuoi = strNgay.Substring(3, 2);
            String Nam;
            if (HaiSoCuoi.IndexOf("0") == 0)
            {
                Nam = "200" + HaiSoCuoi.Substring(1, 1);
            }
            else if (Convert.ToInt16(HaiSoCuoi) <= 99 && Convert.ToInt16(HaiSoCuoi) >= 40)
            {
                Nam = "19" + HaiSoCuoi;
            }
            else
            {
                Nam = "20" + HaiSoCuoi;
            }
            return Nam + "/" + HaiSoDau + "/01";
        }
        public static bool isDateTime(string date)
        {
            try {

                DateTime dt = Convert.ToDateTime(date);
                return true;
            }
            catch {
                return false;
            }
        }
        /// <summary>
        /// Kiểm tra tham số để thêm sửa nhanh cán bộ có đúng hay không
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Values"></param>
        /// <param name="arrLoi"></param>
        /// <returns></returns>
        public static Boolean KiemTraDuocPhepSuaNhanhCanBo(string ParentID, String iID_MaCanBo, NameValueCollection Values, ref NameValueCollection arrLoi)
        {
            Boolean okKhongCoLoi = true;
            String iID_MaDonVi = Values[ParentID + "_iID_MaDonVi"];
            String sSoSoLuong = Values[ParentID + "_sSoSoLuong_CanBo"];
            String iID_MaBacLuong = Values[ParentID + "_iID_MaBacLuong_CanBo"];
            String iID_MaNgachLuong = Values[ParentID + "_iID_MaNgachLuong_CanBo"];
            String rLuongCoBan_HeSo = Values[ParentID + "_rLuongCoBan_HeSo_CanBo"];
            String dNgayNhapNgu = Values[ParentID + "_dNgayNhapNgu_CanBo"];

            String dNgayXuatNgu = Values[ParentID + "_dNgayXuatNgu_CanBo"];
            String dNgayTaiNgu = Values[ParentID + "_dNgayTaiNgu_CanBo"];

            String iNuQuanNhan_CanBo = Values[ParentID + "_iNuQuanNhan_CanBo"];

            String sHoVaTen_CanBo = Values[ParentID + "_sHoVaTen_CanBo"];

            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_iID_MaDonVi", NgonNgu.LayXau("Phải nhập Đơn vị."));
            }
            if (okKhongCoLoi && KiemTraTrung_sSoSoLuong(iID_MaCanBo, sSoSoLuong))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_sSoSoLuong", NgonNgu.LayXau("Trùng Số sổ lương."));
            }
            //if (String.IsNullOrEmpty(sSoSoLuong))
            //{
            //    okKhongCoLoi = false;
            //    arrLoi.Add("err_sSoSoLuong", NgonNgu.LayXau("Phải nhập Số sổ lương."));
            //}
            if (String.IsNullOrEmpty(iID_MaNgachLuong))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_iID_MaNgachLuong", NgonNgu.LayXau("Phải nhập ngạch lương."));
            }
            if (String.IsNullOrEmpty(iID_MaBacLuong))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_iID_MaBacLuong", NgonNgu.LayXau("Phải nhập cấp bậc."));
            }
            if (String.IsNullOrEmpty(sHoVaTen_CanBo))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_iID_MaBacLuong", NgonNgu.LayXau("Phải nhập họ tên cán bộ."));
            }

            if (String.IsNullOrEmpty(rLuongCoBan_HeSo))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_rLuongCoBan_HeSo", NgonNgu.LayXau("Phải nhập Hệ số lương."));
            }
            if (String.IsNullOrEmpty(dNgayNhapNgu) && (iID_MaNgachLuong == "1" || iID_MaNgachLuong == "2" || iID_MaNgachLuong == "4" ))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_dNgayNhapNgu", NgonNgu.LayXau("Phải nhập Ngày nhập ngũ."));
            }

            if (!String.IsNullOrEmpty(dNgayTaiNgu) && String.IsNullOrEmpty(dNgayNhapNgu))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_dNgayNhapNgu", NgonNgu.LayXau("Nhập ngày nhập ngũ trước khi nhập tái ngũ."));
            }

            if (!String.IsNullOrEmpty(dNgayXuatNgu) && String.IsNullOrEmpty(dNgayNhapNgu))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_dNgayNhapNgu", NgonNgu.LayXau("Nhập ngày nhập ngũ trước khi nhập xuất ngũ."));
            }
            if (!String.IsNullOrEmpty(dNgayNhapNgu) && !isDateTime(GhepNgayThangNam(dNgayNhapNgu)))
            {
                okKhongCoLoi = false;
                arrLoi.Add("err_dNgayNhapNgu", NgonNgu.LayXau("Ngày nhập ngũ không đúng định dạng."));
            }
            //if (!String.IsNullOrEmpty(iNuQuanNhan_CanBo) && (iNuQuanNhan_CanBo != "0" || iNuQuanNhan_CanBo != "1" || iNuQuanNhan_CanBo != "2"))
            //{
            //    okKhongCoLoi = false;
            //    arrLoi.Add("err_rLuongCoBan_HeSo", NgonNgu.LayXau("Nữ quân nhân chỉ nhập 1 hoặc 2"));
            //}
            return okKhongCoLoi;
        }

        /// <summary>
        /// Thêm mới 1 cán bộ từ bảng lương
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="Values"></param>
        /// <param name="arrLoi">Mảng lưu trữ lỗi khi truyền giá trị</param>
        /// <param name="MaND"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String ThemMoiCanBo_TuBangLuong(string ParentID, NameValueCollection Values, ref NameValueCollection arrLoi, String MaND, String IPSua)
        {
            String iID_MaCanBo = "";

            NameValueCollection Values_New = new NameValueCollection();
            Values_New.Add(Values);

            for (int j = 0; j <= Values.AllKeys.Length - 1; j++)
            {
                String TenTruong = Values.AllKeys[j];
                if (TenTruong.IndexOf("_CanBo")>=0)
                {
                    String TenTruongMoi = TenTruong.Replace("_CanBo", "");
                    Values_New[TenTruongMoi] = Values[TenTruong];
                }
            }

            Bang bangCanBo = new Bang("CB_CanBo");
            bangCanBo.MaNguoiDungSua = MaND;
            bangCanBo.IPSua = IPSua;
            arrLoi = bangCanBo.TruyenGiaTri(ParentID, Values_New);
            bangCanBo.DuLieuMoi = true;

            String LyDoTangGiam = Values_New[ParentID + "_iID_MaLyDoTangGiam"];
            if (String.IsNullOrEmpty(LyDoTangGiam) == false)
            {
                if (LyDoTangGiam.Substring(0, 1) == "2")
                {
                    bangCanBo.CmdParams.Parameters.AddWithValue("@iID_MaTinhTrangCanBo", 1);
                }
                if (LyDoTangGiam.Substring(0, 1) == "3")
                {
                    bangCanBo.CmdParams.Parameters.AddWithValue("@iID_MaTinhTrangCanBo", -1);
                }
            }
            bangCanBo.CmdParams.Parameters.AddWithValue("@iThem_Sua_TuLuong", 1);//Thêm từ lương=1; sửa từ lương=2;Thêm từ mục cán bộ=0;


            if (KiemTraDuocPhepSuaNhanhCanBo(ParentID, "", Values_New, ref arrLoi))
            {
                iID_MaCanBo = Convert.ToString(bangCanBo.Save());
            }
            return iID_MaCanBo;
        }
    }
}