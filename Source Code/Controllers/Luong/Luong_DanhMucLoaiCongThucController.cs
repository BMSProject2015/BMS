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
    public class Luong_DanhMucLoaiCongThucController : Controller
    {
        //
        // GET: /Luong_DanhMucLoaiCongThuc/
        public string sViewPath = "~/Views/Luong/DanhMucLoaiCongThuc/";

        [Authorize]
        public ActionResult Index(int? DanhMucLoaiCongThuc_page)
        {
            Bang bang = new Bang("L_DanhMucLoaiCongThuc");
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            ViewData["DanhMucLoaiCongThuc_page"] = DanhMucLoaiCongThuc_page;
            return View(sViewPath + "Luong_DanhMucLoaiCongThuc_Index.aspx");
        }

        [Authorize]
        public ActionResult Detail(string iID_MaDanhMucLoaiCongThuc)
        {
            Bang bang = new Bang("L_DanhMucLoaiCongThuc");
            bang.GiaTriKhoa = iID_MaDanhMucLoaiCongThuc;
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
        public ActionResult Delete(string iID_MaDanhMucLoaiCongThuc)
        {
            String SQL = "UPDATE L_DanhMucLoaiCongThuc SET iTrangThai=0 WHERE iID_MaDanhMucLoaiCongThuc=@iID_MaDanhMucLoaiCongThuc";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucLoaiCongThuc", iID_MaDanhMucLoaiCongThuc);

            Connection.UpdateDatabase(cmd, User.Identity.Name, Request.UserHostAddress);

            return RedirectToAction("Index", "Luong_DanhMucLoaiCongThuc");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string iID_MaDanhMucLoaiCongThuc)
        {

            ViewData["iID_MaDanhMucLoaiCongThuc"] = iID_MaDanhMucLoaiCongThuc;

            NameValueCollection data = new NameValueCollection();
            if (String.IsNullOrEmpty(iID_MaDanhMucLoaiCongThuc) == false)
            {

                ViewData["DuLieuMoi"] = "0";
                data = Luong_DanhMucLoaiCongThucModels.LayThongTinLoaiCongThuc(iID_MaDanhMucLoaiCongThuc);
            }
            else
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["data"] = data;
            return View(sViewPath + "Luong_DanhMucLoaiCongThuc_Edit.aspx");

        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID,String iID_MaDanhMucLoaiCongThuc)
        {
            Bang bang = new Bang("L_DanhMucLoaiCongThuc");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            NameValueCollection arrLoi = bang.TruyenGiaTri(ParentID, Request.Form);

          
            
            String sTen = Request.Form[ParentID + "_sTen"];
        

            if (String.IsNullOrEmpty(sTen))
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập công thức");
            }


            if (arrLoi.Count == 0)
            {
                if (bang.DuLieuMoi)
                    bang.Save();
                else
                {
                    bang.GiaTriKhoa = iID_MaDanhMucLoaiCongThuc;
                    bang.Save();
                }
                return RedirectToAction("Index", "Luong_DanhMucLoaiCongThuc");
            }
            else
            {

                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }

                Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                ViewData["DuLieuMoi"] = Convert.ToInt16(bang.DuLieuMoi);
                ViewData["iID_MaDanhMucLoaiCongThuc"] = iID_MaDanhMucLoaiCongThuc;
                ViewData["data"] = dicData["data"];
                return View(sViewPath + "Luong_DanhMucLoaiCongThuc_Edit.aspx");
            }
        }


    }
}
