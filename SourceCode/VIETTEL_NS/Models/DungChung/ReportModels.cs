using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Globalization;
using System.Drawing;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using System.Data.SqlClient;
using DomainModel;
namespace VIETTEL.Models
{
    public class ReportModels
    {

        public static String DieuKien_NganSach(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            String iID_MaDonVi = "";
            iID_MaDonVi = NguoiDung_DonViModels.getDonViByNguoiDung_1(MaND);
            if (String.IsNullOrEmpty(iID_MaDonVi))
                iID_MaDonVi = "'-1'";
            String DK = "",iNamLamViec=DateTime.Now.Year.ToString(),iID_MaNamNganSach="1",iID_MaNguonNganSach="1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK =String.Format(@" AND (iNamLamViec={0} AND iID_MaNamNganSach={1} AND iID_MaNguonNganSach={2})
                                 --AND (iID_MaPhongBan ='{3}' )
                                 AND (iID_MaDonVi IN ({4}))"
                                , iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach, iID_MaPhongBan, iID_MaDonVi);
            return DK;
        }
        public static String DieuKien_NganSach_KhongDV(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(@" AND (iNamLamViec={0} AND iID_MaNamNganSach={1} AND iID_MaNguonNganSach={2})  "
                                , iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach);
            return DK;
        }
        public static String DieuKien_ChiTieu(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(@" AND (iNamLamViec={0} AND iID_MaNamNganSach={1} AND iID_MaNguonNganSach={2})
                                 AND (iID_MaPhongBan ='{3}' )
                                 "
                                , iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach, iID_MaPhongBan);
            return DK;
        }
        public static String DieuKien_PhongBan_DonVi(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            String iID_MaDonVi = "";
            iID_MaDonVi = NguoiDung_DonViModels.getDonViByNguoiDung_1(MaND);
            String DK = "";
            DK = String.Format(@"
                                 AND (iID_MaPhongBan ='{0}')
                                 AND (iID_MaDonVi IN ({1}))"
                                , iID_MaPhongBan, iID_MaDonVi);
            return DK;
        }
        public static String DieuKien_NganSachThuongXuyen(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            String iID_MaDonVi = "";
            iID_MaDonVi = NguoiDung_DonViModels.getDonViByNguoiDung(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(@" AND (PB_PhanBoChiTiet.iNamLamViec={0} AND PB_PhanBoChiTiet.iID_MaNamNganSach={1} AND PB_PhanBoChiTiet.iID_MaNguonNganSach={2}) 
                                  AND (PB_PhanBoChiTiet.iID_MaPhongBan ='{3}' )
                                 AND (PB_PhanBoChiTiet.iID_MaDonVi IN ({4}))", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach,iID_MaPhongBan,iID_MaDonVi);
            return DK;
        }
        public static String DieuKien_NganSach(String MaND, String TenBang)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            String iID_MaDonVi = "";
            iID_MaDonVi = NguoiDung_DonViModels.getDonViByNguoiDung(MaND);
            String DK = "", iNamLamViec = DateTime.Now.Year.ToString(), iID_MaNamNganSach = "1", iID_MaNguonNganSach = "1";
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
                iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
                iID_MaNguonNganSach = Convert.ToString(dt.Rows[0]["iID_MaNguonNganSach"]);
            }
            DK = String.Format(@" AND (" + TenBang + ".iNamLamViec={0} AND " + TenBang + ".iID_MaNamNganSach={1} AND " + TenBang + ".iID_MaNguonNganSach={2})  AND ( " + TenBang + ".iID_MaPhongBan ='{3}' ) AND ( " + TenBang + ".iID_MaDonVi IN ({4}))", iNamLamViec, iID_MaNamNganSach, iID_MaNguonNganSach,iID_MaPhongBan,iID_MaDonVi);
            return DK;
        }
        /// <summary>
        /// Hàm lấy danh sách tất cả các trạng thái duyệt của phân hệ được sắp xếp theo số thứ tự
        /// </summary>
        /// <param name="iID_MaPhanHe">Mã phân hệ đang làm việc</param>
        /// <returns></returns>
        public static DataTable Get_dtDSTrangThaiDuyet(int iID_MaPhanHe,String Title="--- Tất cả ---")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM NS_PhanHe_TrangThaiDuyet WHERE iTrangThai=1 AND iID_MaPhanHe=@iID_MaPhanHe ORDER BY iSTT");
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", -1);
            vR = Connection.GetDataTable(cmd);
           
