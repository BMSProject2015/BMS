<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 24px;
        }
        .style2
        {
            width: 5%;
            height: 24px;
        }
        .style3
        {
            width: 5%;
            height: 33px;
        }
        .style4
        {
            width: 10%;
            height: 33px;
        }
        .style5
        {
            height: 33px;
        }
        .style6
        {
            width: 8%;
            height: 33px;
        }
    </style>
</head>
<body>
    <%        
        
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_ThuongXuyen_34CController.tbTrangThai();
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
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        if (string.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "2";
        }
        dtKhoGiay.Dispose();
        String RutGon = Convert.ToString(ViewData["RutGon"]);
        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if (String.IsNullOrEmpty(ToSo))
        {
            PageLoad = "0";
            ToSo = "1";
        }
        String ToDaXem = "";
        ToDaXem = Convert.ToString(Request.QueryString["ToDaXem"]);
        ToDaXem += ToSo + ",";
        if (ToDaXem == ",") ToDaXem = "";
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        if(String.IsNullOrEmpty(iID_MaDonVi))
        {
            iID_MaDonVi = Guid.Empty.ToString();
            PageLoad = "0";
        }
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });
        String urlReport = "";
       
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_ThuongXuyen_34E", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, RutGon = RutGon, KhoGiay = KhoGiay, ToSo = ToSo, MaND = MaND,iID_MaDonVi=iID_MaDonVi });
        }
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_ThuongXuyen_34E", new { ParentID = ParentID,ToDaXem=ToDaXem }))
        {
    %>
         <div class="box_tong">
            <div class="title_tong">
                 <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td width="47.9%">
                            <span>Báo cáo tổng hợp TX chọn đơn vị </span>
                     
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
                        <td class="style3">
                            </td>
                            <td class="style4">
                                <div>
                                Trạng Thái 
                                </div>
                            </td>
                            <td class="style4">
                                <div>
                                 <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>                                                        
                                </div>
                            </td>							
                             <td class="style4">
                                <div style="text-align:right">
                                     Chọn Tháng:
                                </div>
                            </td>
                            <td class="style3">
                                <div>
                                  <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "Thang_Quy", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>                      
                                </div>
                            </td>
							 <td width="5%" class="style5"></td>                       
                            <td style="width: 25%;" rowspan="3" id="<%= ParentID %>_tdDonVi">
                                 <%rptQuyetToan_ThuongXuyen_34EController.data data = new rptQuyetToan_ThuongXuyen_34EController.data();
                                   rptQuyetToan_ThuongXuyen_34EController rpt = new rptQuyetToan_ThuongXuyen_34EController();
                                   data = rpt.obj_DonVi(ParentID, iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, KhoGiay, ToSo, iID_MaDonVi);
                                %> 
                                <%=data.iID_MaDonVi%>
                            </td>
                              <td class="style6">
                                <div style="text-align:right;">
                                     Chọn Tờ:
                                </div>
                            </td>
                            <td id="<%= ParentID %>_tdToSo" class="style3">
                                <div>
                                <%=data.ToSo%>
                                </div>
                            </td>
                           <td class="style5" >
                           </td>                       
                        </tr>
						 <tr>
    <td class="style1"></td>
    <td class="style1"><div>Khổ giấy in : </div></td>
    <td class="style1"><div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonTo()\" ")%>                      
                                </div></td>
    <td class="style2">
                                <div style="text-align:right;">
                                Rút gọn:
                                </div>
                            </td>
                            <td class="style2">                   
                                <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, RutGon, "RutGon", "", "onclick=\"Chon()\"")%>
                                </div>
                            </td>  
                            <Td class="style1"></Td>
   <td class="style1" colspan="2"><div style="padding-left:70px;">
                               Tờ đã xem:</div>
                    <%=ToDaXem %>
                             </td>
    <td class="style1"></td>
  </tr>
                        <tr>                                               
                            <td>&nbsp;</td>
    					                                                                                   
                            <td colspan="5">
                       			<table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 10px; width: 100%;" >
                                <tr>
                                    <td style="width: 48%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="4%">
                                    </td>
                                  <td style="width: 48%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            	</table>
                            </td>
                            <td></td>
    					    <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    <%} %>
    <script type="text/javascript">
        $(function () {
            $("div#rptMain").hide();
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });       
    </script>
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
            ChonTo();
        }                                            
    </script>
     <script type="text/javascript">
         function Chon() {
             var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
             var Thang_Quy = document.getElementById("<%=ParentID %>_Thang_Quy").value;
             var KhoGiay = document.getElementById("<%=ParentID %>_KhoGiay").value;
             var bRutGon = document.getElementById("<%=ParentID %>_RutGon").checked;
             var RutGon = "";
             if (bRutGon) RutGon = "on";
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&RutGon=#3&MaND=#4&KhoGiay=#5&ToSo=#6&iID_MaDonVi=#7", "rptQuyetToan_ThuongXuyen_34E") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#2", Thang_Quy));
             url = unescape(url.replace("#3", RutGon));
             url = unescape(url.replace("#4", "<%= MaND %>"));
             url = unescape(url.replace("#5", KhoGiay));
             url = unescape(url.replace("#6", "<%= ToSo %>"));
             url = unescape(url.replace("#7", "<%= iID_MaDonVi %>"));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
                 document.getElementById("<%= ParentID %>_tdToSo").innerHTML = data.ToSo;
             });
         }                                            
     </script>
      <script type="text/javascript">
          function ChonTo() {
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
              var Thang_Quy = document.getElementById("<%=ParentID %>_Thang_Quy").value;
              var KhoGiay = document.getElementById("<%=ParentID %>_KhoGiay").value;
              var bRutGon = document.getElementById("<%=ParentID %>_RutGon").checked;
              var RutGon = "";
              if (bRutGon) RutGon = "on";
              var iID_MaDonVi = "";
              $("input:checkbox[check-group='MaDonVi']").each(function (i) {
                  if (this.checked) {
                      if (iID_MaDonVi != "") iID_MaDonVi += ",";
                      iID_MaDonVi += this.value;
                  }
              });
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&RutGon=#3&MaND=#4&KhoGiay=#5&ToSo=#6&iID_MaDonVi=#7", "rptQuyetToan_ThuongXuyen_34E") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
              url = unescape(url.replace("#2", Thang_Quy));
              url = unescape(url.replace("#3", RutGon));
              url = unescape(url.replace("#4", "<%= MaND %>"));
              url = unescape(url.replace("#5", KhoGiay));
              url = unescape(url.replace("#6", "<%= ToSo %>"));
              url = unescape(url.replace("#7", iID_MaDonVi));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_tdToSo").innerHTML = data.ToSo;
              });
          }                                            
      </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_ThuongXuyen_34E", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, RutGon = RutGon, KhoGiay = KhoGiay, ToSo = ToSo, MaND = MaND, iID_MaDonVi = iID_MaDonVi }), "Xuất ra Excel")%>
    <iframe src="<%=urlReport%>"
height="600px" width="100%"></iframe>
</body>
</html>
