<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.CacBang" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
String MaLuat = Request.QueryString["MaLuat"];
BangMenuItem bang = new BangMenuItem();
int ThuTu = 0;
using (Html.BeginForm("MenuItem_CamSubmit", "MenuItem", new { MaLuat = MaLuat }))
{
%>    
<table class="mGrid">
    <tr>
        <th align="center"><%=NgonNgu.LayXau("Menu")%></th>
        <th width="300px" align="center"><%=NgonNgu.LayXau("Cấm xem")%></th>
    </tr>
    <%=bang.LayXauMenuItemCam(MaLuat, 0, 0, ref ThuTu)%>
</table>
<div class="cao5px">&nbsp;</div>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
	    <td width="70%">&nbsp;</td>
	    <td  width="30%" align="right">						
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td><input type="submit" type="button" class="button4" value="<%=NgonNgu.LayXau("Lưu")%>"/></td>
                    <td width="5px"></td>
                    <td><input type="button" value="<%=NgonNgu.LayXau("Hủy")%>" class="button4" onclick="javascript:history.go(-1);" /></td>
                </tr>
            </table>         
	    </td>
    </tr>
</table>
<div class="cao5px">&nbsp;</div>
<%} %>
</asp:Content>
