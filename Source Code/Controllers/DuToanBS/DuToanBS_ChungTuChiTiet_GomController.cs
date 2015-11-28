using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DomainModel;
using VIETTEL.Models;

namespace VIETTEL.Controllers.DuToanBS
{
    public class DuToanBS_ChungTuChiTiet_GomController : Controller
    {
        //
        // GET: /DuToanBS_ChungTuChiTiet_Gom/
        public string sViewPath = "~/Views/DuToan/ChungTuChiTiet/";

        [Authorize]
        public ActionResult Index(String iID_MaChungTu, String sLNS, String iID_MaDonVi)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            return View(sViewPath + "ChungTuChiTiet_Gom_Index.aspx");
        }
       
        [Authorize]
        public ActionResult ChungTuChiTiet_Frame(String sLNS, String MaLoai)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTuChiTiet", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String iID_MaChungTu = Request.Form["DuToan_iID_MaChungTu"];
            String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            String[] arrDSTruong = DSTruong.Split(',');
            Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
            }


            MaLoai = Request.Form["DuToan_MaLoai"];
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["MaLoai"] = MaLoai;
            return View(sViewPath + "ChungTuChiTiet_Index_DanhSach_Frame_Gom.aspx", new { sLNS = sLNS });
            //return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu, LoadLai = "1" });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ChiNganSach, String iID_MaChungTu, String sLNS)
        {
            string idAction = Request.Form["idAction"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoi", "DuToanBS_ChungTuChiTiet_Gom", new {  iID_MaChungTu = iID_MaChungTu,sLNS=sLNS });
            }
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyet", "DuToanBS_ChungTuChiTiet_Gom", new {iID_MaChungTu = iID_MaChungTu,sLNS=sLNS });
            }
            return RedirectToAction("ChungTuChiTiet_Frame", new { iID_MaChungTu = iID_MaChungTu,sLNS=sLNS});
        }
    }
}
