
using System;
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
namespace VIETTEL.Controllers.VayNo.VayVon
{
    public class VayNo_ChungTuChiTietController : Controller
    {
        //
        // GET: /VayNo_ChungTuChiTiet/

        public string sViewPath = "~/Views/VayNo/VayVon/Vay/";
        [Authorize]
        public ActionResult Index(string iID_MaChungTu)
        {

            return View(sViewPath + "VayVon_ChungTu_ThemMoi.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSubmit(String ParentID, String iID_MaChungTu)
        {
            NameValueCollection arrLoi = new NameValueCollection();
            String dHanPhaiTra = Convert.ToString(Request.Form[ParentID + "_vidHanPhaiTra"]);

            String dNgayVay = Convert.ToString(Request.Form[ParentID + "_vidNgayVay"]);
            String rLaiSuat = Convert.ToString(Request.Form[ParentID + "_rLaiSuat"]);
            // String rMienLai = Convert.ToString(Request.Form[ParentID + "_rMienLai"]);
            String rVayTrongThang = Convert.ToString(Request.Form[ParentID + "_rVayTrongThang"]);
            //String rThoiGianThuVon = Convert.ToString(Request.Form[ParentID + "_rThoiGianThuVon"]);  
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);

            String iID_MaTaiKhoan_No = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan_No"]);
            String iID_MaDonVi_No = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi_No"]);
            String iID_MaTaiKhoan_Co = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan_Co"]);
            String iID_MaDonVi_Co = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi_Co"]);
            String sTenDonVi = "", MaTaiKhoan_No = "", MaDonVi_No = "", MaTaiKhoan_Co = "", MaDonVi_Co = "";
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                sTenDonVi = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi, "sTen").ToString();
            }

            if (!String.IsNullOrEmpty(iID_MaTaiKhoan_No))
            {
                MaTaiKhoan_No = CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", iID_MaTaiKhoan_No, "sTen").ToString();
            }

            if (!String.IsNullOrEmpty(iID_MaDonVi_No))
            {
                MaDonVi_No = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi_No, "sTen").ToString();
            }

