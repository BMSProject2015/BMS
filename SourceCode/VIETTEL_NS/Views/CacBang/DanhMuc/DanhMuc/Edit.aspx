<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXauKhongDauTiengViet("Danh mục") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%Html.RenderPartial("~/Views/CacBang/DanhMuc/DanhMuc/Partial_Edit.aspx", new PartialModel("ctlDanhMuc_Edit", (Dictionary<string, object>)ViewData["DC_DanhMuc_dicData"])); %>
</asp:Content>
