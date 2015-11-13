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

namespace VIETTEL.Report_Controllers.QuyetToan.QuyetToanQuanSo
{
    public class rptQTQS_TongHopController:Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuanSo/rptQTQS_TongHop.xls";

        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_TongHop.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Sumit form
        /// </summary>
        /// <param name="ParentID">ParentID=QuyetToan</param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            string iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            string iThang=Request.Form[ParentID+"_iThang"];
            ViewData["PageLoad"] = "1";
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["iThang"] = iThang;
            ViewData["path"] = "~/Report_Views/QuyetToan/QuyetToanQuanSo/rptQTQS_TongHop.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Hiển thị báo cáo dạng pdf
        /// </summary>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public ActionResult ViewPDF(string MaND, string iID_MaPhongBan,string iThang)
        {
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaPhongBan,iThang);
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
        }

        private ExcelFile CreateReport(string path, string MaND, string iID_MaPhongBan,string iThang)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            string iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            if (string.IsNullOrEmpty(iID_MaPhongBan))
            {
                iID_MaPhongBan = "-1";
            }
            string sTenPhuLuc = "PL03b";
            if (iID_MaPhongBan == "-1")
                sTenPhuLuc = "PL03a";
            String sTenPB = LayTenPhongBan(iID_MaPhongBan);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            LoadData(fr, MaND, iID_MaPhongBan,iThang);

            fr = ReportModels.LayThongTinChuKy(fr, "rptQTQS_TongHop");
            fr.SetValue("iNam", iNamLamViec);
            fr.SetValue("iThang", iThang);
            fr.SetValue("sTenPhuLuc", sTenPhuLuc);
            fr.SetValue("sTenPB", sTenPB);
            fr.Run(Result);
            return Result;
        }

        private string LayTenPhongBan(string iID_MaPhongBan)
        {

            String sTemPB = string.Empty;

            String SQL = String.Format(@"SELECT sTen FROM NS_PhongBan WHERE sKyHieu=@sKyHieu AND iTrangThai=1");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sKyHieu", iID_MaPhongBan);
            sTemPB = Connection.GetValueString(cmd, "");
            return sTemPB;
        }

        private void LoadData(FlexCelReport fr, string MaND, string iID_MaPhongBan,string iThang)
        {
            DataTable data = DT_rptTongHop_QuanSo_QuyetToan(MaND, iID_MaPhongBan,iThang);
            data.TableName = "TongHop";
            fr.AddTable("TongHop", data);

            data.Dispose();
        }

        private DataTable DT_rptTongHop_QuanSo_QuyetToan(string MaND, string iID_MaPhongBan,string iThang)
        {
            String DKDonVi = string.Empty;
            String DKPhongBan = string.Empty;
            String DK = string.Empty;
            String SelectDV = string.Empty;
            String SelectFROM = string.Empty;
            String DKHAVING = string.Empty;
            SqlCommand cmd = new SqlCommand();
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd,"A");
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd,"A");
            if (string.IsNullOrEmpty(iID_MaPhongBan))
            {
                iID_MaPhongBan = "-1";
            }
            if (iID_MaPhongBan.Equals("-1"))
            {
                SelectDV = @"SELECT A.iID_MaPhongBan iID_MaDonVi
                                        ,A.sTenPhongBan sTen,";
                DKHAVING = @"HAVING SUM(CASE WHEN sKyHieu='2' THEN rThieuUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuUy ELSE 0 END) <>0  
                OR  SUM(CASE WHEN sKyHieu='2' THEN rThuongUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongUy ELSE 0 END) <>0 
                OR SUM(CASE WHEN sKyHieu='2' THEN rThuongUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongUy ELSE 0 END) <>0 
                OR SUM(CASE WHEN sKyHieu='2' THEN rDaiUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiUy ELSE 0 END) <>0 
                OR SUM(CASE WHEN sKyHieu='2' THEN rThieuTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuTa ELSE 0 END) <>0 
                OR SUM(CASE WHEN sKyHieu='2' THEN rTrungTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungTa ELSE 0 END) <>0
                OR SUM(CASE WHEN sKyHieu='2' THEN rThuongTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongTa ELSE 0 END) <>0
                OR SUM(CASE WHEN sKyHieu='2' THEN rDaiTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiTa ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rTuong ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTuong ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTuong ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTuong ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rTSQ ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTSQ ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTSQ ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTSQ ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rBinhNhi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhi ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rBinhNhat ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhat ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhat ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhat ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rBinhNhat ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhat ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhat ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhat ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rHaSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rHaSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rHaSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rHaSi ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rTrungSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungSi ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rThuongSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongSi ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rQNCN ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNCN ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNCN ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNCN ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rCNVQP ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQP ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQP ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQP ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rLDHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rLDHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rLDHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rLDHD ELSE 0 END) <>0

";
                
                SelectFROM = string.Format(@"FROM QTQS_ChungTuChiTiet A 
                                        where A.iNamLamViec=@iNamLamViec AND A.iTrangThai=1 AND iThang_Quy<=@iThang {0} {1}
                                        GROUP BY A.iID_MaPhongBan,A.sTenPhongBan {2} ORDER BY A.iID_MaPhongBan", DKDonVi, DKPhongBan,DKHAVING);
              
            }
            else
            {
                SelectDV = @"SELECT A.iID_MaDonVi,B.sTen,";
                DKHAVING = @"HAVING SUM(CASE WHEN sKyHieu='2' THEN rThieuUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuUy ELSE 0 END) <>0  
                OR  SUM(CASE WHEN sKyHieu='2' THEN rThuongUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongUy ELSE 0 END) <>0 
                OR SUM(CASE WHEN sKyHieu='2' THEN rThuongUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongUy ELSE 0 END) <>0 
                OR SUM(CASE WHEN sKyHieu='2' THEN rDaiUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiUy ELSE 0 END) <>0 
                OR SUM(CASE WHEN sKyHieu='2' THEN rThieuTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuTa ELSE 0 END) <>0 
                OR SUM(CASE WHEN sKyHieu='2' THEN rTrungTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungTa ELSE 0 END) <>0
                OR SUM(CASE WHEN sKyHieu='2' THEN rThuongTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongTa ELSE 0 END) <>0
                OR SUM(CASE WHEN sKyHieu='2' THEN rDaiTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiTa ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rTuong ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTuong ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTuong ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTuong ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rTSQ ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTSQ ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTSQ ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTSQ ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rBinhNhi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhi ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rBinhNhat ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhat ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhat ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhat ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rBinhNhat ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhat ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhat ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhat ELSE 0 END) <>0

OR SUM(CASE WHEN sKyHieu='2' THEN rHaSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rHaSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rHaSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rHaSi ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rTrungSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungSi ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rThuongSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongSi ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rQNCN ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNCN ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNCN ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNCN ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rCNVQP ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQP ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQP ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQP ELSE 0 END) <>0
OR SUM(CASE WHEN sKyHieu='2' THEN rLDHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rLDHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rLDHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rLDHD ELSE 0 END) <>0

";
                SelectFROM = string.Format(@"FROM QTQS_ChungTuChiTiet A 
                                JOIN NS_DonVi B ON A.iID_MaDonVi=B.iID_MaDonVi
                                where A.iNamLamViec=@iNamLamViec AND A.iTrangThai=1 AND B.iNamLamViec_DonVi=@iNamLamViec AND B.iTrangThai=1
                                AND A.iID_MaPhongBan=@PhongBan AND iThang_Quy<=@iThang {0} {1}
                                GROUP BY A.iID_MaDonVi,B.sTen {2} ORDER BY A.iID_MaDonVi", DKDonVi, DKPhongBan,DKHAVING);
            }
            String sql = String.Format(@"{0} 
                                        rThieuUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rThieuUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuUy ELSE 0 END)
                ),
                rTrungUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rTrungUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungUy ELSE 0 END)
                )
                ,
                rThuongUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rThuongUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongUy ELSE 0 END)
                )
                ,
                rDaiUy=(
                SUM(CASE WHEN sKyHieu='2' THEN rDaiUy ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiUy ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiUy ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiUy ELSE 0 END)
                )
                ,
                rThieuTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rThieuTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThieuTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThieuTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThieuTa ELSE 0 END)
                )
                ,
                rTrungTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rTrungTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungTa ELSE 0 END)
                )
                ,
                rThuongTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rThuongTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongTa ELSE 0 END)
                )
                ,
                rDaiTa=(
                SUM(CASE WHEN sKyHieu='2' THEN rDaiTa ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rDaiTa ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rDaiTa ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rDaiTa ELSE 0 END)
                )
                ,
                rTuong=(
                SUM(CASE WHEN sKyHieu='2' THEN rTuong ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTuong ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTuong ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTuong ELSE 0 END)
                )
                ,
                rTSQ=(
                SUM(CASE WHEN sKyHieu='2' THEN rTSQ ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTSQ ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTSQ ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTSQ ELSE 0 END)
                )
                ,
                rBinhNhi=(
                SUM(CASE WHEN sKyHieu='2' THEN rBinhNhi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhi ELSE 0 END)
                )
                ,
                rBinhNhat=(
                SUM(CASE WHEN sKyHieu='2' THEN rBinhNhat ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rBinhNhat ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rBinhNhat ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rBinhNhat ELSE 0 END)
                )
                ,
                rHaSi=(
                SUM(CASE WHEN sKyHieu='2' THEN rHaSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rHaSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rHaSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rHaSi ELSE 0 END)
                )
                ,
                rTrungSi=(
                SUM(CASE WHEN sKyHieu='2' THEN rTrungSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rTrungSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rTrungSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rTrungSi ELSE 0 END)
                )
                ,
                rThuongSi=(
                SUM(CASE WHEN sKyHieu='2' THEN rThuongSi ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rThuongSi ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rThuongSi ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rThuongSi ELSE 0 END)
                )
                ,
                rQNCN=(
                SUM(CASE WHEN sKyHieu='2' THEN rQNCN ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNCN ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNCN ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNCN ELSE 0 END)
                )
                ,
                rCNVQPCT=(
                SUM(CASE WHEN sKyHieu='2' THEN rCNVQPCT ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQPCT ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQPCT ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQPCT ELSE 0 END)
                )
                ,
                rQNVQPHD=(
                SUM(CASE WHEN sKyHieu='2' THEN rQNVQPHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rQNVQPHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rQNVQPHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rQNVQPHD ELSE 0 END)
                )
                ,
                rCNVQP=(
                SUM(CASE WHEN sKyHieu='2' THEN rCNVQP ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rCNVQP ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rCNVQP ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rCNVQP ELSE 0 END)
                )
                ,
                rLDHD=(
                SUM(CASE WHEN sKyHieu='2' THEN rLDHD ELSE 0 END) -
                SUM(CASE WHEN sKyHieu='3' THEN rLDHD ELSE 0 END)+
                SUM(CASE WHEN sKyHieu='500' THEN rLDHD ELSE 0 END)-
                SUM(CASE WHEN sKyHieu='600' THEN rLDHD ELSE 0 END)
                )
                                        
{1}", SelectDV, SelectFROM);

            cmd.CommandText = sql;
            if (!iID_MaPhongBan.Equals("-1"))
                cmd.Parameters.AddWithValue("@PhongBan", iID_MaPhongBan);
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            cmd.Parameters.AddWithValue("@iThang",iThang);
            DataTable dt = new DataTable();
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
    }
}