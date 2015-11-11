<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<VIETTEL.Models.RegisterModel>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <% using (Html.BeginForm())
       { %>
       <%= Html.ValidationSummary(true, NgonNgu.LayXau("Đăng ký không thành công."))%>
    <div class="box_tong">
        <div class="title_tong">
		    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span><%=NgonNgu.LayXau("Thông tin về tài khoản")%></span>
                    </td>
                </tr>
            </table>
	    </div>    
	    <div id="nhapform">
            <div id="form2">
                <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2">
                    <tr>
                       <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Họ và tên")%></div></td>
                       <td class="td_form2_td5"><div>
                            <%= Html.TextBox("Fullname", "", new { style="width:50%" })%>
                            <span id="error_Fullname" style="color: Red;"></span>
                        </div></td>
                    </tr>
                    <tr>
                       <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Email")%></div></td>
                       <td class="td_form2_td5"><div>
                            <%= Html.TextBox("Email", "", new { style="width:50%" })%>
                            <span id="error_Email" style="color: Red;"></span>
                        </div></td>
                    </tr>
                    <tr>
                       <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tài khoản")%></div></td>
                       <td class="td_form2_td5"><div>
                            <%= Html.TextBox("Username", "", new { style = "width:50%" })%>
                            <span id="error_Username" style="color: Red;"></span>
                        </div></td>
                    </tr>
                    <tr>
                       <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Mật khẩu")%></div></td>
                       <td class="td_form2_td5"><div>
                            <%= Html.Password("Password", "", new { style = "width:50%" })%>
                            <span id="error_Password" style="color: Red;"></span>
                        </div></td>
                    </tr>
                    <tr>
                       <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Nhập lại mật khẩu")%></div></td>
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
                    <tr>
                       <td class="td_form2_td1"></td>
                       <td class="td_form2_td5">
                            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	                    <tr>
            	                    <td>
            	                        <input id="Register" type="button" class="button7" value="<%= NgonNgu.LayXau("Đăng ký")%>" />
            	                    </td>
                                    <td width="5px"></td>
                                    <td style="padding-top: 6px;">
                                        <input type="button" class="button4" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="javascript:history.go(-1);" />
                                    </td>
                                </tr>
                            </table>
                       </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="cao5px">&nbsp;</div>
    <% } %>
    <script type="text/javascript">
        $(function () {
            $("#Register").click(function () {
                
                $("#error").text('').hide();
                $("#info").text('').hide();
                if (validateForm()) {
                    $.get('<%=HamRiengModels.SSODomain%>/user/Register?callback=?',
    				    { username: $("#Username").val(), password: $("#Password").val(), email: $("#Email").val(), fullname: $("#Fullname").val() },
    				    function (ssodata) {
    				        if (ssodata.Status == 'UNSUCCESS') {
    				            $("#error").text(ssodata.Error).show();
    				        } else {
    				            // redirect to authentication page instead of duplicating code here
    				            $("#info").text('Đăng ký thành công tài khoản: ' + $("#Username").val()).show();
    				        }
    				        // make sure to tell jQuery this is a JSONP call
    				    }, 'jsonp');
                }
            });
        });

        function validateForm() {
            var vR = true;
            var minLength = 6;
            var TaiKhoan = $("#Username").val(); 
            var HoTen = $("#Fullname").val();
            var vEmail = $("#Email").val();

            $("#error_Fullname").text("").hide();
            $("#error_Username").text("").hide();
            $("#error_Email").text("").hide();
            $("#error_Password").text("").hide();
            $("#error_ConfirmPassword").text("").hide();


            if (HoTen == null || HoTen == "") {
                $("#error_Fullname").text("Chưa nhập họ tên người dùng!").show();
                if (vR) $("#Fullname").focus();
                vR = false;
            }
            if (TaiKhoan == null || TaiKhoan == "") {
                $("#error_Username").text("Chưa nhập tài khoản!").show();
                if (vR) $("#Username").focus();
                vR = false;
            }            
            if (vEmail == null || vEmail == "") {
                $("#error_Email").text("Chưa nhập email!").show();
                if (vR) $("#Email").focus();
                vR = false;
            }
            var atpos = vEmail.indexOf("@");
            var dotpos = vEmail.lastIndexOf(".");
            if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= vEmail.length) {
                $("#error_Email").text("Email chưa đúng định dạng!").show();
                if (vR) {
                    $("#Email").select();
                    $("#Email").focus();
                }
                vR = false;
            }
            var vPass = $("#Password").val();
            var vPassC = $("#ConfirmPassword").val();
            if (vPass.length < minLength) {
                $("#error_Password").text("Mật khẩu phải lớn hơn 6 ký tự!").show();
                if (vR) $("#Password").focus();
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
            if (vPass == null || vPass == "") {
                $("#error_Password").text("Chưa nhập mật khẩu!").show();
                if (vR) $("#Password").focus();
                vR = false;
            }
            if (vPassC == null || vPassC == "") {
                $("#error_ConfirmPassword").text("Chưa xác nhận mật khẩu!").show();
                if (vR) $("#ConfirmPassword").focus();
                vR = false;
            }            
            return vR;
        }
    </script>
</asp:Content>
