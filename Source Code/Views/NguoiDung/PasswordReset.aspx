<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String MaNguoiDung = Request.QueryString["MaNguoiDung"];    
    String NewPassword = Convert.ToString(ViewData["NewPassword"]);        
    String ParentID = "PasswordReset";
    SqlCommand cmd;
    
    String TenNguoiDung = "", Email = "", UserId="";
    Boolean HoatDong = false;
    if (MaNguoiDung != null)
    {        
        cmd = new SqlCommand("SELECT * FROM QT_NguoiDung WHERE sID_MaNguoiDung=@sID_MaNguoiDung");
        cmd.Parameters.AddWithValue("@sID_MaNguoiDung", MaNguoiDung);
        DataTable dtNguoiDung = Connection.GetDataTable(cmd);
        cmd.Dispose();        
        TenNguoiDung = Convert.ToString( dtNguoiDung.Rows[0]["sHoTen"]);
        HoatDong = Convert.ToBoolean(dtNguoiDung.Rows[0]["bHoatDong"]);
        
        cmd = new SqlCommand("SELECT aspnet_Users.*,aspnet_Membership.Email FROM aspnet_Membership INNER JOIN aspnet_Users ON aspnet_Membership.UserId = aspnet_Users.UserId WHERE UserName=@UserName");
        cmd.Parameters.AddWithValue("@UserName", MaNguoiDung);
        DataTable dtUser = Connection.GetDataTable(cmd);
        Email = Convert.ToString(dtUser.Rows[0]["Email"]);
        UserId = Convert.ToString(dtUser.Rows[0]["UserId"]);
        cmd.Dispose();
        dtNguoiDung.Dispose();
    }
%>

<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <span><%=NgonNgu.LayXau("Thông tin người dùng")%></span>
                </td>
            </tr>
        </table>
	</div>    
    <div id="nhapform">
        <div id="form2">
            <table width="100%" cellpadding="0" cellspacing=""="0" border="0" class="table_form2">
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            <%=NgonNgu.LayXau("Tài khoản") %></div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%= MyHtmlHelper.Label(MaNguoiDung, "sID_MaNguoiDung", "")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            <%=NgonNgu.LayXau("Họ tên") %></div>
                    </td>
                    <td class="td_form2_td5">
                        <div>                            
                            <%= MyHtmlHelper.Label(TenNguoiDung, "sHoTen", "")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            <%=NgonNgu.LayXau("Email") %></div>
                    </td>
                    <td class="td_form2_td5">
                        <div>                           
                            <%= MyHtmlHelper.Label(Email, "sEmail", "")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            <%=NgonNgu.LayXau("Kích hoạt") %></div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.CheckBox(ParentID, HoatDong.ToString(), "bHoatDong", "", "disabled=\"disabled\"")%>                            
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            <%=NgonNgu.LayXau("Mật khẩu") %></div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <span style="background:yellow"><b><%=NewPassword %></b></span>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
 </div>
 
<div class="cao5px">&nbsp;</div>

</asp:Content>
