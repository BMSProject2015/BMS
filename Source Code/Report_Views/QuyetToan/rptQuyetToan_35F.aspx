<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Content/style.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%  
        
        String ParentID = "QuyetToan";
        String MaND = User.Identity.Name;
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_42_6bController.tbTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            if (dtTrangThai.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]);
            }
            else
            {
                iID_MaTrangThaiDuyet = Guid.Empty.ToString();
            }
        }
        dtTrangThai.Dispose();

        DataTable dtDonVi = rptQuyetToan_35FController.Lay_DSDonVi(iID_MaTrangThaiDuyet,MaND);
         String iID_MaDonVi =  Convert.ToString(ViewData["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if(dtDonVi.Rows.Count>0)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]); 
            }
            else
            {
                //iID_MaDonVi = Convert.ToString(ViewData["sMaDonVi"]);
                iID_MaDonVi = Guid.Empty.ToString();
            }
           
        }
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });
        String[] arrDonVi = iID_MaDonVi.Split(',');
        String URLView = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptQuyetToan_35F", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi });
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_35F", new { ParentID = ParentID}))
        {         
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                   <td width="47.9%">
                        <span>Báo cáo tổng hợp theo tháng</span>
                     </td>
                         <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="25%">&nbsp;</td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Trạng Thái  :")%></div>
                        </td>
                         <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon()")%>
                            </div>
                        </td>
                         <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn đơn vị : ")%></div>
                        </td>
                          <td rowspan="4" width="20%" id="<%= ParentID %>_tdDonVi">
                                 <%rptQuyetToan_35FController rpt = new rptQuyetToan_35FController();%> 
                                <%=rpt.obj_DonVi(ParentID, iID_MaTrangThaiDuyet,iID_MaDonVi,MaND)%>
                        </td>
                        <td width="25%">&nbsp;</td>
                    </tr>
                    <tr>
                     <td>&nbsp;</td>
                     <td>&nbsp;</td>
                     <td>&nbsp;</td>
                       
                         <td>&nbsp;</td>
                    </tr>
                      <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                     <tr>
                         <td>&nbsp;</td>
                        <td colspan="2">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                <tr>
                                    <td style="width: 45%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="10%">
                                    </td>
                                    <td style="width: 45%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
   
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_35F", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi }), "Xuất ra Excels")%>
      <script type="text/javascript">
          function Huy() {
              window.location.href = '<%=URL %>';
          }
 </script>
      <script type="text/javascript">
          function Chonall(sLNS) {
              $("input:checkbox[check-group='MaDonVi']").each(function (i) {
                  if (sLNS) {
                      this.checked = true;
                  }
                  else {
                      this.checked = false;
                  }
              });
          }                                            
    </script>
      <script type="text/javascript">
          $(function () {
              $('div.login1 a').click(function () {
                  $('div#rptMain').slideToggle('normal');
                  $(this).toggleClass('active');
                  return false;
              });
          });       
    </script>
     <script type="text/javascript">
         function Chon() {
             var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
            
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDonVi=#2", "rptQuyetToan_35F") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));    
             url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
             });
         }  
         </script> 
    <iframe src="<%=URLView%>"
        height="600px" width="100%"></iframe>
</body>
</html>
