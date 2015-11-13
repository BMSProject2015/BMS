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
    /// Lớp CapPhat_BangDuLieu cho phần bảng của Cấp phát
    /// </summary>
    public class ThuNop_BangDuLieu : BangDuLieu
    {
        public static string MaDonViChoCapPhat = "";
        private List<List<Double>> _arrGiaTriNhom = new List<List<Double>>();
        private List<int> _arrChiSoNhom = new List<int>();
        private List<String> _arrMaMucLucNganSach = new List<String>();
        private DataTable _dtDonVi = null;

        public String strDSChiSoNhom
        {
            get
            {
                String vR = "";
                for (int i = 0; i < _arrChiSoNhom.Count; i++)
                {
                    if (i > 0) vR += ",";
                    vR += _arrChiSoNhom[i];
                }
                return vR;
            }
        }

        public String strMaMucLucNganSach
        {
            get
            {
                String vR = "";
                for (int i = 0; i < _arrMaMucLucNganSach.Count; i++)
                {
                    if (i > 0) vR += ",";
                    vR += _arrMaMucLucNganSach[i];
                }
                return vR;
            }
        }

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaCapPhat"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public ThuNop_BangDuLieu(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND,
                                 String IPSua, int iLoai)
        {
            this._iID_Ma = iID_MaChungTu;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM TN_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();
            _dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(_MaND);
            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            Boolean ND_DuocSuaChungTu =
                LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeThuNopNganSach, MaND,
                                                            iID_MaTrangThaiDuyet);
            if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach,
                                                            iID_MaTrangThaiDuyet))
            {
                _ChiDoc = true;
            }

            if (ND_DuocSuaChungTu == false)
            {
                _ChiDoc = true;
            }

            if (
                LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanHeModels.iID_MaPhanHeThuNopNganSach,
                                                               iID_MaTrangThaiDuyet) &&
                ND_DuocSuaChungTu)
            {
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeThuNopNganSach, iID_MaTrangThaiDuyet))
            {
                _CoCotDuyet = true;
            }

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeThuNopNganSach, MaND);

            _dtChiTiet = ThuNop_ChungTuChiTietModels.Get_dtChungTuChiTiet(_iID_Ma, arrGiaTriTimKiem, "", MaND, iLoai);
           
            _dtChiTiet_Cu = _dtChiTiet.Copy();

            int d = 0;
            String DSTruongTien = "";
            if (iLoai==1)
            {
                DSTruongTien = MucLucNganSachModels.strDSTruongTien_ThuNop_So_Loai1;
               
            }
            else
            {
                DSTruongTien = MucLucNganSachModels.strDSTruongTien_ThuNop_So;
               
            }
            String[] arrDSTruongTien = DSTruongTien.Split(',');
            for (int i = 0; i < dtChiTiet.Rows.Count; i++)
            {
                if (i > 0)
                {
                    if (dtChiTiet.Rows[i - 1]["iID_MaMucLucNganSach"].ToString() !=
                        dtChiTiet.Rows[i]["iID_MaMucLucNganSach"].ToString())
                    {
                        d++;
                    }
                }

                _arrChiSoNhom.Add(d);
                //if (Convert.ToBoolean(dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                //{
                //    Boolean okKhongCoDuLieu = true;
                //    for (int j = 0; j < arrDSTruongTien.Length; j++)
                //    {
                //        if (Convert.ToDouble(dtChiTiet.Rows[i][arrDSTruongTien[j]]) != 0)
                //        {
                //            okKhongCoDuLieu = false;
                //            break;
                //        }
                //    }
                //    if (okKhongCoDuLieu)
                //    {
                //        dtChiTiet.Rows[i]["iID_MaDonVi"] = MaDonViChoCapPhat;
                //    }
                //}
            }

            DienDuLieu(iLoai);
        }

        /// <summary>
        /// Hàm hủy bỏ sẽ hủy dữ liệu của bảng _dtDonVi
        /// </summary>
        ~ThuNop_BangDuLieu()
        {
            if (_dtDonVi != null) _dtDonVi.Dispose();
        }


        /// <summary>
        /// Thuộc tính lấy danh sách mã đơn vị và tên đơn vị cho Javascript
        /// </summary>
        public String strDSDonVi
        {
            get
            {
                String _strDSDonVi = "";
                for (int csDonVi = 0; csDonVi < _dtDonVi.Rows.Count; csDonVi++)
                {
                    if (csDonVi > 0) _strDSDonVi += "##";
                    _strDSDonVi += String.Format("{0}##{1}", _dtDonVi.Rows[csDonVi]["iID_MaDonVi"], _dtDonVi.Rows[csDonVi]["sTen"]);
                }
                return _strDSDonVi;
            }
        }
        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(int iLoai)
        {
            if (_arrDuLieu == null)
            {
                CapNhapHangTongCong(iLoai);
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide(iLoai);
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
                CapNhap_arrEdit(iLoai);
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
                CapNhap_arrType_Rieng();
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
                if (Convert.ToBoolean(R["bLaHangCha"])==false)
                {
                    MaHang = String.Format("{0}_{1}", R["iID_MaChungTuChiTiet"], R["sNG"]);
                }
                 
                _arrDSMaHang.Add(MaHang);

                _arrMaMucLucNganSach.Add(Convert.ToString(R["iID_MaMucLucNganSach"]));
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
            //_arrAlign = new List<string>();
            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = MucLucNganSachModels.strDSTruongTieuDe.Split(',');
           // String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
          
            String[] arrDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong.Split(',');

            //Xác định các cột tiền sẽ hiển thị
            _arrCotTienDuocHienThi = new Dictionary<String, Boolean>();
          
            //Tiêu đề fix:
            for (int j = 1; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                if ( arrDSTruong[j] == "sL" || arrDSTruong[j] == "sK" || arrDSTruong[j] == "sM" || arrDSTruong[j] == "sTM" || arrDSTruong[j] == "sTTM" || arrDSTruong[j] == "sTNG")
                {
                    _arrHienThiCot.Add(false);
                }
                else
                {
                    _arrHienThiCot.Add(true);
                }
                //if (arrDSTruong[j] == "sLNS")
                //{
                //    _arrAlign.Add("left");
                //}
                //else
                //{
                //    //_arrAlign.Add("right");  
                //}
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường iID_MaDonVi
        ///         + Trường của cột tiền trừ sMaCongTrinh, sTenCongTrinh
        ///             - Cột phân bổ: Cột tổng phân bổ cho đơn vị
        ///             - Cột đã cấp: Cột đã cấp cho đơn vị trong năm ngân sách
        ///             - Cột còn lại: Cột còn lại chưa cấp cho đơn vị
        ///         + Trường sTongSo
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide(int iLoai)
        {
            if (iLoai == 1)
            {
               
                String[] arrDSTruongTien = ("iID_MaDonVi,sTenDonVi," + MucLucNganSachModels.strDSTruongTien_ThuNop_Loai1).Split(',');
                String[] arrDSTruongTienTieuDe =
                    "Mã<span style=\"color: Red;\">*</span>,Tên,Kế hoạch,Thu,Thoái thu,Tổng thu,Ghi chú".Split(',');
                String[] arrDSTruongTienDoRong = "40,150,100,100,100,100,250".Split(',');
                String[] arrDSNhom_TieuDe = "Đơn vị,Đơn vị,,Thu nộp,Thu nộp,Thu nộp,".Split(',');
                String[] arrDSNhom_SoCot = "2,2,1,3,3,3,1".Split(',');
                for (int j = 0; j < arrDSTruongTien.Length; j++)
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                    _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
                   
                }
            }
            else if (iLoai == 2)
            {
                String[] arrDSTruongTien = ("TN_sLNS,bThoaiThu,sSoCT,iID_MaDonVi,sTenDonVi," + MucLucNganSachModels.strDSTruongTien_ThuNop).Split(',');
                String[] arrDSTruongTienTieuDe =
                    ("LNS,TT,Số CT,Mã<span style=\"color: Red;\">*</span>,Tên," + MucLucNganSachModels.strDSTruongTien_ThuNopTieuDe).
                        Split(',');
                String[] arrDSTruongTienDoRong =
                    ("60,20,50,40,150," + MucLucNganSachModels.strDSTruongTien_ThuNopDoRong).Split(',');
                String[] arrDSNhom_TieuDe = ",,,Đơn vị,Đơn vị,,,,Chi phí,Chi phí,Chi phí,Chi phí,Chi phí,,,Thuế TNDN qua BQP,Thuế TNDN qua BQP,,,,,,".Split(',');
                String[] arrDSNhom_SoCot = "1,1,1,2,2,1,1,1,5,5,5,5,5,1,1,1,1,1,1,1,1,1,1".Split(',');
                for (int j = 0; j < arrDSTruongTien.Length; j++)
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    
                    _arrSoCotCungNhom.Add(Convert.ToInt32(arrDSNhom_SoCot[j]));
                    _arrTieuDeNhomCot.Add(arrDSNhom_TieuDe[j]);
                    // _arrAlign.Add(arrDSTruongAlign[j]);
                    if(arrDSTruongTien[j]=="rKeHoach")
                    {
                        _arrHienThiCot.Add(false);
                    }
                    else
                    {
                        _arrHienThiCot.Add(true);
                    }
                }
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
            //String strDSTruong = "sMaCongTrinh";
            //String[] arrDSTruong = strDSTruong.Split(',');
            //for (int j = 0; j < arrDSTruong.Length; j++)
            //{
            //    _arrDSMaCot.Add(arrDSTruong[j]);
            //    _arrTieuDe.Add(arrDSTruong[j]);
            //    _arrWidth.Add(0);
            //    _arrHienThiCot.Add(false);
            //    _arrSoCotCungNhom.Add(1);
            //    _arrTieuDeNhomCot.Add("");
            //}
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
        protected void CapNhap_arrEdit(int iLoai)
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
                    if (_arrDSMaCot[j] == "iID_MaDonVi")
                    {
                        //Cot don vi
                        if (_DuocSuaChiTiet && _ChiDoc == false && okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    else if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                    {
                        //Cot duyet
                        if (_DuocSuaDuyet && _ChiDoc == false && okHangChiDoc == false)
                        {
                            okOChiDoc = false;
                        }
                    }
                    else if (_arrDSMaCot[j] == "sTenDonVi" || _arrDSMaCot[j] == "rKeHoach" || _arrDSMaCot[j] == "rTongSo" || _arrDSMaCot[j] == "rChenhLech" || _arrDSMaCot[j] == "rSoChuaPhanPhoi" || _arrDSMaCot[j] == "")
                    {

                        okOChiDoc = true;
                    }
                    else if (_arrDSMaCot[j] == "rTongThu" && iLoai==1)
                    {
                        okOChiDoc = true;
                    }
                    else
                    {
                        // if (_DuocSuaDuyet && _ChiDoc == false && okHangChiDoc == false)
                        if (okHangChiDoc == false)
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
                    if (_arrDSMaCot[j].EndsWith("_ConLai"))
                    {
                        Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                        Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 2]);
                        Double GT3 = Convert.ToDouble(_arrDuLieu[i][j - 3]);
                        _arrDuLieu[i].Add(Convert.ToString(GT3 - GT2 - GT1));
                    }
                    else
                    {
                        _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                    }
                }
            }
        }

        /// <summary>
        /// Hàm tính lại các ô tổng số và tổng cộng các hàng cha
        /// </summary>
        protected void CapNhapHangTongCong(int iLoai)
        {
            String strDSTruongTien = "";
            if (iLoai == 1)
            {
                strDSTruongTien = MucLucNganSachModels.strDSTruongTien_ThuNop_So_Loai1;
            }
            else
            {
                strDSTruongTien = MucLucNganSachModels.strDSTruongTien_ThuNop_So;
            }
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
         

            int len = arrDSTruongTien.Length;
            //Tinh lai cot tong so
            //for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            //{
            //    if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
            //    {
            //        double S;
            //        //rTongSo


            //        //S = Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[1]]) - Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[2]]);

                    
            //        //if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[len - 1]]) != S)
            //        //{
            //        //    _dtChiTiet.Rows[i][arrDSTruongTien[len - 1]] = S;
            //        //}


            //    }
            //}
            //Tinh lai cac hang cha
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]))
                {
                    String iID_MaMucLucNganSach = Convert.ToString(_dtChiTiet.Rows[i]["iID_MaMucLucNganSach"]);
                    for (int k = 0; k < len; k++)
                    {
                        if (i == 2 && k > len-2)
                        {
                            i = 2;
                        }
                        double S;
                        //rTongSo
                        S = 0;
                       
                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iID_MaMucLucNganSach == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach_Cha"]))
                            {
                                S += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                             }
                        }
                        if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[k]]) != S )
                        {
                            _dtChiTiet.Rows[i][arrDSTruongTien[k]] = S;
                           
                        }
                    }
                }
            }
        }

        protected void CapNhap_arrType_Rieng()
        {
            String[] arrDSTruongAutocomplete = "TN_sLNS,iID_MaDonVi".Split(',');
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

    }
}