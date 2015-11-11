<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Content/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%        
        
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        String iID_MaNamNganSach = Convert.ToString(ViewData["iID_MaNamNganSach"]);
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String iThang_Quy = Convert.ToString(ViewData["iThang_Quy"]);
        //dt Trạng thái duyệt
        DataTable dtTrangThai = rptQuyetToan_nghiepvu_45_9Controller.tbTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            if (dtTrangThai.Rows.Count > 1)
            {
                iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[1]["iID_MaTrangThaiDuyet"]);
            }
            else
            {
                iID_MaTrangThaiDuyet = Guid.Empty.ToString();
            }
        }
        dtTrangThai.Dispose();
        DataTable dtNamNganSach = QuyetToanModels.getDSNamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "MaLoai", "sTen");
        dtNamNganSach.Dispose();
        if (String.IsNullOrEmpty(iID_MaNamNganSach))
            iID_MaNamNganSach = "2";
        if (String.IsNullOrEmpty(iThang_Quy))
        {
            String mon = DateTime.Now.Month.ToString();
            if (mon == "1" || mon == "2" || mon == "3")
                iThang_Quy = "1";
            else if (mon == "4" || mon == "5" || mon == "6")
                iThang_Quy = "2";
            else if (mon == "7" || mon == "8" || mon == "9")
                iThang_Quy = "3";
            else
                iThang_Quy = "4";
        }
        DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
        DataRow R1 = dtQuy.NewRow();
        R1["MaQuy"] = "5";
        R1["TenQuy"] = "Bổ sung";
        dtQuy.Rows.Add(R1);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();
        
        String MaPhongBanND = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSach_PhongBan(MaPhongBanND);
        String sLNS = Convert.ToString(ViewData["sLNS"]);
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
        String[] arrLNS = sLNS.Split(',');

        dtLNS.Dispose();
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String[] arrMaDonVi = iID_MaDonVi.Split(',');
        String[] arrView = new String[arrMaDonVi.Length];
        String Chuoi = "";
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        if (String.IsNullOrEmpty(sLNS)) PageLoad = "0";
        if (PageLoad == "1")
        {

            for (int i = 0; i < arrMaDonVi.Length; i++)
            {
                arrView[i] =
                    String.Format(
                        @"/rptQuyetToan_ThongTri/viewpdf?iID_MaDonVi={0}&sLNS={1}&iThang_Quy={2}&iID_MaNamNganSach={3}&MaND={4}",
                        arrMaDonVi[i], sLNS, iThang_Quy, iID_MaNamNganSach, MaND);
                Chuoi += arrView[i];
                if (i < arrMaDonVi.Length - 1)
                    Chuoi += "+";
            }

        }
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = "1" });
            //UrlReport = Url.Action("ViewPDF", "rptQuyetToan_nghiepvu_45_9", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, iThang_Quy = iThang_Quy, iID_MaDonVi = MaDonVi });
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_nghiepvu_45_9", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo so sánh chỉ tiêu quyết toán</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="1%">
                            &nbsp;
                        </td>
                        <td width="5%" class="td_form2_td1">
                            <div>
                                LNS:</div>
                        </td>
                        <td style="width: 30%;" rowspan="20">
                            <div style="width: 99%; height: 500px; overflow: scroll; border: 1px solid black;">
                                <table class="mGrid">
                                    <tr>
                                        <td>
                                            <input type="checkbox" id="checkAll" onclick="Chonall(this.checked)">
                                        </td>
                                        <td>
                                            Chọn tất cả LNS
                                        </td>
                                    </tr>
                                    <%
