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
using System.Text;

namespace VIETTEL.Report_Controllers.NguoiCoCong
{
    public class rptNCC_ThongTriTongHop_2Controller : Controller
    {
        //
        // GET: /rptNCC_ThongTriTongHop_2/

        public string sViewPath = "~/Report_Views/";
        public static String NameFile= ""; 
        public ActionResult Index(String Loai = "", String Kieu = "")
        {
            String sFilePath = "";
            if (Loai == "1" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_2.xls";
            }
            else if (Loai == "1" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_2_Loai1_Kieu2.xls";
            }
            else if (Loai == "2" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_2.xls";
            }
            else if (Loai == "2" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_2.xls";
            }
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/NguoiCoCong/rptNCC_ThongTriTongHop_2.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// Action lấy các giá trị trên form khi thực hiện submit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
           
            String Thang_Quy = "";
            String LoaiThangQuy = Convert.ToString(Request.Form[ParentID + "_LoaiThangQuy"]);
            if (LoaiThangQuy == "1")
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            }
            else
            {
                Thang_Quy = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            }
            String sLNS = Convert.ToString(Request.Form[ParentID + "_sLNS"]);
            String iID_MaDonVi = Convert.ToString(Request.Form[ParentID + "_iID_MaDonVi"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String Loai = Convert.ToString(Request.Form[ParentID + "_Loai"]);
            String Kieu = Convert.ToString(Request.Form[ParentID + "_Kieu"]);
            ViewData["Thang_Quy"] = Thang_Quy;
            ViewData["LoaiThangQuy"] = LoaiThangQuy;
            ViewData["sLNS"] = sLNS;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["Loai"] = Loai;
            ViewData["Kieu"] = Kieu;
            ViewData["path"] = "~/Report_Views/NguoiCoCong/rptNCC_ThongTriTongHop_2.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public long Tong = 0;
        /// <summary>
        /// hàm lấy dữ liệu cho báo cáo
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public DataTable rptNCC_TongHop2(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String Loai, String Kieu)
        {
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(LoaiThangQuy))
            {
                LoaiThangQuy = "1";
            }
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(Thang_Quy))
            {
                Thang_Quy = "1";
            }
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            String[] arrDonvi = iID_MaDonVi.Split(',');
            String DK = "";
            for (int i = 0; i < arrDonvi.Length; i++)
            {
                DK += "DV.iID_MaDonVi=@iID_MaDonVi" + i;
                if (i < arrDonvi.Length - 1)
                    DK += " OR ";
            }
            // Mảng Loại Ngân Sách
           
            String TrangThaiDuyet = "";
            if (iID_MaTrangThaiDuyet == "0")
            {
                TrangThaiDuyet = "iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong) + "'";
            }
            else
            {
                TrangThaiDuyet = "1=1";
            }

            String DK_Thang = "";
            if (LoaiThangQuy == "1")
            {
                switch (Thang_Quy)
                {
                    case "1": DK_Thang = "iThang_Quy IN (1,2,3)";
                        break;
                    case "2": DK_Thang = "iThang_Quy IN (4,5,6)";
                        break;
                    case "3": DK_Thang = "iThang_Quy IN (7,8,9)";
                        break;
                    case "4": DK_Thang = "iThang_Quy IN (10,11,12)";
                        break;
                }
            }
            else
            {
                DK_Thang = "iThang_Quy=@Thang_Quy";
            }
            if (Loai == "1" && Kieu == "1")
            {  
                #region tổng hợp đơn vị và chi tiết
                String SQL = " SELECT b.iID_MaDonVi,b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa,SUM(rTuChi) as  rTuChi";
                SQL += " ,Tong=SUM(rTuChi)";
                SQL += " FROM ((SELECT iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,rTuChi";
                SQL += " FROM NCC_ChungTuChiTiet";
                SQL += " WHERE sLNS=@sLNS  AND sNG<>'' AND {2} {1}  AND iLoai=1 AND iTrangThai=1 AND {0}  ";
                SQL += " GROUP BY iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,rTuChi";
                SQL += " HAVING SUM(rTuChi)!=0";
                SQL += " ) a";
                SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iID_MaDonVi=@iID_MaDonVi AND iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b ON a.iID_MaDonVi=b.iID_MaDonVi)";
                SQL += " GROUP BY b.iID_MaDonVi,b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa";
                SQL = string.Format(SQL, TrangThaiDuyet, ReportModels.DieuKien_NganSach(MaND),DK_Thang);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@Thang_Quy", Thang_Quy);
                }
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["Tong"].ToString());
                }
               
                cmd.Dispose();
                #endregion
            }
          
            if (Loai == "1" && Kieu == "2")
            {
                #region Tổng hợp đơn vị và tổng hợp
                String SQL = " SELECT b.iID_MaDonVi,b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa,SUM(rTuChi) as  rTuChi";
                SQL += " ,Tong=SUM(rTuChi)";
                SQL += " FROM ((SELECT iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,rTuChi";
                SQL += " FROM NCC_ChungTuChiTiet";
                SQL += " WHERE sLNS=@sLNS AND sL=''  AND {2} {1}  AND iLoai=1 AND iTrangThai=1 AND {0}  ";
                SQL += " GROUP BY iID_MaDonVi,sLNS,sL,sK, sM,sTM,sTTM,sNG,sMoTa,rTuChi";
                SQL += " HAVING SUM(rTuChi)!=0";
                SQL += " ) a";
                SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iID_MaDonVi=@iID_MaDonVi AND iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) b ON a.iID_MaDonVi=b.iID_MaDonVi)";
                SQL += " GROUP BY b.iID_MaDonVi,b.sTen,sLNS,sL,sK, sM,sTM,sTTM,sNG,a.sMoTa";
                SQL = string.Format(SQL, TrangThaiDuyet,ReportModels.DieuKien_NganSach(MaND),DK_Thang);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@Thang_Quy", Thang_Quy);
                }
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["Tong"].ToString());
                }
                int a = dt.Rows.Count;
                if (a < 10 && a > 0)
                {
                    for (int i = 0; i < 11 - a; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                cmd.Dispose();
                #endregion
            }

            if (Loai == "2" && Kieu == "1")
            {
                #region Tổng hợp và chi tiết
                DataTable dtDonVi = DSDonVi(MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaTrangThaiDuyet);
                String DK_DonVi = "";
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DK_DonVi += "NCC.iID_MaDonVi=@iID_MaDonVi" + i;
                        if (i < dtDonVi.Rows.Count - 1)
                            DK_DonVi += " OR ";
                    }
                }
                String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.sTen,SUM(rTuChi) as  SoTien
                            FROM NCC_ChungTuChiTiet as NCC
                            INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON NCC.iID_MaDonVi=DV.iID_MaDonVi
                            WHERE  NCC.iTrangThai=1 {1} AND sNG<>'' AND  {0} AND sLNS=@sLNS AND ({2})
                            AND {3}  AND iLoai=1
                            GROUP BY DV.iID_MaDonVi,DV.sTen", TrangThaiDuyet,ReportModels.DieuKien_NganSach(MaND),DK_DonVi,DK_Thang);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@Thang_Quy", Thang_Quy);
                }
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                    }

                }
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
                }
                int a = dt.Rows.Count;
                if (a < 10 && a > 0)
                {
                    for (int i = 0; i < 11 - a; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                cmd.Dispose();
                #endregion
            }
            if (Loai == "2" && Kieu == "2")
            {
                #region Tổng hợp và tổng hợp
                DataTable dtDonVi = DSDonVi(MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaTrangThaiDuyet);
                String DK_DonVi = "";
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        DK_DonVi += "NCC.iID_MaDonVi=@iID_MaDonVi" + i;
                        if (i < dtDonVi.Rows.Count - 1)
                            DK_DonVi += " OR ";
                    }
                }
                String SQL = String.Format(@"SELECT DV.iID_MaDonVi,DV.sTen,SUM(rTuChi) as  SoTien
                            FROM NCC_ChungTuChiTiet as NCC
                              INNER JOIN (SELECT * FROM NS_DonVi WHERE  iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON NCC.iID_MaDonVi=DV.iID_MaDonVi
                            WHERE  NCC.iTrangThai=1 {1} AND sNG<>'' AND {0} AND sLNS=@sLNS AND ({2})
                            AND {3}  AND iLoai=1
                            GROUP BY DV.iID_MaDonVi,DV.sTen", TrangThaiDuyet,ReportModels.DieuKien_NganSach(MaND),DK_DonVi,DK_Thang);
                SqlCommand cmd = new SqlCommand(SQL);
                if (LoaiThangQuy == "0")
                {
                    cmd.Parameters.AddWithValue("@Thang_Quy", Thang_Quy);
                }
                if (dtDonVi.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, dtDonVi.Rows[i]["iID_MaDonVi"].ToString());
                    }

                }
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                dt = Connection.GetDataTable(cmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Tong += long.Parse(dt.Rows[i]["SoTien"].ToString());
                }
                int a = dt.Rows.Count;
                if (a < 10 && a > 0)
                {
                    for (int i = 0; i < 11 - a; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.InsertAt(dr, a + 1);
                    }
                }
                cmd.Dispose();
                #endregion
            }
            return dt;
        }
        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
       
        public ExcelFile CreateReport(String path, String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet,String Loai,String Kieu)
        {
            
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String LoaiThang_Quy = "";
            switch (LoaiThangQuy)
            {
                case "0":
                    LoaiThang_Quy = "Tháng";
                    break;
                case "1":
                    LoaiThang_Quy = "Quý";
                    break;
                case "2":
                    LoaiThang_Quy = "Năm";
                    break;
            }
            String TenLNS = "";
            DataTable dt =ReportModels.MoTa(sLNS);
            if (dt.Rows.Count > 0)
            {
                TenLNS = dt.Rows[0][0].ToString();
            }
            if (Thang_Quy == Guid.Empty.ToString())
            {
                Thang_Quy = "";
            }
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
            String NgayThang = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptNCC_ThongTriTongHop_2");
            LoadData(fr, MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet,Loai,Kieu);
                fr.SetValue("Nam", ReportModels.LayNamLamViec(MaND));
                fr.SetValue("Thang_Quy", Thang_Quy);
                fr.SetValue("BTL", ReportModels.CauHinhTenDonViSuDung(2));
                fr.SetValue("Cap1",ReportModels.CauHinhTenDonViSuDung(1));
                fr.SetValue("Ngay",NgayThang);
                fr.SetValue("Tien", CommonFunction.TienRaChu(Tong).ToString());
                fr.SetValue("LoaiThangQuy", LoaiThang_Quy);
                fr.SetValue("TenLNS", TenLNS);
                if (teN.Rows.Count < 0)
                {
                    fr.SetValue("TenDV", "");
                }
                else
                {
                    fr.SetValue("TenDV", tendv);
                }
                fr.Run(Result);
                return Result;
            
        }
        /// <summary>
        /// lấy tên đơn vị
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            return dt = Connection.GetDataTable(cmd);
        }
        /// <summary>
        /// Hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        private void LoadData(FlexCelReport fr, String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String Loai, String Kieu)
        {
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
            if (String.IsNullOrEmpty(sLNS))
            {
                sLNS = Guid.Empty.ToString();
            }
            DataTable data = rptNCC_TongHop2(MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, Loai, Kieu);
            if (Loai == "1" && Kieu == "2" && Kieu != "1")
            {
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
                data.Dispose();
            }
            else  if (Loai == "1" && Kieu == "1")
            {
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
                data.Dispose();

                DataTable dtTieuMuc;
                dtTieuMuc = HamChung.SelectDistinct("TieuMuc", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
                fr.AddTable("TieuMuc", dtTieuMuc);

                DataTable dtMuc;
                dtMuc = HamChung.SelectDistinct("Muc", dtTieuMuc, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
                fr.AddTable("Muc", dtMuc);

                DataTable dtLoaiNS;
                dtLoaiNS = HamChung.SelectDistinct("LNS", dtMuc, "sLNS", "sLNS,sL,sK,sMoTa", "sLNS,sL");
                fr.AddTable("LNS", dtLoaiNS);
                data.Dispose();
                dtTieuMuc.Dispose();
                dtLoaiNS.Dispose();
                dtMuc.Dispose();
            }
           
            else if (Loai == "2" && Kieu == "1")
            {
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
            }
            else if (Loai == "2" && Kieu == "2")
            {
                data.TableName = "ChiTiet";
                fr.AddTable("ChiTiet", data);
            }
        }
        /// <summary>
        /// Xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String Loai, String Kieu)
        {

            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            String sFilePath = "";
            if (Loai == "1" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_2.xls";
            }
            else if (Loai == "1" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_2_Loai1_Kieu2.xls";
            }
            else if (Loai == "2" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_2.xls";
            }
            else if (Loai == "2" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_2.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, Loai, Kieu);
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
        /// <summary>
        /// Xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String Loai, String Kieu)
        {
           

            clsExcelResult clsResult = new clsExcelResult();
            HamChung.Language();
            String sFilePath = "";
            if (Loai == "1" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_2.xls";
            }
            else if (Loai == "1" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_2_Loai1_Kieu2.xls";
            }
            else if (Loai == "2" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_2.xls";
            }
            else if (Loai == "2" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_2.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, Loai, Kieu);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "ThongTriTongHopMuc2.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <param name="LoaiThangQuy"></param>
        /// <param name="sLNS"></param>
        /// <param name="Thang_Quy"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet, String Loai, String Kieu)
        {
           
            HamChung.Language();
            String sFilePath = "";
            if (Loai == "1" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_2.xls";
            }
            else if (Loai == "1" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTri_2_Loai1_Kieu2.xls";
            }
            else if (Loai == "2" && Kieu == "1")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_2.xls";
            }
            else if (Loai == "2" && Kieu == "2")
            {
                sFilePath = "/Report_ExcelFrom/NguoiCoCong/rptNCC_ThongTriTongHop_2.xls";
            }
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet, Loai, Kieu);
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

       public JsonResult ds_DonVi(String ParentID, String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
       {
           return Json(obj_DonViTheoNam(ParentID, MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet), JsonRequestBehavior.AllowGet);
       }

       public String obj_DonViTheoNam(String ParentID, String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaDonVi, String iID_MaTrangThaiDuyet)
       {
           //String input = "";
           DataTable dt = DSDonVi(MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaTrangThaiDuyet);

           DataTable dtDonvi = DSDonVi(MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaTrangThaiDuyet);
           SelectOptionList sldonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
           String strDonVi = MyHtmlHelper.DropDownList(ParentID, sldonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 90%\"");
           return strDonVi;
           //StringBuilder stbDonVi = new StringBuilder();
           //stbDonVi.Append("<div style=\"width: 250px; height: 70px; overflow: scroll; border:1px solid #006666;\">");
           //stbDonVi.Append("<table class=\"mGrid\">");
           //stbDonVi.Append("<tr>");
           //stbDonVi.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"Chonall(this.checked)\"></td><td> Chọn tất cả đơn vị </td>");

           //String TenDonVi = "", MaDonVi = "";
           //String[] arrDonVi = iID_MaDonVi.Split(',');
           //String _Checked = "checked=\"checked\"";
           //for (int i = 1; i <= dt.Rows.Count; i++)
           //{
           //    MaDonVi = Convert.ToString(dt.Rows[i - 1]["iID_MaDonVi"]);
           //    TenDonVi = Convert.ToString(dt.Rows[i - 1]["sTen"]);
           //    _Checked = "";
           //    for (int j = 1; j <= arrDonVi.Length; j++)
           //    {
           //        if (MaDonVi == arrDonVi[j - 1])
           //        {
           //            _Checked = "checked=\"checked\"";
           //            break;
           //        }
           //    }

           //    input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" />", MaDonVi, _Checked);
           //    stbDonVi.Append("<tr>");
           //    stbDonVi.Append("<td style=\"width: 15%;\">");
           //    stbDonVi.Append(input);
           //    stbDonVi.Append("</td>");
           //    stbDonVi.Append("<td>" + TenDonVi + "</td>");

           //    stbDonVi.Append("</tr>");
           //}
           //stbDonVi.Append("</table>");
           //stbDonVi.Append("</div>");
           //dt.Dispose();
           //String DonVi = stbDonVi.ToString();
           //return DonVi;

       }
       /// <summary>
       /// Hàm lấy dữ liệu theo năm và tháng đổ vào commbox
       /// </summary>
       /// <param name="Thang_Quy"></param>
       /// <param name="iNamLamViec"></param>
       /// <returns></returns>
       public static DataTable DSDonVi(String MaND, String LoaiThangQuy, String Thang_Quy, String sLNS, String iID_MaTrangThaiDuyet)
       {
           DataTable dt = new DataTable();
           if (iID_MaTrangThaiDuyet == "0")
           {
               iID_MaTrangThaiDuyet = "AND iID_MaTrangThaiDuyet='" + LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeNguoiCoCong) + "'";
           }
           else
           {
               iID_MaTrangThaiDuyet = " ";
           }
           String DK_Thang = "";
           if (LoaiThangQuy == "1")
           {
               switch (Thang_Quy)
               {
                   case "1": DK_Thang = "iThang_Quy IN (1,2,3)";
                       break;
                   case "2": DK_Thang = "iThang_Quy IN (4,5,6)";
                       break;
                   case "3": DK_Thang = "iThang_Quy IN (7,8,9)";
                       break;
                   case "4": DK_Thang = "iThang_Quy IN (10,11,12)";
                       break;


               }
           }
           else
           {
               DK_Thang = "iThang_Quy=@ThangQuy";
           }
           SqlCommand cmd = new SqlCommand();
           //dot phan bo
           String SQL = string.Format(@" SELECT DISTINCT DV.iID_MaDonVi,DV.sTen
                                     FROM NCC_ChungTuChiTiet AS NCC
                                     INNER JOIN NS_DonVi AS DV ON NCC.iID_MaDonVi=DV.iID_MaDonVi
                                     WHERE sLNS=@sLNS AND NCC.iTrangThai=1  {0} AND iLoai=1 AND rTuChi>0
                                     AND {2} {1}
                                     ORDER BY iID_MaDonVi", iID_MaTrangThaiDuyet,ReportModels.DieuKien_NganSach(MaND),DK_Thang);
           cmd.CommandText = SQL;
           if (LoaiThangQuy == "0")
           {
               cmd.Parameters.AddWithValue("@ThangQuy", Thang_Quy);
           }
           cmd.Parameters.AddWithValue(@"sLNS", sLNS);
           dt = Connection.GetDataTable(cmd);
           if (dt.Rows.Count == 0)
           {
               DataRow dr = dt.NewRow();
               dr["iID_MaDonVi"] = "";
               dr["sTen"] = "Không có đơn vị";
               dt.Rows.InsertAt(dr, 0);
           }
           cmd.Dispose();
           return dt;
       }

      
        /// <summary>
        /// lấy mô tả loại ngân sách
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY iSTT";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
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
