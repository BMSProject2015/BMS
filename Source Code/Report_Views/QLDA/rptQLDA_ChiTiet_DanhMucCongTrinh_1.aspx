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

        String sSoPheDuyetDauTu = Convert.ToString(ViewData["sSoPheDuyetDauTu"]);
        if (String.IsNullOrEmpty(sSoPheDuyetDauTu))
        {
            sSoPheDuyetDauTu = "-1111111";
        }
        String sSoPheDuyetDuToan = Convert.ToString(ViewData["sSoPheDuyetDuToan"]);
        if (String.IsNullOrEmpty(sSoPheDuyetDuToan))
        {
            sSoPheDuyetDuToan = "-1111111";
        }
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
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptQLDA_ChiTiet_DanhMucCongTrinh_1", new { MaTien = MaTien,sDeAn=sDeAn,sSoPheDuyetDauTu=sSoPheDuyetDauTu,sSoPheDuyetDuToan=sSoPheDuyetDuToan, iCapTongHop = iCapTongHop });
        String URL = Url.Action("Index", "QLDA_Report");
        using (Html.BeginForm("EditSubmit", "rptQLDA_ChiTiet_DanhMucCongTrinh_1", new { ParentID = ParentID }))
        {
    %>
     
         <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td width="50%">
                	    <span>Báo cáo danh mục công trình</span>
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
                        <td width="10%"></td>
                                  
                       <td width="20%" id="<%= ParentID %>_DeAn">
                            <% rptQLDA_ChiTiet_DanhMucCongTrinh_1Controller rpt = new rptQLDA_ChiTiet_DanhMucCongTrinh_1Controller();
                               rptQLDA_ChiTiet_DanhMucCongTrinh_1Controller.QLDA_DMCT _Data = new rptQLDA_ChiTiet_DanhMucCongTrinh_1Controller.QLDA_DMCT();
                               _Data = rpt.obj_QLDA(ParentID, sDeAn, sSoPheDuyetDauTu, sSoPheDuyetDuToan, MaTien);
                            %>
                              <%=_Data.DeAn%>
                            </td> 
                                  
                        <td  width="30%">
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        	<tr>
                            <td style="width: 40%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn số PD đầu tư:")%></div>
                        </td>
                                <td width="60%" id="_sSoDauTu">                           
                                  <%=_Data.SoDauTu%>
                                </td> 
                            </tr>
                            <tr>
                            <td  class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn số PD dự toán:")%></div>
                        </td>
                                <td id="_sSoDuToan">                           
                                  <%=_Data.SoDuToan%>
                                </td>
                            </tr>
                            <tr> 
                             <td class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn loại tiền:")%></div>
                        </td>
                        		<td  id="<%= ParentID %>_Tien">                           
                              <%=_Data.NgoaiTe%>
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
           function ChonDeAn() {
               var sDeAn = "";
               $("input:checkbox[check-group='MaDeAn']").each(function (i) {
                   if (this.checked) {
                       if (sDeAn != "") sDeAn += ",";
                       sDeAn += this.value;
                   }
               });
               jQuery.ajaxSetup({ cache: false });
               var url = unescape('<%= Url.Action("ds_QLDA?ParentID=#0&sDeAn=#1&sSoDauTu=#2&sSoDuToan=#3&MaTien=#4","rptQLDA_ChiTiet_DanhMucCongTrinh_1") %>');
               url = unescape(url.replace("#0", "<%= ParentID %>"));
               url = unescape(url.replace("#1", sDeAn));
               url = unescape(url.replace("#2", "<%= sSoPheDuyetDauTu %>"));
               url = unescape(url.replace("#3", "<%= sSoPheDuyetDuToan %>"));
               url = unescape(url.replace("#4", "<%= MaTien %>"));
               $.getJSON(url, function (data) {
                   document.getElementById("<%= ParentID%>_sSoDauTu").innerHTML = data.SoDauTu;
                   document.getElementById("<%= ParentID%>_sSoDuToan").innerHTML = data.SoDuToan;
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
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQLDA_ChiTiet_DanhMucCongTrinh_1", new { MaTien = MaTien, sDeAn = sDeAn, sSoPheDuyetDauTu = sSoPheDuyetDauTu, sSoPheDuyetDuToan = sSoPheDuyetDuToan, iCapTongHop = iCapTongHop }), "Xuất ra Excels")%>
      <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
