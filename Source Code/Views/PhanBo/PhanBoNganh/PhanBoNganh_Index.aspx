<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Phân bổ ngành
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/jsPhanBo.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>" type="text/javascript"></script>

<%
    String iID_MaChiTieu = Request.QueryString["iID_MaChiTieu"];
%>

    <%Html.RenderPartial("~/Views/PhanBo/PhanBoNganh/PhanBoNganh_Index_DanhSach.ascx", new { ControlID = "PhanBoChiTiet", MaND = User.Identity.Name }); %>    
    <script type="text/javascript">
        $(document).ready(function () {
            jsPhanBo_Url_Frame = '<%=Url.Action("PhanBoNganhChiTiet_Frame", "PhanBo_PhanBoNganh", new { iID_MaChiTieu = iID_MaChiTieu })%>';
            jsPhanBo_Url = '<%=Url.Action("Index", "PhanBo_PhanBoNganh", new { iID_MaChiTieu = iID_MaChiTieu})%>';
        });
	</script>
</asp:Content>
