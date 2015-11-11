using System;
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
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using VIETTEL.Controllers;
using System.IO;
namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDA_03_CP_1Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_03_CP_1.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_03_CP_1.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDotCapPhat = Convert.ToString(Request.Form[ParentID + "_viiID_MaDotCapPhat"]);
            String MaTien = Convert.ToString(Request.Form[ParentID + "_MaTien"]);
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["MaTien"] = MaTien;
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_03_CP_1.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public static DataTable dt_rptQLDA_03_CP_1(String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            String dNgayLap = "01/01/2000";
            //for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            //{
            //    if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
            //    {
            //        dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
            //        break;
            //    }
            //}
            dNgayLap = iID_MaDotCapPhat;
            String dNam = "";
            if (dNgayLap != "01/01/2000")
                dNam = dNgayLap.Substring(6, 4);
           // dtDotCapPhat.Dispose();
           
            String NamLamViec = "2000";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            if (dtCauHinh.Rows.Count > 0)
            {
                NamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            String DK_KHV_NgoaiTe_rSoTienDauNam = "";
            String DK_KHV_NgoaiTe_rSoTienDieuChinh = "";
            String DK_KHV_LoaiNgoaiTe = "";
            String DK_CP_NgoaiTe_TamUng = "";
            String DK_CP_NgoaiTe_ThuTamUng = "";
            String DK_CP_NgoaiTe_ThanhToan = "";
            String DK_CP_LoaiNgoaiTe = "";
            if (MaTien == "0")
            {
                DK_KHV_NgoaiTe_rSoTienDauNam = "rSoTienDauNam/1000000";
                DK_KHV_NgoaiTe_rSoTienDieuChinh = "rSoTienDieuChinh/1000000";
                DK_CP_NgoaiTe_TamUng = "rDeNghiPheDuyetTamUng/1000000";
                DK_CP_NgoaiTe_ThuTamUng = "rDeNghiPheDuyetThuTamUng/1000000";
                DK_CP_NgoaiTe_ThanhToan = "rDeNghiPheDuyetThanhToan/1000000";
                
            }
            else
            {
                DK_KHV_NgoaiTe_rSoTienDauNam = "rNgoaiTe_SoTienDauNam";
                DK_KHV_NgoaiTe_rSoTienDieuChinh = "rNgoaiTe_SoTienDieuChinh";
                DK_CP_NgoaiTe_TamUng = "rNgoaiTe_DeNghiPheDuyetTamUng";
                DK_CP_NgoaiTe_ThuTamUng = "rNgoaiTe_DeNghiPheDuyetThuTamUng";
                DK_CP_NgoaiTe_ThanhToan = "rNgoaiTe_DeNghiPheDuyetThanhToan";
                DK_KHV_LoaiNgoaiTe = " (iID_MaNgoaiTe_SoTienDauNam=@iID_MaNgoaiTe OR iID_MaNgoaiTe_SoTienDieuChinh=@iID_MaNgoaiTe ) AND ";
                DK_CP_LoaiNgoaiTe = " (iID_MaNgoaiTe_DeNghiPheDuyetTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThuTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThanhToan=@iID_MaNgoaiTe) AND ";
            }
            #region Tạo datatable kế hoạch vốn
            String SQLKHV = String.Format(@"SELECT	iID_MaDanhMucDuAn,
	                                        sDeAn,
	                                        sDuAn,
	                                        sDuAnThanhPhan,
	                                        sCongTrinh,
	                                        sHangMucCongTrinh,
                                            sHangMucChiTiet,
	                                        SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	                                        SUBSTRING(sLNS,1,1) as NguonNS,sLNS,
                                             rKeHoachVonNamTruoc=SUM(CASE WHEN iNamLamViec<=@iNamTruoc THEN {0}+{1} ELSE 0 END),
                                              rSoTienDauNam=SUM(CASE WHEN iNamLamViec=@iNamLamViec THEN {0} ELSE 0 END),
                                             rSoTienDieuChinh=SUM(CASE WHEN iNamLamViec=@iNamLamViec THEN {1} ELSE 0 END)
                                            FROM QLDA_KeHoachVon
                                            WHERE 
										          iTrangThai=1 AND {2}
                                                  dNgayKeHoachVon<=@dNgay  AND iLoaiKeHoachVon=1
                                            GROUP BY   iID_MaDanhMucDuAn,
											           sDeAn,
	                                                   sDuAn,
	                                                   sDuAnThanhPhan,
	                                                   sCongTrinh,
	                                                   sHangMucCongTrinh,
                                                       sHangMucChiTiet,
	                                                   SUBSTRING(sTenDuAn,19,10000),
	                                                   SUBSTRING(sLNS,1,1),sLNS
                                            HAVING SUM({0})<>0 OR SUM({1}) <>0 OR SUM(CASE WHEN iNamLamViec<=@iNamTruoc THEN {0}+{1} ELSE 0 END) <>0 
", DK_KHV_NgoaiTe_rSoTienDauNam,DK_KHV_NgoaiTe_rSoTienDieuChinh,DK_KHV_LoaiNgoaiTe);
            SqlCommand cmdKHV = new SqlCommand(SQLKHV);
            cmdKHV.Parameters.AddWithValue("@iNamLamViec", dNam);
            cmdKHV.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt16(dNam) - 1);

            cmdKHV.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgayLap));
            cmdKHV.Parameters.AddWithValue("@dNam", dNam);
            if (MaTien != "0")
            {
                cmdKHV.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dtKHV = Connection.GetDataTable(cmdKHV);
            cmdKHV.Dispose();
            #endregion
            #region tạo dt cấp phát

            String SQL = String.Format(@"
                                       SELECT * FROM(
                                                      SELECT iID_MaDanhMucDuAn,
			                                                 SUBSTRING(sLNS,1,1) as NguonNS,sLNS,
			                                                 -- cap phat Nam nay iLoai=1
			                                                 rDeNghiPheDuyetThanhToan_NamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {2} ELSE 0 END),
			                                                 rDeNghiPheDuyetThanhToan_NamTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {2} ELSE 0 END),
			                                                rDeNghiPheDuyetTongThanhToan_NamTruoc=SUM(CASE WHEN (iNamLamViec<@iNamLamViec AND iID_MaLoaiKeHoachVon IN(1,3)) THEN {2} ELSE 0 END),
    
															rDeNghiPheDuyetTamUng_NamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {0} ELSE 0 END),
															rDeNghiPheDuyetTamUng_NamTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {0} ELSE 0 END),
															
															rDeNghiPheDuyetThuTamUng_NamNay=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {1} ELSE 0 END),
															rDeNghiPheDuyetThuTamUng_NamTruoc=SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {1} ELSE 0 END),
			                                               rSoDuTamUng_NamTruoc=SUM(CASE WHEN (iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1,3)) THEN   {0}-{1} ELSE 0 END)
             FROM QLDA_CapPhat
             WHERE iTrangThai=1 AND {3}
                   iID_MaDotCapPhat IN(SELECT iID_MaDotCapPhat FROM QLDA_CapPhat_Dot WHERE iTrangThai=1 AND dNgayLap<=@dNgayLap )
