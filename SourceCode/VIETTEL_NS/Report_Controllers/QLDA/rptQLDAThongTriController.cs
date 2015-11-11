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
using DomainModel.Abstract;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;

namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDAThongTriController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDAThongTri.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_ThongTri_CP.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public ActionResult EditSubmit(String ParentID)
        {
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String iID_MaDotCapPhat = Convert.ToString(Request.Form[ParentID + "_iID_MaDotCapPhat"]);
            String iLoai = Convert.ToString(Request.Form[ParentID + "_iLoai"]);
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["iLoai"] = iLoai;
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_ThongTri_CP.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        private void LoadData(FlexCelReport fr, String sLNS, String iID_MaDotCapPhat, String iLoai)
        {
            DataTable data = dtQLDA_ThongTri(sLNS, iID_MaDotCapPhat, iLoai);
            data.TableName = "Chitiet";
            fr.AddTable("Chitiet", data);
            data.Dispose();
        }
        public ActionResult ViewPDF(String sLNS, String iID_MaDotCapPhat, String iLoai)
        {
            String DuongDan = "";
            DuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), sLNS, iID_MaDotCapPhat, iLoai);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "BaoCao");
                    pdf.EndExport();
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/pdf");
                }
            }
            return null;
        }
        public clsExcelResult ExportToExcel(String sLNS, String iID_MaDotCapPhat, String iLoai)
        {
            String DuongDan = "";
            DuongDan = sFilePath;

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), sLNS, iID_MaDotCapPhat, iLoai);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongTri.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        public ExcelFile CreateReport(String path, String sLNS, String iID_MaDotCapPhat, String iLoai)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            DataTable dt = dtQLDA_ThongTri(sLNS, iID_MaDotCapPhat, iLoai);
            String NoiDung="",Loai="",dNgayLap="",sNgayLap="",sTenDonVi="",TenLNS="",sL="",sK="",Tien="",Nam="";
            if(dt.Rows.Count>0)
            {
                NoiDung = dt.Rows[0]["sNoiDungCapPhat"].ToString();
                dNgayLap = dt.Rows[0]["dNgayLap"].ToString();
                sTenDonVi = dt.Rows[0]["sTenDonVi"].ToString();
            }
            //Lay tien ra chu
            long TongTien = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["rSoTien"].ToString() != "")
                {
                    TongTien += long.Parse(dt.Rows[i]["rSoTien"].ToString());
                }
            }
            Tien = CommonFunction.TienRaChu(TongTien).ToString();
            dt.Dispose();
            if (!String.IsNullOrEmpty(dNgayLap))
            {
                sNgayLap = "tháng  "+ dNgayLap.Substring(3,2)+"  năm  "+dNgayLap.Substring(6,4);
                Nam = dNgayLap.Substring(6, 4);
            }
            if (iLoai == "1") Loai = "Cấp thanh toán";
            else if (iLoai == "2") Loai = "Thu tạm ứng";
            else if (iLoai == "3") Loai = "Cấp tạm ứng";
            else if (iLoai == "4") Loai = "Thu nộp ngân sách";
            else Loai = "Thu giảm cấp";

            //Lay ten LNS
            DataTable LNS = dtLNS(iID_MaDotCapPhat);
            for (int i = 0; i < LNS.Rows.Count; i++)
            {
                if (sLNS == LNS.Rows[i]["sLNS"].ToString())
                {
                    TenLNS = LNS.Rows[i]["sMoTa"].ToString();
                    sL = LNS.Rows[i]["sL"].ToString();
                    sK = LNS.Rows[i]["sK"].ToString();
                    break;
                }
            }
            LNS.Dispose();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDAThongTri");
            LoadData(fr, sLNS, iID_MaDotCapPhat, iLoai);
            fr.SetValue("NgayThangNam", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("NoiDung", NoiDung);
            fr.SetValue("Loai", Loai);
            fr.SetValue("DonVi", sTenDonVi);
            fr.SetValue("dNgayLap", sNgayLap);
            fr.SetValue("LNS", TenLNS);
            fr.SetValue("sL", sL);
            fr.SetValue("sK", sK);
            fr.SetValue("Tien", Tien);
            fr.SetValue("Nam", Nam);
            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1).ToUpper());
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.Run(Result);
            return Result;
        }
        public static DataTable dtQLDA_ThongTri(String sLNS, String iID_MaDotCapPhat, String iLoai)
        {
            DataTable dt= new DataTable();
            String DK = "",DKHAVING="",DKWHERE="";
            if (iLoai == "1")
            {
                DK = "rDeNghiPheDuyetThanhToan";
                DKHAVING = "HAVING SUM(rDeNghiPheDuyetThanhToan)>0";
                DKWHERE = "AND rDeNghiPheDuyetThanhToan>0";
            }
            else if (iLoai == "2")
            {
                DK = "rDeNghiPheDuyetTamUng";
                DKHAVING = "HAVING SUM(rDeNghiPheDuyetTamUng)<>0";
            }
            else if (iLoai == "3")
            {
                DK = "rDeNghiPheDuyetThuTamUng";
                DKHAVING = "HAVING SUM(rDeNghiPheDuyetThuTamUng)<>0";
            }
            else if (iLoai == "4")
            {
                DK = "rDeNghiPheDuyetThuKhac";
                DKHAVING = "HAVING SUM(rDeNghiPheDuyetThuKhac)<>0";
            }
            else if (iLoai == "5")
            {
                DK = "-rDeNghiPheDuyetThanhToan";
                DKHAVING = "HAVING SUM(rDeNghiPheDuyetThanhToan)<0";
                DKWHERE = "AND rDeNghiPheDuyetThanhToan<0";
            }
            String SQL = String.Format(@" SELECT sLNS,sL,sK,sM
                                         ,sNoiDungCapPhat,dNgayLap,iID_MaDonVi,sTenDonVi,iID_MaDonViThiCong
                                         ,sTenDonViThiCong,SUM(rSoTien) as rSoTien
                                           FROM
                                        (
                                        SELECT sLNS,sL,sK,sM,iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDotCapPhat,
                                        SUM({0}) as rSoTien
                                        FROM QLDA_CapPhat
                                        WHERE iTrangThai=1 
	                                          AND sLNS=@sLNS AND sM IN (9200,9250,9300,9350,9400)
	                                          AND iID_MaDotCapPhat=@iID_MaDotCapPhat
                                              {2}  
                                        GROUP BY sLNS,sL,sK,sM,iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDotCapPhat
                                        {1}) as CP
                                        INNER JOIN (SELECT iID_MaDotCapPhat,sNoiDungCapPhat,CONVERT(varchar,dNgayLap,103) as dNgayLap 
			                                        FROM QLDA_CapPhat_Dot 
			                                        WHERE iTrangThai=1) as CPDot
                                        ON CP.iID_MaDotCapPhat=CPDot.iID_MaDotCapPhat
                                        INNER JOIN 
		                                         (SELECT DISTINCT iID_MaDonVi,sTenDonVi,iID_MaDanhMucDuAn
		                                          FROM QLDA_DanhMucDuAn
		                                          WHERE iTrangThai=1) as DM
                                        ON CP.iID_MaDanhMucDuAn=DM.iID_MaDanhMucDuAn
                                        INNER JOIN ( SELECT  DISTINCT iID_MaDonViThiCong,sTenDonViThiCong,iID_MaHopDong,iID_MaDanhMucDuAn
			                                         FROM QLDA_HopDongChiTiet
			                                         WHERE iTrangThai=1 
				                                           ) as HD
                                        ON CP.iID_MaHopDong=HD.iID_MaHopDong AND CP.iID_MaDanhMucDuAn=HD.iID_MaDanhMucDuAn
                                        GROUP BY sLNS,sL,sK,sM
                                                 ,sNoiDungCapPhat,dNgayLap,iID_MaDonVi,sTenDonVi,iID_MaDonViThiCong
                                                   ,sTenDonViThiCong

", DK, DKHAVING,DKWHERE);
            SqlCommand cmd= new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS",sLNS);
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            dt=Connection.GetDataTable(cmd);
            if (dt.Rows.Count <= 12)
            {
                int count = dt.Rows.Count;
                for (int i = count; i < 12-count; i++)
                {
                    DataRow r= dt.NewRow();
                    dt.Rows.Add(r);
                }
            }
            return dt;
        }

        public static DataTable dtLNS(String iID_MaDotCapPhat)
        {
            DataTable dt;
            String SQL=String.Format(@"SELECT CP.sLNS,sMoTa,CP.sLNS+'-'+sMoTa as TenHT,sL,sK FROM(
                                        SELECT DISTINCT sLNS,sL,sK
                                        FROM  QLDA_CapPhat
                                        WHERE   sM IN(9200,9250,9300,9350,9400)
                                                AND iTrangThai=1
                                                AND iID_MaDotCapPhat=@iID_MaDotCapPhat
                                                ) as CP
                                        INNER JOIN (SELECT sLNS,sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND LEN(sLNS)=7)  as DM
                                        ON CP.sLNS=DM.sLNS");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDotCapPhat", iID_MaDotCapPhat);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }

    }
}
