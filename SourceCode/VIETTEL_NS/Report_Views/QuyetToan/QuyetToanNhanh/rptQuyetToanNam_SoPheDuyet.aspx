<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
        String ParentID = "QuyetToanNhanh";
        String MaND = User.Identity.Name;
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String iLoai = Convert.ToString(ViewData["iLoai"]);
        String iID_MaNamNganSach = Convert.ToString(ViewData["iID_MaNamNganSach"]);
        String iID_MaNguonNganSach = Convert.ToString(ViewData["iID_MaNguonNganSach"]);
        String iID_MaMauBaoCao = Convert.ToString(ViewData["iID_MaMauBaoCao"]);
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptQuyetToanNam_SoPheDuyet", new { MaND = MaND, iID_MaPhongBan = iID_MaPhongBan, iID_MaDonVi = iID_MaDonVi, iID_MaNamNganSach = iID_MaNamNganSach, iLoai = iLoai, iID_MaNguonNganSach = iID_MaNguonNganSach, iID_MaMauBaoCao = iID_MaMauBaoCao });
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();

        DataTable dtNamNganSach = QuyetToanModels.getDSNamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "MaLoai", "sTen");
        dtNamNganSach.Dispose();

        DataTable dtTongHop = QuyetToanModels.getDSTongHop_SoPheDuyet();
        SelectOptionList slTongHop = new SelectOptionList(dtTongHop, "MaLoai", "sTen");
        dtTongHop.Dispose();

        DataTable dtMauBaoCao = QuyetToanModels.getDSMauBaoCao();
        SelectOptionList slMauBaoCao = new SelectOptionList(dtMauBaoCao, "MaLoai", "sTen");
        dtMauBaoCao.Dispose();

        DataTable dtNguonNganSach = QuyetToanModels.getDSNguonNganSach();
        SelectOptionList slNguonNganSach = new SelectOptionList(dtNguonNganSach, "MaLoai", "sTen");
        dtMauBaoCao.Dispose();

        String BackURL = Url.Action("Index", "Home");
        using (Html.BeginForm("EditSubmit", "rptQuyetToanNam_SoPheDuyet", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo số phê duyệt quyết toán ngân sách năm
                            <%=iNamLamViec %></span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2" class="table_form2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 25%">
                            <div>
                                Chọn phòng ban:</div>
                        </td>
                        <td style="width: 15%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <div>
                                Năm ngân sách:</div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNamNganSach, iID_MaNamNganSach, "iID_MaNamNganSach", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <div>
                                Tổng hợp:</div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTongHop, iLoai, "iLoai", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%">
                            <div>
                                Chọn đơn vị:</div>
                        </td>
                        <td style="width: 10%">
                            <div id="<%= ParentID %>_tdDonVi">
                                <% rptQuyetToanNam_SoPheDuyetController rpt = new rptQuyetToanNam_SoPheDuyetController();%>
                                <%=rpt.obj_DonVi(ParentID, iID_MaPhongBan, iID_MaDonVi, MaND)%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <div>
                                Nguồn ngân sách:</div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNguonNganSach, iID_MaNguonNganSach, "iID_MaNguonNganSach", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                         <td class="td_form2_td1" style="width: 10%">
                            <div>
                                Mẫu báo cáo:</div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slMauBaoCao, iID_MaMauBaoCao, "iID_MaMauBaoCao", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td style="width: 2%;" align="left">
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
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToanNam_SoPheDuyet", new { MaND = MaND, iID_MaPhongBan = iID_MaPhongBan, iID_MaDonVi = iID_MaDonVi, iID_MaNamNganSach = iID_MaNamNganSach, iLoai = iLoai, iID_MaNguonNganSach = iID_MaNguonNganSach, iID_MaMauBaoCao = iID_MaMauBaoCao }), "Xuất ra excel")%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
<script type="text/javascript">
    function Chon() {
        jQuery.ajaxSetup({ cache: false });
        var iID_MaPhongBan = document.getElementById("<%= ParentID %>_iID_MaPhongBan").value;


        var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaPhongBan=#1&iID_MaDonVi=#2&MaND=#3","rptQuyetToanNam_SoPheDuyet") %>');
        url = unescape(url.replace("#0", "<%= ParentID %>"));
        url = unescape(url.replace("#1", iID_MaPhongBan));
        url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
        url = unescape(url.replace("#3", "<%= MaND %>"));
        $.getJSON(url, function (data) {
            document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
        });
    }  
</script>
</html>
