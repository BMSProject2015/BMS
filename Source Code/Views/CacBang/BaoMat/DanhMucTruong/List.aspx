<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%Html.RenderPartial("~/Views/CacBang/BaoMat/DanhMucTruong/Partial_List.aspx", new PartialModel("Partial_List", (Dictionary<string, object>)ViewData["PQ_DanhMucTruong_dicData"])); %>
</asp:Content>