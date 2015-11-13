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
using System.IO;

namespace VIETTEL.Controllers
{
    public class TuLieu_VanBanController : Controller
    {
        //
        // GET: /TuLieu_TaiLieu/
        public string sViewPath = "~/Views/TuLieu/VanBan/";
        Bang bang = new Bang("TL_VanBan");
        [Authorize]
        public ActionResult Index(String MaKieuTaiLieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_VanBan", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "Index.aspx");
        }

        public ActionResult List(String MaKieuTaiLieu)
        {

            return View(sViewPath + "LichLamViec.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_VanBan", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String MaKieuTaiLieu = Convert.ToString(Request.Form[ParentID + "_iID_MaKieuTaiLieu"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String sTuKhoa = Convert.ToString(Request.Form[ParentID + "_sTuKhoa"]);
            String sSoHieu = Convert.ToString(Request.Form[ParentID + "_sSoHieu"]);
            String sHieuLuc = Convert.ToString(Request.Form[ParentID + "_sHieuLuc"]);
            String iDM_LoaiVanBan = Convert.ToString(Request.Form[ParentID + "_iDM_LoaiVanBan"]);
            String iDM_NoiBanHanh = Convert.ToString(Request.Form[ParentID + "_iDM_NoiBanHanh"]);
            String sNguoiKy = Convert.ToString(Request.Form[ParentID + "_sNguoiKy"]);
            String TuNgay = Convert.ToString(Request.Form[ParentID + "_vidTuNgayUpload"]);
            String DenNgay = Convert.ToString(Request.Form[ParentID + "_vidDenNgayUpload"]);
            String PageLoad = "1";
            return RedirectToAction("Index", "TuLieu_VanBan", new { MaKieuTaiLieu = MaKieuTaiLieu, sTen = sTen, sSoHieu = sSoHieu, TuNgay = TuNgay, DenNgay = DenNgay, iDM_LoaiVanBan = iDM_LoaiVanBan, iDM_NoiBanHanh = iDM_NoiBanHanh, sNguoiKy = sNguoiKy, sTuKhoa = sTuKhoa, PageLoad = PageLoad, sHieuLuc = sHieuLuc });
        }

        [Authorize]
        public ActionResult Edit(String MaTaiLieu, string path)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_VanBan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //truong hop them moi khong check quyen sua
            bool check = true;
            if (!String.IsNullOrEmpty(MaTaiLieu))
                check = TuLieuLichSuModels.CheckEditOrDelete(User.Identity.Name, MaTaiLieu);
            if (check)
            {
                bang.GiaTriKhoa = MaTaiLieu;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);


                ViewData[bang.TenBang + "_dicData"] = dicData;
                String iID_MaKieuTaiLieu = Request.QueryString["iID_MaKieuTaiLieu"];
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaTaiLieu))
                {
                    //MaDonVi = Globals.getNewGuid().ToString();
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaTaiLieu"] = MaTaiLieu;
                ViewData["iID_MaKieuTaiLieu"] = iID_MaKieuTaiLieu;
                ViewData["path"] = path;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Detail(String MaTaiLieu, string path)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_VanBan", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            bang.GiaTriKhoa = MaTaiLieu;
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);


            ViewData[bang.TenBang + "_dicData"] = dicData;
            String iID_MaKieuTaiLieu = Request.QueryString["iID_MaKieuTaiLieu"];
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(MaTaiLieu))
            {
                //MaDonVi = Globals.getNewGuid().ToString();
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaTaiLieu"] = MaTaiLieu;
            ViewData["iID_MaKieuTaiLieu"] = iID_MaKieuTaiLieu;
            ViewData["path"] = path;
            return View(sViewPath + "Detail.aspx");
        }
        public ActionResult Detail_LichLamViec(String MaTaiLieu, string path)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_VanBan", "Detail") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            bang.GiaTriKhoa = MaTaiLieu;
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);


