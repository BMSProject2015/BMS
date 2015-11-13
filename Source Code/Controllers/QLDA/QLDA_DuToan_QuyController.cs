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
    public class QLDA_DuToan_QuyController : Controller
    {
        //
        // GET: /QLDA_DuToan_Quy/
        public string sViewPath = "~/Views/QLDA/DuToan/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_DuToan_Quy", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "QLDA_DuToan_Quy_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
           
        }
        [Authorize]
        public ActionResult Search()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_DuToan_Quy", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "QLDA_DuToan_Quy_Search.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            //String iID_MaDanhMucDuAn = Request.Form[ParentID + "_iID_MaDanhMucDuAn_Search"];
            //String iQuy = Request.Form[ParentID + "_iQuy_Search"];
            String dTuNgay = Request.Form[ParentID + "_vidTuNgay"];
            String dDenNgay = Request.Form[ParentID + "_vidDenNgay"];
            return RedirectToAction("Index", "QLDA_DuToan_Quy", new { dTuNgay = dTuNgay, dDenNgay = dDenNgay });
            //return RedirectToAction("Search", "QLDA_DuToan_Quy", new { iID_MaDanhMucDuAn = iID_MaDanhMucDuAn, iQuy = iQuy, dTuNgay = dTuNgay, dDenNgay = dDenNgay });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String dNgayLap, String iID_MaDuToanQuy_QuyetDinh, String iQuy)
        {
            if (String.IsNullOrEmpty(dNgayLap) || String.IsNullOrEmpty(iID_MaDuToanQuy_QuyetDinh) || String.IsNullOrEmpty(iQuy))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String TenBangChiTiet = "QLDA_DuToan_Quy";

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
                            bang.CmdParams.Parameters.AddWithValue("@dNgayLap", CommonFunction.LayNgayTuXau(dNgayLap));
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDuToanQuy_QuyetDinh", iID_MaDuToanQuy_QuyetDinh);
                            bang.CmdParams.Parameters.AddWithValue("@iQuy", iQuy);
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
            return RedirectToAction("Detail", new { iID_MaDuToanQuy_QuyetDinh = iID_MaDuToanQuy_QuyetDinh, dNgayLap = dNgayLap, iQuy = iQuy });
            //return RedirectToAction("Index", new {dNgayLap = dNgayLap });
        }

        [Authorize]
        public JsonResult get_GiaTriTongKeHoachVon(String iID_MaDanhMucDuAn, String iID_MaMucLucNganSach, String iLoaiKeHoachVon)
        {
            Double rSoTien = 0, rNgoaiTe = 0;
            String sTenNgoaiTe = "";
            DataTable vR = QLDA_DuToan_QuyModels.Get_Table_KeHoachVon(iID_MaDanhMucDuAn, iID_MaMucLucNganSach, iLoaiKeHoachVon);
            if (vR.Rows.Count > 0)
            {
                rSoTien = Convert.ToDouble(vR.Rows[0]["rSoTien"]);
                rNgoaiTe = Convert.ToDouble(vR.Rows[0]["rNgoaiTe"]);
                sTenNgoaiTe = Convert.ToString(vR.Rows[0]["sTenNgoaiTe"]);
            }
            Object item = new
            {
                rSoTien = rSoTien,
                rNgoaiTe = rNgoaiTe,
                sTenNgoaiTe = sTenNgoaiTe
            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaDuToanQuy_QuyetDinh)
        {
            string iID_MaDuToanNam = "";
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_DuToan_Quy_ChungTu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();

            String dNgayLap = Convert.ToString(Request.Form[ParentID + "_vidNgayLap"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);

            if (dNgayLap == "" && String.IsNullOrEmpty(dNgayLap) == true)
            {
                arrLoi.Add("err_dNgayLap", "Bạn phải nhập ngày lập!");
            }
            if (iQuy == "" && String.IsNullOrEmpty(iQuy) == true)
            {
                arrLoi.Add("err_iQuy", "Bạn phải chọn quý!");
            }
            //if (iQuy != "" && String.IsNullOrEmpty(iQuy) == false && DuLieuMoi == "1")
            //{
            //    int Count = QLDA_DuToan_QuyModels.CheckExits_DuToan_Quy_ChungTu(iQuy, NguoiDungCauHinhModels.LayCauHinhChiTiet(User.Identity.Name, "iNamLamViec").ToString());
            //    if (Count > 0)
            //    {
            //        arrLoi.Add("err_iQuy", "Quý chọn đã lập dự toán!");
            //    }

            //}

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaDuToanQuy_QuyetDinh"] = iID_MaDuToanQuy_QuyetDinh;
                return View(sViewPath + "QLDA_DuToan_Quy_ChungTu_Edit.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                DataRow R = dtCauHinh.Rows[0];

                Bang bang = new Bang("QLDA_DuToan_Quy_ChungTu");

                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = iID_MaDuToanQuy_QuyetDinh;
                if (String.IsNullOrEmpty(iID_MaDuToanQuy_QuyetDinh))
                {
                    Guid MaDuToanNam_QuyetDinhID = Guid.NewGuid();
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDuToanQuy_QuyetDinh", Convert.ToString(MaDuToanNam_QuyetDinhID));
                    iID_MaDuToanNam = Convert.ToString(MaDuToanNam_QuyetDinhID);
                }
                Convert.ToString(bang.Save());
            }
            if (!String.IsNullOrEmpty(iID_MaDuToanNam))
            {
                return RedirectToAction("Detail", new { iID_MaDuToanQuy_QuyetDinh = iID_MaDuToanNam, dNgayLap = dNgayLap, iQuy = iQuy });
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        [Authorize]
        public ActionResult Edit(String iID_MaDuToanQuy_QuyetDinh)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaDuToanQuy_QuyetDinh))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaDuToanQuy_QuyetDinh"] = iID_MaDuToanQuy_QuyetDinh;
            return View(sViewPath + "QLDA_DuToan_Quy_ChungTu_Edit.aspx");
        }
        [Authorize]
        public ActionResult Delete(String iID_MaDuToanQuy_QuyetDinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_DuToan_Quy_ChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            SqlCommand cmd = new SqlCommand("UPDATE QLDA_DuToan_Quy_ChungTu SET iTrangThai = 0 WHERE iID_MaDuToanQuy_QuyetDinh=@iID_MaDuToanQuy_QuyetDinh");
            cmd.Parameters.AddWithValue("@iID_MaDuToanQuy_QuyetDinh", iID_MaDuToanQuy_QuyetDinh);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            Bang bang = new Bang("QLDA_DuToan_Quy");
            bang.GiaTriKhoa = iID_MaDuToanQuy_QuyetDinh;
            bang.Delete();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(String dNgayLap, String iID_MaDuToanQuy_QuyetDinh, String iQuy)
        {
            if (String.IsNullOrEmpty(dNgayLap) || String.IsNullOrEmpty(iID_MaDuToanQuy_QuyetDinh) || String.IsNullOrEmpty(iQuy))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_DuToan_Quy", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int Count = QLDA_DuToan_QuyModels.CheckExits_DuToan(iID_MaDuToanQuy_QuyetDinh);
            if (Count == 0)
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                DataRow R = dtCauHinh.Rows[0];
                QLDA_DuToan_QuyModels.Insert_DuToanQuy_By_KHV(iID_MaDuToanQuy_QuyetDinh, dNgayLap, iQuy,
                                                           Convert.ToInt32(R["iNamLamViec"]), User.Identity.Name,
                                                           Request.UserHostAddress);
                if (dtCauHinh != null)
                {
                    dtCauHinh.Dispose();
                }
            }
            return View(sViewPath + "QLDA_DuToan_Quy_Edit.aspx");
        }
    }
}