GROUP BY 
					iID_MaDanhMucDuAn
					,SUBSTRING(sLNS,1,1),sLNS             
HAVING                                                         
SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {2} ELSE 0 END) <> 0
			                                                 OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {2} ELSE 0 END) <>0
			                                                OR SUM(CASE WHEN (iNamLamViec<@iNamLamViec AND iID_MaLoaiKeHoachVon IN(1,3)) THEN {2} ELSE 0 END)<>0
    
															 OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {0} ELSE 0 END)<>0
															 OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {0} ELSE 0 END)<>0
															
															OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=1 THEN {1} ELSE 0 END)<>0
															OR SUM(CASE WHEN iNamLamViec=@iNamLamViec AND iID_MaLoaiKeHoachVon=3 THEN {1} ELSE 0 END)<>0
			                                               OR SUM(CASE WHEN (iNamLamViec<=@iNamTruoc AND iID_MaLoaiKeHoachVon IN(1,3)) THEN   {0}-{1} ELSE 0 END)  <>0           

           ) as CP
            INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                                                                                       sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTienDo 
                                                                                FROM QLDA_DanhMucDuAn 
                                                                                WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as DM
                                        ON CP.iID_MaDanhMucDuAn=DM.iID_MaDanhMucDuAn
            ", DK_CP_NgoaiTe_TamUng, DK_CP_NgoaiTe_ThuTamUng, DK_CP_NgoaiTe_ThanhToan, DK_CP_LoaiNgoaiTe);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", dNam);
            cmd.Parameters.AddWithValue("@iNamTruoc", Convert.ToInt16(dNam) - 1);
            cmd.Parameters.AddWithValue("@dNgayLap", CommonFunction.LayNgayTuXau(dNgayLap));
            if (MaTien != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
#endregion
            #region Ghép dtKHV vào dtCapPhat
            DataRow addR, R2;
            String sCol = "iID_MaDanhMucDuAn,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,NguonNS,sLNS,sTenDuAn,sHangMucChiTiet,rKeHoachVonNamTruoc,rSoTienDauNam,rSoTienDieuChinh";
            String[] arrCol = sCol.Split(',');
             dt.Columns.Add("rKeHoachVonNamTruoc", typeof(Decimal));
             dt.Columns.Add("rSoTienDauNam", typeof(Decimal));
             dt.Columns.Add("rSoTienDieuChinh", typeof(Decimal));
            for (int i = 0; i < dtKHV.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"iID_MaDanhMucDuAn='{0}' AND sDeAn='{1}' AND sDuAn='{2}' AND sDuAnThanhPhan='{3}' AND sCongTrinh='{4}' AND sHangMucCongTrinh='{5}'
                                                    AND NguonNS='{6}' AND sLNS='{7}'",
                                                  dtKHV.Rows[i]["iID_MaDanhMucDuAn"], dtKHV.Rows[i]["sDeAn"], dtKHV.Rows[i]["sDuAn"], dtKHV.Rows[i]["sDuAnThanhPhan"], dtKHV.Rows[i]["sCongTrinh"],dtKHV.Rows[i]["sHangMucCongTrinh"],
                                                  dtKHV.Rows[i]["NguonNS"], dtKHV.Rows[i]["sLNS"]);
                DataRow[] R = dt.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    addR = dt.NewRow();
                    for (int j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dtKHV.Rows[i][arrCol[j]];
                    }
                    dt.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dtKHV.Rows)
                    {

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dt.Rows[j];

                            for (int c = 0; c < arrCol.Length - 3; c++)
                            {
                                if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dt.Rows[j]["rKeHoachVonNamTruoc"] = R1["rKeHoachVonNamTruoc"];
                                dt.Rows[j]["rSoTienDauNam"] = R1["rSoTienDauNam"];
                                dt.Rows[j]["rSoTienDieuChinh"] = R1["rSoTienDieuChinh"];
                                break;
                            }
                        }
                    }
                }
            }
            //sắp xếp datatable sau khi ghép
            DataView dv = dt.DefaultView;
            dv.Sort = "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet";
            dt = dv.ToTable();
            #endregion         
            return dt;
        }
        public ActionResult ViewPDF(String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDotCapPhat, MaND,MaTien);
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
        public ExcelFile CreateReport(String path, String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            String dNgayLap = "01/01/2000";
            //for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            //{
            //    if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
            //    {
            //        dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
            //        break;
            //    }
            //}
            //dtDotCapPhat.Dispose();
            dNgayLap = iID_MaDotCapPhat;
            String DotCapPhat = " Đến ngày " + dNgayLap.Substring(0, 2) + " tháng " + dNgayLap.Substring(3, 2) + " năm " + dNgayLap.Substring(6, 4);
            String nam = dNgayLap.Substring(6, 4);
            DataTable dtDVT = QLDA_ReportModel.dt_LoaiTien_CP_03(dNgayLap, MaND);
            String DVT = " triệu đồng";
            for (int i = 1; i < dtDVT.Rows.Count; i++)
            {
                if (MaTien == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTenNgoaiTe"].ToString();
                    break;
                }

            }
            dtDVT.Dispose();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_03_CP_1");
            LoadData(fr, iID_MaDotCapPhat, MaND,MaTien);
            fr.SetValue("DVT", DVT);
            fr.SetValue("DotCapPhat", DotCapPhat);
            fr.SetValue("Nam", nam);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.Run(Result);
            return Result;
        }
        private void LoadData(FlexCelReport fr, String iID_MaDotCapPhat, String MaND,String MaTien)
        {

            DataTable data = dt_rptQLDA_03_CP_1(iID_MaDotCapPhat, MaND, MaTien);
            data.TableName = "ChiTiet";
            //Hạng mục chi tiết
            DataTable dtHangMucChiTiet = HamChung.SelectDistinct_QLDA("HMChiTiet", data, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sTienDo");
            // Hạng mục công trình
            DataTable dtHangMucCongTrinh = HamChung.SelectDistinct_QLDA("HMCT", dtHangMucChiTiet, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            //Công trình
            DataTable dtCongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", dtHangMucCongTrinh, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            //Dự án thành phần
            DataTable dtDuAnThanhPhan = HamChung.SelectDistinct_QLDA("DATP", dtCongTrinh, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            //Dự án
            DataTable dtDuAn = HamChung.SelectDistinct_QLDA("DuAn", dtDuAnThanhPhan, "NguonNS,sLNS,sDeAn,sDuAn", "NguonNS,sLNS,sDeAn,sDuAn,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan");
            //Đề án
            DataTable dtDeAn = HamChung.SelectDistinct_QLDA("DeAn", dtDuAn, "NguonNS,sLNS,sDeAn", "NguonNS,sLNS,sDeAn,sTenDuAn,sTienDo", "sDeAn,sDuAn");
            //sLNS
            DataTable dtLNS = HamChung.SelectDistinct("sLNS", dtDeAn, "NguonNS,sLNS", "NguonNS,sLNS,sTenDuAn", "", "");
            //Nguồn
            DataTable dtNguon = HamChung.SelectDistinct("Nguon", dtDeAn, "NguonNS", "NguonNS,sTenDuAn", "", "NguonNS");

            //Thêm tên Loại ngân sách của dtLNS
            for (int i = 0; i < dtLNS.Rows.Count; i++)
            {
                String sLNS = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                DataRow r = dtLNS.Rows[i];
                String SQL = "SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND sLNS=@sLNS";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                String sMoTa = "";
                sMoTa = Connection.GetValueString(cmd, "");
                r["sTenDuAn"] = sMoTa;
            }

            data.TableName = "Chitiet";
            fr.AddTable("Chitiet", data);
            fr.AddTable("HMCT", dtHangMucCongTrinh);
            fr.AddTable("CongTrinh", dtCongTrinh);
            fr.AddTable("DATP", dtDuAnThanhPhan);
            fr.AddTable("DuAn", dtDuAn);
            fr.AddTable("DeAn", dtDeAn);
            fr.AddTable("sLNS", dtLNS);
            fr.AddTable("Nguon", dtNguon);
            dtDeAn.Dispose();
            dtDuAn.Dispose();
            dtDuAnThanhPhan.Dispose();
            dtCongTrinh.Dispose();
            dtNguon.Dispose();
            data.Dispose();
        }
        public JsonResult ds_QLDA(String ParentID, String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            return Json(obj_QLDA(ParentID, iID_MaDotCapPhat, MaND, MaTien), JsonRequestBehavior.AllowGet);
        }
        public String obj_QLDA(String ParentID, String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            String dNgayLap = "01/01/2000";
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            {
                if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
                {
                    dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
                    break;
                }
            }
            dtDotCapPhat.Dispose();
            DataTable dtNgoaiTe = QLDA_ReportModel.dt_LoaiTien_CP_03(dNgayLap, MaND);
            SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTenNgoaiTe");
            String NgoaiTe = MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, MaTien, "MaTien", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtNgoaiTe.Dispose();
            return NgoaiTe;
        }
    }
}
