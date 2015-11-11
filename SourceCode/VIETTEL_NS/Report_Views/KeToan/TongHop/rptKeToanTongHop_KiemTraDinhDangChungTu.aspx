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
    <link rel="Stylesheet" type="text/css" href="../../../Content/Report_Style/ReportCSS.css" />
    <style type="text/css">
         ul.inlineBlock li fieldset legend{text-align:left; padding:3px 6px;font-size:13px;}  
        ul.inlineBlock li fieldset p{padding:2px 4px; text-align:left;}
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <% 
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "KeToan";
        String Nam = DateTime.Now.Year.ToString();
        String TuNgay = Convert.ToString(ViewData["TuNgay"]);
        String DenNgay = Convert.ToString(ViewData["DenNgay"]);
        String TuThang = Convert.ToString(ViewData["TuThang"]);
        String DenThang = Convert.ToString(ViewData["DenThang"]);

        String iNamLamViec = Convert.ToString(DateTime.Now.Year);
        String iTuThang = DateTime.Now.Month.ToString();
        String iDenThang = DateTime.Now.Month.ToString();
        //lay nam lam viec
        var objNguoidung = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
        if (objNguoidung.Rows.Count > 0)
        {
            DataRow dr = objNguoidung.Rows[0];
            iTuThang = iDenThang = HamChung.ConvertToString(dr["iThangLamViec"]);
            iNamLamViec = HamChung.ConvertToString(dr["iNamLamViec"]);
        }
        //ngay       
        if (String.IsNullOrEmpty(TuNgay) == true)
        {
            TuNgay = "1";
        }
        if (String.IsNullOrEmpty(TuThang) == true || TuThang == "")
        {
            TuThang = iTuThang;
        }
        if (String.IsNullOrEmpty(DenThang) == true)
        {
            //DenThang = TuThang;
            DenThang = iDenThang;
        }
        if (String.IsNullOrEmpty(DenNgay) == true)
        {
            DenNgay = HamChung.GetDaysInMonth(Convert.ToInt32(DenThang), Convert.ToInt32(Nam)).ToString();
        }
        //tháng     
        var dtThang = HamChung.getMonth(DateTime.Now, false, "", "");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        ///ngày
        var dtNgay = HamChung.getDaysInMonths(DateTime.Now.Month, DateTime.Now.Year, false, "", "");
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        if (dtNgay != null) dtNgay.Dispose();
        String BackURL = Url.Action("Index", "KeToan_Report");
        //Kieu giay
        String Loai = Convert.ToString(ViewData["Loai"]);
        DataTable dtKieuGiay = VIETTEL.Report_Controllers.KeToan.TongHop.rptKeToanTongHop_KiemTraChungTuController.dtLoaiBangKe();
        SelectOptionList slKieuGiay = new SelectOptionList(dtKieuGiay, "MaBangKe", "TenBangKe");
        if (String.IsNullOrEmpty(Loai))
            Loai = "0";
        dtKieuGiay.Dispose();
        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String iTrangThai = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iTrangThai))
            iTrangThai = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();
        String urlReport = "";
        //String urlHuy = Url.Action("Index", "rptKeToanTongHop_PhanHo");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptKeToanTongHop_KiemTraChungTu", new { iNamLamViec = iNamLamViec, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang, Loai = Loai, iTrangThai = iTrangThai });
        
        using (Html.BeginForm("EditSubmit", "rptKeToanTongHop_KiemTraChungTu", new { ParentID = ParentID, iNamLamViec = iNamLamViec }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo tổng hợp kiểm tra định dạng chứng từ</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width: 100%; margin: 0px auto; padding: 0px 0px; overflow: visible; text-align:center;">                
                <ul class="inlineBlock">
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Khoảng thời gian cần xem") %></legend>
                            <p><%=NgonNgu.LayXau("Từ ngày&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đến ngày&nbsp;&nbsp;&nbsp;&nbsp;Tháng")%></p>
                            <p style="margin-bottom:10px;">
                                <%=MyHtmlHelper.DropDownList(ParentID, slNgay, TuNgay, "TuNgay", "", "class=\"input1_2\"style=\"width: 60px;\"")%>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, TuThang, "TuThang", "", "class=\"input1_2\" style=\"width: 60px;\"")%>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNgay, DenNgay, "DenNgay", "", "class=\"input1_2\" style=\"width: 60px;\"")%>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, DenThang, "DenThang", "", "class=\"input1_2\" style=\"width: 60px;\"")%>
                            </p>
                        </fieldset>
                    </li>
                     <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Loại bảng kê:") %></legend>
                            <p>
                                <%=MyHtmlHelper.DropDownList(ParentID, slKieuGiay, Loai, "Loai", "", "class=\"input1_2\" style=\"width: 200px;\" size='3' tab-index='-1'")%>
                            </p>
                        </fieldset>
                    </li>
                     <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Chọn trạng thái:") %></legend>
                            <p>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 150px;\" size='3' tab-index='-1'")%>
                            </p>
                        </fieldset>
                    </li>
                </ul>
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
            </div>
        </div>
    </div>
    <%} %>
    <script type="text/javascript">
        $(function () {           
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKeToanTongHop_KiemTraChungTu", new { iNamLamViec = iNamLamViec, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang, Loai = Loai, iTrangThai = iTrangThai }), "Export To Excel")%>
    <iframe src="<%=urlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
