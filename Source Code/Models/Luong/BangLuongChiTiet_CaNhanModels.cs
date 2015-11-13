using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class BangLuongChiTiet_CaNhanModels
    {
        private static String ThayLoaiCongThucVaoCongThuc(String sCongThuc, DataTable dtDanhMucLoaiCongThuc)
        {
            String strTG="";
            while (strTG != sCongThuc)
            {
                strTG = sCongThuc;
                for (int i = 0; i < dtDanhMucLoaiCongThuc.Rows.Count; i++)
                {
                    String TenCongThuc = String.Format("{0}", dtDanhMucLoaiCongThuc.Rows[i]["sTen"]);
                    String CongThuc = String.Format("({0})", dtDanhMucLoaiCongThuc.Rows[i]["sCongThuc"]);
                    sCongThuc = sCongThuc.Replace(TenCongThuc, CongThuc);
                }
            }
            return sCongThuc;
        }

        /// <summary>
        /// Tính tiền phụ cấp theo công thức
        ///     - Mục đích: lấy các giá trị có sẵn trong bảng lương chi tiết và bảng phụ cấp để đưa vào công thức sau đó tính giá trị của công thức
        ///     - Lỗi: Lỗi xuất hiện khi có 1 trường trong công thức không có trong bảng lương chi tiết và bảng phụ cấp
        /// </summary>
        /// <param name="sCongThuc">Công thức để tính</param>
        /// <param name="RowChiTiet">Chứa các giá trị của bảng lương chi tiết</param>
        /// <param name="RowPhuCap">Chứa các giá trị của bảng phụ cấp</param>
        /// <returns></returns>
        private static Double TinhTienPhuCapTheoCongThuc(String sCongThuc, DataRow RowChiTiet, DataRow RowPhuCap, DataTable dtDanhMucLoaiCongThuc, DataRow RowChiTietCu, DataTable dtPhuCapCu, ref Boolean okCoThayDoi)
        {
            okCoThayDoi = false;
            sCongThuc = ThayLoaiCongThucVaoCongThuc(sCongThuc, dtDanhMucLoaiCongThuc);
            double vR = 0;
            DataTable dtChiTiet = RowChiTiet.Table;
            DataTable dtPhuCap = RowPhuCap.Table;

            //Thay trường hệ số vào công thưc
            String TenTruong = "[rHeSo]";
            String GiaTri = Convert.ToString(RowPhuCap["rHeSo"]);
            if (GiaTri.IndexOf('.') < 0) GiaTri = GiaTri + ".0";
            GiaTri = String.Format("({0})", GiaTri);
            sCongThuc = sCongThuc.Replace(TenTruong, GiaTri);
            if (dtPhuCapCu == null)
            {
                okCoThayDoi = true;
            }
            else
            {
                for (int i = 0; i < dtPhuCapCu.Rows.Count; i++)
                {
                    if (Convert.ToInt32(RowPhuCap["iID_MaBangLuongChiTiet_PhuCap"]) == Convert.ToInt32(dtPhuCapCu.Rows[i]["iID_MaBangLuongChiTiet_PhuCap"]))
                    {
                        if (Convert.ToDouble(RowPhuCap["rHeSo"]) != Convert.ToDouble(dtPhuCapCu.Rows[i]["rHeSo"]))
                        {
                            okCoThayDoi = true;
                        }
                        break;
                    }
                }
            }

            //Thay trường từ bảng lương chi tiết
            Boolean okTiep = true;
            String TenTruongCu = "";
            while (okTiep)
            {
                TenTruong = CCongThuc.LayTruongTrongCongThuc(sCongThuc);
                if (TenTruong != "" && TenTruong != TenTruongCu)
                {
                    TenTruongCu = TenTruong;
                    GiaTri = Convert.ToString(RowChiTiet[TenTruong]).ToLower();
                    if (TenTruong.StartsWith("r"))
                    {
                        if (GiaTri.IndexOf('.') < 0) GiaTri = GiaTri + ".0";
                        GiaTri = String.Format("({0})", GiaTri);
                        if (RowChiTietCu == null || Convert.ToDouble(RowChiTiet[TenTruong]) != Convert.ToDouble(RowChiTietCu[TenTruong]))
                        {
                            okCoThayDoi = true;
                        }
                    }
                    else
                    {
                        if (RowChiTietCu == null || Convert.ToString(RowChiTiet[TenTruong]) != Convert.ToString(RowChiTietCu[TenTruong]))
                        {
                            okCoThayDoi = true;
                        }
                    }
                    TenTruong = String.Format("[{0}]", TenTruong);
                    sCongThuc = sCongThuc.Replace(TenTruong, GiaTri);
                }
                else
                {
                    okTiep = false;
                }
            }
            if (okCoThayDoi)
            {
                vR = Convert.ToDouble(CString.Evaluate(sCongThuc));
            }
            return vR;
        }

        /// <summary>
        /// Tính tiền theo công thức
        ///     - Mục đích: lấy các giá trị có sắn trong bảng lương chi tiết để đưa vào công thức sau đó tính giá trị của công thức
        ///     - Lỗi: Lỗi xuất hiện khi có 1 trường trong công thức không có trong bảng lương chi tiết
        /// </summary>
        /// <param name="sCongThuc">Công thức để tính</param>
        /// <param name="RowChiTiet">Chứa các giá trị của bảng lương chi tiết</param>
        /// <returns></returns>
        public static Double TinhTienTheoCongThuc(String sCongThuc, DataRow RowChiTiet, DataTable dtDanhMucLoaiCongThuc, DataRow RowChiTietCu, DataTable dtPhuCapCu, ref Boolean okCoThayDoi)
        {
            sCongThuc = ThayLoaiCongThucVaoCongThuc(sCongThuc, dtDanhMucLoaiCongThuc);
            double vR = 0;
            DataTable dtChiTiet = RowChiTiet.Table;
            Boolean okTiep = true;
            String TenTruongCu = "";
            while (okTiep)
            {
                String TenTruong = CCongThuc.LayTruongTrongCongThuc(sCongThuc);
                if (TenTruong != "" && TenTruong != TenTruongCu)
                {
                    TenTruongCu = TenTruong;
                    String GiaTri = Convert.ToString(RowChiTiet[TenTruong]).ToLower();
                    if (TenTruong.StartsWith("r"))
                    {
                        if (GiaTri.IndexOf('.') < 0) GiaTri = GiaTri + ".0";
                        GiaTri = String.Format("({0})", GiaTri);
                        if (RowChiTietCu == null || Convert.ToDouble(RowChiTietCu[TenTruong]) != Convert.ToDouble(RowChiTiet[TenTruong]))
                        {
                            okCoThayDoi = true;
                        }
                    }
                    else
                    {
                        if (RowChiTietCu == null || Convert.ToString(RowChiTietCu[TenTruong]) != Convert.ToString(RowChiTiet[TenTruong]))
                        {
                            okCoThayDoi = true;
                        }
                    }
                    TenTruong = String.Format("[{0}]", TenTruong);
                    sCongThuc = sCongThuc.Replace(TenTruong, GiaTri);
                }
                else
                {
                    okTiep = false;
                }
            }
            if (okCoThayDoi)
            {
                vR = Convert.ToDouble(CString.Evaluate(sCongThuc));
            }
            return vR;
        }

        /// <summary>
        /// Cập nhập các giá trị tử bảng phụ cấp lên bảng lương chi tiết
        /// </summary>
        /// <param name="iID_MaBangLuongChiTiet"></param>
        /// <param name="UserID"></param>
        /// <param name="IPSua"></param>
        public static void CapNhapLenBangLuongChiTiet(String iID_MaBangLuongChiTiet, String UserID, String IPSua, DataRow RowChiTietCu, DataTable dtPhuCapCu)
        {
            DataTable dtChiTiet = LuongModels.Get_ChiTietBangLuongChiTiet(iID_MaBangLuongChiTiet);
            DataTable dtChiTiet_Cu = dtChiTiet.Copy();
            DataTable dtChiTiet_PhuCap = LuongModels.Get_dtLuongPhuCap(iID_MaBangLuongChiTiet);
            DataTable dtChiTiet_PhuCap_Cu = dtChiTiet_PhuCap.Copy();

            DataTable dtDanhMucLoaiCongThuc = LuongModels.Get_dtDanhMucLoaiCongThuc();
            
            DataRow Row = dtChiTiet.Rows[0];
            DataRow Row_Cu = dtChiTiet_Cu.Rows[0];

            //Xác định các công thức của bảng lương chi tiêt
            List<String> arrTenTruongCongThuc = new List<string>();
            List<String> arrCongThuc = new List<string>();
            for (int i = 0; i < dtChiTiet.Columns.Count; i++)
            {
                if (dtChiTiet.Columns[i].ColumnName.EndsWith("_CongThuc"))
                {
                    String sCongThuc = Convert.ToString(Row[i]);
                    if (String.IsNullOrEmpty(sCongThuc) == false)
                    {
                        arrCongThuc.Add(sCongThuc);
                        String TenTruong = dtChiTiet.Columns[i].ColumnName;
                        int csC = TenTruong.LastIndexOf("_CongThuc");
                        TenTruong = String.Format("r{0}", TenTruong.Substring(1, csC - 1));
                        arrTenTruongCongThuc.Add(TenTruong);
                    }
                }
            }
            
            List<int> arrCS_ChiTiet = new List<int>();
            List<bool> arrDD_ChiTiet = new List<bool>();
            for (int i = 0; i < arrCongThuc.Count; i++)
            {
                arrDD_ChiTiet.Add(true);
            }
            //Sắp xếp lại công thức của bảng lương chi tiết
            for (int i = 0; i < arrCongThuc.Count; i++)
            {
                for (int j = 0; j < arrCongThuc.Count; j++)
                {
                    if (arrDD_ChiTiet[j])
                    {
                        String sCongThuc = arrCongThuc[j];
                        String Truong;
                        Boolean okCongThucKhongPhuThuoc = true;
                        do
                        {
                            Truong = CString.LayTruongTrongCongThuc(ref sCongThuc, "0");
                            for (int k = 0; k < arrCongThuc.Count; k++)
                            {
                                if (arrDD_ChiTiet[k])
                                {
                                    if (String.Format("[{0}]", arrTenTruongCongThuc[k]) == Truong)
                                    {
                                        okCongThucKhongPhuThuoc = false;
                                        break;
                                    }
                                }
                            }
                        } while (Truong != "" && okCongThucKhongPhuThuoc);
                        if (okCongThucKhongPhuThuoc)
                        {
                            arrDD_ChiTiet[i] = false;
                            arrCS_ChiTiet.Add(i);
                            break;
                        }
                    }
                }
            }
            //Tính lại công thức của bảng lương chi tiết
            for (int i = 0; i < arrCS_ChiTiet.Count; i++)
            {
                String sCongThuc = arrCongThuc[arrCS_ChiTiet[i]];
                String Truong = arrTenTruongCongThuc[arrCS_ChiTiet[i]];
                Boolean okGiaTriCoThayDoi = false;
                Double rGiaTri= TinhTienTheoCongThuc(sCongThuc, Row, dtDanhMucLoaiCongThuc, RowChiTietCu, dtPhuCapCu, ref okGiaTriCoThayDoi);
                if (okGiaTriCoThayDoi)
                {
                    Row[Truong] = rGiaTri;
                }
            }

            List<int> arrCS_PhuCap = new List<int>();
            List<Boolean> arrDD_PhuCap = new List<Boolean>();
            for (int i = 0; i < dtChiTiet_PhuCap.Rows.Count; i++)
            {
                arrDD_PhuCap.Add(true);
            }

            //Các phụ cấp không có công thức sẽ được tính trước
            for (int i = 0; i < dtChiTiet_PhuCap.Rows.Count; i++)
            {
                if(Convert.ToBoolean(dtChiTiet_PhuCap.Rows[i]["bCongThuc"])==false)
                {
                    arrDD_PhuCap[i] = false;
                    arrCS_PhuCap.Add(i);
                }
            }

            //Các phụ cấp có công thức sẽ được sắp xếp sao cho công thức sau không có trong công thức trước
            for (int i = arrCS_PhuCap.Count; i < dtChiTiet_PhuCap.Rows.Count; i++)
            {
                for (int j = 0; j < dtChiTiet_PhuCap.Rows.Count; j++)
                {
                    if (arrDD_PhuCap[j])
                    {
                        String sCongThuc = Convert.ToString(dtChiTiet_PhuCap.Rows[j]["sCongThuc"]);
                        String Truong;
                        Boolean okCongThucKhongPhuThuoc = true;
                        do{
                            Truong = CString.LayTruongTrongCongThuc(ref sCongThuc,"0");
                            if (String.IsNullOrEmpty(Truong)==false)
                            {
                                for (int k = 0; k < dtChiTiet_PhuCap.Rows.Count; k++)
                                {
                                    if (arrDD_PhuCap[k])
                                    {
                                        if (String.Format("[{0}]", dtChiTiet_PhuCap.Rows[k]["sMaTruongHeSo_BangLuong"]) == Truong ||
                                            String.Format("[{0}]", dtChiTiet_PhuCap.Rows[k]["sMaTruongSoTien_BangLuong"]) == Truong)
                                        {
                                            okCongThucKhongPhuThuoc = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }while(Truong!="" && okCongThucKhongPhuThuoc);
                        if(okCongThucKhongPhuThuoc)
                        {
                            arrDD_PhuCap[j] = false;
                            arrCS_PhuCap.Add(j);
                        }
                    }
                }
            }

            //Gán =0 các phụ cấp đã xóa
            for (int i = 0; i < dtChiTiet.Columns.Count; i++)
            {
                String TenTruong = dtChiTiet.Columns[i].ColumnName;
                if (TenTruong.StartsWith("rPhuCap_") && TenTruong != "rPhuCap_Tong" && TenTruong.EndsWith("_HeSo") == false)
                {
                    //if (strMaPhuCapDaTinh.IndexOf(TenTruong + ",") < 0)
                    //{
                        Row[TenTruong] = 0;
                        Row[TenTruong + "_HeSo"] = 0;
                        Row["s" + TenTruong.Substring(1) + "_MoTa"] = "";
                    //}
                }
            }

            //Tính tiền phụ cấp của từng mục phụ cấp, và lưu thông tin vào bảng lương chi tiết
            String strMaPhuCapDaTinh = ""; ;
            double rHeSo = 0;
            String sMoTa = "";
            double rTongTien = 0;
            for (int i = 0; i < arrCS_PhuCap.Count; i++)
            {
                DataRow RowPhuCap = dtChiTiet_PhuCap.Rows[arrCS_PhuCap[i]];
                rHeSo = Convert.ToDouble(RowPhuCap["rHeSo"]);
                rTongTien = Convert.ToDouble(RowPhuCap["rSoTien"]);
                sMoTa = Convert.ToString(RowPhuCap["iID_MaPhuCap"]);
                if (Convert.ToInt16(RowPhuCap["iLoaiMa"]) == 0)
                {
                    //0: Mã kiểu AB trong đó B là hệ số;
                    if (rHeSo > 0)
                    {
                        String sHeSo = Convert.ToString(rHeSo);
                        if (rHeSo < 10) sHeSo = "0" + sHeSo;
                        sMoTa += sHeSo;
                        RowPhuCap["sMaHT"] = sMoTa;
                    }
                }
                else if (Convert.ToInt16(RowPhuCap["iLoaiMa"]) == 1)
                {
                    //1: Mã theo kiểu AB, trong đó B*10 là hệ số;
                    if (rHeSo > 0)
                    {
                        String sHeSo = Convert.ToString(rHeSo / 10);
                        if (rHeSo < 100) sHeSo = "0" + sHeSo;
                        sHeSo = sHeSo.Replace(".", "");
                        while (sHeSo.EndsWith("0"))
                        {
                            sHeSo = sHeSo.Substring(0, sHeSo.Length - 1);
                        }
                        sMoTa += sHeSo;
                        RowPhuCap["sMaHT"] = sMoTa;
                    }
                }
                if (Convert.ToBoolean(RowPhuCap["bCongThuc"]))
                {
                    //Nếu tính tiền theo công thức thì tính lại tiền phụ cấp
                    String sCongThuc = Convert.ToString(RowPhuCap["sCongThuc"]);
                    Boolean okGiaTriCoThayDoi = false;
                    Double tgThanhTien = TinhTienPhuCapTheoCongThuc(sCongThuc, Row, RowPhuCap, dtDanhMucLoaiCongThuc, RowChiTietCu, dtPhuCapCu, ref okGiaTriCoThayDoi);
                    if (okGiaTriCoThayDoi)
                    {
                        rTongTien = tgThanhTien;
                        RowPhuCap["rSoTien"] = rTongTien;
                    }
                }
                if (Convert.ToString(RowPhuCap["sMaTruongHeSo_BangLuong"]) != "")
                {
                    //Lưu hệ số vào bảng chi tiết lương
                    String TenTruong = Convert.ToString(RowPhuCap["sMaTruongHeSo_BangLuong"]);
                    Row[TenTruong] = rHeSo;
                }
                if (Convert.ToString(RowPhuCap["sMaTruongSoTien_BangLuong"]) != "")
                {
                    //Cộng tổng tiền vào chi tiết lương
                    String TenTruong = Convert.ToString(RowPhuCap["sMaTruongSoTien_BangLuong"]);
                    if (strMaPhuCapDaTinh.IndexOf(TenTruong + ",") >= 0)
                    {
                        Row[TenTruong] = Convert.ToDouble(Row[TenTruong]) + rTongTien;
                    }
                    else
                    {
                        strMaPhuCapDaTinh += TenTruong + ",";
                        Row[TenTruong] = rTongTien;
                    }
                    //Lưu lại mã phụ cấp vào mô tả
                    String TenTruongMoTa = String.Format("s{0}_MoTa", TenTruong.Substring(1));
                    if (Convert.ToString(Row[TenTruongMoTa])!="")
                    {
                        Row[TenTruongMoTa] = String.Format("{0},{1}", Row[TenTruongMoTa], sMoTa);
                    }
                    else
                    {
                        if ((Convert.ToString(Row[TenTruongMoTa]) + ",").IndexOf(TenTruongMoTa + ",") == -1)
                        {
                            Row[TenTruongMoTa] = sMoTa;
                        }
                    }
                }
            }

            

            //Tính lại công thức của bảng lương chi tiết sau khi đã tính tổng phụ cấp
            for (int i = 0; i < arrCS_ChiTiet.Count; i++)
            {
                String sCongThuc = arrCongThuc[arrCS_ChiTiet[i]];
                String Truong = arrTenTruongCongThuc[arrCS_ChiTiet[i]];
                Boolean okGiaTriCoThayDoi = false;
                Double rGiaTri = TinhTienTheoCongThuc(sCongThuc, Row, dtDanhMucLoaiCongThuc, RowChiTietCu, dtPhuCapCu, ref okGiaTriCoThayDoi);
                if (okGiaTriCoThayDoi)
                {
                    Row[Truong] = rGiaTri;
                }
            }

            {
                Bang bang = new Bang("L_BangLuongChiTiet");
                bang.GiaTriKhoa = iID_MaBangLuongChiTiet;
                bang.DuLieuMoi = false;
                bang.MaNguoiDungSua = UserID;
                bang.IPSua = IPSua;
                Boolean CoThayDoi = false;
                for (int i = 0; i < dtChiTiet.Columns.Count; i++)
                {
                    if ((dtChiTiet.Columns[i].ColumnName.StartsWith("r") && checkDouble(dtChiTiet_Cu.Rows[0][i]) != checkDouble(Row[i])) ||
                        (dtChiTiet.Columns[i].ColumnName.StartsWith("r") == false && Convert.ToString(dtChiTiet_Cu.Rows[0][i]) != Convert.ToString(Row[i])))
                    
                    {
                        bang.CmdParams.Parameters.AddWithValue("@" + dtChiTiet.Columns[i].ColumnName, Row[i]);
                        CoThayDoi = true;
                    }
                }
                if (CoThayDoi)
                {
                    bang.Save();
                }
            }

            for(int i=0;i<dtChiTiet_PhuCap.Rows.Count;i++)
            {
                Bang bang_PhuCap = new Bang("L_BangLuongChiTiet_PhuCap");
                bang_PhuCap.GiaTriKhoa = dtChiTiet_PhuCap.Rows[i]["iID_MaBangLuongChiTiet_PhuCap"];
                bang_PhuCap.DuLieuMoi = false;
                bang_PhuCap.MaNguoiDungSua = UserID;
                bang_PhuCap.IPSua = IPSua;
                Boolean CoThayDoi = false;
                for (int j = 0; j < dtChiTiet_PhuCap.Columns.Count; j++)
                {
                    if (Convert.ToString(dtChiTiet_PhuCap.Rows[i][j]) != Convert.ToString(dtChiTiet_PhuCap_Cu.Rows[i][j]))
                    {
                        bang_PhuCap.CmdParams.Parameters.AddWithValue("@" + dtChiTiet_PhuCap.Columns[j].ColumnName, dtChiTiet_PhuCap.Rows[i][j]);
                        CoThayDoi = true;
                    }
                }
                if (CoThayDoi)
                {
                    bang_PhuCap.Save();
                }
            }

            dtChiTiet.Dispose();
            dtChiTiet_PhuCap.Dispose();
        }
        public static double checkDouble(object obj)
        {
            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return 0;
            }
        }
        public static string checkString(object obj)
        {
            try
            {
                return Convert.ToString(obj);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// Thêm 1 hàng vào bảng lương chi tiết
        /// </summary>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="iID_MaCanBo"></param>
        /// <param name="UserID"></param>
        /// <param name="IPSua"></param>
        /// <returns></returns>
        public static String ThemLuongChiTiet_CanBo(String iID_MaBangLuong, 
                                                    String iID_MaCanBo,
                                                    String UserID, String IPSua,
                                                    NameValueCollection prms = null)
        {
            Boolean okLayTuBangLuongThangTruoc = false;

            //Lấy thông tin cán bộ
            DataTable dtCanBo = null;
            String iID_MaDonVi = "";
            if (!String.IsNullOrEmpty(iID_MaCanBo))
            {
                dtCanBo = CanBo_HoSoNhanSuModels.GetChiTiet(iID_MaCanBo);
                DataRow Row_CanBo = dtCanBo.Rows[0];
                iID_MaDonVi = Convert.ToString(Row_CanBo["iID_MaDonVi"]);
            }

            //String iID_MaDonVi = Convert.ToString(Row_CanBo["iID_MaDonVi"]);
            String sTenDonVi = String.Format("{0} - {1}", iID_MaDonVi, DonViModels.Get_TenDonVi(iID_MaDonVi));

            //Lấy thông tin bảng lương
            DataTable dtLuong = LuongModels.Get_ChiTietBangLuong(iID_MaBangLuong);
            int iThangBangLuong = Convert.ToInt32(dtLuong.Rows[0]["iThangBangLuong"]);
            int iNamBangLuong = Convert.ToInt32(dtLuong.Rows[0]["iNamBangLuong"]);
            DateTime ThoiGian = new DateTime(iNamBangLuong, iThangBangLuong, 1);
            int iSoNgayTrongThang = CDate.LaySoNgayTrongThang(ThoiGian);
            
            //Lấy thông tin bảng lương tháng trước
            DateTime ThoiGian_BangLuongTruoc = ThoiGian.AddMonths(-1);
            int iThangBangLuong_Truoc = ThoiGian_BangLuongTruoc.Month;
            int iNamBangLuong_Truoc = ThoiGian_BangLuongTruoc.Year;
            String iID_MaBangLuong_ThangTruoc = LuongModels.Get_iID_MaBangLuong(iID_MaDonVi, iNamBangLuong_Truoc, iThangBangLuong_Truoc);

            DataTable dtChiTiet_ThangTruoc = null;
            DataTable dtPhuCap_ThangTruoc = null;
            if (String.IsNullOrEmpty(iID_MaBangLuong_ThangTruoc) == false)
            {
                //Nếu có bảng lương tháng trước
                //dtChiTiet_ThangTruoc = Get_DataTable_LuongCanBo(iID_MaBangLuong_ThangTruoc, iID_MaCanBo);
                dtChiTiet_ThangTruoc = Get_DataTable_LuongCanBo(iID_MaBangLuong_ThangTruoc);
                if (dtChiTiet_ThangTruoc.Rows.Count > 0)
                {
                    //Lấy các phụ cấp có sẵn của cán bộ từ bảng lương tháng trước
                    dtPhuCap_ThangTruoc = Get_DataTable_PhuCap(Convert.ToString(dtChiTiet_ThangTruoc.Rows[0]["iID_MaBangLuongChiTiet"]));
                }
                else
                {
                    dtChiTiet_ThangTruoc.Dispose();
                    dtChiTiet_ThangTruoc = null;
                }
            }

            //Xác định các tham số
            DataTable dtThamSo = LuongModels.Get_DanhMucThamSo_SapXep(ThoiGian);

            DataTable dtPhuCapLuonCo = Luong_DanhMucPhuCapModels.get_dtPhuCapLuonCo();
            BangCSDL bangCSDL = new BangCSDL("L_BangLuongChiTiet");
            BangCSDL bang_PhuCapCSDL = new BangCSDL("L_BangLuongChiTiet_PhuCap");
            List<String> arrDSMaCot = bangCSDL.DanhSachTruong();
            List<String> arrDSMaCot_PhuCap = bang_PhuCapCSDL.DanhSachTruong();

            //Lưu bảng lương chi tiết
            Bang bang = new Bang("L_BangLuongChiTiet");
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = UserID;
            bang.IPSua = IPSua;
            bang.CmdParams.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            //bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
            //bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
            //bang.CmdParams.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            bang.CmdParams.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
            bang.CmdParams.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);

            if (dtChiTiet_ThangTruoc!=null)
            {
                //Trường hợp có bảng lương tháng trước: Chuyển tất cả các thông tin chi tiết + phụ cấp từ tháng trước
                int csMin_ChiTiet = dtChiTiet_ThangTruoc.Columns.IndexOf("i_TRUONG_CHOT1") + 1;
                int csMax_ChiTiet = dtChiTiet_ThangTruoc.Columns.IndexOf("iSTT");

                DataRow RowChiTiet = dtChiTiet_ThangTruoc.Rows[0];
                //<--Copy dữ liệu của tháng trước
                for (int jChiTiet = csMin_ChiTiet; jChiTiet < csMax_ChiTiet; jChiTiet++)
                {
                    String TenTruong = dtChiTiet_ThangTruoc.Columns[jChiTiet].ColumnName;
                    bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, RowChiTiet[jChiTiet]);
                }
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", RowChiTiet["iID_MaDonVi"]);
                bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", RowChiTiet["sTenDonVi"]);
                bang.CmdParams.Parameters["@sHieuTangGiam"].Value = "S";
                //-->Copy dữ liệu của tháng trước
                okLayTuBangLuongThangTruoc = true;
            }
            else
            {
                bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                bang.CmdParams.Parameters.AddWithValue("@sTenDonVi", sTenDonVi);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
                //Cán bộ không có trong bảng lương nhưng có trong danh sách đơn vị: Thêm các tham số từ bảng cán bộ sang
                if (dtCanBo != null && dtCanBo.Rows.Count > 0)
                {
                    DataRow Row_CanBo = dtCanBo.Rows[0];
                    for (int jCanBo = 0; jCanBo < dtCanBo.Columns.Count; jCanBo++)
                    {
                        String TenTruong = dtCanBo.Columns[jCanBo].ColumnName;
                        for (int jMaCot = 0; jMaCot < arrDSMaCot.Count; jMaCot++)
                        {

                            if (TenTruong + "_CanBo" == arrDSMaCot[jMaCot])
                            {
                                bang.CmdParams.Parameters.AddWithValue("@" + arrDSMaCot[jMaCot], Row_CanBo[jCanBo]);
                                break;
                            }
                            else if (TenTruong == "iID_MaLyDoTangGiam")
                            {
                                bang.CmdParams.Parameters.AddWithValue("@sKyHieu_MucLucQuanSo_HieuTangGiam",
                                                                       Row_CanBo[jCanBo]);
                                break;
                            }
                        }
                    }
                }
                //Thêm các giá trị từ bảng prms
                if (prms != null)
                {
                    for (int jMaCot = 0; jMaCot < arrDSMaCot.Count; jMaCot++)
                    {
                        String TenTruong = arrDSMaCot[jMaCot];
                        if (String.IsNullOrEmpty(prms["Parent_" + TenTruong]) == false)
                        {
                            bang.CmdParams.Parameters.AddWithValue("@" + TenTruong, prms["Parent_" + TenTruong]);
                        }
                    }
                    if (prms["Parent_iID_MaLyDoTangGiam"] == "")
                    {
                        bang.CmdParams.Parameters.AddWithValue("@sKyHieu_MucLucQuanSo_HieuTangGiam", "S");
                    }
                    else if (prms["Parent_iID_MaLyDoTangGiam"].StartsWith("3"))
                    {
                        bang.CmdParams.Parameters.AddWithValue("@sKyHieu_MucLucQuanSo_HieuTangGiam", "G");
                    }
                    else
                    {
                        bang.CmdParams.Parameters.AddWithValue("@sKyHieu_MucLucQuanSo_HieuTangGiam", "T");
                    }
                }
            }

            //<--Điền thông tin từ bảng Bậc lương
            String iID_MaNgachLuong = checkString(bang.CmdParams.Parameters["@iID_MaNgachLuong_CanBo"].Value);
            String iID_MaBacLuong = checkString(bang.CmdParams.Parameters["@iID_MaBacLuong_CanBo"].Value);
            DataTable dtBacLuong = LuongModels.Get_ChiTietDanhMucBacLuong(iID_MaNgachLuong, iID_MaBacLuong);

            Double rHeSoLuong = checkDouble(dtBacLuong.Rows[0]["rHeSoLuong"]);
            Double rHeSo_ANQP = checkDouble(dtBacLuong.Rows[0]["rHeSo_ANQP"]);
            Object rLuongCoBan_HeSo_CanBo_Cu = 0;
            if (rHeSoLuong > 0)
            {
                rLuongCoBan_HeSo_CanBo_Cu = CommonFunction.ThemGiaTriVaoThamSo(bang.CmdParams.Parameters, "@rLuongCoBan_HeSo_CanBo", rHeSoLuong);
            }
            Object rPhuCap_AnNinhQuocPhong_HeSo_Cu = CommonFunction.ThemGiaTriVaoThamSo(bang.CmdParams.Parameters, "@rPhuCap_AnNinhQuocPhong_HeSo", rHeSo_ANQP); 
            dtBacLuong.Dispose();
            //-->Điền thông tin từ bảng Bậc lương

            //<--Điền thông tin iSoNgayTrongThang
            Object iSoNgayTrongThang_Cu = CommonFunction.ThemGiaTriVaoThamSo(bang.CmdParams.Parameters, "@iSoNgayTrongThang", iSoNgayTrongThang);
            //-->Điền thông tin iSoNgayTrongThang

            //<--Điền thông tin từ bảng Tham số
            //Lấy tham số cuối cùng có hiệu lực
            //Ví dụ: Tham số 'A' có 2 hàng có hiệu lực từ (1/11->..) và (1/12->5/12) thì chọn tham số sau
            for (int jDSMaCot = 0; jDSMaCot < arrDSMaCot.Count; jDSMaCot++)
            {
                String TenTruong = arrDSMaCot[jDSMaCot];
                int csThamSo = -1;
                for (int iThamSo = 0; iThamSo < dtThamSo.Rows.Count; iThamSo++)
                {
                    if (TenTruong == Convert.ToString(dtThamSo.Rows[iThamSo]["sKyHieu"]))
                    {
                        csThamSo = iThamSo;
                    }
                }
                if (csThamSo >= 0)
                {
                    CommonFunction.ThemGiaTriVaoThamSo(bang.CmdParams.Parameters, TenTruong, dtThamSo.Rows[csThamSo]["sThamSo"]);
                }
            }
            //-->Điền thông tin từ bảng Tham số

            //<--Lưu thông tin của bảng lương chi tiết
            BangLuongChiTietModels.DieuChinhLaiBangTruocKhiGhi(bang, iNamBangLuong,iThangBangLuong);
            String iID_MaBangLuongChiTiet = Convert.ToString(bang.Save());
            //-->Lưu thông tin của bảng lương chi tiết

            //Lưu bảng phụ cấp chi tiết
            if (okLayTuBangLuongThangTruoc)
            {
                //<--Thêm các phụ cấp khác từ bảng lương của tháng trước
                String iID_MaBangLuongChiTiet_ThangTruoc = Convert.ToString(dtChiTiet_ThangTruoc.Rows[0]["iID_MaBangLuongChiTiet"]);
                int csMin_PhuCap_ThangTruoc = dtPhuCap_ThangTruoc.Columns.IndexOf("i_TRUONG_CHOT1") + 1;
                int csMax_PhuCap_ThangTruoc = dtPhuCap_ThangTruoc.Columns.IndexOf("iSTT");
                for (int iPhuCap_ThangTruoc = 0; iPhuCap_ThangTruoc < dtPhuCap_ThangTruoc.Rows.Count; iPhuCap_ThangTruoc++)
                {
                    DataRow Row_PhuCap_ThangTruoc = dtPhuCap_ThangTruoc.Rows[iPhuCap_ThangTruoc];                        
                    Bang bang_PhuCap = new Bang("L_BangLuongChiTiet_PhuCap");
                    bang_PhuCap.DuLieuMoi = true;
                    bang_PhuCap.MaNguoiDungSua = UserID;
                    bang_PhuCap.IPSua = IPSua;
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
                    for (int j = csMin_PhuCap_ThangTruoc; j < csMax_PhuCap_ThangTruoc; j++)
                    {
                        CommonFunction.ThemGiaTriVaoThamSo(bang_PhuCap.CmdParams.Parameters, dtPhuCap_ThangTruoc.Columns[j].ColumnName, Row_PhuCap_ThangTruoc[j]);
                    }
                    bang_PhuCap.Save();
                }
                //<--END: Thêm các phụ cấp khác từ bảng lương của tháng trước
            }
            else
            {
                for (int iPhuCap = 0; iPhuCap < dtPhuCapLuonCo.Rows.Count; iPhuCap++)
                {
                    DataRow Row_PhuCap = dtPhuCapLuonCo.Rows[iPhuCap];
                    String iID_MaPhuCap = Convert.ToString(Row_PhuCap["iID_MaPhuCap"]);
                    String sMaTruongHeSo_BangLuong = Convert.ToString(Row_PhuCap["sMaTruongHeSo_BangLuong"]);

                    Bang bang_PhuCap = new Bang("L_BangLuongChiTiet_PhuCap");
                    bang_PhuCap.DuLieuMoi = true;
                    bang_PhuCap.MaNguoiDungSua = UserID;
                    bang_PhuCap.IPSua = IPSua;
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iNamBangLuong", iNamBangLuong);
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@iThangBangLuong", iThangBangLuong);
                    bang_PhuCap.CmdParams.Parameters.AddWithValue("@sMaHT", iID_MaPhuCap);

                    int csMin_PhuCap = 0;
                    int csMax_PhuCap = dtPhuCapLuonCo.Columns.IndexOf("iSTT");
                    for (int j = csMin_PhuCap; j < csMax_PhuCap; j++)
                    {
                        String TenTruong = dtPhuCapLuonCo.Columns[j].ColumnName;
                        bang_PhuCap.CmdParams.Parameters.AddWithValue("@" + TenTruong, Row_PhuCap[j]);
                    }
                    //Thêm các giá trị từ bảng prms
                    if (String.IsNullOrEmpty(sMaTruongHeSo_BangLuong) == false && sMaTruongHeSo_BangLuong == "rPhuCap_AnNinhQuocPhong_HeSo")
                    {
                        CommonFunction.ThemGiaTriVaoThamSo(bang_PhuCap.CmdParams.Parameters, "@rHeSo", rHeSo_ANQP);
                    }
                        else if (String.IsNullOrEmpty(sMaTruongHeSo_BangLuong) == false && prms != null && String.IsNullOrEmpty(prms["Parent_" + sMaTruongHeSo_BangLuong]) == false)
                    {
                        CommonFunction.ThemGiaTriVaoThamSo(bang_PhuCap.CmdParams.Parameters, "@rHeSo", prms["Parent_" + sMaTruongHeSo_BangLuong]);
                    }
                    bang_PhuCap.Save();
                }
            }


            if (dtCanBo != null)
            {


                dtCanBo.Dispose();
            }
            if (dtLuong != null) dtLuong.Dispose();
            if (dtThamSo != null) dtThamSo.Dispose();
            if (dtPhuCapLuonCo != null) dtPhuCapLuonCo.Dispose();
            
            //<--END: Lưu bảng phụ cấp chi tiết

            //Tính toán lại các công thức của cán bộ
            if (okLayTuBangLuongThangTruoc)
            {
                DataTable dtChiTiet_PhuCap_Cu = BangLuongChiTietModels.Get_DataTable_PhuCap("", "", "", iID_MaBangLuongChiTiet);
                CapNhapBangLuongChiTiet(iID_MaBangLuongChiTiet, UserID, IPSua, dtChiTiet_ThangTruoc.Rows[0], dtChiTiet_PhuCap_Cu);
            }
            else
            {
                CapNhapBangLuongChiTiet(iID_MaBangLuongChiTiet, UserID, IPSua, null, null);
            }

            if (dtChiTiet_ThangTruoc != null) dtChiTiet_ThangTruoc.Dispose();
            if (dtPhuCap_ThangTruoc != null) dtChiTiet_ThangTruoc.Dispose();
            return iID_MaBangLuongChiTiet;
        }

        

        /// <summary>
        /// Tính toán các giá trị theo công thức của bảng phụ cấp và bảng lương chi tiết
        /// </summary>
        /// <param name="iID_MaBangLuongChiTiet"></param>
        /// <param name="UserID"></param>
        /// <param name="IPSua"></param>
        public static void CapNhapBangLuongChiTiet(String iID_MaBangLuongChiTiet, String UserID, String IPSua, DataRow RowChiTietCu, DataTable dtPhuCapCu)
        {
            BangLuongChiTiet_CaNhanModels.CapNhapLenBangLuongChiTiet(iID_MaBangLuongChiTiet, UserID, IPSua, RowChiTietCu, dtPhuCapCu);
            BangLuong_TruyLinhModels.CapNhapBangLuongTruyLinh(iID_MaBangLuongChiTiet, UserID, IPSua);
        }

        /// <summary>
        /// Lấy thông tin của 1 cán bộ trong bảng lương biết iID_MaBangLuongChiTiet
        /// </summary>
        /// <param name="iID_MaBangLuongChiTiet"></param>
        /// <returns></returns>
        public static DataTable Get_DataTable(String iID_MaBangLuongChiTiet)
        {
            DataTable dt = null;
            String DK = "";

            SqlCommand cmd = new SqlCommand();
            DK = "iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet";
            cmd.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
            String SQL = "SELECT * FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

        /// <summary>
        /// Lấy thông tin về lương chi tiết của 1 cán bộ trong bản lương
        /// </summary>
        /// <param name="iID_MaBangLuong"></param>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static DataTable Get_DataTable_LuongCanBo(String iID_MaBangLuong, String iID_MaCanBo)
        {
            DataTable dt = null;
            String DK = "";

            SqlCommand cmd = new SqlCommand();
            DK = "iID_MaBangLuong=@iID_MaBangLuong AND iID_MaCanBo=@iID_MaCanBo";
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            String SQL = "SELECT * FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND bPhanTruyLinh=0 AND {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        public static DataTable Get_DataTable_LuongCanBo(String iID_MaBangLuong)
        {
            DataTable dt = null;
            String DK = "";

            SqlCommand cmd = new SqlCommand();
            DK = "iID_MaBangLuong=@iID_MaBangLuong";
            cmd.Parameters.AddWithValue("@iID_MaBangLuong", iID_MaBangLuong);
            
            String SQL = "SELECT * FROM L_BangLuongChiTiet WHERE iTrangThai=1 AND bPhanTruyLinh=0 AND {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }
        public static DataTable Get_DataTable_PhuCap(String iID_MaBangLuongChiTiet)
        {
            DataTable dt = null;
            String DK = "";

            SqlCommand cmd = new SqlCommand();
            DK = "iID_MaBangLuongChiTiet=@iID_MaBangLuongChiTiet";
            cmd.Parameters.AddWithValue("@iID_MaBangLuongChiTiet", iID_MaBangLuongChiTiet);
            String SQL = "SELECT * FROM L_BangLuongChiTiet_PhuCap WHERE iTrangThai=1 AND {0}";
            SQL = String.Format(SQL, DK);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            return dt;
        }

    }
}