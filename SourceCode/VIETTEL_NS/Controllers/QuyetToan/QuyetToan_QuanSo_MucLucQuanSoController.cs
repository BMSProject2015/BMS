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

namespace VIETTEL.Controllers.QuyetToan
{
    public class QuyetToan_QuanSo_MucLucQuanSoController : Controller
    {
        //
        // GET: /QuyetToan_QuanSo_MucLucQuanSo/
        public string sViewPath = "~/Views/QuyetToan/QuanSo/MucLucQuanSo/";
        /// <summary>
        /// Action Index
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucQuanSo", "List") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "QuyetToan_QuanSo_MucLuc_Index.aspx");
        }
        /// <summary>
        /// Thêm mục con
        /// </summary>
        /// <param name="iID_MaMucLucQuanSo_Cha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(String iID_MaMucLucQuanSo_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucQuanSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return RedirectToAction("Edit", new { iID_MaMucLucQuanSo_Cha = iID_MaMucLucQuanSo_Cha });
        }
        /// <summary>
        /// Action Thêm mới + Sửa Mục Lục Quân Số
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaMucLucQuanSo, String iID_MaMucLucQuanSo_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucQuanSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaMucLucQuanSo))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            ViewData["iID_MaMucLucQuanSo"] = iID_MaMucLucQuanSo;
            ViewData["iID_MaMucLucQuanSo_Cha"] = iID_MaMucLucQuanSo_Cha;
            return View(sViewPath + "QuyetToan_QuanSo_MucLuc_Edit.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaMucLucQuanSo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucQuanSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd;
            NameValueCollection arrLoi = new NameValueCollection();
            String sKyHieu = Convert.ToString(Request.Form[ParentID + "_sKyHieu"]);
            String sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);
           
            String iID_MaMucLucQuanSo_Cha = Convert.ToString(Request.Form[ParentID + "_iID_MaMucLucQuanSo_Cha"]);
            String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
            String NewID = Globals.getNewGuid().ToString();
            if (sKyHieu == string.Empty || sKyHieu == "")
            {
                arrLoi.Add("err_sKyHieu", "Bạn chưa nhập ký hiệu");
            }
            if (sMoTa == string.Empty || sMoTa == "")
            {
                arrLoi.Add("err_sMoTa", "Bạn chưa nhập mô tả");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaMucLucQuanSo))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaMucLucQuanSo"] = iID_MaMucLucQuanSo;
                ViewData["iID_MaMucLucQuanSo_Cha"] = iID_MaMucLucQuanSo_Cha;
                return View(sViewPath + "QuyetToan_QuanSo_MucLuc_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("NS_MucLucQuanSo");                
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                if (DuLieuMoi == "1")
                {
                    if (iID_MaMucLucQuanSo_Cha == null || iID_MaMucLucQuanSo_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaMucLucQuanSo_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    cmd = new SqlCommand("SELECT COUNT(*) FROM NS_MucLucQuanSo WHERE iID_MaMucLucQuanSo_Cha=@iID_MaMucLucQuanSo_Cha");
                    cmd.Parameters.AddWithValue("@iID_MaMucLucQuanSo_Cha", iID_MaMucLucQuanSo_Cha);
                    int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                    cmd.Dispose();
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon);
                }
                if (DuLieuMoi == "0") {
                    if (iID_MaMucLucQuanSo_Cha == null || iID_MaMucLucQuanSo_Cha == "")
                    {
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaMucLucQuanSo_Cha");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                    }
                    bang.GiaTriKhoa = iID_MaMucLucQuanSo;
                }
                bang.Save();
                return RedirectToAction("Index", new { iID_MaMucLucQuanSo_Cha = iID_MaMucLucQuanSo_Cha });
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String iID_MaMucLucQuanSo_Cha, String iID_MaMucLucQuanSo)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucQuanSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            if (String.IsNullOrEmpty(iID_MaMucLucQuanSo_Cha))
            {
                iID_MaMucLucQuanSo_Cha = "";
            }
            ViewData["iID_MaMucLucQuanSo_Cha"] = iID_MaMucLucQuanSo_Cha;
            ViewData["iID_MaMucLucQuanSo"] = iID_MaMucLucQuanSo;
            return View(sViewPath + "QuyetToan_QuanSo_MucLuc_Sort.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaMucLucQuanSo_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucQuanSo", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            string strOrder = Request.Form["hiddenOrder"].ToString();
            String[] arrTG = strOrder.Split('$');
            int i;
            for (i = 0; i < arrTG.Length - 1; i++)
            {
                Bang bang = new Bang("NS_MucLucQuanSo");
                bang.GiaTriKhoa = arrTG[i];
                bang.DuLieuMoi = false;
                bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                bang.Save();
            }
            return RedirectToAction("Index", new { iID_MaMucLucQuanSo_Cha = iID_MaMucLucQuanSo_Cha });
        }
        [Authorize]
        public ActionResult Delete(String iID_MaMucLucQuanSo, String iID_MaMucLucQuanSo_Cha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucQuanSo", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Boolean bDelete = QuyetToan_QuanSo_MucLucModels.DeleteMucLucQuanSo(iID_MaMucLucQuanSo);

            if (bDelete == true)
            {
                return RedirectToAction("Index", new { iID_MaMucLucQuanSo_Cha = iID_MaMucLucQuanSo_Cha });
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
    }
}
