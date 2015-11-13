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

namespace VIETTEL.Controllers.DotNganSach
{
    public class DuToan_DotNganSachController : Controller
    {
        //
        // GET: /DotNganSach/
        
        public string sViewPath = "~/Views/DuToan/DotNganSach/";
        [Authorize]
        public ActionResult Index()
        {
            //Kiểm tra quyền được xem theo menu. tránh gõ url trực tiếp
            String url = Request.RawUrl;
            if (HamChung.CoQuyenXemTheoMenu(url, User.Identity.Name) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_DotNganSach", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["bThemMoi"] = false;
            return View(sViewPath + "DotNganSach_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddNewSubmit(String ParentID, String ChiNganSach)
        {
            String ThemMoi=Request.Form[ParentID+"_iThemMoi"];
            String MaDotNganSach = Request.Form[ParentID + "_iID_MaDotNganSach1"];
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            if (ThemMoi == "on")
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
                String dNgayDotNganSach = Request.Form[ParentID + "_vidNgayDotNganSach"];
                DateTime d = Convert.ToDateTime(CommonFunction.LayNgayTuXau(dNgayDotNganSach));
                
                Bang bang = new Bang("DT_DotNganSach");
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", NamNganSach);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", NguonNganSach);
                bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", Convert.ToInt32(ChiNganSach));
                bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS + ";");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

                String sGiaTri = NamLamViec + "," + NamNganSach +"," + NguonNganSach+","+ dNgayDotNganSach;
                String TruongDK= "iNamLamViec,iID_MaNamNganSach,iID_MaNguonNganSach,dNgayDotNganSach";

                if (HamChung.Check_Trung("DT_DotNganSach", "iID_MaDotNganSach", Guid.Empty.ToString(),TruongDK, sGiaTri, true))
                {
                    arrLoi.Add("err_dNgayDotNganSach", "Trùng đợt ngân sách");
                }
                if (String.IsNullOrEmpty(sLNS))
                {
                    arrLoi.Add("err_sLNS", "Chưa chọn loại ngân sách");
                }

                if (arrLoi.Count == 0)
                {
                    MaDotNganSach = Convert.ToString(bang.Save());
                }
                else
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    ViewData["bThemMoi"] = true;
                    return View(sViewPath + "DotNganSach_Index.aspx");
                }
            }
            return RedirectToAction("Index", "DuToan_ChungTu", new { MaDotNganSach = MaDotNganSach, sLNS = sLNS});
        }

