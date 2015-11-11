using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;
using System.Collections;
using Oneres.Controllers.Shared;
using VIETTEL.Models;

namespace VIETTEL.Controllers
{
    public class TuLieu_HinhAnhController : Controller
    {
        //
        // GET: /TuLieu_TaiLieu/
        public string sViewPath = "~/Views/TuLieu/HinhAnh/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Anh", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Anh", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String MaKieuTaiLieu = Convert.ToString(Request.Form[ParentID + "_iID_MaKieuTaiLieu"]);
            String MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TuNgay = Convert.ToString(Request.Form[ParentID + "_vidTuNgayUpload"]);
            String DenNgay = Convert.ToString(Request.Form[ParentID + "_vidDenNgayUpload"]);
            return RedirectToAction("Index", "TuLieu_HinhAnh", new { MaKieuTaiLieu = MaKieuTaiLieu, MaDonVi = MaDonVi, TuNgay = TuNgay, DenNgay = DenNgay });
        }

        [Authorize]
        public ActionResult Edit(String MaTaiLieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Anh", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String iID_MaKieuTaiLieu = Request.QueryString["iID_MaKieuTaiLieu"];
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(MaTaiLieu))
            {
                //MaDonVi = Globals.getNewGuid().ToString();
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaTaiLieu"] = MaTaiLieu;
            ViewData["iID_MaKieuTaiLieu"] = iID_MaKieuTaiLieu;
            return View(sViewPath + "Edit.aspx");
        }

