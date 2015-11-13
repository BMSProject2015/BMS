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


namespace VIETTEL.Report_Controllers.BaoHiem
{
    public class rptBH_Chi_74_4Controller : Controller
    {
        //
        // GET: /rptBH_Chi_74_4/

        public string sViewPath = "~/Report_Views/";
        // private const String sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_74_4_Mau1.xls";

        public static String NameFile = "";
        public ActionResult Index(String KhoGiay = "")
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_74_4_Mau1.xls";
            }
            else
            {

                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_74_4_Mau2.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["pageload"] = "0";
                ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_Chi_74_4.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        public ActionResult EditSubmit(String ParentID)
        {
            // String NamLamViec = Convert.ToString(Request.Form[ParentID + "_iNamLamViec"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String LoaiBieu = Convert.ToString(Request.Form[ParentID + "_LoaiBieu"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String BoDongTrong = Convert.ToString(Request.Form[ParentID + "_BoDongTrong"]);
            String KhoGiay = Convert.ToString(Request.Form[ParentID + "_KhoGiay"]);
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["LoaiBieu"] = LoaiBieu;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["BoDongTrong"] = BoDongTrong;
            ViewData["KhoGiay"] = KhoGiay;
            ViewData["path"] = "~/Report_Views/BaoHiem/rptBH_Chi_74_4.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
            //return RedirectToAction("Index", new { NamLamViec = NamLamViec, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, BoDongTrong = BoDongTrong });
        }


        public ExcelFile CreateReport(String path, String MaND, String iID_MaDonVi, String LoaiBieu, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptBH_Chi_74_4");
            String sTenDonVi = "";

            String SQL = "SELECT iID_MaDonVi+'-'+sTen as TenHT FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=@iID_MaDonVi";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            sTenDonVi = Connection.GetValueString(cmd, "");
            //chọn tất cả đơn vị
            if (LoaiBieu == "2") sTenDonVi = "";
            LoadData(fr, MaND, iID_MaDonVi, LoaiBieu, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);
            fr.SetValue("Nam", NamLamViec);
            fr.SetValue("Cap1", BoQuocPhong);
            fr.SetValue("Cap2", CucTaiChinh);
            fr.SetValue("Cap1", ReportModels.CauHinhTenDonViSuDung(1));
            fr.SetValue("Ngay", NgayThang);
            fr.SetValue("TenDV", sTenDonVi);
            fr.Run(Result);
            return Result;

        }

        public DataTable rptBH_Chi_74_4(String MaND, String iID_MaDonVi, String LoaiBieu, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            #region tao dt BH_Chi
            DataTable dt = new DataTable();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
            }

            String DKDonvi = "";

            //xoa dong trong
            DataTable dtDonVi = HienThiDonViTheoNam(MaND, iID_MaTrangThaiDuyet);
            #region Xóa dòng không có dữ liệu
            if (BoDongTrong == "on")
            {
                #region Tất cả đơn vị
                if (LoaiBieu == "2")
                {

                    if (dtDonVi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDonVi.Rows.Count; i++)
                        {
                            DKDonvi += " iID_MaDonVi =@iID_MaDonVi" + i;
                            if (i < dtDonVi.Rows.Count - 1)
                                DKDonvi += " OR ";
                        }
                    }

                }
                #endregion
                #region từng đơn vị
                else
                {
                    DKDonvi = "iID_MaDonVi =@iID_MaDonVi";
                }
                #endregion
                String SQL = string.Format(@" SELECT b.sLNS,b.sL,b.sK,b.sM,b.sTM,b.sTTM,b.sNG,b.sTNG,b.sMoTa ,b.bLaHangCha
                                    ,sum (SLSQ) as SLSQ,sum (TienSQ) as TienSQ,sum (SLQNCN) as SLQNCN,sum (TienQNCN) as TienQNCN,sum (SLCNV) as SLCNV
                                    ,sum (TienCNV) as TienCNV,sum (SLHD) as SLHD,sum (TienLDHD) as TienLDHD,sum (SLHDKhac) as SLHDKhac
                                    ,sum (TienHDKhac) as TienHDKhac,sum (SLHSQ) as SLHSQ,sum (TienHSQ) as TienHSQ
                                    FROM(
                                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG
                                         ,sMoTa=CASE WHEN bLaHangCha=1 THEN UPPER(sMoTa) ELSE sMoTa END
                                         ,bLaHangCha=CASE WHEN bLaHangCha=1 THEN 1 ELSE 0 END
                                        ,SUM(rSLSQ) as SLSQ ,TienSQ=SUM(rTienSQ),SUM(rSLQNCN) as SLQNCN
                                        ,TienQNCN=SUM(rTienQNCN),SUM(rSLCNV) as SLCNV ,TienCNV=SUM(rTienCNV)
                                        ,SUM(rSLHD) as SLHD ,TienLDHD=SUM(rTienHD) ,SUM(rSLHD_Khac) as SLHDKhac
                                        ,TienHDKhac=SUM(rTienHD_Khac) ,SUM(rSLHSQ_CS) as SLHSQ,TienHSQ=SUM(rTienHSQ_CS)
                                        FROM BH_ChungTuChiChiTiet
                                        WHERE  bChi=1 AND iTrangThai=1 {1}  {0}  AND ({2}) AND sTNG<>''
                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iThang_Quy
                                        HAVING SUM(rSLSQ)!=0 OR SUM(rTienSQ)!=0 OR SUM(rSLQNCN)!=0
                                        OR SUM(rTienQNCN)!=0 OR SUM(rSLCNV)!=0 
                                        OR SUM(rTienCNV)!=0 OR SUM(rSLHD)!=0 OR SUM(rTienHD)!=0
                                        OR SUM(rSLHD_Khac)!=0 OR SUM(rTienHD_Khac)!=0 OR SUM(rSLHSQ_CS)!=0  OR SUM(rTienHSQ_CS)!=0
                                        OR bLaHangCha>0) as b
                                    GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa ,bLaHangCha
                                    HAVING 
                                    sum (SLSQ) !=0 or sum (TienSQ) !=0 or sum (SLQNCN) !=0 or sum (TienQNCN) !=0 or sum (SLCNV) !=0
                                    or sum (TienCNV) !=0 or sum (SLHD) !=0 or sum (TienLDHD)!=0 or sum (SLHDKhac) !=0
                                    or sum (TienHDKhac) !=0 or sum (SLHSQ) !=0 or sum (TienHSQ) !=0", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKDonvi);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiBieu == "2")
                {
                    if (dtDonVi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDonVi.Rows.Count; i++)
                        {
                            cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                        }
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }

                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
            #endregion
            #region hiển thị tất cả
            else
            {
                #region Tất cả đơn vị
                if (LoaiBieu == "2")
                {
                    if (dtDonVi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDonVi.Rows.Count; i++)
                        {
                            DKDonvi += " iID_MaDonVi =@iID_MaDonVi" + i;
                            if (i < dtDonVi.Rows.Count - 1)
                                DKDonvi += " OR ";
                        }
                    }

                }
                #endregion
                #region từng đơn vị
                else
                {
                    DKDonvi = "iID_MaDonVi =@iID_MaDonVi";
                }
                #endregion
                String SQL1 = string.Format(@"select sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha
,SUM(SLSQ) as SLSQ ,SUM(TienSQ)as TienSQ ,SUM(SLQNCN) as SLQNCN
,SUM(TienQNCN) as TienQNCN,SUM(SLCNV) as SLCNV ,SUM(TienCNV) as TienCNV
,SUM(SLHD) as SLHD ,SUM(TienLDHD) as TienLDHD,SUM(SLHDKhac) as SLHDKhac
,SUM(TienHDKhac)as TienHDKhac ,SUM(SLHSQ) as SLHSQ ,SUM(TienHSQ) as TienHSQ
from(SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG
                                            ,sMoTa=CASE WHEN bLaHangCha=1 THEN UPPER(sMoTa) ELSE sMoTa END
                                         ,bLaHangCha=CASE WHEN bLaHangCha=1 THEN 1 ELSE 0 END
                                        ,SUM(rSLSQ) as SLSQ ,TienSQ=SUM(rTienSQ) ,SUM(rSLQNCN) as SLQNCN
                                        ,TienQNCN=SUM(rTienQNCN) ,SUM(rSLCNV) as SLCNV ,TienCNV=SUM(rTienCNV)
                                        ,SUM(rSLHD) as SLHD ,TienLDHD=SUM(rTienHD) ,SUM(rSLHD_Khac) as SLHDKhac
                                        ,TienHDKhac=SUM(rTienHD_Khac) ,SUM(rSLHSQ_CS) as SLHSQ ,TienHSQ=SUM(rTienHSQ_CS)
                                        FROM BH_ChungTuChiChiTiet
                                        WHERE  bChi=1 AND iTrangThai=1 {1} {0} AND  ({2}) AND sTNG<>''
                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iThang_Quy) as temp
GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND), DKDonvi);
                SqlCommand cmd1 = new SqlCommand(SQL1);
                if (LoaiBieu == "2")
                {
                    if (dtDonVi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDonVi.Rows.Count; i++)
                        {
                            cmd1.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                        }
                    }
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }

