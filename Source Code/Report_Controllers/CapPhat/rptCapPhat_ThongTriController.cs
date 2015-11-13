using System;
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
using System.Text.RegularExpressions;
using System.Collections;
namespace VIETTEL.Report_Controllers.ThuNop
{
    public class rptCapPhat_ThongTriController : Controller
    {
        // GET: /rptCapPhat_ThongTri/
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath_ChiTiet_Nganh = "/Report_ExcelFrom/CapPhat/rptCapPhat_ThongTri_ChiTiet_Nganh.xls";
        private const String sFilePath_ChiTiet_Muc = "/Report_ExcelFrom/CapPhat/rptCapPhat_ThongTri_ChiTiet_Muc.xls";
        private const String sFilePath_TongHop_Nganh = "/Report_ExcelFrom/CapPhat/rptCapPhat_ThongTri_TongHop_Nganh.xls";
        private const String sFilePath_TongHop_Muc = "/Report_ExcelFrom/CapPhat/rptCapPhat_ThongTri_TongHop_Muc.xls"; 
        
            public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_ThongTri.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <param name="iNamCapPhat"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiTongHop"></param>
        /// <returns></returns>
        /// </summary>
        public static DataTable rptCapPhat_ThongTri(String MaND, String sLNS, String iNamCapPhat, String iID_MaDonVi, String LoaiTongHop)
        {
            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);

            //Báo cáo chi tiết từng đơn vị
            if(LoaiTongHop == "ChiTiet")
            {
                if(!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi !="-1")
                {
                    DK += " AND iID_MaDonVi=@iID_MaDonVi";
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
            }
            //Báo cáo tổng hợp các đơn vị
            else
            {
                if (String.IsNullOrEmpty(iID_MaDonVi))
                    iID_MaDonVi = Guid.Empty.ToString();

                String[] arrDonVi = iID_MaDonVi.Split(',');

                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    DK += "iID_MaDonVi=@MaDonVi" + i;
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i]);
                    if (i < arrDonVi.Length - 1)
                        DK += " OR ";
                }

