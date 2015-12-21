<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="VIETTEL.Models.DuToan" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body>
    <%
        String ParentID = "DuToan";
        int SoCot = 1;
        //dt Loại ngân sách
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = DuToan_ReportModels.dtDonVi(MaND, "1,2,3,4");
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");

        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        dtDonVi.Dispose();
        String BaoCao = Convert.ToString(ViewData["MaTo"]);
        String[] arrBaoCao = BaoCao.Split(',');
        String[] arrView = new String[arrBaoCao.Length];
        String Chuoi = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        if (String.IsNullOrEmpty(iID_MaDonVi)) PageLoad = "0";
        if (PageLoad == "1")
        {

            for (int i = 0; i < arrView.Length; i++)
            {
                if (arrBaoCao[i].StartsWith("/rptDuToan_1040100_TungNganh"))
                {
                    arrView[i] = arrBaoCao[i];
                }
                else
                {
                     arrView[i] =
                    String.Format(
                        @"/{2}/viewpdf?iID_MaDonVi={0}&MaND={1}",
                        iID_MaDonVi, MaND, arrBaoCao[i]);
                }
               
                Chuoi += arrView[i];
                if (i < arrView.Length - 1)
                    Chuoi += "+";
            }

        }
        else
        {
            Chuoi = "";
        }
        String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "1" });
        using (Html.BeginForm("EditSubmit", "rptDuToan_DonVi", new { ParentID = ParentID }))
        {
    %>
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Báo cáo in đơn vị </span>
                </td>
            </tr>
        </table>
    </div>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%" class="td_form2_td1">
            </td>
            <td class="td_form2_td1" style="width: 10%">
                <div>
                    Chọn đơn vị:</div>
            </td>
            <td style="width: 10%" class="td_form2_td1">
                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\" onchange=\"Chon()\"")%>
            </td>
            <td class="td_form2_td1" style="width: 10%">
                <div>
                    Chọn báo cáo:</div>
            </td>
                        <td style="width: 30%" rowspan="3">
                            <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 200px">
                            </div>
                        </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <div style="margin-top: 10px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 40%">
                            </td>
                            <td align="right">
                                <input type="submit" class="button" value="Tiếp tục" />
                            </td>
                            <td style="width: 1%">
                                &nbsp;
                            </td>
                            <td align="left">
                                <input type="button" class="button" value="Hủy" onclick="Huy()" />
                            </td>
                            <td style="width: 40%">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <script type="text/javascript">
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
              function CheckAllTO(value) {
                  $("input:checkbox[check-group='To']").each(function (i) {
                      this.checked = value;
                  });
              }
               function Huy() {
                 window.location.href = '<%=BackURL%>';
             }
             Chon();
            function Chon() {
                 var iID_MaDonVi = document.getElementById("<%=ParentID %>_iID_MaDonVi").value;
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaDonVi=#1&BaoCao=#2&ToSo=#3", "rptDuToan_DonVi") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", iID_MaDonVi));
                url = unescape(url.replace("#2","<%= BaoCao %>"));
                 url = unescape(url.replace("#3","<%= iID_MaDonVi %>"));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }
        </script>
    </table>
    <%} %>
</body>
</html>
