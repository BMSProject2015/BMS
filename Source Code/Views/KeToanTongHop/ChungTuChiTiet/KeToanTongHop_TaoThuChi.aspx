<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        DataTable dtNam = DanhMucModels.DT_Nam(false);
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();



        DataTable dtQuy = DanhMucModels.DT_Thang(false);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaThang", "TenThang");
        dtQuy.Dispose();
        String iNamLamViec = Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(UserID, "iNamLamViec"));
        int iThang = Convert.ToInt32(NguoiDungCauHinhModels.LayCauHinhChiTiet(UserID, "iThangLamViec"));
        String iQuy = "";
        if (String.IsNullOrEmpty(iQuy))
        {
            iQuy = Convert.ToString(iThang);
            //if (iThang > 0 && iThang <= 3)
            //{
            //    iQuy = "1";
            //}
            //else if (iThang > 3 && iThang <= 6)
            //{
            //    iQuy = "2";
            //}
            //else if (iThang > 6 && iThang <= 9)
            //{
            //    iQuy = "3";
            //}
            //else if (iThang > 9 && iThang <= 12)
            //{
            //    iQuy = "4";
            //}
            //else
            //{
            //    iQuy = "1";
            //}
        }
        String URL = Url.Action("Index", "KeToanTongHop_CanDoiThuChiTaiChinh");
        using (Html.BeginForm("EditSubmit_ThuChi", "KeToanTongHop_CanDoiThuChiTaiChinh", new { ParentID = ParentID }))
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
                            <%=NgonNgu.LayXau("Tạo nội dung thu chi từ quý - năm đã có")%>
                            <%
            }
            else
            {
                            %>
                            <%=NgonNgu.LayXau("Tạo nội dung thu chi từ quý - năm đã có")%>
                            <%
            }
                            %>&nbsp; &nbsp; </span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Chọn Tháng lấy số liệu thu/chi</b> <span style="color: red;">(*)</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, "", "iQuy_LaySL", "", "style=\"width:80px;\"")%>
                                  <%= Html.ValidationMessage(ParentID + "_" + "err_iQuy_LaySL")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Chọn năm lấy số liệu thu/chi</b><span style="color: red;">(*)</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNam_LaySL", "", " style=\"width:80px;\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Chọn Tháng đưa số liệu thu/chi</b> <span style="color: red;">(*)</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "style=\"width:80px;\"")%>

                                   <%= Html.ValidationMessage(ParentID + "_" + "err_iQuy")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Chọn năm đưa số liệu thu/chi</b><span style="color: red;">(*)</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNam", "", " style=\"width:80px;\"")%>
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
                                            <input class="button" type="button" value="Hủy" onclick="Huy()" />
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
    <%
        }       
    %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
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
    <div id="dvText" class="popup_block">
        <img src="../../../Content/ajax-loader.gif" /><br />
        <p>
            Hệ thống đang thực hiện yêu cầu...</p>
    </div>
</asp:Content>
