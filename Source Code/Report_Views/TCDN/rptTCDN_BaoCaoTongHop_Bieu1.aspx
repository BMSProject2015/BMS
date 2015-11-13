<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "BaoCaoTongHop";
        String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);
        String iQuy = Convert.ToString(ViewData["iQuy"]);
        String btrongKy = Convert.ToString(ViewData["btrongKy"]);
        String iID_MaLoaiDoanhNghiep = Convert.ToString(ViewData["iID_MaLoaiDoanhNghiep"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String iID_MaNhom = Convert.ToString(ViewData["iID_MaNhom"]);
        String iID_MaKhoi = Convert.ToString(ViewData["iID_MaKhoi"]);
        String iID_MaHinhThucHoatDong = Convert.ToString(ViewData["iID_MaHinhThucHoatDong"]);
        String iID_MaLoaiHinhDoanhNghiep = Convert.ToString(ViewData["iID_MaLoaiHinhDoanhNghiep"]);
        String iLoaiBaoCao = Convert.ToString(ViewData["iLoaiBaoCao"]);
        //Đầu mối quản lý
        var dtDauMoi = DonViModels.DanhSach_DonVi(User.Identity.Name);
        var slDauMoi = new SelectOptionList(dtDauMoi, "iID_MaDonVi", "TenHT");
        DataRow dr = dtDauMoi.NewRow();
        dr["iID_MaDonVi"] = "";
        dr["TenHT"] = "--Chọn tất cả--";
        dtDauMoi.Rows.InsertAt(dr, 0);
        // Loại hình doanh nghiệp
        var dtLoaiHinhDoanhNghiep = DanhMucModels.DT_DanhMuc("TCDN_LoaiHinhDN", true, "--Chọn tất cả--");
        var slLoaiHinhDoanhNghiep = new SelectOptionList(dtLoaiHinhDoanhNghiep, "iID_MaDanhMuc", "sTen");
        dtLoaiHinhDoanhNghiep.Dispose();
        // Hình thức hoạt động
        var dtHinhThucHoatDong = DanhMucModels.DT_DanhMuc("TCDN_HinhThucHoatDong", true, "--Chọn tất cả--");
        var slHinhThucHoatDong = new SelectOptionList(dtHinhThucHoatDong, "iID_MaDanhMuc", "sTen");
        dtHinhThucHoatDong.Dispose();
        // Khối
        var dtKhoi = DanhMucModels.DT_DanhMuc("TCDN_Khoi", true, "--Chọn tất cả--");
        var slKhoi = new SelectOptionList(dtKhoi, "iID_MaDanhMuc", "sTen");
        dtKhoi.Dispose();
        // Nhóm
        var dtNhom = DanhMucModels.DT_DanhMuc("TCDN_NhomDN", true, "--Chọn tất cả--");
        var slNhom = new SelectOptionList(dtNhom, "iID_MaDanhMuc", "sTen");
        dtNhom.Dispose();
        DataTable dtLoaiHinh = DanhMucModels.DT_DanhMuc("TCDN_LoaiHinhDN", true, "--- Chọn loại hình doanh nghiệp ---");
        SelectOptionList slLoaiHinh = new SelectOptionList(dtLoaiHinh, "iID_MaDanhMuc", "sTen");
        dtLoaiHinh.Dispose();

        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();


        // Nhóm
        var dtLoaiBoaCao = TCDNModels.getdsLoaiBaoCaoTOngHop();
        var slLoaiBaoCao = new SelectOptionList(dtLoaiBoaCao, "iLoai", "sTen");
        dtNhom.Dispose();
        
        String BackURL = Url.Action("Index", "TCDN_Report", new { sLoai = "0" });
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptTCDN_BaoCaoTongHop_Bieu1", new { MaND = User.Identity.Name, iQuy = iQuy, bTrongKy = btrongKy, iID_MaDonVi = iID_MaDonVi, iID_MaKhoi = iID_MaKhoi, iID_MaNhom = iID_MaNhom, iID_MaLoaiHinhDoanhNghiep = iID_MaLoaiHinhDoanhNghiep, iID_MaHinhThucHoatDong = iID_MaHinhThucHoatDong,iLoaiBaoCao=iLoaiBaoCao });
        using (Html.BeginForm("EditSubmit", "rptTCDN_BaoCaoTongHop_Bieu1", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp chỉ tiêu năm</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="text-align: right; width: 25%">
                            <div>
                                <b>Chọn quý:</b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: left; width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: right; width: 10%">
                            <div>
                                <b>Trong kỳ:</b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: left; width: 20%">
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, btrongKy, "btrongKy", "", "class=\"input1_2\" style=\"width: 10%;height:15px;\"")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="text-align: right; width: 25%">
                            <div>
                                <b>Đơn vị đầu mối:</b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: left; width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDauMoi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: right; width: 10%">
                            <div>
                                <b>Khối:</b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: left; width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slKhoi, iID_MaKhoi, "iID_MaKhoi", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\"")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="text-align: right; width: 25%">
                            <div>
                                <b>Nhóm:</b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: left; width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNhom, iID_MaNhom, "iID_MaNhom", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: right; width: 10%">
                            <div>
                                <b>Hình thức hoạt động:</b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: left; width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slHinhThucHoatDong, iID_MaHinhThucHoatDong, "iID_MaHinhThucHoatDong", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\"")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="text-align: right; width: 25%">
                            <div>
                                <b>Chọn loại hình doanh nghiệp:</b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: left; width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiHinhDoanhNghiep, iID_MaLoaiHinhDoanhNghiep, "iID_MaLoaiHinhDoanhNghiep", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: right; width: 10%">
                            <div>
                             <b>Chọn loại báo cáo:</b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="text-align: left; width: 20%">
                            <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slLoaiBaoCao, iLoaiBaoCao, "iLoaiBaoCao", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\"")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="margin: 0 auto;" colspan="5">
                            <table cellpadding="0" cellspacing="0" border="0" style="margin-left: 500px;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%}
    %>
    <script type="text/javascript">
        $(function () {
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        }); 
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTCDN_BaoCaoTongHop_Bieu1", new { MaND = User.Identity.Name, iQuy = iQuy, bTrongKy = btrongKy, iID_MaDonVi = iID_MaDonVi, iID_MaKhoi = iID_MaKhoi, iID_MaNhom = iID_MaNhom, iID_MaLoaiHinhDoanhNghiep = iID_MaLoaiHinhDoanhNghiep, iID_MaHinhThucHoatDong = iID_MaHinhThucHoatDong,iLoaiBaoCao=iLoaiBaoCao }), "Xuất ra Excel")%>
    <iframe src="<%=URLView %>" height="600px" width="100%"></iframe>
    
</body>
</html>
