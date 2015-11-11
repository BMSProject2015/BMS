using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;
using System.Web.Mvc;
namespace VIETTEL.Models
{
    public class BangLuongModels
    {
        public static String strDSTruongTienTieuDe = "Số TK,Đơn vị,SSL," +
                                                       "ĐT,CB,NXTNgũ,HSLg,LCB," +
                                                       "V.Khung,Bảo lưu,ANQP,PC Chức vụ,Thâm niên," +
                                                       "TR.Nhiệm,Khu vực-Đặc biệt,Trên hạn định,Nữ QN,PC Khác," +
                                                       "Ăn 1 ngày,Tiền ăn,Trích lương,Trừ khác," + 
                                                       "BHXH,BHYT,NộpBHTN," +
                                                       "BHTN,Tổng BH";

        public static String strDSTruongTien = "sSoTaiKhoan_CanBo,sTenDonVi,sSoSoLuong_CanBo," +
                                                "iID_MaNgachLuong_CanBo,iID_MaBacLuong_CanBo,sNXTNgu,rLuongCoBan_HeSo_CanBo,rLuongCoBan," +
                                                "rPhuCap_VuotKhung,rPhuCap_BaoLuu,rPhuCap_AnNinhQuocPhong,rPhuCap_ChucVu,rPhuCap_ThamNien," +
                                                "rPhuCap_TrachNhiem,rPhuCap_KhuVuc,rPhuCap_TrenHanDinh,rPhuCap_NuQuanNhan,rPhuCap_Khac," +
                                                "rTienAn1Ngay,rKhoanTru_TienAn,rTrichLuong,rKhoanTru_Khac," +
                                                "rBaoHiem_XaHoi_CaNhan,rBaoHiem_YTe_CaNhan,bBaoHiem_ThatNghiep_CaNhan_CoNop," +
                                                "rBaoHiem_ThatNghiep_CaNhan,rBaoHiem_Tong_CaNhan";
        public static String strDSTruongTienDoRong = "80,100,80," +
                                                    "30,30,40,40,100," +
                                                    "100,100,100,100,100," +
                                                    "100,100,100,100,100," +
                                                    "60,60,60,60," + 
                                                    "100,100,60," +
                                                    "100,100";



        public static String strDSTruongTienTieuDe_ThueTNCN = "Số Người PT,Giảm trừ khác,Được giảm thuế," +
                                                       "Thù lao-Thưởng,Nội dung Thù lao-Thưởng,Lợi ích khác," +
                                                       "Đã nộp thuế,Nộp thuế đầu vào,Nội dung nộp thuế đầu vào," +
                                                       "ĐC Thu nhập,ĐC Thuế đã nộp,ĐC Giảm trừ,Trích lương";


        public static String strDSTruongTien_ThueTNCN = "iSoNguoiPhuThuoc_CanBo,rGiamTruKhac,rDuocGiamThue," +
                                                "rThuong,sThuong_MoTa,rLoiIchKhac," +
                                                "rDaNopThue,rNopThueDauVao,sNopThueDauVao_MoTa," +
                                                "rDieuChinhThuNhap,rDieuChinhThueDaNop,rDieuChinhThueGiamTru,rTrichLuong";
        public static String strDSTruongTienDoRong_ThueTNCN = "100,100,100," +
                                                                "100,300,100," +
                                                                "100,100,300," +
                                                                "100,100,100,100";

        public static String strDSTruongTienTieuDe_BaoHiem = "Ốm dài,SN nghỉ," +
                                                                "Bản thân ốm (VNĐ),Số ngày ốm," +
                                                                "Con ốm (VNĐ),Số ngày c.ốm," +
                                                                "DS - PHSK sau ốm (VNĐ),Số ngày," +
                                                                "Sinh Con - nuôi con (VNĐ),Số ngày," +
                                                                "Khám Thai - KHH GD (VNĐ),Số ngày," +
                                                                "DS - PHSK sau thai sản (VNĐ),Số ngày," +
                                                                "Tr.cấp 1 Lần (VNĐ)," +
                                                                "Tr.cấp 1 Lần (VNĐ)," +
                                                                "Tr.cấp hàng tháng (VNĐ)," +
                                                                //"Phục Hồi,Số ngày," +
                                                                 "Tr.cấp phục hồi chức năng (VNĐ)," +
                                                                "Tr.cấp người phục vụ (VNĐ)," +
                                                                "Trợ cấp chết do TNLD.BNN (VNĐ)," +
                                                                "DS - PHSK sau TNLD BNN (VNĐ),Số ngày," +
                                                                "Hưu Trí - Tr.cấp 1 lần (VNĐ)," +
                                                                "Phục Viên - Tr.cấp 1 lần (VNĐ)," +
                                                                "Xuất Ngũ - Tr.cấp 1 lần (VNĐ)," +
                                                                "Thôi Việc - Tr.cấp 1 lần (VNĐ)," +
                                                                "Tử Tuất - Mai táng phí (VNĐ)";

