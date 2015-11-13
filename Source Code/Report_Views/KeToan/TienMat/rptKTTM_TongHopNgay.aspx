<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "QuyetToanNganSach";
       
        String iThang = Request.QueryString["iThang"];

        String iNam = Request.QueryString["iNam"];
      
        String UserID = User.Identity.Name;


        String TuNgay = "1";
        String DenNgay = HamChung.GetDaysInMonth(Convert.ToInt32(iThang), Convert.ToInt32(iNam)).ToString();
        String TuThang = iThang;
        String DenThang = iThang;
        String LoaiBaoCao = "PC3";
        //tháng     
        var dtThang = HamChung.getMonth(DateTime.Now, false, "", "");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        ///ngày
        var dtNgay = HamChung.getDaysInMonths(Convert.ToInt32(iThang), Convert.ToInt32(iNam), false, "", "");
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        if (dtNgay != null) dtNgay.Dispose();
        using (Html.BeginForm("EditSubmit", "rptKTTM_TongHopNgay", new { ParentID = ParentID, iNamLamViec = iNam }))
        {
    %>
    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
        <tr>
            <td style="padding-top: 10px;">
                <fieldset style="border-top-right-radius: 5px; border-top-left-radius: 5px; -moz-border-radius-topright: 5px;
                    -moz-border-radius-topleft: 5px; border: 1px solid #dedede; margin-left: 5px;
                    margin-right: 5px;">
                    <legend style="font-size: 14px; font-family: Tahoma Arial; padding-left: 10px;">Từ ngày&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đến
                        ngày&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tháng</legend>
                    <div style="padding: 10px; float: left;">
                        <%=MyHtmlHelper.DropDownList(ParentID, slNgay, TuNgay, "iNgay1", "", "class=\"input1_2\"style=\"width: 60px;\"")%>&nbsp;&nbsp;&nbsp;
                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, TuThang, "iThang1", "", "class=\"input1_2\" style=\"width: 60px;\"")%>&nbsp;&nbsp;&nbsp;
                        <%=MyHtmlHelper.DropDownList(ParentID, slNgay, DenNgay, "iNgay2", "", "class=\"input1_2\" style=\"width: 60px;\"")%>&nbsp;&nbsp;&nbsp;
                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, DenThang, "iThang2", "", "class=\"input1_2\" style=\"width: 60px;\"")%>
                    </div>
                </fieldset>
            </td>
        </tr>
        <!------------------da sua--------------------->
        <tr>
            <td style="text-align: left;">
                <fieldset style="border-top-right-radius: 5px; border-top-left-radius: 5px; -moz-border-radius-topright: 5px;
                    -moz-border-radius-topleft: 5px; border: 1px solid #dedede; margin-left: 5px;
                    margin-right: 5px;">
                    <legend style="font-size: 14px; font-family: Tahoma Arial; padding-left: 10px;">
                        <%=NgonNgu.LayXau("Phương án in")%>
                    </legend>
                    <p style="padding: 4px 4px 4px 100px;">
                        <%=MyHtmlHelper.Option(ParentID, "PC1", LoaiBaoCao, "LoaiBaoCao", "")%>
                        <b>1. Tổng hợp Thu - Chi</b>
                    </p>
                    <p style="padding: 4px 4px 4px 100px;">
                        <%=MyHtmlHelper.Option(ParentID, "PC2", LoaiBaoCao, "LoaiBaoCao", "")%>
                        <b>2. Phiếu thu</b>
                    </p>
                    <p style="padding: 4px 4px 4px 100px;">
                        <%=MyHtmlHelper.Option(ParentID, "PC3", LoaiBaoCao, "LoaiBaoCao", "")%>
                        <b>3. Phiếu chi</b>
                    </p>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td style="height: 10px;">
            </td>
        </tr>
        <tr align="center" style="text-align: center;">
            <td style="text-align: center;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 10%">
                        </td>
                        <td align="right">
                            <input type="submit" class="button4" value="Tiếp tục" />
                        </td>
                        <td style="width: 1%">
                            &nbsp;
                        </td>
                        <td align="left">
                            <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                        </td>
                        <td style="width: 10%">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%} %>
</body>
</html>
