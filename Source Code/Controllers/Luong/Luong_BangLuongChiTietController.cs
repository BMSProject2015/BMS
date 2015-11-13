using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using DomainModel.Controls;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;


namespace VIETTEL.Controllers.Luong
{
    public class Luong_BangLuongChiTietController : Controller
    {
        //
        // GET: /Luong/
        public string sViewPath = "~/Views/Luong/BangLuongChiTiet/";

        [Authorize]
        public ActionResult Index(int? BangLuongChiTiet_page, String iID_MaBangLuong)
        {
            DataTable dt = LuongModels.Get_ChiTietBangLuong(iID_MaBangLuong);
            ViewData["iNamBangLuong"] = dt.Rows[0]["iNamBangLuong"];
            ViewData["iThangBangLuong"] = dt.Rows[0]["iThangBangLuong"];
            ViewData["iID_MaBangLuong"] = iID_MaBangLuong;
            ViewData["BangLuongChiTiet_page"] = BangLuongChiTiet_page;
            return View(sViewPath + "Luong_BangLuongChiTiet_Index.aspx");
        }

        [Authorize]
        public ActionResult ChiTiet(String iID_MaBangLuong)
        {
            return View(sViewPath + "Frames/Luong_BangLuongChiTiet_Frame_ChiTiet.aspx");
        }

        [Authorize]
        public ActionResult TruyLinh(String iID_MaBangLuong)
        {
            ViewData["bPhanTruyLinh"] = "1";
            return View(sViewPath + "Frames/Luong_BangLuongChiTiet_Frame_ChiTiet.aspx");
        }

        [Authorize]
        public ActionResult BaoHiem(String iID_MaBangLuong)
        {
            return View(sViewPath + "Frames/Luong_BangLuongChiTiet_Frame_BaoHiem.aspx");
        }

        [Authorize]
        public ActionResult ThueTNCN(String iID_MaBangLuong)
        {
            return View(sViewPath + "Frames/Luong_BangLuongChiTiet_Frame_ThueTNCN.aspx");
        }

