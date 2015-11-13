using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using DomainModel;
using DomainModel.Abstract;
using VIETTEL.Models;

namespace VIETTEL.Controllers
{
    public class TuLieu_VideoController : Controller
    {
        //
        // GET: /TuLieu_Video/
        public string sViewPath = "~/Views/TuLieu/Video/";

        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Video", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "Index.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Video", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String MaKieuTaiLieu = Convert.ToString(Request.Form[ParentID + "_iID_MaKieuTaiLieu"]);
            String MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String TuNgay = Convert.ToString(Request.Form[ParentID + "_vidTuNgayUpload"]);
            String DenNgay = Convert.ToString(Request.Form[ParentID + "_vidDenNgayUpload"]);
            return RedirectToAction("Index", "TuLieu_Video", new {MaKieuTaiLieu, MaDonVi, TuNgay, DenNgay});
        }

        [Authorize]
        public ActionResult Edit(String MaTaiLieu)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Video", "Edit") == false)
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
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Video", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            var bang = new Bang("TL_Video");
            bang.GiaTriKhoa = MaTaiLieu;
            bang.Delete();
            return RedirectToAction("Index");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaTaiLieu, string path)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TL_Video", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            try
            {
                DateTime today = DateTime.Now;
                String dateString = today.ToString("yyyy/MM/dd");
                string newPath = AppDomain.CurrentDomain.BaseDirectory + path+"/" + dateString;
                //string newPath = path + dateString;
                if (Directory.Exists(newPath) == false)
                {
                    Directory.CreateDirectory(newPath);
                }
                var arrLoi = new NameValueCollection();
                string filename = "";
                if (HasFile(Request.Files["uploadFile"]))
                {
                    filename = today.ToString("yyyy_MM_dd_HH_mm_ss") + "_" +
                               Path.GetFileName(Request.Files["uploadFile"].FileName);
                    Request.Files["uploadFile"].SaveAs(Path.Combine(newPath, filename));
                }
                String TenTaiLieu = Convert.ToString(Request.Form[ParentID + "_sTen"]);
                int MaTuLieu_DanhMuc = Convert.ToInt32(Request.Form[ParentID + "_iID_MaKieuTaiLieu"]);
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
                    int idTaoMoi =
                        LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(
                            PhanHeModels.iID_MaPhanHeTuLieuLichSu);
                    var bang = new Bang("TL_Video");
                    var param = new NameValueCollection();
                    foreach (String key in Request.Form.Keys)
                    {
                        if (key == ParentID + "_sURL")
                        {
                            string url = path +"/" + dateString + "/" + filename;
                            param.Add(key, url);
                        }
                        else
                        {
                            param.Add(key, Request.Form[key]);
                        }
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
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        public ActionResult TuChoi(String iID_MaTuLieu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TuChoi = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaTuLieu,
                                                                                                 "TL_Video",
                                                                                                 "iID_MaTaiLieu_Video");
            if (iID_MaTrangThaiDuyet_TuChoi <= 0)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            DataTable dtTrangThaiDuyet = LuongCongViecModel.Get_dtTrangThaiDuyet(iID_MaTrangThaiDuyet_TuChoi);
            String NoiDung = Convert.ToString(dtTrangThaiDuyet.Rows[0]["sTen"]);
            if (dtTrangThaiDuyet != null) dtTrangThaiDuyet.Dispose();

            ///Update trạng thái cho bảng tư liệu
            TuLieuLichSuModels.Update_iID_MaTrangThaiDuyet(iID_MaTuLieu, iID_MaTrangThaiDuyet_TuChoi, "TL_Video",
                                                           "iID_MaTaiLieu_Video");
            ///Thêm dữ liệu vào bảng duyệt tư liệu - Lấy mã duyệt tư liệu
            String MaDuyetTuLieu = TuLieuLichSuModels.InsertDuyetTuLieu(iID_MaTuLieu, NoiDung, MaND,
                                                                        Request.UserHostAddress, "TL_DuyetTuLieu");
            return RedirectToAction("Index", "TuLieu_Video");
        }

        [Authorize]
        public ActionResult TrinhDuyet(String iID_MaTuLieu)
        {
            String MaND = User.Identity.Name;
            //Xác định trạng thái duyệt tiếp theo
            int iID_MaTrangThaiDuyet_TrinhDuyet = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND,
                                                                                                         iID_MaTuLieu,
                                                                                                         "TL_Video",
                                                                                                         "iID_MaTaiLieu_Video");
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
            TuLieuLichSuModels.Update_iID_MaTrangThaiDuyet(iID_MaTuLieu, iID_MaTrangThaiDuyet_TrinhDuyet, "TL_Video",
                                                           "iID_MaTaiLieu_Video");
            ///Thêm dữ liệu vào bảng duyệt tư liệu - Lấy mã duyệt tư liệu
            String MaDuyetTuLieu = TuLieuLichSuModels.InsertDuyetTuLieu(iID_MaTuLieu, NoiDung, MaND,
                                                                        Request.UserHostAddress, "TL_DuyetTuLieu");
            return RedirectToAction("Index", "TuLieu_Video");
        }

        public ActionResult Download(String MaTaiLieu)
        {
            var cmd = new SqlCommand("SELECT * FROM TL_Video WHERE iID_MaTaiLieu_Video=@iID_MaTaiLieu");
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
            String strTen = String.Format("{0}{1}", dtTaiLieu.Rows[0]["sTen"], strExt);
            strTen = NgonNgu.LayXauKhongDauTiengViet(strTen);
            cmd.Dispose();
            dtTaiLieu.Dispose();
            if (Request.UserLanguages != null)
            {
                cmd =
                    new SqlCommand("UPDATE TL_Video SET iSoLanTai=iSoLanTai+1 WHERE iID_MaTaiLieu_Video=@iID_MaTaiLieu");
                cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            strTen = strTen.Replace(' ', '_');
            return new DownloadResult {VirtualPath = strURL, FileDownloadName = strTen};
        }

        private bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }
}