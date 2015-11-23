using System;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using VIETTEL.Models;
using System.IO;
using VIETTEL.Models.QuyetToan;

namespace VIETTEL.Report_Controllers.QuyetToan.QuyetToanQuy
{
    public class rptQuyetToan_TongHop_LNSController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const string VIEW_PATH_QUYETTOAN_TONGHOP_LNS = "~/Report_Views/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_LNS.aspx";
        private const String sFilePath = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_LNS.xls";
        private const String sFilePath_To2 = "/Report_ExcelFrom/QuyetToan/QuyetToanQuy/rptQuyetToan_TongHop_LNS_To2.xls";
        
        public ActionResult Index()
        {
            ViewData["path"] = VIEW_PATH_QUYETTOAN_TONGHOP_LNS;
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Lấy các giá trị từ Form gán vào ViewData
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String sLNS = Request.Form["sLNS"];
            String MaTo = Request.Form["MaTo"];
            String iThang_Quy = Request.Form[ParentID + "_iThang_Quy"];
            String iID_MaNamNganSach = Request.Form[ParentID + "_iID_MaNamNganSach"];
            String iID_MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["MaTo"] = MaTo;
            ViewData["iThang_Quy"] = iThang_Quy;
            ViewData["iID_MaNamNganSach"] = iID_MaNamNganSach;
            ViewData["iID_MaPhongBan"] = iID_MaPhongBan;
            ViewData["path"] = VIEW_PATH_QUYETTOAN_TONGHOP_LNS;
            
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Xuất file PDF quyết toán tổng hợp loại ngân sách
        /// </summary>
        /// <param name="MaND">Mã người dung</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="sLNS">Loại ngân sách</param>
        ///  <param name="iThang_Quy">Số tờ</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iThang_Quy, String sLNS, String MaTo, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            HamChung.Language();
            String sDuongDan = "";

            if (MaTo == "1")
                sDuongDan = sFilePath;
            else
                sDuongDan = sFilePath_To2;

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iThang_Quy, MaTo, iID_MaNamNganSach, iID_MaPhongBan);
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

        /// <summary>
        /// Tạo file PDF xuất dữ liệu của quyết toán tổng hợp loại ngân sách
        /// </summary>
        /// <param name="path">đường dẫn</param>
        /// <param name="MaND">Mã người dung</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iThang_Quy">Quý</param>
        /// <param name="MaTo">Số tờ</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iThang_Quy, String MaTo, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQuyetToan_TongHop_LNS");

            LoadData(fr, MaND, sLNS, iThang_Quy, MaTo, iID_MaNamNganSach, iID_MaPhongBan);
            String Nam = ReportModels.LayNamLamViec(MaND);

            //lay ten nam ngan sach
            String NamNganSach = "";
            if (iID_MaNamNganSach == "1")
                NamNganSach = "QUYẾT TOÁN NẮM TRƯỚC";
            else if (iID_MaNamNganSach == "2")
                NamNganSach = "QUYẾT TOÁN NĂM NAY";
            else
            {
                NamNganSach = "TỔNG HỢP";
            }
            String sTenDonVi = "B" + iID_MaPhongBan;
            if (iID_MaPhongBan == "-1")
            {
                sTenDonVi = "";
            }

            DataTable dtDonVi = QuyetToan_ReportModels.dtLNS_DonVi(iThang_Quy, iID_MaNamNganSach, MaND, sLNS, iID_MaPhongBan);
            
            String iID_MaDonVi = "";
           for (int i = 0; i < dtDonVi.Rows.Count; i++)
           {
               iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
           }

           if (!String.IsNullOrEmpty(iID_MaDonVi))
           {
               iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
           }

           String[] arrDonVi = iID_MaDonVi.Split(',');
           String DonVi = iID_MaDonVi;
           String[] TenDV;

           if (MaTo == "1")
           {
               if (arrDonVi.Length < 4)
               {
                   int a1 = 4 - arrDonVi.Length;
                   for (int i = 0; i < a1; i++)
                   {
                       DonVi += ",-1";
                   }
               }

               arrDonVi = DonVi.Split(',');
               TenDV = new String[4];

               for (int i = 0; i < 4; i++)
               {
                   if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                   {
                       TenDV[i] = DonViModels.Get_TenDonVi(arrDonVi[i]);
                   }
               }

               for (int i = 1; i <= TenDV.Length; i++)
               {
                   fr.SetValue("DonVi" + i, TenDV[i - 1]);
               }
           }
           else
           {
               if (arrDonVi.Length < 4 + 7 * (Convert.ToInt16(MaTo) - 1))
               {
                   int a1 = 4 + 7 * (Convert.ToInt16(MaTo) - 1) - arrDonVi.Length;
                   for (int i = 0; i < a1; i++)
                   {
                       DonVi += ",-1";
                   }
                   arrDonVi = DonVi.Split(',');
               }

               TenDV = new String[7];
               int x = 1;

               for (int i = 4 + 7 * ((Convert.ToInt16(MaTo) - 2)); i < 4 + 7 * ((Convert.ToInt16(MaTo) - 1)); i++)
               {
                   if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                   {
                       TenDV[x - 1] = DonViModels.Get_TenDonVi(arrDonVi[i]);
                       x++;
                   }
               }

               for (int i = 1; i <= TenDV.Length; i++)
               {
                   fr.SetValue("DonVi" + i, TenDV[i - 1]);
               }
           }

            dtDonVi.Dispose();

            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("Quy", iThang_Quy);
            fr.SetValue("NamNganSach", NamNganSach);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("ToSo", MaTo);
            fr.Run(Result);

            return Result;
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết của quyết toán tổng hợp loại ngân sách
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="MaND">Mã người dung</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iThang_Quy">Quý</param>
        ///  <param name="MaTo">Số tờ</param>
        /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
        /// <param name="iID_MaPhongBan">Mã phòng ban</param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iThang_Quy, String MaTo, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            DataRow r;
            DataTable data= new DataTable();

            data = QuyetToan_ReportModels.rptQuyetToan_TongHop_LNS(MaND, sLNS, iThang_Quy, MaTo, iID_MaNamNganSach, iID_MaPhongBan);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS1,sLNS3,sLNS5,sLNS", "sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "sLNS1,sLNS3,sLNS5", "sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS5"]));
            }

            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "sLNS1,sLNS3", "sLNS1,sLNS3,sMoTa");
            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS3"]));
            }

            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "sLNS1", "sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS1"]));
            }

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);

            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();

        }

        
       /// <summary>
       /// 
       /// </summary>
       /// <param name="ParentID"></param>
       /// <param name="Thang_Quy">Quý</param>
       ///  <param name="MaTo">Số tờ</param>
       /// <param name="sLNS">Loại ngân sách</param>
       /// <param name="iID_MaNamNganSach">Năm ngân sách</param>
       /// <param name="iID_MaPhongBan">Mã phòng ban</param>
       /// <returns></returns>
        public JsonResult Ds_DonVi(String ParentID, String Thang_Quy, String MaTo, String sLNS, String iID_MaNamNganSach, String iID_MaPhongBan)
        {
            String MaND = User.Identity.Name;
            String sViewPath = "~/Views/DungChung/DonVi/To_DanhSach.ascx";

            DataTable dt = QuyetToan_ReportModels.dtTo_LNS(Thang_Quy, iID_MaNamNganSach, MaND, sLNS,iID_MaPhongBan);

            if (String.IsNullOrEmpty(MaTo))
            {
                MaTo = Guid.Empty.ToString();
            }
            
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, MaTo, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(sViewPath, Model, this);
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

    }
}

