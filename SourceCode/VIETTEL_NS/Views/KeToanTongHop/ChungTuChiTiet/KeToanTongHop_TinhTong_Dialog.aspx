<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=ConfigurationManager.AppSettings["TitleView"]%></title>
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/css/style.css") %>" />

    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/Themes/custom-theme/redmond/jquery-ui-1.8.2.custom.css") %>" />
    <script src="<%= Url.Content("~/Scripts/jsFunctions.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>"
        type="text/javascript"></script>
</head>
<body>
    <%
      
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Create";

        DataTable dt = new DataTable();
        Boolean isNo = true;
        String sSoChungTu = KeToanTongHop_ChungTuModels.TinhTong_HienThiTaiKhoan(iID_MaChungTu, ref dt, ref isNo);
        string strNoiDung = KeToanTongHop_ChungTuModels.getSoChungTuGhiSo(iID_MaChungTu);
        
    %>
    <div style="margin: 5px 0px 0px 0px; text-align:center; color:Navy;">
        <b>
             <%=MyHtmlHelper.Label(strNoiDung, "")%></b>
    </div>
    <div style="margin: 5px 0px 10px 0px;">
        <b>
            <%=MyHtmlHelper.Label(sSoChungTu,"")%></b>
    </div>
    <div style="height: 10px;">
        &nbsp;</div>
    <div style="width: 99%;">
        <table class="mGrid" width="100%" cellpadding="3" cellspacing="3" border="0">
            <tr >
                <th style="width: 50px;" align="center" >
                  <div style="font-size: 14px;">
                    STT</div>  
                </th>
                <th style="width: 120px;" align="center">
                  <div style="font-size: 14px;">
                    <% if (isNo == true)
                       {%>Tài khoản có
                    <% }
                       else
                       { %>Tài khoản nợ
                    <% }
                    %></div>  
                </th>
                <th align="center">
              <div style="font-size: 14px;">    Số tiền</div>  
                </th>
            </tr>
            <%
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    String classtr = "";
                    int STT = i + 1;
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    Double rSoTien = Convert.ToDouble(R["rSoTien"]);
                    string SoTien = "";
                    if (rSoTien < 0)
                    {
                        SoTien = "-" + CommonFunction.DinhDangSo(rSoTien);
                    }
                    else
                    {
                        SoTien = CommonFunction.DinhDangSo(rSoTien);
                    }
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(R["iID_MaTaiKhoan"])%>
                </td>
                <td align="right">
                    <%=SoTien%>
                </td>
            </tr>
            <%}
                if (dt!=null)
                {
                    dt.Dispose();
                }
               %>
        </table>
    </div>
</body>
</html>
