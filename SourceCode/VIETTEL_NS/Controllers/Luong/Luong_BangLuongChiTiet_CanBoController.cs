using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;
using System.Text.RegularExpressions;
namespace VIETTEL.Controllers.Luong
{
    public class Luong_BangLuongChiTiet_CanBoController : Controller
    {
        //
        // GET: /Luong_BangLuongChiTiet_PhuCap/
        public string sViewPath = "~/Views/Luong/BangLuongChiTiet/";

        [Authorize]
        public ActionResult Detail(string iID_MaBangLuongChiTiet)
        {
            return View(sViewPath + "Luong_BangLuongChiTiet_CanBo_Index.aspx");
        }

        /// <summary>
        /// ghép ngày tháng năm dạng MM/yy thành yyyy/mm/dd
        /// </summary>
        /// <param name="strNgay"></param>
        /// <returns></returns>
        public string GhepNgayThangNam(String strNgay)
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

        #region "Hàm cho Autocomplete"
        public JsonResult get_DanhSachDonViTheoBangLuong(String iID_MaBangLuong ,String GiaTri)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT sDanhSachMaDonVi FROM L_BangLuong WHERE iTrangThai=1 AND iID_MaBangLuong=@iID_MaBangLuong";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            String dsMaDonVi = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            String[] arrMaDonVi = dsMaDonVi.Split(',');
            cmd = new SqlCommand();
            String DK = "";
            int i;
            for (i = 0; i < arrMaDonVi.Length; i++)
            {
                if (arrMaDonVi[i] != "")
                {
                    if (DK != "") DK += " OR ";
                    DK += "iID_MaDonVi=@iID_MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrMaDonVi[i]);
                }
            }
            DK += " AND iNamLamViec_DonVi=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            SQL = String.Format("SELECT iID_MaDonVi, sTen FROM NS_DonVi WHERE iTrangThai=1 AND ({0}) AND (iID_MaDonVi + ' - ' + sTen) LIKE @iID_MaDonVi ORDER BY iID_MaDonVi", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDonVi", GiaTri + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                SQL = String.Format("SELECT iID_MaDonVi, sTen FROM NS_DonVi WHERE iTrangThai=1 AND ({0}) ORDER BY iID_MaDonVi", DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            
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

        public JsonResult get_GiaTriDonViTheoBangLuong(String iID_MaBangLuong, String GiaTri)
        {
            String[] arrTuKhoa = GiaTri.Split('-');
            String TuKhoa = arrTuKhoa[0].Trim();

            String SQL = "SELECT sDanhSachMaDonVi FROM L_BangLuong WHERE iTrangThai=1 AND iID_MaBangLuong=@iID_MaBangLuong";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            String dsMaDonVi = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            String[] arrMaDonVi = dsMaDonVi.Split(',');
            cmd = new SqlCommand();
            String DK = "";
            int i;
            for (i = 0; i < arrMaDonVi.Length; i++)
            {
                if (arrMaDonVi[i] != "")
                {
                    if (DK != "") DK += " OR ";
                    DK += "iID_MaDonVi=@iID_MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrMaDonVi[i]);
                }
            }
            DK += " AND iNamLamViec_DonVi=@iNamLamViec";
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt;
            if (TuKhoa == "")
            {
                SQL = String.Format("SELECT TOP 1 iID_MaDonVi, sTen FROM NS_DonVi WHERE iTrangThai=1 AND ({0}) ORDER BY iID_MaDonVi", DK);
                cmd.CommandText = SQL;
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            else
            {
                SQL = String.Format("SELECT TOP 1 iID_MaDonVi, sTen FROM NS_DonVi WHERE iTrangThai=1 AND ({0}) AND (iID_MaDonVi + ' - ' + sTen) LIKE @iID_MaDonVi ORDER BY iID_MaDonVi", DK);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaDonVi", TuKhoa + "%");
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }

            Object item = new
            {
                value = "",
                label = ""
            };
            if (0 < dt.Rows.Count)
            {
                item = new
                {
                    value = String.Format("{0}", dt.Rows[0]["iID_MaDonVi"]),
                    label = String.Format("{0} - {1}", dt.Rows[0]["iID_MaDonVi"], dt.Rows[0]["sTen"])
                };
            }
            dt.Dispose();

            return Json(item, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "Thêm sửa cán bộ"
        [Authorize]
        public ActionResult ThemCanBo()
        {
            return View(sViewPath + "Dialogs/Luong_ThemSuaCanBo_Dialog.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ThemCanBo_Submit(String ParentID, String iID_MaBangLuong)
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = null;
            //Thêm mới cán bộ
            String iID_MaCanBo = CanBo_HoSoNhanSuModels.ThemMoiCanBo_TuBangLuong(ParentID, Request.Form, ref arrLoi, MaND, IPSua);
            if (iID_MaCanBo != "")
            {
                String iID_MaBangLuongChiTiet = BangLuongChiTiet_CaNhanModels.ThemLuongChiTiet_CanBo(iID_MaBangLuong, iID_MaCanBo, User.Identity.Name, Request.UserHostAddress, Request.Form);

                //Cập nhập bảng phụ cấp
                NameValueCollection data = LuongModels.LayThongTinBangLuongChiTiet(iID_MaBangLuongChiTiet);
                // String iID_MaBangLuong = data["iID_MaBangLuong"];
                String TenBangChiTiet = "L_BangLuongChiTiet_PhuCap";

                string idXauMaCacHang = Request.Form["idXauMaCacHang"];
                string idXauMaCacCot = Request.Form["idXauMaCacCot"];
                string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
                string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
                string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];

                String[] arrMaHang = idXauMaCacHang.Split(',');
                String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
                String[] arrMaCot = idXauMaCacCot.Split(',');
                String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

                String iID_MaBangLuongChiTiet_PhuCap;

                //Luu cac hang sua

                if (String.IsNullOrEmpty(idXauDuLieuThayDoi) == false)
                {
                    String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
                    for (int i = 0; i < arrMaHang.Length; i++)
                    {
                        iID_MaBangLuongChiTiet_PhuCap = arrMaHang[i];
                        if (arrHangDaXoa[i] == "1")
                        {
                            //Lưu các hàng đã xóa
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
                                Bang bang_PhuCap = new Bang(TenBangChiTiet);
                                
                                //Du Lieu Moi
                                bang_PhuCap.DuLieuMoi = true;
                                bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                                bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
                                bang_PhuCap.CmdParams.Parameters.AddWithValue("@bLuonCo", false);

                                //Them cac tham so tu bang L_BangLuongChiTiet
                                bang_PhuCap.CmdParams.Parameters.AddWithValue("@iNamBangLuong", data["iNamBangLuong"]);
                                bang_PhuCap.CmdParams.Parameters.AddWithValue("@iThangBangLuong", data["iThangBangLuong"]);
                                bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", data["iID_MaDonVi"]);
                                bang_PhuCap.MaNguoiDungSua = User.Identity.Name;
                                bang_PhuCap.IPSua = Request.UserHostAddress;

                                //Them tham so
                                for (int j = 0; j < arrMaCot.Length; j++)
                                {
                                    if (arrThayDoi[j] == "1")
                                    {
                                        if (arrMaCot[j].EndsWith("_ConLai") == false)
                                        {
                                            String Truong = "@" + arrMaCot[j];
                                            if (arrMaCot[j].StartsWith("b"))
                                            {
                                                //Nhap Kieu checkbox
                                                if (arrGiaTri[j] == "1")
                                                {
                                                    bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, true);
                                                }
                                                else
                                                {
                                                    bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, false);
                                                }
                                            }
                                            else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                                            {
                                                //Nhap Kieu so
                                                if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                                {
                                                    bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                                }
                                                else
                                                {
                                                    bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, 0);
                                                }
                                            }
                                            else
                                            {
                                                //Nhap kieu xau
                                                bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                            }
                                        }
                                    }
                                }
                                bang_PhuCap.Save();
                            }
                        }
                    }
                }

                //Tính lại tất cả các công thức của bảng lương chi tiết
                BangLuongChiTiet_CaNhanModels.CapNhapBangLuongChiTiet(iID_MaBangLuongChiTiet, User.Identity.Name, Request.UserHostAddress, null, null);
                string idAction = Request.Form["idAction"];
                if (idAction == "3")
                {
                    String iID_MaBangLuongChiTiet_Tiep = BangLuongChiTietModels.Lay_iID_MaBangLuongChiTiet_Tiep(iID_MaBangLuong, iID_MaBangLuongChiTiet);
                    if (iID_MaBangLuongChiTiet_Tiep != "")
                    {
                        return RedirectToAction("ThemCanBo", new { iID_MaBangLuong = iID_MaBangLuong, iID_MaBangLuongChiTiet = iID_MaBangLuongChiTiet_Tiep });
                    }
                }
                NameValueCollection dataView = new NameValueCollection();
                dataView.Add(ParentID + "_iID_MaDonVi", Request.Form[ParentID + "_iID_MaDonVi"]);
                dataView.Add(ParentID + "_sTenDonVi", Request.Form[ParentID + "_sTenDonVi"]);
                dataView.Add(ParentID + "_iID_MaNgachLuong_CanBo", Request.Form[ParentID + "_iID_MaNgachLuong_CanBo"]);
                dataView.Add(ParentID + "_iID_MaBacLuong_CanBo", Request.Form[ParentID + "_iID_MaBacLuong_CanBo"]);
                dataView.Add(ParentID + "_iSoNguoiPhuThuoc_CanBo", Request.Form[ParentID + "_iSoNguoiPhuThuoc_CanBo"]);
                dataView.Add(ParentID + "_sHieuTangGiam", Request.Form[ParentID + "_sHieuTangGiam"]);
                dataView.Add(ParentID + "_sKyHieu_MucLucQuanSo_HieuTangGiam", Request.Form[ParentID + "_iID_MaLyDoTangGiam"]);
                dataView.Add(ParentID + "_rLuongCoBan_HeSo_CanBo", Request.Form[ParentID + "_rLuongCoBan_HeSo_CanBo"]);
                dataView.Add(ParentID + "_rLuongCoBan", Convert.ToInt32(Request.Form[ParentID + "_rLuongCoBan"]).ToString());
                
                
                ViewData["data"] = dataView;
                ViewData["PhaiLoadLai"] = "1";

                return View(sViewPath + "Dialogs/Luong_ThemSuaCanBo_Dialog.aspx");
            }
            else
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                NameValueCollection data = new NameValueCollection();
                data.Add(Request.Form);
                ViewData["data"] = data;
                return View(sViewPath + "Dialogs/Luong_ThemSuaCanBo_Dialog.aspx");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SuaCanBo_Submit(String iID_MaBangLuongChiTiet, String ParentID)
        {
            Boolean CoCapNhapDuLieu = true;
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;

            DataTable dtChiTiet = LuongModels.Get_ChiTietBangLuongChiTiet(iID_MaBangLuongChiTiet);
            DataTable dtPhuCap = BangLuongChiTietModels.Get_DataTable_PhuCap("", "", "", iID_MaBangLuongChiTiet);

            int iNamBangLuong = Convert.ToInt32(dtChiTiet.Rows[0]["iNamBangLuong"]);
            int iThangBangLuong = Convert.ToInt32(dtChiTiet.Rows[0]["iThangBangLuong"]);
            string iID_MaCanBo = Convert.ToString(dtChiTiet.Rows[0]["iID_MaCanBo"]);

            //<-- Cập nhập bảng lương chi tiết
            Bang bangChiTiet = new Bang("L_BangLuongChiTiet");
            bangChiTiet.MaNguoiDungSua = User.Identity.Name;
            bangChiTiet.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bangChiTiet.TruyenGiaTri(ParentID, Request.Form);
            bangChiTiet.GiaTriKhoa = iID_MaBangLuongChiTiet;
            bangChiTiet.DuLieuMoi = false;
            BangLuongChiTietModels.DieuChinhLaiBangTruocKhiGhi(bangChiTiet, iNamBangLuong, iThangBangLuong);

            if (CanBo_HoSoNhanSuModels.KiemTraDuocPhepSuaNhanhCanBo(ParentID, iID_MaCanBo, Request.Form, ref arrLoi)==false)
            {
                //Nếu tham số sửa sai thì sẽ báo lỗi
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                NameValueCollection dataLoi = new NameValueCollection();
                dataLoi.Add(Request.Form);
                ViewData["data"] = dataLoi;
                return View(sViewPath + "Dialogs/Luong_ThemSuaCanBo_Dialog.aspx");
            }

            //<--Điền thông tin từ bảng Bậc lương
            String iID_MaNgachLuong = Convert.ToString(bangChiTiet.CmdParams.Parameters["@iID_MaNgachLuong_CanBo"].Value);
            String iID_MaBacLuong = Convert.ToString(bangChiTiet.CmdParams.Parameters["@iID_MaBacLuong_CanBo"].Value);
            DataTable dtBacLuong = LuongModels.Get_ChiTietDanhMucBacLuong(iID_MaNgachLuong, iID_MaBacLuong);

            Double rHeSoLuong = Convert.ToDouble(dtBacLuong.Rows[0]["rHeSoLuong"]);
            Double rHeSo_ANQP = Convert.ToDouble(dtBacLuong.Rows[0]["rHeSo_ANQP"]);
            Object rLuongCoBan_HeSo_CanBo_Cu = 0;
            if (rHeSoLuong > 0)
            {
                rLuongCoBan_HeSo_CanBo_Cu = CommonFunction.ThemGiaTriVaoThamSo(bangChiTiet.CmdParams.Parameters, "@rLuongCoBan_HeSo_CanBo", rHeSoLuong);
            }
            Object rPhuCap_AnNinhQuocPhong_HeSo_Cu = CommonFunction.ThemGiaTriVaoThamSo(bangChiTiet.CmdParams.Parameters, "@rPhuCap_AnNinhQuocPhong_HeSo", rHeSo_ANQP);
            dtBacLuong.Dispose();
            //-->Điền thông tin từ bảng Bậc lương

            bangChiTiet.Save();
            // END: Cập nhập bảng lương chi tiết -->

            //Thay đổi các hệ số phụ cấp của các phụ cấp ẩn: Chức vụ, thâm niên...
            BangCSDL bangCSDL = new BangCSDL("L_BangLuongChiTiet");
            List<String> arrDSMaCot = bangCSDL.DanhSachTruong();
            for (int i = 0; i < arrDSMaCot.Count; i++)
            {
                String TenTruong = arrDSMaCot[i];
                if (TenTruong.StartsWith("rPhuCap_") && TenTruong.EndsWith("_HeSo")==false)
                {
                    String sGiaTri = "";
                    if (TenTruong == "rPhuCap_AnNinhQuocPhong")
                    {
                        sGiaTri = Convert.ToString(rHeSo_ANQP);
                    }
                    else if (Request.Form[ParentID + "_" + TenTruong + "_HeSo"] != null)
                    {
                        sGiaTri = Request.Form[ParentID + "_" + TenTruong + "_HeSo"];
                    }
                    if (CommonFunction.IsNumeric(sGiaTri))
                    {
                        //Phải có giá trị mới thay đổi hệ số phụ cấp
                        double rGiaTri = Convert.ToDouble(sGiaTri);
                        SqlCommand cmd = new SqlCommand("UPDATE L_BangLuongChiTiet_PhuCap SET rHeSo=@rHeSo WHERE iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet AND sMaTruongHeSo_BangLuong=@sMaTruongHeSo_BangLuong");
                        cmd.Parameters.AddWithValue("@rHeSo", rGiaTri);
                        cmd.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
                        cmd.Parameters.AddWithValue("@sMaTruongHeSo_BangLuong", TenTruong + "_HeSo");
                        Connection.UpdateDatabase(cmd, MaND, IPSua);
                    }
                }
            }

            //Cập nhập bảng phụ cấp
            NameValueCollection data = LuongModels.LayThongTinBangLuongChiTiet(iID_MaBangLuongChiTiet);
            String iID_MaBangLuong = data["iID_MaBangLuong"];
            String TenBangChiTiet = "L_BangLuongChiTiet_PhuCap";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];

            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaBangLuongChiTiet_PhuCap;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                iID_MaBangLuongChiTiet_PhuCap = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaBangLuongChiTiet_PhuCap != "")
                    {
                        //Dữ liệu đã có
                        Bang bang_PhuCap = new Bang(TenBangChiTiet);
                        bang_PhuCap.DuLieuMoi = false;
                        bang_PhuCap.GiaTriKhoa = iID_MaBangLuongChiTiet_PhuCap;
                        bang_PhuCap.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                        bang_PhuCap.MaNguoiDungSua = User.Identity.Name;
                        bang_PhuCap.IPSua = Request.UserHostAddress;
                        bang_PhuCap.Save();
                        CoCapNhapDuLieu = true;
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
                        Bang bang_PhuCap = new Bang(TenBangChiTiet);
                        if (iID_MaBangLuongChiTiet_PhuCap == "")
                        {
                            //Du Lieu Moi
                            bang_PhuCap.DuLieuMoi = true;
                            bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                            bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
                            bang_PhuCap.CmdParams.Parameters.AddWithValue("@bLuonCo", false);

                            //Them cac tham so tu bang L_BangLuongChiTiet
                            bang_PhuCap.CmdParams.Parameters.AddWithValue("@iNamBangLuong", data["iNamBangLuong"]);
                            bang_PhuCap.CmdParams.Parameters.AddWithValue("@iThangBangLuong", data["iThangBangLuong"]);
                            bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", data["iID_MaDonVi"]);
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang_PhuCap.GiaTriKhoa = iID_MaBangLuongChiTiet_PhuCap;
                            bang_PhuCap.DuLieuMoi = false;
                        }
                        bang_PhuCap.MaNguoiDungSua = User.Identity.Name;
                        bang_PhuCap.IPSua = Request.UserHostAddress;

                        //Them tham so
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                if (arrMaCot[j].EndsWith("_ConLai") == false)
                                {
                                    String Truong = "@" + arrMaCot[j];
                                    if (arrMaCot[j].StartsWith("b"))
                                    {
                                        //Nhap Kieu checkbox
                                        if (arrGiaTri[j] == "1")
                                        {
                                            bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, true);
                                        }
                                        else
                                        {
                                            bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, false);
                                        }
                                    }
                                    else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                                    {
                                        //Nhap Kieu so
                                        if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                        {
                                            bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                        }
                                        else
                                        {
                                            bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, 0);
                                        }
                                    }
                                    else
                                    {
                                        //Nhap kieu xau
                                        bang_PhuCap.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                    }
                                }
                            }
                        }
                        bang_PhuCap.Save();
                        CoCapNhapDuLieu = true;
                    }
                }
            }
            if (CoCapNhapDuLieu)
            {
                BangLuongChiTiet_CaNhanModels.CapNhapBangLuongChiTiet(iID_MaBangLuongChiTiet, User.Identity.Name, Request.UserHostAddress, dtChiTiet.Rows[0], dtPhuCap);
            }
            string idAction = Request.Form["idAction"];
            if (idAction == "3")
            {
                String iID_MaBangLuongChiTiet_Tiep = BangLuongChiTietModels.Lay_iID_MaBangLuongChiTiet_Tiep(iID_MaBangLuong, iID_MaBangLuongChiTiet);
                if (iID_MaBangLuongChiTiet_Tiep != "")
                {
                    return RedirectToAction("ThemCanBo", new { iID_MaBangLuong = iID_MaBangLuong, iID_MaBangLuongChiTiet = iID_MaBangLuongChiTiet_Tiep });
                }
            }
            return RedirectToAction("ThemCanBo", new { iID_MaBangLuong = iID_MaBangLuong, iID_MaBangLuongChiTiet = iID_MaBangLuongChiTiet, Saved = 1 });
        }
        #endregion
    }
}
