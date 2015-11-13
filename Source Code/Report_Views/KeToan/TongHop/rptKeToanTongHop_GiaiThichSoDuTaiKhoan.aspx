<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../../Scripts/Report/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/Report/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/Report/ShowDialog.js" type="text/javascript"></script>
    <link href="../../../Content/Report_Style/ReportCSS.css" rel="StyleSheet" type="text/css" />
    <style type="text/css">
        .spleft{float:left; padding:2px; font-size:13px; width:135px; text-align:center; vertical-align:middle;}
        .spright{float:right; padding:2px; font-size:13px;width:135px; text-align:center;vertical-align:middle;}    
        fieldset{padding:3px;border:1px solid #dedede; border-radius:3px; -webkit-border-radius:3px; -moz-border-radius:3px;}
        fieldset legend{padding:3px; font-size:13px;font-family:Tahoma Arial;}    
        fieldset p{padding:2px;}
        span.span{padding:2px; font-size:12px;}
    </style>
</head>
<body>
    <% 
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "KeToan";
        String Cap = "3";
        String LoaiBieu = Convert.ToString(ViewData["LoaiBieu"]);
        if (String.IsNullOrEmpty(LoaiBieu))
        {
            LoaiBieu = "rDonViNoiDung";
        }

        String MaTaiKhoan = Convert.ToString(ViewData["MaTaiKhoan"]);
        String Thang = Convert.ToString(ViewData["Thang"]);
        String Nam = Convert.ToString(ViewData["Nam"]);
        //lay nam thang theo nguoi dung
        String MaND = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        if (dtCauHinh.Rows.Count > 0)
        {
            Nam = HamChung.ConvertToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            if (String.IsNullOrEmpty(Thang))
            {
                Thang = HamChung.ConvertToString(dtCauHinh.Rows[0]["iThangLamViec"]);
            }
        }
        if (dtCauHinh != null) dtCauHinh.Dispose();
        //if (String.IsNullOrEmpty(Thang))
        //{
        //    Thang = DateTime.Now.Month.ToString();
        //}
        //if (String.IsNullOrEmpty(Nam))
        //{
        //    Nam = DateTime.Now.Year.ToString();
        //}
        //tháng     
        var dtThang = HamChung.getMonth(DateTime.Now, false, "", "");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        //var dtTaiKhoan = TaiKhoanModels.DT_DSTaiKhoan_PhuongAn(false, "", User.Identity.Name, "63");
        var dtTaiKhoan = rptKeToanTongHop_GiaiThichSoDuTaiKhoanController.TaiKhoan(Convert.ToInt32(Cap), Nam);
        SelectOptionList slTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "sTen");
        if (dtTaiKhoan.Rows.Count > 0 && String.IsNullOrEmpty(MaTaiKhoan))
        {
            MaTaiKhoan = HamChung.ConvertToString(dtTaiKhoan.Rows[0]["iID_MaTaiKhoan"]);
        }
        if (dtTaiKhoan != null) dtTaiKhoan.Dispose();

        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        if (dtNam != null) dtNam.Dispose();
        //Kieu giay
        String KieuGiay = Convert.ToString(ViewData["KieuGiay"]);
        DataTable dtKieuGiay = rptKeToanTongHop_GiaiThichSoDuTaiKhoanController.dtKieuGiay();
        SelectOptionList slKieuGiay = new SelectOptionList(dtKieuGiay, "MaKieuGiay", "TenKieuGiay");
        if (String.IsNullOrEmpty(KieuGiay))
            KieuGiay = "rngang";
        dtKieuGiay.Dispose();
        //Trang Thai duyet
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = Luong_ReportModel.DachSachTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
            iID_MaTrangThaiDuyet = "2";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        String ControllerName = "rptKeToanTongHop_GiaiThichSoDuTaiKhoan";
        String TK = Url.Action("Index", "rptKeToan_DanhMucTaiKhoan", new { sKyHieu = "63", ControllerName = ControllerName });
        String urlReport = "";
        if (PageLoad == "1")
        {
            urlReport = Url.Action("ViewPDF", "rptKeToanTongHop_GiaiThichSoDuTaiKhoan", new { MaTaiKhoan = MaTaiKhoan, Thang = Thang, Nam = Nam, LoaiBieu = LoaiBieu, KieuGiay = KieuGiay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
       
        using (Html.BeginForm("EditSubmit", "rptKeToanTongHop_GiaiThichSoDuTaiKhoan", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo phân tích số dư tài khoản</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div><!--End .title_tong-->        
        <div id="confirmBox" title="Báo cáo phân tích số dư tài khoản" style="width:640px; min-height:150px; display:none;">
            <div>
              
                <div id="left" style="float:left; width:320px; text-align:left; padding:3px;">
                    <fieldset>
                        <legend><%=NgonNgu.LayXau("Loại biểu in ra")%></legend>
                        <p>
                            <%=MyHtmlHelper.Option(ParentID, "rDonViNoiDung", LoaiBieu, "LoaiBieu", "", "onchange=\"Chon()\"")%><span class="span"><b>Đơn vị + Nội dung</b></span>
                            <%=MyHtmlHelper.Option(ParentID, "rDonVi", LoaiBieu, "LoaiBieu", "", "onchange=\"Chon()\"")%><span class="span"><b>Theo đơn vị</b></span>
                            <%=MyHtmlHelper.Option(ParentID, "rNoiDung", LoaiBieu, "LoaiBieu", "", "onchange=\"Chon()\"")%><span class="span"><b>Theo nội dung</b></span>
                        </p>
                    </fieldset>     
                    <div style="display:none;">
                        <%=MyHtmlHelper.TextBox(ParentID, LoaiBieu, "divLoaiBieu", "", "")%>
                    </div>    
                    <fieldset>
                        <legend><%=NgonNgu.LayXau("Chọn thời gian:") %></legend>
                        <p>
                            <span class="spleft">
                               <%=NgonNgu.LayXau("Tháng:") %><%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "Thang", "", "class=\"input1_2\" style=\"width: 70px; float:right;\" ")%>  
                            </span>
                            <span class="spright">
                                <%=NgonNgu.LayXau("Năm:") %><%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "Nam", "", "class=\"input1_2\" style=\"width:80px;float:right;\"")%>
                            </span>
                        </p>
                    </fieldset>
                    <fieldset style="margin-top:4px;">
                        <span class="spleft" style="text-align:left;">
                            <%=NgonNgu.LayXau("Kiểu giấy in:") %><%=MyHtmlHelper.DropDownList(ParentID, slKieuGiay, KieuGiay, "KieuGiay", "", "class=\"input1_2\" style=\"width: 100%;float:right;\" ")%>
                        </span>
                        <span class="spright" style="text-align:left;">
                            <%=NgonNgu.LayXau("Trạng thái:") %><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;float:right;\" ")%>
                        </span>
                    </fieldset>  
                     <fieldset>
                        <legend><%=NgonNgu.LayXau("Chọn đến TK cấp:") %></legend>
                        <p><%=MyHtmlHelper.Option(ParentID,"3",Cap,"CapTK",""," style=\"width:30px\" onclick=\"ChonCap(this.value)\"") %><span><%=NgonNgu.LayXau("Cấp 1") %></span></p>
                        <p><%=MyHtmlHelper.Option(ParentID,"4",Cap,"CapTK",""," style=\"width:30px\" onclick=\"ChonCap(this.value)\"") %><span><%=NgonNgu.LayXau("Cấp 2") %></span></p>
                        <p><%=MyHtmlHelper.Option(ParentID,"5",Cap,"CapTK",""," style=\"width:30px\" onclick=\"ChonCap(this.value)\"") %><span><%=NgonNgu.LayXau("Cấp 3") %></span></p>
                        <p><%=MyHtmlHelper.Option(ParentID,"6",Cap,"CapTK",""," style=\"width:30px\" onclick=\"ChonCap(this.value)\"") %><span><%=NgonNgu.LayXau("Cấp 4") %></span></p>     
                           
                    </fieldset>    
            </div><!--End #right-->

              <div id="right" style="float:right; width:290px; text-align:left; padding:3px;">
                    <fieldset>
                        <legend><b><%=NgonNgu.LayXau("Danh sách tài khoản:")%></b></legend>
                          <%--<input  class="button" onclick='TaiKhoan()' value="<%=NgonNgu.LayXau("Thêm TK- (Ký hiệu 63)")%>"
                    style="display: inline-block; margin-right: 5px;" />--%>
                        <p style="margin-bottom:3px;"><%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, MaTaiKhoan, "MaTaiKhoan", "", "class=\"input1_2\" style=\"width:280px; \" size='15' tab-index='-1'")%></p>                                                      
                    </fieldset>
                </div><!--End #left-->
        </div><!--End div-->       
        <p style="text-align:center; padding:4px; clear:both;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>
        </div><!--End #confirmBox-->
    </div>
    <%} %>
    <script type="text/javascript">
        function TaiKhoan() {
            window.location.href = '<%=TK %>';
        }   
    </script>
    <script type="text/javascript">
 function ChonCap(value)
           {
              
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Get_objTaiKhoan?CapTK=#0&iNamLamViec=#1", "rptKeToanTongHop_GiaiThichSoDuTaiKhoan") %>');
                    url = unescape(url.replace("#0",value));
                    url = unescape(url.replace("#1", <%=Nam %>));
                    $.getJSON(url, function (data) {
                        document.getElementById("<%= ParentID %>_MaTaiKhoan").innerHTML = data;
                
                    });                          
           }
        function Chon() {
            var TenLoaiThang_Quy = document.getElementsByName("<%=ParentID %>_LoaiBieu");
            var LoaiThang_Quy;
            var i = 0;
            for (i = 0; i < TenLoaiThang_Quy.length; i++) {
                if (TenLoaiThang_Quy[i].checked) {
                    LoaiThang_Quy = TenLoaiThang_Quy[i].value;
                }
            }
            document.getElementById("<%= ParentID %>_divLoaiBieu").value = LoaiThang_Quy;
        }
        $(function () {
            if ("<%=PageLoad %>" == '0') {
                ShowDialog(630);
            }
            $('*').keyup(function (e) {
                if (e.keyCode == '27') {
                    Hide();
                }
            });
            $('div.login1 a').click(function () {
                ShowDialog(630);
            });
        });
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }            
                                
    </script>
    
    <iframe  src="<%=urlReport%>" height="600px" width="100%"></iframe>
</body>
</html>