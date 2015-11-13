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
        String sMaPhongBan = Convert.ToString(ViewData["MaPhongBan"]);
        int i;
        String page = Request.QueryString["page"];
        int CurrentPage = 1;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = PhongBan_LNSModels.getDS(sMaPhongBan, CurrentPage, Globals.PageSize);
        double nums = PhongBan_LNSModels.getDS_Count(sMaPhongBan);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        String strThemMoi = "/PhongBanLNS/EditNew?Code=" + sMaPhongBan;
    %>
     <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 12%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |<%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhongBanLNS"), "Phòng ban - Loại ngân sách")%>
                </div>
            </td>
              <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong" >
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>

                     <% if (ViewData["DuLieuMoi"] == "1")
                   {
                       %>
                	 <span><a href="/PhongBanLNS">
                     
                     Danh sách đơn vị quản lý</a></span>
                    <% 
                   }
                   else
                   { %>
                   <span><a href="/PhongBanLNS">Danh sách loại ngân sách quản lý </a> (Phòng ban: <%=DanhMucModels.getTenPB(sMaPhongBan)%>)</span>
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
                    Phòng ban
                </th>
                <th style="width: 30%;" align="left">
                    Loại ngân sách quản lý
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
                    String urlDetailPB = "/PhongBan/Edit?Code=" + R["iID_MaPhongBan"];
                                     
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                </td>
                <td align="left">
                   <a href=<%=urlDetailPB %>><b>
                        
                        
                         <%=dt.Rows[i]["sTen"]%>
                        </b></a>
                </td>
                <td align="left">
                <b>
                        
                        
                         <%=dt.Rows[i]["LNS"]%>
                        </b>
                   
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("EditDetail", "PhongBanLNS", new { Code = R["iID_MaPhongBan"], MaID = R["iID_MaPhongBanLoaiNganSach"], LNS = "" }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("DeleteDetail", "PhongBanLNS", new { Code = R["iID_MaPhongBanLoaiNganSach"], MaID = R["iID_MaPhongBan"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
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
