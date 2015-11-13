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
namespace VIETTEL.Report_Controllers.PhanBo
{
    public class rptPhanBo_4_2Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/PhanBo/rptPhanBo_4_2.xls";
        private const String sFilePath_TNG = "/Report_ExcelFrom/PhanBo/rptPhanBo_4_2_TNG.xls";
        private const String sFilePath_Ngang = "/Report_ExcelFrom/PhanBo/rptPhanBo_4_2_Ngang.xls";
        private const String sFilePath_Ngang_TNG = "/Report_ExcelFrom/PhanBo/rptPhanBo_4_2_Ngang_TNG.xls";
        public String PageLoad = "0";
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                PageLoad = "0";
                ViewData["PageLoad"] = PageLoad;
                ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_4_2.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
       /// <summary>
        /// EditSubmit
       /// </summary>
       /// <param name="ParentID"></param>
       /// <returns></returns>
        public ActionResult EditSubmit(String ParentID, int ChiSo)
        {
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];           
            String iID_MaDotPhanBo = Request.Form[ParentID + "_iID_MaDotPhanBo"];
            String iID_MaDonVi = Request.Form["iID_MaDonVi"];
            String sLNS = Request.Form["sLNS"];
            String MaKieu = Request.Form[ParentID + "_MaKieu"];
            String iThongBao = Request.Form[ParentID + "_iThongBao"];
            String sMuc = Request.Form[ParentID + "_sMuc"];
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["sLNS"] = sLNS;
            ViewData["MaKieu"] = MaKieu;
            UpdateLNS("rptPhanBo_4_2", sLNS);
            ViewData["iID_MaDotPhanBo"] = iID_MaDotPhanBo;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iThongBao"] = iThongBao;
            ViewData["sMuc"] = sMuc;
            ViewData["path"] = "~/Report_Views/PhanBo/rptPhanBo_4_2.aspx";
            PageLoad = "1";
            ViewData["PageLoad"] = PageLoad;
            return View(sViewPath + "ReportView.aspx");                      

        }
        String sLNS = LayLNS("rptPhanBo_4_2");
        /// <summary>
        /// Khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="pageload"></param>
        /// <param name="MaKieu"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String iID_MaDotPhanBo, String iID_MaDonVi, String MaKieu, String iID_MaTrangThaiDuyet,String iThongBao,String sMuc)
        {
            String MaND = User.Identity.Name;
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String DotBoSung = "";
            String dotTruoc="";
            String TenDot = "";
         DataTable dtDotPhanBo=PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet,sLNS);
            for (int i=1;i<dtDotPhanBo.Rows.Count;i++)
            {
                if(iID_MaDotPhanBo==dtDotPhanBo.Rows[i-1]["iID_MaDotPhanBo"].ToString())
                {
                    dotTruoc=dtDotPhanBo.Rows[i-1]["dNgayDotPhanBo"].ToString();
                }
                break;
            }
            if (ReportModels.Get_STTDotPhanBo(MaND,iID_MaTrangThaiDuyet, iID_MaDotPhanBo) >1)
            {
                DotBoSung = "Bổ sung: Đợt  " + (ReportModels.Get_STTDotPhanBo(MaND,iID_MaTrangThaiDuyet, iID_MaDotPhanBo) - 1) + dotTruoc;
            }
            for (int i = 2; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    TenDot = dtDotPhanBo.Rows[i]["dNgayDotPhanBo"].ToString();
                    if (!String.IsNullOrEmpty(TenDot))
                    {
                        TenDot = " - Tháng " + TenDot.Substring(3, 2) + " - Năm " + TenDot.Substring(6, 4);
                    }
                }
            }
             //lấy ngày hiện tại
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String QuanKhu = ReportModels.CauHinhTenDonViSuDung(2);
            String TenDV = "";
            if (!String.IsNullOrEmpty(iID_MaDonVi) && iID_MaDonVi != "00000000-0000-0000-0000-000000000000")
            {
                TenDV=iID_MaDonVi + "-" + DonViModels.Get_TenDonVi(iID_MaDonVi);
            }
            #region LoadData
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptPhanBo_4_2");
            
            DataTable data = Get_ThongBaoChiTieuNganSachNam(MaND,iID_MaTrangThaiDuyet, sLNS, iID_MaDotPhanBo, iID_MaDonVi,iThongBao,iThongBao);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtTieuMuc;
            if (sMuc == "sTNG")
            {
                DataTable dtNganh;
                dtNganh = HamChung.SelectDistinct("Nganh", data, "sNNS,sLNS,sL,sK,sM,sTM,sNG", "sNNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM,sNG");
                fr.AddTable("Nganh", dtNganh);
              
                dtTieuMuc = HamChung.SelectDistinct("TieuMuc", dtNganh, "sNNS,sLNS,sL,sK,sM,sTM", "sNNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
                fr.AddTable("TieuMuc", dtTieuMuc);
                dtNganh.Dispose();
            }
            else
            {
                dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sNNS,sLNS,sL,sK,sM,sTM", "sNNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
                fr.AddTable("TieuMuc", dtTieuMuc);
              
            }


            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sNNS,sLNS,sL,sK,sM", "sNNS,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);

            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("LoaiNS", dtMuc, "sNNS,sLNS", "sNNS,sLNS,sMoTa", "sLNS,sL");
            fr.AddTable("LoaiNS", dtLoaiNS);

            DataTable dtNguonNS;
            dtNguonNS = HamChung.SelectDistinct("NguonNS", dtMuc, "sNNS", "sNNS,sMoTa", "sLNS,sL", "sNNS");
            fr.AddTable("NguonNS", dtNguonNS);

            data.Dispose();
            dtTieuMuc.Dispose();
            dtMuc.Dispose();
            dtLoaiNS.Dispose();
            dtNguonNS.Dispose();
            #endregion
            //tính tổng tiền
            //DataTable dt = QT_ThuongXuyen_25_5(NamLamViec, ThangLamViec, iID_MaDonVi);
            long TongTien = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["DotNay"].ToString() != "")
                {
                    TongTien += long.Parse(data.Rows[i]["DotNay"].ToString());
                }
            }
            String Tien = "";
            Tien = CommonFunction.TienRaChu(TongTien).ToString();
            //set các thông số
         
               
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
                fr.SetValue("Dot", ReportModels.Get_STTDotPhanBo(MaND,iID_MaTrangThaiDuyet,iID_MaDotPhanBo));
                fr.SetValue("DonVi", TenDV);
                fr.SetValue("DotBoSung", DotBoSung);
                fr.SetValue("NgayThangNam", NgayThangNam);
                fr.SetValue("BoQuocPhong", BoQuocPhong);
                fr.SetValue("QuanKhu", QuanKhu);
                fr.SetValue("Tien", Tien);
                fr.SetValue("TenDot",TenDot);
                fr.Run(Result);
                return Result;
            }

       /// <summary>
        /// ViewPDF
       /// </summary>
       /// <param name="iID_MaDotPhanBo"></param>
       /// <param name="iID_MaDonVi"></param>
       /// <param name="MaKieu"></param>
       /// <param name="iID_MaTrangThaiDuyet"></param>
       /// <param name="iThongBao"></param>
       /// <param name="sMuc"></param>
       /// <returns></returns>
        public ActionResult ViewPDF(String iID_MaDotPhanBo, String iID_MaDonVi,String MaKieu, String iID_MaTrangThaiDuyet,String iThongBao,String sMuc)
        {
            HamChung.Language();
            String Duongdan = "";
            if (MaKieu == "1")
                if (sMuc == "sNG")
                    Duongdan = sFilePath;
                else
                    Duongdan = sFilePath_TNG;
            else
                if (sMuc == "sNG")
                    Duongdan = sFilePath_Ngang;
                else
                    Duongdan = sFilePath_Ngang_TNG;
            ExcelFile xls = CreateReport(Server.MapPath(Duongdan), iID_MaDotPhanBo, iID_MaDonVi, MaKieu, iID_MaTrangThaiDuyet, iThongBao,sMuc);
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
        public clsExcelResult ExportToExcel(String iID_MaDotPhanBo, String iID_MaDonVi, String MaKieu, String iID_MaTrangThaiDuyet, String iThongBao, String sMuc)
        {
            HamChung.Language();
            String Duongdan = "";
            if (MaKieu == "1")
                if (sMuc == "sNG")
                    Duongdan = sFilePath;
                else
                    Duongdan = sFilePath_TNG;
            else
                if (sMuc == "sNG")
                    Duongdan = sFilePath_Ngang;
                else
                    Duongdan = sFilePath_Ngang_TNG;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(Duongdan), iID_MaDotPhanBo, iID_MaDonVi, MaKieu, iID_MaTrangThaiDuyet, iThongBao, sMuc);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptPhanBo_4_2.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        public class LNSdata
        {
            public string iID_MaDotPhanBo { get; set; }
            public string iID_MaDonVi { get; set; }
        }

        /// <summary>
        /// onchange
        /// </summary>
        /// <param name="ParentID"></param>
        /// <param name="sLNS"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public JsonResult ds_DotPhanBo(String ParentID, String sLNS, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo, String iID_MaDonVi,String iThongBao)
        {
            LNSdata _LNSdata = new LNSdata();
            #region Đợt phân bổ
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet, sLNS);
            SelectOptionList slPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            _LNSdata.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\" Onchange=\"ChonDV()\"");
            dtDotPhanBo.Dispose();
            #endregion
            #region Danh sách đơn vị
            DataTable dtDonVi = PhanBo_ReportModels.DanhSachDonVi2(MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDotPhanBo, false, true, iThongBao,false);
            String ViewNam = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
            DanhSachDonViModels Model = new DanhSachDonViModels(MaND, iID_MaDonVi, dtDonVi, "rptPhanBo_4_2", 1);
            String strDonVi = HamChung.RenderPartialViewToStringLoad(ViewNam, Model, this);
            _LNSdata.iID_MaDonVi = strDonVi;
            #endregion
            return Json(_LNSdata, JsonRequestBehavior.AllowGet);
        }
        public LNSdata obj_DotPhanBo(String ParentID, String sLNS, String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo, String iID_MaDonVi,String iThongBao)
        {
            LNSdata _LNSdata = new LNSdata();
            #region Đợt phân bổ
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND,iID_MaTrangThaiDuyet, sLNS);
            SelectOptionList slPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
            _LNSdata.iID_MaDotPhanBo = MyHtmlHelper.DropDownList(ParentID, slPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 100%\" Onchange=\"ChonDV()\"");
            dtDotPhanBo.Dispose();
            #endregion
            #region Danh sách đơn vị
            DataTable dtDonVi = PhanBo_ReportModels.DanhSachDonVi2(MaND,iID_MaTrangThaiDuyet, sLNS,iID_MaDotPhanBo,false,true,iThongBao);
            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
            _LNSdata.iID_MaDonVi = MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"");
            #endregion
            return _LNSdata;
        }
        /// <summary>
        /// Lấy LNS
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <returns></returns>
        public static String LayLNS(String ControllerName)
        {
            String SQL = String.Format(@"SELECT sKyHieu,sThamSo FROM DC_ThamSo WHERE sKyHieu=@sKyHieu");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sKyHieu", ControllerName);
            DataTable dt = Connection.GetDataTable(cmd);
            String sLNS = "";
            if (dt.Rows.Count > 0 && dt!=null)
            {
                sLNS = dt.Rows[0]["sThamSo"].ToString();
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
            return sLNS;
        }
        /// <summary>
        /// cập nhật LNS
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="sLNS"></param>
        public void UpdateLNS(String ControllerName,String sLNS)
        {
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            String SQL = String.Format(@"UPDATE DC_ThamSo SET sThamSo=@sLNS  WHERE sKyHieu=@ControllerName");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            cmd.Parameters.AddWithValue("@ControllerName", ControllerName);
            int update=Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }
        /// <summary>
        /// Get_ThongBaoChiTieuNganSachNam
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDotPhanBo"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable Get_ThongBaoChiTieuNganSachNam(String MaND, String iID_MaTrangThaiDuyet, String sLNS, String iID_MaDotPhanBo, String iID_MaDonVi,String iThongBao,String sMuc)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND,iID_MaTrangThaiDuyet, sLNS);
            String DKDotPhanBoTruoc = "";
            if (dtDotPhanBo.Rows.Count > 2)
            {
                for (int i = 2; i < dtDotPhanBo.Rows.Count; i++)
                {
                    if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                    {
                        for (int j = 1; j < i; j++)
                        {
                            DKDotPhanBoTruoc += "IID_MaDotPhanBo=@IID_MaDotPhanBo" + j;
                            if (j < i - 1)
                                DKDotPhanBoTruoc += " OR ";
                        }
                        break;
                    }
                }
            }
            else
            {
                DKDotPhanBoTruoc = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";
            }
            if (String.IsNullOrEmpty(DKDotPhanBoTruoc))
            {
                DKDotPhanBoTruoc = "iID_MaDotPhanBo='" + Guid.Empty.ToString() + "'";

            }
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            String DKLNS = "";
            String[] arrLNS = sLNS.Split(',');
            for (int i = 0; i < arrLNS.Length;i++)
            {
                DKLNS += "sLNS=@sLNS" + i;
                if (i < arrLNS.Length - 1)
                    DKLNS += " OR ";
            }
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            // DK 1-Cap 2-Thu
            String DkThongBao = "", DkThongBao_1 = "";
            if (iThongBao == "1")
            {
                DkThongBao = "rTuChi>0 AND ";
                DkThongBao_1 = "rTuChi";
            }
            else
            {
                DkThongBao = "rTuChi<0 AND ";
                DkThongBao_1 = "-rTuChi";
            }
            String SQL = "";
            //neu in den nganh
            if (sMuc == "sNG")
            {
                SQL = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as sNNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,SUM(LKDotTruoc) as LKDotTruoc,SUM(DotNay) as DotNay
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as sNNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        ,LKDotTruoc=CASE WHEN ({0}) THEN SUM({5}) ELSE 0 END
                                        ,DotNay=CASE WHEN iID_MaDotPhanBo=@iID_MaDotPhanBo THEN SUM({5}) ELSE 0 END
                                        FROM PB_PhanBoChiTiet
                                        WHERE {4} iID_MaDonVi=@iID_MaDonVi AND iTrangThai=1 AND sNG<>'' AND ({1}) {2} {3}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDotPhanBo
                                        HAVING SUM({5})<>0) a
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
                                        HAVING SUM(LKDotTruoc)<>0 OR SUM(DotNay)<>0", DKDotPhanBoTruoc, DKLNS, ReportModels.DieuKien_NganSach(MaND), DK_Duyet, DkThongBao, DkThongBao_1);
            }
             // in tieu nganh
            else
            {
                 SQL = String.Format(@"SELECT SUBSTRING(sLNS,1,1) as sNNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,SUM(LKDotTruoc) as LKDotTruoc,SUM(DotNay) as DotNay
                                        FROM(
                                        SELECT SUBSTRING(sLNS,1,1) as sNNS,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        ,LKDotTruoc=CASE WHEN ({0}) THEN SUM({5}) ELSE 0 END
                                        ,DotNay=CASE WHEN iID_MaDotPhanBo=@iID_MaDotPhanBo THEN SUM({5}) ELSE 0 END
                                        FROM PB_PhanBoChiTiet
                                        WHERE {4} iID_MaDonVi=@iID_MaDonVi AND iTrangThai=1 AND sTNG<>'' AND ({1}) {2} {3}
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,iID_MaDotPhanBo
                                        HAVING SUM({5})<>0) a
                                        GROUP BY SUBSTRING(sLNS,1,1),sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                                        HAVING SUM(LKDotTruoc)<>0 OR SUM(DotNay)<>0", DKDotPhanBoTruoc, DKLNS, ReportModels.DieuKien_NganSach(MaND), DK_Duyet, DkThongBao, DkThongBao_1);
            }
                cmd.CommandText = SQL;
            for (int i = 0; i < arrLNS.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sLNS" + i, arrLNS[i]);
            }
            
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            for (int i = 2; i < dtDotPhanBo.Rows.Count; i++)
            {
                if (iID_MaDotPhanBo == dtDotPhanBo.Rows[i]["iID_MaDotPhanBo"].ToString())
                {
                    for (int j = 1; j < i; j++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDotPhanBo" + j, dtDotPhanBo.Rows[j]["iID_MaDotPhanBo"].ToString());
                    }
                    break;
                }
            
            }  
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }                   
    }
}
