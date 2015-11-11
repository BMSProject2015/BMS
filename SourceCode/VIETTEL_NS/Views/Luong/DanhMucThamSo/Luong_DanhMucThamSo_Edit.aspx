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
    String iID_MaThamSo = Convert.ToString(ViewData["iID_MaThamSo"]);
    String ParentID = "Edit";

    NameValueCollection data = (NameValueCollection)ViewData["data"];
    
    String strReadonly = "", strColor = "";
    if (Convert.ToString(ViewData["DuLieuMoi"]) == "0")
    {
        strReadonly = "readonly=\"readonly\"";
        strColor = "background-color:#CFCCCC";
    }


    using (Html.BeginForm("EditSubmit", "Luong_DanhMucThamSo", new { ParentID = ParentID, iID_MaThamSo = iID_MaThamSo }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaThamSo", iID_MaThamSo)%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin tham số</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing="0" border="0" width="100%">     
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Ngày bắt đầu áp dụng</div></td>
                    <td class="td_form2_td5" width="90%">
                        <div>
                            <%=MyHtmlHelper.DatePicker(ParentID, data, "dThoiGianApDung_BatDau", "","")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_dThoiGianApDung_BatDau")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Ngày kết thúc</div></td>
                    <td class="td_form2_td5" width="90%">
                        <div>
                            <%=MyHtmlHelper.DatePicker(ParentID, data, "dThoiGianApDung_KetThuc", "", "")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_dThoiGianApDung_KetThuc")%>
                        </div>
                    </td>
                </tr>   
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Còn sử dụng</div></td>
                    <td class="td_form2_td5" width="90%">
                        <div>
                            <%=MyHtmlHelper.CheckBox(ParentID, data, "bConSuDung", "", "")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_bConSuDung")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Ký hiệu</div></td>
                    <td class="td_form2_td5" width="90%">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, data, "sKyHieu", "",  String.Format("{0}  style=\"width:98%;{1}\"", strReadonly, strColor))%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sKyHieu")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Nội dung</div></td>
                    <td class="td_form2_td5" width="90%">
                       <div>
                            <%=MyHtmlHelper.TextBox(ParentID, data, "sNoiDung", "", "style=\"width:98%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sNoiDung")%>
                        </div>
                    </td>
                </tr>
                
                <tr>
                    <td class="td_form2_td1" width="10%"><div>Tham số</div></td>
                    <td class="td_form2_td5" width="90%">
                       <div>
                            <%=MyHtmlHelper.TextArea(ParentID, data, "sThamSo", "", "style=\"width:98%;height:200px;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sThamSo")%>
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