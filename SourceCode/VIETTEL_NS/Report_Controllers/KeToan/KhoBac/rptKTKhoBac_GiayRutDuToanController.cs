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

namespace VIETTEL.Report_Controllers.KeToan.KhoBac
{
    public class rptKTKhoBac_GiayRutDuToanController : Controller
    {
        //
        // GET: /rptKTKhoBac_GiayRutDuToan/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKhoBac_GiayRutDuToan.xls";
        private const String sFilePath_NT = "/Report_ExcelFrom/KeToan/KhoBac/rptKTKhoBac_GiayRutDuToan_NT.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKhoBac_GiayRutDuToan.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        public ActionResult EditSubmit(String ParentID, String iNamLamViec, int ChiSo)
        {
            String sMaChungTuChiTiet = Request.Form[ParentID + "_sSoChungTuChiTiet"];
            if (String.IsNullOrEmpty(sMaChungTuChiTiet))
                return RedirectToAction("Index", "KeToanChiTietKhoBac", new { iLoai = "1" });
            else
            {
                String iID_MaChungTu = Request.Form[ParentID + "_iID_MaChungTu"];
                String InTenMLNS = Request.Form[ParentID + "_InTenMLNS"];
                String[] arrMaChungTuChiTiet = sMaChungTuChiTiet.Split(',');
                String sSoChungTuChiTiet = arrMaChungTuChiTiet[ChiSo];
                String iLoai = Request.Form[ParentID + "_iLoai"];
                ViewData["sSoChungTuChiTiet"] = sMaChungTuChiTiet;
                ViewData["ChiSo"] = ChiSo;
                ViewData["iNamLamViec"] = iNamLamViec;
                ViewData["iThang"] = "";
                ViewData["iID_MaChungTu"] = iID_MaChungTu;
                ViewData["iLoai"] = iLoai;
                ViewData["InTenMLNS"] = InTenMLNS;
                ViewData["path"] = "~/Report_Views/KeToan/KhoBac/rptKTKhoBac_GiayRutDuToan.aspx";
                return View(sViewPath + "ReportView.aspx");
            }
        }
        public ExcelFile CreateReport(String path, String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String InTenMLNS, String iLoai)
        {
            String SoChungTuCT = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            FlexCelReport fr = new FlexCelReport();
            DateTime dt = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            String NgayThangNam = ReportModels.Ngay_Thang_Nam_HienTai();
            LoadData(fr, iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, InTenMLNS, iLoai);
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTKhoBac_GiayRutDuToan");
            fr.SetValue("Nam", iNamLamViec);
            fr.SetValue("So", TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0));
            fr.SetValue("Thang", "");
            fr.SetValue("Ngay", "");
            fr.SetValue("Thangs", "");
            fr.SetValue("Nams", "");
            fr.SetValue("NgayThangNam", NgayThangNam);
            fr.SetValue("NgayCT", CommonFunction.LayTruong("KTKB_ChungTuChiTiet", "iID_MaChungTu", iID_MaChungTu, "iNgayCT"));
            fr.SetValue("InTenMLNS", InTenMLNS);
            fr.Run(Result);
            return Result;
        }


