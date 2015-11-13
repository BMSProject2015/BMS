<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.NhanSu" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <% 
        String ParentID = "NhanSu_DanhSach";
        DataTable dtNam = DanhMucModels.DT_Nam(true,"--Tất cả--");
        String Nam = Convert.ToString(ViewData["Nam"]);
        if (String.IsNullOrEmpty(Nam))
        {
            Nam = DateTime.Now.Year.ToString();
        }
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
        {
                URLView = Url.Action("ViewPDF", "rptNhanSu_DanhSach", new { Nam = Nam });
        }
        String urlExport = Url.Action("ExportToExcel", "rptNhanSu_DanhSach", new { Nam = Nam });
        using (Html.BeginForm("EditSubmit", "rptNhanSu_DanhSach", new { ParentID = ParentID, Nam = Nam }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, iID_MaDonVi, "iID_MaDonVi", "")%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách cán bộ</span>
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
                        <td class="td_form2_td1" style="width: 45%;">
                            <div>
                                <%=NgonNgu.LayXau("Tới năm báo cáo:")%></div>
                        </td>
                        <td width="50%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "Nam", "", "class=\"input1_2\" style=\"width:25%;\" ")%>
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" name="submitButton" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="1%">
                                    </td>
                                    <td style="width: 50%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
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
