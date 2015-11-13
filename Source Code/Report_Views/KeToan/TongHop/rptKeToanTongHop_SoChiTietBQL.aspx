<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="<%= Url.Content("~/Scripts/Report/jquery-1.8.0.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/Report/jquery-ui-1.8.23.custom.min.js") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/Report/ShowDialog.js") %>" type="text/javascript"></script>
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Report_Style/ReportCSS.css") %>" />
    <style type="text/css">
        fieldset
        {
            padding: 3px;
            border: 1px solid #dedede;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
        }
        fieldset legend
        {
            padding: 3px;
            font-size: 13px;
            font-family: Tahoma Arial;
        }
        fieldset p
        {
            padding: 2px;
        }
    </style>
</head>
<body>
    <% 
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "KeToan";

        String LoaiBieu = Convert.ToString(Request.QueryString["LoaiBieu"]);
        if (String.IsNullOrEmpty(LoaiBieu))
        {
            LoaiBieu = "rDonViNoiDung";
        }

        String MaTaiKhoan = Convert.ToString(ViewData["MaTaiKhoan"]);
        String TuThang = Convert.ToString(ViewData["TuThang"]);
        String DenThang = Convert.ToString(ViewData["DenThang"]);
        String Nam = Convert.ToString(ViewData["Nam"]);
        String PhongBan = Convert.ToString(ViewData["PhongBan"]);
        String DonVi = Convert.ToString(ViewData["DonVi"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iTrangThai"]);
        String iDisplay = Convert.ToString(ViewData["iDisplay"]);
        if (String.IsNullOrEmpty(TuThang))
        {
            TuThang = "1";
        }
        if (String.IsNullOrEmpty(DenThang))
        {
            DenThang = DanhMucModels.ThangLamViec(User.Identity.Name).ToString();
        }
        if (String.IsNullOrEmpty(Nam))
        {
            Nam = DanhMucModels.NamLamViec(User.Identity.Name).ToString();
        }
        //tháng     
        var dtThang = HamChung.getMonth(DateTime.Now, false, "", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        //Trang thai duyet
        DataTable dtTrangThai = rptQuyetToan_25_5Controller.tbTrangThai();
        // String iID_MaTrangThaiDuyet = Request.QueryString["iTrangThai"];
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
            iID_MaTrangThaiDuyet = "1";
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (dtTrangThai != null)
        {
            dtTrangThai.Dispose();
        }

        SelectOptionList optDonVi = null;
        SelectOptionList slTaiKhoan = new SelectOptionList(null, "iID_MaTaiKhoan", "sTenTaiKhoan"); ;
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        if (dtNam != null) dtNam.Dispose();

        //Phòng ban
        DataTable dtPhongBan = DanhMucModels.getPhongBanByCombobox(false, "--- Chọn phòng ban ---");
        if (dtPhongBan.Rows.Count > 0 && dtPhongBan != null)
        {
            if (String.IsNullOrEmpty(PhongBan))
            {
                PhongBan = HamChung.ConvertToString(dtPhongBan.Rows[0]["iID_MaPhongBan"]);
            }
            DataTable dtDonVi = PhongBan_DonViModels.getDonViByBQL(PhongBan, true, "--- Chọn đơn vị ---");
            optDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
            if (dtDonVi.Rows.Count > 0 && dtDonVi != null)
            {
                if (String.IsNullOrEmpty(DonVi))
                {
                    DonVi = HamChung.ConvertToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
                }
                var dtTaiKhoan = TaiKhoanModels.DSTaiKhoan_BQL_DVi(PhongBan, DonVi, iID_MaTrangThaiDuyet, TuThang, false,
                                                                   "--- Chọn đơn vị ---", User.Identity.Name);
                slTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "sTenTaiKhoan");
                if (dtTaiKhoan.Rows.Count > 0 && dtTaiKhoan != null)
                {
                    if (String.IsNullOrEmpty(MaTaiKhoan))
                    {
                        MaTaiKhoan = HamChung.ConvertToString(dtTaiKhoan.Rows[0]["iID_MaTaiKhoan"]);
                    }
                    // MaTaiKhoan = "111";
                }
                if (dtTaiKhoan != null) dtTaiKhoan.Dispose();
            }
            if (dtDonVi != null) dtDonVi.Dispose();
        }

        SelectOptionList optPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");
        if (dtPhongBan.Rows.Count > 0 && String.IsNullOrEmpty(PhongBan))
        {
            PhongBan = HamChung.ConvertToString(dtPhongBan.Rows[0]["iID_MaPhongBan"]);
        }
        if (dtPhongBan != null) dtPhongBan.Dispose();


        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        rptKTTH_SoChiTietBQLController ctlCTDV = new rptKTTH_SoChiTietBQLController();
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptKTTH_SoChiTietBQL", new { MaTaiKhoan = MaTaiKhoan, TuThang = TuThang, DenThang = DenThang, Nam = Nam, PhongBan = PhongBan, DonVi = DonVi, iTrangThai = iID_MaTrangThaiDuyet, iDisplay = iDisplay });
        using (Html.BeginForm("EditSubmit", "rptKTTH_SoChiTietBQL", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Chi tiết Tài khoản - BQL - Đơn vị</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="confirmBox" title="Chi tiết Tài khoản - BQL - Đơn vị" style="width: 800px;
            min-height: 200px; display: none;">
            <div id="idleft" style="width: 55%; padding: 2px; float: left; text-align: right;">
                <div>
                    <fieldset>
                        <legend><b>
                            <%=NgonNgu.LayXau("Chọn thời gian:") %></b></legend>
                        <table style="width: 100%;" border="0">
                            <tr>
                                <td>
                                    <b>
                                        <%=NgonNgu.LayXau("Từ:") %></b>&nbsp;
                                </td>
                                <td>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, TuThang, "TuThang", "", "class=\"input1_2\" ")%>
                                </td>
                                <td>
                                    <b>
                                        <%=NgonNgu.LayXau("Đến:") %></b>&nbsp;
                                </td>
                                <td>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, DenThang, "DenThang", "", "class=\"input1_2\" ")%>
                                </td>
                                <td>
                                    <b>
                                        <%=NgonNgu.LayXau("Năm:") %></b>&nbsp;
                                </td>
                                <td>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "Nam", "", "class=\"input1_2\" disabled=\"disabled\"  style=\"width: 80px;\"")%>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend><b>
                            <%=NgonNgu.LayXau("Điều kiện lọc:") %></b></legend>
                        <table style="width: 100%;" border="0">
                            <tr>
                                <td>
                                    <b>
                                        <%=NgonNgu.LayXau("BQL:") %></b> &nbsp;&nbsp;
                                </td>
                                <td>
                                    <%=MyHtmlHelper.DropDownList(ParentID, optPhongBan, PhongBan, "iID_MaPhongBan", null, "class=\"input1_2\" onchange=\"ChonBQL(this.value)\"")%>
                                </td>
                                <td>
                                    <b>
                                        <%=NgonNgu.LayXau("Trạng thái:")%></b>&nbsp;&nbsp;
                                </td>
                                <td>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", null, "class=\"input1_2\"")%>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend><b>
                            <%=NgonNgu.LayXau("Chọn đơn vị")%></b> </legend>
                        <table class="mGrid">
                            <tr>
                                <th align="center" style="width: 25px;">
                                    <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                </th>
                                <th align="left" style="font-size: 12px;">
                                    Tên đơn vị
                                </th>
                            </tr>
                        </table>
                        <div id="divDonVi" style="width: 100%; height: 310px; overflow: scroll;">
                            <%=ctlCTDV.get_sDanhSachDonVi(PhongBan,DonVi)%>
                        </div>
                    </fieldset>
                </div>
                <div style="padding-top: 10px; float: left; font-weight: bold;">
                    <%=MyHtmlHelper.CheckBox(ParentID, iDisplay, "iDisplay", "", "")%>
                    Gộp theo số ghi sổ
                </div>
            </div>
            <!--End #idright-->
            <div id="idright" style="width: 43%; padding: 2px; float: right; text-align: right;">
                <fieldset>
                    <legend><b>
                        <%=NgonNgu.LayXau("Danh sách tài khoản")%></b></legend>
                    <p>
                        <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, MaTaiKhoan, "MaTaiKhoan", "", "class=\"input1_2\" size='29'")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_MaTaiKhoan")%>
                    </p>
                </fieldset>
            </div>
            <!--End #idleft-->
            <p style="text-align: center; padding: 4px; clear: both;">
                <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>"
                    style="display: inline-block; margin-right: 5px;" /><input class="button" type="button"
                        value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" style="display: inline-block;
                        margin-left: 5px;" /></p>
        </div>
        <!--End #confirmBox-->
    </div>
    <%} %>
    <script type="text/javascript">
        $(function () {
            $('div.login1 a').click(function () {
                ShowDialog(710);
            });
            $('*').keyup(function (e) {
                if (e.keyCode == '27') {
                    Hide();
                }
            });
            var Check = "<%= PageLoad  %>";
            if (Check == 0) {
                ShowDialog(710);
            }
            else {
                Hide();   
            }
            // ShowDialog(610);
        });
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        function ChonBQL() {
            var iID_MaPhongBan = document.getElementById("<%= ParentID %>_iID_MaPhongBan").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ObjDanhSachDonVi?Id=#0&iID_MaDonVi=#1", "rptKTTH_SoChiTietBQL")%>');
            url = unescape(url.replace("#0", iID_MaPhongBan));
            url = unescape(url.replace("#1", -1));
            $.getJSON(url, function (data) {
                document.getElementById("divDonVi").innerHTML = data;
               // document.getElementById("<%= ParentID %>_iID_MaDonVi").value = <%= DonVi %>;
              ChonDonVi(<%= DonVi %>);
            });
        }
        function ChonDonVi() {
            var DonVi = "";
            $("input:checkbox[check-group='DonVi']").each(function (i) {
                 if (this.checked) {
                     if (DonVi != "") DonVi += ",";
                     DonVi += this.value;
                 }
             });
            jQuery.ajaxSetup({ cache: false });
            var MaPhongBan = document.getElementById("<%= ParentID %>_iID_MaPhongBan").value;
            var MaTrangThai = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
            var iThang = document.getElementById("<%= ParentID %>_DenThang").value;
            var url = unescape('<%= Url.Action("getTaiKhoan?MaPhongBan=#0&MaDonVi=#1&MaTrangThai=#2&iThang=#3", "rptKTTH_SoChiTietBQL")%>');
            url = unescape(url.replace("#0", MaPhongBan));
            url = unescape(url.replace("#1", DonVi));
            url = unescape(url.replace("#2", MaTrangThai));
            url = unescape(url.replace("#3", iThang));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_MaTaiKhoan").innerHTML = data;
            });
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTH_SoChiTietBQL", new { MaTaiKhoan = MaTaiKhoan, TuThang = TuThang, DenThang = DenThang, Nam = Nam, PhongBan = PhongBan, DonVi = DonVi, iTrangThai = iID_MaTrangThaiDuyet, iDisplay = iDisplay }), "Export To Excel")%>
    <iframe src="<%=URLView %>" height="600px" width="100%"></iframe>
</body>
</html>
