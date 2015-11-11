<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    string ParentID = "KTCS_LoaiTaiSan";
    string sTaiKhoan = "", sKyHieu = "";
    int i;
    //đoạn code để khi chọn thêm mới
    String strThemMoi = Url.Action("Create", "KTCS_LoaiTaiSan");
   DataTable dt=KTCS_LoaiTaiSanModels.Get_dtDSLoaiTaiSan();
   
%>

<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách loại tài sản</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />                    
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 7%;" align="center">Mã loại tài sản</th>           
            <th style="width: 30%;" align="center">
                Tên loại tài sản
            </th>
            <th align="center">
                Mô tả
            </th>
            <th style="width: 25%;" align="center">
                Hành động
            </th>
        </tr>
        <% for (i = 0; i < dt.Rows.Count; i++)
           {
               string urlEdit = Url.Action("Edit", "KTCS_LoaiTaiSan", new { iID_Ma = dt.Rows[i]["iID_Ma"] });
               string urlDelete = Url.Action("Delete", "KTCS_LoaiTaiSan", new { iID_Ma = dt.Rows[i]["iID_Ma"] });                             
        %>
        <tr>
            <td align="center"><%=HttpUtility.HtmlDecode(Convert.ToString(dt.Rows[i]["iID_MaLoaiTaiSan"])) %></td>
            <td><%=HttpUtility.HtmlDecode(Convert.ToString(dt.Rows[i]["sTen"])) %></td>
            <td><%=HttpUtility.HtmlDecode(Convert.ToString(dt.Rows[i]["sMoTa"])) %></td>            
            <td>
                <%=MyHtmlHelper.ActionLink(urlEdit,"Sửa") %> |
                <%=MyHtmlHelper.ActionLink(urlDelete,"Xóa") %>
            </td>
        </tr>
        <%} %>
        
    </table>
</div>
</asp:Content>
