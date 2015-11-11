<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QLDA" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>CapPhat_ThongTri_Dialog</title>
</head>
<body>
    <div>
    <%
        String ParentID = "QLDA_CP";
        String iID_MaDotCapPhat = Request.QueryString["iID_MaDotCapPhat"];
        String iLoai="1";
        using (Html.BeginForm("EditSubmit", "rptQLDAThongTri_1", new { ParentID = ParentID, iID_MaDotCapPhat = iID_MaDotCapPhat}))
        {
    %>
     <%=MyHtmlHelper.Hidden(ParentID, iID_MaDotCapPhat, "iID_MaDotCapPhat", "")%>
     <div class="box_tong">
            <div id="Div1">
                <div id="Div2">
                    <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">  
                                <tr>
                                <td></td>
                                <td>
                                <fieldset style="border-top-right-radius:5px; border-top-left-radius:5px; -moz-border-radius-topright:5px;-moz-border-radius-topleft:5px;border:1px solid #dedede; margin-left:5px;margin-right:5px;">
                                                    <legend padding-left:10px; "> <b>  <%=NgonNgu.LayXau("Chọn mẫu  in")%> </b></legend>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "1", iLoai, "iLoai", "")%> 
                                                        <b> 1.Thanh toán </b>
                                                    </p>
                                                    <p style="Padding:4px;">
                                                        <%=MyHtmlHelper.Option(ParentID, "2", iLoai, "iLoai", "")%> 
                                                                    <b> 2.Tạm ứng</b>
                                                    </p> 
                                                </fieldset>  
                                                </td>
                                                <td></td>
                                </tr>
                                <tr>
                                <td></td>
                                <td><b>Lý do: </b> <div><%=MyHtmlHelper.TextArea(ParentID, "", "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div></td>
                                <td></td>
                                </tr>
                                <tr>
                                <td></td>
                                   <td colspan="2"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Dialog_close('<%=ParentID %>');" style=" display:inline-block; margin-left:4px;" />
                                    </td>
                                </tr>
                            </table></td>
                            <td></td>
                                </tr>
                    </table>
                </div>
            </div>
    </div>
    <%
        }
    %>
    </div>
       
</body>
</html>
