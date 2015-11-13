<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.NguoiCoCong" %>
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
            font: bold 12px "Museo 700";
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
    
     .style8
     {
         width: 8px;
     }
     .style9
     {
         width: 25%px;
     }
     .style10
     {
         width: 9px;
     }
     .style11
     {
         width: 20%;
     }
    
 </style>
</head>
<body>
     <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "NguoiCoCong";
        
        String MaND = User.Identity.Name;
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptTCKK_ThongTriController.tbTrangThai();
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
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String LoaiThangQuy = Convert.ToString(ViewData["LoaiThangQuy"]);
        if (String.IsNullOrEmpty(LoaiThangQuy))
        {
            LoaiThangQuy = "0";
        }
      
     
        // dt Tháng
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            if (dtThang.Rows.Count > 0)
            {
                
            }
        }
        dtThang.Dispose();
        //Dt Quý
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        //dt Loại Ngân Sách
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        DataTable dtLNS = ReportModels.NS_LoaiNganSachNguoiCoCong();
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
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
        dtLNS.Dispose();
        // dt Đơn vị
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
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
     
        String[] arrDonVi = iID_MaDonVi.Split(',');

        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        dtDonVi.Dispose();
        String Kieu = Convert.ToString(ViewData["Kieu"]);
        if (String.IsNullOrEmpty(Kieu))
        {
            Kieu = Convert.ToString(ViewData["Kieu"]);
        }
        String[] arrKieu = { "1", "2" };
        if (String.IsNullOrEmpty(Kieu))
            Kieu = arrKieu[0];
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptTCKK_ThongTri", new { MaND = MaND, LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, TruongTien = TruongTien, Kieu = Kieu });
        String BackURL = Url.Action("Index", "NguoiCoCong_Report", new { iLoai = 2 });
        String urlExport = Url.Action("ExportToExcel", "rptTCKK_ThongTri", new { MaND = MaND, LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,TruongTien=TruongTien,Kieu=Kieu });
        using (Html.BeginForm("EditSubmit", "rptTCKK_ThongTri", new { ParentID = ParentID}))
        {
    %>
   <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Thông Tri Trợ cấp khó khăn</span>
                    </td>
                    <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="rptMain"> 
<div style="background-color:#F0F9FE;margin:0 0 0 0;padding-top:8px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td class="style8">&nbsp;</td>
    <td width="10%" align="right"><b>Loại ngân sách : </b></td>
    <td class="style11"><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\"style=\"width: 100%;\" onchange=Chon()")%></td>
    <td width="10%" align="right"><b>Tháng / Quý : </b></td>
    <td class="style9">&nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "0", LoaiThangQuy, "LoaiThangQuy", "", "onchange=Chon()")%>Tháng&nbsp;
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "onchange=Chon()", "class=\"input1_2\" style=\"width:20%;\"onchange=Chon() ")%> &nbsp; &nbsp; &nbsp; &nbsp;
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThangQuy, "LoaiThangQuy", "", "onchange=Chon()")%>Quý&nbsp;
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "iQuy", "", "class=\"input1_2\" style=\"width:20%;\"onchange=Chon()")%></td>
    <td width="144" rowspan="2" align="center"><fieldset style="text-align:left;padding:2px 2px 8px 8px;font-size:11px;width:160px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Trường tiền") %></b></legend>
                           &nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "", "onchange=Chon()")%>&nbsp;&nbsp;Tự Chi<br />
                           &nbsp; <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "onchange=Chon()")%>&nbsp;&nbsp;Hiện Vật 
                           </fieldset> </td>
    <td width="168" rowspan="2" align="center"><fieldset style="text-align:left;padding:2px 2px 8px 8px;font-size:11px;width:160px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Tổng hợp theo") %></b></legend>
                            &nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "1", Kieu, "Kieu", "")%>&nbsp;&nbsp;Chi tiết từng đơn vị<br />
                            &nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "2", Kieu, "Kieu", "")%>&nbsp;&nbsp;Tổng hợp
                           </fieldset></td>
  </tr>
  <tr>
    <td class="style8">&nbsp;</td>
    <td align="right"><b>Trạng thái : </b></td>
    <td class="style11"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon()")%></td>
    <td align="right"><b>Đơn vị : </b></td>
    <td id="<%= ParentID %>_tdDonVi" valign="top" 
          style="margin:0;padding:0;" class="style9"> <%rptTCKK_ThongTriController rptTB1 = new rptTCKK_ThongTriController();%>                                    
                                        <%=rptTB1.obj_DonViTheoNam(ParentID, MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet,TruongTien)%></td>
  </tr>
  <tr>
    <td colspan="7" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin:10px;" >
                                <tr>
                                    <td><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table></td>
  </tr>
</table>

</div>

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
           function Huy() {
               window.location.href = '<%=BackURL%>';
           }
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

         }                                            
    </script>
        <script type="text/javascript">
            function Chon() {
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
                var bTruongTien = document.getElementById("<%= ParentID %>_TruongTien").checked;
                var TruongTien = "";
                if (bTruongTien) TruongTien = "rTuChi";
                else TruongTien = "rHienVat";              
                var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
                var sLNS = document.getElementById("<%=ParentID %>_sLNS").value
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&LoaiThangQuy=#2&Thang_Quy=#3&sLNS=#4&iID_MaDonVi=#5&iID_MaTrangThaiDuyet=#6&TruongTien=#7", "rptTCKK_ThongTri") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", "<%= MaND %>"));
                url = unescape(url.replace("#2", LoaiThangQuy));
                url = unescape(url.replace("#3", Thang));
                url = unescape(url.replace("#4", sLNS));
                url = unescape(url.replace("#5", "<%= iID_MaDonVi %>"));
                url = unescape(url.replace("#6", iID_MaTrangThaiDuyet));
                url = unescape(url.replace("#7", TruongTien));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }                                            
     </script>
     <div>
     </div>
     </div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    <%}%>
    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
