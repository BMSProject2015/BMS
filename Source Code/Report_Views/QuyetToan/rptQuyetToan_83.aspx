<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style7
        {
            height: 57px;
        }
    </style>
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
    
     
    
 </style>
</head>
<body>
     <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "QuyetToanThongTri";
    String MaND = User.Identity.Name;
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
    if (String.IsNullOrEmpty(Thang_Quy))
    {
        Thang_Quy = "1";
    }
    

    // dt Trạng Thái duyệt
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    DataTable dtTrangThai = rptQuyetToan_83Controller.tbTrangThai();
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
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        PageLoad = "0";
    }
    DataTable dtDonVi = rptQuyetToan_83Controller.HienThiDonViTheoNam(MaND,Thang_Quy,iID_MaTrangThaiDuyet);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    //if (String.IsNullOrEmpty(iID_MaDonVi))
    //{
    //    if (dtDonVi.Rows.Count > 0)
    //    {
    //        iID_MaDonVi = dtDonVi.Rows[0]["sTen"].ToString();
    //    }
    //    else
    //    {
    //        iID_MaDonVi = Guid.Empty.ToString();
    //    }
    //}
    dtDonVi.Dispose();
    //thang
    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();
         //
    String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = "0" });
    String urlExport = Url.Action("ExportToExcel", "rptQuyetToan_83", new { MaND = MaND, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    String urlReport = "";
   
    if (PageLoad.Equals("1"))
    {
        urlReport = Url.Action("ViewPDF", "rptQuyetToan_83", new { MaND = MaND, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    }  
         using (Html.BeginForm("EditSubmit", "rptQuyetToan_83", new { ParentID = ParentID}))
    {
    %>   
     <div class="box_tong" style="background-color:#F0F9FE;">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo TỔng hợp quyết toán </span>
                    </td>
                    <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="rptMain" style="padding-top:5px;">

<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="10%">&nbsp;</td>
    <td width="10%" align="right"><b>Tháng :</b></td>
    <td width="10%"><%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "", "class=\"input1_2\" style=\"width: 90%;height:23px;\" onChange=ChonThang()")%></td>
    <td width="10%" align="right"><b>Trạng Thái :</b></td>
    <td width="10%"> <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 80%;heigh:22px;\"onchange=ChonThang()")%></td>
    <td width="10%" align="right"><b>Đơn vị :</b></td>
    <td width="25%" rowspan="4" valign="top">
    <div id="<%= ParentID %>_tdDonVi" style="height:200px;margin-top:5px; overflow:scroll;width:500px;" ></div></td>
    <td></td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td class="style9">&nbsp;</td>
    <td>&nbsp;</td>
    <td class="style8">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="center">&nbsp;</td>
    <td colspan="5" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center">
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
                            <td></td>
  </tr>
</table>



            </div>
        </div>
     <script type="text/javascript">
         function CheckAll(value) {
             $("input:checkbox[check-group='DonVi']").each(function (i) {
                 this.checked = value;
             });
         }                                            
     </script>
       <script type="text/javascript">
           ChonThang();
           function ChonThang() {
               var Thang = document.getElementById("<%=ParentID %>_iThang").value;

               var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
               jQuery.ajaxSetup({ cache: false });
               var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&Thang_Quy=#2&iID_MaDonVi=#3&iID_MaTrangThaiDuyet=#4", "rptQuyetToan_83") %>');
               url = unescape(url.replace("#0", "<%= ParentID %>"));
               url = unescape(url.replace("#1", "<%= MaND %>"));
               url = unescape(url.replace("#2", Thang));
               url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
               url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
               $.getJSON(url, function (data) {
                   document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
               });
           }                                            
     </script>
         <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    </div>
    <%} %>
    <div>
    </div>
    <%
        
        dtThang.Dispose();
    %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
     <script type="text/javascript">
         $(function () {
             $("div#rptMain").hide();
             $('div.login1 a').click(function () {
                 $('div#rptMain').slideToggle('fast');
                 $(this).toggleClass('active');
                 return false;
             });
         });
    </script>
     <iframe src="<%=urlReport%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>
