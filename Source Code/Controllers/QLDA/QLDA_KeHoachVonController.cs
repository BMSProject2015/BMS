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
    public class QLDA_KeHoachVonController : Controller
    {
        //
        // GET: /QLDA_KeHoachVon/
        public string sViewPath = "~/Views/QLDA/KeHoachVon/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_KeHoachVon", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "QLDA_KeHoachVon_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            
        }
        [Authorize]
        public ActionResult Search()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_KeHoachVon", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "QLDA_KeHoachVon_Search.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String iID_MaDanhMucDuAn = Request.Form[ParentID + "_iID_MaDanhMucDuAn_Search"];
            String sSoQuyetDinh = Request.Form[ParentID + "_sSoQuyetDinh"];
            String iLoaiKeHoachVon = Request.Form[ParentID + "_iLoaiKeHoachVon_Search"];
            String dTuNgay = Request.Form[ParentID + "_vidTuNgay"];
            String dDenNgay = Request.Form[ParentID + "_vidDenNgay"];
            String dTuNgayQD = Request.Form[ParentID + "_vidTuNgayQD"];
            String dDenNgayQD = Request.Form[ParentID + "_vidDenNgayQD"];
            return RedirectToAction("Index", "QLDA_KeHoachVon", new
            {
                sSoQuyetDinh = sSoQuyetDinh,
                iLoaiKeHoachVon = iLoaiKeHoachVon,
                TuNgayQD = dTuNgayQD,
                DenNgayQD = dDenNgayQD,
                TuNgay = dTuNgay,
                DenNgay = dTuNgay
            });
           // return RedirectToAction("Search", "QLDA_KeHoachVon", new { iID_MaDanhMucDuAn = iID_MaDanhMucDuAn, iLoaiKeHoachVon = iLoaiKeHoachVon, dTuNgay = dTuNgay, dDenNgay = dDenNgay });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iLoaiKeHoachVon, String dNgayKeHoachVon, String iID_KeHoachVon_QuyetDinh)
        {
            if (String.IsNullOrEmpty(iLoaiKeHoachVon) || String.IsNullOrEmpty(dNgayKeHoachVon) || String.IsNullOrEmpty(iID_KeHoachVon_QuyetDinh))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String TenBangChiTiet = "QLDA_KeHoachVon";

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
                            bang.CmdParams.Parameters.AddWithValue("@dNgayKeHoachVon", CommonFunction.LayNgayTuXau(dNgayKeHoachVon));
                            bang.CmdParams.Parameters.AddWithValue("@iLoaiKeHoachVon", iLoaiKeHoachVon);
                            bang.CmdParams.Parameters.AddWithValue("@iID_KeHoachVon_QuyetDinh", iID_KeHoachVon_QuyetDinh);
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaDanhMucDuAnChiTiet;
                            bang.DuLieuMoi = false;
                        }

                        if (arrGiaTri[0] != null && arrGiaTri[0] != "")
                        {
                            String[] arrGiaTriDuAn = arrGiaTri[0].Split('-');
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
            return RedirectToAction("Detail", new { iID_KeHoachVon_QuyetDinh = iID_KeHoachVon_QuyetDinh, dNgayKeHoachVon = dNgayKeHoachVon, iLoaiKeHoachVon = iLoaiKeHoachVon });
           // return RedirectToAction("Index", new {iLoaiKeHoachVon = iLoaiKeHoachVon, dNgayKeHoachVon = dNgayKeHoachVon });
        }
        public ActionResult Detail(String iID_KeHoachVon_QuyetDinh, String dNgayKeHoachVon, String iLoaiKeHoachVon)
        {
            if (String.IsNullOrEmpty(iLoaiKeHoachVon) || String.IsNullOrEmpty(dNgayKeHoachVon) || String.IsNullOrEmpty(iID_KeHoachVon_QuyetDinh))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_KeHoachVon", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int Count = QLDA_KeHoachVonModels.CheckExits_DuToan(iID_KeHoachVon_QuyetDinh);
            if (Count==0)
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                DataRow R = dtCauHinh.Rows[0];
                QLDA_KeHoachVonModels.Insert_KHV_DuToanNam(iID_KeHoachVon_QuyetDinh, dNgayKeHoachVon, iLoaiKeHoachVon,
                                                           Convert.ToInt32(R["iNamLamViec"]), User.Identity.Name,
                                                           Request.UserHostAddress);
                if (dtCauHinh!=null)
                {
                    dtCauHinh.Dispose();
                }
            }
            return View(sViewPath + "QLDA_KeHoachVon_Edit.aspx");
        }
        #region "CTu"
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_KeHoachVon_QuyetDinh)
        {
            string iID_MaDuToanNam = "";
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_KeHoachVon_ChungTu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String sSoQuyetDinh = Convert.ToString(Request.Form[ParentID + "_sSoQuyetDinh"]);
            String dNgayQD = Convert.ToString(Request.Form[ParentID + "_vidNgayQD"]);
            String dNgayKeHoachVon = Convert.ToString(Request.Form[ParentID + "_vidNgayKeHoachVon"]);
            String iLoaiKeHoachVon = Convert.ToString(Request.Form[ParentID + "_iLoaiKeHoachVon"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            if (sSoQuyetDinh == "" && String.IsNullOrEmpty(sSoQuyetDinh) == true)
            {
                arrLoi.Add("err_sSoQuyetDinh", "Bạn phải nhập số quyết định!");
            }
            if (dNgayQD == "" && String.IsNullOrEmpty(dNgayQD) == true)
            {
                arrLoi.Add("err_dNgayQD", "Bạn phải nhập ngày quyết định!");
            }
            if (dNgayKeHoachVon == "" && String.IsNullOrEmpty(dNgayKeHoachVon) == true)
            {
                arrLoi.Add("err_dNgayKeHoachVon", "Bạn phải nhập ngày lập kế hoạch vốn!");
            }
            if (iLoaiKeHoachVon == "" && String.IsNullOrEmpty(iLoaiKeHoachVon) == true)
            {
                arrLoi.Add("err_iLoaiKeHoachVon", "Bạn phải nhập loại kế hoạch vốn!");
            }
            //if (dNgayLap != "" && String.IsNullOrEmpty(dNgayLap) == false && DuLieuMoi == "1")
            //{
            //    int Count = QLDA_DuToan_NamModels.CheckExits_DuToanNam_ChungTu(dNgayLap);
            //    if (Count > 0)
            //    {
            //        arrLoi.Add("err_dNgayLap", "Ngày lập đã tồn tại!");
            //    }

            //}

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_KeHoachVon_QuyetDinh"] = iID_KeHoachVon_QuyetDinh;
                return View(sViewPath + "QLDA_KeHoachVon_ChungTu_Edit.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                DataRow R = dtCauHinh.Rows[0];

                Bang bang = new Bang("QLDA_KeHoachVon_ChungTu");

                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = iID_KeHoachVon_QuyetDinh;
                if (String.IsNullOrEmpty(iID_KeHoachVon_QuyetDinh))
                {
                    Guid MaDuToanNam_QuyetDinhID = Guid.NewGuid();
                    bang.CmdParams.Parameters.AddWithValue("@iID_KeHoachVon_QuyetDinh", Convert.ToString(MaDuToanNam_QuyetDinhID));
                    iID_MaDuToanNam = Convert.ToString(MaDuToanNam_QuyetDinhID);
                }
                Convert.ToString(bang.Save());
            }
            if (!String.IsNullOrEmpty(iID_MaDuToanNam))
            {

                return RedirectToAction("Detail", new { iID_KeHoachVon_QuyetDinh = iID_MaDuToanNam, dNgayKeHoachVon = dNgayKeHoachVon, iLoaiKeHoachVon = iLoaiKeHoachVon });
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        [Authorize]
        public ActionResult Edit(String iID_KeHoachVon_QuyetDinh)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_KeHoachVon_QuyetDinh))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_KeHoachVon_QuyetDinh"] = iID_KeHoachVon_QuyetDinh;
            return View(sViewPath + "QLDA_KeHoachVon_ChungTu_Edit.aspx");
        }
        [Authorize]
        public ActionResult Delete(String iID_KeHoachVon_QuyetDinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_KeHoachVon_ChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            SqlCommand cmd = new SqlCommand("UPDATE QLDA_KeHoachVon_ChungTu SET iTrangThai = 0 WHERE iID_KeHoachVon_QuyetDinh=@iID_KeHoachVon_QuyetDinh");
            cmd.Parameters.AddWithValue("@iID_KeHoachVon_QuyetDinh", iID_KeHoachVon_QuyetDinh);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            Bang bang = new Bang("QLDA_KeHoachVon");
            bang.GiaTriKhoa = iID_KeHoachVon_QuyetDinh;
            bang.Delete();
            return RedirectToAction("Index");
        }
#endregion
    }
}
