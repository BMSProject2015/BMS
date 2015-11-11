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

namespace VIETTEL.Controllers.Luong
{
    public class Luong_BangLuongController : Controller
    {
        //
        // GET: /Luong/
        public string sViewPath = "~/Views/Luong/BangLuong/";
        [Authorize]
        public ActionResult Index(int? BangLuong_page,String iNamLamViec,String iThangLamViec)
        {   
            ViewData["BangLuong_page"] = BangLuong_page;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iThangLamViec"] = iThangLamViec;
            return View(sViewPath + "Luong_BangLuong_Index.aspx");
        }

        [Authorize]
        public ActionResult Detail(String iID_MaBangLuong)
        {
            Bang bang = new Bang("L_BangLuong");
            bang.GiaTriKhoa = iID_MaBangLuong;
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
        public ActionResult Delete(String iID_MaBangLuong,String iNamLamViec,String iThangLamViec)
        {
            Bang bang = new Bang("L_BangLuong");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.GiaTriKhoa = iID_MaBangLuong;
            bang.Delete();
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iThangLamViec"] = iThangLamViec;
            return View(sViewPath + "Luong_BangLuong_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(String iID_MaBangLuong, String iNamLamViec, String iThangLamViec)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaBangLuong) && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeLuong, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "L_BangLuong", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }

            ViewData["iID_MaBangLuong"] = iID_MaBangLuong;
            NameValueCollection data = new NameValueCollection();
            if (String.IsNullOrEmpty(iID_MaBangLuong) == false)
            {

                ViewData["DuLieuMoi"] = "0";
                data = LuongModels.LayThongTinBangLuong(iID_MaBangLuong);
            }
            else
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["data"] = data;
            ViewData["iNamLamViec"] = iNamLamViec;
            ViewData["iThangLamViec"] = iThangLamViec;
            return View(sViewPath + "Luong_BangLuong_Edit.aspx");

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaBangLuong)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeLuong, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            Bang bang = new Bang("L_BangLuong");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

            int iThangLamViec = CauHinhLuongModels.LayThangLamViec(User.Identity.Name);
            int iNamLamViec = CauHinhLuongModels.LayNamLamViec(User.Identity.Name);
            String sDanhSachMaDonVi = Request.Form["iID_MaDonVi"] + ",";
            if (arrLoi.Count == 0)
            {
                String iID_MaBangLuongAddNew;
                bang.CmdParams.Parameters.AddWithValue("@iThangBangLuong", iThangLamViec);
                bang.CmdParams.Parameters.AddWithValue("@iNamBangLuong", iNamLamViec);
                bang.CmdParams.Parameters.AddWithValue("@sDanhSachMaDonVi", sDanhSachMaDonVi);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeLuong));
                iID_MaBangLuongAddNew= Convert.ToString(bang.Save());
                if (bang.DuLieuMoi)
                {
                    BangLuongChiTietModels.ThemChiTiet(iID_MaBangLuongAddNew, User.Identity.Name, Request.UserHostAddress, iNamLamViec);//Tinh luong tu danh sach can bo hoac tu thang truoc
                    LuongModels.InsertDuyetBangLuong(iID_MaBangLuongAddNew, "Tạo mới", User.Identity.Name, Request.UserHostAddress);
                }
                //return RedirectToAction("Index", "Luong_BangLuong", new { iNamBangLuong = iNamLamViec, iThangBangLuong = iThangLamViec});
                return RedirectToAction("Index", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuongAddNew});
            }
            else
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                ViewData["DuLieuMoi"] = Convert.ToInt16(bang.DuLieuMoi);
                ViewData["iID_MaBangLuong"] = iID_MaBangLuong;
                ViewData["data"] = dicData["data"];
                return View(sViewPath + "Luong_BangLuong_Edit.aspx");
            }
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaBangLuong, String Detail)
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = BangLuongChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaBangLuong);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            LuongModels.Update_iID_MaTrangThaiDuyet(iID_MaBangLuong, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt bảng lương
            String MaDuyetChungTu = LuongModels.InsertDuyetBangLuong(iID_MaBangLuong, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetBangLuongCuoiCung", MaDuyetChungTu);
            LuongModels.UpdateRecord(iID_MaBangLuong, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            //if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(LuongModels.iID_MaPhanHe, iID_MaTrangThaiDuyet_TrinhDuyet))
            //{
            //    //Khi bảng lương đã duyệt sẽ được đưa vào bảng quyết toán lương
            //    BangLuongChiTietModels.ChuyenBangQuyetToan(iID_MaBangLuong, MaND, IPSua);
            //}
            //else
            //{
            //    //Ngược lại sẽ xóa khỏi bảng quyết toán lương
            //}

            return RedirectToAction(Detail, "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaBangLuong, String Detail)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = BangLuongChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaBangLuong);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }


            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua

            LuongModels.CapNhapLaiTruong_sSua(iID_MaBangLuong);
            ///Update trạng thái cho bảng chứng từ
            LuongModels.Update_iID_MaTrangThaiDuyet(iID_MaBangLuong, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetBangLuong = LuongModels.InsertDuyetBangLuong(iID_MaBangLuong, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetBangLuongCuoiCung", MaDuyetBangLuong);
            LuongModels.UpdateRecord(iID_MaBangLuong, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction(Detail, "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });
        }

        [Authorize]
        public void DeleteLuongChiTiet(String MaBangLuong)
        {
            
        }
        public String obj_CauHinhLuong(String MaND, String iThangLamViec, String iNamLamViec)
        {
            String page = Request.QueryString["page"];
            int CurrentPage = 1;
            SqlCommand cmd;

            if (String.IsNullOrEmpty(page) == false)
            {
                CurrentPage = Convert.ToInt32(page);
            }
            DataTable dt = LuongModels.Get_dtBangLuong(iNamLamViec, iThangLamViec, CurrentPage, Globals.PageSize);
            DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeLuong);
            double nums = LuongModels.Get_CountBangLuong(iNamLamViec, iThangLamViec);
            int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
            String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x, iNamBangLuong = iNamLamViec, iThangBangLuong = iThangLamViec }));

            String stbCB = "<table class=\"mGrid\">";
            stbCB += " <tr> <th style=\"width: 3%;\" align=\"center\">";
            stbCB += " STT";
            stbCB += " </th>";
            stbCB += "<th style=\"width: 15%;\" align=\"center\">";
            stbCB += "Tên bảng lương";
            stbCB += "</th>";
            stbCB += "<th align=\"center\">";
            stbCB += "Đơn vị làm lương";
            stbCB += "</th>";
            stbCB += " <th style=\"width: 15%;\" align=\"center\">";
            stbCB += "Trạng thái duyệt";
            stbCB += "</th>";
            stbCB += " <th style=\"width: 5%;\" align=\"center\">";
            stbCB += "Chi tiết";
            stbCB += "</th>";

            stbCB += "<th style=\"width: 5%;\" align=\"center\">";
            stbCB += "Xóa";
            stbCB += "</th>";
            stbCB += "</tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow R = dt.Rows[i];


                Boolean LuongMoi = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeLuong, Convert.ToInt32(R["iID_MaTrangThaiDuyet"]));

                Boolean DuocThemLuong = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeLuong, MaND);

                String sTrangThai = "";
                String strColor = "";
                for (int j = 0; j < dtTrangThai_All.Rows.Count; j++)
                {
                    if (Convert.ToString(R["iID_MaTrangThaiDuyet"]) == Convert.ToString(dtTrangThai_All.Rows[j]["iID_MaTrangThaiDuyet"]))
                    {
                        sTrangThai = Convert.ToString(dtTrangThai_All.Rows[j]["sTen"]);
                        strColor = String.Format("style='background-color: {0}; background-repeat: repeat;'", dtTrangThai_All.Rows[j]["sMauSac"]);
                        break;
                    }
                }
                DataTable dtDonVi = LuongModels.Get_DSDonViCuaBangLuong(Convert.ToString(R["iID_MaBangLuong"]),
                                                                        Int32.Parse(iNamLamViec));
                String TenDonVi = "";
                for (int j = 0; j < dtDonVi.Rows.Count; j++)
                {
                    if (TenDonVi != "") TenDonVi += ",";
                    TenDonVi += Convert.ToString(dtDonVi.Rows[j]["sTen"]);
                }
                dtDonVi.Dispose();

                String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = R["iID_MaBangLuong"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết bảng lương\"");
                String classtr = "";
                int STT = i + 1;
                if (i % 2 == 0)
                {
                    classtr = "class=\"alt\"";
                }

                stbCB += "<tr" + strColor + ">";
                stbCB += "<td align=\"center\">" + R["rownum"];

                stbCB += "</td>";
                stbCB += "<td align=\"left\"> " + HttpUtility.HtmlEncode(R["sTen"]);
                         //"<a href=\"/Luong_BangLuongChiTiet?iID_MaBangLuong=" + R["iID_MaBangLuong"] + "><b>" + HttpUtility.HtmlEncode(R["sTen"]) + "</b></a>";

                stbCB += "</td>";
                stbCB += "<td align=\"left\">" + HttpUtility.HtmlEncode(TenDonVi);
                stbCB += "</td>";

                stbCB += " <td align=\"center\">" + sTrangThai;

                stbCB += "</td>";
                stbCB += "<td align=\"center\">" + strURL + "</td>";

                stbCB += "<td align=\"center\">";
                if (LuongMoi)
                {
                    stbCB += MyHtmlHelper.ActionLink(Url.Action("Delete", "Luong_BangLuong", new { iID_MaBangLuong = R["iID_MaBangLuong"], iNamBangLuong = R["iNamBangLuong"], iThangBangLuong = R["iThangBangLuong"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                }
                stbCB += "</td></tr>";
            }

            stbCB += "<tr class=\"pgr\">";
            stbCB += "<td colspan=\"9\" align=\"right\">" + strPhanTrang + "</td></tr></table>";
            return stbCB;
        }
        [Authorize]
        public JsonResult UpdateCauHinhNamLamViec(String MaND, String iThangLamViec, String iNamLamViec)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (CommonFunction.IsNumeric(iThangLamViec))
            {
                DK = " iThangLamViec=@iThangLamViec";
                cmd.Parameters.AddWithValue("@iThangLamViec", iThangLamViec);
            }
            if (CommonFunction.IsNumeric(iNamLamViec))
            {
                if (String.IsNullOrEmpty(DK) == false)
                {
                    DK = DK + ",iNamLamViec=@iNamLamViec ";
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                }
                else
                {
                    DK = " iNamLamViec=@iNamLamViec ";
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                }
            }

            if (String.IsNullOrEmpty(DK) == false)
            {
                String SQL = String.Format("UPDATE L_CauHinhLuong SET {0} WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao", DK);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
                Connection.UpdateDatabase(cmd);
            }
            cmd.Dispose();
            String strJ = "";

            strJ = String.Format("Dialog_close(location_reload();)");


            return Json(obj_CauHinhLuong(MaND, iThangLamViec, iNamLamViec), JsonRequestBehavior.AllowGet);
        }
    }
}
