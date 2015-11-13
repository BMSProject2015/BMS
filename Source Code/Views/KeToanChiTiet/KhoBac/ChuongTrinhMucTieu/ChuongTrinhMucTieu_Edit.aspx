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
    String iID_MaChuongTrinhMucTieu = Convert.ToString(ViewData["iID_MaChuongTrinhMucTieu"]);
    String ParentID = "Edit";

    DataTable dt = KTCT_KhoBac_ChuongTrinhMucTieuModels.Get_RowChuongTrinhMucTieu(iID_MaChuongTrinhMucTieu);
    
    String sTen = "", sMoTa = "";
    if (dt.Rows.Count > 0)
    {
        sTen = dt.Rows[0]["sTen"].ToString();
        sMoTa = dt.Rows[0]["sMoTa"].ToString();
    }

    String strReadOnlyMa = "";
    String strIcon = "";
    if (ViewData["DuLieuMoi"] == "0") {
        strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
        strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
    }
    
    using (Html.BeginForm("EditSubmit", "KTCT_KhoBac_ChuongTrinhMucTieu", new { ParentID = ParentID, iID_MaChuongTrinhMucTieu = iID_MaChuongTrinhMucTieu }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaChuongTrinhMucTieu", iID_MaChuongTrinhMucTieu)%>
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
<div id="divMenuLeft" style="width: 20%; float:left; position:relative;">
    <%Html.RenderPartial("~/Views/KeToanChiTiet/KhoBac/KTCT_KhoBac_Menu.ascx"); %>
</div>
<div id="divNoiDung" style="width: 80%; float:left; position:relative;">
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin chương trình mục tiêu</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing=""="0" border="0" width="70%">
                <tr>
                    <td class="td_form2_td1"><div>Mã chương trình mục tiêu</div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, iID_MaChuongTrinhMucTieu, "iiID_MaChuongTrinhMucTieu", "", "style=\"width:20%;\" " + strReadOnlyMa + "")%><%=strIcon %>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iiID_MaChuongTrinhMucTieu")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Tên chương trình mục tiêu</div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width:80%;\"")%><br />
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Mô tả</div></td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "style=\"width:80%; height: 150px;\"")%>
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
</div>
</asp:Content>

