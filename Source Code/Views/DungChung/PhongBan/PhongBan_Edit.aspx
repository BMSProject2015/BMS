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
    String MaDonVi = Convert.ToString(ViewData["iID_MaPhongBan"]);
    String ParentID = "Edit";


    DataTable dt = DanhMucModels.GetRow_PhongBan(MaDonVi);
   

    String sTen = "", sMoTa = "";
    if (dt.Rows.Count > 0)
    {
        sTen = dt.Rows[0]["sTen"].ToString();
        sMoTa = dt.Rows[0]["sMoTa"].ToString();       
    }   
    
    using (Html.BeginForm("EditSubmit", "PhongBan", new { ParentID = ParentID, MaDonVi = MaDonVi }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaPhongBan", MaDonVi)%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin phòng ban</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing=""="0" border="0" width="70%">
                
                <tr>
                    <td class="td_form2_td1"><div>Tên phòng ban &nbsp;<span  style="color:Red;">*</span></div> </td>
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
                            <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "style=\"width:80%;\"")%>
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
