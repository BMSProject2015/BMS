using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using DomainModel;
using DomainModel.Abstract;
using VIETTEL.Models;
using VIETTEL.Models.DungChung;

namespace VIETTEL.Controllers.DuToanBS
{
    public class DuToanBS_ChungTuController : Controller
    {
        #region Hằng Số
        public static readonly string CREATE = "Create";
        public static readonly string EDIT = "Edit";
        public static readonly string DELETE = "Delete";
        public static readonly string CONTROLLER_NAME = "DuToanBS_ChungTu";
        public static readonly string PERMITION_MESSAGE_CONTROLLER = "PermitionMessage";
        public static readonly string VIEW_ROOTPATH = "~/Views/DuToanBS/ChungTu/";
        public static readonly string VIEW_CHUNGTU_INDEX = "ChungTu_Index.aspx";
        public static readonly string VIEW_CHUNGTU_EDIT = "ChungTu_Edit.aspx";
        public static readonly string VIEW_CHUNGTU_GOM_INDEX = "ChungTu_Gom_Index.aspx";
        public static readonly string VIEW_CHUNGTU_GOM_EDIT = "ChungTu_Gom_Edit.aspx";
        public static readonly string VIEW_CHUNGTU_GOM_THCUC_INDEX = "ChungTu_Gom_THCuc_Index.aspx";
        public static readonly string VIEW_CHUNGTU_GOM_THCUC_EDIT = "ChungTu_Gom_THCuc_Edit.aspx";
        #endregion

