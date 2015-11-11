<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 423px;
        }
    </style>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "CanDoiThuChiTaiChinh";
        String MaND = User.Identity.Name;
        String iNamLamViec = DateTime.Now.Year.ToString();
        String iThangLamViec = DateTime.Now.Month.ToString();
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        {
            iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            iThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
        }

        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        String DonViTinh = Convert.ToString(ViewData["DonViTinh"]);
        String pageload = Convert.ToString(ViewData["PageLoad"]);
        String iQuy = Convert.ToString(ViewData["iQuy"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "1";
        }

        DataTable dtTrangThai = rptKeToanTongHop_CanDoiThuChiTaiChinhController.tbTrangThai();
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
        if (dtTrangThai != null)
        {
            dtTrangThai.Dispose();
        }
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
        if (dtDonViTinh != null)
        {
            dtDonViTinh.Dispose();
        }
        String iNgay = Convert.ToString(ViewData["iNgay"]);
        String iThang = Convert.ToString(ViewData["iThang"]);
        //if (String.IsNullOrEmpty(iNgay))
        //    iNgay = HamChung.GetDaysInMonth(Convert.ToInt32(iThangLamViec), Convert.ToInt32(iNamLamViec)).ToString();
        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null)
        {
            dtThang.Dispose();
        }
        if (String.IsNullOrEmpty(iThang))
            iThang = iThangLamViec;
        DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNamLamViec));
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        if (String.IsNullOrEmpty(iNgay))
        {
            iNgay = Convert.ToString(dtNgay.Rows[dtNgay.Rows.Count - 1]["MaNgay"]);
        }
        if (dtNgay != null)
        {
            dtNgay.Dispose();
        }
        DataTable dtQuy = DanhMucModels.DT_Thang(false);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaThang", "TenThang");
        dtQuy.Dispose();
        if (String.IsNullOrEmpty(iQuy)) iQuy = Request.QueryString["iQuy"];
        if (String.IsNullOrEmpty(iQuy))
        {
            iQuy = Convert.ToString(iThangLamViec);
            //int iThangLV = Convert.ToInt32(iThangLamViec);
            // if (iThangLV > 3 && iThangLV <= 6)
            //{
            //    iQuy = "2";
            //}
            // else if (iThangLV > 6 && iThangLV <= 9)
            //{
            //    iQuy = "3";
            //}
            // else if (iThangLV > 9 && iThangLV <= 12)
            //{
            //    iQuy = "4";
            //}
            //else
            //{
            //    iQuy = "1";
            //}
        }
        String strQuayLai = Url.Action("SoDoLuong", "KeToanTongHop");
        String urlReport = pageload.Equals("1") ? Url.Action("ViewPDF", "rptKeToanTongHop_CanDoiThuChiTaiChinh", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iNgay = iNgay, iThang = iThang, DonViTinh = DonViTinh, iQuy=iQuy }) : "";
        using (Html.BeginForm("EditSubmit", "rptKeToanTongHop_CanDoiThuChiTaiChinh", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Cân đối thu chi tài chính</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
            <div id="Div2" style="margin-top: 5px;">
                <table width="100%" border="0" class="table_form2">
                    <tr>
                        <td width="20%">
                        </td>
                        <td width="100px" align="right">
                            <b>Trạng thái:</b>
                        </td>
                        <td width="100px">
                            <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\"")%>
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
                        <td width="70px">
                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonThang(this.value)\" ")%>
                        </td>
                        <td align="right" width="50px">
                            <b>Năm : </b>
                        </td>
                        <td width="50px">
                            <%=MyHtmlHelper.TextBox(ParentID, iNamLamViec, "", "", "class=\"input1_2\" style=\"width: 100%; \" disabled=\"disabled\"")%>
                        </td>
                        <td align="right" width="150px">
                            <b>Tháng in dữ liệu: </b>
                        </td>
                        <td width="80px">
                          <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Convert.ToString(iQuy), "iQuy", "", " style=\"width:80px;\"")%>
                        </td>
                        <td align="right" width="120px">
                            <b>Đơn vị tính : </b>
                        </td>
                        <td width="10%">
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, DonViTinh, "DonViTinh", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                        </td>
                        <td width="20%">
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
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="javascript:location.href='<%=strQuayLai%>'" />
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
    <%}
        
    %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKeToanTongHop_CanDoiThuChiTaiChinh", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iNgay = iNgay, iThang = iThang, DonViTinh = DonViTinh, iQuy = iQuy }), "Export to Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
    <script type="text/javascript">
        function ChonThang(value) {
            
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&MaND=#1&iThang=#2&iNgay=#3","rptKTTK_ChiTietTamThu") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= MaND %>"));
            url = unescape(url.replace("#2", value));
            url = unescape(url.replace("#3", "<%= iNgay %>"));
          
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID%>_ngay1").innerHTML = data;
            });
        }              
    
    </script>
</body>
</html>
