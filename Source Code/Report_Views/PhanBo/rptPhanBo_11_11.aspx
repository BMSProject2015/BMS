<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link href="../../Content/style.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
   <%  
        String MaND = User.Identity.Name;
        String ParentID = "TongHopChiTieu";
        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLoaiNganSach = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        if (String.IsNullOrEmpty(sLNS))
        {
            if (dtLoaiNganSach.Rows.Count > 0)
            {
                sLNS = Convert.ToString(dtLoaiNganSach.Rows[0]["sLNS"]);
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
        String[] arrLNS = sLNS.Split(',');
        dtLoaiNganSach.Dispose();

        //dt Trạng thái duyệt
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
        DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHePhanBo);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
       
       
        DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet, sLNS);
        SelectOptionList slDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
        String iID_MaDotPhanBo = Convert.ToString(ViewData["iID_MaDotPhanBo"]);
        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            if (dtDotPhanBo.Rows.Count > 1)
            {
                iID_MaDotPhanBo = Convert.ToString(dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"]);
            }
            else
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
        }
        dtDotPhanBo.Dispose();
        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }


        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        dtKhoGiay.Dispose();
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "2"; 
        }
    
        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if (String.IsNullOrEmpty(ToSo))
        {
            ToSo = "1";
        }
        String BackURL = Url.Action("Index", "PhanBo_Report");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptPhanBo_11_11", new { MaND=MaND,sLNS = sLNS, iID_MaDotPhanBo = iID_MaDotPhanBo, TruongTien = TruongTien, KhoGiay = KhoGiay, ToSo = ToSo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        using (Html.BeginForm("EditSubmit", "rptPhanBo_11_11", new { ParentID = ParentID }))
        {
    %>

 <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo phân bổ tổng hợp chỉ tiêu-Phân bổ</span>
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
                        <td width="10%"></td>
                         <td width="5%" class="td_form2_td1">LNS:</td>
                         <td style="width: 25%;" rowspan="26">
                            <div style="width: 100%; height: 500px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">
                                <tr>
                               <td><input type="checkbox" id="LNS" onclick="ChonLNS()"></td>
                                <td> Chọn tất cả LNS </td>
                                </tr>                      
                                    <%
String TenLNS = ""; String LNS = "";
String _Checked = "checked=\"checked\"";
for (int i = 0; i < dtLoaiNganSach.Rows.Count; i++)
{
    _Checked = "";
    TenLNS = Convert.ToString(dtLoaiNganSach.Rows[i]["TenHT"]);
    LNS = Convert.ToString(dtLoaiNganSach.Rows[i]["sLNS"]);
    for (int j = 0; j < arrLNS.Length; j++)
    {
        if (LNS == arrLNS[j])
        {
            _Checked = "checked=\"checked\"";
            break;
        }
    }                                                                                 
                                    %>
                                    <tr>
                                        <td style="width: 15%;">
                                            <input type="checkbox" value="<%=LNS %>" <%=_Checked %> check-group="sLNS" id="sLNS" onchange="Chon()"
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
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Trạng Thái: ")%></div>
                        </td>
                        <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn đợt phân bổ: ")%></div>
                        </td>
                         <td width="15%" id="<%= ParentID %>_tdDotPhanBo">
                             <% rptPhanBo_11_11Controller rpt = new rptPhanBo_11_11Controller();
                                rptPhanBo_11_11Controller.data _Data = new rptPhanBo_11_11Controller.data();
                                _Data=rpt.obj_DanhSachDotPhatBo(ParentID, MaND, iID_MaTrangThaiDuyet, sLNS, iID_MaDotPhanBo,TruongTien,KhoGiay,ToSo);%>                           
                               <%=_Data.iID_MaDotPhanBo %>
                        </td>
                        <td></td>
                      
                    </tr>
                   <tr>
                   <td></td>
                   <td></td>
                   <td class="td_form2_td1">
                            <div>
                                Khổ giấy:</div>
                        </td>
                        <td>
                            <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonDPB()\"")%>
                        </td>
                        <td class="td_form2_td1" style="width: 5%;">
                            <div>
                                <%=NgonNgu.LayXau("Trường Tiền: ")%></div>
                        </td>
                        <td>
                            <div>
                            <div>
                                 <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien","", "onchange=\"ChonDPB()\"")%>Tự Chi&nbsp;&nbsp;
                                 <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien","", "onchange=\"ChonDPB()\"")%>Hiện Vật
                            </div>
                            </div>
                        </td>
                   </tr>
                   <tr>
                   <td></td>
                   <td></td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            <div>
                                <%=NgonNgu.LayXau("Tờ số: ")%></div>
                        </td>
                       
                             <td width="10%" id="<%= ParentID %>_tdToSo">                          
                           <%=_Data.ToSo %>                    
                            </td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                          
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"      
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"               
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"               
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                  
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                     
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                    
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                  
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                    
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                          
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                        
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                         
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                          
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                        
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                      
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                        
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                          
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                          
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                        
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                       
                                 &nbsp;</td>
                        
                   </tr>
                   <tr>
                   <td>&nbsp;</td>
                   <td>&nbsp;</td>
                  
                        
                        
                        <td class="td_form2_td1" style="width: 5%;">
                            &nbsp;</td>
                       
                             <td width="10%"                         
                                 &nbsp;</td>
                        
                   </tr>
                    <tr>
                           <td>
                        </td> <td>
                        </td>                                      
                        
                        <td>
                        </td>
                         
                        <td colspan="2"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
