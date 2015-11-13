
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        String page = Request.QueryString["page"];
        int CurrentPage = 1;
        

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = MucLucNganSachModels.Get_dtMucLucNganSach(CurrentPage, Globals.PageSize);
        double nums = MucLucNganSachModels.Get_MucLucNganSach_Count();
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        String strThemMoi = "";
        %>

  <div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Mục lục ngân sách</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 5%;" align="center">LNS</th>
            <th style="width: 5%;" align="center">L</th>
            <th style="width: 5%;" align="center">K</th>
            <th style="width: 5%;" align="center">Mục</th>
            <th style="width: 5%;" align="center">Tiểu mục</th>
            <th style="width: 5%;" align="center">Tiểu tiết mục</th>
            <th style="width: 5%;" align="center">Ngành</th>
            <th style="width: 5%;" align="center">Tiểu ngành</th>
            <th style="width: 15%;" align="center">Nội dung</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            if (i % 2 == 0)
            {
                classtr = "class=\"alt\"";
            }
            %>
            <tr <%=classtr %>>
                <td align="center"><%=R["rownum"]%></td>     
                <td align="left"><%=HttpUtility.HtmlEncode(R["sLNS"])%></td>                
                <td align="center"><%=HttpUtility.HtmlEncode(R["sL"])%></td>                     
                <td align="left"><%=HttpUtility.HtmlEncode(R["sK"])%></td>                                                
                <td align="center"><%=HttpUtility.HtmlEncode(R["sM"])%></td>                     
                <td align="center"><%=HttpUtility.HtmlEncode(R["sTM"])%></td>                     
                <td align="center"><%=HttpUtility.HtmlEncode(R["sTTM"])%></td>                     
                <td align="center"><%=HttpUtility.HtmlEncode(R["sNG"])%></td>                     
                <td align="center"><%=HttpUtility.HtmlEncode(R["sTNG"])%></td>                     
                <td align="center"><%=HttpUtility.HtmlEncode(R["sMoTa"])%></td>                     
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "MucLucNganSach", new { iID_MaMucLucNganSach = R["iID_MaMucLucNganSach"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "MucLucNganSach", new { iID_MaMucLucNganSach = R["iID_MaMucLucNganSach"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
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
