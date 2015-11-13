<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String UserID = User.Identity.Name;
        String MaChungTu = Convert.ToString(ViewData["MaChungTu"]);
        String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);
        String iTapSo = Convert.ToString(ViewData["iTapSo"]);
        String iDenSoChungTu = Convert.ToString(ViewData["iDenSoChungTu"]);
        String iTuSoChungTu = Convert.ToString(ViewData["iTuSoChungTu"]);
        using (Html.BeginForm("EditSubmit_TapSo", "KeToanTongHop_ChungTu", new { ParentID = ParentID }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
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
                        <span>
                            <%
        if (ViewData["DuLieuMoi"] == "1")
        {
                            %>
                            <%=NgonNgu.LayXau("Nhập tập sổ")%>
                            <%
                    }
                    else
                    {
                            %>
                            <%=NgonNgu.LayXau("Tạo tập số")%>
                            <%
                    }
                            %>&nbsp; &nbsp; </span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <div style="width: 60%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    <b>Tập số</b> <span style="color: red;">(*)</span></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, iTapSo, "iTapSo", "", "class=\"input1_2\"",2)%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iTapSo")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    <b>Từ số chứng từ</b><span style="color: red;">(*)</span></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, iTuSoChungTu, "iTuSoChungTu", "", "class=\"input1_2\"", 1)%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iTuSoChungTu")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    <b>Đến số chứng từ</b><span style="color: red;">(*)</span></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, iDenSoChungTu, "iDenSoChungTu", "", "class=\"input1_2\"", 1)%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iDenSoChungTu")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td width="65%" class="td_form2_td5">
                                                &nbsp;
                                            </td>
                                            <td width="30%" align="right" class="td_form2_td5">
                                                <input type="submit" class="button" id="btnLuu" value="Lưu" />
                                            </td>
                                            <td width="5px">
                                                &nbsp;
                                            </td>
                                            <td class="td_form2_td5">
                                                <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%
    }       
    %>
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
    <div id="dvText" class="popup_block">
        <img src="../../../Content/ajax-loader.gif" /><br />
        <p>
            Hệ thống đang thực hiện yêu cầu...</p>
    </div>
</asp:Content>
