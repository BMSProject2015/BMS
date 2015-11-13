<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
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
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        String iThang = Convert.ToString(ViewData["iThang"]);
        if (String.IsNullOrEmpty(iThang))
            iThang = DateTime.Now.Month.ToString();
        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtDonVi = QuyetToanModels.getDSPhongBan_QuanSo(iNamLamViec, MaND);

        SelectOptionList slPhongBan = new SelectOptionList(dtDonVi, "iID_MaPhongBan", "sTenPhongBan");
        if (String.IsNullOrEmpty(iID_MaPhongBan))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                iID_MaPhongBan = Convert.ToString(dtDonVi.Rows[0]["iID_MaPhongBan"]);
            }
            else
            {
                iID_MaPhongBan = "-1";
            }
        }
        dtDonVi.Dispose();
        String[] arrView = new String[1];
        String Chuoi = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        if (String.IsNullOrEmpty(iID_MaPhongBan)) PageLoad = "0";
        if (PageLoad == "1")
        {
            arrView[0] =
                String.Format(
                    @"/rptQTQS_THQS_TongHop/viewpdf?iID_MaPhongBan={0}&MaND={1}&iThang={2}",
                    iID_MaPhongBan, MaND,iThang);
            Chuoi += arrView[0];
        }
        String URL = Url.Action("Index", "QuyetToan_QuanSo_Report");
        
        using (Html.BeginForm("EditSubmit", "rptQTQS_THQS_TongHop", new { ParentID = ParentID }))
        {
    %>
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Báo cáo tổng hợp tình hình thực hiện quân số theo đơn vị năm
                        <%=iNamLamViec %>
                    </span>
                </td>
            </tr>
        </table>
    </div>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%" class="td_form2_td1">
            </td>
            <td class="td_form2_td1" style="width: 20%">
                <div>
                    Chọn phòng ban:</div>
            </td>
            <td style="width: 20%">
                <div>
                    <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChoniThang()\"")%>
                </div>
            </td>
              <td class="td_form2_td1" style="width: 10%">
                <div>
                    Chọn tháng:</div>
            </td>
            <td style="width: 20%">
                <div>
                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 50%\" onchange=\"ChoniThang()\"")%>
                </div>
            </td>
            <td class="td_form2_td1">
            </td>
        </tr>
        <tr>
            <td colspan="6">
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
              function CheckAllDV(value) {
                  $("input:checkbox[check-group='DonVi']").each(function (i) {
                      this.checked = value;
                  });
              }
              function Huy() {
               window.location.href = '<%=URL %>';
           }
        </script>
    </table>
    <%} %>
</body>
</html>
