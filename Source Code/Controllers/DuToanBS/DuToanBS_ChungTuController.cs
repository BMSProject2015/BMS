using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using DomainModel;
using DomainModel.Abstract;
using VIETTEL.Models;
using VIETTEL.Models.DungChung;
using System.Data.SqlClient;

namespace VIETTEL.Controllers.DuToanBS
{
    public class DuToanBSChungTuController : Controller
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
            switch (iLoai)
            {
                case "1":
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_INDEX);
                case "2":
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_THCUC_INDEX);
                default: 
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_INDEX);
            }
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
            if (!BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu", EDIT))
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
            if (!BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu_TLTH", EDIT))
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
            if (!BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu_TLTHCuc", EDIT))
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
        public ActionResult ThemSuaChungTu(string ParentID, string MaChungTu, string sLNS1)
        {
            string MaND = User.Identity.Name;
            string sChucNang = EDIT;

            //Xác định trường hợp thêm hay xóa.
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }

            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu", sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            string sLNS = Convert.ToString(Request.Form["sLNS"]);
            
            // Khởi tạo và gán giá trị cho bảng dữ liệu
            Bang bang = new Bang("DTBS_ChungTu");
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            if (sChucNang == CREATE)
            {
                try
                {
                    string MaChungTuAddNew = DuToanBS_ChungTuModels.ThemChungTu(bang, MaND, sLNS);
                    return RedirectToAction("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = MaChungTuAddNew });
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(ParentID + "_" + ers[0], ers[1]);
                    ViewData["bThemMoi"] = "true";
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_INDEX);
                }
            }
            else
            {
                try
                {
                    DuToanBS_ChungTuModels.SuaChungTu(bang, MaChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new { sLNS1 = sLNS1 });
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(ParentID + "_" + ers[0], ers[1]);

                    ViewData["MaChungTu"] = MaChungTu;
                    ViewData["sLNS1"] = sLNS1;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_EDIT);
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
        public ActionResult ThemSuaChungTuTLTH(String ParentID, String MaChungTu, String sLNS1)
        {
            String MaND = User.Identity.Name;
            string sChucNang = EDIT;
            //Xác định thêm hay xóa
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }

            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu_TLTH", sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            string dsMaChungTu = Convert.ToString(Request.Form["iID_MaChungTu"]);

            Bang bang = new Bang("DTBS_ChungTu_TLTH");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            if (sChucNang == CREATE)
            {
                try
                {
                    String MaChungTuAddNew = DuToanBS_ChungTuModels.ThemChungTuTLTH(bang, MaND, dsMaChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new { iLoai = 1, sLNS1 = sLNS1 });
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(ParentID + "_" + ers[0], ers[1]);
                    ViewData["bThemMoi"] = "true";
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_INDEX);
                }
            }
            else
            {
                try
                {
                    DuToanBS_ChungTuModels.SuaChungTuTLTH(bang, MaChungTu, dsMaChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new { iLoai = 1, sLNS1 = sLNS1 });
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(ParentID + "_" + ers[0], ers[1]);

                    ViewData["iID_MaChungTu"] = MaChungTu;
                    ViewData["sLNS"] = sLNS1;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_EDIT);
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
        public ActionResult ThemSuaChungTuTLTHCuc(String ParentID, String MaChungTu, String sLNS1)
        {
            String MaND = User.Identity.Name;
            string sChucNang = EDIT;
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu_TLTHCuc", sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            string dsMaChungTuTLTH = Convert.ToString(Request.Form["iID_MaChungTu_TLTH"]);

            Bang bang = new Bang("DTBS_ChungTu_TLTHCuc");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            if (sChucNang == CREATE)
            {
                try
                {
                    DuToanBS_ChungTuModels.ThemChungTuTLTHCuc(bang, MaND, dsMaChungTuTLTH);
                    return RedirectToAction("Index", CONTROLLER_NAME, new { iLoai = 2, sLNS1 = sLNS1 });
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(ParentID + "_" + ers[0], ers[1]);
                    ViewData["bThemMoi"] = "true";
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_THCUC_INDEX);
                }
            }
            else
            {
                try
                {
                    DuToanBS_ChungTuModels.SuaChungTuTLTHCuc(bang, MaChungTu, dsMaChungTuTLTH);
                    return RedirectToAction("Index", CONTROLLER_NAME, new { iLoai = 2, sLNS1 = sLNS1 });
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(ParentID + "_" + ers[0], ers[1]);
                    ViewData["iID_MaChungTu"] = MaChungTu;
                    ViewData["sLNS"] = sLNS1;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_THCUC_EDIT);
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
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTu", DELETE) == false)
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
            if (!BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTu_TLTH", DELETE))
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
            if (!BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTu_TLTHCuc", DELETE))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            DuToanBS_ChungTuModels.XoaChungTuTLTHCuc(iID_MaChungTu);
            return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 2, sLNS1 = sLNS1});
        }

        #endregion

        #region Trình Duyệt
        /// <summary>
        /// Trình duyệt chứng từ
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <param name="iLoai"></param>
        /// <param name="sLNS"></param>
        /// <param name="iKyThuat"></param>
        /// <param name="sLyDo"></param>
        /// <param name="maChungTuTLTH"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TrinhDuyetChungTu(string maChungTu, string sLNS, string iKyThuat, string sLyDo, string maChungTuTLTH)
        {
            string MaND = User.Identity.Name;

            //Xác định trạng thái duyệt tiếp theo
            int maTrangThaiTiepTheo = 0;
            if (sLNS.StartsWith("1040100") && iKyThuat == "1")
                maTrangThaiTiepTheo = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyetBaoDam(MaND, maChungTu);
            else
                maTrangThaiTiepTheo = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyet(MaND, maChungTu);

            if (maTrangThaiTiepTheo <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            if (String.IsNullOrEmpty(sLyDo))
                sLyDo = Convert.ToString(Request.Form["DuToan_sLyDo"]);

            //update ly do
            DuToanBS_ChungTuModels.CapNhatLyDoChungTu(maChungTu, sLyDo);
            ///Update trạng thái cho bảng chứng từ
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTu(maChungTu, maTrangThaiTiepTheo, true, MaND, Request.UserHostAddress);

            //Update trạng thái chứng từ TLTH
            DuToanBS_ChungTuModels.CapNhatChungTuTLTH(maChungTu, MaND);
            ViewData["LoadLai"] = "1";
            
            if (sLNS.Contains("1040100"))
            {
                return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { sLNS = sLNS, iKyThuat = iKyThuat, iID_MaChungTu = maChungTuTLTH });
            }
            return RedirectToAction("Index", "DuToanBS_ChungTu", new { sLNS1 = sLNS, iKyThuat = iKyThuat, iID_MaChungTu = maChungTuTLTH });
        }

        /// <summary>
        /// Trình duyệt chứng từ TLTH
        /// </summary>
        /// <param name="maChungTuTLTH"></param>
        /// <param name="iLoai"></param>
        /// <param name="sLNS"></param>
        /// <param name="maChungTuTLTHCuc"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TrinhDuyetChungTuTLTH(string maChungTuTLTH, string iLoai, string sLNS, string maChungTuTLTHCuc)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = 0;
            if (sLNS.StartsWith("1040100"))
                iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyetTLTHBaoDam(MaND, maChungTuTLTH);
            else
                iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyetTLTH(MaND, maChungTuTLTH);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            //Update trạng thái duyệt
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTuTLTH(maChungTuTLTH, iID_MaTrangThaiDuyet_TrinhDuyet, MaND, Request.UserHostAddress);
            DuToanBS_ChungTuModels.CapNhatChungTuTLTHCuc(maChungTuTLTH, MaND);
            if (sLNS == "1040100")
            {
                return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { sLNS = sLNS, iLoai = iLoai, iID_MaChungTu = maChungTuTLTHCuc });
            }
            return RedirectToAction("Index", "DuToanBS_ChungTu", new { sLNS1 = sLNS, iLoai = iLoai, iID_MaChungTu = maChungTuTLTHCuc });
        }

        /// <summary>
        /// Trình duyệt chứng từ TLTH Cục
        /// </summary>
        /// <param name="maChungTuTLTHCuc"></param>
        /// <param name="iLoai"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TrinhDuyetChungTuTLTHCuc(string maChungTuTLTHCuc, string iLoai, string sLNS)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int maTrangThaiTiepTheo = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyetTLTHCuc(MaND, maChungTuTLTHCuc);
            if (maTrangThaiTiepTheo <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Update mã trạng thái duyệt
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTuTLTHCuc(maChungTuTLTHCuc, maTrangThaiTiepTheo, MaND, Request.UserHostAddress);
            return RedirectToAction("Index", "DuToanBS_ChungTu", new { sLNS = sLNS, iLoai = iLoai });
        } 
        #endregion

        #region Từ Chối
        /// <summary>
        /// Từ chối chứng từ
        /// </summary>
        /// <param name="maChungTu"></param>
        /// <param name="iLoai"></param>
        /// <param name="sLNS"></param>
        /// <param name="sLyDo"></param>
        /// <param name="maChungTuTLTH"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TuChoiChungTu(string maChungTu, string iLoai, string sLNS, string sLyDo, string maChungTuTLTH)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = 0;
            if (sLNS.StartsWith("1040100"))
                iID_MaTrangThaiDuyet_TuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoiBaoDam(MaND, maChungTu);
            else
                iID_MaTrangThaiDuyet_TuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoi(MaND, maChungTu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(sLyDo))
                sLyDo = Convert.ToString(Request.Form["DuToan_sLyDo"]);
            //update ly do
            DuToanBS_ChungTuModels.CapNhatLyDoChungTu(maChungTu, sLyDo);

            //Update mã trạng thái duyệt
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTu(maChungTu, iID_MaTrangThaiDuyet_TuChoi, true, MaND, Request.UserHostAddress);

            //Update chứng từ TLTH
            DuToanBS_ChungTuModels.CapNhatChungTuTLTH(maChungTu, MaND);
            if (sLNS.Contains("1040100"))
            {
                return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { sLNS = sLNS, iID_MaChungTu = maChungTuTLTH });
            }
            return RedirectToAction("Index", "DuToanBS_ChungTu", new { sLNS1 = sLNS, iID_MaChungTu = maChungTuTLTH });
        }

        /// <summary>
        /// Từ chối chứng từ TLTH
        /// </summary>
        /// <param name="maChungTuTLTH"></param>
        /// <param name="iLoai"></param>
        /// <param name="sLNS"></param>
        /// <param name="maChungTuTLTHCuc"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TuChoiChungTuTLTH(string maChungTuTLTH, string iLoai, string sLNS, string maChungTuTLTHCuc)
        {
            string MaND = User.Identity.Name;
            
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = 0;
            if (sLNS.StartsWith("1040100"))
                iID_MaTrangThaiDuyet_TuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoiTLTHBaoDam(MaND, maChungTuTLTH);
            else
                iID_MaTrangThaiDuyet_TuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoiTLTH(MaND, maChungTuTLTH);
            
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Updatr mã trạng thái duyệt
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTuTLTH(maChungTuTLTH, iID_MaTrangThaiDuyet_TuChoi, MaND, Request.UserHostAddress);

            DuToanBS_ChungTuModels.CapNhatChungTuTLTHCuc(maChungTuTLTH, MaND);
            if (sLNS == "1040100")
            {
                return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { sLNS = sLNS, iLoai = iLoai, iID_MaChungTu = maChungTuTLTHCuc });
            }
                return RedirectToAction("Index", "DuToanBS_ChungTu", new { sLNS1 = sLNS, iLoai = iLoai, iID_MaChungTu = maChungTuTLTHCuc });
        }

        /// <summary>
        /// Từ chối chứng từ TLTH Cục
        /// </summary>
        /// <param name="maChungTuTLTHCuc"></param>
        /// <param name="iLoai"></param>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult TuChoiChungTuTLTHCuc(string maChungTuTLTHCuc, string iLoai, string sLNS)
        {
            string MaND = User.Identity.Name;
            
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoiTLTHCuc(MaND, maChungTuTLTHCuc);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTuTLTHCuc(maChungTuTLTHCuc, iID_MaTrangThaiDuyet_TrinhDuyet, MaND, Request.UserHostAddress);
            return RedirectToAction("Index", "DuToanBS_ChungTu", new { sLNS = sLNS, iLoai = iLoai });
        } 
        #endregion
    }
}