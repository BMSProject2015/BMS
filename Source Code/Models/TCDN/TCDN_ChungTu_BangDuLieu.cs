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
    public class TCDN_ChungTu_BangDuLieu : BangDuLieu
    {
        private int iQuy;
        private int NamLamViec;
        private Boolean DuocNhapCotDauKy = true;
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public TCDN_ChungTu_BangDuLieu(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua,String iLoai)
        {
            this._iID_Ma = iID_MaChungTu;
            this._MaND = MaND;
            this._IPSua = IPSua;
            
            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM TCDN_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            this.iQuy=Convert.ToInt16(_dtBang.Rows[0]["iQuy"]);
            this.NamLamViec=Convert.ToInt16(_dtBang.Rows[0]["iNamLamViec"]);
            cmd.Dispose();
            String iID_MaDoanhNghiep = Convert.ToString(_dtBang.Rows[0]["iID_MaDoanhNghiep"]);
            int Nam = NamLamViec;
            //int Quy = iQuy;
            //if (iQuy == 1)
            //{
            //    Nam = NamLamViec - 1;
            //    Quy = 4;
            //}
            //else
            //{
            //    Quy = iQuy - 1;
            //}
            //DataTable dtDauKy =TCDN_ChungTuChiTietModels.Get_dtQuyNam(Nam, Quy, iID_MaDoanhNghiep);
            //if (dtDauKy.Rows.Count > 0)
            //{
            //    DuocNhapCotDauKy = false;
            //}

            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeThongKeTCDN, MaND, iID_MaTrangThaiDuyet);
            if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(PhanHeModels.iID_MaPhanHeThongKeTCDN, iID_MaTrangThaiDuyet))
            {
                _ChiDoc = true;
            }

            if (ND_DuocSuaChungTu == false)
            {
                _ChiDoc = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanHeModels.iID_MaPhanHeThongKeTCDN, iID_MaTrangThaiDuyet) &&
                ND_DuocSuaChungTu)
            {
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeThongKeTCDN, iID_MaTrangThaiDuyet))
            {
                _CoCotDuyet = true;
            }

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeThongKeTCDN, MaND);

            _dtChiTiet = TCDN_ChungTuChiTietModels.Get_dtChungTuChiTiet(_iID_Ma, arrGiaTriTimKiem, iLoai);

            _dtChiTiet_Cu = _dtChiTiet.Copy();

            DienDuLieu(iQuy,iLoai);
           
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(int Quy,String iLoai)
        {
            if (_arrDuLieu == null)
            {
                CapNhapDanhSachMaHang();
                CapNhapHangTongCong();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide(Quy, iLoai);
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
                CapNhap_arrEdit(Quy, iLoai);
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
                CapNhap_arrType_Rieng(iLoai);
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
                String MaHang = "";

                MaHang = String.Format("{0}_{1}", R["iID_MaChungTuChiTiet"], R["sKyHieu"]);
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
            _arrFormat = new List<string>();
           
            String strDSTruongTieuDe = "Ký hiệu,Chỉ tiêu";
            String strDSTruong = "sKyHieu,sTen";
            String strDSTruongDoRong = "50,250"; 

            String[] arrDSTruong = strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = strDSTruongTieuDe.Split(',');
           
            String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');

            //Tiêu đề fix: Thêm trường sMaCongTrinh, sTenCongTrinh
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
        ///         + Trường sTongSo
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide(int iQuy,String iLoai)
        {

            String strDSTruongTienTieuDe = "Năm trước,Kế hoạch, Đã thực hiện,Thực hiện, NN/NT,TH/KH";
            String strDSTruongTien = "rNamTruoc,rKeHoach,rDaThucHien,rThucHien,rNTNN,rTHKH";
            String strDSTruongTienDoRong = "120,120,120,120,120,120";

            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            String[] arrDSTruongTienTieuDe = strDSTruongTienTieuDe.Split(',');
            String[] arrDSTruongTienDoRong = strDSTruongTienDoRong.Split(',');
            String[] arrDSNhom_TieuDe =
               ",Năm nay,Năm nay,Năm nay,So sánh (%),So sánh (%)".Split(',');
            String[] arrDSNhom_SoCot = "1,3,3,3,2,2".Split(',');
            if(iQuy==1)
            {
                 strDSTruongTienTieuDe = "Năm trước,Kế hoạch, Đã thực hiện,Thực hiện, NN/NT,TH/KH";
                 strDSTruongTien = "rNamTruoc,rKeHoach,rDaThucHien,rThucHien,rNTNN,rTHKH";
                 strDSTruongTienDoRong = "120,120,120,120,120,120";

                arrDSTruongTien = strDSTruongTien.Split(',');
                arrDSTruongTienTieuDe = strDSTruongTienTieuDe.Split(',');
                arrDSTruongTienDoRong = strDSTruongTienDoRong.Split(',');
                 arrDSNhom_TieuDe =
                   ",Năm nay,Năm nay,So sánh (%),So sánh (%),So sánh (%)".Split(',');
                 arrDSNhom_SoCot = "1,2,1,3,3,3".Split(',');
            }
            //Loai 4
            if(iLoai=="4")
            {
                strDSTruongTienTieuDe = "Năm trước,Năm nay";
                strDSTruongTien = "sNamTruoc_4,sThucHien_4";
                strDSTruongTienDoRong = "120,120";

                arrDSTruongTien = strDSTruongTien.Split(',');
                arrDSTruongTienTieuDe = strDSTruongTienTieuDe.Split(',');
                arrDSTruongTienDoRong = strDSTruongTienDoRong.Split(',');
                arrDSNhom_TieuDe =
                  ",,".Split(',');
                arrDSNhom_SoCot = "1,1".Split(',');
            }
            //Tiêu đề tiền
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruongTien[j]);
                
                if(iQuy==1)
                {
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    if(arrDSTruongTien[j]=="rDaThucHien")
                    {
                        _arrSoCotCungNhom.Add(1);
                        _arrTieuDeNhomCot.Add("");
                        _arrHienThiCot.Add(false);
                    }
                    else
                    {
                        _arrHienThiCot.Add(true);
                        
                        _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                        _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
                    }
                }
                else
                {
                    _arrHienThiCot.Add(true);
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                    _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
                }
               
               
                //_arrFormat.Add("1");
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
                _arrLaHangCha.Add(Convert.ToBoolean(R["bLaHangCha"]));
                int CSCha = -1;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Convert.ToString(R["iID_MaChiTieu_Cha"]) == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaChiTieu"]) && Convert.ToBoolean(_dtChiTiet.Rows[j]["bLaTong"])==true)
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
        protected void CapNhap_arrEdit(int iQuy,String iLoai)
        {
            _arrEdit = new List<List<string>>();
            for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
            {
                Boolean okHangChiDoc = false;
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];

                //if (Convert.ToBoolean(R["bLaHangCha"]))
                //{
                //    okHangChiDoc = true;
                //}
                if (Convert.ToBoolean(R["bLaTong"]))
                {
                    okHangChiDoc = true;
                }
                if (Convert.ToBoolean(R["bLaText"]))
                {
                    okHangChiDoc = true;
                }
                if(iLoai=="4")
                {
                    if(!String.IsNullOrEmpty(Convert.ToString(R["sCongThuc"])))
                    {
                        okHangChiDoc = true;
                    }
                }
                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = true;
                    //Xac dinh o chi doc
                    if (iQuy == 1)
                    {
                        if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo" || _arrDSMaCot[j] == "rNamTruoc" ||
                            _arrDSMaCot[j] == "rNTNN" || _arrDSMaCot[j] == "rTHKH" || _arrDSMaCot[j] == "rDaThucHien" )
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
                    }
                    else
                    {
                        if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo" || _arrDSMaCot[j] == "rNamTruoc" ||
                            _arrDSMaCot[j] == "rNTNN" || _arrDSMaCot[j] == "rTHKH" || _arrDSMaCot[j] == "rDaThucHien" || _arrDSMaCot[j] == "rKeHoach")
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
                    }

                    //if (_arrDSMaCot[j] == "rSoDauNam" && DuocNhapCotDauKy == false)
                    //{
                    //    okOChiDoc = true;
                    //}

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
        /// Hàm cập nhập kiểu nhập cho các cột
        ///     - Cột có prefix 'b': kiểu '2' (checkbox)
        ///     - Cột có prefix 'r' hoặc 'i' (trừ 'iID'): kiểu '1' (textbox number)
        ///     - Ngược lại: kiểu '0' (textbox)
        /// </summary>
        protected void CapNhap_arrType_Rieng(String iLoai)
        {
            String[] arrDSTruongAutocomplete = "".Split(',');
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
                if (iLoai == "4")
                {
                    KieuNhap = "0";
                }
                _arrType.Add(KieuNhap);
            }
        }
        /// <summary>
        /// Hàm tính lại các ô tổng số và tổng cộng các hàng cha
        /// </summary>
        protected void CapNhapHangTongCong()
        {
            String strDSTruongTien = "rNamTruoc,rKeHoach,rDaThucHien,rThucHien";
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            int len = arrDSTruongTien.Length;
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaTong"]))
                {
                    //TInh tong cac truong tien
                    String iiD_MaChiTieu = Convert.ToString(_dtChiTiet.Rows[i]["iiD_MaChiTieu"]);
                    for (int k = 0; k < len; k++)
                    {
                        double S;
                        S = 0;
                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iiD_MaChiTieu == Convert.ToString(_dtChiTiet.Rows[j]["iiD_MaChiTieu_Cha"]))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[j][arrDSTruongTien[k]])))
                                {
                                    S += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                                }
                            }
                        }
                        _dtChiTiet.Rows[i][arrDSTruongTien[k]] = S;
                    }
                    //Tinh 2 cot ty le
                    if (Convert.ToDecimal(_dtChiTiet.Rows[i]["rNamTruoc"]) != 0)
                    _dtChiTiet.Rows[i]["rNTNN"] = (Math.Round((Convert.ToDecimal(_dtChiTiet.Rows[i]["rThucHien"]) + Convert.ToDecimal(_dtChiTiet.Rows[i]["rDaThucHien"])) /
                                          Convert.ToDecimal(_dtChiTiet.Rows[i]["rNamTruoc"]) * 100, 0));
                    if (Convert.ToDecimal(_dtChiTiet.Rows[i]["rKeHoach"]) != 0)
                    _dtChiTiet.Rows[i]["rTHKH"] = (Math.Round((Convert.ToDecimal(_dtChiTiet.Rows[i]["rThucHien"]) + Convert.ToDecimal(_dtChiTiet.Rows[i]["rDaThucHien"])) /
                                        Convert.ToDecimal(_dtChiTiet.Rows[i]["rKeHoach"]) * 100, 0));
                }
            }
          
        }
    }
}