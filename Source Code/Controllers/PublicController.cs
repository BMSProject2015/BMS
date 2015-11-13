using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;
using System.IO;
namespace VIETTEL.Controllers
{
    public class PublicController : Controller
    {
        public JsonResult get_CheckLogin(String UserName)
        {
            return Json(new { data = get_objCheckLogin(UserName) }, JsonRequestBehavior.AllowGet);
        }

        public static int get_objCheckLogin(String UserName)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT bHoatDong " +
                        "FROM QT_NguoiDung " +
                        "WHERE sID_MaNguoiDung=@sID_MaNguoiDung;";
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", UserName);
            dt = Connection.GetDataTable(cmd);
            int vR = 0;
            if (dt.Rows.Count > 0 && Convert.ToBoolean(dt.Rows[0]["bHoatDong"]))
            {
                vR = 1;
            }
            dt.Dispose();
            return vR;
        }

        public JsonResult get_newID()
        {
            return Json(Globals.getNewGuid(), JsonRequestBehavior.AllowGet); ;
        }

        #region Lấy danh sách AJAX: iID_MaDonVi, sMaCongTrinh, iID_MaMucLucNganSach
        [Authorize]
        public JsonResult get_DanhSach_LNS(String Truong, String GiaTri, String DSGiaTri, String sLNS)
        {
            if (Truong == "iID_MaMucLucNganSach")
            {
                return get_DanhSachMucLucNganSach_LNS(GiaTri, DSGiaTri, sLNS);
            }
            else if (Truong == "sTenDonVi")
            {
                return get_DanhSachDonVi(GiaTri);
            }
            else if (Truong == "sTenDonVi_BaoDam")
            {
                return get_DanhSachDonVi_BaoDam(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenDonViCoTen")
            {
                return get_DanhSachDonVi(GiaTri);
            }
            else if (Truong == "sTenTaiKhoan")
            {
                return get_DanhSachTaiKhoan(GiaTri);
            }
            else if (Truong == "sTenTaiKhoanGiaiThich")
            {
                return get_DanhSachTaiKhoanGiaiThich(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTinhChatCapThu")
            {
                return get_DanhSachTinhChatCapThu(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenCongTrinh")
            {
                return get_DanhSachCongTrinh(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenTaiSan")
            {
                return get_DanhSachTaiSan(GiaTri);
            }
            else if (Truong == "sTenNhomTaiSan")
            {
                return get_DanhSachNhomTaiSan(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenLoaiTaiSan")
            {
                return get_DanhSachLoaiTaiSan(GiaTri);
            }
            else if (Truong == "iID_MaChuongTrinhMucTieu")
            {
                return get_DanhSachChuongTrinhMucTieu(GiaTri);
            }
            else if (Truong == "sTenPhongBan")
            {
                return get_DanhSachPhongBan(GiaTri);
            }
            else if (Truong == "iID_MaPhongBanDich")
            {
                return get_DanhSachPhongBan(GiaTri);
            }
            else if (Truong == "sTenNhanVien")
            {
                return get_DanhSachNhanVien(GiaTri);
            }
            else if (Truong == "sSoChungTuCapThu")
            {
                return get_DanhSachChungTuCapThu(GiaTri);
            }
            else if (Truong == "sTenNgoaiTe")
            {
                return get_DanhSachNgoaiTe(GiaTri);
            }
            else if (Truong == "sTenNgachLuong")
            {
                return Get_DanhSachNgachLuong(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenBacLuong")
            {
                return Get_DanhSachBacLuong(GiaTri, DSGiaTri);
            }
            else if (Truong == "sMaTruong")
            {
                return get_DanhSachTruongBangLuongChiTiet(GiaTri);
            }
            else if (Truong == "sTenLyDoTangGiam")
            {
                return Get_DanhSachLyDoTangGiam(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenChuDauTu")
            {
                return Get_DanhSachChuDauTu(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenBanQuanLy")
            {
                return Get_DanhSachBanQuanLy(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenDonViThiCong")
            {
                return Get_DanhSachDonViThiCong(GiaTri);
            }
            else if (Truong == "sLoaiKeHoachVon")
            {
                return Get_DanhSachLoaiKeHoachVon(GiaTri);
            }
            else if (Truong == "sTenLoaiDieuChinh")
            {
                return Get_DanhSachLoaiDieuChinh(GiaTri);
            }
            else if (Truong == "sTenDuAn")
            {
                return get_DanhSachMucLucDuAn(GiaTri);
            }
            else if (Truong == "iID_MaKyHieuHachToan")
            {
                return get_DanhSachKyHieuHachToan(GiaTri);
            }
            else if (Truong == "sLoaiST")
            {
                return get_DanhSachLoaiST(GiaTri);
            }
            else if (Truong == "sThuChi")
            {
                return get_DanhSachThuChi(GiaTri);
            }
            else if (Truong == "sKyHieuNgoaiTe")
            {
                return get_DanhSachNgoaiTe_KeToan(GiaTri);
            }
            else if (Truong == "iTinhChat")
            {
                return get_TinhChat_KeToan(GiaTri);
            }
            else if (Truong == "iNgay")
            {
                return get_DSNgay_TheoThang(GiaTri);
            }
            else if (Truong == "iThang")
            {
                return get_DS_Thang(GiaTri);
            }
            else if (Truong == "iLoaiThuChi")
            {
                return get_DS_Loai_ThuChi(GiaTri);
            }
            return null;
        }
        [Authorize]
        public JsonResult get_DanhSach(String Truong, String GiaTri, String DSGiaTri)
        {
            if (Truong == "iID_MaMucLucNganSach")
            {
                return get_DanhSachMucLucNganSach(GiaTri, DSGiaTri);
            }
            else if (Truong == "TN_sLNS")
            {
                return get_DanhSachLNS(GiaTri);
            }
            else if (Truong == "sTenDonVi")
            {
                return get_DanhSachDonVi(GiaTri);
            }
            else if (Truong == "sTenDonViCoTen")
            {
                return get_DanhSachDonVi(GiaTri);
            }
            else if (Truong == "sTenTaiKhoan")
            {
                return get_DanhSachTaiKhoan(GiaTri);
            }
            else if (Truong == "sTenTaiKhoanGiaiThich")
            {
                return get_DanhSachTaiKhoanGiaiThich(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTinhChatCapThu")
            {
                return get_DanhSachTinhChatCapThu(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenCongTrinh")
            {
                return get_DanhSachCongTrinh(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenTaiSan")
            {
                return get_DanhSachTaiSan(GiaTri);
            }
            else if (Truong == "sTenNhomTaiSan")
            {
                return get_DanhSachNhomTaiSan(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenLoaiTaiSan")
            {
                return get_DanhSachLoaiTaiSan(GiaTri);
            }
            else if (Truong == "iID_MaChuongTrinhMucTieu")
            {
                return get_DanhSachChuongTrinhMucTieu(GiaTri);
            }
            else if (Truong == "sTenPhongBan")
            {
                return get_DanhSachPhongBan(GiaTri);
            }
            else if (Truong == "sTenNhanVien")
            {
                return get_DanhSachNhanVien(GiaTri);
            }
            else if (Truong == "sSoChungTuCapThu")
            {
                return get_DanhSachChungTuCapThu(GiaTri);
            }
            else if (Truong == "sTenNgoaiTe")
            {
                return get_DanhSachNgoaiTe(GiaTri);
            }
            else if (Truong == "sTenNgachLuong")
            {
                return Get_DanhSachNgachLuong(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenBacLuong")
            {
                return Get_DanhSachBacLuong(GiaTri, DSGiaTri);
            }
            else if (Truong == "sMaTruong")
            {
                return get_DanhSachTruongBangLuongChiTiet(GiaTri);
            }
            else if (Truong == "sTenLyDoTangGiam")
            {
                return Get_DanhSachLyDoTangGiam(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenChuDauTu")
            {
                return Get_DanhSachChuDauTu(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenBanQuanLy")
            {
                return Get_DanhSachBanQuanLy(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenDonViThiCong")
            {
                return Get_DanhSachDonViThiCong(GiaTri);
            }
            else if (Truong == "sLoaiKeHoachVon")
            {
                return Get_DanhSachLoaiKeHoachVon(GiaTri);
            }
            else if (Truong == "sTenLoaiDieuChinh")
            {
                return Get_DanhSachLoaiDieuChinh(GiaTri);
            }
            else if (Truong == "sTenDuAn")
            {
                return get_DanhSachMucLucDuAn(GiaTri);
            }
            else if (Truong == "iID_MaKyHieuHachToan")
            {
                return get_DanhSachKyHieuHachToan(GiaTri);
            }
            else if (Truong == "sLoaiST")
            {
                return get_DanhSachLoaiST(GiaTri);
            }
            else if (Truong == "sThuChi")
            {
                return get_DanhSachThuChi(GiaTri);
            }
            else if (Truong == "sKyHieuNgoaiTe")
            {
                return get_DanhSachNgoaiTe_KeToan(GiaTri);
            }
            else if (Truong == "iTinhChat_KT")
            {
                return get_TinhChat_KeToan(GiaTri);
            }
            else if (Truong == "iNgay")
            {
                return get_DSNgay_TheoThang(GiaTri);
            }
            else if (Truong == "iThang")
            {
                return get_DS_Thang(GiaTri);
            }
            else if (Truong == "iLoaiThuChi")
            {
                return get_DS_Loai_ThuChi(GiaTri);
            }
            else if (Truong == "iLoai")
            {
                return get_DS_iLoai(GiaTri);
            }
            else if (Truong == "iThamQuyen")
            {
                return get_DS_iThamQuyen(GiaTri);
            }
            else if (Truong == "iTinhChat")
            {
                return get_DS_iTinhChat(GiaTri);
            }
            else if (Truong == "iNhom")
            {
                return get_DS_iNhom(GiaTri);
            }
            return null;
        }
        private JsonResult get_DanhSachLoaiST(String GiaTri)
        {
            int i = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("sLoaiST");

            DataRow dr = dt.NewRow();
            dr[0] = "S";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "T";
            dt.Rows.Add(dr);
            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = dt.Rows[i][0],
                    label = dt.Rows[i][0]
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachThuChi(String GiaTri)
        {
            int i = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("sThuChi");

            DataRow dr = dt.NewRow();
            dr[0] = "X";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "1";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "2";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "3";
            dt.Rows.Add(dr);
            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = dt.Rows[i][0],
                    label = dt.Rows[i][0]
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Danh sach muc luc ngan sach co LNS
        /// </summary>
        /// <param name="GiaTri"></param>
        /// <param name="DSGiaTri"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        private JsonResult get_DanhSachMucLucNganSach_LNS(String GiaTri, String DSGiaTri, String sLNS)
        {
            String MaND = User.Identity.Name;
            DataTable dtLNS = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            String DKLNS = "";
            for (int j = 0; j < dtLNS.Rows.Count; j++)
            {
                DKLNS = DKLNS + Convert.ToString(dtLNS.Rows[j]["sLNS"]) + ",";
            }
            DKLNS = DKLNS.Remove(DKLNS.Length - 1);
            DKLNS = " AND sLNS IN (" + DKLNS + ") ";
            if (DSGiaTri == null) DSGiaTri = "";
            String[] arrDSTruong = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG".Split(',');
            String[] arrDSGiaTri = DSGiaTri.Split(',');

            SqlCommand cmd = new SqlCommand();
            String DK = "", Truong;
            int i = 0;
            if (DSGiaTri != "")
            {
                for (i = 0; i < arrDSGiaTri.Length; i++)
                {
                    DK += String.Format("{0}=@{0} AND ", arrDSTruong[i]);
                    cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
                }
            }
            Truong = arrDSTruong[i];
            //SQL cũ

            String SQL = String.Format("SELECT DISTINCT {0} FROM NS_MucLucNganSach WHERE iTrangThai=1  {2} AND {1} {0} LIKE @{0} AND sLNS LIKE @sLNS1 GROUP BY {0} ORDER BY {0}", Truong, DK, DKLNS);
            cmd.Parameters.AddWithValue("@" + Truong, GiaTri + "%");
            cmd.Parameters.AddWithValue("@sLNS1", sLNS + "%");
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count <= 0)
            {
                GiaTri = "";
                cmd = new SqlCommand();
                if (DSGiaTri != "")
                {
                    for (i = 0; i < arrDSGiaTri.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
                    }
                }
                SQL = String.Format("SELECT DISTINCT {0} FROM NS_MucLucNganSach WHERE iTrangThai=1  {2} AND {1} {0}  LIKE @{0}  AND {0}<>'' AND sLNS LIKE @sLNS1 GROUP BY {0} ORDER BY {0}", Truong, DK, DKLNS);
                cmd.Parameters.AddWithValue("@" + Truong, GiaTri + "%");
                cmd.Parameters.AddWithValue("@sLNS1", sLNS + "%");
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
            }
            cmd.Dispose();

            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = dt.Rows[i][0],
                    label = dt.Rows[i][0]
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachMucLucNganSach(String GiaTri, String DSGiaTri)
        {
            String MaND = User.Identity.Name;
            DataTable dtLNS = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            String DKLNS = "";
            for (int j = 0; j < dtLNS.Rows.Count; j++)
            {
                DKLNS = DKLNS + Convert.ToString(dtLNS.Rows[j]["sLNS"]) + ",";
            }
            DKLNS = DKLNS.Remove(DKLNS.Length - 1);
            DKLNS = " AND sLNS IN (" + DKLNS + ") ";
            if (DSGiaTri == null) DSGiaTri = "";
            String[] arrDSTruong = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG".Split(',');
            String[] arrDSGiaTri = DSGiaTri.Split(',');

            SqlCommand cmd = new SqlCommand();
            String DK = "", Truong;
            int i = 0;
            if (DSGiaTri != "")
            {
                for (i = 0; i < arrDSGiaTri.Length; i++)
                {
                    DK += String.Format("{0}=@{0} AND ", arrDSTruong[i]);
                    cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
                }
            }
            Truong = arrDSTruong[i];
            //SQL cũ
            //String SQL = String.Format("SELECT TOP 10 {0} FROM NS_MucLucNganSach WHERE iTrangThai=1 AND bLaHangCha=0 {2} AND {1} {0} LIKE @{0} GROUP BY {0} ORDER BY {0}", Truong, DK, DKLNS);
            String SQL = String.Format("SELECT TOP 10 {0} FROM NS_MucLucNganSach WHERE iTrangThai=1  {2} AND {1} {0} LIKE @{0} GROUP BY {0} ORDER BY {0}", Truong, DK, DKLNS);
            cmd.Parameters.AddWithValue("@" + Truong, GiaTri + "%");
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count <= 0)
            {
                GiaTri = "";
                cmd = new SqlCommand();
                if (DSGiaTri != "")
                {
                    for (i = 0; i < arrDSGiaTri.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
                    }
                }
                SQL = String.Format("SELECT TOP 10 {0} FROM NS_MucLucNganSach WHERE iTrangThai=1  {2} AND {1} {0}  LIKE @{0}  AND {0}<>'' GROUP BY {0} ORDER BY {0}", Truong, DK, DKLNS);
                cmd.Parameters.AddWithValue("@" + Truong, GiaTri + "%");
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
            }
            cmd.Dispose();

            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = dt.Rows[i][0],
                    label = dt.Rows[i][0]
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachMucLucDuAn(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 iID_MaDanhMucDuAn, sDeAn +'-'+ sDuAn +'-'+ sDuAnThanhPhan +'-'+ sCongTrinh +'-'+ sHangMucCongTrinh +'-'+ sHangMucChiTiet +'-'+ sTenDuAn AS sTenDuAn " +
                         "FROM QLDA_DanhMucDuAn " +
                         "WHERE iTrangThai = 1 AND " +
                               "(sXauNoiMa_DuAn LIKE @sTen OR sTenDuAn LIKE @sTenHT) " +
                         "ORDER BY sXauNoiMa_DuAn";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", "%" + GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTenHT", "%" + GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = "SELECT iID_MaDanhMucDuAn,  sDeAn +'-'+ sDuAn +'-'+ sDuAnThanhPhan +'-'+ sCongTrinh +'-'+ sHangMucCongTrinh +'-'+ sHangMucChiTiet +'-'+ sTenDuAn AS sTenDuAn " +
                         "FROM QLDA_DanhMucDuAn " +
                         "WHERE iTrangThai = 1 " +
                         "ORDER BY sXauNoiMa_DuAn";
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaDanhMucDuAn"]),
                    label = String.Format("{0}", dt.Rows[i]["sTenDuAn"])
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachTaiKhoan(String GiaTri)
        {
            String MaND = User.Identity.Name;
            int iNamLamViec = Convert.ToInt16(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
            List<Object> list = new List<Object>();

            SqlCommand cmd = new SqlCommand();
            //bo dk bLaHangCha = 0
            String DK = "iTrangThai=1 AND iNam=@iNam AND iID_MaTaiKhoan LIKE @iID_MaTaiKhoan";
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", GiaTri + "%");
            cmd.Parameters.AddWithValue("@iNam", iNamLamViec);
            String SQL = String.Format(@"SELECT TOP 10 iID_MaTaiKhoan, sTen FROM KT_TaiKhoan WHERE {0}
                                         AND (SELECT COUNT(iID_MaTaiKhoan) AS c from KT_TaiKhoan AS KT WHERE iID_MaTaiKhoan LIKE (KT_TaiKhoan.iID_MaTaiKhoan + '%')
                                                AND LEN(iID_MaTaiKhoan) > LEN(KT_TaiKhoan.iID_MaTaiKhoan)   )= 0
                                         ORDER BY iID_MaTaiKhoan", DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                cmd = new SqlCommand();
                //bo dk bLaHangCha = 0
                DK = "iTrangThai=1  AND iNam=@iNam";
                cmd.Parameters.AddWithValue("@iNam", iNamLamViec);
                SQL = String.Format(@"SELECT iID_MaTaiKhoan, sTen FROM KT_TaiKhoan WHERE {0} 
                                         AND (SELECT COUNT(iID_MaTaiKhoan) AS c from KT_TaiKhoan AS KT WHERE iID_MaTaiKhoan LIKE (KT_TaiKhoan.iID_MaTaiKhoan + '%')
                                                AND LEN(iID_MaTaiKhoan) > LEN(KT_TaiKhoan.iID_MaTaiKhoan)   )= 0
                                         ORDER BY iID_MaTaiKhoan", DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaTaiKhoan"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaTaiKhoan"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachDonVi_BaoDam(String GiaTri,String dsGiaTri)
        {
            String MaND = User.Identity.Name;
            List<Object> list = new List<Object>();
            String DK = String.Format(@"iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi IN (SELECT DISTINCT iID_MaDonVi
 FROM NS_PhongBan_DonVi
  WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec)");
            String SQL = String.Format("SELECT TOP 10 iID_MaDonVi, sTen FROM NS_DonVi WHERE {0} AND (iID_MaDonVi LIKE @iID_MaDonVi OR sTen LIKE @STen OR sSoTaiKhoan LIKE @sSoTaiKhoan) ORDER BY iID_MaDonVi", DK);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTen", "%" + GiaTri + "%");
            cmd.Parameters.AddWithValue("@sSoTaiKhoan", "%" + GiaTri + "%");
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                 DK = String.Format(@"iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi IN (SELECT DISTINCT iID_MaDonVi
 FROM NS_PhongBan_DonVi
  WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec 
  AND iID_MaPhongBan IN (SELECT iID_MaPhongBan FROM NS_PhongBan WHERE iTrangThai=1 AND sKyHieu='02'))");
                SQL = String.Format("SELECT TOP 10 iID_MaDonVi, sTen FROM NS_DonVi WHERE {0} ORDER BY iID_MaDonVi", DK);
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaDonVi"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaDonVi"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachDonVi(String GiaTri)
        {
            String MaND = User.Identity.Name;
            List<Object> list = new List<Object>();

            
            String DK = "iTrangThai=1 AND iID_MaDonVi IN (SELECT iID_MaDonVi FROM NS_NguoiDung_DonVi WHERE iTrangThai=1) AND ";
            String SQL = String.Format("SELECT TOP 10 iID_MaDonVi, sTen FROM NS_DonVi WHERE {0} iID_MaDonVi LIKE @iID_MaDonVi AND iNamLamViec_DonVi=@iNamLamViec ORDER BY iID_MaDonVi", DK);
            //String DK = String.Format("iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi IN (SELECT iID_MaDonVi FROM NS_NguoiDung_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sMaNguoiDung=@sMaNguoiDung) ");
            //String SQL = String.Format("SELECT TOP 10 iID_MaDonVi, sTen FROM NS_DonVi WHERE {0} AND (iID_MaDonVi LIKE @iID_MaDonVi OR sTen LIKE @STen OR sSoTaiKhoan LIKE @sSoTaiKhoan) ORDER BY iID_MaDonVi", DK);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", GiaTri + "%");
            //cmd.Parameters.AddWithValue("@sTen", "%" + GiaTri + "%");
            //cmd.Parameters.AddWithValue("@sSoTaiKhoan", "%" + GiaTri + "%");
            //cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                DK = "iTrangThai=1  AND iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi IN (SELECT iID_MaDonVi FROM NS_NguoiDung_DonVi WHERE iTrangThai=1  AND iNamLamViec=@iNamLamViec AND sMaNguoiDung=@sMaNguoiDung) ";
                SQL = String.Format("SELECT TOP 10 iID_MaDonVi, sTen FROM NS_DonVi WHERE {0} ORDER BY iID_MaDonVi", DK);
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaDonVi"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaDonVi"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachLNS(String GiaTri)
        {
            String MaND = User.Identity.Name;
            List<Object> list = new List<Object>();
            String SQL = String.Format(@"SELECT  sLNS, sMoTa FROM NS_MucLucNganSach
WHERE SUBSTRING(sLNS,1,1)=8 AND iTrangThai=1 
AND LEN(sLNS)=7 AND sL='' AND sLNS Like '%{0}%'
ORDER BY sLNS", GiaTri);
            SqlCommand cmd = new SqlCommand(SQL);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = String.Format(@"SELECT TOP 10  sLNS, sMoTa FROM NS_MucLucNganSach
WHERE SUBSTRING(sLNS,1,1)=8 AND iTrangThai=1 
AND LEN(sLNS)=7 AND sL=''
ORDER BY sLNS");
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sLNS"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["sLNS"], dt.Rows[i]["sMoTa"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachCongTrinh(String GiaTri, String DSGiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = String.Format("SELECT TOP 10 sMaCongTrinh, sTen FROM NS_MucLucDuAn WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND ( sMaCongTrinh LIKE @sMaCongTrinh OR sTen LIKE @sTen) ORDER BY sMaCongTrinh");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sMaCongTrinh", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTen", "%" + GiaTri + "%");
            cmd.Parameters.AddWithValue("@iID_MaDonVi", DSGiaTri);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = String.Format("SELECT sMaCongTrinh, sTen FROM NS_MucLucDuAn WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi ORDER BY sMaCongTrinh");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", DSGiaTri);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sMaCongTrinh"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["sMaCongTrinh"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachChuongTrinhMucTieu(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = String.Format("SELECT TOP 10 iID_MaChuongTrinhMucTieu, sTen FROM KT_ChuongTrinhMucTieu WHERE iTrangThai=1 AND iID_MaChuongTrinhMucTieu LIKE @iID_MaChuongTrinhMucTieu ORDER BY iID_MaChuongTrinhMucTieu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChuongTrinhMucTieu", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = String.Format("SELECT iID_MaChuongTrinhMucTieu, sTen FROM KT_ChuongTrinhMucTieu WHERE iTrangThai=1 ORDER BY iID_MaChuongTrinhMucTieu");
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaChuongTrinhMucTieu"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaChuongTrinhMucTieu"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachPhongBan(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = String.Format("SELECT TOP 10 sKyHieu, sTen FROM NS_PhongBan WHERE iTrangThai=1 AND sKyHieu LIKE @sKyHieu ORDER BY sKyHieu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = String.Format("SELECT sKyHieu, sTen FROM NS_PhongBan WHERE iTrangThai=1 ORDER BY sKyHieu");
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sKyHieu"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["sKyHieu"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachLoaiTaiSan(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String MaND = User.Identity.Name;
            int iNamLamViec = Convert.ToInt32(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
            String SQL = String.Format("SELECT TOP 10 iID_MaLoaiTaiSan, sTen FROM KTCS_LoaiTaiSan WHERE iTrangThai=1 AND (iID_MaLoaiTaiSan LIKE @iID_MaLoaiTaiSan OR sTen=@sTen) ORDER BY iID_MaLoaiTaiSan");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = String.Format("SELECT iID_MaLoaiTaiSan, sTen FROM KTCS_LoaiTaiSan WHERE iTrangThai=1 ORDER BY iID_MaLoaiTaiSan");
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaLoaiTaiSan"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaLoaiTaiSan"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachNhomTaiSan(String GiaTri, String DSGiaTri)
        {
            List<Object> list = new List<Object>();
            String MaND = User.Identity.Name;
            int iNamLamViec = Convert.ToInt32(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
            String SQL = String.Format("SELECT TOP 10 iID_MaNhomTaiSan, sTen FROM KTCS_NhomTaiSan WHERE iTrangThai=1 AND bLaHangCha = 0 AND iID_MaLoaiTaiSan=@iID_MaLoaiTaiSan AND iID_MaNhomTaiSan LIKE @iID_MaNhomTaiSan ORDER BY iID_MaNhomTaiSan");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", GiaTri + "%");
            cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", DSGiaTri);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = String.Format("SELECT iID_MaNhomTaiSan, sTen FROM KTCS_NhomTaiSan WHERE iTrangThai=1 AND iID_MaLoaiTaiSan=@iID_MaLoaiTaiSan AND bLaHangCha = 0 ORDER BY iID_MaNhomTaiSan");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", DSGiaTri);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaNhomTaiSan"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaNhomTaiSan"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachTaiSan(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String MaND = User.Identity.Name;
            int iNamLamViec = Convert.ToInt32(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
            String SQL = String.Format("SELECT TOP 10 iID_MaTaiSan, sTenTaiSan FROM KTCS_TaiSan WHERE iTrangThai=1 AND iID_MaTaiSan LIKE @iID_MaTaiSan ORDER BY iID_MaTaiSan");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTaiSan", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = String.Format("SELECT iID_MaTaiSan, sTenTaiSan FROM KTCS_TaiSan WHERE iTrangThai=1 ORDER BY iID_MaTaiSan");
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaTaiSan"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaTaiSan"], dt.Rows[i]["sTenTaiSan"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachNhanVien(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = String.Format("SELECT TOP 10 iID_MaNhanVien, sTen FROM KT_NhanVien WHERE iTrangThai=1 AND iID_MaNhanVien LIKE @sKyHieu ORDER BY iID_MaNhanVien");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = String.Format("SELECT iID_MaNhanVien, sTen FROM KT_NhanVien WHERE iTrangThai=1 ORDER BY iID_MaNhanVien");
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaNhanVien"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaNhanVien"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachChungTuCapThu(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = String.Format("SELECT TOP 10 iID_MaChungTu_Duyet, sSoChungTu, sNoiDung FROM KTTG_ChungTuCapThu_Duyet WHERE iTrangThai=1 AND rSoTien !=0 AND sSoChungTu LIKE @sSoChungTu ORDER BY sSoChungTu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sSoChungTu", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = String.Format("SELECT iID_MaChungTu_Duyet, sSoChungTu, sNoiDung FROM KTTG_ChungTuCapThu_Duyet WHERE iTrangThai=1 AND rSoTien !=0 ORDER BY sSoChungTu");
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaChungTu_Duyet"]),
                    label = String.Format("{0}", dt.Rows[i]["sSoChungTu"] + " - " + dt.Rows[i]["sNoiDung"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachTaiKhoanGiaiThich(String GiaTri, String iID_MaTaiKhoan)
        {
            List<Object> list = new List<Object>();
            //String SQL = "SELECT TOP 10 iID_MaTaiKhoanGiaiThich,sKyHieu, sTen " +
            //             "FROM KT_TaiKhoanGiaiThich " +
            //             "WHERE iTrangThai = 1 AND " +
            //                   "iID_MaTaiKhoan = @iID_MaTaiKhoan AND " +
            //                   "(sTen LIKE @sTen OR sKyHieu LIKE @sKyHieu)" +
            //             " ORDER BY sTen";
            String SQL =
                @"SELECT TOP 100 tk.iID_MaTaiKhoanGiaiThich,ct.sKyHieu, ct.sTen from KT_TaiKhoanGiaiThich tk,  
KT_TaiKhoanDanhMucChiTiet ct where ct.iTrangThai = 1 AND tk.iTrangThai = 1 and tk.iID_MaTaiKhoan = @iID_MaTaiKhoan 
AND tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet AND (ct.sTen LIKE @sTen OR ct.sKyHieu LIKE @sKyHieu) ORDER BY ct.sKyHieu";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", "%" + GiaTri + "%");
            cmd.Parameters.AddWithValue("@sKyHieu", GiaTri + "%");
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL =
                  @"SELECT  TOP 100 tk.iID_MaTaiKhoanGiaiThich,ct.sKyHieu, ct.sTen from KT_TaiKhoanGiaiThich tk,  
KT_TaiKhoanDanhMucChiTiet ct where ct.iTrangThai = 1 AND tk.iTrangThai = 1 and tk.iID_MaTaiKhoan = @iID_MaTaiKhoan 
AND tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet  ORDER BY ct.sKyHieu";
                //SQL = "SELECT iID_MaTaiKhoanGiaiThich,sKyHieu, sTen " +
                //         "FROM KT_TaiKhoanGiaiThich " +
                //         "WHERE iTrangThai = 1 AND " +
                //               "iID_MaTaiKhoan = @iID_MaTaiKhoan " +
                //         "ORDER BY sTen";
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}-{1}", dt.Rows[i]["sKyHieu"], dt.Rows[i]["sTen"]),
                    label = String.Format("{0}-{1}", dt.Rows[i]["sKyHieu"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachNgoaiTe(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 iID_MaNgoaiTe, sTen " +
                         "FROM QLDA_NgoaiTe " +
                         "WHERE iTrangThai = 1 AND " +
                               "sTen LIKE @sTen " +
                         "ORDER BY sTen";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = "SELECT iID_MaNgoaiTe, sTen " +
                         "FROM QLDA_NgoaiTe " +
                         "WHERE iTrangThai = 1 " +
                         "ORDER BY sTen";
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaNgoaiTe"]),
                    label = String.Format("{0}", dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachTruongBangLuongChiTiet(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = String.Format("sTenTruongPhuCap LIKE '{0}%'", GiaTri);
            DataTable dtTruongPhuCap = LuongModels.dt_TruongPhuCap();
            DataRow[] arrR = dtTruongPhuCap.Select(SQL);

            DataTable dt = new DataTable();
            dt.Columns.Add("sTenTruongPhuCap", typeof(String));
            dt.Columns.Add("sTruongPhuCap", typeof(String));
            int i;
            DataRow R;
            for (i = 0; i < arrR.Length; i++)
            {
                R = dt.NewRow();
                R["sTenTruongPhuCap"] = arrR[i]["sTenTruongPhuCap"];
                R["sTruongPhuCap"] = arrR[i]["sTruongPhuCap"];
                dt.Rows.Add(R);
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sTruongPhuCap"]),
                    label = String.Format("{0}", dt.Rows[i]["sTenTruongPhuCap"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_DanhSachLyDoTangGiam(String term, String term1)
        {
            List<Object> list = new List<Object>();
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_MucLucQuanSo WHERE iTrangThai=1 AND sKyHieu LIKE @sKyHieu ORDER BY sKyHieu");
            cmd.Parameters.AddWithValue("@sKyHieu", term + "%");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //if (dt.Rows.Count == 0)
            //{
            //    //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
            //    dt.Dispose();
            //    cmd = new SqlCommand("SELECT * FROM NS_MucLucQuanSo WHERE iTrangThai=1 ORDER BY sKyHieu");
            //    dt = Connection.GetDataTable(cmd);
            //    cmd.Dispose();
            //}
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sKyHieu"]),
                    label = String.Format("{0} - {1}", "T", dt.Rows[i]["sKyHieu"]),
                    MoTa = dt.Rows[i]["sMoTa"]
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_DanhSachNgachLuong(String term, String term1)
        {
            List<Object> list = new List<Object>();
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM L_DanhMucNgachLuong WHERE iTrangThai=1 AND iID_MaNgachLuong LIKE @iID_MaNgachLuong ORDER BY iID_MaNgachLuong");
            cmd.Parameters.AddWithValue("@iID_MaNgachLuong", term + "%");
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                cmd = new SqlCommand("SELECT * FROM L_DanhMucNgachLuong WHERE iTrangThai=1 ORDER BY iID_MaNgachLuong");
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaNgachLuong"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaNgachLuong"], dt.Rows[i]["sTenNgachLuong"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_DanhSachBacLuong(String term, String term1)
        {
            List<Object> list = new List<Object>();
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM L_DanhMucBacLuong WHERE iTrangThai=1 AND iID_MaNgachLuong = @iID_MaNgachLuong AND iID_MaBacLuong LIKE @iID_MaBacLuong ORDER BY iID_MaBacLuong");
            cmd.Parameters.AddWithValue("@iID_MaBacLuong", term + "%");
            cmd.Parameters.AddWithValue("@iID_MaNgachLuong", term1);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                cmd = new SqlCommand("SELECT * FROM L_DanhMucBacLuong WHERE iTrangThai=1 AND iID_MaNgachLuong = @iID_MaNgachLuong ORDER BY iID_MaBacLuong");
                cmd.Parameters.AddWithValue("@iID_MaNgachLuong", term1);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaBacLuong"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaBacLuong"], dt.Rows[i]["sTenBacLuong"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult Get_DanhSachChuDauTu(String GiaTri, String DSGiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 iID_MaChuDauTu, sTen " +
                         "FROM QLDA_ChuDauTu " +
                         "WHERE iTrangThai = 1 AND iID_MaDonVi=@iID_MaDonVi AND " +
                               "sTen LIKE @sTen " +
                         "ORDER BY sTen";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
            cmd.Parameters.AddWithValue("@iID_MaDonVi", DSGiaTri);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaChuDauTu"]),
                    label = String.Format("{0}", dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult Get_DanhSachBanQuanLy(String GiaTri, String DSGiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 iID_MaBanQuanLy, sTenBanQuanLy " +
                         "FROM QLDA_BanQuanLy " +
                         "WHERE iTrangThai = 1 AND iID_MaChuDauTu=@iID_MaChuDauTu AND " +
                            "( iID_MaBanQuanLy LIKE @sTenBanQuanLy OR " +
                               "sTenBanQuanLy LIKE @sTenBanQuanLy )" +
                         " ORDER BY sTenBanQuanLy";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenBanQuanLy", GiaTri + "%");
            cmd.Parameters.AddWithValue("@iID_MaChuDauTu", DSGiaTri);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaBanQuanLy"]),
                    label = String.Format("{0}", dt.Rows[i]["sTenBanQuanLy"])
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult Get_DanhSachDonViThiCong(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 iID_MaDonViThiCong, sTen " +
                         "FROM QLDA_DonViThiCong " +
                         "WHERE iTrangThai = 1 AND " +
                               "(iID_MaDonViThiCong=@iID_MaDonViThiCong OR sTen LIKE @sTen) " +
                         "ORDER BY sTen";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonViThiCong", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = "SELECT iID_MaDonViThiCong, sTen " +
                         "FROM QLDA_DonViThiCong " +
                         "WHERE iTrangThai = 1 " +
                         "ORDER BY sTen";
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaDonViThiCong"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaDonViThiCong"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult Get_DanhSachLoaiKeHoachVon(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 iID_MaLoaiKeHoachVon, sTen " +
                         "FROM QLDA_KeHoachVon_Loai " +
                         "WHERE iTrangThai = 1 AND " +
                               "(iID_MaLoaiKeHoachVon=@iID_MaLoaiKeHoachVon OR sTen LIKE @sTen) " +
                         "ORDER BY iSTT";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaLoaiKeHoachVon", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = "SELECT iID_MaLoaiKeHoachVon, sTen " +
                         "FROM QLDA_KeHoachVon_Loai " +
                         "WHERE iTrangThai = 1 " +
                         "ORDER BY iSTT";
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaLoaiKeHoachVon"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaLoaiKeHoachVon"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult Get_DanhSachLoaiDieuChinh(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 iID_MaLoaiDieuChinh, sTen " +
                         "FROM QLDA_LoaiDieuChinh " +
                         "WHERE iTrangThai = 1 AND " +
                               "(iID_MaLoaiDieuChinh=@iID_MaLoaiDieuChinh OR sTen LIKE @sTen) " +
                         "ORDER BY iSTT";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaLoaiDieuChinh", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = "SELECT iID_MaLoaiDieuChinh, sTen " +
                         "FROM QLDA_LoaiDieuChinh " +
                         "WHERE iTrangThai = 1 " +
                         "ORDER BY iSTT";
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaLoaiDieuChinh"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaLoaiDieuChinh"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_DanhSachTinhChatCapThu(String GiaTri, String DSGiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 iID_MaTinhChatCapThu, sTen " +
                         "FROM KTTG_TinhChatCapThu " +
                         "WHERE iTrangThai = 1  AND " +
                               "(iID_MaTinhChatCapThu LIKE @iID_MaTinhChatCapThu OR sTen LIKE @sTen) " +
                         "ORDER BY sTen";
            SqlCommand cmd = new SqlCommand(SQL);
            // cmd.Parameters.AddWithValue("@bLoai", DSGiaTri);
            cmd.Parameters.AddWithValue("@iID_MaTinhChatCapThu", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = "SELECT iID_MaTinhChatCapThu, sTen " +
                         "FROM KTTG_TinhChatCapThu " +
                         "WHERE iTrangThai = 1 " +
                         "ORDER BY sTen";
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaTinhChatCapThu"]),
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaTinhChatCapThu"], dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachKyHieuHachToan(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT top 10  iID_MaKyHieuHachToan,iID_MaKyHieuHachToan AS sTen " +
                         " FROM (SELECT  DISTINCT iID_MaKyHieuHachToan" +
                         " FROM KTCS_KyHieuHachToanChiTiet" +
                         " WHERE iTrangThai = 1 AND " +
                               "(iID_MaKyHieuHachToan LIKE @iID_MaKyHieuHachToan) ) AS KH" +
                         " ORDER BY iID_MaKyHieuHachToan ";
            SqlCommand cmd = new SqlCommand(SQL);

            cmd.Parameters.AddWithValue("@iID_MaKyHieuHachToan", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = "SELECT  DISTINCT iID_MaKyHieuHachToan" +
                         " FROM KTCS_KyHieuHachToanChiTiet" +
                         " WHERE iTrangThai = 1 ";
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaKyHieuHachToan"]),
                    label = String.Format("{0}", dt.Rows[i]["iID_MaKyHieuHachToan"])
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_DanhSachNgoaiTe_KeToan(String GiaTri)
        {
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            int iThang = DateTime.Now.Month;
            if (dtCauHinh.Rows.Count > 0)
            {

                iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
            }

            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 iID_MaNgoaiTe, sTen FROM KTKB_NgoaiTe WHERE iTrangThai = 1 AND iNamLamViec=@iNamLamViec AND iThangLamViec=@iThangLamViec AND sTen LIKE @sTenKH ORDER BY iID_MaNgoaiTe";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iThangLamViec", iThang);
            cmd.Parameters.AddWithValue("@sTenKH", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = "SELECT iID_MaNgoaiTe, sTen " +
                         "FROM KTKB_NgoaiTe " +
                         "WHERE iTrangThai = 1 AND iNamLamViec=@iNamLamViec AND iThangLamViec=@iThangLamViec " +
                         "ORDER BY iID_MaNgoaiTe";
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                cmd.Parameters.AddWithValue("@iThangLamViec", iThang);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iID_MaNgoaiTe"]),
                    label = String.Format("{0}", dt.Rows[i]["iID_MaNgoaiTe"] + "-" + dt.Rows[i]["sTen"])
                };
                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_TinhChat_KeToan(String GiaTri)
        {
            int i = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("iTinhChat");

            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "2";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "3";
            dt.Rows.Add(dr);
            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = dt.Rows[i][0],
                    label = dt.Rows[i][0]
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        private JsonResult get_DS_Thang(String GiaTri)
        {
            int i = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("iThang");
            if (!String.IsNullOrEmpty(GiaTri) && Int32.Parse(GiaTri) <= 12 && Int32.Parse(GiaTri) > 0)
            {
                DataRow dr = dt.NewRow();
                dr[0] = GiaTri;
                dt.Rows.Add(dr);
                dr = null;
            }
            //else
            //{
            //    for (int j = 1; j <= 12; j++)
            //    {
            //        DataRow dr = dt.NewRow();
            //        dr[0] = j;
            //        dt.Rows.Add(dr);
            //        dr = null;
            //    }
            //}
            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = dt.Rows[i][0],
                    label = dt.Rows[i][0]
                };
                list.Add(item);
            }
            if (dt != null)
            {
                dt.Dispose();
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }
        private JsonResult get_DS_iLoai(String GiaTri)
        {
            int i = 0;
            String TenLoaiDanhMuc = "LoaiCongTrinhDuAn";
            String SQL = String.Format(@"SELECT * FROM DC_danhmuc
    WHERE iTrangThai=1 AND sTenKhoa  like @sTenKhoa AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null)
            {
                dt.Dispose();
            }
            if (dt.Rows.Count <= 0)
            {
                SQL = String.Format(@"SELECT * FROM DC_danhmuc
                                WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
                dt = Connection.GetDataTable(cmd);
            }
        
            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sTenKhoa"]),
                    label = String.Format("{0}", dt.Rows[i]["sTenKhoa"] + "-" + dt.Rows[i]["sTen"])

                };
                list.Add(item);
            }
            if (dt != null)
            {
                dt.Dispose();
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }
        private JsonResult get_DS_iThamQuyen(String GiaTri)
        {
            int i = 0;
            String TenLoaiDanhMuc = "ThamQuyenCongTrinhDuAn";
            String SQL = String.Format(@"SELECT * FROM DC_danhmuc
    WHERE iTrangThai=1 AND sTenKhoa  like @sTenKhoa AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", GiaTri+"%");
            cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null)
            {
                dt.Dispose();
            }
            if (dt.Rows.Count <= 0)
            {
                SQL = String.Format(@"SELECT * FROM DC_danhmuc
                                WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
                dt = Connection.GetDataTable(cmd);
            }

            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sTenKhoa"]),
                    label = String.Format("{0}", dt.Rows[i]["sTenKhoa"] + "-" + dt.Rows[i]["sTen"])

                };
                list.Add(item);
            }
            if (dt != null)
            {
                dt.Dispose();
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }
        private JsonResult get_DS_iTinhChat(String GiaTri)
        {
            int i = 0;
            String TenLoaiDanhMuc = "TinhChatCongTrinhDuAn";
            String SQL = String.Format(@"SELECT * FROM DC_danhmuc
    WHERE iTrangThai=1 AND sTenKhoa  like @sTenKhoa AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null)
            {
                dt.Dispose();
            }
            if (dt.Rows.Count <= 0)
            {
                SQL = String.Format(@"SELECT * FROM DC_danhmuc
                                WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
                dt = Connection.GetDataTable(cmd);
            }

            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sTenKhoa"]),
                    label = String.Format("{0}", dt.Rows[i]["sTenKhoa"] + "-" + dt.Rows[i]["sTen"])

                };
                list.Add(item);
            }
            if (dt != null)
            {
                dt.Dispose();
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }
        private JsonResult get_DS_iNhom(String GiaTri)
        {
            int i = 0;
            String TenLoaiDanhMuc = "NhomCongTrinhDuAn";
            String SQL = String.Format(@"SELECT * FROM DC_danhmuc
    WHERE iTrangThai=1 AND sTenKhoa  like @sTenKhoa AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", GiaTri + "%");
            cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null)
            {
                dt.Dispose();
            }
            if (dt.Rows.Count <= 0)
            {
                SQL = String.Format(@"SELECT * FROM DC_danhmuc
                                WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
                dt = Connection.GetDataTable(cmd);
            }

            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sTenKhoa"]),
                    label = String.Format("{0}", dt.Rows[i]["sTenKhoa"] + "-" + dt.Rows[i]["sTen"])

                };
                list.Add(item);
            }
            if (dt != null)
            {
                dt.Dispose();
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }
        private JsonResult get_DS_Loai_ThuChi(String GiaTri)
        {
            int i = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("iLoai", typeof(int));
            dt.Columns.Add("sTenLoai", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = 1;
            dr[1] = "Thu";
            dt.Rows.Add(dr);
            dr = null;

            DataRow dr1 = dt.NewRow();
            dr1[0] = 2;
            dr1[1] = "Chi";
            dt.Rows.Add(dr1);
            dr1 = null;
            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["iLoai"]),
                    label = String.Format("{0}", dt.Rows[i]["iLoai"] + "-" + dt.Rows[i]["sTenLoai"])

                };
                list.Add(item);
            }
            if (dt != null)
            {
                dt.Dispose();
            }

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        private JsonResult get_DSNgay_TheoThang(String GiaTri)
        {
            String MaND = User.Identity.Name;
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            int iNam = DateTime.Now.Year;
            int iThang = DateTime.Now.Month;
            if (dtCauHinh.Rows.Count > 0 && dtCauHinh != null)
            {
                iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
                iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
            }
            if (dtCauHinh != null) dtCauHinh.Dispose();
            int i = 0;

            int songay = GetDaysInMonth(iThang, iNam);
            DataTable dt = new DataTable();
            dt.Columns.Add("iNgay");
            if (!String.IsNullOrEmpty(GiaTri) && Int32.Parse(GiaTri) <= songay && Int32.Parse(GiaTri) > 0)
            {
                DataRow dr = dt.NewRow();
                dr[0] = GiaTri;
                dt.Rows.Add(dr);
                dr = null;
            }
            //else
            //{
            //    for (int j = 1; j <= songay; j++)
            //    {
            //        DataRow dr = dt.NewRow();
            //        dr[0] = j;
            //        dt.Rows.Add(dr);
            //        dr = null;

            //    }
            //}
            List<Object> list = new List<Object>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = dt.Rows[i][0],
                    label = dt.Rows[i][0]
                };
                list.Add(item);
            }
            if (dt != null)
            {
                dt.Dispose();
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        private int GetDaysInMonth(int month, int year)
        {
            if (month < 1 || month > 12)
            {
                return 31;
            }
            if (1 == month || 3 == month || 5 == month || 7 == month || 8 == month ||
                10 == month || 12 == month)
            {
                return 31;
            }
            else if (2 == month)
            {

                if (0 == (year % 4))
                {

                    if (0 == (year % 400))
                    {
                        return 29;
                    }
                    else if (0 == (year % 100))
                    {
                        return 28;
                    }


                    return 29;
                }

                return 28;
            }
            return 30;
        }

        #endregion

        #region Lấy 1 hang AJAX: iID_MaDonVi, sMaCongTrinhPhongBan
        [Authorize]
        public JsonResult get_GiaTri_LNS(String Truong, String GiaTri, String DSGiaTri, String sLNS)
        {
            if (Truong == "iID_MaMucLucNganSach")
            {
                return get_GiaTriMucLucNganSach_LNS(GiaTri, DSGiaTri, sLNS);
            }
            else if (Truong == "sTenDonVi")
            {
                return get_GiaTriDonVi(GiaTri);
            }
            else if (Truong == "sTenDonViBaoDam")
            {
                return get_GiaTriDonViCoTen(GiaTri);
            }
            else if (Truong == "sTenDonViCoTen")
            {
                return get_GiaTriDonViCoTen(GiaTri);
            }
            else if (Truong == "sTenTaiKhoanGiaiThich")
            {
                return get_GiaTriGiaiThich(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTinhChatCapThu")
            {
                return get_GiaTriTinhChatCapThu(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenTaiKhoan")
            {
                return get_GiaTriTaiKhoan(GiaTri);
            }
            else if (Truong == "sTenCongTrinh")
            {
                return get_GiaTriCongTrinh(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenTaiSan")
            {
                return get_GiaTriTaiSan(GiaTri);
            }
            else if (Truong == "sTenLoaiTaiSan")
            {
                return get_GiaTriLoaiTaiSan(GiaTri);
            }
            else if (Truong == "sTenNhomTaiSan")
            {
                return get_GiaTriNhomTaiSan(GiaTri, DSGiaTri);
            }
            else if (Truong == "iID_MaChuongTrinhMucTieu")
            {
                return get_GiaTriChuongTrinhMucTieu(GiaTri);
            }
            else if (Truong == "sTenPhongBan")
            {
                return get_GiaTriPhongBan(GiaTri);
            }
            else if (Truong == "iID_MaPhongBanDich")
            {
                return get_GiaTriPhongBanDich(GiaTri);
            }
            else if (Truong == "sTenNhanVien")
            {
                return get_GiaTriNhanVien(GiaTri);
            }
            else if (Truong == "sSoChungTuCapThu")
            {
                return get_GiaTriChungTuCapThu(GiaTri);
            }
            else if (Truong == "sMaHT_PhuCap")
            {
                return get_GiaTriPhuCap(GiaTri);
            }
            else if (Truong == "sTenNgoaiTe")
            {
                return get_GiaTriNgoaiTe(GiaTri);
            }
            else if (Truong == "sTenNgachLuong")
            {
                return get_GiaTriNgachLuong(GiaTri);
            }
            else if (Truong == "sTenBacLuong")
            {
                return get_GiaTriBacLuong(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenLyDoTangGiam")
            {
                return Get_GiaTriLyDoTangGiam(GiaTri);
            }
            else if (Truong == "sMaTruong")
            {
                return get_GiaTriTruongBangLuongChiTiet(GiaTri);
            }
            else if (Truong == "sTenChuDauTu")
            {
                return get_GiaTriChuDauTu(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenBanQuanLy")
            {
                return get_GiaTriBanQuanLy(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenDonViThiCong")
            {
                return get_GiaTriDonViThiCong(GiaTri);
            }
            else if (Truong == "sLoaiKeHoachVon")
            {
                return get_GiaTrLoaiKeHoachVon(GiaTri);
            }
            else if (Truong == "sTenLoaiDieuChinh")
            {
                return get_GiaTrLoaiDieuChinh(GiaTri);
            }
            else if (Truong == "sTenDuAn")
            {
                return get_GiaTriMucLucDuAn(GiaTri);
            }
            else if (Truong == "iID_MaKyHieuHachToan")
            {
                return get_GiaTriKyHieuHachToan(GiaTri);
            }
            else if (Truong == "sLoaiST")
            {
                return get_GiaTriLoaiST(GiaTri);
            }
            else if (Truong == "sThuChi")
            {
                return get_GiaTriThuChi(GiaTri);
            }
            else if (Truong == "sKyHieuNgoaiTe")
            {
                return get_GiaTriNgoaiTe_KeToan(GiaTri);
            }
            else if (Truong == "iTinhChat")
            {
                return get_GiaTriTinhChat(GiaTri);
            }
            else if (Truong == "iNgay")
            {
                return get_GiaTri_Ngay_TheoThang(GiaTri);
            }
            else if (Truong == "iThang")
            {
                return get_GiaTri_Thang(GiaTri);
            }
            else if (Truong == "iLoaiThuChi")
            {
                return get_GiaTri_LoaiThuChi(GiaTri);
            }
            return null;
        }
        [Authorize]
        public JsonResult get_GiaTri(String Truong, String GiaTri, String DSGiaTri)
        {
            if (Truong == "iID_MaMucLucNganSach")
            {
                return get_GiaTriMucLucNganSach(GiaTri, DSGiaTri);
            }
            else if (Truong == "TN_sLNS")
            {
                return get_GiaTrisLNS(GiaTri);
            }
            else if (Truong == "sTenDonVi_BaoDam")
            {
                return get_GiaTriDonViCoTen(GiaTri);
            }
            else if (Truong == "sTenDonVi")
            {
                return get_GiaTriDonVi(GiaTri);
            }
            else if (Truong == "sTenDonViCoTen")
            {
                return get_GiaTriDonViCoTen(GiaTri);
            }
            else if (Truong == "sTenTaiKhoanGiaiThich")
            {
                return get_GiaTriGiaiThich(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTinhChatCapThu")
            {
                return get_GiaTriTinhChatCapThu(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenTaiKhoan")
            {
                return get_GiaTriTaiKhoan(GiaTri);
            }
            else if (Truong == "sTenCongTrinh")
            {
                return get_GiaTriCongTrinh(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenTaiSan")
            {
                return get_GiaTriTaiSan(GiaTri);
            }
            else if (Truong == "sTenLoaiTaiSan")
            {
                return get_GiaTriLoaiTaiSan(GiaTri);
            }
            else if (Truong == "sTenNhomTaiSan")
            {
                return get_GiaTriNhomTaiSan(GiaTri, DSGiaTri);
            }
            else if (Truong == "iID_MaChuongTrinhMucTieu")
            {
                return get_GiaTriChuongTrinhMucTieu(GiaTri);
            }
            else if (Truong == "sTenPhongBan")
            {
                return get_GiaTriPhongBan(GiaTri);
            }
            else if (Truong == "sTenNhanVien")
            {
                return get_GiaTriNhanVien(GiaTri);
            }
            else if (Truong == "sSoChungTuCapThu")
            {
                return get_GiaTriChungTuCapThu(GiaTri);
            }
            else if (Truong == "sMaHT_PhuCap")
            {
                return get_GiaTriPhuCap(GiaTri);
            }
            else if (Truong == "sTenNgoaiTe")
            {
                return get_GiaTriNgoaiTe(GiaTri);
            }
            else if (Truong == "sTenNgachLuong")
            {
                return get_GiaTriNgachLuong(GiaTri);
            }
            else if (Truong == "sTenBacLuong")
            {
                return get_GiaTriBacLuong(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenLyDoTangGiam")
            {
                return Get_GiaTriLyDoTangGiam(GiaTri);
            }
            else if (Truong == "sMaTruong")
            {
                return get_GiaTriTruongBangLuongChiTiet(GiaTri);
            }
            else if (Truong == "sTenChuDauTu")
            {
                return get_GiaTriChuDauTu(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenBanQuanLy")
            {
                return get_GiaTriBanQuanLy(GiaTri, DSGiaTri);
            }
            else if (Truong == "sTenDonViThiCong")
            {
                return get_GiaTriDonViThiCong(GiaTri);
            }
            else if (Truong == "sLoaiKeHoachVon")
            {
                return get_GiaTrLoaiKeHoachVon(GiaTri);
            }
            else if (Truong == "sTenLoaiDieuChinh")
            {
                return get_GiaTrLoaiDieuChinh(GiaTri);
            }
            else if (Truong == "sTenDuAn")
            {
                return get_GiaTriMucLucDuAn(GiaTri);
            }
            else if (Truong == "iID_MaKyHieuHachToan")
            {
                return get_GiaTriKyHieuHachToan(GiaTri);
            }
            else if (Truong == "sLoaiST")
            {
                return get_GiaTriLoaiST(GiaTri);
            }
            else if (Truong == "sThuChi")
            {
                return get_GiaTriThuChi(GiaTri);
            }
            else if (Truong == "sKyHieuNgoaiTe")
            {
                return get_GiaTriNgoaiTe_KeToan(GiaTri);
            }
            else if (Truong == "iTinhChatKT")
            {
                return get_GiaTriTinhChat(GiaTri);
            }
            else if (Truong == "iNgay")
            {
                return get_GiaTri_Ngay_TheoThang(GiaTri);
            }
            else if (Truong == "iThang")
            {
                return get_GiaTri_Thang(GiaTri);
            }
            else if (Truong == "iLoaiThuChi")
            {
                return get_GiaTri_LoaiThuChi(GiaTri);
            }
            else if (Truong == "iLoai")
            {
                return get_GiaTri_iLoai(GiaTri);
            }
            else if (Truong == "iThamQuyen")
            {
                return get_GiaTri_iThamQuyen(GiaTri);
            }
            else if (Truong == "iTinhChat")
            {
                return get_GiaTri_iTinhChat(GiaTri);
            }
            else if (Truong == "iNhom")
            {
                return get_GiaTri_iNhom(GiaTri);
            }
            return null;
        }
        private JsonResult get_GiaTriLoaiST(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };


            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("sLoaiST");

                DataRow dr = dt.NewRow();
                dr[0] = "S";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "T";
                dt.Rows.Add(dr);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (GiaTri == dt.Rows[i][0].ToString())
                        item = new
                        {
                            value = String.Format("{0}", dt.Rows[i][0]),
                            label = String.Format("{0}", dt.Rows[i][0])
                        };

                }
                dt.Dispose();
            }

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTriThuChi(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };

            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("sThuChi");

                DataRow dr = dt.NewRow();
                dr[0] = "X";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "1";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "2";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "3";
                dt.Rows.Add(dr);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (GiaTri == dt.Rows[i][0].ToString())
                        item = new
                        {
                            value = String.Format("{0}", dt.Rows[i][0]),
                            label = String.Format("{0}", dt.Rows[i][0])
                        };

                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTriTinhChat(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };

            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("iTinhChat");

                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "2";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "3";
                dt.Rows.Add(dr);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (GiaTri == dt.Rows[i][0].ToString())
                        item = new
                        {
                            value = String.Format("{0}", dt.Rows[i][0]),
                            label = String.Format("{0}", dt.Rows[i][0])
                        };

                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTri_Thang(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };

            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("iThang");
                for (int j = 1; j <= 12; j++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = j;
                    dt.Rows.Add(dr);
                    dr = null;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (GiaTri == dt.Rows[i][0].ToString())
                        item = new
                        {
                            value = String.Format("{0}", dt.Rows[i][0]),
                            label = String.Format("{0}", dt.Rows[i][0])
                        };

                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTri_iThamQuyen(String GiaTri)
        {
            String TenLoaiDanhMuc = "ThamQuyenCongTrinhDuAn";
            String SQL = String.Format(@"SELECT * FROM DC_danhmuc
    WHERE iTrangThai=1 AND sTenKhoa =@sTenKhoa AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang)");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa",GiaTri);
            cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null)
            {
                dt.Dispose();
            }
            if (dt.Rows.Count <= 0)
            {
                SQL = String.Format(@"SELECT * FROM DC_danhmuc
                                WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
                 cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
                 dt = Connection.GetDataTable(cmd);
            }
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                
                    if (dt.Rows.Count>0)
                        item = new
                        {
                            value = String.Format("{0}", dt.Rows[0]["sTenKhoa"]),
                            label = String.Format("{0}", dt.Rows[0]["sTenKhoa"])
                        };
                dt.Dispose();
            }
            
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTri_iLoai(String GiaTri)
        {
            String TenLoaiDanhMuc = "LoaiCongTrinhDuAn";
            String SQL = String.Format(@"SELECT * FROM DC_danhmuc
    WHERE iTrangThai=1 AND sTenKhoa =@sTenKhoa AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang)");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", GiaTri);
            cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null)
            {
                dt.Dispose();
            }
            if (dt.Rows.Count <= 0)
            {
                SQL = String.Format(@"SELECT * FROM DC_danhmuc
                                WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
                dt = Connection.GetDataTable(cmd);
            }
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {

                if (dt.Rows.Count > 0)
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["sTenKhoa"]),
                        label = String.Format("{0}", dt.Rows[0]["sTenKhoa"])
                    };
                dt.Dispose();
            }

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTri_iTinhChat(String GiaTri)
        {
            String TenLoaiDanhMuc = "TinhChatCongTrinhDuAn";
            String SQL = String.Format(@"SELECT * FROM DC_danhmuc
    WHERE iTrangThai=1 AND sTenKhoa =@sTenKhoa AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang)");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", GiaTri);
            cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null)
            {
                dt.Dispose();
            }
            if (dt.Rows.Count <= 0)
            {
                SQL = String.Format(@"SELECT * FROM DC_danhmuc
                                WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
                dt = Connection.GetDataTable(cmd);
            }
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {

                if (dt.Rows.Count > 0)
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["sTenKhoa"]),
                        label = String.Format("{0}", dt.Rows[0]["sTenKhoa"])
                    };
                dt.Dispose();
            }

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTri_iNhom(String GiaTri)
        {
            String TenLoaiDanhMuc = "NhomCongTrinhDuAn";
            String SQL = String.Format(@"SELECT * FROM DC_danhmuc
    WHERE iTrangThai=1 AND sTenKhoa =@sTenKhoa AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang)");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTenKhoa", GiaTri);
            cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null)
            {
                dt.Dispose();
            }
            if (dt.Rows.Count <= 0)
            {
                SQL = String.Format(@"SELECT * FROM DC_danhmuc
                                WHERE iTrangThai=1 AND iID_MaLoaiDanhMuc=(SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE iTrangThai=1 AND sTenBang=@sTenBang) ORDER BY iSTT");
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sTenBang", TenLoaiDanhMuc);
                dt = Connection.GetDataTable(cmd);
            }
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {

                if (dt.Rows.Count > 0)
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["sTenKhoa"]),
                        label = String.Format("{0}", dt.Rows[0]["sTenKhoa"])
                    };
                dt.Dispose();
            }

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTri_LoaiThuChi(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };

            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("iLoai", typeof(int));
                dt.Columns.Add("sTenLoai", typeof(string));

                DataRow dr = dt.NewRow();
                dr[0] = 1;
                dr[1] = "Thu";
                dt.Rows.Add(dr);
                dr = null;

                DataRow dr1 = dt.NewRow();
                dr1[0] = 2;
                dr1[1] = "Chi";
                dt.Rows.Add(dr1);
                dr1 = null;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (GiaTri == dt.Rows[i][0].ToString())
                        item = new
                        {
                            value = String.Format("{0}", dt.Rows[i]["iLoai"]),
                            label = String.Format("{0}", dt.Rows[i]["iLoai"])
                        };

                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTri_Ngay_TheoThang(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };

            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String MaND = User.Identity.Name;
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                int iNam = DateTime.Now.Year;
                int iThang = DateTime.Now.Month;
                if (dtCauHinh.Rows.Count > 0 && dtCauHinh != null)
                {
                    iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
                    iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
                }
                if (dtCauHinh != null) dtCauHinh.Dispose();

                int songay = GetDaysInMonth(iThang, iNam);
                DataTable dt = new DataTable();
                dt.Columns.Add("iNgay");
                for (int j = 1; j <= songay; j++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = j;
                    dt.Rows.Add(dr);
                    dr = null;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (GiaTri == dt.Rows[i][0].ToString())
                        item = new
                        {
                            value = String.Format("{0}", dt.Rows[i][0]),
                            label = String.Format("{0}", dt.Rows[i][0])
                        };

                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTriMucLucNganSach_LNS(String GiaTri, String DSGiaTri, String sLNS)
        {
            String MaND = User.Identity.Name;
            DataTable dtLNS = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            String DKLNS = "";
            for (int j = 0; j < dtLNS.Rows.Count; j++)
            {
                DKLNS = DKLNS + Convert.ToString(dtLNS.Rows[j]["sLNS"]) + ",";
            }
            DKLNS = DKLNS.Remove(DKLNS.Length - 1);
            DKLNS = " AND sLNS IN (" + DKLNS + ") ";

            if (DSGiaTri == null) DSGiaTri = "";
            String strDSTruong = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sXauNoiMa," + MucLucNganSachModels.strDSDuocNhapTruongTien;
            String[] arrDSTruong = strDSTruong.Split(',');
            String[] arrDSGiaTri = DSGiaTri.Split(',');

            SqlCommand cmd = new SqlCommand();
            String DK = "", Truong;
            int i = 0;
            if (DSGiaTri != "")
            {
                for (i = 0; i < arrDSGiaTri.Length; i++)
                {
                    DK += String.Format("{0}=@{0} AND ", arrDSTruong[i]);
                    cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
                }
            }
            Truong = arrDSTruong[i];
            String DK_T = "";
            if (DSGiaTri != "")
            {
                switch (arrDSGiaTri.Length)
                {
                    case 0:
                        DK_T = " AND sL=''";
                        break;
                    case 2:
                    case 3:
                        DK_T = " AND sTM=''";
                        break;
                    case 4:
                        //DK_T = " AND (sTTM='')";
                        break;
                    case 5:
                        DK_T = "";
                        break;
                }
            }
            else
            {
                DK_T = " AND sL=''";
            }
            //truy van cu
            // String SQL = String.Format("SELECT TOP 2 * FROM NS_MucLucNganSach WHERE iTrangThai=1 AND bLaHangCha=0 {2} AND {1} {0} LIKE @{0}  ORDER BY sXauNoiMa", Truong, DK, DKLNS);
            //truy van da sua
            String SQL = String.Format("SELECT TOP 2 * FROM NS_MucLucNganSach WHERE iTrangThai=1  {2} AND {1} {0} LIKE @{0} AND sLNS LIKE @sLNS1 ORDER BY sXauNoiMa", Truong, DK, DKLNS, "");
            cmd.Parameters.AddWithValue("@" + Truong, GiaTri + "%");
            cmd.Parameters.AddWithValue("@sLNS1", sLNS + "%");
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            Object item = new
            {
                value = GiaTri,
                label = GiaTri,
                CoChiTiet = "0",
                CoHangPhuHop = "0"
            };

            if (dt.Rows.Count == 1)
            {
                String ThongTinThem = "";
                int csSTT = dt.Columns.IndexOf("iSTT");
                int d = 0;
                for (int j = 0; j < csSTT; j++)
                {
                    string TenTruong = dt.Columns[j].ColumnName;
                    if (d > 0)
                    {
                        ThongTinThem += "#|";
                    }
                    if (TenTruong.StartsWith("b"))
                    {
                        ThongTinThem += String.Format("{0}##{1}", TenTruong, Convert.ToBoolean(dt.Rows[0][TenTruong]) ? "1" : "0");
                    }
                    else
                    {
                        ThongTinThem += String.Format("{0}##{1}", TenTruong, dt.Rows[0][TenTruong]);
                    }
                    d++;
                }
                item = new
                {
                    value = dt.Rows[0][Truong],
                    label = dt.Rows[0][Truong],
                    CoChiTiet = "1",
                    iID_MaMucLucNganSach = dt.Rows[0]["iID_MaMucLucNganSach"],
                    ThongTinThem = ThongTinThem
                };
            }
            else if (dt.Rows.Count > 0)
            {
                //Thêm để lấy mô tả khi đến mục được chọn
                #region Tuannn thêm ngày 12/12/2012
                String ThongTinThem = "";
                int csSTT = dt.Columns.IndexOf("iSTT");
                int d = 0;
                for (int j = 0; j < csSTT; j++)
                {
                    string TenTruong = dt.Columns[j].ColumnName;
                    if (d > 0)
                    {
                        ThongTinThem += "#|";
                    }
                    if (TenTruong.StartsWith("b"))
                    {
                        ThongTinThem += String.Format("{0}##{1}", TenTruong, Convert.ToBoolean(dt.Rows[0][TenTruong]) ? "1" : "0");
                    }
                    else
                    {
                        ThongTinThem += String.Format("{0}##{1}", TenTruong, dt.Rows[0][TenTruong]);
                    }
                    d++;
                }
                #endregion
                item = new
                {
                    value = dt.Rows[0][Truong],
                    label = dt.Rows[0][Truong],
                    CoChiTiet = "1",// tuannn sửa 
                    //CoChiTiet = "0",
                    CoHangPhuHop = "1",
                    iID_MaMucLucNganSach = dt.Rows[0]["iID_MaMucLucNganSach"],// tuannn thêm mới
                    ThongTinThem = ThongTinThem// tuannn thêm mới
                };
            }
            dt.Dispose();

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTriMucLucNganSach(String GiaTri, String DSGiaTri)
        {
            String MaND = User.Identity.Name;
            DataTable dtLNS = NganSach_HamChungModels.DSLNSCuaPhongBan(MaND);
            String DKLNS = "";
            for (int j = 0; j < dtLNS.Rows.Count; j++)
            {
                DKLNS = DKLNS + Convert.ToString(dtLNS.Rows[j]["sLNS"]) + ",";
            }
            DKLNS = DKLNS.Remove(DKLNS.Length - 1);
            DKLNS = " AND sLNS IN (" + DKLNS + ") ";

            if (DSGiaTri == null) DSGiaTri = "";
            String strDSTruong = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,sXauNoiMa," + MucLucNganSachModels.strDSDuocNhapTruongTien;
            String[] arrDSTruong = strDSTruong.Split(',');
            String[] arrDSGiaTri = DSGiaTri.Split(',');

            SqlCommand cmd = new SqlCommand();
            String DK = "", Truong;
            int i = 0;
            if (DSGiaTri != "")
            {
                for (i = 0; i < arrDSGiaTri.Length; i++)
                {
                    DK += String.Format("{0}=@{0} AND ", arrDSTruong[i]);
                    cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
                }
            }
            Truong = arrDSTruong[i];
            String DK_T = "";
            if (DSGiaTri != "")
            {
                switch (arrDSGiaTri.Length)
                {
                    case 0:
                        DK_T = " AND sL=''";
                        break;
                    case 2:
                    case 3:
                        DK_T = " AND sTM=''";
                        break;
                    case 4:
                        //DK_T = " AND (sTTM='')";
                        break;
                    case 5:
                        DK_T = "";
                        break;
                }
            }
            else
            {
                DK_T = " AND sL=''";
            }
            //truy van cu
            // String SQL = String.Format("SELECT TOP 2 * FROM NS_MucLucNganSach WHERE iTrangThai=1 AND bLaHangCha=0 {2} AND {1} {0} LIKE @{0}  ORDER BY sXauNoiMa", Truong, DK, DKLNS);
            //truy van da sua
            String SQL = String.Format("SELECT TOP 2 * FROM NS_MucLucNganSach WHERE iTrangThai=1  {2} AND {1} {0} LIKE @{0}  ORDER BY sXauNoiMa", Truong, DK, DKLNS, "");
            cmd.Parameters.AddWithValue("@" + Truong, GiaTri + "%");
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            Object item = new
            {
                value = GiaTri,
                label = GiaTri,
                CoChiTiet = "0",
                CoHangPhuHop = "0"
            };

            if (dt.Rows.Count == 1)
            {
                String ThongTinThem = "";
                int csSTT = dt.Columns.IndexOf("iSTT");
                int d = 0;
                for (int j = 0; j < csSTT; j++)
                {
                    string TenTruong = dt.Columns[j].ColumnName;
                    if (d > 0)
                    {
                        ThongTinThem += "#|";
                    }
                    if (TenTruong.StartsWith("b"))
                    {
                        ThongTinThem += String.Format("{0}##{1}", TenTruong, Convert.ToBoolean(dt.Rows[0][TenTruong]) ? "1" : "0");
                    }
                    else
                    {
                        ThongTinThem += String.Format("{0}##{1}", TenTruong, dt.Rows[0][TenTruong]);
                    }
                    d++;
                }
                item = new
                {
                    value = dt.Rows[0][Truong],
                    label = dt.Rows[0][Truong],
                    CoChiTiet = "1",
                    iID_MaMucLucNganSach = dt.Rows[0]["iID_MaMucLucNganSach"],
                    ThongTinThem = ThongTinThem
                };
            }
            else if (dt.Rows.Count > 0)
            {
                //Thêm để lấy mô tả khi đến mục được chọn
                #region Tuannn thêm ngày 12/12/2012
                String ThongTinThem = "";
                int csSTT = dt.Columns.IndexOf("iSTT");
                int d = 0;
                for (int j = 0; j < csSTT; j++)
                {
                    string TenTruong = dt.Columns[j].ColumnName;
                    if (d > 0)
                    {
                        ThongTinThem += "#|";
                    }
                    if (TenTruong.StartsWith("b"))
                    {
                        ThongTinThem += String.Format("{0}##{1}", TenTruong, Convert.ToBoolean(dt.Rows[0][TenTruong]) ? "1" : "0");
                    }
                    else
                    {
                        ThongTinThem += String.Format("{0}##{1}", TenTruong, dt.Rows[0][TenTruong]);
                    }
                    d++;
                }
                #endregion
                item = new
                {
                    value = dt.Rows[0][Truong],
                    label = dt.Rows[0][Truong],
                    CoChiTiet = "1",// tuannn sửa 
                    //CoChiTiet = "0",
                    CoHangPhuHop = "1",
                    iID_MaMucLucNganSach = dt.Rows[0]["iID_MaMucLucNganSach"],// tuannn thêm mới
                    ThongTinThem = ThongTinThem// tuannn thêm mới
                };
            }
            dt.Dispose();

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriMucLucDuAn(String GiaTri)
        {
            string DK = "";
            SqlCommand cmd = new SqlCommand();
            if (HamRiengModels.IsValidGuid(GiaTri))
            {
                DK = "iID_MaDanhMucDuAn=@iID_MaDanhMucDuAn";
                cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", GiaTri);
            }
            else
            {
                String[] arrGiaTri = GiaTri.Split('-');
                int count = arrGiaTri.Count();
                if (GiaTri.IndexOf('-') != -1 && count >= 7)
                {
                    string Right = GiaTri.Substring(GiaTri.LastIndexOf('-') + 1);
                    string Left = GiaTri.Substring(0, GiaTri.LastIndexOf('-'));
                    // DK = "(sXauNoiMa_DuAn + '-' + sTenDuAn) LIKE @sTen";
                    // cmd.Parameters.AddWithValue("@sTen", "%" + GiaTri + "%");
                    DK = "(sXauNoiMa_DuAn LIKE @sTen OR sTenDuAn=@sTenHT)";
                    cmd.Parameters.AddWithValue("@sTen", Left + "%");
                    cmd.Parameters.AddWithValue("@sTenHT", Right + "%");
                }
                else
                {
                    DK = "(sXauNoiMa_DuAn LIKE @sTen OR sTenDuAn=@sTenHT)";
                    cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
                    cmd.Parameters.AddWithValue("@sTenHT", GiaTri + "%");
                }

            }
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                //string sXauNoiMa_DuAn = get_sXauNoiMa_DuAn(GiaTri);
                //DK = "  sXauNoiMa_DuAn=@sXauNoiMa_DuAn";
                //cmd.Parameters.AddWithValue("@sXauNoiMa_DuAn", sXauNoiMa_DuAn);

                List<Object> list = new List<Object>();
                String SQL = "SELECT iID_MaDanhMucDuAn,  sDeAn +'-'+ sDuAn +'-'+ sDuAnThanhPhan +'-'+ sCongTrinh +'-'+ sHangMucCongTrinh +'-'+ sHangMucChiTiet +'-'+ sTenDuAn AS sTenDuAn " +
                         "FROM QLDA_DanhMucDuAn " +
                         "WHERE iTrangThai = 1 AND {0} " +
                         "ORDER BY sXauNoiMa_DuAn";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaDanhMucDuAn"]),
                        label = String.Format("{0}", dt.Rows[0]["sTenDuAn"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriDonVi(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String MaND = User.Identity.Name;
                String DK = "iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi IN (SELECT iID_MaDonVi FROM NS_NguoiDung_DonVi WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND sMaNguoiDung=@sMaNguoiDung) AND ";
                String SQL = String.Format("SELECT TOP 1 iID_MaDonVi, sTen FROM NS_DonVi WHERE {0} (iID_MaDonVi LIKE @iID_MaDonVi) ORDER BY iID_MaDonVi", DK);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", GiaTri + "%");
                // cmd.Parameters.AddWithValue("@sTen", "%" + GiaTri + "%");
                // cmd.Parameters.AddWithValue("@sSoTaiKhoan", "%" + GiaTri + "%");
                cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaDonVi"]),
                        label = String.Format("{0}", dt.Rows[0]["iID_MaDonVi"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTrisLNS(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String MaND = User.Identity.Name;
                String SQL = String.Format(@"SELECT  sLNS, sMoTa FROM NS_MucLucNganSach
WHERE SUBSTRING(sLNS,1,1)=8 AND iTrangThai=1 
AND LEN(sLNS)=7 AND sL='' AND sLNS Like '%{0}%'
ORDER BY sLNS", GiaTri);
                SqlCommand cmd = new SqlCommand(SQL);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["sLNS"]),
                        label = String.Format("{0}", dt.Rows[0]["sLNS"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTriDonViCoTen(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String MaND = User.Identity.Name;
                List<Object> list = new List<Object>();
                String DK = "iTrangThai=1 AND iID_MaDonVi IN (SELECT iID_MaDonVi FROM NS_NguoiDung_DonVi WHERE iTrangThai=1) AND ";
                String SQL = String.Format("SELECT TOP 2 iID_MaDonVi, sTen FROM NS_DonVi WHERE {0} iID_MaDonVi LIKE @iID_MaDonVi AND iNamLamViec_DonVi=@iNamLamViec ORDER BY iID_MaDonVi", DK);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", GiaTri + "%");
               // cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaDonVi"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaDonVi"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriTaiKhoan(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = "",
                CoTaiKhoanGiaiThich = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String MaND = User.Identity.Name;
                int iNamLamViec = Convert.ToInt16(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
                List<Object> list = new List<Object>();

                SqlCommand cmd = new SqlCommand();
                String DK = "iTrangThai=1 AND bLaHangCha=0 AND iNam=@iNam AND iID_MaTaiKhoan LIKE @iID_MaTaiKhoan";
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", GiaTri + "%");
                cmd.Parameters.AddWithValue("@iNam", iNamLamViec);
                String SQL = String.Format("SELECT TOP 1 iID_MaTaiKhoan, sTen FROM KT_TaiKhoan WHERE {0} ORDER BY iID_MaTaiKhoan", DK);
                cmd.CommandText = SQL;
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    SQL = "SELECT Count(*) FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND iID_MaTaiKhoan=@iID_MaTaiKhoan";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", dt.Rows[0]["iID_MaTaiKhoan"]);
                    String CoTaiKhoanGiaiThich = (Convert.ToInt32(Connection.GetValue(cmd, 0)) > 0) ? "1" : "0";
                    cmd.Dispose();
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaTaiKhoan"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaTaiKhoan"], dt.Rows[0]["sTen"]),
                        CoTaiKhoanGiaiThich = CoTaiKhoanGiaiThich
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriCongTrinh(String GiaTri, String DSGiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = String.Format("SELECT TOP 10 sMaCongTrinh, sTen FROM NS_MucLucDuAn WHERE iTrangThai=1 AND (sMaCongTrinh LIKE @sMaCongTrinh OR sTen LIKE @sTen) AND iID_MaDonVi=@iID_MaDonVi ORDER BY sMaCongTrinh");
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sMaCongTrinh", GiaTri + "%");
                cmd.Parameters.AddWithValue("@sTen","%"+ GiaTri + "%");
                cmd.Parameters.AddWithValue("@iID_MaDonVi", DSGiaTri);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();


                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["sMaCongTrinh"]),
                        label = String.Format("{0}", dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriLoaiTaiSan(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = "",
                CoTaiKhoanGiaiThich = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String MaND = User.Identity.Name;
                int iNamLamViec = Convert.ToInt16(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
                List<Object> list = new List<Object>();

                SqlCommand cmd = new SqlCommand();
                String DK = "iTrangThai=1 AND (iID_MaLoaiTaiSan LIKE @iID_MaLoaiTaiSan OR sTen=@sTen)";
                cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", GiaTri + "%");
                cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
                String SQL = String.Format("SELECT TOP 1 iID_MaLoaiTaiSan, sTen FROM KTCS_LoaiTaiSan WHERE {0} ORDER BY iID_MaLoaiTaiSan", DK);
                cmd.CommandText = SQL;
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaLoaiTaiSan"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaLoaiTaiSan"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTriNhomTaiSan(String GiaTri, String DSGiaTri)
        {
            Object item = new
            {
                value = "",
                label = "",
                CoTaiKhoanGiaiThich = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String MaND = User.Identity.Name;
                int iNamLamViec = Convert.ToInt16(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
                List<Object> list = new List<Object>();

                SqlCommand cmd = new SqlCommand();
                String DK = "iTrangThai=1 AND bLaHangCha = 0  AND iID_MaLoaiTaiSan LIKE @iID_MaLoaiTaiSan AND iID_MaNhomTaiSan LIKE @iID_MaNhomTaiSan";
                cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan", GiaTri + "%");
                cmd.Parameters.AddWithValue("@iID_MaLoaiTaiSan", DSGiaTri);
                String SQL = String.Format("SELECT TOP 1 iID_MaNhomTaiSan, sTen FROM KTCS_NhomTaiSan WHERE {0} ORDER BY iID_MaNhomTaiSan", DK);
                cmd.CommandText = SQL;
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaNhomTaiSan"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaNhomTaiSan"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriTaiSan(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = "",
                CoTaiKhoanGiaiThich = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String MaND = User.Identity.Name;
                int iNamLamViec = Convert.ToInt16(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
                List<Object> list = new List<Object>();

                SqlCommand cmd = new SqlCommand();
                String DK = "iTrangThai=1 AND iID_MaTaiSan LIKE @iID_MaTaiSan";
                cmd.Parameters.AddWithValue("@iID_MaTaiSan", GiaTri + "%");
                String SQL = String.Format("SELECT TOP 1 iID_MaTaiSan, sTenTaiSan FROM KTCS_TaiSan WHERE {0} ORDER BY iID_MaTaiSan", DK);
                cmd.CommandText = SQL;
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();

                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaTaiSan"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaTaiSan"], dt.Rows[0]["sTenTaiSan"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriChuongTrinhMucTieu(String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = String.Format("SELECT TOP 1 iID_MaChuongTrinhMucTieu, sTen FROM KT_ChuongTrinhMucTieu WHERE iTrangThai=1 AND iID_MaChuongTrinhMucTieu LIKE @iID_MaChuongTrinhMucTieu ORDER BY iID_MaChuongTrinhMucTieu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChuongTrinhMucTieu", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            Object item = new
            {
                value = "",
                label = ""
            };
            if (dt.Rows.Count > 0)
            {
                item = new
                {
                    value = String.Format("{0}", dt.Rows[0]["iID_MaChuongTrinhMucTieu"]),
                    label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaChuongTrinhMucTieu"], dt.Rows[0]["sTen"])
                };
            }
            dt.Dispose();

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriPhongBan(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = String.Format("SELECT TOP 10 sKyHieu, sTen FROM NS_PhongBan WHERE iTrangThai=1 AND sKyHieu LIKE @sKyHieu ORDER BY sKyHieu");
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sKyHieu", GiaTri + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["sKyHieu"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["sKyHieu"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTriPhongBanDich(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = String.Format("SELECT TOP 10 sKyHieu, sTen FROM NS_PhongBan WHERE iTrangThai=1 AND sKyHieu LIKE @sKyHieu ORDER BY sKyHieu");
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sKyHieu", GiaTri + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["sKyHieu"]),
                        label = String.Format("{0}", dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTriNhanVien(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = String.Format("SELECT TOP 10 iID_MaNhanVien, sTen FROM KT_NhanVien WHERE iTrangThai=1 AND iID_MaNhanVien LIKE @sKyHieu ORDER BY iID_MaNhanVien");
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sKyHieu", GiaTri + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaNhanVien"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaNhanVien"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriChungTuCapThu(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = String.Format("SELECT TOP 10 iID_MaChungTu_Duyet, sSoChungTu, sNoiDung FROM KTTG_ChungTuCapThu_Duyet WHERE iTrangThai=1 AND rSoTien !=0 AND (iID_MaChungTu_Duyet=@iID_MaChungTu_Duyet OR sSoChungTu LIKE @sSoChungTu) ORDER BY sSoChungTu");
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sSoChungTu", GiaTri + "%");
                cmd.Parameters.AddWithValue("@iID_MaChungTu_Duyet", GiaTri);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaChungTu_Duyet"]),
                        label = String.Format("{0}", dt.Rows[0]["sSoChungTu"])
                        //label = String.Format("{0}", dt.Rows[0]["sSoChungTu"] + " - " + dt.Rows[0]["sNoiDung"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriPhuCap(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                GiaTri = GiaTri.Trim().ToUpper();
                List<Object> list = new List<Object>();
                String SQL = String.Format("SELECT * FROM L_DanhMucPhuCap WHERE iTrangThai=1 AND bLuonCo=0 ORDER BY iID_MaPhuCap");
                SqlCommand cmd = new SqlCommand(SQL);

                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();

                int cs = -1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int iLoaiMa = Convert.ToInt32(dt.Rows[i]["iLoaiMa"]);
                    String iID_MaPhuCap = Convert.ToString(dt.Rows[i]["iID_MaPhuCap"]);
                    int iDoDaiMaToiThieu = Convert.ToInt32(dt.Rows[i]["iDoDaiMaToiThieu"]);
                    if (iLoaiMa == 2)
                    {
                        if (iID_MaPhuCap == GiaTri)
                        {
                            cs = i;
                            break;
                        }
                    }
                    else
                    {
                        if (GiaTri.Length >= iDoDaiMaToiThieu && GiaTri.StartsWith(iID_MaPhuCap))
                        {
                            cs = i;
                        }
                    }
                }


                if (cs > 0)
                {
                    String iID_MaPhuCap = Convert.ToString(dt.Rows[cs]["iID_MaPhuCap"]);
                    int iLoaiMa = Convert.ToInt32(dt.Rows[cs]["iLoaiMa"]);


                    if (iLoaiMa == 0)
                    {
                        //Loaị mã kiểu AB trong đó B là hệ số phụ cấp
                        Double rHeSo = Convert.ToDouble(GiaTri.Substring(iID_MaPhuCap.Length));
                        if (iLoaiMa == 1)
                        {
                            rHeSo = rHeSo * 10;
                        }
                        dt.Rows[cs]["rHeSo"] = rHeSo;
                    }
                    else if (iLoaiMa == 1)
                    {
                        //Loaị mã kiểu AB trong đó B là hệ số phụ cấp
                        String sHeSo = GiaTri.Substring(iID_MaPhuCap.Length);
                        sHeSo = sHeSo.Insert(2, ".");
                        Double rHeSo = Convert.ToDouble(sHeSo);
                        if (iLoaiMa == 1)
                        {
                            rHeSo = rHeSo * 10;
                        }
                        dt.Rows[cs]["rHeSo"] = rHeSo;
                    }
                    String ThongTinThem = "";
                    int csSTT = dt.Columns.IndexOf("iSTT");
                    int d = 0;
                    String TenTruong;
                    for (int j = 0; j < csSTT; j++)
                    {
                        if (d > 0)
                        {
                            ThongTinThem += "#|";
                        }
                        TenTruong = dt.Columns[j].ColumnName;
                        if (TenTruong.StartsWith("b"))
                        {
                            ThongTinThem += String.Format("{0}##{1}", TenTruong, Convert.ToBoolean(dt.Rows[cs][TenTruong]) ? "1" : "0");
                        }
                        else
                        {
                            ThongTinThem += String.Format("{0}##{1}", TenTruong, dt.Rows[cs][TenTruong]);
                        }
                        d++;
                    }
                    item = new
                    {
                        value = GiaTri,
                        label = GiaTri,
                        ThongTinThem = ThongTinThem,
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriGiaiThich(String GiaTri, String iID_MaTaiKhoan)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String[] arrGiaTri = GiaTri.Split('-');
                //String SQL = "SELECT TOP 10 iID_MaTaiKhoanGiaiThich,sKyHieu, sTen " +
                //             "FROM KT_TaiKhoanGiaiThich " +
                //             "WHERE iTrangThai = 1" +
                //             " AND iID_MaTaiKhoan = @iID_MaTaiKhoan AND " +
                //                   "(sTen LIKE @sTen OR sKyHieu LIKE @sKyHieu)" +
                //             " ORDER BY sTen";

                String SQL = @"SELECT  ct.iID_MaTaiKhoanDanhMucChiTiet,ct.sKyHieu, ct.sTen from KT_TaiKhoanGiaiThich tk,  
KT_TaiKhoanDanhMucChiTiet ct where ct.iTrangThai = 1 AND tk.iTrangThai = 1 and tk.iID_MaTaiKhoan = @iID_MaTaiKhoan 
AND tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet AND (ct.sTen LIKE @sTen OR ct.sKyHieu LIKE @sKyHieu) ORDER BY ct.sKyHieu";
                SqlCommand cmd = new SqlCommand(SQL);
                if (arrGiaTri.Count() > 0)
                {
                    string GiaTri0 = arrGiaTri[0];
                    cmd.Parameters.AddWithValue("@sTen", "%" + GiaTri0 + "%");
                    cmd.Parameters.AddWithValue("@sKyHieu", GiaTri0 + "%");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@sTen", "%" + GiaTri + "%");
                    cmd.Parameters.AddWithValue("@sKyHieu", GiaTri + "%");
                }
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaTaiKhoanDanhMucChiTiet"]),
                        label = String.Format("{0}-{1}", dt.Rows[0]["sKyHieu"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriTinhChatCapThu(String GiaTri, String DSGiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String SQL = "SELECT TOP 10 iID_MaTinhChatCapThu, sTen " +
                             "FROM KTTG_TinhChatCapThu " +
                             "WHERE iTrangThai = 1  AND " +
                                   "(iID_MaTinhChatCapThu LIKE @iID_MaTinhChatCapThu OR sTen LIKE @sTen) " +
                             "ORDER BY sTen";
                SqlCommand cmd = new SqlCommand(SQL);
                //cmd.Parameters.AddWithValue("@bLoai", DSGiaTri);
                cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
                cmd.Parameters.AddWithValue("@iID_MaTinhChatCapThu", GiaTri + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaTinhChatCapThu"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaTinhChatCapThu"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriNgoaiTe(String GiaTri)
        {
            string DK = "";
            SqlCommand cmd = new SqlCommand();
            int num1;
            bool res = int.TryParse(GiaTri, out num1);
            if (res == false)
            {
                // String is not a number.
                DK = "sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
            }
            else
            {
                DK = "iID_MaNgoaiTe=@iID_MaNgoaiTe";
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", GiaTri);
            }
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = "SELECT TOP 10 iID_MaNgoaiTe, sTen " +
                             "FROM QLDA_NgoaiTe " +
                             "WHERE iTrangThai = 1 AND {0} " +
                             "ORDER BY sTen";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaNgoaiTe"]),
                        label = String.Format("{0}", dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriChuDauTu(String GiaTri, String DSGiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = "SELECT TOP 10 iID_MaChuDauTu, sTen " +
                             "FROM QLDA_ChuDauTu " +
                             "WHERE iTrangThai = 1 AND iID_MaDonVi=@iID_MaDonVi AND " +
                                   " (iID_MaChuDauTu LIKE @iID_MaChuDauTu  OR sTen LIKE @iID_MaChuDauTu)" +
                             "ORDER BY sTen";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaChuDauTu", GiaTri + "%");
                cmd.Parameters.AddWithValue("@iID_MaDonVi", DSGiaTri);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaChuDauTu"]),
                        label = String.Format("{0}", dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriBanQuanLy(String GiaTri, String DSGiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = "SELECT TOP 10 iID_MaBanQuanLy, sTenBanQuanLy " +
                             "FROM QLDA_BanQuanLy " +
                             "WHERE iTrangThai = 1 AND iID_MaChuDauTu=@iID_MaChuDauTu AND " +
                                   "(iID_MaBanQuanLy LIKE @iID_MaBanQuanLy OR sTenBanQuanLy LIKE @iID_MaBanQuanLy)" +
                             "ORDER BY sTenBanQuanLy";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaBanQuanLy", GiaTri + "%");
                cmd.Parameters.AddWithValue("@iID_MaChuDauTu", DSGiaTri);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaBanQuanLy"]),
                        label = String.Format("{0}", dt.Rows[0]["sTenBanQuanLy"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriDonViThiCong(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = "SELECT TOP 10 iID_MaDonViThiCong, sTen " +
                             "FROM QLDA_DonViThiCong " +
                             "WHERE iTrangThai = 1 AND " +
                                   "(iID_MaDonViThiCong LIKE @iID_MaDonViThiCong OR sTen LIKE @sTen)" +
                             "ORDER BY sTen";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaDonViThiCong", GiaTri + "%");
                cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaDonViThiCong"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaDonViThiCong"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTrLoaiKeHoachVon(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = "SELECT TOP 10 iID_MaLoaiKeHoachVon, sTen " +
                             "FROM QLDA_KeHoachVon_Loai " +
                             "WHERE iTrangThai = 1 AND " +
                                   "(iID_MaLoaiKeHoachVon LIKE @iID_MaLoaiKeHoachVon OR sTen LIKE @sTen)" +
                             "ORDER BY sTen";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaLoaiKeHoachVon", GiaTri + "%");
                cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaLoaiKeHoachVon"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaLoaiKeHoachVon"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTrLoaiDieuChinh(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = "SELECT TOP 10 iID_MaLoaiDieuChinh, sTen " +
                             "FROM QLDA_LoaiDieuChinh " +
                             "WHERE iTrangThai = 1 AND " +
                                   "(iID_MaLoaiDieuChinh LIKE @iID_MaLoaiDieuChinh OR sTen LIKE @sTen)" +
                             "ORDER BY sTen";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaLoaiDieuChinh", GiaTri + "%");
                cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaLoaiDieuChinh"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaLoaiDieuChinh"], dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_GiaTriNgachLuong(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String[] arrTuKhoa = GiaTri.Split('-');
                String TuKhoa = arrTuKhoa[0].Trim();
                SqlCommand cmd = new SqlCommand("SELECT * FROM L_DanhMucNgachLuong WHERE iTrangThai=1 AND iID_MaNgachLuong LIKE @iID_MaNgachLuong ORDER BY iID_MaNgachLuong");
                cmd.Parameters.AddWithValue("@iID_MaNgachLuong", TuKhoa + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaNgachLuong"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaNgachLuong"], dt.Rows[0]["sTenNgachLuong"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriBacLuong(String GiaTri, String iID_MaNgachLuong)
        {
            Object item = new
            {
                value = "",
                label = "",
                ThongTinThem = "0"
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String[] arrTuKhoa = GiaTri.Split('-');
                String TuKhoa = arrTuKhoa[0].Trim();
                SqlCommand cmd = new SqlCommand("SELECT * FROM L_DanhMucBacLuong WHERE iTrangThai=1 AND iID_MaBacLuong LIKE @iID_MaBacLuong AND iID_MaNgachLuong = @iID_MaNgachLuong ORDER BY iID_MaBacLuong");
                cmd.Parameters.AddWithValue("@iID_MaBacLuong", TuKhoa + "%");
                cmd.Parameters.AddWithValue("@iID_MaNgachLuong", iID_MaNgachLuong);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaBacLuong"]),
                        label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaBacLuong"], dt.Rows[0]["sTenBacLuong"]),
                        ThongTinThem = dt.Rows[0]["rHeSoLuong"]
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_GiaTriLyDoTangGiam(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = "S",
                MoTa = "",
                sHieuTangGiam = "S"
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String[] arrTuKhoa = GiaTri.Split('-');
                String TuKhoa = arrTuKhoa[arrTuKhoa.Length - 1].Trim();
                SqlCommand cmd = new SqlCommand("SELECT * FROM NS_MucLucQuanSo WHERE iTrangThai=1 AND sKyHieu LIKE @sKyHieu ORDER BY sKyHieu");
                cmd.Parameters.AddWithValue("@sKyHieu", TuKhoa + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    String sHieuTangGiam = "T";
                    String sKyHieu = Convert.ToString(dt.Rows[0]["sKyHieu"]);
                    if (sKyHieu.StartsWith("3")) sHieuTangGiam = "G";
                    item = new
                    {
                        value = sKyHieu,
                        label = String.Format("{0} - {1}", sHieuTangGiam, sKyHieu),
                        MoTa = dt.Rows[0]["sMoTa"],
                        sHieuTangGiam = sHieuTangGiam
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        private JsonResult get_GiaTriTruongBangLuongChiTiet(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String SQL = String.Format("sTenTruongPhuCap LIKE '{0}%'", GiaTri);
                DataTable dtTruongPhuCap = LuongModels.dt_TruongPhuCap();
                DataRow[] arrR = dtTruongPhuCap.Select(SQL);

                DataTable dt = new DataTable();
                dt.Columns.Add("sTenTruongPhuCap", typeof(String));
                dt.Columns.Add("sTruongPhuCap", typeof(String));
                int i;
                DataRow R;
                for (i = 0; i < arrR.Length; i++)
                {
                    R = dt.NewRow();
                    R["sTenTruongPhuCap"] = arrR[i]["sTenTruongPhuCap"];
                    R["sTruongPhuCap"] = arrR[i]["sTruongPhuCap"];
                    dt.Rows.Add(R);
                }
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["sTenTruongPhuCap"]),
                        label = String.Format("{0}", dt.Rows[0]["sTruongPhuCap"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_GiaTriKyHieuHachToan(String GiaTri)
        {
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                String SQL = "SELECT  DISTINCT iID_MaKyHieuHachToan" +
                         " FROM KTCS_KyHieuHachToanChiTiet" +
                         " WHERE iTrangThai = 1 AND " +
                               " (iID_MaKyHieuHachToan LIKE @iID_MaKyHieuHachToan)";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaKyHieuHachToan", GiaTri + "%");
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaKyHieuHachToan"]),
                        label = String.Format("{0}", dt.Rows[0]["iID_MaKyHieuHachToan"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        private JsonResult get_GiaTriNgoaiTe_KeToan(String GiaTri)
        {
            string DK = "";
            SqlCommand cmd = new SqlCommand();
            int num1;
            bool res = int.TryParse(GiaTri, out num1);
            if (res == false)
            {
                // String is not a number.
                DK = "sTen LIKE @sTen";
                cmd.Parameters.AddWithValue("@sTen", GiaTri + "%");
            }
            else
            {
                DK = "iID_MaNgoaiTe=@iID_MaNgoaiTe";
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", GiaTri);
            }
            Object item = new
            {
                value = "",
                label = ""
            };
            if (String.IsNullOrEmpty(GiaTri) == false)
            {
                List<Object> list = new List<Object>();
                String SQL = "SELECT iID_MaNgoaiTe, sTen " +
                             " FROM KTKB_NgoaiTe " +
                             "WHERE iTrangThai = 1 AND {0} " +
                             "ORDER BY sTen";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    item = new
                    {
                        value = String.Format("{0}", dt.Rows[0]["iID_MaNgoaiTe"]),
                        label = String.Format("{0}", dt.Rows[0]["sTen"])
                    };
                }
                dt.Dispose();
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public static string get_sXauNoiMa_DuAn(String iID_MaDanhMucDuAn)
        {
            string vR;
            SqlCommand cmd = new SqlCommand();
            String DK = "iTrangThai = 1 ";
            if (String.IsNullOrEmpty(iID_MaDanhMucDuAn) == false && iID_MaDanhMucDuAn != Convert.ToString(Guid.Empty))
            {
                DK += " AND iID_MaDanhMucDuAn = @iID_MaDanhMucDuAn";
                cmd.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
            }

            String SQL = String.Format("SELECT sXauNoiMa_DuAn FROM QLDA_DanhMucDuAn WHERE {0}", DK); //DISTINCT iID_MaDanhMucDuAn, dNgayLap
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();

            return vR;
        }
        #endregion


    }
}
