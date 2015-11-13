<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QLDA" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>
    </title>
</head>
<body>
    <%   
        String ParentID = "QLDA";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String dNgay = Convert.ToString(ViewData["dNgay"]);
        if (String.IsNullOrEmpty(dNgay))
            dNgay= DateTime.Now.ToString("dd/MM/yyyy");
        String MaTien = Convert.ToString(ViewData["MaTien"]);
        if (String.IsNullOrEmpty(MaTien))
        {
            PageLoad = "0";
        }
        String sDeAn = Convert.ToString(ViewData["sDeAn"]);
        if (String.IsNullOrEmpty(sDeAn))
        {
            PageLoad = "0";
        }
        String iCapTongHop = Convert.ToString(ViewData["iCapTongHop"]);
        if (String.IsNullOrEmpty(iCapTongHop))
        {
            iCapTongHop = "5";
        }
        String iID_MaLoaiKeHoachVon = Convert.ToString(ViewData["iID_MaLoaiKeHoachVon"]);
        DataTable dtLoaiKeHoachVon = QLDA_ReportModel.dt_LoaiKeHoachVon();
        if(String.IsNullOrEmpty(iID_MaLoaiKeHoachVon))
        {
            if (dtLoaiKeHoachVon.Rows.Count > 0 && dtLoaiKeHoachVon != null)
                iID_MaLoaiKeHoachVon = dtLoaiKeHoachVon.Rows[0]["iID_MaLoaiKeHoachVon"].ToString();
            else iID_MaLoaiKeHoachVon = "1";
        }
        SelectOptionList slLoaiKHV = new SelectOptionList(dtLoaiKeHoachVon, "iID_MaLoaiKeHoachVon", "sTen");
        dtLoaiKeHoachVon.Dispose();
        
        //sLNS
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        
        if (String.IsNullOrEmpty(sLNS))
        {
            DataTable dtLNS = QLDA_ReportModel.dt_LoaiNganSachKHV(dNgay, sDeAn);
            if (dtLNS.Rows.Count > 0 && dtLNS != null) sLNS = dtLNS.Rows[0]["sLNS"].ToString();
            else
                PageLoad = "0";
        }
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptQLDA_ThongBaoKHV", new { dNgay = dNgay, sDeAn = sDeAn, MaTien = MaTien, iCapTongHop = iCapTongHop, iID_MaLoaiKeHoachVon = iID_MaLoaiKeHoachVon,sLNS=sLNS });
        String URL = Url.Action("Index", "QLDA_Report");
        using (Html.BeginForm("EditSubmit", "rptQLDA_ThongBaoKHV", new { ParentID = ParentID }))
        {
    %>
     
         <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td width="50%">
                	    <span>Báo cáo thông báo kế hoạch vốn</span>
                    </td>
                    <td width="50%" align="left">
                        <div class="login12"><a href="#">Ẩn/Hiện form lọc báo cáo</a></div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
          <div id="Div2">
            <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">                
                        <tr>
                        <td width="1%"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Chọn ngày:")%></div>
                        </td>
                             <td style="width: 10%;">
                                <div><%=MyHtmlHelper.DatePicker(ParentID, dNgay, "dNgay", "", "onchange=\"ChonThang()\"")%></div>
                            </td>              
                       <td width="20%" id="<%= ParentID %>_DeAn">
                            <% rptQLDA_ThongBaoKHVController rpt = new rptQLDA_ThongBaoKHVController();
                               rptQLDA_ThongBaoKHVController.QLDA_ThongBaoKHV _Data = new rptQLDA_ThongBaoKHVController.QLDA_ThongBaoKHV();
                               _Data = rpt.obj_QLDA(ParentID,dNgay,MaTien,sDeAn,sLNS);
                            %>
                              <%=_Data.DeAn%>
                            </td> 
                                   <td style="width: 30%;">
                            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                            		<td class="td_form2_td1" style="width: 40%;"><div><%=NgonNgu.LayXau("Chọn LNS:")%></div></td>
                                    <td id="<%= ParentID %>_sLNS">                      
                              <%=_Data.sLNS%>
                            </td>
                            	</tr>
                            	<tr>
                            		<td class="td_form2_td1" style="width: 40%;"><div><%=NgonNgu.LayXau("Chọn loại tiền:")%></div></td>
                                     <td id="<%= ParentID %>_Tien">                           
                              <%=_Data.NgoaiTe%>
                            </td>
                            	</tr>
                                <tr>
                            		<td class="td_form2_td1" style="width: 40%;"><div><%=NgonNgu.LayXau("Chọn loại KHV:")%></div></td>
                                     <td>                           
                                        <%=MyHtmlHelper.DropDownList(ParentID, slLoaiKHV, iID_MaLoaiKeHoachVon, "iID_MaLoaiKeHoachVon","","class=\"input1_2\" style=\"width: 80%\"")%>
                                    </td>
                            	</tr>
                            </table>
                                 </td>
                             <td width="20%"><div>
                                <fieldset>
                                    <legend><b>Tổng hợp tới cấp</b></legend>
                                    <table>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Đề án:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "0", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Dự án:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "1", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Dự án thành phần:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "2", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Công trình:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "3", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    HM Công trình:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "4", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    HM Chi Tiết:</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "5", iCapTongHop, "iCapTongHop", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            </td>
                         <td></td>
                    </tr>
                    <tr>
                    <td></td>
                    
                    </tr>
					 <tr>
                      <td></td>
                      <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
    </div> 
    <%} %>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=URL %>';
         }
    </script>
    <script type="text/javascript">
        $(function () {
            $('div.login12 a').click(function () {
                $('div#Div1').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });

        });
    </script>
      <script type="text/javascript">
          function ChonThang() {
              var dNgay = document.getElementById("<%=ParentID %>_vidNgay").value;
              var sDeAn = "";
              $("input:checkbox[check-group='MaDeAn']").each(function (i) {
                  if (this.checked) {
                      if (sDeAn != "") sDeAn += ",";
                      sDeAn += this.value;
                  }
              });
              var sLNS = document.getElementById("<%=ParentID %>_sLNS").value;
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("ds_QLDA?ParentID=#0&dNgay=#1&MaTien=#2&sDeAn=#3&sLNS=#4","rptQLDA_ThongBaoKHV") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", dNgay));
              url = unescape(url.replace("#2", "<%= MaTien %>"));
              url = unescape(url.replace("#3", sDeAn));
              url = unescape(url.replace("#4", sLNS));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID%>_DeAn").innerHTML = data.DeAn;
                  document.getElementById("<%= ParentID%>_Tien").innerHTML = data.NgoaiTe;
                  document.getElementById("<%= ParentID%>_sLNS").innerHTML = data.sLNS;
              });
          }                                            
      </script>
      <script type="text/javascript">
          function ChonDeAn() {
              var dNgay = document.getElementById("<%=ParentID %>_vidNgay").value;
              var sDeAn = "";
              $("input:checkbox[check-group='MaDeAn']").each(function (i) {
                  if (this.checked) {
                      if (sDeAn != "") sDeAn += ",";
                      sDeAn += this.value;
                  }
              });
              var sLNS = document.getElementById("<%=ParentID %>_sLNS").value;
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("ds_QLDA?ParentID=#0&dNgay=#1&MaTien=#2&sDeAn=#3&sLNS=#4","rptQLDA_ThongBaoKHV") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", dNgay));
              url = unescape(url.replace("#2", "<%= MaTien %>"));
              url = unescape(url.replace("#3", sDeAn));
              url = unescape(url.replace("#4", sLNS));
              $.getJSON(url, function (data) {
                  //document.getElementById("<%= ParentID%>_DeAn").innerHTML = data.DeAn;
                  document.getElementById("<%= ParentID%>_Tien").innerHTML = data.NgoaiTe;
                  document.getElementById("<%= ParentID%>_sLNS").innerHTML = data.sLNS;
              });
          }                                            
      </script>
     <script type="text/javascript">
         function Chonall(sDeAn) {
             $("input:checkbox[check-group='MaDeAn']").each(function (i) {
                 if (sDeAn) {
                     this.checked = true;
                 }
                 else {
                     this.checked = false;
                 }
             }
             );
             ChonDeAn();
         }                                            
    </script>
      <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
