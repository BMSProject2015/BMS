using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using DomainModel;
using DomainModel.Abstract;
using VIETTEL.Models;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models.DuToanBS;

namespace VIETTEL.Controllers.DuToanBS
{
    public class DuToanBSChungTuController : Controller
    {
        #region Hằng Số

        private const string CREATE = "Create";
        private const string EDIT = "Edit";
        private const string DELETE = "Delete";
        private const string CONTROLLER_NAME = "DuToanBSChungTu";
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
        /// <param name="parentID">Parent ID</param>
        /// <param name="sLNS1">LNS 1</param>
        /// <param name="iLoai">Loại xử lý</param>
        /// <param name="maChungTu">Mã chứng từ TLTH</param>
        /// <returns>RedirectToAction</returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TimKiemChungTu(string parentID, string sLNS1, string iLoai, string maChungTu)
        {
            string TuNgay = Request.Form[parentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            string DenNgay = Request.Form[parentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            string sLNS_TK = Request.Form[parentID + "_ddlLNStk"];
            string iID_MaTrangThaiDuyet = Request.Form[parentID + "_ddlIDMaTrangThai"];
            return RedirectToAction("Index", CONTROLLER_NAME,
                new
                {
                    sLNS1 = sLNS1,
                    iLoai = iLoai,
                    TuNgay = TuNgay,
                    DenNgay = DenNgay,
                    iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,
                    sLNS_TK = sLNS_TK,
                    iID_MaChungTu = maChungTu
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
        /// <param name="parentID"></param>
        /// <param name="maChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ThemSuaChungTu(string parentID, string maChungTu, string sLNS1, string iKyThuat)
        {
            string maND = User.Identity.Name;
            string sChucNang = EDIT;

            //Xác định trường hợp thêm hay xóa.
            if (Request.Form[parentID + "_DuLieuMoi"] == "1")
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
                sLNS = Convert.ToString(Request.Form[parentID + "_sLNS"]);
            }
            string dNgayChungTu = Convert.ToString(Request.Form[parentID + "_vidNgayChungTu"]);
            // Khởi tạo và gán giá trị cho bảng dữ liệu
            Bang bang = new Bang("DTBS_ChungTu");
            bang.MaNguoiDungSua = maND;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(parentID, Request.Form);
            if (sChucNang == CREATE)
            {
                try
                {
                    string maChungTuAddNew = DuToanBSChungTuModels.ThemChungTu(bang, maND, sLNS, iKyThuat,dNgayChungTu);
                    return RedirectToAction("Index", "DuToanBSChungTuChiTiet", new {iID_MaChungTu = maChungTuAddNew});
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(parentID + "_" + ers[0], ers[1]);
                    ViewData["sLNS"] = sLNS;
                    ViewData["bThemMoi"] = "true";
                    ViewData["dNgayChungTu"] = dNgayChungTu;
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
                    DuToanBSChungTuModels.SuaChungTu(bang, maChungTu,dNgayChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {sLNS1 = sLNS1});
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(parentID + "_" + ers[0], ers[1]);

                    ViewData["MaChungTu"] = maChungTu;
                    ViewData["sLNS1"] = sLNS1;
                    ViewData["sLNS"] = sLNS;
                    ViewData["dNgayChungTu"] = dNgayChungTu;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_EDIT);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="maChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ThemSuaChungTuTLTH(string parentID, string maChungTu, string sLNS1)
        {
            string MaND = User.Identity.Name;
            string sChucNang = EDIT;
            //Xác định thêm hay xóa
            if (Request.Form[parentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }

            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu_TLTH", sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            string dsMaChungTu = Convert.ToString(Request.Form["iID_MaChungTu"]);
            string dNgayChungTu = Convert.ToString(Request.Form[parentID + "_vidNgayChungTu"]);

            Bang bang = new Bang("DTBS_ChungTu_TLTH");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(parentID, Request.Form);
            if (sChucNang == CREATE)
            {
                try
                {
                    DuToanBSChungTuModels.ThemChungTuTLTH(bang, MaND, dsMaChungTu, dNgayChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 1, sLNS1 = sLNS1});
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(parentID + "_" + ers[0], ers[1]);
                    ViewData["bThemMoi"] = "true";
                    ViewData["dNgayChungTu"] = dNgayChungTu;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_INDEX);
                }
            }
            else
            {
                try
                {
                    DuToanBSChungTuModels.SuaChungTuTLTH(bang, maChungTu, dsMaChungTu, dNgayChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 1, sLNS1 = sLNS1});
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(parentID + "_" + ers[0], ers[1]);

                    ViewData["iID_MaChungTu"] = maChungTu;
                    ViewData["sLNS"] = sLNS1;
                    ViewData["dNgayChungTu"] = dNgayChungTu;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_EDIT);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="maChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ThemSuaChungTuTLTHCuc(string parentID, string maChungTu, string sLNS1)
        {
            String MaND = User.Identity.Name;
            string sChucNang = EDIT;
            if (Request.Form[parentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = CREATE;
            }
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, "DTBS_ChungTu_TLTHCuc", sChucNang) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            string dsMaChungTuTLTH = Convert.ToString(Request.Form["iID_MaChungTu_TLTH"]);
            string dNgayChungTu = Convert.ToString(Request.Form[parentID + "_vidNgayChungTu"]);

            Bang bang = new Bang("DTBS_ChungTu_TLTHCuc");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.TruyenGiaTri(parentID, Request.Form);
            if (sChucNang == CREATE)
            {
                try
                {
                    DuToanBSChungTuModels.ThemChungTuTLTHCuc(bang, MaND, dsMaChungTuTLTH, dNgayChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 2, sLNS1 = sLNS1});
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(parentID + "_" + ers[0], ers[1]);
                    ViewData["bThemMoi"] = "true";
                    ViewData["dNgayChungTu"] = dNgayChungTu;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_THCUC_INDEX);
                }
            }
            else
            {
                try
                {
                    DuToanBSChungTuModels.SuaChungTuTLTHCuc(bang, maChungTu, dsMaChungTuTLTH, dNgayChungTu);
                    return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 2, sLNS1 = sLNS1});
                }
                catch (Exception ex)
                {
                    string er = ex.Message;
                    string[] ers = er.Split('|');
                    ModelState.AddModelError(parentID + "_" + ers[0], ers[1]);
                    ViewData["iID_MaChungTu"] = maChungTu;
                    ViewData["sLNS"] = sLNS1;
                    ViewData["dNgayChungTu"] = dNgayChungTu;
                    return View(VIEW_ROOTPATH + VIEW_CHUNGTU_GOM_THCUC_EDIT);
                }
            }
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_GomNhanBKhac(string parentID, string maChungTu, string sLNS1, string iLan2)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[parentID + "_DuLieuMoi"] == "1")
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
            String NgayChungTu = Convert.ToString(Request.Form[parentID + "_vidNgayChungTu"]);
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (iID_MaChungTu == string.Empty || iID_MaChungTu == "" || iID_MaChungTu == null)
            {
                arrLoi.Add("err_iID_MaChungTu", "Không có đợt được chọn!");
            }
            if (Request.Form[parentID + "_DuLieuMoi"] == "1")
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
                    ModelState.AddModelError(parentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["MaChungTu"] = maChungTu;
                if (Request.Form[parentID + "_DuLieuMoi"] == "1")
                {
                    ViewData["DuLieuMoi"] = "0";
                    ViewData["sLNS"] = sLNS1;
                    return RedirectToAction("index", "DuToanBSChungTu", new { iLoai = 3 });

                }
                else
                {
                    ViewData["MaChungTu"] = maChungTu;
                    ViewData["DuLieuMoi"] = "0";
                    ViewData["sLNS"] = sLNS1;
                    return View(VIEW_ROOTPATH + "ChungTu_Edit.aspx");
                }
            }
            else
            {

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(parentID, Request.Form);
                if (Request.Form[parentID + "_DuLieuMoi"] == "1")
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

                    return RedirectToAction("Index", "DuToanBSChungTu", new { iLoai = 3, sLNS = sLNS1 });
                }
                else
                {
                    bang.GiaTriKhoa = maChungTu;
                    bang.DuLieuMoi = false;
                    bang.Save();
                    return RedirectToAction("Index", "DuToanBSChungTu", new { iLoai = 3, sLNS = sLNS1 });
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
        public ActionResult XoaChungTu(string iID_MaChungTu, string MaDotNganSach, string ChiNganSach, string sLNS1)
        {
            //Kiểm tra quyền
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTu", DELETE) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            DuToanBSChungTuModels.XoaChungTu(iID_MaChungTu);
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
        public ActionResult XoaChungTuTLTH(string iID_MaChungTu, string sLNS1)
        {
            if (!BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTu_TLTH", DELETE))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            DuToanBSChungTuModels.XoaChungTuTLTH(iID_MaChungTu);
            return RedirectToAction("Index", CONTROLLER_NAME, new {iLoai = 1, sLNS1 = sLNS1});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="sLNS1"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult XoaChungTuTLTHCuc(string iID_MaChungTu, string sLNS1)
        {
            if (!BaoMat.ChoPhepLamViec(User.Identity.Name, "DTBS_ChungTu_TLTHCuc", DELETE))
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }
            DuToanBSChungTuModels.XoaChungTuTLTHCuc(iID_MaChungTu);
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
        public ActionResult TrinhDuyetChungTu(string maChungTu, string sLNS, string iKyThuat, string sLyDo, string maChungTuTLTH, string chiTiet)
        {
            string MaND = User.Identity.Name;

            //Xác định trạng thái duyệt tiếp theo
            int maTrangThaiTiepTheo = 0;
            if (sLNS.StartsWith("1040100") && iKyThuat == "1")
                maTrangThaiTiepTheo = DuToanBSChungTuModels.LayMaTrangThaiTrinhDuyetBaoDam(MaND, maChungTu);
            else
                maTrangThaiTiepTheo = DuToanBSChungTuModels.LayMaTrangThaiTrinhDuyet(MaND, maChungTu);

            if (maTrangThaiTiepTheo <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            if (String.IsNullOrEmpty(sLyDo))
                sLyDo = Convert.ToString(Request.Form["DuToan_sLyDo"]);

            //update ly do
            DuToanBSChungTuModels.CapNhatLyDoChungTu(maChungTu, sLyDo);
            ///Update trạng thái cho bảng chứng từ
            DuToanBSChungTuModels.CapNhatTrangThaiDuyetChungTu(maChungTu, maTrangThaiTiepTheo, true, MaND,
                Request.UserHostAddress);

            //Update trạng thái chứng từ TLTH
            DuToanBSChungTuModels.CapNhatChungTuTLTH(maChungTu, MaND);
            ViewData["LoadLai"] = "1";
            if (!String.IsNullOrEmpty(chiTiet) && chiTiet == "1")
            {
                return RedirectToAction("ChungTuChiTietFrame", "DuToanBSChungTuChiTiet", new { iID_MaChungTu = maChungTu });
            }
            return RedirectToAction("Index", "DuToanBSChungTu",
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
            iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBSChungTuModels.LayMaTrangThaiTrinhDuyetTLTH(MaND,
                    maChungTuTLTH);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            //Update trạng thái duyệt
            DuToanBSChungTuModels.CapNhatTrangThaiDuyetChungTuTLTH(maChungTuTLTH, iID_MaTrangThaiDuyet_TrinhDuyet, MaND,
                Request.UserHostAddress);
            DuToanBSChungTuModels.CapNhatChungTuTLTHCuc(maChungTuTLTH, MaND);
            
            return RedirectToAction("Index", "DuToanBSChungTu",
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
            int maTrangThaiTiepTheo = DuToanBSChungTuModels.LayMaTrangThaiTrinhDuyetTLTHCuc(maND, maChungTuTLTHCuc);
            if (maTrangThaiTiepTheo <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Update mã trạng thái duyệt
            DuToanBSChungTuModels.CapNhatTrangThaiDuyetChungTuTLTHCuc(maChungTuTLTHCuc, maTrangThaiTiepTheo, maND,
                Request.UserHostAddress);
            return RedirectToAction("Index", "DuToanBSChungTu", new {sLNS = sLNS, iLoai = iLoai});
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
        public ActionResult TuChoiChungTu(string maChungTu, string iLoai, string sLNS, string sLyDo, string maChungTuTLTH, string chiTiet)
        {
            string maND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iIdMaTrangThaiDuyetTuChoi = 0;
            if (sLNS.StartsWith("1040100"))
                iIdMaTrangThaiDuyetTuChoi = DuToanBSChungTuModels.LayMaTrangThaiTuChoiBaoDam(maND, maChungTu);
            else
                iIdMaTrangThaiDuyetTuChoi = DuToanBSChungTuModels.LayMaTrangThaiTuChoi(maND, maChungTu);
            if (iIdMaTrangThaiDuyetTuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(sLyDo))
                sLyDo = Convert.ToString(Request.Form["DuToan_sLyDo"]);
            //update ly do
            DuToanBSChungTuModels.CapNhatLyDoChungTu(maChungTu, sLyDo);

            //Update mã trạng thái duyệt
            DuToanBSChungTuModels.CapNhatTrangThaiDuyetChungTu(maChungTu, iIdMaTrangThaiDuyetTuChoi, true, maND,
                Request.UserHostAddress);

            //Update chứng từ TLTH
            DuToanBSChungTuModels.CapNhatChungTuTLTH(maChungTu, maND);
            if (!String.IsNullOrEmpty(chiTiet) && chiTiet == "1")
            {
                return RedirectToAction("ChungTuChiTietFrame", "DuToanBSChungTuChiTiet", new { iID_MaChungTu = maChungTu });
            }
            return RedirectToAction("Index", "DuToanBSChungTu", new {sLNS1 = sLNS, iID_MaChungTu = maChungTuTLTH});
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
            idMaTrangThaiDuyetTuChoi = DuToanBSChungTuModels.LayMaTrangThaiTuChoiTLTH(maND, maChungTuTLTH);

            if (idMaTrangThaiDuyetTuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Updatr mã trạng thái duyệt
            DuToanBSChungTuModels.CapNhatTrangThaiDuyetChungTuTLTH(maChungTuTLTH, idMaTrangThaiDuyetTuChoi, maND,
                Request.UserHostAddress);

            DuToanBSChungTuModels.CapNhatChungTuTLTHCuc(maChungTuTLTH, maND);
            return RedirectToAction("Index", "DuToanBSChungTu",
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
            int maTrangThaiTuChoi = DuToanBSChungTuModels.LayMaTrangThaiTuChoiTLTHCuc(maND,
                maChungTuTLTHCuc);
            if (maTrangThaiTuChoi <= 0)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE_CONTROLLER);
            }

            DuToanBSChungTuModels.CapNhatTrangThaiDuyetChungTuTLTHCuc(maChungTuTLTHCuc, maTrangThaiTuChoi,
                maND, Request.UserHostAddress);
            return RedirectToAction("Index", "DuToanBSChungTu", new {sLNS = sLNS, iLoai = iLoai});
        }

        #endregion
    }
}