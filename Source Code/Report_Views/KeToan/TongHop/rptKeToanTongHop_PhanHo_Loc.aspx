<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String MaND = User.Identity.Name;
        String iNamLamViec = Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
        String ParentID = "KeToan";
        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(User.Identity.Name);

        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
        String[] arrMaDonVi = iID_MaDonVi.Split(',');

        String iThang = Convert.ToString(ViewData["iThang"]);
        if (String.IsNullOrEmpty(iThang))
        {
            iThang = Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iThangLamViec"));
        }
        dtThang.Dispose();

        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        String urlReport = "";
        String ControllerName = "rptKeToanTongHop_PhanHo";
        String TK = Url.Action("Index", "rptKeToan_DanhMucTaiKhoan", new { sKyHieu = "39", ControllerName = ControllerName });
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptKeToanTongHop_PhanHo", new { iThang = iThang, iID_MaDonVi = iID_MaDonVi, UserName = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        VIETTEL.Report_Controllers.KeToan.TongHop.rptKeToanTongHop_PhanHoController ctlPhanHo = new VIETTEL.Report_Controllers.KeToan.TongHop.rptKeToanTongHop_PhanHoController();
        using (Html.BeginForm("EditSubmit_Loc", "rptKeToanTongHop_PhanHo", new { ParentID = ParentID }))
        { 
    %>
    <link href="../../../Content/Report_Style/ReportCSS.css" rel="StyleSheet" type="text/css" />
    <script src="../../../Scripts/Report/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/Report/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/Report/ShowDialog.js" type="text/javascript"></script>
    <style type="text/css">
        fieldset
        {
            padding: 3px;
            border: 1px solid #dedede;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            margin-bottom: 3px;
        }
        fieldset legend
        {
            padding: 3px;
            font-size: 14px;
            font-family: Tahoma Arial;
        }
        /*fieldset p{padding:2px;}
        fieldset p span{padding:2px; font-size:13.5px;}
        div#td_TaiKhoan.mGrid tr:even{background-color:#dedede;}*/
    </style>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 120px;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: Gray; text-transform: uppercase; font-weight: bold;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo phân hộ (Nhập phương án tài khoản theo ký hiệu 39)</span>
                    </td>
                </tr>
            </table>
        </div>
        <!--End .title_tong-->
        <div id="confirmBox" title="Báo cáo phân hộ (Nhập phương án tài khoản theo ký hiệu 39)"
            style="width: 600px; min-height: 200px;">
            <fieldset>
                <div id="idleft" style="width: 48%; padding: 2px; float: left; text-align: right;">
                    <span style="float: left; font-size: 13px; width: 30%; text-align: right;"><b>
                        <%=NgonNgu.LayXau("Tháng:") %></b> </span>
                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "onchange=ChonThang(this.value) class=\"input1_2\" style=\"width: 60px;\" ")%>
                    <span style="font-size: 13px; text-align: center;"><b>
                        <%=NgonNgu.LayXau("Năm làm việc:")%></b>
                        <%=MyHtmlHelper.TextBox(ParentID, iNamLamViec, "", "", "class=\"input1_2\" style=\"width: 50px; \" disabled=\"disabled\"")%>
                        <%--    <%=iNamLamViec %>--%>
                    </span>
                </div>
                <div id="idright" style="width: 48%; padding: 2px; float: right; text-align: right;">
                    <span style="float: left; font-size: 13px;  text-align: right;"><b>
                        <%=NgonNgu.LayXau("Trạng thái:") %></b> </span>
                    <div style="float: right;">
                        <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "onchange=ChonThang(this.value) class=\"input1_2\" style=\"width: 120px;\" ")%>
                        <input class="button" id="Submit1" onclick='TaiKhoan()' value="<%=NgonNgu.LayXau("Thêm TK")%>"
                            style="display: inline-block; margin-right: 5px;" />
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <legend><b>
                    <%=NgonNgu.LayXau("Chọn đơn vị")%></b></legend>
                <div id="divDonVi">
                    <%=ctlPhanHo.get_sDanhSachDonVi(iThang, iNamLamViec, MaND,iID_MaTrangThaiDuyet) %>
                </div>
            </fieldset>
            <p style="text-align: center; padding: 4px; clear: both;">
                <input type="submit" class="button" id="Submit2" onclick='clicks()' value="<%=NgonNgu.LayXau("Thực hiện")%>"
                    style="display: inline-block; margin-right: 5px;" /><input class="button" type="button"
                        value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" style="display: inline-block;
                        margin-left: 5px;" />
            </p>
        </div>
        <!--End #confirmBox-->
    </div>
    <!--End .box_tong-->
    <script type="text/javascript">
        function ChonThang(value) {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var iThangCT = document.getElementById("<%=ParentID %>_iThang").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ObjDanhSachDonVi?iThangCT=#0&iNamLamViec=#1&MaND=#2&iID_MaTrangThaiDuyet=#3", "rptKeToanTongHop_PhanHo") %>');
            url = unescape(url.replace("#0", iThangCT));
            url = unescape(url.replace("#1", '<%=iNamLamViec %>'));
            url = unescape(url.replace("#2", '<%=MaND %>'));
            url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));

            $.getJSON(url, function (data) {
                document.getElementById('divDonVi').innerHTML = data;
            });
        }
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        $(function () {
            ShowDialog(640);
        });

        function TaiKhoan() {
            window.location.href = '<%=TK %>';
        }
    </script>
    <%} %>
</asp:Content>
