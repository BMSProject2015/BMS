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

namespace VIETTEL.Models
{
    public class TCDN_BaoCaoTaiChinh_BangDuLieu:BangDuLieu
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaDoanhNghiep"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public TCDN_BaoCaoTaiChinh_BangDuLieu(String iNamLamViec, String iQuy, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            //this._iID_Ma = iQuy;
            this._MaND = MaND;
            this._IPSua = IPSua;

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeThongKeTCDN, MaND);
            _dtChiTiet = TCDN_BaoCaoTaiChinhModels.Get_dtBaoCaoChiTiet(iNamLamViec, iQuy, arrGiaTriTimKiem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();

            DienDuLieu();
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu()
        {
            if (_arrDuLieu == null)
            {
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
            }
        }

        /// <summary>
        /// Hàm cập nhập vào tham số _arrDSMaHang
        /// </summary>
        protected void CapNhapDanhSachMaHang()
        {
            _arrDSMaHang = new List<string>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                DataRow R = _dtChiTiet.Rows[i];
                String MaHang = Convert.ToString(R["iID_MaBaoCaoTaiChinh"]);
                _arrDSMaHang.Add(MaHang);
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Cột Fixed của bảng gồm:
        ///         + Các trường của mục lục quân số
        ///     - Cập nhập số lượng cột Fixed
        /// </summary>
        protected void CapNhapDanhSachMaCot_Fixed()
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();

            String strDSTruongTienTieuDe = "Vốn điều lệ (ngàn đồng),Vốn NN (ngàn đồng),Tỷ lệ %,Tổng,Vốn đầu tư,Thặng dư,Quỹ ĐTư PT,Quĩ DP TC,Chênh lệch tỷ giá,Lợi nhuận chưa PP,Vốn khác,Doanh thu,Lợi nhuận trước thuế,Lợi nhuận sau thuế,Bằng tiền,Bằng cổ phiểu,Cộng,Nộp ngân sách,Lao động BQ,Tổng quĩ lương,THu nhập BQ,Phải nộp,Đã nộp,Còn phải nộp,Cổ tức các năm trước,Cổ tức năm nay,Đã nộp,Còn phải nộp,Tiền đất chưa nộp,Tiền đất năm nay,Đã nộp,Còn phải nộp,Tổng số còn phải nộp,Tổng số đã nộp,Ghi chú";
            String strDSTruongTien = "rVongDieuLe,rVonNhaNuoc,rTyLe,rTongVon_ChuSoHuu,rVonDauTu_ChuSoHuu,rThangDu_ChuSoHuu,rQuyDTPT_ChuSoHuu,rQuyDPPT_ChuSoHuu,rChenhLechTyGia_ChuSoHuu,rLoiNhuanChuaPP_ChuSoHuu,rVonKhac_ChuSoHuu,rDoanhThu,rLoiNhuanTruocThue,rLoiNhuanSauThue,rBangTien_VonNhaNuoc,rBangCoPhieu_VonNhaNuoc,rCong_VonNhaNuoc,rNopNganSach,rLaoDongBinhQuan,rTongQuyLuong,rThuNhapBinhQuan,rPhaiNop_VonNhaNuocKhiCoPhanHoa,rDaNop_VonNhaNuocKhiCoPhanHoa,rConPhaiNop_VonNhaNuocKhiCoPhanHoa,rCoTucNamTruoc_CoTuc,rCoTucNamNay_CoTuc,rDaNop_CoTuc,rConPhaiNop_CoTuc,rTienDatChuaNop_TienThueDat,rTienDatNamNay_TienThueDat,rDaNop_TienThueDat,rConPhaiNop_TienThueDat,rTongSoConPhaiNop,rTongSoDaNop,sGhiChu";
            String strDSTruongTienDoRong = "150,150,60,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,350";
            String strDSTruongTieuDe = "Tên công ty";
            String strDSTruong = "sTenDoanhNghiep";
            String strDSTruongDoRong = "350"; 

            String[] arrDSTruong = strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = strDSTruongTieuDe.Split(',');
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            String[] arrDSTruongTienTieuDe = strDSTruongTienTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = strDSTruongTienDoRong.Split(',');
            String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');
            String[] arrDSNhom_TieuDe = ",,,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,,,,Lợi nhuận chia theo vốn nhà nước,Lợi nhuận chia theo vốn nhà nước,Lợi nhuận chia theo vốn nhà nước,,,,,Tiền bán vốn nhà nước khi cổ phần hóa,Tiền bán vốn nhà nước khi cổ phần hóa,Tiền bán vốn nhà nước khi cổ phần hóa,Cổ tức,Cổ tức,Cổ tức,Cổ tức,Tiền thuê đất,Tiền thuê đất,Tiền thuê đất,Tiền thuê đất,,,".Split(',');
            String[] arrDSNhom_SoCot = "1,1,1,8,8,8,8,8,8,8,8,1,1,1,3,3,3,1,1,1,1,3,3,3,4,4,4,4,4,4,4,4,1,1,1".Split(',');


            //Tiêu đề fix: Thêm trường sMaCongTrinh, sTenCongTrinh
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
            }

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường của cột tiền
        ///         + Trường sTongSo
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String strDSTruongTienTieuDe = "Vốn điều lệ (ngàn đồng),Vốn NN (ngàn đồng),Tỷ lệ %,Tổng,Vốn đầu tư,Thặng dư,Quỹ ĐTư PT,Quĩ DP TC,Chênh lệch tỷ giá,Lợi nhuận chưa PP,Vốn khác,Doanh thu,Lợi nhuận trước thuế,Lợi nhuận sau thuế,Bằng tiền,Bằng cổ phiểu,Cộng,Nộp ngân sách,Lao động BQ,Tổng quĩ lương,THu nhập BQ,Phải nộp,Đã nộp,Còn phải nộp,Cổ tức các năm trước,Cổ tức năm nay,Đã nộp,Còn phải nộp,Tiền đất chưa nộp,Tiền đất năm nay,Đã nộp,Còn phải nộp,Tổng số còn phải nộp,Tổng số đã nộp,Ghi chú";
            String strDSTruongTien = "rVongDieuLe,rVonNhaNuoc,rTyLe,rTongVon_ChuSoHuu,rVonDauTu_ChuSoHuu,rThangDu_ChuSoHuu,rQuyDTPT_ChuSoHuu,rQuyDPPT_ChuSoHuu,rChenhLechTyGia_ChuSoHuu,rLoiNhuanChuaPP_ChuSoHuu,rVonKhac_ChuSoHuu,rDoanhThu,rLoiNhuanTruocThue,rLoiNhuanSauThue,rBangTien_VonNhaNuoc,rBangCoPhieu_VonNhaNuoc,rCong_VonNhaNuoc,rNopNganSach,rLaoDongBinhQuan,rTongQuyLuong,rThuNhapBinhQuan,rPhaiNop_VonNhaNuocKhiCoPhanHoa,rDaNop_VonNhaNuocKhiCoPhanHoa,rConPhaiNop_VonNhaNuocKhiCoPhanHoa,rCoTucNamTruoc_CoTuc,rCoTucNamNay_CoTuc,rDaNop_CoTuc,rConPhaiNop_CoTuc,rTienDatChuaNop_TienThueDat,rTienDatNamNay_TienThueDat,rDaNop_TienThueDat,rConPhaiNop_TienThueDat,rTongSoConPhaiNop,rTongSoDaNop,sGhiChu";
            String strDSTruongTienDoRong = "150,150,60,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,150,350"; 
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            String[] arrDSTruongTienTieuDe = strDSTruongTienTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = strDSTruongTienDoRong.Split(',');
            String[] arrDSNhom_TieuDe = ",,,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,Vốn chủ sở hữu,,,,Lợi nhuận chia theo vốn nhà nước,Lợi nhuận chia theo vốn nhà nước,Lợi nhuận chia theo vốn nhà nước,,,,,Tiền bán vốn nhà nước khi cổ phần hóa,Tiền bán vốn nhà nước khi cổ phần hóa,Tiền bán vốn nhà nước khi cổ phần hóa,Cổ tức,Cổ tức,Cổ tức,Cổ tức,Tiền thuê đất,Tiền thuê đất,Tiền thuê đất,Tiền thuê đất,,,".Split(',');
            String[] arrDSNhom_SoCot = "1,1,1,8,8,8,8,8,8,8,8,1,1,1,3,3,3,1,1,1,1,3,3,3,4,4,4,4,4,4,4,4,1,1,1".Split(',');

            //Tiêu đề tiền
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruongTien[j]);
                _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
            }

            //Them cot duyet
            if (CoCotDuyet)
            {
                //Cột đồng ý
                _arrDSMaCot.Add("bDongY");
                if (_ChiDoc)
                {
                    _arrTieuDe.Add("<div class='check'></div>");
                }
                else
                {
                    _arrTieuDe.Add("<div class='check' onclick='BangDuLieu_CheckAll();'></div>");
                }
                _arrWidth.Add(20);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
                //Cột Lý do
                _arrDSMaCot.Add("sLyDo");
                _arrTieuDe.Add("Nhận xét");
                _arrWidth.Add(200);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }

            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        }

        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCot_Them()
        {
        }

        /// <summary>
        /// Hàm xác định hàng cha, con
        /// </summary>
        protected void CapNhap_arrLaHangCha()
        {
            //Xác định hàng là hàng cha, con
            _arrCSCha = new List<int>();
            _arrLaHangCha = new List<bool>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                DataRow R = _dtChiTiet.Rows[i];
                //Xac dinh hang nay co phai la hang cha khong?
                //_arrLaHangCha.Add(Convert.ToBoolean(R["bLaHangCha"]));
                //int CSCha = -1;
                //for (int j = i - 1; j >= 0; j--)
                //{
                //    if (Convert.ToString(R["iID_MaChiTieuHoSo_Cha"]) == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaChiTieuHoSo"]))
                //    {
                //        CSCha = j;
                //        break;
                //    }
                //}
                //_arrCSCha.Add(CSCha);
            }
        }

        /// <summary>
        /// Hàm xác định các ô có được Edit hay không
        /// </summary>
        protected void CapNhap_arrEdit()
        {
            _arrEdit = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                Boolean okHangChiDoc = false;
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];

                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = true;
                    //Xac dinh o chi doc
                    if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                    {
                        //Cot duyet
                        if (_DuocSuaDuyet && _ChiDoc == false && okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    
                    else
                    {
                        //Cot tien
                        if (_DuocSuaChiTiet &&
                                _ChiDoc == false && 
                                _arrDSMaCot[j] != "rTyLe" &&
                                _arrDSMaCot[j] != "rTongVon_ChuSoHuu" &&
                                _arrDSMaCot[j] != "rCong_VonNhaNuoc" &&
                                _arrDSMaCot[j] != "rConPhaiNop_VonNhaNuocKhiCoPhanHoa" &&
                                _arrDSMaCot[j] != "rConPhaiNop_CoTuc" &&
                                _arrDSMaCot[j] != "rConPhaiNop_TienThueDat" &&
                                _arrDSMaCot[j] != "rTongSoConPhaiNop" &&
                                _arrDSMaCot[j] != "rTongSoDaNop" &&
                                okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    if (okOChiDoc)
                    {
                        _arrEdit[i].Add("");
                    }
                    else
                    {
                        _arrEdit[i].Add("1");
                    }
                }
            }
        }

        /// <summary>
        /// Hàm cập nhập mảng dữ liệu
        /// </summary>
        protected void CapNhap_arrDuLieu()
        {
            _arrDuLieu = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                _arrDuLieu.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    //Xac dinh gia tri
                    if (_arrDSMaCot[j].EndsWith("rTyLe"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 2]);
                        _arrDuLieu[i].Add(Convert.ToString(GT2 / GT1 * 100));
                    }
                    //else if (_arrDSMaCot[j].EndsWith("rTongVon_ChuSoHuu"))
                    //{
                    //    Double GT1 = Convert.ToDouble(_arrDuLieu[i][j + 1]);
                    //    Double GT2 = Convert.ToDouble(_arrDuLieu[i][j + 2]);
                    //    Double GT3 = Convert.ToDouble(_arrDuLieu[i][j + 3]);
                    //    Double GT4 = Convert.ToDouble(_arrDuLieu[i][j + 4]);
                    //    Double GT5 = Convert.ToDouble(_arrDuLieu[i][j + 5]);
                    //    Double GT6 = Convert.ToDouble(_arrDuLieu[i][j + 6]);
                    //    Double GT7 = Convert.ToDouble(_arrDuLieu[i][j + 7]);
                    //    _arrDuLieu[i].Add(Convert.ToString(GT1 + GT2 + GT3 + GT4 + GT5 + GT6 + GT7));
                    //}
                    else if (_arrDSMaCot[j].EndsWith("rCong_VonNhaNuoc"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 2]);
                        _arrDuLieu[i].Add(Convert.ToString(GT1 + GT2));
                    }
                    else if (_arrDSMaCot[j].EndsWith("rConPhaiNop_VonNhaNuocKhiCoPhanHoa"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 2]);
                        _arrDuLieu[i].Add(Convert.ToString(GT2 - GT1));
                    }
                    else if (_arrDSMaCot[j].EndsWith("rConPhaiNop_CoTuc"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 2]);
                        _arrDuLieu[i].Add(Convert.ToString(GT2 - GT1));
                    }
                    else if (_arrDSMaCot[j].EndsWith("rConPhaiNop_TienThueDat"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 2]);
                        _arrDuLieu[i].Add(Convert.ToString(GT2 - GT1));
                    }
                    else if (_arrDSMaCot[j].EndsWith("rTongSoConPhaiNop"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 9]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 5]);
                        Double GT3 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                        _arrDuLieu[i].Add(Convert.ToString(GT1 + GT2 + GT3));
                    }
                    else if (_arrDSMaCot[j].EndsWith("rTongSoDaNop"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 11]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 7]);
                        Double GT3 = Convert.ToDouble(_arrDuLieu[i][j - 3]);
                        _arrDuLieu[i].Add(Convert.ToString(GT1 + GT2 + GT3));
                    }
                    else
                    {
                        _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                    }
                }
            }
        }
    }
}