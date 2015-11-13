<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.CapPhat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%  
        String iDM_MaLoaiCapPhat = Convert.ToString(ViewData["iID_MaLoaiCapPhat"]);
        DataTable dtLoaiCapPhat = CommonFunction.Lay_dtDanhMuc("LoaiCapPhat");
        DataRow R4 = dtLoaiCapPhat.NewRow();
        R4["iID_MaDanhMuc"] = Guid.Empty;
        R4["sTen"] = "--- Chọn tất cả ---";
        dtLoaiCapPhat.Rows.InsertAt(R4, 0);
        dtLoaiCapPhat.Dispose();
        SelectOptionList slLoaiCapPhat = new SelectOptionList(dtLoaiCapPhat, "iID_MaDanhMuc", "sTen");
        dtLoaiCapPhat.Dispose();

        String iID_MaTinhChatCapThu = Convert.ToString(ViewData["iID_MaTinhChatCapThu"]);
        DataTable dtTinhChatCapThu = TinhChatCapThuModels.Get_dtTinhChatCapThu();
        SelectOptionList slTinhChatCapThu = new SelectOptionList(dtTinhChatCapThu, "iID_MaTinhChatCapThu", "sTen");
        DataRow R2 = dtTinhChatCapThu.NewRow();
        R2["iID_MaTinhChatCapThu"] = "-1";
        R2["sTen"] = "--- Chọn tất cả ---";
        dtTinhChatCapThu.Rows.InsertAt(R2, 0);
        dtTinhChatCapThu.Dispose();

        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        DataTable dtPhongBan = DanhMucModels.NS_PhongBan();
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");
        DataRow R1 = dtPhongBan.NewRow();
        R1["iID_MaPhongBan"] = Guid.Empty;
        R1["sTen"] = "--- Chọn tất cả ---";
        dtPhongBan.Rows.InsertAt(R1, 0);
        dtPhongBan.Dispose();

        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(User.Identity.Name);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        DataRow R3 = dtDonVi.NewRow();
        R3["iID_MaDonVi"] = Guid.Empty;
        R3["sTen"] = "--- Chọn tất cả ---";
        dtDonVi.Rows.InsertAt(R3, 1);
        dtDonVi.Dispose();


        String LuyKe = Request.QueryString["LuyKe"];
        if (String.IsNullOrEmpty(LuyKe))
        {
            LuyKe = "";
        }
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "CapPhat";

        String iID_MaCapPhat = Convert.ToString(ViewData["iID_MaCapPhat"]);

        String iID_MaDonVi = Convert.ToString(ViewData["sMaDonVi"]);
        String[] arrMaDonVi;
        String MaDonVi = "-111";//= arrMaDonVi[ChiSo];
        int ChiSo = 0;
        if (String.IsNullOrEmpty(iID_MaDonVi) == false)
        {
            arrMaDonVi = iID_MaDonVi.Split(',');

            ChiSo = Convert.ToInt16(Request.QueryString["ChiSo"]);
            if (ChiSo == arrMaDonVi.Length)
            {
                ChiSo = 0;
            }
            if (ChiSo <= arrMaDonVi.Length - 1)
            {
                MaDonVi = arrMaDonVi[ChiSo];
                ChiSo = ChiSo + 1;
            }
            else
            {
                ChiSo = 0;
            }
        }
        else
        {
            iID_MaDonVi = "-111";
        }
        //khoi tao dt loai thong tri
        DataTable dtLoaiThongTri = new DataTable();
        dtLoaiThongTri.Columns.Add("sLoaiThongTri", typeof(String));
        dtLoaiThongTri.Columns.Add("sTenThongTri", typeof(String));
        DataRow r = dtLoaiThongTri.NewRow();
        r[0] = "sNG";
        r[1] = "Ngành";
        dtLoaiThongTri.Rows.Add(r);

        r = dtLoaiThongTri.NewRow();
        r[0] = "sTM";
        r[1] = "Tiểu mục";
        dtLoaiThongTri.Rows.Add(r);

        r = dtLoaiThongTri.NewRow();
        r[0] = "sM";
        r[1] = "Mục";
        dtLoaiThongTri.Rows.Add(r);

        r = dtLoaiThongTri.NewRow();
        r[0] = "sLNS";
        r[1] = "Loại ngân sách";
        dtLoaiThongTri.Rows.Add(r);

        SelectOptionList slLoaiThongTri = new SelectOptionList(dtLoaiThongTri, "sLoaiThongTri", "sTenThongTri");
        String sLoaiThongTri = Convert.ToString(ViewData["sLoaiThongTri"]);

        if (String.IsNullOrEmpty(sLoaiThongTri))
        {
            sLoaiThongTri = "sNG";
        }
        String iID_MaLoaiCapPhat = Convert.ToString(ViewData["iID_MaLoaiCapPhat"]);
        String urlExportToExcel = Url.Action("ExportToExcel", "rptCapPhatThongTri_ChonChungTu", new { iID_MaDonVi = MaDonVi, iID_MaCapPhat = iID_MaCapPhat, iDM_MaLoaiCapPhat = iDM_MaLoaiCapPhat, sLoaiThongTri = sLoaiThongTri });
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
         String URLPDF="";
         String BackURL = Url.Action("Index", "CapPhat_Report");
        if(PageLoad=="1")
            URLPDF = Url.Action("ViewPDF", "rptCapPhatThongTri_ChonChungTu", new { iID_MaDonVi = MaDonVi, iID_MaCapPhat = iID_MaCapPhat, iDM_MaLoaiCapPhat = iDM_MaLoaiCapPhat, sLoaiThongTri = sLoaiThongTri });
        using (Html.BeginForm("EditSubmit", "rptCapPhatThongTri_ChonChungTu", new { ParentID = ParentID, ChiSo = ChiSo }))
        {                  
    %>
    <div class="box_tong">
        <div class="title_tong">
             <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td width="47.9%">
                            <span>Báo cáo thông chi chọn chứng từ</span>
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
                        <td class="style1" align="right" width="20%">
                            <div>
                                <%=NgonNgu.LayXau("Loại cấp phát : ")%></div>
                        </td>
                        <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiCapPhat, iID_MaLoaiCapPhat, "iID_MaLoaiCapPhat", "", "class=\"input1_2\" style=\"width: 100%\" onchange=ChonCP()")%>
                            </div>
                        </td>
                        <td width="20%" id="<%= ParentID %>_tdCapPhat" rowspan="6">
                            <% rptCapPhatThongTri_ChonChungTuController rpt = new rptCapPhatThongTri_ChonChungTuController();
                               rptCapPhatThongTri_ChonChungTuController.data _Data = new rptCapPhatThongTri_ChonChungTuController.data();
                            %>
                            <%=_Data.iID_MaCapPhat%>
                        </td>
                        <td width="20%" id="<%= ParentID %>_tdDonVi" rowspan="6">
                         <%=_Data.iID_MaDonVi%>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1" align="right" valign="top">
                            <div>
                                <%=NgonNgu.LayXau("Loại thông tri:")%></div>
                        </td>
                        <td width="10%" valign="top">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiThongTri, sLoaiThongTri, "sLoaiThongTri", "", "class=\"input1_2\" style=\"width: 100%\" ")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                    <td></td>
                    </tr>
                   <tr>
                    <td></td>
                    </tr>
                   <tr>
                    <td></td>
                    </tr><tr>
                    <td></td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                        <td colspan="5">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin-bottom: 10px;
                                margin-right: 10px; margin-top: 10px;" width="100%">
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
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <%=MyHtmlHelper.ActionLink(urlExportToExcel,"Export to Excel") %>
      <iframe src="<%=URLPDF%>"
        height="600px" width="100%"></iframe>
