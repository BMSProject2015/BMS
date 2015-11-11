<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
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
        String ParentID = "DoanhNghiep";
        String iID_MaDoanhNghiep = Convert.ToString(Request.QueryString["iID_MaDoanhNghiep"]);
        String iID_MaDuAn = Convert.ToString(Request.QueryString["iID_MaDuAn"]);
        if (String.IsNullOrEmpty(iID_MaDuAn)) iID_MaDuAn = Guid.Empty.ToString();
        String sTenDuAn = "";

        DataTable dtDuAn = TCDNModels.getChiTietdtDuAn(iID_MaDuAn);
        if(dtDuAn.Rows.Count>0)
        {
            sTenDuAn =Convert.ToString(dtDuAn.Rows[0]["sTenDuAn"]);
        }
            
        
        String bHoanThanh = "";
       
        String DuLieuMoi = "";
        if (iID_MaDuAn == Guid.Empty.ToString()) DuLieuMoi = "1";
    %>
    <div style="clear: both">
    </div>
    <div class="box_tong">
        <% using (Html.BeginForm("EditSubmit_DuAnDangDauTu", "TCDN_HoSo_DoanhNghiep", new { ParentID = ParentID, iID_MaDoanhNghiep = iID_MaDoanhNghiep, iID_MaDuAn = iID_MaDuAn }))
           {%>
             <%= Html.Hidden(ParentID + "_DuLieuMoi", DuLieuMoi)%>
        <div id="nhapform">
            <div id="form2">
                <div>
                    <table width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 30%">
                                <div>
                                    <b>
                                        <%= NgonNgu.LayXau("Tên dự án") %></b></div>
                            </td>
                            <td class="td_form2_td5" style="width: 45%">
                                <div>
                                    <%= MyHtmlHelper.TextBox(ParentID, sTenDuAn, "sTenDuAn", "", "class=\"input1_2\" ")%>
                                </div>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                           <td class="td_form2_td1" style="width: 30%">
                                <div>
                                    <b>
                                        <%= NgonNgu.LayXau("Đã hoàn thành") %></b></div>
                            </td>
                            <td class="td_form2_td5" style="width: 5%">
                                <div>
                                    <%= MyHtmlHelper.CheckBox(ParentID, bHoanThanh, "bHoanThanh", "", "")%>
                                </div>
                            </td>
                            <td>
                                
                            </td>

                        </tr>
                        <tr>
                             <td colspan="3">
                <div style="margin-top: 5px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="right" style="width: 48%">
                                <input type="submit" class="button" value="Tiếp tục" />
                            </td>
                            <td style="width: 2%">
                                &nbsp;
                            </td>
                            <td align="left"  style="width: 45%">
                                <input type="button" class="button" value="Hủy" onclick="Dialog_close('<%=ParentID %>');" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
         <% } %>
    </div>
     <script language="javascript" type="text/javascript">
         var mytext = document.getElementById("DoanhNghiep_sTenDuAn");
         mytext.focus();

     </script>
</body>
</html>
