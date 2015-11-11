<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<span style="color: Gray;">Xin chào:</span>
<%= Html.Encode(Page.User.Identity.Name) %>
<span style="color: Gray;">(Thời gian:</span>
<%= DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")%>
<span style="color: Gray;">- IP:</span>
<%= Request.UserHostAddress%>
<span style="color: Gray;">)</span>&nbsp;| <a href="<%=Url.Action("ChangePassword", "Account")%>">
    Đổi mật khẩu</a> | <a href="<%=Url.Action("SSOLogOff", "Account")%>">Thoát</a>