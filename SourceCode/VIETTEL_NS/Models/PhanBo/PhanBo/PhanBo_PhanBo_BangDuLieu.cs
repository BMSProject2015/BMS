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
    /// Lớp PhanBo_PhanBo_BangDuLieu cho phần bảng của Phân bổ
    /// </summary>
    public class PhanBo_PhanBo_BangDuLieu : BangDuLieu
    {
        private DataTable dt;
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaPhanBo">Mã phân bổ</param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        public PhanBo_PhanBo_BangDuLieu(String iID_MaPhanBo, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua)
        {
            this._iID_Ma = iID_MaPhanBo;
            this._MaND = MaND;
            this._IPSua = IPSua;

            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM PB_PhanBo WHERE iID_MaPhanBo=@iID_MaPhanBo AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaPhanBo", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();

            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);

            Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND, iID_MaTrangThaiDuyet);
            if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(PhanBoModels.iID_MaPhanHePhanBo, iID_MaTrangThaiDuyet))
            {
                _ChiDoc = true;
            }

            if (ND_DuocSuaChungTu == false)
            {
                _ChiDoc = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanBoModels.iID_MaPhanHePhanBo, iID_MaTrangThaiDuyet) &&
                ND_DuocSuaChungTu)
            {
                _CoCotDuyet = true;
                _DuocSuaDuyet = true;
            }

            if (LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanBoModels.iID_MaPhanHePhanBo, iID_MaTrangThaiDuyet))
            {
                _CoCotDuyet = true;
            }

            _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHePhanBo, MaND);

            _dtChiTiet = PhanBo_PhanBoChiTietModels.Get_dtPhanBoChiTiet(_iID_Ma, arrGiaTriTimKiem);
            _dtChiTiet_Cu = _dtChiTiet.Copy();
            //dt = DonViChoPhanBo(_iID_Ma);
            dt = LaySoTienConLai(_iID_Ma);
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
                CapNhapHangTongCong();
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed();
                CapNhapDanhSachMaCot_Slide();
                CapNhapDanhSachMaCot_Them();
                CapNhap_arrLaHangCha();
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
                String MaHang = Convert.ToString(R["iID_MaPhanBoChiTiet"]);
                _arrDSMaHang.Add(MaHang);
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

            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = MucLucNganSachModels.strDSTruongTieuDe.Split(',');
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien_So.Split(',');
            String[] arrDSTruongTienTieuDe = MucLucNganSachModels.strDSTruongTienTieuDe_So.Split(',');
            String[] arrDSTruongTienDoRong = MucLucNganSachModels.strDSTruongTienDoRong_So.Split(',');
            String[] arrDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong.Split(',');

            //Xác định các cột tiền sẽ hiển thị
            _arrCotTienDuocHienThi = new Dictionary<String, Boolean>();
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                _arrCotTienDuocHienThi.Add(arrDSTruongTien[j], false);
                for (int i = 0; i < _dtChiTiet.Rows.Count; i++)
                {
                    DataRow R = _dtChiTiet.Rows[i];
                    if (Convert.ToBoolean(R["b" + arrDSTruongTien[j]]))
                    {
                        _arrCotTienDuocHienThi[arrDSTruongTien[j]] = true;
                        break;
                    }
                }
            }

            //Tiêu đề fix: Thêm trường sTenCongTrinh
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                _arrHienThiCot.Add(true);
                _arrSoCotCungNhom.Add(1);
                _arrTieuDeNhomCot.Add("");
            }
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if ((arrDSTruongTien[j] == "sTenCongTrinh")
                    && _arrCotTienDuocHienThi[arrDSTruongTien[j]])
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add(arrDSTruongTienTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(1);
                    _arrTieuDeNhomCot.Add("");
                }
            }

            _nCotFixed = _arrDSMaCot.Count;
        }

        /// <summary>
        /// Hàm thêm danh sách cột Slide vào bảng
        ///     - Cột Slide của bảng gồm:
        ///         + Trường của cột tiền trừ sMaCongTrinh, sTenCongTrinh
        ///         + Trường sTongSo
        ///         + Trường bDongY, sLyDo
        ///     - Cập nhập số lượng cột Slide
        /// </summary>
        protected void CapNhapDanhSachMaCot_Slide()
        {
            String[] arrDSTruongTien = "rNgay,rSoNguoi,rChiTaiKhoBac,rTonKho,rTuChi,rChiTapTrung,rHangNhap,rHangMua,rHienVat,rDuPhong,rPhanCap,rTongSo".Split(',');
            String[] arrDSTruongTienTieuDe = "Ngày,Người,Chi tại kho bạc,Tồn kho,Tự chi,Chi tập trung,Hàng nhập,Hàng mua,Hiện vật,Dự phòng,Phân cấp,Tổng số".Split(',');
            String[] arrDSTruongTienDoRong = "100,100,100,100,100,100,100,100,100,100,100,100,100".Split(',');

            //Tiêu đề tiền
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                if (arrDSTruongTien[j] == "rTongSo" ||
                    (arrDSTruongTien[j] != "sTenCongTrinh" && _arrCotTienDuocHienThi[arrDSTruongTien[j]]))
                {
                    _arrDSMaCot.Add(arrDSTruongTien[j] + "_ChiTieu");
                    _arrTieuDe.Add("Phân bổ ngành");
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(3);
                    _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

                    _arrDSMaCot.Add(arrDSTruongTien[j]);
                    _arrTieuDe.Add("Phân bổ đơn vị");
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(3);
                    _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);

                    _arrDSMaCot.Add(arrDSTruongTien[j] + "_ConLai");
                    _arrTieuDe.Add("Còn lại");
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongTienDoRong[j]));
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(3);
                    _arrTieuDeNhomCot.Add(arrDSTruongTienTieuDe[j]);
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
            String strDSTruong = "sMaCongTrinh";
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
                            okHangChiDoc == false &&
                            _arrDSMaCot[j] != "rTongSo" &&
                            _arrDSMaCot[j].EndsWith("_ChiTieu") == false &&
                            _arrDSMaCot[j].EndsWith("_ConLai") == false &&
                            _dtChiTiet.Columns.IndexOf("b" + _arrDSMaCot[j]) >=0 && 
                            Convert.ToBoolean(R["b" + _arrDSMaCot[j]]))
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
                        String sXauNoiMa = Convert.ToString(R["sXauNoiMa"]);
                        String sXauNoiMa1 = "";
                        for (int c = 0; c < dt.Rows.Count; c++)
                        {
                            sXauNoiMa1 = Convert.ToString(dt.Rows[c]["sXauNoiMa"]);
                            if (sXauNoiMa1.Equals(sXauNoiMa))
                            {
                                _arrDuLieu[i].Add(Convert.ToString(dt.Rows[c][_arrDSMaCot[j].Replace("_ConLai","")]));
                                break;
                            }
                        }
                        //Double GT1 = Convert.ToDouble(_arrDuLieu[i][j - 1]);
                       // Double GT2 = Convert.ToDouble(_arrDuLieu[i][j - 2]);
                       // _arrDuLieu[i].Add(Convert.ToString(GT2 - GT1));
                    }
                    else
                    {
                        //tuannn sửa ngày 13/7/2012
                        if (_arrDSMaCot[j].IndexOf("_ChiTieu")>0)
                        {
                            _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j].Replace("_ChiTieu","")]));
                        }
                        else
                        {
                            _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Hàm tính lại các ô tổng số và tổng cộng các hàng cha
        /// </summary>
        protected void CapNhapHangTongCong()
        {
            String strDSTruongTien = MucLucNganSachModels.strDSTruongTien_So + ",rTongSo";
            String strDSTruongTien_ChiTieu = MucLucNganSachModels.strDSTruongTien_So.Replace(",", "_ChiTieu,") + "_ChiTieu,rTongSo_ChiTieu";
            String[] arrDSTruongTien = strDSTruongTien.Split(',');
            String[] arrDSTruongTien_ChiTieu = strDSTruongTien_ChiTieu.Split(',');

            int len = arrDSTruongTien.Length;
            //Tinh lai cot tong so
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                {
                    double S;
                    //rTongSo
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien[k].StartsWith("rChiTapTrung") == false)
                        {
                            S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[k]]);
                        }
                    }
                    if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[len - 1]]) != S)
                    {
                        _dtChiTiet.Rows[i][arrDSTruongTien[len - 1]] = S;
                    }
                    //rTongSo_ChiTieu
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien_ChiTieu[k].StartsWith("rChiTapTrung")==false)
                        {
                            S += Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[k]]);
                        }
                    }
                    if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[len - 1]]) != S)
                    {
                        _dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[len - 1]] = S;
                    }
                }
            }
            //Tinh lai cac hang cha
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]))
                {
                    String iID_MaMucLucNganSach = Convert.ToString(_dtChiTiet.Rows[i]["iID_MaMucLucNganSach"]);
                    for (int k = 0; k < len; k++)
                    {
                        double S, S_ChiTieu;
                        //rTongSo
                        S = 0;
                        S_ChiTieu = 0;
                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iID_MaMucLucNganSach == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach_Cha"]))
                            {
                                S += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                                S_ChiTieu += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien_ChiTieu[k]]);
                            }
                        }
                        if (Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien[k]]) != S ||
                            Convert.ToDouble(_dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[k]]) != S_ChiTieu)
                        {
                            _dtChiTiet.Rows[i][arrDSTruongTien[k]] = S;
                            _dtChiTiet.Rows[i][arrDSTruongTien_ChiTieu[k]] = S_ChiTieu;
                        }
                    }
                }
            }
            //Tinh lai cot tong so con lai
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(dt.Rows[i]["bLaHangCha"]) == false)
                {
                    double S;
                    //rTongSo
                    S = 0;
                    for (int k = 0; k < len - 1; k++)
                    {
                        if (arrDSTruongTien[k].StartsWith("rChiTapTrung") == false)
                        {
                            S += Convert.ToDouble(dt.Rows[i][arrDSTruongTien[k]]);
                        }
                    }
                    if (Convert.ToDouble(dt.Rows[i][arrDSTruongTien[len - 1]]) != S)
                    {
                        dt.Rows[i][arrDSTruongTien[len - 1]] = S;
                    }
                }
            }

            //Tinh lai hang cha con lai
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(dt.Rows[i]["bLaHangCha"]))
                {
                    String iID_MaMucLucNganSach = Convert.ToString(dt.Rows[i]["iID_MaMucLucNganSach"]);
                    for (int k = 0; k < len; k++)
                    {
                        double S;
                        //rTongSo
                        S = 0;
                        for (int j = i + 1; j < dt.Rows.Count; j++)
                        {
                            if (iID_MaMucLucNganSach == Convert.ToString(dt.Rows[j]["iID_MaMucLucNganSach_Cha"]))
                            {
                                S += Convert.ToDouble(dt.Rows[j][arrDSTruongTien[k]]);
                            }
                        }
                        if (Convert.ToDouble(dt.Rows[i][arrDSTruongTien[k]]) != S)
                        {
                            dt.Rows[i][arrDSTruongTien[k]] = S;
                        }
                    }
                }
            }
        }
        //lấy đơn vị chờ phân bổ để vào tường còn lại
        //hiện tại không dùng chuyển sang dùng hàm  LaySoTienConLai()
        protected DataTable DonViChoPhanBo(String iID_MaPhanBo)
        {
            String SQL = "SELECT * FROM PB_PhanBoChiTiet WHERE iID_MaDonVi='99' AND iID_MaChiTieu = (SELECT iID_MaChiTieu FROM PB_PhanBo WHERE iID_MaPhanBo=@iID_MaPhanBo)";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanBo",iID_MaPhanBo);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        //lấy số tiền còn lại trong phân bổ cho đơn vị
        protected DataTable LaySoTienConLai(String iID_MaPhanBo)
        {
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien_Full.Split(',');
            String sSumPB = "", sChiTieu = "",sSelect="";
            for (int i = 0; i < arrDSTruongTien.Length; i++)
            {
                if (arrDSTruongTien[i].StartsWith("r"))
                {
                    sChiTieu = sChiTieu+"," + arrDSTruongTien[i];
                    sSumPB = sSumPB + String.Format(",SUM({0}) AS {0}", arrDSTruongTien[i]);
                    sSelect = sSelect + String.Format(",{0}=CT.{0} - PB.{0}", arrDSTruongTien[i]);
                    //if (i < arrDSTruongTien.Length - 1)
                    //{
                    //    sSumPB = sSumPB + ",";
                    //    sChiTieu = sChiTieu + ",";
                    //    sSelect = sSelect + ",";
                    //}
                }
            }
            String SQL = String.Format(@"SELECT CT.iID_MaMucLucNganSach,CT.iID_MaMucLucNganSach_Cha,CT.bLaHangCha,CT.sXauNoiMa,CT.sLNS,CT.sL,CT.sK,CT.sM,CT.sTM,CT.sTTM,CT.sNG,CT.sTNG,CT.sMoTa
                        {0}
                        FROM (
                        (SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                        {1}
                        FROM PB_ChiTieuChiTiet 
                        WHERE iID_MaChiTieu=(SELECT iID_MaChiTieu FROM PB_PhanBo WHERE iID_MaPhanBo=@iID_MaPhanBo)
                        ) CT
                        INNER JOIN 
                        (SELECT iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa
                        {2}
                        FROM PB_PhanBoChiTiet
                        WHERE iID_MaDonVi <>'99' AND iID_MaChiTieu=(SELECT iID_MaChiTieu FROM PB_PhanBo WHERE iID_MaPhanBo=@iID_MaPhanBo)
                        GROUP BY iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,bLaHangCha,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa) PB
                        ON CT.sXauNoiMa=PB.sXauNoiMa
                        )", sSelect,sChiTieu,sSumPB);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanBo",iID_MaPhanBo);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;

        }
    }
}