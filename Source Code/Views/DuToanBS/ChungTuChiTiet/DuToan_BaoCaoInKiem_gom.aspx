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
        string ParentID = "DuToanBS";
        string MaND = User.Identity.Name;
        string iID_MaChungTu_TLTH = Convert.ToString(Request.QueryString["iID_MaChungTu"]);
        string iChiTapTrung = Convert.ToString(Request.QueryString["iChiTapTrung"]);
        string iID_MaPhongBan = "";
        
        //dt Phòng Ban.
        DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
        if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
        {
            DataRow drPhongBan = dtPhongBan.Rows[0];
            iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
            dtPhongBan.Dispose();
        }

        DataTable dt = DuToanBS_ChungTuModels.Get_DanhSachChungTu(iID_MaChungTu_TLTH, "", iID_MaPhongBan, "", "", MaND, "", "", "", "", "", false);
        
        //dtKieu xem
        string sKieuXem = Convert.ToString(ViewData["sKieuXem"]);
        DataTable dtKieuXem = DuToanBS_ReportModels.dtKieuXem();
        SelectOptionList slKieuXem= new SelectOptionList(dtKieuXem,"iID","sTen");
        if (String.IsNullOrEmpty(sKieuXem))
        {
            if (dtKieuXem.Rows.Count > 0)
            {
                sKieuXem = Convert.ToString(dtKieuXem.Rows[0]["iID"]);
            }
            else
            {
                sKieuXem = Guid.Empty.ToString();
            }
        }
        dtKieuXem.Dispose();

        //iDonViTinh
        String iDonViTinh = "";
        DataTable dtDonViTinh = DuToanBS_ReportModels.dtDonViTinh();
        SelectOptionList slDonViTinh= new SelectOptionList(dtDonViTinh,"iID","sTen");
        if (dtDonViTinh.Rows.Count > 0)
        {
            iDonViTinh = Convert.ToString(dtDonViTinh.Rows[0]["iID"]);
        }
        else
        {
            iDonViTinh = Guid.Empty.ToString();
        }
        dtDonViTinh.Dispose();

        int SoCot = 1;
        
        using (Html.BeginForm("EditSubmit", "rptDuToanBS_BieuKiem_Gom", FormMethod.Post, new { target = "_blank" }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, iID_MaChungTu_TLTH, "iID_MaChungTu_TLTH", "")%>
     <%=MyHtmlHelper.Hidden(ParentID, iChiTapTrung, "iChiTapTrung", "")%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td class="td_form2_td1" style="width: 10%">
                <div>Chọn kiểu xem:</div>
            </td>
            <td class="td_form2_td1" style="width: 10%">
                <div>
                   <%=MyHtmlHelper.DropDownList(ParentID,slKieuXem,sKieuXem,"sKieuXem","","class=\"input1_2\" style=\"width: 100%\"") %>
                </div>
            </td>
            <td class="td_form2_td1" style="width: 10%">
                <div>Chọn Đợt:</div>
            </td>
            <td style="width: 25%" rowspan="10">
                <div style="overflow: scroll; height: 400px">
                    <table class="mGrid" style="width: 100%">
                        <tr>
                            <th align="center" style="width: 40px;">
                                <input type="checkbox" id="abc" onclick="CheckAllDV(this.checked)" />
                            </th>
                            <%for (int c = 0; c < SoCot * 2 - 1; c++)
                              {%>
                            <th>
                            </th>
                            <%} %>
                        </tr>
                        <%
                        String strTen = "", strMa = "", strChecked = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                        %>
                        <tr>
                        <% for (int c = 0; c < 1; c++)
                            {
                                if (i + c < dt.Rows.Count)
                                {
                                    strChecked = "";
                                    strTen = Convert.ToString(dt.Rows[i + c]["sDSLNS"]).Substring(0, 7) + '-' +
                                            CommonFunction.LayXauNgay(
                                            Convert.ToDateTime(dt.Rows[i + c]["dNgayChungTu"])) + '-' +
                                            Convert.ToString(dt.Rows[i + c]["sID_MaNguoiDungtao"]) + '-' +
                                            Convert.ToString(dt.Rows[i + c]["sNoiDung"]) + '-' +
                                            Convert.ToString(dt.Rows[i + c]["sLyDo"]);
                                    strMa = Convert.ToString(dt.Rows[i + c]["iID_MaChungTu"]);
                        %>
                            <td align="center" style="width: 40px;">
                                <input type="checkbox" value="<%= strMa %>" <%= strChecked %> check-group="rptChungTu"
                                    id="rptiID_MaChungTu" name="rptiID_MaChungTu" onclick="Chon()" />
                            </td>
                            <td align="left">
                                <%= strTen%>
                            </td>
                            <% } %>
                            <% } %>
                        </tr>
                        <%}%>
                    </table>
                </div>
            </td>
            <td class="td_form2_td1" style="width: 10%">
                <div>Chọn Đơn Vị:</div>
            </td>
            <td rowspan="10" style="width: 30%;">
                <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 400px">
                </div>
            </td>
        </tr>
        <tr>
            <td class="td_form2_td1">
                <div>Chọn ĐVT:</div>
            </td>
            <td class="td_form2_td1">
                <div><%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iDonViTinh, "iDonViTinh", "", "class=\"input1_2\" style=\"width: 100%\"")%></div>
            </td>
            <td class="td_form2_td1">
                <div>
                    <b></b>
                </div>
            </td>
            <td class="td_form2_td1">
                <div>
                    <b></b>
                </div>
            </td>
        </tr>
        <tr>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
        </tr>
        <tr>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
        </tr>
        <tr>
             <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
        </tr>
        <tr>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
        </tr>
        <tr>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
        </tr>
        <tr>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
        </tr>
        <tr>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
        </tr>
        <tr>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
        </tr>
        <tr>
             <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
            <td class="td_form2_td1">&nbsp;</td>
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
                                <input type="button" class="button" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                            </td>
                            <td style="width: 40%">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
         <script type="text/javascript">
             function CheckAll(value) {
                 $("input:checkbox[check-group='DonVi']").each(function (i) {
                     this.checked = value;
                 });
             }                                            
        </script>
        <script type="text/javascript">
            function CheckAllDV(value) {
                $("input:checkbox[check-group='rptChungTu']").each(function (i) {
                    this.checked = value;
                });
                Chon();
            }

            Chon();
            function Chon() {
                var dsMaChungTu = "";
                $("input:checkbox[check-group='rptChungTu']").each(function (i) {
                    if (this.checked) {
                        if (dsMaChungTu != "") 
                            dsMaChungTu += ",";
                        dsMaChungTu += this.value;
                    }
                });
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&dsMaChungTu=#1", "rptDuToanBS_BieuKiem_Gom") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", dsMaChungTu));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }                                            
        </script>
    </table>
    <%} %>
</body>
</html>
