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
    <style type="text/css">
        .div-floatleft
        {
            float:left;    
            max-height:110px;            
        }
        .div-label
        {           
            font-size:13px;  
            padding-top:2px;
            min-width:100px;     
        }
        .div-txt
        {
            padding-top:5px;
            width:250px;            
        }    
        .p
        {
            height:20px;
            line-height:20px;
            padding:2px 4px 2px 2px;     
            text-align:right;                       
        }
    </style>    
</head>
<body>
    <%   
    String ParentID = "BaoCaoNganSachNam";    
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];  
    DataTable dtDonVi = rptDuToan_InBiaController.Get_DonVi();
    SelectOptionList slDonvi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        if (dtDonVi.Rows.Count > 0)
        {
            iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
        }
        else
        {
            iID_MaDonVi = Guid.Empty.ToString();
        }
    }
    String NgayGui = Request.QueryString["NgayGui"];
    if (String.IsNullOrEmpty(NgayGui))
    {
        NgayGui = DateTime.Now.Date.ToString("dd-MM-yyyy");
    }
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="1"});
    using (Html.BeginForm("EditSubmit", "rptDuToan_InBia", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>In bìa dự toán ngân sách</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="table_form2" class="table_form2">
            <div id="" style="width:600px; margin:0px auto; min-height:10px;">                
                <div class="div-floatleft div-label" style="max-width:160px; text-align:left; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Chọn đơn vị:")%></p>                      
                </div>
                <div class="div-floatleft div-txt" style="width:160px;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slDonvi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%\"")%></p>                    
                </div>
                <div class="div-floatleft div-label" style="max-width:160px; text-align:left; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Gửi về trước")%></p>                      
                </div>
                <div class="div-floatleft div-txt" style="width:120px; margin:auto">                   
                       <%=MyHtmlHelper.DatePicker(ParentID, NgayGui, "dtNgayGui", "", "class=\"input1_2\" onblur=isDate(this)\" style=\"z-index:1\"")%>                                
                </div>
            </div>
            <div style="height:5px; clear:both;"></div>
            <div id="both" style="clear:both; height:30px; line-height:30px;">
                <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 5px auto; width:200px;">
                    <tr>
                        <td><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                        <td width="5px"></td>
                        <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                    </tr>
               </table>   
            </div>
        </div>
    </div>
    <%} %>    
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
    <%    
        dtDonVi.Dispose();
    %>
   <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToan_InBia", new { NgayGui = NgayGui, iID_MaDonVi = iID_MaDonVi }), "Xuất ra excel")%>
   <div>
        <iframe src="<%=Url.Action("ViewPDF","rptDuToan_InBia",new{NgayGui=NgayGui,iID_MaDonVi=iID_MaDonVi})%>" height="600px" width="100%" style="z-index:-1;"></iframe>
   </div>
 </body>
</html>
