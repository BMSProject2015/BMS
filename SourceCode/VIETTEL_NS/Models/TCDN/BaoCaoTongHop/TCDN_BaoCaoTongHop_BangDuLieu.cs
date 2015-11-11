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
    public class TCDN_BaoCaoTongHop_BangDuLieu:BangDuLieu
    {
        private int iNamLamViec;
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaDoanhNghiep"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public TCDN_BaoCaoTongHop_BangDuLieu(String iNamLamViec, String iQuy, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            //this._iID_Ma = iQuy;
            this._MaND = MaND;
            this._IPSua = IPSua;
            this.iNamLamViec = Convert.ToInt32(iNamLamViec);
            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeThongKeTCDN, MaND);
            _dtChiTiet = TCDN_BaoCaoTongHopModels.Get_dtBaoCaoTongHop(iNamLamViec, iQuy, arrGiaTriTimKiem);
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
                String MaHang = Convert.ToString(R["iID_MaBaoCaoTongHop"]);
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
           
            String strDSTruongTieuDe = "Tên công ty";
            String strDSTruong = "sTenDoanhNghiep";
             


            String[] arrDSTruongTieuDeFix = strDSTruongTieuDe.Split(',');
            //Tiêu đề fix: Thêm trường sMaCongTrinh, sTenCongTrinh

                _arrDSMaCot.Add(strDSTruong);
                _arrTieuDe.Add(strDSTruongTieuDe);
                _arrWidth.Add(250);
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            

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

            String[] arrDSTruong = TCDN_BaoCaoTongHopModels.sDanhSachTruong.Split(',');
            String[] arrDSTruongTieuDe = TCDN_BaoCaoTongHopModels.sDanhSachTruongTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = TCDN_BaoCaoTongHopModels.sDanhSachTruongDoRong.Split(',');
            String[] arrDSTruongDoRong = TCDN_BaoCaoTongHopModels.sDanhSachTruongDoRong.Split(',');
            String TenCotNhom = "";
            int SoCotNhom = 1;
            //Tiêu đề tiền
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                
                if (j <= 5)
                {
                    TenCotNhom = "Năm " + Convert.ToString(iNamLamViec - 1);
                    SoCotNhom = 6;
                }
                else if (j > 5 && j <= 11)
                {
                    TenCotNhom = "Kế hoạch năm " + Convert.ToString(iNamLamViec);
                    SoCotNhom = 6;
                }
                else
                {
                    TenCotNhom = "Ước thực hiện 6 tháng năm " + Convert.ToString(iNamLamViec);
                    SoCotNhom = 5;
                }
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(SoCotNhom);
                _arrTieuDeNhomCot.Add(TenCotNhom);
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
                        _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));                   
                }
            }
        }
    }
}