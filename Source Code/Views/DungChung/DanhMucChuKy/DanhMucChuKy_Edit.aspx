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
    String iID_MaChuKy = Convert.ToString(ViewData["iID_MaChuKy"]);
    String ParentID = "Edit";

    NameValueCollection data = (NameValueCollection)ViewData["data"];

 

    using (Html.BeginForm("EditSubmit", "DanhMucChuKy", new { ParentID = ParentID, iID_MaChuKy = iID_MaChuKy }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaChuKy", iID_MaChuKy)%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> | <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop"), "Danh sách chứng từ ghi sổ")%>
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
            	<td>
                	<span>Nhập thông tin chữ ký</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing="0" border="0" width="70%">
                <tr>
                    <td class="td_form2_td1"><div>Ký hiệu</div></td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, data, "sKyHieu", "", "style=\"width:98%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sKyHieu")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Tên</div></td>
                    <td class="td_form2_td5">
                       <div>
                            <%=MyHtmlHelper.TextBox(ParentID, data, "sTen", "", "style=\"width:98%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                        </div>
                    </td>
                </tr>              
            </table>
        </div>
    </div>
</div><br />
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
            	        <input type="submit" class="button4" value="Lưu" />
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<%
    }
%>
</asp:Content>