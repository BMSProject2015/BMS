<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String iID_MaKieuTaiLieu = Convert.ToString(ViewData["iID_MaKieuTaiLieu"]);
    String iID_MaKieuTaiLieu_Cha = Convert.ToString(ViewData["iID_MaKieuTaiLieu_Cha"]);
    String ParentID = "Edit";

    SqlCommand cmd;
    cmd = new SqlCommand("SELECT * FROM TL_DanhMucTaiLieu WHERE iID_MaKieuTaiLieu=@iID_MaKieuTaiLieu");
    cmd.Parameters.AddWithValue("@iID_MaKieuTaiLieu", iID_MaKieuTaiLieu);
    DataTable dt = Connection.GetDataTable(cmd);
        
    cmd.Dispose();

    String TenKieuTaiLieu = "";
    Boolean bLaHangCha = false;
    if (dt.Rows.Count > 0)
    {
        TenKieuTaiLieu = dt.Rows[0]["sTen"].ToString();
        bLaHangCha = Convert.ToBoolean(dt.Rows[0]["bLaHangCha"]);
    }
    String tgLaHangCha = "";
    if (bLaHangCha == true)
    {
        tgLaHangCha = "on";
    }
    
    using (Html.BeginForm("EditSubmit", "TuLieu_DanhMuc", new { ParentID = ParentID, iID_MaKieuTaiLieu = iID_MaKieuTaiLieu }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaKieuTaiLieu", iID_MaKieuTaiLieu)%>
<%= Html.Hidden(ParentID + "_iID_MaKieuTaiLieu_Cha", iID_MaKieuTaiLieu_Cha)%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TuLieu_DanhMuc"), "Danh mục lĩnh vực")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin lĩnh vực</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="5" cellspacing="5" width="50%">
                <tr>
                    <td class="td_form2_td1"><div><b>Tên lĩnh vực</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, TenKieuTaiLieu, "sTen", "", "style=\"width:100%;\"")%><br />
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                        </div>
                    </td>
                </tr>
             <%--   <tr>
                    <td class="td_form2_td1"><div><b>Là hàng cha</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.CheckBox(ParentID, tgLaHangCha, "bLaHangCha", String.Format("value='{0}'", bLaHangCha))%></div>
                    </td>
                </tr>--%>
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
    } dt.Dispose();
%>
</asp:Content>
