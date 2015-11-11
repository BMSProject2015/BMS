<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%
    String MaND = User.Identity.Name;
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    if (ViewData["Saved"] == "1")
    {
        %>
        <script type="text/javascript">
            $(document).ready(function () {
                parent.ChungTuChiTiet_Saved();
            });
        </script>
        <%
    }
    if (ViewData["KhongThemDuoc"] == "1")
    {
        %>
        <script type="text/javascript">
            $(document).ready(function () {
                parent.alert("Không thêm được chứng từ ghi sổ.");
            });
        </script>
        <%
    }
    else
    {
        %>
        <script src="<%= Url.Content("~/Scripts/KeToanCongSan/jsBang_KeToanCongSan_ChungTuChiTiet.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
        <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
        <script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
        <script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
        <script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script> 
        <%Html.RenderPartial("~/Views/CongSan/ChungTu/KeToanCongSan_ChungTuChiTiet_Frame_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name, iID_MaChungTu = iID_MaChungTu }); %>
        <%
    }
%>
</asp:Content>
