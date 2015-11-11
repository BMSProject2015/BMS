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

namespace VIETTEL.Controllers.KeToanTongHop
{
    public class KeToanTongHop_GiaiThichTaiKhoanController : Controller
    {
        //
        // GET: /KeToanTongHop_GiaiThichTaiKhoan/
        public string sViewPath = "~/Views/KeToanTongHop/DanhMuc/GiaiThichTaiKhoan/";
        public ActionResult Index(String iID_MaTaiKhoan)
        {
            ViewData["DuLieuMoi"] = "1";           
            return View(sViewPath + "KeToanTongHop_GiaiThichTaiKhoan_Index.aspx");
        }
        [Authorize]
        public ActionResult Edit(String iID_MaTaiKhoan, String iID_MaGiaiThich)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoanGiaiThich", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            ViewData["DuLieuMoi"] = "0";
            if (String.IsNullOrEmpty(iID_MaGiaiThich))
            {
                ViewData["DuLieuMoi"] = "1";
            }
            return View(sViewPath + "KeToanTongHop_GiaiThichTaiKhoan_Index.aspx");
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit(String ParentID)
        {
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoanGiaiThich", "Edit") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            NameValueCollection arrLoi = new NameValueCollection();

            String iDisplay = Request.Form[ParentID + "_iDisplay"];
            String iID_MaTaiKhoan = Request.Form[ParentID + "_iID_MaTaiKhoan"];
            String iID_MaTaiKhoan_Copy = Request.Form[ParentID + "_iID_MaTaiKhoan_Copy"];
           

            String SQL = "DELETE KT_TaiKhoanGiaiThich WHERE iID_MaTaiKhoan=@iID_MaTaiKhoan";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            if (iDisplay == "on")
            {
                SQL =
                    @"INSERT INTO KT_TaiKhoanGiaiThich(iID_MaTaiKhoan,iID_MaTaiKhoanDanhMucChiTiet,sKyHieu,sTen,iSTT,iTrangThai,bPublic,iID_MaNhomNguoiDung_Public
           ,iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao,dNgayTao,sID_MaNguoiDungTao,iSoLanSua,dNgaySua,sIPSua,sID_MaNguoiDungSua) select @iID_MaTaiKhoan,iID_MaTaiKhoanDanhMucChiTiet,sKyHieu,sTen,iSTT,iTrangThai,bPublic,iID_MaNhomNguoiDung_Public
           ,iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao,getdate(),sID_MaNguoiDungTao,iSoLanSua,getdate(),sIPSua,sID_MaNguoiDungSua from KT_TaiKhoanGiaiThich where iTrangThai=1 AND iID_MaTaiKhoan=@iID_MaTaiKhoan_Copy";
        
                cmd.Parameters.Clear();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan_Copy", iID_MaTaiKhoan_Copy);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
            }
            else
            {

                String sMaChiTiet = Request.Form[ParentID + "_iID_MaTaiKhoanDanhMucChiTiet"];
                String[] arrMaChiTiet = sMaChiTiet.Split(',');
                if (arrMaChiTiet.Count() == 0)
                {
                    return RedirectToAction("Index", new {iID_MaTaiKhoan = iID_MaTaiKhoan});
                }
                String sKyHieu = "", sTen = "", iID_MaTaiKhoanDanhMucChiTiet = "";
                Bang bang = new Bang("KT_TaiKhoanGiaiThich");
                bang.DuLieuMoi = true;
                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);

                for (int i = 0; i < arrMaChiTiet.Length; i++)
                {
                    sKyHieu = Request.Form["sKyHieu_" + arrMaChiTiet[i]];
                    sTen = Request.Form["sTen_" + arrMaChiTiet[i]];
                    iID_MaTaiKhoanDanhMucChiTiet = Request.Form["iID_MaTaiKhoanDanhMucChiTiet_" + arrMaChiTiet[i]];
                    if (i == 0)
                    {
                        bang.CmdParams.Parameters.AddWithValue("@sTen", sTen);
                        bang.CmdParams.Parameters.AddWithValue("@sKyHieu", sKyHieu);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet",
                                                               iID_MaTaiKhoanDanhMucChiTiet);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaTaiKhoanGiaiThich", Guid.NewGuid());
                    }
                    else
                    {
                        bang.CmdParams.Parameters["@sTen"].Value = sTen;
                        bang.CmdParams.Parameters["@sKyHieu"].Value = sKyHieu;
                        bang.CmdParams.Parameters["@iID_MaTaiKhoanDanhMucChiTiet"].Value = iID_MaTaiKhoanDanhMucChiTiet;
                        bang.CmdParams.Parameters["@iID_MaTaiKhoanGiaiThich"].Value = Guid.NewGuid();
                    }

                    bang.Save();
                }
            }
            return RedirectToAction("Index", new {iID_MaTaiKhoan = iID_MaTaiKhoan});

        }

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="iID_MaGiaiThich"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(String iID_MaGiaiThich, String iID_MaTaiKhoan)
        {
            //kiểm tra quyền có được phép xóa
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, "KT_TaiKhoanGiaiThich", "Delete") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }

            Bang bang = new Bang("KT_TaiKhoanGiaiThich");
            bang.TruongKhoa = "iID_MaTaiKhoanGiaiThich";
            bang.GiaTriKhoa = iID_MaGiaiThich;
            bang.Delete();
            return RedirectToAction("Index", new { iID_MaTaiKhoan = iID_MaTaiKhoan });
        }
    }
}
