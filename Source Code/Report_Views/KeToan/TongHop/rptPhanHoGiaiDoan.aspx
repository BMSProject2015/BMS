<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="../../../Content/Report_Style/ReportCSS.css" />
    <style type="text/css">
       ul.inlineBlock li fieldset legend{text-align:left; padding:3px 6px;font-size:13px;}  
       ul.inlineBlock li fieldset p{padding:2px 4px; text-align:left;}
    </style>
</head>
<body>
     <%
    String ParentID = "KeToan";
    String MaND = User.Identity.Name;
         String PageLoad=Convert.ToString(ViewData["PageLoad"]);
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
    String iThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
    String iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
    String iThang1 = Convert.ToString(ViewData["iThang1"]);
    if (String.IsNullOrEmpty(iThang1))
        iThang1 = "1";
    String iThang2 = Convert.ToString(ViewData["iThang2"]);
    if (String.IsNullOrEmpty(iThang2))
        iThang2 = iThangLamViec;
    String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
    var dtiID_MaTaiKhoan = rptPhanHoGiaiDoanController.TenTaiKhoan(iNamLamViec);
    SelectOptionList slTK = new SelectOptionList(dtiID_MaTaiKhoan, "iID_MaTaiKhoan", "TenTK");
    if (String.IsNullOrEmpty(iID_MaTaiKhoan))
    {
        if (dtiID_MaTaiKhoan.Rows.Count > 0)
        {
            iID_MaTaiKhoan = Convert.ToString(dtiID_MaTaiKhoan.Rows[0]["iID_MaTaiKhoan"]);
        }
        else
        {
            iID_MaTaiKhoan = "";
        }
    }
         
   //Chọn tháng1
    DataTable dtThang1 = DanhMucModels.DT_Thang();
    SelectOptionList slThang1 = new SelectOptionList(dtThang1, "MaThang", "TenThang");
    if (String.IsNullOrEmpty(iThang1))
    {
        iThang1 = Convert.ToString(dtThang1.Rows[1]["TenThang"]);
    }
    dtThang1.Dispose();
    //Chọn tháng2
    DataTable dtThang2 = DanhMucModels.DT_Thang();
    SelectOptionList slThang2 = new SelectOptionList(dtThang2, "MaThang", "TenThang");
    if (String.IsNullOrEmpty(iThang2))
    {
        iThang2 = Convert.ToString(dtThang2.Rows[1]["TenThang"]);
    }
    dtThang2.Dispose();
    //chọn loại báo cáo
    String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
    if (String.IsNullOrEmpty(KhoGiay))
    {
        KhoGiay = "rNgang";
    }
    DataTable dtKhoGiay = rptPhanHoGiaiDoanController.DanhSach_LoaiBaoCao();
    SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaLoai", "TenLoai");
    dtKhoGiay.Dispose();

    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    DataTable dtTrangThai = Luong_ReportModel.DachSachTrangThai();
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet","sTen");
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = "1";
    }
    dtTrangThai.Dispose();
    //Đơn vị
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    if (String.IsNullOrEmpty(iID_MaDonVi)||iID_MaDonVi=="00000000-0000-0000-0000-000000000000") 
    {
        PageLoad = "0";
    }
    String UrlReport = "";
    if (PageLoad == "1")
    {
        UrlReport = Url.Action("ViewPDF", "rptPhanHoGiaiDoan", new { MaND = MaND, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1, iThang2 = iThang2, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay });
    }
    String BackURL = Url.Action("Index", "KeToan_Report");
    using (Html.BeginForm("EditSubmit", "rptPhanHoGiaiDoan", new { ParentID = ParentID }))
   {
    %>   
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width:48%;"><span>Báo cáo phân hộ giai đoạn</span></td>
                    <td><div class="login1" style="width:50px;"><a href="#"></a></div></td>
                </tr>
            </table>
        </div><!---End .title_tong--->         
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width:100%; margin:0px auto; padding:0px 0px; overflow:visible; text-align:center; ">
                <ul class="inlineBlock">
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Mã tài khoản:")%></legend>
                            <p>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTK, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 250px; \" onchange=LocDV()")%>
                            </p>
                        </fieldset>                    
                    </li>
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Từ tháng&nbsp;&nbsp;&nbsp;Đến tháng")%></legend>                           
                            <p>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang1, iThang1, "iThang1", "", "class=\"input1_2\" style=\"width: 60px;\" onchange=LocDV()")%>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang2, iThang2, "iThang2", "", "class=\"input1_2\" style=\"width: 60px;\" onchange=LocDV()")%>
                            </p>                        
                        </fieldset>
                    </li>                
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Khổ giấy:")%></legend>
                            <p><%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 110px;\" onchange=LocDV()")%></p>
                        </fieldset>                    
                    </li>
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Trạng thái:")%></legend>
                            <p><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 180px;\" onchange=LocDV()")%></p>
                        </fieldset>                    
                    </li>
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Đơn vị:") %></legend>
                            <p id="<%=ParentID %>_divDonVi">
                                <%rptPhanHoGiaiDoanController rptTB1 = new rptPhanHoGiaiDoanController();%>
                                <%=rptTB1.obj_DSDonVi(ParentID,MaND,iID_MaTaiKhoan,iThang1,iThang2,iID_MaTrangThaiDuyet, iID_MaDonVi)%>
                            </p>
                        </fieldset>                    
                    </li>
                </ul><!---End .inlineBlock--->
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
            </div><!---End #rptMain--->
        </div><!----End #table_form2---->
    </div><!---End .box_tong--->
    <%} %>
  <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptPhanHoGiaiDoan", new { MaND = MaND, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1, iThang2 = iThang2, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay }), "Xuất ra file Excel")%>
     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>
    <script type="text/javascript">
        $(function () {
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });            
        });
    </script>
     <script type="text/javascript">
         function LocDV() {
             var iID_MaTaiKhoan = document.getElementById("<%=ParentID %>_iID_MaTaiKhoan").value;
             var iThang1 = document.getElementById("<%=ParentID %>_iThang1").value;
             var iThang2 = document.getElementById("<%=ParentID %>_iThang2").value;
             var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&MaND=#1&iID_MaTaiKhoan=#2&iThang1=#3&iThang2=#4&iID_MaTrangThaiDuyet=#5&iID_MaDonVi=#6", "rptPhanHoGiaiDoan") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", "<%= MaND %>"));
             url = unescape(url.replace("#2", iID_MaTaiKhoan));
             url = unescape(url.replace("#3", iThang1));
             url = unescape(url.replace("#4", iThang2));
             url = unescape(url.replace("#5", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#6", "<%= iID_MaDonVi %>"));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
             });
         }
         function Huy() {
             window.location.href = '<%=BackURL %>';
         }                                       
     </script>
</body>
</html>
