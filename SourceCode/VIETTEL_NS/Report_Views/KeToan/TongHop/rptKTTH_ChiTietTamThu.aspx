<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <%
        String ParentID = "KeToan";
        String MaND = User.Identity.Name;
        String iNamLamViec = "2012";
        String iThangLamViec = "1";
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        {
            iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            iThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
        }

        String pageload = Convert.ToString(ViewData["pageload"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        String DonViTinh = Convert.ToString(ViewData["DonViTinh"]);
        String sNoiDung = Convert.ToString(ViewData["NoiDung"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "1";
        }
        DataTable dtTrangThai = rptKTTK_ChiTietTamThuController.tbTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            if (dtTrangThai.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]);
            }
            else
            {
                iID_MaTrangThaiDuyet = Guid.Empty.ToString();
            }
        }
        dtTrangThai.Dispose();

        DataTable dtDonViTinh = rptKTTK_ChiTietTamThuController.DonViTinh();
        SelectOptionList slDonVi = new SelectOptionList(dtDonViTinh, "iID_DonViTinh", "TenDonVi");
        if (String.IsNullOrEmpty(DonViTinh))
        {
            if (dtDonViTinh.Rows.Count > 0)
            {
                DonViTinh = Convert.ToString(dtDonViTinh.Rows[0]["iID_DonViTinh"]);
            }
            else
            {
                DonViTinh = Guid.Empty.ToString();
            }
        }
        dtDonViTinh.Dispose();
        String iNgay = Convert.ToString(ViewData["iNgay"]);
        String iThang = Convert.ToString(ViewData["iThang"]);
        if (String.IsNullOrEmpty(iNgay))
            iNgay = HamChung.GetDaysInMonth(Convert.ToInt32(iThangLamViec), Convert.ToInt32(iNamLamViec)).ToString();
        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        if (String.IsNullOrEmpty(iThang))
            iThang = iThangLamViec;
        DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNamLamViec));
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        if (String.IsNullOrEmpty(iNgay))
        {
            iNgay = "1";
        }
        dtNgay.Dispose();
        var tbl = rptKTTK_ChiTietTamThuController.DT_Loai(true, "Tất cả", User.Identity.Name, false);
        SelectOptionList slNoiDung = new SelectOptionList(tbl, "sKyHieu", "sTen");
        if (tbl != null) tbl.Dispose();
        String urlReport = pageload.Equals("1") ? Url.Action("ViewPDF", "rptKTTK_ChiTietTamThu", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iNgay = iNgay, iThang = iThang, DonViTinh = DonViTinh,NoiDung=sNoiDung }) : "";

        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        using (Html.BeginForm("EditSubmit", "rptKTTK_ChiTietTamThu", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo chi tiết tạm thu (Tài khoản 331)</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
            <div id="Div2" style="margin-top: 5px;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table_form2">
                    <tr>
                        <td width="10%">
                        </td>
                        <td align="right" width="90px">
                            <b>Trạng thái : </b>
                        </td>
                        <td width="100px">
                            <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                        </td>
                         <td class="td_form2_td1"  width="120px">
                            <div>
                               <b> In theo nội dung:</b></div>
                        </td>
                        <td width="250px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNoiDung, sNoiDung, "NoiDung", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td align="right" width="50px">
                            <b>Ngày : </b>
                        </td>
                        <td width="80px" id="<%= ParentID %>_ngay1">
                            <% rptKTTK_ChiTietTamThuController rpt = new rptKTTK_ChiTietTamThuController();%>
                            <%= rpt.get_sNgayThang(ParentID, MaND,iThang, iNgay)
                            %>
                        </td>
                        <td align="right" width="50px">
                            <b>Tháng : </b>
                        </td>
                        <td width="50px">
                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonThang()\" ")%>
                        </td>
                        <td align="right" width="50px">
                            <b>Năm : </b>
                        </td>
                        <td width="50px">
                            <%=MyHtmlHelper.TextBox(ParentID, iNamLamViec, "", "", "class=\"input1_2\" style=\"width: 100%; \" disabled=\"disabled\"")%>
                        </td>
                        <td align="right" width="100px">
                            <b>Đơn vị tính : </b>
                        </td>
                        <td width="150px">
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, DonViTinh, "DonViTinh", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="12" align="center">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick=" Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTK_ChiTietTamThu", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iNgay = iNgay, iThang = iThang, DonViTinh = DonViTinh, NoiDung = sNoiDung }), "Xuất ra file Excel")%>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
    </script>
    <script type="text/javascript">
        function ChonThang() {
            var iThang = document.getElementById("<%=ParentID %>_iThang").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&MaND=#1&iThang=#2&iNgay=#3","rptKTTK_ChiTietTamThu") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= MaND %>"));
            url = unescape(url.replace("#2", iThang));
            url = unescape(url.replace("#3", "<%= iNgay %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID%>_ngay1").innerHTML = data;
            });
        }                                            
    </script>
    <%} %>
    <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
