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
    public class KeToan_RutDuToan_BangDuLieu : BangDuLieu
    {
        private int _iThang, _iNam;

        /// /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public KeToan_RutDuToan_BangDuLieu(String iID_MaDotRutDuToan,Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            
            this._MaND = MaND;
            this._IPSua = IPSua;
            this._ChiDoc = false;
            this._DuocSuaChiTiet = true;

           

            _dtChiTiet = KeToan_RutDuToanDotModels.Get_ChiTietDotRutDuToan(iID_MaDotRutDuToan);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu();
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu()
        {
            
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
                CapNhap_arrType_Rieng();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
            
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
                String MaHang = Convert.ToString(R["iID_MaRutDuToanChiTiet"]);
                _arrDSMaHang.Add(MaHang);
            }
        }

        /// <summary>
        /// Hàm thêm danh sách cột Fixed vào bảng
        ///     - Không có cột Fixed
        ///     - Cập nhập số lượng cột Fixed
        /// </summary>
        protected void CapNhapDanhSachMaCot_Fixed()
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();

            String strDSTruong = "sLNS,sL,sK,sM,sTM,sMoTa";
            String strDSTruongTieuDe = "LNS,L,K,M,TM,Nội dung";
            String strDSTruongDoRong = "60,60,60,60,60,250";

            String[] arrDSTruong = strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = strDSTruongTieuDe.Split(',');
            String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
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
        /// <param name="iLoai"></param>
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String strDSTruong = "rSoTien,sTenDonVi,sTenChuongTrinhMucTieu";
            String strDSTruongTieuDe = "Tổng số,Tên đơn vị,Tên chương trình - Mục tiêu";
            String strDSTruongDoRong = "150,200,300";

            String[] arrDSTruong = strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = strDSTruongTieuDe.Split(',');
            String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
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
            String strDSTruong = "iID_MaDonVi,iID_MaChuongTrinhMucTieu";
            String[] arrDSTruong = strDSTruong.Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                if (String.IsNullOrEmpty(arrDSTruong[j])==false)
                {
                 
                    _arrDSMaCot.Add(arrDSTruong[j]);
                    _arrTieuDe.Add(arrDSTruong[j]);
                    _arrWidth.Add(0);
                    _arrHienThiCot.Add(false);
                    _arrSoCotCungNhom.Add(1);
                    _arrTieuDeNhomCot.Add("");
                }
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
                    Boolean okOChiDoc = false;
                    //Xac dinh o chi doc
                    if (_arrDSMaCot[j] == "bDongYChiTiet" || _arrDSMaCot[j] == "bChon")
                    {
                        okOChiDoc = false;
                    }

                    if (okOChiDoc || okHangChiDoc)
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
                   if (_arrDSMaCot[j] == "sSTT")
                    {
                        _arrDuLieu[i].Add(Convert.ToString(i+1));
                    }                    
                    else if (_arrDSMaCot[j].StartsWith("d"))
                    {
                        _arrDuLieu[i].Add(CommonFunction.ChuyenNgaySangXau(R[_arrDSMaCot[j]]));
                    }
                    else
                    {
                        _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                    }
                }
            }
        }


        /// <summary>
        /// Hàm cập nhập kiểu nhập cho các cột để lấy AutoComplate
        ///     - Cột có prefix 'b': kiểu '2' (checkbox)
        ///     - Cột có prefix 'r' hoặc 'i' (trừ 'iID'): kiểu '1' (textbox number)
        ///     - Ngược lại: kiểu '0' (textbox)
        /// </summary>
        protected void CapNhap_arrType_Rieng()
        {
            String[] arrDSTruongAutocomplete = "sTenDonVi,sTenChuongTrinhMucTieu".Split(',');
            //Xac dinh kieu truong nhap du lieu
            _arrType = new List<string>();
            String KieuNhap;
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                KieuNhap = "0";
                if (_arrDSMaCot[j].StartsWith("b"))
                {
                    //Nhap Kieu checkbox
                    KieuNhap = "2";
                }
                else if (_arrDSMaCot[j].StartsWith("r") || (_arrDSMaCot[j].StartsWith("iID") == false && _arrDSMaCot[j].StartsWith("i")))
                {
                    //Nhap Kieu so
                    KieuNhap = "1";
                }
                else
                {
                    //Nhap kieu xau
                    for (int i = 0; i < arrDSTruongAutocomplete.Length; i++)
                    {
                        if (_arrDSMaCot[j] == arrDSTruongAutocomplete[i])
                        {
                            //Nhap Kieu autocomplete
                            KieuNhap = "3";
                            break;
                        }
                    }
                }
                _arrType.Add(KieuNhap);
            }
        }
        /// <summary>
        /// Hàm xác định hàng cha, con để bôi đậm nhạt và cấm thêm
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
        
    }
}