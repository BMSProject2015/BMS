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
    public class KeToanTongHop_ChungTuChiTietController : Controller
    {
        //
        // GET: /KeToanTongHop_ChungTuChiTiet/
        public string sViewPath = "~/Views/KeToanTongHop/ChungTuChiTiet/";

        [Authorize]
        public ActionResult ChungTu_Frame()
        {
            return View(sViewPath + "KeToanTongHop_ChungTu_Frame.aspx");
        }

        [Authorize]
        public ActionResult ChungTuChiTiet_Frame()
        {
            return View(sViewPath + "KeToanTongHop_ChungTuChiTiet_Frame.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaChungTu)
        {
            iID_MaChungTu = Request.Form["iID_MaChungTu"];
            String MaND = User.Identity.Name;
            DataTable dtChungTu = KeToanTongHop_ChungTuModels.GetChungTu(iID_MaChungTu);
            if (dtChungTu.Rows.Count == 0)
            {
                dtChungTu.Dispose();

                if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND) ||
                    LuongCongViecModel.KiemTra_TroLyTongHop(MaND))
                {
                    //Trợ lý phòng ban và trợ lý tổng hợp được quyền thêm chứng từ
                }
                else
                {
                    //Không có quyền thêm chứng từ
                    return RedirectToAction("Index", "PermitionMessage");
                }
                KeToanTongHop_ChungTuModels.InsertRecord(iID_MaChungTu, Request.Form, MaND, Request.UserHostAddress);
                dtChungTu = KeToanTongHop_ChungTuModels.GetChungTu(iID_MaChungTu);
            }
            if (dtChungTu.Rows.Count == 1)
            {
                String TenBangChiTiet = "KT_ChungTuChiTiet";

                string idXauMaCacHang = Request.Form["idXauMaCacHang"];
                string idXauMaCacCot = Request.Form["idXauMaCacCot"];
                string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
                string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
                string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
                String[] arrMaHang = idXauMaCacHang.Split(',');
                String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
                String[] arrMaCot = idXauMaCacCot.Split(',');
                String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

                String iID_MaChungTuChiTiet;
                //Luu cac hang sua
                String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
                for (int i = 0; i < arrMaHang.Length; i++)
                {
                    iID_MaChungTuChiTiet = arrMaHang[i];
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (iID_MaChungTuChiTiet != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang(TenBangChiTiet);
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                            bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.Save();
                        }
                    }
                    else
                    {
                        String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                        String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                  
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
                            iID_MaChungTuChiTiet = arrMaHang[i];
                            if (iID_MaChungTuChiTiet == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                                bang.CmdParams.Parameters.AddWithValue("@iNgayCT", dtChungTu.Rows[0]["iNgay"]);
                                bang.CmdParams.Parameters.AddWithValue("@iThangCT", dtChungTu.Rows[0]["iThang"]);
                                bang.CmdParams.Parameters.AddWithValue("@sSoChungTuGhiSo", dtChungTu.Rows[0]["sSoChungTu"]);
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaChungTuChiTiet;
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
                                    else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                                    {
                                        //Nhap Kieu so
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        {
                                            bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
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
                            if (KeToanTongHop_ChungTuChiTietModels.KiemTraCoDuocCapNhapBang(bang))
                            {
                                bang.Save();
                            }
                        }
                    }
                }
                string SQL = "UPDATE KT_ChungTu SET rTongSo=(SELECT SUM(rSoTien) FROM KT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu) WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                Connection.UpdateDatabase(cmd);
                cmd.Parameters.Clear();
                cmd.Dispose();
                /////cap nhat lai ct
                cmd = new SqlCommand("select COUNT(*)  FROM KT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                Double rTongSo = Convert.ToDouble(Connection.GetValue(cmd, 0));
                cmd.Parameters.Clear();
                cmd.Dispose();
                if (rTongSo == 0)
                {
                    string mySQL = @"DELETE KT_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu;
                    DELETE KT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu;";
                    cmd = new SqlCommand(mySQL);
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                }
//                SqlCommand cmd = new SqlCommand("SELECT SUM(rSoTien) FROM KT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu");
//                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
//                Double rTongSo = Convert.ToDouble(Connection.GetValue(cmd, 0));
//                cmd.Parameters.Clear();
//                cmd.Dispose();              
//                if (rTongSo == 0)
//                {
//                    string mySQL = @"DELETE KT_ChungTu WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu;
//                    DELETE KT_ChungTuChiTiet WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu;";
//                    cmd = new SqlCommand(mySQL);
//                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
//                    Connection.UpdateDatabase(cmd);
//                    cmd.Dispose();
//                }
//                else
//                {
//                    Bang bangChungTu = new Bang("KT_ChungTu");
//                    bangChungTu.DuLieuMoi = false;
//                    bangChungTu.GiaTriKhoa = iID_MaChungTu;
//                    bangChungTu.CmdParams.Parameters.AddWithValue("@rTongSo", rTongSo);
//                    bangChungTu.MaNguoiDungSua = User.Identity.Name;
//                    bangChungTu.IPSua = Request.UserHostAddress;
//                    bangChungTu.Save();
//                }
            }
            else
            {
                ViewData["KhongThemDuoc"] = "1";
            }
            dtChungTu.Dispose();
            ViewData["Saved"] = "1";
            return View(sViewPath + "KeToanTongHop_ChungTuChiTiet_Frame.aspx");
        }

        /// <summary>
        /// Thêm dữ liệu từ bảng KTTG_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="OnSuccess"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaDiv"></param>
        /// <returns></returns>
        public JavaScriptResult Edit_Fast_TienGui_Submit(String ParentID, String OnSuccess, String iID_MaChungTu, String iThang, String iNam, String MaND, String IPSua)
        {
            NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

            String iNgay = Request.Form[ParentID + "_iNgay"];
            String strMaChungTuChiTietTienGui = Request.Form[ParentID + "_txt"];
            String sDonVi = Request.Form[ParentID + "_sDonVi"];
            String sSoChungTu = Request.Form[ParentID + "_sSoChungTu"];
           // return JavaScript(String.Format("Dialog_close('{0}');", ParentID));

            if (String.IsNullOrEmpty(strMaChungTuChiTietTienGui) || String.IsNullOrEmpty(sSoChungTu))
                return JavaScript(String.Format("Dialog_close('{0}');", ParentID));
            else
            {
                Boolean KiemTra_sSoChungTu = KeToanTongHop_ChungTuModels.KiemTra_sSoChungTu(sSoChungTu, iNam);

                String[] arrMaChungTuChiTietTienGui = strMaChungTuChiTietTienGui.Split(',');
                int i;
                for (i = 0; i < arrMaChungTuChiTietTienGui.Length; i++)
                {
                    if (arrMaChungTuChiTietTienGui[i] != "" && String.IsNullOrEmpty(arrMaChungTuChiTietTienGui[i]) == false)
                    {
                        //Lấy chi tiết bảng chi tiết của phần tiền gửi: KTTG_ChungTuChiTiet
                        int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, iThang, iNam, arrMaChungTuChiTietTienGui[i], 2);
                        if (intDaCo == 0)
                        {
                            DataTable dt = KTCT_TienGui_ChungTuChiTietModels.Get_dtChungTuChiTiet_Row(arrMaChungTuChiTietTienGui[i]);
                            DataRow R = dt.Rows[0];

                            //Insert into data vào bảng: KT_ChungTuChiTiet
                            Bang bang = new Bang("KT_ChungTuChiTiet");
                            bang.DuLieuMoi = true;
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
                            bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienGui", arrMaChungTuChiTietTienGui[i]);
                            bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
                            bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
                            bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "UNC-" + R["sSoChungTuChiTiet"]);
                            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sNoiDung"]);
                            bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rSoTien"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R["iID_MaTaiKhoan_No"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R["sTenTaiKhoan_No"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R["iID_MaTaiKhoan_Co"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R["sTenTaiKhoan_Co"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R["iID_MaDonVi_No"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R["sTenDonVi_No"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R["iID_MaDonVi_Co"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R["sTenDonVi_Co"]);
                            bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

                            bang.Save();
                            dt.Dispose();
                        }
                    }
                }

                String strJ = "";
                if (String.IsNullOrEmpty(OnSuccess) == false)
                {
                    strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
                }
                else
                {
                    strJ = String.Format("Dialog_close('{0}');", ParentID);
                }
                return JavaScript(strJ);
            }
        }

        /// <summary>
        /// Thêm dữ liệu từ bảng KTTM_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="OnSuccess"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public JavaScriptResult Edit_Fast_TienMat_Submit(String ParentID, String OnSuccess, String iID_MaChungTu, String iThang, String iNam)
        {
            NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

            String iNgay = Request.Form[ParentID + "_iNgay"];
            String strMaChungTuChiTietTienMat = Request.Form[ParentID + "_txt"];
            if (String.IsNullOrEmpty(strMaChungTuChiTietTienMat))
                return JavaScript(String.Format("Dialog_close('{0}');", ParentID));
            else
            {
                String[] arrMaChungTuChiTietTienMat = strMaChungTuChiTietTienMat.Split(',');

                int i;
                for (i = 0; i < arrMaChungTuChiTietTienMat.Length; i++)
                {
                    
                    if (arrMaChungTuChiTietTienMat[i] != "" && String.IsNullOrEmpty(arrMaChungTuChiTietTienMat[i]) == false)
                    {
                        int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, iThang, iNam, arrMaChungTuChiTietTienMat[i], 3);
                        if (intDaCo == 0)
                        {
                            //Lấy chi tiết bảng chi tiết của phần tiền gửi: KTTM_ChungTuChiTiet
                            DataTable dt = KTCT_TienMat_ChungTuChiTietModels.Get_dtChungTuChiTiet_Row(arrMaChungTuChiTietTienMat[i]);
                            DataRow R = dt.Rows[0];

                            //Insert into data vào bảng: KT_ChungTuChiTiet
                            Bang bang = new Bang("KT_ChungTuChiTiet");
                            bang.DuLieuMoi = true;
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
                            bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_TienMat", arrMaChungTuChiTietTienMat[i]);
                            bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
                            bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
                            bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "PTPC-" + R["sSoChungTuChiTiet"]);
                            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sNoiDung"]);
                            bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rSoTien"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R["iID_MaTaiKhoan_No"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R["sTenTaiKhoan_No"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R["iID_MaTaiKhoan_Co"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R["sTenTaiKhoan_Co"]);

                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R["iID_MaDonVi_No"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R["sTenDonVi_No"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R["iID_MaDonVi_Co"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R["sTenDonVi_Co"]);

                            bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

                            bang.Save();
                            dt.Dispose();
                        }
                    }
                }

                String strJ = "";
                if (String.IsNullOrEmpty(OnSuccess) == false)
                {
                    strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
                }
                else
                {
                    strJ = String.Format("Dialog_close('{0}');", ParentID);
                }
                return JavaScript(strJ);
            }
        }
        /// <summary>
        /// Thêm dữ liệu từ bảng KTKB_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="OnSuccess"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public JavaScriptResult Edit_Fast_KhoBac_Submit(String ParentID, String OnSuccess, String iID_MaChungTu, String iThang, String iNam)
        {
            NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

            String iNgay = Request.Form[ParentID + "_iNgay"];
            String strMaChungTuChiTietTienMat = Request.Form[ParentID + "_txt"];
            if (String.IsNullOrEmpty(strMaChungTuChiTietTienMat))
                return JavaScript(String.Format("Dialog_close('{0}');", ParentID));
            else
            {
                String[] arrMaChungTuChiTietTienMat = strMaChungTuChiTietTienMat.Split(',');
                String optNoCo = Convert.ToString(Request.Form[ParentID + "_optNoCo"]);
                String sTaiKhoan = Convert.ToString(Request.Form[ParentID + "_sTaiKHoan"]);
                String sTenTaiKhoan = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(sTaiKhoan);

                int i;
                for (i = 0; i < arrMaChungTuChiTietTienMat.Length; i++)
                {
                    if (arrMaChungTuChiTietTienMat[i] != "" && String.IsNullOrEmpty(arrMaChungTuChiTietTienMat[i]) == false)
                    {
                        int intDaCo = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTiet_TheoThang(iID_MaChungTu, iThang, iNam, arrMaChungTuChiTietTienMat[i], 1);
                        if (intDaCo == 0)
                        {
                            //Lấy chi tiết bảng chi tiết của phần tiền gửi: KTTM_ChungTuChiTiet
                            DataTable dt = KTCT_KhoBac_ChungTuChiTietModels.Get_dtChungTuChiTiet_Row(arrMaChungTuChiTietTienMat[i]);
                            DataRow R = dt.Rows[0];

                            //Insert into data vào bảng: KT_ChungTuChiTiet
                            Bang bang = new Bang("KT_ChungTuChiTiet");
                            bang.DuLieuMoi = true;
                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
                            bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_KhoBac", arrMaChungTuChiTietTienMat[i]);
                            bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
                            bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
                            bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "RDT-" + R["sSoChungTuChiTiet"]);
                            bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sNoiDung"]);
                            bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rDTRut"]);
                            switch (optNoCo)
                            {
                                case "tkNo":
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", sTaiKhoan);
                                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", sTenTaiKhoan);
                                    break;
                                case "tkCo":
                                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", sTaiKhoan);
                                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", sTenTaiKhoan);
                                    break;
                                case "tkTrong":
                                    break;
                            }
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_No", R["iID_MaDonVi_Nhan"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_No", R["sTenDonVi_Nhan"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi_Co", R["iID_MaDonVi_Tra"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTenDonVi_Co", R["sTenDonVi_Tra"]);
                            bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

                            bang.Save();
                            dt.Dispose();
                        }
                    }
                }

                String strJ = "";
                if (String.IsNullOrEmpty(OnSuccess) == false)
                {
                    strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
                }
                else
                {
                    strJ = String.Format("Dialog_close('{0}');", ParentID);
                }
                return JavaScript(strJ);
            }
        }
        /// <summary>
        /// Thêm dữ liệu từ bảng QTA_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="OnSuccess"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public JavaScriptResult Edit_Fast_QuyetToan_Submit(String ParentID, String OnSuccess, String iID_MaChungTu)
        {
            NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

            String iNgay = Request.Form[ParentID + "_iNgay"];
            String strMaChungTuChiTietQuyetToan = Request.Form[ParentID + "_txt"];
            String[] arrMaChungTuChiTietQuyetToan = strMaChungTuChiTietQuyetToan.Split(',');

            int i;
            for (i = 0; i < arrMaChungTuChiTietQuyetToan.Length; i++)
            {
                if (arrMaChungTuChiTietQuyetToan[i] != "")
                {
                    //Lấy chi tiết bảng chi tiết của phần tiền gửi: QTA_ChungTuChiTiet
                    DataTable dt = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTietQuyetToan_TheoMaQuyetToan(arrMaChungTuChiTietQuyetToan[i]);
                    DataRow R = dt.Rows[0];

                    //Insert into data vào bảng: KT_ChungTuChiTiet
                    Bang bang = new Bang("KT_ChungTuChiTiet");
                    bang.DuLieuMoi = true;
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
                    bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_QuyetToan", arrMaChungTuChiTietQuyetToan[i]);
                    bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
                    bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
                    bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "QT-" + R["sTienToChungTu"] + R["iSoChungTu"]);
                    bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sLNS"] + "-" + R["sMoTa"]);
                    bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rTongSo"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R["iID_MaTaiKhoan_No"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R["sTenTaiKhoan_No"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R["iID_MaTaiKhoan_Co"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R["sTenTaiKhoan_Co"]);
                    //bang.CmdParams.Parameters.AddWithValue("@sKyHieuPhongBan_No", R["sKyHieuPhongBan_Tra"]);
                    //bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan_No", R["sTenPhongBan_Tra"]);
                    //bang.CmdParams.Parameters.AddWithValue("@sKyHieuPhongBan_Co", R["sKyHieuPhongBan_Tra"]);
                    //bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan_Co", R["sTenPhongBan_Tra"]);
                    bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

                    bang.Save();
                    dt.Dispose();
                }
            }

            String strJ = "";
            if (String.IsNullOrEmpty(OnSuccess) == false)
            {
                strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
            }
            else
            {
                strJ = String.Format("Dialog_close('{0}');", ParentID);
            }
            return JavaScript(strJ);
        }
        /// <summary>
        /// Thêm dữ liệu từ bảng KTCS_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="OnSuccess"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public JavaScriptResult Edit_Fast_CongSan_Submit(String ParentID, String OnSuccess, String iID_MaChungTu)
        {
            NameValueCollection data = KeToanTongHop_ChungTuModels.LayThongTin(iID_MaChungTu);

            String iNgay = Request.Form[ParentID + "_iNgay"];
            String strMaChungTuChiTietQuyetToan = Request.Form[ParentID + "_txt"];
            String[] arrMaChungTuChiTietQuyetToan = strMaChungTuChiTietQuyetToan.Split(',');

            int i;
            for (i = 0; i < arrMaChungTuChiTietQuyetToan.Length; i++)
            {
                if (arrMaChungTuChiTietQuyetToan[i] != "")
                {
                    //Lấy chi tiết bảng chi tiết của phần tiền gửi: QTA_ChungTuChiTiet
                    DataTable dt = KeToanTongHop_ChungTuChiTietModels.Lay_DanhSachChungTuChiTietQuyetToan_TheoMaQuyetToan(arrMaChungTuChiTietQuyetToan[i]);
                    DataRow R = dt.Rows[0];

                    //Insert into data vào bảng: KT_ChungTuChiTiet
                    Bang bang = new Bang("KT_ChungTuChiTiet");
                    bang.DuLieuMoi = true;
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", data["iID_MaPhongBan"]);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", data["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iNgayCT", data["iNgay"]);
                    bang.CmdParams.Parameters.AddWithValue("@iThangCT", data["iThang"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTuChiTiet_QuyetToan", arrMaChungTuChiTietQuyetToan[i]);
                    bang.CmdParams.Parameters.AddWithValue("@iNgay", iNgay);
                    bang.CmdParams.Parameters.AddWithValue("@iThang", data["iThang"]);
                    bang.CmdParams.Parameters.AddWithValue("@sSoChungTuChiTiet", "QT-" + R["sTienToChungTu"] + R["iSoChungTu"]);
                    bang.CmdParams.Parameters.AddWithValue("@sNoiDung", R["sLNS"] + "-" + R["sMoTa"]);
                    bang.CmdParams.Parameters.AddWithValue("@rSoTien", R["rTongSo"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_No", R["iID_MaTaiKhoan_No"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_No", R["sTenTaiKhoan_No"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan_Co", R["iID_MaTaiKhoan_Co"]);
                    bang.CmdParams.Parameters.AddWithValue("@sTenTaiKhoan_Co", R["sTenTaiKhoan_Co"]);
                    //bang.CmdParams.Parameters.AddWithValue("@sKyHieuPhongBan_No", R["sKyHieuPhongBan_Tra"]);
                    //bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan_No", R["sTenPhongBan_Tra"]);
                    //bang.CmdParams.Parameters.AddWithValue("@sKyHieuPhongBan_Co", R["sKyHieuPhongBan_Tra"]);
                    //bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan_Co", R["sTenPhongBan_Tra"]);
                    bang.CmdParams.Parameters.AddWithValue("@sGhiChu", R["sGhiChu"]);

                    bang.Save();
                    dt.Dispose();
                }
            }

            String strJ = "";
            if (String.IsNullOrEmpty(OnSuccess) == false)
            {
                strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
            }
            else
            {
                strJ = String.Format("Dialog_close('{0}');", ParentID);
            }
            return JavaScript(strJ);
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iID_MaChungTu)
        {
            String sLNS = Request.Form["Search_sLNS"];
            String sL = Request.Form["Search_sL"];
            String sK = Request.Form["Search_sK"];
            String sM = Request.Form["Search_sM"];
            String sTM = Request.Form["Search_sTM"];
            String sTTM = Request.Form["Search_sTTM"];
            String sNG = Request.Form["Search_sNG"];
            String sTNG = Request.Form["Search_sTNG"];

            return RedirectToAction("Index", new { iID_MaChungTu = iID_MaChungTu, sLNS = sLNS, sL = sL, sK = sK, sM = sM, sTM = sTM, sTTM = sTTM, sNG = sNG, sTNG = sTNG });
        }

        [Authorize]
        public JsonResult get_ThongTinChungTuMoi(String iNamLamViec)
        {
            int iSoChungTu_GoiY = KeToanTongHop_ChungTuModels.GetMaxChungTu_CuoiCung(iNamLamViec);
            string sSoChungTu = "";

            Boolean ok = false;
            int d = 0;
            do
            {
                iSoChungTu_GoiY++;
                ok = KeToanTongHop_ChungTuModels.KiemTra_sSoChungTu(Convert.ToString(iSoChungTu_GoiY), iNamLamViec);
                d++;
            } while (ok==false && d<10);
            if (ok)
            {
                sSoChungTu = Convert.ToString(iSoChungTu_GoiY);
            }
            Object item = new
            {
                iID_MaChungTu = Globals.getNewGuid(),
                iNgay = DateTime.Now.Day,
                sSoChungTu = sSoChungTu
            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ChungTuChiTietTuChoi()
        {
            return View(sViewPath + "KeToanTongHop_ChungTuChiTietTuChoi.aspx");
        }

        [Authorize]
        public ActionResult ChungTuChiTietTuChoi_Frame()
        {
            return View(sViewPath + "KeToanTongHop_ChungTuChiTietTuChoi_Frame.aspx");
        }
        [Authorize]
        public ActionResult Show_Dialog_Tao_CTGS()
        {
            return View(sViewPath + "KeToanTongHop_ChungTu_Dialog.aspx");
        }
        [Authorize]
        public ActionResult Show_Dialog_F12()
        {
            return View(sViewPath + "KeToanTongHop_TinhTong_Dialog.aspx");
        }
        public JsonResult get_SoChungTuDuyet(String idSoChungTu, String iNamLamViec, String idSoChungTuCu)
        {
            return Json(get_objSoChungTuDuyet(idSoChungTu, iNamLamViec, idSoChungTuCu), JsonRequestBehavior.AllowGet);
        }
        public static String get_objSoChungTuDuyet(String idSoChungTu, String iNamLamViec, String idSoChungTuCu)
        {
            String strMess = "";
            if (!String.IsNullOrEmpty(idSoChungTu) && !String.IsNullOrEmpty(idSoChungTuCu) && idSoChungTu != idSoChungTuCu)
            {
                DataTable dt = KeToanTongHop_ChungTuModels.Get_So_ChungTu(idSoChungTu, iNamLamViec, idSoChungTuCu);
                if (dt != null && dt.Rows.Count > 0)
                {
                    strMess = "Số CTGS đã tồn tại - Toàn bộ số C.T chi tiết sẽ lưu vào số CTGS này!";
                }
                else
                {
                    strMess = "";
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
            return strMess;
        }
    }
}
