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
    
     .style1
     {
         width: 93px;
     }
     .style2
     {
         width: 168px;
     }
     .style3
     {
         width: 180px;
     }
    
 </style>
</head>
<body>
     <%
         /// <summary>
         /// <prame name="NamLamViec">lấy năm làm việc</prame>
         /// <prame name="UserID">kiểm tra user theo phòng ban</prame>
         /// <prame name="Thang_Quy">lấy tháng</prame>
         /// <prame name="LoaiThangQuy">lấy loại tháng quý</prame>
         /// <prame name="iID_MaDonVi">đơn vị</prame>
         /// </summary>
         /// <returns></returns>
        String ParentID = "BaoHiem";
        String UserID = User.Identity.Name;
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy)) Thang_Quy = "1";
        
        String LoaiThangQuy = Convert.ToString(ViewData["LoaiThangQuy"]);
        if (String.IsNullOrEmpty(LoaiThangQuy)) LoaiThangQuy = "0";
         
        String Thang = "0", Quy = "0";
         // 0 - Thang       1 - Quy
        if (LoaiThangQuy == "0") Thang = Thang_Quy; else Quy = Thang_Quy;
             
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet)) iID_MaTrangThaiDuyet = "0";
         
        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if (String.IsNullOrEmpty(ToSo))ToSo = "1";

        /// <summary>
        /// lấy dữ liệu cho combox tháng
        /// </summary>
        /// <returns></returns>
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        /// <summary>
        /// lấy dữ liệu cho combox Quý
        /// </summary>
        /// <returns></returns>
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        // dt Trạng Thái Duyệt

        DataTable dtTrangThai = rptBH_ThongTri64Controller.tbTrangThai();
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
         
         //dt chon to
        DataTable dtToSo =rptBH_TongHop67Controller.dtTo(UserID, LoaiThangQuy, Thang_Quy, iID_MaTrangThaiDuyet);
        SelectOptionList slToSo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
         
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "1";
        }

        /// <summary>
        /// lấy dữ liệu cho radio loại bảo hiểm
        /// </summary>
        /// <returns></returns>
        String LoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
        String[] arrLoaiBaoCao = { "1", "2", "3", "4","5","6","7","8","9" };
        if (String.IsNullOrEmpty(LoaiBaoCao))
            LoaiBaoCao = arrLoaiBaoCao[0];
         // Url Action thực hiện xuất dự liệu ra file excel
        String BackURL = Url.Action("Index", "BaoHiem_Report/index", new { bChi = 0 });
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
         String UrlReport="";
          if(PageLoad=="1")
          UrlReport=Url.Action("ViewPDF","rptBH_TongHop67", new{ LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, LoaiBaoCao = LoaiBaoCao, KhoGiay = KhoGiay, ToSo = ToSo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet});
        
             
        String urlExport = Url.Action("ExportToExcel", "rptBH_TongHop67", new { LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, LoaiBaoCao = LoaiBaoCao, KhoGiay = KhoGiay, ToSo = ToSo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        using (Html.BeginForm("EditSubmit", "rptBH_TongHop67", new { ParentID = ParentID}))
        {
    %>
    <script type="text/javascript">
        function DoiSoTo() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var LoaiThangQuy = "";
            var objLoaiThangQuy = document.getElementsByName("<%=ParentID %>_LoaiThangQuy");
            for (i = 0; i < objLoaiThangQuy.length; i = i + 1) {
                if (objLoaiThangQuy[i].checked == true) {
                    LoaiThangQuy = objLoaiThangQuy[i].value;
                }
            }
            var ThangQuy = "";
            if (LoaiThangQuy == "0") {
                ThangQuy = document.getElementById("<%=ParentID %>_iThang").value;
            } else {
                ThangQuy = document.getElementById("<%=ParentID %>_iQuy").value;
            }
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("DS_To?ParentID=#0&MaND=#1&LoaiThangQuy=#2&Thang_Quy=#3&iID_MaTrangThaiDuyet=#4&ToSo=#5", "rptBH_TongHop67") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= UserID %>"));
            url = unescape(url.replace("#2", LoaiThangQuy));
            url = unescape(url.replace("#3", ThangQuy));
            url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#5", "<%= ToSo %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_td_chon_to").innerHTML = data;
            });
        }                 
    </script>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng thu bảo hiểm_</span>
                    </td>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;">
        <div id="rptMain" style="margin:0 auto;background-color:#F0F9FE;padding-top:5px;">
 <table width="100%" border="0" cellpadding="0" cellspacing="0">
     <tr>
         <td width="250" valign="top">
         </td>
         <td width="120" height="120" valign="top">
             <table>
                 <tr>
                     <td align="right">
                         <b>
                             <%=NgonNgu.LayXau("Trạng Thái :")%></b>
                     </td>
                     <td>
                         <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "onchange=\"DoiSoTo()\" class=\"input1_2\" style=\"width:100px;heigh:22px;\"")%>
                     </td>
                 </tr>
                 <tr>
                     <td colspan="2" align="right">
                         <fieldset style="height: 60px; padding-left: 5px; width: 160px; text-align: left;
                             padding-left: 3px; -moz-border-radius: 3px; -webkit-border-radius: 3px; -khtml-border-radius: 3px;
                             border-radius: 3px; border: 1px #C0C0C0 solid;">
                             <legend><b>Chọn tháng quý</b></legend>
                             <div>
                                 <%=MyHtmlHelper.Option(ParentID, "0", LoaiThangQuy, "LoaiThangQuy", "", "onclick=DoiSoTo()")%>Tháng
                                 <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "", "onchange=\"DoiSoTo()\" class=\"input1_2\" style=\"width:100px;\" onchange=ChonThang()")%>
                             </div>
                             <div>
                                 <%=MyHtmlHelper.Option(ParentID, "1", LoaiThangQuy, "LoaiThangQuy","", "onclick=DoiSoTo()")%>Quý&nbsp;&nbsp;&nbsp;
                                 <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "onchange=\"DoiSoTo()\" class=\"input1_2\" style=\"width:100px;\"onchange=ChonThang()")%><br />
                             </div>
                         </fieldset>
                     </td>
                 </tr>
             </table>
         </td>
         <td width="550px" valign="top" align="center">
             <fieldset style="height: 80px; padding-left: 5px; width: 450px; text-align: left;
                 padding-left: 3px; -moz-border-radius: 3px; -webkit-border-radius: 3px; -khtml-border-radius: 3px;
                 border-radius: 3px; border: 1px #C0C0C0 solid;">
                 <legend><b>Chọn Loại bảo hiểm</b></legend>
                 <div style="height: 70px; float: left; border-right: #CCCCCC 1px dashed; width: 150px;
                     padding-left: 5px; padding-top: 3px;">
                     <%=MyHtmlHelper.Option(ParentID, "1", LoaiBaoCao, "LoaiBaoCao", "")%>&nbsp;&nbsp;BHXH&nbsp;(
                     Cá nhân đóng )<br />
                     <%=MyHtmlHelper.Option(ParentID, "2", LoaiBaoCao, "LoaiBaoCao", "")%>&nbsp;&nbsp;BHYT&nbsp;(
                     Cá Nhân đóng )<br />
                     <%=MyHtmlHelper.Option(ParentID, "3", LoaiBaoCao, "LoaiBaoCao", "")%>&nbsp;&nbsp;BHTN&nbsp;(
                     Cá Nhân Đóng )
                 </div>
                 <div style="height: 70px; float: left; border-right: #CCCCCC 1px dashed; width: 130px;
                     padding-left: 5px; padding-top: 3px;">
                     <%=MyHtmlHelper.Option(ParentID, "4", LoaiBaoCao, "LoaiBaoCao", "")%>&nbsp;&nbsp;BHXH&nbsp;(
                     Đơn vị đóng )<br />
                     <%=MyHtmlHelper.Option(ParentID, "5", LoaiBaoCao, "LoaiBaoCao", "")%>&nbsp;&nbsp;BHYT&nbsp;(
                     Đơn vị đóng )<br />
                     <%=MyHtmlHelper.Option(ParentID, "6", LoaiBaoCao, "LoaiBaoCao", "")%>&nbsp;&nbsp;BHTN&nbsp;(
                     Đơn vị đóng )
                 </div>
                 <div style="height: 70px; float: left; width: 120px; padding-left: 5px; padding-top: 5px;">
                     <%=MyHtmlHelper.Option(ParentID, "7", LoaiBaoCao, "LoaiBaoCao", "")%>&nbsp;&nbsp;BHXH&nbsp;(
                     Tổng hợp )<br />
                     <%=MyHtmlHelper.Option(ParentID, "8", LoaiBaoCao, "LoaiBaoCao", "")%>&nbsp;&nbsp;BHYT&nbsp;(
                     Tổng hợp)<br />
                     <%=MyHtmlHelper.Option(ParentID, "9", LoaiBaoCao, "LoaiBaoCao", "")%>&nbsp;&nbsp;BHTN&nbsp;(
                     Tổng hợp)
                 </div>
             </fieldset>
         </td>
         <td width="199" valign="top">
             <table border="0" cellspacing="0" cellpadding="0" width="100%">
                 <tr>
                     <td width="120" align="right">
                         <b>
                             <%=NgonNgu.LayXau("Chọn tờ :")%></b>
                     </td>
                     <td class="style2" id = "<%= ParentID %>_td_chon_to">
                         <%= MyHtmlHelper.DropDownList(ParentID, slToSo, ToSo, "ToSo", "", "class=\"input1_2\" style=\"width: 100px\"")%>
                     </td>
                 </tr>
                 <tr>
                     <td colspan="2" align="right">
                         <fieldset style="text-align: left; padding: 5px 5px 8px 8px; font-size: 11px; width: 160px;
                             -moz-border-radius: 3px; -webkit-border-radius: 3px; -khtml-border-radius: 3px;
                             border-radius: 3px; border: 1px #C0C0C0 solid;">
                             <legend><b>
                                 <%=NgonNgu.LayXau("In trên giấy") %></b></legend>
                             <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay", "")%>A4 - Giấy nhỏ<br />
                             <%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "KhoGiay", "")%>A3 - Giấy to
                         </fieldset>
                     </td>
                 </tr>
             </table>
         </td>
         <td></td>
     </tr>
  <tr>
    <td height="27" colspan="6" align="center"><table cellpadding="0" cellspacing="0" border="0">
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
