using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using System.Data.SqlClient;
using System.Collections.Specialized;
using DomainModel.Abstract;
using VIETTEL.Models;

namespace Oneres.Controllers.Luat
{
    public class LuatController : Controller
    {
        public string sViewPath = "~/Views/CacBang/BaoMat/Luat/";
        public Bang bang = new Bang("PQ_Luat");

        [Authorize]
        public ActionResult Index(int? Luat_page)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                dicData["Luat_page"] = Luat_page;
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "List.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Detail(string MaLuat)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.GiaTriKhoa = MaLuat;
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
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                String SQL = "SELECT iID_MaLuat, sTen FROM PQ_Luat ORDER BY sTen";
                DataTable dt = Connection.GetDataTable(SQL);
                DataRow R = dt.NewRow();
                dt.Rows.InsertAt(R, 0);
                //R["iID_MaLuat"] = "-1";
                R["sTen"] = "Tạo mới";
                dicData["slLuat"] = new SelectOptionList(dt, "iID_MaLuat", "sTen");
                NameValueCollection data = (NameValueCollection) dicData["data"];
                dicData["DuLieuMoi"] = "1";
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string MaLuat)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.GiaTriKhoa = MaLuat;
                bang.Delete();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string MaLuat)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.GiaTriKhoa = MaLuat;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                NameValueCollection data = (NameValueCollection) dicData["data"];
                if (dicData != null)
                {
                    dicData["DuLieuMoi"] = "0";
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Edit.aspx");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ControlID)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ControlID, Request.Form);
                if (arrLoi.Count == 0)
                {
                    bang.Save();
                    String MaLuat, MaLuat_Base = "";

                    MaLuat = Convert.ToString(bang.GiaTriKhoa);
                    MaLuat_Base = Request.Form[ControlID + "_iID_MaLuat_Base"];
                    if (String.IsNullOrEmpty(MaLuat_Base) == false && MaLuat_Base != "-1")
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText =
                            "INSERT INTO PQ_Bang_ChucNangCam(iID_MaLuat,sTenBang,sChucNang,sIPSua,sID_MaNguoiDungSua) " +
                            "SELECT @iID_MaLuat,sTenBang,sChucNang,@sIPSua,@sID_MaNguoiDungSua " +
                            "FROM PQ_Bang_ChucNangCam " +
                            "WHERE iID_MaLuat=@iID_MaLuat_Base;";
                        cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
                        cmd.Parameters.AddWithValue("@iID_MaLuat_Base", MaLuat_Base);
                        cmd.Parameters.AddWithValue("@sIPSua", Request.UserHostAddress);
                        cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", User.Identity.Name);
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();
                        cmd = new SqlCommand();
                        cmd.CommandText =
                            "INSERT INTO PQ_Bang_TruongCam(iID_MaLuat,sTenBang,sTenTruong,sIPSua,sID_MaNguoiDungSua) " +
                            "SELECT @iID_MaLuat,sTenBang,sTenTruong,@sIPSua,@sID_MaNguoiDungSua " +
                            "FROM PQ_Bang_TruongCam " +
                            "WHERE iID_MaLuat=@iID_MaLuat_Base;";
                        cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
                        cmd.Parameters.AddWithValue("@iID_MaLuat_Base", MaLuat_Base);
                        cmd.Parameters.AddWithValue("@sIPSua", Request.UserHostAddress);
                        cmd.Parameters.AddWithValue("@sID_MaNguoiDungSua", User.Identity.Name);
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ControlID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Edit.aspx");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
    }
}
