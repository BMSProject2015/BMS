<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>KTCS_TaiSan_TrangThai</title>
</head>
<body>
    <div>
    <% 
        String ParentID = "Edit";
        String iID_MaTaiSan = Request.QueryString["iID_MaTaiSan"];
        DataTable dtTaiSan = KTCS_TaiSanModels.Get_dtTaiSan(iID_MaTaiSan);
        NameValueCollection data = new NameValueCollection();
        data["iTrangThaiTaiSan"] = Convert.ToString(dtTaiSan.Rows[0]["iTrangThaiTaiSan"]);
        using (Html.BeginForm("TrangThai_Submit", "KTCS_TaiSan", new { ParentID = ParentID }))
     {       
    %>
   
     <%=MyHtmlHelper.Hidden(ParentID, iID_MaTaiSan, "iID_MaTaiSan", "")%>
     <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
        
        <tr>
            <td> Đang sử dụng</td>
            <td> <%=MyHtmlHelper.Option(ParentID, "2", data, "iTrangThaiTaiSan", "")%> </td>
        </tr>
        <tr>
            <td>Điều chuyển</td>
            <td> <%=MyHtmlHelper.Option(ParentID, "4", data, "iTrangThaiTaiSan", "")%> </td>
        </tr>
        <tr>
            <td> Loại khỏi biên chế</td>
            <td> <%=MyHtmlHelper.Option(ParentID, "3", data, "iTrangThaiTaiSan", "")%> </td>
        </tr>
        <tr>
            <td colspan="4" class="td_form2_td1" align="right">
            	<div style="text-align:right; float:right; width:100%">
                    <input type="submit" class="button4" value="Lưu" style="float:right; margin-left:10px;"/>
            	</div> 
            </td>
        </tr>
     </table>
     
     <%} %>
    </div>
</body>
</html>
