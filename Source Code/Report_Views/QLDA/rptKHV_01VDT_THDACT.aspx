<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QLDA" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <% 
        String ParentID = "KHV_Bieu01_THDACT";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        DataTable dtNgoaiTe = DanhMucModels.QLDA_NgoaiTe(true,"VNĐ");
        String NgoaiTe = Convert.ToString(ViewData["NgoaiTe"]);
        if (String.IsNullOrEmpty(NgoaiTe))
        {
            NgoaiTe = "0";
        }
        SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTen");
        dtNgoaiTe.Dispose();

        String dDenNgay = Convert.ToString(ViewData["dDenNgay"]);
        if (dDenNgay == "")
            dDenNgay = DateTime.Now.ToShortDateString();
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptKHV_01VDT_THDACT", new { NgoaiTe = NgoaiTe, vidDenNgay = dDenNgay });
        String URL = Url.Action("Index", "QLDA_Report");
        String urlExport = Url.Action("ExportToExcel", "rptKHV_01VDT_THDACT", new { NgoaiTe = NgoaiTe, vidDenNgay = dDenNgay });
        using (Html.BeginForm("EditSubmit", "rptKHV_01VDT_THDACT", new { ParentID = ParentID, vidDenNgay = dDenNgay }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo kế hoạch vốn đầu tư</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td>
                        </td>
                        
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                            </div>
                        </td>
                        <td width="20%" style="vertical-align: middle;">
                            <div>
                               
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("Ngoại tệ:")%></div>
                        </td>
                        <td width="20%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, NgoaiTe, "NgoaiTe", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("Đến ngày:")%></div>
                        </td>
                        <td width="20%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, dDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 5%;">
                            <div>
                                
                            </div>
                        </td>
                        <td width="15%" style="vertical-align: middle;">
                            <div>
                                
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="8">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 47%;" align="right">
                                        <input type="submit" class="button" name="submitButton" id="Submit1" value="<%=NgonNgu.LayXau("Danh sách")%>" />
                                    </td>
                                    <td width="1%">
                                    </td>
                                    <td style="width: 48%;" align="left">
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
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    <%} %>
    <iframe src="<%=URLView %>" height="600px" width="100%"></iframe>
</body>
</html>
  <script type="text/javascript">
      function Huy() {
          window.location.href = '<%=URL %>';
      }
    </script>