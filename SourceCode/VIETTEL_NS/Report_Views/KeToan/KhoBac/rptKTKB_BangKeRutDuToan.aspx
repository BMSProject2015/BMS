<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.KhoBac" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        ul.inlineBlock{
	        list-style: none inside;			
        }
        ul.inlineBlock li{			
	        /*-webkit-box-shadow: 2px 2px 0 #cecece;
	        box-shadow: 2px 2px 0 #cecece;	*/	
	        -webkit-box-shadow: rgba(200, 200, 200, 0.7) 0 4px 10px -1px;
            box-shadow: rgba(200, 200, 200, 0.7) 0 4px 10px -1px;
	        padding: 2px 5px;
	        display: inline-block;
	        vertical-align: middle; /*Mở comment để xem thuộc tính vertical-align*/
	        margin-right: 3px;
	        margin-left: 0px;
	        font-size: 13px;			
	        border-radius: 3px;
	        position: relative;
	    /*fix for IE 7*/
	        zoom:1;
	        *display: inline;		        
        }
        ul.inlineBlock li span
        {
            padding:2px 1px;   
        }
        ul.inlineBlock li p{
            padding:1px;    
        }
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
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px -29px;
            -webkit-border-radius:2px;
            border-radius:2px;
        }    
        div.login1 a.active {
            background-position:  20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
        .errorafter
        {
           background-color:Yellow;
        }        
        ul.inlineBlock li fieldset .div
        {
            width:90px; height:23px; display:inline-block; text-align:center; padding:1px; font-size:14px;-moz-border-radius:3px;-webkit-border-radius:3px; border-radius:3px;
            font-family:Tahoma Arial; color:Black; line-height:23px; cursor:pointer; background-color:#dedede;  
        }
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
    String ParentID = "KeToan";
    String iID_MaDonVi_Nhan = Convert.ToString(ViewData["iID_MaDonVi_Nhan"]);
    String MaND = User.Identity.Name;
    DataTable dtCauHinh=NguoiDungCauHinhModels.LayCauHinh(MaND);
    String iNamLamViec=dtCauHinh.Rows[0]["iNamLamViec"].ToString();
    String iThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
    String iID_MaNguonNganSach = dtCauHinh.Rows[0]["iID_MaNguonNganSach"].ToString();
    String iID_MaNamNganSach = dtCauHinh.Rows[0]["iID_MaNamNganSach"].ToString();
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

    //Khổ giấy
    String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
    DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
    SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
    if (String.IsNullOrEmpty(KhoGiay))
    {
        KhoGiay = "2";
    }
    dtKhoGiay.Dispose();
    // Chọn Tờ
    String MaTo = Convert.ToString(ViewData["MaTo"]);
    DataTable dtMaTo = rptKTKB_BangKeRutDuToanController.dt_ToIn(iThang1, iNgay1, iThang2, iNgay2, iNamLamViec,iID_MaNguonNganSach,iID_MaNamNganSach, KhoGiay);
    SelectOptionList slMaTo = new SelectOptionList(dtMaTo, "MaTo", "TenTo");
    if (String.IsNullOrEmpty(MaTo))
    {
        MaTo = "1";
    }
    dtMaTo.Dispose();
    //Chọn Đơn vị tính
    String DVT = Convert.ToString(ViewData["DVT"]);
    DataTable dtDVT = rptKTKB_BangKeRutDuToanController.dt_DVT();
    SelectOptionList slDVT = new SelectOptionList(dtDVT, "MaDVT", "TenDVT");
    if (String.IsNullOrEmpty(DVT))
    {
        DVT = "1";
    }
    dtDVT.Dispose();
    String URL = Url.Action("Index", "KeToan_ChiTiet_Report");
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    DataTable dtTrangThai = Luong_ReportModel.DachSachTrangThai();
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    String iTrangThai = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    if (String.IsNullOrEmpty(iTrangThai))
        iTrangThai = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();
    String UrlReport = "";
    if (PageLoad == "1")
        UrlReport =Url.Action("ViewPDF","rptKTKB_BangKeRutDuToan",new{iThang1=iThang1,iNgay1 = iNgay1,iThang2=iThang2,iNgay2=iNgay2,iNamLamViec=iNamLamViec,KhoGiay=KhoGiay,MaTo=MaTo,DVT=DVT,iID_MaNguonNganSach=iID_MaNguonNganSach,iID_MaNamNganSach=iID_MaNamNganSach});
    using (Html.BeginForm("EditSubmit", "rptKTKB_BangKeRutDuToan", new { ParentID = ParentID }))
    {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>                    
                    <td width="47.9%"><span>Báo cáo bảng kê rút dự toán</span></td>
                    <td width="52%" style=" text-align:left;">
                        <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                    </td>
                </tr>
            </table>
        </div>
        <% rptKTKB_BangKeRutDuToanController rpt = new rptKTKB_BangKeRutDuToanController();
           rptKTKB_BangKeRutDuToanController.RutDuToan _Data = new rptKTKB_BangKeRutDuToanController.RutDuToan();
                               _Data = rpt.get_sNgayThang(ParentID, iNamLamViec,iID_MaNguonNganSach,iID_MaNamNganSach, iNgay1, iThang1, iNgay2, iThang2, KhoGiay, MaTo);
        %>
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width:100%; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:visible;">
                <ul class="inlineBlock">                       
                    <li>
                        <fieldset style="border:1px solid #cecece;padding:3px 5px;border-radius:3px;-moz-border-radius:3px;-webkit-border-radius:3px; font-size:13px;">
                            <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Từ ngày&nbsp;&nbsp;&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đến ngày&nbsp;&nbsp;&nbsp;Tháng")%></legend>
                            <p style="padding:3px; margin-bottom:5px;">
                                <label id="<%= ParentID %>_ngay1" style=" font-size:14px;padding:4px;"><%=_Data.Ngay1%></label>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang1, "iThang1", "", "style=\"width:70px;padding:2px; border:1px solid #dedede;\" onchange=\"ChonThang()\"")%>                                            
                                <label id="<%=ParentID %>_ngay2" style=" font-size:14px;padding:4px;"><%=_Data.Ngay2%></label>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang2, "iThang2", "", "style=\"width:70px;padding:2px; border:1px solid #dedede;\" onchange=\"ChonThang()\"")%>
                            </p>
                        </fieldset>
                    </li>
                    <li><p style="text-align:right; height:50px; padding:4px; "><span style="height:61px; vertical-align:top; padding:2px 5px;"><%=NgonNgu.LayXau("Khổ giấy:")%></span><%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "style=\"width:140px; padding:2px; height:50px; border:1px solid #dedede;font-size:14.5px;\" size='2' tab-index='-1' onchange=\"ChonThang()\"")%></p></li>
                    <%--<li><p style="text-align:right;height:50px; padding:4px;"><span style="height:61px; vertical-align:top; padding:2px 5px;"><%=NgonNgu.LayXau("Trạng thái duyệt:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 120px; padding:2px; height:50px; border:1px solid #dedede; font-size:14.5px;\" size='2' tab-index='-1'")%></p></li>--%>
                    <li>
                        <p style="text-align:right; padding:4px 3px 1px 3px; height:24.5px;"><span style="padding:2px 5px; margin-right:1px;"><%=NgonNgu.LayXau("Chọn tờ:")%></span><span id="<%= ParentID %>_ChonTo"><%=_Data.SoTo%></span></p>
                        <p style="text-align:right; padding:1px 3px 4px 3px; height:24.5px;"><span style="padding:2px 5px;"><%=NgonNgu.LayXau("Đơn vị tính:")%></span><%=MyHtmlHelper.DropDownList(ParentID, slDVT, DVT, "DVT", "", "style=\"width:150px; padding:2px;border:1px solid #dedede;\"")%></p>
                    </li>
                </ul><!--End .inlineBlock-->
                <%=MyHtmlHelper.Hidden(ParentID,iNamLamViec,"iNamLamViec","")%>
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
            </div><!--End #rptMain-->
        </div><!--End #table_form2-->
    </div>
    <%} %>
     <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTKB_BangKeRutDuToan", new { iThang1 = iThang1, iNgay1 = iNgay1, iThang2 = iThang2, iNgay2 = iNgay2, iNamLamViec = iNamLamViec, KhoGiay = KhoGiay, MaTo = MaTo, DVT = DVT, iID_MaNguonNganSach = iID_MaNguonNganSach, iID_MaNamNganSach = iID_MaNamNganSach }), "Xuất ra Excels")%>
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
               var KhoGiay = document.getElementById("<%=ParentID %>_KhoGiay").value;
               var iNamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value;
               jQuery.ajaxSetup({ cache: false });
               var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&iNamLamViec=#1&iID_MaNguonNganSach=#2&iID_MaNamNganSach=#3&iNgay1=#4&iThang1=#5&iNgay2=#6&iThang2=#7&KhoGiay=#8&MaTo=#9","rptKTKB_BangKeRutDuToan") %>');
               url = unescape(url.replace("#0", "<%= ParentID %>"));
               url = unescape(url.replace("#1", iNamLamViec));
               url = unescape(url.replace("#2", "<%=iID_MaNguonNganSach%>"));
               url = unescape(url.replace("#3", "<%=iID_MaNamNganSach%>"));
               url = unescape(url.replace("#4", iNgay1));
               url = unescape(url.replace("#5", iThang1));
               url = unescape(url.replace("#6", iNgay2));
               url = unescape(url.replace("#7", iThang2));
               url = unescape(url.replace("#8", KhoGiay));
               url = unescape(url.replace("#9","<%=MaTo%>"));
               $.getJSON(url, function (data) {
                   document.getElementById("<%= ParentID%>_ngay1").innerHTML = data.Ngay1;
                   document.getElementById("<%= ParentID%>_ngay2").innerHTML = data.Ngay2;
                   document.getElementById("<%= ParentID%>_ChonTo").innerHTML = data.SoTo;
               });
           }                                            
     </script>
     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>    
</body>
</html>