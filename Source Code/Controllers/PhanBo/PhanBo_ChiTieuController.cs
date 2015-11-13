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
    public class PhanBo_ChiTieuController : Controller
    {
        //
        // GET: /PhanBo_ChiTieu/
        public string sViewPath = "~/Views/PhanBo/ChiTieu/";
        [Authorize]
        public ActionResult Index(String MaDotPhanBo)
        {
            ViewData["MaDotPhanBo"] = MaDotPhanBo;
            return View(sViewPath + "ChiTieu_Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID, String MaDotPhanBo)
        {
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChiTieu = Request.Form[ParentID + "_iSoChiTieu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Index", "PhanBo_ChiTieu", new { MaDotPhanBo = MaDotPhanBo, SoChiTieu = SoChiTieu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchDuyetSubmit(String ParentID, String ChiNganSach)
        {
            String MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String NgayDotNganSach = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dNgayDotPhanBo"];
            String TuNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dTuNgay"];
            String DenNgay = Request.Form[ParentID + "_" + NgonNgu.MaDate + "dDenNgay"];
            String SoChiTieu = Request.Form[ParentID + "_iSoChiTieu"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];

            return RedirectToAction("Duyet", "PhanBo_ChiTieu", new { MaPhongBan = MaPhongBan, NgayDotNganSach = NgayDotNganSach, SoChiTieu = SoChiTieu, TuNgay = TuNgay, DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }


        [Authorize]
        public ActionResult Create_ChiTieu(String MaDotPhanBo, String iID_MaChiTieu)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChiTieu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "PB_ChiTieu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChiTieu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaDotPhanBo"] = MaDotPhanBo;
            ViewData["MaChiTieu"] = iID_MaChiTieu;
            return View(sViewPath + "Create.aspx");
        }
        [Authorize]
        public ActionResult Edit(String MaDotPhanBo, String iID_MaChiTieu)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChiTieu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "PB_ChiTieu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            ViewData["MaDotPhanBo"] = MaDotPhanBo;
            ViewData["MaChiTieu"] = iID_MaChiTieu;
            if (String.IsNullOrEmpty(iID_MaChiTieu))
            {
                ViewData["DuLieuMoi"] = "1";
                return View(sViewPath + "Create.aspx");
            }
            else
            {
                return View(sViewPath + "ChiTieu_Edit.aspx");
            }
        }
        //Tạo chỉ tiêu đầu năm
        [Authorize]
        public ActionResult Create(String MaDotPhanBo, String iID_MaChiTieu)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaChiTieu) && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND) == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (BaoMat.ChoPhepLamViec(MaND, "PB_ChiTieu", "Edit") == false)
            {
                //Phải có quyền thêm chứng từ
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChiTieu))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["MaDotPhanBo"] = MaDotPhanBo;
            ViewData["MaChiTieu"] = iID_MaChiTieu;
            return View(sViewPath + "ChiTieu_Create.aspx");
        }



        /// <summary>
        /// Submit việc thêm mới sửa 1 chỉ tiêu
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaDotPhanBo"></param>
        /// <param name="MaChiTieu"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaDotPhanBo, String MaChiTieu)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHePhanBo, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("PB_ChiTieu");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;

            //<--Kiểm tra tính đúng đắn của dữ liệu
            NameValueCollection arrLoi = new NameValueCollection();

            String dNgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (HamChung.isDate(dNgayChungTu) == false)
            {
                arrLoi.Add("err_dNgayChungTu", "Ngày không đúng");
            }
            if (String.IsNullOrEmpty(dNgayChungTu))
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            //Kiểm tra tính đúng đắn của dữ liệu-->
            String siID_MaDuToan = Request.Form["iID_MaDuToan"];
            if (arrLoi.Count > 0)
            {
                //Khi dữ liệu nhập vào không đúng
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["MaDotPhanBo"] = MaDotPhanBo;
                ViewData["MaChiTieu"] = MaChiTieu;
                return View(sViewPath + "ChiTieu_Edit.aspx");
            }
            else
            {
                //Khi dữ liệu nhập vào đúng
                DataTable dtDotPhanBo = PhanBo_DotPhanBoModels.GetDotPhanBo(MaDotPhanBo);
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    //Trường hợp thêm mới 1 chỉ tiêu
                    //B1: Thêm chỉ tiêu
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(PhanBoModels.iID_MaPhanHeChiTieu));
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
                    bang.CmdParams.Parameters.AddWithValue("@dNgayDotPhanBo", dtDotPhanBo.Rows[0]["dNgayDotPhanBo"]);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtDotPhanBo.Rows[0]["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtDotPhanBo.Rows[0]["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtDotPhanBo.Rows[0]["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanBoModels.iID_MaPhanHeChiTieu));
                    String MaChiTieuAddNew = Convert.ToString(bang.Save());
                    MaChiTieu = MaChiTieuAddNew;
                    //B2: Thêm danh sách chứng từ dự toán được chọn
                    PhanBo_ChiTieuModels.Delete_ChiTieu_DuToan(MaChiTieu);
                    PhanBo_ChiTieuModels.Update_ChiTieu_DuToan(MaChiTieu, siID_MaDuToan, MaND, Request.UserHostAddress);
                    //B3: Thêm chi tiết chỉ tiêu
                    PhanBo_ChiTieuChiTietModels.ThemChiTiet(MaChiTieuAddNew, User.Identity.Name, Request.UserHostAddress);
                    
                                        
                    //PhanBo_ChiTieuModels.InsertDuyetChiTieu(MaChiTieuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    //Trường hợp sửa thông tin chỉ tiêu cũ
                    DataTable dt = PhanBo_ChiTieuModels.GET_DanhSachDuToanDuocChon(MaChiTieu);
                    String[] arrDuToan= siID_MaDuToan.Split(',');
                    int CoThayDoi = 0;

                    if (dt.Rows.Count >= arrDuToan.Length)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < arrDuToan.Length; j++)
                            {
                                if (Convert.ToString(dt.Rows[i]["iID_MaDuToan"]) == arrDuToan[j])
                                {
                                    CoThayDoi += 1;
                                }
                            }

                        }
                    }
                    else
                    {
                        CoThayDoi = dt.Rows.Count-1;
                    }
                    bang.GiaTriKhoa = MaChiTieu;
                    bang.Save();
                    if (CoThayDoi==dt.Rows.Count-1)
                    {
                        
                        //B1: Thêm danh sách chứng từ dự toán được chọn
                        PhanBo_ChiTieuModels.Delete_ChiTieu_DuToan(MaChiTieu);
                        PhanBo_ChiTieuModels.Update_ChiTieu_DuToan(MaChiTieu, siID_MaDuToan, MaND, Request.UserHostAddress);
                        //B2: Xóa chỉ tiêu
                        PhanBo_ChiTieuModels.Delete_ChiTieuChiTiet(MaChiTieu, Request.UserHostAddress, MaND);
                        //Xóa phân bổ
                        PhanBo_ChiTieuModels.Delete_PhanBo(MaChiTieu);
                        //B3: Thêm chi tiết chỉ tiêu
                        PhanBo_ChiTieuChiTietModels.ThemChiTiet(MaChiTieu, User.Identity.Name, Request.UserHostAddress);
                    }
                    dt.Dispose();
                }
            }
            return RedirectToAction("Index", "PhanBo_ChiTieuChiTiet", new { iID_MaChiTieu = MaChiTieu });
        }
        /// <summary>
        /// Submit việc thêm mới sửa 1 chỉ tiêu
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="MaDotPhanBo"></param>
        /// <param name="MaChiTieu"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_DauNam(String ParentID, String MaDotPhanBo, String MaChiTieu)
        {
            String MaND = User.Identity.Name;
            string sChucNang = "Edit";
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1" && LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHePhanBo, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                sChucNang = "Create";
            }
            Bang bang = new Bang("PB_ChiTieu");
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, sChucNang) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int i;

            //<--Kiểm tra tính đúng đắn của dữ liệu
            NameValueCollection arrLoi = new NameValueCollection();

            String dNgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            if (HamChung.isDate(dNgayChungTu) == false)
            {
                arrLoi.Add("err_dNgayChungTu", "Ngày không đúng");
            }
            if (String.IsNullOrEmpty(dNgayChungTu))
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            //Kiểm tra tính đúng đắn của dữ liệu-->

            if (arrLoi.Count > 0)
            {
                //Khi dữ liệu nhập vào không đúng
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["MaDotPhanBo"] = MaDotPhanBo;
                ViewData["MaChiTieu"] = MaChiTieu;
                return View(sViewPath + "ChiTieu_Edit.aspx");
            }
            else
            {

                //Khi dữ liệu nhập vào đúng
                
                String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
                String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

                String SQL = String.Format("SELECT iID_MaDotPhanBo FROM PB_DotPhanBo WHERE iTrangThai=1 AND Convert(varchar,dNgayDotPhanBo,111)='{0}/01/01' AND iNamLamViec=@iNamLamViec AND iID_MaNamNganSach=@iID_MaNamNganSach AND iID_MaNguonNganSach=@iID_MaNguonNganSach",NamLamViec);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", NamNganSach);
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", NguonNganSach);

                
                MaDotPhanBo = Connection.GetValueString(cmd,"");
                cmd.Dispose();
                //Nếu chưa có đợt phân bổ đầu năm là đợt có ngày 1/1/năm làm việc
                if (String.IsNullOrEmpty(MaDotPhanBo))
                {
                    
                    //Thêm mới đợt phân bổ đầu năm
                    String dNgayDotPhanBo = String.Format("{0}/01/01", NamLamViec);
                    Bang bangDotPhanBo = new Bang("PB_DotPhanBo");
                    bangDotPhanBo.CmdParams.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
                    bangDotPhanBo.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", NamNganSach);
                    bangDotPhanBo.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", NguonNganSach);
                    bangDotPhanBo.CmdParams.Parameters.AddWithValue("@dNgayDotPhanBo", dNgayDotPhanBo);
                    bangDotPhanBo.CmdParams.Parameters.AddWithValue("@bChiNganSach", 0);
                    bangDotPhanBo.CmdParams.Parameters.AddWithValue("@sDSLNS", sLNS + ";");
                    bangDotPhanBo.MaNguoiDungSua = User.Identity.Name;
                    bangDotPhanBo.IPSua = Request.UserHostAddress;
                    MaDotPhanBo = Convert.ToString(bangDotPhanBo.Save());
                }
                //

                DataTable dtDotPhanBo = PhanBo_DotPhanBoModels.GetDotPhanBo(MaDotPhanBo);
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
                {
                    //Trường hợp thêm mới 1 chỉ tiêu
                    //B1: Thêm chỉ tiêu
                    bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(PhanBoModels.iID_MaPhanHeChiTieu));
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
                    bang.CmdParams.Parameters.AddWithValue("@dNgayDotPhanBo", dtDotPhanBo.Rows[0]["dNgayDotPhanBo"]);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtDotPhanBo.Rows[0]["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtDotPhanBo.Rows[0]["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtDotPhanBo.Rows[0]["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanBoModels.iID_MaPhanHeChiTieu));
                    String MaChiTieuAddNew = Convert.ToString(bang.Save());
                    MaChiTieu = MaChiTieuAddNew;

                    //B2: Thêm chi tiết chỉ tiêu
                    PhanBo_ChiTieuChiTietModels.ThemChiTiet(MaChiTieuAddNew, User.Identity.Name, Request.UserHostAddress);

                    //PhanBo_ChiTieuModels.InsertDuyetChiTieu(MaChiTieuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                }
                else
                {
                    //Trường hợp sửa thông tin chỉ tiêu cũ
                    bang.GiaTriKhoa = MaChiTieu;
                    bang.Save();
                }
            }
            return RedirectToAction("Index", "PhanBo_ChiTieuChiTiet", new { iID_MaChiTieu = MaChiTieu });
        }

        [Authorize]
        public ActionResult Delete(String iID_MaChiTieu, String MaDotPhanBo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "PB_ChiTieu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            int iXoa = 0;
            iXoa = PhanBo_ChiTieuModels.Delete_ChiTieu(iID_MaChiTieu, Request.UserHostAddress, User.Identity.Name);
            return RedirectToAction("Index", "PhanBo_ChiTieu", new { MaDotPhanBo = MaDotPhanBo });
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaChiTieu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = PhanBo_ChiTieuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChiTieu);
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng chứng từ
            PhanBo_ChiTieuModels.Update_iID_MaTrangThaiDuyet(iID_MaChiTieu, iID_MaTrangThaiDuyet_TrinhDuyet, true, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChiTieu = PhanBo_ChiTieuModels.InsertDuyetChiTieu(iID_MaChiTieu, NoiDung, MaND, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChiTieuCuoiCung", MaDuyetChiTieu);
            PhanBo_ChiTieuModels.UpdateRecord(iID_MaChiTieu, cmd.Parameters, User.Identity.Name, Request.UserHostAddress);
            cmd.Dispose();

            int iID_MaTrangThaiTuChoi = PhanBo_ChiTieuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChiTieu);
            return RedirectToAction("ChiTieuChiTiet_Frame", "PhanBo_ChiTieuChiTiet", new { iID_MaChiTieu = iID_MaChiTieu });
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaChiTieu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = PhanBo_ChiTieuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChiTieu);
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            dtTrangThaiDuyet.Dispose();

            //Cập nhập trường sSua
            PhanBo_DuyetChiTieuModels.CapNhapLaiTruong_sSua(iID_MaChiTieu);

            ///Update trạng thái cho bảng chứng từ
            PhanBo_ChiTieuModels.Update_iID_MaTrangThaiDuyet(iID_MaChiTieu, iID_MaTrangThaiDuyet_TuChoi, false, MaND, Request.UserHostAddress);

            ///Thêm dữ liệu vào bảng duyệt chứng từ - Lấy mã duyệt chứng từ
            String MaDuyetChiTieu = PhanBo_ChiTieuModels.InsertDuyetChiTieu(iID_MaChiTieu, NoiDung, NoiDung, Request.UserHostAddress);

            ///Update Mã duyệt chứng từ cuối vào bảng chứng từ
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaDuyetChiTieuCuoiCung", MaDuyetChiTieu);
            PhanBo_ChiTieuModels.UpdateRecord(iID_MaChiTieu, cmd.Parameters, MaND, Request.UserHostAddress);
            cmd.Dispose();

            return RedirectToAction("ChiTieuChiTiet_Frame", "PhanBo_ChiTieuChiTiet", new { iID_MaChiTieu = iID_MaChiTieu });
        }

        [Authorize]
        public ActionResult Duyet()
        {
            return View(sViewPath + "ChiTieu_Duyet.aspx");
        }


        #region Chuyển năm sau
        [Authorize]
        public ActionResult ChuyenNamSau()
        {

            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return View(sViewPath + "ChiTieu_ChuyenNamSau.aspx");
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
            NganSach_HamChungModels.ChuyenNamSau(MaND, IPSua, MaTrangThaiDuyet, "PB_ChiTieu", "PB_ChiTieuChiTiet",true, "PB_DotPhanBo", "dNgayDotPhanBo");
            return RedirectToAction("Index");

        }
        #endregion
    }
}
