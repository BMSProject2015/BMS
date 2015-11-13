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
    int i;
    String ParentID = "KTCT_KhoBac_ChuongTrinhMucTieu";
    String page = Request.QueryString["page"];
    int CurrentPage = 1;        
    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = KTCT_KhoBac_ChuongTrinhMucTieuModels.Get_DanhSach(CurrentPage, Globals.PageSize);

    double nums = KTCT_KhoBac_ChuongTrinhMucTieuModels.Get_DanhSach_Count();
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {page  = x}));
    String strThemMoi = Url.Action("Edit", "KTCT_KhoBac_ChuongTrinhMucTieu");
    String strSapXep = Url.Action("Sort", "KTCT_KhoBac_ChuongTrinhMucTieu");  
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td style="width: 10%">
            <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-top: 5px; padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_KhoBac_ChuongTrinhMucTieu"), "Chương trình mục tiêu")%>
            </div>
        </td>
    </tr>
</table>
<%--<div id="divMenuLeft" style="width: 20%; float:left; position:relative;">
    <%Html.RenderPartial("~/Views/KeToanChiTiet/KhoBac/KTCT_KhoBac_Menu.ascx"); %>
</div>--%>
<div id="divNoiDung" style="width: 100%; float:left; position:relative;">
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách chương trình mục tiêu</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    <input id="Button2" type="button" class="button_title" value="Sắp xếp" onclick="javascript:location.href='<%=strSapXep %>'" />
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 17%;" align="center">Mã chương trình mục tiêu</th>
            <th style="width: 30%;" align="left">Tên chương trình mục tiêu</th>
            <th style="width: 40%;" align="left">Mô tả</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            if (i % 2 == 0)
            {
                classtr = "class=\"alt\"";
            }
            %>
            <tr <%=classtr %>>
                <td align="center"><%=STT%></td>     
                <td align="center"><%=dt.Rows[i]["iID_MaChuongTrinhMucTieu"]%></td>       
                <td align="left"><%=dt.Rows[i]["sTen"]%></td>
                <td align="left"><%=dt.Rows[i]["sMoTa"]%></td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "KTCT_KhoBac_ChuongTrinhMucTieu", new { iID_MaChuongTrinhMucTieu = R["iID_MaChuongTrinhMucTieu"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "KTCT_KhoBac_ChuongTrinhMucTieu", new { iID_MaChuongTrinhMucTieu = R["iID_MaChuongTrinhMucTieu"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="9" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
</div>
</asp:Content>