        public ActionResult Edit(String MaDotNganSach)
        {
            ViewData["MaDotNganSach"] = MaDotNganSach;
            return View(sViewPath + "DotNganSach_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaDotNganSach, String ChiNganSach)
        {
            string sChucNang = "Edit";
            Bang bang = new Bang("DT_DotNganSach");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            String NgayDotNganSach = Convert.ToString(Request.Form[ParentID + "_vidNgayDotNganSach"]);
            String sGiaTri = NamLamViec + "," + NamNganSach + "," + NguonNganSach + "," + NgayDotNganSach;
            String TruongDK = "iNamLamViec,iID_MaNamNganSach,iID_MaNguonNganSach,dNgayDotNganSach";

            if (HamChung.Check_Trung("DT_DotNganSach", "iID_MaDotNganSach", MaDotNganSach, TruongDK, sGiaTri, false))            
            {
                arrLoi.Add("err_dNgayDotNganSach", "Trùng ngày đợt ngân sách!");
            }

            if (NgayDotNganSach == string.Empty || NgayDotNganSach == "" || NgayDotNganSach == null)
            {
                arrLoi.Add("err_dNgayDotNganSach", "Bạn chưa nhập ngày cho đợt ngân sách!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["MaDotNganSach"] = MaDotNganSach;
                return View(sViewPath + "DotNganSach_Edit.aspx");
            }
            else
            {
                DuToan_DotNganSachModels.UpdateDotNganSach(MaDotNganSach, NgayDotNganSach);
            }
            return RedirectToAction("Index", "DuToan_DotNganSach", new { ChiNganSach = ChiNganSach });
        }

        public ActionResult Delete(String MaDotNganSach, String ChiNganSach)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_DotNganSach", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = DuToan_DotNganSachModels.Delete_DotNganSach(MaDotNganSach, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "DuToan_DotNganSach", new {ChiNganSach = ChiNganSach, iXoa = iXoa});
        }

        public JsonResult get_objDotNganSach(String ParentID, String NamLamViec, int bChiNganSach, String MaNguonNamNganSach, String MaNamNganSach, String sLNS)
        {
            return Json(get_NgayDotNganSach(ParentID, NamLamViec, bChiNganSach, MaNguonNamNganSach, MaNamNganSach, sLNS), JsonRequestBehavior.AllowGet);
        }

        public static String get_NgayDotNganSach(String ParentID, String NamLamViec, int bChiNganSach, String MaNguonNamNganSach, String MaNamNganSach, String sLNS)
        {
            String SQL = "SELECT Convert(varchar(10),DT_DotNganSach.dNgayDotNganSach,103) as dNgayDotNganSachHT, DT_DotNganSach.* " +
                          "FROM DT_DotNganSach " +
                          "WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND " +
                                  "bChiNganSach = @bChiNganSach AND " +
                                  "iID_MaNguonNganSach=@iID_MaNguonNganSach AND " +
                                  "iID_MaNamNganSach=@iID_MaNamNganSach AND " +
                                  "sDSLNS LIKE @sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@bChiNganSach", bChiNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", MaNguonNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", MaNamNganSach);
            cmd.Parameters.AddWithValue("@sLNS", String.Format("%{0};%", sLNS));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String strData = string.Empty;
            StringBuilder builder = new StringBuilder();
            if (dt != null)
            {
                builder.Append("<table class='mGrid'>");
                builder.Append("<tr>");
                builder.Append("<th style=\"width: 3%;\" align=\"center\">STT</th>");
                builder.Append("<th style=\"width: 20%;\" align=\"center\">Ngày đợt ngân sách</th>");
                builder.Append("<th style=\"width: 17%;\" align=\"center\">Năm làm việc</th>");
                builder.Append("<th style=\"width: 20%;\" align=\"center\">Năm ngân sách</th>");
                builder.Append("<th style=\"width: 20%;\" align=\"center\">Nguồn ngân sách</th>");
                builder.Append("<th style=\"width: 10%;\" align=\"center\">Người tạo</th>");
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
                    
                    String TenNamNganSach = "", TenNguonNganSach = "";
                    DataTable dtNamNganSach = DanhMucModels.NS_NamNganSach();
                    int j;
                    for (j = 0; j < dtNamNganSach.Rows.Count; j++) {
                        if (Convert.ToString(R["iID_MaNamNganSach"]) == Convert.ToString(dtNamNganSach.Rows[j]["iID_MaNamNganSach"]))
                        {
                            TenNamNganSach = Convert.ToString(dtNamNganSach.Rows[j]["sTen"]);
                            break;
                        }
                    }
                    DataTable dtNguonNganSach = DanhMucModels.NS_NguonNganSach();
                    for (j = 0; j < dtNguonNganSach.Rows.Count; j++)
                    {
                        if (Convert.ToString(R["iID_MaNguonNganSach"]) == Convert.ToString(dtNguonNganSach.Rows[j]["iID_MaNguonNganSach"]))
                        {
                            TenNguonNganSach = Convert.ToString(dtNguonNganSach.Rows[j]["sTen"]);
                            break;
                        }
                    }
                    String urlDetail = "/DuToan_ChungTu/Index?ChiNganSach=" + bChiNganSach + "&MaDotNganSach=" + R["iID_MaDotNganSach"]+ "&sLNS=" + sLNS;
                    String urlEdit = "/DuToan_DotNganSach/Edit?ChiNganSach="+ bChiNganSach +"&MaDotNganSach=" + R["iID_MaDotNganSach"];
                    String urlDelete = "/DuToan_DotNganSach/Delete?ChiNganSach=" + bChiNganSach + "&MaDotNganSach=" + R["iID_MaDotNganSach"];

                    builder.Append("<tr "+ classtr +">");
                    builder.Append("<td align=\"center\">");
                    builder.Append("" + STT + "");
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\"><b>{1}</b></a>", urlDetail, R["dNgayDotNganSachHT"]));
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append("" + R["iNamLamViec"] + "");
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append("" + TenNamNganSach + "");
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append("" + TenNguonNganSach + "");
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append("" + R["sID_MaNguoiDungTao"] + "");
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\">{1}</a>", urlEdit, "<img src='../Content/Themes/images/edit.gif' alt='' />"));
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a onclick=\"javascript:return confirm('Bạn có chắc chắn muốn xóa không?');\" href=\"{0}\">{1}</a>", urlDelete, "<img src='../Content/Themes/images/delete.gif' alt='' />"));
                    builder.Append("</td>");
                    builder.Append("</tr>");
                }
                builder.Append("</table>");
                strData = builder.ToString();
            }
            return strData;
        }        
    }
}
