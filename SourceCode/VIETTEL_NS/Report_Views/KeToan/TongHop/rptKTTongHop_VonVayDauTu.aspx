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

        var dtTaiKhoan = TaiKhoanModels.DT_DSTaiKhoanGiaiThich_PhuongAn(User.Identity.Name, "163");
        SelectOptionList slTaiKhoan = new SelectOptionList(dtTaiKhoan, "sKyHieu", "sTen");
        if (dtTaiKhoan.Rows.Count > 0 && String.IsNullOrEmpty(MaTaiKhoan))
        {
            MaTaiKhoan = HamChung.ConvertToString(dtTaiKhoan.Rows[0]["sKyHieu"]);
        }
        if (dtTaiKhoan != null) dtTaiKhoan.Dispose();

        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        if (dtNam != null) dtNam.Dispose();
        //Kieu giay
        String KieuGiay = Convert.ToString(ViewData["KieuGiay"]);
        DataTable dtKieuGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKieuGiay = new SelectOptionList(dtKieuGiay, "MaKhoGiay", "TenKhoGiay");
        if (String.IsNullOrEmpty(KieuGiay))
            KieuGiay = "2";
        dtKieuGiay.Dispose();
        String DVT = Convert.ToString(ViewData["DVT"]);
        if (String.IsNullOrEmpty(DVT))
        {
            DVT = "0";
        }
        DataTable dtDVT = rptKTTH_TongHopCapVonController.DanhSach_LoaiBaoCao();
        SelectOptionList slLoaiBaoCao = new SelectOptionList(dtDVT, "MaLoai", "TenLoai");
        dtDVT.Dispose();
        //Trang Thai duyet
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = Luong_ReportModel.DachSachTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
            iID_MaTrangThaiDuyet = "1";
        dtTrangThai.Dispose();
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        String ControllerName = "rptKTTongHop_VonVayDauTu";
        String TK = Url.Action("Index", "rptKeToan_DanhMucTaiKhoanGiaiThich", new { sKyHieu = "163", ControllerName = ControllerName });
        String urlReport = "";
        if (PageLoad == "1")
        {
            urlReport = Url.Action("ViewPDF", "rptKTTongHop_VonVayDauTu", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iNamLamViec = "", iThang = "", DonViTinh = DVT, KhoGiay = KieuGiay });
        }
        using (Html.BeginForm("EditSubmit", "rptKTTongHop_VonVayDauTu", new { ParentID = ParentID }))
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
        <div id="confirmBox" title="Báo cáo tổng hợp vốn vay đầu tư" style="width:640px; min-height:150px; display:none;">
            <div>
                <div id="left" style="float:left; width:290px; text-align:left; padding:3px;">
                    <fieldset>
                        <legend><b><%=NgonNgu.LayXau("Danh sách tài khoản: (Ký hiệu 163)")%></b></legend>
                          <input  class="button" onclick='TaiKhoan()' value="<%=NgonNgu.LayXau("Thêm TK")%>"
                    style="display: inline-block; margin-right: 5px;" />
                        <p style="margin-bottom:3px;"><%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, MaTaiKhoan, "MaTaiKhoan", "", "class=\"input1_2\" style=\"width:280px; \" size='9' tab-index='-1'")%></p>                                                      
                    </fieldset>
                </div><!--End #left-->
                <div id="right" style="float:right; width:320px; text-align:left; padding:3px;">
                    
                    <fieldset style="margin-top:4px;">
                        <span class="spleft" style="text-align:left;">
                            <%=NgonNgu.LayXau("Kiểu giấy in:") %><%=MyHtmlHelper.DropDownList(ParentID, slKieuGiay, KieuGiay, "KieuGiay", "", "class=\"input1_2\" style=\"width: 100%;float:right;\" ")%>
                        </span>
                        <span class="spright" style="text-align:left;">
                            <%=NgonNgu.LayXau("Trạng thái:") %><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;float:right;\" ")%>
                        </span>
                         <span class="spleft" style="text-align:left;">
                            <%=NgonNgu.LayXau("ĐVT:") %><%=MyHtmlHelper.DropDownList(ParentID, slLoaiBaoCao, DVT, "DVT", "", "class=\"input1_2\" style=\"width: 100%;float:right;\" ")%>
                        </span>
                    </fieldset>      
            </div><!--End #right-->
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