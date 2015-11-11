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
    /// Lớp DanhMucBaoHiem_BangDuLieu cho phần bảng của Danh mục bảo hiểm
    /// </summary>
    public class DanhMucBaoHiem_BangDuLieu:BangDuLieu
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public DanhMucBaoHiem_BangDuLieu(String iNamLamViec,String iThang, String MaND, String IPSua)
        {
            this._iID_Ma = null;
            this._MaND = MaND;
            this._IPSua = IPSua;

            _dtBang = null;

            _DuocSuaChiTiet = true;//LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);

            _dtChiTiet = DanhMucThuBaoHiemModels.Get_DTChiTietDanhMuc(iNamLamViec, iThang);
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
                CapNhap_arrType();
               // CapNhap_arrLaHangCha();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();             
                CapNhap_arrFormat_Rieng();
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
                String MaHang = Convert.ToString(R["iID_MaDanhMucThuBaoHiem"]);
                _arrDSMaHang.Add(MaHang);               
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Cột Fixed của bảng gồm:
        ///         + Các trường của mục lục ngân sách
        ///     - Cập nhập số lượng cột Fixed
        /// </summary>
        protected void CapNhapDanhSachMaCot_Fixed()
        {
            String[] arrDSTruong = DanhMucThuBaoHiemModels.strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = DanhMucThuBaoHiemModels.strDSTruongTieuDe.Split(',');
            String[] arrDSTruongTien = DanhMucThuBaoHiemModels.strDSTruongTien.Split(',');
            String[] arrDSTruongTienTieuDe = DanhMucThuBaoHiemModels.strDSTruongTienTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = DanhMucThuBaoHiemModels.strDSTruongTienDoRong.Split(',');
            String[] arrDSTruongDoRong = DanhMucThuBaoHiemModels.strDSTruongDoRong.Split(',');

            //Tiêu đề fix: Thêm trường sMaCongTrinh, sTenCongTrinh
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();
            _arrHienThiCot = new List<Boolean>();         
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường của cột tiền
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String[] arrDSTruongTien = DanhMucThuBaoHiemModels.strDSTruongTien.Split(',');
            String[] arrDSTruongTienTieuDe = DanhMucThuBaoHiemModels.strDSTruongTienTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = DanhMucThuBaoHiemModels.strDSTruongTienDoRong.Split(',');

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
            
            //Them cot duyet
            if (CoCotDuyet)
            {
                _arrDSMaCot.Add("bDongY");
                _arrDSMaCot.Add("sLyDo");
                if (_ChiDoc)
                {
                    _arrTieuDe.Add("<div class='check'></div>");
                }
                else
                {
                    _arrTieuDe.Add("<div class='check' onclick='BangDuLieu_CheckAll();'></div>");
                }
                _arrTieuDe.Add("Nhận xét");
                _arrWidth.Add(20);
                _arrWidth.Add(200);
            }
            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        }

        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCot_Them()
        {
            String[] arrDSTruongTien = "iID_MaMucLucDoiTuong,iID_MaMucLucDoiTuong_Cha,bLaHangCha".Split(',');

            //Tiêu đề tiền
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {

                _arrDSMaCot.Add(arrDSTruongTien[j]);
                _arrTieuDe.Add(arrDSTruongTien[j]);
                _arrWidth.Add(100);
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

                if (Convert.ToBoolean(R["bLaHangCha"]))
                {
                    okHangChiDoc = true;
                }

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
            CapNhap_arrThayDoi();            
            _arrDuLieu = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                _arrDuLieu.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];
                String MaHang = Convert.ToString(R["iID_MaDanhMucThuBaoHiem"]);
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    //Xac dinh gia tri
                    _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                    _arrThayDoi[i][j] = true;
                }
            }
        }
        protected void CapNhap_arrFormat_Rieng()
        {
            CapNhap_arrFormat();
            String[] arrDSTruongFormat = DanhMucThuBaoHiemModels.strDSTruongTien.Split(',');
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                for (int i = 0; i < arrDSTruongFormat.Length; i++)
                {
                    if (_arrDSMaCot[j] == arrDSTruongFormat[i])
                    {
                        _arrFormat[j] = "3";//Định dạng kiểu số thực 3 số sau dấu phẩy
                    }
                }
            }
        }

    }
}