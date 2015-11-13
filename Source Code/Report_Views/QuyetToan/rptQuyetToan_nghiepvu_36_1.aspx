<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%--<head id="Head1" runat="server">
    <title></title>
</head>--%>
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%  
            
        String ParentID = "QuyetToan";
        String MaND = User.Identity.Name;
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtPhongBan = DanhMucModels.NS_PhongBan();
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSachNghiepVu(false);
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
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
        dtLNS.Dispose();
        String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
        if (String.IsNullOrEmpty(LoaiThang_Quy))
        {
            LoaiThang_Quy = "0";
        }


        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String Thang = "0";
        String Quy = "0";
        if (LoaiThang_Quy == "0")
        {
            Thang = Thang_Quy;
            Quy = "0";
        }
        else
        {
            Thang = "0";
            Quy = Thang_Quy;
        }
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_NghiepVu_40_2Controller.tbTrangThai();
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
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        DataTable dtDonVi = rptQuyetToan_nghiepvu_36_1Controller.LayDSDonVi(iID_MaPhongBan, iID_MaTrangThaiDuyet, sLNS, Thang_Quy, LoaiThang_Quy, TruongTien, MaND);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            //iID_MaDonVi=Request.QueryString["iID_MaDonVi"];
            if (dtDonVi.Rows.Count > 1)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[1]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        dtDonVi.Dispose();

        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 1 });
        String urlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_nghiepvu_36_1", new {iID_MaPhongBan = iID_MaPhongBan, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, LoaiThang_Quy = LoaiThang_Quy, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi, sLNS = sLNS, TruongTien = TruongTien });
        }
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_nghiepvu_36_1", new { ParentID = ParentID }))
        {       
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width: 45%">
                        <span>Báo cáo nhập dữ liệu</span>
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
            <div id="rptMain" style="margin: 2px auto; padding: 2px 2px; overflow: visible; text-align: center">
                <ul class="inlineBlock">
                    <li>
                        <fieldset style="width: 120px; height: 60px; border-color: Olive;">
                            <legend>
                                <%=NgonNgu.LayXau("Phòng ban:")%></legend>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 95%\" onchange=\"Chon()\" disabled=\"disabled\"")%>
                            </div>
                        </fieldset>
                    </li>
                    <li>
                        <fieldset style="width: 200px; height: 60px; border-color: Olive;">
                            <legend>
                                <%=NgonNgu.LayXau("Loại ngân sách ")%></legend>
                             <div id="<%=ParentID %>_divLNS">

                              
                            </div>
                        </fieldset>
                    </li>
                    <li>
                        <fieldset style="width: 250px; height: 60px; border-color: Olive;">
                            <legend>
                                <%=NgonNgu.LayXau("Chọn tháng&nbsp;&nbsp;quý")%></legend>
                            <p>
                                <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:10%;\" onchange=\"ChonLNS()\"")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "", "class=\"input1_2\" style=\"width:25%;\" onchange=\"ChonLNS()\"")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:10%;\" onchange=\"ChonLNS()\"")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "class=\"input1_2\" style=\"width:25%;\" onchange=\"ChonLNS()\"")%>
                            </p>
                        </fieldset>
                    </li>
                       <li>
                        <fieldset style="width: 150px; height: 60px; border-color: Olive;">
                            <legend>
                                <%=NgonNgu.LayXau("Trạng thái duyệt &nbsp;")%></legend>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "style=\"width:95%\" onchange=\"ChonLNS()\"")%></div>
                            <div>
                            </div>
                        </fieldset>
                    </li>
                    <li>
                        <fieldset style="width: 150px; height: 60px; border-color: Olive;">
                            <legend>
                                <%=NgonNgu.LayXau("Đơn vị")%></legend>
                            <div id="<%=ParentID %>_divDonVi">
                                <%--   <%rptQuyetToan_nghiepvu_36_1Controller rptTB1 = new rptQuyetToan_nghiepvu_36_1Controller();%>
                                <% Object item=  rptTB1.obj_DSDonVi(ParentID, iID_MaTrangThaiDuyet, sLNS, Thang_Quy, LoaiThang_Quy, iID_MaDonVi, TruongTien, MaND);%>
                                <%=item %>--%>
                            </div>
                        </fieldset>
                    </li>
                 
                    <li>
                        <fieldset style="width: 240px; height: 60px; text-align: left; border-color: Olive;">
                            <legend>
                                <%=NgonNgu.LayXau("Kết quả in cho:") %></legend>
                            <div style="padding-bottom: 5px;">
                                <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "", "onchange=\"ChonLNS()\"")%><span
                                    style="font-size: 12px; font-weight: bold; padding-right: 10px;">1. Tự chi</span>
                                <%=MyHtmlHelper.Option(ParentID, "sTNG", TruongTien, "LoaiIn", "", "onchange=\"ChonLNS()\"")%><span
                                    style="font-size: 12px; font-weight: bold;">In đến tiểu ngành (TNG)</span>
                            </div>
                            <div>
                                <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "onchange=\"ChonLNS()\"")%><span
                                    style="font-size: 12px; font-weight: bold;">2. Hiện vât</span>
                                <%=MyHtmlHelper.Option(ParentID, "sTNG", TruongTien, "LoaiIn", "", "onchange=\"ChonLNS()\"")%><span
                                    style="font-size: 12px; font-weight: bold;">In đến ngành (NG)</span>
                            </div>
                        </fieldset>
                    </li>
                </ul>
                <ul class="inlineBlock">
                    <li>
                        <fieldset style="width: 200px; text-align: center; height: 50px; border-color: Olive;">
                            <legend>
                                <%=NgonNgu.LayXau("Số QT trong tháng ")%></legend>
                            <div style="float: left;">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            Số QT:
                                        </td>
                                        <td id="lblSoQT">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tổng số:
                                        </td>
                                        <td id="lblTongSo">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </li>
                </ul>
                <%--<%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 40%; display:none;\"")%>--%>
                <p style="text-align: center; padding: 4px;">
                    <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>"
                        style="display: inline-block; margin-right: 5px;" /><input class="button" type="button"
                            value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" style="display: inline-block;
                            margin-left: 5px;" /></p>
            </div>
            <!--End #rptMain-->
        </div>
    </div>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_nghiepvu_36_1", new { iID_MaPhongBan = iID_MaPhongBan,iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, LoaiThang_Quy = LoaiThang_Quy, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi, sLNS = sLNS, TruongTien = TruongTien }), "Xuất ra Excels")%>
    <script type="text/javascript">

        $(document).ready(function () {

            $('.title_tong a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        
    </script>
    <script type="text/javascript">
        Chon();
        function Chon() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
            var iID_MaPhongBan = document.getElementById("<%= ParentID %>_iID_MaPhongBan").value;
         
            var TenLoaiThang_Quy = document.getElementsByName("<%=ParentID %>_LoaiThang_Quy");
            var LoaiThang_Quy;
            var Thang_Quy;
            var i = 0;
            for (i = 0; i < TenLoaiThang_Quy.length; i++) {
                if (TenLoaiThang_Quy[i].checked) {
                    LoaiThang_Quy = TenLoaiThang_Quy[i].value;
                }
            }
            if (LoaiThang_Quy == 0) {
                Thang_Quy = document.getElementById("<%=ParentID %>_iThang").value;
            }
            else {
                Thang_Quy = document.getElementById("<%=ParentID %>_iQuy").value;
            }
            var bTruongTien = document.getElementById("<%= ParentID %>_TruongTien").checked;
            var TruongTien = "";
            if (bTruongTien) TruongTien = "rTuChi";
            else TruongTien = "rHienVat";
            var objDonVi = document.getElementById("<%= ParentID %>_iID_MaDonVi");
            var objLNS = document.getElementById("<%= ParentID %>_sLNS");
            var iID_MaDonVi = '-1111';
            var sLNS = "-1";
        
                sLNS = '<%=sLNS %>';
            
            if (objDonVi != null) {
                iID_MaDonVi = objDonVi.options[objDonVi.selectedIndex].value;
            }
            else {
                iID_MaDonVi = '<%=iID_MaDonVi %>';
            }

            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&iID_MaPhongBan=#7&sLNS=#1&iID_MaTrangThaiDuyet=#2&Thang_Quy=#3&LoaiThang_Quy=#4&iID_MaDonVi=#5&TruongTien=#6", "rptQuyetToan_nghiepvu_36_1") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", sLNS));
            url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#3", Thang_Quy));
            url = unescape(url.replace("#4", LoaiThang_Quy));
            url = unescape(url.replace("#5", iID_MaDonVi));
            url = unescape(url.replace("#6", TruongTien));
            url = unescape(url.replace("#7", iID_MaPhongBan));
            $.getJSON(url, function (item) {
                document.getElementById("<%= ParentID %>_divLNS").innerHTML = item.dsLNS;
                
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = item.dsDonVi;
                document.getElementById("lblSoQT").innerHTML = item.sSoQT;
                document.getElementById("lblTongSo").innerHTML = item.sTongQT;
            });
        }                                            
    </script>
     <script type="text/javascript">
         function ChonLNS() {
             var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
             var iID_MaPhongBan = document.getElementById("<%= ParentID %>_iID_MaPhongBan").value;

             var TenLoaiThang_Quy = document.getElementsByName("<%=ParentID %>_LoaiThang_Quy");
             var LoaiThang_Quy;
             var Thang_Quy;
             var i = 0;
             for (i = 0; i < TenLoaiThang_Quy.length; i++) {
                 if (TenLoaiThang_Quy[i].checked) {
                     LoaiThang_Quy = TenLoaiThang_Quy[i].value;
                 }
             }
             if (LoaiThang_Quy == 0) {
                 Thang_Quy = document.getElementById("<%=ParentID %>_iThang").value;
             }
             else {
                 Thang_Quy = document.getElementById("<%=ParentID %>_iQuy").value;
             }
             var bTruongTien = document.getElementById("<%= ParentID %>_TruongTien").checked;
             var TruongTien = "";
             if (bTruongTien) TruongTien = "rTuChi";
             else TruongTien = "rHienVat";
             var objDonVi = document.getElementById("<%= ParentID %>_iID_MaDonVi");
             var objLNS = document.getElementById("<%= ParentID %>_sLNS");
             var iID_MaDonVi = '-1111';
             var sLNS = "-1";

             sLNS = document.getElementById("<%= ParentID %>_sLNS").value;

             if (objDonVi != null) {
                 iID_MaDonVi = objDonVi.options[objDonVi.selectedIndex].value;
             }
             else {
                 iID_MaDonVi = '<%=iID_MaDonVi %>';
             }

             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&iID_MaPhongBan=#7&sLNS=#1&iID_MaTrangThaiDuyet=#2&Thang_Quy=#3&LoaiThang_Quy=#4&iID_MaDonVi=#5&TruongTien=#6", "rptQuyetToan_nghiepvu_36_1") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", sLNS));
             url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#3", Thang_Quy));
             url = unescape(url.replace("#4", LoaiThang_Quy));
             url = unescape(url.replace("#5", iID_MaDonVi));
             url = unescape(url.replace("#6", TruongTien));
             url = unescape(url.replace("#7", iID_MaPhongBan));
             $.getJSON(url, function (item) {

                 document.getElementById("<%= ParentID %>_divDonVi").innerHTML = item.dsDonVi;
                 document.getElementById("lblSoQT").innerHTML = item.sSoQT;
                 document.getElementById("lblTongSo").innerHTML = item.sTongQT;
             });
         }                                            
    </script>
     <script type="text/javascript">
         function ChonDV() {
             var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
             var iID_MaPhongBan = document.getElementById("<%= ParentID %>_iID_MaPhongBan").value;

             var TenLoaiThang_Quy = document.getElementsByName("<%=ParentID %>_LoaiThang_Quy");
             var LoaiThang_Quy;
             var Thang_Quy;
             var i = 0;
             for (i = 0; i < TenLoaiThang_Quy.length; i++) {
                 if (TenLoaiThang_Quy[i].checked) {
                     LoaiThang_Quy = TenLoaiThang_Quy[i].value;
                 }
             }
             if (LoaiThang_Quy == 0) {
                 Thang_Quy = document.getElementById("<%=ParentID %>_iThang").value;
             }
             else {
                 Thang_Quy = document.getElementById("<%=ParentID %>_iQuy").value;
             }
             var bTruongTien = document.getElementById("<%= ParentID %>_TruongTien").checked;
             var TruongTien = "";
             if (bTruongTien) TruongTien = "rTuChi";
             else TruongTien = "rHienVat";
             var objDonVi = document.getElementById("<%= ParentID %>_iID_MaDonVi").value;
             var objLNS = document.getElementById("<%= ParentID %>_sLNS");
             var iID_MaDonVi = '-1111';
             var sLNS = "-1";

             sLNS = document.getElementById("<%= ParentID %>_sLNS").value;

             iID_MaDonVi = objDonVi;

             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&iID_MaPhongBan=#7&sLNS=#1&iID_MaTrangThaiDuyet=#2&Thang_Quy=#3&LoaiThang_Quy=#4&iID_MaDonVi=#5&TruongTien=#6", "rptQuyetToan_nghiepvu_36_1") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", sLNS));
             url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#3", Thang_Quy));
             url = unescape(url.replace("#4", LoaiThang_Quy));
             url = unescape(url.replace("#5", iID_MaDonVi));
             url = unescape(url.replace("#6", TruongTien));
             url = unescape(url.replace("#7", iID_MaPhongBan));
             $.getJSON(url, function (item) {

                 document.getElementById("lblSoQT").innerHTML = item.sSoQT;
                 document.getElementById("lblTongSo").innerHTML = item.sTongQT;
             });
         }                                            
    </script>
    <%} %>
    <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
