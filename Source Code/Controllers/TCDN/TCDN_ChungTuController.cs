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
namespace VIETTEL.Controllers.TCDN
{
    public class TCDN_ChungTuController : Controller
    {
        //
        // GET: /TCDN_ChungTu/
        public string sViewPath = "~/Views/TCDN/ChungTu/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "TCDN_ChungTu_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String iID_MaDoanhNghiep = Request.Form[ParentID + "_iID_MaDoanhNghiep"];
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iQuy = Request.Form[ParentID + "_iQuy"];

            return RedirectToAction("Index", "TCDN_ChungTu", new { iID_MaDoanhNghiep = iID_MaDoanhNghiep, iQuy = iQuy, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
        [Authorize]
        public ActionResult Edit(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChungTu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(TCDNModels.iID_MaPhanHe, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "TCDN_ChungTu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChungTu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaChungTu"] = iID_MaChungTu;
            return View(sViewPath + "TCDN_ChungTu_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaChungTu)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(TCDNModels.iID_MaPhanHe, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("TCDN_ChungTu");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaDoanhNghiep = Convert.ToString(Request.Form[ParentID + "_iID_MaDoanhNghiep"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (iID_MaDoanhNghiep == Convert.ToString(Guid.Empty) || iID_MaDoanhNghiep == "" || iID_MaDoanhNghiep == null)
            {
                arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị!");
            }
            if (iQuy == string.Empty || iQuy == "" || iQuy == null)
            {
                arrLoi.Add("err_iQuy", "Bạn chưa chọn tháng cho chứng từ!");
            }
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaChungTu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["MaChungTu"] = MaChungTu;
                return View(sViewPath + "TCDN_ChungTu_Edit.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];

                String sLoai = "0;1";

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(TCDNModels.iID_MaPhanHe));
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(TCDNModels.iID_MaPhanHe));
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    //TCDN_ChungTuChiTietModels.ThemChiTiet(MaChungTuAddNew, sLoai, MaND, Request.UserHostAddress);
                    //TCDN_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                    return RedirectToAction("Index", "TCDN_ChungTuChiTiet", new { iID_MaChungTu = MaChungTuAddNew });
                }
                else
                {
                    bang.GiaTriKhoa = MaChungTu;
                    String SQL =
                        String.Format(
                            @"UPDATE TCDN_ChungTuChiTiet SET iID_MaDoanhNghiep=@iID_MaDoanhNghiep,iQuy=@iQuy WHERE iID_MaChungTu=@iID_MaChungTu");
                    SqlCommand cmd= new SqlCommand(SQL);
                    cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
                    cmd.Parameters.AddWithValue("@iQuy", iQuy);
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", MaChungTu);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                    bang.Save();
                }
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "TCDN_ChungTu");
        }

        [Authorize]
        public ActionResult Delete(String iID_MaChungTu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_ChungTu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = TCDN_ChungTuModels.Delete_ChungTu(iID_MaChungTu, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "TCDN_ChungTu");
        }

        [Authorize]
        public ActionResult Duyet()
        {
            return RedirectToAction("Index", "TCDN_ChungTu");
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = TCDN_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            TCDN_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = TCDN_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            TCDN_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            int iID_MaTrangThaiTuChoi = TCDN_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            return RedirectToAction("Index", "TCDN_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = TCDN_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            TCDN_DuyetChungTuModels.CapNhapLaiTruong_sSua(iID_MaChungTu);

            ///Update trạng thái cho bảng chứng từ
            TCDN_ChungTuModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = TCDN_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            TCDN_ChungTuModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Index", "TCDN_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }
    }
}
