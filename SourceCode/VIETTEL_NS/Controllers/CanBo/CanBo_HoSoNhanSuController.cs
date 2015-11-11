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
namespace VIETTEL.Controllers.NhanSu
{
    public class CanBo_HoSoNhanSuController : Controller
    {
        //
        // GET: /QuyetToan_ChungTuChiTiet/
        public string sViewPath = "~/Views/CanBo/Pages/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "CanBo_DanhSachHoSo_List.aspx");
        }

        /// <summary>
        /// Thêm mới hoặc sửa chứng từ
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaCanBo)
        {
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaCanBo))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaCanBo"] = iID_MaCanBo;
            return View(sViewPath + "CanBo_HoSoNhanSu_Edit.aspx");
        }

        /// <summary>
        /// Lưu trữ vào CSDL
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaCanBo)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeNhanSu, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("CB_CanBo");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();

            String DuLieuMoi = Request.Form[ParentID + "_DuLieuMoi"];
            String sSoHieuCBCC = Convert.ToString(Request.Form[ParentID + "_sSoHieuCBCC"]);
            //String iID_MaDoiTuong = Convert.ToString(Request.Form[ParentID + "_iID_MaDoiTuong"]);
            String sHoTen = Convert.ToString(Request.Form[ParentID + "_sHoDem"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);

            String sDiaChi = Convert.ToString(Request.Form[ParentID + "_sDiaChi"]);

            String iID_MaNgachLuong = Convert.ToString(Request.Form[ParentID + "_iID_MaNgachLuong"]);
            //String iID_MaBacLuong = Convert.ToString(Request.Form[ParentID + "_iID_MaBacLuong"]);
            String iID_MaTrinhDoVanHoa = Convert.ToString(Request.Form[ParentID + "_iID_MaTrinhDoVanHoa"]);
            String iID_MaTrinhDoChuyenMonCaoNhat = Convert.ToString(Request.Form[ParentID + "_iID_MaTrinhDoChuyenMonCaoNhat"]);

            String dNgayVaoCQ = Convert.ToString(Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayVaoCQ"]);
            String dNgayNhapNgu = Convert.ToString(Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayNhapNgu"]);
            if (sSoHieuCBCC == string.Empty || sSoHieuCBCC == "" || sSoHieuCBCC == null)
            {
                arrLoi.Add("err_sSoHieuCBCC", MessageModels.sSoHieuCBCC);
            }
            //if (iID_MaDoiTuong == string.Empty || iID_MaDoiTuong == "" || iID_MaDoiTuong == null || iID_MaDoiTuong == Convert.ToString(Guid.Empty))
            //{
            //    arrLoi.Add("err_iID_MaDoiTuong", MessageModels.sDoiTuong);
            //}
            if (sHoTen == string.Empty || sHoTen == "" || sHoTen == null)
            {
                arrLoi.Add("err_sHoTen", MessageModels.sHoDem);
            }
            if (sTen == string.Empty || sTen == "" || sTen == null)
            {
                arrLoi.Add("err_sTen", MessageModels.sTenCB);
            }
            if (iID_MaDonVi == string.Empty || iID_MaDonVi == "" || iID_MaDonVi == null)
            {
                arrLoi.Add("err_iID_MaPhongBan", MessageModels.sPhongBan);
            }
            //if (sDiaChi == string.Empty || sDiaChi == "" || sDiaChi == null)
            //{
            //    arrLoi.Add("err_sDiaChi", MessageModels.sNoiO);
            //}
            if (iID_MaNgachLuong == string.Empty || iID_MaNgachLuong == "" || iID_MaNgachLuong == null || iID_MaNgachLuong == Convert.ToString(Guid.Empty))
            {
                arrLoi.Add("err_iID_MaNgachLuong", MessageModels.sNgachCongChuc);
            }

            //if (iID_MaBacLuong == string.Empty || iID_MaBacLuong == "" || iID_MaBacLuong == null || iID_MaBacLuong == Convert.ToString(Guid.Empty))
            //{
            //    arrLoi.Add("err_iID_MaBacLuong", MessageModels.sBacLuong);
            //}


            //if (iID_MaTrinhDoVanHoa == string.Empty || iID_MaTrinhDoVanHoa == "" || iID_MaTrinhDoVanHoa == null || iID_MaTrinhDoVanHoa == Convert.ToString(Guid.Empty))
            //{
            //    arrLoi.Add("err_iID_MaTrinhDoVanHoa", MessageModels.sTrinhDoHV);
            //}
            //if (iID_MaTrinhDoChuyenMonCaoNhat == string.Empty || iID_MaTrinhDoChuyenMonCaoNhat == "" || iID_MaTrinhDoChuyenMonCaoNhat == null || iID_MaTrinhDoChuyenMonCaoNhat == Convert.ToString(Guid.Empty))
            //{
            //    arrLoi.Add("err_iID_MaTrinhDoChuyenMonCaoNhat", MessageModels.sTrinhDoChuyenMon);
            //}
            if (dNgayVaoCQ == string.Empty || dNgayVaoCQ == "" || dNgayVaoCQ == null)
            {
                arrLoi.Add("err_dNgayVaoCQ", MessageModels.sNgayVaoCQ);
            }
            if (dNgayNhapNgu == string.Empty || dNgayNhapNgu == "" || dNgayNhapNgu == null)
            {
                arrLoi.Add("err_dNgayNhapNgu", "Nhập ngày nhập ngũ");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }

                ViewData["iID_MaCanBo"] = iID_MaCanBo;
                return View(sViewPath + "CanBo_HoSoNhanSu_Edit.aspx");
            }
            else
            {
                string sTenDonVi = GetTenDonVi(iID_MaDonVi);
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                //tu khoa hỗ trợ việc tìm có dấu và không có dấu
                string strChuoi = sHoTen + " " + sTen + " " + NgonNgu.LayXauKhongDauTiengViet(sHoTen) + " " + NgonNgu.LayXauKhongDauTiengViet(sTen);
                bang.CmdParams.Parameters.AddWithValue("@sTuKhoa", strChuoi);
                bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTam", Guid.NewGuid());
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {

                    bang.Save();
                }
                else
                {
                    //bang.TruongKhoaKieuSo = true;
                    bang.TruongKhoa = "iID_MaCanBo";
                    bang.GiaTriKhoa = iID_MaCanBo;
                    bang.Save();
                }
                //cap nhat ten don vi
                //string sTenDonVi = GetTenDonVi(iID_MaDonVi);

                //SqlCommand cmd = new SqlCommand("UPDATE CB_CanBo SET sTenDonVi=@sTenDonVi WHERE iID_MaCanBo=@iID_MaCanBo");

                //cmd.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
                //cmd.Parameters.AddWithValue("@iID_MaCanBo", MaCanBo);
                //Connection.UpdateDatabase(cmd);
                //cmd.Dispose();
            }
            return RedirectToAction("Edit", "CanBo_HoSoNhanSu");
        }
        /// <summary>
        /// Cập nhật chi tiết
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditTinhHinhKT(String ParentID, String iID_MaCanBo)
        {
            String MaND = User.Identity.Name;          
            Bang bang = new Bang("CB_CanBo");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            //tu khoa hỗ trợ việc tìm có dấu và không có dấu      

            //bang.TruongKhoaKieuSo = true;
            bang.TruongKhoa = "iID_MaCanBo";
            bang.GiaTriKhoa = iID_MaCanBo;
            bang.Save();


            return RedirectToAction("Edit", "CanBo_HoSoNhanSu", new { iID_MaCanBo = iID_MaCanBo });
        }

      
        /// <summary>
        /// Xóa thông tin cán bộ
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaCanBo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "CB_CanBo", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Boolean kt = CanBo_HoSoNhanSuModels.CheckMaNhanSu(Convert.ToString(iID_MaCanBo));
            //if (kt == true)
            //{ 
            //    return RedirectToAction("Index", "PermitionMessage"); 
            //}
            //Xóa dữ liệu trong bảng KTCS_ChungTuGhiSo
            Bang bang = new Bang("CB_CanBo");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruongKhoa = "iID_MaCanBo";
            bang.GiaTriKhoa = iID_MaCanBo;
            bang.Delete();
            return View(sViewPath + "CanBo_DanhSachHoSo_List.aspx");
        }

        /// <summary>
        /// Tìm kiếm nhân sự
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SearchSubmit(String ParentID)
        {
            //Đơn vị
            String iID_MaDonVi = "";
            String MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            if (String.IsNullOrEmpty(MaDonVi) == false && MaDonVi != Convert.ToString(Guid.Empty)) iID_MaDonVi = MaDonVi;
            //Trình độ
            String iID_MaTrinhDo = "";
            String MaTrinhDo = Request.Form[ParentID + "_iID_MaTrinhDo"];
            if (String.IsNullOrEmpty(MaTrinhDo) == false && MaTrinhDo != Convert.ToString(Guid.Empty)) iID_MaTrinhDo = MaTrinhDo;
            String iTuoiTu = Request.Form[ParentID + "_iTuoiTu"];
            String iDenTuoi = Request.Form[ParentID + "_iDenTuoi"];
            String sMaHoSo = Request.Form[ParentID + "_sMaHoSo"];
            String sHoTen = Request.Form[ParentID + "_sHoTen"];
            //Chức vụ
            String iID_MaChucVu = "";
            String MaChucVu = Request.Form[ParentID + "_iID_MaChucVu"];
            if (String.IsNullOrEmpty(MaChucVu) == false && MaChucVu != Convert.ToString(Guid.Empty)) iID_MaChucVu = MaChucVu;
            //đối tượng
            String iID_MaDT = "";
            String MaDT = Request.Form[ParentID + "_iID_MaDT"];
            if (String.IsNullOrEmpty(MaDT) == false && MaDT != Convert.ToString(Guid.Empty)) iID_MaDT = MaDT;
            //ngạch
            String iID_MaNgach = "";
            String MaNgach = Request.Form[ParentID + "_iID_MaNgach"];
            if (String.IsNullOrEmpty(MaNgach) == false && MaNgach != Convert.ToString(Guid.Empty)) iID_MaNgach = MaNgach;
            String sSoHieuCBCC = Request.Form[ParentID + "_sSoHieuCBCC"];
            //lý luận chính trị
            String iID_MaTrinhDoLyLuanCT = "";
            String MaTrinhDoLyLuanCT = Request.Form[ParentID + "_iID_MaTrinhDoLyLuanCT"];
            if (String.IsNullOrEmpty(MaTrinhDoLyLuanCT) == false && MaTrinhDoLyLuanCT != Convert.ToString(Guid.Empty)) iID_MaTrinhDoLyLuanCT = MaTrinhDoLyLuanCT;
          //tình trạng cán bộ
            String iID_MaTinhTrangCanBo ="";
            String MaTinhTrangCanBo = Request.Form[ParentID + "_iID_MaTinhTrangCanBo"];
            if (String.IsNullOrEmpty(MaTinhTrangCanBo) == false && MaTinhTrangCanBo != "-1") iID_MaTinhTrangCanBo = MaTinhTrangCanBo;
            return RedirectToAction("Index", "CanBo_HoSoNhanSu", new
            {
                iID_MaDonVi = iID_MaDonVi,
                iID_MaTrinhDo = iID_MaTrinhDo,
                iTuoiTu = iTuoiTu,
                iDenTuoi = iDenTuoi,
                sMaHoSo = sMaHoSo,
                sHoTen = sHoTen,
                iID_MaChucVu = iID_MaChucVu,
                iID_MaDT = iID_MaDT,
                iID_MaNgach = iID_MaNgach,
                sSoHieuCBCC = sSoHieuCBCC,
                iID_MaTrinhDoLyLuanCT = iID_MaTrinhDoLyLuanCT,
                iID_MaTinhTrangCanBo = iID_MaTinhTrangCanBo
            });
        }


        /// <summary>
        /// Thêm dữ liệu từ bảng QTA_ChungTuChiTiet vào bảng KT_ChungTuChiTiet
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="OnSuccess"></param>
        /// <param name="iID_MaChungTu"></param>
        /// <returns></returns>
        public JavaScriptResult Edit_Fast_DaoTao_Submit(String ParentID, String OnSuccess, String iID_MaCanBo)
        {
            //Insert into data vào bảng: KT_ChungTuChiTiet
            Bang bang = new Bang("CB_QuaTrinhDaoTao");
            bang.TruyenGiaTri(ParentID, Request.Form);
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);          
            bang.Save();   

            String strJ = "";
            if (String.IsNullOrEmpty(OnSuccess) == false)
            {
                strJ = String.Format("Dialog_close('{0}');{1}();", ParentID, OnSuccess);
            }
            else
            {
                strJ = String.Format("Dialog_close('{0}');", ParentID);
            }
            return JavaScript(strJ);
        }

        /// <summary>
        /// Lay huyện bởi tỉnh
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult getQuanHuyen(String Id)
        {
            var dt = CanBo_DanhMucNhanSuModels.getHuyen(Id, true, "--- Lựa chọn ---");
            JsonResult value = Json(HamChung.getGiaTri("iID_MaHuyen", "sTenHuyen", dt), JsonRequestBehavior.AllowGet);
            if (dt != null) dt.Dispose();
            return value;
        }
        /// <summary>
        /// Lấy xã phưởng
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult getXaPhuong(String Id)
        {
            var dt = CanBo_DanhMucNhanSuModels.getXaPhuong(Id, true, "--- Lựa chọn ---");
            JsonResult value = Json(HamChung.getGiaTri("iID_MaXaPhuong", "sTenXaPhuong", dt), JsonRequestBehavior.AllowGet);
            if (dt != null) dt.Dispose();
            return value;
        }
        /// <summary>
        /// Lấy bậc lương
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult getBacLuong(String Id)
        {
            var dt = CanBo_DanhMucNhanSuModels.getBacLuong(Id, true, "--- Lựa chọn ---");
            JsonResult value = Json(HamChung.getGiaTri("iID_MaBacLuong", "sTenBacLuong", dt), JsonRequestBehavior.AllowGet);
            if (dt != null) dt.Dispose();
            return value;
        }
        /// <summary>
        /// Lấy trạng thái nữ quân nhân theo giới tính
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult getGioiTinh(String Id)
        {
            DataTable dt = null;
            if (Id == "1") dt = CanBo_DanhMucNhanSuModels.getNuQuanNhan(true, "--- Lựa chọn ---", 0);
            else dt = CanBo_DanhMucNhanSuModels.getNuQuanNhan(true, "--- Lựa chọn ---", 1);
            JsonResult value = Json(HamChung.getGiaTri("iID_Ma", "sTen", dt), JsonRequestBehavior.AllowGet);
            if (dt != null) dt.Dispose();
            return value;
        }
        /// <summary>
        /// Lý do tăng giảm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult getLyDoTangGiam(String Id)
        {
            var dt = CanBo_DanhMucNhanSuModels.getLyDoTangGiam(true, "--- Lựa chọn ---", Id);
            JsonResult value = Json(HamChung.getGiaTri("iID_MaLyDoTangGiam", "sTen", dt), JsonRequestBehavior.AllowGet);
            if (dt != null) dt.Dispose();
            return value;
        }
        /// <summary>
        /// Lay he so luong
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult getHeSoLuong(String Id, String MaNgach)
        {
            string sHeSoLuong = CanBo_DanhMucNhanSuModels.getHeSoLuong(MaNgach, Id);
            JsonResult value = Json(sHeSoLuong, JsonRequestBehavior.AllowGet);         
            return value;
        }

        /// <summary>
        /// Lay  ten don vi
        /// </summary>
        /// <param name="MaDonVi"> ma don vi</param>
        /// <returns></returns>
        public string GetTenDonVi(String MaDonVi)
        {
            string sTenDonVi = "";
            try
            {
                DataTable dtDonVi = CanBo_DanhMucNhanSuModels.GetDonVi(MaDonVi);
                if(dtDonVi.Rows.Count>0)
                {
                    sTenDonVi = Convert.ToString(dtDonVi.Rows[0]["sTen"]);
                }
            }
            catch (Exception)
            {
                
            }
            return sTenDonVi;
        }
    }
}