        [Authorize]
        public ActionResult Detail(string iID_MaBangLuongChiTiet)
        {
            Bang bang = new Bang("L_BangLuongChiTiet");
            bang.GiaTriKhoa = iID_MaBangLuongChiTiet;
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, false);
            if (dicData != null)
            {
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "Detail.aspx");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string iID_MaBangLuongChiTiet)
        {
            Bang bang = new Bang("L_BangLuongChiTiet");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.GiaTriKhoa = iID_MaBangLuongChiTiet;
            bang.Delete();
            return RedirectToAction("Index", "BangLuong");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string iID_MaBangLuongChiTiet)
        {

            ViewData["iID_MaBangLuongChiTiet"] = iID_MaBangLuongChiTiet;
            NameValueCollection data = new NameValueCollection();
            if (String.IsNullOrEmpty(iID_MaBangLuongChiTiet) == false)
            {

                ViewData["DuLieuMoi"] = "0";
                data = LuongModels.LayThongTinBangLuongChiTiet(iID_MaBangLuongChiTiet);
            }
            else
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["data"] = data;
            return View(sViewPath + "Luong_BangLuong_Edit.aspx");

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaBangLuong)
        {
            int ThangLamViec = DanhMucModels.ThangLamViec(User.Identity.Name);
            int NamLamViec = DanhMucModels.NamLamViec(User.Identity.Name);
            String iID_MaDuyetBangLuong;
            SqlCommand cmd = new SqlCommand("SELECT iID_MaDuyetBangLuongCuoiCung FROM L_BangLuong WHERE iID_MaBangLuong=@iID_MaBangLuong");
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            iID_MaDuyetBangLuong = Connection.GetValueString(cmd, "");
            cmd.Dispose();

            String strMaBangLuongChiTiet = Request.Form[ParentID + "_iID_MaBangLuongChiTiet"];
            String[] arrMaBangLuongChiTiet = strMaBangLuongChiTiet.Split(',');
            for (int i = 0; i < arrMaBangLuongChiTiet.Length; i++)
            {
                Bang bang = new Bang("L_BangLuongChiTiet");
           

                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID + "_" + arrMaBangLuongChiTiet[i], Request.Form);
                bang.DuLieuMoi = false;
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = arrMaBangLuongChiTiet[i];

                Bang bangDuyet = new Bang("L_DuyetBangLuongChiTiet");
                bangDuyet.MaNguoiDungSua = User.Identity.Name;
                bangDuyet.IPSua = Request.UserHostAddress;


                String iID_MaDuyetBangLuongChiTiet = Request.Form[ParentID + "_" + arrMaBangLuongChiTiet[i] + "_iID_MaDuyetBangLuongChiTiet"];
                if (String.IsNullOrEmpty(iID_MaDuyetBangLuongChiTiet))
                {
                    bangDuyet.DuLieuMoi = true;
                    bangDuyet.CmdParams.Parameters.AddWithValue("@iID_MaDuyetBangLuong", iID_MaDuyetBangLuong);
                    bangDuyet.CmdParams.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                    bangDuyet.CmdParams.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", arrMaBangLuongChiTiet[i]);
                    bang.CmdParams.Parameters["@iID_MaDuyetBangLuongChiTiet"].Value = bangDuyet.GiaTriKhoa;
                }
                else
                {
                    bangDuyet.DuLieuMoi = false;
                    bangDuyet.GiaTriKhoa = iID_MaDuyetBangLuongChiTiet;
                }
                bangDuyet.CmdParams.Parameters.AddWithValue("@sLyDo", bang.CmdParams.Parameters["@sLyDo"].Value);
                bangDuyet.Save();

                bang.Save();

            }
            return RedirectToAction("Index", "Luong_BangLuongChiTiet", new {iID_MaBangLuong=iID_MaBangLuong });
           
        }

        public JavaScriptResult Edit_Fast_LuongChiTiet_Submit(String ParentID, String OnSuccess)
        {
            Bang bang = new Bang("L_BangLuongChiTiet");

            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            String iID_MaBangLuong = Request.Form[ParentID + "_iID_MaBangLuong"];
            if (arrLoi.Count == 0)
            {
                DataTable dt= LuongModels.Get_ChiTietBangLuong(iID_MaBangLuong);


                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dt.Rows[0]["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iThangLamViec", dt.Rows[0]["iThangLamViec"]);
                bang.Save();

                dt.Dispose();
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
            return null;
        }

        //public JavaScriptResult Edit_LuongPhuCap_Submit(String ParentID, String OnSuccess)
        //{
            
        //    String striID_MaBangLuongChiTiet = Request.Form[ParentID + "_iID_MaBangLuongChiTiet"];
        //    String striID_MaPhuCap = Request.Form[ParentID + "_iID_MaPhuCap"];
        //    String[] arriID_MaPhuCap = striID_MaPhuCap.Split(',');
        //    String rSoTien, sCongThuc, bCongThuc, rMuc;
        //    String iID_MaBangLuongChiTiet = Request.Form[ParentID + "_iID_MaBangLuongChiTiet"];

        //    for(int i=0;i<arriID_MaPhuCap.Length;i++)
        //    {
        //        Bang bang = new Bang("L_BangLuongChiTiet_PhuCap");
        //        bang.DuLieuMoi = true;
        //        bang.MaNguoiDungSua = User.Identity.Name;
        //        bang.IPSua = Request.UserHostAddress;
        //        sCongThuc = Request.Form[ParentID + "_" + arriID_MaPhuCap[i] + "_sCongThuc"];
        //        rSoTien = Request.Form[ParentID + "_" + arriID_MaPhuCap[i] + "_rSoTien"];
        //        bCongThuc = Request.Form[ParentID + "_" + arriID_MaPhuCap[i] + "_bCongThuc"];
        //        rMuc = Request.Form[ParentID + "_" + arriID_MaPhuCap[i] + "_rMuc"];
        //        if (String.IsNullOrEmpty(rSoTien)) rSoTien = "0";
        //        if (String.IsNullOrEmpty(rMuc)) rMuc = "0";

        //        bang.CmdParams.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
        //        bang.CmdParams.Parameters.AddWithValue("@iID_MaPhuCap", arriID_MaPhuCap[i]);
        //        bang.CmdParams.Parameters.AddWithValue("@sCongThuc", sCongThuc);
        //        bang.CmdParams.Parameters.AddWithValue("@rSoTien", rSoTien);                
        //        bang.CmdParams.Parameters.AddWithValue("@bCongThuc", bCongThuc);
        //        bang.CmdParams.Parameters.AddWithValue("@rMuc", rMuc);
        //        bang.Save();
        //    }
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
            
        //}

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaBangLuong, String Detail)
        {
            DataTable dtLuongChiTietCu = BangLuongChiTietModels.Get_DataTable(iID_MaBangLuong);
            DataTable dtLuongChiTiet_PhuCapCu = BangLuongChiTietModels.Get_DataTable_PhuCap(iID_MaBangLuong);

            NameValueCollection data = LuongModels.LayThongTinBangLuong(iID_MaBangLuong);
            String TenBangChiTiet = "L_BangLuongChiTiet";

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];

            int iNamBangLuong = Convert.ToInt32(data["iNamBangLuong"]);
            int iThangBangLuong = Convert.ToInt32(data["iThangBangLuong"]);
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaBangLuongChiTiet;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                iID_MaBangLuongChiTiet = arrMaHang[i];
                if (String.IsNullOrEmpty(iID_MaBangLuongChiTiet) == false)
                {
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (iID_MaBangLuongChiTiet != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang(TenBangChiTiet);
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = iID_MaBangLuongChiTiet;
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
                            //Không thêm mới, chỉ làm với 

                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaBangLuongChiTiet;
                            bang.DuLieuMoi = false;

                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;



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
                            }
                            BangLuongChiTietModels.DieuChinhLaiBangTruocKhiGhi(bang, iNamBangLuong, iThangBangLuong);
                            bang.Save();
                            for (int j = 0; j < dtLuongChiTietCu.Rows.Count; j++)
                            {
                                if (Convert.ToString(dtLuongChiTietCu.Rows[j]["iID_MaBangLuongChiTiet"]) == iID_MaBangLuongChiTiet)
                                {
                                    BangLuongChiTiet_CaNhanModels.CapNhapBangLuongChiTiet(iID_MaBangLuongChiTiet, User.Identity.Name, Request.UserHostAddress, dtLuongChiTietCu.Rows[j], dtLuongChiTiet_PhuCapCu);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "Luong_BangLuong", new { iID_MaBangLuong = iID_MaBangLuong, Detail = Detail });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "Luong_BangLuong", new { iID_MaBangLuong = iID_MaBangLuong, Detail = Detail });
            }
            return RedirectToAction(Detail, new { iID_MaBangLuong = iID_MaBangLuong });
        }


        [Authorize]
        public ActionResult Detail_Dialog(string iID_MaBangLuongChiTiet)
        {
            return View(sViewPath + "Luong_BangLuongChiTiet_Dialog.aspx");
        }

        #region Điều chỉnh chung
        [Authorize]
        public ActionResult TrichLuong()
        {
            return View(sViewPath + "Dialogs/Luong_TrichLuong_Dialog.aspx");
        }

        [Authorize]
        public ActionResult TrichLuong_Submit(String ParentID, String iID_MaBangLuong)
        {
            
            String iTrichLuong_Loai = Request.Form[ParentID + "_iTrichLuong_Loai"];
            String rTrichLuong_SoLuong = Request.Form[ParentID + "_rTrichLuong_SoLuong"];
            
            BangLuongChiTietModels.TrichLuong(iID_MaBangLuong, iTrichLuong_Loai, rTrichLuong_SoLuong, User.Identity.Name, Request.UserHostAddress);

            return RedirectToAction("TrichLuong", new { iID_MaBangLuong = iID_MaBangLuong, Saved = 1 });
        }

        [Authorize]
        public ActionResult DieuChinhTienAn()
        {
            return View(sViewPath + "Dialogs/Luong_DieuChinhTienAn_Dialog.aspx");
        }

        [Authorize]
        public ActionResult DieuChinhTienAn_Submit(String ParentID, String iID_MaBangLuong)
        {

            String rTienAn1Ngay = Request.Form[ParentID + "_rTienAn1Ngay"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
                       
            Bang bang = new Bang("L_BangLuongChiTiet");
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị");
            }

            if (arrLoi.Count == 0)
            {
                BangLuongChiTietModels.DieuChinhTienAn1Ngay(iID_MaBangLuong, iID_MaDonVi, rTienAn1Ngay, User.Identity.Name, Request.UserHostAddress);
                return RedirectToAction("DieuChinhTienAn", new { iID_MaBangLuong = iID_MaBangLuong, Saved = 1 });
            }
            else
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }                           
                return View(sViewPath + "Luong_DieuChinhTienAn.aspx");
            }
        }

        [Authorize]
        public ActionResult HeSoKhuVuc()
        {
            return View(sViewPath + "Dialogs/Luong_HeSoKhuVuc_Dialog.aspx");
        }

        [Authorize]
        public ActionResult HeSoKhuVuc_Submit(String ParentID, String iID_MaBangLuong)
        {
            String rPhuCap_KhuVuc_HeSo = Request.Form[ParentID + "_rPhuCap_KhuVuc_HeSo"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];


            Bang bang = new Bang("L_BangLuongChiTiet");
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị");
            }

            if (arrLoi.Count == 0)
            {
                BangLuongChiTietModels.HeSoKhuVuc(iID_MaBangLuong, iID_MaDonVi, rPhuCap_KhuVuc_HeSo, User.Identity.Name, Request.UserHostAddress);
                return RedirectToAction("HeSoKhuVuc", new { iID_MaBangLuong = iID_MaBangLuong, Saved = 1 });
            }
            else
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                return View(sViewPath + "Luong_HeSoKhuVuc_Dialog.aspx");
            }
        }


        [Authorize]
        public ActionResult HuyTapThe()
        {
            return View(sViewPath + "Dialogs/Luong_HuyTapThe_Dialog.aspx");
        }

        [Authorize]
        public ActionResult HuyTapThe_Submit(String ParentID, String iID_MaBangLuong)
        {

            String iID_MaNgachLuong_CanBo = Request.Form[ParentID + "_iID_MaNgachLuong_CanBo"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String sHieuTangGiam=Request.Form[ParentID + "_sHieuTangGiam"];
            String sKyHieu_MucLucQuanSo_HieuTangGiam = Request.Form[ParentID + "_sKyHieu_MucLucQuanSo_HieuTangGiam"];
            Bang bang = new Bang("L_BangLuongChiTiet");
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị");
            }

            if (String.IsNullOrEmpty(iID_MaNgachLuong_CanBo) || iID_MaNgachLuong_CanBo==Guid.Empty.ToString())
            {
                arrLoi.Add("err_iID_MaNgachLuong_CanBo", "Bạn chưa chọn ngạch lương");
            }

            if (arrLoi.Count == 0)
            {
                BangLuongChiTietModels.HuyTapThe(iID_MaBangLuong, iID_MaDonVi, iID_MaNgachLuong_CanBo, sHieuTangGiam, sKyHieu_MucLucQuanSo_HieuTangGiam, User.Identity.Name, Request.UserHostAddress);
                return RedirectToAction("HuyTapThe", new { iID_MaBangLuong = iID_MaBangLuong, Saved = 1 });
            }
            else
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                return View(sViewPath + "Luong_HuyTapThe_Dialog.aspx");
            }
        }
        
        public JsonResult get_objTienAnCuaDonVi(String iID_MaBangLuong, String iID_MaDonVi)
        {
            return Json(LayTienAnCuaDonVi(iID_MaBangLuong, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }

        public String LayTienAnCuaDonVi(String iID_MaBangLuong, String iID_MaDonVi)
        {
            String SQL = "SELECT TOP 1 rTienAn1Ngay FROM L_BangLuongChiTiet WHERE iID_MaBangLuong=@iID_MaBangLuong AND iID_MaDonVi=@iID_MaDonVi AND rTienAn1Ngay>0";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong",iID_MaBangLuong);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }

        public JsonResult get_objHeSoKhuVucCuaDonVi(String iID_MaBangLuong, String iID_MaDonVi)
        {
            return Json(HeSoKhuVucCuaDonVi(iID_MaBangLuong, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }

        public String HeSoKhuVucCuaDonVi(String iID_MaBangLuong, String iID_MaDonVi)
        {
            String SQL = "SELECT TOP 1 rPhuCap_KhuVuc_HeSo FROM L_BangLuongChiTiet WHERE iID_MaBangLuong=@iID_MaBangLuong AND iID_MaDonVi=@iID_MaDonVi AND rPhuCap_KhuVuc_HeSo > 0";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            String vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }

        public JsonResult get_objDSLyDoTangGiam(String ParentID, String sHieuTangGiam, String sKyHieu)
        {
            return Json(LayDSLyDoTangGiam(ParentID,sHieuTangGiam,sKyHieu),JsonRequestBehavior.AllowGet);
        }

        public String LayDSLyDoTangGiam(String ParentID,String sHieuTangGiam,String sKyHieu)
        {
            DataTable dt = BangLuongChiTietModels.Get_dtLyDoTangGiam(sHieuTangGiam);
            SelectOptionList slLyDoTangGiam = new SelectOptionList(dt, "sKyHieu", "sMoTa");
            if (String.IsNullOrEmpty(sKyHieu) && dt.Rows.Count>0)
                sKyHieu = Convert.ToString(dt.Rows[0]["sKyHieu"]);
            dt.Dispose();
            return MyHtmlHelper.DropDownList(ParentID, slLyDoTangGiam, sKyHieu, "sKyHieu_MucLucQuanSo_HieuTangGiam","", "tab-index='-1' style=\"width:147px;\"");
        }

        #endregion

        #region Lấy thông tin cán bộ
        [Authorize]
        public JsonResult get_DanhSachCanBo(String term, String term1)
        {

            List<Object> list = new List<Object>();
            String SQL = String.Format("SELECT TOP 20 CB_CanBo.*, sHoDem + ' ' + sTen as HoTen FROM CB_CanBo WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND (sHoDem + ' ' + sTen LIKE @sTimKiem) ORDER BY sTen,sHoDem");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTimKiem", "%" + term + "%");
            cmd.Parameters.AddWithValue("@iID_MaDonVi",term1);
            DataTable dt = Connection.GetDataTable(cmd);
            int i;
            String TenNgachLuong = "", TenBacLuong;
            //Mã tham số lương tối thiểu =50;
            Decimal LuongTT= LuongModels.ThamSo_LuongToiThieu("50");
            for (i = 0; i < dt.Rows.Count; i++)
            {
                TenNgachLuong = LuongModels.Get_TenNgachLuong(Convert.ToString(dt.Rows[i]["iID_MaNgachLuong"]));
                TenBacLuong = LuongModels.Get_TenBacLuong(Convert.ToString(dt.Rows[i]["iID_MaBacLuong"]));
                    
                Object item = new
                {
                    label = String.Format("{0} - {1}", dt.Rows[i]["iID_MaCanBo"], dt.Rows[i]["HoTen"]),
                    value = Convert.ToString(dt.Rows[i]["iID_MaCanBo"]),
                    iID_MaNgachLuong = Convert.ToString(dt.Rows[i]["iID_MaNgachLuong"]),
                    iID_MaBacLuong = Convert.ToString(dt.Rows[i]["iID_MaBacLuong"]),
                    iID_MaTinhTrangCanBo = Convert.ToString(dt.Rows[i]["iID_MaTinhTrangCanBo"]),
                    dNgayNhapNgu = Convert.ToString(dt.Rows[i]["dNgayNhapNgu"]),
                    dNgayXuatNgu = Convert.ToString(dt.Rows[i]["dNgayXuatNgu"]),
                    dNgayTaiNgu = Convert.ToString(dt.Rows[i]["dNgayTaiNgu"]),
                    sSoSoLuong = Convert.ToString(dt.Rows[i]["sSoSoLuong"]),
                    sSoTaiKhoan = Convert.ToString(dt.Rows[i]["sSoTaiKhoan"]),
                    sHoDem = dt.Rows[i]["sHoDem"],
                    sTen = dt.Rows[i]["sTen"],
                    HoTen = dt.Rows[i]["HoTen"],
                    TenNgachLuong = TenNgachLuong,
                    LuongToiThieu = LuongTT,
                    iSoNguoiPhuThuoc = LaySoNguoiPhuThuoc( Convert.ToString(dt.Rows[i]["iID_MaCanBo"])),
                    TenBacLuong = TenBacLuong
                };

                list.Add(item);
            }
            dt.Dispose();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public static int LaySoNguoiPhuThuoc(String iID_MaCanBo)
        {
            String SQL = "SELECT COUNT(*) FROM CB_NguoiPhuThuoc WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo AND dTuNgay<=@dTuNgay AND @dDenNgay<=dDenNgay";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@dTuNgay", DateTime.Now);
            cmd.Parameters.AddWithValue("@dDenNgay", DateTime.Now);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            int vR=Convert.ToInt16(Connection.GetValue(cmd,0));
            cmd.Dispose();
            return vR;
        }
        #endregion
        [Authorize]
      
        public ActionResult UpdateSSL(String iID_MaBangLuong)
        {
            LuongModels.UpdateSSL();
            return RedirectToAction("Index", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });
        }
    }
}