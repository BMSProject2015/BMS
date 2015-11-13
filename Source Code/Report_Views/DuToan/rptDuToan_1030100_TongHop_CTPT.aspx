<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
   <%
    String ParentID = "DuToan";

    String URLView = "";
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
    String MaND = User.Identity.Name;
    String iNamLamViec = ReportModels.LayNamLamViec(MaND);
    if (PageLoad == "1")
    {
        URLView = Url.Action("ViewPDF", "rptDuToan_1030100_TongHop_CTPT", new { MaND = MaND, iID_MaPhongBan = iID_MaPhongBan });
    }  
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="0"});
    DataTable dtPhongBan = DuToan_ReportModels.getDSPhongBan(iNamLamViec, MaND,"1030100");
    SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
    dtPhongBan.Dispose();
    using (Html.BeginForm("EditSubmit", "rptDuToan_1030100_TongHop_CTPT", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo Dự toán chi ngân sách Quốc phòng ( phần ngân sách xây dựng cơ bản-Công trình phổ thông) năm <%=iNamLamViec %></span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                 <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="text-align:center;width:60%">
                            <div><b>Chọn phòng ban: </b>   <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 20%;height:24px;\"")%>    
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="margin:0 auto;" colspan = "2">
                            <table cellpadding="0" cellspacing="0" border="0" style="margin-left:45%;">
                                <tr>
                                    <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                 </table>
            </div>
        </div>
    </div>
    <%}
       
         %>
 
      <script type="text/javascript">
          function Huy() {
              window.location.href = '<%=BackURL%>';
          }
    </script> 
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToan_1030100_TongHop_CTPT", new { MaND = MaND, iID_MaPhongBan = iID_MaPhongBan }), "Export to Excel")%>
      <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