                if (!String.IsNullOrEmpty(DK))
                    DK = " AND (" + DK + ")";
            }

            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN (" + sLNS + ")";
            }
       
            //Báo cáo chi tiết từng đơn vị
            if(LoaiTongHop == "ChiTiet")
            {
                SQL = String.Format(@"
                    SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                            SUBSTRING(sLNS,1,3) as sLNS3,
                            SUBSTRING(sLNS,1,5) as sLNS5,
                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                            ,SUM(rTuChi) as rTuChi
                     FROM CP_CapPhatChiTiet
                     WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
                            AND CONVERT(VARCHAR(10), dNgayCapPhat, 103) = '{3}' 
                     GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                     HAVING SUM(rTuChi)<>0 ", DK, DKDonVi, DKPhongBan, iNamCapPhat);
            }
            //Báo cáo tổng hợp các đơn vị
            else
            {
                SQL = String.Format(@"
                    SELECT SUBSTRING(sLNS,1,1) as sLNS1,
                            SUBSTRING(sLNS,1,3) as sLNS3,
                            SUBSTRING(sLNS,1,5) as sLNS5,
                            sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                            ,SUM(rTuChi) as rTuChi
                     FROM CP_CapPhatChiTiet
                     WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec {0} {1} {2}
                             AND CONVERT(VARCHAR(10), dNgayCapPhat, 103) = '{3}' 
                     GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi,sTenDonVi
                     HAVING SUM(rTuChi)<>0 ", DK, DKDonVi, DKPhongBan, iNamCapPhat);
            }
            
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            //Get value from View page
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String iNamCapPhat = Request.Form[ParentID + "_iNamCapPhat"];
            String MaPhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String LoaiTongHop = Request.Form[ParentID + "_LoaiTongHop"];
            String DenMuc = Request.Form[ParentID + "_DenMuc"];
            String LoaiCapPhat = Request.Form[ParentID + "_LoaiCapPhat"];
            String LoaiThongTri = Request.Form[ParentID + "_LoaiThongTri"];
            
            //Set value for ViewData
            ViewData["PageLoad"] = "1";
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iNamCapPhat"] = iNamCapPhat;
            ViewData["MaPhongBan"] = MaPhongBan;
            ViewData["LoaiTongHop"] = LoaiTongHop;
            ViewData["DenMuc"] = DenMuc;
            ViewData["LoaiCapPhat"] = LoaiCapPhat;
            ViewData["LoaiThongTri"] = LoaiThongTri;

            ViewData["path"] = "~/Report_Views/CapPhat/rptCapPhat_ThongTri.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <param name="iNamCapPhat"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiTongHop"></param>
        /// <param name="LoaiCapPhat"></param>
        /// <param name="LoaiThongTri"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String sLNS, String iNamCapPhat, String iID_MaDonVi, String LoaiTongHop, String LoaiCapPhat, String LoaiThongTri)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path); 
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptCapPhat_ThongTri");
            
            LoadData(fr, MaND, sLNS, iNamCapPhat, iID_MaDonVi, LoaiTongHop);
            //VungNV: 2015/09/30 lấy tháng của đợt cấp phát
            DateTime dNamCapPhat = Convert.ToDateTime(iNamCapPhat);
            String Thang = dNamCapPhat.Month.ToString();
            String Nam = ReportModels.LayNamLamViec(MaND);
            String sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi,MaND);

            //VungNV: 2015/09/30 Set value for LoaiCapPhat
            if (String.IsNullOrWhiteSpace(LoaiCapPhat)) 
            {
                DataTable dtLoaiThongTri = CommonFunction.Lay_dtDanhMuc("LoaiCapPhat"); 

                foreach(DataRow row in dtLoaiThongTri.Rows)
                {
                    string sMaLoai = row["iID_MaDanhMuc"].ToString();

                    if(sMaLoai == LoaiThongTri)
                    {
                        LoaiCapPhat = row["sTen"].ToString();
                        break;
                    }
                }
            }

            fr.SetValue("BoQuocPhong", ReportModels.CauHinhTenDonViSuDung(1, MaND));
            fr.SetValue("QuanKhu", ReportModels.CauHinhTenDonViSuDung(2, MaND));
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Nam", Nam);
            fr.SetValue("NgayLap", "tháng "+ Thang +" năm " + Nam);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("LoaiCapPhat", LoaiCapPhat.Trim());

            fr.Run(Result);
            return Result;
        }

        /// <summary>
        /// Đổ dư liệu xuống báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <param name="iNamCapPhat"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiTongHop"></param>
        /// <returns></returns>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String iNamCapPhat, String iID_MaDonVi, String LoaiTongHop)
        {
            int SoDong = 0;
            DataRow r;
            DataTable data = new DataTable();
            DataTable dtDonVi = new DataTable();
            //Hiển thị chi tiết
            if(LoaiTongHop == "ChiTiet")
            {
                data = rptCapPhat_ThongTri(MaND, sLNS, iNamCapPhat, iID_MaDonVi, LoaiTongHop);
            }
            //Hiển thị tổng hợp
            else
            {
                dtDonVi = rptCapPhat_ThongTri(MaND, sLNS, iNamCapPhat, iID_MaDonVi, LoaiTongHop);
                data = HamChung.SelectDistinct("ChiTiet", dtDonVi, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa");
                fr.AddTable("dtDonVi", dtDonVi);
                dtDonVi.Dispose();
            }

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
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }

            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "sLNS1,sLNS3", "sLNS1,sLNS3,sMoTa");
            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }

            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "sLNS1", "sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
            }

            long TongTien = 0;
            if (LoaiTongHop == "ChiTiet")
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (data.Rows[i]["rTuChi"].ToString() != "")
                    {
                        TongTien += long.Parse(data.Rows[i]["rTuChi"].ToString());
                    }
                }
            }
            else
            {
                for (int i = 0; i < dtDonVi.Rows.Count; i++)
                {
                    if (dtDonVi.Rows[i]["rTuChi"].ToString() != "")
                    {
                        TongTien += long.Parse(dtDonVi.Rows[i]["rTuChi"].ToString());
                    }
                }
            }

            //In loại tiền bằng chữ
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();

            //Ghi chú
            DataTable dt = new DataTable();
            dt.Columns.Add("sGhiChu", typeof(String));
            int soChu1Trang = 80;

            String SQL =
                String.Format(
                    @"SELECT sGhiChu 
                      FROM CP_GhiChu 
                      WHERE sTen='rptCapPhat_ThongTri' AND sID_MaNguoiDung=@MaND AND iID_MaDonVi=@iID_MaDonVi");

            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@MaND", MaND);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            String sGhiChu = Connection.GetValueString(cmd, "");
            ArrayList arrDongTong = new ArrayList();
            String[] arrDong = Regex.Split(sGhiChu, "&#10;");

            for (int i = 0; i < arrDong.Length; i++)
            {
                if (arrDong[i] != "")
                {
                    int tg = 0;
                    String s = "";
                    String[] arrDongCon = arrDong[i].Split(' ');
                    for (int j = 0; j < arrDongCon.Length; j++)
                    {
                        int x = arrDongCon[j].Length;
                        tg = tg + x + 1;
                        if (tg > soChu1Trang)
                        {
                            arrDongTong.Add(s);
                            j--;
                            tg = 0;
                            s = "";
                            continue;
                        }

                        s += arrDongCon[j].Trim() + " ";
                    }

                    if (tg <= soChu1Trang) 
                        arrDongTong.Add(s);
                }
            }

            for (int j = 0; j < arrDongTong.Count; j++)
            {
                r = dt.NewRow();
                r["sGhiChu"] = arrDongTong[j];
                dt.Rows.Add(r);
            }

            SoDong = data.Rows.Count;

            for (int i = 0; i < dtsTM.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dtsTM.Rows[i]["sMoTa"])))
                    SoDong++;
            }

            for (int i = 0; i < dtsM.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dtsM.Rows[i]["sMoTa"])))
                    SoDong++;
            }

            for (int i = 0; i < dtsL.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dtsL.Rows[i]["sMoTa"])))
                    SoDong++;
            }

            for (int i = 0; i < dtsLNS.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dtsLNS.Rows[i]["sMoTa"])))
                    SoDong++;
            }

            fr.AddTable("dtDongTrang", dt);
            fr.SetValue("Tien", Tien);
            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);
            int KhoanhCachDong = 120;
            int SoDongTrang1 = 23;
            int SoDongGhiChu = dt.Rows.Count;
           
            //trang 1 voi cỡ chữ 10, số dòng trên trang 23 dòng
            if (SoDongGhiChu == 0)
            {
                SoDongTrang1 = 23;
                if(SoDong<=SoDongTrang1+3 && SoDong>SoDongTrang1)
                    KhoanhCachDong = 158 + (SoDongTrang1 - SoDong)*2;
            }
             //có ghi chú
            else
            {
                if(SoDongGhiChu<=10)
                {
                    if (SoDong + SoDongGhiChu > SoDongTrang1 - 3 && SoDong + SoDongGhiChu < SoDongTrang1 + 3)
                    {
                        KhoanhCachDong = 200;
                    }

                }
            }

            fr.SetExpression("test", "<#Row height(Autofit;"+KhoanhCachDong+")>");
            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();

        }

        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS=@sLNS");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            sMoTa = Connection.GetValueString(cmd, "");
            return sMoTa;
        }

        /// <summary>
        /// Hiển thị báo cáo theo định dạng PDF
        /// </summary>
        /// <param name="MaND"></param>
        /// <param name="sLNS"></param>
        /// <param name="iNamCapPhat"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="LoaiTongHop"></param>
        /// <param name="LoaiCapPhat"></param>
        /// <param name="LoaiThongTri"></param>
        /// <returns></returns>

        public ActionResult ViewPDF(String MaND, String iNamCapPhat, String sLNS, String iID_MaDonVi, 
                    String LoaiTongHop, String DenMuc, String LoaiCapPhat, String LoaiThongTri)
        {
            HamChung.Language();
            String sDuongDan = "";

            //Hiện thị chi tiết
            if(LoaiTongHop == "ChiTiet")
            {
                //Hiện thị đến ngành
                if(DenMuc=="Nganh")
                    sDuongDan = sFilePath_ChiTiet_Nganh;
                //Hiện thị đến mục
                else
                    sDuongDan = sFilePath_ChiTiet_Muc;
            }
            //Hiện thị tổng hợp
            else
            {
                //Hiện thị đến ngành
                if(DenMuc == "Nganh")
                    sDuongDan = sFilePath_TongHop_Nganh;
                //Hiện thị đến mục
                else
                    sDuongDan = sFilePath_TongHop_Muc;
            }

            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iNamCapPhat, iID_MaDonVi, LoaiTongHop, LoaiCapPhat, LoaiThongTri);
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

        public clsExcelResult ExportToExcel(String MaND, String sLNS, String iNamCapPhat, String iID_MaDonVi,
                     String LoaiTongHop, String DenMuc, String LoaiCapPhat, String LoaiThongTri)
        {
            HamChung.Language();
            String sDuongDan = "";
            //Hiện thị chi tiết
            if (LoaiTongHop == "ChiTiet")
            {
                //Hiện thị đến ngành
                if (DenMuc == "Nganh")
                    sDuongDan = sFilePath_ChiTiet_Nganh;
                //Hiện thị đến mục
                else
                    sDuongDan = sFilePath_ChiTiet_Muc;
            }
           //Hiện thị tổng hợp
            else
            {
                //Hiện thị đến ngành
                if (DenMuc == "Nganh")
                    sDuongDan = sFilePath_TongHop_Nganh;
                //Hiện thị đến mục
                else 
                    sDuongDan = sFilePath_TongHop_Muc;
            }
           
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sDuongDan), MaND, sLNS, iNamCapPhat, iID_MaDonVi, LoaiTongHop,LoaiCapPhat, LoaiThongTri);
            
            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "Thongtriquyettoan.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
        //VungNV: 2015/09/30 Lấy danh sách LNS 
        public JsonResult Ds_LNS(String ParentID, String iNamCapPhat, String LoaiThongTri, String sLNS, String iID_MaPhongBan)
        {
            String MaND = User.Identity.Name;
            DataTable dt = CapPhat_ChungTuModels.dtLoaiThongTri_LNS(iNamCapPhat, MaND, LoaiThongTri, iID_MaPhongBan);

            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }

            String ViewNam = "~/Views/DungChung/DonVi/LNS_DanhSach_ThongTri.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, sLNS, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);

            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        //VungNV: 2015/09/30 Lấy danh sách các đơn vị
        public JsonResult Ds_DonVi(String ParentID, String iNamCapPhat, String LoaiThongTri, String sLNS, String iID_MaDonVi, String iID_MaPhongBan)
        {
            String MaND = User.Identity.Name;
            DataTable dt = CapPhat_ChungTuModels.dtLNS_DonVi(iNamCapPhat, LoaiThongTri, MaND, sLNS, iID_MaPhongBan);

            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }

            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach_ThongTri.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dt, ParentID);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        //VungNV: 2015/09/30 Lấy ghi chú của các Đơn vị
        public JsonResult DsGhiChu(String ParentID, String MaND, String iID_MaDonVi, String sGhiChu)
        {
            sGhiChu = "";
            String sTen = "rptCapPhat_ThongTri";
            String SQL = "";
            SQL = string.Format(@"SELECT sGhiChu FROM CP_GhiChu WHERE sTen=@sTen AND sID_MaNguoiDung=@sID_MaNguoiDung AND iID_MaDonVi=@iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sTen", sTen);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            sGhiChu = Connection.GetValueString(cmd, "");
            String strDonVi = "";

            if (iID_MaDonVi != "-1")
                strDonVi = MyHtmlHelper.TextArea(ParentID, sGhiChu, "sGhiChu", "", "style=\"width:100%; height: 220px\" onchange=\"changeTest(this.value)\"");
            
            return Json(strDonVi, JsonRequestBehavior.AllowGet);
        }

        //VungNV: 2015/09/30 thêm mới hoặc update ghi chú 
        public ActionResult Update_GhiChu(String sGhiChu, String MaND, String iID_MaDonVi)
        {
            String sTen = "rptCapPhat_ThongTri";
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            SQL = String.Format(
                @"IF NOT EXISTS(
		            SELECT sGhiChu 
		            FROM CP_GhiChu 
		            WHERE sTen = @sTen AND sID_MaNguoiDung = @sID_MaNguoiDung AND iID_MaDonVi = @iID_MaDonVi
	            )
            INSERT INTO CP_GhiChu(sTen, sID_MaNguoiDung, iID_MaDonVi, sGhiChu) 
		            VALUES(@sTen, @sID_MaNguoiDung, @iID_MaDonVi, @sGhiChu)
            ELSE 
	            UPDATE CP_GhiChu 
	            SET sGhiChu=@sGhiChu 
	            WHERE  sTen = @sTen AND	 sID_MaNguoiDung = @sID_MaNguoiDung AND iID_MaDonVi =@iID_MaDonVi");


            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sTen", sTen);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            sGhiChu = sGhiChu.Replace("^", "&#10;");
            cmd.Parameters.AddWithValue("@sGhiChu", sGhiChu);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        //VungNV: 2015/09/30 thêm mới hoặc update loại cấp phát
        public ActionResult update_LoaiCapPhat(String LoaiCapPhat, String MaND)
        {
           String sTen = "rptCapPhat_ThongTri_LoaiCapPhat";
            String SQL = "";
            SQL = String.Format(
                @"IF NOT EXISTS(
	                SELECT sGhiChu 
	                FROM CP_GhiChu 
	                WHERE sTen =  @sTen AND sID_MaNguoiDung = @sID_MaNguoiDung
	            )
            INSERT INTO CP_GhiChu(sTen, sID_MaNguoiDung, sGhiChu) 
		            VALUES( @sTen,@sID_MaNguoiDung, @sGhiChu)
            ELSE 
                UPDATE CP_GhiChu 
                SET sGhiChu=@sGhiChu 
                WHERE  sTen = @sTen AND	 sID_MaNguoiDung = @sID_MaNguoiDung");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sTen", sTen);
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@sGhiChu", LoaiCapPhat);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();

            return Json("", JsonRequestBehavior.AllowGet);

        }

        //VungNV: 2015/09/30 lấy giá trị của loại cấp phát
        public ActionResult getLoaiCapPhat(String ParentID, String MaND) 
        {
            String sLoaiCapPhat = "";
            String sTen = "rptCapPhat_ThongTri_LoaiCapPhat";
            String SQL = "";
            SQL = String.Format(
                @"SELECT sGhiChu FROM CP_GhiChu WHERE sTen=@sTen AND sID_MaNguoiDung=@sID_MaNguoiDung"
               );

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaND);
            cmd.Parameters.AddWithValue("@sTen", sTen);
            sLoaiCapPhat = Connection.GetValueString(cmd, "");
            cmd.Dispose();
           
            return Json(sLoaiCapPhat, JsonRequestBehavior.AllowGet);
        }

    }
}

