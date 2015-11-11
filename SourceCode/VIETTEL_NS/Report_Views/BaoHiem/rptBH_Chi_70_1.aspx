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
           
        }    
        div.login1 a {
            color: #545998;
            text-decoration: none;
            font: bold 10px "Museo 700";
            display: block;
            width: 250px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
          
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
    
     .style5
     {
         width: 27px;
     }
     .style6
     {
         width: 73px;
     }
     .style7
     {
         width: 24px;
     }
     .style8
     {
         width: 153px;
     }
     .style9
     {
         width: 235px;
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
        String ThangQuy = Convert.ToString(ViewData["ThangQuy"]);
        if (String.IsNullOrEmpty(BoDongTrong))
        {
            BoDongTrong = "on";
        }
        if (String.IsNullOrEmpty(ThangQuy))
        {
            ThangQuy = "1";
        }
        String iALL = Convert.ToString(ViewData["iALL"]);
        String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
        if (String.IsNullOrEmpty(LoaiThang_Quy))
        {
            LoaiThang_Quy = "0";
        }
        String iThang = "", iQuy = "";
        if(LoaiThang_Quy=="0")
        {
            iThang = ThangQuy;
        }
        else 
        {
            iQuy = ThangQuy;
        }
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        /// <summary>
        /// lấy dữ liệu cho combox đơn bị
        /// </summary>
        /// <returns></returns>
        DataTable dtDonVi = rptBH_Chi_70_1Controller.HienThiDonViTheoNam(MaND, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTenDonVi");
         //Kiểm tra dữ liệu lớn hơn 0 
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }  
        dtDonVi.Dispose();
        // dt Trạng Thái Duyệt
       
        DataTable dtTrangThai = rptBH_Chi_70_1Controller.tbTrangThai();
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
        /// <summary>
        /// Kiểm tra radio đơn vị tính
        /// </summary>
        /// <returns></returns>

        
        
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "2";
        }
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptBH_Chi_70_1", new { MaND = MaND, ThangQuy = ThangQuy, LoaiThang_Quy = LoaiThang_Quy, iID_MaDonVi = iID_MaDonVi, iALL = iALL,  iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, BoDongTrong = BoDongTrong, KhoGiay = KhoGiay });
        
         // Url Action export ra file excel
        String BackURL = Url.Action("Index", "BaoHiem_Report", new { bChi = "1" });
        String urlExport = Url.Action("ExportToExcel", "rptBH_Chi_70_1", new { MaND = MaND, ThangQuy = ThangQuy, LoaiThang_Quy = LoaiThang_Quy, iID_MaDonVi = iID_MaDonVi, iALL = iALL,  iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, BoDongTrong = BoDongTrong, KhoGiay = KhoGiay });
        using (Html.BeginForm("EditSubmit", "rptBH_Chi_70_1", new { ParentID = ParentID}))
        {
    %>
    
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo quyết toán chi các chế độ chi BHXH</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                      <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                           <%--<div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>--%>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;">
   <div id="rptMain" style="margin:0 auto;background-color:#F0F9FE;">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td class="style5">&nbsp;</td>
    <td width="73" align="right"><b>Trạng thái : </b></td>
    <td width="215"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 93%;heigh:22px;\"onchange=ChonThang()")%></td>
    <td class="style7">&nbsp;</td>
    <td align="right" class="style6"><b>Tháng quý : </b></td>
    <td class="style9"><%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", " class=\"input1_2\" style=\"width:10%;\" onchange=ChonThang()")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width:17%;\"onchange=ChonThang() ")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "class=\"input1_2\" style=\"width:10%;\" onchange=ChonThang()")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:17%;\" onchange=ChonThang()")%><br /></td>
    <td rowspan="2" class="style8">
                                   <%-- <div style="float:right; width:150px; padding-top:10px;">
                            <p class="p"><span style="font-size:9pt; line-height:22px; padding-left:4px;"><b>Bỏ dòng không có dữ liệu :</b> <%=MyHtmlHelper.CheckBox(ParentID, BoDongTrong, "BoDongTrong","")%></span></p>
                        </div> --%>
                        <fieldset style="height:50px;padding-top:10px;padding-left:10px;width:120px;text-align:left;-moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                                    <legend><b>In kiểu</b></legend>                                 
                                    <%=MyHtmlHelper.Option(ParentID, "off", BoDongTrong, "BoDongTrong", "", "")%>&nbsp;<b>
                                     Toàn bộ danh mục </b>
                                   <br /> <%=MyHtmlHelper.Option(ParentID, "on", BoDongTrong, "BoDongTrong", "", "")%>&nbsp;<b>
                                         Theo số liệu có</b>
                                </fieldset>
                              </td>
    <td width="150" rowspan="2"><fieldset style="height:50px;padding-top:10px;padding-left:10px;width:120px;text-align:left;-moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                                    <legend><b>Khổ giấy</b></legend>
                                    <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay", "", "")%>&nbsp;<b>
                                     A3 - Giấy to </b>
                                   <br /> <%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "KhoGiay", "", "")%>&nbsp;<b>
                                         A4 - Giấy nhỏ</b>
                                </fieldset></td>
    <td width="30">&nbsp;</td>
  </tr>
  <tr>
    <td class="style5">&nbsp;</td>
    <td>&nbsp;</td>
    <td><b>Tất cả các đơn vị :</b><%=MyHtmlHelper.CheckBox(ParentID, iALL, "iALL", "")%></td>
    <td class="style7">&nbsp;</td>
    <td align="right" class="style6"><b>Đơn vị : </b></td>
    <td class="style9"><div id="<%= ParentID %>_tdDonVi" style="width:100%;padding-left:5px;"><%rptBH_Chi_70_1Controller rpt = new rptBH_Chi_70_1Controller();%> 
                                <%=rpt.obj_DonViTheoLNS(ParentID,MaND,ThangQuy,LoaiThang_Quy, iID_MaDonVi,iID_MaTrangThaiDuyet)%></div></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="9" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin:0 auto;">
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
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&MaND=#1&ThangQuy=#2&LoaiThang_Quy=#3&iID_MaDonVi=#4&iID_MaTrangThaiDuyet=#5", "rptBH_Chi_70_1") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= MaND %>"));
            url = unescape(url.replace("#2", Thang));
            url = unescape(url.replace("#3", LoaiThang_Quy));
            url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#5", iID_MaTrangThaiDuyet));
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
    </script>
    </div>
    <%} %>
    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
