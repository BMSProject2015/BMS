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
        String ParentID = "DuToan";
        String iID_MaChungTu = Convert.ToString(Request.QueryString["iID_MaChungTu"]);
        String sLNS = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu", "iID_MaChungTu", iID_MaChungTu, "sDSLNS"));
        DataTable dtTuyChinh = QuyetToanModels.getDanhSachTuyChinh_DuToan(sLNS);
        SelectOptionList slTuyChinh = new SelectOptionList(dtTuyChinh, "MaLoai", "sTen");     
        dtTuyChinh.Dispose();
        String MaLoai = "";
         if (String.IsNullOrEmpty(MaLoai)) MaLoai = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu", "iID_MaChungTu", iID_MaChungTu, "MaLoai"));
        using (Html.BeginForm("ChungTuChiTiet_Frame", "DuToan_PhanCapChungTuChiTiet", new { ParentID = ParentID }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTu,"iID_MaChungTu","") %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 20%" class="td_form2_td1"></td>
               <td class="td_form2_td1" style="width: 20%" >
                            <div> Chọn tùy chinh:</div> 
                        </td>
            <td class="td_form2_td1" style="width: 40%">
             <%=MyHtmlHelper.DropDownList(ParentID,slTuyChinh,MaLoai,"MaLoai","","class=\"input1_2\" style=\"width: 100%\"") %>
            </td>
            <td class="td_form2_td1"></td>
        </tr>
                           
        <tr>
            <td colspan="4">
                <div style="margin-top: 10px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 40%">
                            </td>
                            <td align="right">
                                <input type="submit" class="button" value="Tiếp tục" />
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
