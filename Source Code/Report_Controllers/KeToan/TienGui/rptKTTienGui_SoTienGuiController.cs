using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;


namespace VIETTEL.Report_Controllers.KeToan.TienGui
{
    public class rptKTTienGui_SoTienGuiController : Controller
    {
        //
        // GET: /rptKTTienGui_UyNhiemChi/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TienGui/rptKTTienGui_SoTienGui.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTienGui_SoTienGui.aspx";
            ViewData["LoadPage"] = 0;
            return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            
        }

        public ActionResult EditSubmit(String ParentID, String iNamLamViec)
        {
            string TaiKhoan = Request.Form[ParentID + "_TaiKhoan"];
            String iThang1 = Convert.ToString(Request.Form[ParentID + "_iThang1"]);
            String iThang2 = Convert.ToString(Request.Form[ParentID + "_iThang2"]);
            String iNgay1 = Convert.ToString(Request.Form[ParentID + "_iNgay1"]);
            String iNgay2 = Convert.ToString(Request.Form[ParentID + "_iNgay2"]);
            String locDuLieu = Convert.ToString(Request.Form[ParentID + "_iLocDuLieu"]);
            if (String.IsNullOrEmpty(TaiKhoan))
                return RedirectToAction("Index", "rptKTTienGui_SoTienGui");
            else
            {
                String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
                ViewData["FileName"] = FileName;
                ViewData["iThang1"] = iThang1;
                ViewData["iNamLamViec"] = iNamLamViec;
                ViewData["iThang2"] = iThang2;
                ViewData["iNgay1"] = iNgay1;
                ViewData["iNgay2"] = iNgay2;
                ViewData["iTaiKhoan"] = TaiKhoan;
                ViewData["locDuLieu"] = locDuLieu;
                ViewData["path"] = "~/Report_Views/KeToan/TienGui/rptKTTienGui_SoTienGui.aspx";
                ViewData["LoadPage"] = 1;
                return View(sViewPath + "ReportView.aspx");
            }

        }
        public ExcelFile CreateReport(String path, String iNgay1, String iNgay2, String iThang1, String iThang2, String iTaiKhoan, String iNamLamViec, bool bTrangThaiDuyet)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTienGui_SoTienGui");
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            decimal rSoDuDauKy = dtSoDuDauKy(iNgay1, iThang1, iTaiKhoan, iNamLamViec, bTrangThaiDuyet);
            LoadData(fr, iNgay1, iNgay2, iThang1, iThang2, iTaiKhoan, iNamLamViec, bTrangThaiDuyet);
            fr.SetValue("TuNgay", iNgay1);
            fr.SetValue("TuThang", iThang1);
            fr.SetValue("ToiNgay", iNgay2);
            fr.SetValue("ToiThang", iThang2);
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("Ngay", ngay);
            fr.SetValue("MaTaiKhoan", iTaiKhoan);
            fr.SetValue("SoDuDauKy", rSoDuDauKy);
            fr.SetValue("LuyKeCo", Convert.ToString(dLuyKeCo(iNgay2,iThang2,iTaiKhoan,iNamLamViec,bTrangThaiDuyet)));
            fr.SetValue("LuyKeNo", dLuyKeNo(iNgay2, iThang2, iTaiKhoan, iNamLamViec, bTrangThaiDuyet));
            //fr.SetValue("Thangs", String.Format("{0:MM}", dt));
            fr.SetValue("TenTaiKhoan", getTenTaiKhoan(iTaiKhoan));
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("BoQuocPhong", BoQuocPhong);    
            fr.Run(Result);
            return Result;
        }

        public clsExcelResult ExportToExcel(String iNamLamViec, String iThang, String iID_MaChungTu, String sSoChungTuChiTiet, String LoaiBaoCao, String inmuc, bool bTrangThaiDuyet)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iThang, iID_MaChungTu, sSoChungTuChiTiet, LoaiBaoCao, inmuc, bTrangThaiDuyet);
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ChoDoanhnghiep.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        
        public ActionResult ViewPDF(String iNgay1, String iNgay2, String iThang1, String iThang2, String iTaiKhoan, String iNamLamViec, bool bTrangThaiDuyet )
        {
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
          
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNgay1,  iNgay2,  iThang1,  iThang2,  iTaiKhoan,  iNamLamViec, bTrangThaiDuyet);
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
        private void LoadData(FlexCelReport fr, String iNgay1, String iNgay2, String iThang1, String iThang2, String iTaiKhoan, String iNamLamViec,bool bTrangThaiDuyet)
        {
            DataTable data = dtChiTiet(iNgay1, iNgay2, iThang1, iThang2, iTaiKhoan, iNamLamViec,bTrangThaiDuyet);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();            

        }

        public DataTable dtChiTiet(String iNgay1, String iNgay2, String iThang1, String iThang2, String iTaiKhoan, String iNamLamViec, bool bTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            try
            {
                if (bTrangThaiDuyet)
                {
                    //Lấy mã trạng thái duyệt
                    int iID_MaTrangThaiDuyet = LuongCongViecModel.layTrangThaiDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
                    string query =
                        string.Format(
                            @"select sSoChungTuGhiSo as soGhiSo,iNgay as ngayGhiSo,iThang as thangGhiSo,sSoChungTuChiTiet as soChungTu,iNgayCT as ngayChungTu,iThangCT as thangChungTu,sNoiDung,@MaTaiKhoan as tkDoiChieu,
                            case when iID_MaTaiKhoan_No = @MaTaiKhoan  then iID_MaTaiKhoan_Co else iID_MaTaiKhoan_No end  AS tkDoiUng,
                            case when iID_MaTaiKhoan_Co = @MaTaiKhoan  then rSoTien else null end  AS rCo,
                            case when iID_MaTaiKhoan_No = @MaTaiKhoan  then rSoTien else null end  AS rNo
                            from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec 
                            and ((iThang < @iThang2) or ( (iThang = @iThang2) and  (iNgay <= @iNgay2)))
                            and ((iThang > @iThang1) or ( (iThang = @iThang1) and  (iNgay >= @iNgay1)))
                            and (iID_MaTaiKhoan_No = @MaTaiKhoan or iID_MaTaiKhoan_Co = @MaTaiKhoan) and iTrangThai = 1 and iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet");
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay1", iNgay1);
                    cmd.Parameters.AddWithValue("@iThang1", iThang1);
                    cmd.Parameters.AddWithValue("@iNgay2", iNgay2);
                    cmd.Parameters.AddWithValue("@iThang2", iThang2);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
                else
                {
                    string query =
                       string.Format(
                           @"select sSoChungTuGhiSo as soGhiSo,iNgay as ngayGhiSo,iThang as thangGhiSo,sSoChungTuChiTiet as soChungTu,iNgayCT as ngayChungTu,iThangCT as thangChungTu,sNoiDung,@MaTaiKhoan as tkDoiChieu,
                            case when iID_MaTaiKhoan_No = @MaTaiKhoan  then iID_MaTaiKhoan_Co else iID_MaTaiKhoan_No end  AS tkDoiUng,
                            case when iID_MaTaiKhoan_Co = @MaTaiKhoan  then rSoTien else null end  AS rCo,
                            case when iID_MaTaiKhoan_No = @MaTaiKhoan  then rSoTien else null end  AS rNo
                            from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec 
                            and ((iThang < @iThang2) or ( (iThang = @iThang2) and  (iNgay <= @iNgay2)))
                            and ((iThang > @iThang1) or ( (iThang = @iThang1) and  (iNgay >= @iNgay1)))
                            and (iID_MaTaiKhoan_No = @MaTaiKhoan or iID_MaTaiKhoan_Co = @MaTaiKhoan) and iTrangThai = 1");
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay1", iNgay1);
                    cmd.Parameters.AddWithValue("@iThang1", iThang1);
                    cmd.Parameters.AddWithValue("@iNgay2", iNgay2);
                    cmd.Parameters.AddWithValue("@iThang2", iThang2);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    dt = Connection.GetDataTable(cmd);
                    cmd.Dispose();
                }
            }
            catch (Exception)
            {
                return null;
            }
          
            return dt;
        }
        public Decimal dtSoDuDauKy(String iNgay, String iThang, String iTaiKhoan, String iNamLamViec, bool bTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            decimal dDuNoDauKy = 0;
             //Lấy mã trạng thái duyệt
                int iID_MaTrangThaiDuyet = LuongCongViecModel.layTrangThaiDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
          
                if (bTrangThaiDuyet)
                {
                    //Lấy sô dư tháng 0 từ kề toán tổng họp
                    String SQL_No0 = @"SELECT SUM(rSoTien) as rNo_0 FROM KT_ChungTuChiTiet
                                  WHERE iNamLamViec=@iNamLamViec AND iThangCT=0 
                                  and (iID_MaTaiKhoan_No = @MaTaiKhoan ) and iTrangThai = 1 and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet";
                     SqlCommand cmd = new SqlCommand();
                    cmd.CommandText=SQL_No0;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    dt = Connection.GetDataTable(cmd);
                    decimal dDuNo_0 = 0;
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        dDuNo_0 = Convert.ToDecimal(dt.Rows[0][0]);
                    }
                    String SQL_Co0 = @"SELECT SUM(rSoTien) as rNo_0 FROM KT_ChungTuChiTiet
                                  WHERE iNamLamViec=@iNamLamViec AND iThangCT=0 
                                  and (iID_MaTaiKhoan_Co = @MaTaiKhoan ) and iTrangThai = 1 and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet";
                    cmd = new SqlCommand();
                    cmd.CommandText = SQL_Co0;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    dt = Connection.GetDataTable(cmd);
                    decimal dDuCo_0 = 0;
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        dDuCo_0 = Convert.ToDecimal(dt.Rows[0][0]);
                    }
                    //Lấy số dư nợ đầu kỳ
                    string queryDuNo =
                        string.Format(
                            @"select sum(rSoTien) as TongTienNo from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec and ( (iThangCT < @iThang) or ( (iThangCT = @iThang) and (iNgayCT <= @iNgay))) 
                            and (iID_MaTaiKhoan_No = @MaTaiKhoan ) and iTrangThai = 1 and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet");
                    cmd = new SqlCommand();
                    cmd.CommandText = queryDuNo;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    dt = Connection.GetDataTable(cmd);
                    decimal dDuNo = 0;
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        dDuNo = Convert.ToDecimal(dt.Rows[0][0]);
                    }

                    //Lấy số dư có đầu kỳ
                    string queryDuCo =
                        string.Format(
                            @"select sum(rSoTien) as TongTienNo from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec and ( (iThangCT < @iThang) or ( (iThangCT = @iThang) and (iNgayCT <= @iNgay)) )
                            and (iID_MaTaiKhoan_Co = @MaTaiKhoan ) and iTrangThai = 1 and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet");
                    cmd = new SqlCommand();
                    cmd.CommandText = queryDuCo;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    dt = Connection.GetDataTable(cmd);
                    decimal dDuCo = 0;
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        dDuCo = Convert.ToDecimal(dt.Rows[0][0]);
                    }
                    dDuNoDauKy =dDuNo_0-dDuCo_0+dDuNo - dDuCo;
                }
                else
                {
                    //Lấy sô dư tháng 0 từ kề toán tổng họp
                    String SQL_No0 = @"SELECT SUM(rSoTien) as rNo_0 FROM KT_ChungTuChiTiet
                                  WHERE iNamLamViec=@iNamLamViec AND iThangCT=0 
                                  and (iID_MaTaiKhoan_No = @MaTaiKhoan ) and iTrangThai = 1 and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_No0;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    dt = Connection.GetDataTable(cmd);
                    decimal dDuNo_0 = 0;
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        dDuNo_0 = Convert.ToDecimal(dt.Rows[0][0]);
                    }
                    String SQL_Co0 = @"SELECT SUM(rSoTien) as rNo_0 FROM KT_ChungTuChiTiet
                                  WHERE iNamLamViec=@iNamLamViec AND iThangCT=0 
                                  and (iID_MaTaiKhoan_Co = @MaTaiKhoan ) and iTrangThai = 1 and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet";
                    cmd = new SqlCommand();
                    cmd.CommandText = SQL_Co0;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    dt = Connection.GetDataTable(cmd);
                    decimal dDuCo_0 = 0;
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                            dDuCo_0 = Convert.ToDecimal(dt.Rows[0][0]);
                    }
                    //Lấy số dư nợ đầu kỳ
                    string queryDuNo =
                        string.Format(
                            @"select sum(rSoTien) as TongTienNo from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec  and ( (iThangCT < @iThang) or ( (iThangCT = @iThang) and (iNgayCT <= @iNgay)) ) 
                            and (iID_MaTaiKhoan_No = @MaTaiKhoan ) and iTrangThai = 1");
                    cmd = new SqlCommand();
                    cmd.CommandText = queryDuNo;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    dt = Connection.GetDataTable(cmd);
                    decimal dDuNo = 0;
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        dDuNo = Convert.ToDecimal(dt.Rows[0][0]);
                    }

                    //Lấy số dư có đầu kỳ
                    string queryDuCo =
                        string.Format(
                            @"select sum(rSoTien) as TongTienNo from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec and ( (iThangCT < @iThang) or ( (iThangCT = @iThang) and (iNgayCT <= @iNgay)) )
                            and (iID_MaTaiKhoan_Co = @MaTaiKhoan ) and iTrangThai = 1");
                    cmd = new SqlCommand();
                    cmd.CommandText = queryDuCo;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    dt = Connection.GetDataTable(cmd);
                    decimal dDuCo = 0;
                     if (dt != null && dt.Rows.Count != 0)
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        dDuCo = Convert.ToDecimal(dt.Rows[0][0]);
                    }
                     dDuNoDauKy = dDuNo_0 - dDuCo_0 + dDuNo - dDuCo;
                }
               
           
            
            return dDuNoDauKy;
        }
        public Decimal dLuyKeCo(String iNgay, String iThang, String iTaiKhoan, String iNamLamViec, bool bTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            decimal dLuyKeCo = 0;
            try
            {
                if (bTrangThaiDuyet)
                {
                    //Lấy mã trạng thái duyệt
                    int iID_MaTrangThaiDuyet = LuongCongViecModel.layTrangThaiDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
                    //Lấy số dư nợ đầu kỳ
                    string queryDuCo =
                        string.Format(
                            @"select sum(rSoTien) as luyKeCo from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec and ( (iThang < @iThang) or ( (iThang = @iThang) and (iNgay <= @iNgay))) 
                            and (iID_MaTaiKhoan_Co = @MaTaiKhoan ) and iTrangThai = 1 and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet");
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = queryDuCo;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    dt = Connection.GetDataTable(cmd);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        dLuyKeCo = Convert.ToDecimal(dt.Rows[0][0]);
                    }

                }
                else
                {
                  
                    SqlCommand cmd = new SqlCommand();
                    //Lấy số dư có đầu kỳ
                    string queryDuCo =
                        string.Format(
                            @"select sum(rSoTien) as luyKeCo from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec and ( (iThang < @iThang) or ( (iThang = @iThang) and (iNgay <= @iNgay)) )
                            and (iID_MaTaiKhoan_Co = @MaTaiKhoan ) and iTrangThai = 1");
                    cmd = new SqlCommand();
                    cmd.CommandText = queryDuCo;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    dt = Connection.GetDataTable(cmd);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        dLuyKeCo = Convert.ToDecimal(dt.Rows[0][0]);
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return dLuyKeCo;
        }

        public Decimal dLuyKeNo(String iNgay, String iThang, String iTaiKhoan, String iNamLamViec, bool bTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            decimal dLuyKeCo = 0;
            try
            {
                if (bTrangThaiDuyet)
                {
                    //Lấy mã trạng thái duyệt
                    int iID_MaTrangThaiDuyet = LuongCongViecModel.layTrangThaiDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
                    //Lấy số dư nợ đầu kỳ
                    string queryDuCo =
                        string.Format(
                            @"select sum(rSoTien) as luyKeNo from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec and ( (iThang < @iThang) or ( (iThang = @iThang) and (iNgay <= @iNgay))) 
                            and (iID_MaTaiKhoan_No = @MaTaiKhoan ) and iTrangThai = 1 and iID_MaTrangThaiDuyet =@iID_MaTrangThaiDuyet");
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = queryDuCo;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                    dt = Connection.GetDataTable(cmd);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        dLuyKeCo = Convert.ToDecimal(dt.Rows[0][0]);
                    }

                }
                else
                {

                    SqlCommand cmd = new SqlCommand();
                    //Lấy số dư có đầu kỳ
                    string queryDuCo =
                        string.Format(
                            @"select sum(rSoTien) as luyKeNo from KTTG_ChungTuChiTiet
                            where iNamLamViec = @iNamLamViec and ( (iThang < @iThang) or ( (iThang = @iThang) and (iNgay <= @iNgay)) )
                            and (iID_MaTaiKhoan_No = @MaTaiKhoan ) and iTrangThai = 1");
                    cmd = new SqlCommand();
                    cmd.CommandText = queryDuCo;
                    cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                    cmd.Parameters.AddWithValue("@iNgay", iNgay);
                    cmd.Parameters.AddWithValue("@iThang", iThang);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", iTaiKhoan);
                    dt = Connection.GetDataTable(cmd);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        dLuyKeCo = Convert.ToDecimal(dt.Rows[0][0]);
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return dLuyKeCo;
        }
        public static DataTable Lay_SoChungTu(String iNamLamViec, String iThang, String iID_MaChungTu)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT CT.sSoChungTuChiTiet +'#'+ iID_MaDonVi_Nhan AS sSoChungTuChiTiet,Convert(varchar,CT.sSoChungTuChiTiet)+'-'+ SUBSTRING(CT.sTenDonVi_Nhan,charindex('-',CT.sTenDonVi_Nhan)+1,100) AS sTenDonVi_Nhan";
            SQL += " FROM KTTG_ChungTuChiTiet as CT WHERE  iTrangThai =1 and iNamLamViec = @iNamLamViec";
            SQL += "  AND CT.iID_MaChungTu=@iID_MaChungTu";
            SQL += " Group by iID_MaChungTu,sSoChungTuChiTiet,sTenDonVi_Nhan,iID_MaDonVi_Nhan ORDER BY sTenDonVi_Nhan";
            //AND iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            //cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Tách lấy số chứng từ chi tiết mã đơn vị nhận
        /// </summary>
        /// <param name="sValue">Chuỗi số chứng từ chi tiết và mã đơn vị nhận</param>
        /// <param name="LoaiCanLay">=0 lấy số chứng từ ,1 lấy mã đơn vị nhận</param>
        /// <returns></returns>
        public static String TachLaySoChungTu_DonViNhan(String sValue, int LoaiCanLay)
        {
            String[] arrValue = sValue.Split('#');
            return arrValue[LoaiCanLay];
        }

        /// <summary>
        /// Lấy danh sách tài khoản tiền gửi tương ứng với năm làm việc
        /// </summary>
        /// <param name="iNamLamViec"></param>
        /// <returns></returns>
        public static DataTable dsTaiKhoan(string iNamLamViec)
        {
            DataTable dt = new DataTable();
            try
            {
                //lấy danh sách tài khoản từ bảng tham số
                string query = string.Format(@"select sThamSo from KT_DanhMucThamSo where sKyHieu = @sKyHieu");
                string sKyHieu = "68";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
                dt = Connection.GetDataTable(cmd);
                if (dt != null && dt.Rows.Count != 0)
                {
                    string sListTaiKhoan = Convert.ToString(dt.Rows[0][0]);
                    query = string.Format(@"select iID_MaTaiKhoan,sTen,iID_MaTaiKhoan+'-'+sTen as TenHT from dbo.KT_TaiKhoan where iTrangThai=1 AND  iID_MaTaiKhoan  in ({0}) and iNam = @iNam ", sListTaiKhoan);
                    cmd = new SqlCommand();
                    cmd.CommandText = query;
                    //cmd.Parameters.AddWithValue("@sListTaiKhoan", sListTaiKhoan);
                    cmd.Parameters.AddWithValue("@iNam", iNamLamViec);
                    dt = Connection.GetDataTable(cmd);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }


            return dt;
        }
        public static string getTenTaiKhoan(string MaTaiKhoan)
        {
            DataTable dt = new DataTable();
            string sV = string.Empty;
            try
            {
                //lấy danh sách tài khoản từ bảng tham số
                string query = string.Format(@"select  sTen from KT_TaiKhoan
                                            where iID_MaTaiKhoan = @iID_MaTaiKhoan");
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", MaTaiKhoan);
                dt = Connection.GetDataTable(cmd);
                if (dt != null && dt.Rows.Count != 0)
                {
                    sV = Convert.ToString(dt.Rows[0][0]);
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception)
            {
                return string.Empty;
            }
            return sV;
        }

    }
}
