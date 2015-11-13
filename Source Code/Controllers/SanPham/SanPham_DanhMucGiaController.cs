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

namespace VIETTEL.Controllers.TCDN
{
    public class SanPham_DanhMucGiaController : Controller
    {
        //
        // GET: / SanPham_DanhMucGia/
        public string sViewPath = "~/Views/SanPham/DanhMucGia/";
        [Authorize]
        public ActionResult Index(String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, " DM_SanPham_DanhMucGia", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaSanPham"] = iID_MaSanPham;
            ViewData["iID_MaLoaiHinh"] = iID_MaLoaiHinh;
            return View(sViewPath + "Index.aspx");
        }
        /// <summary>
        /// Action Thêm mới + Sửa
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaDanhMucGia, String iID_MaDanhMucGia_Cha, String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_SanPham_DanhMucGia", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaDanhMucGia))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaDanhMucGia"] = iID_MaDanhMucGia;
            ViewData["iID_MaDanhMucGia_Cha"] = iID_MaDanhMucGia_Cha;
            ViewData["iID_MaSanPham"] = iID_MaSanPham;
            ViewData["iID_MaLoaiHinh"] = iID_MaLoaiHinh;
            return View(sViewPath + "Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaDanhMucGia)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_SanPham_DanhMucGia", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd;
            NameValueCollection arrLoi = new NameValueCollection();
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String iID_MaDanhMucGia_Cha = Convert.ToString(Request.Form[ParentID + "_iID_MaDanhMucGia_Cha"]);
            String iDM_MaDonViTinh = Convert.ToString(Request.Form[ParentID + "_iDM_MaDonViTinh"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            if (sTen == string.Empty || sTen == "")
            {
                arrLoi.Add("err_sTen", MessageModels.sTen);
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaDanhMucGia))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaDanhMucGia"] = iID_MaDanhMucGia;
                ViewData["iID_MaDanhMucGia_Cha"] = iID_MaDanhMucGia_Cha;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("DM_SanPham_DanhMucGia");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);

                if (DuLieuMoi == "1")
                {
                    if (iID_MaDanhMucGia_Cha == null || iID_MaDanhMucGia_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaDanhMucGia_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    string SQL = "SELECT  MAX(iSTT) AS  iSTT FROM DM_SanPham_DanhMucGia WHERE 1=1";
                    cmd = new SqlCommand();
                    if (iID_MaDanhMucGia_Cha != null && iID_MaDanhMucGia_Cha != "")
                    {
                        SQL += " AND iID_MaDanhMucGia_Cha=@iID_MaDanhMucGia_Cha";
                        cmd.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia_Cha);
                    }
                    cmd.CommandText = SQL;
                    int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                    cmd.Dispose();
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon);
                }
                if (DuLieuMoi == "0")
                {
                    if (iID_MaDanhMucGia_Cha == null || iID_MaDanhMucGia_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaDanhMucGia_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    bang.GiaTriKhoa = iID_MaDanhMucGia;
                }
                bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", "dddddddd-dddd-dddd-dddd-dddddddddddd");
                bang.CmdParams.Parameters.AddWithValue("@sTen_DonVi", SanPham_VatTuModels.Get_TenDonViTinh(bang.CmdParams.Parameters["@iDM_MaDonViTinh"].Value.ToString()));
                bang.Save();
                if (String.IsNullOrEmpty(iID_MaDanhMucGia)) iID_MaDanhMucGia = SanPham_DanhMucGiaModels.Get_MaxId_DanhMucGia();
                ThemChiTietGia(iID_MaDanhMucGia, Request.Form[ParentID + "_iID_MaSanPham"], Request.Form[ParentID + "_iID_MaLoaiHinh"]);
                return RedirectToAction("Index", new { iID_MaSanPham = Request.Form[ParentID + "_iID_MaSanPham"], iID_MaLoaiHinh = Request.Form[ParentID + "_iID_MaLoaiHinh"] });
            }
        }
        /// <summary>
        /// Hiển thị form sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaDanhMucGia_Cha"></param>
        /// <param name="iID_MaDanhMucGia"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String iID_MaDanhMucGia_Cha, String iID_MaDanhMucGia, String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_SanPham_DanhMucGia", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(iID_MaDanhMucGia_Cha))
            {
                iID_MaDanhMucGia_Cha = "";
            }
            ViewData["iID_MaDanhMucGia_Cha"] = iID_MaDanhMucGia_Cha;
            ViewData["iID_MaDanhMucGia"] = iID_MaDanhMucGia;
            ViewData["iID_MaSanPham"] = iID_MaSanPham;
            ViewData["iID_MaLoaiHinh"] = iID_MaLoaiHinh;
            return View(sViewPath + "Sort.aspx");
        }
        /// <summary>
        /// Sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaDanhMucGia_Cha"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaDanhMucGia_Cha, String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_SanPham_DanhMucGia", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            for (i = 0; i < arrTG.Length - 1; i++)
            {
                Bang bang = new Bang("DM_SanPham_DanhMucGia");
                bang.GiaTriKhoa = arrTG[i];
                bang.DuLieuMoi = false;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            return RedirectToAction("Index", new { iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh });
        }
        /// <summary>
        /// Lệnh điều hướng xóa tài khoản kế toán
        /// </summary>
        /// <param name="iID_MaDanhMucGia">Mã tài khoản</param>
        /// <param name="iID_MaDanhMucGia_Cha">Mã tài khoản cấp cha</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaDanhMucGia, String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            //kiểm tra quyền có được phép xóa
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_SanPham_DanhMucGia", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Boolean bDelete = false;
            bDelete = SanPham_DanhMucGiaModels.Delete(iID_MaDanhMucGia);
            if (bDelete == true)
            {
                return RedirectToAction("Index", new { iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh });
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Tìm kiếm loại tài khoản
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchSubmit(String ParentID)
        {
            String sTaiKhoan = Request.Form[ParentID + "_sTaiKhoan"];
            String sKyHieu = Request.Form[ParentID + "_sKyHieu"];
            return RedirectToAction("Index", "DanhMucGia", new { ParentID = ParentID, Ten = sTaiKhoan, KyHieu = sKyHieu });
        }

        public Boolean CheckMaTaiKhoan(String sKyHieu)
        {
            Boolean vR = false;
            int iSoDuLieu = TCDN_ChiTieuModels.Get_So_KyHieuChiTieu(sKyHieu);
            if (iSoDuLieu > 0)
            {
                vR = true;
            }
            return vR;
        }
        [Authorize]
        public ActionResult Frame(String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, " DM_SanPham_DanhMucGia", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["iID_MaSanPham"] = iID_MaSanPham;
            ViewData["iID_MaLoaiHinh"] = iID_MaLoaiHinh;
            return View(sViewPath + "DanhMuc_Frame.aspx");
        }
        /// <summary>
        /// Chọn vật tư
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChooseSubmit(String ParentID, String iID_MaDanhMucGia, String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DM_SanPham_DanhMucGia", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("DM_SanPham_DanhMucGia");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.DuLieuMoi = true;
            foreach (String key in Request.Form.AllKeys)
            {
                if (key.StartsWith(ParentID + "_chonVatTu_"))
                {
                    String iID_MaVatTu = Request.Form[key];
                    String bMuaNgoai = Request.Form[ParentID + "_bMuaNgoai_" + iID_MaVatTu];
                    String bNganSach = Request.Form[ParentID + "_bNganSach_" + iID_MaVatTu];
                    DataTable dt = SanPham_VatTuModels.Get_VatTu(iID_MaVatTu);
                    if (dt.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(bMuaNgoai))
                        {
                            bang.CmdParams.Parameters.Clear();
                            bang.CmdParams.Parameters.AddWithValue("@sTen", dt.Rows[0]["sTen"]);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia);
                            bang.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", dt.Rows[0]["iDM_MaDonViTinh"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTen_DonVi", dt.Rows[0]["sTen_DonVi"]);
                            bang.CmdParams.Parameters.AddWithValue("@bNganSach", 0);
                            bang.Save();
                            ThemChiTietGia(SanPham_DanhMucGiaModels.Get_MaxId_DanhMucGia(), iID_MaSanPham, iID_MaLoaiHinh);
                        }
                        if (!String.IsNullOrEmpty(bNganSach))
                        {
                            bang.CmdParams.Parameters.Clear();
                            bang.CmdParams.Parameters.AddWithValue("@sTen", dt.Rows[0]["sTen"] + " NSQP bảo đảm");
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
                            bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia);
                            bang.CmdParams.Parameters.AddWithValue("@iDM_MaDonViTinh", dt.Rows[0]["iDM_MaDonViTinh"]);
                            bang.CmdParams.Parameters.AddWithValue("@sTen_DonVi", dt.Rows[0]["sTen_DonVi"]);
                            bang.CmdParams.Parameters.AddWithValue("@bNganSach", 1);
                            bang.Save();
                            ThemChiTietGia(SanPham_DanhMucGiaModels.Get_MaxId_DanhMucGia(), iID_MaSanPham, iID_MaLoaiHinh);
                        }
                    }
                }
            }
            //bang.DuLieuMoi = true;
            //bang.Save();
            return RedirectToAction("Index", new { iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh });
        }
        public void ThemChiTietGia(String iID_MaDanhMucGia, String iID_MaSanPham, String iID_MaLoaiHinh)
        {
            DataTable dt = SanPham_DanhMucGiaModels.Get_ChiTietDanhMucGia_Row(iID_MaDanhMucGia);
            String iID_MaDanhMucGia_Cha = "0";
            if (dt.Rows.Count > 0)
            {
                DataRow CauHinh = dt.Rows[0];
                iID_MaDanhMucGia_Cha = Convert.ToString(CauHinh["iID_MaDanhMucGia_Cha"]);

                DataTable ListChiTietGia = SanPham_DanhMucGiaModels.Get_DanhSachChiTietGia(iID_MaSanPham, "", "", iID_MaLoaiHinh);
                foreach (DataRow row in ListChiTietGia.Rows)
                {

                    String iID_MaDanhMucGia_Cha_ChiTiet = "0";
                    if (iID_MaDanhMucGia_Cha != "0")
                    {
                        SqlCommand cmd = new SqlCommand();
                        String query = String.Format(@"SELECT TOP 1 iID_MaDanhMucGia FROM DM_SanPham_DanhMucGia WHERE iID_MaChiTietGia = @iID_MaChiTietGia AND iID_MaDanhMucGia_CauHinh = @iID_MaDanhMucGia_Cha");
                        cmd.Parameters.AddWithValue("@iID_MaChiTietGia", row["iID_MaChiTietGia"]);
                        cmd.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia_Cha);
                        cmd.CommandText = query;
                        iID_MaDanhMucGia_Cha_ChiTiet = Connection.GetValueString(cmd, "0");
                    }
                    Bang bang = new Bang("DM_SanPham_DanhMucGia");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.DuLieuMoi = true;
                    //bang.CmdParams.Parameters.AddWithValue("@iID_MaSanPham", row["iID_MaSanPham"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTietGia", row["iID_MaChiTietGia"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia_Cha_ChiTiet);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDanhMucGia_CauHinh", iID_MaDanhMucGia);
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", getMaxSTTConDanhMucGia_ChiTiet(Convert.ToString(row["iID_MaChiTietGia"]), iID_MaDanhMucGia_Cha_ChiTiet) + 1);
                    if (CauHinh["iID_MaVatTu"].ToString() == "dddddddd-dddd-dddd-dddd-dddddddddddd")
                    {
                        bang.CmdParams.Parameters.AddWithValue("@rDonGia_DangThucHien", 0);
                    }
                    else
                    {
                        DataTable dtGia = SanPham_VatTuModels.Get_GiaVatTu_Row(CauHinh["iID_MaVatTu"].ToString());
                        String GiaVatTu = "0";
                        if (dtGia.Rows.Count > 0)
                        {
                            if (!Convert.ToBoolean(CauHinh["bNganSach"]))
                            {
                                GiaVatTu = dtGia.Rows[0]["rGia"].ToString();
                            }
                            else
                            {
                                GiaVatTu = dtGia.Rows[0]["rGia_NS"].ToString();
                            }
                        }
                        if (String.IsNullOrEmpty(GiaVatTu)) GiaVatTu = "0";
                        bang.CmdParams.Parameters.AddWithValue("@rDonGia_DangThucHien", GiaVatTu);
                        bang.CmdParams.Parameters.AddWithValue("@rDonGia_DV_DeNghi", GiaVatTu);
                        bang.CmdParams.Parameters.AddWithValue("@rDonGia_DatHang_DeNghi", GiaVatTu);
                        bang.CmdParams.Parameters.AddWithValue("@rDonGia_CTC_DeNghi", GiaVatTu);
                    }
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String TenTruong = dt.Columns[j].ColumnName;
                        if (TenTruong != "iID_MaDanhMucGia" && TenTruong != "iID_MaDanhMucGia_Cha" && TenTruong != "iID_MaChiTietGia"
                            && TenTruong != "iID_MaDanhMucGia_CauHinh" && TenTruong != "dNgayTao" && TenTruong != "rDonGia_DangThucHien" && TenTruong != "rDonGia_DV_DeNghi"
                            && TenTruong != "rDonGia_DatHang_DeNghi" && TenTruong != "rDonGia_CTC_DeNghi")
                        {
                            bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, CauHinh[j]);
                        }
                    }
                    bang.Save();
                    int ThuTu = 0;
                    SanPham_DanhMucGiaModels.CapNhatSttDanhMucGia(iID_MaSanPham, Convert.ToString(row["iID_MaChiTietGia"]), "", 0, ref ThuTu);
                }
            }
        }
        public int getMaxSTTConDanhMucGia_ChiTiet(String iID_MaChiTietGia, String iID_MaDanhMucGia_Cha)
        {
            SqlCommand cmd = new SqlCommand();
            String query = String.Format(@"SELECT MAX(iSTT) AS maxSTT FROM DM_SanPham_DanhMucGia WHERE iID_MaChiTietGia = @iID_MaChiTietGia AND iID_MaDanhMucGia_Cha = @iID_MaDanhMucGia_Cha");
            cmd.Parameters.AddWithValue("@iID_MaChiTietGia", iID_MaChiTietGia);
            cmd.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia_Cha);
            cmd.CommandText = query;
            return (int)Connection.GetValue(cmd, 0);
        }
    }
}
