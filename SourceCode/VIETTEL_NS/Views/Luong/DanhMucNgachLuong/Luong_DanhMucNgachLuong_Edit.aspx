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
    String iID_MaNgachLuong = Convert.ToString(ViewData["iID_MaNgachLuong"]);
    String ParentID = "Edit";

    NameValueCollection data = (NameValueCollection)ViewData["data"];

    String strReadonly = "",strColor="";
    if (Convert.ToString(ViewData["DuLieuMoi"]) == "0")
    {
        strReadonly = "readonly=\"readonly\"";
        strColor = "background-color:#CFCCCC";
    }

    using (Html.BeginForm("EditSubmit", "Luong_DanhMucNgachLuong", new { ParentID = ParentID}))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin ngạch lương</span>
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
                        
                            <%=MyHtmlHelper.TextBox(ParentID, data, "iID_MaNgachLuong", "", String.Format("{0} style=\"width:98%;{1}\"", strReadonly, strColor))%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNgachLuong")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Tên ngạch</div></td>
                    <td class="td_form2_td5">
                       <div>
                            <%=MyHtmlHelper.TextBox(ParentID, data, "sTenNgachLuong", "", "style=\"width:98%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTenNgachLuong")%>
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