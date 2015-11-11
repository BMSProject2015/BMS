using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;

namespace VIETTEL.Controllers.KeToanTongHop
{
    public class KeToanTongHop_CanDoiThuChiTaiChinhController : Controller
    {
        //
        // GET: /KeToanTongHop_ChungTuChiTiet/
        public string sViewPath = "~/Views/KeToanTongHop/ChungTuChiTiet/";

        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "KeToanTongHop_CanDoiThuChiTaiChinh_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
           
        }
        [Authorize]
        public ActionResult Edit()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return View(sViewPath + "KeToanTongHop_TaoThuChi.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

        }
        [Authorize]
        public ActionResult ChungTuChiTiet_Frame()
        {
            return View(sViewPath + "KeToanTongHop_CanDoiThuChiTaiChinh_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iNam, String iQuy, String iLoai)
        {
            String MaND = User.Identity.Name;
            iNam = Request.Form["iNam"];
            if (String.IsNullOrEmpty(iNam))
                iNam = Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));

            String TenBangChiTiet = "KT_CanDoiThuChiTaiChinh";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] {BangDuLieu.DauCachHang},
                                                              StringSplitOptions.None);

            String iID_MaThuChi;
            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] {BangDuLieu.DauCachHang},
                                                               StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                iID_MaThuChi = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaThuChi != "")
                    {
                        //Dữ liệu đã có
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = iID_MaThuChi;
                        bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.Save();
                    }
                }
                else
                {
                    String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] {BangDuLieu.DauCachO},
                                                                StringSplitOptions.None);
                    String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] {BangDuLieu.DauCachO},
                                                                  StringSplitOptions.None);

                    Boolean okCoThayDoi = false;
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            okCoThayDoi = true;
                            break;
                        }
                    }
                    if (okCoThayDoi)
                    {
                        Bang bang = new Bang(TenBangChiTiet);
                        iID_MaThuChi = arrMaHang[i];
                        if (iID_MaThuChi == "")
                        {
                            //Du Lieu Moi
                            bang.DuLieuMoi = true;
                            bang.CmdParams.Parameters.AddWithValue("@iNam", iNam);
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaThuChi;
                            bang.DuLieuMoi = false;
                        }
                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        //Them tham so
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                String Truong = "@" + arrMaCot[j];
                                if (arrMaCot[j].StartsWith("b"))
                                {
                                    //Nhap Kieu checkbox
                                    if (arrGiaTri[j] == "1")
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                    }
                                    else
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                    }
                                }
                                else if (arrMaCot[j].StartsWith("r") ||
                                         (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                                {
                                    //Nhap Kieu so
                                    if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong,
                                                                               Convert.ToDouble(arrGiaTri[j]));
                                    }
                                }
                                else if (arrMaCot[j].StartsWith("iID"))
                                {
                                    //Nhap Kieu so
                                    if (!String.IsNullOrEmpty(arrGiaTri[j]))
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong,
                                                                               Convert.ToDouble(arrGiaTri[j]));
                                    }
                                    else
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong, DBNull.Value);
                                    }
                                }
                                else
                                {
                                    //Nhap kieu xau
                                    bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                }
                            }
                        }
                        //Remove cột sID_MaNguoiDungTao
                        if (bang.CmdParams.Parameters.IndexOf("@sID_MaNguoiDungTao") > 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(bang.CmdParams.Parameters.IndexOf("@sID_MaNguoiDungTao"));
                        }
                        //if (KeToanTongHop_ChungTuChiTietModels.KiemTraCoDuocCapNhapBang(bang))
                        //{
                        //    bang.Save();
                        //}
                        bang.CmdParams.Parameters.AddWithValue("@iQuy", iQuy);
                        bang.Save();
                        CapNhapLai(Convert.ToInt32(iQuy), Convert.ToInt32(iNam));
                    }
                }
            }
            ViewData["Saved"] = "1";
            return RedirectToAction("Index", "KeToanTongHop_CanDoiThuChiTaiChinh",
                                    new {iNam = iNam, iQuy = iQuy, iLoai = iLoai});
            //return View(sViewPath + "KeToanTongHop_CanDoiThuChiTaiChinh_Index.aspx");
        }

        // cap nhat lai
        public static void CapNhapLai(int iQuy, int iNam)
        {
            SqlCommand cmd;
            String SQL =
                String.Format(
                    @"UPDATE KT_CanDoiThuChiTaiChinh  set iID_MaThuChi_Cha=(select top 1 iID_MaThuChi from 
KT_CanDoiThuChiTaiChinh kt where  (kt.sKyHieu=KT_CanDoiThuChiTaiChinh.sKyHieu_Cha) and kt.iTrangThai=1 AND kt.iNam=@iNam AND kt.iQuy=@iQuy)
WHERE iTrangThai=1 AND iNam=@iNam AND iQuy=@iQuy; --and iID_MaThuChi=@iID_MaThuChi;
UPDATE KT_CanDoiThuChiTaiChinh set bLaHangCha=1 where bLaHangCha=0 AND iNam=@iNam AND iQuy=@iQuy and exists (select iID_MaThuChi from KT_CanDoiThuChiTaiChinh ct where ct.iTrangThai=1 AND ct.iNam=@iNam AND ct.iQuy=@iQuy AND ct.sKyHieu_Cha=KT_CanDoiThuChiTaiChinh.sKyHieu);
UPDATE KT_CanDoiThuChiTaiChinh set bLaHangCha=0 where bLaHangCha=1 AND iNam=@iNam AND iQuy=@iQuy and not exists (select iID_MaThuChi from KT_CanDoiThuChiTaiChinh ct where ct.iTrangThai=1 AND ct.iNam=@iNam AND ct.iQuy=@iQuy AND ct.sKyHieu_Cha=KT_CanDoiThuChiTaiChinh.sKyHieu);");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_ThuChi(String ParentID)
        {
            String iQuy_LaySL = Request.Form[ParentID + "_iQuy_LaySL"];
            String iNam_LaySL = Request.Form[ParentID + "_iNam_LaySL"];
            String iQuy = Request.Form[ParentID + "_iQuy"];
            String iNam = Request.Form[ParentID + "_iNam"];
            
            NameValueCollection arrLoi = new NameValueCollection();
            int Count_LaySL = KiemTra_TonTai_ThuChi(Convert.ToInt32(iQuy_LaySL), Convert.ToInt32(iNam_LaySL));
            if (Count_LaySL == 0)
            {
                arrLoi.Add("err_iQuy_LaySL", "Quý/năm chọn chưa có cấu hình cân đối thu chi");
            }
            int Count_Dua_DL = KiemTra_TonTai_ThuChi(Convert.ToInt32(iQuy), Convert.ToInt32(iNam));
            if (Count_Dua_DL > 0)
            {
                arrLoi.Add("err_iQuy", "Quý/năm chọn đã có cấu hình cân đối thu chi");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }

                return RedirectToAction("Index", "KeToanTongHop_CanDoiThuChiTaiChinh");

            }
            else
            {
                String SQL = @"--Cap nhat  danh muc bang can doi thu chi
update KT_CanDoiThuChiTaiChinh set iTrangThai=0 where iNam=@iNamSau AND iQuy=@iQuy;
INSERT INTO KT_CanDoiThuChiTaiChinh(iID_MaThuChi_Cha,bLaHangCha,bHienThi,bTuDongTinh,iQuy,iNam,iLoaiThuChi,sXauNoiMa
,sKyHieu,sKyHieu_Cha,sNoiDung,sTenTaiKhoan_TienViet,bCoTKGT_TienViet,sTenTaiKhoan_NgoaiTe
,bCoTKGT_NgoaiTe,sTenTaiKhoan_Tong,bCoTKGT_Tong,bTuTinhTong,sID_MaNguoiDungTao) 
 SELECT iID_MaThuChi_Cha,bLaHangCha,bHienThi,bTuDongTinh,@iQuy,@iNamSau,iLoaiThuChi,sXauNoiMa
,sKyHieu,sKyHieu_Cha,sNoiDung,sTenTaiKhoan_TienViet,bCoTKGT_TienViet,sTenTaiKhoan_NgoaiTe
,bCoTKGT_NgoaiTe,sTenTaiKhoan_Tong,bCoTKGT_Tong,bTuTinhTong,@sID_MaNguoiDungTao from KT_CanDoiThuChiTaiChinh where iTrangThai=1 and iNam=@iNam_LaySL AND iQuy=@iQuy_LaySL;
UPDATE KT_CanDoiThuChiTaiChinh  set iID_MaThuChi_Cha=(select top 1 iID_MaThuChi from 
KT_CanDoiThuChiTaiChinh kt where  (kt.sKyHieu=KT_CanDoiThuChiTaiChinh.sKyHieu_Cha) and kt.iTrangThai=1 AND kt.iNam=@iNamSau AND kt.iQuy=@iQuy)
WHERE iTrangThai=1 AND iNam=@iNamSau AND iQuy=@iQuy; --and iID_MaThuChi=@iID_MaThuChi;
UPDATE KT_CanDoiThuChiTaiChinh set bLaHangCha=1 where bLaHangCha=0 AND iNam=@iNamSau AND iQuy=@iQuy and exists (select iID_MaThuChi from KT_CanDoiThuChiTaiChinh ct where ct.iTrangThai=1 AND ct.iNam=@iNamSau AND ct.iQuy=@iQuy AND ct.sKyHieu_Cha=KT_CanDoiThuChiTaiChinh.sKyHieu);
UPDATE KT_CanDoiThuChiTaiChinh set bLaHangCha=0 where bLaHangCha=1 AND iNam=@iNamSau AND iQuy=@iQuy and not exists (select iID_MaThuChi from KT_CanDoiThuChiTaiChinh ct where ct.iTrangThai=1 AND ct.iNam=@iNamSau AND ct.iQuy=@iQuy AND ct.sKyHieu_Cha=KT_CanDoiThuChiTaiChinh.sKyHieu);";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
                cmd.Parameters.AddWithValue("@iNamSau", iNam);
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", User.Identity.Name);
                cmd.Parameters.AddWithValue("@iQuy_LaySL", iQuy_LaySL);
                cmd.Parameters.AddWithValue("@iNam_LaySL", iNam_LaySL);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
                return RedirectToAction("Index", "KeToanTongHop_CanDoiThuChiTaiChinh");
            }
        }
        public static int KiemTra_TonTai_ThuChi(int iQuy, int iNamLamViec)
        {
            SqlCommand cmd =
                new SqlCommand(
                    "SELECT COUNT(*) FROM KT_CanDoiThuChiTaiChinh WHERE iTrangThai=1  AND iNam=@iNamLamViec AND iQuy=@iQuy");
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            int vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }

        ///// <summary>
        ///// Thêm dữ liệu từ bảng KTTG_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        ///// </summary>
        ///// <param name="ParentID"></param>
        ///// <param name="OnSuccess"></param>
        ///// <param name="iID_MaChungTu"></param>
        ///// <param name="MaDiv"></param>
        ///// <returns></returns>
        //public JavaScriptResult Edit_Fast_TienGui_Submit(String ParentID, String OnSuccess, String iID_MaChungTu, String iThang, String iNam, String MaND, String IPSua)
        //{
        //    NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

        //    String iNgay = Request.Form[ParentID + "_iNgay"];
        //    String strMaChungTuChiTietTienGui = Request.Form[ParentID + "_txt"];
        //    String sDonVi = Request.Form[ParentID + "_sDonVi"];
        //    String sSoChungTu = Request.Form[ParentID + "_sSoChungTu"];
        //   // return JavaScript(String.Format("Dialog_close('{0}');", ParentID));

        //    if (String.IsNullOrEmpty(strMaChungTuChiTietTienGui) || String.IsNullOrEmpty(sSoChungTu))
        //        return JavaScript(String.Format("Dialog_close('{0}');", ParentID));
        //    else
        //    {
        //        Boolean KiemTra_sSoChungTu = KeToanTongHop_ChungTuModels.KiemTra_sSoChungTu(sSoChungTu, iNam);

        //        String[] arrMaChungTuChiTietTienGui = strMaChungTuChiTietTienGui.Split(',');
        //        int i;
        //        for (i = 0; i < arrMaChungTuChiTietTienGui.Length; i++)
        //        {
        //            if (arrMaChungTuChiTietTienGui[i] != "" && String.IsNullOrEmpty(arrMaChungTuChiTietTienGui[i]) == false)
        //            {
        //                //Lấy chi tiết bảng chi tiết của phần tiền gửi: KTTG_ChungTuChiTiet
        //                int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, iThang, iNam, arrMaChungTuChiTietTienGui[i], 2);
        //                if (intDaCo == 0)
        //                {
        //                    DataTable dt = KTCT_TienGui_ChungTuChiTietModels.Get_dtChungTuChiTiet_Row(arrMaChungTuChiTietTienGui[i]);
        //                    DataRow R = dt.Rows[0];

        //                    //Insert into data vào bảng: KT_ChungTuChiTiet
        //                    Bang bang = new Bang("KT_ChungTuChiTiet");
        //                    bang.DuLieuMoi = true;
        //                    bang.MaNguoiDungSua = User.Identity.Name;
        //                    bang.IPSua = Request.UserHostAddress;
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienGui", arrMaChungTuChiTietTienGui[i]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
        //                    bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "UNC-" + R["sSoChungTuChiTiet"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sNoiDung"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rSoTien"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R["iID_MaTaiKhoan_No"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R["sTenTaiKhoan_No"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R["iID_MaTaiKhoan_Co"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R["sTenTaiKhoan_Co"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R["iID_MaDonVi_No"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R["sTenDonVi_No"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R["iID_MaDonVi_Co"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R["sTenDonVi_Co"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

        //                    bang.Save();
        //                    dt.Dispose();
        //                }
        //            }
        //        }

        //        String strJ = "";
        //        if (String.IsNullOrEmpty(OnSuccess) == false)
        //        {
        //            strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
        //        }
        //        else
        //        {
        //            strJ = String.Format("Dialog_close('{0}');", ParentID);
        //        }
        //        return JavaScript(strJ);
        //    }
        //}

        ///// <summary>
        ///// Thêm dữ liệu từ bảng KTTM_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        ///// </summary>
        ///// <param name="ParentID"></param>
        ///// <param name="OnSuccess"></param>
        ///// <param name="iID_MaChungTu"></param>
        ///// <returns></returns>
        //public JavaScriptResult Edit_Fast_TienMat_Submit(String ParentID, String OnSuccess, String iID_MaChungTu, String iThang, String iNam)
        //{
        //    NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

        //    String iNgay = Request.Form[ParentID + "_iNgay"];
        //    String strMaChungTuChiTietTienMat = Request.Form[ParentID + "_txt"];
        //    if (String.IsNullOrEmpty(strMaChungTuChiTietTienMat))
        //        return JavaScript(String.Format("Dialog_close('{0}');", ParentID));
        //    else
        //    {
        //        String[] arrMaChungTuChiTietTienMat = strMaChungTuChiTietTienMat.Split(',');

        //        int i;
        //        for (i = 0; i < arrMaChungTuChiTietTienMat.Length; i++)
        //        {
                    
        //            if (arrMaChungTuChiTietTienMat[i] != "" && String.IsNullOrEmpty(arrMaChungTuChiTietTienMat[i]) == false)
        //            {
        //                int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, iThang, iNam, arrMaChungTuChiTietTienMat[i], 3);
        //                if (intDaCo == 0)
        //                {
        //                    //Lấy chi tiết bảng chi tiết của phần tiền gửi: KTTM_ChungTuChiTiet
        //                    DataTable dt = KTCT_TienMat_ChungTuChiTietModels.Get_dtChungTuChiTiet_Row(arrMaChungTuChiTietTienMat[i]);
        //                    DataRow R = dt.Rows[0];

        //                    //Insert into data vào bảng: KT_ChungTuChiTiet
        //                    Bang bang = new Bang("KT_ChungTuChiTiet");
        //                    bang.DuLieuMoi = true;
        //                    bang.MaNguoiDungSua = User.Identity.Name;
        //                    bang.IPSua = Request.UserHostAddress;
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienMat", arrMaChungTuChiTietTienMat[i]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
        //                    bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "PTPC-" + R["sSoChungTuChiTiet"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sNoiDung"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rSoTien"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R["iID_MaTaiKhoan_No"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R["sTenTaiKhoan_No"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R["iID_MaTaiKhoan_Co"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R["sTenTaiKhoan_Co"]);

        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R["iID_MaDonVi_No"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R["sTenDonVi_No"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R["iID_MaDonVi_Co"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R["sTenDonVi_Co"]);

        //                    bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

        //                    bang.Save();
        //                    dt.Dispose();
        //                }
        //            }
        //        }

        //        String strJ = "";
        //        if (String.IsNullOrEmpty(OnSuccess) == false)
        //        {
        //            strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
        //        }
        //        else
        //        {
        //            strJ = String.Format("Dialog_close('{0}');", ParentID);
        //        }
        //        return JavaScript(strJ);
        //    }
        //}
        ///// <summary>
        ///// Thêm dữ liệu từ bảng KTKB_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        ///// </summary>
        ///// <param name="ParentID"></param>
        ///// <param name="OnSuccess"></param>
        ///// <param name="iID_MaChungTu"></param>
        ///// <returns></returns>
        //public JavaScriptResult Edit_Fast_KhoBac_Submit(String ParentID, String OnSuccess, String iID_MaChungTu, String iThang, String iNam)
        //{
        //    NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

        //    String iNgay = Request.Form[ParentID + "_iNgay"];
        //    String strMaChungTuChiTietTienMat = Request.Form[ParentID + "_txt"];
        //    if (String.IsNullOrEmpty(strMaChungTuChiTietTienMat))
        //        return JavaScript(String.Format("Dialog_close('{0}');", ParentID));
        //    else
        //    {
        //        String[] arrMaChungTuChiTietTienMat = strMaChungTuChiTietTienMat.Split(',');
        //        String optNoCo = Convert.ToString(Request.Form[ParentID + "_optNoCo"]);
        //        String sTaiKhoan = Convert.ToString(Request.Form[ParentID + "_sTaiKHoan"]);
        //        String sTenTaiKhoan = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(sTaiKhoan);

        //        int i;
        //        for (i = 0; i < arrMaChungTuChiTietTienMat.Length; i++)
        //        {
        //            if (arrMaChungTuChiTietTienMat[i] != "" && String.IsNullOrEmpty(arrMaChungTuChiTietTienMat[i]) == false)
        //            {
        //                int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, iThang, iNam, arrMaChungTuChiTietTienMat[i], 1);
        //                if (intDaCo == 0)
        //                {
        //                    //Lấy chi tiết bảng chi tiết của phần tiền gửi: KTTM_ChungTuChiTiet
        //                    DataTable dt = KTCT_KhoBac_ChungTuChiTietModels.Get_dtChungTuChiTiet_Row(arrMaChungTuChiTietTienMat[i]);
        //                    DataRow R = dt.Rows[0];

        //                    //Insert into data vào bảng: KT_ChungTuChiTiet
        //                    Bang bang = new Bang("KT_ChungTuChiTiet");
        //                    bang.DuLieuMoi = true;
        //                    bang.MaNguoiDungSua = User.Identity.Name;
        //                    bang.IPSua = Request.UserHostAddress;
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_KhoBac", arrMaChungTuChiTietTienMat[i]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
        //                    bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "RDT-" + R["sSoChungTuChiTiet"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sNoiDung"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rDTRut"]);
        //                    switch (optNoCo)
        //                    {
        //                        case "tkNo":
        //                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", sTaiKhoan);
        //                            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", sTenTaiKhoan);
        //                            break;
        //                        case "tkCo":
        //                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", sTaiKhoan);
        //                            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", sTenTaiKhoan);
        //                            break;
        //                        case "tkTrong":
        //                            break;
        //                    }
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R["iID_MaDonVi_Nhan"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R["sTenDonVi_Nhan"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R["iID_MaDonVi_Tra"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R["sTenDonVi_Tra"]);
        //                    bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

        //                    bang.Save();
        //                    dt.Dispose();
        //                }
        //            }
        //        }

        //        String strJ = "";
        //        if (String.IsNullOrEmpty(OnSuccess) == false)
        //        {
        //            strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
        //        }
        //        else
        //        {
        //            strJ = String.Format("Dialog_close('{0}');", ParentID);
        //        }
        //        return JavaScript(strJ);
        //    }
        //}
        ///// <summary>
        ///// Thêm dữ liệu từ bảng QTA_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        ///// </summary>
        ///// <param name="ParentID"></param>
        ///// <param name="OnSuccess"></param>
        ///// <param name="iID_MaChungTu"></param>
        ///// <returns></returns>
        //public JavaScriptResult Edit_Fast_QuyetToan_Submit(String ParentID, String OnSuccess, String iID_MaChungTu)
        //{
        //    NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

        //    String iNgay = Request.Form[ParentID + "_iNgay"];
        //    String strMaChungTuChiTietQuyetToan = Request.Form[ParentID + "_txt"];
        //    String[] arrMaChungTuChiTietQuyetToan = strMaChungTuChiTietQuyetToan.Split(',');

        //    int i;
        //    for (i = 0; i < arrMaChungTuChiTietQuyetToan.Length; i++)
        //    {
        //        if (arrMaChungTuChiTietQuyetToan[i] != "")
        //        {
        //            //Lấy chi tiết bảng chi tiết của phần tiền gửi: QTA_ChungTuChiTiet
        //            DataTable dt = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTietQuyetToan_TheoMaQuyetToan(arrMaChungTuChiTietQuyetToan[i]);
        //            DataRow R = dt.Rows[0];

        //            //Insert into data vào bảng: KT_ChungTuChiTiet
        //            Bang bang = new Bang("KT_ChungTuChiTiet");
        //            bang.DuLieuMoi = true;
        //            bang.MaNguoiDungSua = User.Identity.Name;
        //            bang.IPSua = Request.UserHostAddress;
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_QuyetToan", arrMaChungTuChiTietQuyetToan[i]);
        //            bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
        //            bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "QT-" + R["sTienToChungTu"] + R["iSoChungTu"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sLNS"] + "-" + R["sMoTa"]);
        //            bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rTongSo"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R["iID_MaTaiKhoan_No"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R["sTenTaiKhoan_No"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R["iID_MaTaiKhoan_Co"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R["sTenTaiKhoan_Co"]);
        //            //bang.CmdParams.Parameters.AddWithValue("@sKyHieuPhongBan_No", R["sKyHieuPhongBan_Tra"]);
        //            //bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan_No", R["sTenPhongBan_Tra"]);
        //            //bang.CmdParams.Parameters.AddWithValue("@sKyHieuPhongBan_Co", R["sKyHieuPhongBan_Tra"]);
        //            //bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan_Co", R["sTenPhongBan_Tra"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

        //            bang.Save();
        //            dt.Dispose();
        //        }
        //    }

        //    String strJ = "";
        //    if (String.IsNullOrEmpty(OnSuccess) == false)
        //    {
        //        strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
        //    }
        //    else
        //    {
        //        strJ = String.Format("Dialog_close('{0}');", ParentID);
        //    }
        //    return JavaScript(strJ);
        //}
        ///// <summary>
        ///// Thêm dữ liệu từ bảng KTCS_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        ///// </summary>
        ///// <param name="ParentID"></param>
        ///// <param name="OnSuccess"></param>
        ///// <param name="iID_MaChungTu"></param>
        ///// <returns></returns>
        //public JavaScriptResult Edit_Fast_CongSan_Submit(String ParentID, String OnSuccess, String iID_MaChungTu)
        //{
        //    NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

        //    String iNgay = Request.Form[ParentID + "_iNgay"];
        //    String strMaChungTuChiTietQuyetToan = Request.Form[ParentID + "_txt"];
        //    String[] arrMaChungTuChiTietQuyetToan = strMaChungTuChiTietQuyetToan.Split(',');

        //    int i;
        //    for (i = 0; i < arrMaChungTuChiTietQuyetToan.Length; i++)
        //    {
        //        if (arrMaChungTuChiTietQuyetToan[i] != "")
        //        {
        //            //Lấy chi tiết bảng chi tiết của phần tiền gửi: QTA_ChungTuChiTiet
        //            DataTable dt = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTietQuyetToan_TheoMaQuyetToan(arrMaChungTuChiTietQuyetToan[i]);
        //            DataRow R = dt.Rows[0];

        //            //Insert into data vào bảng: KT_ChungTuChiTiet
        //            Bang bang = new Bang("KT_ChungTuChiTiet");
        //            bang.DuLieuMoi = true;
        //            bang.MaNguoiDungSua = User.Identity.Name;
        //            bang.IPSua = Request.UserHostAddress;
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_QuyetToan", arrMaChungTuChiTietQuyetToan[i]);
        //            bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
        //            bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "QT-" + R["sTienToChungTu"] + R["iSoChungTu"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sLNS"] + "-" + R["sMoTa"]);
        //            bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rTongSo"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R["iID_MaTaiKhoan_No"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R["sTenTaiKhoan_No"]);
        //            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R["iID_MaTaiKhoan_Co"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R["sTenTaiKhoan_Co"]);
        //            //bang.CmdParams.Parameters.AddWithValue("@sKyHieuPhongBan_No", R["sKyHieuPhongBan_Tra"]);
        //            //bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan_No", R["sTenPhongBan_Tra"]);
        //            //bang.CmdParams.Parameters.AddWithValue("@sKyHieuPhongBan_Co", R["sKyHieuPhongBan_Tra"]);
        //            //bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan_Co", R["sTenPhongBan_Tra"]);
        //            bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

        //            bang.Save();
        //            dt.Dispose();
        //        }
        //    }

        //    String strJ = "";
        //    if (String.IsNullOrEmpty(OnSuccess) == false)
        //    {
        //        strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
        //    }
        //    else
        //    {
        //        strJ = String.Format("Dialog_close('{0}');", ParentID);
        //    }
        //    return JavaScript(strJ);
        //}
        //[Authorize]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult SearchSubmit(String ParentID)
        //{
        //    String iNam = Request.Form[ParentID + "_iNam"];

        //    return RedirectToAction("Index", new { iNam = iNam });
        //}

        //[Authorize]
        //public JsonResult get_ThongTinChungTuMoi(String iNamLamViec)
        //{
        //    int iSoChungTu_GoiY = KeToanTongHop_ChungTuModels.GetMaxChungTu_CuoiCung(iNamLamViec);
        //    string sSoChungTu = "";

        //    Boolean ok = false;
        //    int d = 0;
        //    do
        //    {
        //        iSoChungTu_GoiY++;
        //        ok = KeToanTongHop_ChungTuModels.KiemTra_sSoChungTu(Convert.ToString(iSoChungTu_GoiY), iNamLamViec);
        //        d++;
        //    } while (ok==false && d<10);
        //    if (ok)
        //    {
        //        sSoChungTu = Convert.ToString(iSoChungTu_GoiY);
        //    }
        //    Object item = new
        //    {
        //        iID_MaChungTu = Globals.getNewGuid(),
        //        iNgay = DateTime.Now.Day,
        //        sSoChungTu = sSoChungTu
        //    };
        //    return Json(item, JsonRequestBehavior.AllowGet);
        //}

        //[Authorize]
        //public ActionResult ChungTuChiTietTuChoi()
        //{
        //    return View(sViewPath + "KeToanTongHop_ChungTuChiTietTuChoi.aspx");
        //}

        //[Authorize]
        //public ActionResult ChungTuChiTietTuChoi_Frame()
        //{
        //    return View(sViewPath + "KeToanTongHop_ChungTuChiTietTuChoi_Frame.aspx");
        //}
    }
}