<td>
                        </td>
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
            window.location.href = '<%=BackURL%>';
        }
    </script>
    <script type="text/javascript">
        function ChonLNS() {
            var sLNS = document.getElementById("LNS").checked;
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
             var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;
             
             var sLNS = "";
             $("input:checkbox[check-group='sLNS']").each(function (i) {
                 if (this.checked) {
                     if (sLNS != "") sLNS += ",";
                     sLNS += this.value;
                 }
             });
             $("input:checkbox[check-group='MaDV']").each(function (i) {
                 if (this.checked) {
                     if (iID_MaDonVi != "") iID_MaDonVi += ",";
                     iID_MaDonVi += this.value;
                 }
             });
             var TruongTien = document.getElementById("<%=ParentID%>_TruongTien").checked;
             var TenTruongTien = "";
             if (TruongTien == true)
                 TenTruongTien = "rTuChi";
             else
                 TenTruongTien = "rHienVat";
             var KhoGiay = document.getElementById("<%=ParentID%>_KhoGiay").value;
             var Toso = document.getElementById("<%=ParentID%>_ToSo").value;
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("ds_DotPhanBo?ParentID=#0&MaND=#1&iID_MaTrangThaiDuyet=#2&sLNS=#3&iID_MaDotPhanBo=#4&TruongTien=#5&KhoGiay=#6&ToSo=#7", "rptPhanBo_11_11") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", "<%=MaND %>"));
             url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#3", sLNS));
             url = unescape(url.replace("#4", "<%= iID_MaDotPhanBo %>"));
             url = unescape(url.replace("#5", TenTruongTien));
             url = unescape(url.replace("#6", KhoGiay));
             url = unescape(url.replace("#7", Toso));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_tdDotPhanBo").innerHTML = data.iID_MaDotPhanBo;
                 document.getElementById("<%= ParentID %>_tdToSo").innerHTML = data.ToSo;
                
             });
         }  
                                                   
     </script>
      <script type="text/javascript">
          function ChonDPB() {
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;
              var iID_MaDotPhanBo = document.getElementById("<%=ParentID%>_iID_MaDotPhanBo").value;
              var sLNS = "";
              $("input:checkbox[check-group='sLNS']").each(function (i) {
                  if (this.checked) {
                      if (sLNS != "") sLNS += ",";
                      sLNS += this.value;
                  }
              });
              $("input:checkbox[check-group='MaDV']").each(function (i) {
                  if (this.checked) {
                      if (iID_MaDonVi != "") iID_MaDonVi += ",";
                      iID_MaDonVi += this.value;
                  }
              });
              var TruongTien = document.getElementById("<%=ParentID%>_TruongTien").checked;
              var TenTruongTien = "";
              if (TruongTien == true)
                  TenTruongTien = "rTuChi";
              else
                  TenTruongTien = "rHienVat";
              var KhoGiay = document.getElementById("<%=ParentID%>_KhoGiay").value;
              var Toso = document.getElementById("<%=ParentID%>_ToSo").value;
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("ds_DotPhanBo?ParentID=#0&MaND=#1&iID_MaTrangThaiDuyet=#2&sLNS=#3&iID_MaDotPhanBo=#4&TruongTien=#5&KhoGiay=#6&ToSo=#7", "rptPhanBo_11_11") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", "<%=MaND %>"));
              url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));
              url = unescape(url.replace("#3", sLNS));
              url = unescape(url.replace("#4", iID_MaDotPhanBo));
              url = unescape(url.replace("#5", TenTruongTien));
              url = unescape(url.replace("#6", KhoGiay));
              url = unescape(url.replace("#7", Toso));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_tdToSo").innerHTML = data.ToSo;

              });
          }  
                                                   
     </script>
    <iframe src="<%=URLView%>"
        height="600px" width="100%"></iframe>
</body>
</html>
