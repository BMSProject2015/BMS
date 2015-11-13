<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="DomainModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Sort
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%Html.RenderPartial("~/Views/CacBang/DanhMuc/LoaiDanhMuc/Partial_Sort.aspx", new PartialModel("ctLoaiDanhMuc_List", (Dictionary<string, object>)ViewData["DC_LoaiDanhMuc_dicData"])); %>
</asp:Content>
