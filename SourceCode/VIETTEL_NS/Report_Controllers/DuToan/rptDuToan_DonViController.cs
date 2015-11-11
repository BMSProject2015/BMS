using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToan_DonViController : Controller
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/rptDuToan_DonVi.xls";


        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_DonVi.aspx";
                ViewData["PageLoad"] = "0";
               // DanhSachBaoCao("trolyphongban", "110", "");
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String MaTo = Request.Form["MaTo"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            ViewData["PageLoad"] = "1";
            ViewData["MaTo"] = MaTo;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["path"] = "~/Report_Views/DuToan/rptDuToan_DonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        public JsonResult Ds_DonVi(String ParentID, String iID_MaDonVi,String BaoCao, String MaTo)
        {
            String MaND = User.Identity.Name;
            if (String.IsNullOrEmpty(iID_MaDonVi))
                iID_MaDonVi = "1";
            DataTable dt = DanhSachBaoCao(MaND, iID_MaDonVi, MaTo);


            String ViewNam = "~/Views/DungChung/DonVi/BaoCao_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, BaoCao, dt, ParentID);
            dt.Dispose();
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }
        public static DataTable DanhSachBaoCao(String MaND, String iID_MaDonVi, String MaTo)
        {
            DataTable dtCoSoLieu = new DataTable();
            DataTable vR = DanhMucModels.DT_DanhMuc_All("DanhSachBaoCaoDonVi");
            dtCoSoLieu = vR.Copy();
            String sConTroller = "", sLNS = "";
            for (int i = vR.Rows.Count-1; i >=0; i--)
            {

                bool bCoSoLieu = true;
                DataRow dr = vR.Rows[i];
                sConTroller = Convert.ToString(vR.Rows[i]["sTenKhoa"]);
                sLNS = Convert.ToString(vR.Rows[i]["sGhiChu"]);
                String DK = "";
                SqlCommand cmd = new SqlCommand();
                if (!String.IsNullOrEmpty(sLNS))
                {
                    String[] arrLNS = sLNS.Split(',');
                    for (int j = 0; j < arrLNS.Length; j++)
                    {
                        DK += " sLNS LIKE @sLNS" + j;
                        if (j < arrLNS.Length - 1)
                            DK += " OR ";
                        cmd.Parameters.AddWithValue("@sLNS"+j,arrLNS[j]+"%");
                    }
                    String SQL="";
                    if (sConTroller == "rptDuToan_1040100_TungNganh")
                    {
                        dtCoSoLieu.Rows.RemoveAt(i);
                        String DSNganh = "";
                        
                        String iID_MaNganhMLNS = Convert.ToString(CommonFunction.LayTruong("NS_MucLucNganSach_Nganh", "iID_MaNganh", iID_MaDonVi, "iID_MaNganhMLNS"));
                        DSNganh = " AND sNG IN (" + iID_MaNganhMLNS + ")";
                        if (String.IsNullOrEmpty(iID_MaNganhMLNS)) DSNganh = " AND sNG IN (123)";
                        String SQLNganh = "SELECT distinct sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
                        SQLNganh += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
                        SQLNganh += " FROM DT_ChungTuChiTiet_PhanCap WHERE sLNS='1020100' AND MaLoai<>'1' {1} AND iTrangThai=1   {0} ";

                        SQLNganh += " UNION SELECT distinct sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa";
                        SQLNganh += ",sM +'.'+ sTM +'.'+ sTTM +'.'+ sNG AS NG";
                        SQLNganh += " FROM DT_ChungTuChiTiet WHERE sLNS=1040100 AND iKyThuat=1 AND MaLoai=1 {1} AND iTrangThai=1   {0}   ORDER BY sM,sTM,sTTM,sNG";
                        SQLNganh = String.Format(SQLNganh, DSNganh, ReportModels.DieuKien_NganSach_KhongDV(MaND));
                        SqlCommand cmdNG = new SqlCommand(SQLNganh);
                        //cmdNG.Parameters.AddWithValue("@NamLamViec", NamLamViec);
                        DataTable dtNG = Connection.GetDataTable(cmdNG);
                        cmdNG.Dispose();

                        //co so lieu
                        if (dtNG.Rows.Count > 0)
                        {
                            String MaNganh = "";
                             MaNganh = Convert.ToString(CommonFunction.LayTruong("NS_MucLucNganSach_Nganh", "iID_MaNganh", iID_MaDonVi, "iID"));
                            DataTable dtTo = rptDuToan_1040100_TungNganhController.DanhSachToIn(MaND, MaNganh, "1", "0", "0");
                            if (dtTo.Rows.Count >= 1)
                            {
                                for (int j = 1; j <= dtTo.Rows.Count; j++)
                                {
                                    DataRow dr1 = dtCoSoLieu.NewRow();
                                    dr1["sTenKhoa"] = "/rptDuToan_1040100_TungNganh/viewpdf?ToSo="+j+"&MaND="+MaND+"&Nganh="+MaNganh+"&sLNS=0&iID_MaPhongBan=0";
                                    dr1["sGhiChu"] = "1020100,1020000";
                                    dr1["sTen"] = "Bảo đảm chi tiết tờ "+j;
                                    dtCoSoLieu.Rows.InsertAt(dr1,i + j - 1);
                                }
                              
                            }
                            dtTo.Dispose();

                        }
                        SQL = String.Format(@"SELECT SUM(count)
FROM(
SELECT COUNT(*) as count
FROM DT_ChungTuChiTiet
WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND ({0})
 AND (rTuChi<>0 OR rHienVat<>0 OR rDuPhong<>0 OR rPhanCap<>0 OR rHangNhap<>0 OR rHangMua<>0)
UNION
SELECT COUNT(*) as count
FROM DT_ChungTuChiTiet_PhanCap
WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND ({0})
 AND (rTuChi<>0 OR rHienVat<>0 OR rDuPhong<>0 OR rPhanCap<>0 OR rHangNhap<>0 OR rHangMua<>0)) as a
", DK);
                        cmd.CommandText = SQL;
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                        int a = Convert.ToInt16(Connection.GetValue(cmd, 0));
                        if (a <= 0)
                            bCoSoLieu = false;
                        if (bCoSoLieu == false)
                        {
                            dtCoSoLieu.Rows.RemoveAt(i);
                        }
                        else
                        {
                            //Dem so tờ có trong báo cáo
                        }
                    }
                    else
                    {
                        SQL = String.Format(@"

SELECT COUNT(*) as count
FROM DT_ChungTuChiTiet
WHERE iTrangThai=1 AND iID_MaDonVi=@iID_MaDonVi AND ({0})
 AND (rTuChi<>0 OR rHienVat<>0 OR rDuPhong<>0 OR rPhanCap<>0 OR rHangNhap<>0 OR rHangMua<>0)


", DK);

                        cmd.CommandText = SQL;
                        cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                        int a = Convert.ToInt16(Connection.GetValue(cmd, 0));
                        if (a <= 0)
                            bCoSoLieu = false;
                        if (bCoSoLieu == false)
                        {
                            dtCoSoLieu.Rows.RemoveAt(i);
                        }
                    }
                        
                }
                cmd.Dispose();
                //Check co so lieu
            }
            dtCoSoLieu.Dispose();
            vR.Dispose();
            return dtCoSoLieu;
        }

    }
}

