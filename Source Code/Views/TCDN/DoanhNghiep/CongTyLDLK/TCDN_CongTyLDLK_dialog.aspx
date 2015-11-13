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
        String iID_Ma = Convert.ToString(Request.QueryString["iID_Ma"]);
        if (String.IsNullOrEmpty(iID_Ma)) iID_Ma = Guid.Empty.ToString();
        String sTen = "";
        String iID_MaHinhThucHoatDong = "";
        String sVonDieuLe = "", sVonGop = "";
        DataTable dtCtyLDLK = TCDNModels.getChiTietCongTyLDLK(iID_Ma);
        if (dtCtyLDLK.Rows.Count > 0)
        {
            sTen = Convert.ToString(dtCtyLDLK.Rows[0]["sTen"]);
            iID_MaHinhThucHoatDong = Convert.ToString(dtCtyLDLK.Rows[0]["iID_MaHinhThucHoatDong"]);
            sVonDieuLe = Convert.ToString(dtCtyLDLK.Rows[0]["sVonDieuLe"]);
            sVonGop = Convert.ToString(dtCtyLDLK.Rows[0]["sVonGop"]);
        }

        
        // Hình thức hoạt động
       
        var dtHinhThucHoatDong = DanhMucModels.DT_DanhMuc("TCDN_HinhThucHoatDong", false, "Chọn hình thức hoạt động");
        var slHinhThucHoatDong = new SelectOptionList(dtHinhThucHoatDong, "iID_MaDanhMuc", "sTen");
       
       
        
        String DuLieuMoi = "";
        if (iID_Ma==Guid.Empty.ToString()) DuLieuMoi = "1";
    %>
    <div style="clear: both">
    </div>
    <div class="box_tong">
        <% using (Html.BeginForm("EditSubmit_CongTyLDLK", "TCDN_HoSo_DoanhNghiep", new { ParentID = ParentID, iID_MaDoanhNghiep = iID_MaDoanhNghiep, iID_Ma = iID_Ma }))
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
                                        <%= NgonNgu.LayXau("Tên công ty") %></b></div>
                            </td>
                            <td class="td_form2_td5" style="width: 45%">
                                <div>
                                    <%= MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\" ")%>
                                </div>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 30%">
                                <div>
                                    <b>
                                        <%= NgonNgu.LayXau("Hình thức hoạt động") %></b></div>
                            </td>
                            <td class="td_form2_td5" style="width: 45%">
                                <div>
                                     <%=MyHtmlHelper.DropDownList(ParentID, slHinhThucHoatDong, iID_MaHinhThucHoatDong, "iID_MaHinhThucHoatDong", "", "class='input1_2' style='with:100px;'")%>
                                </div>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 30%">
                                <div>
                                    <b>
                                        <%= NgonNgu.LayXau("Vốn điều lệ") %></b></div>
                            </td>
                            <td class="td_form2_td5" style="width: 45%">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sVonDieuLe, "sVonDieuLe", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='500' tab-index='-1'")%>
                                </div>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 30%">
                                <div>
                                    <b>
                                        <%= NgonNgu.LayXau("Số vốn góp") %></b></div>
                            </td>
                            <td class="td_form2_td5" style="width: 45%">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sVonGop, "sVonGop", "", "class=\"input1_2\" style=\"width:100%;\" maxlength='500' tab-index='-1'")%>
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
         var mytext = document.getElementById("DoanhNghiep_sTen");
         mytext.focus();

     </script>
</body>
</html>
