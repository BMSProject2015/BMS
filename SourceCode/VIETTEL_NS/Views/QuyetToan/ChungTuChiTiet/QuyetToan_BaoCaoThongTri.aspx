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
        String ParentID = "QuyetToan";
        String iID_MaChungTu = Convert.ToString(Request.QueryString["iID_MaChungTu"]);
        DataTable dt = DanhMucModels.DT_DanhMuc_All("TenThongTriQuyetToan");
        SelectOptionList slThongTri = new SelectOptionList(dt, "iID_MaDanhMuc", "sTen");

        String iID_MaDanhMuc = "";
       
        String sGhiChu = rptQuyetToan_ThongTri_ChungTuController.DsGhiChu(iID_MaChungTu);
        String[] arrGhiChu = sGhiChu.Split(',');
        sGhiChu = arrGhiChu[0];
        iID_MaDanhMuc = arrGhiChu[1];
        if (String.IsNullOrEmpty(iID_MaDanhMuc))
        {
            if (dt.Rows.Count > 0)
            {
                iID_MaDanhMuc = dt.Rows[0]["iID_MaDanhMuc"].ToString();
            }
        }
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_ThongTri_ChungTu", FormMethod.Post, new { target = "_blank" }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTu,"iID_MaChungTu","") %>
     <div class="box_tong">
   <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
        <tr>
            <td colspan="4">
                <div style="margin-top: 10px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="td_form2_td1" align="right" style="width:20%">
                                <div>
                                    <%=NgonNgu.LayXau("Tên thông tri:")%></div>
                            </td>
                            <td align="left">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slThongTri, iID_MaDanhMuc, "iLoaiGhiChu", "", "class=\"input1_2\" style=\"width: 100%;\" size='8' ")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                              <td class="td_form2_td1" align="right">
                                <div>
                                    <%=NgonNgu.LayXau("Ghi chú:")%></div>
                            </td>
                            <td align="left">
                                <div>
                                    <%=MyHtmlHelper.TextArea(ParentID, sGhiChu, "sGhiChu", "", "class=\"input1_2\" style=\"width: 100%; height:300px\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:center">
                                <input type="submit" class="button" value="In thông tri" style="display:inline" />
                                <input type="button" class="button" value="Hủy" style="display:inline" onclick="Dialog_close('<%=ParentID %>');" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    </div>
    <%} %>
</body>
</html>
