using System;

using System.Data.SqlClient;

using System.Web.Mvc;

using DomainModel;


namespace VIETTEL.Controllers
{
    public class TrangChuController : Controller
    {
        public string sViewPath = "~/Views/Home/";
        [Authorize]
        public ActionResult Index()
        {
            return View(sViewPath + "TrangChu.aspx");
        }
        [Authorize]
        public ActionResult NganSach()
        {
            return View(sViewPath + "NganSach.aspx");
        }
        [Authorize]
        public JsonResult UpdateCauHinhNamLamViec(String MaND, String iThangLamViec, String iNamLamViec, String MaNamNganSach, String MaNguonNganSach)
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
            if (String.IsNullOrEmpty(DK) == false)
            {
                DK = DK + ",iID_MaNamNganSach=@iID_MaNamNganSach ";
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", MaNamNganSach);
            }
            else
            {
                DK = " iID_MaNamNganSach=@iID_MaNamNganSach ";
                cmd.Parameters.AddWithValue("@iID_MaNamNganSach", MaNamNganSach);
            }
            if (String.IsNullOrEmpty(DK) == false)
            {
                DK = DK + ",iID_MaNguonNganSach=@iID_MaNguonNganSach ";
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", MaNguonNganSach);
            }
            else
            {
                DK = " iID_MaNguonNganSach=@iID_MaNguonNganSach ";
                cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", MaNguonNganSach);
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
