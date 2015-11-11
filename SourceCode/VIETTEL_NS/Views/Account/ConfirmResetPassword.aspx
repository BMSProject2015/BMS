<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Thiết lập mật khẩu")%></span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<div id="form2">	
          <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2" >
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tài khoản của bạn")%></div></td>
               <td class="td_form2_td5"><div>
                     <%=Request.QueryString["TaiKhoan"]%>
                </div></td>                
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Email")%></div></td>
               <td class="td_form2_td5"><div>
                     <span id="email"></span>
                </div></td>                
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Mật khẩu mới")%></div></td>
               <td class="td_form2_td5"><div>
                    <span id="password" style="background:yellow"></span>
                </div></td>                
            </tr>
            <tr>
                <td class="td_form2_td1"></td>
                <td class="td_form2_td5">
                    <span id="error"></span>
                </td>
            </tr>
        </table>
      </div>
    </div>
</div>
<script type="text/javascript">
    $.get('<%=HamRiengModels.SSODomain%>/user/ConfirmResetPassword?callback=?',
		{ TaiKhoan: '<%=Request.QueryString["TaiKhoan"]%>', id: '<%=Request.QueryString["id"]%>' },
		function(ssodata) {
		    if (ssodata.Username == '') {
		        $("#error").text('Tài khoản không tồn tại.').show();
		    } else {
		        // redirect to authentication page instead of duplicating code here
		        $("#email").text(ssodata.ConfirmResetPasswordResult.Email).show();
		        $("#password").text(ssodata.ConfirmResetPasswordResult.Password).show();
		    }
		    // make sure to tell jQuery this is a JSONP call
		}, 'jsonp');
</script>
</asp:Content>