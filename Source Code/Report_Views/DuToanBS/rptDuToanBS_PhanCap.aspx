<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models.DuToanBS" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToanBS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
    <style type="text/css">
        .style1
        {
            width: 10%;
            height: 10px;
        }
        .style2
        {
            height: 10px;
        }
    </style>
</head>
<body>
    <%
        String ParentID = "DuToanBS";
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);

        String iID_MaDot = Convert.ToString(ViewData["iID_MaDot"]);
        DataTable dtDot = DuToanBS_ReportModels.LayDSDot(iNamLamViec, MaND);
        SelectOptionList slDot = new SelectOptionList(dtDot, "iDotCap", "iDotCap");
        //if (String.IsNullOrEmpty(iID_MaDot))
        //{
        //    if (dtDot.Rows.Count > 0)
        //        iID_MaDot = Convert.ToString(dtDot.Rows[0]["MaDot"]);
        //    else
        //        iID_MaDot = Guid.Empty.ToString();
        //}
        dtDot.Dispose();
                
        //dt Danh sách phòng ban
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        DataTable dtPhongBan = DuToanBS_ReportModels.LayDSPhongBan(iNamLamViec, MaND); 
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();
        
        //dt Loại ngân sách
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);

        String BackURL = Url.Action("Index", "DuToanBS_Report", new { Loai = 0 });

        String[] arrDonVi = iID_MaDonVi.Split(',');
        String[] arrView = new String[arrDonVi.Length];
        String Chuoi = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        if (String.IsNullOrEmpty(sLNS)) PageLoad = "0";
        if (PageLoad == "1")
        {

            arrView = new string[1];
                arrView[0] =
                    String.Format(
                        @"/rptDuToanBS_PhanCap/viewpdf?iID_MaDonVi={0}&sLNS={1}&iID_MaDot={2}&iID_MaPhongBan={3}&MaND={4}",
                        iID_MaDonVi, sLNS, iID_MaDot, iID_MaPhongBan, MaND);
                Chuoi += arrView[0];
        }


        using (Html.BeginForm("FormSubmit", "rptDuToanBS_PhanCap", new { ParentID = ParentID }))
        {
    %>
   
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo bảng kiểm số liệu phân cấp</span>
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
                        <td class="td_form2_td1" style="width: 8%; height: 20px">
                            <div>
                                <b>Chọn đợt</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%; height: 20px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDot, iID_MaDot, "iID_MaDot", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width :8% ; height : 20px">
                            <div><b>Chọn đơn vị</b></div>
                        </td>
                         <td class="td_form2_td5" rowspan="25" style="width: 20%;">
                             <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width : 8%; height:20px">
                            <div><b>Loại ngân sách</b></div>
                        </td>
                       <td class="td_form2_td5" rowspan="25" style="width: 20%;">
                            <div id="<%= ParentID %>_tdLNS" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 2%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                         <div>
                                <b>Chọn phòng ban</b>
                         </div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%; height: 20px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon() ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
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
                $(document).ready(function () {
                   $('.title_tong a').click(function () {
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
            function CheckAllDonVi(value) {
                $("input:checkbox[check-group='DV']").each(function (i) {
                    this.checked = value;
                });
                ChonDonVi();
            }                                            
        </script>
        
        <script type="text/javascript">
            function CheckAll(value) {
                $("input:checkbox[check-group='LNS']").each(function (i) {
                    this.checked = value;
                });
            }                                            
        </script>

        <script type="text/javascript">
            function ChonDonVi() {
                var iID_MaDot = document.getElementById("<%=ParentID %>_iID_MaDot").value;
                var MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;
                var iID_MaDonVi = "";
                $("input:checkbox[check-group='DV']").each(function (i) {
                    if (this.checked) {
                        if (iID_MaDonVi != "") iID_MaDonVi += ",";
                        iID_MaDonVi += this.value;
                    }
                });
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("LayDanhSachLNS?ParentID=#0&iID_MaDot=#1&iID_MaDonVi=#2&sLNS=#3&iID_MaPhongBan=#4", "rptDuToanBS_PhanCap") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", iID_MaDot));
                url = unescape(url.replace("#2", iID_MaDonVi));
                url = unescape(url.replace("#3", "<%= sLNS %>"));
                url = unescape(url.replace("#4", MaPhongBan));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdLNS").innerHTML = data;
                });
            }

            
            //don vi
            Chon();
            function Chon() {
                var iID_MaDonVi = '<%=iID_MaDonVi %>';
                var iID_MaDot = document.getElementById("<%=ParentID %>_iID_MaDot").value;
                var iID_MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;

                jQuery.ajaxSetup({ cache: false });

                var url = unescape('<%= Url.Action("LayDanhSachDonVi?ParentID=#0&iID_MaDot=#1&iID_MaPhongBan=#2&iID_MaDonVi=#3", "rptDuToanBS_PhanCap") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", iID_MaDot));
                url = unescape(url.replace("#2", iID_MaPhongBan));
                url = unescape(url.replace("#3", iID_MaDonVi));

                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                    ChonDonVi();
                });
                //ChonDonVi();
            }
                                                   
        </script>
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
        </script>
    </div>
    <%} %>
</body>
</html>
