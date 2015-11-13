using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Controllers.QLDA
{
    public class QLDA_HopDongController : Controller
    {
        //
        // GET: /QLDA_HopDong/
        public string sViewPath = "~/Views/QLDA/HopDong/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_HopDong", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "QLDA_HopDong_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
           
        }
        [Authorize]
        public ActionResult List()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_HopDong", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "QLDA_HopDong_List.aspx");
        }
        [Authorize]
        public ActionResult Edit(String iID_MaHopDong)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaHopDong))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaHopDong"] = iID_MaHopDong;
            return View(sViewPath + "QLDA_HopDong_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddNewSubmit(String ParentID)
        {
            String ThemMoi = Request.Form[ParentID + "_iThemMoi"];
            if (ThemMoi == "on")
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                String sSoHopDong = Request.Form[ParentID + "_sSoHopDong"];
                String dNgayHopDong = Request.Form[ParentID + "_vidNgayHopDong"];
                String dNgayLap = Request.Form[ParentID + "_vidNgayLap"];
                String sNoiDung = Request.Form[ParentID + "_sNoiDung"];
                Bang bang = new Bang("QLDA_HopDong");
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
                if (sSoHopDong == null || sSoHopDong == "")
                {
                    arrLoi.Add("err_sSoHopDong", "Bạn chưa nhập số hợp đồng!");
                }
                if (dNgayLap == null || dNgayLap == "")
                {
                    arrLoi.Add("err_dNgayLap", "Bạn chưa nhập ngày lập!");
                }
                if (dNgayHopDong == null || dNgayHopDong == "")
                {
                    arrLoi.Add("err_dNgayHopDong", "Bạn chưa nhập ngày hợp đồng!");
                }
                if (arrLoi.Count == 0)
                {
                    bang.Save();
                }
                else
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    ViewData["bThemMoi"] = true;
                    return View(sViewPath + "QLDA_HopDong_List.aspx");
                }
            }
            return RedirectToAction("List", "QLDA_HopDong");
        }
        public ActionResult EditSubmit(String ParentID,String iID_MaHopDong)
        {
            
                String sSoHopDong = Request.Form[ParentID + "_sSoHopDong"];
                String dNgayHopDong = Request.Form[ParentID + "_vidNgayHopDong"];
                String dNgayLap = Request.Form[ParentID + "_vidNgayLap"];
                String sNoiDung = Request.Form[ParentID + "_sNoiDung"];
                Bang bang = new Bang("QLDA_HopDong");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
               
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = iID_MaHopDong;
                bang.DuLieuMoi = false;
                if (sSoHopDong == null || sSoHopDong == "")
                {
                    arrLoi.Add("err_sSoHopDong", "Bạn chưa nhập số hợp đồng!");
                }
                if (dNgayLap == null || dNgayLap == "")
                {
                    arrLoi.Add("err_dNgayLap", "Bạn chưa nhập ngày lập!");
                }
                if (dNgayHopDong == null || dNgayHopDong == "")
                {
                    arrLoi.Add("err_dNgayHopDong", "Bạn chưa nhập ngày hợp đồng!");
                }
                if (arrLoi.Count == 0)
                {
                    bang.Save();
                }
                else
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    ViewData["bThemMoi"] = true;
                    return View(sViewPath + "QLDA_HopDong_List.aspx");
                }
            
            return RedirectToAction("List", "QLDA_HopDong");
        }
        [Authorize]
        public ActionResult Delete(String iID_MaHopDong)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_HopDong", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = QLDA_HopDongModels.Delete_HopDong(iID_MaHopDong, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("List", "QLDA_HopDong");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String SoHopDong = Request.Form[ParentID + "_sSoHopDong_Search"];
            String dTuNgay = Request.Form[ParentID + "_vidTuNgay"];
            String dDenNgay = Request.Form[ParentID + "_vidDenNgay"];
            String sNoiDung = Request.Form[ParentID + "_sNoiDung"];
            return RedirectToAction("List", "QLDA_HopDong", new { SoHopDong = SoHopDong, dTuNgay = dTuNgay, dDenNgay = dDenNgay,sNoiDung=sNoiDung });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaHopDong)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String TenBangChiTiet = "QLDA_HopDongChiTiet";

            DataTable dtHopDong = QLDA_HopDongModels.Get_Row_HopDong(iID_MaHopDong);

            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaDanhMucDuAnChiTiet;

            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length - 1; i++)
            {
                iID_MaDanhMucDuAnChiTiet = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaDanhMucDuAnChiTiet != "")
                    {
                        //Dữ liệu đã có
                        Bang bang = new Bang(TenBangChiTiet);
                        bang.DuLieuMoi = false;
                        bang.GiaTriKhoa = iID_MaDanhMucDuAnChiTiet;
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
                        iID_MaDanhMucDuAnChiTiet = arrMaHang[i];
                        if (iID_MaDanhMucDuAnChiTiet == "")
                        {
                            //Du Lieu Moi
                            bang.DuLieuMoi = true;
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
                            bang.CmdParams.Parameters.AddWithValue("@dNgayLap", dtHopDong.Rows[0]["dNgayLap"]);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                            //bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                            //bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaDanhMucDuAnChiTiet;
                            bang.DuLieuMoi = false;
                        }

                        if (arrGiaTri[1] != null && arrGiaTri[1] != "")
                        {
                            String[] arrGiaTriDuAn = arrGiaTri[1].Split('-');
                            //Điền thông tin các trường cho dự án                        
                            bang.CmdParams.Parameters.AddWithValue("@sDeAn", arrGiaTriDuAn[0]);
                            bang.CmdParams.Parameters.AddWithValue("@sDuAn", arrGiaTriDuAn[1]);
                            bang.CmdParams.Parameters.AddWithValue("@sDuAnThanhPhan", arrGiaTriDuAn[2]);
                            bang.CmdParams.Parameters.AddWithValue("@sCongTrinh", arrGiaTriDuAn[3]);
                            bang.CmdParams.Parameters.AddWithValue("@sHangMucCongTrinh", arrGiaTriDuAn[4]);
                            bang.CmdParams.Parameters.AddWithValue("@sHangMucChiTiet", arrGiaTriDuAn[5]);
                            bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa_DuAn", arrGiaTriDuAn[0] + "-" + arrGiaTriDuAn[1] + "-" + arrGiaTriDuAn[2] + "-" + arrGiaTriDuAn[3] + "-" + arrGiaTriDuAn[4] + "-" + arrGiaTriDuAn[5]);
                        }

                        bang.MaNguoiDungSua = User.Identity.Name;
                        bang.IPSua = Request.UserHostAddress;
                        bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
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
                        bang.Save();
                    }
                }
            }
            //Sự kiện bấn vào nút trình duyệt
            //string idAction = Request.Form["idAction"];
            //if (idAction == "1")
            //{
            //    return RedirectToAction("TuChoi", "KTCT_TienMat_ChungTu", new { iID_MaDanhMucDuAn = iID_MaDanhMucDuAn });
            //}
            //else if (idAction == "2")
            //{
            //    return RedirectToAction("TrinhDuyet", "KTCT_TienMat_ChungTu", new { iID_MaDanhMucDuAn = iID_MaDanhMucDuAn });
            //}
            return RedirectToAction("Index", new {iID_MaHopDong=iID_MaHopDong});
        }
    }
}
