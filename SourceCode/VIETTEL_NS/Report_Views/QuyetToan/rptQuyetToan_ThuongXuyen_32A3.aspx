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
    <style type="text/css">
        .style1
        {
            height: 19px;
        }
    </style>
</head>
<body>
  <%  

      String ParentID = "QuyetToan";
      String MaND = User.Identity.Name;
      String PageLoad = Convert.ToString(ViewData["PageLoad"]);
      String Thang = "0", Quy = "0";
      String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
      String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
      String ToSo = Convert.ToString(ViewData["ToSo"]);
      if (String.IsNullOrEmpty(ToSo))
      {

          PageLoad = "0";
          ToSo = "1";
      }
      if (String.IsNullOrEmpty(LoaiThang_Quy))
      {
          LoaiThang_Quy = "1";
      }
      if (LoaiThang_Quy == "0")
      {
          Thang = Thang_Quy;
          Quy = "0";
      }
      else
      {
          Thang = "0";
          Quy = Thang_Quy;
      }
      String TongHop = Convert.ToString(ViewData["TongHop"]);
      
      if (String.IsNullOrEmpty(Thang_Quy))
      {
          Thang_Quy = "0";
      }
      String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
      DataTable dtTrangThai = rptQuyetToan_ThuongXuyen_32A3Controller.tbTrangThai();
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

      DataTable dtQuy = DanhMucModels.DT_Quy();
      SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
      dtQuy.Dispose();

      DataTable dtThang = DanhMucModels.DT_Thang();
      SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
      dtThang.Dispose();
      String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
     
      DataTable dtDonVi = rptQuyetToan_ThuongXuyen_32A3Controller.LayDSDonVi(iID_MaTrangThaiDuyet,Thang_Quy,LoaiThang_Quy,MaND);
      if (String.IsNullOrEmpty(iID_MaDonVi))
      {
          if (dtDonVi.Rows.Count > 0)
          {
              iID_MaDonVi = dtDonVi.Rows[0]["iID_MaDonVi"].ToString();
          }
          else
          {
              iID_MaDonVi = Guid.Empty.ToString();
          }
      }
      String[] arrDonVi = iID_MaDonVi.Split(',');     
      SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
      dtDonVi.Dispose();

      String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
      DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
      SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
      if (String.IsNullOrEmpty(KhoGiay))
      {
          KhoGiay = "2";
      }
      dtKhoGiay.Dispose();
      String ToDaXem = "";
      ToDaXem = Convert.ToString(Request.QueryString["ToDaXem"]);
      ToDaXem += ToSo + ",";
      if (ToDaXem == ",") ToDaXem = "";
      String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });
     
      String urlReport = "";
      if (PageLoad.Equals("1"))
      {
          urlReport = Url.Action("ViewPDF", "rptQuyetToan_ThuongXuyen_32A3", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, KhoGiay = KhoGiay,MaND=MaND,ToSo=ToSo });
      }
      using (Html.BeginForm("EditSubmit", "rptQuyetToan_ThuongXuyen_32A3", new { ParentID = ParentID, ToDaXem = ToDaXem }))
      {
      %> 
         <div class="box_tong">
          <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                     <td width="47.9%">
                        <span>Báo cáo tổng hợp 3 yếu tố chọn đơn vị</span>
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
                    <td width="15%" rowspan="16"></td>
                        <td class="td_form2_td1" width="10%">
                            <div>
                               Trạng Thái :</div>
                        </td>
                        <td width="25%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 50%\" onchange=Chon()")%>
                            </div>
                        </td>
                       
                        <td class="td_form2_td1" style="width: 10%;" rowspan="14">
                            <div>
                                <%=NgonNgu.LayXau("Chọn đơn vị:")%></div>
                        </td>
                        <td rowspan="14" width="30%" id="<%= ParentID %>_tdDonVi" style="height:350px;">
                                 <%rptQuyetToan_ThuongXuyen_32A3Controller rpt = new rptQuyetToan_ThuongXuyen_32A3Controller();
                                   rptQuyetToan_ThuongXuyen_32A3Controller.data _data = new rptQuyetToan_ThuongXuyen_32A3Controller.data();
                                   _data = rpt.obj_DonVi(ParentID, iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, MaND, KhoGiay, ToSo);                      
          %> 
                                <%=_data.iID_MaDonVi%>
                        </td>
                        <td rowspan="14"></td>
                    </tr>                                      
                    <tr>   
                          <td class="td_form2_td1">
                            <div>
                                Tháng/Quý:</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", " class=\"input1_2\" style=\"width:10%;\" onchange=Chon()")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "", "class=\"input1_2\" style=\"width:25%;\" onchange=Chon() ")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "class=\"input1_2\" style=\"width:10%;\" onchange=Chon()")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "class=\"input1_2\" style=\"width:25%;\" onchange=Chon()")%><br />
                            </div>
                        </td>  
                    </tr>
                    <tr>
                        <td class="td_form2_td1"><div>
                                Khổ Giấy:</div></td>
                        <td>
                        <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 50%\" onchange=ChonTo()")%></td>
                    </tr>
                     <tr>
                        <td class="td_form2_td1"><div>
                               Chọn tờ:</div></td>
                        <td id="<%= ParentID %>_tdTo"><%=_data.ToSo %></td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">Tờ đã xem:</td>
                        <td><%=ToDaXem %></td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">&nbsp;</td>
                        <td>&nbsp;</td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">&nbsp;</td>
                        <td>&nbsp;</td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">&nbsp;</td>
                        <td>&nbsp;</td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">&nbsp;</td>
                        <td>&nbsp;</td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">&nbsp;</td>
                        <td>&nbsp;</td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">&nbsp;</td>
                        <td>&nbsp;</td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">&nbsp;</td>
                        <td>&nbsp;</td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">&nbsp;</td>
                        <td>&nbsp;</td>
                         
                      
                    </tr>
                     <tr>
                        <td class="td_form2_td1">&nbsp;</td>
                        <td>&nbsp;</td>
                         
                      
                    </tr>
                    <tr>
                     <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>&nbsp;</td>
                    </tr>
                    <tr>
                     <td class="td_form2_td1"><div>
                               </div></td>
                    <td>
                   
                    </td>
                    <td></td>
                    </tr>
                    <tr>
                    <td></td>
                    <td colspan="4"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
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
          $("div#rptMain").hide();
          $('div.login1 a').click(function () {
              $('div#rptMain').slideToggle('normal');
              $(this).toggleClass('active');
              return false;
          });
      });       
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
             var LoaiThang_QuyCheck = document.getElementById("<%= ParentID %>_LoaiThang_Quy").checked;
             var LoaiThang_Quy
             var Thang
             if (LoaiThang_QuyCheck == true) {
                 Thang = document.getElementById("<%= ParentID %>_iThang").value;
                 LoaiThang_Quy = 0;
             }
             else {
                 Thang = document.getElementById("<%= ParentID %>_iQuy").value;
                 LoaiThang_Quy = 1;
             }
             var iID_MaDonVi = "";
             $("input:checkbox[check-group='MaDonVi']").each(function (i) {
                 if (this.checked) {
                     if (iID_MaDonVi != "") iID_MaDonVi += ",";
                     iID_MaDonVi += this.value;
                 }
             });
             var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
             var KhoGiay = document.getElementById("<%= ParentID %>_KhoGiay").value;
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&LoaiThang_Quy=#3&iID_MaDonVi=#4&MaND=#5&KhoGiay=#6&ToSo=#7", "rptQuyetToan_ThuongXuyen_32A3") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#2", Thang));
             url = unescape(url.replace("#3", LoaiThang_Quy));
             url = unescape(url.replace("#4", iID_MaDonVi));
             url = unescape(url.replace("#5", "<%= MaND %>"));
             url = unescape(url.replace("#6", KhoGiay));
             url = unescape(url.replace("#7", "<%= ToSo %>"));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
                 document.getElementById("<%= ParentID %>_tdTo").innerHTML = data.ToSo;
             });
         }                                            
     </script>
      <script type="text/javascript">
          function ChonTo() {
              var LoaiThang_QuyCheck = document.getElementById("<%= ParentID %>_LoaiThang_Quy").checked;
              var LoaiThang_Quy
              var Thang
              if (LoaiThang_QuyCheck == true) {
                  Thang = document.getElementById("<%= ParentID %>_iThang").value;
                  LoaiThang_Quy = 0;
              }
              else {
                  Thang = document.getElementById("<%= ParentID %>_iQuy").value;
                  LoaiThang_Quy = 1;
              }
              var iID_MaDonVi = "";
              $("input:checkbox[check-group='MaDonVi']").each(function (i) {
                  if (this.checked) {
                      if (iID_MaDonVi != "") iID_MaDonVi += ",";
                      iID_MaDonVi += this.value;
                  }
              });
              var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
              var KhoGiay = document.getElementById("<%= ParentID %>_KhoGiay").value;
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&LoaiThang_Quy=#3&iID_MaDonVi=#4&MaND=#5&KhoGiay=#6&ToSo=#7", "rptQuyetToan_ThuongXuyen_32A3") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
              url = unescape(url.replace("#2", Thang));
              url = unescape(url.replace("#3", LoaiThang_Quy));
              url = unescape(url.replace("#4", iID_MaDonVi));
              url = unescape(url.replace("#5", "<%= MaND %>"));
              url = unescape(url.replace("#6", KhoGiay));
              url = unescape(url.replace("#7", "<%= ToSo %>"));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_tdTo").innerHTML = data.ToSo;
              });
          }                                            
     </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_ThuongXuyen_32A3", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, KhoGiay = KhoGiay, MaND = MaND, ToSo = ToSo }), "Xuất ra Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
