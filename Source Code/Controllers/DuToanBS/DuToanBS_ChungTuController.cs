using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using DomainModel;
using DomainModel.Abstract;
using VIETTEL.Models;
using VIETTEL.Models.DungChung;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models.DuToanBS;

namespace VIETTEL.Controllers.DuToanBS
{
    public class DuToanBS_ChungTuController : Controller
    {
        #region Hằng Số

        private const string CREATE = "Create";
        private const string EDIT = "Edit";
        private const string DELETE = "Delete";
        private const string CONTROLLER_NAME = "DuToanBS_ChungTu";
        private const string PERMITION_MESSAGE_CONTROLLER = "PermitionMessage";
        private const string VIEW_ROOTPATH = "~/Views/DuToanBS/ChungTu/";
        private const string VIEW_CHUNGTU_INDEX = "ChungTu_Index.aspx";
        private const string VIEW_CHUNGTUBD_INDEX = "ChungTuBD_Index.aspx";
        private const string VIEW_CHUNGTU_EDIT = "ChungTu_Edit.aspx";
        private const string VIEW_CHUNGTU_GOM_INDEX = "ChungTu_Gom_Index.aspx";
        private const string VIEW_CHUNGTU_GOM_EDIT = "ChungTu_Gom_Edit.aspx";
        private const string VIEW_CHUNGTU_GOM_THCUC_INDEX = "ChungTu_Gom_THCuc_Index.aspx";
        private const string VIEW_CHUNGTU_GOM_THCUC_EDIT = "ChungTu_Gom_THCuc_Edit.aspx";
        private const string VIEW_NHAN_KYTHUAT_INDEX = "ChungTu_NhanKyThuat_Index.aspx";
        private const string VIEW_NHAN_KYTHUAT_DETAIL = "ChungTu_NhanKyThuat_Detail.aspx";
        private const string VIEW_GOM_LAN2_INDEX = "ChungTu_GomLan2_Index.aspx";
        private const string VIEW_NHAN_BKHAC_INDEX = "ChungTu_Gom_NhanBKhac_Index.aspx";
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
        public ActionResult Index(string MaDotNganSach, string sLNS1, string iLoai, string iLan, string iKyThuat)
        {
            ViewData["MaDotNganSach"] = MaDotNganSach;
            ViewData["iKyThuat"] = iKyThuat;
            switch (iLoai)
            {
                case "1":
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_INDEX);
                case "2":
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_THCUC_INDEX);
                case "3":
                    return View(VIEW_ROOTPATH + VIEW_NHAN_BKHAC_INDEX);
                case "4":
                    return View(VIEW_ROOTPATH + VIEW_NHAN_KYTHUAT_DETAIL);
                case "5":
                    return View(VIEW_ROOTPATH + VIEW_NHAN_KYTHUAT_INDEX);
                default:
                    if (iLan == "lan2")
                    {
                        return View(VIEW_ROOTPATH + VIEW_GOM_LAN2_INDEX);
                    }
                    if (sLNS1 == "104")
                    {
                        return View(VIEW_ROOTPATH + VIEW_CHUNGTUBD_INDEX);
                    }
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
            string maND = User.Identity.Name;
            //Kiểm tra quyền
            if (String.IsNullOrEmpty(iID_MaChungTu) &&
                !LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanModels.iID_MaPhanHe, maND))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            if (!BaoMat.ChoPhepLamViec(maND, "DTBS_ChungTu", EDIT))
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
        public ActionResult SuaChungTuTLTH(string iID_MaChungTu, string sLNS1)
        {
            string maND = User.Identity.Name;
            //Kiểm tra quyền
            if (String.IsNullOrEmpty(iID_MaChungTu) &&
                !LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanModels.iID_MaPhanHe, maND))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            if (!BaoMat.ChoPhepLamViec(maND, "DTBS_ChungTu_TLTH", EDIT))
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
        public ActionResult SuaChungTuTLTHCuc(string iID_MaChungTu, string sLNS1)
        {
            string maND = User.Identity.Name;
            //Kiểm tra quyền
            if (String.IsNullOrEmpty(iID_MaChungTu) &&
                !LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanModels.iID_MaPhanHe, maND))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            if (!BaoMat.ChoPhepLamViec(maND, "DTBS_ChungTu_TLTHCuc", EDIT))
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
        public ActionResult ThemSuaChungTu(string ParentID, string MaChungTu, string sLNS1, string iKyThuat)
        {
            string maND = User.Identity.Name;
            string sChucNang = EDIT;

            //Xác định trường hợp thêm hay xóa.
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }

            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(maND, "DTBS_ChungTu", sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            string sLNS = Convert.ToString(Request.Form["sLNS"]);
            if (sLNS1 == "104")
            {
                sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            }

            // Khởi tạo và gán giá trị cho bảng dữ liệu
            Bang bang = new Bang("DTBS_ChungTu");
            bang.MaNguoiDungSua = maND;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(ParentID, Request.Form);
            if (sChucNang == CREATE)
            {
                try
                {
                    string maChungTuAddNew = DuToanBS_ChungTuModels.ThemChungTu(bang, maND, sLNS, iKyThuat);
                    return RedirectToAction("Index", "DuToanBS_ChungTuChiTiet", new {iID_MaChungTu = maChungTuAddNew});
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(ParentID + "_" + ers[0], ers[1]);
                    ViewData["bThemMoi"] = "true";
                    if (sLNS1 == "104")
                    {
                        return View(VIEW_ROOTPATH + VIEW_CHUNGTUBD_INDEX);
                    }
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_INDEX);
                }
            }
            else
            {
                try
                {
                    DuToanBS_ChungTuModels.SuaChungTu(bang, MaChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {sLNS1 = sLNS1});
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
        public ActionResult ThemSuaChungTuTLTH(string ParentID, string MaChungTu, string sLNS1)
        {
            string MaND = User.Identity.Name;
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
                    DuToanBS_ChungTuModels.ThemChungTuTLTH(bang, MaND, dsMaChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 1, sLNS1 = sLNS1});
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
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 1, sLNS1 = sLNS1});
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
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 2, sLNS1 = sLNS1});
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
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 2, sLNS1 = sLNS1});
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


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_GomNhanBKhac(String ParentID, String MaChungTu, String sLNS1, String iLan2)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("DTBS_ChungTu_TLTH");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaChungTu = Convert.ToString(Request.Form["iID_MaChungTu"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (iID_MaChungTu == string.Empty || iID_MaChungTu == "" || iID_MaChungTu == null)
            {
                arrLoi.Add("err_iID_MaChungTu", "Không có đợt được chọn!");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
                {
                    arrLoi.Add("err_sLNS", "Không có đợt ngân sách!");
                }
            }

            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["MaChungTu"] = MaChungTu;
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    ViewData["DuLieuMoi"] = "0";
                    ViewData["sLNS"] = sLNS1;
                    return RedirectToAction("index", "DuToanBS_ChungTu_BaoDam", new { iLoai = 3 });

                }
                else
                {
                    ViewData["MaChungTu"] = MaChungTu;
                    ViewData["DuLieuMoi"] = "0";
                    ViewData["sLNS"] = sLNS1;
                    return View(VIEW_ROOTPATH + "ChungTu_Edit.aspx");
                }
            }
            else
            {

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    //lay soChungtuTheoLamLamViec
                    int iSoChungTu = 0;
                    String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
                    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

                    // iSoChungTu = DuToanBS_ChungTu_BaoDamModels.iSoChungTu(iNamLamViec)+1;
                    //bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(DuToanModels.iID_MaPhanHe));
                    // bang.CmdParams.Parameters.AddWithValue("@iSoChungTu", iSoChungTu);
                    String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
                    if (dtCauHinh.Rows.Count > 0)
                    {
                        iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                        iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                        dtCauHinh.Dispose();
                    }
                    DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
                    if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                    {
                        DataRow drPhongBan = dtPhongBan.Rows[0];
                        iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                        sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                        dtPhongBan.Dispose();
                    }
                    String DK = "";
                    if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Guid.Empty.ToString();
                    String[] arrChungtu = iID_MaChungTu.Split(',');
                    SqlCommand cmd = new SqlCommand();
                    for (int j = 0; j < arrChungtu.Length; j++)
                    {
                        DK += " iID_MaChungTu =@iID_MaChungTu" + j;
                        if (j < arrChungtu.Length - 1)
                            DK += " OR ";
                        cmd.Parameters.AddWithValue("@iID_MaChungTu" + j, arrChungtu[j]);

                    }
                    int iID_MaTrangThaiDuyet = 3;
                    ///Update trạng thái check cho bảng chứng từ
                    String SQL = "";
                    //neu gom lan 2
                    if (iLan2 == "1")
                        SQL = @"UPDATE DTBS_ChungTu SET iCheckLan2=1 WHERE iTrangThai=1 AND (" + DK + ")";
                    else
                        SQL = @"UPDATE DTBS_ChungTu SET iCheck=1 WHERE iTrangThai=1 AND (" + DK + ")";
                    cmd.CommandText = SQL;
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();

                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                    bang.CmdParams.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    bang.CmdParams.Parameters.AddWithValue("@iLoai", "3");
                    String MaChungTuAddNew = Convert.ToString(bang.Save());
                    //DuToanBS_ChungTu_BaoDamModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới ", User.Identity.Name, Request.UserHostAddress);

                    return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { iLoai = 3, sLNS = sLNS1 });
                }
                else
                {
                    bang.GiaTriKhoa = MaChungTu;
                    bang.DuLieuMoi = false;
                    bang.Save();
                    return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam", new { iLoai = 3, sLNS = sLNS1 });
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
        public ActionResult TrinhDuyetChungTu(string maChungTu, string sLNS, string iKyThuat, string sLyDo,
            string maChungTuTLTH)
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
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTu(maChungTu, maTrangThaiTiepTheo, true, MaND,
                Request.UserHostAddress);

            //Update trạng thái chứng từ TLTH
            DuToanBS_ChungTuModels.CapNhatChungTuTLTH(maChungTu, MaND);
            ViewData["LoadLai"] = "1";

            if (sLNS.Contains("1040100"))
            {
                return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam",
                    new {sLNS = sLNS, iKyThuat = iKyThuat, iID_MaChungTu = maChungTuTLTH});
            }
            return RedirectToAction("Index", "DuToanBS_ChungTu",
                new {sLNS1 = sLNS, iKyThuat = iKyThuat, iID_MaChungTu = maChungTuTLTH});
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
        public ActionResult TrinhDuyetChungTuTLTH(string maChungTuTLTH, string iLoai, string sLNS,
            string maChungTuTLTHCuc)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = 0;
            if (sLNS.StartsWith("1040100"))
                iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyetTLTHBaoDam(MaND,
                    maChungTuTLTH);
            else
                iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyetTLTH(MaND,
                    maChungTuTLTH);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            //Update trạng thái duyệt
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTuTLTH(maChungTuTLTH, iID_MaTrangThaiDuyet_TrinhDuyet, MaND,
                Request.UserHostAddress);
            DuToanBS_ChungTuModels.CapNhatChungTuTLTHCuc(maChungTuTLTH, MaND);
            if (sLNS == "1040100")
            {
                return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam",
                    new {sLNS = sLNS, iLoai = iLoai, iID_MaChungTu = maChungTuTLTHCuc});
            }
            return RedirectToAction("Index", "DuToanBS_ChungTu",
                new {sLNS1 = sLNS, iLoai = iLoai, iID_MaChungTu = maChungTuTLTHCuc});
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
            string maND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int maTrangThaiTiepTheo = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyetTLTHCuc(maND, maChungTuTLTHCuc);
            if (maTrangThaiTiepTheo <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Update mã trạng thái duyệt
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTuTLTHCuc(maChungTuTLTHCuc, maTrangThaiTiepTheo, maND,
                Request.UserHostAddress);
            return RedirectToAction("Index", "DuToanBS_ChungTu", new {sLNS = sLNS, iLoai = iLoai});
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
            string maND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iIdMaTrangThaiDuyetTuChoi = 0;
            if (sLNS.StartsWith("1040100"))
                iIdMaTrangThaiDuyetTuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoiBaoDam(maND, maChungTu);
            else
                iIdMaTrangThaiDuyetTuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoi(maND, maChungTu);
            if (iIdMaTrangThaiDuyetTuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(sLyDo))
                sLyDo = Convert.ToString(Request.Form["DuToan_sLyDo"]);
            //update ly do
            DuToanBS_ChungTuModels.CapNhatLyDoChungTu(maChungTu, sLyDo);

            //Update mã trạng thái duyệt
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTu(maChungTu, iIdMaTrangThaiDuyetTuChoi, true, maND,
                Request.UserHostAddress);

            //Update chứng từ TLTH
            DuToanBS_ChungTuModels.CapNhatChungTuTLTH(maChungTu, maND);
            if (sLNS.Contains("1040100"))
            {
                return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam",
                    new {sLNS = sLNS, iID_MaChungTu = maChungTuTLTH});
            }
            return RedirectToAction("Index", "DuToanBS_ChungTu", new {sLNS1 = sLNS, iID_MaChungTu = maChungTuTLTH});
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
            string maND = User.Identity.Name;

            //Xác định trạng thái duyệt tiếp theo
            int idMaTrangThaiDuyetTuChoi = 0;
            if (sLNS.StartsWith("1040100"))
                idMaTrangThaiDuyetTuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoiTLTHBaoDam(maND, maChungTuTLTH);
            else
                idMaTrangThaiDuyetTuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoiTLTH(maND, maChungTuTLTH);

            if (idMaTrangThaiDuyetTuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Updatr mã trạng thái duyệt
            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTuTLTH(maChungTuTLTH, idMaTrangThaiDuyetTuChoi, maND,
                Request.UserHostAddress);

            DuToanBS_ChungTuModels.CapNhatChungTuTLTHCuc(maChungTuTLTH, maND);
            if (sLNS == "1040100")
            {
                return RedirectToAction("Index", "DuToanBS_ChungTu_BaoDam",
                    new {sLNS = sLNS, iLoai = iLoai, iID_MaChungTu = maChungTuTLTHCuc});
            }
            return RedirectToAction("Index", "DuToanBS_ChungTu",
                new {sLNS1 = sLNS, iLoai = iLoai, iID_MaChungTu = maChungTuTLTHCuc});
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
            string maND = User.Identity.Name;

            //Xác định trạng thái duyệt tiếp theo
            int maTrangThaiTuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoiTLTHCuc(maND,
                maChungTuTLTHCuc);
            if (maTrangThaiTuChoi <= 0)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            DuToanBS_ChungTuModels.CapNhatTrangThaiDuyetChungTuTLTHCuc(maChungTuTLTHCuc, maTrangThaiTuChoi,
                maND, Request.UserHostAddress);
            return RedirectToAction("Index", "DuToanBS_ChungTu", new {sLNS = sLNS, iLoai = iLoai});
        }

        #endregion
    }
}