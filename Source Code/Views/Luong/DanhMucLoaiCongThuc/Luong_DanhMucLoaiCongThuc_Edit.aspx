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
    String iID_MaDanhMucLoaiCongThuc = Convert.ToString(ViewData["iID_MaDanhMucLoaiCongThuc"]);
    String ParentID = "Edit";

    NameValueCollection data = (NameValueCollection)ViewData["data"];

    using (Html.BeginForm("EditSubmit", "Luong_DanhMucLoaiCongThuc", new { ParentID = ParentID, iID_MaDanhMucLoaiCongThuc = iID_MaDanhMucLoaiCongThuc }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<% %>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập loại công thức</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table class="mGrid">
                <tr><th colspan="6"><b>Tên trường</b></th></tr>
                <tr style="height:20px;">
                    <td>rLuongCoBan</td>
                    <td>rPhuCap_ChucVu</td>
                    <td>rPhuCap_ThamNien</td>
                    <td>rPhuCap_VuotKhung</td>
                    <td>rLuongToiThieu</td>
                    <td>rPhuCap_BaoLuu</td>                    
                </tr>
                <tr style="height:20px;">
                    <td>rPhuCap_AnNinhQuocPhong</td>
                    <td>rPhuCap_DacBiet</td>
                    <td>rPhuCap_TrenHanDinh</td>
                    <td>rPhuCap_NuQuanNhan</td>
                    <td>rPhuCap_Khac</td>                    
                    <td>&nbsp;</td>
                </tr>
                <tr><td colspan="6">&nbsp;</td></tr>
            </table>
            <table cellpadding="0"  cellspacing="0" border="0" width="100%">
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Tên công thức</div></td>
                    <td class="td_form2_td5" width="90%">
                       <div>
                            <%=MyHtmlHelper.TextBox(ParentID, data, "sTen", "", "style=\"width:98%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Mô tả</div></td>
                    <td class="td_form2_td5" width="90%">
                       <div>
                            <%=MyHtmlHelper.TextArea(ParentID, data, "sMoTa", "", "style=\"width:98%;height:100px;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sMoTa")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Công thức</div></td>
                    <td class="td_form2_td5" width="90%">
                       <div>
                            <%=MyHtmlHelper.TextArea(ParentID, data, "sCongThuc", "", "style=\"width:98%;height:200px;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sCongThuc")%>
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