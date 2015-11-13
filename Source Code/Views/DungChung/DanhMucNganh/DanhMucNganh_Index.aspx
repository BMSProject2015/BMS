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
        String ParentID = "DanhMucNganh";
        String page = Request.QueryString["page"];
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = MucLucNganSach_NganhModels.LayDanhSachNganh(CurrentPage, Globals.PageSize);
        double nums = MucLucNganSach_NganhModels.Count_DanhSachNganh();
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        String strThemMoi = Url.Action("Edit", "DanhMucNganh");  
        using (Html.BeginForm("EditSubit", "DanhMucNganh", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Mục lục ngân sách Ngành</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input id="Button1" type="button" class="button" value="Thêm mới"
                            onclick="javascript:location.href='<%=strThemMoi %>'" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table class="mGrid">
                <tr>
                    <th style="width: 5%;" align="center">
                        STT
                    </th>
                    <th style="width: 5%;" align="left">
                        Mã Ngành
                    </th>
                    <th style="width: 15%;" align="left">
                        Tên Ngành
                    </th>
                    <th style="width: 25%;" align="left">
                         Mã Ngành(MLNS)
                    </th>
                    <th style="width: 35%;" align="left">
                       Người quản lý
                    </th>
                    <th style="width: 5%;" align="center">
                        Sửa
                    </th>
                    <th style="width: 5%;" align="center">
                      Xóa
                    </th>
                </tr>
                <%               
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow R = dt.Rows[i];
                        string strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
                        String classtr = "";
                        int STT = i + 1;
                        if (i % 2 == 0)
                        {
                            classtr = "class=\"alt\"";
                        }
                        String urlDetail = "/DanhMucNganh/Edit?iID=" + R["iID"];
                        String s = "Sửa";
                %>
                <tr <%=classtr %>>
                    <td align="center">
                        <%=STT%>
                    </td>
                    <td>
                        <%=dt.Rows[i]["iID_MaNganh"]%>
                    </td>
                    <td>
                        <%=dt.Rows[i]["sTenNganh"]%>
                    </td>
                    <td>
                         <%=dt.Rows[i]["iID_MaNganhMLNS"]%>
                    </td>
                      <td>
                         <%=dt.Rows[i]["sMaNguoiQuanLy"]%>
                    </td>
                    <td>
                          <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "DanhMucNganh", new { iID = R["iID"], page = CurrentPage }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                    </td>
                     <td>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "DanhMucNganh", new { iID = R["iID"], page = CurrentPage }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
                    </td>
                     
                    <%} %>
                </tr>
                <tr class="pgr">
                    <td colspan="9" align="right">
                        <%=strPhanTrang%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%} %>
</asp:Content>
