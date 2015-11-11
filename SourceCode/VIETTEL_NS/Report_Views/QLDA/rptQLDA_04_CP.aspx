<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QLDA" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%   
        String ParentID = "QLDA";
        String MaND = User.Identity.Name;

        DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
        SelectOptionList  slDotCapPhat = new SelectOptionList(dtDotCapPhat, "iID_MaDotCapPhat", "dNgayCapPhat");
        String iID_MaDotCapPhat = Convert.ToString(ViewData["iID_MaDotCapPhat"]);
        //String dNgayCapPhat ="01/01/2000" ;
        //if (dtDotCapPhat.Rows.Count>0)
        //    dNgayCapPhat = dtDotCapPhat.Rows[0]["dNgayCapPhat"].ToString();
        if (String.IsNullOrEmpty(iID_MaDotCapPhat))
        {
            iID_MaDotCapPhat = DateTime.Now.ToShortDateString();
        //    if (dtDotCapPhat.Rows.Count > 0)
        //    {
        //        iID_MaDotCapPhat = dtDotCapPhat.Rows[0]["iID_MaDotCapPhat"].ToString();
        //        dNgayCapPhat = dtDotCapPhat.Rows[0]["dNgayCapPhat"].ToString();
        //    }
        //    else
        //    {
        //        iID_MaDotCapPhat = Guid.Empty.ToString();
        //    }
        }
        //dtDotCapPhat.Dispose();

        String MaTien = Convert.ToString(ViewData["MaTien"]);
        if(String.IsNullOrEmpty(MaTien))
        {
            MaTien = "0";
        }
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptQLDA_04_CP", new { iID_MaDotCapPhat = iID_MaDotCapPhat, MaND = MaND, MaTien = MaTien });
        String URL = Url.Action("Index", "QLDA_Report");
        using (Html.BeginForm("EditSubmit", "rptQLDA_04_CP", new { ParentID = ParentID }))
        {
    %>
     
         <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo chi tiết cấp phát vốn</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
          <div id="Div2">
            <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">                
                        <tr>
                        <td width="30%"></td>
                         <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn ngày:")%></div>                        </td>
                        <td width="10%">
                              <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, iID_MaDotCapPhat, "iID_MaDotCapPhat", "", "class=\"input1_2\" style=\"width: 80%\"")%> </div>
                                 </td>  
                           <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn loại tiền:")%></div></td>
                        <td width="10%" id="<%= ParentID %>_LoaiTien">                            
                            <% rptQLDA_03_CPController rpt = new rptQLDA_03_CPController();%>                
                             <%=rpt.obj_QLDA(ParentID, iID_MaDotCapPhat, MaND, MaTien)%>                           
                          </td>                             
                         <td></td>
                    </tr>
					 <tr>
                      <td></td>
                      <td colspan="4"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
                      <td></td>
                    </tr>
                 </table>
            </div>
        </div>
        <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQLDA_04_CP", new { iID_MaDotCapPhat = iID_MaDotCapPhat, MaND = MaND, MaTien = MaTien }), "xuat excel") %>
    </div> 
    <%} %>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=URL %>';
         }
    </script>
      <script type="text/javascript">
          function ChonDot() {
              var iID_MaDotCapPhat = document.getElementById("<%=ParentID %>_iID_MaDotCapPhat").value;
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("ds_QLDA?ParentID=#0&iID_MaDotCapPhat=#1&MaND=#2&MaTien=#3","rptQLDA_03_CP") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", iID_MaDotCapPhat));
              url = unescape(url.replace("#2", "<%= MaND %>"));
              url = unescape(url.replace("#3", "<%= MaTien %>"));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID%>_LoaiTien").innerHTML = data
              });
          }                                            
     </script>
      <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
