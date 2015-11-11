<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        .style1
        {
            width: 139px;
        }
        .style2
        {
        }
    </style>
</head>
<body>
    <%
        String ParentID = "QuyetToan";

        String BackURL = Url.Action("Index", "QuyetToan_QuanSo_Report");
        String URLView = "";
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        String iThang1 = Convert.ToString(ViewData["iThang1"]);
        String iThang2 = Convert.ToString(ViewData["iThang2"]);
        String iThang3 = Convert.ToString(ViewData["iThang3"]);
        String iThang4 = Convert.ToString(ViewData["iThang4"]);
        String bQuy = Convert.ToString(ViewData["bQuy"]);
        DataTable dtThang = new DataTable();
        dtThang.Columns.Add("Id", typeof(string));
        dtThang.Columns.Add("Text", typeof(string));
        for (int i = 0; i <= 12; i++)
        {
            dtThang.Rows.Add(i.ToString(), i == -1 ? "" : i.ToString());
        }
        SelectOptionList slThang = new SelectOptionList(dtThang, "Id", "Text");
        dtThang.Dispose();

        int SoCot = 1;
        //dt Loại ngân sách
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan_QuanSo(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();

        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";
        if (PageLoad == "1")
        {
            URLView = Url.Action("ViewPDF", "rptQTQS_ThuongXuyen",
                                     new
                                         {
                                             MaND = MaND,
                                             iID_MaPhongBan = iID_MaPhongBan,
                                             iThang1 = iThang1,
                                             iThang2 = iThang2,
                                             iThang3 = iThang3,
                                             iThang4 = iThang4,
                                             bQuy=bQuy
                                         });

        }
        using (Html.BeginForm("EditSubmit", "rptQTQS_ThuongXuyen", new { ParentID = ParentID }))
        {
    %>
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Báo cáo tổng hợp quân số năm
                        <%=iNamLamViec %>
                    </span>
                </td>
            </tr>
        </table>
    </div>
    <table style="width: 50%; margin: 0 auto;">
        <tr>
            <td class="style1">
                &nbsp; Chọn phòng ban
            </td>
            <td class="style2" colspan="6">
                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 80%;height:24px;\"")%>
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp; Chọn các tháng/quý
            </td>
            <td class="style2">
                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang1, "iThang1", "", "style='width:80%'")%>
            </td>
            <td>
                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang2, "iThang2", "", "style='width:80%'")%>
            </td>
            <td>
                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang3, "iThang3", "", "style='width:80%'")%>
            </td>
            <td>
                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang4, "iThang4", "", "style='width:80%'")%>
            </td>
              <td style="text-align:right">
              Quý:
            </td>
            <td>
                <%=MyHtmlHelper.CheckBox(ParentID, bQuy, "bQuy", "", "style='width:80%'")%>
            </td>
        </tr>
        <tr>
            
            <td class="style2" colspan="7">
                <table cellpadding="0" cellspacing="0" border="0" style="margin-left: 45%;">
                    <tr>
                        <td>
                            <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%} %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQTQS_ThuongXuyen", new
                                                                                            {
                                                                                                MaND = MaND,
                                                                                                iID_MaPhongBan = iID_MaPhongBan,
                                                                                                iThang1 = iThang1,
                                                                                                iThang2 = iThang2,
                                                                                                iThang3 = iThang3,
                                                                                                iThang4 = iThang4,bQuy=bQuy
                                                                                            }), "Export to Excel")%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
<script type="text/javascript">
    Chon();
    $(document).ready(function () {
        $('.title_tong a').click(function () {
            $('div#rptMain').slideToggle('normal');
            $(this).toggleClass('active');
            return false;
        });

    });
                          
</script>
