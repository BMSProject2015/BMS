using System;
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

namespace VIETTEL.Controllers
{
    public class QLDA_CapPhatController : Controller
    {
        //
        // GET: /QLDA_CapPhat/
        public string sViewPath = "~/Views/QLDA/CapPhat/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_CapPhat", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "QLDA_CapPhat_Dot_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            
        }
        [Authorize]
        public ActionResult Edit(String iID_MaDotCapPhat)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaDotCapPhat))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            return View(sViewPath + "QLDA_CapPhat_Edit.aspx");
        }
        [Authorize]
        public ActionResult AddCapPhat(String iID_MaDotCapPhat)
        {
            ViewData["DuLieuMoi"] = "1";
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            return View(sViewPath + "QLDA_CapPhat_List.aspx");
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
                String dNgayDotNganSach = Request.Form[ParentID + "_vidNgayLap"];
                String sNoiDungCapPhat = Request.Form[ParentID + "_sNoiDungCapPhat"];
                DateTime d = Convert.ToDateTime(CommonFunction.LayNgayTuXau(dNgayDotNganSach));
                Int32 iSoDot = 0;
                if (QLDA_CapPhatModels.Get_Max_Dot(NamLamViec) != "")
                {
                    iSoDot = Convert.ToInt32(QLDA_CapPhatModels.Get_Max_Dot(NamLamViec)) + 1;
                };

                Bang bang = new Bang("QLDA_CapPhat_Dot");
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                bang.CmdParams.Parameters.AddWithValue("@iDot", iSoDot);
                bang.CmdParams.Parameters.AddWithValue("@sTen", "Đợt");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
                if (dNgayDotNganSach == null || dNgayDotNganSach == "")
                {
                    arrLoi.Add("err_dNgayLap", "Trùng đợt ngân sách");
                }
                if (sNoiDungCapPhat == null || sNoiDungCapPhat == "")
                {
                    arrLoi.Add("err_sNoiDungCapPhat", "Bạn phải nhập nội dung cấp phát!");
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
                    return View(sViewPath + "QLDA_CapPhat_Dot_Index.aspx");
                }
            }
            return RedirectToAction("Index", "QLDA_CapPhat");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SuaSubmit(String ParentID, String iID_MaDotCapPhat)
        {


          
            String dNgayDotNganSach = Request.Form[ParentID + "_vidNgayLap"];
            String sNoiDungCapPhat = Request.Form[ParentID + "_sNoiDungCapPhat"];
            Bang bang = new Bang("QLDA_CapPhat_Dot");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
            bang.DuLieuMoi = false;
            bang.GiaTriKhoa = iID_MaDotCapPhat;
            if (dNgayDotNganSach == null || dNgayDotNganSach == "")
            {
                arrLoi.Add("err_dNgayLap", "Trùng đợt ngân sách");
            }
            if (sNoiDungCapPhat == null || sNoiDungCapPhat == "")
            {
                arrLoi.Add("err_sNoiDungCapPhat", "Bạn phải nhập nội dung cấp phát!");
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
                return View(sViewPath + "QLDA_CapPhat_Dot_Index.aspx");
            }

            return RedirectToAction("Index", "QLDA_CapPhat");
        }
        [Authorize]
        public ActionResult Delete_Dot(String iID_MaDotCapPhat)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_CapPhat_Dot", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = QLDA_CapPhatModels.Delete_Dot_CapPhat(iID_MaDotCapPhat, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "QLDA_CapPhat");
        }
        [Authorize]
        public ActionResult Detail(String iID_MaDotCapPhat)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_CapPhat", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            return View(sViewPath + "QLDA_CapPhat_Index.aspx");
        }
        [Authorize]
        public ActionResult ChiTiet(String iID_MaDotCapPhat)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_CapPhat", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            return View(sViewPath + "QLDA_CapPhat_ChiTiet.aspx");
        }
        [Authorize]
        public ActionResult btnPrev(String iID_MaDotCapPhat, String iID_MaCapPhat, String iNam)
        {
            DataTable dt = QLDA_CapPhatModels.Get_PrevRow_CapPhat(iID_MaDotCapPhat, iID_MaCapPhat, iNam);
            String MaCapPhat = Convert.ToString(dt.Rows[0]["iID_MaCapPhat"]);
            dt.Dispose();
            ViewData["DuLieuMoi"] = "0";
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["iID_MaCapPhat"] = MaCapPhat;
            return View(sViewPath + "QLDA_CapPhat_List.aspx");
        }
        [Authorize]
        public ActionResult btnNext(String iID_MaDotCapPhat, String iID_MaCapPhat, String iNam)
        {
            DataTable dt = QLDA_CapPhatModels.Get_NextRow_CapPhat(iID_MaDotCapPhat, iID_MaCapPhat, iNam);
            String MaCapPhat = Convert.ToString(dt.Rows[0]["iID_MaCapPhat"]);
            dt.Dispose();
            ViewData["DuLieuMoi"] = "0";
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["iID_MaCapPhat"] = MaCapPhat;
            return View(sViewPath + "QLDA_CapPhat_List.aspx");
        }
        [Authorize]
        public ActionResult btnCheck(String iID_MaDotCapPhat, String iID_MaCapPhat)
        {
            ViewData["DuLieuMoi"] = "1";
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            return View(sViewPath + "QLDA_CapPhat_List.aspx");
        }
        [Authorize]
        public ActionResult btnTrinhDuyet(String iID_MaDotCapPhat, String iID_MaCapPhat)
        {
            ViewData["DuLieuMoi"] = "1";
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            return View(sViewPath + "QLDA_CapPhat_List.aspx");
        }
        [Authorize]
        public ActionResult btnThongTri(String iID_MaDotCapPhat, String iID_MaCapPhat)
        {
            ViewData["DuLieuMoi"] = "1";
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            return View(sViewPath + "QLDA_CapPhat_List.aspx");
        }
        [Authorize]
        public ActionResult List(String iID_MaDotCapPhat, String iID_MaCapPhat)
        {
            ViewData["DuLieuMoi"] = "0";

            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            DataTable dt = QLDA_CapPhatModels.Get_MaxRow_CapPhat(iID_MaDotCapPhat, NamLamViec);
            if (dt.Rows.Count > 0)
            {
                iID_MaCapPhat = Convert.ToString(dt.Rows[0]["iID_MaCapPhat"]);
            }
            if (String.IsNullOrEmpty(iID_MaCapPhat))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
            return View(sViewPath + "QLDA_CapPhat_List.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LuuCapPhatSubmit(String ParentID, String iID_MaCapPhat, String iID_MaDotCapPhat)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_CapPhat", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaHopDong = Convert.ToString(Request.Form[ParentID + "_iID_MaHopDong"]);
            String sDonViThuHuong = Convert.ToString(Request.Form[ParentID + "_sDonViThuHuong"]);
            String sSoTaiKhoan = Convert.ToString(Request.Form[ParentID + "_sSoTaiKhoan"]);
            String sNganHang = Convert.ToString(Request.Form[ParentID + "_sNganHang"]);
            String iID_MaNguonNganSach = Convert.ToString(Request.Form[ParentID + "_iID_MaNguonNganSach"]);
            String dNgayDeNghi = Convert.ToString(Request.Form[ParentID + "_vidNgayDeNghi"]);
            String rChuDauTuTamUng = Convert.ToString(Request.Form[ParentID + "_rChuDauTuTamUng"]);
            String rPheDuyetTamUng = Convert.ToString(Request.Form[ParentID + "_rPheDuyetTamUng"]);
            String rChuDauTuThanhToan = Convert.ToString(Request.Form[ParentID + "_rChuDauTuThanhToan"]);
            String rPheDuyetThanhToanTrongNam = Convert.ToString(Request.Form[ParentID + "_rPheDuyetThanhToanTrongNam"]);
            String rPheDuyetThanhToanHoanThanh = Convert.ToString(Request.Form[ParentID + "_rPheDuyetThanhToanHoanThanh"]);
            String rChuDauTuThuTamUng = Convert.ToString(Request.Form[ParentID + "_rChuDauTuThuTamUng"]);
            String rPheDuyetThuTamUng = Convert.ToString(Request.Form[ParentID + "_rPheDuyetThuTamUng"]);
            String rPheDuyetThuKhac = Convert.ToString(Request.Form[ParentID + "_rPheDuyetThuKhac"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            if (iID_MaHopDong == "" || iID_MaHopDong == Guid.Empty.ToString())
            {
                arrLoi.Add("err_iID_MaHopDong", "Bạn phải chọn hợp đồng!");
            }
            if (sDonViThuHuong == "" && String.IsNullOrEmpty(sDonViThuHuong) == true)
            {
                arrLoi.Add("err_sDonViThuHuong", "Bạn phải nhập đơn vị thụ hưởng!");
            }
            if (sSoTaiKhoan == "" && String.IsNullOrEmpty(sSoTaiKhoan) == true)
            {
                arrLoi.Add("err_sSoTaiKhoan", "Bạn phải nhập số tài khoản!");
            }
            if (sNganHang == "" && String.IsNullOrEmpty(sNganHang) == true)
            {
                arrLoi.Add("err_sNganHang", "Bạn phải nhập tên ngân hàng!");
            }
            if (iID_MaNguonNganSach == "" && String.IsNullOrEmpty(iID_MaNguonNganSach) == true)
            {
                arrLoi.Add("err_iID_MaNguonNganSach", "Bạn phải chọn nguồn ngân sách!");
            }
            if (dNgayDeNghi == "" && String.IsNullOrEmpty(dNgayDeNghi) == true)
            {
                arrLoi.Add("err_dNgayDeNghi", "Bạn phải chọn ngày đề nghị!");
            }
            if (rChuDauTuTamUng == "" && String.IsNullOrEmpty(rChuDauTuTamUng) == true)
            {
                arrLoi.Add("err_rChuDauTuTamUng", "Bạn phải nhập số tiền chủ đầu tư tạm ứng!");
            }
            if (rPheDuyetTamUng == "" && String.IsNullOrEmpty(rPheDuyetTamUng) == true)
            {
                arrLoi.Add("err_rPheDuyetTamUng", "Bạn phải nhập phê duyệt tạm ứng!");
            }
            if (rChuDauTuThanhToan == "" && String.IsNullOrEmpty(rChuDauTuThanhToan) == true)
            {
                arrLoi.Add("err_rChuDauTuThanhToan", "Bạn phải nhập số tiền chủ đầu tư thanh toán!");
            }
            if (rPheDuyetThanhToanTrongNam == "" && String.IsNullOrEmpty(rPheDuyetThanhToanTrongNam) == true)
            {
                arrLoi.Add("err_rPheDuyetThanhToanTrongNam", "Bạn phải nhập phê duyệt thanh toán trong năm!");
            }
            if (rPheDuyetThanhToanHoanThanh == "" && String.IsNullOrEmpty(rPheDuyetThanhToanHoanThanh) == true)
            {
                arrLoi.Add("err_rPheDuyetThanhToanHoanThanh", "Bạn phải nhập phê duyệt thanh toán hoàn thành!");
            }
            if (rChuDauTuThuTamUng == "" && String.IsNullOrEmpty(rChuDauTuThuTamUng) == true)
            {
                arrLoi.Add("err_rChuDauTuThuTamUng", "Bạn phải nhập chủ đầu tư tạm ứng!");
            }
            if (rPheDuyetThuTamUng == "" && String.IsNullOrEmpty(rPheDuyetThuTamUng) == true)
            {
                arrLoi.Add("err_rPheDuyetThuTamUng", "Bạn phải nhập phê duyệt thu tạm ứng!");
            }
            if (rPheDuyetThuKhac == "" && String.IsNullOrEmpty(rPheDuyetThuKhac) == true)
            {
                arrLoi.Add("err_rPheDuyetThuKhac", "Bạn phải nhập phê duyệt thu khác!");
            }
            if (iID_MaTrangThaiDuyet == "" && iID_MaTrangThaiDuyet == "-1")
            {
                arrLoi.Add("err_iID_MaTrangThaiDuyet", "Bạn phải chọn trạng thái!");
            }

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "1";
                ViewData["iID_MaCapPhat"] = iID_MaCapPhat;
                ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
                return View(sViewPath + "QLDA_CapPhat_List.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                DataRow R = dtCauHinh.Rows[0];

                Bang bang = new Bang("QLDA_CapPhat");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);

                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    String iID_MaCapPhatAddNew = Convert.ToString(bang.Save());
                    iID_MaCapPhat = iID_MaCapPhatAddNew;
                }
                else
                {
                    bang.GiaTriKhoa = iID_MaCapPhat;
                    bang.Save();
                }
            }
            return RedirectToAction("List", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaCapPhat = iID_MaCapPhat });
        }
        public JsonResult get_objDotNganSach(String ParentID, String NamLamViec, String MaND)
        {
            return Json(get_NgayDotNganSach(ParentID, NamLamViec, MaND), JsonRequestBehavior.AllowGet);
        }

        public static String get_NgayDotNganSach(String ParentID, String NamLamViec, String MaND)
        {
            String sNguoiDung = "";
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            //Xác định là nhóm trợ lý phòng ban là người tạo mới trong NS_PhanHe_TrangThaiDuyet
            bool isModify = LuongCongViecModel.NguoiDungTaoMoi(PhanHeModels.iID_MaPhanHeVonDauTu, MaND);
            if (isModify)
            {
                sNguoiDung = MaND;
                DK = " AND sID_MaNguoiDungTao=@sID_MaNguoiDungTao";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
            }
            String SQL = String.Format(@"SELECT Convert(varchar(10),QLDA_CapPhat_Dot.dNgayLap,103) as dNgayDotNganSachHT, QLDA_CapPhat_Dot.*  
                          FROM QLDA_CapPhat_Dot 
                          WHERE iNamLamViec=@iNamLamViec {0} AND iTrangThai=1 
                          ORDER BY iDot", DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String strData = string.Empty;
            StringBuilder builder = new StringBuilder();
            if (dt != null)
            {
                builder.Append("<table class='mGrid'>");
                builder.Append("<tr>");
                builder.Append("<th style=\"width: 3%;\" align=\"center\">STT</th>");
                builder.Append("<th style=\"width: 5%;\" align=\"center\">Năm làm việc</th>");
                builder.Append("<th style=\"width: 15%;\" align=\"center\">Ngày cấp phát</th>");
                builder.Append("<th style=\"width: 10%;\" align=\"center\">Số cấp phát</th>");
                builder.Append("<th style=\"width: 32%;\" align=\"center\">Nội dung cấp phát</th>");
                builder.Append("<th style=\"width: 15%;\" align=\"center\">Người tạo</th>");
                builder.Append("<th style=\"width: 5%;\" align=\"center\">Chi tiết</th>");
                builder.Append("<th style=\"width: 5%;\" align=\"center\">Sửa</th>");
                builder.Append("<th style=\"width: 5%;\" align=\"center\">Xóa</th>");
                builder.Append("</tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    int STT = i + 1;
                    String classtr = "";
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }

                    String urlDetail = "/QLDA_CapPhat/Detail?iID_MaDotCapPhat=" + R["iID_MaDotCapPhat"];
                    String urlChiTiet = "/QLDA_CapPhat/ChiTiet?iID_MaDotCapPhat=" + R["iID_MaDotCapPhat"];
                    String urlEdit = "/QLDA_CapPhat/Edit?iID_MaDotCapPhat=" + R["iID_MaDotCapPhat"];
                    String urlDelete = "/QLDA_CapPhat/Delete_Dot?iID_MaDotCapPhat=" + R["iID_MaDotCapPhat"];

                    builder.Append("<tr " + classtr + ">");
                    builder.Append("<td align=\"center\">");
                    builder.Append("" + STT + "");
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append("" + R["iNamLamViec"] + "");
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\"><b>{1}</b></a>", urlDetail, CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayLap"]))));
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\"><b>{1}</b></a>", urlDetail, R["iDot"]));
                    builder.Append("</td>");
                    builder.Append("<td align=\"left\">");
                    builder.Append(String.Format("{0}", Convert.ToString(R["sNoiDungCapPhat"])));
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("{0}", Convert.ToString(R["sID_MaNguoiDungTao"])));
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\">{1}</a>", urlChiTiet, "<img src='../Content/Themes/images/btnSetting.png' alt='' />"));
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\">{1}</a>", urlEdit, "<img src='../Content/Themes/images/Edit.gif' alt='' />"));
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\">{1}</a>", urlDelete, "<img src='../Content/Themes/images/delete.gif' alt='' />"));
                    builder.Append("</td>");
                    builder.Append("</tr>");
                }
                builder.Append("</table>");
                strData = builder.ToString();
            }
            return strData;
        }

        public JsonResult get_dtHopDong(String ParentID, String iID_MaHopDong)
        {
            return Json(get_objHopDong(ParentID, iID_MaHopDong), JsonRequestBehavior.AllowGet);
        }

        public static String get_objHopDong(String ParentID, String iID_MaHopDong)
        {
            String strThongTinHopDong = string.Empty;
            if (string.IsNullOrEmpty(iID_MaHopDong) == false)
            {
                String strSQL = String.Format("SELECT * FROM QLDA_HopDongChiTiet WHERE iID_MaHopDong = @iID_MaHopDong AND iTrangThai = 1");
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strSQL;
                cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                DataRow R;
                if (dt.Rows.Count > 0)
                {
                    R = dt.Rows[0];
                    String strDonViThiCong = Convert.ToString(QLDA_DonViThiCongModels.Get_Row_Data(Convert.ToString(R["iID_MaDonViThiCong"])).Rows[0]["sTen"]);
                    DataTable dtDuAn = QLDA_DanhMucDuAnModels.Row_DanhMucDuAn(Convert.ToString(R["iID_MaDanhMucDuAn"]));
                    String strTenDuAn = Convert.ToString(dtDuAn.Rows[0]["TenHT"]);
                    String strDonViChuQuan = Convert.ToString(DanhMucModels.GetRow_DonVi(Convert.ToString(dtDuAn.Rows[0]["iID_MaDonVi"])).Rows[0]["TenHT"]);
                    String strChuDauTu = Convert.ToString(QLDA_ChuDauTuModels.Get_Row_ChuDauTu(Convert.ToString(dtDuAn.Rows[0]["iID_MaChuDauTu"])).Rows[0]["sTen"]);
                    dtDuAn.Dispose();
                    dt.Dispose();

                    String strTongGiaTriHopDong = Convert.ToString(R["rSoTien"]);

                    strThongTinHopDong = strTenDuAn + "#############$$$$$" + strDonViChuQuan + "#############$$$$$" + strChuDauTu + "#############$$$$$" + strDonViThiCong + "#############$$$$$" + CommonFunction.DinhDangSo(strTongGiaTriHopDong);
                }
            }
            return strThongTinHopDong;
        }

        public JsonResult get_dtMucLucDuan(String ParentID, String iID_MaHopDong, String iID_MaDanhMucDuAn)
        {
            return Json(get_objMucLucDuan(ParentID, iID_MaHopDong, iID_MaDanhMucDuAn), JsonRequestBehavior.AllowGet);
        }

        public static String get_objMucLucDuan(String ParentID, String iID_MaHopDong, String iID_MaDanhMucDuAn)
        {
            String strDanhMucDuAn = string.Empty;
            if (string.IsNullOrEmpty(iID_MaHopDong) == false)
            {
                String strSQL = String.Format("SELECT iID_MaDanhMucDuAn,sTenDuAn FROM QLDA_HopDongChiTiet WHERE iID_MaHopDong = @iID_MaHopDong AND iTrangThai = 1 ORDER BY sXauNoiMa_DuAn");
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strSQL;
                cmd.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
                DataTable dt = Connection.GetDataTable(cmd);

                if (dt != null)
                {
                    DataRow R = dt.NewRow();
                    R["iID_MaDanhMucDuAn"] = Guid.Empty;
                    R["sTenDuAn"] = "--- Danh sách công trình dự án ---";
                    dt.Rows.InsertAt(R, 0);

                    SelectOptionList slDanhMucDuan = new SelectOptionList(dt, "iID_MaDanhMucDuAn", "sTenDuAn");
                    strDanhMucDuAn = MyHtmlHelper.DropDownList(ParentID, slDanhMucDuan, iID_MaDanhMucDuAn, "iID_MaDanhMucDuAn", null, "class=\"textbox_uploadbox\" onchange=\"ddlHangMucDuan_SelectedValueChanged(this)\"");
                }
            }
            return "<div>" + strDanhMucDuAn + "</div>";

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaDotCapPhat, String iID_MaHopDong, String iID_MaDanhMucDuAn)
        {
            String MaND = User.Identity.Name;
            String iID_MaChungTu = "";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String iID_MaNguonNganSach = Convert.ToString(R["iID_MaNguonNganSach"]);
            String iID_MaNamNganSach = Convert.ToString(R["iID_MaNamNganSach"]);
            String iNamLamViec = Convert.ToString(R["iNamLamViec"]);
            dtCauHinh.Dispose();

            DataTable dtChungTu = QLDA_CapPhatModels.GetChungTu(iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, Convert.ToString(R["iNamLamViec"]));
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
                String iID_MaChungTuNews = QLDA_CapPhatModels.InsertRecord(iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, Request.Form, MaND, Request.UserHostAddress);
                dtChungTu = QLDA_CapPhatModels.GetChungTu(iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNamLamViec);
            }
            if (dtChungTu.Rows.Count == 1)
            {
                iID_MaChungTu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
                String TenBangChiTiet = "QLDA_CapPhat";

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

                String iID_MaCapPhat;

                //Luu cac hang sua
                String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
                for (int i = 0; i < arrMaHang.Length - 1; i++)
                {
                    iID_MaCapPhat = arrMaHang[i];
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (iID_MaCapPhat != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang(TenBangChiTiet);
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = iID_MaCapPhat;
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
                            iID_MaCapPhat = arrMaHang[i];
                            if (iID_MaCapPhat == "")
                            {
                                //Du Lieu Moi
                                bang.DuLieuMoi = true;
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaHopDong", iID_MaHopDong);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucDuAn", iID_MaDanhMucDuAn);
                                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaCapPhat;
                                bang.DuLieuMoi = false;
                            }

                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.CmdParams.Parameters.AddWithValue("@sNoiDungCapPhat", "");
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
                string idAction = Request.Form["idAction"];
                if (idAction == "1")
                {
                    return RedirectToAction("TuChoi", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaHopDong = iID_MaHopDong, iID_MaDanhMucDuAn = iID_MaDanhMucDuAn, iNam = iNamLamViec });
                }
                else if (idAction == "2")
                {
                    return RedirectToAction("TrinhDuyet", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaHopDong = iID_MaHopDong, iID_MaDanhMucDuAn = iID_MaDanhMucDuAn, iNam = iNamLamViec });
                }
                return RedirectToAction("Detail", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaHopDong = iID_MaHopDong, iID_MaDanhMucDuAn = iID_MaDanhMucDuAn });
            }
            else
            {
                return RedirectToAction("Detail", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaHopDong = iID_MaHopDong, iID_MaDanhMucDuAn = iID_MaDanhMucDuAn });
            }
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChiTietSubmit(String iID_MaDotCapPhat)
        {
            String MaND = User.Identity.Name;
            String iID_MaChungTu = "";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            DataRow R = dtCauHinh.Rows[0];
            String iID_MaNguonNganSach = Convert.ToString(R["iID_MaNguonNganSach"]);
            String iID_MaNamNganSach = Convert.ToString(R["iID_MaNamNganSach"]);
            String iNamLamViec = Convert.ToString(R["iNamLamViec"]);
            dtCauHinh.Dispose();

            DataTable dtChungTu = QLDA_CapPhatModels.GetChungTu(iID_MaDotCapPhat, "", "", Convert.ToString(R["iNamLamViec"]));
           
           
              //  iID_MaChungTu = Convert.ToString(dtChungTu.Rows[j]["iID_MaChungTu"]);
                String TenBangChiTiet = "QLDA_CapPhat";
                string idXauMaCacHang = Request.Form["idXauMaCacHang"];
                string idXauMaCacCot = Request.Form["idXauMaCacCot"];
                string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
                string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
                string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
                String[] arrMaHang = idXauMaCacHang.Split(',');
                String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
                String[] arrMaCot = idXauMaCacCot.Split(',');
                String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

                String iID_MaCapPhat;

                //Luu cac hang sua
                String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
                for (int i = 0; i < arrMaHang.Length - 1; i++)
                {
                    iID_MaCapPhat = arrMaHang[i];
                    if (arrHangDaXoa[i] == "1")
                    {
                        //Lưu các hàng đã xóa
                        if (iID_MaCapPhat != "")
                        {
                            //Dữ liệu đã có
                            Bang bang = new Bang(TenBangChiTiet);
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = iID_MaCapPhat;
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
                            iID_MaCapPhat = arrMaHang[i];
                            if (iID_MaCapPhat == "")
                            {
                            }
                            else
                            {
                                //Du Lieu Da Co
                                bang.GiaTriKhoa = iID_MaCapPhat;
                                bang.DuLieuMoi = false;
                            }

                            bang.MaNguoiDungSua = User.Identity.Name;
                            bang.IPSua = Request.UserHostAddress;
                            bang.CmdParams.Parameters.AddWithValue("@sNoiDungCapPhat", "");
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
                //    return RedirectToAction("TuChoi", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaHopDong = iID_MaHopDong, iID_MaDanhMucDuAn = iID_MaDanhMucDuAn, iNam = iNamLamViec });
                //}
                //else if (idAction == "2")
                //{
                //    return RedirectToAction("TrinhDuyet", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaHopDong = iID_MaHopDong, iID_MaDanhMucDuAn = iID_MaDanhMucDuAn, iNam = iNamLamViec });
                //}
                return RedirectToAction("ChiTiet", new { iID_MaDotCapPhat = iID_MaDotCapPhat  });
            }
            
        
        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaDotCapPhat, String iID_MaHopDong, String iID_MaDanhMucDuAn, String iNam)
        {
            String MaND = User.Identity.Name;
            DataTable dtChungTu = QLDA_CapPhatModels.GetChungTu(iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNam);
            String iID_MaChungTu = "";
            if (dtChungTu.Rows.Count > 0)
            {
                iID_MaChungTu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
                //Xác định trạng thái duyệt tiếp theo
                int iID_MaTrangThaiDuyet_TrinhDuyet = QLDA_CapPhatModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNam);
                if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
                String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
                dtTrangThaiDuyet.Dispose();

                ///Update trạng thái cho bảng chứng từ
                QLDA_CapPhatModels.Update_iID_MaTrangThaiDuyet(iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNam, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

                ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
                String MaDuyetChungTu = QLDA_CapPhatModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, MaND, Request.UserHostAddress);

                ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
                SqlCommand cmd;
                cmd = new SqlCommand();
                String SQL = "UPDATE QLDA_CapPhat_ChungTu SET iID_MaDuyetChungTuCuoiCung=@iID_MaDuyetChungTuCuoiCung WHERE iID_MaChungTu=@iID_MaChungTu";
                cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.CommandText = SQL;
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                int iID_MaTrangThaiTuChoi = QLDA_CapPhatModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNam);
            }
            return RedirectToAction("Detail", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaHopDong = iID_MaHopDong, iID_MaDanhMucDuAn = iID_MaDanhMucDuAn });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaDotCapPhat, String iID_MaHopDong, String iID_MaDanhMucDuAn, String iNam)
        {
            String MaND = User.Identity.Name;
            DataTable dtChungTu = QLDA_CapPhatModels.GetChungTu(iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNam);
            String iID_MaChungTu = "";
            if (dtChungTu.Rows.Count > 0)
            {
                iID_MaChungTu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
                //Xác định trạng thái duyệt tiếp theo
                int iID_MaTrangThaiDuyet_TuChoi = QLDA_CapPhatModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNam);
                if (iID_MaTrangThaiDuyet_TuChoi <= 0)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
                String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
                dtTrangThaiDuyet.Dispose();

                //Cập nhập trường sSua
                QLDA_CapPhat_DuyetChungTuModels.CapNhapLaiTruong_sSua(iID_MaChungTu);

                ///Update trạng thái cho bảng chứng từ
                QLDA_CapPhatModels.Update_iID_MaTrangThaiDuyet(iID_MaDotCapPhat, iID_MaHopDong, iID_MaDanhMucDuAn, iNam, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

                ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
                String MaDuyetChungTu = QLDA_CapPhatModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, NoiDung, Request.UserHostAddress);

                ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
                SqlCommand cmd;
                cmd = new SqlCommand();
                String SQL = "UPDATE QLDA_CapPhat_ChungTu SET iID_MaDuyetChungTuCuoiCung=@iID_MaDuyetChungTuCuoiCung WHERE iID_MaChungTu=@iID_MaChungTu";
                cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.CommandText = SQL;
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            return RedirectToAction("Detail", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaHopDong = iID_MaHopDong, iID_MaDanhMucDuAn = iID_MaDanhMucDuAn });
        }

        [Authorize]
        public JsonResult get_MucLucNganSach(String iID_MaHopDong, String iID_MaDanhMucDuAn)
        {
            String iID_MaMucLucNganSach = "", sXauNoiMa = "", sLNS = "", sL = "", sK = "", sM = "", sTM = "", sTTM = "", sNG = "", sTNG = "", sMoTa = "";
            DataTable vR;
            vR = QLDA_CapPhatModels.Get_GiaTri_MucLucNganSach_HopDong(iID_MaHopDong, iID_MaDanhMucDuAn);
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
    }
}
