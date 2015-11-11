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
    /// <summary>
    /// Lớp QuyetToan_ChungTu_BangDuLieu cho phần bảng của Quyết toán
    /// </summary>
    public class QuyetToan_QuyetToan_BaoHiem_BangDuLieu : BangDuLieu
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public QuyetToan_QuyetToan_BaoHiem_BangDuLieu(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            
            this._MaND = MaND;
            this._IPSua = IPSua;

            

            //Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND, iID_MaTrangThaiDuyet);
            //if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan, iID_MaTrangThaiDuyet))
            //{
            //    _ChiDoc = true;
            //}

            //if (ND_DuocSuaChungTu == false)
            //{
            //    _ChiDoc = true;
            //}

            //if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan, iID_MaTrangThaiDuyet) &&
            //    ND_DuocSuaChungTu)
            //{
            //    _CoCotDuyet = true;
            //    _DuocSuaDuyet = true;
            //}

            //if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(QuyetToanModels.iID_MaPhanHeQuyetToan, iID_MaTrangThaiDuyet))
            //{
            //    _CoCotDuyet = true;
            //}
            _ChiDoc = false;
            _CoCotDuyet = false;
            _DuocSuaChiTiet = true;//LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);

            _dtChiTiet = QuyetToan_QuyetToan_BaoHiemModels.Get_QuyetToanBaoHiem(iID_MaChungTu);
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
                //CapNhapHangTongCong();
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                //CapNhapDanhSachMaCot_Them();
                //CapNhap_arrLaHangCha();
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
                String MaHang = Convert.ToString(R["iID_MaQuyetToanBaoHiem"]);
                _arrDSMaHang.Add(MaHang);
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Cột Fixed của bảng gồm:
        ///         + Các trường của mục lục ngân sách
        ///         + Trường sMaCongTrinh, sTenCongTrinh của cột tiền
        ///     - Cập nhập số lượng cột Fixed
        /// </summary>
        protected void CapNhapDanhSachMaCot_Fixed()
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();

            //String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            //String[] arrDSTruongTieuDe = MucLucNganSachModels.strDSTruongTieuDe.Split(',');
            //String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            //String[] arrDSTruongTienTieuDe = MucLucNganSachModels.strDSTruongTienTieuDe.Split(',');
            //String[] arrDSTruongTienDoRong = MucLucNganSachModels.strDSTruongTienDoRong.Split(',');
            //String[] arrDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong.Split(',');

            ////Xác định các cột tiền sẽ hiển thị
            //_arrCotTienDuocHienThi = new Dictionary<String, Boolean>();
            //for (int j = 0; j < arrDSTruongTien.Length; j++)
            //{
            //    _arrCotTienDuocHienThi.Add(arrDSTruongTien[j], false);
            //    for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            //    {
            //        DataRow R = _dtChiTiet.Rows[i];
            //        if (Convert.ToBoolean(R["b" + arrDSTruongTien[j]]))
            //        {
            //            _arrCotTienDuocHienThi[arrDSTruongTien[j]] = true;
            //            break;
            //        }
            //    }
            //}

            ////Tiêu đề fix: Thêm trường sMaCongTrinh, sTenCongTrinh
            //for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            //{
            //    _arrDSMaCot.Add(arrDSTruong[j]);
            //    _arrTieuDe.Add(arrDSTruongTieuDe[j]);
            //    _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
            //    _arrHienThiCot.Add(true);
            //    _arrSoCotCungNhom.Add(1);
            //    _arrTieuDeNhomCot.Add("");
            //}

            //_nCotFixed = _arrDSMaCot.Count;
            _nCotFixed = 0;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường của cột tiền trừ sMaCongTrinh, sTenCongTrinh
        ///         + Trường sTongSo
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String strDSTruongTien = "sKyHieuDoiTuong,iSTT,sHoTen,rSoNguoi,rLuongThang,iSoNgay,rLuongCoBan,rThamNien,rPhuCapChucVu,rPhuCapKhac,rTongSo";
            String strDSTruongTienTieuDe = "Đối tượng,STT, Họ tên,Số người,Lương tháng,Số ngày,Lương CB,Thâm niên, PC Chức vụ,PC Khác,Tổng";
            String strDSTruongTienDoRong = "70,20,150,60,100,60,80,80,80,80,80";
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            String[] arrDSTruongTienTieuDe = strDSTruongTienTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = strDSTruongTienDoRong.Split(',');


            //Tiêu đề tiền
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
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
            String strDSTruong = "";
            String[] arrDSTruong = strDSTruong.Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruong[j]);
                _arrWidth.Add(0);
                _arrHienThiCot.Add(false);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
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
                _arrLaHangCha.Add(Convert.ToBoolean(R["bLaHangCha"]));
                int CSCha = -1;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Convert.ToString(R["iID_MaMucLucNganSach_Cha"]) == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach"]))
                    {
                        CSCha = j;
                        break;
                    }
                }
                _arrCSCha.Add(CSCha);
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
                    if (_arrDSMaCot[j] == "rTongSo")
                    {

                        okOChiDoc = true;
                        
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

        /// <summary>
        /// Hàm tính lại các ô tổng số và tổng cộng các hàng cha
        /// </summary>
        protected void CapNhapHangTongCong()
        {
            String strDSTruongTien = MucLucNganSachModels.strDSTruongTien_So;
            String strDSTruongTien_ChiTieu = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "_ChiTieu,")+"_ChiTieu";
            String strDSTruongTien_DaQuyetToan = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "_DaQuyetToan,")+ "_DaQuyetToan";
            String strDSTruongTien_ConLai = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "_ConLai,")+"_DaQuyetToan";
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            String[] arrDSTruongTien_ChiTieu = strDSTruongTien_ChiTieu.Split(',');
            String[] arrDSTruongTien_DaQuyetToan = strDSTruongTien_DaQuyetToan.Split(',');
            String[] arrDSTruongTien_ConLai = strDSTruongTien_ConLai.Split(',');

            int len = arrDSTruongTien.Length;
            //Tinh lai cot tong so
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                {
                    double S;
                    //rTongSo
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien[k].StartsWith("rChiTapTrung") == false)
                        {
                            S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[k]]);
                        }
                    }
                    if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[len - 1]]) != S)
                    {
                        _dtChiTiet.Rows[i][arrDSTruongTien[len - 1]] = S;
                    }
                    //rTongSo_ChiTieu
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien_ChiTieu[k].StartsWith("rChiTapTrung") == false)
                        {
                            S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[k]]);
                        }
                    }
                    if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[len - 1]]) != S)
                    {
                        _dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[len - 1]] = S;
                    }
                    //rTongSo_DaQuyetToan
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien_DaQuyetToan[k].StartsWith("rChiTapTrung") == false)
                        {
                            S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[k]]);
                        }
                    }
                    if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[len - 1]]) != S)
                    {
                        _dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[len - 1]] = S;
                    }

                    //rTongSo _ConLai
                    //S = 0;
                    //for (int k = 0; k < len - 1; k++)
                    //{
                    //    if (arrDSTruongTien_ConLai[k].StartsWith("rChiTapTrung") == false)
                    //    {
                    //        S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ConLai[k]]);
                    //    }
                    //}
                    //if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ConLai[len - 1]]) != S)
                    //{
                    //    _dtChiTiet.Rows[i][arrDSTruongTien_ConLai[len - 1]] = S;
                    //}
                }
            }
            //Tinh lai cac hang cha
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]))
                {
                    String iID_MaMucLucNganSach = Convert.ToString(_dtChiTiet.Rows[i]["iID_MaMucLucNganSach"]);
                    for (int k = 0; k < len; k++)
                    {
                        double S, S_ChiTieu,S_DaQuyetToan,S_ConLai;
                        //rTongSo
                        S = 0;
                        S_ChiTieu = 0;
                        S_DaQuyetToan = 0;
                        S_ConLai = 0;
                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iID_MaMucLucNganSach == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach_Cha"]))
                            {
                                S += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                                S_ChiTieu += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien_ChiTieu[k]]);
                                S_DaQuyetToan += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien_DaQuyetToan[k]]);
                              //  S_ConLai += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien_ConLai[k]]);
                            }
                        }
                        if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[k]]) != S ||
                            Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[k]]) != S_ChiTieu)
                        {
                            _dtChiTiet.Rows[i][arrDSTruongTien[k]] = S;
                            _dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[k]] = S_ChiTieu;
                            _dtChiTiet.Rows[i][arrDSTruongTien_DaQuyetToan[k]] = S_DaQuyetToan;
                           // _dtChiTiet.Rows[i][arrDSTruongTien_ConLai[k]] = S_ConLai;
                        }
                    }
                }
            }
        }

    }
}