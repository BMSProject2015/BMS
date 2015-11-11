<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=ConfigurationManager.AppSettings["TitleView"]%></title>
</head>
<body>
    <%
        String MaDiv = Request.QueryString["idDiv"];
        String MaDivDate = Request.QueryString["idDivDate"];
        String ChiNganSach = Request.QueryString["ChiNganSach"];
        String MaDotNganSach = Request.QueryString["MaDotNganSach"];
        String sLNS = Request.QueryString["sLNS"];
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";
        DataTable dt = null ;

        using (Ajax.BeginForm("Edit_Fast_Submit", "DuToan_ChungTu", new { ParentID = ParentID, OnSuccess = OnSuccess, MaDotNganSach= MaDotNganSach, ChiNganSach = ChiNganSach, sLNS = sLNS, MaDiv = MaDiv, MaDivDate = MaDivDate }, new AjaxOptions { }))
        {
    %>
    <div style="background-color: #f0f9fe; background-repeat: repeat; border: solid 1px #ec3237">
        <div style="padding: 10px;">
            <div id="nhapform">
                <div id="form2">
                    <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                        <tr>
                            <td class="td_form2_td1" style="width: 20%;">
                                <div>Ngày chứng từ</div>
                            </td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DatePicker(ParentID, "", "dNgayChungTu", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>Nội dung</div>
                            </td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextArea(ParentID, "", "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div>
                            </td>
                        </tr>
                        <tr><td style="height: 10px; font-size: 5px;" colspan="2">&nbsp;</td></tr>
                        <tr>
                            <td align="right" colspan="2">
                                <input type="submit" class="button4" value="Thêm mới" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%
        } if (dt != null) { dt.Dispose(); };
    %>
</body>
</html>