                DataRow R = vR.NewRow();
                R["iID_MaTrangThaiDuyet"] = 0;
                R["sTen"] = Title;
                vR.Rows.Add(R);
                R = vR.NewRow();
                R["iID_MaTrangThaiDuyet"] = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(iID_MaPhanHe);
                R["sTen"] = "Đã duyệt";
                vR.Rows.Add(R);
            cmd.Dispose();
            return vR;
        }

        public static String DieuKien_TrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet ";
        #region Lấy thông tin chữ ký
        public static FlexCelReport LayThongTinChuKy(FlexCelReport fr, String ControllerName, Boolean HienThi = true)
        {
            DataTable dt = DanhMuc_BaoCao_ChuKyModels.Get_dtLayThongTinChuKy(ControllerName);
            if (HienThi == true && dt != null && dt.Rows.Count > 0)
            {

                fr.SetValue("ThuaLenh1", dt.Rows[0]["sTenThuaLenh1"]);
                fr.SetValue("ThuaLenh2", dt.Rows[0]["sTenThuaLenh2"]);
                fr.SetValue("ThuaLenh3", dt.Rows[0]["sTenThuaLenh3"]);
                fr.SetValue("ThuaLenh4", dt.Rows[0]["sTenThuaLenh4"]);
                fr.SetValue("ThuaLenh5", dt.Rows[0]["sTenThuaLenh5"]);

                fr.SetValue("ChucDanh1", dt.Rows[0]["sTenChucDanh1"]);
                fr.SetValue("ChucDanh2", dt.Rows[0]["sTenChucDanh2"]);
                fr.SetValue("ChucDanh3", dt.Rows[0]["sTenChucDanh3"]);
                fr.SetValue("ChucDanh4", dt.Rows[0]["sTenChucDanh4"]);
                fr.SetValue("ChucDanh5", dt.Rows[0]["sTenChucDanh5"]);

                fr.SetValue("Ten1", dt.Rows[0]["sTen1"]);
                fr.SetValue("Ten2", dt.Rows[0]["sTen2"]);
                fr.SetValue("Ten3", dt.Rows[0]["sTen3"]);
                fr.SetValue("Ten4", dt.Rows[0]["sTen4"]);
                fr.SetValue("Ten5", dt.Rows[0]["sTen5"]);

            }
            else
            {

                fr.SetValue("ThuaLenh1", "");
                fr.SetValue("ThuaLenh2", "");
                fr.SetValue("ThuaLenh3", "");
                fr.SetValue("ThuaLenh4", "");
                fr.SetValue("ThuaLenh5", "");

                fr.SetValue("ChucDanh1", "");
                fr.SetValue("ChucDanh2", "");
                fr.SetValue("ChucDanh3", "");
                fr.SetValue("ChucDanh4", "");
                fr.SetValue("ChucDanh5", "");

                fr.SetValue("Ten1", "");
                fr.SetValue("Ten2", "");
                fr.SetValue("Ten3", "");
                fr.SetValue("Ten4", "");
                fr.SetValue("Ten5", "");
            }
            return fr;
        }

        #endregion



        public static void FillData(ExcelFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotCuaMotTrang, String CoDienDuLieu, Boolean bSTT)
        {
            TFlxFormat fmt;
            //Tính số trang và số cột cần thêm để đủ một trang
            int SoCotDu = (DenCotCua_DT - TuCotCua_DT) % SoCotCuaMotTrang;
            int SoCotCanThem = 0;
            int TongSoCot = 0;
            if (SoCotDu != 0)
            {
                SoCotCanThem = SoCotCuaMotTrang - SoCotDu;
            }
            TongSoCot = DenCotCua_DT + SoCotCanThem - TuCotCua_DT;
            int SoTrang = TongSoCot / SoCotCuaMotTrang;
            //SET border cho số cột cần thêm
            for (int c = DenCotCua_DT + TuCot; c < TongSoCot + TuCot; c++)
            {
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    fmt = xls.GetCellVisibleFormatDef(h + TuHang, c);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Family = 1;
                    fmt.Font.CharSet = 0;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.left;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(h + TuHang, c, xls.AddFormat(fmt));
                }
            }

            int _C = TuCot;
            int widthcolTong = 4205;
            int w6 = widthcolTong / 6;
            int d = 0;
            object GiaTriO = null;
            #region Fill dữ liệu những cột động
            for (int c = 0; c < TongSoCot; c++)
            {
                Type _Type = typeof(String);
                if (c + TuCotCua_DT <= DenCotCua_DT)
                    _Type = dt.Columns[c + TuCotCua_DT].DataType;
                switch (_Type.ToString())
                {
                    case "System.Decimal":
                        fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Family = 1;
                        fmt.Font.CharSet = 0;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.right;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "_-* #,##0\\ _₫_-;\\-* #,##0\\ _₫_-;_-* \"-\"??\\ _₫_-;_-@_-";
                        break;
                    default:
                        fmt = xls.GetCellVisibleFormatDef(TuHang, 2);
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Family = 1;
                        fmt.Font.CharSet = 0;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.left;
                        fmt.VAlignment = TVFlxAlignment.center;
                        break;
                }

                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    GiaTriO = null;
                    if (c == TuCot - 1)
                        GiaTriO = dt.Rows[h][c];
                    xls.SetCellFormat(h + TuHang, _C, xls.AddFormat(fmt));
                    if (c + TuCotCua_DT <= DenCotCua_DT)
                        xls.SetCellValue(h + TuHang, _C, dt.Rows[h][c + TuCotCua_DT]);
                    if (d > 6)
                        xls.SetColWidth(_C, w6 + 4059);
                }
                _C++;
                d++;
            }
            #endregion

            #region Fill dữ liệu những cột cố định

            String KyTu1, KyTu2, strSum;
            int cot = 1;
            for (int h = 0; h < dt.Rows.Count; h++)
            {
                //set cho cột STT
                if (bSTT)
                {
                    fmt = xls.GetCellVisibleFormatDef(h + TuHang, 1);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Family = 1;
                    fmt.Font.CharSet = 0;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    fmt.WrapText = true;
                    xls.SetCellFormat(h + TuHang, 1, xls.AddFormat(fmt));
                    xls.SetCellValue(h + TuHang, 1, h + 1);
                }
                //set cho cột Đơn vị
                fmt = xls.GetCellVisibleFormatDef(h + TuHang, 2);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.left;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(h + TuHang, 2, xls.AddFormat(fmt));
                xls.SetCellValue(h + TuHang, 2, dt.Rows[h][0]);

                //Set cho cột tổng sô
                fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.right;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Format = "_-* #,##0\\ _₫_-;\\-* #,##0\\ _₫_-;_-* \"-\"??\\ _₫_-;_-@_-";
                xls.SetCellFormat(h + TuHang, TuCot - 1, xls.AddFormat(fmt));


                KyTu1 = HamChung.ExportExcel_MaCot(TuCot);
                KyTu2 = HamChung.ExportExcel_MaCot(TuCot + TongSoCot - 1);
                strSum = String.Format("=SUM({0}{1}:{2}{1})", KyTu1, h + TuHang, KyTu2);
                xls.SetCellFormat(h + TuHang, TuCot - 1, xls.AddFormat(fmt));
                xls.SetCellValue(h + TuHang, TuCot - 1, new TFormula(strSum));


            }
            #endregion

        }


        public static DataTable GET_dtDotPhanBo(String MaDotPhanBo)
        {
            DataTable dt = null;
            String SQL = "SELECT convert(varchar(10),PB_DotPhanBo.dNgayDotPhanBo,103), MONTH(dNgayDotPhanBo) as Thang,YEAR(dNgayDotPhanBo) as Nam";
            SQL += " FROM PB_DotPhanBo";
            SQL += " WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo ";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", MaDotPhanBo);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static int Get_STTDotPhanBo(String NamLamViec, String iID_MaDotPhanBo)
        {

            String SQL, DK = "";
            SqlCommand cmd = new SqlCommand();
            SQL = String.Format(@"SELECT COUNT(a.iID_MaDotPhanBo)
                                FROM (SELECT DISTINCT iID_MaDotPhanBo,dNgayDotPhanBo FROM PB_PhanBo
		                                WHERE dNgayDotPhanBo <= (SELECT DISTINCT dNgayDotPhanBo FROM PB_DotPhanBo 
								                                WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo AND iNamLamViec=@iNamLamViec AND iTrangThai=1)) as B
                                INNER JOIN (SELECT DISTINCT iID_MaDotPhanBo FROM PB_PhanBoChiTiet
			                                WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet) AS A ON A.iID_MaDotPhanBo=B.iID_MaDotPhanBo
                                ");
            cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHePhanBo));
            int STT = Convert.ToInt16(Connection.GetValue(cmd, 1));
            cmd.Dispose();
            return STT;
        }
        public static int Get_STTDotPhanBo(String MaND, String iID_MaTrangThaiDuyet, String iID_MaDotPhanBo)
        {

            String SQL, DK = "";
            SqlCommand cmd = new SqlCommand();
            String DK_Duyet = ReportModels.DieuKien_TrangThaiDuyet;
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "0")
            {
                DK_Duyet = "";
            }
            else
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            SQL = String.Format(@"SELECT COUNT(a.iID_MaDotPhanBo)
                                FROM (SELECT DISTINCT iID_MaDotPhanBo,dNgayDotPhanBo FROM PB_PhanBo
		                        WHERE dNgayDotPhanBo <= (SELECT DISTINCT dNgayDotPhanBo FROM PB_DotPhanBo 
								WHERE iID_MaDotPhanBo=@iID_MaDotPhanBo {0} AND iTrangThai=1)) as B
                                INNER JOIN (SELECT DISTINCT iID_MaDotPhanBo FROM PB_PhanBoChiTiet
			                                WHERE iTrangThai=1 {0} {1}) AS A ON A.iID_MaDotPhanBo=B.iID_MaDotPhanBo
                                ", ReportModels.DieuKien_NganSach(MaND),DK_Duyet);
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaDotPhanBo", iID_MaDotPhanBo);
            int STT = Convert.ToInt16(Connection.GetValue(cmd, 1));
            cmd.Dispose();
            return STT;
        }
        /// <summary>
        /// Lấy giá trị ngày tháng năm hiên tại
        /// </summary>
        /// <returns></returns>
        public static String Ngay_Thang_Nam_HienTai()
        {
            String Ngay = "";
            int day = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            Ngay = "Ngày  " + day + " tháng  " + month + "  năm  " + year;
            return Ngay;
        }
        public static String Thang_Nam_HienTai()
        {
            String Ngay = "";
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            Ngay = " tháng  " + month + "  năm  " + year;
            return Ngay;
        }
        /// <summary>
        /// Chọn khổ giấy in
        /// </summary>
        /// <returns></returns>
        public static DataTable LoaiKhoGiay()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaKhoGiay", typeof(String));
            dt.Columns.Add("TenKhoGiay", typeof(String));
            DataRow R1 = dt.NewRow();
            dt.Rows.Add(R1);
            R1[0] = "1";
            R1[1] = "In khổ giấy A3";
            DataRow R2 = dt.NewRow();
            dt.Rows.Add(R2);
            R2[0] = "2";
            R2[1] = "In khổ giấy A4";
            dt.Dispose();
            return dt;
        }
        /// <summary>
        /// Tên đơn vị sử dụng
        /// </summary>
        /// <param name="LoaiDonVi">1: Đơn vị cấp 1 2:Đơn vị cấp 2 3:Đơn vị cấp 3</param>
        /// <returns></returns>
        public static String CauHinhTenDonViSuDung(int LoaiDonVi)
        {
            String Ten = "";
            if (LoaiDonVi == 1)
            {
                Ten = LayDonviSD("KT_DanhMucThamSo", "sKyHieu", "91", "sThamSo").ToUpper();
            }
            else if (LoaiDonVi == 2)
            {
                Ten = LayDonviSD("KT_DanhMucThamSo", "sKyHieu", "92", "sThamSo").ToUpper();
            }
            else
            {
                Ten = LayDonviSD("KT_DanhMucThamSo", "sKyHieu", "93", "sThamSo").ToUpper();
            }
            return Ten;
        }
        public static String CauHinhTenDonViSuDung(int LoaiDonVi,String MaND)
        {
            String Ten = "";
            if (LoaiDonVi == 1)
            {
                Ten = LayDonviSD("KT_DanhMucThamSo", "sKyHieu", "91", "sThamSo", MaND).ToUpper();
            }
            else if (LoaiDonVi == 2)
            {
                Ten = LayDonviSD("KT_DanhMucThamSo", "sKyHieu", "92", "sThamSo", MaND).ToUpper();
            }
            else
            {
                Ten = LayDonviSD("KT_DanhMucThamSo", "sKyHieu", "93", "sThamSo", MaND).ToUpper();
            }
            return Ten;
        }
        /// <summary>
        /// Lấy danh sách loại ngân sách
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DtLoaiNganSach()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String SQL = string.Format(@"SELECT sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach WHERE LEN(sLNS)=7 AND sL = '' ORDER By sXauNoiMa");
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
        /// <summary>
        /// hàm lấy mô tả loại ngân sách
        /// </summary>
        /// <param name="sLNS"></param>
        /// <returns></returns>
        public static DataTable MoTa(string sLNS)
        {
            DataTable dt = null;
            string SQL = "SELECT TOP 1 sMoTa FROM NS_MucLucNganSach WHERE sLNS=@sLNS ORDER BY sXauNoiMa";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@sLNS", sLNS);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// danh sách loại ngân sách người có công
        /// </summary>
        /// <param name="All"></param>
        /// <returns></returns>
        public static DataTable NS_LoaiNganSachNguoiCoCong(Boolean All = false)
        {
            DataTable dt;
            SqlCommand cmd = new SqlCommand(@" SELECT   sLNS, sLNS +' - '+ sMoTa AS TenHT FROM NS_MucLucNganSach 
                                                WHERE LEN(sLNS)=7 AND LEFT(sLNS,3) = '206' AND sL = '' ORDER BY sXauNoiMa");
            dt = Connection.GetDataTable(cmd);
            if (All)
            {
                DataRow R = dt.NewRow();
                R["sLNS"] = "";
                R["TenHT"] = "---Danh sách loại ngân sách---";
                dt.Rows.InsertAt(R, 0);
            }
            cmd.Dispose();
            return dt;
        }
        /// <summary>
        /// Lấy đơn vị sử dụng
        /// </summary>
        /// <param name="TenBang">Tên bảng</param>
        /// <param name="DK">Tên trường tìm kiếm</param>
        /// <param name="GiaTri">Giá trị trường tìm kiếm</param>
        /// <param name="Result">Tên trường cấn lấy giá trị</param>
        /// <returns></returns>
        public static String LayDonviSD(String TenBang, String DK, String GiaTri, String Result)
        {
            String kq = "";
            String SQL = String.Format(@"SELECT TOP 1 {0}
                                        FROM {1}
                                        WHERE {2}={3} AND iNamLamViec=@iNamLamViec
                                        GROUP BY {0} ", Result, TenBang, DK, GiaTri);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                kq = " ";
            }
            else
            {
                kq = dt.Rows[0][Result].ToString();
            }
            return kq;
        }
        public static String LayDonviSD(String TenBang, String DK, String GiaTri, String Result,String MaND)
        {
            String kq = "";
            String SQL = String.Format(@"SELECT TOP 1 {0}
                                        FROM {1}
                                        WHERE {2}={3} AND iNamLamViec=@iNamLamViec
                                        GROUP BY {0} ", Result, TenBang, DK, GiaTri);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count == 0)
            {
                kq = " ";
            }
            else
            {
                kq = dt.Rows[0][Result].ToString();
            }
            return kq;
        }
        public static String LayNamLamViec(String MaND)
        {
            DataTable dt = NguoiDungCauHinhModels.LayCauHinh(MaND);
            String iNamLamViec = DateTime.Now.Year.ToString();
            if (dt.Rows.Count > 0)
            {
                iNamLamViec = Convert.ToString(dt.Rows[0]["iNamLamViec"]);
            }
            return iNamLamViec;
        }
        #region báo cáo QT
        public static XlsFile TaoTieuDe(XlsFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotTrang1, int SoCotTrangLonHon1, Boolean NghiepVu = false)
        {
            xls.NewFile(1);    //Create a new Excel file with 1 sheet.

            //Set the names of the sheets
            xls.ActiveSheet = 1;
            xls.SheetName = "Sheet1";

            xls.ActiveSheet = 1;    //Set the sheet we are working in.

            //Global Workbook Options
            xls.OptionsAutoCompressPictures = true;

            #region //Styles.
            TFlxFormat StyleFmt;
            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0_);_(\"$\"* \\(#,##0\\);_(\"$\"* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2), StyleFmt);
            #endregion
            #region  //Named Ranges

            TXlsNamedRange Range;
            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 1, 32, "='Sheet1'!$A:$G,'Sheet1'!$3:$5");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            xls.SetNamedRange(Range);

            #endregion
            #region //Printer Settings
            THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
            HeadersAndFooters.AlignMargins = true;
            HeadersAndFooters.ScaleWithDoc = true;
            HeadersAndFooters.DiffFirstPage = true;
            HeadersAndFooters.DiffEvenPages = false;
            HeadersAndFooters.DefaultFooter = "&RTrang: &P/&N";
            if (NghiepVu == true)
            {
                HeadersAndFooters.FirstHeader = "&L&\"Times New Roman,Bold\"<#QuanKhu>\n<#Phong>\n&C&\"Times New Roman,Bold\"TỔNG HỢP QUYẾT TOÁN <#sLNS> - PHẦN <#TruongTien>\n <#LoaiThangQuy> <#Thang> năm <#Nam>";
            }

            else
            {
                HeadersAndFooters.FirstHeader = "&L&\"Times New Roman,Bold\"<#QuanKhu>\n<#Phong>\n&C&\"Times New Roman,Bold\"TỔNG HỢP QUYẾT TOÁN LƯƠNG,PHỤ CẤP TIỀN ĂN\n Tháng <#Thang> năm <#Nam>";

            }
            HeadersAndFooters.FirstFooter = "&RTrang: &P/&N";
            HeadersAndFooters.EvenHeader = "";
            HeadersAndFooters.EvenFooter = "";
            xls.SetPageHeaderAndFooter(HeadersAndFooters);

            //You can set the margins in 2 ways, the one commented here or the one below:
            //    TXlsMargins PrintMargins = xls.GetPrintMargins();
            //    PrintMargins.Left = 0.196850393700787;
            //    PrintMargins.Top = 0.590551181102362;
            //    PrintMargins.Right = 0.196850393700787;
            //    PrintMargins.Bottom = 0.748031496062992;
            //    PrintMargins.Header = 0.31496062992126;
            //    PrintMargins.Footer = 0.31496062992126;
            //    xls.SetPrintMargins(PrintMargins);
            xls.SetPrintMargins(new TXlsMargins(0.196850393700787, 0.590551181102362, 0.196850393700787, 0.748031496062992, 0.31496062992126, 0.31496062992126));
            xls.PrintXResolution = 600;
            xls.PrintYResolution = 600;
            xls.PrintOptions = TPrintOptions.None;
            xls.PrintPaperSize = TPaperSize.A4;

            //Printer Driver Settings are a blob of data specific to a printer
            //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
            //    byte[] PrinterData = {
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x00, 0x06, 0xDC, 0x00, 0x00, 0x00, 0x03, 0xFF, 0x00, 0x00, 0x02, 0x00, 0x09, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x64, 0x00, 0x01, 0x00, 0x0F, 0x00, 0x58, 0x02, 0x02, 0x00, 0x01, 0x00, 0x58, 0x02, 
            //        0x02, 0x00, 0x00, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x74, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 
            //        0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    };
            //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
            //    xls.SetPrinterDriverSettings(PrinterDriverSettings);
            #endregion
            int TongSoHang = dt.Rows.Count;
            int _TuCot = TuCot;
            int TongSoCot = 0;
            int SoTrang = 1;
            if ((DenCotCua_DT - TuCotCua_DT) <= SoCotTrang1)
            {
                int SoCotDu = ((DenCotCua_DT - TuCotCua_DT)) % SoCotTrang1;
                int SoCotCanThem = 0;


                SoCotCanThem = SoCotTrang1 - SoCotDu;

                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (DenCotCua_DT - TuCotCua_DT - SoCotTrang1) % SoCotTrangLonHon1;
                int SoCotCanThem = 0;


                SoCotCanThem = SoCotTrangLonHon1 - SoCotDu;
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;

                SoTrang = 1 + (TongSoCot - SoCotTrang1) / SoCotTrangLonHon1;
            }
            #region //Set up rows and columns
            xls.DefaultColWidth = 2340;
            xls.SetColWidth(1, 950);    //(2.82 + 0.75) * 256

            TFlxFormat ColFmt;
            ColFmt = xls.GetFormat(xls.GetColFormat(1));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(1, xls.AddFormat(ColFmt));
            xls.SetColWidth(2, 950);    //(2.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(2));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(2, xls.AddFormat(ColFmt));
            xls.SetColWidth(3, 1000);    //(2.96 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(3));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(3, xls.AddFormat(ColFmt));
            xls.SetColWidth(4, 1000);    //(2.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(4));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(4, xls.AddFormat(ColFmt));
            xls.SetColWidth(5, 900);    //(2.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(5));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(5, xls.AddFormat(ColFmt));
            xls.SetColWidth(6, 800);    //(2.39 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(6));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(6, xls.AddFormat(ColFmt));
            xls.SetColWidth(7, 7300);    //(27.68 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(7));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(7, xls.AddFormat(ColFmt));
            xls.SetColWidth(8, 3400);    //(12.39 + 0.75) * 256 

            for (int i = 0; i < TongSoCot; i++)
            {
                xls.SetColWidth(_TuCot + i, 3400);
            }
            xls.SetRowHeight(4, 600);
            xls.SetRowHeight(5, 600);
            xls.DefaultRowHeight = 300;
            #endregion

            #region//Merged Cells
            xls.MergeCells(4, 1, 5, 6);
            xls.MergeCells(4, 7, 5, 7);
            xls.MergeCells(4, 8, 5, 8);
            _TuCot = TuCot;
            if (SoTrang == 1)
            {
                xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrang1 - 1);
            }
            else
            {
                xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrang1 - 1);
                _TuCot = _TuCot + SoCotTrang1;
                for (int i = 1; i < SoTrang; i++)
                {
                    xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrangLonHon1 - 1);
                    _TuCot = _TuCot + SoCotTrangLonHon1;
                }
            }
            #endregion
            #region //Set the cell values
            #region set tieu de cot tinh
            TFlxFormat fmt;
            fmt = xls.GetCellVisibleFormatDef(1, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 1, xls.AddFormat(fmt));
            xls.SetCellValue(1, 1, "<#auto page breaks>");
            fmt = xls.GetCellVisibleFormatDef(4, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 1, xls.AddFormat(fmt));
            xls.SetCellValue(4, 1, "L-K-M-TM-TTM-NG");

            fmt = xls.GetCellVisibleFormatDef(4, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 7, xls.AddFormat(fmt));
            xls.SetCellValue(4, 7, "Nội dung");

            fmt = xls.GetCellVisibleFormatDef(4, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 8, xls.AddFormat(fmt));
            xls.SetCellValue(4, 8, "Tổng cộng");

            fmt = xls.GetCellVisibleFormatDef(5, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 1, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 7, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 8, xls.AddFormat(fmt));
            #endregion
            #region set tieu de cot dong
            #region set hang trongdo va donvitinh
            _TuCot = TuCot;

            //set trang 1
            fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Italic;
            fmt.Font.Family = 1;
            xls.SetCellFormat(3, _TuCot + 4, xls.AddFormat(fmt));
            xls.SetCellValue(3, _TuCot + 4, "Đơn vị tính: đồng    Tờ số 1");

            fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, _TuCot, xls.AddFormat(fmt));
            xls.SetCellValue(4, _TuCot, "Trong đó");
            for (int j = _TuCot + 1; j <= _TuCot + SoCotTrang1 - 1; j++)
            {
                fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(4, j, xls.AddFormat(fmt));
            }
            _TuCot = _TuCot + SoCotTrang1;
            //set cac trang con lai            
            for (int i = 1; i < SoTrang; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 5);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Italic;
                fmt.Font.Family = 1;
                xls.SetCellFormat(3, _TuCot + 5, xls.AddFormat(fmt));
                xls.SetCellValue(3, _TuCot + 5, "Đơn vị tính đồng   Tờ số " + Convert.ToInt16(i + 1));

                fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(4, _TuCot, xls.AddFormat(fmt));
                xls.SetCellValue(4, _TuCot, "Trong đó");
                for (int j = _TuCot + 1; j <= _TuCot + SoCotTrangLonHon1 - 1; j++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 200;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(4, j, xls.AddFormat(fmt));
                }
                _TuCot = _TuCot + SoCotTrangLonHon1;

            }
            #endregion

            #region set cac cot don vi
            _TuCot = TuCot;
            String TenCot;
            int _TuCotCua_DT = TuCotCua_DT;
            for (int i = 0; i < SoCotTrang1; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(5, _TuCot + i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                TenCot = "";
                if (DenCotCua_DT > _TuCotCua_DT)
                {
                    TenCot = "<#TenDV" + i + ">";
                }
                xls.SetCellFormat(5, _TuCot + i, xls.AddFormat(fmt));
                xls.SetCellValue(5, _TuCot + i, TenCot);
            }
            _TuCotCua_DT = _TuCotCua_DT + SoCotTrang1;
            _TuCot = _TuCot + SoCotTrang1;
            for (int i = 0; i < TongSoCot - SoCotTrang1; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(5, _TuCot + i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                TenCot = "";
                int a = Convert.ToInt16(SoCotTrang1) + i;
                if (DenCotCua_DT > _TuCotCua_DT)
                {
                    TenCot = "<#TenDV" + a + ">";
                }
                xls.SetCellFormat(5, _TuCot + i, xls.AddFormat(fmt));
                xls.SetCellValue(5, _TuCot + i, TenCot);
            }
            #endregion

            #region ngày tháng năm, chữ ký
            //ngaythangnam
            xls.MergeCells(TongSoHang + TuHang + 2, 13, TongSoHang + TuHang + 2, 14);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 13);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 2, 13, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 2, 13, "<#NgayThangNam>");

            //ChuKy
            xls.MergeCells(TongSoHang + TuHang + 3, 1, TongSoHang + TuHang + 3, 6);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 1);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 1, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 1, "<#row height(autofit)><#ThuaLenh1>\n\n<#ChucDanh1>\n\n\n\n\n\n\n\n<#Ten1>");


            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 7);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 7, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 7, "<#row height(autofit)><#ThuaLenh2>\n\n<#ChucDanh2>\n\n\n\n\n\n\n\n<#Ten2>");

            xls.MergeCells(TongSoHang + TuHang + 3, 8, TongSoHang + TuHang + 3, 9);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 8, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 8, "<#row height(autofit)><#ThuaLenh3>\n\n<#ChucDanh3>\n\n\n\n\n\n\n\n<#Ten3>");

            xls.MergeCells(TongSoHang + TuHang + 3, 10, TongSoHang + TuHang + 4, 11);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 10);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 10, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 10, "<#row height(autofit)><#ThuaLenh4>\n\n<#ChucDanh4>\n\n\n\n\n\n\n\n<#Ten4>");

            xls.MergeCells(TongSoHang + TuHang + 3, 13, TongSoHang + TuHang + 3, 14);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 13);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 13, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 13, "<#row height(autofit)><#ThuaLenh5>\n\n<#ChucDanh5>\n\n\n\n\n\n\n\n<#Ten5>");
            for (int i = 1; i < SoTrang; i++)
            {
                xls.MergeCells(TongSoHang + TuHang + 2, 13 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 2, 14 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 13 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(TongSoHang + TuHang + 2, 13 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 2, 13 + SoCotTrangLonHon1 * i, "<#NgayThangNam>");

                xls.MergeCells(TongSoHang + TuHang + 3, 8 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 2, 9 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 8 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 2, 8 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 2, 8 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh3>\n\n<#ChucDanh3>\n\n\n\n\n\n\n\n<#Ten3>");

                xls.MergeCells(TongSoHang + TuHang + 3, 10 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 11 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 10 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 10 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 10 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh4>\n\n<#ChucDanh4>\n\n\n\n\n\n\n\n<#Ten4>");

                xls.MergeCells(TongSoHang + TuHang + 3, 13 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 13 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 13 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 13 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh5>\n\n<#ChucDanh5>\n\n\n\n\n\n\n\n<#Ten5>");
            }
            #endregion
            #endregion
            //Cell selection and scroll position.
            xls.SelectCell(11, 7, false);
            #endregion
            //Protection

            TSheetProtectionOptions SheetProtectionOptions;
            SheetProtectionOptions = new TSheetProtectionOptions(false);
            SheetProtectionOptions.SelectLockedCells = true;
            SheetProtectionOptions.SelectUnlockedCells = true;
            xls.Protection.SetSheetProtection(null, SheetProtectionOptions);
            return xls;

        }
        public static XlsFile TaoTieuDe_A3(XlsFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotTrang1, int SoCotTrangLonHon1, Boolean NghiepVu = false)
        {
            xls.NewFile(1);    //Create a new Excel file with 1 sheet.

            //Set the names of the sheets
            xls.ActiveSheet = 1;
            xls.SheetName = "Sheet1";

            xls.ActiveSheet = 1;    //Set the sheet we are working in.

            //Global Workbook Options
            xls.OptionsAutoCompressPictures = true;

            #region //Styles.
            TFlxFormat StyleFmt;
            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0_);_(\"$\"* \\(#,##0\\);_(\"$\"* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2), StyleFmt);
            #endregion
            #region  //Named Ranges

            TXlsNamedRange Range;
            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 1, 32, "='Sheet1'!$A:$G,'Sheet1'!$3:$5");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            xls.SetNamedRange(Range);

            #endregion
            #region //Printer Settings
            THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
            HeadersAndFooters.AlignMargins = true;
            HeadersAndFooters.ScaleWithDoc = true;
            HeadersAndFooters.DiffFirstPage = true;
            HeadersAndFooters.DiffEvenPages = false;
            HeadersAndFooters.DefaultFooter = "&RTrang: &P/&N";
            if (NghiepVu == true)
            {
                HeadersAndFooters.FirstHeader = "&L&\"Times New Roman,Bold\"<#QuanKhu>\n<#Phong>\n&C&\"Times New Roman,Bold\"TỔNG HỢP QUYẾT TOÁN <#sLNS> - PHẦN <#TruongTien>\n <#LoaiThangQuy> <#Thang> năm <#Nam>";
            }

            else
            {
                HeadersAndFooters.FirstHeader = "&L&\"Times New Roman,Bold\"<#QuanKhu>\n<#Phong>\n&C&\"Times New Roman,Bold\"TỔNG HỢP QUYẾT TOÁN LƯƠNG,PHỤ CẤP TIỀN ĂN\n Tháng <#Thang> năm <#Nam>";

            }

            HeadersAndFooters.FirstFooter = "&RTrang: &P/&N";
            HeadersAndFooters.EvenHeader = "";
            HeadersAndFooters.EvenFooter = "";
            xls.SetPageHeaderAndFooter(HeadersAndFooters);

            //You can set the margins in 2 ways, the one commented here or the one below:
            //    TXlsMargins PrintMargins = xls.GetPrintMargins();
            //    PrintMargins.Left = 0.196850393700787;
            //    PrintMargins.Top = 0.590551181102362;
            //    PrintMargins.Right = 0.196850393700787;
            //    PrintMargins.Bottom = 0.748031496062992;
            //    PrintMargins.Header = 0.31496062992126;
            //    PrintMargins.Footer = 0.31496062992126;
            //    xls.SetPrintMargins(PrintMargins);
            xls.SetPrintMargins(new TXlsMargins(0.393700787401575, 0.590551181102362, 0.393700787401575, 0.748031496062992, 0.31496062992126, 0.31496062992126));
            xls.PrintXResolution = 600;
            xls.PrintYResolution = 600;
            xls.PrintOptions = TPrintOptions.None;
            xls.PrintPaperSize = TPaperSize.A3;

            //Printer Driver Settings are a blob of data specific to a printer
            //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
            //    byte[] PrinterData = {
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x00, 0x06, 0xDC, 0x00, 0x00, 0x00, 0x03, 0xFF, 0x00, 0x00, 0x02, 0x00, 0x09, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x64, 0x00, 0x01, 0x00, 0x0F, 0x00, 0x58, 0x02, 0x02, 0x00, 0x01, 0x00, 0x58, 0x02, 
            //        0x02, 0x00, 0x00, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x74, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 
            //        0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    };
            //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
            //    xls.SetPrinterDriverSettings(PrinterDriverSettings);
            #endregion
            int TongSoHang = dt.Rows.Count;
            int _TuCot = TuCot;
            int TongSoCot = 0;
            int SoTrang = 1;
            if ((DenCotCua_DT - TuCotCua_DT) <= SoCotTrang1)
            {
                int SoCotDu = ((DenCotCua_DT - TuCotCua_DT)) % SoCotTrang1;
                int SoCotCanThem = 0;
                SoCotCanThem = SoCotTrang1 - SoCotDu;
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (DenCotCua_DT - TuCotCua_DT - SoCotTrang1) % SoCotTrangLonHon1;
                int SoCotCanThem = 0;
                SoCotCanThem = SoCotTrangLonHon1 - SoCotDu;
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
                SoTrang = 1 + (TongSoCot - SoCotTrang1) / SoCotTrangLonHon1;
            }
            #region //Set up rows and columns
            xls.DefaultColWidth = 2340;
            xls.SetColWidth(1, 950);    //(2.82 + 0.75) * 256

            TFlxFormat ColFmt;
            ColFmt = xls.GetFormat(xls.GetColFormat(1));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(1, xls.AddFormat(ColFmt));
            xls.SetColWidth(2, 950);    //(2.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(2));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(2, xls.AddFormat(ColFmt));
            xls.SetColWidth(3, 1000);    //(2.96 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(3));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(3, xls.AddFormat(ColFmt));
            xls.SetColWidth(4, 1000);    //(2.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(4));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(4, xls.AddFormat(ColFmt));
            xls.SetColWidth(5, 900);    //(2.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(5));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(5, xls.AddFormat(ColFmt));
            xls.SetColWidth(6, 800);    //(2.39 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(6));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(6, xls.AddFormat(ColFmt));
            xls.SetColWidth(7, 8000);    //(27.68 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(7));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(7, xls.AddFormat(ColFmt));
            xls.SetColWidth(8, 3400);    //(12.39 + 0.75) * 256 

            for (int i = 0; i < TongSoCot; i++)
            {
                xls.SetColWidth(_TuCot + i, 3400);
            }
            xls.SetRowHeight(4, 600);
            xls.SetRowHeight(5, 600);
            xls.DefaultRowHeight = 300;
            #endregion

            #region//Merged Cells
            xls.MergeCells(4, 1, 5, 6);
            xls.MergeCells(4, 7, 5, 7);
            xls.MergeCells(4, 8, 5, 8);
            _TuCot = TuCot;
            if (SoTrang == 1)
            {
                xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrang1 - 1);
            }
            else
            {
                xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrang1 - 1);
                _TuCot = _TuCot + SoCotTrang1;
                for (int i = 1; i < SoTrang; i++)
                {
                    xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrangLonHon1 - 1);
                    _TuCot = _TuCot + SoCotTrangLonHon1;
                }
            }
            #endregion
            #region //Set the cell values
            #region set tieu de cot tinh
            TFlxFormat fmt;

            fmt = xls.GetCellVisibleFormatDef(1, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 1, xls.AddFormat(fmt));
            xls.SetCellValue(1, 1, "<#auto page breaks>");

            fmt = xls.GetCellVisibleFormatDef(4, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 1, xls.AddFormat(fmt));
            xls.SetCellValue(4, 1, "L-K-M-TM-TTM-NG");

            fmt = xls.GetCellVisibleFormatDef(4, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 7, xls.AddFormat(fmt));
            xls.SetCellValue(4, 7, "Nội dung");

            fmt = xls.GetCellVisibleFormatDef(4, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 8, xls.AddFormat(fmt));
            xls.SetCellValue(4, 8, "Tổng cộng");

            fmt = xls.GetCellVisibleFormatDef(5, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 1, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 7, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 8, xls.AddFormat(fmt));
            #endregion
            #region set tieu de cot dong
            #region set hang trongdo va donvitinh
            _TuCot = TuCot;

            //set trang 1
            fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Italic;
            fmt.Font.Family = 1;
            xls.SetCellFormat(3, _TuCot + 8, xls.AddFormat(fmt));
            xls.SetCellValue(3, _TuCot + 8, "Đơn vị tính: đồng    Tờ số 1");

            fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, _TuCot, xls.AddFormat(fmt));
            xls.SetCellValue(4, _TuCot, "Trong đó");
            for (int j = _TuCot + 1; j <= _TuCot + SoCotTrang1 - 1; j++)
            {
                fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(4, j, xls.AddFormat(fmt));
            }
            _TuCot = _TuCot + SoCotTrang1;
            //set cac trang con lai            
            for (int i = 1; i < SoTrang; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 9);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Italic;
                fmt.Font.Family = 1;
                xls.SetCellFormat(3, _TuCot + 9, xls.AddFormat(fmt));
                xls.SetCellValue(3, _TuCot + 9, "Đơn vị tính đồng   Tờ số " + Convert.ToInt16(i + 1));

                fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(4, _TuCot, xls.AddFormat(fmt));
                xls.SetCellValue(4, _TuCot, "Trong đó");
                for (int j = _TuCot + 1; j <= _TuCot + SoCotTrangLonHon1 - 1; j++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 200;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(4, j, xls.AddFormat(fmt));
                }
                _TuCot = _TuCot + SoCotTrangLonHon1;

            }
            #endregion

            #region set cac cot don vi
            _TuCot = TuCot;
            String TenCot;
            int _TuCotCua_DT = TuCotCua_DT;
            for (int i = 0; i < SoCotTrang1; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(5, _TuCot + i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                TenCot = "";
                if (DenCotCua_DT > _TuCotCua_DT)
                {
                    TenCot = "<#TenDV" + i + ">";
                }
                xls.SetCellFormat(5, _TuCot + i, xls.AddFormat(fmt));
                xls.SetCellValue(5, _TuCot + i, TenCot);
            }
            _TuCotCua_DT = _TuCotCua_DT + SoCotTrang1;
            _TuCot = _TuCot + SoCotTrang1;
            for (int i = 0; i < TongSoCot - SoCotTrang1; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(5, _TuCot + i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                TenCot = "";
                int a = Convert.ToInt16(SoCotTrang1) + i;
                if (DenCotCua_DT > _TuCotCua_DT)
                {
                    TenCot = "<#TenDV" + a + ">";
                }
                xls.SetCellFormat(5, _TuCot + i, xls.AddFormat(fmt));
                xls.SetCellValue(5, _TuCot + i, TenCot);
            }
            #endregion

            #region ngày tháng năm, chữ ký
            //ngaythangnam
            xls.MergeCells(TongSoHang + TuHang + 2, 17, TongSoHang + TuHang + 2, 18);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 17);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 2, 17, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 2, 17, "<#NgayThangNam>");

            //ChuKy               
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 7);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 7, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 7, "<#row height(autofit)><#ThuaLenh1>\n\n<#ChucDanh1>\n\n\n\n\n\n\n\n<#Ten1>");

            xls.MergeCells(TongSoHang + TuHang + 3, 8, TongSoHang + TuHang + 3, 9);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 8);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 8, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 8, "<#row height(autofit)><#ThuaLenh2>\n\n<#ChucDanh2>\n\n\n\n\n\n\n\n<#Ten2>");

            xls.MergeCells(TongSoHang + TuHang + 3, 11, TongSoHang + TuHang + 3, 12);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 11);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 11, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 11, "<#row height(autofit)><#ThuaLenh3>\n\n<#ChucDanh3>\n\n\n\n\n\n\n\n<#Ten3>");

            xls.MergeCells(TongSoHang + TuHang + 3, 14, TongSoHang + TuHang + 4, 15);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 14);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 14, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 14, "<#row height(autofit)><#ThuaLenh4>\n\n<#ChucDanh4>\n\n\n\n\n\n\n\n<#Ten4>");

            xls.MergeCells(TongSoHang + TuHang + 3, 17, TongSoHang + TuHang + 3, 18);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 17);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 17, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 17, "<#row height(autofit)><#ThuaLenh5>\n\n<#ChucDanh5>\n\n\n\n\n\n\n\n<#Ten5>");
            for (int i = 1; i < SoTrang; i++)
            {
                xls.MergeCells(TongSoHang + TuHang + 2, 17 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 2, 18 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 17 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(TongSoHang + TuHang + 2, 17 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 2, 17 + SoCotTrangLonHon1 * i, "<#NgayThangNam>");


                xls.MergeCells(TongSoHang + TuHang + 3, 8 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 9 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 8 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 8 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 8 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh2>\n\n<#ChucDanh2>\n\n\n\n\n\n\n\n<#Ten2>"); xls.MergeCells(TongSoHang + TuHang + 3, 8 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 2, 9 + SoCotTrangLonHon1 * i);

                xls.MergeCells(TongSoHang + TuHang + 3, 11 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 12 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 11 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 11 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 11 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh3>\n\n<#ChucDanh3>\n\n\n\n\n\n\n\n<#Ten3>");

                xls.MergeCells(TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 15 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh4>\n\n<#ChucDanh4>\n\n\n\n\n\n\n\n<#Ten4>");

                xls.MergeCells(TongSoHang + TuHang + 3, 17 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 18 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 17 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 17 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 17 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh5>\n\n<#ChucDanh5>\n\n\n\n\n\n\n\n<#Ten5>");
            }
            #endregion
            #endregion
            //Cell selection and scroll position.
            xls.SelectCell(11, 7, false);
            #endregion
            //Protection

            TSheetProtectionOptions SheetProtectionOptions;
            SheetProtectionOptions = new TSheetProtectionOptions(false);
            SheetProtectionOptions.SelectLockedCells = true;
            SheetProtectionOptions.SelectUnlockedCells = true;
            xls.Protection.SetSheetProtection(null, SheetProtectionOptions);
            return xls;

        }
        public static XlsFile TaoTieuDeLuyKe(XlsFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotTrang1, int SoCotTrangLonHon1, Boolean NghiepVu = false)
        {
            xls.NewFile(1);    //Create a new Excel file with 1 sheet.

            //Set the names of the sheets
            xls.ActiveSheet = 1;
            xls.SheetName = "Sheet1";

            xls.ActiveSheet = 1;    //Set the sheet we are working in.

            //Global Workbook Options
            xls.OptionsAutoCompressPictures = true;

            #region Styles.
            TFlxFormat StyleFmt;
            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0_);_(\"$\"* \\(#,##0\\);_(\"$\"* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2), StyleFmt);
            #endregion
            #region //Named Ranges


            TXlsNamedRange Range;
            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 1, 32, "='Sheet1'!$A:$G,'Sheet1'!$3:$5");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            xls.SetNamedRange(Range);
            #endregion
            #region//Printer Settings
            THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
            HeadersAndFooters.AlignMargins = true;
            HeadersAndFooters.ScaleWithDoc = true;
            HeadersAndFooters.DiffFirstPage = true;
            HeadersAndFooters.DiffEvenPages = false;
            HeadersAndFooters.DefaultHeader = "";
            HeadersAndFooters.DefaultFooter = "&RTrang: &P/&N";
            if (NghiepVu == true)
            {
                HeadersAndFooters.FirstHeader = "&L&\"Times New Roman,Bold\"<#QuanKhu>\n<#Phong>\n&C&\"Times New Roman,Bold\"TỔNG HỢP QUYẾT TOÁN <#sLNS> - PHẦN <#TruongTien>\n <#LoaiThangQuy> <#Thang> năm <#Nam>";
            }

            else
            {
                HeadersAndFooters.FirstHeader = "&L&\"Times New Roman,Bold\"<#QuanKhu>\n<#Phong>\n&C&\"Times New Roman,Bold\"TỔNG HỢP QUYẾT TOÁN LƯƠNG,PHỤ CẤP TIỀN ĂN\n Tháng <#Thang> năm <#Nam>";

            }
            HeadersAndFooters.FirstFooter = "&RTrang: &P/&N";
            HeadersAndFooters.EvenHeader = "";
            HeadersAndFooters.EvenFooter = "";
            xls.SetPageHeaderAndFooter(HeadersAndFooters);

            //You can set the margins in 2 ways, the one commented here or the one below:
            //    TXlsMargins PrintMargins = xls.GetPrintMargins();
            //    PrintMargins.Left = 0.196850393700787;
            //    PrintMargins.Top = 0.590551181102362;
            //    PrintMargins.Right = 0.196850393700787;
            //    PrintMargins.Bottom = 0.748031496062992;
            //    PrintMargins.Header = 0.31496062992126;
            //    PrintMargins.Footer = 0.31496062992126;
            //    xls.SetPrintMargins(PrintMargins);
            xls.SetPrintMargins(new TXlsMargins(0.196850393700787, 0.590551181102362, 0.196850393700787, 0.748031496062992, 0.31496062992126, 0.31496062992126));
            xls.PrintXResolution = 600;
            xls.PrintYResolution = 600;
            xls.PrintOptions = TPrintOptions.None;
            xls.PrintPaperSize = TPaperSize.A4;

            //Printer Driver Settings are a blob of data specific to a printer
            //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
            //    byte[] PrinterData = {
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x00, 0x06, 0xDC, 0x00, 0x00, 0x00, 0x03, 0xAF, 0x00, 0x00, 0x02, 0x00, 0x09, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x64, 0x00, 0x01, 0x00, 0x0F, 0x00, 0x58, 0x02, 0x02, 0x00, 0x01, 0x00, 0x58, 0x02, 
            //        0x03, 0x00, 0x01, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x74, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 
            //        0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    };
            //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
            //    xls.SetPrinterDriverSettings(PrinterDriverSettings);
            #endregion
            int TongSoHang = dt.Rows.Count;
            int _TuCot = TuCot;
            int TongSoCot = 0;
            int SoTrang = 1;
            if ((DenCotCua_DT - TuCotCua_DT) <= SoCotTrang1)
            {
                int SoCotDu = ((DenCotCua_DT - TuCotCua_DT)) % SoCotTrang1;
                int SoCotCanThem = 0;
                SoCotCanThem = SoCotTrang1 - SoCotDu;
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (DenCotCua_DT - TuCotCua_DT - SoCotTrang1) % SoCotTrangLonHon1;
                int SoCotCanThem = 0;
                SoCotCanThem = SoCotTrangLonHon1 - SoCotDu;
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
                SoTrang = 1 + (TongSoCot - SoCotTrang1) / SoCotTrangLonHon1;
            }
            #region//Set up rows and columns
            xls.DefaultColWidth = 2340;
            xls.SetColWidth(1, 950);    //(3.25 + 0.75) * 256

            TFlxFormat ColFmt;
            ColFmt = xls.GetFormat(xls.GetColFormat(1));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(1, xls.AddFormat(ColFmt));
            xls.SetColWidth(2, 950);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(2));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(2, xls.AddFormat(ColFmt));
            xls.SetColWidth(3, 1000);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(3));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(3, xls.AddFormat(ColFmt));
            xls.SetColWidth(4, 1000);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(4));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(4, xls.AddFormat(ColFmt));
            xls.SetColWidth(5, 900);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(4));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(5, xls.AddFormat(ColFmt));
            xls.SetColWidth(6, 900);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(5));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(6, xls.AddFormat(ColFmt));
            xls.SetColWidth(7, 7300);    //(27.54 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(6));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(7, xls.AddFormat(ColFmt));
            xls.SetColWidth(8, 3400);    //(12.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(7));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(8, xls.AddFormat(ColFmt));
            xls.SetColWidth(9, 3400);    //(12.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(8));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(9, xls.AddFormat(ColFmt));
            xls.SetColWidth(10, 3400);    //(12.82 + 0.75) * 256

            for (int i = 0; i < TongSoCot; i++)
            {
                xls.SetColWidth(_TuCot + i, 3400);
            }
            xls.SetRowHeight(4, 600);
            xls.SetRowHeight(5, 600);
            xls.DefaultRowHeight = 300;
            #endregion

            #region //Merged Cells
            xls.MergeCells(4, 1, 5, 6);
            xls.MergeCells(4, 7, 5, 7);
            xls.MergeCells(4, 8, 5, 8);
            xls.MergeCells(4, 9, 5, 9);
            xls.MergeCells(4, 10, 5, 10);
            _TuCot = TuCot;
            if (SoTrang == 1)
            {
                xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrang1 - 1);
            }
            else
            {
                xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrang1 - 1);
                _TuCot = _TuCot + SoCotTrang1;
                for (int i = 1; i < SoTrang; i++)
                {
                    xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrangLonHon1 - 1);
                    _TuCot = _TuCot + SoCotTrangLonHon1;
                }
            }
            #endregion
            //Set the cell values
            #region set tieu de cot tinh
            TFlxFormat fmt;

            fmt = xls.GetCellVisibleFormatDef(1, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 1, xls.AddFormat(fmt));
            xls.SetCellValue(1, 1, "<#auto page breaks>");

            fmt = xls.GetCellVisibleFormatDef(4, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 1, xls.AddFormat(fmt));
            xls.SetCellValue(4, 1, "L-K-M-TM-TTM-NG");

            fmt = xls.GetCellVisibleFormatDef(4, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 7, xls.AddFormat(fmt));
            xls.SetCellValue(4, 7, "Nội dung");

            fmt = xls.GetCellVisibleFormatDef(4, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 8, xls.AddFormat(fmt));
            xls.SetCellValue(4, 8, "Chỉ Tiêu");

            fmt = xls.GetCellVisibleFormatDef(4, 9);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 9, xls.AddFormat(fmt));
            xls.SetCellValue(4, 9, "Cộng trong kỳ");

            fmt = xls.GetCellVisibleFormatDef(4, 10);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 10, xls.AddFormat(fmt));
            xls.SetCellValue(4, 10, "Lũy kế đến");



            fmt = xls.GetCellVisibleFormatDef(5, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 1, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 7, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 8, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 9);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 9, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 10);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 10, xls.AddFormat(fmt));
            #endregion
            #region Set tieu de dong
            #region set hang trongdo va donvitinh
            _TuCot = TuCot;

            //set trang 1
            fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Italic;
            fmt.Font.Family = 1;
            xls.SetCellFormat(3, _TuCot + 2, xls.AddFormat(fmt));
            xls.SetCellValue(3, _TuCot + 2, "Đơn vị tính đồng     Tờ số 1");

            fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, _TuCot, xls.AddFormat(fmt));
            xls.SetCellValue(4, _TuCot, "Trong đó");
            for (int j = _TuCot + 1; j <= _TuCot + SoCotTrang1 - 1; j++)
            {
                fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(4, j, xls.AddFormat(fmt));
            }
            _TuCot = _TuCot + SoCotTrang1;
            //set cac trang con lai            
            for (int i = 1; i < SoTrang; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 5);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Italic;
                fmt.Font.Family = 1;
                xls.SetCellFormat(3, _TuCot + 5, xls.AddFormat(fmt));
                xls.SetCellValue(3, _TuCot + 5, "Đơn vị tính  đồng      Tờ số: " + Convert.ToInt16(i + 1));

                fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(4, _TuCot, xls.AddFormat(fmt));
                xls.SetCellValue(4, _TuCot, "Trong đó");
                for (int j = _TuCot + 1; j <= _TuCot + SoCotTrangLonHon1 - 1; j++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 200;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(4, j, xls.AddFormat(fmt));
                }
                _TuCot = _TuCot + SoCotTrangLonHon1;

            }
            #endregion
            #region set cac cot don vi
            _TuCot = TuCot;
            String TenCot;
            int _TuCotCua_DT = TuCotCua_DT;
            for (int i = 0; i < SoCotTrang1; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(5, _TuCot + i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                TenCot = "";
                if (DenCotCua_DT > _TuCotCua_DT)
                {
                    TenCot = "<#TenDV" + i + ">";
                }
                xls.SetCellFormat(5, _TuCot + i, xls.AddFormat(fmt));
                xls.SetCellValue(5, _TuCot + i, TenCot);
            }
            _TuCotCua_DT = _TuCotCua_DT + SoCotTrang1;
            _TuCot = _TuCot + SoCotTrang1;
            for (int i = 0; i < TongSoCot - SoCotTrang1; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(5, _TuCot + i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                TenCot = "";
                int a = Convert.ToInt16(SoCotTrang1) + i;
                if (DenCotCua_DT > _TuCotCua_DT)
                {
                    TenCot = "<#TenDV" + a + ">";
                }
                xls.SetCellFormat(5, _TuCot + i, xls.AddFormat(fmt));
                xls.SetCellValue(5, _TuCot + i, TenCot);
            }

            #endregion


            //ngaythangnam
            xls.MergeCells(TongSoHang + TuHang + 2, 13, TongSoHang + TuHang + 2, 14);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 13);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 2, 13, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 2, 13, "<#NgayThangNam>");

            //ChuKy
            xls.MergeCells(TongSoHang + TuHang + 3, 1, TongSoHang + TuHang + 3, 6);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 1);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 1, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 1, "<#row height(autofit)><#ThuaLenh1>\n\n<#ChucDanh1>\n\n\n\n\n\n\n\n<#Ten1>");


            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 7);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 7, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 7, "<#row height(autofit)><#ThuaLenh2>\n\n<#ChucDanh2>\n\n\n\n\n\n\n\n<#Ten2>");

            xls.MergeCells(TongSoHang + TuHang + 3, 8, TongSoHang + TuHang + 3, 9);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 8, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 8, "<#row height(autofit)><#ThuaLenh3>\n\n<#ChucDanh3>\n\n\n\n\n\n\n\n<#Ten3>");

            xls.MergeCells(TongSoHang + TuHang + 3, 10, TongSoHang + TuHang + 4, 11);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 10);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 10, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 10, "<#row height(autofit)><#ThuaLenh4>\n\n<#ChucDanh4>\n\n\n\n\n\n\n\n<#Ten4>");

            xls.MergeCells(TongSoHang + TuHang + 3, 13, TongSoHang + TuHang + 3, 14);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 13);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 13, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 13, "<#row height(autofit)><#ThuaLenh5>\n\n<#ChucDanh5>\n\n\n\n\n\n\n\n<#Ten5>");
            for (int i = 1; i < SoTrang; i++)
            {
                xls.MergeCells(TongSoHang + TuHang + 2, 13 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 2, 14 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 13 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(TongSoHang + TuHang + 2, 13 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 2, 13 + SoCotTrangLonHon1 * i, "<#NgayThangNam>");

                xls.MergeCells(TongSoHang + TuHang + 3, 8 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 2, 9 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 8 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 2, 8 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 2, 8 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh3>\n\n<#ChucDanh3>\n\n\n\n\n\n\n\n<#Ten3>");

                xls.MergeCells(TongSoHang + TuHang + 3, 10 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 11 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 10 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 10 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 10 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh4>\n\n<#ChucDanh4>\n\n\n\n\n\n\n\n<#Ten4>");

                xls.MergeCells(TongSoHang + TuHang + 3, 13 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 13 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 13 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 13 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh5>\n\n<#ChucDanh5>\n\n\n\n\n\n\n\n<#Ten5>");
            }
            #endregion

            //Cell selection and scroll position.
            xls.SelectCell(12, 7, false);

            //Protection

            TSheetProtectionOptions SheetProtectionOptions;
            SheetProtectionOptions = new TSheetProtectionOptions(false);
            SheetProtectionOptions.SelectLockedCells = true;
            SheetProtectionOptions.SelectUnlockedCells = true;
            xls.Protection.SetSheetProtection(null, SheetProtectionOptions);
            return xls;
        }
        public static XlsFile TaoTieuDeLuyKe_A3(XlsFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotTrang1, int SoCotTrangLonHon1, Boolean NghiepVu = false)
        {
            xls.NewFile(1);    //Create a new Excel file with 1 sheet.

            //Set the names of the sheets
            xls.ActiveSheet = 1;
            xls.SheetName = "Sheet1";

            xls.ActiveSheet = 1;    //Set the sheet we are working in.

            //Global Workbook Options
            xls.OptionsAutoCompressPictures = true;

            #region Styles.
            TFlxFormat StyleFmt;
            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0.00_);_(* \\(#,##0.00\\);_(* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 4), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Normal, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(* #,##0_);_(* \\(#,##0\\);_(* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0_);_(\"$\"* \\(#,##0\\);_(\"$\"* \"-\"_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency0, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Percent, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            StyleFmt.Format = "_(\"$\"* #,##0.00_);_(\"$\"* \\(#,##0.00\\);_(\"$\"* \"-\"??_);_(@_)";
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Currency, 0), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.RowLevel, 2), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 1), StyleFmt);

            StyleFmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2));
            StyleFmt.Font.Name = "Calibri";
            StyleFmt.Font.Size20 = 220;
            StyleFmt.Font.Color = TExcelColor.FromTheme(TThemeColor.Foreground1);
            StyleFmt.Font.Family = 2;
            xls.SetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.ColLevel, 2), StyleFmt);
            #endregion
            #region //Named Ranges


            TXlsNamedRange Range;
            string RangeName;
            RangeName = TXlsNamedRange.GetInternalName(InternalNameRange.Print_Titles);
            Range = new TXlsNamedRange(RangeName, 1, 32, "='Sheet1'!$A:$G,'Sheet1'!$3:$5");
            //You could also use: Range = new TXlsNamedRange(RangeName, 1, 0, 0, 0, 0, 0, 32);
            xls.SetNamedRange(Range);
            #endregion
            #region//Printer Settings
            THeaderAndFooter HeadersAndFooters = new THeaderAndFooter();
            HeadersAndFooters.AlignMargins = true;
            HeadersAndFooters.ScaleWithDoc = true;
            HeadersAndFooters.DiffFirstPage = true;
            HeadersAndFooters.DiffEvenPages = false;
            HeadersAndFooters.DefaultHeader = "";
            HeadersAndFooters.DefaultFooter = "&RTrang: &P/&N";
            if (NghiepVu == true)
            {
                HeadersAndFooters.FirstHeader = "&L&\"Times New Roman,Bold\"<#QuanKhu>\n<#Phong>\n&C&\"Times New Roman,Bold\"TỔNG HỢP QUYẾT TOÁN <#sLNS> - PHẦN <#TruongTien>\n <#LoaiThangQuy> <#Thang> năm <#Nam>";
            }

            else
            {
                HeadersAndFooters.FirstHeader = "&L&\"Times New Roman,Bold\"<#QuanKhu>\n<#Phong>\n&C&\"Times New Roman,Bold\"TỔNG HỢP QUYẾT TOÁN LƯƠNG,PHỤ CẤP TIỀN ĂN\n Tháng <#Thang> năm <#Nam>";

            }
            HeadersAndFooters.FirstFooter = "&RTrang: &P/&N";
            HeadersAndFooters.EvenHeader = "";
            HeadersAndFooters.EvenFooter = "";
            xls.SetPageHeaderAndFooter(HeadersAndFooters);

            //You can set the margins in 2 ways, the one commented here or the one below:
            //    TXlsMargins PrintMargins = xls.GetPrintMargins();
            //    PrintMargins.Left = 0.196850393700787;
            //    PrintMargins.Top = 0.590551181102362;
            //    PrintMargins.Right = 0.196850393700787;
            //    PrintMargins.Bottom = 0.748031496062992;
            //    PrintMargins.Header = 0.31496062992126;
            //    PrintMargins.Footer = 0.31496062992126;
            //    xls.SetPrintMargins(PrintMargins);
            xls.SetPrintMargins(new TXlsMargins(0.393700787401575, 0.590551181102362, 0.393700787401575, 0.748031496062992, 0.31496062992126, 0.31496062992126));
            xls.PrintXResolution = 600;
            xls.PrintYResolution = 600;
            xls.PrintOptions = TPrintOptions.None;
            xls.PrintPaperSize = TPaperSize.A3;

            //Printer Driver Settings are a blob of data specific to a printer
            //This code is commented by default because normally you do not want to hard code the printer settings of an specific printer.
            //    byte[] PrinterData = {
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x04, 0x00, 0x06, 0xDC, 0x00, 0x00, 0x00, 0x03, 0xAF, 0x00, 0x00, 0x02, 0x00, 0x09, 0x00, 0xEA, 0x0A, 0x6F, 0x08, 0x64, 0x00, 0x01, 0x00, 0x0F, 0x00, 0x58, 0x02, 0x02, 0x00, 0x01, 0x00, 0x58, 0x02, 
            //        0x03, 0x00, 0x01, 0x00, 0x4C, 0x00, 0x65, 0x00, 0x74, 0x00, 0x74, 0x00, 0x65, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            //        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 
            //        0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            //    };
            //    TPrinterDriverSettings PrinterDriverSettings = new TPrinterDriveSettings(PrinterData);
            //    xls.SetPrinterDriverSettings(PrinterDriverSettings);
            #endregion
            int TongSoHang = dt.Rows.Count;
            int _TuCot = TuCot;
            int TongSoCot = 0;
            int SoTrang = 1;
            if ((DenCotCua_DT - TuCotCua_DT) <= SoCotTrang1)
            {
                int SoCotDu = ((DenCotCua_DT - TuCotCua_DT)) % SoCotTrang1;
                int SoCotCanThem = 0;


                SoCotCanThem = SoCotTrang1 - SoCotDu;

                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (DenCotCua_DT - TuCotCua_DT - SoCotTrang1) % SoCotTrangLonHon1;
                int SoCotCanThem = 0;


                SoCotCanThem = SoCotTrangLonHon1 - SoCotDu;
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;

                SoTrang = 1 + (TongSoCot - SoCotTrang1) / SoCotTrangLonHon1;
            }
            #region//Set up rows and columns
            xls.DefaultColWidth = 2340;
            xls.SetColWidth(1, 950);    //(3.25 + 0.75) * 256

            TFlxFormat ColFmt;
            ColFmt = xls.GetFormat(xls.GetColFormat(1));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(1, xls.AddFormat(ColFmt));
            xls.SetColWidth(2, 950);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(2));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(2, xls.AddFormat(ColFmt));
            xls.SetColWidth(3, 1000);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(3));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(3, xls.AddFormat(ColFmt));
            xls.SetColWidth(4, 1000);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(4));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(4, xls.AddFormat(ColFmt));
            xls.SetColWidth(5, 900);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(4));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(5, xls.AddFormat(ColFmt));
            xls.SetColWidth(6, 900);    //(3.25 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(5));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(6, xls.AddFormat(ColFmt));
            xls.SetColWidth(7, 8500);    //(27.54 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(6));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(7, xls.AddFormat(ColFmt));
            xls.SetColWidth(8, 3400);    //(12.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(7));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(8, xls.AddFormat(ColFmt));
            xls.SetColWidth(9, 3400);    //(12.82 + 0.75) * 256

            ColFmt = xls.GetFormat(xls.GetColFormat(8));
            ColFmt.Font.Name = "Times New Roman";
            ColFmt.Font.Family = 1;
            xls.SetColFormat(9, xls.AddFormat(ColFmt));
            xls.SetColWidth(10, 3400);    //(12.82 + 0.75) * 256

            for (int i = 0; i < TongSoCot; i++)
            {
                xls.SetColWidth(_TuCot + i, 3400);
            }
            xls.SetRowHeight(4, 600);
            xls.SetRowHeight(5, 600);
            xls.DefaultRowHeight = 300;
            #endregion

            #region //Merged Cells
            xls.MergeCells(4, 1, 5, 6);
            xls.MergeCells(4, 7, 5, 7);
            xls.MergeCells(4, 8, 5, 8);
            xls.MergeCells(4, 9, 5, 9);
            xls.MergeCells(4, 10, 5, 10);
            _TuCot = TuCot;
            if (SoTrang == 1)
            {
                xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrang1 - 1);
            }
            else
            {
                xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrang1 - 1);
                _TuCot = _TuCot + SoCotTrang1;
                for (int i = 1; i < SoTrang; i++)
                {
                    xls.MergeCells(4, _TuCot, 4, _TuCot + SoCotTrangLonHon1 - 1);
                    _TuCot = _TuCot + SoCotTrangLonHon1;
                }
            }
            #endregion
            #region set tieu de cot tinh
            TFlxFormat fmt;

            fmt = xls.GetCellVisibleFormatDef(1, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(1, 1, xls.AddFormat(fmt));
            xls.SetCellValue(1, 1, "<#auto page breaks>");

            fmt = xls.GetCellVisibleFormatDef(4, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 1, xls.AddFormat(fmt));
            xls.SetCellValue(4, 1, "L-K-M-TM-TTM-NG");

            fmt = xls.GetCellVisibleFormatDef(4, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(4, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 7, xls.AddFormat(fmt));
            xls.SetCellValue(4, 7, "Nội dung");

            fmt = xls.GetCellVisibleFormatDef(4, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 8, xls.AddFormat(fmt));
            xls.SetCellValue(4, 8, "Chi Tiêu");

            fmt = xls.GetCellVisibleFormatDef(4, 9);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 9, xls.AddFormat(fmt));
            xls.SetCellValue(4, 9, "Cộng trong kỳ");

            fmt = xls.GetCellVisibleFormatDef(4, 10);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, 10, xls.AddFormat(fmt));
            xls.SetCellValue(4, 10, "Lũy kế đến");



            fmt = xls.GetCellVisibleFormatDef(5, 1);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 1, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 2);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 3);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 4);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 5);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 5, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 7);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 7, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 8);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 8, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 9);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 9, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(5, 10);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(5, 10, xls.AddFormat(fmt));
            #endregion
            #region Set tieu de dong
            #region set hang trongdo va donvitinh
            _TuCot = TuCot;

            //set trang 1
            fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 6);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Italic;
            fmt.Font.Family = 1;
            xls.SetCellFormat(3, _TuCot + 6, xls.AddFormat(fmt));
            xls.SetCellValue(3, _TuCot + 6, "Đơn vị tính đồng     Tờ số 1");

            fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Size20 = 200;
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(4, _TuCot, xls.AddFormat(fmt));
            xls.SetCellValue(4, _TuCot, "Trong đó");
            for (int j = _TuCot + 1; j <= _TuCot + SoCotTrang1 - 1; j++)
            {
                fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(4, j, xls.AddFormat(fmt));
            }
            _TuCot = _TuCot + SoCotTrang1;
            //set cac trang con lai            
            for (int i = 1; i < SoTrang; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(3, _TuCot + 9);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Italic;
                fmt.Font.Family = 1;
                xls.SetCellFormat(3, _TuCot + 9, xls.AddFormat(fmt));
                xls.SetCellValue(3, _TuCot + 9, "Đơn vị tính  đồng      Tờ số: " + Convert.ToInt16(i + 1));

                fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(4, _TuCot, xls.AddFormat(fmt));
                xls.SetCellValue(4, _TuCot, "Trong đó");
                for (int j = _TuCot + 1; j <= _TuCot + SoCotTrangLonHon1 - 1; j++)
                {
                    fmt = xls.GetCellVisibleFormatDef(4, _TuCot);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 200;
                    fmt.Font.Style = TFlxFontStyles.Bold;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.center;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(4, j, xls.AddFormat(fmt));
                }
                _TuCot = _TuCot + SoCotTrangLonHon1;

            }
            #endregion
            #region set cac cot don vi
            _TuCot = TuCot;
            String TenCot;
            int _TuCotCua_DT = TuCotCua_DT;
            for (int i = 0; i < SoCotTrang1; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(5, _TuCot + i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                TenCot = "";
                if (DenCotCua_DT > _TuCotCua_DT)
                {
                    TenCot = "<#TenDV" + i + ">";
                }
                xls.SetCellFormat(5, _TuCot + i, xls.AddFormat(fmt));
                xls.SetCellValue(5, _TuCot + i, TenCot);
            }
            _TuCotCua_DT = _TuCotCua_DT + SoCotTrang1;
            _TuCot = _TuCot + SoCotTrang1;
            for (int i = 0; i < TongSoCot - SoCotTrang1; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(5, _TuCot + i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Size20 = 200;
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Left.Color = TExcelColor.Automatic;
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                TenCot = "";
                int a = Convert.ToInt16(SoCotTrang1) + i;
                if (DenCotCua_DT > _TuCotCua_DT)
                {
                    TenCot = "<#TenDV" + a + ">";
                }
                xls.SetCellFormat(5, _TuCot + i, xls.AddFormat(fmt));
                xls.SetCellValue(5, _TuCot + i, TenCot);
            }

            #endregion


            //ngaythangnam
            xls.MergeCells(TongSoHang + TuHang + 2, 17, TongSoHang + TuHang + 2, 18);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 17);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 2, 17, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 2, 17, "<#NgayThangNam>");

            //ChuKy           
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 7);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 7, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 7, "<#row height(autofit)><#ThuaLenh1>\n\n<#ChucDanh1>\n\n\n\n\n\n\n\n<#Ten1>");

            xls.MergeCells(TongSoHang + TuHang + 3, 8, TongSoHang + TuHang + 3, 9);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 8);
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 8, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 8, "<#row height(autofit)><#ThuaLenh2>\n\n<#ChucDanh2>\n\n\n\n\n\n\n\n<#Ten2>");

            xls.MergeCells(TongSoHang + TuHang + 3, 11, TongSoHang + TuHang + 3, 12);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 4, 11);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 11, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 11, "<#row height(autofit)><#ThuaLenh3>\n\n<#ChucDanh3>\n\n\n\n\n\n\n\n<#Ten3>");

            xls.MergeCells(TongSoHang + TuHang + 3, 14, TongSoHang + TuHang + 4, 15);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 14);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 14, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 14, "<#row height(autofit)><#ThuaLenh4>\n\n<#ChucDanh4>\n\n\n\n\n\n\n\n<#Ten4>");

            xls.MergeCells(TongSoHang + TuHang + 3, 17, TongSoHang + TuHang + 3, 18);
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 17);
            fmt.Font.Name = "Times New Roman";
            fmt.Font.Style = TFlxFontStyles.Bold;
            fmt.Font.Family = 1;
            fmt.Font.CharSet = 0;
            fmt.Font.Size20 = 200;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.WrapText = true;
            xls.SetCellFormat(TongSoHang + TuHang + 3, 17, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 3, 17, "<#row height(autofit)><#ThuaLenh5>\n\n<#ChucDanh5>\n\n\n\n\n\n\n\n<#Ten5>");
            //set trang lon hon 1
            for (int i = 1; i < SoTrang; i++)
            {
                xls.MergeCells(TongSoHang + TuHang + 2, 17 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 2, 18 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 17 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                xls.SetCellFormat(TongSoHang + TuHang + 2, 17 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 2, 17 + SoCotTrangLonHon1 * i, "<#NgayThangNam>");

                xls.MergeCells(TongSoHang + TuHang + 3, 8 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 2, 9 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 8 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 2, 8 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 2, 8 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh2>\n\n<#ChucDanh2>\n\n\n\n\n\n\n\n<#Ten2>");

                xls.MergeCells(TongSoHang + TuHang + 3, 11 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 2, 12 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 2, 11 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 2, 11 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 2, 11 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh3>\n\n<#ChucDanh3>\n\n\n\n\n\n\n\n<#Ten3>");

                xls.MergeCells(TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 15 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 14 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh4>\n\n<#ChucDanh4>\n\n\n\n\n\n\n\n<#Ten4>");

                xls.MergeCells(TongSoHang + TuHang + 3, 17 + SoCotTrangLonHon1 * i, TongSoHang + TuHang + 3, 18 + SoCotTrangLonHon1 * i);
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 3, 17 + SoCotTrangLonHon1 * i);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Style = TFlxFontStyles.Bold;
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.WrapText = true;
                xls.SetCellFormat(TongSoHang + TuHang + 3, 17 + SoCotTrangLonHon1 * i, xls.AddFormat(fmt));
                xls.SetCellValue(TongSoHang + TuHang + 3, 17 + SoCotTrangLonHon1 * i, "<#row height(autofit)><#ThuaLenh5>\n\n<#ChucDanh5>\n\n\n\n\n\n\n\n<#Ten5>");
            }
            #endregion
            return xls;
        }
        public static void Filldata(XlsFile xls, DataTable dt, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotTrang1, int SoCotTrangLonHon1, String MapCotCoDinh)
        {
            TFlxFormat fmt;
            Object GiaTriO;
            int TongSoHang = dt.Rows.Count;
            int _TuCot = TuCot;
            int TongSoCot = 0;
            int SoTrang = 1;
            if ((DenCotCua_DT - TuCotCua_DT) <= SoCotTrang1)
            {
                int SoCotDu = ((DenCotCua_DT - TuCotCua_DT)) % SoCotTrang1;
                int SoCotCanThem = 0;


                SoCotCanThem = SoCotTrang1 - SoCotDu;

                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (DenCotCua_DT - TuCotCua_DT - SoCotTrang1) % SoCotTrangLonHon1;
                int SoCotCanThem = 0;


                SoCotCanThem = SoCotTrangLonHon1 - SoCotDu;
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;

                SoTrang = 1 + (TongSoCot - SoCotTrang1) / SoCotTrangLonHon1;
            }
            //set border cho cot can them
            for (int c = DenCotCua_DT - TuCotCua_DT + TuCot; c < TongSoCot + TuCot; c++)
            {
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    fmt = xls.GetCellVisibleFormatDef(h + TuHang, c);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 200;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.right;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(h + TuHang, c, xls.AddFormat(fmt));
                }
            }
            String[] arrMapCot = MapCotCoDinh.Split('|');
            String[] arrCot_Excel = arrMapCot[0].Split(',');
            String[] arrCot_DT = arrMapCot[1].Split(',');
            #region Fill dữ liệu những cột động
            _TuCot = TuCot;
            int d = 0;
            for (int c = 0; c < TongSoCot; c++)
            {
                Type _Type = typeof(String);
                if (c + TuCotCua_DT < DenCotCua_DT)
                    _Type = dt.Columns[c + TuCotCua_DT].DataType;
                switch (_Type.ToString())
                {
                    case "System.Decimal":
                        fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 200;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.right;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "#,##0;-#,##0;;@";
                        break;
                    default:
                        fmt = xls.GetCellVisibleFormatDef(TuHang, 2);
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 200;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.left;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "#,##0;-#,##0;;@";
                        break;
                }
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    GiaTriO = null;
                    string s = Convert.ToString(dt.Rows[h]["sTTM"]);
                    if (Convert.ToString(dt.Rows[h]["sTTM"]) == "")
                    {
                        fmt.Font.Style = TFlxFontStyles.Bold;
                    }
                    else
                    {
                        fmt.Font.Style = TFlxFontStyles.None;
                    }
                    if (Convert.ToString(dt.Rows[h]["sNG"]) == "" && Convert.ToString(dt.Rows[h]["sTM"]) == "")
                    {
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;


                    }
                    else
                    {
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;

                    }
                    xls.SetCellFormat(h + TuHang, _TuCot, xls.AddFormat(fmt));
                    if (c + TuCotCua_DT < DenCotCua_DT)
                        xls.SetCellValue(h + TuHang, _TuCot, dt.Rows[h][c + TuCotCua_DT]);

                }
                _TuCot++;
            }
            #endregion
            #region Fill dữ liệu những cột tĩnh
            _TuCot = TuCot;
            String KyTu1, strSum;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int c = 0; c < arrCot_Excel.Length; c++)
                {
                    fmt = xls.GetCellVisibleFormatDef(TuHang + i, Convert.ToInt32(arrCot_Excel[c]));
                    if (c >= arrCot_Excel.Length - 1)
                    {
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 200;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.right;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "#,##0;-#,##0;;@";
                    }
                    else
                    {
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 200;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.left;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "#,##0;-#,##0;;@";
                    }
                    fmt.WrapText = true;
                    xls.AutofitRow(i + TuHang, true, 1);
                    GiaTriO = null;
                    if (c < arrCot_DT.Length)
                        GiaTriO = dt.Rows[i][Convert.ToInt16(arrCot_DT[c])];
                    if (Convert.ToString(dt.Rows[i]["sTM"]) == "")//nếu cột TM="";
                    {
                        fmt.Font.Style = TFlxFontStyles.Bold;
                    }
                    else
                    {
                        if (c < 3)
                        {
                            GiaTriO = null;

                        }
                        else { }

                        if (Convert.ToString(dt.Rows[i]["sTTM"]) != "") //nếu cột TTM=="",
                        {
                            if (c < 4)
                            {
                                GiaTriO = null;

                            }
                        }
                        else
                        {
                            fmt.Font.Style = TFlxFontStyles.Bold;//Set Bold cho hàng TM
                        }
                    }
                    if (Convert.ToString(dt.Rows[i]["sNG"]) == "" && Convert.ToString(dt.Rows[i]["sTM"]) == "")
                    {
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;


                    }
                    else
                    {
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;

                    }
                    if (c < arrCot_Excel.Length)
                    {
                        xls.SetCellFormat(i + TuHang, Convert.ToInt16(arrCot_Excel[c]), xls.AddFormat(fmt));
                        xls.SetCellValue(i + TuHang, Convert.ToInt16(arrCot_Excel[c]), GiaTriO);
                    }
                }
            }
            #endregion

            #region  hàng tổng cuối trang
            //set cột tổng cuối báo cáo

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 1);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 1, xls.AddFormat(fmt));
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 2);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 2, xls.AddFormat(fmt));
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 3);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 4);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 5);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 5, xls.AddFormat(fmt));


            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 6);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 7);

            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.Font.Style = TFlxFontStyles.Bold;
            xls.SetCellFormat(TongSoHang + TuHang, 7, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang, 7, "Cộng:          Trong Kỳ: ");
            _TuCot = TuCot;
            for (int i = 0; i <= TongSoCot; i++)
            {
                fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.right;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Format = "#,##0;-#,##0;;@";
                fmt.Font.Style = TFlxFontStyles.Bold;
                xls.SetCellFormat(TongSoHang + TuHang, _TuCot - 1 + i, xls.AddFormat(fmt));
                KyTu1 = HamChung.ExportExcel_MaCot(_TuCot - 1 + i);
                if (TongSoHang > 1)
                {
                    strSum = String.Format("=SUMIF(F{1}:F{3},\"<>\"&\"\",{0}{1}:{2}{3})", KyTu1, TuHang, KyTu1, TongSoHang + TuHang - 1);
                    xls.SetCellFormat(TongSoHang + TuHang, _TuCot - 1 + i, xls.AddFormat(fmt));
                    xls.SetCellValue(TongSoHang + TuHang, _TuCot - 1 + i, new TFormula(strSum));
                }
            }

            // set cot den ky nay

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 1);

            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 1, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 2);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 3);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 4);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + 1 + TuHang, 5);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 5, xls.AddFormat(fmt));


            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 6);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 7);

            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.Font.Style = TFlxFontStyles.Bold;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 7, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 1, 7, "                 Đến kỳ này:");
            _TuCot = TuCot;

            for (int i = 0; i < TongSoCot + 1; i++)
            {
                fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.right;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Format = "#,##0;-#,##0;;@";
                fmt.Font.Style = TFlxFontStyles.Bold;
                xls.SetCellFormat(TongSoHang + TuHang + 1, _TuCot - 1 + i, xls.AddFormat(fmt));
            }
            _TuCot = TuCot;
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, _TuCot + SoCotTrang1 - 1);
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.Font.Style = TFlxFontStyles.Bold;
            xls.SetCellFormat(TongSoHang + TuHang, _TuCot + SoCotTrang1 - 1, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, _TuCot + SoCotTrang1 - 1);
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.Font.Style = TFlxFontStyles.Bold;
            xls.SetCellFormat(TongSoHang + TuHang + 1, _TuCot + SoCotTrang1 - 1, xls.AddFormat(fmt));

            _TuCot = TuCot + SoCotTrang1;
            for (int i = 1; i < SoTrang; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, _TuCot + SoCotTrangLonHon1 * i - 1);
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Font.Style = TFlxFontStyles.Bold;
                xls.SetCellFormat(TongSoHang + TuHang, _TuCot + SoCotTrangLonHon1 * i - 1, xls.AddFormat(fmt));

                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, _TuCot + SoCotTrangLonHon1 * i - 1);
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Font.Style = TFlxFontStyles.Bold;
                xls.SetCellFormat(TongSoHang + TuHang + 1, _TuCot + SoCotTrangLonHon1 * i - 1, xls.AddFormat(fmt));
            }
            TXlsNamedRange Range;
            Range = new TXlsNamedRange("KeepRows_1_", 0, 1, TuHang - 4, 1, TongSoHang + TuHang + 3, FlxConsts.Max_Columns + 1, 0);
            xls.SetNamedRange(Range);
            Range = new TXlsNamedRange("KeepRows_2_", 0, 1, TongSoHang + TuHang, 1, TongSoHang + TuHang + 3, FlxConsts.Max_Columns + 1, 0);
            xls.SetNamedRange(Range);
            #endregion
        }
        public static void FilldataLuyKe(XlsFile xls, DataTable dt, DataTable dtLuyKe, int TuHang, int TuCot, int TuCotCua_DT, int DenCotCua_DT, int SoCotTrang1, int SoCotTrangLonHon1, String MapCotCoDinh)
        {
            TFlxFormat fmt;
            Object GiaTriO;
            int TongSoHang = dt.Rows.Count;
            int _TuCot = TuCot;
            int TongSoCot = 0;
            int SoTrang = 1;
            if ((DenCotCua_DT - TuCotCua_DT) <= SoCotTrang1)
            {
                int SoCotDu = ((DenCotCua_DT - TuCotCua_DT)) % SoCotTrang1;
                int SoCotCanThem = 0;

                SoCotCanThem = SoCotTrang1 - SoCotDu;

                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;
            }
            else
            {
                int SoCotDu = (DenCotCua_DT - TuCotCua_DT - SoCotTrang1) % SoCotTrangLonHon1;
                int SoCotCanThem = 0;


                SoCotCanThem = SoCotTrangLonHon1 - SoCotDu;
                TongSoCot = (DenCotCua_DT - TuCotCua_DT) + SoCotCanThem;

                SoTrang = 1 + (TongSoCot - SoCotTrang1) / SoCotTrangLonHon1;
            }
            //set border cho cot can them
            for (int c = DenCotCua_DT - TuCotCua_DT + TuCot; c < TongSoCot + TuCot; c++)
            {
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    fmt = xls.GetCellVisibleFormatDef(h + TuHang, c);
                    fmt.Font.Name = "Times New Roman";
                    fmt.Font.Size20 = 200;
                    fmt.Font.Family = 1;
                    fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Left.Color = TExcelColor.Automatic;
                    fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Right.Color = TExcelColor.Automatic;
                    fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                    fmt.Borders.Top.Color = TExcelColor.Automatic;
                    fmt.HAlignment = THFlxAlignment.right;
                    fmt.VAlignment = TVFlxAlignment.center;
                    xls.SetCellFormat(h + TuHang, c, xls.AddFormat(fmt));
                }
            }
            String[] arrMapCot = MapCotCoDinh.Split('|');
            String[] arrCot_Excel = arrMapCot[0].Split(',');
            String[] arrCot_DT = arrMapCot[1].Split(',');

            #region Fill dữ liệu những cột động
            _TuCot = TuCot;
            for (int c = 0; c < TongSoCot; c++)
            {
                Type _Type = typeof(String);
                if (c + TuCotCua_DT < DenCotCua_DT)
                    _Type = dt.Columns[c + TuCotCua_DT].DataType;
                switch (_Type.ToString())
                {
                    case "System.Decimal":
                        fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 200;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.right;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "#,##0;-#,##0;;@";
                        break;
                    default:
                        fmt = xls.GetCellVisibleFormatDef(TuHang, 2);
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 200;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.left;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "#,##0;-#,##0;;@";
                        break;
                }
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    if (Convert.ToString(dt.Rows[h]["sNG"]) == "" && Convert.ToString(dt.Rows[h]["sTM"]) == "")
                    {
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;


                    }
                    else
                    {
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;

                    }
                    if (Convert.ToString(dt.Rows[h]["sTTM"]) == "")
                    {
                        fmt.Font.Style = TFlxFontStyles.Bold;
                    }
                    else
                    {
                        fmt.Font.Style = TFlxFontStyles.None;
                    }
                    GiaTriO = null;
                    xls.SetCellFormat(h + TuHang, _TuCot, xls.AddFormat(fmt));
                    if (c + TuCotCua_DT < DenCotCua_DT)
                        xls.SetCellValue(h + TuHang, _TuCot, dt.Rows[h][c + TuCotCua_DT]);
                }
                _TuCot++;
            }
            #endregion

            #region Fill dữ liệu những cột tĩnh
            _TuCot = TuCot;
            String KyTu1, strSum;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int c = 0; c < arrCot_Excel.Length; c++)
                {
                    fmt = xls.GetCellVisibleFormatDef(TuHang + i, Convert.ToInt32(arrCot_Excel[c]));
                    if (c >= arrCot_Excel.Length - 3)
                    {
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 200;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.right;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "#,##0;-#,##0;;@";
                    }
                    else
                    {
                        fmt.Font.Name = "Times New Roman";
                        fmt.Font.Size20 = 200;
                        fmt.Font.Family = 1;
                        fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Left.Color = TExcelColor.Automatic;
                        fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Right.Color = TExcelColor.Automatic;
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                        fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Top.Color = TExcelColor.Automatic;
                        fmt.HAlignment = THFlxAlignment.left;
                        fmt.VAlignment = TVFlxAlignment.center;
                        fmt.Format = "#,##0;-#,##0;;@";
                    }
                    fmt.WrapText = true;
                    xls.AutofitRow(i + TuHang, true, 1);
                    GiaTriO = null;
                    if (c < arrCot_DT.Length)
                        GiaTriO = dt.Rows[i][Convert.ToInt16(arrCot_DT[c])];
                    if (Convert.ToString(dt.Rows[i]["sTM"]) == "")//nếu cột TM="";
                    {
                        fmt.Font.Style = TFlxFontStyles.Bold;
                    }
                    else
                    {
                        if (c < 3)
                        {
                            GiaTriO = null;

                        }
                        else { }

                        if (Convert.ToString(dt.Rows[i]["sTTM"]) != "") //nếu cột TTM=="",
                        {
                            if (c < 4)
                            {
                                GiaTriO = null;

                            }
                        }
                        else
                        {
                            fmt.Font.Style = TFlxFontStyles.Bold;//Set Bold cho hàng TM
                        }
                    }
                    if (Convert.ToString(dt.Rows[i]["sNG"]) == "" && Convert.ToString(dt.Rows[i]["sTM"]) == "")
                    {
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    }
                    else
                    {
                        fmt.Borders.Bottom.Style = TFlxBorderStyle.Dotted;
                        fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                    }
                    if (c < arrCot_Excel.Length)
                    {
                        xls.SetCellFormat(i + TuHang, Convert.ToInt16(arrCot_Excel[c]), xls.AddFormat(fmt));
                        xls.SetCellValue(i + TuHang, Convert.ToInt16(arrCot_Excel[c]), GiaTriO);
                    }
                    else
                    {
                        xls.SetCellFormat(i + TuHang, c, xls.AddFormat(fmt));
                    }
                }

            }
            //set cột tổng cuối báo cáo

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 1);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 1, xls.AddFormat(fmt));
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 2);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 2, xls.AddFormat(fmt));
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 3);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 4);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 5);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 5, xls.AddFormat(fmt));


            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 6);
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, 7);

            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.Font.Style = TFlxFontStyles.Bold;
            xls.SetCellFormat(TongSoHang + TuHang, 7, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang, 7, "Cộng:          Trong Kỳ: ");
            _TuCot = TuCot;
            for (int i = 0; i <= TongSoCot + 2; i++)
            {
                fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.right;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Format = "#,##0;-#,##0;;@";
                fmt.Font.Style = TFlxFontStyles.Bold;
                xls.SetCellFormat(TongSoHang + TuHang, _TuCot - 3 + i, xls.AddFormat(fmt));
                KyTu1 = HamChung.ExportExcel_MaCot(_TuCot - 3 + i);
                if (TongSoHang > 1)
                {
                    strSum = String.Format("=SUMIF(F{1}:F{3},\"<>\"&\"\",{0}{1}:{2}{3})", KyTu1, TuHang, KyTu1, TongSoHang + TuHang - 1);
                    xls.SetCellFormat(TongSoHang + TuHang, _TuCot - 3 + i, xls.AddFormat(fmt));
                    xls.SetCellValue(TongSoHang + TuHang, _TuCot - 3 + i, new TFormula(strSum));
                }
            }
            //XOA gia tri cot luy ke den
            xls.SetCellValue(TongSoHang + TuHang, _TuCot - 1, "");


            // set cot den ky nay

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 1);

            fmt.Borders.Left.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Left.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 1, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 2);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 2, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 3);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 3, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 4);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 4, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + 1 + TuHang, 5);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 5, xls.AddFormat(fmt));


            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 6);
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.right;
            fmt.VAlignment = TVFlxAlignment.center;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 6, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, 7);

            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.Font.Style = TFlxFontStyles.Bold;
            xls.SetCellFormat(TongSoHang + TuHang + 1, 7, xls.AddFormat(fmt));
            xls.SetCellValue(TongSoHang + TuHang + 1, 7, "                 Đến kỳ này:");
            _TuCot = TuCot;

            for (int i = 0; i < TongSoCot + 3; i++)
            {
                fmt = xls.GetStyle(xls.GetBuiltInStyleName(TBuiltInStyle.Comma, 0), true);
                fmt.Font.Name = "Times New Roman";
                fmt.Font.Family = 1;
                fmt.Font.CharSet = 0;
                fmt.Font.Size20 = 200;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.right;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Format = "#,##0;-#,##0;;@";
                fmt.Font.Style = TFlxFontStyles.Bold;
                xls.SetCellFormat(TongSoHang + TuHang + 1, _TuCot - 3 + i, xls.AddFormat(fmt));
                KyTu1 = HamChung.ExportExcel_MaCot(_TuCot - 3 + i);
                String ChiTieu = String.Format("=H{0}", TongSoHang + TuHang);
                if (TongSoHang > 1)
                {
                    xls.SetCellValue(TongSoHang + TuHang + 1, _TuCot - 3, new TFormula(ChiTieu));
                    if (i < dtLuyKe.Columns.Count && dtLuyKe.Rows.Count > 0)
                    {
                        xls.SetCellValue(TongSoHang + TuHang + 1, _TuCot - 1 + i, dtLuyKe.Rows[0][i]);
                    }

                }
            }
            _TuCot = TuCot;
            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, _TuCot + SoCotTrang1 - 1);
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Top.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.Font.Style = TFlxFontStyles.Bold;
            xls.SetCellFormat(TongSoHang + TuHang, _TuCot + SoCotTrang1 - 1, xls.AddFormat(fmt));

            fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, _TuCot + SoCotTrang1 - 1);
            fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Right.Color = TExcelColor.Automatic;
            fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            fmt.Borders.Bottom.Color = TExcelColor.Automatic;
            fmt.HAlignment = THFlxAlignment.center;
            fmt.VAlignment = TVFlxAlignment.center;
            fmt.Font.Style = TFlxFontStyles.Bold;
            xls.SetCellFormat(TongSoHang + TuHang + 1, _TuCot + SoCotTrang1 - 1, xls.AddFormat(fmt));

            _TuCot = TuCot + SoCotTrang1;
            for (int i = 1; i < SoTrang; i++)
            {
                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang, _TuCot + SoCotTrangLonHon1 * i - 1);
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Top.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Top.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Font.Style = TFlxFontStyles.Bold;
                xls.SetCellFormat(TongSoHang + TuHang, _TuCot + SoCotTrangLonHon1 * i - 1, xls.AddFormat(fmt));

                fmt = xls.GetCellVisibleFormatDef(TongSoHang + TuHang + 1, _TuCot + SoCotTrangLonHon1 * i - 1);
                fmt.Borders.Right.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Right.Color = TExcelColor.Automatic;
                fmt.Borders.Bottom.Style = TFlxBorderStyle.Thin;
                fmt.Borders.Bottom.Color = TExcelColor.Automatic;
                fmt.HAlignment = THFlxAlignment.center;
                fmt.VAlignment = TVFlxAlignment.center;
                fmt.Font.Style = TFlxFontStyles.Bold;
                xls.SetCellFormat(TongSoHang + TuHang + 1, _TuCot + SoCotTrangLonHon1 * i - 1, xls.AddFormat(fmt));
            }
            TXlsNamedRange Range;
            Range = new TXlsNamedRange("KeepRows_1_", 0, 1, TuHang - 4, 1, TongSoHang + TuHang + 3, FlxConsts.Max_Columns + 1, 0);
            xls.SetNamedRange(Range);
            Range = new TXlsNamedRange("KeepRows_2_", 0, 1, TongSoHang + TuHang, 1, TongSoHang + TuHang + 3, FlxConsts.Max_Columns + 1, 0);
            xls.SetNamedRange(Range);
            #endregion
        }
        #endregion

        public static void getDataTable7Cap(DataTable data, ref DataTable dtsTM, ref DataTable dtsM, ref DataTable dtsL, ref DataTable dtsLNS, ref DataTable dtsLNS5, ref DataTable dtsLNS3, ref DataTable dtsLNS1)
        {
            DataRow r;
            dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS1,sLNS3,sLNS5,sLNS", "sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "sLNS1,sLNS3,sLNS5", "sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }
            dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "sLNS1,sLNS3", "sLNS1,sLNS3,sMoTa");

            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "sLNS1", "sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
            }
        }
        public static void getDataTable9Cap(DataTable data, ref DataTable dtsTM, ref DataTable dtsM, ref DataTable dtsL, ref DataTable dtsLNS, ref DataTable dtsLNS5, ref DataTable dtsLNS3, ref DataTable dtsLNS1, ref DataTable dtDonVi, ref DataTable dtPhongBan)
        {
            DataRow r;
            dtsTM = HamChung.SelectDistinct("dtsTM", data, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "iID_MaPhongBan,iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "iID_MaPhongBan,iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            dtsL = HamChung.SelectDistinct("dtsL", dtsM, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "iID_MaPhongBan,iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS", "iID_MaPhongBan,iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");
            dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3,sLNS5", "iID_MaPhongBan,iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS5"]));
            }
            dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "iID_MaPhongBan,iID_MaDonVi,sLNS1,sLNS3", "iID_MaPhongBan,iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sMoTa");

            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS3"]));
            }
            dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "iID_MaPhongBan,iID_MaDonVi,sLNS1", "iID_MaPhongBan,iID_MaDonVi,sTenDonVi,sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = LayMoTa(Convert.ToString(r["sLNS1"]));
            }
            dtDonVi = HamChung.SelectDistinct("dtDonVi", dtsLNS1, "iID_MaPhongBan,iID_MaDonVi", "iID_MaPhongBan,iID_MaDonVi,sTenDonVi");
            dtPhongBan = HamChung.SelectDistinct("dtPhongBan", dtDonVi, "iID_MaPhongBan", "iID_MaPhongBan");
        }
        public static String LayMoTa(String sLNS)
        {
            String sMoTa = "";

            String SQL = String.Format(@"SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS={0}", sLNS);
            sMoTa = Connection.GetValueString(SQL, "");
            return sMoTa;
        }
    }
}