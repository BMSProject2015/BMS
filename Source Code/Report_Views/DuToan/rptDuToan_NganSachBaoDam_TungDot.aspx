<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models.DuToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
        String ParentID = "BaoCaoNganSachNam";
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
        R1["TenDot"] = "--Chọn đợt--";
        dtDot.Rows.Add(R1);
        SelectOptionList slDot = new SelectOptionList(dtDot, "MaDot", "TenDot");
        dtDot.Dispose();
        
        DataTable dtTo = rptDuToan_NganSachBaoDam_TungDotController.DanhSachToIn(MaND, Nganh, ToSo, MaDot, iID_MaPhongBan);
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
                        @"/rptDuToan_NganSachBaoDam_TungDot/viewpdf?ToSo={0}&MaND={1}&Nganh={2}&MaDot={3}&iID_MaPhongBan={4}",
                        arrMaTo[i], MaND, Nganh, MaDot, iID_MaPhongBan);
                Chuoi += arrView[i];
                if (i < arrMaTo.Length - 1)
                    Chuoi += "+";
            }

        }

        String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "1" });
        using (Html.BeginForm("FormSubmit", "rptDuToan_NganSachBaoDam_TungDot", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo dự toán ngân sách bảo đảm
                            <%=iNamLamViec%>
                            </span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color: #F0F9FE;">
            <div id="Div2">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="td_form2_td1" style="width: 5%">
                        </td>
                        <td style="width: 10%" class="td_form2_td1">
                            <div><b>Chọn ngành</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNganh, Nganh, "Nganh", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <div>
                                Chọn tờ</div>
                        </td>
                        <td class="td_form2_td5" style="width: 30%" rowspan="10">
                            <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 200px">
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 5%">
                        </td>
                        <td style="width: 10%" class="td_form2_td1">
                            <div><b>Chọn phòng ban</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slBDich, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1">
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 5%">
                        </td>
                        <td style="width: 10%" class="td_form2_td1">
                            <div><b>Chọn đợt</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 14%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDot, MaDot, "MaDot", "", "class=\"input1_2\" style=\"width: 100%;height:24px;\" onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1">
                        </td>
                    </tr>
                    <tr >
                        <td>
                         &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td >
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
                        <td >
                         &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
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
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
        </script>
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
                var url = unescape('<%= Url.Action("LayDanhSachDonVi?ParentID=#0&Nganh=#1&ToSo=#2&MaDot=#3&iID_MaPhongBan=#4", "rptDuToan_NganSachBaoDam_TungDot") %>');
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
    </div>
    <%}
        
    %>

</body>
</html>
