<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm())
       { %>
       <%= Html.ValidationSummary(true, NgonNgu.LayXau("Đăng ký không thành công."))%>
    <div class="box_tong">
        <div class="title_tong">
		    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span><%=NgonNgu.LayXau("Thông tin sửa tài khoản")%></span>
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
            	        <td><input id="Register" type="button" class="button4" value="<%= NgonNgu.LayXau("Sửa")%>" /></td>
                        <td width="5px"></td>
                        <td><input type="button" class="button4" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="javascript:history.go(-1);" /></td>
                    </tr>
                </table>         
	        </td>
        </tr>
    </table>
    <div class="cao5px">&nbsp;</div>
    <% }  %>
    <script type="text/javascript">
        $.get('<%=HamRiengModels.SSODomain%>/user/GetUserInfo?callback=?',
    				{ username: '<%= Request.QueryString["MaNguoiDung"]%>' },
    				function (ssodata) {
    				    if (ssodata.GetUserInfoResult.Username != '') {
    				        $("#Fullname").val(ssodata.GetUserInfoResult.Fullname);
    				        $("#Email").val(ssodata.GetUserInfoResult.Email);
    				    }
    				    // make sure to tell jQuery this is a JSONP call
    				}, 'jsonp');


        $(function () {
            $("#Register").click(function () {

                $("#error").text('').hide();
                $("#info").text('').hide();
                if (validateForm()) {
                    $.get('<%=HamRiengModels.SSODomain%>/user/ChangeUserInfo?callback=?',
    				    { username: '<%= Request.QueryString["MaNguoiDung"]%>', email: $("#Email").val(), fullname: $("#Fullname").val() },
    				    function (ssodata) {
    				        if (ssodata.Status == 'UNSUCCESS') {
    				            $("#error").text(ssodata.Error).show();
    				        } else {
    				            // redirect to authentication page instead of duplicating code here
    				            $("#info").text('Sửa thông tin tài khoản thành công!').show();
    				        }
    				        // make sure to tell jQuery this is a JSONP call
    				    }, 'jsonp');
                }
            });
        });

        function validateForm() {
            var vR = true;
            var HoTen = $("#Fullname").val();
            var vEmail = $("#Email").val();

            $("#error_Fullname").text("").hide();
            $("#error_Email").text("").hide();

            if (HoTen == null || HoTen == "") {
                $("#error_Fullname").text("Chưa nhập họ tên người dùng!").show();
                if (vR) $("#Fullname").focus();
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
            return vR;
        }
    </script>
</asp:Content>
