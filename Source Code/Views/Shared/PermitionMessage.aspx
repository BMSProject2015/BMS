<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="DomainModel.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div class="pagedingchuan">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td style="width: 100%">
                    <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                        <b><%=NgonNgu.LayXau("Thông báo quyền truy cập")%></b>
                    </div>         
                </td>
            </tr>
        </table>
    </div>
    <table cellpadding="0"  cellspacing="0" border="0" class="table_form3" >
        <tr>
            <td width="100%" align="center" style="text-align: center; padding-top: 20px; padding-bottom: 20px;">
                <h3><%=NgonNgu.LayXau("Bạn không được phép sử dụng chức năng này")%></h3><br />
                <%=Html.ActionLink(NgonNgu.LayXau("Trang chủ"),"Index","Home") %>
            </td>
        </tr>
    </table>
</asp:Content>