</body>
</html>
<script type="text/javascript">
    function Huy() {
        window.location.href = '<%=BackURL%>';
    }
    ChonCP();
    function ChonCP() {
        var iID_MaLoaiCapPhat = document.getElementById("<%=ParentID %>_iID_MaLoaiCapPhat").value;
        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("ds_DonVi?iDM_LoaiCapPhat=#0&iID_MaDonVi=#1&iID_MaCapPhat=#2", "rptCapPhatThongTri_ChonChungTu") %>');
        url = unescape(url.replace("#0", iID_MaLoaiCapPhat));
        url = unescape(url.replace("#1", "<%= iID_MaDonVi %>"));
        url = unescape(url.replace("#2", "<%= iID_MaCapPhat %>"));
        $.getJSON(url, function (data) {

            document.getElementById("<%= ParentID %>_tdCapPhat").innerHTML = data.iID_MaCapPhat;
            document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
        });
    }
    function ChonallCT(value) {
        $("input:checkbox[check-group='MaCapPhat']").each(function (i) {
            this.checked = value;
        });
        ChonDV();
    }
    function ChonallDV(value) {
        $("input:checkbox[check-group='MaDonVi']").each(function (i) {
            this.checked = value;
        });
    }
    function ChonDV() {
        var iID_MaLoaiCapPhat = document.getElementById("<%=ParentID %>_iID_MaLoaiCapPhat").value;
        var iID_MaCapPhat = "";
        $("input:checkbox[check-group='MaCapPhat']").each(function (i) {
            if (this.checked) {
                if (iID_MaCapPhat != "") iID_MaCapPhat += ",";
                iID_MaCapPhat += this.value;
            }
        });
        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("ds_DonVi?iDM_LoaiCapPhat=#0&iID_MaDonVi=#1&iID_MaCapPhat=#2", "rptCapPhatThongTri_ChonChungTu") %>');
        url = unescape(url.replace("#0", iID_MaLoaiCapPhat));
        url = unescape(url.replace("#1", "<%= iID_MaDonVi %>"));
        url = unescape(url.replace("#2", iID_MaCapPhat));
        $.getJSON(url, function (data) {

            document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
        });
    }
    $(function () {
        $("div#rptMain").hide();
        $('div.login1 a').click(function () {
            $('div#rptMain').slideToggle('normal');
            $(this).toggleClass('active');
            return false;
        });
    });                                             
</script>
