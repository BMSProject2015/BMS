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
using DomainModel.Abstract;
using System.IO;
using System.Text;

namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptThongTriChuyenTienController : Controller
    {
        //
        // GET: /rptThongTriChuyenTien/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/KhoBac/rptThongTriChuyenTien.xls";
        public static String NameFile = "";
        public String LoaiTT = "";
        public String LoaiNS = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
            ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptThongTriChuyenTien.aspx";
            return View(sViewPath + "ReportView.aspx");
            }
             else
             {
                 return RedirectToAction("Index", "PermitionMessage");
             }
        }
        public ActionResult EditSubmit(String ParentID, String iNamLamViec, String iID_MaNamNganSach, String iID_MaNguonNganSach, String iThang, int ChiSo)
        {
            String sMaChungTuChiTiet = Request.Form[ParentID + "_sSoChungTuChiTiet"];
            if (String.IsNullOrEmpty(sMaChungTuChiTiet))
                return RedirectToAction("Index", "KeToanChiTietKhoBac",new {iLoai ="1"});
            else
            {
                String[] arrMaChungTuChiTiet = sMaChungTuChiTiet.Split(',');
                String sSoChungTuChiTiet = arrMaChungTuChiTiet[ChiSo];
                String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
                //ViewData["ssSoChungTuChiTiet"] = sMaChungTuChiTiet;
                String iID_MaChungTu = Request.Form[ParentID + "_iID_MaChungTu"];
                String InTenMLNS = Request.Form[ParentID + "_InTenMLNS"];
                String iID_MaThongTri = Convert.ToString(Request.Form["ThongTri" + "_iID_MaThongTri"]);
                String chkThemMoi = Convert.ToString(Request.Form[ParentID + "_chkThemMoi"]);
                if (chkThemMoi == "on")
                {
                    String sTen = Convert.ToString(Request.Form[ParentID + "_sTen"]);
                    String sNoiDung = Convert.ToString(Request.Form[ParentID + "_sNoiDung"]);
                    Bang bang = new Bang("KT_LoaiThongTri");
                    bang.MaNguoiDungSua = User.Identity.Name;
                    bang.IPSua = Request.UserHostAddress;
                    bang.CmdParams.Parameters.AddWithValue("@sLoaiKhoan", sTen);
                    bang.CmdParams.Parameters.AddWithValue("@sTenLoaiNS", sNoiDung);
                    iID_MaThongTri = Convert.ToString(bang.Save());
                }
                ViewData["sSoChungTuChiTiet"] = sMaChungTuChiTiet;
                ViewData["ChiSo"] = ChiSo;
                ViewData["iNamLamViec"] = iNamLamViec;
                ViewData["iThang"] = "";
                ViewData["iID_MaChungTu"] = iID_MaChungTu;
                ViewData["InTenMLNS"] = InTenMLNS;
                ViewData["chkThemMoi"] = chkThemMoi;
                ViewData["iID_MaThongTri"] = iID_MaThongTri;

                ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptThongTriChuyenTien.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
        }
        public ExcelFile CreateReport(String path, String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String iID_MaNamNganSach, String iID_MaNguonNganSach,String UserID,String iID_MaThongTri)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();           
            LoadData(fr, iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, iID_MaNamNganSach, iID_MaNguonNganSach);
            //Lấy thông tin đơn vị nhận
            DataTable dtDVN = dtDanhSachDonVi_Nhan(iNamLamViec,"",iID_MaChungTu);
            String DVN = "";
            if (dtDVN.Rows.Count > 0)
            {
                DVN = dtDVN.Rows[0]["sTenDonVi_Nhan"].ToString();
            }
            fr = ReportModels.LayThongTinChuKy(fr, "rptThongTriChuyenTien");
            fr.SetValue("Nams", iNamLamViec);
            fr.SetValue("So", sSoChungTuChiTiet);            
            fr.SetValue("Ngay", ngay);          
            fr.SetValue("ThangCT", DanhMucModels.ThangLamViec(UserID));
            fr.SetValue("DonVi", DVN);
           
            String sCT = "";
            sCT = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);
            fr.SetValue("RDT", "RDT-" + sCT);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            DataTable dtTong = ThongTriChuyenTien(iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, iID_MaNamNganSach, iID_MaNguonNganSach);
            long tong = 0;
            if (dtTong.Rows.Count > 0)
            {
                for (int i = 0; i < dtTong.Rows.Count; i++)
                {
                    tong += String.IsNullOrEmpty(dtTong.Rows[i]["rDTRut"].ToString()) ? 0 : long.Parse(dtTong.Rows[i]["rDTRut"].ToString());
                }
            }

            //lay ten thong tri
            DataTable dtLoaiThongTri;
            String SQL = "SELECT * FROM KT_LoaiThongTri WHERE iID_MaThongTri=@iID_MaThongTri";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaThongTri", iID_MaThongTri);
            dtLoaiThongTri = Connection.GetDataTable(cmd);
            cmd.Dispose();
            LoaiTT = Convert.ToString(dtLoaiThongTri.Rows[0]["sLoaiKhoan"]);
            LoaiNS = Convert.ToString(dtLoaiThongTri.Rows[0]["sTenLoaiNS"]);
            fr.SetValue("LoaiTT", LoaiTT.ToUpper());
            fr.SetValue("LoaiNS", LoaiNS.ToUpper());

            fr.SetValue("Tien", CommonFunction.TienRaChu(tong));
            fr.Run(Result);
            return Result;
        }
        
        public clsExcelResult ExportToPDF(String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String iID_MaNamNganSach, String iID_MaNguonNganSach,String UserID,String iID_MaThongTri)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, iID_MaNamNganSach, iID_MaNguonNganSach, UserID, iID_MaThongTri);

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
        public clsExcelResult ExportToExcel(String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String iID_MaNamNganSach, String iID_MaNguonNganSach, String UserID, String iID_MaThongTri)
        {
            HamChung.Language();
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, iID_MaNamNganSach, iID_MaNguonNganSach, UserID, iID_MaThongTri);
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
        public ActionResult ViewPDF(String MaND, String iID_MaChungTu, String sSoChungTuChiTiet, String InTenMLNS, String UserID, String iID_MaThongTri)
        {
            HamChung.Language();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
            dtCauHinh.Dispose();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), iNam, iID_MaChungTu, sSoChungTuChiTiet, iID_MaNamNganSach, iID_MaNguonNganSach, MaND, iID_MaThongTri);
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
        private void LoadData(FlexCelReport fr, String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String iID_MaNamNganSach, String iID_MaNguonNganSach)
        {

            DataTable data = ThongTriChuyenTien(iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, iID_MaNamNganSach, iID_MaNguonNganSach);
            data.TableName = "ChiTiet";
            DataTable dtMuc;
            dtMuc = HamChung.SelectDistinct("Muc", data, "NguonNS,sLNS,sLK,sL,sK,sM", "NguonNS,sLNS,sLK,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            fr.AddTable("Muc", dtMuc);
            DataTable dtLoaiNS;
            dtLoaiNS = HamChung.SelectDistinct("Loai", dtMuc, "NguonNS,sLNS,sLK,sL,sK", "NguonNS,sLNS,sLK,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            int a = dtLoaiNS.Rows.Count;
            if (a < 10 && a > 0)
            {
                for (int i = 0; i < (10 - a); i++)
                {
                    DataRow r = dtLoaiNS.NewRow();
                    dtLoaiNS.Rows.Add(r);
                }
            }
            fr.AddTable("Loai", dtLoaiNS);
            fr.AddTable("ChiTiet", data);
            data.Dispose();
            dtLoaiNS.Dispose();
            dtMuc.Dispose();
        }

        public DataTable DVNhan(String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String iID_MaNamNganSach, String iID_MaNguonNganSach)
        {
            // loai=1 rut du toan
            String SoChungTuCT = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);
            String MaDonViNhan = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 1);

            String SQL1 = " SELECT TOP 1 sLoaiST,sThuChi,CT.iID_MaNhanVien_Nhan,CT.sTenNhanVien_Nhan,NV.sChungMinhThu, NV.dNgayCap,NV.sNoiCap";
            SQL1 += ",CT.sSoChungTuChiTiet,CT.sTenDonVi_Nhan,CT.iID_MaDonVi_Nhan";
            SQL1 += ",CT.sNoiDung,CT.rDTRut, CT.sCapNganSach, CT.iID_MaChuongTrinhMucTieu_Nhan, CT.sTenChuongTrinhMucTieu_Nhan";
            SQL1 += ",CT.iID_MaNguonNganSach, CT.sK, CT.sM, CT.sC, DV.sMaSo, DV.sSoTaiKhoan, DV.sKhoBac,DV.sDiaChi";
            SQL1 += " FROM (SELECT * FROM KTKB_ChungTuChiTiet ";
            SQL1 += " WHERE iLoai = 1 AND iTrangThai =1 AND iNamLamViec=@iNamLamViec AND iID_MaChungTu=@iID_MaChungTu AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND sSoChungTuChiTiet =@sSoChungTuChiTiet AND iID_MaDonVi_Nhan=@iID_MaDonVi_Nhan) as CT";
            SQL1 += " LEFT JOIN KT_NhanVien as NV ON NV.iID_MaNhanVien = CT.iID_MaNhanVien_Nhan";
            SQL1 += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON DV.iID_MaDonVi = CT.iID_MaDonVi_Nhan";
            SQL1 += " ORDER BY CT.dNgayTao";

            SqlCommand cmd = new SqlCommand(SQL1);
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@sSoChungTuChiTiet", SoChungTuCT);
            cmd.Parameters.AddWithValue("@iID_MaDonVi_Nhan", MaDonViNhan);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        public DataTable ThongTriChuyenTien(String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String iID_MaNamNganSach, String iID_MaNguonNganSach)
        {

            String SoChungTuCT = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);

            String SQL = " SELECT substring(sLNS,1,1) NguonNS,sLNS,sL+'-'+sK AS sLK,sL,sK,sM,sTM,sMoTa,SUM(rDTRut) as rDTRut";
            SQL += " FROM KTKB_ChungTuChiTiet";
            SQL += " WHERE iLoai =1 AND iTrangThai=1 AND iID_MaNguonNganSach=@iID_MaNguonNganSach";
            SQL += " AND iID_MaChungTu =@iID_MaChungTu ";
            SQL += " AND iID_MaNamNganSach=@iID_MaNamNganSach  AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            SQL += " AND sSoChungTuChiTiet=@sSoChungTuChiTiet";
            SQL += " GROUP BY sLNS,sL,sK,sM,sTM,sMoTa";
            SQL += " HAVING SUM(rDTRut)>0";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sSoChungTuChiTiet", SoChungTuCT);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));

            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //        tong += long.Parse(dt.Rows[i]["rDTRut"].ToString());
            //}
            return dt;

        }

        public static DataTable dtDanhSachDonVi_Nhan(String iNamLamViec, String iThang, String iID_MaChungTu)
        {
            String SQL = String.Format(@" SELECT sSoChungTuChiTiet +'#'+iID_MaDonVi_Nhan AS sSoChungTuChiTiet,Convert(varchar,CT.sSoChungTuChiTiet)+'-'+ SUBSTRING(CT.sTenDonVi_Nhan,charindex('-',CT.sTenDonVi_Nhan)+1,100) AS sTenDonVi_Nhan
             FROM KTKB_ChungTuChiTiet as CT 
              WHERE CT.iLoai =1 AND CT.iID_MaChungTu =@iID_MaChungTu AND CT.iTrangThai =1  
             AND CT.iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet 
            GROUP BY sSoChungTuChiTiet,sTenDonVi_Nhan,iID_MaDonVi_Nhan
             ORDER BY sTenDonVi_Nhan
            ");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
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
        
    }
}
