<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
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
            font: bold 13px "Museo 700";
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
    
 </style>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "BaoHiem";
        String MaND = User.Identity.Name;
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String Thang = "0", Quy = "0";
        String ThangQuy = Convert.ToString(ViewData["ThangQuy"]);
        if (String.IsNullOrEmpty(ThangQuy))
        {
            ThangQuy = "1";
        }
        String LoaiThangQuy = Convert.ToString(ViewData["LoaiThangQuy"]);
        if (String.IsNullOrEmpty(LoaiThangQuy))
        {
            LoaiThangQuy = "0";
        }
        if (LoaiThangQuy == "0")
        {
            Thang = ThangQuy;
            Quy = "0";
        }
        else
        {
            Thang = "0";
            Quy = ThangQuy;
        }
        if (String.IsNullOrEmpty(ThangQuy))
        {
            ThangQuy = "1";
        }
        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if (String.IsNullOrEmpty(ToSo))
        {
            PageLoad = "0";
        }
        String ToDaXem = Convert.ToString(ViewData["ToDaXem"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
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
       
        DataTable dtTrangThai = rptBH_Chi_73_3Controller.tbTrangThai();
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
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "1";
        }
       // String KhoGiay = Request.QueryString["KhoGiay"];
       
        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        String RutGon = Convert.ToString(ViewData["RutGon"]);
         // Url Action export ra file excel
       
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptBH_Chi_73_3", new { MaND = MaND, ThangQuy = ThangQuy,LoaiThangQuy=LoaiThangQuy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, TruongTien = TruongTien, KhoGiay = KhoGiay, ToSo = ToSo, RutGon = RutGon });

        // Url Action export ra file excel
        String BackURL = Url.Action("Index", "BaoHiem_Report", new { bChi = "1" });
        String urlExport = Url.Action("ExportToExcel", "rptBH_Chi_73_3", new { MaND = MaND, ThangQuy = ThangQuy, LoaiThangQuy = LoaiThangQuy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, TruongTien = TruongTien, KhoGiay = KhoGiay, ToSo = ToSo, RutGon = RutGon });
        using (Html.BeginForm("EditSubmit", "rptBH_Chi_73_3", new { ParentID = ParentID, ToDaXem = ToDaXem }))
        {
    %>
    
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo quyết toán chi BHXH</span>
                    </td>
                    <td width="52%" style=" text-align:left;">
                          <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;">
<div id="rptMain" style="padding-top:5px;background-color:#F0F9FE;">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="19">&nbsp;</td>
    <td width="92" align="right"><b>Trạng thái : </b></td>
    <td width="199"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\"onchange=ChonTo()")%></td>
    <td width="96" align="center"><b>Chọn tờ : </b></td>
    <td width="216" id="<%= ParentID %>_tdToSo">
     <% rptBH_Chi_73_3Controller rpt = new rptBH_Chi_73_3Controller();
        rptBH_Chi_73_3Controller.data _Data = new rptBH_Chi_73_3Controller.data();
        _Data = rpt.obj_DanhSachTo(ParentID, MaND, ThangQuy, LoaiThangQuy, iID_MaTrangThaiDuyet, KhoGiay, RutGon, ToSo);%>                           
                               <%=_Data.ToSo %>
     </td>
    <td width="26">&nbsp;</td>
    <td width="167" rowspan="2"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:130px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("In cho loại") %></b></legend>
                            &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "", "")%> &nbsp;&nbsp;Tự Chi<br />
                           &nbsp;&nbsp;  <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "")%> &nbsp;&nbsp;Hiện Vật
                           </fieldset></td>
    <td width="151" rowspan="2"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:130px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("In trên giấy") %></b></legend>
                            &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay","", "onchange=ChonTo()")%> &nbsp;&nbsp;A3 - Giấy to<br />
                            &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "KhoGiay","", "onchange=ChonTo()")%> &nbsp;&nbsp;A4 - Giấy nhỏ
                           </fieldset></td>
    <td width="14">&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td align="right"><b>Tháng / Quý : </b></td>
    <td><%=MyHtmlHelper.Option(ParentID, "0", LoaiThangQuy, "LoaiThangQuy", "", "onchange=ChonTo()")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "onchange=ChonTo()", "class=\"input1_2\" style=\"width:26%;\" onchange=ChonTo()")%>
                               &nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "1", LoaiThangQuy, "LoaiThangQuy", "", "onchange=ChonTo()")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "onchange=ChonTo()", "class=\"input1_2\" style=\"width:29%;\"onchange=ChonTo()")%><br /></td>
    <td align="right"><b>Mẫu Rút gọn : </b></td>
    <td> <%=MyHtmlHelper.CheckBox(ParentID, RutGon, "RutGon", "", "onclick=\"ChonTo()\"")%>  &nbsp;&nbsp;&nbsp;&nbsp;Tờ đã xem :(<%=ToDaXem %>) </td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="9" align="center"><table cellpadding="0" cellspacing="0" border="0" style="margin: 10px;text-align:center;">
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
    <script type="text/javascript">
        function ChonTo() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;

            var LoaiThang_QuyCheck = document.getElementById("<%= ParentID %>_LoaiThangQuy").checked;
            var LoaiThangQuy
            var Thang
            if (LoaiThang_QuyCheck == true) {
                Thang = document.getElementById("<%= ParentID %>_iThang").value;
                LoaiThangQuy = 0;
            }
            else {
                Thang = document.getElementById("<%= ParentID %>_iQuy").value;
                LoaiThangQuy = 1;
            }

            var bKhoGiay = document.getElementById("<%= ParentID %>_KhoGiay").checked;
            var KhoGiay = "";
            if (bKhoGiay) KhoGiay = "1";
            else KhoGiay = "2";        
            var Toso = document.getElementById("<%=ParentID%>_ToSo").value;
            var bRutGon = document.getElementById("<%=ParentID %>_RutGon").checked;
            var RutGon = "";
            if (bRutGon) RutGon = "on";
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_To?ParentID=#0&MaND=#1&ThangQuy=#2&LoaiThangQuy=#3&iID_MaTrangThaiDuyet=#4&KhoGiay=#5&RutGon=#6&ToSo=#7", "rptBH_Chi_73_3") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%=MaND %>"));
            url = unescape(url.replace("#2", Thang));
            url = unescape(url.replace("#3", LoaiThangQuy));
            url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#5", KhoGiay));
            url = unescape(url.replace("#6", RutGon));
            url = unescape(url.replace("#7", Toso));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_tdToSo").innerHTML = data.ToSo;
            });
        }                                        
     </script>
    </div>
    <%} %>
    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
