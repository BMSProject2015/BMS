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
    public class Luong_DanhMucPhuCap_MucLucNganSach_BangDuLieu:BangDuLieu
    {
        public static String[] arrDSTruong = { "sMaTruong", "sTenNgachLuong", "sTenBacLuong", "sLNS", "sL", "sK", "sM", "sTM", "sTTM", "sNG", "sTNG", "sMoTa"};
        public static String[] arrDSTruongTieuDe = { "Trường", "Ngạch lương", "Bậc lương", "LNS", "L", "K", "M", "TM", "TTM", "NG", "TNG", "Nội dung"};
        public static String[] arrDSTruongDoRong = { "200", "200", "200", "60", "30", "30", "30", "30", "30", "30", "30", "200"};

                   /// /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaPhuCap"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public Luong_DanhMucPhuCap_MucLucNganSach_BangDuLieu(String iID_MaPhuCap, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iID_Ma = iID_MaPhuCap;
            this._MaND = MaND;
            this._IPSua = IPSua;            
            _dtBang = null;
            
            _ChiDoc = false;
            _DuocSuaChiTiet = true;
            _dtChiTiet = Luong_DanhMucPhuCapModels.Get_dtDanhMucPhuCap_MucLucNganSach(iID_MaPhuCap);            
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu(iID_MaPhuCap);
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(String iID_MaPhuCap)
        {
            if (_arrDuLieu == null)
            {
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide(iID_MaPhuCap);
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrType_Rieng();
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
                String MaHang = Convert.ToString(R["iID_MaDanhMucPhuCap_MucLucNganSach"]);
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
        protected void CapNhapDanhSachMaCot_Slide(String iID_MaPhuCap)
        {
            
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                if (String.IsNullOrEmpty(iID_MaPhuCap)==false && arrDSTruong[j]=="sMaTruong")
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
            String strDSTruong = "iID_MaNgachLuong,iID_MaBacLuong,iID_MaMucLucNganSach,sXauNoiMa";
            String[] arrDSTruong = strDSTruong.Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruong[j]);
                _arrWidth.Add(0);
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
                
                        //Cot tien
                    if (_DuocSuaChiTiet &&
                            _ChiDoc == false &&
                            okHangChiDoc == false)
                    {
                        okOChiDoc = false;
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
        /// Hàm cập nhập kiểu nhập cho các cột
        ///     - Cột có prefix 'b': kiểu '2' (checkbox)
        ///     - Cột có prefix 'r' hoặc 'i' (trừ 'iID'): kiểu '1' (textbox number)
        ///     - Ngược lại: kiểu '0' (textbox)
        /// </summary>
        protected void CapNhap_arrType_Rieng()
        {
            //Xac dinh kieu truong nhap du lieu
            _arrType = new List<string>();
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                if (_arrDSMaCot[j].StartsWith("b"))
                {
                    //Nhap Kieu checkbox
                    _arrType.Add("2");
                }
                else if (_arrDSMaCot[j].StartsWith("r") || (_arrDSMaCot[j].StartsWith("iID") == false && _arrDSMaCot[j].StartsWith("i")))
                {
                    //Nhap Kieu so
                    _arrType.Add("1");
                }
                else
                {
                    //Nhap Kieu autocomplete
                    _arrType.Add("3");
                }
                //else
                //{
                //    //Nhap kieu xau
                //    _arrType.Add("0");
                //}
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