        public static String strDSTruongTien_BaoHiem = "bOmDaiNgay,iSoNgayNghiOm," +
                                                        "rBaoHiemChi_OmDau_BanThanOm,rBaoHiemChi_OmDau_BanThanOm_SoNgay," +
                                                        "rBaoHiemChi_OmDau_ConOm,rBaoHiemChi_OmDau_ConOm_SoNgay," +
                                                        "rBaoHiemChi_OmDau_DuongSuc,rBaoHiemChi_OmDau_DuongSuc_SoNgay," +
                                                        "rBaoHiemChi_ThaiSan_SinhCon,rBaoHiemChi_ThaiSan_SinhCon_SoNgay," +
                                                        "rBaoHiemChi_ThaiSan_KhamThai,rBaoHiemChi_ThaiSan_KhamThai_SoNgay," +
                                                        "rBaoHiemChi_ThaiSan_DuongSuc,rBaoHiemChi_ThaiSan_DuongSuc_SoNgay," +
                                                        "rBaoHiemChi_ThaiSan_1Lan," +
                                                        "rBaoHiemChi_TaiNan_1Lan," +
                                                        "rBaoHiemChi_TaiNan_HangThang," +
                                                        //"rBaoHiemChi_TaiNan_PhucHoi,rBaoHiemChi_TaiNan_PhucHoi_SoNgay," +
                                                          "rBaoHiemChi_TaiNan_PhucHoi," +
                                                        "rBaoHiemChi_TaiNan_NguoiPhucVu," +
                                                        "rBaoHiemChi_TaiNan_Chet," +
                                                        "rBaoHiemChi_TaiNan_DuongSuc,rBaoHiemChi_TaiNan_DuongSuc_SoNgay," +
                                                        "rBaoHiemChi_HuuTri," +
                                                        "rBaoHiemChi_PhucVien," +
                                                        "rBaoHiemChi_XuatNgu," +
                                                        "rBaoHiemChi_ThoiViec," +
                                                        "rBaoHiemChi_TuTuat";

        public static String strDSTruongTienDoRong_BaoHiem =    "50,60," +
                                                                "120,80," +
                                                                "100,80," +
                                                                "155,80," +
                                                                "150,80," +
                                                                "150,80," +
                                                                "165,80," +
                                                                "155," +
                                                                "155," +
                                                                "155," +
                                                                //"100,100," +
                                                                  "175," +
                                                                "155," +
                                                                "175," +
                                                                "165,100," +
                                                                "165," +
                                                                "165," +
                                                                "165," +
                                                                "165," +
                                                                "165";

        public static String strDSTruongSoCotCungNhom_BaoHiem = "1,1," +
                                                                "6,6," +
                                                                "6,6," +
                                                                "6,6," +
                                                                "7,7," +
                                                                "7,7," +
                                                                "7,7," +
                                                                "7,7," +
                                                                "7,7," +
                                                                "7," +
                                                                "7,7," +
                                                                "7," +
                                                               // "10," +
                                                               // "10,10," +
                                                                "1," +
                                                                "1," +
                                                                "1," +
                                                                "1," +
                                                                "1";

        public static String strDSTruongTieuDeNhomCot_BaoHiem = ",," +
                                                                "Trợ cấp ốm đau,Trợ cấp ốm đau," +
                                                                "Trợ cấp ốm đau,Trợ cấp ốm đau," +
                                                                "Trợ cấp ốm đau,Trợ cấp ốm đau," +
                                                                "Trợ cấp thai sản,Trợ cấp thai sản," +
                                                                "Trợ cấp thai sản,Trợ cấp thai sản," +
                                                                "Trợ cấp thai sản,Trợ cấp thai sản," +
                                                                //"Trợ cấp thai sản,Tai nạn lao động," +
                                                                 "Trợ cấp thai sản," +
                                                                "Tai nạn lao động,Tai nạn lao động," +
                                                                "Tai nạn lao động," +
                                                                "Tai nạn lao động,Tai nạn lao động," +
                                                                "Tai nạn lao động," +
                                                                "Tai nạn lao động," +
                                                                //"Tai nạn lao động,Tai nạn lao động," +
                                                                "," +
                                                                "," +
                                                                "," +
                                                                "," +
                                                                  "," +
                                                                "";
        
