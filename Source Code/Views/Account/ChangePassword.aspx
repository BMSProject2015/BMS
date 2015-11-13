<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<VIETTEL.Models.ChangePasswordModel>" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="VIETTEL.Models" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<%=NgonNgu.LayXau("Mật khẩu có độ dài tối thiểu " + Html.Encode(ViewData["PasswordLength"]) + " ký tự.")%>--%>
    <br />
    <br />
    <div class="box_tong">
        <div class="title_tong">
		    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span><%=NgonNgu.LayXau("Thay đổi mật khẩu") %></span>
                    </td>
                </tr>
            </table>
	    </div>    
	    <div id="nhapform">
            <div id="form2">
                <% using (Html.BeginForm()) { %>
                <%= Html.ValidationSummary(true, NgonNgu.LayXau("Thay đổi mật khẩu không thành công!")) %>
                <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2">
                    <tr>
                       <td class="td_form2_td1"><div><%= NgonNgu.LayXau("Mật khẩu cũ") %></div></td>
                       <td class="td_form2_td5"><div>
                             <%= Html.Password("OldPassword", "", new { style = "width:50%" })%>
                             <span id="error_OldPassword" style="color: Red;"></span>
                        </div></td>
                    </tr>
                    <tr>
                       <td class="td_form2_td1"><div><%= NgonNgu.LayXau("Mật khẩu mới") %></div></td>
                       <td class="td_form2_td5"><div>
                             <%= Html.Password("NewPassword", "", new { style = "width:50%" })%>
                             <span id="error_NewPassword" style="color: Red;"></span>
                        </div></td>
                    </tr>
                    <tr>
                       <td class="td_form2_td1"><div><%= NgonNgu.LayXau("Nhập lại mật khẩu mới") %></div></td>
                       <td class="td_form2_td5"><div>
                             <%= Html.Password("ConfirmPassword", "", new { style = "width:50%" })%>
                             <span id="error_ConfirmPassword" style="color: Red;"></span>
                        </div></td>
                    </tr>
                    <tr>
                       <td class="td_form2_td1"></td>
                       <td class="td_form2_td5">
                            <span id="info"></span>
                            <span id="error"></span>
                       </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="cao5px">&nbsp;</div>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
	        <td width="70%">&nbsp;
	        </td>
	        <td  width="30%" align="right">						
                <table cellpadding="0" cellspacing="0" border="0" align="right">
        	        <tr>
            	        <td><input id="ChangePassword" type="button" value="<%= NgonNgu.LayXau("Thay đổi mật khẩu")%>" /></td>
                        <td width="5px"></td>
                        <td><input type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="javascript:history.go(-1);" /></td>
                    </tr>
                </table>         
	        </td>
        </tr>
    </table>
    <div class="cao5px">&nbsp;</div>
    <% } %>
    <script type="text/javascript">
        $(function () {
            $("#ChangePassword").click(function () {
                $("#error").text('').hide();
                $("#info").text('').hide();
                if (validateForm()) {
                    if ($("#NewPassword").val() == $("#ConfirmPassword").val()) {
                        $.get('<%=HamRiengModels.SSODomain%>/user/ChangePassword?callback=?',
    				        { oldpassword: $("#OldPassword").val(), newpassword: $("#NewPassword").val() },
    				        function (ssodata) {
    				            if (ssodata.Status == 'UNSUCCESS') {
    				                $("#error").text(ssodata.Error).show();
    				            } else {
    				                // redirect to authentication page instead of duplicating code here
    				                document.location = '<%=Url.Action("ChangePasswordSuccess", "Account") %>';
    				            }
    				            // make sure to tell jQuery this is a JSONP call
    				        }, 'jsonp');
                    }
                    else {
                        $("#error").text('Mật khẩu không hợp lệ.').show();
                    }
                }
            });
        });

        function validateForm() {
            var vR = true;
            var minLength = 6;
            $("#error_OldPassword").text("").hide();
            $("#error_NewPassword").text("").hide();
            $("#error_ConfirmPassword").text("").hide();

            var OldPassword = $("#OldPassword").val();
            var vPass = $("#NewPassword").val();
            var vPassC = $("#ConfirmPassword").val();
            if (OldPassword.length < minLength) {
                $("#error_OldPassword").text("Mật khẩu phải lớn hơn 6 ký tự!").show();
                if (vR) $("#OldPassword").focus();
                vR = false;
            }
            if (vPass.length < minLength) {
                $("#error_NewPassword").text("Mật khẩu phải lớn hơn 6 ký tự!").show();
                if (vR) $("#NewPassword").focus();
                vR = false;
            }
            if (vPass != vPassC) {
                $("#error_ConfirmPassword").text("Xác nhận mật khẩu chưa đúng!").show();
                if (vR) {
                    $("#ConfirmPassword").select();
                    $("#ConfirmPassword").focus();
                }
                vR = false;
            }
            if (OldPassword == null || OldPassword == "") {
                $("#error_OldPassword").text("Chưa nhập mật khẩu cũ!").show();
                if (vR) $("#OldPassword").focus();
                vR = false;
            }
            if (vPass == null || vPass == "") {
                $("#error_NewPassword").text("Chưa nhập mật khẩu mới!").show();
                if (vR) $("#NewPassword").focus();
                vR = false;
            }
            if (vPassC == null || vPassC == "") {
                $("#error_ConfirmPassword").text("Chưa xác nhận mật khẩu mới!").show();
                if (vR) $("#ConfirmPassword").focus();
                vR = false;
            }
            return vR;
        }
    </script>
</asp:Content>
