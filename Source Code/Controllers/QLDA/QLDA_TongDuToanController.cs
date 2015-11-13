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
    public class QLDA_TongDuToanController : Controller
    {
        //
        // GET: /QLDA_TongDuToan/
        public string sViewPath = "~/Views/QLDA/TongDuToan/";
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
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_TongDuToan", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "QLDA_TongDuToan_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
           
        }

        [Authorize]
        public ActionResult List()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_TongDuToan_QuyetDinh", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "QLDA_TongDuToan_List.aspx");
        }
        [Authorize]
        public ActionResult Edit(String iID_MaTongDuToan_QuyetDinh)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaTongDuToan_QuyetDinh))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaTongDuToan_QuyetDinh"] = iID_MaTongDuToan_QuyetDinh;
            return View(sViewPath + "QLDA_TongDuToan_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaTongDuToan_QuyetDinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_TongDuToan_QuyetDinh", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String sSoQuyetDinh = Convert.ToString(Request.Form[ParentID + "_sSoQuyetDinh"]);
            String dNgayQuyetDinh = Convert.ToString(Request.Form[ParentID + "_vidNgayQuyetDinh"]);
            String dNgayLap = Convert.ToString(Request.Form[ParentID + "_vidNgayLap"]);
            String sCapPheDuyet = Convert.ToString(Request.Form[ParentID + "_sCapPheDuyet"]);
            if (sSoQuyetDinh == "" && String.IsNullOrEmpty(sSoQuyetDinh) == true)
            {
                arrLoi.Add("err_sSoQuyetDinh", "Bạn phải chọn nhập số quyết định!");
            }
            if (dNgayQuyetDinh == "" && String.IsNullOrEmpty(dNgayQuyetDinh) == true)
            {
                arrLoi.Add("err_dNgayQuyetDinh", "Bạn phải chọn ngày quyết định!");
            }
            if (dNgayLap == "" && String.IsNullOrEmpty(dNgayLap) == true)
            {
                arrLoi.Add("err_dNgayLap", "Bạn phải nhập ngày lập!");
            }
            if (sCapPheDuyet == "" && String.IsNullOrEmpty(sCapPheDuyet) == true)
            {
                arrLoi.Add("err_sCapPheDuyet", "Bạn phải nhập cấp phê duyệt!");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && sSoQuyetDinh != "")
            {
                if (CheckMaTrung(sSoQuyetDinh) == true)
                {
                    arrLoi.Add("err_sSoQuyetDinh", "Số quyết định đã tồn tại!");
                }
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaTongDuToan_QuyetDinh"] = iID_MaTongDuToan_QuyetDinh;
                return View(sViewPath + "QLDA_TongDuToan_Edit.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                DataRow R = dtCauHinh.Rows[0];

                Bang bang = new Bang("QLDA_TongDuToan_QuyetDinh");

                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = iID_MaTongDuToan_QuyetDinh;
                bang.Save();
            }
            return RedirectToAction("List");
        }
        [Authorize]
        public ActionResult Delete(String iID_MaTongDuToan_QuyetDinh, String sSoQuyetDinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_TongDuToan_QuyetDinh", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            SqlCommand cmd = new SqlCommand("UPDATE QLDA_TongDuToan SET iTrangThai = 0 WHERE sSoPheDuyet=@sSoPheDuyet");
            cmd.Parameters.AddWithValue("@sSoPheDuyet", sSoQuyetDinh);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            Bang bang = new Bang("QLDA_TongDuToan_QuyetDinh");
            bang.GiaTriKhoa = iID_MaTongDuToan_QuyetDinh;
            bang.Delete();
            return RedirectToAction("List");
        }
        public Boolean CheckMaTrung(String sSoQuyetDinh)
        {
            Boolean vR = false;
            DataTable dt = QLDA_TongDuToanModels.Get_Row_TongDuToan_QuyetDinh(sSoQuyetDinh);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            if (dt != null) dt.Dispose();
            return vR;
        }
        public JsonResult get_SoQuyetDinh(String sSoQuyetDinh)
        {
            return Json(get_objCheckSoQuyetDinh(sSoQuyetDinh), JsonRequestBehavior.AllowGet);
        }
        public static String get_objCheckSoQuyetDinh(String sSoQuyetDinh)
        {
            String strMess = "";
            Boolean vR = false;
            DataTable dt = QLDA_TongDuToanModels.Get_Row_TongDuToan_QuyetDinh(sSoQuyetDinh);
            if (dt.Rows.Count > 0)
            {
                strMess = "Số quyết định đã tồn tại!";
            }
            else
            {
                strMess = "Số quyết định có thể thêm được!";
            }
            return strMess;
        }
        [Authorize]
        public ActionResult Search()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_TongDuToan", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "QLDA_TongDuToan_Search.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String sSoQuyetDinh = Request.Form[ParentID + "_sSoQuyetDinh"];
            String sNguoiDung = Request.Form[ParentID + "_sNguoiDung"];
            String dTuNgay = Request.Form[ParentID + "_vidTuNgay"];
            String dDenNgay = Request.Form[ParentID + "_vidDenNgay"];

            return RedirectToAction("List", "QLDA_TongDuToan", new { sSoQuyetDinh = sSoQuyetDinh, dTuNgay = dTuNgay, dDenNgay = dDenNgay, sNguoiDung = sNguoiDung });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String sSoQuyetDinh, String iID_MaTongDuToan_QuyetDinh)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String TenBangChiTiet = "QLDA_TongDuToan";

            //Lấy thông tin bảng quyết định
            NameValueCollection data = QLDA_TongDuToanModels.LayThongTin(sSoQuyetDinh);
            String dNgayQuyetDinh = CommonFunction.LayXauNgay(Convert.ToDateTime(data["dNgayQuyetDinh"]));
            String dNgayLap = CommonFunction.LayXauNgay(Convert.ToDateTime(data["dNgayLap"]));

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
                            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                            bang.CmdParams.Parameters.AddWithValue("@dNgayLap", CommonFunction.LayNgayTuXau(dNgayLap));
                            bang.CmdParams.Parameters.AddWithValue("@sSoPheDuyet", data["sSoQuyetDinh"]);
                            bang.CmdParams.Parameters.AddWithValue("@dNgayPheDuyet", data["dNgayQuyetDinh"]);
                            bang.CmdParams.Parameters.AddWithValue("@sCapPheDuyet", data["sCapPheDuyet"]);
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
                                if (arrMaCot[j].StartsWith("d"))
                                {
                                    //Nhap kieu date
                                    bang.CmdParams.Parameters.AddWithValue(Truong, CommonFunction.ChuyenXauSangNgay(arrGiaTri[j]));
                                }
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
            return RedirectToAction("Index", new { iID_MaTongDuToan_QuyetDinh = iID_MaTongDuToan_QuyetDinh, sSoQuyetDinh = sSoQuyetDinh });
        }

        [Authorize]
        public JsonResult get_GiaTriTongDauTu(String iID_MaDanhMucDuAn, String iID_MaMucLucNganSach)
        {
            Double rSoTien = 0,rNgoaiTe=0;
            String sTenNgoaiTe = "";
            //DataTable vR = QLDA_TongDuToanModels.Get_Table_TongDauTu(iID_MaDanhMucDuAn, iID_MaMucLucNganSach);
            //if (vR.Rows.Count > 0) {
            //    rSoTien = Convert.ToDouble(vR.Rows[0]["rSoTien"]);
            //    rNgoaiTe = Convert.ToDouble(vR.Rows[0]["rNgoaiTe"]);
            //    sTenNgoaiTe = Convert.ToString(vR.Rows[0]["sTenNgoaiTe"]);
            //}
            String vR = "";
            vR = QLDA_TongDuToanModels.Get_GiaTri_TongDauTu(iID_MaDanhMucDuAn, iID_MaMucLucNganSach);
            if (vR != "")
            {
                String[] arrGiaThi = vR.Split('_');
                rSoTien = Convert.ToDouble(arrGiaThi[0]);
                rNgoaiTe = Convert.ToDouble(arrGiaThi[1]);
                sTenNgoaiTe = Convert.ToString(arrGiaThi[2]);
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
        public JsonResult get_MucLucNganSach(String iID_MaDanhMucDuAn)
        {
            String iID_MaMucLucNganSach = "", sXauNoiMa = "", sLNS = "", sL = "", sK = "", sM = "", sTM = "", sTTM = "", sNG = "", sTNG = "", sMoTa = "";
            DataTable vR;
            vR = QLDA_TongDuToanModels.Get_GiaTri_MucLucNganSach(iID_MaDanhMucDuAn);
            if (vR.Rows.Count > 0)
            {
                iID_MaMucLucNganSach = Convert.ToString(vR.Rows[0]["iID_MaMucLucNganSach"]);
                sXauNoiMa = Convert.ToString(vR.Rows[0]["sXauNoiMa"]);
                sLNS = Convert.ToString(vR.Rows[0]["sLNS"]);
                sL = Convert.ToString(vR.Rows[0]["sL"]);
                sK = Convert.ToString(vR.Rows[0]["sK"]);
                sM = Convert.ToString(vR.Rows[0]["sM"]);
                sTM = Convert.ToString(vR.Rows[0]["sTM"]);
                sTTM = Convert.ToString(vR.Rows[0]["sTTM"]);
                sNG = Convert.ToString(vR.Rows[0]["sNG"]);
                sTNG = Convert.ToString(vR.Rows[0]["sTNG"]);
                sMoTa = Convert.ToString(vR.Rows[0]["sMoTa"]);
            }

            Object item = new
            {
                iID_MaMucLucNganSach = iID_MaMucLucNganSach,
                sXauNoiMa = sXauNoiMa,
                sLNS = sLNS,
                sL = sL,
                sK = sK,
                sM = sM,
                sTM = sTM,
                sTTM = sTTM,
                sNG = sNG,
                sTNG = sTNG,
                sMoTa = sMoTa
            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult get_MucLucNganSach_DuToan(String iID_MaDanhMucDuAn)
        {
            String iID_MaMucLucNganSach = "", sXauNoiMa = "", sLNS = "", sL = "", sK = "", sM = "", sTM = "", sTTM = "", sNG = "", sTNG = "", sMoTa = "";
            DataTable vR;
            vR = QLDA_TongDuToanModels.Get_GiaTri_MucLucNganSach_DuToan(iID_MaDanhMucDuAn);
            if (vR.Rows.Count > 0)
            {
                iID_MaMucLucNganSach = Convert.ToString(vR.Rows[0]["iID_MaMucLucNganSach"]);
                sXauNoiMa = Convert.ToString(vR.Rows[0]["sXauNoiMa"]);
                sLNS = Convert.ToString(vR.Rows[0]["sLNS"]);
                sL = Convert.ToString(vR.Rows[0]["sL"]);
                sK = Convert.ToString(vR.Rows[0]["sK"]);
                sM = Convert.ToString(vR.Rows[0]["sM"]);
                sTM = Convert.ToString(vR.Rows[0]["sTM"]);
                sTTM = Convert.ToString(vR.Rows[0]["sTTM"]);
                sNG = Convert.ToString(vR.Rows[0]["sNG"]);
                sTNG = Convert.ToString(vR.Rows[0]["sTNG"]);
                sMoTa = Convert.ToString(vR.Rows[0]["sMoTa"]);
            }

            Object item = new
            {
                iID_MaMucLucNganSach = iID_MaMucLucNganSach,
                sXauNoiMa = sXauNoiMa,
                sLNS = sLNS,
                sL = sL,
                sK = sK,
                sM = sM,
                sTM = sTM,
                sTTM = sTTM,
                sNG = sNG,
                sTNG = sTNG,
                sMoTa = sMoTa
            };
            return Json(item, JsonRequestBehavior.AllowGet);
            //return Json(strGiaTri, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult get_DanhSach(String sTerm)
        {
            return get_DanhSachTongDauTu(sTerm);
        }

        private JsonResult get_DanhSachTongDauTu(String sTerm)
        {
            List<Object> list = new List<Object>();
            String SQL = "SELECT TOP 10 sSoQuyetDinh " +
                         "FROM QLDA_TongDuToan_QuyetDinh " +
                         "WHERE iTrangThai = 1 AND sSoQuyetDinh LIKE @sSoQuyetDinh " +
                         "ORDER BY sSoQuyetDinh";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sSoQuyetDinh", sTerm + "%");
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count == 0)
            {
                //Trong trường hợp không tìm ra dữ liệu sẽ hiển thị tất cả
                dt.Dispose();
                SQL = "SELECT sSoQuyetDinh " +
                         "FROM QLDA_TongDuToan_QuyetDinh " +
                         "WHERE iTrangThai = 1 " +
                         "ORDER BY sSoQuyetDinh";
                cmd = new SqlCommand(SQL);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                Object item = new
                {
                    value = String.Format("{0}", dt.Rows[i]["sSoQuyetDinh"]),
                    label = String.Format("{0}", dt.Rows[i]["sSoQuyetDinh"])
                };
                list.Add(item);
            }
            dt.Dispose();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
