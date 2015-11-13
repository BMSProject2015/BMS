<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Content/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <div>
        <%
            String ParentID = "PhanBo";
            String PageLoad = Convert.ToString(ViewData["PageLoad"]);
            String MaND = User.Identity.Name;
            //Loai ngan sach
            String sLNS = Convert.ToString(ViewData["sLNS"]);
            String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
            DataTable dtLNS = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
            SelectOptionList slLoaiNganSach = new SelectOptionList(dtLNS, "sLNS", "sLNS");
            if (String.IsNullOrEmpty(sLNS))
            {
                if (dtLNS.Rows.Count > 0)
                {
                    sLNS = dtLNS.Rows[0]["sLNS"].ToString();
                }
                else
                {
                    sLNS = Guid.Empty.ToString();
                }
            }
            dtLNS.Dispose();

            //dt Trạng thái duyệt
            String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
            if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
            {
                iID_MaTrangThaiDuyet = "0";
            }
            DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHePhanBo);
            SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
            dtTrangThai.Dispose();

            //Dot phân bổ
            String iID_MaDotPhanBo = Convert.ToString(ViewData["iID_MaDotPhanBo"]);
            if (String.IsNullOrEmpty(iID_MaDotPhanBo))
            {
                DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet, sLNS);
                if (dtDotPhanBo.Rows.Count > 1)
                {
                    iID_MaDotPhanBo = dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString();
                }
                else
                {
                    iID_MaDotPhanBo = Guid.Empty.ToString();
                }
                dtDotPhanBo.Dispose();
            }

            String[] arrLNS = sLNS.Split(',');
            String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
            if (String.IsNullOrEmpty(iID_MaDonVi))
            {
                DataTable dtDonVi = PhanBo_ReportModels.DanhSachDonVi2(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo);
                if (dtDonVi.Rows.Count > 1)
                {
                    iID_MaDonVi = dtDonVi.Rows[1]["iID_MaDonVi"].ToString();
                }
                else
                {
                    iID_MaDonVi = Guid.Empty.ToString();
                }
                dtDonVi.Dispose();
            }
            //kieu doc hay ngang
            String MaKieu = Convert.ToString(ViewData["MaKieu"]);
            DataTable dtKieu = PhanBo_ReportModels.KieuHienThi();
            SelectOptionList slKieu = new SelectOptionList(dtKieu, "MaKieu", "TenKieu");
            if (String.IsNullOrEmpty(MaKieu))
            {
                MaKieu = "1";
            }
            dtKieu.Dispose();
            // Kiểu thông báo
            String iThongBao = Convert.ToString(ViewData["iThongBao"]);
            DataTable dtThongBao = PhanBo_ReportModels.KieuThongBao();
            SelectOptionList slThongBao = new SelectOptionList(dtThongBao, "MaThongBao", "TenThongBao");
            if (String.IsNullOrEmpty(iThongBao))
            {
                iThongBao = "1";
            }
            dtThongBao.Dispose();
            // den muc
            String sMuc = Convert.ToString(ViewData["sMuc"]);
            DataTable dtMuc = PhanBo_ReportModels.KieuMuc();
            SelectOptionList slMuc = new SelectOptionList(dtMuc, "MaMuc", "TenMuc");
            if (String.IsNullOrEmpty(sMuc))
            {
                sMuc = "sNG";
            }
            dtMuc.Dispose();
            String URLView = "";
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
            if (PageLoad == "1")
            {
                URLView = Url.Action("ViewPDF", "rptPhanBo_4_2", new { iID_MaDotPhanBo = iID_MaDotPhanBo, iID_MaDonVi = MaDonVi, MaKieu = MaKieu, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iThongBao = iThongBao, sMuc = sMuc });
            }
            if (PageLoad != "1") ChiSo = 0;
            String BackURL = Url.Action("Index", "PhanBo_Report");
            using (Html.BeginForm("EditSubmit", "rptPhanBo_4_2", new { ParentID = ParentID, ChiSo = ChiSo }))
            {%>
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td width="47.9%">
                            <span>Báo cáo thông báo ngân sách- thông báo lũy kê</span>
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
                            <td style="width: 5%;">
                            </td>
                            <td style="width: 25%;" rowspan="23">
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
                            <td style="width: 8%;" class="td_form2_td1">
                                <div>
                                    Trạng Thái:
                                </div>
                            </td>
                            <td style="width: 10%;">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\" onchange=Chon()")%></div>
                            </td>
                            <td style="width: 8%;" class="td_form2_td1">
                                <div>
                                    Đợt phân bổ :
                                </div>
                            </td>
                            <td width="10%" id="<%= ParentID %>_tdDot">
                                <% rptPhanBo_4_2Controller rpt = new rptPhanBo_4_2Controller();
                                   rptPhanBo_4_2Controller.LNSdata _Data = new rptPhanBo_4_2Controller.LNSdata();
                                   _Data = rpt.obj_DotPhanBo(ParentID, sLNS, MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo, iID_MaDonVi, iThongBao);
                                %>
                                <%=_Data.iID_MaDotPhanBo%>
                            </td>
                            <td class="td_form2_td1" width="8%">
                                <div>
                                    Đơn vị :
                                </div>
                            </td>
                            <td width="20%" rowspan="23">
                                <div id="<%= ParentID %>_tdDonVi" style="height: 485px; overflow: scroll;">
                                </div>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td class="td_form2_td1">
                                <div>
                                    Thông báo:
                                </div>
                            </td>
                            <td width="10%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slThongBao, iThongBao, "iThongBao", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\" onchange=ChonDV()")%></div>
                            </td>
                            <td class="td_form2_td1">
                                <div>
                                    Kiểu In:
                                </div>
                            </td>
                            <td>
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slKieu, MaKieu, "MaKieu", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                                </div>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td class="td_form2_td1">
                                <div>
                                </div>
                            </td>
                            <td>
                                <%--<div>
                                 <%=MyHtmlHelper.DropDownList(ParentID, slMuc, sMuc, "sMuc", "", "class=\"input1_2\" style=\"width: 100%\"")%>                                </div>      --%>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            <td>
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
                            </td>
                            <td>
                            </td>
                            <td colspan="2">
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
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $("div#rptMain").hide();
                $('div.login1 a').click(function () {
                    $('div#rptMain').slideToggle('normal');
                    $(this).toggleClass('active');
                    return false;
                });
            });       
        </script>
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
        </script>
        <script type="text/javascript">
            Chon();
            function Chon() {
                var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                var iThongBao = document.getElementById("<%=ParentID %>_iThongBao").value;
                var sLNS = "";
                $("input:checkbox[check-group='sLNS']").each(function (i) {
                    if (this.checked) {
                        if (sLNS != "") sLNS += ",";
                        sLNS += this.value;
                    }
                });
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("ds_DotPhanBo?ParentID=#0&sLNS=#1&MaND=#2&iID_MaTrangThaiDuyet=#3&iID_MaDotPhanBo=#4&iID_MaDonVi=#5&iThongBao=#6", "rptPhanBo_4_2") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", sLNS));
                url = unescape(url.replace("#2", "<%=MaND %>"));
                url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));
                url = unescape(url.replace("#4", "<%= iID_MaDotPhanBo %>"));
                url = unescape(url.replace("#5", "<%= iID_MaDonVi %>"));
                url = unescape(url.replace("#6", iThongBao));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDot").innerHTML = data.iID_MaDotPhanBo;
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
                });
            }                                            
        </script>
        <script type="text/javascript">
            function ChonDV() {
                var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                var iID_MaDotPhanBo = document.getElementById("<%=ParentID %>_iID_MaDotPhanBo").value;
                var iThongBao = document.getElementById("<%=ParentID %>_iThongBao").value;
                var sLNS = "";
                $("input:checkbox[check-group='sLNS']").each(function (i) {
                    if (this.checked) {
                        if (sLNS != "") sLNS += ",";
                        sLNS += this.value;
                    }
                });

                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("ds_DotPhanBo?ParentID=#0&sLNS=#1&MaND=#2&iID_MaTrangThaiDuyet=#3&iID_MaDotPhanBo=#4&iID_MaDonVi=#5&iThongBao=#6", "rptPhanBo_4_2") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", sLNS));
                url = unescape(url.replace("#2", "<%=MaND %>"));
                url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));
                url = unescape(url.replace("#4", iID_MaDotPhanBo));
                url = unescape(url.replace("#5", "<%= iID_MaDonVi %>"));
                url = unescape(url.replace("#6", iThongBao));
                $.getJSON(url, function (data) {

                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
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
            function CheckAll(value) {
                $("input:checkbox[check-group='DonVi']").each(function (i) {
                    this.checked = value;
                });
            }                                            
        </script>
        <%}%>
        <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
