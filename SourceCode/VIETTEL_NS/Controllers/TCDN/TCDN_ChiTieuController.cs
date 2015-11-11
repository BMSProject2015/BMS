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

namespace VIETTEL.Controllers.TCDN
{
    public class TCDN_ChiTieuController : Controller
    {
        //
        // GET: /TCDN_ChiTieu/
        public string sViewPath = "~/Views/TCDN/ChiTieu/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_ChiTieu", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "TCDN_ChiTieu_Index.aspx");
        }
        /// <summary>
        /// Thêm mục con
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(String iID_MaChiTieu_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return RedirectToAction("Edit", new { iID_MaChiTieu_Cha = iID_MaChiTieu_Cha });
        }
        /// <summary>
        /// Action Thêm mới + Sửa Mục Lục Quân Số
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaChiTieu, String iID_MaChiTieu_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChiTieu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaChiTieu"] = iID_MaChiTieu;
            ViewData["iID_MaChiTieu_Cha"] = iID_MaChiTieu_Cha;
            return View(sViewPath + "TCDN_ChiTieu_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaChiTieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd;
            NameValueCollection arrLoi = new NameValueCollection();
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String iID_MaChiTieu_Cha = Convert.ToString(Request.Form[ParentID + "_iID_MaChiTieu_Cha"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            if (sTen == string.Empty || sTen == "")
            {
                arrLoi.Add("err_sTen", MessageModels.sTen);
            }            
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaChiTieu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaChiTieu"] = iID_MaChiTieu;
                ViewData["iID_MaChiTieu_Cha"] = iID_MaChiTieu_Cha;
                return View(sViewPath + "TCDN_ChiTieu_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("TCDN_ChiTieu");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);

                if (DuLieuMoi == "1")
                {
                    if (iID_MaChiTieu_Cha == null || iID_MaChiTieu_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaChiTieu_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    string SQL = "SELECT  MAX(iSTT) AS  iSTT FROM TCDN_ChiTieu WHERE 1=1";
                    cmd = new SqlCommand();
                    if (iID_MaChiTieu_Cha != null && iID_MaChiTieu_Cha != "")
                    {
                        SQL += " AND iID_MaChiTieu_Cha=@iID_MaChiTieu_Cha";
                        cmd.Parameters.AddWithValue("@iID_MaChiTieu_Cha", iID_MaChiTieu_Cha);
                    }
                    cmd.CommandText = SQL;
                    int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                    cmd.Dispose();
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon);
                }
                if (DuLieuMoi == "0")
                {
                    if (iID_MaChiTieu_Cha == null || iID_MaChiTieu_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaChiTieu_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    bang.GiaTriKhoa = iID_MaChiTieu;
                }
                bang.Save();
                return RedirectToAction("Index", new { iID_MaChiTieu_Cha = iID_MaChiTieu_Cha });
            }
        }
        /// <summary>
        /// Hiển thị form sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaChiTieu_Cha"></param>
        /// <param name="iID_MaChiTieu"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String iID_MaChiTieu_Cha, String iID_MaChiTieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(iID_MaChiTieu_Cha))
            {
                iID_MaChiTieu_Cha = "";
            }
            ViewData["iID_MaChiTieu_Cha"] = iID_MaChiTieu_Cha;
            ViewData["iID_MaChiTieu"] = iID_MaChiTieu;
            return View(sViewPath + "TCDN_ChiTieu_Sort.aspx");
        }
        /// <summary>
        /// Sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaChiTieu_Cha"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaChiTieu_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            for (i = 0; i < arrTG.Length - 1; i++)
            {
                Bang bang = new Bang("TCDN_ChiTieu");
                bang.GiaTriKhoa = arrTG[i];
                bang.DuLieuMoi = false;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            return RedirectToAction("Index", new { iID_MaChiTieu_Cha = iID_MaChiTieu_Cha });
        }
        /// <summary>
        /// Lệnh điều hướng xóa tài khoản kế toán
        /// </summary>
        /// <param name="iID_MaChiTieu">Mã tài khoản</param>
        /// <param name="iID_MaChiTieu_Cha">Mã tài khoản cấp cha</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaChiTieu, String iID_MaChiTieu_Cha)
        {
            //kiểm tra quyền có được phép xóa
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_ChiTieu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Boolean bDelete = false;
            bDelete = TCDN_ChiTieuModels.Delete(iID_MaChiTieu);
            if (bDelete == true)
            {
                return RedirectToAction("Index", new { iID_MaChiTieu_Cha = iID_MaChiTieu_Cha });
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Tìm kiếm loại tài khoản
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String sTenChiTieu = Request.Form[ParentID + "_sTenChiTieu"];
            String sKyHieu = Request.Form[ParentID + "_sKyHieu"];
            return RedirectToAction("Index", "TCDN_ChiTieu", new { ParentID = ParentID, sTenChiTieu = sTenChiTieu, KyHieu = sKyHieu });
        }

        public Boolean CheckMaTaiKhoan(String sKyHieu)
        {
            Boolean vR = false;
            int iSoDuLieu = TCDN_ChiTieuModels.Get_So_KyHieuChiTieu(sKyHieu);
            if (iSoDuLieu > 0)
            {
                vR = true;
            }
            return vR;
        }
    }
}
