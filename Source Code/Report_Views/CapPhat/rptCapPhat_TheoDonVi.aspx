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
      //String LuyKe = Convert.ToString(ViewData["LuyKe"]);
      //if(String.IsNullOrEmpty(LuyKe))
      //{
      //    LuyKe = "";
      //}
      String ParentID = "CapPhat";
      //dt Trạng thái duyệt
      String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
      DataTable dtTrangThai = rptCapPhat_TheoDonViController.tbTrangThai();
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
      
      //String LoaiCapPhat = Convert.ToString(ViewData["iDM_MaLoaiCapPhat"]);
      //DataTable dtLoaiCapPhat = rptCapPhat_TheoDonViController.DanhSach_LoaiCapPhat();
      //if (String.IsNullOrEmpty(LoaiCapPhat))
      //{
      //    if (dtLoaiCapPhat.Rows.Count > 2)
      //    {
      //        LoaiCapPhat = Convert.ToString(dtLoaiCapPhat.Rows[2]["iID_MaDanhMuc"]);
      //    }
      //    else
      //    {

      //        LoaiCapPhat = Guid.Empty.ToString();
      //    }
              
      //}
      //SelectOptionList slLoaiCapPhat = new SelectOptionList(dtLoaiCapPhat, "iID_MaDanhMuc", "sTen");
      //dtLoaiCapPhat.Dispose();

      String Thang = Convert.ToString(ViewData["Thang"]);
      //DataTable dtThang = rptCapPhat_TheoDonViController.DanhSach_Thang_CapPhat(iID_MaTrangThaiDuyet, MaND);
      //if (String.IsNullOrEmpty(Thang))
      //{
      //    if (dtThang.Rows.Count > 1)
      //    {

      //        Thang = Convert.ToString(dtThang.Rows[1]["Thang"]);
      //    }
      //}
      //SelectOptionList slThang = new SelectOptionList(dtThang, "Thang", "TenThang");
      
      String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
      if(String.IsNullOrEmpty(iID_MaDonVi))
      {
          DataTable dtDonVi = rptCapPhat_TheoDonViController.DanhSach_DonVi(iID_MaTrangThaiDuyet, Thang, MaND);
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
      //String LoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
      //if (String.IsNullOrEmpty(LoaiBaoCao))
      //{
      //    LoaiBaoCao = "Nganh";
      //}
      //DataTable dtLoaiBaoCao = rptCapPhat_TheoDonViController.DanhSach_LoaiBaoCao();
      //SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "MaLoai", "TenLoai");
      //dtLoaiBaoCao.Dispose();
      String PageLoad = Convert.ToString(ViewData["PageLoad"]);
      String URL = Url.Action("Index", "CapPhat_Report");
      String UrlReport = "";
      if (PageLoad == "1")
          UrlReport = Url.Action("ViewPDF", "rptCapPhat_TheoDonVi", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, Thang = Thang});
      using (Html.BeginForm("EditSubmit", "rptCapPhat_TheoDonVi", new { ParentID = ParentID }))
      {                  
   %> 
        <div class="box_tong">
            <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo chi tiết cấp kinh phí</span>
                    </td>
                </tr>
            </table>
            </div>
            <div id="Div1" style="background-color:#F0F9FE;">
                <div id="Div2" style="padding-top:5px;">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                        <tr>
                            <td></td>
                            <td class="style1" align="right" width="15%">
                                <div>
                                    <b><%=NgonNgu.LayXau("Trạng Thái: ")%></div></b>
                            </td>
                            <td width="15%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=ChonLCP()")%>
                                </div>
                            </td>
                             <td class="td_form2_td1" align="right" width="15%">
                                <div>
                                    <b><%=NgonNgu.LayXau("Chọn tháng: ")%></b></div>
                            </td>
                            <td id="<%= ParentID %>_tdThang" width="15%">
                             <%  
                                   rptCapPhat_TheoDonViController rptTB1 = new rptCapPhat_TheoDonViController();
                                   rptCapPhat_TheoDonViController.LCPData _LCPData = new rptCapPhat_TheoDonViController.LCPData();
                                     _LCPData = rptTB1.obj_NgayCapPhat(ParentID, iID_MaTrangThaiDuyet, iID_MaDonVi, Thang, MaND);
                                %>
                                <%=_LCPData.Thang%>
                            </td>
                            <td class="style1" align="right" width="15%">
                                <div><b><%=NgonNgu.LayXau("Chọn đơn vị: ")%></b></div>
                            </td>
                            <td id="<%= ParentID %>_tdDonVi" width="15%">
                               
                                <%=_LCPData.DonVi%>
                            </td>
                           
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="7">
                                <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin-bottom: 10px;
                                    margin-right: 10px; margin-top: 10px;" width="100%">
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
                                </table>
                            </td>
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
                 jQuery.ajaxSetup({ cache: false });
                 var url = unescape('<%= Url.Action("ds_NgayCapPhat?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDonVi=#2&Thang=#3", "rptCapPhat_TheoDonVi") %>');
                 url = unescape(url.replace("#0", "<%= ParentID %>"));
                 url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
                 url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
                 url = unescape(url.replace("#3", "<%= Thang %>"));                            
                 $.getJSON(url, function (data) {
                     document.getElementById("<%= ParentID %>_tdThang").innerHTML = data.Thang;
                     document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.DonVi;
                 });
             }                                            
        </script>
         <script type="text/javascript">
             function ChonThang() {
                 var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                 var Thang = document.getElementById("<%=ParentID %>_Thang").value;              
                  jQuery.ajaxSetup({ cache: false });
                  var url = unescape('<%= Url.Action("ds_NgayCapPhat?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDonVi=#2&Thang=#3", "rptCapPhat_TheoDonVi") %>');
                 url = unescape(url.replace("#0", "<%= ParentID %>"));
                 url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
                 url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
                 url = unescape(url.replace("#3", Thang));                          
                 $.getJSON(url, function (data) {
                     document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.DonVi;
                 });
             }                                             
         </script>
          
   <%} %>
   <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptCapPhat_TheoDonVi", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, Thang = Thang}), "Xuất ra Excel")%>
    <iframe src="<%=UrlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
