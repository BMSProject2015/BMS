<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "MucLuc_DoiTuong";
    String MaND = User.Identity.Name;
    String iNamLamViecCH = NguoiDungCauHinhModels.iNamLamViec.ToString();
    DataTable dt = DanhMucThuBaoHiemModels.GET_DanhSachThuBaoHiem(iNamLamViecCH);
    String strThemMoi = Url.Action("Edit", "BaoHiem_DanhMucThuBaoHiem");
     
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "MucLucDoiTuong"), "Danh sách đối tượng")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Mục lục ngân sách đối tượng</span>
                </td>
                <td align="right">
                    <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                   
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 20%;" align="center">Tháng</th>
            <th style="width: 50%;" align="center">Năm làm việc</th>
            <th style="width: 5%;" align="center">Chi tiết</th>
        </tr>
        <%
            DataRow R;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                R = dt.Rows[i];
                String iThang = Convert.ToString(R["iThang"]);
                String iNamLamViec = Convert.ToString(R["iNamLamViec"]);
                String strURL = MyHtmlHelper.ActionLink(Url.Action("Detail", "BaoHiem_DanhMucThuBaoHiem", new { iNamLamViec = iNamLamViec,iThang=iThang }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết\"");
                %>
                <tr>
                    <td><%=iThang%></td>
                    <td><%=iNamLamViec%></td>
                    <td>  <%=strURL %></td>
                </tr>
    <%} %>
    </table>
</div>
</asp:Content>




