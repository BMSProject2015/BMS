using System;
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

namespace VIETTEL.Controllers.VayNo
{
    public class TraNoController : Controller
    {
        //
        // GET: /TraNo/

        public string sViewPath = "~/Views/VayNo/VayVon/TraNo/";
        [Authorize]
        public ActionResult Index()
        {


            return View(sViewPath + "ThuVon_Index.aspx");
        }
        [Authorize]
        public ActionResult Detail(String iID_VayChiTiet)
        {
            //ViewData["DuLieuMoi"] = "0";
            //if (String.IsNullOrEmpty(iID_VayChiTiet))
            //{
            //    ViewData["DuLieuMoi"] = "1";
            //}
            ViewData["iID_VayChiTiet"] = iID_VayChiTiet;
            ViewData["iID_ThuVonChiTiet"] = "";
            return View(sViewPath + "ThuVon_ThemMoi.aspx");
        }
        /// <summary>
        /// Xem chi tiết vay vốn chi tiết
        /// </summary>
        /// <param name="iID_VayChiTiet"></param>
        /// <param name="iID_ThuVonChiTiet"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Detail_Edit(String iID_VayChiTiet, String iID_ThuVonChiTiet)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_ThuVonChiTiet))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_VayChiTiet"] = iID_VayChiTiet;
            ViewData["iID_ThuVonChiTiet"] = iID_ThuVonChiTiet;
            return View(sViewPath + "ThuVon_ThemMoi.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ThuVonSummit(String ParentID, String iID_VayChiTiet)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "VN_ThuVonChiTiet", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            //String _iID_ThuVonChiTiet = Convert.ToString(Request.Form[ParentID + "_iID_ThuVonChiTiet"]);
            String _iID_ThuVonChiTiet = Convert.ToString(Request.Form[ParentID + "_iID_ThuVonChiTiet"]);
            String dNgayTra = Convert.ToString(Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayTra"]);
            String sNguoiTra = Convert.ToString(Request.Form[ParentID + "_sNguoiTra"]);
            String sCMNDNguoiTra = Convert.ToString(Request.Form[ParentID + "_sCMNDNguoiTra"]);
            String rTraVon = Convert.ToString(Request.Form[ParentID + "_rThuVon"]);
            String rTraLai = Convert.ToString(Request.Form[ParentID + "_rThuLai"]);
            String sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);

            if (dNgayTra == "")
            {
                arrLoi.Add("err_dNgayTra", MessageModels.sNgayTra);
            }
            //if (sNguoiTra == "")
            //{
            //    arrLoi.Add("err_sNguoiTra", MessageModels.sNguoiTra);
            //}
            //if (sCMNDNguoiTra == "")
            //{
            //    arrLoi.Add("err_sCMNDNguoiTra", MessageModels.sSoCMND);
            //}
            if (rTraVon == "" && rTraLai == "")
            {
                arrLoi.Add("err_rTraVon", MessageModels.sVonLai);
            }

            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["sNguoiTra"] = sNguoiTra;
                ViewData["sCMNDNguoiTra"] = sCMNDNguoiTra;
                ViewData["rTraVon"] = rTraVon;
                ViewData["rTraLai"] = rTraLai;
                ViewData["sMoTa"] = sMoTa;
                return View(sViewPath + "ThuVon_ThemMoi.aspx");
            }
            else
            {
                // ViewData["DuLieuMoi"] = "1";
                Bang bang = new Bang("VN_ThuVonChiTiet");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (_iID_ThuVonChiTiet == Convert.ToString(Guid.Empty) || _iID_ThuVonChiTiet == "")
                {
                    bang.CmdParams.Parameters.AddWithValue("@iThang", VayNoModels.ThangChungTuChiTiet(iID_VayChiTiet));
                    bang.CmdParams.Parameters.AddWithValue("@iNam", VayNoModels.NamChungTuChiTiet(iID_VayChiTiet));
                    bang.GiaTriKhoa = Guid.NewGuid();
                }
                else
                    bang.GiaTriKhoa = _iID_ThuVonChiTiet;
                bang.Save();

                //
                //ThuVonModels.UpdateVayNo(iID_VayChiTiet, Convert.ToDateTime(dNgayTra));
                return RedirectToAction("Detail", "TraNo", new { iID_VayChiTiet = iID_VayChiTiet });

            }
        }
        /// <summary>
        /// Tìm kiếm chi tiết vay vốn
        /// </summary>
        /// <param name="iID_VayChiTiet">Mã vay chi tiết</param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String Thang = Request.Form[ParentID + "_" + "MaThang"];
            String Nam = Request.Form[ParentID + "_" + "MaNam"];
            String iID_MaDonVi = Request.Form[ParentID + "_" + "iID_MaDonVi"];
            String iID_MaNoiDung = Request.Form[ParentID + "_" + "iID_MaNoiDung"];
            String dFromNgayTao = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dFromNgayTao"];
            String dToNgayTao = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dToNgayTao"];
            String dFromNgayTra = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dFromNgayTra"];
            String dToNgayTra = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dToNgayTra"];

            return RedirectToAction("Index", "TraNo", new { Thang = Thang, Nam = Thang, MaDonVi = iID_MaDonVi, MaNoiDung = iID_MaNoiDung, dFromNgayTao = dFromNgayTao, dToNgayTao = dToNgayTao, dFromNgayTra = dFromNgayTra, dToNgayTra = dToNgayTra });
        }
        /// <summary>
        /// Xóa chi tiết vay vốn
        /// </summary>
        /// <param name="iID_VayChiTiet">Mã vay chi tiết</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Deleted(String iID_VayChiTiet)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "VN_ThuVonChiTiet", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Xóa dữ liệu trong bảng DT_DotNganSach
            Bang bang = new Bang("VN_ThuVonChiTiet");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.GiaTriKhoa = iID_VayChiTiet;
            bang.Delete();
            return RedirectToAction("Detail", "TraNo", new { iID_VayChiTiet = iID_VayChiTiet });
        }
    }
}
