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
        String ParentID = "Index";
        String page = Request.QueryString["page"];
        int CurrentPage = 1;
        SqlCommand cmd;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        DataTable dt = Luong_DanhMucPhuCapModels.Get_dtDanhMucPhuCap(CurrentPage, Globals.PageSize);

        double nums = Luong_DanhMucPhuCapModels.Get_CountDanhMucPhuCap();
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        String strThemMoi = Url.Action("Edit", "Luong_DanhMucPhuCap");    
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 110px;">
                <div style="padding-left: 10px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237; float: left;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Luong_BangLuong"), "D.sách bảng lương")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách phụ cấp</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 7%;" align="center">
                    Mã Phụ Cấp
                </th>
                <th style="width: 7%;" align="center">
                    Loại mã
                </th>
                <th style="width: 20%;" align="center">
                    Tên phụ cấp
                </th>
                <th align="center">
                    Công thức thiết lập
                </th>
                <th style="width: 5%;" align="center">
                    Sửa
                </th>
                <th style="width: 5%;" align="center">
                    Xóa
                </th>
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
                <td align="center">
                    <%=R["rownum"]%>
                </td>
                <td align="center">
                    <%=HttpUtility.HtmlEncode(R["iID_MaPhuCap"])%>
                </td>
                <td align="center">
                    <%=HttpUtility.HtmlEncode(R["iLoaiMa"])%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(R["sTenPhuCap"])%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(R["sCongThuc"])%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "Luong_DanhMucPhuCap", new { iID_MaPhuCap = R["iID_MaPhuCap"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "Luong_DanhMucPhuCap", new { iID_MaPhuCap = R["iID_MaPhuCap"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
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
</asp:Content>
