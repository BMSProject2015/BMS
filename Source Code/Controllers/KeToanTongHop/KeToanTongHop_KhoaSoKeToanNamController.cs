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
    public class KeToanTongHop_KhoaSoKeToanNamController : Controller
    {
        //
        // GET: /KeToanTongHop_KhoaSoKeToanNam/
        public string sViewPath = "~/Views/KeToanTongHop/KhoaSoKeToanNam/";
        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "KeToanTongHop_KhoaSoKeToanNam_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ParentID)
        {
            String MaND = User.Identity.Name;
            String iNam = Request.Form[ParentID + "_iNamKhoaSo"];
            Int32 iThang = KeToanTongHop_KhoaSoKeToanModels.CheckThangLonNhatCoPhatSinh(iNam);
            String sCheck = "";

            sCheck = Convert.ToString(KeToanTongHop_KhoaSoKeToanModels.CheckDuyetChungTuGhiSo(iNam, Convert.ToString(iThang)));

            KeToanTongHop_KhoaSoKeToanModels.ThemChiTietKhoaSoNam(iNam, Convert.ToString(iThang), MaND, Request.UserHostAddress);

            return RedirectToAction("Index", "KeToanTongHop_KhoaSoKeToanNam", new { sCheck = sCheck });
        }
    }
}
