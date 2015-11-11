<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String ParentID = "ThamSo";
        String page = Request.QueryString["page"];
        String UserName = User.Identity.Name;
        int CurrentPage = 1;
        SqlCommand cmd;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        DataTable dt = KeToan_DanhMucThamSoModels.GetDanhSach(CurrentPage, Globals.PageSize, DanhMucModels.NamLamViec(UserName));

        double nums = KeToan_DanhMucThamSoModels.GetDanhSach_Count(DanhMucModels.NamLamViec(UserName));
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        String strThemMoi = Url.Action("Edit", "KeToanTongHop_ThamSo");
        String strCopy = Url.Action("CopyDuLieu", "KeToanTongHop_ThamSo");    
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                </div>
            </td>
             <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width:75%">
                        <span>Danh sách tham số</span>
                    </td>
                    <%if (dt.Rows.Count <= 0)
                      { %>
                    <td align="right" style="padding-right: 10px;">
                        <input id="Button2" type="button" class="button_title" value="Lấy dữ liệu năm trước" onclick="javascript:location.href='<%=strCopy %>'" />
                    </td>
                    <%} %>
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
                <th style="width: 10%;" align="center">
                    Ký hiệu
                </th>
                <th align="center" style="width: 20%;">
                    Nội dung
                </th>
                <th align="center">
                    Tham số
                </th>
                <th style="width: 20%;" align="center">
                    Báo cáo
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
                    String bChoPhepXoa = HamChung.ConvertToString(R["bChoPhepXoa"]);
                    String classtr = "";
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
                    <%= HttpUtility.HtmlEncode( dt.Rows[i]["sKyHieu"])%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(dt.Rows[i]["sNoiDung"])%>
                </td>
                <td align="left" style="text-align: justify;">
                    <div style="width: 600px;">
                        <%=HttpUtility.HtmlEncode(dt.Rows[i]["sThamSo"])%></div>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(dt.Rows[i]["sTen"])%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "KeToanTongHop_ThamSo", new { iID_MaThamSo = R["iID_MaThamSo"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                   <%-- <% if (bChoPhepXoa == "True" || bChoPhepXoa == "true")
                       { %>--%>
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "KeToanTongHop_ThamSo", new { iID_MaThamSo = R["iID_MaThamSo"], bChoPhepXoa = R["bChoPhepXoa"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
                  <%--  <%}%>--%>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="7" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
    <% if (dt != null) dt.Dispose(); %>
</asp:Content>
