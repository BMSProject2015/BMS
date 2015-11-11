using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Controls;
using DomainModel.Abstract;
using DomainModel;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Data;
using VIETTEL.CacBang;
using VIETTEL.Models;
namespace Oneres.Controllers.CacBang.MENU
{
    public class MenuItemController : Controller
    {
       public string sViewPath = "~/Views/CacBang/Menu/MenuItem/";
        public BangMenuItem bang = new BangMenuItem();



        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MENU_MenuItem", "Detail") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "List.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Detail(string MaMenuItem)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MENU_MenuItem", "Detail") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            bang.GiaTriKhoa = MaMenuItem;
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, false);
            if (dicData != null)
            {
                dicData["iID_MaMenuItem"] = MaMenuItem;
               
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
        public ActionResult Create(int MaMenuItemCha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MENU_MenuItem", "Create") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
            NameValueCollection data = (NameValueCollection)dicData["data"];
            data["iID_MaMenuItemCha"] = MaMenuItemCha.ToString();

            dicData["DuLieuMoi"] = "1";
            ViewData[bang.TenBang + "_dicData"] = dicData;
            return View(sViewPath + "Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int MaMenuItem)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MENU_MenuItem", "Delete") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd = new SqlCommand("DELETE FROM MENU_MenuItem WHERE iID_MaMenuItem=@iID_MaMenuItem");
            cmd.Parameters.AddWithValue("@iID_MaMenuItem", MaMenuItem);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return RedirectToAction("Index");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int MaMenuItem)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MENU_MenuItem", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            bang.GiaTriKhoa = MaMenuItem;
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
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
                if (bang.DuLieuMoi)
                {
                    bang.GiaTriKhoa = null;
                    int cs = bang.CmdParams.Parameters.IndexOf("@" + bang.TruongKhoa);
                    if (cs >= 0)
                    {   
                        bang.CmdParams.Parameters.RemoveAt(cs);
                    }
                }
                bang.Save();
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

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(int MaMenuItemCha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MENU_MenuItem", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
            NameValueCollection data = (NameValueCollection)dicData["data"];
            data["iID_MaMenuItemCha"] = MaMenuItemCha.ToString();

            dicData["DuLieuMoi"] = "1";
            ViewData[bang.TenBang + "_dicData"] = dicData;
            return View(sViewPath + "Sort.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String ControlID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MENU_MenuItem", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            SqlCommand cmd;
            for (i = 0; i < arrTG.Length-1; i++)
            {
                cmd = new SqlCommand();
                cmd.CommandText = String.Format("Update MENU_MenuItem SET tThuTu={0} WHERE iID_MaMenuItem=@iID_MaMenuItem", i);
                cmd.Parameters.AddWithValue("@iID_MaMenuItem", arrTG[i]);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            return View(sViewPath + "List.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MenuItem_Cam()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MENU_MenuItem", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "MenuItem_Cam.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MenuItem_CamSubmit(String MaLuat)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "MENU_MenuItem", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String DSMaMenuItem = Request.Form["MenuItem_Cam"];
            
            if (String.IsNullOrEmpty(DSMaMenuItem) == false)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM PQ_MenuItem_Cam WHERE iID_MaLuat=@iID_MaLuat");
                cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
                String[] arr = DSMaMenuItem.Split(',');
                int MaMenuItem;
                for (int i = 0; i < arr.Length; i++)
                {
                    MaMenuItem = Convert.ToInt32(arr[i]);
                    Bang bang = new Bang("PQ_MenuItem_Cam");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaMenuItem", MaMenuItem);
                    bang.Save();
                }
            }
            return RedirectToAction("Index", "Luat", new { MaLuat = MaLuat });
        }
        
    }
}
