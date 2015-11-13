<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%
        if (Request.QueryString["Saved"] == "1")
        {
    %>
    <script type="text/javascript">
        $(document).ready(function () {
            parent.jsLuong_Dialog_Close(true);
        });                                 
    </script>
    <%
        }
        else
        {
            String ParentID = "BangLuongChiTiet";
            String iID_MaBangLuong = Request.QueryString["iID_MaBangLuong"];
            DataTable dtDonVi = LuongModels.LayDanhSachDonViCuaBangLuong(iID_MaBangLuong, true, "Tất cả");

            SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");

            dtDonVi.Dispose();
            string strPhaiLoadLai = "";
            if (Convert.ToString(ViewData["PhaiLoadLai"]) == "1")
            {
                strPhaiLoadLai = "true";
            }
            using (Html.BeginForm("DieuChinhTienAn_Submit", "Luong_BangLuongChiTiet", new { ParentID = ParentID, iID_MaBangLuong = iID_MaBangLuong }))
            {
    %>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="mGrid">
             <tr>
                    <td class="td_form2_td1" colspan="2">
                        <div style="font-weight:bold; text-align:left; font-size:13px;">
                            Điều chỉnh tiền ăn một ngày của đơn vị:</div>
                    </td>
                   
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Đơn vị&nbsp;<span style="color:red">*</span></div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi", "", "onchange=ChonDonVi(this.value); style='width:100%;'")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
                        </div>
                    </td>
                </tr>
                <%--<tr id="tr_SoTien">
                                            <td class="td_form2_td1">
                                                <div>
                                                    Tiền ăn một ngày cũ</div>
                                            </td>
                                            <td class="td_form2_td5">
                                                <div>
                                                    <%=MyHtmlHelper.TextBox(ParentID, "0", "rTienAn1NgayCu", "")%>
                                                </div>
                                            </td>
                                        </tr>--%>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Tiền ăn một ngày mới&nbsp;<span style="color:red">*</span></div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, "0", "rTienAn1Ngay", "", "style='width:98%;' tab-index='-1'")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px; font-size: 5px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <br />
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
               
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td align="right" >
                        <input type="submit" id="btnLuu" class="button4" value="Thực hiện" />
                    </td>
                    <td style="width: 20px;">
                        &nbsp;
                    </td>
                    <td align="left" >
                        <input type="button" class="button4" value="Hủy" onclick="parent.jsLuong_Dialog_Close(<%=strPhaiLoadLai%>);" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%}%>
    <script type="text/javascript">

        function ChonDonVi(value) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("get_objTienAnCuaDonVi?iID_MaBangLuong=#0&iID_MaDonVi=#1","Luong_BangLuongChiTiet") %>');
            url = unescape(url.replace("#0", '<%=iID_MaBangLuong %>'));
            url = unescape(url.replace("#1", value));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_rTienAn1NgayCu_show").value = FormatNumber(data, 0);
            });
        }

        $(document).ready(function () {
            //Hide the div tag when page is loading
            $('#dvText').hide();

            //For Show the div or any HTML element
            $("#btnLuu").click(function () {
                $('#dvText').show();
                $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
                $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer               
            });

            //For hide the div or any HTML element
            $("#aHide").click(function () {
                $('#dvText').hide();
            });

            $(window).resize(function () {
                $('.popup_block').css({
                    position: 'absolute',
                    left: ($(window).width() - $('.popup_block').outerWidth()) / 2,
                    top: ($(window).height() - $('.popup_block').outerHeight()) / 2
                });
            });
            // To initially run the function:
            $(window).resize();
            //Fade in Background
        });                                 
    </script>
    <div id="idDialog" style="display: none;">
    </div>
    <div id="dvText" class="popup_block">
        <img src="../../../Content/ajax-loader.gif" /><br />
        <p>
            Hệ thống đang thực hiện yêu cầu...</p>
    </div>
    <%}%>
</asp:Content>
