<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách báo cáo nhân sự</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table class="mGrid">
                    <tr>
                        <th style="width: 3%;">
                            STT
                        </th>
                        <th style="width: 15%;">
                            Mã báo cáo - phụ lục
                        </th>
                        <th style="width: 60%;">
                            Tên báo cáo
                        </th>
                        <th style="width: 22%;">
                            Ghi chú
                        </th>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            1
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNhanSu_DanhSach"), "Danh sách các bộ")%>
                        </td>
                        <td>
                            Quang
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            2
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNhanSu_DanhSachNghiHuu"), "Danh sách các bộ hưu")%>
                        </td>
                        <td>
                            Quang
                        </td>
                    </tr>                    
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            3
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNhanSu_CapBac"), "Khai thác dữ liệu theo cấp bậc")%>
                        </td>
                        <td>
                            Quang
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            4
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNhanSu_DoTuoi"), "Khai thác dữ liệu theo độ tuổi")%>
                        </td>
                        <td>
                            Quang
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            5
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNhanSu_TrinhDo"), "Khai thác dữ liệu theo trình độ")%>
                        </td>
                        <td>
                            Quang
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            6
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNhanSu_ChucVu"), "Khai thác dữ liệu theo chức vụ")%>
                        </td>
                        <td>
                            Quang
                        </td>
                    </tr>
                    <tr class="alt">
                        <td align="center" style="padding: 3px 2px;">
                            7
                        </td>
                        <td align="center" style="padding: 3px 2px;">
                        </td>
                        <td style="padding: 3px 2px;">
                            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNhanSu_TangGiam"), "Danh sách cán bộ tài chính tăng, giảm")%>
                        </td>
                        <td>
                            Quang
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
