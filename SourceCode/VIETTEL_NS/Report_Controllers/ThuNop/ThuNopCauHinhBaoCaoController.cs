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

namespace VIETTEL.Controllers.ThuNop
{
    public class ThuNopCauHinhBaoCaoController : Controller
    {
        //
        // GET: /ThuNop_ChungTu/
        public string sViewPath = "~/Views/ThuNop/";

        [Authorize]
        public ActionResult Index(String iLoai)
        {
            ViewData["iLoai"] = iLoai;
            return View(sViewPath + "CauHinhBaoCao.aspx");
        }

        [Authorize]
        public ActionResult Edit(String iID_MaCot, String iLoai)
        {
            ViewData["iLoai"] = iLoai;
            ViewData["iID_MaCot"] = iID_MaCot;
            return View(sViewPath + "CauHinhBaoCao_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaChungTu, String iLoai)
        {
            String MaND = User.Identity.Name;
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String sLoaiHinh = Convert.ToString(Request.Form["sDeAn"]);
            if (String.IsNullOrEmpty(sLoaiHinh)) sLoaiHinh = "-1";
            String SQL =
                String.Format(
                    @"UPDATE TN_CauHinhBaoCao SET sTen=@sTen,sLNS=@sLoaiHinh WHERE iID_MaBaoCao=@iLoai AND iID_MaCot=@iID_MaCot");
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", sTen);
            cmd.Parameters.AddWithValue("@sLoaiHinh", sLoaiHinh);
            cmd.Parameters.AddWithValue("@iLoai", iLoai);
            cmd.Parameters.AddWithValue("@iID_MaCot", iID_MaChungTu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            return View(sViewPath + "CauHinhBaoCao.aspx");

        }


    }
}
