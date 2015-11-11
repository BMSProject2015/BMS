<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXauKhongDauTiengViet("Loại danh mục") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%Html.RenderPartial("~/Views/CacBang/DanhMuc/LoaiDanhMuc/Partial_Edit.aspx", new PartialModel("ctlLoaiDanhMuc_Edit", (Dictionary<string, object>)ViewData["DC_LoaiDanhMuc_dicData"])); %>
</asp:Content>