        public static String strDSTruongTieuDe = "Họ đệm,Tên";
        public static String strDSTruong = "sHoDem_CanBo,sTen_CanBo";
        public static String strDSTruongDoRong = "120,40";

        public static String[] arrDSTruongTieuDe = strDSTruong.Split(',');
        public static String[] arrDSTruong = strDSTruong.Split(',');
        public static String[] arrDSTruongDoRong = strDSTruong.Split(',');


        public static String LayXauDanhSachDonVi(string Path, string XauHanhDong, string XauSapXep, String MaDonViCha, int Cap,String MaND, ref int ThuTu, int iNamLamViec)
        {
            String vR = "";
            String SQL = "";
            SqlCommand cmd;
            if (MaDonViCha != null && MaDonViCha != "")
            {
                SQL = "SELECT NS_DonVi.* FROM NS_DonVi WHERE iTrangThai=1 AND iID_MaDonViCha=@iID_MaDonViCha AND iNamLamViec_DonVi=@iNamLamViec";                
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@iID_MaDonViCha", MaDonViCha);
            }
            else
            {
                SQL = "SELECT DISTINCT NS_DonVi.iID_MaDonVi, NS_DonVi.sTen FROM NS_NguoiDung_DonVi INNER JOIN NS_DonVi ON (NS_NguoiDung_DonVi.iID_MaDonVi=NS_DonVi.iID_MaDonVi)";
                SQL += " WHERE iID_MaDonViCha is null AND  NS_NguoiDung_DonVi.iTrangThai=1 and NS_DonVi.iNamLamViec_DonVi=@iNamLamViec";
                SQL += " AND NS_DonVi.iTrangThai=1 AND sMaNguoiDung=@sMaNguoiDung ORDER BY NS_DonVi.sTen";
                cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                cmd.Parameters.AddWithValue("@sMaNguoiDung", MaND);                
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;

                string strPG = "", strXauMucLucQuanSoCon, strDoanTrang = "";

                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];
                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaDonVi"].ToString());
                    strXauMucLucQuanSoCon = LayXauDanhSachDonVi(Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaDonVi"]), Cap + 1, MaND, ref ThuTu, iNamLamViec);

                    if (strXauMucLucQuanSoCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaDonVi"].ToString());
                    }
                    strPG += string.Format("<tr>");

                    String Checkbox = String.Format("<input type=\"checkbox\" value=\"{0}\" check-group=\"DonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" onclick=\"ChonDonVi(this.checked,'{0}')\" />", Row["iID_MaDonVi"]);
                    if (Cap == 0)
                    {
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px; width: 30px;\">{0}<b>{1}</b></td>", strDoanTrang, Checkbox);
                        strPG += string.Format("<td style=\"background-color:#f4f9fd;padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sTen"]);
                    }
                    else
                    {
                        String Checkbox_PhongBan = String.Format("<input type=\"checkbox\" value=\"{0}\" check-group=\"{1}_PhongBan\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" onclick=\"ChonDonVi(this.checked,'{0}')\" />", Row["iID_MaDonVi"], MaDonViCha);
                        if (tgThuTu % 2 == 0)
                        {

                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Checkbox_PhongBan);
                            strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                        }
                        else
                        {
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Checkbox_PhongBan);
                            strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]);
                        }
                    }
                    if (tgThuTu % 2 == 0)
                    {
                        strPG += string.Format("<td style=\"background-color:#dff0fb;padding: 3px 3px;\">{0}</td>", strHanhDong);
                    }
                    else
                    {
                        strPG += string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", strHanhDong);
                    }

                    strPG += string.Format("</tr>");
                    strPG += strXauMucLucQuanSoCon;
                }
                vR = String.Format("{0}", strPG);
            }
            dt.Dispose();
            return vR;
        }
    }
}