<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.NhanSu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<% 
    String ParentID = "NhanSu";
    String iID_MaCanBo = Request.QueryString["iID_MaCanBo"];
    using (Html.BeginForm("EditSubmit", "rptNhanSu_SoYeuLyLich", new { ParentID = ParentID }))
    {
  %>
        <%} %>
        
   <iframe src="<%=Url.Action("ViewPDF","rptNhanSu_SoYeuLyLich",new{iID_MaCanBo=iID_MaCanBo})%>" height="600px" width="100%"></iframe>
</body>
</html>
