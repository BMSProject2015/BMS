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
        string strPhaiLoadLai = "";
        if (Convert.ToString(ViewData["PhaiLoadLai"]) == "1")
        {
            strPhaiLoadLai = "true";
        }
        using (Html.BeginForm("TrichLuong_Submit", "Luong_BangLuongChiTiet", new { ParentID = ParentID, iID_MaBangLuong = iID_MaBangLuong }))
        {%>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="mGrid">
            <tr>
                    <td class="td_form2_td1" colspan="2">
                        <div style="font-weight:bold; text-align:left; font-size:13px;">
                            Chọn phương án trích lương:</div>
                    </td>
                   
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Số tiền trích lương, PC</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.Option(ParentID, "2", "2", "iTrichLuong_Loai", "", "onchange=Chon('2');")%>
                        </div>
                    </td>
                </tr>
                <tr id="tr_SoTien" style="display: none">
                    <td class="td_form2_td1">
                        <div>
                            Số tiền</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, "0", "rTrichLuong_SoLuong", "")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Một ngày lương tối thiểu</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.Option(ParentID, "0", "0", "iTrichLuong_Loai", "", "onchange=Chon('0');")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Một ngày lương cơ bản</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.Option(ParentID, "1", "1", "iTrichLuong_Loai", "", "onchange=Chon('1');")%>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <table border="0" cellpadding="0" cellspacing="0" style="text-align: center;" width="100%">
                <tr>
                    <td align="right">
                        <input type="submit" id="btnLuu" class="button4" value="Tr.lương" />
                    </td>
                    <td style="width: 20px;">
                        &nbsp;
                    </td>
                    <td align="left">
                        <input type="button" class="button4" value="Thoát" onclick="parent.jsLuong_Dialog_Close(<%=strPhaiLoadLai%>);" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%} %>
    <script type="text/javascript">
        function Chon(value) {
            if (value == 2) {
                document.getElementById('<%=ParentID %>_rTrichLuong_SoLuong').value = 0;
                document.getElementById('<%=ParentID %>_rTrichLuong_SoLuong_show').value = 0;
                document.getElementById('tr_SoTien').style.display = '';              
            }
            else {
                document.getElementById('tr_SoTien').style.display = 'none';
                document.getElementById('<%=ParentID %>_rTrichLuong_SoLuong').value = 1;
            }
        }

    </script>
    <script type="text/javascript">
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
    <%} %>
</asp:Content>
