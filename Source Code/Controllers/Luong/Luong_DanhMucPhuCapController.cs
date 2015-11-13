using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using DomainModel.Abstract;
using System.Data.SqlClient;
using System.Collections.Specialized;
using VIETTEL.Models;

namespace VIETTEL.Controllers.Luong
{
    public class Luong_DanhMucPhuCapController : Controller
    {
        //
        // GET: /Luong_DanhMucPhuCap/
        public string sViewPath = "~/Views/Luong/DanhMucPhuCap/";

        [Authorize]
        public ActionResult Index(int? DanhMucPhuCap_page)
        {
            Bang bang = new Bang("L_DanhMucPhuCap");
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            ViewData["DanhMucPhuCap_page"] = DanhMucPhuCap_page;
            return View(sViewPath + "Luong_DanhMucPhuCap_Index.aspx");
        }

        [Authorize]
        public ActionResult Detail(string iID_MaPhuCap)
        {
            Bang bang = new Bang("L_DanhMucPhuCap");
            bang.GiaTriKhoa = iID_MaPhuCap;
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


        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string iID_MaPhuCap)
        {
            Bang bang = new Bang("L_DanhMucPhuCap");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.GiaTriKhoa = iID_MaPhuCap;
            bang.Delete();

            SqlCommand cmd = new SqlCommand("UPDATE L_DanhMucTruong_MucLucNganSach SET iTrangThai=0 WHERE sMaTruong=@sMaTruong");
            cmd.Parameters.AddWithValue("@sMaTruong", "PhuCap_" + iID_MaPhuCap);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return RedirectToAction("Index", "Luong_DanhMucPhuCap");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string iID_MaPhuCap)
        {

            ViewData["iID_MaPhuCap"] = iID_MaPhuCap;

            NameValueCollection data = new NameValueCollection();
            if (String.IsNullOrEmpty(iID_MaPhuCap) == false)
            {
                ViewData["DuLieuMoi"] = "0";
                data = LuongModels.LayThongTinPhuCap(iID_MaPhuCap);
                data = Luong_DanhMucPhuCapModels.Get_dataPhuCapMucLuc(data, iID_MaPhuCap);
            }
            else
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["data"] = data;
            return View(sViewPath + "Luong_DanhMucPhuCap_Edit.aspx");

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            Bang bangPhuCapMucLucNS = new Bang("L_DanhMucTruong_MucLucNganSach");
            bangPhuCapMucLucNS.MaNguoiDungSua = User.Identity.Name;
            bangPhuCapMucLucNS.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi1 = bangPhuCapMucLucNS.TruyenGiaTri(ParentID, Request.Form);

            Bang bang = new Bang("L_DanhMucPhuCap");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

            String iID_MaPhuCap = Request.Form[ParentID + "_iID_MaPhuCap"];
            iID_MaPhuCap = iID_MaPhuCap.Trim();
            String sTenPhuCap = Request.Form[ParentID + "_sTenPhuCap"];
            if (HamChung.Check_Trung(bang.TenBang, bang.TruongKhoa, iID_MaPhuCap, "iID_MaPhuCap", iID_MaPhuCap, bang.DuLieuMoi))
            {
                arrLoi.Add("err_iID_MaPhuCap", "Không được nhập trùng ký hiệu");
            }

            if (String.IsNullOrEmpty(iID_MaPhuCap))
            {
                arrLoi.Add("err_iID_MaPhuCap", "Bạn chưa nhập ký hiệu");
            }

            if (String.IsNullOrEmpty(sTenPhuCap))
            {
                arrLoi.Add("err_sTenPhuCap", "Bạn chưa nhập tên phụ cấp");
            }


            if (arrLoi.Count == 0)
            {
                bang.CmdParams.Parameters.AddWithValue("@sMaTruong","PhuCap_"+iID_MaPhuCap);
                bang.GiaTriKhoa = iID_MaPhuCap;
                bang.Save();
                Luong_DanhMucPhuCapModels.DetailSubmit(iID_MaPhuCap, User.Identity.Name, Request.UserHostAddress, Request.Form);             
                return RedirectToAction("Index", "Luong_DanhMucPhuCap");
            }
            else
            {

                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                for (int i = 0; i <= arrLoi1.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi1.GetKey(i), arrLoi1[i]);
                }

                Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                ViewData["DuLieuMoi"] = Convert.ToInt16(bang.DuLieuMoi);
                ViewData["iID_MaPhuCap"] = iID_MaPhuCap;
                ViewData["data"] = dicData["data"];
                return View(sViewPath + "Luong_DanhMucPhuCap_Edit.aspx");
            }
        }

    }
}
