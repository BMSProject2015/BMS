using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;
using System.IO;
namespace VIETTEL.Controllers.DungChung
{


    public class DonViController : Controller
    {
        //
        // GET: /DonVi/
        public string sViewPath = "~/Views/DungChung/DonVi/";
        [Authorize]
        public ActionResult Index()
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_DonVi", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            return View(sViewPath + "DonVi_Index.aspx");
        }

        [Authorize]
        public ActionResult Create(String iID_MaDonViCha)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_DonVi", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection data = new NameValueCollection();

            ViewData["DuLieuMoi"] = "1";
            if (String.IsNullOrEmpty(iID_MaDonViCha)) iID_MaDonViCha = "0";
            else data = DonViModels.LayThongTinDonVi(iID_MaDonViCha);
            ViewData["iID_MaDonViCha"] = iID_MaDonViCha;
            ViewData["data"] = data;
            return View(sViewPath + "DonVi_Edit.aspx");
        }

        [Authorize]
        public ActionResult Edit(String iID_MaDonVi)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_DonVi", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            NameValueCollection data = new NameValueCollection();
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            else
            {
                data = DonViModels.LayThongTinDonVi(iID_MaDonVi);
            }
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["data"] = data;
            return View(sViewPath + "DonVi_Edit.aspx");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID, String MaDonVi)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_DonVi", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            NameValueCollection arrLoi = new NameValueCollection();
            String MaKhoiDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaKhoiDonVi"]);
            String MaLoaiDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaLoaiDonVi"]);
            String MaNhomDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaNhomDonVi"]);
            String sMaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
            String sMoTa = Convert.ToString(Request.Form[ParentID + "_sTenVietTat"]);
            String sMaDonViCha = Convert.ToString(Request.Form[ParentID + "_iID_MaDonViCha"]);

            //if (MaKhoiDonVi == Convert.ToString(Guid.Empty) || MaKhoiDonVi == "")
            //{
            //    arrLoi.Add("err_iID_MaKhoiDonVi", "Bạn chưa khối đơn vị!");
            //}
            //if (MaLoaiDonVi == Convert.ToString(Guid.Empty) || MaLoaiDonVi == "")
            //{
            //    arrLoi.Add("err_iID_MaLoaiDonVi", "Bạn chưa loại đơn vị!");
            //}
            //if (MaNhomDonVi == Convert.ToString(Guid.Empty) || MaNhomDonVi == "")
            //{
            //    arrLoi.Add("err_iID_MaNhomDonVi", "Bạn chưa nhập nhóm đơn vị!");
            //}
            if (sMaDonVi == string.Empty || sMaDonVi == "")
            {
                arrLoi.Add("err_iMaDonVi", "Bạn chưa nhập mã đơn vị!");
            }
            if (String.IsNullOrEmpty(sMaDonVi) == false && sMaDonVi.Length > 5)
            {
                arrLoi.Add("err_iMaDonVi", "Mã đơn vị quá ký tự cho phép!");
            }
            if (Request.Form[ParentID + "_DuLieuMoi"] == "1")
            {
                if (CheckMaDonVi(sMaDonVi) == true)
                {
                    arrLoi.Add("err_iMaDonVi", "Mã đơn vị đã tồn tại!");
                }
            }
            if (sTen == string.Empty || sTen == "")
            {
                arrLoi.Add("err_sTen", "Bạn chưa nhập tên đơn vị!");
            }
            if (arrLoi.Count > 0)
            {
                for (int i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                NameValueCollection data = new NameValueCollection();
                data.Add(Request.Form);
                ViewData["iID_MaDonVi"] = MaDonVi;
                ViewData["data"] = data;
                return View(sViewPath + "DonVi_Edit.aspx");
            }
            else
            {
                Bang bang = new Bang("NS_DonVi");
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);
                //bang.GiaTriKhoa = sMaDonVi;           
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
                bang.Save();
                //update cấp bậc
                int iCap = 1;
                if (sMaDonViCha != "")
                {
                    iCap = GetCapDonVi(sMaDonViCha);
                }
                SqlCommand cmd = new SqlCommand("UPDATE NS_DonVi SET iCap=@iCap " +
                                     " WHERE iID_MaDonVi=@sMaDonVi AND iNamLamViec_DonVi=@iNamLamViec_DonVi");
                cmd.Parameters.AddWithValue("@iCap", iCap);
                cmd.Parameters.AddWithValue("@sMaDonVi", sMaDonVi);
                cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();

                return View(sViewPath + "DonVi_Index.aspx");
            }
        }
        [Authorize]
        public ActionResult ThemDanhMucDonVi()
        {
            DataTable dtDV = DonViModels.DanhSach_DonVi(User.Identity.Name);
            if (dtDV.Rows.Count > 0)
            {
                ViewData["ThongBao"] = "Đã có danh mục";
            }
            else
            {
                if (CommonFunction.IsNumeric(NguoiDungCauHinhModels.iNamLamViec))
                {
                    int iNamLamViec = Convert.ToInt16(NguoiDungCauHinhModels.iNamLamViec);
                    String SQL = String.Format(@"INSERT INTO NS_DonVi(iID_MaDonVi,sMaSo,sTen,sTenTomTat,sSoTaiKhoan,sDiaChi,sKhoBac,sMoTa,
                    iCap,iHuongLuong,iID_MaDonViCha,iNamLamViec_DonVi,iID_MaPhongBan,iID_MaKhoiDonVi,iID_MaLoaiDonVi,
                    sTenLoaiDonVi,iID_MaNhomDonVi,iSTT,iTrangThai,bPublic,
                    iID_MaNhomNguoiDung_Public,iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao,
                    sID_MaNguoiDungTao,iSoLanSua,sIPSua,sID_MaNguoiDungSua,iCapNS
                    )
                    SELECT  iID_MaDonVi,sMaSo,sTen,sTenTomTat,sSoTaiKhoan,sDiaChi,sKhoBac,
                    sMoTa,iCap,iHuongLuong,iID_MaDonViCha,{0},iID_MaPhongBan,
                    iID_MaKhoiDonVi,iID_MaLoaiDonVi,sTenLoaiDonVi,iID_MaNhomDonVi,
                    iSTT,iTrangThai,bPublic,iID_MaNhomNguoiDung_Public,
                    iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao,sID_MaNguoiDungTao,iSoLanSua,sIPSua,sID_MaNguoiDungSua,iCapNS
                    FROM    NS_DonVi
                    WHERE   iNamLamViec_DonVi={1}", iNamLamViec, iNamLamViec - 1);
                    SqlCommand cmd = new SqlCommand(SQL);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                    //Thêm phân quyền phòng ban đơn vị
                    ThemPhongBanDonVi(iNamLamViec);
                    //Thêm phân quyền người dùng đơn vị
                    ThemNguoiDungDonVi(iNamLamViec);
                }
            }
            return View(sViewPath + "DonVi_Index.aspx");
        }
        //Thêm phân quyền phòng ban đơn vị
        public static void ThemPhongBanDonVi(int iNamLamViec)
        {
            String SQL = String.Format(@"INSERT INTO NS_PhongBan_DonVi (iID_MaPhongBan,iID_MaDonVi,iNamLamViec,sID_MaNguoiDungTao,sID_MaNguoiDungSua,sIPSua)
                                         SELECT iID_MaPhongBan,iID_MaDonVi,{0},sID_MaNguoiDungTao,sID_MaNguoiDungSua,sIPSua
                                        FROM    NS_PhongBan_DonVi
                                        WHERE   iNamLamViec={1}", iNamLamViec,iNamLamViec-1);
            SqlCommand cmd = new SqlCommand(SQL);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        //Thêm phân quyền người dùng đơn vị
        public static void ThemNguoiDungDonVi(int iNamLamViec)
        {
            String SQL = String.Format(@"INSERT INTO NS_NguoiDung_DonVi (sMaNguoiDung,iID_MaDonVi,iNamLamViec,sID_MaNguoiDungTao,sID_MaNguoiDungSua,sIPSua)
                                        SELECT sMaNguoiDung,iID_MaDonVi,{0},sID_MaNguoiDungTao,sID_MaNguoiDungSua,sIPSua
                                        FROM    NS_NguoiDung_DonVi
                                        WHERE   iNamLamViec={1}", iNamLamViec, iNamLamViec - 1);
            SqlCommand cmd = new SqlCommand(SQL);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }


        public Boolean CheckMaDonVi(String MaDonVi)
        {
            Boolean vR = false;
            DataTable dt = DanhMucModels.GetRow_DonVi(MaDonVi);
            if (dt.Rows.Count > 0)
            {
                vR = true;
            }
            return vR;
        }

        public int GetCapDonVi(String MaDonVi)
        {
            int iCap = 1;
            try
            {
                DataTable dt = DanhMucModels.Get_OneRow_DonVi(MaDonVi);
                if (dt.Rows.Count > 0)
                {
                    iCap = Convert.ToInt32(dt.Rows[0]["iCap"]);
                }
            }
            catch (Exception)
            {
                iCap = 0;
            }
            return iCap + 1;
        }

        [Authorize]
        public ActionResult Delete(String MaDonVi)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "NS_DonVi", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            SqlCommand cmd = new SqlCommand("DELETE NS_DonVi WHERE iID_MaDonVi=@iID_MaDonVi AND iNamLamViec_DonVi=@iNamLamViec_DonVi");
            cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            cmd.Parameters.AddWithValue("@iNamLamViec_DonVi", NguoiDungCauHinhModels.iNamLamViec);
            Connection.UpdateDatabase(cmd);

            return View(sViewPath + "DonVi_Index.aspx");
        }



    }
}
