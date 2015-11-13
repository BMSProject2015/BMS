<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%Html.RenderPartial(ViewData["Partial_View"].ToString(), new { ControlID = ViewData["ControlID"], OnSuccess = ViewData["OnSuccess"] }); %>
