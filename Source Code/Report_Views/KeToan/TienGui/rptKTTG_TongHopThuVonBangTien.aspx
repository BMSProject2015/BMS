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
    String iThang = Convert.ToString(ViewData["iThang"]);
        if(String.IsNullOrEmpty(iThang))
        {
            iThang = iThangLamViec;
        }
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang,"MaThang","TenThang");
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
        UrlReport = Url.Action("ViewPDF", "rptKTTG_TongHopThuVonBangTien", new { MaND = MaND, iID_MaTrangThaiDuyet = iTrangThai,iThang=iThang });
    using (Html.BeginForm("EditSubmit", "rptKTTG_TongHopThuVonBangTien", new { ParentID = ParentID }))
    {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>                    
                    <td width="47.9%"><span>Báo cáo tổng hợp thu vốn bằng tiền</span></td>
                    <td width="52%" style=" text-align:left;">
                        <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
         <%=MyHtmlHelper.Hidden(ParentID,iNamLamViec,"iNamLamViec","")%>
               <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                <tr>
                    <td width="20%">&nbsp;</td>
                     <td  class="td_form2_td1" width="10%">Chọn tháng :&nbsp;&nbsp; </td>
                <td width="10%"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "style=\"width:100%;padding:2px; border:1px solid #dedede;\"")%></td>
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
                
               </table>
        </div>
    </div>
    <%} %>
     <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTG_TongHopThuVonBangTien", new { MaND = MaND, iID_MaTrangThaiDuyet = iTrangThai, iThang = iThang }), "Xuất ra Excels")%>
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