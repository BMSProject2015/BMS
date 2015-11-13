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

namespace VIETTEL.Controllers.VayNo
{
    public class VayNo_DuyetController : Controller
    {
        //
        // GET: /VayNo_Duyet/
        public string sViewPath = "~/Views/VayNo/";
        public ActionResult Index()
        {
            return View(sViewPath + "VayNo_Duyet.aspx");
        }
        [Authorize]
        public ActionResult Detail(string MaID)
        {
            //if (NganSach_HamChungModels.TroLyPhongBan(User.Identity.Name) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_ChungTu", "Edit") == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}

            ViewData["MaID"] = MaID;
            return View(sViewPath + "VayNo_Detail.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchDuyetSubmit(String ParentID)
        {
          
            String MaNam = Request.Form[ParentID + "_MaNam"];
            //
            String iID_MaDonVi = "";
            String key = Request.Form[ParentID + "_iID_MaDonVi"];
            if (key != Convert.ToString(Guid.Empty)) iID_MaDonVi = key;   

            String dTuNgayVay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgayVay"];
            String dTuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];            
            String iSoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String MaThang = Request.Form[ParentID + "_MaThang"];
            //
            String iID_MaNoiDung = Request.Form[ParentID + "_iID_MaNoiDung"];        

            String dDenNgayVay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgayVay"];
            String dDenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            //
            String iID_MaTrangThaiDuyet = "";
            String KeyMaTT = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            if (KeyMaTT != "-1") iID_MaTrangThaiDuyet = KeyMaTT;

            return RedirectToAction("Index", "VayNo_Duyet", new
            {
                Nam = MaNam,
                Thang = MaThang,
                MaDonVi = iID_MaDonVi.Trim(),
                NDVayNo = iID_MaNoiDung.Trim(),
                TuNgayVay = dTuNgayVay.Trim(),
                DenNgayVay = dDenNgayVay.Trim(),
                TuNgay = dTuNgay.Trim(),
                DenNgay = dDenNgay.Trim(),
                SoChungTu = iSoChungTu.Trim(),
                iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet.Trim()
            });
        }
        //[Authorize]
        //public ActionResult PheDuyet(String MaID)
        //{
        //    String _sMoTa = Convert.ToString(Request.Form["_sMoTa"]);
        //    String MaND = User.Identity.Name;
        //    //Xác định trạng thái duyệt tiếp theo
        //    int iID_MaTrangThaiDuyet_TrinhDuyet = VayNoModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, MaID);
        //    if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
        //    {
        //        return RedirectToAction("Index", "PermitionMessage");
        //    }
        //    DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
        //    String NoiDung = "";
        //    if (dtTrangThaiDuyet.Rows.Count > 0)
        //        NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
        //    if (dtTrangThaiDuyet != null) dtTrangThaiDuyet.Dispose();

        //    ///Update trạng thái cho bảng chứng từ
        //    VayNoModels.Update_iID_MaTrangThaiDuyet(MaID, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

        //    ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
        //    String MaDuyetChungTu = VayNoModels.InsertDuyetChungTu(MaID, NoiDung, VayNoModels.ConvertToString(_sMoTa).Trim(), MaND, Request.UserHostAddress);

        //    ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
        //    SqlCommand cmd;
        //    cmd = new SqlCommand();
        //    cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
        //    VayNoModels.UpdateRecord(MaID, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
        //    cmd.Dispose();
        //    return RedirectToAction("Detail", "VayNo_Duyet", new { MaID = MaID });
        //}
        
        //// từ chối
        //[Authorize]
      
        //public ActionResult TuChoi(String ParentID,  String MaID)
        //{
        //    NameValueCollection arrLoi = new NameValueCollection();
        //    String _sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);
        //    if (_sMoTa == "")
        //    {
        //        arrLoi.Add("err_sMoTa", MessageModels.sTuChoi);              
        //    }
        //    if (arrLoi.Count > 0)
        //    {
        //        for (int i = 0; i <= arrLoi.Count - 1; i++)
        //        {
        //            ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
        //        }            
        //        return RedirectToAction("Detail", "VayNo_Duyet", new { MaID = MaID });
        //    }
        //    else
        //    {
        //        String MaND = User.Identity.Name;
        //        //Xác định trạng thái duyệt tiếp theo
        //        int iID_MaTrangThaiDuyet_TuChoi = VayNoModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, MaID);
        //        if (iID_MaTrangThaiDuyet_TuChoi <= 0)
        //        {
        //            return RedirectToAction("Index", "PermitionMessage");
        //        }
        //        DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
        //        String NoiDung = "";
        //        if (dtTrangThaiDuyet.Rows.Count > 0)
        //            NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
        //        if (dtTrangThaiDuyet != null) dtTrangThaiDuyet.Dispose();


        //        ///Update trạng thái cho bảng chứng từ
        //        VayNoModels.Update_iID_MaTrangThaiDuyet(MaID, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

        //        ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
        //        String MaDuyetChungTu = VayNoModels.InsertDuyetChungTu(MaID, NoiDung, VayNoModels.ConvertToString(_sMoTa).Trim(), MaND, Request.UserHostAddress);

        //        ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
        //        SqlCommand cmd;
        //        cmd = new SqlCommand();
        //        cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
        //        VayNoModels.UpdateRecord(MaID, cmd.Parameters, MaND, Request.UserHostAddress);
        //        cmd.Dispose();
        //        return RedirectToAction("Detail", "VayNo_Duyet", new { MaID = MaID });
        //    }
        //}
    }
}
