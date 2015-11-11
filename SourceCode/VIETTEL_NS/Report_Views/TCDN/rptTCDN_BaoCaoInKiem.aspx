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
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        String iQuy = Convert.ToString(ViewData["iQuy"]);
        if (String.IsNullOrEmpty(iQuy))
        {
            String mon = DateTime.Now.Month.ToString();
            if (mon == "1" || mon == "2" || mon == "3")
                iQuy = "1";
            else if (mon == "4" || mon == "5" || mon == "6")
                iQuy = "2";
            else if (mon == "7" || mon == "8" || mon == "9")
                iQuy = "3";
            else
                iQuy = "4";
        }
        //ngân sách
        String iLoai = Convert.ToString(ViewData["iLoai"]);
        DataTable dtLoaiBaoCao = TCDNModels.getdsLoaiBaoCao();
        SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "iLoai", "sTen");
        if (String.IsNullOrEmpty(iLoai))
        {
            if (dtLoaiBaoCao.Rows.Count > 0)
            {
                iLoai = Convert.ToString(dtLoaiBaoCao.Rows[0]["iLoai"]);
            }
            else
            {
                iLoai = Guid.Empty.ToString();
            }
        }
        dtLoaiBaoCao.Dispose();

        String DVT = Convert.ToString(ViewData["DVT"]);
        DataTable dtDVT = TCDNModels.getdsDVTo();
        SelectOptionList slDVT = new SelectOptionList(dtDVT, "DVT", "sTen");
        if (String.IsNullOrEmpty(DVT))
        {
            if (dtDVT.Rows.Count > 0)
            {
                DVT = Convert.ToString(dtDVT.Rows[0]["DVT"]);
            }
            else
            {
                DVT = "1";
            }
        }
        dtLoaiBaoCao.Dispose();
        String bTrongKy = Convert.ToString(ViewData["bTrongKy"]);
        String iID_MaDoanhNghiep = Convert.ToString(ViewData["iID_MaDoanhNghiep"]);
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });
        String[] arrDonVi = iID_MaDoanhNghiep.Split(',');
        String[] arrView = new String[arrDonVi.Length];
        String Chuoi = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        if (String.IsNullOrEmpty(iID_MaDoanhNghiep)) PageLoad = "0";
        if (PageLoad == "1")
        {
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                arrView[i] = String.Format(@"/rptTCDN_BaoCaoInKiem/viewpdf?iID_MaDoanhNghiep={0}&iLoai={1}&iQuy={2}&MaND={3}&DVT={4}&bTrongKy={5}", arrDonVi[i], iLoai, iQuy, MaND,DVT,bTrongKy);
                Chuoi += arrView[i];
                if (i < arrDonVi.Length - 1)
                    Chuoi += ",";
            }
        }
        String urlExport = Url.Action("ExportToExcel", "rptTCDN_BaoCaoInKiem", new { });
        using (Html.BeginForm("EditSubmit", "rptTCDN_BaoCaoInKiem", new { ParentID = ParentID, }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo hồ sơ doanh nghiệp năm 
                            <%=iNamLamViec%></span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain" style="background-color: #F0F9FE;">
            <div id="Div2" style="margin-left: 10px;" class="table_form2">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <div>
                                <b>Chọn báo cáo :</b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiBaoCao, iLoai, "iLoai", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <b>Đơn vị :</b>
                        </td>
                        <td rowspan="20 " style="width: 30%;">
                            <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 300px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Chọn Quý :</b></div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Trong  kỳ :</b></div>
                        </td>
                        <td  style="text-align: left">
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, bTrongKy, "bTrongKy", "", "class=\"input1_2\" style=\"width:0%;\"")%>
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Đon vị tính :</b></div>
                        </td>
                        <td  style="text-align: left">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDVT, DVT, "DVT", "", "class=\"input1_2\" style=\"width:100%;\"")%>
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                   
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <table cellpadding="0" cellspacing="0" border="0" align="center">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <script type="text/javascript">
            function CheckAll(value) {
                $("input:checkbox[check-group='DonVi']").each(function (i) {
                    this.checked = value;
                });
            }                                            
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                   $('.title_tong a').click(function () {
                    $('div#rptMain').slideToggle('normal');
                    $(this).toggleClass('active');
                    return false;
                });
                var count = <%=arrView.Length%>;
                var Chuoi = '<%=Chuoi%>';
                var Mang=Chuoi.split(",");
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
            Chon();
            function Chon() {
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&iID_MaDonVi=#1", "rptTCDN_BaoCaoInKiem") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", "<%= iID_MaDoanhNghiep %>"));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }
                                                     
        </script>
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
        </script>
        <div>
            <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
        </div>
    </div>
    <%} %>
</body>
</html>
