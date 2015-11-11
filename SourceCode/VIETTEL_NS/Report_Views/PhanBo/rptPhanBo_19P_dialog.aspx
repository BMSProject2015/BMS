<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<%
            String ParentID = "PhanBo_19P";
            using (Html.BeginForm("EditSubmit", "rptPhanBo_19P", new { ParentID = ParentID }))
            {
    %>
     <div class="box_tong">
         <div class="title_tong">
            
        </div>
        <div id="Div1">
          <div id="Div2">
            <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                  
                  <tr>
                  <td width="10%"><b>1. Tiêu dề chuẩn </b></td>
                  <td width="10%"></td>
                  </tr> 
                  
                 </table>
            </div>
        </div>
    </div>
    <%} %> 
 </body>
 </html>
 