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
        String sMaNguoiDung = Convert.ToString(ViewData["sMaNguoiDung"]);
        int i;
        String page = Request.QueryString["page"];
        int CurrentPage = 1;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = NguoiDung_PhongBanModels.getDS(sMaNguoiDung, CurrentPage, Globals.PageSize);
        double nums = NguoiDung_PhongBanModels.getDS_Count(sMaNguoiDung);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        String strThemMoi = "/NguoiDungPhongBan/EditNew?sMaNguoiDung=" + sMaNguoiDung;
    %>
    <div class="box_tong">
        <div class="title_tong" >
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>

                     <% if (ViewData["DuLieuMoi"] == "1")
                   {
                       %>
                	 <span><a href="/NguoiDungPhongBan">
                     
                     Danh sách đơn vị quản lý</a></span>
                    <% 
                   }
                   else
                   { %>
                   <span><a href="/NguoiDungPhongBan">Danh sách phòng ban quản lý </a> (Người dùng: <%=sMaNguoiDung%>)</span>
                    <% } %>
                       
                    </td>
                    <td align="right" style="padding-right: 10px; ">
                    <div style="cursor:pointer;">
                        <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />&nbsp;
                   
                        <input id="Button2" type="button" class="button_title" value="Quay về" onclick="javascript:history.go(-1)" /></div>
                    </td>
                </tr>
            </table>
        </div>
       
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 10%;" align="left">
                    Mã người dùng
                </th>
                <th style="width: 30%;" align="left">
                    Phòng ban quản lý
                </th>
                <th style="width: 10%;" align="left">
                    Ngày tạo
                </th>
                <th style="width: 10%;" align="left">
                    Người tạo
                </th>
                <th style="width: 5%;" align="center">
                    Hoạt động
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

                    String bPublicNew = Convert.ToString(R["bPublic"]);
                    string strIconNew = "<img src='../Content/Themes/images/tick.png' alt='' />";
                    String classtr = "";
                    int STT = i + 1;
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    String urlDetail = "/PhongBan/Edit?sMaNguoiDung=" + R["iID_MaPhongBan"];            
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                </td>
                <td align="left">
                    <%=dt.Rows[i]["sMaNguoiDung"]%>
                </td>
                <td align="left">
                 <a href=<%=urlDetail %>><b>
                        
                        
                         <%=dt.Rows[i]["sTen"]%>
                        </b></a>
                   
                </td>
                <td align="left">
                    <%=dt.Rows[i]["dNgayTao"]%>
                </td>
                <td align="left">
                    <%=dt.Rows[i]["sID_MaNguoiDungTao"]%>
                </td>
                <td align="center">
                    <% if (bPublicNew == "True")
                       { %>
                    <%=strIconNew%>
                    <%} %>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("EditDetail", "NguoiDungPhongBan", new { sMaNguoiDung = R["sMaNguoiDung"], 
    
    MaNguoiDungPhongBan = R["iID_MaNguoiDungPhongBan"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("DeleteDetail", "NguoiDungPhongBan", new { sMaNguoiDung = R["iID_MaNguoiDungPhongBan"], MaNguoiDungPhongBan = R["sMaNguoiDung"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
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
