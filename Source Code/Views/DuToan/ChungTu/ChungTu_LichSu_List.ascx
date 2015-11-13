<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<%
    int i;
    String ParentID = "DuToan_ChungTu";
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];

    DataTable dt = DuToan_DuyetChungTuModels.GetDuyetChungTu(iID_MaChungTu);
%>
<div class="box_tong">
    <table class="mGrid" id="<%= ParentID %>_thList">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 15%;" align="center">Trạng thái</th>
            <th style="width: 15%;" align="center">Người thực hiện</th>
            <th style="width: 12%;" align="center">Ngày tạo</th>
            <th style="width: 53%;" align="center">Nhận xét</th>
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
                <td align="center"><%=STT%></td>            
                <td align="center"><%=R["sNoiDung"]%></td>
                <td align="center"><%=R["sID_MaNguoiDungTao"]%></td>
                <td align="center"><%=R["dNgayDuyet"]%></td>
                <td align="center"></td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="5" align="right">
            </td>
        </tr>
    </table>
</div>
<%
    dt.Dispose();
 %>