<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienGui" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        div.login1
        {
            text-align: center;
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }
        div.login1 a
        {
            color: white;
            text-decoration: none;
            font: bold 16px "Museo 700";
            display: block;
            width: 50px;
            height: 20px;
            line-height: 20px;
            margin: 0px auto;
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px -29px;
            -webkit-border-radius: 2px;
            border-radius: 2px;
        }
        div.login1 a.active
        {
            background-position: 20px 1px;
        }
        div.login1 a:active, a:focus
        {
            outline: none;
        }
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
        string ParentID = "SoTienGui";
        String UserID = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
        String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
        String sLocDuLieu = Convert.ToString(ViewData["locDuLieu"]);
        bool bTrangThaiDuyet = false;
        DataTable dtTaiKhoan = rptKTTienGui_SoTienGuiController.dsTaiKhoan(iNamLamViec);
        SelectOptionList slTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "TenHT");
        dtTaiKhoan.Dispose();
        int iNam = DateTime.Now.Year;
        String iTaiKhoan = Convert.ToString(ViewData["iTaiKhoan"]);
        String iNgay1 = Convert.ToString(ViewData["iNgay1"]);
        String iNgay2 = Convert.ToString(ViewData["iNgay2"]);
        String iThang1 = Convert.ToString(ViewData["iThang1"]);
        String iThang2 = Convert.ToString(ViewData["iThang2"]);
        if (String.IsNullOrEmpty(iThang1)) iThang1 = "1";
        if (String.IsNullOrEmpty(iThang2)) iThang2 = "1";
        int ThangHienTai = DateTime.Now.Month;
        //Chọn từ tháng
        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        //Chọn từ ngày
        DataTable dtNgay = DanhMucModels.DT_Ngay(ThangHienTai, iNam, false);
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        if (String.IsNullOrEmpty(iNgay1))
        {
            iNgay1 = Convert.ToString(dtNgay.Rows[0]["TenNgay"]);
        }
        if (String.IsNullOrEmpty(iNgay2))
        {
            iNgay2 = Convert.ToString(dtNgay.Rows[dtNgay.Rows.Count - 1]["TenNgay"]);
        }
        if (String.IsNullOrEmpty(sLocDuLieu))
        {
            sLocDuLieu = "0";
        }
        if (sLocDuLieu == "0") bTrangThaiDuyet = false;
        else bTrangThaiDuyet = true;
        dtNgay.Dispose();
        String URL = Url.Action("Index", "KeToan_ChiTiet_Report");
        String UrlReport = String.Empty;
        if (Convert.ToString(ViewData["LoadPage"]).Equals("1"))
        {
            UrlReport = Url.Action("ViewPDF", "rptKTTienGui_SoTienGui", new { iNgay1 = iNgay1, iNgay2 = iNgay2, iThang1 = iThang1, iThang2 = iThang2, iTaiKhoan = iTaiKhoan, iNamLamViec = iNamLamViec, bTrangThaiDuyet = bTrangThaiDuyet });
        }
        String ControllerName = "rptKTTienGui_SoTienGui";
        String TK = Url.Action("Index", "rptKeToan_DanhMucTaiKhoan", new { sKyHieu = "68", ControllerName = ControllerName });
        using (Html.BeginForm("EditSubmit", "rptKTTienGui_SoTienGui", new { ParentID = ParentID, iNamLamViec = iNamLamViec }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo số tiền gửi</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width: 100%; margin: 0px auto; padding: 0px 0px; overflow: visible;">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td style="width: 40%">
                            <div>
                                <fieldset>
                                    <legend><b>Danh sánh tài khoản</b></legend>
                                     <input  class="button" onclick='TaiKhoan()' value="<%=NgonNgu.LayXau("Thêm TK")%>"
                    style="display: inline-block; margin-right: 5px;" />
                                    <p>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iTaiKhoan, "TaiKhoan", "", "class=\"input1_2\" style=\"width: 100%;height:100px; padding:4px 2px 4px 2px;\" size='4' tab-index='-1'")%></p>
                                </fieldset>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="padding-left: 20px; width: 30%" valign="top">
                            <div>
                                <fieldset style="width: 80%; height: 80px">
                                    <legend><b>Lọc dữ liệu</b></legend>
                                    <table>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    <b>Tất cả</b></div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "0", sLocDuLieu, "iLocDuLieu", "", " style=\"width:30px\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    <b>Đã phê duyệt</b></div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "1", sLocDuLieu, "iLocDuLieu", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="padding-left: 20px; width: 30%" valign="top">
                            <div>
                                <fieldset style="width: 80%; height: 80px">
                                    <legend><b>Từ Ngày ... tháng Đến Ngày ... tháng</b></legend>
                                    <table>
                                        <tr>
                                            <td class="td_form2_td1" width="80px" style="padding-right: 10px;">
                                                <div>
                                                    Từ Ngày</div>
                                            </td>
                                            <td class="td_form2_td1" align="left">
                                                <div id="td_iNgay1">
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay1, "iNgay1", "","style=\"width:50px\"")%></div>
                                            </td>
                                            <td class="td_form2_td1" width="50px" style="padding-right: 10px;">
                                                <div>
                                                    Tháng</div>
                                            </td>
                                            <td class="td_form2_td1" align="left">
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang1, "iThang1", "", "style=\"width:50px\" onchange=\"ChonThang(this.value,'iNgay1')\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" width="80px" style="padding-right: 10px;">
                                                <div>
                                                    Đến Ngày</div>
                                            </td>
                                            <td class="td_form2_td1" align="left">
                                                <div id="td_iNgay2">
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay2, "iNgay2", "","style=\"width:50px\"")%></div>
                                            </td>
                                            <td class="td_form2_td1" width="50px" style="padding-right: 10px;">
                                                <div>
                                                    Tháng</div>
                                            </td>
                                            <td class="td_form2_td1" align="left">
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang2, "iThang2", "", "style=\"width:50px\" onchange=\"ChonThang(this.value,'iNgay2')\"")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="width: 100%">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%">
                                    </td>
                                    <td style="width: 5%; text-align: right" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td style="width: 2%">
                                    </td>
                                    <td style="width: 48%; text-align: left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <script type="text/javascript">
        $(function () {
            $("div#rptMain").hide();
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });
       
    </script>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        function TaiKhoan() {
            window.location.href = '<%=TK %>';
        } 
    </script>
    <iframe src="<%=UrlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
