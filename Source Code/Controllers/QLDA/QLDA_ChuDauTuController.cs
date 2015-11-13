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
    public class QLDA_ChuDauTuController : Controller
    {
        //
        // GET: /QLDA_ChuDauTu/
        public string sViewPath = "~/Views/QLDA/DanhMuc/ChuDauTu/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // bool flag = CheckChuDauTuExist("sTen", "Công ty Cổ phần Hương Giang");
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_ChuDauTu", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "QLDA_ChuDauTu_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

          
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String sMaChuDauTu = Request.Form[ParentID + "_sMaChuDauTu"];
            String sTenChuDauTu = Request.Form[ParentID + "_sTenChuDauTu"];

            return RedirectToAction("Index", "QLDA_ChuDauTu", new { sMaChuDauTu = sMaChuDauTu, sTenChuDauTu = sTenChuDauTu });
        }
        [Authorize]
        public ActionResult Edit(String iID_MaChuDauTu)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChuDauTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaChuDauTu"] = iID_MaChuDauTu;
            DataTable dtChuDauTu = QLDA_ChuDauTuModels.Get_Row_ChuDauTuByID(iID_MaChuDauTu);
            if(dtChuDauTu != null && dtChuDauTu.Rows.Count >0)
            {
                ViewData["sMa"] = Convert.ToString(dtChuDauTu.Rows[0]["sMa"]);
            }
            return View(sViewPath + "QLDA_ChuDauTu_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_ChuDauTu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String sMaChuDauTu = Convert.ToString(Request.Form[ParentID + "_iID_MaChuDauTu"]);
            if(Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sMaChuDauTu = Convert.ToString(Guid.NewGuid());
            }
            String sMa = Convert.ToString(Request.Form[ParentID + "_sMa"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String sTenVietTat = Convert.ToString(Request.Form[ParentID + "_sTenVietTat"]);
            String sSoTaiKhoan = Convert.ToString(Request.Form[ParentID + "_sSoTaiKhoan"]);
            DataTable dtExist;
            string sMaChuDauTuExist = "";
            string sMaExist = "";
            String sTenDonVi = "";
            if (sMa == "" && String.IsNullOrEmpty(sMa) == true)
            {
                arrLoi.Add("err_iID_MaChuDauTu", "Bạn phải chọn nhập mã chủ đầu tư!");
            }
            if (sTen == "" && String.IsNullOrEmpty(sTen) == true)
            {
                arrLoi.Add("err_sTen", "Bạn phải nhập tên chủ đầu tư!");
            }
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                arrLoi.Add("err_iID_MaDonVi", "Bạn phải chọn đơn vị!");
            }
            else
            {
                 sTenDonVi =Convert.ToString(CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen"));
            }

            if (sMaChuDauTuExist == "")
            {
                dtExist = CheckChuDauTuExist("sMa", sMa, Request.Form[ParentID + "_DuLieuMoi"], sMa, sMaChuDauTu);//CheckExistMaTrung(sMa);
                if (dtExist != null && dtExist.Rows.Count > 0)
                {
                    arrLoi.Add("err_iID_MaChuDauTu", "Mã chủ đầu tư đã tồn tại!");
                    sMaChuDauTuExist = dtExist.Rows[0]["iID_MaChuDauTu"].ToString();
                    sMaExist = dtExist.Rows[0]["sMa"].ToString();
                }
            }
            if (sMaChuDauTuExist == "")
            {
                dtExist = CheckChuDauTuExist("sTen", sTen, Request.Form[ParentID + "_DuLieuMoi"], sMa,sMaChuDauTu);
                if (dtExist != null && dtExist.Rows.Count > 0)
                {
                    arrLoi.Add("err_sTen", "Tên chủ đầu tư đã tồn tại!");
                    sMaChuDauTuExist = dtExist.Rows[0]["iID_MaChuDauTu"].ToString();
                    sMaExist = dtExist.Rows[0]["sMa"].ToString();
                }
            }
            if (sMaChuDauTuExist != "")
            {
                sMaChuDauTu = sMaChuDauTuExist;
                sMa = sMaExist;
            }
            //}
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaChuDauTu"] = sMaChuDauTu;
                ViewData["sMa"] = sMa;
                ViewData["DuLieuMoi"] = "0";
                return View(sViewPath + "QLDA_ChuDauTu_Edit.aspx");
            }
            else
            {

                Bang bang = new Bang("QLDA_ChuDauTu");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.GiaTriKhoa = sMaChuDauTu;
                bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
                bang.Save();
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Delete(String iID_MaChuDauTu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QLDA_ChuDauTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("QLDA_ChuDauTu");
            bang.GiaTriKhoa = iID_MaChuDauTu;
            bang.Delete();
            return View(sViewPath + "QLDA_ChuDauTu_Index.aspx");
        }
        public Boolean CheckMaTrung(String iID_MaChuDauTu)
        {
            Boolean vR = false;
            DataTable dt = QLDA_ChuDauTuModels.Get_Row_ChuDauTu(iID_MaChuDauTu);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            if (dt != null) dt.Dispose();
            return vR;
        }
        /// <summary>
        /// kiem tra ma chu dau tu, kq tra ve table
        /// </summary>
        /// <param name="sMa"></param>
        /// <returns></returns>
        public DataTable CheckExistMaTrung(String sMa)
        {
            DataTable dt = QLDA_ChuDauTuModels.Get_Row_ChuDauTu(sMa);
            return dt;
        }
        public JsonResult get_MaChuDauTu(String iID_MaChuDauTu)
        {
            return Json(get_objCheckMaTrung(iID_MaChuDauTu), JsonRequestBehavior.AllowGet);
        }

        public static String get_objCheckMaTrung(String iID_MaChuDauTu)
        {
            String strMess = "";
            Boolean vR = false;
            DataTable dt = QLDA_ChuDauTuModels.Get_Row_ChuDauTu(iID_MaChuDauTu);
            if (dt.Rows.Count > 0)
            {
                strMess = "Mã chủ đầu tư đã tồn tại!";
            }
            else
            {
                strMess = "Mã này có thể thêm được!";
            }
            return strMess;
        }

        /// <summary>
        /// kiem tra trung chu dau tu
        /// </summary>
        /// <param name="sfieldName">ten truong tim kiem</param>
        /// <param name="sSearchValue">gia tri</param>
        /// <returns></returns>
        public DataTable CheckChuDauTuExist(String sfieldName, string sSearchValue, string sEdit, string sMaChuDauTu,string iID)
        {
            Boolean vR = false;
            DataTable dt = QLDA_ChuDauTuModels.Get_Row_ChuDauTu_CheckExistByMaAndID(sfieldName, sSearchValue, sEdit, sMaChuDauTu, iID);
            return dt;
        }

        /// <summary>
        /// check thong tin cua chu dau tu co ton tai khong
        /// </summary>
        /// <param name="sfieldName">ten truong kiem tra</param>
        /// <param name="sSearchValue">gia tri</param>
        /// <param name="sEdit">0: Edit; 1: them moi    </param>
        /// <param name="sMaChuDauTu">ma chu dau tu</param>
        /// <param name="sID">ID chu dau tu</param>
        /// <returns></returns>
        public JsonResult get_ChuDauTuExist(String sfieldName, string sSearchValue, string sEdit, string sMaChuDauTu,string sID)
        {
            return Json(get_objCheckExist(sfieldName, sSearchValue, sEdit, sMaChuDauTu,sID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// quangvv 06/09/2012
        /// Ham kiểm tra trùng lặp 1 trong các dữ liệu sau: mã, tên, tên viết tắt, số tài khoản
        /// </summary>
        /// <param name="sfieldName">ten truong tim kiem</param>
        /// <param name="sSearchValue">gia tri tim kiem</param>
        /// <returns></returns>
        public static String get_objCheckExist(String sfieldName, string sSearchValue, string sEdit, string sMaChuDauTu,string sID)
        {
            String strMess = "";
            Boolean vR = false;
            DataTable dt = QLDA_ChuDauTuModels.Get_Row_ChuDauTu_CheckExistByMaAndID(sfieldName, sSearchValue.Trim(), sEdit, sMaChuDauTu, sID);
            if (dt != null)
            {
                strMess = dt.Rows.Count > 0 ? "1" : "0";
            }
            else
            {
                strMess = "0";
            }
            return strMess;
        }

    }
}
