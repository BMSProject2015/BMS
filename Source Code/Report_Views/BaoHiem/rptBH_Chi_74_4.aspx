<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.BaoHiem" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
 <style type="text/css">
     div.login1 {
            text-align : center;    
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }    
        div.login1 a {
            color: #545998;
            text-decoration: none;
            font: bold 10px "Museo 700";
            display: block;
            width: 250px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px -29px;
            -webkit-border-radius:2px;
            border-radius:2px;
        }
        div.login1 a:hover
        {
            text-decoration:underline;
            color:#471083;
        }    
        div.login1 a.active {
            background-position:  20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
    
 </style>
</head>
<body>
    <%
         /// <summary>
         /// Hiển thị danh sách Mã + Ten DV
         /// <prame name="NamLamViec">lấy năm làm việc</prame>
         /// <prame name="UserID">kiểm tra user theo phòng ban</prame>
         /// <prame name="Thang_Quy">lấy tháng</prame>
         /// <prame name="LoaiThangQuy">lấy loại tháng quý</prame>
         /// <prame name="iID_MaDonVi">đơn vị</prame>
         /// </summary>
         /// <returns></returns>
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "BaoHiem";
        
        String MaND = User.Identity.Name;
        String BoDongTrong = Convert.ToString(ViewData["BoDongTrong"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        
        //if (String.IsNullOrEmpty(BoDongTrong))
        //{
        //    BoDongTrong = "on";
        //}

        /// <summary>
        /// lấy dữ liệu cho combox đơn bị
        /// </summary>
        /// <returns></returns>
         String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptBH_Chi_74_4Controller.tbTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
        DataTable dtDonVi =rptBH_Chi_74_4Controller.HienThiDonViTheoNam(MaND,iID_MaTrangThaiDuyet);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTenDonVi");
         //Kiểm tra dữ liệu lớn hơn 0 
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["sTenDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }  
        dtDonVi.Dispose();
        // dt Trạng Thái Duyệt
      
        /// <summary>
        /// Kiểm tra radio đơn vị tính
        /// </summary>
        /// <returns></returns>
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "1";
        }
        
        String LoaiBieu = Convert.ToString(ViewData["LoaiBieu"]);
        if (String.IsNullOrEmpty(LoaiBieu))
        {
            LoaiBieu = Convert.ToString(ViewData["LoaiBieu"]);
        }
        String[] arrLoaiBieu = { "1", "2"};
        if (String.IsNullOrEmpty(LoaiBieu))
            LoaiBieu = arrLoaiBieu[0];
         // Url Action export ra file excel
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptBH_Chi_74_4", new { MaND = MaND, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, BoDongTrong = BoDongTrong,KhoGiay = KhoGiay});
        String BackURL = Url.Action("Index", "BaoHiem_Report", new { bChi = "1" });
        String urlExport = Url.Action("ExportToExcel", "rptBH_Chi_74_4", new { MaND = MaND, iID_MaDonVi = iID_MaDonVi, LoaiBieu = LoaiBieu, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, BoDongTrong = BoDongTrong, KhoGiay = KhoGiay });
        using (Html.BeginForm("EditSubmit", "rptBH_Chi_74_4", new { ParentID = ParentID}))
        {
    %>
    
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                         <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;margin:0 auto;padding:0;">
        <div id="rptMain" style="margin:0 auto;background-color:#F0F9FE">
<table width="100%" height="81" border="0" cellpadding="0" cellspacing="0" align="center">
  <tr>
   
    <td width="188" align="right"><b>Trạng thái : </b></td>
    <td width="204"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 70%;heigh:22px;\"onchange=ChonThang()")%></td>
    <td width="240" rowspan="2"><fieldset style="height:50px;padding-top:10px;padding-left:10px;width:200px;text-align:left;">
                                    <legend><b>Kết quả in ra cho</b></legend>
                                    <%=MyHtmlHelper.Option(ParentID, "1", LoaiBieu, "LoaiBieu", "", "")%>&nbsp;<b>
                                      Từng đơn vị</b>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%=MyHtmlHelper.Option(ParentID, "2", LoaiBieu, "LoaiBieu", "", "")%>&nbsp;<b>Theo
                                        Tất Cả</b>
                                </fieldset></td>
     <td width="240" rowspan="2"><fieldset style="height:50px;padding-top:10px;padding-left:10px;width:230px;text-align:left;">
                                    <legend><b>In trên giấy</b></legend>
                                    <%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "KhoGiay", "", "")%>&nbsp;<b>
                                      A4 - Giấy nhỏ</b>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay", "", "")%>&nbsp;<b>Theo
                                        A3 - Giấy to</b>
                                </fieldset></td>
  </tr>
  
  </tr>
  <tr>

    <td align="right" ><b>Đơn vị : </b></td>
    <td><div id="<%= ParentID %>_tdDonVi" style="width:250px;"><%rptBH_Chi_74_4Controller rpt = new rptBH_Chi_74_4Controller();%> 
                                <%=rpt.obj_DonViTheoLNS(ParentID, MaND, iID_MaDonVi, iID_MaTrangThaiDuyet)%></div></td>
    <td align="right"> </td>
    <td width = "220px"><b>Bỏ dòng không có dữ liệu :</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp<%=MyHtmlHelper.CheckBox(ParentID, BoDongTrong, "BoDongTrong","")%></td>
  </tr>
  <tr>
    <td colspan="5" align="center"><table cellpadding="0" cellspacing="0" border="0" style="margin: 10px;text-align:center;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
  </tr>
</table>

</div>

    <script type="text/javascript">
        function ChonThang() {
          
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&iID_MaDonVi=#2&iID_MaTrangThaiDuyet=#3", "rptBH_Chi_74_4") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= MaND %>"));
            url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
            });
        }                                            
     </script>
        </div>
        <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
      <script type="text/javascript">
          $(function () {
              $("div#rptMain").show();
              $('div.login1 a').click(function () {
                  $('div#rptMain').slideToggle('fast');
                  $(this).toggleClass('active');
                  return false;
              });
          });
          function ChonThang() {

              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;
             
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&MaND=#1&iID_MaDonVi=#2&iID_MaTrangThaiDuyet=#3", "rptBH_Chi_74_4") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", "<%= MaND %>"));
              url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
              url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
              });
          }            
    </script>
    </div>
    <%} %>
    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
