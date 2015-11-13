<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
<%@ Import Namespace="FlexCel.Core" %>
<%@ Import Namespace="FlexCel.Render" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
   
        String ParentID = "BaoCaoNganSachNam";
        int SoCot = 1;
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String ToSo = Request.QueryString["ToSo"];
        String Nganh = Convert.ToString(ViewData["Nganh"]);
        String MaDot = Convert.ToString(ViewData["MaDot"]);
        //String sLNS = Convert.ToString(ViewData["sLNS"]);
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        if (String.IsNullOrEmpty(MaDot))
        {
            MaDot = "-1";
        }
        if (String.IsNullOrEmpty(Nganh))
        {
            Nganh = "0";
        }
        if (String.IsNullOrEmpty(iID_MaPhongBan))
        {
            iID_MaPhongBan = "0";
        }
        if (String.IsNullOrEmpty(ToSo))
        {
            ToSo = "1";
        }
        DataTable dtNganh = DuToan_ReportModels.dtNganh_PC(MaND, "1020100");
        SelectOptionList slNganh = new SelectOptionList(dtNganh, "iID", "sTenNganh");

        //B dich
        DataTable dtBDich = DuToan_ReportModels.dtPhongBanInBaoDam();
        SelectOptionList slBDich = new SelectOptionList(dtBDich, "iID", "sTen");
        dtBDich.Dispose();
        //Nguon bao dam

        DataTable dtDot = new DataTable();
        dtDot.Columns.Add("MaDot", typeof(string));
        dtDot.Columns.Add("TenDot", typeof(string));
        DataRow R1 = dtDot.NewRow();
        R1["MaDot"] = "-1";
        R1["TenDot"] = "Null";
        dtDot.Rows.Add(R1);
            SelectOptionList slDot = new SelectOptionList(dtDot, "MaDot", "TenDot");
        dtDot.Dispose();
        DataTable dtTo = rptDuToan_1040100_TungDotController.DanhSachToIn(MaND, Nganh, ToSo, MaDot, iID_MaPhongBan);
        String MaTo = Convert.ToString(ViewData["MaTo"]);
        String[] arrMaDonVi = MaTo.Split(',');
        String[] arrMaTo = MaTo.Split(',');
        String Chuoi = "";
        String[] arrView = new String[arrMaTo.Length];
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        if (String.IsNullOrEmpty(MaTo)) PageLoad = "0";
        if (PageLoad == "1")
        {
            for (int i = 0; i < arrMaTo.Length; i++)
            {
                arrView[i] =
                    String.Format(
                        @"/rptDuToan_1040100_TungDot/viewpdf?ToSo={0}&MaND={1}&Nganh={2}&MaDot={3}&iID_MaPhongBan={4}",
                        arrMaTo[i], MaND, Nganh, MaDot, iID_MaPhongBan);
                Chuoi += arrView[i];
                if (i < arrMaTo.Length - 1)
                    Chuoi += "+";
            }

        }

        String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "1" });
        using (Html.BeginForm("EditSubmit", "rptDuToan_1040100_TungDot", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo dự toán chi ngân sách quốc phòng năm
                            <%=iNamLamViec%>
                            (Phần phụ lục 5b)</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td style="width: 10%" class="td_form2_td1">
                            <b>Chọn ngành: </b>
                        </td>
                        <td class="td_form2_td1" style="text-align: center; width: 30%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNganh, Nganh, "Nganh", "", "class=\"input1_2\" style=\"width: 40%;height:24px;\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <div>
                                Chọn tờ:</div>
                        </td>
                        <td style="width: 30%" rowspan="3">
                            <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 200px">
                            </div>
                        </td>
                        <td class="td_form2_td1">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%" class="td_form2_td1">
                            <b>Chọn phòng ban: </b>
                        </td>
                        <td class="td_form2_td1" style="text-align: center; width: 30%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slBDich, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 40%;height:24px;\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%" class="td_form2_td1">
                            <b>Chọn đợt: </b>
                        </td>
                        <td class="td_form2_td1" style="text-align: center; width: 30%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDot, MaDot, "MaDot", "", "class=\"input1_2\" style=\"width: 40%;height:24px;\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
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
                Chon();
            function Chon() {
                 var Nganh = document.getElementById("<%=ParentID %>_Nganh").value;
                    var MaDot = document.getElementById("<%=ParentID %>_MaDot").value;
                       var iID_MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&Nganh=#1&ToSo=#2&MaDot=#3&iID_MaPhongBan=#4", "rptDuToan_1040100_TungDot") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", Nganh));
                url = unescape(url.replace("#2","<%= MaTo %>"));
                url = unescape(url.replace("#3",MaDot));
                url = unescape(url.replace("#4",iID_MaPhongBan));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }
                    </script>
                </table>
            </div>
        </div>
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
        </script>
    </div>
    <%}
        
    %>
    <div>
        <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToan_1040100_TungDot", new { MaND = MaND, Nganh = Nganh, ToSo = MaTo, iID_MaPhongBan = iID_MaPhongBan, MaDot = MaDot }), "Xuất ra Excels")%>
    </div>
</body>
</html>