        public clsExcelResult ExportToExcel(String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String InTenMLNS, String iLoai)
        {
            String Duongdan = sFilePath;
            if (iLoai == "5")
            {
                Duongdan = sFilePath_NT;
            }
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(Duongdan), iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, InTenMLNS, iLoai);
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
        public ActionResult ViewPDF(String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String InTenMLNS, String iLoai)
        {
            HamChung.Language();
            String Duongdan = sFilePath;
            if (iLoai == "5")
            {
                Duongdan = sFilePath_NT;
            }
            ExcelFile xls = CreateReport(Server.MapPath(Duongdan), iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, InTenMLNS, iLoai);
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
        private void LoadData(FlexCelReport fr, String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String InTenMLNS, String iLoai)
        {
            DataTable data = DVTra(iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet,iLoai);
            data.TableName = "DVTra";
            fr.AddTable("DVTra", data);
            DataTable data1 = DVNhan(iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, iLoai);
            data.TableName = "DVNhan";
            fr.AddTable("DVNhan", data1);
            DataTable dtChiTiet = Get_dtChiTiet(iNamLamViec, iID_MaChungTu, sSoChungTuChiTiet, iLoai);

            int a = dtChiTiet.Rows.Count;
            int tg = 0;
            int countGhiChu = 0;
            if (a <= 5) countGhiChu = a;
            else if (a < 10) countGhiChu = 10 - a;

            String[] arrGhiChu = new String[countGhiChu];
            if (countGhiChu == 1) arrGhiChu[0] = Convert.ToString(dtChiTiet.Rows[0]["sGhiChu"]);
            for (int i = 1; i < a; i++)
            {
                DataRow r = dtChiTiet.Rows[i];
                DataRow r1 = dtChiTiet.Rows[tg];

                arrGhiChu[i - 1] = "( " + Convert.ToString(dtChiTiet.Rows[i - 1]["sGhiChu"]) + " )";

                if (r["sNoiDung"].ToString().Equals(r1["sNoiDung"].ToString()) == false)
                {
                    tg = i;
                }
                if (r["sNoiDung"].ToString().Equals(r1["sNoiDung"].ToString()))
                {
                    r["sNoiDung"] = "";
                }


                if (InTenMLNS == "on")
                {
                    if (r["sMoTa"].ToString().Equals(r1["sMoTa"].ToString()) == false)
                    {
                        tg = i;
                    }
                    if (r["sMoTa"].ToString().Equals(r1["sMoTa"].ToString()))
                    {
                        r["sMoTa"] = "";
                    }

                }
            }
            if (a < 10 && a > 0)
            {
                if (InTenMLNS != "on")
                {
                    for (int i = 0; i < countGhiChu; i++)
                    {
                        DataRow r = dtChiTiet.NewRow();
                        r["sNoiDung"] = arrGhiChu[i];
                        dtChiTiet.Rows.Add(r);

                    }

                }
                a = dtChiTiet.Rows.Count;
                for (int i = 0; i < (10 - a); i++)
                {
                    DataRow r = dtChiTiet.NewRow();
                    dtChiTiet.Rows.Add(r);
                }
            }
            fr.AddTable("ChiTiet", dtChiTiet);
            Double Tong = 0;
            for (int i = 0; i < dtChiTiet.Rows.Count; i++)
            {
                if (dtChiTiet.Rows[i]["rDTRut"] != DBNull.Value)
                    Tong = Tong + Convert.ToDouble(dtChiTiet.Rows[i]["rDTRut"]);
            }
            String sTien = Convert.ToString(Tong);
            long Tien = long.Parse(sTien);
            if (Tien < 0)
            {
                Tien = Tien * (-1);
                fr.SetValue("Tien", "Âm " + CommonFunction.TienRaChu(Tien));
            }
            else
                fr.SetValue("Tien", CommonFunction.TienRaChu(Tien));
            String CK = "", TC = "", TU = "", TM = "", Du = "", ChuaDu = ""; ;
            String NguoiChuyenTien = "";
            String sTenNhanVien = "", sChungMinhThu = "", dNgayCap = "", sNoiCap = "";
            if (data1.Rows.Count > 0)
            {
                if (Convert.ToString(data1.Rows[0]["sLoaiST"]) == "S") CK = "X";
                else TM = "X";
                if (Convert.ToString(data1.Rows[0]["sThuChi"]) == "1") TC = "X";
                else
                {
                    TU = "X";
                    if (Convert.ToString(data1.Rows[0]["sThuChi"]) == "2")
                        Du = "X";
                    else
                        ChuaDu = "X";
                }
                sTenNhanVien = data1.Rows[0]["sTenNhanVien_Nhan"].ToString();
                sChungMinhThu = data1.Rows[0]["sChungMinhThu"].ToString();
                dNgayCap = data1.Rows[0]["dNgayCap"].ToString();
                sNoiCap = data1.Rows[0]["sNoiCap"].ToString();
                int a1 = sTenNhanVien.Length;
                int a2 = sChungMinhThu.Length;
                int a3 = dNgayCap.Length;
                int a4 = sNoiCap.Length;
                int len1 = 50, len2 = 20, len3 = 25, len4 = 35;
                int tam = 0;
                if (a1 <= len1)
                {
                    tam = len1 - a1;
                    for (int i = 0; i < tam; i++)
                    {
                        sTenNhanVien += " ";
                    }
                }
                if (a2 <= len2)
                {
                    tam = len2 - a2;
                    for (int i = 0; i < tam; i++)
                    {
                        sChungMinhThu += " ";
                    }
                }
                if (a3 <= len3)
                {
                    tam = len3 - a3;
                    for (int i = 0; i < tam; i++)
                    {
                        dNgayCap += " ";
                    }
                }
                if (a4 <= len4)
                {
                    tam = len4 - a4;
                    for (int i = 0; i < tam; i++)
                    {
                        sNoiCap += " ";
                    }
                }
                if (TM == "X")
                {
                    NguoiChuyenTien = "Hoặc người nhận tiền:" + sTenNhanVien + " Số CMND: " + sChungMinhThu + "Ngày cấp: " + dNgayCap + "Tại: " + sNoiCap;
                }
            }
            fr.SetValue("TC", TC);
            fr.SetValue("TU", TU);
            fr.SetValue("CK", CK);
            fr.SetValue("TM", TM);
            fr.SetValue("Du", Du);
            fr.SetValue("ChuaDu", ChuaDu);
            fr.SetValue("NguoiChuyenTien", NguoiChuyenTien);
            fr.SetValue("sTenNhanVien", sTenNhanVien);
            fr.SetValue("sChungMinhThu", sChungMinhThu);
            fr.SetValue("sNoiCap", sNoiCap);
            fr.SetValue("dNgayCap", dNgayCap);
            data.Dispose();
            data1.Dispose();
        }


        public DataTable Get_dtChiTiet(String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String iLoai)
        {
            if (iLoai != "5")
            {
                String SoChungTuCT = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);
                DataTable dt;
                String SQL = "SELECT * ";
                SQL += " FROM KTKB_ChungTuChiTiet as CT,NS_NguonNganSach as Nguon ";
                SQL += " WHERE CT.iLoai =1  ";
                SQL += " AND CT.iTrangThai =1  ";
                SQL += " AND iNamLamViec=@iNamLamViec ";
                SQL += " AND CT.iID_MaChungTu =@iID_MaChungTu ";
                SQL += " AND CT.iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                SQL += " AND sSoChungTuChiTiet=@sSoChungTuChiTiet";
                SQL += " AND CT.iID_MaNguonNganSach=Nguon.iID_MaNguonNganSach";
                SQL += " ORDER BY CT.dNgayTao";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                cmd.Parameters.AddWithValue("@sSoChungTuChiTiet", SoChungTuCT);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
            else
            {
                String SoChungTuCT = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);
                DataTable dt;
                String SQL = "SELECT * ";
                SQL += " FROM KTKB_ChungTuChiTiet as CT,NS_NguonNganSach as Nguon ";
                SQL += " WHERE   ";
                SQL += "  CT.iTrangThai =1  ";
                SQL += " AND iNamLamViec=@iNamLamViec ";
                SQL += " AND CT.iID_MaChungTu =@iID_MaChungTu ";
                SQL += " AND CT.iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                SQL += " AND sSoChungTuChiTiet=@sSoChungTuChiTiet";
                SQL += " AND CT.iID_MaNguonNganSach=Nguon.iID_MaNguonNganSach";
                SQL += " ORDER BY CT.dNgayTao";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                cmd.Parameters.AddWithValue("@sSoChungTuChiTiet", SoChungTuCT);
                dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
        }

        public DataTable DVTra(String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet, String iLoai)
        {
            if (iLoai != "5")
            {
                String SoChungTuCT = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);
                String MaDonViNhan = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 1);

                String SQL1 = "SELECT TOP 1 *";
                SQL1 += " FROM KTKB_ChungTuChiTiet as CT ";
                SQL1 += " WHERE CT.iLoai =1  ";
                SQL1 += " AND CT.iTrangThai =1  ";
                SQL1 += " AND CT.iID_MaChungTu =@iID_MaChungTu ";
                SQL1 += " AND CT.iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                SQL1 += " AND sSoChungTuChiTiet=@sSoChungTuChiTiet";
                SQL1 += " AND iID_MaDonVi_Nhan=@iID_MaDonVi_Nhan";
                SQL1 += " ORDER BY dNgayTao";

                String SQL = " SELECT TOP 1 sLoaiST,sThuChi";
                SQL += ",CT.sSoChungTuChiTiet,CT.sTenDonVi_Tra,CT.iID_MaDonVi_Tra";
                SQL += ",CT.sNoiDung,CT.rDTRut, CT.sCapNganSach, CT.iID_MaChuongTrinhMucTieu_Tra, CT.sTenChuongTrinhMucTieu_Tra";
                SQL += ",CT.iID_MaNguonNganSach, CT.sK, CT.sM, CT.sC, DV.sMaSo, DV.sSoTaiKhoan, DV.sKhoBac,DV.sDiaChi";
                SQL += " FROM ({0}) as CT";
                SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON DV.iID_MaDonVi = CT.iID_MaDonVi_Tra";
                SQL += " ORDER BY CT.dNgayTao";
                SQL = String.Format(SQL, SQL1);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.Parameters.AddWithValue("@sSoChungTuChiTiet", SoChungTuCT);
                cmd.Parameters.AddWithValue("@iID_MaDonVi_Nhan", MaDonViNhan);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
            else
            {
                String SoChungTuCT = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);
                String MaDonViNhan = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 1);