                dt = Connection.GetDataTable(cmd1);
                cmd1.Dispose();

            }
            DataTable dtChiTieu = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {

                iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = "";
            }
            String SQLChiTieu = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,ChiTieu
                                                        FROM(
                                                        SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iID_MaDotPhanBo,sum(rTuChi) as ChiTieu
                                                        FROM PB_PhanBoChiTiet
                                                        WHERE ({0}) {1} AND iTrangThai = 1
                                                        {1} AND sLNS = 2200000
                                                        GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,bLaHangCha,iID_MaDotPhanBo
                                                        HAVING SUM(rTuChi)<>0) as a
                                                        INNER JOIN (SELECT iID_MaDotPhanBo,dNgayDotPhanBo FROM PB_DotPhanBo WHERE month(dNgayDotPhanBo)<='12') as b
                                                        ON a.iID_MaDotPhanBo=b.iID_MaDotPhanBo 
                                                    ", DKDonvi, ReportModels.DieuKien_NganSach(MaND), iID_MaTrangThaiDuyet);
            SqlCommand cmdChiTieu = new SqlCommand(SQLChiTieu);

            if (LoaiBieu == "2")
            {
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                    }
                }
            }
            else
            {
                cmdChiTieu.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            }
            cmdChiTieu.Parameters.AddWithValue("@NamLamViec", NamLamViec);
            //cmdChiTieu.Parameters.AddWithValue("@dNgayDotPhanBo", CommonFunction.LayNgayTuXau(dNgayCapPhat));

            dtChiTieu = Connection.GetDataTable(cmdChiTieu);
            cmdChiTieu.Dispose();
            #endregion
            #endregion
            #region ghep dt vao dt chi tieu
            DataRow _addR;
            String _sCol = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,ChiTieu";
            String[] _arrCol = _sCol.Split(',');

            dt.Columns.Add("ChiTieu", typeof(Decimal));
            for (int i = 0; i < dtChiTieu.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@" sLNS='{0}' AND sL='{1}' AND sK='{2}' AND sM='{3}' AND sTM='{4}'
                                                    AND sTTM='{5}' AND sNG='{6}' AND sTNG='{7}'",
                                                  dtChiTieu.Rows[i]["sLNS"], dtChiTieu.Rows[i]["sL"],
                                                  dtChiTieu.Rows[i]["sK"],
                                                  dtChiTieu.Rows[i]["sM"], dtChiTieu.Rows[i]["sTM"],
                                                  dtChiTieu.Rows[i]["sTTM"], dtChiTieu.Rows[i]["sNG"], dtChiTieu.Rows[i]["sTNG"]
                                                  );
                DataRow[] R = dt.Select(xauTruyVan);
                DataRow R2;
                if (R == null || R.Length == 0)
                {
                    _addR = dt.NewRow();
                    for (int j = 0; j < _arrCol.Length; j++)
                    {
                        _addR[_arrCol[j]] = dtChiTieu.Rows[i][_arrCol[j]];
                    }
                    dt.Rows.Add(_addR);
                }
                else
                {
                    foreach (DataRow R1 in dtChiTieu.Rows)
                    {

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dt.Rows[j];

                            for (int c = 0; c < _arrCol.Length - 2; c++)
                            {
                                if (R2[_arrCol[c]].Equals(R1[_arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dt.Rows[j]["ChiTieu"] = R1["ChiTieu"];

                                break;
                            }

                        }
                    }
                }
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
            dt = dv.ToTable();

            #endregion


            return dt;
        }
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaDonVi, String LoaiBieu, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            DataTable data = rptBH_Chi_74_4(MaND, iID_MaDonVi, LoaiBieu, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            DataTable dtNganh;
            dtNganh = HamChung.SelectDistinct("Nganh", data, "sLNS,sL,sK,sM,sTM,sTTM,sNG", "sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "sLNS,sL,sK,sNG,sTNG");
            fr.AddTable("Nganh", dtNganh);

            DataTable dtTieuMuc;
            dtTieuMuc = HamChung.SelectDistinct("TieuMuc", dtNganh, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sTTM,sNG");
            fr.AddTable("TieuMuc", dtTieuMuc);

            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTTM");
            fr.AddTable("Muc", dtMuc);

            data.Dispose();
            dtNganh.Dispose();
            dtTieuMuc.Dispose();
            dtMuc.Dispose();

        }

        public clsExcelResult ExportToPDF(String MaND, String iID_MaDonVi, String LoaiBieu, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            HamChung.Language();
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_74_4_Mau1.xls";
            }
            else
            {

                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_74_4_Mau2.xls";
            }
            clsExcelResult clsResult = new clsExcelResult();

            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, LoaiBieu, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);
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

        public clsExcelResult ExportToExcel(String MaND, String iID_MaDonVi, String LoaiBieu, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_74_4_Mau1.xls";
            }
            else
            {

                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_74_4_Mau2.xls";
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, LoaiBieu, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "baocaoquyettoanChiCacCheDoBHXH.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }

        public ActionResult ViewPDF(String MaND, String iID_MaDonVi, String LoaiBieu, String iID_MaTrangThaiDuyet, String BoDongTrong, String KhoGiay)
        {
            String sFilePath = "";
            if (KhoGiay == "1")
            {
                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_74_4_Mau1.xls";
            }
            else
            {

                sFilePath = "/Report_ExcelFrom/BaoHiem/rptBH_Chi_74_4_Mau2.xls";
            }
            HamChung.Language();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaDonVi, LoaiBieu, iID_MaTrangThaiDuyet, BoDongTrong, KhoGiay);
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
        [HttpGet]

        public JsonResult ds_DonVi(String ParentID, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            return Json(obj_DonViTheoLNS(ParentID, MaND, iID_MaDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
        }

        public String obj_DonViTheoLNS(String ParentID, String MaND, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
        {
            DataTable dtDonvi = HienThiDonViTheoNam(MaND, iID_MaTrangThaiDuyet);
            SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTenDonVi");
            String strLNS = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 70%\"");
            return strLNS;

        }


        public static DataTable HienThiDonViTheoNam(String MaND, String iID_MaTrangThaiDuyet)
        {
            DataTable dt = new DataTable();
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeBaoHiem) + "'";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }

            String SQL = string.Format(@"SELECT DISTINCT 
                                         iID_MaDonVi,sTenDonVi 
                                         FROM BH_ChungTuChiChiTiet
                                         WHERE iTrangThai=1 {1} AND bChi=1  {0} AND sLNS<>''
                                         ORDER BY iID_MaDonVi", iID_MaTrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND));
            SqlCommand cmd = new SqlCommand(SQL);

            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                DataRow R = dt.NewRow();
                R["iID_MaDonVi"] = "";
                R["sTenDonVi"] = "Không có đơn vị";
                dt.Rows.InsertAt(R, 0);
            }
            return dt;
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
    }
}
