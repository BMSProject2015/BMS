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
    public class TCDN_HoSoDoanhNghiep_ChiTieuController : Controller
    {
        //
        // GET: /TCDN_HoSoDoanhNghiep_ChiTieu/
        public string sViewPath = "~/Views/TCDN/HoSoDoanhNghiep/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_HoSoDoanhNghiep_ChiTieu", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "TCDN_HoSoDoanhNghiep_ChiTieu_Index.aspx");
        }
        /// <summary>
        /// Thêm mục con
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(String iID_MaChiTieuHoSo_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_HoSoDoanhNghiep_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return RedirectToAction("Edit", new { iID_MaChiTieuHoSo_Cha = iID_MaChiTieuHoSo_Cha });
        }
        /// <summary>
        /// Action Thêm mới + Sửa Mục Lục Quân Số
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaChiTieuHoSo, String iID_MaChiTieuHoSo_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_HoSoDoanhNghiep_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaChiTieuHoSo))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaChiTieuHoSo"] = iID_MaChiTieuHoSo;
            ViewData["iID_MaChiTieuHoSo_Cha"] = iID_MaChiTieuHoSo_Cha;
            return View(sViewPath + "TCDN_HoSoDoanhNghiep_ChiTieu_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaChiTieuHoSo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_HoSoDoanhNghiep_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd;
            NameValueCollection arrLoi = new NameValueCollection();
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String iID_MaChiTieuHoSo_Cha = Convert.ToString(Request.Form[ParentID + "_iID_MaChiTieuHoSo_Cha"]);
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
                if (String.IsNullOrEmpty(iID_MaChiTieuHoSo))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaChiTieuHoSo"] = iID_MaChiTieuHoSo;
                ViewData["iID_MaChiTieuHoSo_Cha"] = iID_MaChiTieuHoSo_Cha;
                return View(sViewPath + "TCDN_HoSoDoanhNghiep_ChiTieu_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("TCDN_HoSoDoanhNghiep_ChiTieu");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);

                String iID_MaChiTieuHoSo_New = "";
                if (DuLieuMoi == "1")
                {
                    if (iID_MaChiTieuHoSo_Cha == null || iID_MaChiTieuHoSo_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaChiTieuHoSo_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    String SQL = "SELECT  MAX(iSTT) AS  iSTT FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE 1=1";
                    cmd = new SqlCommand();
                    if (iID_MaChiTieuHoSo_Cha != null && iID_MaChiTieuHoSo_Cha != "")
                    {
                        SQL += " AND iID_MaChiTieuHoSo_Cha=@iID_MaChiTieuHoSo_Cha";
                        cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo_Cha", iID_MaChiTieuHoSo_Cha);
                    }
                    cmd.CommandText = SQL;
                    int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                    cmd.Dispose();
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon);
                }
                if (DuLieuMoi == "0")
                {
                    if (iID_MaChiTieuHoSo_Cha == null || iID_MaChiTieuHoSo_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaChiTieuHoSo_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    bang.GiaTriKhoa = iID_MaChiTieuHoSo;
                    iID_MaChiTieuHoSo_New = iID_MaChiTieuHoSo;
                }

                bang.Save();

                if (DuLieuMoi == "1")
                {
                    String SQLMA = "SELECT  MAX(iID_MaChiTieuHoSo) AS  iID_MaChiTieuHoSo FROM TCDN_HoSoDoanhNghiep_ChiTieu WHERE iTrangThai=1";
                    cmd = new SqlCommand();
                    cmd.CommandText = SQLMA;
                    iID_MaChiTieuHoSo_New = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();
                }

                //Map dữ liệu giữa các chỉ tiêu cân đối kế toán
                if (iID_MaChiTieuHoSo_New != null && iID_MaChiTieuHoSo_New != "")
                {
                    cmd = new SqlCommand("DELETE FROM TCDN_HoSoDoanhNghiepChiTieu_ChiTieuCanDoi WHERE iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo");
                    cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo_New);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                    String iID_MaChiTieu = Convert.ToString(Request.Form["iID_MaChiTieu"]);
                    if (iID_MaChiTieu != null && iID_MaChiTieu != "")
                    {
                        String[] arrMaChiTieu = iID_MaChiTieu.Split(',');
                        for (int i = 0; i < arrMaChiTieu.Length; i++)
                        {
                            if (arrMaChiTieu[i] != "")
                            {
                                Bang bangct = new Bang("TCDN_HoSoDoanhNghiepChiTieu_ChiTieuCanDoi");
                                bangct.MaNguoiDungSua = User.Identity.Name;
                                bangct.IPSua = Request.UserHostAddress;
                                bangct.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu", arrMaChiTieu[i]);
                                bangct.CmdParams.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo_New);
                                bangct.Save();
                            }
                        }
                    }
                }

                //Map dữ liệu giữa các chỉ tiêu bảng báo cáo tài chính
                if (iID_MaChiTieuHoSo_New != null && iID_MaChiTieuHoSo_New != "")
                {
                    cmd = new SqlCommand("DELETE FROM TCDN_BaoCaoTaiChinhTruongLayDuLieu_ChiTieuHoSo WHERE iID_MaChiTieuHoSo=@iID_MaChiTieuHoSo");
                    cmd.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo_New);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                    String iID_MaChiTieuBaoCaoTaiChinh = Convert.ToString(Request.Form["iID_MaChiTieuBaoCaoTaiChinh"]);
                    if (iID_MaChiTieuBaoCaoTaiChinh != null && iID_MaChiTieuBaoCaoTaiChinh != "")
                    {
                        String[] arrMaChiTieuBaoCaoTaiChinh = iID_MaChiTieuBaoCaoTaiChinh.Split(',');
                        for (int i = 0; i < arrMaChiTieuBaoCaoTaiChinh.Length; i++)
                        {
                            if (arrMaChiTieuBaoCaoTaiChinh[i] != "")
                            {
                                Bang bangct = new Bang("TCDN_BaoCaoTaiChinhTruongLayDuLieu_ChiTieuHoSo");
                                bangct.MaNguoiDungSua = User.Identity.Name;
                                bangct.IPSua = Request.UserHostAddress;
                                bangct.CmdParams.Parameters.AddWithValue("@iID_MaTruongKhoa", arrMaChiTieuBaoCaoTaiChinh[i]);
                                bangct.CmdParams.Parameters.AddWithValue("@iID_MaChiTieuHoSo", iID_MaChiTieuHoSo_New);
                                bangct.Save();
                            }
                        }
                    }
                }

                return RedirectToAction("Index", new { iID_MaChiTieuHoSo_Cha = iID_MaChiTieuHoSo_Cha });
            }
        }
        /// <summary>
        /// Hiển thị form sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaChiTieuHoSo_Cha"></param>
        /// <param name="iID_MaChiTieuHoSo"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String iID_MaChiTieuHoSo_Cha, String iID_MaChiTieuHoSo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_HoSoDoanhNghiep_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(iID_MaChiTieuHoSo_Cha))
            {
                iID_MaChiTieuHoSo_Cha = "";
            }
            ViewData["iID_MaChiTieuHoSo_Cha"] = iID_MaChiTieuHoSo_Cha;
            ViewData["iID_MaChiTieuHoSo"] = iID_MaChiTieuHoSo;
            return View(sViewPath + "TCDN_HoSoDoanhNghiep_ChiTieu_Sort.aspx");
        }
        /// <summary>
        /// Sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaChiTieuHoSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaChiTieuHoSo_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_HoSoDoanhNghiep_ChiTieu", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            for (i = 0; i < arrTG.Length - 1; i++)
            {
                Bang bang = new Bang("TCDN_HoSoDoanhNghiep_ChiTieu");
                bang.GiaTriKhoa = arrTG[i];
                bang.DuLieuMoi = false;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            return RedirectToAction("Index", new { iID_MaChiTieuHoSo_Cha = iID_MaChiTieuHoSo_Cha });
        }
        /// <summary>
        /// Lệnh điều hướng xóa tài khoản kế toán
        /// </summary>
        /// <param name="iID_MaChiTieuHoSo">Mã tài khoản</param>
        /// <param name="iID_MaChiTieuHoSo_Cha">Mã tài khoản cấp cha</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaChiTieuHoSo, String iID_MaChiTieuHoSo_Cha)
        {
            //kiểm tra quyền có được phép xóa
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "TCDN_HoSoDoanhNghiep_ChiTieu", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Boolean bDelete = false;
            bDelete = TCDN_HoSoDoanhNghiep_ChiTieuModels.Delete(iID_MaChiTieuHoSo);
            if (bDelete == true)
            {
                return RedirectToAction("Index", new { iID_MaChiTieuHoSo_Cha = iID_MaChiTieuHoSo_Cha });
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
            return RedirectToAction("Index", "TCDN_HoSoDoanhNghiep_ChiTieu", new { ParentID = ParentID, Ten = sTaiKhoan, KyHieu = sKyHieu });
        }

        public Boolean CheckMaTaiKhoan(String sKyHieu)
        {
            Boolean vR = false;
            int iSoDuLieu = TCDN_HoSoDoanhNghiep_ChiTieuModels.Get_So_KyHieuChiTieu(sKyHieu);
            if (iSoDuLieu > 0)
            {
                vR = true;
            }
            return vR;
        }


    }
}
