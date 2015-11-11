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

namespace VIETTEL.Controllers.CongSan
{
    public class KTCS_TinhKhauHaoController : Controller
    {
        //
        // GET: /KTCS_TinhKhauHao/
        public string sViewPath = "~/Views/CongSan/TinhKhauHao/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_KhauHaoHangNam", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KTCS_KhauHaoTaiSan_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String ParentID)
        {
            String MaND = User.Identity.Name;
            String iNam = Request.Form[ParentID + "_iNamKhauHao"];
            String SQL;
            SqlCommand cmd;

            //Xóa bảng KTCS_KhauHaoHangNam
            cmd = new SqlCommand("DELETE KTCS_KhauHaoHangNam WHERE iID_MaTaiSan In (SELECT iID_MaTaiSan FROM KTCS_TaiSan WHERE iTrangThai=1 AND bDaKhauHao = 1 AND dNgayDuaVaoKhauHao is not null)");
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            SQL = String.Format("SELECT * FROM KTCS_TaiSan WHERE iTrangThai=1 AND bDaKhauHao = 1 AND dNgayDuaVaoKhauHao is not null AND iID_MaTaiSan IN(SELECT iID_MaTaiSan FROM KTCS_ChungTuChiTiet WHERE iTrangThai=1) ORDER BY dNgayTao");
            cmd = new SqlCommand(SQL);
            DataTable dtTaiSan = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dtTaiSan.Rows.Count > 0) {
                for (int i = 0; i < dtTaiSan.Rows.Count; i++) {
                    KTCS_KhauHaoModels.TinhKhauHaoTaiSan(Convert.ToString(dtTaiSan.Rows[i]["iID_MaTaiSan"]), Convert.ToInt32(iNam), User.Identity.Name, Request.UserHostAddress);
                }
            }

            return RedirectToAction("Index", "KTCS_TinhKhauHao", new { iNam = iNam});
        }
        [Authorize]
        public ActionResult Detail()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_KhauHaoHangNam", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KTCS_KhauHaoTaiSan_Detail.aspx");
        }
    }
}
