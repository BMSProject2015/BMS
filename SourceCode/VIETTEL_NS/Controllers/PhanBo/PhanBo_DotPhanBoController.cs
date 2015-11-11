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
    public class PhanBo_DotPhanBoController : Controller
    {
        //
        // GET: /PhanBo_DotPhanBo/
        public string sViewPath = "~/Views/PhanBo/DotPhanBo/";
        [Authorize]
        public ActionResult Index()
        {
            //Kiểm tra quyền được xem theo menu. tránh gõ url trực tiếp
            //String url = Request.RawUrl;
            //if (HamChung.CoQuyenXemTheoMenu(url, User.Identity.Name) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "PB_DotPhanBo", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "DotPhanBo_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddNewSubmit(String ParentID, String ChiNganSach)
        {
            String MaDotPhanBo = "";
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
            String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            Bang bang = new Bang("PB_DotPhanBo");
            bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", NamNganSach);
            bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", NguonNganSach);
            bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", Convert.ToInt32(ChiNganSach));
            bang.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS + ";");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);
            
            String NgayDotPhanBo = Convert.ToString(Request.Form[ParentID + "_vidNgayDotPhanBo"]);

            if (HamChung.isDate(NgayDotPhanBo)==false)
            {
                arrLoi.Add("err_dNgayDotPhanBo", "Ngày không đúng");
            }
            DateTime d = Convert.ToDateTime(CommonFunction.LayNgayTuXau(NgayDotPhanBo));

            if (NgayDotPhanBo == string.Empty || NgayDotPhanBo == "" || NgayDotPhanBo == null)
            {
                arrLoi.Add("err_dNgayDotPhanBo", "Bạn chưa nhập ngày cho đợt phân bổ!");
            }
            if (HamChung.Check_Trung("PB_DotPhanBo", "iID_MaDotPhanBo", Guid.Empty.ToString(), "dNgayDotPhanBo,bDotPhanBoTong", NgayDotPhanBo+",0", true))
            {
                arrLoi.Add("err_dNgayDotPhanBo", "Trùng đợt phân bổ");
            }

            

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["MaDotPhanBo"] = MaDotPhanBo;
                return View(sViewPath + "DotPhanBo_Index.aspx");
            }
            else
            {
                MaDotPhanBo = Convert.ToString(bang.Save());
                return RedirectToAction("Index", "PhanBo_DotPhanBo", new { MaDotPhanBo = MaDotPhanBo, sLNS = sLNS });
            }
           
        }

        public ActionResult Edit(String MaDotPhanBo)
        {
            ViewData["MaDotPhanBo"] = MaDotPhanBo;
            return View(sViewPath + "DotPhanBo_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaDotPhanBo, String ChiNganSach)
        {
            string sChucNang = "Edit";
            Bang bang = new Bang("PB_DotPhanBo");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();

            String NgayDotPhanBo = Convert.ToString(Request.Form[ParentID + "_vidNgayDotPhanBo"]);
            DateTime d = Convert.ToDateTime(CommonFunction.LayNgayTuXau(NgayDotPhanBo));
                
            if (NgayDotPhanBo == string.Empty || NgayDotPhanBo == "" || NgayDotPhanBo == null)
            {
                arrLoi.Add("err_dNgayDotPhanBo", "Bạn chưa nhập ngày cho đợt phân bổ!");
            }
            if (HamChung.Check_Trung("PB_DotPhanBo", "iID_MaDotPhanBo", MaDotPhanBo, "dNgayDotPhanBo", NgayDotPhanBo, false))
            {
                arrLoi.Add("err_dNgayDotPhanBo", "Trùng đợt phân bổ");
            }

            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["MaDotPhanBo"] = MaDotPhanBo;
                return View(sViewPath + "DotPhanBo_Edit.aspx");
            }
            else
            {
                PhanBo_DotPhanBoModels.UpdateDotPhanBo(MaDotPhanBo, NgayDotPhanBo);
            }
            return RedirectToAction("Index", "PhanBo_DotPhanBo", new { ChiNganSach = ChiNganSach });
        }

        public ActionResult Delete(String MaDotPhanBo, String ChiNganSach)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "PB_DotPhanBo", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            int MaTrangThaiDaDuyet=LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo);
            if (HamChung.Checked_Delete("PB_ChiTieuChiTiet", "iID_MaDotPhanBo,iID_MaTrangThaiDuyet", MaDotPhanBo + "," + MaTrangThaiDaDuyet.ToString())==false
            || HamChung.Checked_Delete("PB_PhanBoChiTiet", "iID_MaDotPhanBo,iID_MaTrangThaiDuyet", MaDotPhanBo + "," + MaTrangThaiDaDuyet.ToString())==false)
            {
                iXoa = -1;
            }
            else
            {
                iXoa = PhanBo_DotPhanBoModels.Delete_DotPhanBo(MaDotPhanBo, Request.UserHostAddress, User.Identity.Name);
            }
            return RedirectToAction("Index", "PhanBo_DotPhanBo", new { ChiNganSach = ChiNganSach,iXoa=iXoa });
        }

        public JsonResult get_objDotNganSach(String ParentID, String NamLamViec, int bChiNganSach, String MaNguonNamNganSach, String MaNamNganSach, String sLNS)
        {
            return Json(get_NgayDotPhanBo(ParentID, NamLamViec, bChiNganSach, MaNguonNamNganSach, MaNamNganSach, sLNS), JsonRequestBehavior.AllowGet);
        }

        public static String get_NgayDotPhanBo(String ParentID, String NamLamViec, int bChiNganSach, String MaNguonNamNganSach, String MaNamNganSach, String sLNS)
        {
            String SQL = "SELECT Convert(varchar(10),PB_DotPhanBo.dNgayDotPhanBo,103) as dNgayDotPhanBoHT, PB_DotPhanBo.* " +
                          "FROM PB_DotPhanBo " +
                          "WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND " +
                                  "bChiNganSach = @bChiNganSach AND " +
                                  "iID_MaNguonNganSach=@iID_MaNguonNganSach AND " +
                                  "iID_MaNamNganSach=@iID_MaNamNganSach " +
                                  " ORDER BY dNgayDotPhanBo ";
                                  //"sDSLNS LIKE @sLNS";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@bChiNganSach", bChiNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", MaNguonNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", MaNamNganSach);
            //cmd.Parameters.AddWithValue("@sLNS", String.Format("%{0};%", sLNS));
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
                    for (j = 0; j < dtNamNganSach.Rows.Count; j++)
                    {
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
                    String urlDetail = "/PhanBo_ChiTieu/Index?MaDotPhanBo=" + R["iID_MaDotPhanBo"];
                    String urlEdit = "/PhanBo_DotPhanBo/Edit?MaDotPhanBo=" + R["iID_MaDotPhanBo"];
                    String urlDelete = MyHtmlHelper.ActionLink("/PhanBo_DotPhanBo/Delete?MaDotPhanBo=" + R["iID_MaDotPhanBo"], "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete","");// "/PhanBo_DotPhanBo/Delete?MaDotPhanBo=" + R["iID_MaDotPhanBo"];

                    builder.Append("<tr " + classtr + ">");
                    builder.Append("<td align=\"center\">");
                    builder.Append("" + STT + "");
                    builder.Append("</td>");
                    builder.Append("<td align=\"center\">");
                    builder.Append(String.Format("<a href=\"{0}\"><b>{1}</b></a>", urlDetail, R["dNgayDotPhanBoHT"]));
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
                    builder.Append(String.Format("{0}", urlDelete));
                    builder.Append("</td>");
                    builder.Append("</tr>");
                }
                builder.Append("</table>");
                strData = builder.ToString();
            }
            dt.Dispose();
            return strData;
        }      
    }
}
