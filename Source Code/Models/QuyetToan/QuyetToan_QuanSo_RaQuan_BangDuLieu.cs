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
    public class QuyetToan_QuanSo_RaQuan_BangDuLieu:BangDuLieu
    {    /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public QuyetToan_QuanSo_RaQuan_BangDuLieu(String iThang, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            
            this._MaND = MaND;
            this._IPSua = IPSua;
            Object iNamLamViec = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM QTQS_QuyetToanRaQuan WHERE iThang=@iThang AND iNamLamViec=@iNamLamViec AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeQuyetToan);//không có trạng thái duyệt

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
            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);

            _dtChiTiet = QuyetToan_QuanSo_RaQuanModels.GetRaQuan(iThang,iNamLamViec.ToString());
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
                //CapNhapDanhSachMaCot_Them();
               // CapNhap_arrLaHangCha();
                CapNhap_arrEdit();
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
                //CapNhap_arrFormat();
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
                String MaHang = Convert.ToString(R["iID_MaQuanSoRaQuan"]);
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

            String[] arrDSTruong = "sTenDonVi".Split(',');
            String[] arrDSTruongTieuDe = "Tên đơn vị".Split(',');
            String[] arrDSTruongDoRong = "100".Split(',');

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
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String[] arrDSTruongTien = "rVeHuu,rXuatNgu,rPhucVien,rThoiViec,rTongSo".Split(',');
            String[] arrDSTruongTienTieuDe = "Hưu trí,Xuất ngũ,Phục viên,Thôi việc,Tổng cộng".Split(',');
            String[] arrDSTruongTienDoRong = "100,100,100,100,100".Split(',');

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
                    if (Convert.ToString(R["iID_MaMucLucQuanSo_Cha"]) == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucQuanSo"]))
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

        protected void CapNhap_arrFormat()
        {
        
            String[] arrDSTruongFormat = ("rVeHuu,rXuatNgu,rPhucVien,rThoiViec").Split(',');
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                for (int i = 0; i < arrDSTruongFormat.Length; i++)
                {
                    if (_arrDSMaCot[j] == arrDSTruongFormat[i])
                    {
                        _arrFormat[j] = "2";//Định dạng kiểu số thực
                    }
                }
            }
        }
    }
}