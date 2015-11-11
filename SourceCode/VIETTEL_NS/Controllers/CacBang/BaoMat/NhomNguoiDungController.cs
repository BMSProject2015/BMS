using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DomainModel.Controls;
using DomainModel;
using System.Data.SqlClient;
using System.Collections.Specialized;
using DomainModel.Abstract;
using VIETTEL.Models;
namespace Oneres.Controllers.NhomNguoiDung
{
    public class NhomNguoiDungController : Controller
    {
        public string sViewPath = "~/Views/CacBang/BaoMat/NhomNguoiDung/";

        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NhomNguoiDung", "Detail") == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                Bang bang = new Bang("QT_NhomNguoiDung");
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "List.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Detail(string MaNhomNguoiDung)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NhomNguoiDung", "Detail") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("QT_NhomNguoiDung");
            bang.GiaTriKhoa = MaNhomNguoiDung;
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, false);
            if (dicData != null)
            {
                ViewData[bang.TenBang + "_dicData"] = dicData;
                return View(sViewPath + "Detail.aspx");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(string MaNhomNguoiDungCha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NhomNguoiDung", "Create") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("QT_NhomNguoiDung");
            Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
            NameValueCollection data = (NameValueCollection)dicData["data"];
            dicData["DuLieuMoi"] = "1";
            dicData["MaNhomNguoiDungCha"] = MaNhomNguoiDungCha;
            ViewData[bang.TenBang + "_dicData"] = dicData;
            return View(sViewPath + "Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string MaNhomNguoiDung)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NhomNguoiDung", "Delete") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            Bang bang = new Bang("QT_NhomNguoiDung");
            bang.MaNguoiDungSua = User.Identity.Name;
            bang.IPSua = Request.UserHostAddress;
            bang.GiaTriKhoa = MaNhomNguoiDung;
            bang.Delete();
            return RedirectToAction("Edit");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string MaNhomNguoiDung)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                String sChucNang = "Create";
                if (MaNhomNguoiDung != null)
                {
                    sChucNang = "Edit";
                }
                if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NhomNguoiDung", sChucNang) == false)
                {
                    return RedirectToAction("Index", "PermitionMessage");
                }
                Bang bang = new Bang("QT_NhomNguoiDung");
                bang.GiaTriKhoa = MaNhomNguoiDung;
                Dictionary<string, object> dicData = bang.LayGoiDuLieu(null, true);
                NameValueCollection data = (NameValueCollection)dicData["data"];
                if (dicData != null)
                {
                    dicData["DuLieuMoi"] = "0";
                    int cs = MaNhomNguoiDung.LastIndexOf("-");
                    String MaNhomNguoiDungCha = MaNhomNguoiDung.Substring(0, cs);
                    dicData["MaNhomNguoiDungCha"] = MaNhomNguoiDungCha;
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Edit.aspx");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ControlID, String MaNhomNguoiDungCha)
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                Bang bang = new Bang("QT_NhomNguoiDung");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                NameValueCollection arrLoi = bang.TruyenGiaTri(ControlID, Request.Form);
                if (arrLoi.Count == 0)
                {
                    if (bang.DuLieuMoi)
                    {
                        String SQL = String.Format("UPDATE QT_NhomNguoiDung SET iSoLuongNhomCon=iSoLuongNhomCon+1 WHERE iID_MaNhomNguoiDung='{0}'", MaNhomNguoiDungCha);
                        SqlCommand cmd = new SqlCommand(SQL);
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();
                        SQL = String.Format("SELECT iSoLuongNhomCon FROM QT_NhomNguoiDung WHERE iID_MaNhomNguoiDung='{0}'", MaNhomNguoiDungCha);
                        int SoNhom = Convert.ToInt16(Connection.GetValue(SQL, 0));
                        bang.GiaTriKhoa = String.Format("{0}-{1}", MaNhomNguoiDungCha, SoNhom);
                        int cs = bang.CmdParams.Parameters.IndexOf("@iID_MaNhomNguoiDung");
                        if (cs >= 0)
                        {
                            bang.CmdParams.Parameters.RemoveAt(cs);
                        }
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaNhomNguoiDung", bang.GiaTriKhoa);
                    }
                    bang.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    for (int i = 0; i <= arrLoi.Count - 1; i++)
                    {
                        ModelState.AddModelError(ControlID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                    }
                    Dictionary<string, object> dicData = bang.LayGoiDuLieu(Request.Form, true);
                    ViewData[bang.TenBang + "_dicData"] = dicData;
                    return View(sViewPath + "Edit.aspx");
                }
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        [Authorize]
        public ActionResult Edit_Luat(string MaNhomNguoiDung)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NhomNguoiDung", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["MaNhomNguoiDung"] = MaNhomNguoiDung;
            return View(sViewPath + "Edit_Luat.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete_Luat(string MaNhomNguoiDung, string MaLuat)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NhomNguoiDung", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM PQ_NhomNguoiDung_Luat WHERE iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung AND iID_MaLuat=@iID_MaLuat";
            cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
            cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return RedirectToAction("Edit_Luat", new { MaNhomNguoiDung = MaNhomNguoiDung });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditLuatSubmit(String ControlID, String MaNhomNguoiDung)
        {
            if (!HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String MaLuat = Request.Form[ControlID + "_iID_MaLuat"];
            if (String.IsNullOrEmpty(MaLuat) == false)
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT COUNT(*) FROM PQ_NhomNguoiDung_Luat WHERE iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung AND iID_MaLuat=@iID_MaLuat";
                cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
                cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
                if ((int)(Connection.GetValue(cmd, 0)) == 0)
                {
                    Bang bang = new Bang("PQ_NhomNguoiDung_Luat");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
                    bang.Save();
                }
                cmd.Dispose();
            }
            return RedirectToAction("Edit_Luat", new { MaNhomNguoiDung = MaNhomNguoiDung });
        }

        [Authorize]
        public ActionResult Edit_NguoiDung(string MaNhomNguoiDung)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NguoiDung", "Edit") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["MaNhomNguoiDung"] = MaNhomNguoiDung;
            return View(sViewPath + "Edit_NguoiDung.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete_NguoiDung(string MaNhomNguoiDung, string MaNguoiDung)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "QT_NguoiDung", "Delete") == false || !HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE QT_NguoiDung SET iID_MaNhomNguoiDung='' WHERE sID_MaNguoiDung=@sID_MaNguoiDung";
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaNguoiDung);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            return RedirectToAction("Edit_NguoiDung", new { MaNhomNguoiDung = MaNhomNguoiDung });
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditNguoiDungSubmit(String ControlID, String MaNhomNguoiDung)
        {
            if (!HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String MaNguoiDung = Request.Form["sID_MaNguoiDung"];
            String iDoiTuongNguoiDung = Request.Form["CauHinh_iDoiTuongNguoiDung"];
            if (Request.Form["DaKiemTra"] == "1")
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT Count(*) FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung ";
                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaNguoiDung);
                Connection.UpdateDatabase(cmd);
                int d = Convert.ToInt16(Connection.GetValue(cmd, 0));
                cmd.Dispose();

                cmd = new SqlCommand();
                if (d == 0)
                {
                    cmd.CommandText = "INSERT INTO QT_NguoiDung(iID_MaNhomNguoiDung, sID_MaNguoiDung, sHoTen,iDoiTuongNguoiDung) VALUES(@iID_MaNhomNguoiDung, @sID_MaNguoiDung, @sHoTen,@iDoiTuongNguoiDung)";
                }
                else
                {
                    cmd.CommandText = "UPDATE QT_NguoiDung SET iID_MaNhomNguoiDung = @iID_MaNhomNguoiDung, sHoTen = @sHoTen, iDoiTuongNguoiDung = @iDoiTuongNguoiDung,iTrangThai=1 WHERE sID_MaNguoiDung = @sID_MaNguoiDung";
                }
                cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaNguoiDung);
                cmd.Parameters.AddWithValue("@sHoTen", MaNguoiDung);
                cmd.Parameters.AddWithValue("@iDoiTuongNguoiDung", iDoiTuongNguoiDung);
                cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            return RedirectToAction("Edit_NguoiDung", new { MaNhomNguoiDung = MaNhomNguoiDung });
        }
    }
}
