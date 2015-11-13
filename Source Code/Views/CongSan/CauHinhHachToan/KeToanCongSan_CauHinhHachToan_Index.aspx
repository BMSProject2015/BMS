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
    String strThemMoi = Url.Action("Create", "KTCS_CauHinhHachToan");
    DataTable dt = KTCS_CauHinhHachToanModels.Get_dtDSCauHinhHachToan();
    String iID_MaKyHieuHachToan = "";
%>

<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách cấu hình hạch toán</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button1" type="button" class="button_title" value="Thêm cấu hình mới" onclick="javascript:location.href='<%=strThemMoi %>'" />                    
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 30%;">Ký hiệu</th>
           
            <th style="width: 30%;" align="center">
               Tài khoản nợ
            </th>
            <th align="center">
                Tài khoản có
            </th>
            <th style="width: 30%;" align="center">
               Giá trị
            </th>
            <th style="width: 25%;" align="center">
                Hành động
            </th>
        </tr>
        <% for (i = 0; i < dt.Rows.Count; i++)
           {
               iID_MaKyHieuHachToan = Convert.ToString(dt.Rows[i]["iID_MaKyHieuHachToan"]);
               String urlThemMoi = Url.Action("Create", "KTCS_CauHinhHachToan", new { iID_MaKyHieuHachToan = iID_MaKyHieuHachToan });
               string urlEdit = Url.Action("Edit", "KTCS_CauHinhHachToan", new { iID_MaKyHieuHachToanChiTiet = dt.Rows[i]["iID_MaKyHieuHachToanChiTiet"] });
               string urlDelete = Url.Action("Delete", "KTCS_CauHinhHachToan", new { iID_MaKyHieuHachToanChiTiet = dt.Rows[i]["iID_MaKyHieuHachToanChiTiet"] });                             
        %>
        <tr>
            <td><%=HttpUtility.HtmlDecode(Convert.ToString(dt.Rows[i]["iID_MaKyHieuHachToan"]))%></td>
            <td><%=HttpUtility.HtmlDecode(Convert.ToString(dt.Rows[i]["iID_MaTaiKhoan_No"]))%></td>
            <td><%=HttpUtility.HtmlDecode(Convert.ToString(dt.Rows[i]["iID_MaTaiKhoan_Co"]))%></td>            
            <td><%=HttpUtility.HtmlDecode(Convert.ToString(dt.Rows[i]["sGiaTri"]))%></td>            
            <td>
                <%=MyHtmlHelper.ActionLink(urlThemMoi, "Thêm cấu hình")%>|
                <%=MyHtmlHelper.ActionLink(urlEdit,"Sửa") %>|
                <%=MyHtmlHelper.ActionLink(urlDelete,"Xóa") %>
            </td>
        </tr>
        <%} %>
        
    </table>
</div>
</asp:Content>
