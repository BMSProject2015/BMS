using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;

namespace VIETTEL.Controllers.DuToan
{
    public class VayNo_NoiDungController : Controller
    {
        //
        // GET: /Vay No/
        public string sViewPath = "~/Views/VayNo/";
        [Authorize]
        public ActionResult Index(string MaNoiDung, string TenNoiDung)
        {
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false) {
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            ViewData["iID_MaNoiDung"] = MaNoiDung;
            ViewData["sTenNoiDung"] = TenNoiDung;
            return View(sViewPath + "VayNo_NoiDung_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmitNoiDung(String ParentID, String MaNoiDung, String TenNoiDung)
        {
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            String iID_MaNoiDung = Request.Form[ParentID + "_" + "iID_MaNoiDung"];
            String sTenNoiDung = Request.Form[ParentID + "_" + "sTenNoiDung"];
            return RedirectToAction("Index", "VayNo_NoiDung", new { MaNoiDung = iID_MaNoiDung, TenNoiDung = sTenNoiDung });
        }

        [Authorize]
        public ActionResult EditNoiDung(string ID)
        {
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTu", "Edit") == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(ID))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["ID"] = ID;
            return View(sViewPath + "VayNo_NoiDung_ThemMoi.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmitNoiDung(String ParentID)
        {
            string sChucNang = "Edit";

            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DC_DanhMucNoiDung", sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            string ID = Convert.ToString(Request.Form[ParentID + "_ID"]);
            String iID_MaNoiDung = Convert.ToString(Request.Form[ParentID + "_iID_MaNoiDung"]);
            String sTenNoiDung = Convert.ToString(Request.Form[ParentID + "_sTenNoiDung"]);
            String sMoTaChung = Convert.ToString(Request.Form[ParentID + "_sMoTaChung"]);


            if (sChucNang == "Create" && (iID_MaNoiDung == string.Empty || iID_MaNoiDung == "" || iID_MaNoiDung == null))
            {
                arrLoi.Add("err_iID_MaNoiDung", "Bạn phải nhập mã nội dung!");
            }
            if (sChucNang == "Create" && iID_MaNoiDung != "" && checkMa(iID_MaNoiDung))
            {
                arrLoi.Add("err_iID_MaNoiDung", "Mã nội dung đã tồn tại!");
            }
            if (sTenNoiDung == string.Empty || sTenNoiDung == "" || sTenNoiDung == null)
            {
                arrLoi.Add("err_sTenNoiDung", "Bạn chưa nhập tên nội dung!");
            }

            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaNoiDung"] = iID_MaNoiDung;
                ViewData["sTenNoiDung"] = sTenNoiDung;

                ViewData["sMoTaChung"] = sMoTaChung;
                return RedirectToAction("EditNoiDung", "VayNo_NoiDung");
            }
            else
            {

                Bang bang = new Bang("DC_DanhMucNoiDung");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (ID != "")
                    bang.GiaTriKhoa = ID;
                else bang.GiaTriKhoa = iID_MaNoiDung;
                bang.Save();
                return RedirectToAction("EditNoiDung", "VayNo_NoiDung");
            }


        }
        /// <summary>
        /// Kiểm tra mã nội dung có bị trùng không
        /// </summary>
        /// <param name="MaID"></param>
        /// <returns></returns>
        private Boolean checkMa(string MaID)
        {
            Boolean vR = false;
            string sql = "SELECT iID_MaNoiDung FROM DC_DanhMucNoiDung WHERE iID_MaNoiDung=@iID_MaNoiDung";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaNoiDung", MaID);
            cmd.CommandText = sql;
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0) vR = true;
            cmd.Dispose();
            if (dt != null) dt.Dispose();
            return vR;
        }
        [Authorize]
        public ActionResult DeleteNoiDung(String ID)
        {
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = VayNoModels.XoaThongTinNoiDung(ID);
            return RedirectToAction("Index", "VayNo_NoiDung", new { MaNoiDung = string.Empty, TenNoiDung = string.Empty });
        }
    }
}