        #region Index

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaDotNganSach"></param>
        /// <param name="sLNS1">LNS 1</param>
        /// <param name="iLoai">Loại xử lý</param>
        /// <returns>View</returns>
        [Authorize]
        public ActionResult Index(string MaDotNganSach, string sLNS1, string iLoai)
        {
            ViewData["MaDotNganSach"] = MaDotNganSach;
            if (iLoai == "1")
            {
                return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_INDEX);
            }
            else if (iLoai == "2")
            {
                return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_THCUC_INDEX);
            }
            return View(VIEW_ROOTPATH + VIEW_CHUNGTU_INDEX);
        }

        #endregion

        #region Tìm Kiếm Chứng Từ

        /// <summary>
        /// Tìm kiếm chứng từ
        /// </summary>
        /// <param name="ParentID">Parent ID</param>
        /// <param name="sLNS1">LNS 1</param>
        /// <param name="iLoai">Loại xử lý</param>
        /// <param name="iID_MaChungTu_TLTH">Mã chứng từ TLTH</param>
        /// <returns>RedirectToAction</returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TimKiemChungTu(string ParentID, string sLNS1, string iLoai, string iID_MaChungTu_TLTH)
        {
            string TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            string DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            string SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
            string sLNS_TK = Request.Form[ParentID + "_sLNS_TK"];
            string iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            return RedirectToAction("Index", CONTROLLER_NAME,
                new
                {
                    sLNS1 = sLNS1,
                    iLoai = iLoai,
                    SoChungTu = SoChungTu,
                    TuNgay = TuNgay,
                    DenNgay = DenNgay,
                    iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,
                    sLNS_TK = sLNS_TK,
                    iID_MaChungTu = iID_MaChungTu_TLTH
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="ChiNganSach"></param>
        /// <returns></returns>
        //[Authorize]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult SearchDuyetSubmit(String ParentID, String ChiNganSach)
        //{
        //    string MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
        //    string MaLoaiNganSach = Request.Form[ParentID + "_sLNS"];
        //    string NgayDotNganSach = Request.Form[ParentID + "_" + NgonNgu.MaDate + "sNgayDotNganSach"];
        //    string TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
        //    string DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
        //    string SoChungTu = Request.Form[ParentID + "_iSoChungTu"];
        //    string TrangThaiChungTu = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

        //    return RedirectToAction("Duyet", "DuToanBS_ChungTu",
        //        new
        //        {
        //            ChiNganSach = ChiNganSach,
        //            MaPhongBan = MaPhongBan,
        //            MaLoaiNganSach = MaLoaiNganSach,
        //            NgayDotNganSach = NgayDotNganSach,
        //            SoChungTu = SoChungTu,
        //            TuNgay = TuNgay,
        //            DenNgay = DenNgay,
        //            iID_MaTrangThaiDuyet = TrangThaiChungTu
        //        });
        //}

        #endregion

        #region Chuyển Page Sửa Chứng Từ

        /// <summary>
        /// Chuyển đến page sửa chứng từ
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SuaChungTu(string iID_MaChungTu, string sLNS1)
        {
            String MaND = User.Identity.Name;
            //Kiểm tra quyền
            if (String.IsNullOrEmpty(iID_MaChungTu) &&
                !LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanModels.iID_MaPhanHe, MaND))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            if (!BaoMat.ChoPhepLamViec(MaND, BangModels.DTBS_CHUNGTU, EDIT))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            ViewData["MaChungTu"] = iID_MaChungTu;
            ViewData["sLNS1"] = sLNS1;
            //Go to edit page
            return View(VIEW_ROOTPATH + VIEW_CHUNGTU_EDIT);
        }

        /// <summary>
        /// Chuyển đến page sửa chứng từ TLTH
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SuaChungTuTLTH(String iID_MaChungTu, String sLNS1)
        {
            String MaND = User.Identity.Name;
            //Kiểm tra quyền
            if (String.IsNullOrEmpty(iID_MaChungTu) &&
                !LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanModels.iID_MaPhanHe, MaND))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            if (!BaoMat.ChoPhepLamViec(MaND, BangModels.DTBS_ChungTu_TLTH, EDIT))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["sLNS1"] = sLNS1;
            return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_EDIT);
        }

        /// <summary>
        /// Chuyển đến page sửa chứng từ TLTHCuc
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult SuaChungTuTLTHCuc(String iID_MaChungTu, String sLNS1)
        {
            String MaND = User.Identity.Name;
            //Kiểm tra quyền
            if (String.IsNullOrEmpty(iID_MaChungTu) &&
                !LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanModels.iID_MaPhanHe, MaND))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            if (!BaoMat.ChoPhepLamViec(MaND, BangModels.DTBS_ChungTu_TLTHCuc, EDIT))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["sLNS1"] = sLNS1;
            return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_THCUC_EDIT);
        }

        #endregion

        #region Thêm/Sửa Chứng Từ

        /// <summary>
        /// Thêm/Sửa Chứng từ
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(string ParentID, string MaChungTu, string sLNS1)
        {
            string MaND = User.Identity.Name;
            string sChucNang = EDIT;

            //Xác định trường hợp thêm hay xóa.
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }

            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, BangModels.DTBS_CHUNGTU, sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            //Kiểm tra dữ liệu
            NameValueCollection Errors = new NameValueCollection();
            string sLNS = Convert.ToString(Request.Form["sLNS"]);
            string NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (String.IsNullOrEmpty(NgayChungTu))
            {
                Errors.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (sChucNang == CREATE)
            {
                if (sLNS == string.Empty || sLNS == "" || sLNS == null)
                {
                    Errors.Add("err_sLNS", "Bạn chưa chọn LNS!");
                }
            }

            //Có lỗi xảy ra
            if (Errors.Count > 0)
            {
                for (int i = 0; i <= Errors.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + Errors.GetKey(i), Errors[i]);
                }
                if (sChucNang == CREATE)
                {
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_INDEX);
                }
                else
                {
                    ViewData["MaChungTu"] = MaChungTu;
                    ViewData["sLNS1"] = sLNS1;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_EDIT);
                }
            }
            //Không có lỗi, update dữ liệu
            else
            {
                // Khởi tạo và gán giá trị cho bảng dữ liệu
                Bang bang = new Bang("DTBS_ChungTu");
                bang.MaNguoiDungSua = MaND;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (sChucNang == CREATE)
                {
                    string MaChungTuAddNew = DuToanBS_ChungTuModels.ThemChungTu(bang, MaND, sLNS);
                    return RedirectToAction("Index", "DuToanBS_ChungTuChiTiet", new {iID_MaChungTu = MaChungTuAddNew});
                }
                else
                {
                    DuToanBS_ChungTuModels.SuaChungTu(bang, MaChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {sLNS1 = sLNS1});
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_Gom(String ParentID, String MaChungTu, String sLNS1)
        {
            String MaND = User.Identity.Name;
            string sChucNang = EDIT;
            //Xác định thêm hay xóa
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }

            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, BangModels.DTBS_ChungTu_TLTH, sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            //Validate.
            NameValueCollection Errors = new NameValueCollection();
            string dsMaChungTu = Convert.ToString(Request.Form["iID_MaChungTu"]);
            string NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);

            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                Errors.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (String.IsNullOrEmpty(dsMaChungTu))
            {
                Errors.Add("err_ChungTu", "Không có đợt được chọn!");
            }

            if (Errors.Count > 0)
            {
                for (int i = 0; i <= Errors.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + Errors.GetKey(i), Errors[i]);
                }
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    return RedirectToAction("index", CONTROLLER_NAME, new {iLoai = 1, sLNS1 = sLNS1});
                }
                else
                {
                    ViewData["iID_MaChungTu"] = MaChungTu;
                    ViewData["sLNS"] = sLNS1;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_EDIT);
                }
            }
            else
            {
                Bang bang = new Bang(BangModels.DTBS_ChungTu_TLTH);
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (sChucNang == CREATE)
                {
                    String MaChungTuAddNew = DuToanBS_ChungTuModels.ThemChungTuTLTH(bang, MaND, dsMaChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 1, sLNS1 = sLNS1});
                }
                else
                {
                    DuToanBS_ChungTuModels.SuaChungTuTLTH(bang, MaChungTu, dsMaChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 1, sLNS1 = sLNS1});
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_Gom_THCuc(String ParentID, String MaChungTu, String sLNS1)
        {
            String MaND = User.Identity.Name;
            string sChucNang = EDIT;
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, BangModels.DTBS_ChungTu_TLTHCuc, sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            int i;
            NameValueCollection Errors = new NameValueCollection();
            string dsMaChungTuTLTH = Convert.ToString(Request.Form["iID_MaChungTu_TLTH"]);
            string NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                Errors.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (String.IsNullOrEmpty(dsMaChungTuTLTH))
            {
                Errors.Add("err_iID_MaChungTu", "Không có đợt được chọn!");
            }

            if (Errors.Count > 0)
            {
                for (i = 0; i <= Errors.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + Errors.GetKey(i), Errors[i]);
                }
                if (sChucNang == CREATE)
                {
                    return RedirectToAction("index", CONTROLLER_NAME, new {iLoai = 2, sLNS1 = sLNS1});
                }
                else
                {
                    ViewData["MaChungTu"] = MaChungTu;
                    ViewData["sLNS"] = sLNS1;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_THCUC_EDIT);
                }
            }
            else
            {
                Bang bang = new Bang(BangModels.DTBS_ChungTu_TLTHCuc);
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (sChucNang == CREATE)
                {
                    DuToanBS_ChungTuModels.ThemChungTuTLTHCuc(bang, MaND, dsMaChungTuTLTH);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 2, sLNS1 = sLNS1});
                }
                else
                {
                    DuToanBS_ChungTuModels.SuaChungTuTLTHCuc(bang, MaChungTu, dsMaChungTuTLTH);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 2, sLNS1 = sLNS1});
                }
            }
        }

        #endregion

        #region Xóa Chứng Từ

        /// <summary>
        /// Xóa chứng từ dự toán bổ sung
        /// </summary>
        /// <param name="iID_MaChungTu">Mã chứng từ</param>
        /// <param name="MaDotNganSach">Mã đợt ngân sách</param>
        /// <param name="ChiNganSach">Chi ngân sách</param>
        /// <param name="sLNS1">LNS 1</param>
        /// <returns>RedirectToAction</returns>
        [Authorize]
        public ActionResult XoaChungTu(String iID_MaChungTu, String MaDotNganSach, String ChiNganSach, String sLNS1)
        {
            //Kiểm tra quyền
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, BangModels.DTBS_CHUNGTU, DELETE) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            DuToanBS_ChungTuModels.XoaChungTu(iID_MaChungTu);
            return RedirectToAction("Index", CONTROLLER_NAME,
                new {MaDotNganSach = MaDotNganSach, ChiNganSach = ChiNganSach, sLNS1 = sLNS1});
        }

        /// <summary>
        /// Xóa chứng từ TLTH
        /// </summary>
        /// <param name="iID_MaChungTu">Mã chứng từ</param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult XoaChungTuTLTH(String iID_MaChungTu, String sLNS1)
        {
            if (!BaoMat.ChoPhepLamViec(User.Identity.Name, BangModels.DTBS_ChungTu_TLTH, DELETE))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            DuToanBS_ChungTuModels.XoaChungTuTLTH(iID_MaChungTu);
            return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 1, sLNS1 = sLNS1});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult XoaChungTuTLTHCuc(String iID_MaChungTu, String sLNS1)
        {
            if (!BaoMat.ChoPhepLamViec(User.Identity.Name, BangModels.DTBS_ChungTu_TLTHCuc, DELETE))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            DuToanBS_ChungTuModels.XoaChungTuTLTHCuc(iID_MaChungTu);
            return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 2, sLNS1 = sLNS1});
        }

        #endregion

        /// <summary>
        /// Duyệt chứng từ
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Duyet()
        {
            return View(VIEW_ROOTPATH + "ChungTu_Duyet.aspx");
        }
    }
}