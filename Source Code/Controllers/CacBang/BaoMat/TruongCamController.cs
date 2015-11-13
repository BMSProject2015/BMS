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
namespace Oneres.Controllers.TruongCam
{
    public class TruongCamController : Controller
    {
        public string sViewPath = "~/Views/CacBang/BaoMat/TruongCam/";

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string MaLuat, String TenBang)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["MaLuat"] = MaLuat;
            ViewData["TenBang"] = TenBang;
            return View(sViewPath + "Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ControlID, String MaLuat, String TenBang)
        {
             if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            String tg = Request.Form[ControlID + "_txt"];
            String[] arr = tg.Split(',');
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM PQ_Bang_TruongCam WHERE iID_MaLuat=@iID_MaLuat AND sTenBang=@sTenBang";
            cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
            cmd.Parameters.AddWithValue("@sTenBang", TenBang);
            Connection.UpdateDatabase(cmd);
            int i;
            tg = "";
            for (i = 0; i < arr.Length; i++)
            {
                String[] arr1 = arr[i].Split(';');
                
                if(arr1.Length>1)
                {
                    tg += arr1[0] + BaoMat.KyTuTach;
                }
            }
            if (tg != "")
            {
                Bang bang = new Bang("PQ_Bang_TruongCam");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.CmdParams.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
                bang.CmdParams.Parameters.AddWithValue("@sTenBang", TenBang);
                bang.CmdParams.Parameters.AddWithValue("@sTenTruong", tg);
                bang.Save();
            }
            return RedirectToAction("Detail", "Luat", new { MaLuat = MaLuat });
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
    }
}
