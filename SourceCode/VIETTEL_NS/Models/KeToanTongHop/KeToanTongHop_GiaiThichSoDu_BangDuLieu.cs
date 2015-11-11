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
    public class KeToanTongHop_GiaiThichSoDu_BangDuLieu : BangDuLieu
    {
        private int _iThang, _iNam;
          /// /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public KeToanTongHop_GiaiThichSoDu_BangDuLieu(int iThang, int iNam,int iLoai, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iThang = iThang;
            this._iNam = iNam;
            this._MaND = MaND;
            this._IPSua = IPSua;
            this._ChiDoc = false;
            this._DuocSuaChiTiet = true;

            
            _dtChiTiet = KeToanTongHop_GiaiThichSoDuModels.Get_dtDanhSachGiaiThichSoDu(iThang, iNam, iLoai, arrGiaTriTimKiem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu(iLoai);
        }
               /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(int iLoai)
        {
            if (_arrDuLieu == null)
            {
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide(iLoai);
                CapNhap_arrCotDuocPhepNhap(iLoai);
                //CapNhapDanhSachMaCot_Them();
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
                String MaHang = Convert.ToString(R["iID_MaGiaiThich"]);
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
        protected void CapNhapDanhSachMaCot_Slide(int iLoai)
        {
            String[] arrDSTruong = "sNoiDung,sLyDo,rSoTien".Split(',');
            String[] arrDSTruongTieuDe = "Tên đơn vị-cá nhân,Lý do,Số tiền".Split(',');
            String[] arrDSTruongDoRong = "300,400,150".Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                if (iLoai == 4 && (arrDSTruong[j] == "sNoiDung" || arrDSTruong[j] == "rSoTien"))
                {
                    _arrHienThiCot.Add(false);
                }
                else
                {
                    _arrHienThiCot.Add(true);
                }
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
                if (String.IsNullOrEmpty(arrDSTruong[j]) == false)
                {
                    _arrDSMaCot.Add(arrDSTruong[j]);
                    _arrTieuDe.Add(arrDSTruong[j]);
                    _arrWidth.Add(0);
                }
            }
        }

        /// <summary>
        /// Hàm xác định các cột có được Edit hay không
        /// </summary>
        protected void CapNhap_arrCotDuocPhepNhap(int iLoai)
        {
            _arrCotDuocPhepNhap = new List<Boolean>();
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                Boolean okCotDuocPhepNhap = true;
                //Xac dinh o chi doc
                //if (_ChiDoc == false)
                //{
                //    if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                //    {
                //        //Cot duyet
                //        if (_DuocSuaDuyet == false)
                //        {
                //            okCotDuocPhepNhap = false;
                //        }
                //    }
                //    else
                //    {
                //        if (_DuocSuaChiTiet == false)
                //        {
                //            okCotDuocPhepNhap = false;
                //        }
                //    }
                //}
                if (iLoai == 4 && (_arrDSMaCot[j] == "sNoiDung" || _arrDSMaCot[j] == "rSoTien"))
                {
                    okCotDuocPhepNhap = false;
                }
                else
                {
                    okCotDuocPhepNhap = true;
                }
                _arrCotDuocPhepNhap.Add(okCotDuocPhepNhap);
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
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];              
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = false;                   
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