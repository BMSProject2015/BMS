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
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.PhanBo
{
    public class rptThongBaoChiTieuNganSachNamTheoLNSvaDonViController : Controller
    {
        //
        // GET: /rptTongHopNganSachNamTheoLNSvaDonVi/
        public string sViewPath = "~/Report_Views/DuToan/";
        public static String NameFile = "";
        public ActionResult Index(String KieuTrang="")
        {
            String sFilePath = "";
            if (KieuTrang == "1")
            {
                sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi_Doc.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String sLNS = Request.Form["sLNS"];
            String iID_MaDonVi = Request.Form[ParentID + "_iID_MaDonVi"];
            String KieuTrang = Request.Form[ParentID + "_KieuTrang"];
            String Kieuin = Request.Form[ParentID + "_Kieuin"];
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["KieuTrang"] = KieuTrang;
            ViewData["Kieuin"] = Kieuin;
            ViewData["PageLoad"] = "1";
            ViewData["path"] = "~/Report_Views/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi.aspx";
            return View(sViewPath + "ReportView.aspx");
        }

        public long Tong = 0;
        public  DataTable rpThongBaoChiTieuNganSachNam(String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi,String KieuTrang,String Kieuin)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "a.sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            if (Kieuin == "1")
            {
                String SQL = "  SELECT SUBSTRING(a.sLNS,1,1) as NguonNS,b.iID_MaDonVi,b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa,SUM(rTuChi) rTuChi, SUM(rHienVat) rHienVat";
                SQL += " ,Tong=SUM(rTuChi) + SUM(rHienVat)";
                SQL += " FROM PB_PhanBoChiTiet a  INNER JOIN (SELECT * FROM NS_DonVi WHERE iID_MaDonVi=@iID_MaDonVi AND iNamLamViec_DonVi=@iNamLamViec) b ON a.iID_MaDonVi=b.iID_MaDonVi";
                SQL += " WHERE ({0}) AND sNG<>'' AND a.iTrangThai=1 " + ReportModels.DieuKien_NganSach(MaND, "a") + iID_MaTrangThaiDuyet;
                SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa,b.sTen,b.iID_MaDonVi";
                SQL += " HAVING SUM(rTuChi)!=0 or SUM(rHienVat)!=0";
                SQL += " ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc,a.sMoTa asc,b.sTen asc,b.iID_MaDonVi asc";
                SQL = string.Format(SQL, DKLNS);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["Tong"].ToString());
                }
                cmd.Dispose();
            }
            else
            {
                String SQL = "  SELECT SUBSTRING(a.sLNS,1,1) as NguonNS,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa,SUM(rTuChi) rTuChi, SUM(rHienVat) rHienVat";
                SQL += " ,Tong=SUM(rTuChi) + SUM(rHienVat)";
                SQL += " FROM PB_PhanBoChiTiet a  INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b ON a.iID_MaDonVi=b.iID_MaDonVi";
                SQL += " WHERE ({0}) AND sNG<>'' AND a.iTrangThai=1 " + ReportModels.DieuKien_NganSach(MaND, "a") + iID_MaTrangThaiDuyet;
                SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,a.sMoTa";
                SQL += " HAVING SUM(rTuChi)!=0 or SUM(rHienVat)!=0";
                SQL += " ORDER BY sLNS asc,sL asc,sK asc,sM asc,sTM asc,sTTM asc,sNG asc,a.sMoTa asc";
                SQL = string.Format(SQL, DKLNS);
                SqlCommand cmd = new SqlCommand(SQL);
                for (int i = 0; i < arrLNS.Length; i++)
                {
                    cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
                }
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["Tong"].ToString());
                }
                cmd.Dispose();
            }
            return dt;
        }
        //hàm lấy tên đơn vị
        public DataTable tendonvi(String ID)
        {
            if (String.IsNullOrEmpty(ID))
            {
                ID = Guid.Empty.ToString();
            }

            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        [HttpGet]

        public JsonResult ds_DonVi(String ParentID, String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi)
        {
            return Json(obj_DonViTheoLNS(ParentID, MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonViTheoLNS(String ParentID, String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi)
        {
            DataTable dtDonvi = HienThiDonViTheoLNS(MaND,sLNS, iID_MaTrangThaiDuyet);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
            String strLNS = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width:55%\"");
            return strLNS;
    
        }

        public static DataTable HienThiDonViTheoLNS(String MaND,String sLNS, String iID_MaTrangThaiDuyet)
        {
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            DataTable dt = new DataTable();
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length; i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String SQL = string.Format(@"SELECT PB.iID_MaDonVi,DV.sTen 
                                            FROM PB_PhanBoChiTiet as PB
                                            INNER JOIN (SELECT iID_MaDonVi as MaDonVi,sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON PB.iID_MaDonVi=DV.MaDonVi
                                            WHERE PB.iTrangThai=1 AND ({2}) {0} {1}
                                            GROUP BY PB.iID_MaDonVi,DV.sTen
                                            ORDER BY PB.iID_MaDonVi", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKLNS);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
           
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["sTen"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
        }


        public ExcelFile CreateReport(String path, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String KieuTrang, String Kieuin)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            
            String tendv = "";
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable teN = tendonvi(iID_MaDonVi);
             if (teN.Rows.Count > 0)
                {
                    tendv = teN.Rows[0][0].ToString();
                }
                else
                {
                    tendv = "";
                }
            
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi,KieuTrang,Kieuin);
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
                fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("Ngay", NgayThang);
                //fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
                fr.SetValue("sLNS", sLNS);
                if (Kieuin == "1")
                {
                    fr.SetValue("TenDV", tendv);
                }
                else
                {
                    fr.SetValue("TenDV", "");
                }
                fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2));
                fr.Run(Result);
                return Result;
            
        }

        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String KieuTrang, String Kieuin)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable data = rpThongBaoChiTieuNganSachNam(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi,KieuTrang,Kieuin);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "NguonNS,sLNS,sL,sK,sM,sTM", "NguonNS,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "NguonNS,sLNS,sL,sK,sM", "NguonNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "NguonNS,sLNS", "NguonNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "NguonNS", "NguonNS,sMoTa", "sLNS,sL", "NguonNS");
            fr.AddTable("NguonNS", dtNguonNS);
            dtTieuMuc.Dispose();
            dtMuc.Dispose();
            dtLoaiNS.Dispose();
            dtNguonNS.Dispose();
            data.Dispose();
        }

        public clsExcelResult ExportToPDF(String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String KieuTrang, String Kieuin)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KieuTrang == "1")
            {
                sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi_Doc.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, KieuTrang, Kieuin);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "Test.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }

            }
        }

        public clsExcelResult ExportToExcel(String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String KieuTrang, String Kieuin)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (KieuTrang == "1")
            {
                sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi_Doc.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, KieuTrang, Kieuin);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongBaoChiTieuTheoLNSvaDonVi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public ActionResult ViewPDF(String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDonVi, String KieuTrang, String Kieuin)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KieuTrang == "1")
            {
                sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi_Doc.xls";
            }
            else
            {
                sFilePath = "/Report_ExcelFrom/PhanBo/rptThongBaoChiTieuNganSachNamTheoLNSvaDonVi.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iID_MaTrangThaiDuyet, sLNS, iID_MaDonVi, KieuTrang, Kieuin);
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
        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
        public static DataTable tbTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("TenTrangThai", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }
        public static DataTable DtLoaiNganSach(String MaND)
        {
            DataTable dt = new DataTable();

            String SQL = string.Format(@"SELECT DISTINCT sLNS,sLNS+'-'+ sMoTa as sTen
                                        FROM PB_PhanBoChiTiet
                                        WHERE sL='' AND iTrangThai=1 {0} AND LEN(sLNS)=7
                                        ORDER BY sLNS", ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }

    }
}
