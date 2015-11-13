<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=ConfigurationManager.AppSettings["TitleView"]%></title>
</head>
<body>
    <%
        int i;
        String ParentID = "VayNo_Duyet";
        String MaChungTu = Request.QueryString["iID_MaChungTu"];
        DataTable dt = KTCT_TienMat_ChungTuModels.getLichSuChungTu(MaChungTu);
    %>
    <div class="box_tong">
        <table class="mGrid" id="<%= ParentID %>_thList">
            <tr>
                <th style="width: 5%;" align="center">
                    STT
                </th>
                <th style="width: 15%;" align="center">
                    Trạng thái chứng từ
                </th>
                <th style="width: 15%;" align="center">
                    Người thực hiện
                </th>
                <th style="width: 12%;" align="center">
                    Ngày tạo
                </th>
                <th style="width: 53%;" align="center">
                    Nhận xét
                </th>
            </tr>
            <%
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    String classtr = "";
                    int STT = i + 1;
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(Convert.ToString(R["sNoiDung"]))%>
                </td>
                <td align="left">
                    <%= Convert.ToString(R["sID_MaNguoiDungTao"])%>
                </td>
                <td align="center">
                    <%= HamChung.getStringNull(HamChung.ConvertDateTime(R["dNgayTao"]).ToString("dd/MM/yyyy HH:mm:ss"))%>
                </td>
                <td align="left">
                    <%= Convert.ToString(R["sSua"])%>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="5" align="right">
                </td>
            </tr>
            <tr>
                <td colspan="5" style="padding-top: 10px;" align="center">
                    <input type="button" class="button4" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                </td>
            </tr>
        </table>
    </div>
    
    <%
        if (dt != null) dt.Dispose();
    %>
</body>
</html>
