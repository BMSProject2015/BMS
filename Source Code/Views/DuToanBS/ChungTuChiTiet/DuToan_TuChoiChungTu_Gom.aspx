<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body>
    <%
        string ParentID = "DuToan";
        string iID_MaChungTu = Convert.ToString(Request.QueryString["iID_MaChungTu"]);
        string maChungTuTLTHCuc = Convert.ToString(Request.QueryString["iID_MaChungTu_TLTHCuc"]);
        string sLNS = Convert.ToString(Request.QueryString["sLNS"]);
        string slyDo = "";
        string idAction = "1";
        
        using (Html.BeginForm("TuChoiChungTuTLTH", "DuToanBSChungTu", new { ParentID = ParentID, maChungTuTLTH = iID_MaChungTu, sLNS = sLNS, iLoai = 1, maChungTuTLTHCuc = maChungTuTLTHCuc }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, idAction, "idAction", "")%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%" class="td_form2_td1"></td>
                <td class="td_form2_td1" style="width: 20%" >
                    <div><b> Lý do: </b></div> 
                </td>
            <td class="td_form2_td1" style="width: 40%">
             <%=MyHtmlHelper.TextArea(ParentID, slyDo, "sLyDo", "", "class=\"input1_2\" style=\"width: 100%\"")%>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <div style="margin-top: 10px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 40%">
                            </td>
                            <td align="right">
                                <input type="submit" class="button"  value="Tiếp tục" id="btnDuyet" />
                            </td>
                            <td style="width: 1%">
                                &nbsp;
                            </td>
                            <td align="left">
                                <input type="button" class="button" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                            </td>
                            <td style="width: 40%">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <%} %>
</body>
</html>
