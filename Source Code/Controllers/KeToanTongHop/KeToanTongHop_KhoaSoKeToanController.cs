using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;

namespace VIETTEL.Controllers.KeToanTongHop
{
    public class KeToanTongHop_KhoaSoKeToanController : Controller
    {
        //
        // GET: /KeToanTongHop_KhoaSoKeToan/
        public string sViewPath = "~/Views/KeToanTongHop/KhoaSoKeToan/";
        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "KeToanTongHop_KhoaSoKeToan_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ParentID, String iNam)
        {
            String MaND = User.Identity.Name;
            String iThang = Request.Form[ParentID + "_iThangKhoaSo"];
            String sCheck = "", sCheckNam = "";
            for (int i = 0; i <= Convert.ToInt32(iThang); i++)
            {
                sCheck = Convert.ToString(KeToanTongHop_KhoaSoKeToanModels.CheckDuyetChungTuGhiSo(iNam, Convert.ToString(i)));

                KeToanTongHop_KhoaSoKeToanModels.ThemChiTiet(iNam, Convert.ToString(i), MaND, Request.UserHostAddress);


            }
            if (Convert.ToInt32(iThang) == 12)
            {
                sCheckNam = Convert.ToString(KeToanTongHop_KhoaSoKeToanModels.CheckDuyetChungTuGhiSo(iNam, Convert.ToString(iThang)));
                KeToanTongHop_KhoaSoKeToanModels.ThemChiTietKhoaSoNam(iNam, Convert.ToString(iThang), MaND, Request.UserHostAddress);
            }
            return RedirectToAction("Index", "KeToanTongHop_KhoaSoKeToan", new { iThang = iThang, sCheck = sCheck });
        }
        [Authorize]
        public ActionResult BangCanDoiTaiKhoan()
        {
            return View(sViewPath + "KeToanTongHop_BangCanDoiKeToan_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String iNam)
        {
            String MaND = User.Identity.Name;
            String iThang = Request.Form[ParentID + "_iThangKhoaSo"];

            return RedirectToAction("BangCanDoiTaiKhoan", "KeToanTongHop_KhoaSoKeToan", new { iThang = iThang });
        }
    }
}
