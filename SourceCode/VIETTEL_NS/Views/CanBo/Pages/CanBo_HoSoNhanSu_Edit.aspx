<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String ParentID = "Edit";
        String MaND = User.Identity.Name;
        String sHoTen = "", sHoDem = "", sTen = "", dNgayTuyenDung = "", dNgayVaoCQ = "", iID_MaTinh = "", iID_MaHuyen = "", iID_MaXaPhuong = "", sSoHieuCBCC = "", iID_MaDoiTuong = "", sTenGoiKhac = "",
            sSoSoLuong = "", sSoTaiKhoan = "", sNganHang = "", dNgaySinh = "", bGioiTinh = "", iID_ChucVuHienTai = "", iID_ChucVuKiemNhiem = "", iID_MaCapBac = "", sHoKhauThuongTru = "",
            sNoiSinh = "", sDiaChi = "", sEmail = "", sWebsite = "", sSoDienThoai = "", sTinhTrangSucKhoe = "", rChieuCao = "", rCanNang = "", sLaThuongBinhHang = "", iID_MaGiaDinhChinhSach = "", iID_MaNhomMau = "",
            iID_MaPhongBan = "", iID_MaCheDoHuongLuong = "", iID_MaDanToc = "", iID_MaTonGiao = "", iID_MaThanhPhanGD = "", iID_MaTinhTrangHonNhan = "",
            iID_MaTrinhDoVanHoa = "", iID_MaTrinhDoQuanLyNN = "", iID_MaHocHam = "", iID_MaTrinhDoLyLuanChinhTri = "", iID_MaTrinhDoTinHoc = "", iID_MaTrinhDoChuyenMonCaoNhat = "",
            sNoiDaoTao="", iID_MaChuyenNganh = "", iID_MaTinhTrangCanBo = "", sNgoaiNgu = "", sCongViecChinhDangLam = "", sCMND = "", dNgayCap = "", sNoiCap = "", sSoHoChieu = "", dNgayCapHoChieu = "",
            dHanSuDungHoChieu = "", iID_MaLoaiHoChieu = "", sNgheNghiepTruocTuyenDung = "", sVaoCQODau = "", dNgayThamGiaCM = "", dNgayVaoDang = "", dNgayChinhThuc = "", dNgayThamGiaToChucCTXH = "",
            iID_MaChucVu = "", dNgayNhapNgu = "", dNgayXuatNgu = "", dNgayTaiNgu = "", iID_MaQuanHam = "", iNamPhongQuanHam = "", iID_MaNgachLuong = "", iID_MaBacLuong = "",
            dNgayHuongNgach = "", sMaSoHoSo = "", sViTriLuuHoSo = "", sImages = "../../../images/Portrait.jpg", dNgayVaoDoan = "", sNoiVaoDoan = "", iID_MaTrinhDoChiHuyQuanSu = "", sTiengDanToc = "",
            sHoTenThuongDung = "", sBiDanh = "", sBenhChinh = "", iID_MaThanhPhanBanThan = "", iID_MaHocVi = "", iNamPhongHocVi = "", iNamPhongHocHam = "", iID_MaLyDoTangGiam = "", iNuQuanNhan = "", rLuongCoBan_HeSo="";

        String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
        DataTable dtTrangThai = new DataTable();
        dtTrangThai.Columns.Add("iID_MaTrangThai", typeof(string));
        dtTrangThai.Columns.Add("sTen", typeof(string));
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        DataRow R1 = dtTrangThai.NewRow();
        R1["iID_MaTrangThai"] = "T";
        R1["sTen"] = "Tăng";
        dtTrangThai.Rows.Add(R1);
        DataRow R2 = dtTrangThai.NewRow();
        R2["iID_MaTrangThai"] = "G";
        R2["sTen"] = "Giảm";
        dtTrangThai.Rows.Add(R2);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThai", "sTen");

        //Đổ dữ liệu vào cột tôn giáo
        var dtTonGiao = DanhMucModels.DT_DanhMuc("CanBo_TonGiao", true, "--- Lựa chọn ---");
        SelectOptionList slMaTonGiao = new SelectOptionList(dtTonGiao, "iID_MaDanhMuc", "sTen");
        if (dtTonGiao != null) dtTonGiao.Dispose();
        //Đổ dữ liệu vào cột tôn giáo
        var dtGDCS = DanhMucModels.DT_DanhMuc("CanBo_GiaDinhChinhSach", true, "--- Lựa chọn ---");
        SelectOptionList slMaGiaDinhChinhSach = new SelectOptionList(dtGDCS, "iID_MaDanhMuc", "sTen");
        if (dtGDCS != null) dtGDCS.Dispose();
        //Đổ dữ liệu vào chức vụ
        var dtChucVu = CanBo_DanhMucNhanSuModels.getChucVu(true, "--- Lựa chọn ---");// DanhMucModels.DT_DanhMuc("CanBo_ChucVu", true, "--- Lựa chọn ---");
        SelectOptionList slMaChucVu = new SelectOptionList(dtChucVu, "sID_MaChucVu", "sTen");
        if (dtChucVu != null) dtChucVu.Dispose();
        //Tình trạng hôn nhân
        var dtTinhTrangHonNhan = DanhMucModels.DT_DanhMuc("CanBo_TinhTrangHonNhan", true, "--- Lựa chọn ---");
        SelectOptionList slMaTinhTrangHonNhan = new SelectOptionList(dtTinhTrangHonNhan, "iID_MaDanhMuc", "sTen");
        if (dtTinhTrangHonNhan != null) dtTinhTrangHonNhan.Dispose();
        //Đối tượng
        var dtDoiTuong = DanhMucModels.DT_DanhMuc("CanBo_DoiTuong", true, "--- Lựa chọn ---");
        SelectOptionList slMaDoiTuong = new SelectOptionList(dtDoiTuong, "iID_MaDanhMuc", "sTen");
        if (dtDoiTuong != null) dtDoiTuong.Dispose();
        //Đối tượng
        var dtTPGD = DanhMucModels.DT_DanhMuc("CanBo_ThanhPhanGD", true, "--- Lựa chọn ---");
        SelectOptionList slMaThanhPhanGD = new SelectOptionList(dtTPGD, "iID_MaDanhMuc", "sTen");
        if (dtTPGD != null) dtTPGD.Dispose();
        //Trình độ học vấn
        //var dtTrinhDoHocVan = DanhMucModels.DT_DanhMuc("CanBo_TrinhDoHocVan", true, "--- Lựa chọn ---");
        //SelectOptionList slMaTrinhDoHocVan = new SelectOptionList(dtTrinhDoHocVan, "iID_MaDanhMuc", "sTen");
        //if (dtTrinhDoHocVan != null) dtTrinhDoHocVan.Dispose();
        var dtTrinhDoHocVan = CanBo_DanhMucNhanSuModels.getHocVan(true, "--- Lựa chọn ---");
        SelectOptionList slMaTrinhDoHocVan = new SelectOptionList(dtTrinhDoHocVan, "sID_MaTrinhDo", "sTen");
        if (dtTrinhDoHocVan != null) dtTrinhDoHocVan.Dispose();
        //Chế độ hưởng lương
        var dtHuongLuong = DanhMucModels.DT_DanhMuc("CanBo_HuongLuong", true, "--- Lựa chọn ---");
        SelectOptionList slMaHuongLuong = new SelectOptionList(dtHuongLuong, "iID_MaDanhMuc", "sTen");
        if (dtHuongLuong != null) dtHuongLuong.Dispose();
        //Tình trạng tin học
        var dtTinhTrangTinHoc = DanhMucModels.DT_DanhMuc("CanBo_TrinhDoTinHoc", true, "--- Lựa chọn ---");
        SelectOptionList slMaTinhTrangTinHoc = new SelectOptionList(dtTinhTrangTinHoc, "iID_MaDanhMuc", "sTen");
        if (dtTinhTrangTinHoc != null) dtTinhTrangTinHoc.Dispose();
        //Quân hàm
        var dtQuanHam = DanhMucModels.DT_DanhMuc("CanBo_QuanHam", true, "--- Lựa chọn ---");
        SelectOptionList slMaQuanHam = new SelectOptionList(dtQuanHam, "iID_MaDanhMuc", "sTen");
        if (dtQuanHam != null) dtQuanHam.Dispose();

        //Chuyên ngành
        var dtChuyenNganh = DanhMucModels.DT_DanhMuc("CanBo_ChuyenNganh", true, "--- Lựa chọn ---");
        SelectOptionList slMaChuyenNganh = new SelectOptionList(dtChuyenNganh, "iID_MaDanhMuc", "sTen");
        if (dtChuyenNganh != null) dtChuyenNganh.Dispose();
        //Dân tộc
        var dtDanToc = DanhMucModels.DT_DanhMuc("CanBo_DanToc", true, "--- Lựa chọn ---");
        SelectOptionList slMaDanToc = new SelectOptionList(dtDanToc, "iID_MaDanhMuc", "sTen");
        if (dtDanToc != null) dtDanToc.Dispose();

        //Trình độ quản lý nhà nước
        var dtTrinhDoQuanLyNhaNuoc = DanhMucModels.DT_DanhMuc("CanBo_TrinhDoQuanLyNhaNuoc", true, "--- Lựa chọn ---");
        SelectOptionList slMaTrinhDoQLNN = new SelectOptionList(dtDanToc, "iID_MaDanhMuc", "sTen");
        if (dtTrinhDoQuanLyNhaNuoc != null) dtTrinhDoQuanLyNhaNuoc.Dispose();
        //Học hàm
        var dtHocHam = DanhMucModels.DT_DanhMuc("CanBo_HocHam", true, "--- Lựa chọn ---");
        SelectOptionList slMaHocHam = new SelectOptionList(dtHocHam, "iID_MaDanhMuc", "sTen");
        if (dtHocHam != null) dtHocHam.Dispose();
        //Học vị
        var dtHocVi = DanhMucModels.DT_DanhMuc("CanBo_HocVi", true, "--- Lựa chọn ---");
        SelectOptionList slMaHocVi = new SelectOptionList(dtHocVi, "iID_MaDanhMuc", "sTen");
        if (dtHocVi != null) dtHocVi.Dispose();

        //Loại hộ chiếu
        var dtLoaiHoChieu = DanhMucModels.DT_DanhMuc("CanBo_LoaiHoChieu", true, "--- Lựa chọn ---");
        SelectOptionList slMaLoaiHoChieu = new SelectOptionList(dtLoaiHoChieu, "iID_MaDanhMuc", "sTen");

        if (dtLoaiHoChieu != null) dtLoaiHoChieu.Dispose();
        //Trình độ lý luận chính trị
        var dtTrinhDoLyLuanChinhTri = DanhMucModels.DT_DanhMuc("CanBo_TrinhDoLyLuanChinhTri", true, "--- Lựa chọn ---");
        SelectOptionList slMaTrinhDoLyLuanChinhTri = new SelectOptionList(dtTrinhDoLyLuanChinhTri, "iID_MaDanhMuc", "sTen");
        if (dtTrinhDoLyLuanChinhTri != null) dtTrinhDoLyLuanChinhTri.Dispose();
        //Trình độ chuyên môn
        var dtTrinhDoChuyenMon = CanBo_DanhMucNhanSuModels.getHocVan(true, "--- Lựa chọn ---");//DanhMucModels.DT_DanhMuc("CanBo_TrinhDoChuyenMon", true, "--- Lựa chọn ---");
        SelectOptionList slMaTrinhDoChuyenMon = new SelectOptionList(dtTrinhDoChuyenMon, "sID_MaTrinhDo", "sTen");
        if (dtTrinhDoChuyenMon != null) dtTrinhDoChuyenMon.Dispose();

        //Nhóm máu
        var dtNhomMau = DanhMucModels.DT_DanhMuc("CanBo_NhomMau", true, "--- Lựa chọn ---");
        SelectOptionList slMaNhomMau = new SelectOptionList(dtNhomMau, "iID_MaDanhMuc", "sTen");
        if (dtNhomMau != null) dtNhomMau.Dispose();
        //tình trạng cán bộ
        var dtTTCB = CanBo_DanhMucNhanSuModels.getTinhTrangCB(false, "--- Lựa chọn ---");
        SelectOptionList slMaTinhTrangCB = new SelectOptionList(dtTTCB, "iID_Ma", "sTen");
        if (dtTTCB != null) dtTTCB.Dispose();
        //Giới tính
        var dtGioiTinh = CanBo_DanhMucNhanSuModels.getGioiTinh(false, "--- Lựa chọn ---");
        SelectOptionList slGioiTinh = new SelectOptionList(dtGioiTinh, "iID_Ma", "sTen");
        if (dtGioiTinh != null) dtGioiTinh.Dispose();
        //Đơn vị
        // var dtDonVi = DanhMucModels.getPhongBanByCombobox(true, "--- Lựa chọn ---");
        var dtDonVi = DanhMucModels.getDonViByComboboxGroup(true, "--- Lựa chọn ---");
        SelectOptionList slMaDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        if (dtDonVi != null) dtDonVi.Dispose();


        //Ngạch lương
        var dtNgachLuong = CanBo_DanhMucNhanSuModels.getNgachLuong(true, "--- Lựa chọn ---");
        SelectOptionList slMaNghachLuong = new SelectOptionList(dtNgachLuong, "iID_MaNgachLuong", "sTenNgachLuong");
        if (dtNgachLuong != null) dtNgachLuong.Dispose();

        //Bậc lương
        //var dtBacLuong = CanBo_DanhMucNhanSuModels.getBacLuong("", true, "--- Lựa chọn ---");
        //SelectOptionList slBacLuong = new SelectOptionList(dtBacLuong, "iID_MaBacLuong", "sTenBacLuong");
        //if (dtBacLuong != null) dtBacLuong.Dispose();

        //tinh
        var dtTinh = CanBo_DanhMucNhanSuModels.getTinh(true, "--- Lựa chọn ---");
        SelectOptionList slMaTinh = new SelectOptionList(dtTinh, "iID_MaTinh", "sTenTinh");
        if (dtTinh != null) dtTinh.Dispose();
        ////huyện
        //var dtHuyen = CanBo_DanhMucNhanSuModels.getHuyen("", true, "--- Lựa chọn ---");
        //SelectOptionList slMaHuyen = new SelectOptionList(dtHuyen, "iID_MaHuyen", "sTenHuyen");
        //if (dtHuyen != null) dtHuyen.Dispose();
        ////xã phuonwg
        //var dtXaphuong = CanBo_DanhMucNhanSuModels.getXaPhuong("", true, "--- Lựa chọn ---");
        //SelectOptionList slMaXaPhuong = new SelectOptionList(dtXaphuong, "iID_MaXaPhuong", "sTenXaPhuong");
        //if (dtXaphuong != null) dtXaphuong.Dispose();

        //Nghệ nghiệp
        var dtTPBT = DanhMucModels.DT_DanhMuc("CanBo_ThanhPhanBanThan", true, "--- Lựa chọn ---");
        SelectOptionList slMaTPBT = new SelectOptionList(dtTPBT, "iID_MaDanhMuc", "sTen");
        if (dtTPBT != null) dtTPBT.Dispose();


        var dtTDChiHuyQS = DanhMucModels.DT_DanhMuc("CanBo_TrinhDoChiHuyQuanSu", true, "--- Lựa chọn ---");
        SelectOptionList slMaTrinhDoChiHuyQuanSu = new SelectOptionList(dtTDChiHuyQS, "iID_MaDanhMuc", "sTen");
        if (dtTDChiHuyQS != null) dtTDChiHuyQS.Dispose();

        DataTable dtLyDoTangGiam = null, dtQuanNhan = null, dtBacLuong = null, dtHuyen = null, dtXaphuong = null;
        SelectOptionList slMaLyDoTangGiam = null, slMaQuanNhan = null, slBacLuong = null, slMaHuyen = null, slMaXaPhuong = null;


        ///chi tiết cán bộ
        string iID_MaCanBo = Convert.ToString(Request.QueryString["iID_MaCanBo"]);
        if (String.IsNullOrEmpty(iID_MaCanBo) == false && iID_MaCanBo != "")
        {
            var dtCanBo = CanBo_HoSoNhanSuModels.GetChiTiet(iID_MaCanBo);
            if (dtCanBo.Rows.Count > 0)
            {
                DataRow DR = dtCanBo.Rows[0];
                sHoDem = HamChung.ConvertToString(DR["sHoDem"]);
                sTen = HamChung.ConvertToString(DR["sTen"]);
                sHoTen = HamChung.ConvertToString(DR["sHoDem"]) + " " + HamChung.ConvertToString(DR["sTen"]);
                if (HamChung.ConvertDateTime(DR["dNgayTuyenDung"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayTuyenDung = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayTuyenDung"]));
                if (HamChung.ConvertDateTime(DR["dNgayVaoCQ"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayVaoCQ = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayVaoCQ"]));
                iID_MaTinh = HamChung.ConvertToString(DR["iID_MaTinh"]);

                //huyện
                dtHuyen = CanBo_DanhMucNhanSuModels.getHuyen(HamChung.ConvertToString(DR["iID_MaTinh"]), true, "--- Lựa chọn ---");
                slMaHuyen = new SelectOptionList(dtHuyen, "iID_MaHuyen", "sTenHuyen");



                iID_MaHuyen = HamChung.ConvertToString(DR["iID_MaHuyen"]);
                //xa phuong
                dtXaphuong = CanBo_DanhMucNhanSuModels.getXaPhuong(HamChung.ConvertToString(DR["iID_MaHuyen"]), true, "--- Lựa chọn ---");
                slMaXaPhuong = new SelectOptionList(dtXaphuong, "iID_MaXaPhuong", "sTenXaPhuong");
                iID_MaXaPhuong = HamChung.ConvertToString(DR["iID_MaXaPhuong"]);
                sSoHieuCBCC = HamChung.ConvertToString(DR["sSoHieuCBCC"]);
                // iID_MaDoiTuong = HamChung.ConvertToString(DR["iID_MaDoiTuong"]);
                sTenGoiKhac = HamChung.ConvertToString(DR["sTenGoiKhac"]);

                sSoSoLuong = HamChung.ConvertToString(DR["sSoSoLuong"]);
                sSoTaiKhoan = HamChung.ConvertToString(DR["sSoTaiKhoan"]);
                sNganHang = HamChung.ConvertToString(DR["sNganHang"]);
                if (HamChung.ConvertDateTime(DR["dNgaySinh"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgaySinh = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgaySinh"]));
                string gtinh = HamChung.ConvertToString(DR["bGioiTinh"]);
                bGioiTinh = gtinh;///
                if (gtinh == "1") dtQuanNhan = CanBo_DanhMucNhanSuModels.getNuQuanNhan(true, "--- Lựa chọn ---", 0);
                else dtQuanNhan = CanBo_DanhMucNhanSuModels.getNuQuanNhan(true, "--- Lựa chọn ---", 1);
                slMaQuanNhan = new SelectOptionList(dtQuanNhan, "iID_Ma", "sTen");

                iID_ChucVuHienTai = HamChung.ConvertToString(DR["sID_ChucVuHienTai"]);
                //iID_ChucVuKiemNhiem = "", iID_MaCapBac = "",
                sHoKhauThuongTru = HamChung.ConvertToString(DR["sHoKhauThuongTru"]);
                sNoiSinh = HamChung.ConvertToString(DR["sNoiSinh"]);
                sDiaChi = HamChung.ConvertToString(DR["sDiaChi"]);
                sEmail = HamChung.ConvertToString(DR["sEmail"]);
                sWebsite = HamChung.ConvertToString(DR["sWebsite"]);
                sSoDienThoai = HamChung.ConvertToString(DR["sSoDienThoai"]);
                sTinhTrangSucKhoe = HamChung.ConvertToString(DR["sTinhTrangSucKhoe"]);
                rLuongCoBan_HeSo = HamChung.ConvertToString(DR["rLuongCoBan_HeSo"]);
                String ChieuCao = HamChung.ConvertToString(DR["rChieuCao"]);
                if (ChieuCao != "0")
                    rChieuCao = ChieuCao;
                String CanNang = HamChung.ConvertToString(DR["rCanNang"]);
                if (CanNang != "0")
                    rCanNang = CanNang;
                sLaThuongBinhHang = HamChung.ConvertToString(DR["sLaThuongBinhHang"]);
                iID_MaGiaDinhChinhSach = HamChung.ConvertToString(DR["iID_MaGiaDinhChinhSach"]);
                iID_MaNhomMau = HamChung.ConvertToString(DR["iID_MaNhomMau"]);

                iID_MaPhongBan = HamChung.ConvertToString(DR["iID_MaDonVi"]);
                iID_MaCheDoHuongLuong = HamChung.ConvertToString(DR["iID_MaCheDoHuongLuong"]);
                iID_MaDanToc = HamChung.ConvertToString(DR["iID_MaDanToc"]);
                iID_MaTonGiao = HamChung.ConvertToString(DR["iID_MaTonGiao"]);
                iID_MaThanhPhanGD = HamChung.ConvertToString(DR["iID_MaThanhPhanGD"]);
                iID_MaTinhTrangHonNhan = HamChung.ConvertToString(DR["iID_MaTinhTrangHonNhan"]);
                iID_MaTrinhDoVanHoa = HamChung.ConvertToString(DR["iID_MaTrinhDoVanHoa"]);
                iID_MaTrinhDoQuanLyNN = HamChung.ConvertToString(DR["iID_MaTrinhDoQuanLyNN"]);
                iID_MaHocHam = HamChung.ConvertToString(DR["iID_MaHocHam"]);
                iID_MaTrinhDoLyLuanChinhTri = HamChung.ConvertToString(DR["iID_MaTrinhDoLyLuanChinhTri"]);
                iID_MaTrinhDoTinHoc = HamChung.ConvertToString(DR["iID_MaTrinhDoTinHoc"]);
                iID_MaTrinhDoChuyenMonCaoNhat = HamChung.ConvertToString(DR["sID_MaTrinhDoChuyenMonCaoNhat"]);
                sNoiDaoTao = HamChung.ConvertToString(DR["sNoiDaoTao"]);
                iID_MaChuyenNganh = HamChung.ConvertToString(DR["iID_MaChuyenNganh"]);

                //tăng giảm
                iID_MaTinhTrangCanBo = HamChung.ConvertToString(DR["iID_MaTinhTrangCanBo"]);
                dtLyDoTangGiam = CanBo_DanhMucNhanSuModels.getLyDoTangGiam(true, "--- Lựa chọn ---", HamChung.ConvertToString(DR["iID_MaTinhTrangCanBo"]));
                slMaLyDoTangGiam = new SelectOptionList(dtLyDoTangGiam, "iID_MaLyDoTangGiam", "sTen");
                iID_MaLyDoTangGiam = HamChung.ConvertToString(DR["iID_MaLyDoTangGiam"]);

                sNgoaiNgu = HamChung.ConvertToString(DR["sNgoaiNgu"]);
                sCongViecChinhDangLam = HamChung.ConvertToString(DR["sCongViecChinhDangLam"]);
                sCMND = HamChung.ConvertToString(DR["sCMND"]);
                if (HamChung.ConvertDateTime(DR["dNgayCap"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayCap = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayCap"]));
                sNoiCap = HamChung.ConvertToString(DR["sNoiCap"]);
                sSoHoChieu = HamChung.ConvertToString(DR["sSoHoChieu"]);
                if (HamChung.ConvertDateTime(DR["dNgayCapHoChieu"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayCapHoChieu = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayCapHoChieu"]));
                if (HamChung.ConvertDateTime(DR["dHanSuDungHoChieu"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dHanSuDungHoChieu = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dHanSuDungHoChieu"]));
                iID_MaLoaiHoChieu = HamChung.ConvertToString(DR["iID_MaLoaiHoChieu"]);
                sNgheNghiepTruocTuyenDung = HamChung.ConvertToString(DR["sNgheNghiepTruocTuyenDung"]);
                sVaoCQODau = HamChung.ConvertToString(DR["sVaoCQODau"]);
                if (HamChung.ConvertDateTime(DR["dNgayThamGiaCM"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayThamGiaCM = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayThamGiaCM"]));
                if (HamChung.ConvertDateTime(DR["dNgayVaoDang"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayVaoDang = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayVaoDang"]));
                if (HamChung.ConvertDateTime(DR["dNgayChinhThuc"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayChinhThuc = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayChinhThuc"]));
                if (HamChung.ConvertDateTime(DR["dNgayThamGiaToChucCTXH"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayThamGiaToChucCTXH = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayThamGiaToChucCTXH"]));

                iID_MaChucVu = HamChung.ConvertToString(DR["iID_MaChucVu"]);
                if (HamChung.ConvertDateTime(DR["dNgayNhapNgu"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayNhapNgu = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayNhapNgu"]));
                if (HamChung.ConvertDateTime(DR["dNgayXuatNgu"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayXuatNgu = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayXuatNgu"]));
                if (HamChung.ConvertDateTime(DR["dNgayTaiNgu"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayTaiNgu = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayTaiNgu"]));
                iID_MaQuanHam = HamChung.ConvertToString(DR["iID_MaQuanHam"]);
                String NamPhongQuanHam = HamChung.ConvertToString(DR["iNamPhongQuanHam"]);
                if (iNamPhongQuanHam != "0")
                    iNamPhongQuanHam = NamPhongQuanHam;
                iID_MaNgachLuong = HamChung.ConvertToString(DR["iID_MaNgachLuong"]);
                //bac luong
                dtBacLuong = CanBo_DanhMucNhanSuModels.getBacLuong(HamChung.ConvertToString(DR["iID_MaNgachLuong"]), true, "--- Lựa chọn ---");
                slBacLuong = new SelectOptionList(dtBacLuong, "iID_MaBacLuong", "sTenBacLuong");

                iID_MaBacLuong = HamChung.ConvertToString(DR["iID_MaBacLuong"]);
                if (HamChung.ConvertDateTime(DR["dNgayHuongNgach"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayHuongNgach = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayHuongNgach"]));
                sMaSoHoSo = HamChung.ConvertToString(DR["sMaSoHoSo"]);
                sViTriLuuHoSo = HamChung.ConvertToString(DR["sViTriLuuHoSo"]);
                string Anh = HamChung.ConvertToString(DR["sAnh"]);
                if (Anh != "" && String.IsNullOrEmpty(Anh) == false) sImages = Anh;

                if (HamChung.ConvertDateTime(DR["dNgayVaoDoan"]).ToString("dd/MM/yyyy") != "01/01/0001")
                    dNgayVaoDoan = CommonFunction.LayXauNgay(HamChung.ConvertDateTime(DR["dNgayVaoDoan"]));
                sNoiVaoDoan = HamChung.ConvertToString(DR["sNoiVaoDoan"]);
                iID_MaTrinhDoChiHuyQuanSu = HamChung.ConvertToString(DR["iID_MaTrinhDoChiHuyQuanSu"]);
                sTiengDanToc = HamChung.ConvertToString(DR["sTiengDanToc"]);

                sHoTenThuongDung = HamChung.ConvertToString(DR["sHoTenThuongDung"]);
                sBiDanh = HamChung.ConvertToString(DR["sBiDanh"]);
                sBenhChinh = HamChung.ConvertToString(DR["sBenhChinh"]);
                iID_MaThanhPhanBanThan = HamChung.ConvertToString(DR["iID_MaThanhPhanBanThan"]);
                iID_MaHocVi = HamChung.ConvertToString(DR["iID_MaHocVi"]);

                String NamPhongHocVi = HamChung.ConvertToString(DR["iNamPhongHocVi"]);
                if (NamPhongHocVi != "0")
                    iNamPhongHocVi = NamPhongHocVi;

                String NamPhongHocHam = HamChung.ConvertToString(DR["iNamPhongHocHam"]);
                if (NamPhongHocHam != "0")
                    iNamPhongHocHam = NamPhongHocHam;

            }
            if (dtCanBo != null) dtCanBo.Dispose();
        }
        else
        {
            dtQuanNhan = CanBo_DanhMucNhanSuModels.getNuQuanNhan(true, "--- Lựa chọn ---", 0);
            slMaQuanNhan = new SelectOptionList(dtQuanNhan, "iID_Ma", "sTen");

            dtBacLuong = CanBo_DanhMucNhanSuModels.getBacLuong("", true, "--- Lựa chọn ---");
            slBacLuong = new SelectOptionList(dtBacLuong, "iID_MaBacLuong", "sTenBacLuong");


            dtHuyen = CanBo_DanhMucNhanSuModels.getHuyen("", true, "--- Lựa chọn ---");
            slMaHuyen = new SelectOptionList(dtHuyen, "iID_MaHuyen", "sTenHuyen");
            //xa phuong
            dtXaphuong = CanBo_DanhMucNhanSuModels.getXaPhuong("", true, "--- Lựa chọn ---");
            slMaXaPhuong = new SelectOptionList(dtXaphuong, "iID_MaXaPhuong", "sTenXaPhuong");
            //ly do tang giam
            dtLyDoTangGiam = CanBo_DanhMucNhanSuModels.getLyDoTangGiam(true, "--- Lựa chọn ---", "");
            slMaLyDoTangGiam = new SelectOptionList(dtLyDoTangGiam, "iID_MaLyDoTangGiam", "sTen");
        }

        if (dtLyDoTangGiam != null) dtLyDoTangGiam.Dispose();
        if (dtQuanNhan != null) dtQuanNhan.Dispose();
        if (dtBacLuong != null) dtBacLuong.Dispose();
        if (dtHuyen != null) dtHuyen.Dispose();
        if (dtXaphuong != null) dtXaphuong.Dispose();
        if (dtLyDoTangGiam != null) dtLyDoTangGiam.Dispose();
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <%=NgonNgu.LayXau("Liên kết nhanh: ")%>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "CanBo_HoSoNhanSu"), "Danh sách hồ sơ nhân sự")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>
                            <%
                                if (ViewData["DuLieuMoi"] == "1" && ViewData["DuLieuMoi"] != null)
                                {
                            %>
                            <%=NgonNgu.LayXau("Nhập thông tin cán bộ")%>
                            <%
                                }
                                else
                                {
                            %>
                            <%=NgonNgu.LayXau("Sửa thông tin cán bộ")%>
                            <%
                                }
                            %></span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <div style="width: 100%; float: left;">
                    <div style="width: 100%; float: left;">
                        <script type="text/javascript">
                            $(document).ready(function () {
                                $("#tabs").tabs();
                            });    
                        </script>
                        <div id="tabs">
                            <ul>
                                <li><a href="#tabs-1">Hồ sơ CBCC</a></li>
                                <%
                                    if (ViewData["DuLieuMoi"] != "1" && ViewData["DuLieuMoi"] != null)
                                    {
                                %>
                                <li><a href="#tabs-4">Lịch sử bản thân</a></li>
                                <li><a href="#tabs-9">Tình hình KT - CT</a></li>
                                <li><a href="#tabs-2">Đào tạo bồi dưỡng</a></li>
                                <li><a href="#tabs-3">Quá trình công tác</a></li>
                                <li><a href="#tabs-5">Khen thưởng</a></li>
                                <li><a href="#tabs-6">Kỷ luật</a></li>
                                <li><a href="#tabs-7">Người phụ thuộc</a></li>
                                <li><a href="#tabs-8">Đi nước ngoài</a></li>
                                <%
                                    }
                                %>
                            </ul>
                            <div id="tabs-1">
                                <%
                                    using (Html.BeginForm("EditSubmit", "CanBo_HoSoNhanSu", new { ParentID = ParentID, iID_MaCanBo = iID_MaCanBo }))
                                    {
                                %>
                                <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
                                <div style="width: 15%; float: left">
                                    <table width="100%" border="0">
                                        <tr style="width: 100%" align="center">
                                            <td style="width: 100%" align="center" colspan="2">
                                                <img src="<%= sImages %>" style="width: 120px; height: 150px; border: 1px Solid Silver" />
                                            </td>
                                        </tr>
                                        <tr style="width: 100%" align="center">
                                            <td style="width: 100%" align="center" colspan="2">
                                                <% =MyHtmlHelper.UploadImage("uploadFile", "Libraries/Files", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"))%>
                                                <%= Html.Hidden(ParentID + "_sFileName")%>
                                                <br />
                                                <%= MyHtmlHelper.TextBox(ParentID, Url, "sAnh", "", "readonly style=\"width:200px; display:none;\"")%>
                                                <script type="text/javascript">
                                                    uploadFile.addListener(uploadFile.UPLOAD_COMPLETE, uploadFile_OnComplete);
                                                    function uploadFile_OnComplete(FileName, url) {
                                                        document.getElementById("<%=ParentID%>_sFileName").value = FileName;
                                                        document.getElementById("<%=ParentID%>_sAnh").value = "../../../" + uploadFile.serverPath + "/" + url;

                                                    }
                                                </script>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 85%;">
                                    <table border="0" cellpadding="10" cellspacing="10" width="100%">
                                        <tr>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Họ và tên &nbsp;<span style="color: Red;">*</span>
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 45%;">
                                                                <%=MyHtmlHelper.TextBox(ParentID, sHoDem, "sHoDem", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='50' tab-index='-1'")%>
                                                                <br />
                                                                <%= Html.ValidationMessage(ParentID + "_" + "err_sHoTen")%>
                                                            </td>
                                                            <td style="width: 5%;">
                                                            </td>
                                                            <td style="width: 45%;">
                                                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='50'")%>
                                                                <br />
                                                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Tên gọi khác</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sTenGoiKhac, "sTenGoiKhac", "", "class=\"input1_2\" style=\"width:97%;\" maxlength='50'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày sinh</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgaySinh, "dNgaySinh", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Họ tên thường dùng
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sHoTenThuongDung, "sHoTenThuongDung", "", "class=\"input1_2\" style=\"width:97%;\" maxlength='50'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Bí danh</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sBiDanh, "sBiDanh", "", "class=\"input1_2\" style=\"width:97%;\" maxlength='50'")%>
                                                </div>
                                            </td>
                                          <td class="td_form2_td10">
                                                <div>
                                                    Giới tính&nbsp;<span style="color: Red;">*</span></div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slGioiTinh, bGioiTinh, "bGioiTinh", "", "class=\"input1_2\" style=\"with:100px;\"  onchange=\"ChonNuQuanNhan(this.value)\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td10" style="width: 15%">
                                                <div>
                                                    Số hiệu SQ/QN&nbsp;<span style="color: Red;">*</span></div>
                                            </td>
                                            <td style="width: 20%">
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sSoHieuCBCC, "sSoHieuCBCC", "", "class=\"input1_2\"  style=\"width:100%;\" maxlength='50'")%><br />
                                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sSoHieuCBCC")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10" style="width: 13%">
                                                <div>
                                                    Hưởng lương</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaHuongLuong, iID_MaCheDoHuongLuong, "iID_MaCheDoHuongLuong", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                              <td class="td_form2_td10">
                                                <div>
                                                    Nữ quân nhân</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaQuanNhan, iNuQuanNhan, "iNuQuanNhan", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                           
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Đối tượng&nbsp;<span style="color: Red;">*</span>
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaNghachLuong, iID_MaNgachLuong, "iID_MaNgachLuong", "", "class=\"input1_2\" style=\"with:100px;\" onchange=\"ChonBacLuong(this.value)\"")%>
                                                    <br />
                                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNgachLuong")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Bậc lương&nbsp;<span style="color: Red;">*</span></div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slBacLuong, iID_MaBacLuong, "iID_MaBacLuong", "", "class=\"input1_2\" style=\"with:100px;\" onchange=\"ChonNgachLuong(this.value)\"")%>
                                                    <br />
                                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaBacLuong")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Hệ số lương</div>
                                            </td>
                                            <td>
                                               <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, rLuongCoBan_HeSo, "rLuongCoBan_HeSo", "", "class=\"input1_2\" style=\"width:97%;\" maxlength='5'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                         <td class="td_form2_td10">
                                                <div>
                                                    Ngày hưởng bặc/ngạch</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayHuongNgach, "dNgayHuongNgach", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                             <td class="td_form2_td10">
                                                <div>
                                                    Nhóm máu
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaNhomMau, iID_MaNhomMau, "iID_MaNhomMau", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Bệnh chính
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sBenhChinh, "sBenhChinh", "", "class=\"input1_2\" style=\"width:99%;\"  maxlength='200'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Vào cơ quan, ở đâu
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sVaoCQODau, "sVaoCQODau", "", "class=\"input1_2\" style=\"width:99%;\" maxlength='250'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày tuyển dụng
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayTuyenDung, "dNgayTuyenDung", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày vào CQ hiện tại &nbsp;<span style="color: Red;">*</span>
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayVaoCQ, "dNgayVaoCQ", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayVaoCQ")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày nhập ngũ&nbsp;<span style="color: Red;">*</span>
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayNhapNgu, "dNgayNhapNgu", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayNhapNgu")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Xuất ngũ
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayXuatNgu, "dNgayXuatNgu", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày tái ngũ
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayTaiNgu, "dNgayTaiNgu", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Đơn vị công tác&nbsp;<span style="color: Red;">*</span></div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaDonVi, iID_MaPhongBan, "iID_MaDonVi", "", "class=\"input1_2\"")%>
                                                    <br />
                                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaPhongBan")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Tình trạng cán bộ&nbsp;<span style="color: Red;">*</span>
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTinhTrangCB, iID_MaTinhTrangCanBo, "iID_MaTinhTrangCanBo", "", "class=\"input1_2\" style=\"with:100px;\" onchange=\"ChonLyDoTangGiam(this.value)\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Lý do tăng giảm
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaLyDoTangGiam, iID_MaLyDoTangGiam, "iID_MaLyDoTangGiam", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo0">
                                            <td class="td_form2_td10" style="width: 15%">
                                                <div>
                                                    Quê quán (Tỉnh/Thành phố)
                                                </div>
                                            </td>
                                            <td style="width: 20%">
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTinh, iID_MaTinh, "iID_MaTinh", "", "class=\"input1_2\" onchange=\"ChonQuanHuyen(this.value)\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10" style="width: 13%">
                                                <div>
                                                    Quận/huyện</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaHuyen, iID_MaHuyen, "iID_MaHuyen", "", "class=\"input1_2\" onchange=\"ChonXaPhuong(this.value)\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Xã/phường</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaXaPhuong, iID_MaXaPhuong, "iID_MaXaPhuong", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo1">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Dân tộc
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaDanToc, iID_MaDanToc, "iID_MaDanToc", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Tôn giáo</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTonGiao, iID_MaTonGiao, "iID_MaTonGiao", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Thành phần gia đình</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaThanhPhanGD, iID_MaThanhPhanGD, "iID_MaThanhPhanGD", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo2">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Tình trạng hôn nhân
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTinhTrangHonNhan, iID_MaTinhTrangHonNhan, "iID_MaTinhTrangHonNhan", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Email</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sEmail, "sEmail", "", "class=\"input1_2\"  maxlength='150'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Điện thoại</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sSoDienThoai, "sSoDienThoai", "", "class=\"input1_2\"  maxlength='50'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo3">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Số sổ lương
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sSoSoLuong, "sSoSoLuong", "", "class=\"input1_2\"  maxlength='50'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Website</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sWebsite, "sWebsite", "", "class=\"input1_2\"  maxlength='50'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Thương binh hạng</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sLaThuongBinhHang, "sLaThuongBinhHang", "", "class=\"input1_2\"  maxlength='50'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo4">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Gia đình chính sách</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaGiaDinhChinhSach, iID_MaGiaDinhChinhSach, "iID_MaGiaDinhChinhSach", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Mã số hồ sơ
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sMaSoHoSo, "sMaSoHoSo", "", "class=\"input1_2\"  maxlength='50'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Vị trí lưu trữ hồ sơ</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sViTriLuuHoSo, "sViTriLuuHoSo", "", "class=\"input1_2\"  maxlength='250'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo5">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Tình trạng sức khỏe
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sTinhTrangSucKhoe, "sTinhTrangSucKhoe", "", "class=\"input1_2\"  maxlength='150'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Chiều cao (cm)</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, rChieuCao, "rChieuCao", "", "class=\"input1_2\" maxlength='3'", 1)%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Cân nặng</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, rCanNang, "rCanNang", "", "class=\"input1_2\" maxlength='3'", 1)%>
                                                </div>
                                            </td>
                                        </tr>
                                      
                                        <tr id="tb_CanBo6">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Hộ khẩu thường trú
                                                </div>
                                            </td>
                                            <td colspan="5">
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sHoKhauThuongTru, "sHoKhauThuongTru", "", "class=\"input1_2\" style=\"width:99%;\"  maxlength='4000'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo7">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Nơi sinh
                                                </div>
                                            </td>
                                            <td colspan="5">
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sNoiSinh, "sNoiSinh", "", "class=\"input1_2\" style=\"width:99%;\"  maxlength='4000'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo8">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Nơi ở hiện nay
                                                </div>
                                            </td>
                                            <td colspan="5">
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sDiaChi, "sDiaChi", "", "class=\"input1_2\" style=\"width:99%;\"  maxlength='4000'")%>
                                                    <%--<br />
                                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sDiaChi")%>--%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo9">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Số CMND
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sCMND, "sCMND", "", "class=\"input1_2\"  maxlength='50'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày cấp
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayCap, "dNgayCap", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Nơi cấp</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sNoiCap, "sNoiCap", "", "class=\"input1_2\" maxlength='250'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo11">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Số hộ chiếu
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sSoHoChieu, "sSoHoChieu", "", "class=\"input1_2\" maxlength='50'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày cấp
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayCapHoChieu, "dNgayCapHoChieu", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Hạn sử dụng
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dHanSuDungHoChieu, "dHanSuDungHoChieu", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo12">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Loại hộ chiếu
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaLoaiHoChieu, iID_MaLoaiHoChieu, "iID_MaLoaiHoChieu", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Chức vụ hiện tại
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaChucVu, iID_ChucVuHienTai, "sID_ChucVuHienTai", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Thành phần bản thân</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTPBT, iID_MaThanhPhanBanThan, "iID_MaThanhPhanBanThan", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo13">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Trình độ học vấn
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTrinhDoHocVan, iID_MaTrinhDoVanHoa, "iID_MaTrinhDoVanHoa", "", "class=\"input1_2\"")%>
                                                    <%-- <br />
                                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTrinhDoVanHoa")%>--%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Trình độ QL nhà nước
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTrinhDoQLNN, iID_MaTrinhDoQuanLyNN, "iID_MaTrinhDoQuanLyNN", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Học hàm
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaHocHam, iID_MaHocHam, "iID_MaHocHam", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo14">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Học vị</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaHocVi, iID_MaHocVi, "iID_MaHocVi", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Năm phong học vị</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, iNamPhongHocVi, "iNamPhongHocVi", "", "class=\"input1_2\" maxlength='4'", 1)%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Năm phong học hàm</div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, iNamPhongHocHam, "iNamPhongHocHam", "", "class=\"input1_2\" maxlength='4'", 1)%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo15">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Trình độ lý luận chính trị
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTrinhDoLyLuanChinhTri, iID_MaTrinhDoLyLuanChinhTri, "iID_MaTrinhDoLyLuanChinhTri", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Trình độ CM cao nhất
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTrinhDoChuyenMon, iID_MaTrinhDoChuyenMonCaoNhat, "sID_MaTrinhDoChuyenMonCaoNhat", "", "class=\"input1_2\"")%>
                                                    <%--  <br />
                                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTrinhDoChuyenMonCaoNhat")%>--%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Nơi đào tạo
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sNoiDaoTao, "sNoiDaoTao", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo16">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Chuyên ngành
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaChuyenNganh, iID_MaChuyenNganh, "iID_MaChuyenNganh", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Trình độ chỉ huy quân sự
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTrinhDoChiHuyQuanSu, iID_MaTrinhDoChiHuyQuanSu, "iID_MaTrinhDoChiHuyQuanSu", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                   Trình độ tin học
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaTinhTrangTinHoc, iID_MaTrinhDoTinHoc, "iID_MaTrinhDoTinHoc", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="Tr1">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngoại ngữ
                                                </div>
                                            </td>
                                            <td colspan="5">
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sNgoaiNgu, "sNgoaiNgu", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo17">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày vào Đoàn
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayVaoDoan, "dNgayVaoDoan", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Nơi vào đoàn
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sNoiVaoDoan, "sNoiVaoDoan", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Tiếng dân tộc
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sTiengDanToc, "sTiengDanToc", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo18">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày tham gia cách mạng
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayThamGiaCM, "dNgayThamGiaCM", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày vào ĐCSVN
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayVaoDang, "dNgayVaoDang", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày chính thức vào đảng
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayChinhThuc, "dNgayChinhThuc", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo19">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày tham gia các tổ chức CT-XH
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayThamGiaToChucCTXH, "dNgayThamGiaToChucCTXH", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Chức vụ tham gia các tổ chức CT-XH hiện tại
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaChucVu, iID_MaChucVu, "sID_MaChucVu", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Ngày chính thức
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayChinhThuc, "dNgayChinhThuc", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo20">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Quân hàm cao nhất
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slMaQuanHam, iID_MaQuanHam, "iID_MaQuanHam", "", "class=\"input1_2\"")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Năm phong quân hàm
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, iNamPhongQuanHam, "iNamPhongQuanHam", "", "class=\"input1_2\" style=\"width:50%\" maxlength='4'", 2)%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Công việc chính đang làm
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sCongViecChinhDangLam, "sCongViecChinhDangLam", "", "class=\"input1_2\" maxlength='250'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo21">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Nghề nghiệp bản thân trước khi được tuyển dụng
                                                </div>
                                            </td>
                                            <td colspan="5">
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sNgheNghiepTruocTuyenDung, "sNgheNghiepTruocTuyenDung", "", "class=\"input1_2\" style=\"width:99%;\" maxlength='250'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tb_CanBo10">
                                            <td class="td_form2_td10">
                                                <div>
                                                    Số tài khoản ngân hàng
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sSoTaiKhoan, "sSoTaiKhoan", "", "class=\"input1_2\" style=\"width:99%;\" maxlength='50'")%>
                                                </div>
                                            </td>
                                            <td class="td_form2_td10">
                                                <div>
                                                    Mở tại ngân hàng
                                                </div>
                                            </td>
                                            <td colspan="3">
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, sNganHang, "sNganHang", "", "class=\"input1_2\" style=\"width:99%;\" maxlength='250'")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr style="background-color: #dff0fb;cursor: pointer;">
                                            <td class="td_form2_td10">
                                                <div>
                                                    <b>
                                                        <%=MyHtmlHelper.TextBox("CauHinh", "Hiển thị tất cả", "HienThi", null, "class=\"input1_2\" style=\"border:none;text-align: center;background-color:white;font-weight:bold;color: #3b5998;\"")%></b></div>
                                            </td>
                                            <td colspan="5">
                                                <div style="cursor: pointer;">
                                                    <%=MyHtmlHelper.CheckBox(ParentID, "Hiển thị", "iDisplay", "", "onclick=\"CheckDisplay(this.checked)\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="6" style="height: 20px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="6" style="background-color: Window;">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="100px">
                                                         <%
                                                             if (ViewData["DuLieuMoi"] != "1" && ViewData["DuLieuMoi"] != null)
                                                             {
%>
                                                            <a href='<%=Url.Action("Index", "rptNhanSu_SoYeuLyLich", new {iID_MaCanBo = iID_MaCanBo})%>'
                                                                class="button" style="color: White;">In SYLL</a>
                                                                <%
                                                             }
%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="right" width="90px">
                                                            <input type="submit" class="button" id="Submit2" value="Lưu" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="center" width="100px">
                                                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%} %>
                            </div>
                            <%
                                if (ViewData["DuLieuMoi"] != "1" && ViewData["DuLieuMoi"] != null)
                                {
                            %>
                            <div id="tabs-2">
                                <%Html.RenderPartial("~/Views/CanBo/UserControls/CanBo_DaoTaoBoiDuong.ascx", new { ControlID = "DTBD", MaND = User.Identity.Name, iID_MaCanBo = iID_MaCanBo }); %>
                            </div>
                            <div id="tabs-3">
                                <%Html.RenderPartial("~/Views/CanBo/UserControls/CanBo_QuaTrinhCongTac.ascx", new { ControlID = "QTCT", MaND = User.Identity.Name, iID_MaCanBo = iID_MaCanBo }); %>
                            </div>
                            <div id="tabs-4">
                                <%Html.RenderPartial("~/Views/CanBo/UserControls/CanBo_LichSuBanThan.ascx", new { ControlID = "QTCT", MaND = User.Identity.Name, iID_MaCanBo = iID_MaCanBo }); %>
                            </div>
                            <div id="tabs-5">
                                <%Html.RenderPartial("~/Views/CanBo/UserControls/CanBo_KhenThuong.ascx", new { ControlID = "KhenThuong", MaND = User.Identity.Name, iID_MaCanBo = iID_MaCanBo }); %>
                            </div>
                            <div id="tabs-6">
                                <%Html.RenderPartial("~/Views/CanBo/UserControls/CanBo_KyLuat.ascx", new { ControlID = "KyLuat", MaND = User.Identity.Name, iID_MaCanBo = iID_MaCanBo }); %>
                            </div>
                            <div id="tabs-7">
                                <%Html.RenderPartial("~/Views/CanBo/UserControls/CanBo_NguoiPhuThuoc.ascx", new { ControlID = "PThuoc", MaND = User.Identity.Name, iID_MaCanBo = iID_MaCanBo }); %>
                            </div>
                            <div id="tabs-8">
                                <%Html.RenderPartial("~/Views/CanBo/UserControls/CanBo_DiNuocNgoai.ascx", new { ControlID = "DiNN", MaND = User.Identity.Name, iID_MaCanBo = iID_MaCanBo }); %>
                            </div>
                            <div id="tabs-9">
                                <%Html.RenderPartial("~/Views/CanBo/UserControls/CanBo_TinhHinhKinhTeChinhTri.ascx", new { ControlID = "KTCT", MaND = User.Identity.Name, iID_MaCanBo = iID_MaCanBo }); %>
                            </div>
                            <%
                                }
                            %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        CheckDisplay('False');
        function CheckDisplay(value) {
            for (var i = 0; i < 22; i++) {
                if (value == true || value == 'True') {
                    document.getElementById('tb_CanBo' + i).style.display = '';
                    document.getElementById('CauHinh_HienThi').value = 'Ẩn chi tiết';
                } else {
                    document.getElementById('tb_CanBo' + i).style.display = 'none';
                    document.getElementById('CauHinh_HienThi').value = 'Hiển thị chi tiết';
                }
            }
        }

        function ChonQuanHuyen(Id) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("getQuanHuyen?Id=#0", "CanBo_HoSoNhanSu")%>');
            url = unescape(url.replace("#0", Id));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_iID_MaHuyen").innerHTML = data;
            });
        }
        function ChonXaPhuong(Id) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("getXaPhuong?Id=#0", "CanBo_HoSoNhanSu")%>');
            url = unescape(url.replace("#0", Id));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_iID_MaXaPhuong").innerHTML = data;
            });
        }
        function ChonBacLuong(Id) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("getBacLuong?Id=#0", "CanBo_HoSoNhanSu")%>');
            url = unescape(url.replace("#0", Id));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_iID_MaBacLuong").innerHTML = data;
            });
        }
        function ChonNgachLuong(Id) {
            jQuery.ajaxSetup({ cache: false });
            var ngach = document.getElementById("<%= ParentID %>_iID_MaNgachLuong").value;
            var url = unescape('<%= Url.Action("getHeSoLuong?Id=#0&MaNgach=#1", "CanBo_HoSoNhanSu")%>');
            url = unescape(url.replace("#0", Id));
            url = unescape(url.replace("#1", ngach));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_rLuongCoBan_HeSo").innerHTML = data;
            });
        }
        function ChonLyDoTangGiam(Id) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("getLyDoTangGiam?Id=#0", "CanBo_HoSoNhanSu")%>');
            url = unescape(url.replace("#0", Id));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_iID_MaLyDoTangGiam").innerHTML = data;
            });
        }
        function ChonNuQuanNhan(Id) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("getGioiTinh?Id=#0", "CanBo_HoSoNhanSu")%>');
            url = unescape(url.replace("#0", Id));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_iNuQuanNhan").innerHTML = data;
            });
        }
    </script>
</asp:Content>
