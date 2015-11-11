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
namespace VIETTEL.Controllers
{
    public class TuLieu_DanhMucController : Controller
    {
        //
        // GET: /TuLieu_DanhMuc/
        public string sViewPath = "~/Views/TuLieu/DanhMuc/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_DanhMucTaiLieu", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "Index.aspx");
        }
        /// <summary>
        /// Thêm mục con
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(String iID_MaKieuTaiLieu_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_DanhMucTaiLieu", "Create") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return RedirectToAction("Edit", new { iID_MaKieuTaiLieu_Cha = iID_MaKieuTaiLieu_Cha });
        }
        /// <summary>
        /// Action Thêm mới + Sửa Mục Lục Quân Số
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaKieuTaiLieu, String iID_MaKieuTaiLieu_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_DanhMucTaiLieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaKieuTaiLieu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaKieuTaiLieu"] = iID_MaKieuTaiLieu;
            ViewData["iID_MaKieuTaiLieu_Cha"] = iID_MaKieuTaiLieu_Cha;
            return View(sViewPath + "Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaKieuTaiLieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_DanhMucTaiLieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd;
            NameValueCollection arrLoi = new NameValueCollection();
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String iID_MaKieuTaiLieu_Cha = Convert.ToString(Request.Form[ParentID + "_iID_MaKieuTaiLieu_Cha"]);
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
                if (String.IsNullOrEmpty(iID_MaKieuTaiLieu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaKieuTaiLieu"] = iID_MaKieuTaiLieu;
                ViewData["iID_MaKieuTaiLieu_Cha"] = iID_MaKieuTaiLieu_Cha;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("TL_DanhMucTaiLieu");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);

                String iID_MaKieuTaiLieu_New = "";
                if (DuLieuMoi == "1")
                {
                    if (iID_MaKieuTaiLieu_Cha == null || iID_MaKieuTaiLieu_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaKieuTaiLieu_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    String SQL = "SELECT  MAX(iSTT) AS  iSTT FROM TL_DanhMucTaiLieu WHERE 1=1";
                    cmd = new SqlCommand();
                    if (iID_MaKieuTaiLieu_Cha != null && iID_MaKieuTaiLieu_Cha != "")
                    {
                        SQL += " AND iID_MaKieuTaiLieu_Cha=@iID_MaKieuTaiLieu_Cha";
                        cmd.Parameters.AddWithValue("@iID_MaKieuTaiLieu_Cha", iID_MaKieuTaiLieu_Cha);
                    }
                    cmd.CommandText = SQL;
                    int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                    cmd.Dispose();
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon);
                }
                if (DuLieuMoi == "0")
                {
                    if (iID_MaKieuTaiLieu_Cha == null || iID_MaKieuTaiLieu_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaKieuTaiLieu_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    bang.GiaTriKhoa = iID_MaKieuTaiLieu;
                    iID_MaKieuTaiLieu_New = iID_MaKieuTaiLieu;
                }

                bang.Save();

                return RedirectToAction("Index", new { iID_MaKieuTaiLieu_Cha = iID_MaKieuTaiLieu_Cha });
            }
        }
        /// <summary>
        /// Hiển thị form sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaKieuTaiLieu_Cha"></param>
        /// <param name="iID_MaKieuTaiLieu"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String iID_MaKieuTaiLieu_Cha, String iID_MaKieuTaiLieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_DanhMucTaiLieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(iID_MaKieuTaiLieu_Cha))
            {
                iID_MaKieuTaiLieu_Cha = "";
            }
            ViewData["iID_MaKieuTaiLieu_Cha"] = iID_MaKieuTaiLieu_Cha;
            ViewData["iID_MaKieuTaiLieu"] = iID_MaKieuTaiLieu;
            return View(sViewPath + "Sort.aspx");
        }
        /// <summary>
        /// Sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaKieuTaiLieu_Cha"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaKieuTaiLieu_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_DanhMucTaiLieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            for (i = 0; i < arrTG.Length - 1; i++)
            {
                Bang bang = new Bang("TL_DanhMucTaiLieu");
                bang.GiaTriKhoa = arrTG[i];
                bang.DuLieuMoi = false;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            return RedirectToAction("Index", new { iID_MaKieuTaiLieu_Cha = iID_MaKieuTaiLieu_Cha });
        }
     
        [Authorize]
        public ActionResult Delete(String iID_MaKieuTaiLieu, String iID_MaKieuTaiLieu_Cha)
        {
            //kiểm tra quyền có được phép xóa
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_DanhMucTaiLieu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Boolean bDelete = false;
            bDelete = TuLieuLichSuModels.Delete(iID_MaKieuTaiLieu);
            if (bDelete == true)
            {
                return RedirectToAction("Index", new { iID_MaKieuTaiLieu_Cha = iID_MaKieuTaiLieu_Cha });
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
            String sTaiKhoan = Request.Form[ParentID + "_sTaiKhoan"];
            String sKyHieu = Request.Form[ParentID + "_sKyHieu"];
            return RedirectToAction("Index", "TL_DanhMucTaiLieu", new { ParentID = ParentID, Ten = sTaiKhoan, KyHieu = sKyHieu });
        }
    }
}
