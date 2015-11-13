<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
 <%
     String ParentID = "Edit";
     DataTable dt = rptThongBaoCapChiTieuNganSachNamController.LayDongNoiDung();
     using (Html.BeginForm("DongNoiDung", "rptThongBaoCapChiTieuNganSachNam", new { ParentID = ParentID }))
         {
    %>
    <div class="box_tong" style="background-color:#F0F9FE;">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Nhập nội dung </span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1" >
            <div id="Div2" style="margin-top:5px;">                 
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 176px">
  <% for (int i = 0; i < dt.Rows.Count; i++)
     {
         %>
         <tr>
    <td align="right">Nội dung dòng <%= i + 1 %> :</td>
    <td>&nbsp;</td>
    <td><%=MyHtmlHelper.TextBox("", dt.Rows[i]["sThamSo"],Convert.ToString( dt.Rows[i]["sNoiDung"]), "", "style=\"width:80%;height:23px;\"")%></td>
  </tr>
         <%} %>
  <tr>
    <td colspan="3" align="center"><table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button4" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table></td>
  </tr>
</table>

            </div>
        </div>
    </div>
    <%} %>