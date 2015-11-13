<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
        String ParentID = "QuyetToan";
        String MaND = User.Identity.Name;
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String iThang_Quy = Convert.ToString(ViewData["iThang_Quy"]);
        String iID_MaNamNganSach = Convert.ToString(ViewData["iID_MaNamNganSach"]);
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptQuyetToan_BieuKiemAll", new { viID_MaPhongBan = iID_MaPhongBan, viThang_Quy = iThang_Quy, viID_MaDonVi = iID_MaDonVi, viID_MaNamNganSach = iID_MaNamNganSach });
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();

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

        String BackURL = Url.Action("Index", "Home");
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_BieuKiemAll", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo các khoản thu nộp ngân sách</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2" class="table_form2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 30%">
                            <div>
                                Chọn phòng ban:</div>
                        </td>
                        <td style="width: 20%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <div>
                                Chọn đơn vị:</div>
                        </td>
                        <td style="width: 20%">
                            <div id="<%= ParentID %>_tdDonVi">
                                <%rptQuyetToan_BieuKiemAllController rpt = new rptQuyetToan_BieuKiemAllController();%>
                                <%=rpt.obj_DonVi(ParentID, iID_MaPhongBan,iThang_Quy,iID_MaNamNganSach,iID_MaDonVi,MaND)%>
                            </div>
                        </td>
                       
                    </tr>
                    <tr>
                     <td class="td_form2_td1" >
                            <div>
                                Chọn quý:</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iThang_Quy, "iThang_Quy", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1">
                            <div>
                                Năm ngân sách:</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNamNganSach, iID_MaNamNganSach, "iID_MaNamNganSach", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td style="width: 2%;" align="left"></td>
                                        <td style="width: 49%;" align="left">
                                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="Huy()" />
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
    <div>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_BieuKiemAll", new { viID_MaPhongBan = iID_MaPhongBan, viThang_Quy = iThang_Quy, viID_MaDonVi = iID_MaDonVi, viID_MaNamNganSach = iID_MaNamNganSach }), "Xuất ra excel")%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
<script type="text/javascript">
    function Chon() {
        jQuery.ajaxSetup({ cache: false });
        var iID_MaPhongBan = document.getElementById("<%= ParentID %>_iID_MaPhongBan").value;
        var iThang_Quy = document.getElementById("<%= ParentID %>_iThang_Quy").value;
        var iID_MaNamNganSach = document.getElementById("<%= ParentID %>_iID_MaNamNganSach").value;
        var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&oiID_MaPhongBan=#1&oiThang_Quy=#2&oiID_MaNamNganSach=#3&oiID_MaDonVi=#4&oMaND=#5","rptQuyetToan_BieuKiemAll") %>');
        url = unescape(url.replace("#0", "<%= ParentID %>"));
        url = unescape(url.replace("#1", iID_MaPhongBan));
        url = unescape(url.replace("#2", iThang_Quy));
        url = unescape(url.replace("#3", iID_MaNamNganSach));
        url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
        url = unescape(url.replace("#5", "<%= MaND %>"));
        $.getJSON(url, function (data) {
            document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
        });
    }  
</script>
</html>
