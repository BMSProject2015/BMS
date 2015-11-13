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

namespace VIETTEL.Controllers.CongSan
{
    public class KTCS_NhomTaiSanController : Controller
    {
        //
        // GET: /KTCS_NhomTaiSan/
        public string sViewPath = "~/Views/CongSan/NhomTaiSan/";
        /// <summary>
        /// Điều hướng về form danh sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_NhomTaiSan", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "KeToanCongSan_NhomTaiSan_Index.aspx");
        }
        /// <summary>
        /// Thêm mục con
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(String iID_MaNhomTaiSan_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_NhomTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return RedirectToAction("Edit", "KTCS_NhomTaiSan", new { iID_MaNhomTaiSan_Cha = iID_MaNhomTaiSan_Cha });
        }
        /// <summary>
        /// Action Thêm mới + Sửa Mục Lục Quân Số
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaNhomTaiSan, String iID_MaNhomTaiSan_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_NhomTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaNhomTaiSan))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaNhomTaiSan"] = iID_MaNhomTaiSan;
            ViewData["iID_MaNhomTaiSan_Cha"] = iID_MaNhomTaiSan_Cha;
            return View(sViewPath + "KeToanCongSan_NhomTaiSan_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaNhomTaiSan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_NhomTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            NameValueCollection arrLoi = new NameValueCollection();
            String siID_MaNhomTaiSan = Convert.ToString(Request.Form[ParentID + "_iID_MaNhomTaiSan"]);
            String iID_MaLoaiTaiSan = Request.Form[ParentID + "_iID_MaLoaiTaiSan"];
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String iID_MaNhomTaiSan_Cha = Convert.ToString(Request.Form[ParentID + "_iID_MaNhomTaiSan_Cha"]);
            String sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);
            String rSoNamKhauHao = Convert.ToString(Request.Form[ParentID + "_rSoNamKhauHao"]);
            String bLaHangCha = Convert.ToString(Request.Form[ParentID + "_bLaHangCha"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            if (siID_MaNhomTaiSan == string.Empty || siID_MaNhomTaiSan == "")
            {
                arrLoi.Add("err_iID_MaNhomTaiSan", MessageModels.sKyHieu);
            }
            if (sTen == string.Empty || sTen == "")
            {
                arrLoi.Add("err_sTen", MessageModels.sTen);
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                if (CheckMaTaiKhoan(siID_MaNhomTaiSan) == true)
                {
                    arrLoi.Add("err_iID_MaNhomTaiSan", "Mã tài khoản đã tồn tại!");
                }
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaNhomTaiSan))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaNhomTaiSan"] = iID_MaNhomTaiSan;
                ViewData["iID_MaNhomTaiSan_Cha"] = iID_MaNhomTaiSan_Cha;
                return View(sViewPath + "KeToanCongSan_NhomTaiSan_Edit.aspx");
            }
            else
            {
                if (DuLieuMoi == "1")
                {
                    string TaiKhoanCha = KTCS_NhomTaiSanModels.LayTaiKhoanCha(iID_MaNhomTaiSan);
                    string sTaiKhoanCha = iID_MaNhomTaiSan_Cha;
                    SqlCommand cmd;
                    Bang bang = new Bang("KTCS_NhomTaiSan");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);

                    string SQL = "SELECT  MAX(iSTT) AS  iSTT FROM KTCS_NhomTaiSan WHERE 1=1";
                    cmd = new SqlCommand();
                    if (iID_MaNhomTaiSan_Cha != null && iID_MaNhomTaiSan_Cha != "")
                    {
                        SQL += " AND iID_MaNhomTaiSan_Cha=@iID_MaNhomTaiSan_Cha";
                        cmd.Parameters.AddWithValue("@iID_MaNhomTaiSan_Cha", iID_MaNhomTaiSan_Cha);
                    }
                    cmd.CommandText = SQL;
                    int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                    cmd.Dispose();
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon);

                    if (iID_MaNhomTaiSan_Cha == null || iID_MaNhomTaiSan_Cha == "")
                    {
                        bang.CmdParams.Parameters["@iID_MaNhomTaiSan_Cha"].Value = TaiKhoanCha;
                        sTaiKhoanCha = TaiKhoanCha;
                    }
                    bang.Save();
                    cmd.Dispose();
                    if (String.IsNullOrEmpty(sTaiKhoanCha) == false)
                    {
                        KTCS_NhomTaiSanModels.UpdateTaiKhoan_LaHangCha(sTaiKhoanCha);
                    }
                }
                else
                {
                    Boolean LaHangCha = false;
                    if (bLaHangCha == "on") LaHangCha = true;
                    String SQL = "UPDATE KTCS_NhomTaiSan SET iID_MaNhomTaiSan=@iID_MaNhomTaiSan, sTen=@sTen, iID_MaNhomTaiSan_Cha=@iID_MaNhomTaiSan_Cha, sMoTa=@sMoTa," +
                        " rSoNamKhauHao=@rSoNamKhauHao,bLaHangCha=@bLaHangCha,iID_MaLoaiTaiSan=@iID_MaLoaiTaiSan WHERE iID_MaNhomTaiSan=@iID_MaNhomTaiSan";
                    SqlCommand cmdSQL = new SqlCommand();
                    cmdSQL.CommandText = SQL;
                    cmdSQL.Parameters.AddWithValue("@iID_MaNhomTaiSan", siID_MaNhomTaiSan);
                    cmdSQL.Parameters.AddWithValue("@sTen", sTen);
                    if (iID_MaNhomTaiSan_Cha == null || iID_MaNhomTaiSan_Cha == "")
                    {
                        cmdSQL.Parameters.AddWithValue("@iID_MaNhomTaiSan_Cha", KTCS_NhomTaiSanModels.LayTaiKhoanCha(iID_MaNhomTaiSan));
                    }
                    else cmdSQL.Parameters.AddWithValue("@iID_MaNhomTaiSan_Cha", iID_MaNhomTaiSan_Cha);
                    cmdSQL.Parameters.AddWithValue("@sMoTa", sMoTa);
                    cmdSQL.Parameters.AddWithValue("@rSoNamKhauHao", Convert.ToDouble(rSoNamKhauHao));
                    cmdSQL.Parameters.AddWithValue("@bLaHangCha", LaHangCha);
                    cmdSQL.Parameters.AddWithValue("@iID_MaLoaiTaiSan", iID_MaLoaiTaiSan);
                    Connection.UpdateDatabase(cmdSQL);
                    cmdSQL.Dispose();
                }
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Hiển thị form sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaNhomTaiSan_Cha"></param>
        /// <param name="iID_MaNhomTaiSan"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String iID_MaNhomTaiSan_Cha, String iID_MaNhomTaiSan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_NhomTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(iID_MaNhomTaiSan_Cha))
            {
                iID_MaNhomTaiSan_Cha = "";
            }
            ViewData["iID_MaNhomTaiSan_Cha"] = iID_MaNhomTaiSan_Cha;
            ViewData["iID_MaNhomTaiSan"] = iID_MaNhomTaiSan;
            return View(sViewPath + "KeToanCongSan_NhomTaiSan_Sort.aspx");
        }
        /// <summary>
        /// Sắp xếp tài khoản
        /// </summary>
        /// <param name="iID_MaNhomTaiSan_Cha"></param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaNhomTaiSan_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_NhomTaiSan", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            for (i = 0; i < arrTG.Length - 1; i++)
            {
                Bang bang = new Bang("KTCS_NhomTaiSan");
                bang.GiaTriKhoa = arrTG[i];
                bang.DuLieuMoi = false;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            return RedirectToAction("Index", new { iID_MaNhomTaiSan_Cha = iID_MaNhomTaiSan_Cha });
        }
        /// <summary>
        /// Lệnh điều hướng xóa tài khoản kế toán
        /// </summary>
        /// <param name="iID_MaNhomTaiSan">Mã tài khoản</param>
        /// <param name="iID_MaNhomTaiSan_Cha">Mã tài khoản cấp cha</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaNhomTaiSan, String iID_MaNhomTaiSan_Cha)
        {
            //kiểm tra quyền có được phép xóa
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KTCS_NhomTaiSan", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Boolean bDelete = false;
            bDelete = KTCS_NhomTaiSanModels.DeleteTaiKhoan(iID_MaNhomTaiSan);
            if (bDelete == true)
            {
                return RedirectToAction("Index", new { iID_MaNhomTaiSan_Cha = iID_MaNhomTaiSan_Cha });
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
            return RedirectToAction("Index", "KTCS_NhomTaiKhoan", new { ParentID = ParentID, Ten = sTaiKhoan, KyHieu = sKyHieu });
        }

        public Boolean CheckMaTaiKhoan(String iID_MaNhomTaiSan)
        {
            Boolean vR = false;
            DataTable dt = KTCS_NhomTaiSanModels.getChiTietTK(iID_MaNhomTaiSan);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            return vR;
        }
    }
}
