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
    /// Lớp KTCT_KhoBac_ChungTuChiTiet_BangDuLieu cho phần bảng của Kế toán chi tiết Kho bạc
    /// </summary>
    public class KTCT_KhoBac_ChungTuChiTiet_BangDuLieu : BangDuLieu
    {
        public String strDSMaND = "";
        private int _iLoai;
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public KTCT_KhoBac_ChungTuChiTiet_BangDuLieu(String iID_MaChungTu, int iLoai, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iID_Ma = iID_MaChungTu;
            this._iLoai = iLoai;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM KTKB_ChungTu WHERE iID_MaChungTu=@iID_MaChungTu AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int iID_MaTrangThaiDuyet = -1;
            if (_dtBang.Rows.Count > 0)
            {
                _iLoai = Convert.ToInt32(_dtBang.Rows[0]["iLoai"]);
                iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);
                Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND, iID_MaTrangThaiDuyet);
                if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                {
                    _ChiDoc = true;
                }

                if (ND_DuocSuaChungTu == false)
                {
                    _ChiDoc = true;
                }

                if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND))
                {
                    if (LuongCongViecModel.KiemTra_TrangThaiKhoiTao(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) ||
                        LuongCongViecModel.KiemTra_TrangThaiTuChoi(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                    {
                        _ChiDoc = false;
                    }
                }

                if ((LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) ||
                      LuongCongViecModel.KiemTra_TrangThaiKhoiTao(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet)
                      || LuongCongViecModel.KiemTra_TrangThaiTuChoi(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet)
                      ) &&
                      ND_DuocSuaChungTu)
                {
                    _DuocSuaDuyet = true;
                }
                _CoCotDuyet = true;
            }
            else
            {
                _ChiDoc = false;
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
                _DuocSuaChiTiet = true;
            }

            if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND))
            {
                _DuocSuaDuyet = false;
            }

            //_DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND);
            if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND) || LuongCongViecModel.KiemTra_TroLyTongHop(MaND))
            {
                //_DuocSuaChiTiet = true;
                if (iID_MaTrangThaiDuyet >= 0)
                {
                    if (LuongCongViecModel.KiemTra_TrangThaiKhoiTao(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) ||
                        LuongCongViecModel.KiemTra_TrangThaiTuChoi(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                    {
                        _DuocSuaChiTiet = true;
                    }
                }
            }


            if (_DuocSuaChiTiet == false && iID_MaTrangThaiDuyet >= 0)
            {
                if (LuongCongViecModel.KiemTra_TroLyTongHop(MaND))
                {
                    if (_ChiDoc == false ||
                        LuongCongViecModel.KiemTra_TrangThaiKhoiTao(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet) ||
                        LuongCongViecModel.KiemTra_TrangThaiTuChoi(KeToanTongHopModels.iID_MaPhanHe, iID_MaTrangThaiDuyet))
                    {
                        _DuocSuaChiTiet = true;
                        _ChiDoc = false;
                    }
                }
            }
            _dtChiTiet = KTCT_KhoBac_ChungTuChiTietModels.Get_dtChungTuChiTiet(_iID_Ma, arrGiaTriTimKiem, _MaND);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            DienDuLieu(_iLoai);
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
                CapNhapDanhSachMaCot_Them(iLoai);
                CapNhap_arrCotDuocPhepNhap();
                CapNhap_arrType_Rieng(iLoai);
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
                String MaHang = Convert.ToString(R["iID_MaChungTuChiTiet"]);
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
            String[] arrDSTruong = null;
            String[] arrDSTruongTieuDe = null;
            String[] arrDSTruongDoRong = null;

            switch (iLoai) { 
                //case 1:
                //    //Rút dự toán
                //    arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sCapNganSach,sLoaiST,sThuChi,sTenDonVi_Tra,sTenDonVi_Nhan,sTenNhanVien_Nhan,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,rDTRut,sNoiDung,sGhiChu,sTenChuongTrinhMucTieu_Tra,sTenChuongTrinhMucTieu_Nhan".Split(',');
                //    arrDSTruongTieuDe = "Ngày,Tháng,Số ChTừ,Cấp NS,Loại S/T,Th.Chi,Đơn vị trả,Đơn vị nhận,Người nhận,LNS,L,K,M,TM,TTM,NG,TNG,Tên MLNS,Số tiền rút,Nội dung thanh toán - chuyển tiền,Ghi chú,CT - MT Cấp,CT-MT Nhận".Split(',');
                //    arrDSTruongDoRong = "40,40,60,40,50,40,150,150,150,70,70,70,70,70,70,70,70,150,100,350,350,250,250".Split(',');
                //    break;
                //case 2:
                //    //Nhập số duyệt tạm ứng
                //    arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sCapNganSach,sLoaiST,sTenDonVi_Nhan,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,rSoTien,sNoiDung,sGhiChu".Split(',');
                //    arrDSTruongTieuDe = "Ngày,Tháng,Số ChTừ,Cấp NS,Loại S/T,Đơn vị,LNS,L,K,M,TM,TTM,NG,TNG,Tên MLNS,Số tiền duyệt,Nội dung,Ghi chú".Split(',');
                //    arrDSTruongDoRong = "60,60,80,80,80,150,70,70,70,70,70,70,70,70,150,100,350,350".Split(',');
                //    break;
                //case 3:
                //    //Khôi phục dự toán
                //    arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sCapNganSach,sLoaiST,sTenDonVi_Nhan,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,rDTKhoiPhuc,sNoiDung,sGhiChu".Split(',');
                //    arrDSTruongTieuDe = "Ngày,Tháng,Số ChTừ,Cấp NS,Loại S/T,Đơn vị,LNS,L,K,M,TM,TTM,NG,TNG,Tên MLNS,Số tiền khôi phục,Nội dung,Ghi chú".Split(',');
                //    arrDSTruongDoRong = "60,60,80,80,80,150,70,70,70,70,70,70,70,70,150,100,350,350".Split(',');
                //    break;
                //case 4:
                //    //Hủy dự toán
                //    arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sCapNganSach,sLoaiST,sTenDonVi_Nhan,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa,rDTHuy,sNoiDung,sGhiChu".Split(',');
                //    arrDSTruongTieuDe = "Ngày,Tháng,Số ChTừ,Cấp NS,Loại S/T,Đơn vị,LNS,L,K,M,TM,TTM,NG,TNG,Tên MLNS,Số tiền hủy,Nội dung,Ghi chú".Split(',');
                //    arrDSTruongDoRong = "60,60,80,80,80,150,70,70,70,70,70,70,70,70,150,100,350,350".Split(',');
                //    break;

                case 1:
                    //Rút dự toán
                    arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sTenNhanVien_Nhan,sSoChungTuCapThu,sTenDonVi_Tra,sTenDonVi_Nhan,sTenTaiKhoan_Co,sTenTaiKhoan_No,sLoaiST,sThuChi,sLNS,sL,sK,sM,sTM,sMoTa,rTongCap,rTongThu,rDTRut,sNoiDung,sGhiChu,sTenChuongTrinhMucTieu_Tra,sTenChuongTrinhMucTieu_Nhan".Split(',');
                    arrDSTruongTieuDe = "Ngày <span style=\"color: Red;\">*</span>,Tháng <span style=\"color: Red;\">*</span>,Số ChTừ,Người nhận,Số Duyệt DT,Đơn vị trả,Đơn vị nhận,Tài khoản có, Tài khoản nợ,Loại S/T<span style=\"color: Red;\">*</span>,Th.Chi<span style=\"color: Red;\">*</span>,LNS,L,K,M,TM,Tên MLNS,Tổng cấp,Tổng thu,Số tiền rút <span style=\"color: Red;\">*</span>,Nội dung thanh toán - chuyển tiền,Ghi chú,CT - MT Cấp,CT-MT Nhận".Split(',');
                    arrDSTruongDoRong = "40,40,60,150,150,150,150,200,200,50,40,70,70,70,70,70,250,100,100,150,350,350,250,250".Split(',');
                    break;
                case 2:
                    //Nhập số duyệt tạm ứng
                    arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sLoaiST,sTenDonVi_Nhan,sLNS,sL,sK,sM,sTM,sMoTa,rSoTien,sNoiDung,sGhiChu".Split(',');
                    arrDSTruongTieuDe = "Ngày <span style=\"color: Red;\">*</span>,Tháng <span style=\"color: Red;\">*</span>,Số ChTừ,Loại S/T,Đơn vị,LNS,L,K,M,TM,Tên MLNS,Số tiền duyệt <span style=\"color: Red;\">*</span>,Nội dung,Ghi chú".Split(',');
                    arrDSTruongDoRong = "60,60,80,80,150,70,70,70,70,70,250,100,350,350".Split(',');
                    break;
                case 3:
                    //Khôi phục dự toán
                    arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sLoaiST,sTenDonVi_Nhan,sLNS,sL,sK,sM,sTM,sMoTa,rDTKhoiPhuc,sNoiDung,sGhiChu".Split(',');
                    arrDSTruongTieuDe = "Ngày <span style=\"color: Red;\">*</span>,Tháng <span style=\"color: Red;\">*</span>,Số ChTừ,Loại S/T,Đơn vị,LNS,L,K,M,TM,Tên MLNS,Số tiền khôi phục <span style=\"color: Red;\">*</span>,Nội dung,Ghi chú".Split(',');
                    arrDSTruongDoRong = "60,60,80,80,150,70,70,70,70,70,250,100,350,350".Split(',');
                    break;
                case 4:
                    //Hủy dự toán
                    arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sLoaiST,sTenDonVi_Nhan,sLNS,sL,sK,sM,sTM,sMoTa,rDTHuy,sNoiDung,sGhiChu".Split(',');
                    arrDSTruongTieuDe = "Ngày <span style=\"color: Red;\">(*)</span>,Tháng <span style=\"color: Red;\">(*)</span>,Số ChTừ,Loại S/T,Đơn vị,LNS,L,K,M,TM,Tên MLNS,Số tiền hủy <span style=\"color: Red;\">(*)</span>,Nội dung,Ghi chú".Split(',');
                    arrDSTruongDoRong = "60,60,80,80,150,70,70,70,70,70,250,100,350,350".Split(',');
                    break;
                case 5:
                    //Rút ngoai te
                    arrDSTruong = "iNgay,iThang,sSoChungTuChiTiet,sTenNhanVien_Nhan,sSoChungTuCapThu,sTenDonVi_Tra,sTenDonVi_Nhan,sTenTaiKhoan_Co,sTenTaiKhoan_No,sLoaiST,sThuChi,sLNS,sL,sK,sM,sTM,sMoTa,iTinhChat,sKyHieuNgoaiTe,rTyGia,rNguyenTe,rDTRut,sNoiDung,sGhiChu,sTenChuongTrinhMucTieu_Tra,sTenChuongTrinhMucTieu_Nhan".Split(',');
                    arrDSTruongTieuDe = "Ngày <span style=\"color: Red;\">*</span>,Tháng <span style=\"color: Red;\">*</span>,Số ChTừ,Người nhận,Số Duyệt DT,Đơn vị trả,Đơn vị nhận,Tài khoản có, Tài khoản nợ,Loại S/T<span style=\"color: Red;\">*</span>,Th.Chi<span style=\"color: Red;\">*</span>,LNS,L,K,M,TM,Tên MLNS,Tính chất,Ký hiệu ngoại tệ,Tỷ giá,Nguyên tệ,Số tiền rút <span style=\"color: Red;\">*</span>,Nội dung thanh toán - chuyển tiền,Ghi chú,CT - MT Cấp,CT-MT Nhận".Split(',');
                    arrDSTruongDoRong = "40,40,60,150,150,150,150,200,200,50,40,70,70,70,70,70,250,100,100,150,150,150,350,350,250,250".Split(',');
                    break;
            }

            for (int j = 0; j < arrDSTruong.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
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
        protected void CapNhapDanhSachMaCot_Them(int iLoai)
        {
            String strDSTruong = "";
            switch (iLoai)
            {
                case 1:
                    //Rút dự toán
                    strDSTruong = "iID_MaMucLucNganSach,sXauNoiMa,iID_MaDonVi_Tra,iID_MaDonVi_Nhan,iID_MaChuongTrinhMucTieu_Tra,iID_MaChuongTrinhMucTieu_Nhan,iID_MaNhanVien_Nhan,iID_MaChungTu_Duyet,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No";
                    break;
                case 2:
                    //Nhập số duyệt tạm ứng
                    strDSTruong = "iID_MaMucLucNganSach,sXauNoiMa,iID_MaDonVi_Nhan";
                    break;
                case 3:
                    //Khôi phục dự toán
                    strDSTruong = "iID_MaMucLucNganSach,sXauNoiMa,iID_MaDonVi_Nhan";
                    break;
                case 4:
                    //Hủy dự toán
                    strDSTruong = "iID_MaMucLucNganSach,sXauNoiMa,iID_MaDonVi_Nhan";
                    break;
                case 5:
                    //Rút ngoai te
                    strDSTruong = "iID_MaMucLucNganSach,sXauNoiMa,iID_MaDonVi_Tra,iID_MaDonVi_Nhan,iID_MaChuongTrinhMucTieu_Tra,iID_MaChuongTrinhMucTieu_Nhan,iID_MaNhanVien_Nhan,iID_MaChungTu_Duyet,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No,iID_MaNgoaiTe";
                    break;
            }
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
        /// Hàm xác định các cột có được Edit hay không
        /// </summary>
        protected void CapNhap_arrCotDuocPhepNhap()
        {
            _arrCotDuocPhepNhap = new List<Boolean>();
            for (int j = 0; j < _arrDSMaCot.Count; j++)
            {
                Boolean okCotDuocPhepNhap = true;
                //Xac dinh o chi doc
                if (_ChiDoc == false)
                {
                    if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
                    {
                        //Cot duyet
                        if (_DuocSuaDuyet == false)
                        {
                            okCotDuocPhepNhap = false;
                        }
                    }
                    else if (_arrDSMaCot[j] == "sCapNganSach")
                    {
                        okCotDuocPhepNhap = false;
                    }
                    else
                    {
                        if (_DuocSuaChiTiet == false)
                        {
                            okCotDuocPhepNhap = false;
                        }
                    }
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
                Boolean okHangChiDoc = false;
                _arrEdit.Add(new List<string>());
                DataRow R = _dtChiTiet.Rows[i];

                for (int j = 0; j < _arrDSMaCot.Count; j++)
                {
                    Boolean okOChiDoc = true;
                    //Xac dinh o chi doc
                    if (_arrDSMaCot[j] == "rTongCap" ||
                        _arrDSMaCot[j] == "rTongThu" || _arrDSMaCot[j] == "sCapNganSach")
                    {
                        okOChiDoc = true;
                    }
                    else if (_arrDSMaCot[j] == "bDongY" || _arrDSMaCot[j] == "sLyDo")
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
                            //Phuonglt yêu cầu tài khoản cấp trợ lý tổng hợp thì được sửa tất cả dữ liệu của tài khoản cấp trợ lý phòng ban.
                            // Tài khoản cấp trợ lý phòng ban thì dữ liệu của tài khoản nào sửa dữ liệu của tài khoản đó.
                            if (Convert.ToString(R["sID_MaNguoiDungTao"]) == _MaND || LuongCongViecModel.KiemTra_TroLyTongHop(_MaND))
                            {
                                okOChiDoc = false;
                            }
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
        /// Hàm cập nhập kiểu nhập cho các cột
        ///     - Cột có prefix 'b': kiểu '2' (checkbox)
        ///     - Cột có prefix 'r' hoặc 'i' (trừ 'iID'): kiểu '1' (textbox number)
        ///     - Ngược lại: kiểu '0' (textbox)
        /// </summary>
        protected void CapNhap_arrType_Rieng(int iLoai)
        {
            String sDSTruongAuTo = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sTenDonVi_Tra,sTenDonVi_Nhan,sTenChuongTrinhMucTieu_Tra,sTenChuongTrinhMucTieu_Nhan,sSoChungTuCapThu,sLoaiST,sThuChi,sTenTaiKhoan_Co,sTenTaiKhoan_No";
            if (iLoai == 1) sDSTruongAuTo = sDSTruongAuTo + ",sTenNhanVien_Nhan,sSoChungTuCapThu,sLoaiST,sThuChi";
            if (iLoai == 5) sDSTruongAuTo = sDSTruongAuTo + ",sTenNhanVien_Nhan,sSoChungTuCapThu,sKyHieuNgoaiTe,iTinhChat,sLoaiST,sThuChi";
            String[] arrDSTruongAutocomplete = sDSTruongAuTo.Split(',');

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
                    if (_arrDSMaCot[j].StartsWith("iTinhChat"))
                    {
                        KieuNhap = "3"; 
                    }
                    else
                    {
                        //Nhap Kieu so
                        KieuNhap = "1";  
                    }
                  
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
                    if (_arrDSMaCot[j] == "sMauSac")
                    {
                        String sMauSac = "";
                        String sLyDo = Convert.ToString(R["sLyDo"]);
                        Boolean bDongY = Convert.ToBoolean(R["bDongY"]);
                        if (bDongY)
                        {
                            sMauSac = sMauSac_DongY;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(sLyDo) == false)
                            {
                                sMauSac = sMauSac_TuChoi;
                            }
                        }
                        _arrDuLieu[i].Add(sMauSac);
                    }
                    else
                    {
                        _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                    }
                }
                if (strDSMaND.IndexOf(String.Format("#|{0}##", R["sID_MaNguoiDungTao"])) == -1)
                {
                    strDSMaND += String.Format("#|{0}##{0}", R["sID_MaNguoiDungTao"]);
                }
            }
        }
    }
}