        [Authorize]
        public ActionResult Delete(String MaTaiLieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Anh", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("TL_Anh");
            bang.GiaTriKhoa = MaTaiLieu;
            bang.Delete();
            return RedirectToAction("Index");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaTaiLieu , String selectPath)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Anh", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DateTime today =DateTime.Now;
            String dateString =today.ToString("yyyy/MM/dd");
            string newPath = AppDomain.CurrentDomain.BaseDirectory + selectPath +"/" + dateString;
            //string newPath = selectPath + dateString;
            if (Directory.Exists(newPath) == false)
            {
                Directory.CreateDirectory(newPath);
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String ImageServerPath = Server.MapPath("~/");
            String Path = selectPath;
            String PathThums = selectPath + "/Thums";
            String RealPathThums = selectPath + "/RealThums";
            String FileName = "";
            String FileNameThums = "";
            String FileNameRealThums = "";
            if (HasFile(Request.Files["uploadFile"]))
            {
                FileName = Request.Files["uploadFile"].FileName;
                //filename = today.ToString("yyyy_MM_dd_HH_mm_ss") + "_" + Path.GetFileName(Request.Files["uploadFile"].FileName);
                //Request.Files["uploadFile"].SaveAs(Path.Combine(path, filename));
                System.Drawing.Image originalImg = System.Drawing.Image.FromStream(Request.Files["uploadFile"].InputStream);
                int WidthRealThums = -1, HeightRealThums = -1, WidthThums = UploadController.FixWidthThums, HeightThums = UploadController.FixHeightThums, Width = -1, Height = -1;
                UploadController.SaveImage(originalImg, ImageServerPath, Path, PathThums, RealPathThums, ref FileName, ref FileNameThums, ref FileNameRealThums, ref Width, ref Height, WidthThums, HeightThums, ref WidthRealThums, ref HeightRealThums);
            }
            String TenTaiLieu = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            int MaTuLieu_DanhMuc = Convert.ToInt32(Request.Form[ParentID + "_iID_MaKieuTaiLieu"]);
            int idTaoMoi =
                   VIETTEL.Models.LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(
                       VIETTEL.Models.PhanHeModels.iID_MaPhanHeTuLieuLichSu);
            if (TenTaiLieu == String.Empty || TenTaiLieu == "")
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập tên tài liệu!");
            }
            if (MaTuLieu_DanhMuc <= 0)
            {
                arrLoi.Add("err_iID_MaTuLieu_DanhMuc", "Bạn chưa chọn kiểu tài liệu!");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["iID_MaTaiLieu"] = MaTaiLieu;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                String MaDonVi = Request.Form[ParentID + "_sMaDonVi"];
                String MaTaiLieuLienQuan = Request.Form[ParentID + "_sMaTaiLieuLienQuan"];
                Bang bang = new Bang("TL_Anh");
                NameValueCollection param = new NameValueCollection();
                foreach (String key in Request.Form.Keys)
                {
                    param.Add(key, Request.Form[key]);
                }
                if (string.IsNullOrEmpty(MaTaiLieu))
                {
                    param.Add(ParentID + "_sFileName", FileName);
                    param.Add(ParentID + "_sFileNameThums", FileNameThums);
                    param.Add(ParentID + "_sFileNameRealThums", FileNameRealThums);
                }
                bang.TruyenGiaTri(ParentID, param);
                bang.CmdParams.Parameters["@sMaDonVi"].Value = "," + MaDonVi + ",";
                if (MaTaiLieuLienQuan != null)
                {
                    bang.CmdParams.Parameters["@sMaTaiLieuLienQuan"].Value = "," + MaTaiLieuLienQuan + ",";
                }
                bang.CmdParams.Parameters["@iID_MaTrangThaiDuyet"].Value = idTaoMoi;
                bang.Save();
                return RedirectToAction("Index");
            }            
        }
        [Authorize]

        public ActionResult TuChoi(String iID_MaTuLieu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaTuLieu, "TL_Anh", "iID_MaTaiLieu_Anh");
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            if (dtTrangThaiDuyet != null) dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng tư liệu
            TuLieuLichSuModels.Update_iID_MaTrangThaiDuyet(iID_MaTuLieu, iID_MaTrangThaiDuyet_TuChoi, "TL_Anh", "iID_MaTaiLieu_Anh");
            ///Thêm dữ liệu vào bảng duyệt tư liệu - Lấy mã duyệt tư liệu
            String MaDuyetTuLieu = TuLieuLichSuModels.InsertDuyetTuLieu(iID_MaTuLieu, NoiDung, MaND, Request.UserHostAddress, "TL_DuyetTuLieu");
            return RedirectToAction("Index", "TuLieu_HinhAnh");

        }

        [Authorize]

        public ActionResult TrinhDuyet(String iID_MaTuLieu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaTuLieu, "TL_Anh", "iID_MaTaiLieu_Anh");
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
            TuLieuLichSuModels.Update_iID_MaTrangThaiDuyet(iID_MaTuLieu, iID_MaTrangThaiDuyet_TrinhDuyet, "TL_Anh", "iID_MaTaiLieu_Anh");
            ///Thêm dữ liệu vào bảng duyệt tư liệu - Lấy mã duyệt tư liệu
            String MaDuyetTuLieu = TuLieuLichSuModels.InsertDuyetTuLieu(iID_MaTuLieu, NoiDung, MaND, Request.UserHostAddress, "TL_DuyetTuLieu");
            return RedirectToAction("Index", "TuLieu_HinhAnh");

        }

        public ActionResult Download(String MaTaiLieu)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM TL_Anh WHERE iID_MaTaiLieu_Anh=@iID_MaTaiLieu");
            cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
            DataTable dtTaiLieu = Connection.GetDataTable(cmd);
            String tg = Convert.ToString(dtTaiLieu.Rows[0]["sFileNameRealThums"]);
            String strExt = "";
            int i = tg.LastIndexOf('.');
            if (i >= 0)
            {
                strExt = tg.Substring(i);
            }
            String strURL = String.Format("~/{0}", dtTaiLieu.Rows[0]["sFileNameRealThums"]);
            String strTen = String.Format("{0}{1}", dtTaiLieu.Rows[0]["sTen"], strExt);
            strTen = NgonNgu.LayXauKhongDauTiengViet(strTen);
            cmd.Dispose();
            dtTaiLieu.Dispose();
            if (Request.UserLanguages != null)
            {
                cmd = new SqlCommand("UPDATE TL_Anh SET iSoLanTai=iSoLanTai+1 WHERE iID_MaTaiLieu_Anh=@iID_MaTaiLieu");
                cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            strTen = strTen.Replace(' ', '_');
            return new DownloadResult { VirtualPath = strURL, FileDownloadName = strTen };
        }
        private bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }
}
