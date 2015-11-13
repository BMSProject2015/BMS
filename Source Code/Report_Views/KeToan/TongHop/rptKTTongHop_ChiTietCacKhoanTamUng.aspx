<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
        String ParentID = "KeToan";
        String MaND = User.Identity.Name;
        String iNamLamViec = DateTime.Now.Year.ToString();
        String iThangLamViec = "1";
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        {
            iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            iThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
        }

        String pageload = Convert.ToString(ViewData["pageload"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        String BQL = Convert.ToString(ViewData["BQL"]);
        String DonViTinh = Convert.ToString(ViewData["DonViTinh"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
        DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(3, "Tất cả");
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
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
     
        dtNgay.Dispose();
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

        var tbl = rptKTTongHop_ChiTietCacKhoanTamUngController.DT_DS_BQL(true, "Tất cả", User.Identity.Name, false);
        SelectOptionList slBQL = new SelectOptionList(tbl, "sKyHieu", "sTen");
        if (tbl != null) tbl.Dispose();
        String urlReport = pageload.Equals("1")
                               ? Url.Action("ViewPDF", "rptKTTongHop_ChiTietCacKhoanTamUng",
                                            new
                                                {
                                                    MaND = MaND,
                                                    iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,
                                                    iNgay = iNgay,
                                                    iThang = iThang,
                                                    DonViTinh = DonViTinh,
                                                    BQL = BQL
                                                })
                               : "";

        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        using (Html.BeginForm("EditSubmit", "rptKTTongHop_ChiTietCacKhoanTamUng", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo chi tiết các khoản tạm ứng (Tài khoản 312)</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
            <div id="Div2" style="margin-top: 5px;">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="15%">
                        </td>
                        <td class="td_form2_td1"  width="150px">
                            <div>
                               <b> Trạng thái:</b></div>
                        </td>
                        <td width="150px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1"  width="110px">
                            <div>
                               <b> Chọn BQL:</b></div>
                        </td>
                        <td width="150px">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slBQL, BQL, "BQL", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="50px">
                            <div>
                                <b>Ngày:</b></div>
                        </td>
                        <td width="50px" id="<%= ParentID %>_ngay1">
                            <% rptKTTongHop_ChiTietCacKhoanTamUngController rpt = new rptKTTongHop_ChiTietCacKhoanTamUngController();%>
                            <%= rpt.get_sNgayThang(ParentID, MaND,iThang, iNgay)
                            %>
                        </td>
                        <td class="td_form2_td1" width="50px">
                            <div>
                              <b>  Tháng:</b></div>
                        </td>
                        <td width="5%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonThang()\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="50px">
                            <div>
                              <b> Năm:</b></div>
                        </td>
                        <td width="70px">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iNamLamViec, "", "", "class=\"input1_2\" style=\"width: 70px; \" disabled=\"disabled\"")%>
                            </div>
                        </td>
                        <td align="right" width="110px">
                            <b>Đơn vị tính : </b>
                        </td>
                        <td width="10%">
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, DonViTinh, "DonViTinh", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                        </td>
                        <td width="15%">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="10">
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
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTongHop_ChiTietCacKhoanTamUng", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iNgay = iNgay, iThang = iThang, DonViTinh = DonViTinh, BQL = BQL }), "Xuất ra file Excel")%>
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
            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&MaND=#1&iThang=#2&iNgay=#3","rptKTTongHop_ChiTietCacKhoanTamUng") %>');
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
