<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.CapPhat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 7%;
        }
        .table_form2
        {
            height: 101px;
        }
    </style>
</head>
<body>
  <%  
      
     
      String MaND = User.Identity.Name;
      String LuyKe = Convert.ToString(ViewData["LuyKe"]);
      if(String.IsNullOrEmpty(LuyKe))
      {
          LuyKe = "";
      }
      String ParentID = "CapPhat";
      //dt Trạng thái duyệt
      String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
      DataTable dtTrangThai = rptCapPhat_79_3Controller.tbTrangThai();
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
      
      String LoaiCapPhat = Convert.ToString(ViewData["iDM_MaLoaiCapPhat"]);
      DataTable dtLoaiCapPhat = rptCapPhat_79_3Controller.DanhSach_LoaiCapPhat();
      if (String.IsNullOrEmpty(LoaiCapPhat))
      {
          if (dtLoaiCapPhat.Rows.Count > 2)
          {
              LoaiCapPhat = Convert.ToString(dtLoaiCapPhat.Rows[2]["iID_MaDanhMuc"]);
          }
          else
          {

              LoaiCapPhat = Guid.Empty.ToString();
          }
              
      }
      SelectOptionList slLoaiCapPhat = new SelectOptionList(dtLoaiCapPhat, "iID_MaDanhMuc", "sTen");
      dtLoaiCapPhat.Dispose();

      String dNgayCapPhat = Convert.ToString(ViewData["dNgayCapPhat"]);

      if (String.IsNullOrEmpty(dNgayCapPhat))
      {
          DataTable dtNgayCapPhat=rptCapPhat_79_3Controller.DanhSach_Ngay_CapPhat(LoaiCapPhat,iID_MaTrangThaiDuyet,MaND);
          if (dtNgayCapPhat.Rows.Count > 1)
          {
            
              dNgayCapPhat = Convert.ToString(dtNgayCapPhat.Rows[1]["dNgayCapPhat"]);
              dtNgayCapPhat.Dispose();
          }
          else  {
              dNgayCapPhat = "01/01/2000";
          }
      }
      String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
      if(String.IsNullOrEmpty(iID_MaDonVi))
      {          
          DataTable dtDonVi = rptCapPhat_79_3Controller.DanhSach_DonVi(iID_MaTrangThaiDuyet, dNgayCapPhat, LuyKe, LoaiCapPhat,MaND);
          if (dtDonVi.Rows.Count > 2)
          {
              iID_MaDonVi = Convert.ToString(dtDonVi.Rows[2]["iID_MaDonVi"]);
          }
          else
          {
              iID_MaDonVi = "-2";
          }
          dtDonVi.Dispose();
      }
      String LoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
      if (String.IsNullOrEmpty(LoaiBaoCao))
      {
          LoaiBaoCao = "Nganh";
      }
      DataTable dtLoaiBaoCao = rptCapPhat_79_3Controller.DanhSach_LoaiBaoCao();
      SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "MaLoai", "TenLoai");
      dtLoaiBaoCao.Dispose();
      String PageLoad = Convert.ToString(ViewData["PageLoad"]);
      String URL = Url.Action("Index", "CapPhat_Report");
      String UrlReport = "";
      if (PageLoad == "1")
          UrlReport = Url.Action("ViewPDF", "rptCapPhat_79_3", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, dNgayCapPhat = dNgayCapPhat, iDM_MaLoaiCapPhat = LoaiCapPhat, LuyKe = LuyKe, LoaiBaoCao = LoaiBaoCao });
      using (Html.BeginForm("EditSubmit", "rptCapPhat_79_3", new { ParentID = ParentID }))
      {                  
   %> 
        <div class="box_tong">
            <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo chi tiết cấp NS</span>
                    </td>
                </tr>
            </table>
            </div>
            <div id="Div1" style="background-color:#F0F9FE;">
                <div id="Div2" style="padding-top:5px;">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                         <tr>
                            <td class="style1" align="right">
                            <div>
                                <%=NgonNgu.LayXau("Trạng Thái : ")%></div>                            </td>
                            <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=ChonLCP()")%>                            </div>                            </td>
                             <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Loại cấp phát: ")%></div>                            </td>
                            <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiCapPhat, LoaiCapPhat, "iID_MaDanhMuc", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonLCP()\"")%>                            </div>                            </td>
                          
                            <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Ngày cấp phát: ")%></div>                            </td>
                            <td id="<%= ParentID %>_tdNgayCapPhat" style="width:10%;">
                                 <%rptCapPhat_79_3Controller rptTB1 = new rptCapPhat_79_3Controller();
                                   rptCapPhat_79_3Controller.LCPData _LCPData = new rptCapPhat_79_3Controller.LCPData();
                                   _LCPData = rptTB1.obj_NgayCapPhat(ParentID, iID_MaTrangThaiDuyet, iID_MaDonVi, dNgayCapPhat, LoaiCapPhat, LuyKe,MaND);
              %>
                                     <%=_LCPData.NgayCapPhat%>                            </td>
                                       <td class="td_form2_td1" style="width: 5%;">
                            <div>
                                <%=NgonNgu.LayXau("Lũy kế:")%></div>                            </td>
                            <td width="3%">
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, LuyKe, "LuyKe", "", "onchange=ChonLCP()")%>                            </div>                            </td>
                         </tr>
                         <tr>
                    
                            <td class="style1" align="right">
                            <div>
                                <%=NgonNgu.LayXau("Chọn đơn vị: ")%></div>                            </td>
                            <td id="<%= ParentID %>_tdDonVi">                              
                                    <%=_LCPData.DonVi%>                            </td>
                             <td class="td_form2_td1">
                            <div>
                                <%=NgonNgu.LayXau("Chọn loại báo cáo: ")%></div>                            </td>
                            <td>
                                 <%=MyHtmlHelper.DropDownList(ParentID, slLoaiBaoCao, LoaiBaoCao, "MaLoai", "", "class=\"input1_2\" style=\"width: 100%\"")%>                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                         </tr>
                         <tr>
                           <td class="style1">&nbsp;</td>
                           <td colspan="7"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin-bottom: 10px;margin-right:10px;margin-top:10px;" width="100%">
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
                         </tr>
                    </table>                   
                </div>
            </div>
        </div>
         <script type="text/javascript">
             function Huy() {
                 window.location.href = '<%=URL %>';
             }
    </script>
         <script type="text/javascript">
             function ChonLCP() {
                 var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                 var LoaiCapPhat = document.getElementById("<%=ParentID %>_iID_MaDanhMuc").value;
                 var _luyke = document.getElementById("<%= ParentID %>_LuyKe").checked;
                 var LuyKe=""
                 if (_luyke) {
                     LuyKe = "on";
                 }    
                 jQuery.ajaxSetup({ cache: false });
                 var url = unescape('<%= Url.Action("ds_NgayCapPhat?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDonVi=#2&dNgayCapPhat=#3&iDM_MaLoaiCapPhat=#4&LuyKe=#5", "rptCapPhat_79_3") %>');
                 url = unescape(url.replace("#0", "<%= ParentID %>"));
                 url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
                 url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
                 url = unescape(url.replace("#3", "<%= dNgayCapPhat %>"));
                 url = unescape(url.replace("#4", LoaiCapPhat));
                 url = unescape(url.replace("#5", LuyKe));                            
                 $.getJSON(url, function (data) {
                     document.getElementById("<%= ParentID %>_tdNgayCapPhat").innerHTML = data.NgayCapPhat;
                     document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.DonVi;
                 });
             }                                            
        </script>
         <script type="text/javascript">
             function ChonNCP() {
                 var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                 var LoaiCapPhat = document.getElementById("<%=ParentID %>_iID_MaDanhMuc").value;
                 var NgayCapPhat=document.getElementById("<%=ParentID %>_dNgayCapPhat").value;
                 var _luyke = document.getElementById("<%= ParentID %>_LuyKe").checked;
                 var LuyKe=""
                 if (_luyke) {
                     LuyKe = "on";
                 }               
                  jQuery.ajaxSetup({ cache: false });
                  var url = unescape('<%= Url.Action("ds_NgayCapPhat?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDonVi=#2&dNgayCapPhat=#3&iDM_MaLoaiCapPhat=#4&LuyKe=#5", "rptCapPhat_79_3") %>');
                 url = unescape(url.replace("#0", "<%= ParentID %>"));
                 url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
                 url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
                 url = unescape(url.replace("#3", NgayCapPhat));
                 url = unescape(url.replace("#4", LoaiCapPhat));
                 url = unescape(url.replace("#5", LuyKe));                            
                 $.getJSON(url, function (data) {
                     document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.DonVi;
                 });
             }                                             
         </script>
          
   <%} %>
   <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptCapPhat_79_3", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, dNgayCapPhat = dNgayCapPhat, iDM_MaLoaiCapPhat = LoaiCapPhat, LuyKe = LuyKe, LoaiBaoCao = LoaiBaoCao }), "Xuất ra Excel")%>
    <iframe src="<%=UrlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