                String SQL1 = "SELECT TOP 1 *";
                SQL1 += " FROM KTKB_ChungTuChiTiet as CT ";
                SQL1 += " WHERE   ";
                SQL1 += "  CT.iTrangThai =1  ";
                SQL1 += " AND CT.iID_MaChungTu =@iID_MaChungTu ";
                SQL1 += " AND CT.iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                SQL1 += " AND sSoChungTuChiTiet=@sSoChungTuChiTiet";
                SQL1 += " AND iID_MaDonVi_Nhan=@iID_MaDonVi_Nhan";
                SQL1 += " ORDER BY dNgayTao";

                String SQL = " SELECT TOP 1 sLoaiST,sThuChi";
                SQL += ",CT.sSoChungTuChiTiet,CT.sTenDonVi_Tra,CT.iID_MaDonVi_Tra";
                SQL += ",CT.sNoiDung,CT.rDTRut, CT.sCapNganSach, CT.iID_MaChuongTrinhMucTieu_Tra, CT.sTenChuongTrinhMucTieu_Tra";
                SQL += ",CT.iID_MaNguonNganSach, CT.sK, CT.sM, CT.sC, DV.sMaSo, DV.sSoTaiKhoan, DV.sKhoBac,DV.sDiaChi";
                SQL += " FROM ({0}) as CT";
                SQL += " INNER JOIN (SELECT * FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec) as DV ON DV.iID_MaDonVi = CT.iID_MaDonVi_Tra";
                SQL += " ORDER BY CT.dNgayTao";
                SQL = String.Format(SQL, SQL1);
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.Parameters.AddWithValue("@sSoChungTuChiTiet", SoChungTuCT);
                cmd.Parameters.AddWithValue("@iID_MaDonVi_Nhan", MaDonViNhan);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
        }

        public DataTable DVNhan(String iNamLamViec, String iID_MaChungTu, String sSoChungTuChiTiet,String iLoai)
        {
            // loai=1 rut du toan
            String SoChungTuCT = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 0);
            String MaDonViNhan = TachLaySoChungTu_DonViNhan(sSoChungTuChiTiet, 1);
            if (iLoai != "5")
            {
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
            else
            {
                String SQL1 = " SELECT TOP 1 sLoaiST,sThuChi,CT.iID_MaNhanVien_Nhan,CT.sTenNhanVien_Nhan,NV.sChungMinhThu, NV.dNgayCap,NV.sNoiCap";
                SQL1 += ",CT.sSoChungTuChiTiet,CT.sTenDonVi_Nhan,CT.iID_MaDonVi_Nhan";
                SQL1 += ",CT.sNoiDung,CT.rDTRut, CT.sCapNganSach, CT.iID_MaChuongTrinhMucTieu_Nhan, CT.sTenChuongTrinhMucTieu_Nhan";
                SQL1 += ",CT.iID_MaNguonNganSach, CT.sK, CT.sM, CT.sC, DV.sMaSo, DV.sSoTaiKhoan, DV.sKhoBac,DV.sDiaChi";
                SQL1 += " FROM (SELECT * FROM KTKB_ChungTuChiTiet ";
                SQL1 += " WHERE  iTrangThai =1 AND iNamLamViec=@iNamLamViec AND iID_MaChungTu=@iID_MaChungTu AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND sSoChungTuChiTiet =@sSoChungTuChiTiet AND iID_MaDonVi_Nhan=@iID_MaDonVi_Nhan) as CT";
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
           
        }

        public static DataTable Lay_SoChungTu(String iNamLamViec, String iThang, String iID_MaChungTu,String iLoai)
        {
            String DK = "";
            SqlCommand cmd = new SqlCommand();

            if (iLoai != "5")
            {
                String SQL = " SELECT CT.sSoChungTuChiTiet+'#'+iID_MaDonVi_Nhan AS sSoChungTuChiTiet";
                SQL += ",Convert(varchar,CT.sSoChungTuChiTiet)+'-'+ SUBSTRING(CT.sTenDonVi_Nhan,charindex('-',CT.sTenDonVi_Nhan)+1,100) AS sTenDonVi_Nhan";
                SQL += " FROM KTKB_ChungTuChiTiet as CT ";
                SQL += "  WHERE CT.iLoai =1 AND CT.iID_MaChungTu =@iID_MaChungTu AND CT.iTrangThai =1  ";
                SQL += " AND CT.iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                SQL += " GROUP BY CT.sSoChungTuChiTiet,CT.sTenDonVi_Nhan,iID_MaDonVi_Nhan";
                SQL += " ORDER BY sTenDonVi_Nhan";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
            else
            {
                String SQL = " SELECT CT.sSoChungTuChiTiet+'#'+iID_MaDonVi_Nhan AS sSoChungTuChiTiet";
                SQL += ",Convert(varchar,CT.sSoChungTuChiTiet)+'-'+ SUBSTRING(CT.sTenDonVi_Nhan,charindex('-',CT.sTenDonVi_Nhan)+1,100) AS sTenDonVi_Nhan";
                SQL += " FROM KTKB_ChungTuChiTiet as CT ";
                SQL += "  WHERE CT.iID_MaChungTu =@iID_MaChungTu AND CT.iTrangThai =1  ";
                SQL += " AND CT.iID_MaTrangThaiDuyet = @iID_MaTrangThaiDuyet";
                SQL += " GROUP BY CT.sSoChungTuChiTiet,CT.sTenDonVi_Nhan,iID_MaDonVi_Nhan";
                SQL += " ORDER BY sTenDonVi_Nhan";
                SQL = String.Format(SQL, DK);
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                return dt;
            }
          
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

        public JsonResult SoDongChungTu(String iID_MaChungTu, String iNamLamViec)
        {

            String ThamSo = KeToan_DanhMucThamSoModels.LayThongTinThamSo("200", iNamLamViec);
            String SQL = "SELECT COUNT(*) AS SL,sSoChungTuChiTiet";
            SQL += " FROM KTKB_ChungTuChiTiet ";
            SQL += " WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
            SQL += " GROUP BY sSoChungTuChiTiet";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
            DataTable dt = Connection.GetDataTable(cmd);
            String SoCT = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt16(dt.Rows[i]["SL"]) > Convert.ToInt16(ThamSo))
                {
                    SoCT += Convert.ToString(dt.Rows[i]["sSoChungTuChiTiet"]) + ",";
                }
            }
            if (SoCT.Length > 0)
            {
                SoCT = SoCT.Remove(SoCT.Length - 1, 1);
            }
            Object item = new
            {
                SoCT = SoCT,
                ThamSo = ThamSo
            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TachSoChungTu(String iID_MaChungTu, String dsSoCT, String ThamSo)
        {
            int iThamSo = Convert.ToInt16(ThamSo);
            int iThamSoDau = iThamSo;
            String[] arrSCT = dsSoCT.Split(',');
            SqlCommand cmd;
            for (int i = 0; i < arrSCT.Length; i++)
            {
                String SQL = "SELECT iID_MaChungTuChiTiet,sSoChungTuChiTiet";
                SQL += " FROM KTKB_ChungTuChiTiet ";
                SQL += " WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu";
                SQL += " AND sSoChungTuChiTiet=@sSoChungTuChiTiet";
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                cmd.Parameters.AddWithValue("@sSoChungTuChiTiet", arrSCT[i]);
                DataTable dt = Connection.GetDataTable(cmd);
                cmd.Dispose();
                String sSoChungTuChiTiet = "";
                int SoLan = 1;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (j + 1 > iThamSo)
                    {
                        iThamSo = iThamSo + iThamSoDau;
                        SoLan = SoLan + 1;
                    }
                    sSoChungTuChiTiet = arrSCT[i] + "-" + SoLan.ToString();
                    SQL = "UPDATE KTKB_ChungTuChiTiet  SET sSoChungTuChiTiet=@sSoChungTuChiTiet WHERE iID_MaChungTuChiTiet=@iID_MaChungTuChiTiet ";
                    cmd = new SqlCommand();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iID_MaChungTuChiTiet", dt.Rows[j]["iID_MaChungTuChiTiet"]);
                    cmd.Parameters.AddWithValue("@sSoChungTuChiTiet", sSoChungTuChiTiet);
                    Connection.UpdateDatabase(cmd);
                    cmd.Dispose();
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}
