using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;

namespace VIETTEL.Controllers.DuToan
{
    public class VayVonController : Controller
    {
        //
        // GET: /Vay No/
        public string sViewPath = "~/Views/VayNo/VayVon/Vay/";
        [Authorize]
        public ActionResult Index(string MaDonVi, string MaNoiDung, string dFromNgayTao, string dToNgayTao, string dFromNgayTra, string dToNgayTra)
        {
            ViewData["dFromNgayTao"] = dFromNgayTao;
            ViewData["dToNgayTao"] = dToNgayTao;
            ViewData["dFromNgayTra"] = dFromNgayTra;
            ViewData["dToNgayTra"] = dToNgayTra;
            ViewData["MaDonVi"] = MaNoiDung;
            ViewData["MaDonVi"] = MaDonVi;
            return View(sViewPath + "VayVon_ChungTu_Index.aspx");
        }

  
        [Authorize]
        public ActionResult Edit(string iID_VayChiTiet)
        {
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTu", "Edit") == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_VayChiTiet))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_VayChiTiet"] = iID_VayChiTiet;
            return View(sViewPath + "VayVon_ChungTu_ThemMoi.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            string iID_VayChiTiet = Convert.ToString(Request.Form[ParentID + "_iID_VayChiTiet"]);
            Guid idVayChiTiet;
            if (iID_VayChiTiet == null || iID_VayChiTiet == string.Empty)
            {
                idVayChiTiet = Guid.NewGuid();
            }
            else
            {
                idVayChiTiet = new Guid(iID_VayChiTiet);
            }

            String iID_Loai = Convert.ToString(Request.Form[ParentID + "_iID_Loai"]);
            String rLaiSuat = Convert.ToString(Request.Form[ParentID + "_rLaiSuat"]);
            String rMienLai = Convert.ToString(Request.Form[ParentID + "_rMienLai"]);
            //String rDuVonCu = Convert.ToString(Request.Form[ParentID + "_rDuVonCu"]);
            //String rDuLaiCu = Convert.ToString(Request.Form[ParentID + "_rDuLaiCu"]);
            String rVayTrongThang = Convert.ToString(Request.Form[ParentID + "_rVayTrongThang"]);

            String dHanPhaiTra = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dHanPhaiTra"]; 
            String rThoiGianThuVon = Convert.ToString(Request.Form[ParentID + "_rThoiGianThuVon"]);
            String dNgayVay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayVay"];
            if (iID_Loai == string.Empty || iID_Loai == "" || iID_Loai == null)
            {
                arrLoi.Add("err_iID_Loai", "Bạn chưa nhập loại vay!");
            }
            if (rLaiSuat == string.Empty || rLaiSuat == "" || rLaiSuat == null)
            {
                arrLoi.Add("err__rLaiSuat", "Bạn chưa nhập lãi suất!");
            }
            if (rVayTrongThang == string.Empty || rVayTrongThang == "" || rVayTrongThang == null)
            {
                arrLoi.Add("err_rVayTrongThang", "Bạn chưa nhập số vay trong tháng!");
            }
            if (dNgayVay == string.Empty || dNgayVay == "" || dNgayVay == null)
            {
                arrLoi.Add("err_dNgayVay", "Bạn chưa nhập ngày vay!");
            }
            if (dHanPhaiTra == string.Empty || dHanPhaiTra == "" || dHanPhaiTra == null)
            {
                arrLoi.Add("err_dHanPhaiTra", "Bạn chưa nhập ngày phải trả!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                return View(sViewPath + "VayVon_Index.aspx");
            }
            else
            {

                Bang bang = new Bang("VN_VayChiTiet");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (iID_VayChiTiet == null || iID_VayChiTiet == "")
                {
                    bang.GiaTriKhoa = Guid.NewGuid();
                }
                else
                    bang.GiaTriKhoa = iID_VayChiTiet;
                bang.Save();
                return View(sViewPath + "VayVon_Index.aspx");

            }
            return RedirectToAction("Index", "VayVon", new { MaNoiDung = string.Empty, TenNoiDung = string.Empty });
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String Thang = Request.Form[ParentID + "_MaThang"];
            String Nam = Request.Form[ParentID + "_MaNam"];
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String TrangThaiChungTu = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            if (TrangThaiChungTu == "-1") TrangThaiChungTu = "";
            return RedirectToAction("Index", "VayVon", new { ParentID = ParentID, Nam = Nam, Thang = Thang, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, TrangThaiChungTu = TrangThaiChungTu });
        }
        [Authorize]
        public ActionResult DeleteNoiDung(String ID)
        {
            int iXoa = 0;
            iXoa = VayNoModels.XoaThongTinNoiDung(ID);
            return RedirectToAction("Index", "VayVon", new { MaNoiDung = string.Empty, TenNoiDung = string.Empty});
        }
        public ActionResult DeleteVayVon(String iID_Vay)
        {
            int iXoa = 0;
            iXoa = VayNoModels.XoaThongTinVayVon(iID_Vay);
            return RedirectToAction("Index", "VayVon", new { MaNoiDung = string.Empty, TenNoiDung = string.Empty });
        }
        public static String GetBQuanLy(String ParentID, String MaDonVi)
        {
            String SQL = String.Format(@"SELECT DT2.sTen FROM VN_DonVi_BQuanLy DT1
                                        JOIN NS_PhongBan DT2 ON DT2.iID_MaPhongBan = DT1.iID_MaPhongBan
                                        WHERE DT1.iID_MaDonVi = @iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String strData = string.Empty;
            StringBuilder builder = new StringBuilder();
            if (dt != null)
            {
                if (dt.Rows.Count != 0)
                {
                    builder.Append("<td class=\"td_form2_td1\">");
                    builder.Append("<div>B quản lý</div></td>");
                    builder.Append("<td class=\"td_form2_td5\">");
                    builder.Append("<div class=\"input1_2\">");
                    builder.Append(dt.Rows[0]["sTen"]);
                    builder.Append("</div>");
                    builder.Append("</td>");
                }
                strData = builder.ToString();

            }
            return strData;
        }
        public JsonResult get_objPhongQuanLy(String ParentID, String MaDonVi)
        {
            return Json(GetBQuanLy(ParentID, MaDonVi), JsonRequestBehavior.AllowGet);
        }

    }
}
