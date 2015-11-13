<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienGui" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.KhoBac" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
      
        div.login1 {
            text-align : center;    
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }    
        div.login1 a {
            color: white;
            text-decoration: none;
            font: bold 16px "Museo 700";
            display: block;
            width: 50px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px 1px;
            -webkit-border-radius:2px;
            border-radius:2px;
        }    
        div.login1 a.active {
            background-position:  20px -29px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
                
    </style>
   
</head>
<body>
    <%
    String ParentID = "KeToan";
    String MaND = User.Identity.Name;
    DataTable dtCauHinh=NguoiDungCauHinhModels.LayCauHinh(MaND);
    String iNamLamViec=dtCauHinh.Rows[0]["iNamLamViec"].ToString();
    String iThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
    String iNgay1=Convert.ToString(ViewData["iNgay1"]);
    String iNgay2=Convert.ToString(ViewData["iNgay2"]);
    String iThang1=Convert.ToString(ViewData["iThang1"]);
    String iThang2=Convert.ToString(ViewData["iThang2"]);
    if (String.IsNullOrEmpty(iThang1))
    {
        iThang1 = iThangLamViec;
    }
    if (String.IsNullOrEmpty(iThang2))
    {
        iThang2 = iThangLamViec;
    }
    DataTable dtNgay1 = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang1), Convert.ToInt16(iNamLamViec));
    SelectOptionList slNgay1 = new SelectOptionList(dtNgay1, "MaNgay", "TenNgay");
    if (String.IsNullOrEmpty(iNgay1))
    {
        iNgay1 = "1";
    }
    dtNgay1.Dispose();
    DataTable dtNgay2 = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang2), Convert.ToInt16(iNamLamViec));
    SelectOptionList slNgay2 = new SelectOptionList(dtNgay2, "MaNgay", "TenNgay");
    int SoNgayTrongThang2 = DateTime.DaysInMonth(Convert.ToInt16(iNamLamViec), Convert.ToInt16(iThang2));
    if (String.IsNullOrEmpty(iNgay2))
    {
        iNgay2 = dtNgay2.Rows[SoNgayTrongThang2]["MaNgay"].ToString();
    }
    dtNgay2.Dispose();
    DataTable dtThang = DanhMucModels.DT_Thang(false);
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();
           
    String URL = Url.Action("Index", "KeToan_ChiTiet_Report");
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    DataTable dtTrangThai = Luong_ReportModel.DachSachTrangThai();
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    String iTrangThai = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    if (String.IsNullOrEmpty(iTrangThai))
        iTrangThai = "-100";
    String UrlReport = "";
    if (PageLoad == "1")
        UrlReport = Url.Action("ViewPDF", "rptKTTG_TongHopThucThu_ThucChiVonBangTien", new { MaND = MaND, iNgay1 = iNgay1, iThang1 = iThang1, iNgay2 = iNgay2, iThang2 = iThang2, iID_MaTrangThaiDuyet = iTrangThai });
    using (Html.BeginForm("EditSubmit", "rptKTTG_TongHopThucThu_ThucChiVonBangTien", new { ParentID = ParentID }))
    {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>                    
                    <td width="47.9%"><span>Báo cáo tổng hợp thực thu - thực chi vốn bằng tiền</span></td>
                    <td width="52%" style=" text-align:left;">
                        <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
          <% rptKeToanTongHopNgayController rpt = new rptKeToanTongHopNgayController();
             rptKeToanTongHopNgayController.RutDuToan _Data = new rptKeToanTongHopNgayController.RutDuToan();
                               _Data = rpt.get_sNgayThang(ParentID, iNamLamViec,iNgay1, iThang1, iNgay2, iThang2);
        %>
         <%=MyHtmlHelper.Hidden(ParentID,iNamLamViec,"iNamLamViec","")%>
               <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                <tr>
                    <td width="10%">&nbsp;</td>
                              <td class="td_form2_td1" width="10%" >Chọn ngày:&nbsp;&nbsp; </td>
                            <td width="25%"><fieldset style="border:1px solid #cecece;padding:3px 5px;border-radius:3px;-moz-border-radius:3px;-webkit-border-radius:3px; font-size:13px;">
                            <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Từ ngày&nbsp;&nbsp;&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đến ngày&nbsp;&nbsp;&nbsp;Tháng")%></legend>
                            <p style="padding:3px; margin-bottom:5px;">
                                <label id="<%= ParentID %>_ngay1" style=" font-size:14px;padding:4px;"><%=_Data.Ngay1%></label>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang1, "iThang1", "", "style=\"width:50px;padding:2px; border:1px solid #dedede;\" onchange=\"ChonThang()\"")%>                                            
                                <label id="<%=ParentID %>_ngay2" style=" font-size:14px;padding:4px;"><%=_Data.Ngay2%></label>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang2, "iThang2", "", "style=\"width:50px;padding:2px; border:1px solid #dedede;\" onchange=\"ChonThang()\"")%>
                            </p>
                        </fieldset> </td>
                        <td  class="td_form2_td1" width="10%">Trạng thái :&nbsp;&nbsp; </td>
                <td width="15%"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "style=\"width:100%;padding:2px; border:1px solid #dedede;\"")%></td>
                    <td></td>
                </tr>
                <tr>
                <td></td>
                <td></td>
                  <td></td>                      
                       <td colspan="1"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
               
                <td></td>
                </tr>
                <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                </tr>
                <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                </tr>
                <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                </tr>
               </table>
        </div>
    </div>
    <%} %>
     <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTG_TongHopThucThu_ThucChiVonBangTien", new { MaND = MaND, iNgay1 = iNgay1, iThang1 = iThang1, iNgay2 = iNgay2, iThang2 = iThang2, iID_MaTrangThaiDuyet = iTrangThai }), "Xuất ra Excels")%>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        $(function () {
//         $("div#rptMain").hide();
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });            
        });
    </script>
    <script type="text/javascript">
        function ChonThang() {
            var iNgay1 = document.getElementById("<%=ParentID %>_iNgay1").value;
            var iThang1 = document.getElementById("<%=ParentID %>_iThang1").value;
            var iNgay2 = document.getElementById("<%=ParentID %>_iNgay2").value;
            var iThang2 = document.getElementById("<%=ParentID %>_iThang2").value;
            var iNamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&iNamLamViec=#1&iNgay1=#2&iThang1=#3&iNgay2=#4&iThang2=#5","rptKeToanTongHopNgay") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iNamLamViec));
            url = unescape(url.replace("#2", iNgay1));
            url = unescape(url.replace("#3", iThang1));
            url = unescape(url.replace("#4", iNgay2));
            url = unescape(url.replace("#5", iThang2));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID%>_ngay1").innerHTML = data.Ngay1;
                document.getElementById("<%= ParentID%>_ngay2").innerHTML = data.Ngay2;
            });
        }                                            
     </script>
     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>    
</body>
</html>