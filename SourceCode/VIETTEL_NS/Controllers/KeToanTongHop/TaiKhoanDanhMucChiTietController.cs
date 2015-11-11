using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using VIETTEL.Models;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;

namespace VIETTEL.Controllers.KeToanTongHop
{
    public class TaiKhoanDanhMucChiTietController : Controller
    {
        //
        // GET: /TaiKhoanDanhMucChiTiet/
        // GET: /Luong_DanhMucPhuCap_MucLucNganSach/
        public string sViewPath = "~/Views/KeToanTongHop/DanhMuc/TaiKhoanDanhMucChiTiet/";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
               // return View(sViewPath + "TaiKhoanDanhMucChiTiet.aspx");
                return View(sViewPath + "TaiKhoanDanhMucChiTiet_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult List()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                 return View(sViewPath + "TaiKhoanDanhMucChiTiet.aspx");
               
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DetailSubmit(String iID_MaTaiKhoan)
        {
            TaiKhoanDanhMucChiTietModels.DetailSubmit(User.Identity.Name, Request.UserHostAddress, iID_MaTaiKhoan,
                                                      Request.Form);
            return RedirectToAction("List", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoan = iID_MaTaiKhoan });
        }

        [Authorize]
        public JsonResult KiemTraTrungKyHieu(String iID_MaTaiKhoanDanhMucChiTiet,String sKyHieu)
        {
            Boolean DuLieuMoi=true;
            if (String.IsNullOrEmpty(iID_MaTaiKhoanDanhMucChiTiet) == false) DuLieuMoi = false;
            Boolean Trung = HamChung.Check_Trung("KT_TaiKhoanDanhMucChiTiet", "iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet, "sKyHieu", sKyHieu, DuLieuMoi);

            //String SQL = String.Format("SELECT COUNT(*) FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sKyHieu='{0}'",sKyHieu);
            //int VR = Convert.ToInt32(Connection.GetValue(SQL, 0));
            //if (VR > 0)
                return Json(Trung, JsonRequestBehavior.AllowGet);
            //else
            //    return Json(HamChung.Check_Trung("KT_TaiKhoanDanhMucChiTiet", "iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet, "sKyHieu", sKyHieu, DuLieuMoi), JsonRequestBehavior.AllowGet);           
        }
        [Authorize]
        public ActionResult Create(String iID_MaTaiKhoanDanhMucChiTiet_Cha, String iID_MaTaiKhoan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoanDanhMucChiTiet", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection data = new NameValueCollection();

            ViewData["DuLieuMoi"] = "1";
            if (String.IsNullOrEmpty(iID_MaTaiKhoanDanhMucChiTiet_Cha)) iID_MaTaiKhoanDanhMucChiTiet_Cha = "0";
            else data = TaiKhoanDanhMucChiTietModels.LayThongTinChiTietTaiKhoanDanhMuc(iID_MaTaiKhoanDanhMucChiTiet_Cha);
            data["iID_MaTaiKhoanDanhMucChiTiet_Cha"] = iID_MaTaiKhoanDanhMucChiTiet_Cha;
            data["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["data"] = data;
            return View(sViewPath + "TaiKhoanDanhMucChiTiet_Edit.aspx");
        }

        [Authorize]
        public ActionResult Edit(String iID_MaTaiKhoanDanhMucChiTiet, String iID_MaTaiKhoan)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoanDanhMucChiTiet", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            NameValueCollection data = new NameValueCollection();
            if (String.IsNullOrEmpty(iID_MaTaiKhoanDanhMucChiTiet))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            else
            {
                data = TaiKhoanDanhMucChiTietModels.LayThongTinChiTietTaiKhoanDanhMuc(iID_MaTaiKhoanDanhMucChiTiet);
            }
            ViewData["iID_MaTaiKhoanDanhMucChiTiet"] = iID_MaTaiKhoanDanhMucChiTiet;
            ViewData["iID_MaTaiKhoan"] = iID_MaTaiKhoan;
            ViewData["data"] = data;
            return View(sViewPath + "TaiKhoanDanhMucChiTiet_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoanDanhMucChiTiet", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            String iID_MaTaiKhoan = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoan"]);
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaTaiKhoanDanhMucChiTiet = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoanDanhMucChiTiet"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String sKyHieu = Convert.ToString(Request.Form[ParentID + "_sKyHieu"]);
            String iID_MaNgoaiTe = Convert.ToString(Request.Form[ParentID + "_iID_MaNgoaiTe"]);
            String iID_MaTaiKhoanDanhMucChiTiet_Cha = Convert.ToString(Request.Form[ParentID + "_iID_MaTaiKhoanDanhMucChiTiet_Cha"]);


            if (sKyHieu == string.Empty || sKyHieu == "")
            {
                arrLoi.Add("err_sKyHieu", "Bạn chưa nhập ký hiệu!");
            }
           
            Boolean DuLieuMoi = true;
            if (Request.Form[ParentID + "_DuLieuMoi"] == "0")
            {
                DuLieuMoi = false;
            }
            Boolean Trung = HamChung.Check_Trung("KT_TaiKhoanDanhMucChiTiet", "iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet, "sKyHieu", sKyHieu, DuLieuMoi);
            if (Trung)
            {

                arrLoi.Add("err_sKyHieu", "Ký hiệu đã tồn tại!");
             
            }
            if (sTen == string.Empty || sTen == "")
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập tên tài khoản chi tiết!");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                NameValueCollection data = new NameValueCollection();
                data.Add(Request.Form);
                ViewData["DuLieuMoi"] = Request.Form[ParentID + "_DuLieuMoi"];
                ViewData["iID_MaTaiKhoanDanhMucChiTiet"] = iID_MaTaiKhoanDanhMucChiTiet;
                ViewData["data"] = data;
                return View(sViewPath + "TaiKhoanDanhMucChiTiet_Index.aspx");
            }
            else
            {
                Bang bang = new Bang("KT_TaiKhoanDanhMucChiTiet");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                String NgoaiTeID = TaiKhoanDanhMucChiTietModels.getTen_NgoaiTe(Convert.ToInt32(iID_MaNgoaiTe));
                bang.CmdParams.Parameters.AddWithValue("@sTenNgoaiTe", NgoaiTeID);
                String ID = Convert.ToString(bang.Save());
                if (!String.IsNullOrEmpty(iID_MaTaiKhoan) && iID_MaTaiKhoan!=Guid.Empty.ToString())
                {
                    if (DuLieuMoi == true)
                    {
                        String MaTaiKhoanDanhMucChiTiet_ID =
                            TaiKhoanDanhMucChiTietModels.getiID_MaTaiKhoanDanhMucChiTiet(sKyHieu);
                        Bang bang_GT = new Bang("KT_TaiKhoanGiaiThich");
                        bang_GT.MaNguoiDungSua = User.Identity.Name;
                        bang_GT.IPSua = Request.UserHostAddress;
                        bang_GT.TruyenGiaTri(ParentID, Request.Form);
                        bang_GT.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                        bang_GT.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", MaTaiKhoanDanhMucChiTiet_ID);
                        bang_GT.Save();
                    }
                    else
                    {
                        String MySQL =
                            @"UPDATE KT_TaiKhoanGiaiThich SET sKyHieu=@sKyHieu, sTen=@sTen where iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet";
                        SqlCommand cmd = new SqlCommand();
                        cmd.Parameters.Clear();
                        cmd.CommandText = MySQL;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
                        cmd.Parameters.AddWithValue("@sTen", sTen);
                        cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                        Connection.UpdateDatabase(cmd);
                        cmd.Dispose();

                    }
                }
                
                return View(sViewPath + "TaiKhoanDanhMucChiTiet_Index.aspx");
            }
        }
        [Authorize]
        public ActionResult Delete(String iID_MaTaiKhoanDanhMucChiTiet)
        {
            //kiểm tra quyền có được phép xóa
            //if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoanDanhMucChiTiet", "Delete") == false)
            //{
            //    return RedirectToAction("Index", "PermitionMessage");
            //}

            Boolean bDelete = false;
            if (TaiKhoanDanhMucChiTietModels.CheckTaiKhoan(iID_MaTaiKhoanDanhMucChiTiet)==true)
            {
                //return;
            }
            else
            {
                bDelete = TaiKhoanDanhMucChiTietModels.DeleteTaiKhoan(iID_MaTaiKhoanDanhMucChiTiet);
            }
            if (bDelete == true)
            {
                return RedirectToAction("Index", new { iID_MaTaiKhoanDanhMucChiTiet = iID_MaTaiKhoanDanhMucChiTiet });
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

    }
}
