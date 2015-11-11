<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .div-floatleft
        {                
            max-height:80px;            
        }
        .div-label
        {           
            font-size:13px;  
            padding:5px 0px;                 
        }
        .div-txt
        {
            padding-top:5px;                  
        }    
        .p
        {
            height:23px;
            line-height:23px;
            padding:1px 2px;    
        }
    </style>
</head>
<body>
     <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "QuyetToanQuanSo";
    
    String iThang = Convert.ToString(ViewData["iThang"]);
    
    String MaND = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
    String iNamLamViec = DateTime.Now.Year.ToString();
    if (dtCauHinh.Rows.Count > 0)
    {
        iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

    }
    DateTime dNgayHienTai = DateTime.Now;
   
   
    //Loai ngan sach
    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    if (String.IsNullOrEmpty(iThang))
    {
        iThang = DanhMucModels.ThangLamViec(MaND).ToString();
    }
    String pageload = Convert.ToString(ViewData["pageload"]);
    String _Checked = "";    
    String urlReport = "";
    if (pageload == "1")
        urlReport = Url.Action("ViewPDF", "rptQSQT_QuanSoRaQuan", new { iThang = iThang });
    dtThang.Dispose();
    String URL = Url.Action("Index", "QuyetToan_QuanSo_Report");
    using (Html.BeginForm("EditSubmit", "rptQSQT_QuanSoRaQuan", new { ParentID = ParentID }))
    {
    %>   
     <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Quân số ra quân</span>
                    </td>
                </tr>
            </table>
        </div>        
   <div id="table_form2" class="table_form2">
                <div id="rptMain" style=" margin:5px auto; padding:5px 5px; overflow:visible; text-align:center">
               <ul class="inlineBlock">  
               <li >
               <span style="text-align:right;padding:2px 1px; line-height:28px;" ><label class="label" style="min-width:132px; margin-right:7px;">
                            <%=NgonNgu.LayXau("Làm đến tháng")%>&nbsp;&nbsp;
                            </label> <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "style=\"width:200px\"")%></span>   
               </li>
               </ul>
               
                  
                    <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
                </div>
            </div>
    <%} %>
    <div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
                
        </script>
    </div>  
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQSQT_QuanSoRaQuan", new { iThang = iThang }), "ExportToExcel")%>
     <iframe src="<%=urlReport%>" height="600px" width="100%">
     </iframe>  
</body>
</html>
