<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <%Html.RenderPartial("~/Views/CacBang/Menu/MenuItem/Partial_Detail.aspx", new PartialModel("ctlMenuItem_Detail", (Dictionary<string, object>)ViewData["MENU_MenuItem_dicData"])); %>
</asp:Content>
