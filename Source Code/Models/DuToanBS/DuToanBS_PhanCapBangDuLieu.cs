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

namespace VIETTEL.Models.DuToanBS
{
    /// <summary>
    /// Lớp DuToanBS_PhanCapBangDuLieu cho phần bảng của Phân bổ chỉ tiêu
    /// </summary>
    public class DuToanBS_PhanCapBangDuLieu : BangDuLieu
    {
        protected int _iNamLamViec;
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
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="MaND">Mã người dùng</param>
        /// <param name="IPSua">IP của máy yêu cầu</param>
        ///sLNS,sL,sK,sM,sTM,sTTM
        public DuToanBS_PhanCapBangDuLieu(String iID_MaChungTu, Dictionary<String, String> arrGiaTriTimKiem, String MaND, String IPSua, String sLNS,String MaLoai)
        {
            this._iID_Ma = iID_MaChungTu;
            this._MaND = MaND;
            this._IPSua = IPSua;
            _dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(_MaND);
            String SQL;
            SqlCommand cmd;
            SQL = "SELECT * FROM DTBS_ChungTuChiTiet WHERE iID_MaChungTuChiTiet=@iID_MaChungTu AND iTrangThai=1";
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@iID_MaChungTu", _iID_Ma);
            cmd.CommandText = SQL;
            _dtBang = Connection.GetDataTable(cmd);
            cmd.Dispose();
            String sXauNoiMa = Convert.ToString(_dtBang.Rows[0]["sXauNoiMa"]);
            if (!String.IsNullOrEmpty(sXauNoiMa))
                sXauNoiMa = sXauNoiMa.Substring(8);
            _iNamLamViec = Convert.ToInt32(_dtBang.Rows[0]["iNamLamViec"]);
           
            int iID_MaTrangThaiDuyet = Convert.ToInt32(_dtBang.Rows[0]["iID_MaTrangThaiDuyet"]);
            String iID_MaNguoiDungTao = Convert.ToString(_dtBang.Rows[0]["sID_MaNguoiDungTao"]);
            String iID_MaChungTuTo =Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTuChiTiet", "iID_MaChungTuChiTiet", iID_MaChungTu, "iID_MaChungTu"));
            String iKyThuat = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", iID_MaChungTuTo, "iKyThuat")); ;
            // String sLNS = Convert.ToString(_dtBang.Rows[0]["sDSLNS"]);
            Boolean ND_DuocSuaChungTu = false;
            Boolean CheckTrangThaiDuyetMoiTao = false;
            if (iKyThuat == "1")
            {
                ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND,
                                                                                       iID_MaTrangThaiDuyet);
                _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND);
                CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet);
            }
            else
            {
                ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(DuToanBSModels.iID_MaPhanHe, MaND,
                                                                                      iID_MaTrangThaiDuyet);
                _DuocSuaChiTiet = LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanBSModels.iID_MaPhanHe, MaND);
                CheckTrangThaiDuyetMoiTao = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeDuToan, iID_MaTrangThaiDuyet);
            }

            //Trolytonghop duoc sua chung tu do minh tạo
            Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);

            if (checkTroLyTongHop && CheckTrangThaiDuyetMoiTao)
            {
                ND_DuocSuaChungTu = true;
                _DuocSuaChiTiet = true;
            }
            if (ND_DuocSuaChungTu == false)
            {
                _ChiDoc = true;
            }
            //duoc sua khi o trang thai moi tao
            if (CheckTrangThaiDuyetMoiTao == false) _DuocSuaChiTiet = false;

            if (iKyThuat == "1")
            {
                //Trang thai tu choi
                if (MaND == iID_MaNguoiDungTao && LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeChiTieu, iID_MaTrangThaiDuyet))
                {
                    _DuocSuaChiTiet = true;
                    _ChiDoc = false;
                }
                //Tro ly tong hop dc sua chung tu
                if (checkTroLyTongHop && iID_MaTrangThaiDuyet == DuToanBS_ChungTuChiTietModels.iMaTrangThaiDuyetKT)
                {
                    ND_DuocSuaChungTu = true;
                    _DuocSuaChiTiet = true;
                    _ChiDoc = false;
                }
            }
            else
            {
                //Trang thai tu choi
                if (MaND == iID_MaNguoiDungTao && LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeDuToan, iID_MaTrangThaiDuyet))
                {
                    _DuocSuaChiTiet = true;
                    _ChiDoc = false;
                }
                if (checkTroLyTongHop && iID_MaTrangThaiDuyet == DuToanBS_ChungTuChiTietModels.iMaTrangThaiDuyetKT)
                {
                    ND_DuocSuaChungTu = true;
                    _DuocSuaChiTiet = true;
                    _ChiDoc = false;
                }
            }
            MaLoai = Convert.ToString(_dtBang.Rows[0]["MaLoai"]);
            //Phan cap lan 2
            
            _dtChiTiet = DuToanBS_ChungTuChiTietModels.GetChungTuChiTiet_PhanCap(_iID_Ma, arrGiaTriTimKiem, MaND, sLNS, sXauNoiMa, iKyThuat,MaLoai);

            ThemHangTongCong();

            _dtChiTiet_Cu = _dtChiTiet.Copy();

            DienDuLieu(sLNS, MaLoai, iKyThuat);
        }
        protected void CapNhapHangTongCong()
        {
            String strDSTruongTien = MucLucNganSachModels.strDSTruongTien_So;
            String[] arrDSTruongTien = strDSTruongTien.Split(',');


            int len = arrDSTruongTien.Length;


            //Tinh lai cac hang cha
            for (int i = _dtChiTiet.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]))
                {
                    String iID_MaMucLucNganSach = Convert.ToString(_dtChiTiet.Rows[i]["iID_MaMucLucNganSach"]);
                    for (int k = 0; k < len; k++)
                    {
                        if (i == 2 && k > len - 2)
                        {
                            i = 2;
                        }
                        double S, S_PhanBo, S_DaCap, S_ConLai;
                        //rTongSo
                        S = 0;
                        for (int j = i + 1; j < _dtChiTiet.Rows.Count; j++)
                        {
                            if (iID_MaMucLucNganSach == Convert.ToString(_dtChiTiet.Rows[j]["iID_MaMucLucNganSach_Cha"]))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[j][arrDSTruongTien[k]])))
                                {
                                    S += Convert.ToDouble(_dtChiTiet.Rows[j][arrDSTruongTien[k]]);
                                }
                            }
                        }
                       
                                _dtChiTiet.Rows[i][arrDSTruongTien[k]] = S;
                    }
                }
            }
            //Them Hàng tổng cộng
            DataRow dr = _dtChiTiet.NewRow();

            DataRow[] arrdr = _dtChiTiet.Select("sNG<>''");
            double Tong = 0;
            for (int k = 0; k < len; k++)
            {
                Tong = 0;
                for (int i = 0; i < arrdr.Length; i++)
                {

                    if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i][arrDSTruongTien[k]])))
                    {
                        Tong += Convert.ToDouble(arrdr[i][arrDSTruongTien[k]]);
                    }

                }
                dr[arrDSTruongTien[k]] = Tong;
            }
            dr["sMoTa"] = "TỔNG CỘNG";
            dr["bLaHangCha"] = true;
            _dtChiTiet.Rows.Add(dr);
        }
        protected void ThemHangTongCong()
        {
            ////Thêm hàng tổng cộng 
            //DataRow R = _dtChiTiet.NewRow();
            //_dtChiTiet.Rows.Add(R);

            //for (int j = 0; j < _dtChiTiet.Columns.Count; j++)
            //{
            //    String TenCot = _dtChiTiet.Columns[j].ColumnName;
            //    if (TenCot.StartsWith("r"))
            //    {
            //        Double S = 0;
            //        for (int i = 0; i < _dtChiTiet.Rows.Count - 1; i++)
            //        {
            //            S += Convert.ToDouble(_dtChiTiet.Rows[i][TenCot]);
            //        }
            //        R[TenCot] = S;
            //    }

            //}
        }

        /// <summary>
        /// Hàm điền dữ liệu
        /// Mục đích: Điền tất cả thông tin vào các tham số của đối tượng Bảng dữ liệu
        /// </summary>
        protected void DienDuLieu(String sLNS,String MaLoai,String iKyThuat)
        {
            String iID_MaPhongBan = "";
            DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(_MaND);
            if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
            {
                DataRow drPhongBan = dtPhongBan.Rows[0];
                iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                dtPhongBan.Dispose();
            }
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            if (_arrDuLieu == null)
            {
                if (MaLoai == "DuToan")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    bool ok = true;
                    CapNhapHangTongCong();
                    for (int i = count; i >= 0; i--)
                    {
                        ok = false;
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])))
                            {
                                ok = false;
                            }
                            else if ((!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) && Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
                            {
                                ok = true;
                            }
                        }
                        //Loai bo het dong khong co du lieu
                        if (ok==false)
                        {
                            _dtChiTiet.Rows.RemoveAt(i);
                        }

                    }
                   
                }
                else if (MaLoai == "ChuaDuToan")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    bool ok = true;
                   
                    for (int i = count; i >= 0; i--)
                    {
                        ok = false;
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])))
                            {
                                ok = false;
                            }
                            else if ((!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) && Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
                            {
                                ok = true;
                            }
                        }
                        //Loai bo het dong  co du lieu
                        if (ok)
                        {
                            _dtChiTiet.Rows.RemoveAt(i);
                        }
                        
                    }
                    CapNhapHangTongCong();  
                }
                else if (MaLoai == "PhanCap")
                {
                    int count = dtChiTiet.Rows.Count - 1;
                    bool ok = true;
                    CapNhapHangTongCong();
                    for (int i = count; i >= 0; i--)
                    {
                        ok = false;
                        for (int j = 0; j < arrDSTruongTien.Length; j++)
                        {
                            if (arrDSTruongTien[j] == "rPhanCap" || arrDSTruongTien[j] == "rHangNhap" || arrDSTruongTien[j] == "rHangMua")
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])))
                                {
                                    ok = false;
                                }
                                else if (
                                    (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i][arrDSTruongTien[j]])) &&
                                     Convert.ToDecimal(_dtChiTiet.Rows[i][arrDSTruongTien[j]]) > 0))
                                {
                                    ok = true;
                                }
                            }
                        }
                        //Loai bo het dong khong co du lieu
                        if (ok == false)
                        {
                            _dtChiTiet.Rows.RemoveAt(i);
                        }
                       
                    }
                    count = _dtChiTiet.Rows.Count;
                    _dtChiTiet.Rows.RemoveAt(count-1);
                    CapNhapHangTongCong();
                }

                else
                {
                    //nganh ky thuat
                    if (iKyThuat == "1")
                    {
                        
                    }
                    else
                    {
                        //int count = dtChiTiet.Rows.Count - 1;
                        //bool ok = true;
                        //for (int i = count; i >= 0; i--)
                        //{
                        //    ok = true;
                        //    if (!String.IsNullOrEmpty(Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBan"])) && iID_MaPhongBan != Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBan"]) && iID_MaPhongBan != Convert.ToString(_dtChiTiet.Rows[i]["iID_MaPhongBanDich"]) && Convert.ToBoolean(_dtChiTiet.Rows[i]["bLaHangCha"]) == false)
                        //    {
                        //        ok = false;
                        //    }


                        //    if (ok == false)
                        //    {
                        //        _dtChiTiet.Rows.RemoveAt(i);
                        //    }
                        //}
                    }
                    CapNhapHangTongCong();
                }
                //if (MaLoai != "1" && MaLoai != "2")
                //{
                //    String SQL = "UPDATE DTBS_ChungTuChiTiet SET MaLoai='" + MaLoai + "' WHERE iID_MaChungTuChiTiet='" + _iID_Ma + "'";
                //    SqlCommand cmd = new SqlCommand(SQL);
                //    Connection.UpdateDatabase(cmd);
                //}


                //Them thong tin phần đầu ngân sách bảo đảm

                DataTable dtCT = _dtBang;

                DataRow rows = dtCT.Rows[0];


                DataRow dr = _dtChiTiet.NewRow();
                dr["sMoTa"] = "TỔNG Cộng";
                dr["sTenDonVi"] = rows["sTenDonVi"];
                dr["sTenDonVi_BaoDam"] = rows["sTenDonVi"];
                //dr["rHangNhap"] = rows["rHangNhap"];
                //dr["rHangMua"] = rows["rHangMua"];
                dr["rTuChi"] = rows["rPhanCap"];
                dr["rHienVat"] = rows["rHienVat"];
                dr["bLaHangCha"] = "true";
                dr["sLNS"] = rows["sLNS"];
                dr["sL"] = rows["sL"];
                dr["sK"] = rows["sK"];
                dr["sM"] = rows["sM"];
                dr["sTM"] = rows["sTM"];
                dr["sTTM"] = rows["sTTM"];
                dr["sNG"] = rows["sNG"];
                
                dtChiTiet.Rows.InsertAt(dr, 0);


                //Tinh o con lai
                DataRow[] arrdr = _dtChiTiet.Select("sNG<>'' AND bLaHangCha='False'");
                double Tong_TC = 0,Tong_HV = 0,Tong_HN = 0,Tong_DP = 0,Tong_HM = 0,Tong_PC = 0;
                
                    for (int i = 0; i < arrdr.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rTuChi"])))
                        {
                            Tong_TC += Convert.ToDouble(arrdr[i]["rTuChi"]);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rHienVat"])))
                        {
                            Tong_HV += Convert.ToDouble(arrdr[i]["rHienVat"]);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rDuPhong"])))
                        {
                            Tong_DP += Convert.ToDouble(arrdr[i]["rDuPhong"]);
                        }

                        if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rHangNhap"])))
                        {
                            Tong_HN += Convert.ToDouble(arrdr[i]["rHangNhap"]);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rHangMua"])))
                        {
                            Tong_HM += Convert.ToDouble(arrdr[i]["rHangMua"]);
                        }
                        if (!String.IsNullOrEmpty(Convert.ToString(arrdr[i]["rPhanCap"])))
                        {
                            Tong_PC += Convert.ToDouble(arrdr[i]["rPhanCap"]);
                        }

                    }
                

                 dr = _dtChiTiet.NewRow();
                dr["sMoTa"] = "Còn lại";
                dr["sTenDonVi_BaoDam"] = "";
                dr["rTuChi"] = Convert.ToDouble(rows["rPhanCap"]) - (Tong_TC); 
                dr["rHienVat"] = Convert.ToDouble(rows["rHienVat"]) - (Tong_HV);
                dr["rDuPhong"] =  0;

                dr["rHangNhap"] = 0;
                dr["rHangMua"] =  0;
                dr["rPhanCap"] = 0;
                dr["bLaHangCha"] = "true";
                dtChiTiet.Rows.InsertAt(dr, 1);
                CapNhapDanhSachMaHang();
                CapNhapDanhSachMaCot_Fixed(sLNS);
                CapNhapDanhSachMaCot_Slide(sLNS,MaLoai);
                CapNhapDanhSachMaCot_Them(sLNS);
                CapNhap_arrEdit(sLNS);
                CapNhap_arrDuLieu();
                CapNhap_arrThayDoi();
                CapNhap_arrLaHangCha();
               
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
                if (Convert.ToBoolean(R["bLaHangCha"]) == false)
                {
                    MaHang = String.Format("{0}_{1}", R["iID_MaChungTuChiTiet"], R["iID_MaMucLucNganSach"]);
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
        protected void CapNhapDanhSachMaCot_Fixed(String sLNS)
        {
            //Khởi tạo các mảng
            _arrHienThiCot = new List<Boolean>();
            _arrTieuDe = new List<string>();
            _arrDSMaCot = new List<string>();
            _arrWidth = new List<int>();

            String[] arrDSTruong = MucLucNganSachModels.strDSTruong.Split(',');
            String[] arrDSTruongTieuDe = MucLucNganSachModels.strDSTruongTieuDe.Split(',');
            String[] arrDSTruongTien = MucLucNganSachModels.strDSTruongTien.Split(',');
            String[] arrDSTruongDoRong = MucLucNganSachModels.strDSTruongDoRong.Split(',');

            //Xác định các cột tiền sẽ hiển thị
            _arrCotTienDuocHienThi = new Dictionary<String, Boolean>();
            for (int j = 0; j < arrDSTruongTien.Length; j++)
            {
                _arrCotTienDuocHienThi.Add(arrDSTruongTien[j], false);
                for (int i = 2; i < _dtChiTiet.Rows.Count-1; i++)
                {
                    DataRow R = _dtChiTiet.Rows[i];
                    if (Convert.ToBoolean(R["b" + arrDSTruongTien[j]]))
                    {
                        _arrCotTienDuocHienThi[arrDSTruongTien[j]] = true;
                        break;
                    }
                }
            }

            //Tiêu đề fix: Thêm trường sMaCongTrinh, sTenCongTrinh
            for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
            {
                _arrDSMaCot.Add(arrDSTruong[j]);
                _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                if (arrDSTruong[j] == "rTongSo" || arrDSTruong[j] == "sTNG")
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
        protected void CapNhapDanhSachMaCot_Slide(String sLNS,String MaLoai)
        {
            String strDSTruong = "";
            String strDSTruongTieuDe = "";
            String strDSTruongDoRong = "";

           
                 strDSTruong = "sTenDonVi_BaoDam,iID_MaPhongBanDich,rTuChi,rHienVat,rHangNhap,rHangMua,rDuPhong,rPhanCap";
                strDSTruongTieuDe = "Đơn vị,B,Tự chi, Hiện vật, Hàng nhập, Hàng mua, Dự phòng, Phân cấp";
                strDSTruongDoRong = "200,30,130,130,130,130,130,130";


                String[] arrDSTruong = strDSTruong.Split(',');
                String[] arrDSTruongTieuDe = strDSTruongTieuDe.Split(',');
                String[] arrDSTruongDoRong = strDSTruongDoRong.Split(',');

               

                //Phần mục lục ngân sách
                for (int j = 0; j < arrDSTruongTieuDe.Length; j++)
                {
                    _arrDSMaCot.Add(arrDSTruong[j]);
                    _arrTieuDe.Add(arrDSTruongTieuDe[j]);
                    _arrWidth.Add(Convert.ToInt32(arrDSTruongDoRong[j]));
                    if (arrDSTruong[j] == "sNG" && sLNS.Substring(0, 1) == "8")
                    {
                        _arrHienThiCot.Add(false);
                    }
                    else if (arrDSTruong[j] == "rHangNhap" || arrDSTruong[j] == "rHangMua" || arrDSTruong[j] == "rDuPhong" || arrDSTruong[j] == "rPhanCap")
                        _arrHienThiCot.Add(false);
                    else
                        _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(1);
                    _arrTieuDeNhomCot.Add("");
                }
                           
               


         
           
            //Them cot duyet
            //if (CoCotDuyet)
            //{
            //    //Cột đồng ý
            //    _arrDSMaCot.Add("bDongY");
            //    if (_ChiDoc)
            //    {
            //        _arrTieuDe.Add("<div class='check'></div>");
            //    }
            //    else
            //    {
            //        _arrTieuDe.Add("<div class='check' onclick='BangDuLieu_CheckAll();'></div>");
            //    }
            //    _arrWidth.Add(20);
            //    _arrHienThiCot.Add(true);
            //    _arrSoCotCungNhom.Add(1);
            //    _arrTieuDeNhomCot.Add("");
            //    //Cột Lý do
            //    _arrDSMaCot.Add("sLyDo");
            //    _arrTieuDe.Add("Nhận xét");
            //    _arrWidth.Add(200);
            //    _arrHienThiCot.Add(true);
            //    _arrSoCotCungNhom.Add(1);
            //    _arrTieuDeNhomCot.Add("");
            //}
               
                    arrDSMaCot.Add("sGhiChu");
                    _arrTieuDe.Add("Ghi Chú");
                    _arrWidth.Add(300);
                    _arrHienThiCot.Add(true);
                    _arrSoCotCungNhom.Add(1);
                    _arrTieuDeNhomCot.Add("");
                
            _nCotSlide = _arrDSMaCot.Count - _nCotFixed;
        }

        /// <summary>
        /// Hàm thêm các cột thêm của bảng
        /// </summary>
        protected void CapNhapDanhSachMaCot_Them(String sLNS)
        {
            String strDSTruong = "";
           
               
                    strDSTruong =
                        "sLNS,sL,sK,iID_MaDonVi,iID_MaMucLucNganSach,sMaCongTrinh,sMauSac,sFontColor,sFontBold";
              
            

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
        /// Hàm xác định các ô có được Edit hay không
        /// </summary>
        protected void CapNhap_arrEdit(String sLNS)
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
                    else if (_DuocSuaChiTiet && okHangChiDoc == false && arrDSMaCot[j] == "sGhiChu")
                    {
                        okOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet &&  okHangChiDoc == false && arrDSMaCot[j] == "sTenDonVi")
                    {
                        okOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet &&  okHangChiDoc == false && arrDSMaCot[j] == "sTenDonVi_BaoDam")
                    {
                        okOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet && okHangChiDoc == false && arrDSMaCot[j] == "iID_MaPhongBanDich")
                    {
                        okOChiDoc = false;
                    }
                    else if (_DuocSuaChiTiet &&  okHangChiDoc == false && arrDSMaCot[j] == "bPhanCap")
                    {
                        okOChiDoc = false;
                    }
                    else
                    {
                        //Cot tien
                        if (_DuocSuaChiTiet &&
                                _ChiDoc == false &&
                                okHangChiDoc == false &&
                                _arrDSMaCot[j] != "rTongSo" &&
                                _arrDSMaCot[j].EndsWith("_PhanBo") == false &&
                                _arrDSMaCot[j].EndsWith("_DaCap") == false &&
                                _arrDSMaCot[j].EndsWith("_ConLai") == false &&
                                _dtChiTiet.Columns.IndexOf('b' + _arrDSMaCot[j]) >= 0 &&
                                Convert.ToBoolean(R['b' + _arrDSMaCot[j]]))
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
                for (int j = 0; j < _arrDSMaCot.Count - 3; j++)
                {
                    //Xac dinh gia tri
                    _arrDuLieu[i].Add(Convert.ToString(R[_arrDSMaCot[j]]));
                }

                if (i == _dtChiTiet.Rows.Count-1)
                {
                    _arrDuLieu[i].Add("#A0A0A0");
                    _arrDuLieu[i].Add("#FF0000");
                    _arrDuLieu[i].Add("bold");
                }
                else if (i == 1)
                {
                  
                    double CL_TC = Convert.ToDouble(_dtChiTiet.Rows[1]["rTuChi"]);
                    double CL_HV = Convert.ToDouble(_dtChiTiet.Rows[1]["rHienVat"]);
                    double CL_DP = Convert.ToDouble(_dtChiTiet.Rows[1]["rDuPhong"]);

                    double CL_HN =Convert.ToDouble(_dtChiTiet.Rows[1]["rHangNhap"]);
                    double CL_HM = Convert.ToDouble(_dtChiTiet.Rows[1]["rHangMua"]);
                    double CL_PC = Convert.ToDouble(_dtChiTiet.Rows[1]["rPhanCap"]);

                    //if (CL_HN < 0 || CL_HM < 0 || CL_PC < 0)
                    //{
                    //    thieu = "1";
                    //}
                    //else
                    //{
                    //    if (CL_HN > 0 || CL_HM > 0 || CL_PC > 0)
                    //        thua = "1";
                    //    else
                    //        Du = "1";


                    //}
                  //  double TongCL = CL_HN + CL_HM + CL_PC+CL_TC+CL_DP;

                    double dConLaiTC = DuToanBS_ChungTuChiTietModels.dTongConLai(Convert.ToString(_dtBang.Rows[0]["iID_MaChungTuChiTiet"]),"rTuChi");
                    double dConLaiHV = DuToanBS_ChungTuChiTietModels.dTongConLai(Convert.ToString(_dtBang.Rows[0]["iID_MaChungTuChiTiet"]), "rHienVat");
                    double dPhanCap = Convert.ToDouble(_dtBang.Rows[0]["rPhanCap"]);
                    double dHienVat = Convert.ToDouble(_dtBang.Rows[0]["rHienVat"]);
                    double TongCLTC = dPhanCap - dConLaiTC;
                    double TongCLHV = dHienVat - dConLaiHV;
                    if (TongCLTC < 0 || TongCLHV<0)
                    {
                        _arrDuLieu[i].Add("#FF0000");
                        _arrDuLieu[i].Add("");
                        _arrDuLieu[i].Add("");
                    }
                    else if (TongCLTC == 0 && TongCLHV == 0)
                    {
                      
                        _arrDuLieu[i].Add("#3399FF");
                        _arrDuLieu[i].Add("");
                        _arrDuLieu[i].Add("");
                    }
                    else
                    {
                        _arrDuLieu[i].Add("#f8e6d1");
                        _arrDuLieu[i].Add("");
                        _arrDuLieu[i].Add("");
                    }
                  
                }
                else
                {
                    _arrDuLieu[i].Add("");
                    _arrDuLieu[i].Add("");
                    _arrDuLieu[i].Add("");
                }
            }
        }
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