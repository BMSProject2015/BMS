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


namespace VIETTEL.Controllers.KeToanChiTiet.KhoBac
{
    public class KeToanChiTietKhoBacController : Controller
    {
        //
        // GET: /KeToanChiTietKhoBac/
        public string sViewPath = "~/Views/KeToanChiTiet/KhoBac/";
        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "KeToanChiTiet_KhoBac_ChungTu_Detail.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            
        }
        [Authorize]
        public ActionResult ChungTu_Frame()
        {
            return View(sViewPath + "KeToanChiTiet_KhoBac_ChungTu_Frame.aspx");
        }

        [Authorize]
        public ActionResult ChungTuChiTiet_Frame(String iLoai)
        {
            ViewData["iLoai"] = iLoai;
            return View(sViewPath + "KeToanChiTiet_KhoBac_ChungTuChiTiet_Frame.aspx");
        }

        public JsonResult UpdateCauHinhNamLamViec(String MaND, String iThangLamViec, String iNamLamViec, String iID_MaNamNganSach, String iID_MaNguonNganSach)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            if (CommonFunction.IsNumeric(iThangLamViec))
            {
                DK = " iThangLamViec=@iThangLamViec";
                cmd.Parameters.AddWithValue("@iThangLamViec", iThangLamViec);
            }
            if (CommonFunction.IsNumeric(iNamLamViec))
            {
                if (String.IsNullOrEmpty(DK) == false)
                {
                    DK = DK + ",iNamLamViec=@iNamLamViec ";
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                }
                else
                {
                    DK = " iNamLamViec=@iNamLamViec ";
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                }
            }
            if (CommonFunction.IsNumeric(iID_MaNamNganSach))
            {
                if (String.IsNullOrEmpty(DK) == false)
                {
                    DK = DK + ",iID_MaNamNganSach=@iID_MaNamNganSach ";
                    cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                }
                else
                {
                    DK = " iID_MaNamNganSach=@iID_MaNamNganSach ";
                    cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                }
            }
            if (CommonFunction.IsNumeric(iID_MaNguonNganSach))
            {
                if (String.IsNullOrEmpty(DK) == false)
                {
                    DK = DK + ",iID_MaNguonNganSach=@iID_MaNguonNganSach ";
                    cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                }
                else
                {
                    DK = " iID_MaNguonNganSach=@iID_MaNguonNganSach ";
                    cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                }
            }
            if (String.IsNullOrEmpty(DK) == false)
            {
                String SQL = String.Format("UPDATE DC_NguoiDungCauHinh SET {0} WHERE sID_MaNguoiDungTao=@sID_MaNguoiDungTao", DK);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
                Connection.UpdateDatabase(cmd);
            }
            cmd.Dispose();
            String strJ = "";

            strJ = String.Format("Dialog_close(location_reload();)");


            return Json("a", JsonRequestBehavior.AllowGet);
        }
    }
}