String TenLNS = ""; String sLNS1 = "";
String _Checked = "checked=\"checked\"";
for (int i = 0; i < dtLNS.Rows.Count; i++)
{
    _Checked = "";
    TenLNS = Convert.ToString(dtLNS.Rows[i]["TenHT"]);
    sLNS1 = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
    for (int j = 0; j < arrLNS.Length; j++)
    {
        if (sLNS1 == arrLNS[j])
        {
            _Checked = "checked=\"checked\"";
            break;
        }
    }    
                                    %>
                                    <tr>
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="Checkbox1"
                                                onchange="Chon()" name="sLNS" />
                                        </td>
                                        <td>
                                            <%=TenLNS%>
                                        </td>
                                    </tr>
                                    <%}%>
                                </table>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                Trạng Thái :
                            </div>
                        </td>
                        <td style="width: 20%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 50%\" onchange=\" Chon()\"")%>
                            </div>
                        </td>
                        <td rowspan="20" width="80px" align="right" valign="top">
                            <b>Đơn vị :</b>
                        </td>
                        <td rowspan="20" style="width: 30%; vertical-align: top;">
                            <div id="<%= ParentID %>_divDonVi" style="height: 500px; overflow: scroll; vertical-align: top;">
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td class="td_form2_td1">
                            <div>
                                <%=NgonNgu.LayXau("Quý: ")%></div>
                        </td>
                        <td>
                            <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iThang_Quy, "iThang_Quy", "", "class=\"input1_2\" style=\"width:50%;\" onchange=\" Chon()\"")%>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                     <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td class="td_form2_td1">
                            <div>
                                <%=NgonNgu.LayXau("Phòng ban: ")%></div>
                        </td>
                        <td>
                            <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:50%;\" onchange=\" Chon()\"")%>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td class="td_form2_td1">
                            <div>
                                <%=NgonNgu.LayXau("Năm ngân sách: ")%></div>
                        </td>
                        <td>
                            <%=MyHtmlHelper.DropDownList(ParentID, slNamNganSach, iID_MaNamNganSach, "iID_MaNamNganSach", "", "class=\"input1_2\" style=\"width:50%;\" onchange=\" Chon()\"")%>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td_form2_td1">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="6">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //         $("div#rptMain").hide();
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
             var count = <%=arrView.Length%>;
                var Chuoi = '<%=Chuoi%>';
                var Mang=Chuoi.split("+");
                   var pageLoad = <%=PageLoad %>;
                   if(pageLoad=="1") {
                var siteArray = new Array(count);
                for (var i = 0; i < count; i++) {
                    siteArray[i] = Mang[i];
                }
                    for (var i = 0; i < count; i++) {
                        window.open(siteArray[i], '_blank');
                    }
                } 
        });       
    </script>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
    </div>
    <script type="text/javascript">
        function CheckAll(value) {
            $("input:checkbox[check-group='DonVi']").each(function (i) {
                this.checked = value;
            });
        }                                            
    </script>
    <script type="text/javascript">
        function Chonall(sLNS) {
            $("input:checkbox[check-group='sLNS']").each(function (i) {
                if (sLNS) {
                    this.checked = true;
                }
                else {
                    this.checked = false;
                }

            });
            Chon();
        }                                            
    </script>
    <script type="text/javascript">
        Chon();
        function Chon() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var iThang_Quy = document.getElementById("<%=ParentID %>_iThang_Quy").value;
            var iID_MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;
            var iID_MaNamNganSach = document.getElementById("<%=ParentID %>_iID_MaNamNganSach").value;
            var sLNS = "";
            $("input:checkbox[check-group='sLNS']").each(function (i) {
                if (this.checked) {
                    if (sLNS != "") sLNS += ",";
                    sLNS += this.value;
                }
            });
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&jiID_MaTrangThaiDuyet=#1&jiThang_Quy=#2&jiID_MaPhongBan=#3&jiID_MaDonVi=#4&jsLNS=#5&jiID_MaNamNganSach=#6", "rptQuyetToan_nghiepvu_45_9") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", iThang_Quy));
            url = unescape(url.replace("#3", iID_MaPhongBan));
            url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#5", sLNS));
            url = unescape(url.replace("#6", iID_MaNamNganSach));

            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
            });
        }                                            
    </script>
    <%} %>
</body>
</html>
