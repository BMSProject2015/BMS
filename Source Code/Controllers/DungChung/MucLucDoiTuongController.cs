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

namespace VIETTEL.Controllers.DungChung
{
    public class MucLucDoiTuongController : Controller
    {
        //
        // GET: /MucLucDoiTuong/
        public string sViewPath = "~/Views/DungChung/MucLucDoiTuong/";

        /// <summary>
        /// Action Index
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {

                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDoiTuong", "List") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return View(sViewPath + "MucLuc_DoiTuong_Index.aspx");

            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        /// <summary>
        /// Thêm mục con
        /// </summary>
        /// <param name="iID_MaMucLucDoiTuong_Cha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(String iID_MaMucLucDoiTuong_Cha)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDoiTuong", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                return RedirectToAction("Edit", new {iID_MaMucLucDoiTuong_Cha = iID_MaMucLucDoiTuong_Cha});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        /// <summary>
        /// Action Thêm mới + Sửa Mục Lục Quân Số
        /// </summary>
        /// <param name="MaHangMau"></param>
        /// <param name="MaHangMauCha"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(String iID_MaMucLucDoiTuong, String iID_MaMucLucDoiTuong_Cha)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDoiTuong", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(iID_MaMucLucDoiTuong))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["iID_MaMucLucDoiTuong"] = iID_MaMucLucDoiTuong;
                ViewData["iID_MaMucLucDoiTuong_Cha"] = iID_MaMucLucDoiTuong_Cha;
                return View(sViewPath + "MucLuc_DoiTuong_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String iID_MaMucLucDoiTuong)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDoiTuong", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                SqlCommand cmd;
                NameValueCollection arrLoi = new NameValueCollection();
                String sKyHieu = Convert.ToString(Request.Form[ParentID + "_sKyHieu"]);
                String sMoTa = Convert.ToString(Request.Form[ParentID + "_sMoTa"]);
                String iID_MaMucLucDoiTuong_Cha = Convert.ToString(Request.Form[ParentID + "_iID_MaMucLucDoiTuong_Cha"]);
                String DuLieuMoi = Convert.ToString(Request.Form[ParentID + "_DuLieuMoi"]);
                String NewID = Globals.getNewGuid().ToString();
                Boolean bDuLieuMoi=true;
                if(DuLieuMoi=="0")
                    bDuLieuMoi=false;
                //Kiểm tra xem trung ký hiệu
                Boolean Trung = HamChung.Check_Trung("NS_MucLucDoiTuong","iID_MaMucLucDoiTuong",iID_MaMucLucDoiTuong,"sKyHieu",sKyHieu,bDuLieuMoi);
//                int iSoKyHieu = NganSach_DoiTuongModels.GetKyHieuDoiTuong(sKyHieu);
                if (Trung)
                {
                    arrLoi.Add("err_sKyHieu", "Ký hiệu đã tồn tại!");
                }
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
                    if (String.IsNullOrEmpty(iID_MaMucLucDoiTuong))
                    {
                        ViewData["DuLieuMoi"] = "1";
                    }
                    ViewData["iID_MaMucLucDoiTuong"] = iID_MaMucLucDoiTuong;
                    ViewData["iID_MaMucLucDoiTuong_Cha"] = iID_MaMucLucDoiTuong_Cha;
                    return View(sViewPath + "MucLuc_DoiTuong_Edit.aspx");
                }
                else
                {
                    Bang bang = new Bang("NS_MucLucDoiTuong");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.TruyenGiaTri(ParentID, Request.Form);
                    if (DuLieuMoi == "1")
                    {
                        if (iID_MaMucLucDoiTuong_Cha == null || iID_MaMucLucDoiTuong_Cha == "")
                        {
                            int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaMucLucDoiTuong_Cha");
                            if (cs >= 0)
                            {
                                bang.CmdParams.Parameters.RemoveAt(cs);
                            }
                        }
                        cmd =
                            new SqlCommand(
                                "SELECT COUNT(*) FROM NS_MucLucDoiTuong WHERE iID_MaMucLucDoiTuong_Cha=@iID_MaMucLucDoiTuong_Cha");
                        cmd.Parameters.AddWithValue("@iID_MaMucLucDoiTuong_Cha", iID_MaMucLucDoiTuong_Cha);
                        int SoHangMauCon = Convert.ToInt32(Connection.GetValue(cmd, 0));
                        cmd.Dispose();
                        bang.CmdParams.Parameters.AddWithValue("@iSTT", SoHangMauCon);
                    }
                    if (DuLieuMoi == "0")
                    {
                        if (iID_MaMucLucDoiTuong_Cha == null || iID_MaMucLucDoiTuong_Cha == "")
                        {
                            int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaMucLucDoiTuong_Cha");
                            if (cs >= 0)
                            {
                                bang.CmdParams.Parameters.RemoveAt(cs);
                            }
                        }
                        bang.GiaTriKhoa = iID_MaMucLucDoiTuong;
                    }
                    bang.Save();
                    return RedirectToAction("Index", new {iID_MaMucLucDoiTuong_Cha = iID_MaMucLucDoiTuong_Cha});
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sort(String iID_MaMucLucDoiTuong_Cha, String iID_MaMucLucDoiTuong)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDoiTuong", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                if (String.IsNullOrEmpty(iID_MaMucLucDoiTuong_Cha))
                {
                    iID_MaMucLucDoiTuong_Cha = "";
                }
                ViewData["iID_MaMucLucDoiTuong_Cha"] = iID_MaMucLucDoiTuong_Cha;
                ViewData["iID_MaMucLucDoiTuong"] = iID_MaMucLucDoiTuong;
                return View(sViewPath + "MucLuc_DoiTuong_Sort.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SortSubmit(String iID_MaMucLucDoiTuong_Cha)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDoiTuong", "Edit") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                string strOrder = Request.Form["hiddenOrder"].ToString();
                String[] arrTG = strOrder.Split('$');
                int i;
                for (i = 0; i < arrTG.Length - 1; i++)
                {
                    Bang bang = new Bang("NS_MucLucDoiTuong");
                    bang.GiaTriKhoa = arrTG[i];
                    bang.DuLieuMoi = false;
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                    bang.Save();
                }
                return RedirectToAction("Index", new {iID_MaMucLucDoiTuong_Cha = iID_MaMucLucDoiTuong_Cha});
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Delete(String iID_MaMucLucDoiTuong, String iID_MaMucLucDoiTuong_Cha)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_MucLucDoiTuong", "Delete") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }

                Boolean bDelete = NganSach_DoiTuongModels.DeleteMucLucDoiTuong(iID_MaMucLucDoiTuong);

                if (bDelete == true)
                {
                    return RedirectToAction("Index", new {iID_MaMucLucDoiTuong_Cha = iID_MaMucLucDoiTuong_Cha});
                }
                else
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
    }
}