            ViewData[bang.TenBang + "_dicData"] = dicData;
            String iID_MaKieuTaiLieu = Request.QueryString["iID_MaKieuTaiLieu"];
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(MaTaiLieu))
            {
                //MaDonVi = Globals.getNewGuid().ToString();
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaTaiLieu"] = MaTaiLieu;
            ViewData["iID_MaKieuTaiLieu"] = iID_MaKieuTaiLieu;
            ViewData["path"] = path;
            return View(sViewPath + "DetailLichLamViec.aspx");
        }

        [Authorize]
        public ActionResult Delete(String MaTaiLieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_VanBan", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            bool check = true;
            check = TuLieuLichSuModels.CheckEditOrDelete(User.Identity.Name, MaTaiLieu);
            if (check == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String MaKieuTaiLieu = Convert.ToString(CommonFunction.LayTruong("Tl_VanBan", "iID_MaTaiLieu", MaTaiLieu, "iID_MaKieuTaiLieu"));
            //String UrlCu = Convert.ToString(CommonFunction.LayTruong("Tl_VanBan", "iID_MaTaiLieu", MaTaiLieu, "sURL"));
            //try
            //{
            //    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + UrlCu);
            //}
            //catch { }
            bang.GiaTriKhoa = MaTaiLieu;
            bang.Delete();
            return RedirectToAction("Index", new { MaKieuTaiLieu = MaKieuTaiLieu, PageLoad = "1" });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaTaiLieu, String MaKieuTaiLieu, string path)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_VanBan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String TenTaiLieu = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String sSoHieu = Convert.ToString(Request.Form[ParentID + "_sSoHieu"]);
            String sFileName = "";
            String sTuKhoaLQ = Convert.ToString(Request.Form[ParentID + "_sTuKhoaLQ"]);
            int MaTuLieu_DanhMuc = Convert.ToInt32(Request.Form[ParentID + "_iID_MaKieuTaiLieu"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            String dNgayHieuLuc = Convert.ToString(Request.Form[ParentID + "_vidNgayHieuLuc"]);
            String dNgayHetHan = Convert.ToString(Request.Form[ParentID + "_vidNgayHetHan"]);
            //kiem tra ngay het han phai lon hon hoac bang ngay hieu luc
            if (!String.IsNullOrEmpty(dNgayHetHan) && !String.IsNullOrEmpty(dNgayHieuLuc))
            {
                int KQ = DateTime.Compare(Convert.ToDateTime(CommonFunction.LayNgayTuXau(dNgayHetHan)), Convert.ToDateTime(CommonFunction.LayNgayTuXau(dNgayHieuLuc)));
                //ngay het han be hon ngay hieu luc
                if (KQ < 0)
                    arrLoi.Add("err_dNgayHetHan", "Ngày hết hiệu lực không được nhỏ hơn ngày hiệu lực!");

            }
            if (TenTaiLieu == String.Empty || TenTaiLieu == "")
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập tên tài liệu!");
            }
            //Check trung


            //Kiem tra DK so hieu khong đc để trống
            if (sSoHieu == String.Empty || sSoHieu == "")
            {
                arrLoi.Add("err_sSoHieu", "Bạn chưa nhập số văn bản!");
            }
            else
            {
                //Check trung
                String SQL = "";
                SqlCommand cmd = new SqlCommand();
                if (!String.IsNullOrEmpty(MaTaiLieu))
                {
                    SQL = String.Format("SELECT sSoHieu FROM TL_VanBan WHERE iTrangThai=1 AND sSoHieu=@sSoHieu AND iID_MaTaiLieu!=@iID_MaTaiLieu");
                    cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
                }
                else
                {
                    SQL = String.Format("SELECT sSoHieu FROM TL_VanBan WHERE iTrangThai=1 AND sSoHieu=@sSoHieu");
                }
                cmd.Parameters.AddWithValue("@sSoHieu", sSoHieu);
                cmd.CommandText = SQL;
                String sSoHieu1 = Connection.GetValueString(cmd, "");
                if (String.Compare(sSoHieu, sSoHieu1) == 0)
                {
                    arrLoi.Add("err_sSoHieu", "Đã có số văn bản này");
                }
            }

            if (MaTuLieu_DanhMuc <= 0)
            {
                arrLoi.Add("err_iID_MaTuLieu_DanhMuc", "Bạn chưa chọn kiểu tài liệu!");
            }
            DateTime today = DateTime.Now;
            String dateString = today.ToString("yyyy/MM/dd");
            string newPath = AppDomain.CurrentDomain.BaseDirectory + path + "/" + dateString;
            //string newPath = path + dateString;
            if (Directory.Exists(newPath) == false)
            {
                Directory.CreateDirectory(newPath);
            }
            string filename = "";
            sFileName = Path.GetFileName(Request.Files["uploadFile"].FileName);
            //
            String filename1 = "";
            filename1 = Convert.ToString(Request.Form[ParentID + "_filename1"]);
            //if (String.IsNullOrEmpty(filename1))
            //{
            //upload file len serverl
            filename = today.ToString("yyyy_MM_dd_HH_mm_ss") + "_" +
                       Path.GetFileName(Request.Files["uploadFile"].FileName);
            Request.Files["uploadFile"].SaveAs(Path.Combine(newPath, filename));

            //}
            string url = path + "/" + dateString + "/" + filename;
            String UrlCu = "";
            if (String.IsNullOrEmpty(sFileName))
            {
                sFileName = Convert.ToString(Request.Form[ParentID + "_sFileName"]);
                url = Convert.ToString(Request.Form[ParentID + "_sURL"]);
            }
            else
            {
                //   UrlCu = url;
                // System.IO.File.Delete(UrlCu);
            }
            if (String.IsNullOrEmpty(sFileName))
            {
                arrLoi.Add("err_sFileName", "Bạn chưa chọn tài liệu đính kèm!");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaTaiLieu"] = MaTaiLieu;
                bang.GiaTriKhoa = MaTaiLieu;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                ViewData[bang.TenBang + "_dicData"] = dicData;
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaTaiLieu))
                {
                    //MaDonVi = Globals.getNewGuid().ToString();
                    ViewData["DuLieuMoi"] = "1";
                }
                filename1 = filename;
                ViewData["sFileName"] = sFileName;
                ViewData["url"] = url;

                ViewData["iID_MaTaiLieu"] = MaTaiLieu;
                ViewData["iID_MaKieuTaiLieu"] = MaTuLieu_DanhMuc;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                int idTaoMoi =
                    VIETTEL.Models.LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(
                        VIETTEL.Models.PhanHeModels.iID_MaPhanHeTuLieuLichSu);
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (sFileName != "" || !String.IsNullOrEmpty(sFileName))
                {
                    bang.CmdParams.Parameters["@sFileName"].Value = sFileName;
                    bang.CmdParams.Parameters["@sURL"].Value = url;
                }
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                // bang.CmdParams.Parameters["@iID_MaTrangThaiDuyet"].Value = idTaoMoi;
                bang.Save();
                return RedirectToAction("Index", new { MaKieuTaiLieu = MaKieuTaiLieu, PageLoad = "1" });
            }
        }
        private bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
        [Authorize]

        public ActionResult TuChoi(String iID_MaTuLieu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaTuLieu, "TL_VanBan", "iID_MaTaiLieu");
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            if (dtTrangThaiDuyet != null) dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng tư liệu
            TuLieuLichSuModels.Update_iID_MaTrangThaiDuyet(iID_MaTuLieu, iID_MaTrangThaiDuyet_TuChoi, "TL_VanBan", "iID_MaTaiLieu");
            ///Thêm dữ liệu vào bảng duyệt tư liệu - Lấy mã duyệt tư liệu
            String MaDuyetTuLieu = TuLieuLichSuModels.InsertDuyetTuLieu(iID_MaTuLieu, NoiDung, MaND, Request.UserHostAddress, "TL_DuyetTuLieu");
            return RedirectToAction("Index", "TuLieu_TaiLieu");

        }

        [Authorize]

        public ActionResult TrinhDuyet(String iID_MaTuLieu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaTuLieu, "TL_VanBan", "iID_MaTaiLieu");
            if (iID_MaTrangThaiDuyet_TrinhDuyet <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TrinhDuyet);
            String NoiDung = "";
            if (dtTrangThaiDuyet.Rows.Count > 0)
                NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            if (dtTrangThaiDuyet != null) dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng tư liệu
            TuLieuLichSuModels.Update_iID_MaTrangThaiDuyet(iID_MaTuLieu, iID_MaTrangThaiDuyet_TrinhDuyet, "TL_VanBan", "iID_MaTaiLieu");
            ///Thêm dữ liệu vào bảng duyệt tư liệu - Lấy mã duyệt tư liệu
            String MaDuyetTuLieu = TuLieuLichSuModels.InsertDuyetTuLieu(iID_MaTuLieu, NoiDung, MaND, Request.UserHostAddress, "TL_DuyetTuLieu");
            return RedirectToAction("Index", "TuLieu_TaiLieu");

        }
        public ActionResult Download(String MaTaiLieu)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM TL_VanBan WHERE iID_MaTaiLieu=@iID_MaTaiLieu");
            cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
            DataTable dtTaiLieu = Connection.GetDataTable(cmd);
            String tg = Convert.ToString(dtTaiLieu.Rows[0]["sURL"]);
            String strExt = "";
            int i = tg.LastIndexOf('.');
            if (i >= 0)
            {
                strExt = tg.Substring(i);
            }
            String strURL = String.Format("~/{0}", dtTaiLieu.Rows[0]["sURL"]);
            String strTen = String.Format("{0}", dtTaiLieu.Rows[0]["sFileName"]);
            strTen = NgonNgu.LayXauKhongDauTiengViet(strTen);
            cmd.Dispose();
            dtTaiLieu.Dispose();
            if (Request.UserLanguages != null)
            {
                cmd = new SqlCommand("UPDATE TL_VanBan SET iSoLanTai=iSoLanTai+1 WHERE iID_MaTaiLieu=@iID_MaTaiLieu");
                cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            strTen = strTen.Replace(' ', '_');
            return new DownloadResult { VirtualPath = strURL, FileDownloadName = strTen };
        }
    }


}
