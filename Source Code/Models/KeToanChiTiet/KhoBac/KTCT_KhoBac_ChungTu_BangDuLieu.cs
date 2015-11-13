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
    public class KTCT_KhoBac_ChungTu_BangDuLieu : BangDuLieu
    {
        private int _iThang, _iNam, _iLoai, _iNamNganSach, _iNguonNganSach;

        /// /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public KTCT_KhoBac_ChungTu_BangDuLieu(int iThang, int iNam, int iNamNganSach, int iNguonNganSach, int iLoai, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iThang = iThang;
            this._iNam = iNam;
            this._iLoai = iLoai;
            this._iNamNganSach = iNamNganSach;
            this._iNguonNganSach = iNguonNganSach;

            this._MaND = MaND;
            this._IPSua = IPSua;
            this._ChiDoc = false;
            this._DuocSuaChiTiet = false;

            if (LuongCongViecModel.KiemTra_TroLyTongHop(MaND))
            {
                this._DuocSuaChiTiet = true;
            }

            _dtChiTiet = KTCT_KhoBac_ChungTuModels.List_DanhSachChungTu(_iThang, _iNam, iNamNganSach, _iNguonNganSach, iLoai, arrGiaTriTimKiem);
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
                String MaHang = Convert.ToString(R["iID_MaChungTu"]);
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

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        /// <param name="iLoai"></param>
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String strDSTruong = "sSTT,sSoChungTu,iNgay,sNoiDung,rTongSo,sTenTrangThaiDuyet,sNgayChungTu";
            String strDSTruongTieuDe = "STT,Số chứng từ,Ngày <span style=\"color: Red;\">*</span>,Nội dung,Tổng số,Trạng thái,Ngày tạo";
            String strDSTruongDoRong = "30,80,50,450,150,150,120";

            if (LuongCongViecModel.KiemTra_TroLyPhongBan(_MaND) == false)
            {
                strDSTruong = "bChon," + strDSTruong;
                strDSTruongTieuDe = "<input type=\"checkbox\" id=\"checkAll\" onclick=\"ChonTrinhDuyet(this.checked)\">," + strDSTruongTieuDe;
                strDSTruongDoRong = "30," + strDSTruongDoRong;
            }

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
            String strDSTruong = "sMauSac";
            String[] arrDSTruong = strDSTruong.Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                if (String.IsNullOrEmpty(arrDSTruong[j])==false)
                {
                    _arrDSMaCot.Add(arrDSTruong[j]);
                    _arrTieuDe.Add(arrDSTruong[j]);
                    _arrWidth.Add(0);
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
                Boolean okHangChiDoc = true;
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];

                int iID_MaTrangThaiDuyet = Convert.ToInt32(R["iID_MaTrangThaiDuyet"]);
                if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(KeToanTongHopModels.iID_MaPhanHe, this._MaND, iID_MaTrangThaiDuyet))
                {
                    okHangChiDoc = false;
                }
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = true;
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
                    if (_arrDSMaCot[j] == "bDongYChiTiet" || _arrDSMaCot[j] == "bChon")
                    {
                        _arrDuLieu[i].Add("0");
                    }
                    else if (_arrDSMaCot[j] == "sSTT")
                    {
                        _arrDuLieu[i].Add(Convert.ToString(i + 1));
                    }
                    else if (_arrDSMaCot[j] == "sTenTrangThaiDuyet")
                    {
                        int iID_MaTrangThaiDuyet = Convert.ToInt32(R["iID_MaTrangThaiDuyet"]);
                        String sTenTrangThaiDuyet = LuongCongViecModel.TrangThaiDuyet(iID_MaTrangThaiDuyet);
                        _arrDuLieu[i].Add(sTenTrangThaiDuyet);
                    }
                    else if (_arrDSMaCot[j] == "sMauSac")
                    {
                        int iID_MaTrangThaiDuyet = Convert.ToInt32(R["iID_MaTrangThaiDuyet"]);
                        String sMauSac = LuongCongViecModel.TrangThaiDuyet_MauSac(iID_MaTrangThaiDuyet);
                        _arrDuLieu[i].Add(sMauSac);
                    }
                    else if (_arrDSMaCot[j].StartsWith("d"))
                    {
                        // _arrDuLieu[i].Add(CommonFunction.ChuyenNgaySangXau(R[_arrDSMaCot[j]]));
                        _arrDuLieu[i].Add(Convert.ToDateTime(R[_arrDSMaCot[j]]).ToString("dd/MM/yyyy HH:mm:ss"));
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