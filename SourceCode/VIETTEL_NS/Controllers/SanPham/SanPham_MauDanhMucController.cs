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
using System.Collections;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Text;

namespace VIETTEL.Controllers.TCDN
{
    public class SanPham_MauDanhMucController : Controller
    {
        //
        // GET: / SanPham_MauDanhMuc/
        public string sViewPath = "~/Views/SanPham/MauDanhMuc/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, " DC_DanhMuc", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "Index.aspx");
        }
        /// <summary>
        /// Action Thêm mới + Sửa
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaDanhMuc, String iID_MaDanhMucCha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DC_DanhMuc", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaDanhMuc))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaDanhMuc"] = iID_MaDanhMuc;
            ViewData["iID_MaDanhMucCha"] = iID_MaDanhMucCha;
            return View(sViewPath + "Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaDanhMuc)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DC_DanhMuc", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd;
            NameValueCollection arrLoi = new NameValueCollection();
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String iID_MaDanhMucCha = Convert.ToString(Request.Form[ParentID + "_iID_MaDanhMucCha"]);
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
                if (String.IsNullOrEmpty(iID_MaDanhMuc))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaDanhMuc"] = iID_MaDanhMuc;
                ViewData["iID_MaDanhMucCha"] = iID_MaDanhMucCha;
                return View(sViewPath + "Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("DC_DanhMuc");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (DuLieuMoi == "1")
                {
                    if (iID_MaDanhMucCha == null || iID_MaDanhMucCha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaDanhMucCha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    string SQL = "SELECT  MAX(iSTT) AS  iSTT FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang = 'MauDanhMucGiaSP')";
                    cmd = new SqlCommand();
                    if (iID_MaDanhMucCha != null && iID_MaDanhMucCha != "")
                    {
                        SQL += " AND iID_MaDanhMucCha=@iID_MaDanhMucCha";
                        cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", iID_MaDanhMucCha);
                    }
                    cmd.CommandText = SQL;
                    int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon + 1);

                    cmd.CommandText = "SELECT TOP 1 iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang = 'MauDanhMucGiaSP'";
                    String iID_MaLoaiDanhMuc = Convert.ToString(Connection.GetValue(cmd, "dddddddd-dddd-dddd-dddd-dddddddddddd"));
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", iID_MaLoaiDanhMuc);
                    cmd.Dispose();
                }
                if (DuLieuMoi == "0")
                {
                    if (iID_MaDanhMucCha == null || iID_MaDanhMucCha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaDanhMucCha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    bang.GiaTriKhoa = iID_MaDanhMuc;
                }
                bang.Save();
                return RedirectToAction("Index", new {});
            }
        }
        /// <summary>
        /// Hiển thị form sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaDanhMucCha"></param>
        /// <param name="iID_MaDanhMuc"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String iID_MaDanhMucCha, String iID_MaDanhMuc)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DC_DanhMuc", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(iID_MaDanhMucCha))
            {
                iID_MaDanhMucCha = "";
            }
            ViewData["iID_MaDanhMucCha"] = iID_MaDanhMucCha;
            ViewData["iID_MaDanhMuc"] = iID_MaDanhMuc;
            return View(sViewPath + "Sort.aspx");
        }
        /// <summary>
        /// Sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaDanhMucCha"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaDanhMucCha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DC_DanhMuc", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            for (i = 0; i < arrTG.Length - 1; i++)
            {
                Bang bang = new Bang("DC_DanhMuc");
                bang.GiaTriKhoa = arrTG[i];
                bang.DuLieuMoi = false;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            return RedirectToAction("Index", new { });
        }
        /// <summary>
        /// Lệnh điều hướng xóa tài khoản kế toán
        /// </summary>
        /// <param name="iID_MaDanhMuc">Mã tài khoản</param>
        /// <param name="iID_MaDanhMucCha">Mã tài khoản cấp cha</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaDanhMuc)
        {
            //kiểm tra quyền có được phép xóa
            //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "DC_DanhMuc", "Delete") == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}

            Boolean bDelete = true;
            try
            {
                Bang bang = new Bang("DC_DanhMuc");
                bang.GiaTriKhoa = iID_MaDanhMuc;
                bang.Delete();
            }
            catch (System.Exception ex)
            {
                bDelete = false;
            }
            if (bDelete == true)
            {
                return RedirectToAction("Index", new { });
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
            return RedirectToAction("Index",new {});
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
        public static ArrayList LayDanhSachDanhMuc(String MaDanhMucCha, int Cap, ref int ThuTu)
        {
            String SQL = "";
            ArrayList ListChiTiet = new ArrayList();
            SqlCommand cmd = new SqlCommand();
            SQL = "SELECT * FROM DC_DanhMuc WHERE bHoatDong=1 AND iTrangThai = 1";
            if (String.IsNullOrEmpty(MaDanhMucCha))
                SQL += " AND iID_MaDanhMucCha IS NULL ";
            else
                SQL += " AND iID_MaDanhMucCha = '" + MaDanhMucCha + "'";
            SQL += " AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang = 'MauDanhMucGiaSP')";
            SQL += " ORDER BY iSTT";
            cmd.CommandText = SQL;
            //DataTable dt = CommonFunction.dtData(cmd, "", Trang, SoBanGhi);
            DataTable dt = Connection.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;
                String strDoanTrang = "";
                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    // int STT = i + 1;
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    Hashtable rowChiTiet = new Hashtable();
                    ArrayList listMucLucQuanSoCon = new ArrayList();
                    rowChiTiet.Add("Cap", Cap);
                    rowChiTiet.Add("tgThuTu", tgThuTu);
                    rowChiTiet.Add("iID_MaDanhMuc", Row["iID_MaDanhMuc"]);
                    rowChiTiet.Add("sTenKhoa", strDoanTrang + Row["sTenKhoa"].ToString());
                    rowChiTiet.Add("sTen", strDoanTrang + Row["sTen"].ToString());
                    listMucLucQuanSoCon = LayDanhSachDanhMuc(Convert.ToString(Row["iID_MaDanhMuc"]), Cap + 1, ref ThuTu);
                    if (listMucLucQuanSoCon.Count > 0)
                    {
                        rowChiTiet.Add("laCha", 1);
                    }
                    else
                    {
                        rowChiTiet.Add("laCha", 0);
                    }
                    ListChiTiet.Add(rowChiTiet);
                    if (listMucLucQuanSoCon.Count > 0) ListChiTiet.AddRange(listMucLucQuanSoCon);
                }
            }
            dt.Dispose();
            return ListChiTiet;
        }
    }
}
