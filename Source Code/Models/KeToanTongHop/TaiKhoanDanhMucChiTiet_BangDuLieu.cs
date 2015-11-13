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
    public class TaiKhoanDanhMucChiTiet_BangDuLieu:BangDuLieu
    {
     
        /// /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaPhuCap"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public TaiKhoanDanhMucChiTiet_BangDuLieu(String iID_MaTaiKhoan, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
                   {
                       this._iID_Ma = iID_MaTaiKhoan;
                       this._MaND = MaND;
                       this._IPSua = IPSua;
                       _dtBang = null;

                       _ChiDoc = false;
                       _DuocSuaChiTiet = true;
                       //
                       String SQL = "";
                       SqlCommand cmd = new SqlCommand();
                       if (String.IsNullOrEmpty(iID_MaTaiKhoan) || iID_MaTaiKhoan == Guid.Empty.ToString())
                       {
                       SQL =
                           String.Format(
                               @"select iID_MaTaiKhoanDanhMucChiTiet,sKyHieu,sTen,ISNULL(rSoTienNgoaiTe,0) as rSoTienNgoaiTe,sTenNgoaiTe,ISNULL(iID_MaNgoaiTe,0) as iID_MaNgoaiTe, iID_MaTaiKhoanDanhMucChiTiet_Cha,bLaHangCha,sXauNoiMa from KT_TaiKhoanDanhMucChiTiet  WHERE iTrangThai = 1 order by skyhieu");
                       }
                       else
                       {

                           SQL = String.Format(@"SELECT ct.iID_MaTaiKhoanDanhMucChiTiet,ct.sKyHieu,ct.sTen,ISNULL(ct.rSoTienNgoaiTe,0) as rSoTienNgoaiTe,ct.sTenNgoaiTe,ISNULL(ct.iID_MaNgoaiTe,0) as iID_MaNgoaiTe, ct.iID_MaTaiKhoanDanhMucChiTiet_Cha,ct.bLaHangCha,ct.sXauNoiMa FROM KT_TaiKhoanDanhMucChiTiet ct WHERE ct.iTrangThai = 1 and  exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanGiaiThich tk where tk.iTrangThai=1 and tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet and iID_MaTaiKhoan=@iID_MaTaiKhoan) ORDER BY ct.sKyHieu");
                           cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                       }
                       cmd.CommandText = SQL;
                       DataTable dt = Connection.GetDataTable(cmd);
                       cmd.Dispose();
                       int ThuTu = 0;
                       _dtChiTiet = TaiKhoanDanhMucChiTietModels.getstring(dt, 0, ref ThuTu, false);
                       //_dtChiTiet = TaiKhoanDanhMucChiTietModels.Get_DanhSachTaiKhoanDanhMucChiTiet();            
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
                //String MaHang = String.Format("{0}_{1}_{2}", Convert.ToString(R["iID_MaTaiKhoanDanhMucChiTiet"]),
                //                              Convert.ToString(R["sKyHieu"]),
                //                              Convert.ToString(R["sTen"]));
                String MaHang = Convert.ToString(R["iID_MaTaiKhoanDanhMucChiTiet"]);
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
            //Khoi tao bien dinh dang
            _arrAlign = new List<string>();
            _arrFormat = new List<string>();
            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        /// <param name="iLoai"></param>
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String[] arrDSTruong = "sKyHieu,sTen,sXauNoiMa,rSoTienNgoaiTe,sTenNgoaiTe".Split(',');
            String[] arrDSTruongTieuDe = "Ký hiệu<span style=\"color: Red;\">*</span>,Nội dung giải thích<span style=\"color: Red;\">*</span>,Ký hiệu cha,Số tiền ngoại tệ,Loại ngoại tệ".Split(',');
            String[] arrDSTruongDoRong = "100,300,200,200,100".Split(',');
           // String DSTruongFormat = "2";
           // String[] arrDSTruongFormat = DSTruongFormat.Split(',');
            String[] arrDSTruongAlign = "left,left,left,right,left".Split(',');
            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));                
                _arrHienThiCot.Add(true);                
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
                _arrAlign.Add(arrDSTruongAlign[j]);
                //Dinh dang format tien la so thap phan
                if (arrDSTruong[j] == "rSoTienNgoaiTe")
                {
                    _arrFormat.Add("2");
                }

                else
                {
                    _arrFormat.Add("");
                }
               
            }
    

            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
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
                    if (Convert.ToString(R["iID_MaTaiKhoanDanhMucChiTiet_Cha"]) == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaTaiKhoanDanhMucChiTiet"]))
                    {
                        CSCha = j;
                        break;
                    }
                }
                _arrCSCha.Add(CSCha);
            }
        }
        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCot_Them()
        {
            //String strDSTruong ="";
            //String[] arrDSTruong = strDSTruong.Split(',');
            //for (int j = 0; j < arrDSTruong.Length; j++)
            //{
            //    if (arrDSTruong[j] != "")
            //    {
            //        _arrDSMaCot.Add(arrDSTruong[j]);
            //        _arrTieuDe.Add(arrDSTruong[j]);
            //        _arrWidth.Add(0);
            //    }
            //}
            String[] arrDSTruong = "iID_MaNgoaiTe".Split(',');
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
            String[] arrDSTruongAutocomplete = "sTenNgoaiTe".Split(',');
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