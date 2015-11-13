using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;

namespace VIETTEL.Controllers.VayNo
{
    public class ChungTuVayNoController : Controller
    {
        //
        // GET: /ChungTuVayNo/
        public string sViewPath = "~/Views/VayNo/ChungTu/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "VN_Vay", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "ChungTu_VayNo_Edit.aspx");
        }
        [Authorize]
        public ActionResult Edit(String iID_MaChungTu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "VN_Vay", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChungTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_Vay"] = iID_MaChungTu;
            return View(sViewPath + "ChungTu_VayNo_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaChungTu)
        {
            String MaCT = "";
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeTinDung, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("VN_Vay");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();

            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayChungTu"]);
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                String SoChungTu = Convert.ToString(Request.Form[ParentID + "_sSoChungTu"]);
                if (SoChungTu == string.Empty || SoChungTu == "" || SoChungTu == null)
                {
                    arrLoi.Add("err_sSoChungTu", MessageModels.iSochungTu);
                }
                if (SoChungTu != "" && VayNoModels.CheckChungTu(SoChungTu))
                {
                    arrLoi.Add("err_sSoChungTu", MessageModels.iSochungTuTonTai);
                }
            }
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", MessageModels.iNgayChungTu);
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }

                ViewData["iID_Vay"] = iID_MaChungTu;
                return View(sViewPath + "ChungTu_VayNo_Edit.aspx");
            }
            else
            {

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    //bang.CmdParams.Parameters.AddWithValue("@iThang", DanhMucModels.ThangLamViec(User.Identity.Name));
                    //bang.CmdParams.Parameters.AddWithValue("@iNam", DanhMucModels.NamLamViec(User.Identity.Name));
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeTinDung));
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    VayNoModels.InsertDuyetChungTu(MaChungTuAddNew, MessageModels.sMoiTao, User.Identity.Name, Request.UserHostAddress);
                    MaCT = MaChungTuAddNew;
                }
                else
                {
                    bang.GiaTriKhoa = iID_MaChungTu;
                    bang.Save();
                    MaCT = iID_MaChungTu;
                }
            }
            return RedirectToAction("Index", "VayNo_ChungTuChiTiet", new { iID_MaChungTu = MaCT });
        }
        [Authorize]
        public ActionResult Delete(String ParentID, String iID_MaChungTu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "VN_Vay", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = VayNoModels.Delete_ChungTu(iID_MaChungTu, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "VayVon");
        }



    }
}
