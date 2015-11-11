<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        #menu1 ul
        {
            background: url("images/ui-bg_gloss-wave_55_5c9ccc_500x100.png") repeat-x scroll 50% 50% #5c9ccc;
            border: 1px solid #4297d7;
            color: #fff;
            border-radius: 5px;
            font-weight: bold;
            margin: 0;
            padding: 0em 0.2em 0;
        }
        
        #menu1 ul li
        {
            float: left;
            padding: 4px 2em;
            text-decoration: none;
            cursor: pointer;
            display: inline;
        }
        
        #menu1 ul a
        {
        }
    </style>
    <style>
        .span_required
        {
            color: red;
        }
        #tbl_ttChung td
        {
            padding: 3px;
        }
    </style>
    <%
        // lay du lieu
        string ParentID = "DoanhNghiep";
        string iID_MaDoanhNghiep = string.Empty;
        string sTenDoanhNghiep = string.Empty;
        string sTenThuongGoi = string.Empty;
        string sTenVietTat = string.Empty;
        string iID_MaLoaiHinhDoanhNghiep = string.Empty;
        string iID_MaHinhThucHoatDong = string.Empty;
        string sTenGiaoDich = string.Empty;
        string sTruSoDongQuan = string.Empty;
        string iID_MaKhoi = string.Empty;
        string iID_MaNhom = string.Empty;
        string iID_MaDonVi = string.Empty;

        string sHoTenChuTich = string.Empty;
        string sHoTenGiamDoc = string.Empty;
        string sHoTenKeToan = string.Empty;
        string sHoTenTruongBanKiemSoat = string.Empty;

        string sCapBacChuTich = string.Empty;
        string sCapBacGiamDoc = string.Empty;
        string sCapBacKeToan = string.Empty;
        string sCapBacTruongBanKiemSoat = string.Empty;

        string sDTChuTich = string.Empty;
        string sDTGiamDoc = string.Empty;
        string sDTKeToan = string.Empty;
        string sDTTruongBanKiemSoat = string.Empty;

        string sDDChuTich = string.Empty;
        string sDDGiamDoc = string.Empty;
        string sDDKeToan = string.Empty;
        string sDDTruongBanKiemSoat = string.Empty;

        double rTongSoVon_DieuLe = 0;
        double rTyLeVonDieuLe = 0;
        string sThongTinKhac = string.Empty;
        iID_MaDoanhNghiep = Convert.ToString(Request.QueryString["iID_MaDoanhNghiep"]);
        //Đầu mối quản lý
        var dtDauMoi = DonViModels.DanhSach_DonVi(User.Identity.Name);
        var slDauMoi = new SelectOptionList(dtDauMoi, "iID_MaDonVi", "TenHT");
        // Loại hình doanh nghiệp
        var dtLoaiHinhDoanhNghiep = DanhMucModels.DT_DanhMuc("TCDN_LoaiHinhDN", false, "Chọn loại hình doanh nghiệp");
        var slLoaiHinhDoanhNghiep = new SelectOptionList(dtLoaiHinhDoanhNghiep, "iID_MaDanhMuc", "sTen");
        // Hình thức hoạt động
        var dtHinhThucHoatDong = DanhMucModels.DT_DanhMuc("TCDN_HinhThucHoatDong", false, "Chọn hình thức hoạt động");
        var slHinhThucHoatDong = new SelectOptionList(dtHinhThucHoatDong, "iID_MaDanhMuc", "sTen");
        // Khối
        var dtKhoi = DanhMucModels.DT_DanhMuc("TCDN_Khoi", false, "Chọn khối doanh nghiệp");
        var slKhoi = new SelectOptionList(dtKhoi, "iID_MaDanhMuc", "sTen");
        // Nhóm
        var dtNhom = DanhMucModels.DT_DanhMuc("TCDN_NhomDN", false, "Chọn nhóm doanh nghiệp");
        var slNhom = new SelectOptionList(dtNhom, "iID_MaDanhMuc", "sTen");

        String iLoai = Convert.ToString(Request.QueryString["iLoai"]);
        if (String.IsNullOrEmpty(iLoai)) iLoai = "1";

        // Chi tiết doanh nghiệp
        String BackURL = Url.Action("Index", "TCDN_HoSo_DoanhNghiep");
        String InURL = "";
        InURL = String.Format(
                         @"/rptTCDN_BaoCaoChiTiet/viewpdf?iID_MaDoanhNghiep={0}&iLoai=0&MaND={1}",
                         iID_MaDoanhNghiep, User.Identity.Name);
        if (!string.IsNullOrEmpty(iID_MaDoanhNghiep) && iID_MaDoanhNghiep != "")
        {
            var dtDoanhNghiep = TCDN_HoSo_DoanhNghepModels.GetChiTiet(iID_MaDoanhNghiep);
            if (dtDoanhNghiep != null && dtDoanhNghiep.Rows != null && dtDoanhNghiep.Rows.Count > 0)
            {
                DataRow dr = dtDoanhNghiep.Rows[0];
                sTenDoanhNghiep = HamChung.ConvertToString(dr["sTenDoanhNghiep"]);
                sTenThuongGoi = HamChung.ConvertToString(dr["sTenThuongGoi"]);
                sTenVietTat = HamChung.ConvertToString(dr["sTenVietTat"]);
                iID_MaLoaiHinhDoanhNghiep = HamChung.ConvertToString(dr["iID_MaLoaiHinhDoanhNghiep"]);
                sTenGiaoDich = HamChung.ConvertToString(dr["sTenGiaoDich"]);
                iID_MaHinhThucHoatDong = HamChung.ConvertToString(dr["iID_MaHinhThucHoatDong"]);
                sTruSoDongQuan = HamChung.ConvertToString(dr["sTruSoDongQuan"]);
                iID_MaKhoi = HamChung.ConvertToString(dr["iID_MaKhoi"]);
                iID_MaNhom = HamChung.ConvertToString(dr["iID_MaNhom"]);
                rTongSoVon_DieuLe = HamChung.ConvertToDouble(dr["rTongSoVon_DieuLe"]);
                rTyLeVonDieuLe = HamChung.ConvertToDouble(dr["rTyLeVonDieuLe"]);

                iID_MaDonVi = HamChung.ConvertToString(dr["iID_MaDonVi"]);

                sHoTenChuTich = HamChung.ConvertToString(dr["sHoTenChuTich"]);
                sHoTenGiamDoc = HamChung.ConvertToString(dr["sHoTenGiamDoc"]);
                sHoTenKeToan = HamChung.ConvertToString(dr["sHoTenKeToan"]);
                sHoTenTruongBanKiemSoat = HamChung.ConvertToString(dr["sHoTenTruongBanKiemSoat"]);

                sCapBacChuTich = HamChung.ConvertToString(dr["sCapBacChuTich"]);
                sCapBacGiamDoc = HamChung.ConvertToString(dr["sCapBacGiamDoc"]);
                sCapBacKeToan = HamChung.ConvertToString(dr["sCapBacKeToan"]);
                sCapBacTruongBanKiemSoat = HamChung.ConvertToString(dr["sCapBacTruongBanKiemSoat"]);

                sDTChuTich = HamChung.ConvertToString(dr["sDTChuTich"]);
                sDTGiamDoc = HamChung.ConvertToString(dr["sDTGiamDoc"]);
                sDTKeToan = HamChung.ConvertToString(dr["sDTKeToan"]);
                sDTTruongBanKiemSoat = HamChung.ConvertToString(dr["sDTTruongBanKiemSoat"]);

                sDDChuTich = HamChung.ConvertToString(dr["sDDChuTich"]);
                sDDGiamDoc = HamChung.ConvertToString(dr["sDDGiamDoc"]);
                sDDKeToan = HamChung.ConvertToString(dr["sDDKeToan"]);
                sDDTruongBanKiemSoat = HamChung.ConvertToString(dr["sDDTruongBanKiemSoat"]);
                sThongTinKhac = HamChung.ConvertToString(dr["sThongTinKhac"]);
            }

        }
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_HoSo_DoanhNghiep"), "Danh sách hồ sơ Doanh nghiệp")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div id="nhapform">
            <div id="form2">
                <div style="width: 100%; float: left;">
                    <div id="menu1">
                        <%=TCDN_ChungTuChiTietModels.GetMenuHoSoDN(iID_MaDoanhNghiep, iLoai, ViewData["DuLieuMoi"].ToString())%>
                    </div>
                    <div style="width: 100%; float: left;">
                        <%
                            using (Html.BeginForm("EditSubmit", "TCDN_HoSo_DoanhNghiep", new { ParentId = ParentID, iID_MaDoanhNghiep = iID_MaDoanhNghiep }))
                            {
                        %>
                        <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
                        <div>
                            <table style="width: 100%" class="tbl_form" id="tbl_ttChung">
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%">
                                        Tên doanh nghiệp <span class="span_required">*</span>
                                    </td>
                                    <td class="td_form2_td1" style="width: 35%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sTenDoanhNghiep, "sTenDoanhNghiep", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='500' tab-index='-1'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sTenDoanhNghiep")%>
                                    </td>
                                    <td class="td_form2_td1" style="width: 15%">
                                        Loại hình doanh nghiệp
                                    </td>
                                    <td style="width: 35%">
                                        <%=MyHtmlHelper.DropDownList(ParentID, slLoaiHinhDoanhNghiep, iID_MaLoaiHinhDoanhNghiep, "iID_MaLoaiHinhDoanhNghiep", "", "class='input1_2' style='with:100px;'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaLoaiHinhDoanhNghiep")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        Tên thường gọi
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.TextBox(ParentID, sTenThuongGoi, "sTenThuongGoi", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='500' tab-index='-1'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sTenThuongGoi")%>
                                    </td>
                                    <td class="td_form2_td1">
                                        Hình thức hoạt động
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slHinhThucHoatDong, iID_MaHinhThucHoatDong, "iID_MaHinhThucHoatDong", "", "class='input1_2' style='with:100px;'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaHinhThucHoatDong")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        Tên giao dịch
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.TextBox(ParentID, sTenGiaoDich, "sTenGiaoDich", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='500' tab-index='-1'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sTenGiaoDich")%>
                                    </td>
                                    <td class="td_form2_td1">
                                        Trụ sở đóng quân
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.TextBox(ParentID, sTruSoDongQuan, "sTruSoDongQuan", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='500' tab-index='-1'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sTruSoDongQuan")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        Tên viết tắt
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.TextBox(ParentID, sTenVietTat, "sTenVietTat", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='500' tab-index='-1'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sTenVietTat")%>
                                    </td>
                                    <td class="td_form2_td1">
                                        <label style="text-decoration: underline">
                                            <b>Vốn điều lệ</b></label>
                                    </td>
                                    <td class="td_form2_td1">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        Đầu mối quản lý
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slDauMoi, iID_MaDonVi, "iID_MaDonVi", "", "class='input1_2' style='with:100px;'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaKhoi")%>
                                    </td>
                                    <td class="td_form2_td1">
                                        Tổng số vốn (tỷ đồng)
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.TextBox(ParentID, rTongSoVon_DieuLe, "rTongSoVon_DieuLe", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='500' tab-index='-1'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_rTongSoVon_DieuLe")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        Khối
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slKhoi, iID_MaKhoi, "iID_MaKhoi", "", "class='input1_2' style='with:100px;'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaKhoi")%>
                                    </td>
                                    <td class="td_form2_td1">
                                        Tỷ lệ vốn NN (%)
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.TextBox(ParentID, rTyLeVonDieuLe, "rTyLeVonDieuLe", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='500' tab-index='-1'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_rTyLeVonDieuLe")%>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        Nhóm
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slNhom, iID_MaNhom, "iID_MaNhom", "", "class='input1_2' style='with:100px;'")%>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNhom")%>
                                    </td>
                                    <td class="td_form2_td1">
                                    </td>
                                    <td class="td_form2_td1">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <label style="text-decoration: underline">
                                            <b>Chức danh quản lý</b></label>
                                    </td>
                                    <td class="td_form2_td1">
                                    </td>
                                    <td class="td_form2_td1">
                                    </td>
                                    <td class="td_form2_td1">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table class="mGrid" width="100%">
                                            <tr>
                                                <th style="width: 15%;" align="center">
                                                    Chức danh
                                                </th>
                                                <th style="width: 15%">
                                                    Họ và tên
                                                </th>
                                                <th style="width: 15%;" align="center">
                                                    Cấp bậc
                                                </th>
                                                <th style="width: 15%;" align="center">
                                                    Số điện thoại bàn
                                                </th>
                                                <th style="width: 15%;" align="center">
                                                    Số điện thoai di động
                                                </th>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%;" align="center">
                                                    <b>Chủ tịch (HĐQT, HĐTV, Công ty): </b>
                                                </td>
                                                <td style="width: 15%">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sHoTenChuTich, "sHoTenChuTich", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sCapBacChuTich, "sCapBacChuTich", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sDTChuTich, "sDTChuTich", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sDDChuTich, "sDDChuTich", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%;" align="center">
                                                    <b>Tổng giám đốc/giám đốc:</b>
                                                </td>
                                                <td style="width: 15%">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sHoTenGiamDoc, "sHoTenGiamDoc", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sCapBacGiamDoc, "sCapBacGiamDoc", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sDTGiamDoc, "sDTGiamDoc", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sDDGiamDoc, "sDDGiamDoc", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%;" align="center">
                                                    <b>Kế toán trưởng: </b>
                                                </td>
                                                <td style="width: 15%">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sHoTenKeToan, "sHoTenKeToan", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sCapBacKeToan, "sCapBacKeToan", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sDTKeToan, "sDTKeToan", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sDDKeToan, "sDDKeToan", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%;" align="center">
                                                    <b>Trưởng ban kiểm soát: </b>
                                                </td>
                                                <td style="width: 15%">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sHoTenTruongBanKiemSoat, "sHoTenTruongBanKiemSoat", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sCapBacTruongBanKiemSoat, "sCapBacTruongBanKiemSoat", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sDTTruongBanKiemSoat, "sDTTruongBanKiemSoat", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                                <td style="width: 15%;" align="center">
                                                    <%=MyHtmlHelper.TextBox(ParentID, sDDTruongBanKiemSoat, "sDDTruongBanKiemSoat", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        Các thông tin khác
                                    </td>
                                    <td colspan="3">
                                        <%=MyHtmlHelper.TextArea(ParentID, sThongTinKhac, "sThongTinKhac","", "class=\"input1_2\" style=\"width:100%; height:80px\"")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4" class="td_form2_td1">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td align="right" width="45%">
                                                    <input type="submit" class="button" id="Submit2" value="Lưu" />
                                                </td>
                                                <td width="2%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" width="8%">
                                                    <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="Huy()" />
                                                </td>
                                                <%if (ViewData["DuLieuMoi"] != "1")
                                                  {%>
                                                <td align="left">
                                                    <input class="button" type="button" value="<%= NgonNgu.LayXau("In hồ sơ") %>" onclick="In()" />
                                                </td>
                                                <% }
                                                  else
                                                  {%>
                                                <td align="left">
                                                </td>
                                                <%} %>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <%
                            }
                        %>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        function In() {

            window.open('<%=InURL%>', '_blank');
        }
    </script>
</asp:Content>
