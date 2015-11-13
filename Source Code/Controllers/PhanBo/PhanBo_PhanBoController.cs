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
namespace VIETTEL.Controllers.PhanBo
{
    public class PhanBo_PhanBoController : Controller
    {
        //
        // GET: /PhanBo_PhanBo/
        public string sViewPath = "~/Views/PhanBo/PhanBo/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "PhanBo_Index.aspx");
        }
      
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String MaDotPhanBo)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoPhanBo = Request.Form[ParentID + "_iSoPhanBo"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Index", "PhanBo_PhanBo", new { MaDotPhanBo = MaDotPhanBo, SoPhanBo = SoPhanBo, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
      
        [Authorize]
        public ActionResult Edit(String iID_MaPhanBo, String iID_MaChiTieu)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChiTieu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "PB_PhanBo", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaPhanBo))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaPhanBo"] = iID_MaPhanBo;
            ViewData["MaChiTieu"] = iID_MaChiTieu;
            return View(sViewPath + "PhanBo_Edit.aspx");
        }

        

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaPhanBo, String MaChiTieu)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("PB_PhanBo");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();

            String dNgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (dNgayChungTu == string.Empty || dNgayChungTu == "" || dNgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaPhanBo"] = iID_MaPhanBo;
                ViewData["MaChiTieu"] = MaChiTieu;
                return View(sViewPath + "PhanBo_Edit.aspx");
            }
            else
            {
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "0")
                {
                    bang.GiaTriKhoa = iID_MaPhanBo;
                    bang.Save();
                }
            }
            return RedirectToAction("Index", "PhanBo_PhanBo", new { iID_MaChiTieu = MaChiTieu });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Submit_PhanBoTong(String ParentID, String iID_MaPhanBo)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            //if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("PB_PhanBo");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();

            String dNgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (dNgayChungTu == string.Empty || dNgayChungTu == "" || dNgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaPhanBo"] = iID_MaPhanBo;
                return View(sViewPath + "PhanBo_Create.aspx");
            }
            else
            {
                DataTable dt=NguoiDungCauHinhModels.LayCauHinh(MaND);
                int iNamLamViec=DateTime.Now.Year;
                String iID_MaNguonNganSach="",iID_MaNamNganSach="";
                if(dt.Rows.Count>0)
                {
                    iNamLamViec=Convert.ToInt16(dt.Rows[0]["iNamLamViec"]);
                    iID_MaNguonNganSach=Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
                    iID_MaNamNganSach=Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                }
                //Tạo dợt phân bổ'
                SqlCommand cmd = new SqlCommand(@"SELECT iID_MaDotPhanBo FROM PB_DotPhanBo 
                WHERE iTrangThai=1 AND bDotPhanBoTong=1 AND iNamLamViec=@iNamLamViec 
                AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND iID_MaNamNganSach=@iID_MaNamNganSach
                AND dNgayDotPhanBo=@dNgayDotPhanBo");
                cmd.Parameters.AddWithValue("@dNgayDotPhanBo",CommonFunction.LayNgayTuXau(dNgayChungTu));
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                String iID_MaDotPhanBo = Connection.GetValueString(cmd, "");
                //Lấy mã trạng thái trợ lý phòng ban trình duyệt
                int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHePhanBo);
                iID_MaTrangThaiDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(iID_MaTrangThaiDuyet);
                if (String.IsNullOrEmpty(iID_MaDotPhanBo))
                {
                   iID_MaDotPhanBo=TaoDotPhanBoTong(dNgayChungTu, iID_MaNguonNganSach, iID_MaNamNganSach, iNamLamViec);
                }
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                bang.CmdParams.Parameters.AddWithValue("@bPhanBoTong", 1);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu", Guid.Empty.ToString());
                bang.CmdParams.Parameters.AddWithValue("@dNgayDotPhanBo", CommonFunction.LayNgayTuXau(dNgayChungTu));
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", "");
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec",iNamLamViec);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                bang.CmdParams.Parameters.AddWithValue("@bChiNganSach", 1);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                bang.CmdParams.Parameters.AddWithValue("@sDSLNS", "");
                if (Request.Form[ParentID + "_DuLieuMoi"] == "0")
                {
                    bang.GiaTriKhoa = iID_MaPhanBo;
                }
               iID_MaPhanBo=Convert.ToString(bang.Save());
            }
            return RedirectToAction("Edit", "PhanBo_Tong", new { iID_MaPhanBo = iID_MaPhanBo });
        }

        public String TaoDotPhanBoTong(String dNgayDotPhanBo, String iID_MaNguonNganSach, String iID_MaNamNganSach, int iNamLamViec)
        {
            String DanhSachTruong="dNgayDotPhanBo,iID_MaNguonNganSach,iID_MaNamNganSach,iNamLamViec,bDotPhanBoTong";
            String DanhSachGiaTri=String.Format("{0},{1},{2},{3},{4}",dNgayDotPhanBo,iID_MaNguonNganSach,iID_MaNamNganSach,iNamLamViec,1);
            String iID_MaDotPhanBo = "";
            if (HamChung.Check_Trung("PB_DotPhanBo", "iID_MaDotPhanBo", "", DanhSachTruong, DanhSachGiaTri, true) == false)
            {
                Bang bangDotPhanBo = new Bang("PB_DotPhanBo");
                bangDotPhanBo.CmdParams.Parameters.AddWithValue("@dNgayDotPhanBo", CommonFunction.LayNgayTuXau(dNgayDotPhanBo));
                bangDotPhanBo.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                bangDotPhanBo.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                bangDotPhanBo.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                bangDotPhanBo.CmdParams.Parameters.AddWithValue("@bDotPhanBoTong", 1);
                iID_MaDotPhanBo=Convert.ToString(bangDotPhanBo.Save());
            }
            return iID_MaDotPhanBo;
        }

        [Authorize]
        public ActionResult Delete(String iID_MaPhanBo, String MaDotPhanBo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DT_PhanBo", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = PhanBo_PhanBoModels.Delete_PhanBo(iID_MaPhanBo, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "PhanBo_PhanBo", new { MaDotPhanBo = MaDotPhanBo });
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaPhanBo)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = PhanBo_PhanBoChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaPhanBo);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            PhanBo_PhanBoModels.Update_iID_MaTrangThaiDuyet(iID_MaPhanBo, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetPhanBo = PhanBo_PhanBoModels.InsertDuyetPhanBo(iID_MaPhanBo, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetPhanBoCuoiCung", MaDuyetPhanBo);
            PhanBo_PhanBoModels.UpdateRecord(iID_MaPhanBo, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            //update trạng thái trình duyệt của phân bổ tổng nếu phân bổ này là con của phân bổ tổng
            PhanBo_TongModels.UpdateTrinhDuyet(iID_MaPhanBo, MaND, Request.UserHostAddress);

            int iID_MaTrangThaiTuChoi = PhanBo_PhanBoChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaPhanBo);
            return RedirectToAction("PhanBoChiTiet_Frame", "PhanBo_PhanBoChiTiet", new { iID_MaPhanBo = iID_MaPhanBo });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaPhanBo)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = PhanBo_PhanBoChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaPhanBo);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            PhanBo_DuyetPhanBoModels.CapNhapLaiTruong_sSua(iID_MaPhanBo);

            ///Update trạng thái cho bảng chứng từ
            PhanBo_PhanBoModels.Update_iID_MaTrangThaiDuyet(iID_MaPhanBo, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetPhanBo = PhanBo_PhanBoModels.InsertDuyetPhanBo(iID_MaPhanBo, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetPhanBoCuoiCung", MaDuyetPhanBo);
            PhanBo_PhanBoModels.UpdateRecord(iID_MaPhanBo, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("PhanBoChiTiet_Frame", "PhanBo_PhanBoChiTiet", new { iID_MaPhanBo = iID_MaPhanBo });
        }

        [Authorize]
        public ActionResult Duyet()
        {
            return View(sViewPath + "PhanBo_Duyet.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchDuyetSubmit(String ParentID)
        {
            String MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String NgayDotNganSach = Request.Form[ParentID + "_" + NgonNgu.MaDate + "sNgayDotNganSach"];
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            String TrangThaiChungTu = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Duyet", "PhanBo_PhanBo", new { MaPhongBan = MaPhongBan, NgayDotNganSach = NgayDotNganSach, SoChungTu = SoChungTu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = TrangThaiChungTu });
        }

        #region Chuyển năm sau
        [Authorize]
        public ActionResult ChuyenNamSau()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return View(sViewPath + "PhanBo_ChuyenNamSau.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_ChuyenNamSau()
        {
            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            int MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo);
            NganSach_HamChungModels.ChuyenNamSau(MaND, IPSua, MaTrangThaiDuyet, "PB_PhanBo", "PB_PhanBoChiTiet", true, "PB_DotPhanBo", "dNgayDotPhanBo");
            return RedirectToAction("Index");

        }
        #endregion
    }
}