            if (!String.IsNullOrEmpty(iID_MaTaiKhoan_Co))
            {
                MaTaiKhoan_Co = CommonFunction.LayTruong("KT_TaiKhoan", "iID_MaTaiKhoan", iID_MaTaiKhoan_Co, "sTen").ToString();
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi_Co))
            {
                MaDonVi_Co = CommonFunction.LayTruong("NS_DonVi", "iID_MaDonVi", iID_MaDonVi_Co, "sTen").ToString();
            }
            String iID_MaNoiDung = Convert.ToString(Request.Form[ParentID + "_iID_MaNoiDung"]);
            String sTenNoiDung = "";
            if (!String.IsNullOrEmpty(iID_MaNoiDung))
            {
                sTenNoiDung = CommonFunction.LayTruong("DC_DanhMucNoiDung", "iID_MaNoiDung", iID_MaNoiDung, "sTenNoiDung").ToString();
            }
            if (String.IsNullOrEmpty(dNgayVay) == true || dNgayVay == "")
            {
                arrLoi.Add("err_dNgayVay", MessageModels.dNgayVay);
            }
            if (String.IsNullOrEmpty(rLaiSuat) == true || rLaiSuat == "")
            {
                arrLoi.Add("err_rLaiSuat", MessageModels.rLaiSuat);
            }

            //if (String.IsNullOrEmpty(rMienLai) == true || rMienLai == "")
            //{
            //    arrLoi.Add("err_rMienLai", MessageModels.rMienLai);
            //}

            if (String.IsNullOrEmpty(rVayTrongThang) == true || rVayTrongThang == "")
            {
                arrLoi.Add("err_rVayTrongThang", MessageModels.rVayTrongThang);
            }
            if (String.IsNullOrEmpty(rVayTrongThang) == false && rVayTrongThang != "" && decimal.Parse(rVayTrongThang) <= 0)
            {
                arrLoi.Add("err_rVayTrongThang", MessageModels.rVayTrongThang);
            }
            if (String.IsNullOrEmpty(dHanPhaiTra) == true || dHanPhaiTra == "")
            {
                arrLoi.Add("err_dHanPhaiTra", MessageModels.dHanPhaiTra);
            }
            //if (String.IsNullOrEmpty(rThoiGianThuVon) == true || rThoiGianThuVon == "")
            //{
            //    arrLoi.Add("err_rThoiGianThuVon", MessageModels.rThoiGianThuVon);
            //}
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaChungTu"] = iID_MaChungTu;
                return View(sViewPath + "VayVon_ChungTu_ThemMoi.aspx");
            }
            else
            {
                Bang bang = new Bang("VN_VayChiTiet");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (!String.IsNullOrEmpty(iID_MaDonVi))
                {
                    bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", iID_MaDonVi + " - " + sTenDonVi);
                }
                if (!String.IsNullOrEmpty(iID_MaNoiDung))
                {
                    bang.CmdParams.Parameters.AddWithValue("@sNoiDungVay", iID_MaNoiDung + " - " + sTenNoiDung);
                }
                if (!String.IsNullOrEmpty(iID_MaTaiKhoan_No))
                {
                    bang.CmdParams.Parameters.AddWithValue("@sTaiKhoanNo", iID_MaTaiKhoan_No + " - " + MaTaiKhoan_No);
                }
                if (!String.IsNullOrEmpty(iID_MaDonVi_No))
                {
                    bang.CmdParams.Parameters.AddWithValue("@sDonViNo", iID_MaDonVi_No + " - " + MaDonVi_No);
                }
                if (!String.IsNullOrEmpty(iID_MaTaiKhoan_Co))
                {
                    bang.CmdParams.Parameters.AddWithValue("@sTaiKhoanCo", iID_MaTaiKhoan_Co + " - " + MaTaiKhoan_Co);
                }
                if (!String.IsNullOrEmpty(iID_MaDonVi_Co))
                {
                    bang.CmdParams.Parameters.AddWithValue("@sDonViCo", iID_MaDonVi_Co + " - " + MaDonVi_Co);
                }
                bang.CmdParams.Parameters.AddWithValue("@iThang", VayNoModels.ThangChungTu(iID_MaChungTu));
                bang.CmdParams.Parameters.AddWithValue("@iNam", VayNoModels.NamChungTu(iID_MaChungTu));
                bang.GiaTriKhoa = Guid.NewGuid();
                bang.Save();
                return RedirectToAction("Index", "VayNo_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            }
        }

        [Authorize]

        public ActionResult TrinhDuyet(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = VayNoModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = "";
            if (dtTrangThaiDuyet.Rows.Count > 0)
                NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            if (dtTrangThaiDuyet != null) dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            VayNoModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = VayNoModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            VayNoModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            int iID_MaTrangThaiTuChoi = VayNoModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);

            //if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeTinDung, User.Identity.Name))
            //{
                return RedirectToAction("Index", "VayNo_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            //}
            //else
            //{
            //    return RedirectToAction("Duyet", "VayNo_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            //}

        }
        //// từ chối
        [Authorize]

        public ActionResult TuChoi(String iID_MaChungTu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = VayNoModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            if (dtTrangThaiDuyet != null) dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            VayNoModels.CapNhapLaiTruong_sSua(iID_MaChungTu);

            ///Update trạng thái cho bảng chứng từ
            VayNoModels.Update_iID_MaTrangThaiDuyet(iID_MaChungTu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChungTu = DuToan_ChungTuModels.InsertDuyetChungTu(iID_MaChungTu, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChungTuCuoiCung", MaDuyetChungTu);
            VayNoModels.UpdateRecord(iID_MaChungTu, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("Index", "VayNo_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });

        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String iID_MaChungTu)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "VN_VayChiTiet", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = VayNoModels.SuaChungTuChiTiet(iID_MaChungTu, Request.Form, User.Identity.Name, Request.UserHostAddress);
            if (arrLoi == null || arrLoi.Count == 0)
            {
                return RedirectToAction("Index", "VayNo_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            }
            else
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError("_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaChungTu"] = iID_MaChungTu;
                return View(sViewPath + "VayVon_ChungTu_ThemMoi.aspx");
            }
        }

        [Authorize]
        public ActionResult Delete(String iID_MaChungTuChiTiet, String iID_MaChungTu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "VN_VayChiTiet", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Xóa dữ liệu trong bảng DT_ChungTuChiTiet
            SqlCommand cmd;
            cmd = new SqlCommand("DELETE FROM VN_VayChiTiet WHERE iID_VayChiTiet=@iID_VayChiTiet");
            cmd.Parameters.AddWithValue("@iID_VayChiTiet", iID_MaChungTuChiTiet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return RedirectToAction("Index", "VayNo_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        }
    }
}
