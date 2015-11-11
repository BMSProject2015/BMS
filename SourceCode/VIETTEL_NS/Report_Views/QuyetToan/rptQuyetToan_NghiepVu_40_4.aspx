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
      <link href="../../Content/style.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
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
        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_NghiepVu_40_4Controller.tbTrangThai();
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
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSachNghiepVuNhaNuoc_PhongBan(iID_MaPhongBan);
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        if (String.IsNullOrEmpty(sLNS))
        {
            if (dtLNS.Rows.Count > 0)
            {
                sLNS = Convert.ToString(dtLNS.Rows[0]["sLNS"]);
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }

        }
        String[] arrLNS = sLNS.Split(',');
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
        dtLNS.Dispose();
        String RutGon = Convert.ToString(ViewData["RutGon"]);
        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if (String.IsNullOrEmpty(ToSo))
        {
            PageLoad = "0";
            ToSo = "1";
        }
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        if (string.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "2";
        }
        dtKhoGiay.Dispose();
        
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            iID_MaDonVi = Guid.Empty.ToString();
            PageLoad = "0";
        }
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });
        String urlReport = "";
        String ToDaXem = "";
        ToDaXem = Convert.ToString(Request.QueryString["ToDaXem"]);
        ToDaXem += ToSo + ",";
        if (ToDaXem == ",") ToDaXem = "";
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_nghiepvu_40_4", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, Thang_Quy = Thang_Quy, TruongTien = TruongTien, RutGon = RutGon, MaND=MaND,ToSo=ToSo,KhoGiay=KhoGiay });
        }
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = "1" });
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_nghiepvu_40_4", new { ParentID = ParentID, ToDaXem = ToDaXem }))
        {
    %>
         <div class="box_tong">
            <div class="title_tong">
                 <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                        <span>Báo cáo tổng hợp quyết toán quý chọn đơn vị</span>
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
                        <td style="width: 2%;">
                            </td>
                            <td class="td_form2_td1" style="width: 7%;">
                                <div>
                               Trạng Thái :
                                </div>
                            </td>
                            <td style="width: 12%;">
                                <div>
                                 <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>                                                        
                                </div>
                            </td>
                            <td class="td_form2_td1" style="width: 7%;">
                                <div>
                                     Chọn Quý:
                                </div>
                            </td>
                            <td style="width: 7%;">
                                <div>
                                  <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "Thang_Quy", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>                      
                                </div>
                            </td>    
                              <td width="6%" align="right">LNS:</td>                        
                            <td style="width: 20%;" rowspan="20">
                            <div style="width: 100%; height: 500px; overflow: scroll; border: 1px solid black;">
                                <table class="mGrid">
                                <tr>
                                  <td>&nbsp;<input type="checkbox" id="checkLNS"  onclick="ChonLNSall(this.checked)" ></td><td> &nbsp; Chọn tất cả LNS </td>
                                </tr>
                                    <%
                                    String TenLNS = ""; String sLNS1 = "";
                                    String _Checked = "checked=\"checked\"";  
                                    for (int i = 0; i < dtLNS.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        TenLNS = Convert.ToString(dtLNS.Rows[i]["TenHT"]);
                                        sLNS1 = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                                        for (int j = 0; j < arrLNS.Length; j++)
                                        {
                                            if (sLNS1 == arrLNS[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="sLNS" onchange="Chon()"
                                                name="sLNS" />
                                        </td>
                                        <td>
                                            <%=TenLNS%>
                                        </td>
                                    </tr>
                                  <%}%>
                                </table>
                            </div>
                            </td>
                             <td width="6%" align="right">Đơn vị:</td>                       
                            <td style="width: 20%;" rowspan="19" id="<%= ParentID %>_tdDonVi">
                                 <%rptQuyetToan_NghiepVu_40_4Controller.data data = new rptQuyetToan_NghiepVu_40_4Controller.data();
                                   rptQuyetToan_NghiepVu_40_4Controller rpt = new rptQuyetToan_NghiepVu_40_4Controller();
                                   data = rpt.obj_DonVi(ParentID, iID_MaTrangThaiDuyet, Thang_Quy, RutGon, MaND, KhoGiay, ToSo,sLNS,TruongTien,iID_MaDonVi);
                                %> 
                                <%=data.iID_MaDonVi%>
                            </td>
                           <td style="width: 10%;">
                            </td>                       
                        </tr>
                        <tr>
                         <td>
                            </td> 
                            <td class="td_form2_td1">
                                <div>
                               Trường Tiền:
                                </div>
                            </td>                                                                     
                            <td>                     
                               <div>                      
                                 <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "","onchange=Chon()")%>Tự Chi&nbsp;
                                 <%=MyHtmlHelper.Option(ParentID, "rHienVat",TruongTien, "TruongTien","","onchange=Chon()")%>Hiện Vật
                                </div>
                            </td>                                                    
                            <td class="td_form2_td1">
                                <div>
                                Rút gọn:
                                </div>
                            </td>
                            <td>                   
                                <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, RutGon, "RutGon", "", "onclick=\"Chon()\"")%>
                                </div>
                            </td>
                              <td>
                            </td>  
                         <td>
                            </td>                      
                        </tr>
                        <tr>
    <td>&nbsp;</td>
     <td class="td_form2_td1" style="width: 7%;">
                                <div>
                                Khổ giấy:
                                </div>
                            </td>
                            <td >
                                <div>
                                  <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonTo()\"")%>
                                </div>
                            </td>
                        <td class="td_form2_td1" style="width: 7%;">
                                <div>
                               Chọn tờ :
                                </div>
                            </td>
                            <td id="<%= ParentID %>_tdToSo">
                                <div>
                                 <%=data.ToSo %>
                                </div>
                            </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
  <td></td>
  <td></td>
<td></td>
 <td class="td_form2_td1"><div>
                               Tờ đã xem:</div></td>
 <td>
                               <%=ToDaXem %>
                                </td>
<td></td>
<td></td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
  <tr>
  <td>&nbsp;</td>
  <td>&nbsp;</td>
<td>&nbsp;</td>
 <td class="td_form2_td1">&nbsp;</td>
 <td>
                               &nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
  </tr>
                        <tr>                        
                        <td style="width: 10%;">
                            </td>                                                            
                         <td colspan="4">
                       			<table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 10px; width: 100%;" >
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
                         <td>&nbsp;</td>
    <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
         <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_NghiepVu_40_4", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, Thang_Quy = Thang_Quy, TruongTien = TruongTien, RutGon = RutGon, MaND = MaND, ToSo = ToSo, KhoGiay = KhoGiay }), "Xuất ra Excel")%>
    <%} %>
   <script type="text/javascript">
       function ChonallDV(sLNS) {
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
            window.location.href = '<%=BackURL%>';
        }
    </script>
       <script type="text/javascript">
           function ChonLNSall(sLNS) {
               $("input:checkbox[check-group='sLNS']").each(function (i) {
                   if (sLNS) {
                       this.checked = true;
                   }
                   else {
                       this.checked = false;
                   }
                 
               });
               Chon();
           }                                            
    </script>
     <script type="text/javascript">
         function Chon() {
             var Thang = document.getElementById("<%= ParentID %>_Thang_Quy").value;
             var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
             var sLNS = "";
             $("input:checkbox[check-group='sLNS']").each(function (i) {
                 if (this.checked) {
                     if (sLNS != "") sLNS += ",";
                     sLNS += this.value;
                 }
             });

             var bRutGon = document.getElementById("<%=ParentID %>_RutGon").checked;
             var RutGon = "";
             if (bRutGon) RutGon = "on";
             var TruongTienCheck = document.getElementById("<%= ParentID %>_TruongTien").checked;
             var TruongTien = "";
             if (TruongTienCheck) {
                 TruongTien = "rTuChi";
             }
             else {
                 TruongTien = "rHienVat";
             }
             var KhoGiay = document.getElementById("<%=ParentID %>_KhoGiay").value;
             var iID_MaDonVi = "";
             $("input:checkbox[check-group='MaDonVi']").each(function (i) {
                 if (this.checked) {
                     if (iID_MaDonVi != "") iID_MaDonVi += ",";
                     iID_MaDonVi += this.value;
                 }
             });
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&RutGon=#3&MaND=#4&KhoGiay=#5&ToSo=#6&sLNS=#7&TruongTien=#8&iID_MaDonVi=#9", "rptQuyetToan_NghiepVu_40_4") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#2", Thang));
             url = unescape(url.replace("#3", RutGon));
             url = unescape(url.replace("#4", "<%= MaND %>"));
             url = unescape(url.replace("#5", KhoGiay));
             url = unescape(url.replace("#6", "<%= ToSo %>"));
             url = unescape(url.replace("#7", sLNS));
             url = unescape(url.replace("#8", TruongTien));
             url = unescape(url.replace("#9", iID_MaDonVi));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
                 document.getElementById("<%= ParentID %>_tdToSo").innerHTML = data.ToSo;
             });
         }                                           
     </script>
 <script type="text/javascript">
     function ChonTo() {
         var Thang = document.getElementById("<%= ParentID %>_Thang_Quy").value;
         var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
         var sLNS = "";
         $("input:checkbox[check-group='sLNS']").each(function (i) {
             if (this.checked) {
                 if (sLNS != "") sLNS += ",";
                 sLNS += this.value;
             }
         });
         var iID_MaDonVi = "";
         $("input:checkbox[check-group='MaDonVi']").each(function (i) {
             if (this.checked) {
                 if (iID_MaDonVi != "") iID_MaDonVi += ",";
                 iID_MaDonVi += this.value;
             }
         });
         var bRutGon = document.getElementById("<%=ParentID %>_RutGon").checked;
         var RutGon = "";
         if (bRutGon) RutGon = "on";
         var TruongTienCheck = document.getElementById("<%= ParentID %>_TruongTien").checked;
         var TruongTien = "";
         if (TruongTienCheck) {
             TruongTien = "rTuChi";
         }
         else {
             TruongTien = "rHienVat";
         }
         var KhoGiay = document.getElementById("<%=ParentID %>_KhoGiay").value;

         jQuery.ajaxSetup({ cache: false });
         var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&RutGon=#3&MaND=#4&KhoGiay=#5&ToSo=#6&sLNS=#7&TruongTien=#8&iID_MaDonVi=#9", "rptQuyetToan_NghiepVu_40_4") %>');
         url = unescape(url.replace("#0", "<%= ParentID %>"));
         url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
         url = unescape(url.replace("#2", Thang));
         url = unescape(url.replace("#3", RutGon));
         url = unescape(url.replace("#4", "<%= MaND %>"));
         url = unescape(url.replace("#5", KhoGiay));
         url = unescape(url.replace("#6", "<%= ToSo %>"));
         url = unescape(url.replace("#7", sLNS));
         url = unescape(url.replace("#8", TruongTien));
         url = unescape(url.replace("#9", iID_MaDonVi));
         $.getJSON(url, function (data) {
             document.getElementById("<%= ParentID %>_tdToSo").innerHTML = data.ToSo;
         });
     }                                           
 </script>
    <iframe src="<%=urlReport%>"
height="600px" width="100%"></iframe>
</body>
</html>
