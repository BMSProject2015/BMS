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
    String iID_MaMucLucQuanSo = Convert.ToString(ViewData["iID_MaMucLucQuanSo"]);
    String iID_MaMucLucQuanSo_Cha = Convert.ToString(ViewData["iID_MaMucLucQuanSo_Cha"]);
    String ParentID = "Edit";
    String sKyHieu = "", sMoTa = "",sHienThi="";
    bool bLaHangCha = false;
    DataTable dt = QuyetToan_QuanSo_MucLucModels.GetMucLucQuanSo(iID_MaMucLucQuanSo);
    if (iID_MaMucLucQuanSo != null && iID_MaMucLucQuanSo != "")
    {
        sKyHieu = dt.Rows[0]["sKyHieu"].ToString();
        sMoTa = Convert.ToString(dt.Rows[0]["sMoTa"]);
        sHienThi = Convert.ToString(dt.Rows[0]["sHienThi"]);
        bLaHangCha = Convert.ToBoolean(dt.Rows[0]["bLaHangCha"]);
    }

    String tgLaHangCha = "";
    if (bLaHangCha == true)
    {
        tgLaHangCha = "on";
    }
    
    using (Html.BeginForm("EditSubmit", "QuyetToan_QuanSo_MucLucQuanSo", new { ParentID = ParentID, iID_MaMucLucQuanSo = iID_MaMucLucQuanSo }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaMucLucQuanSo_Cha", iID_MaMucLucQuanSo_Cha)%>
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_QuanSo_MucLucQuanSo"), "Mục lục quân số")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin mục lục quân số</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="5" cellspacing="5" width="50%">
                <tr>
                    <td class="td_form2_td1"><div><b>Ký hiệu</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sKyHieu, "sKyHieu", "", "style=\"width:100%;\"", 2)%><br />
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sKyHieu")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><b>Mô tả</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sMoTa, "sMoTa", "", "style=\"width:100%;\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_sMoTa")%>
                        </div>
                    </td>
                </tr>
                  <tr>
                    <td class="td_form2_td1"><div><b>Hiển thị</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sHienThi, "sHienThi", "", "style=\"width:100%;\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_sHienThi")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><b>Là hàng cha</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.CheckBox(ParentID, tgLaHangCha, "bLaHangCha", String.Format("value='{0}'", bLaHangCha))%></div>
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
 } if (dt != null) { dt.Dispose(); };    
%>
</asp:Content>
