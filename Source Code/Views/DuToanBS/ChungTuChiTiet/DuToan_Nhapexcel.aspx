<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
   <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoNganSachNam";
    String MaND=User.Identity.Name;
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    String iLoai = Request.QueryString["iLoai"];
    using (Html.BeginForm("UpLoadExcel", "DuToanBSChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu, iLoai = iLoai }, FormMethod.Post, new { enctype = "multipart/form-data" }))
    {
    %>   
    <div class="box_tong">
        <div id="Div1">
            <div id="Div2">
                 <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                                    <td  class="td_form2_td1" style="width: 25%;color: Red; text-align:center" colspan="2">
                                        <div>
                                           <b> <%=NgonNgu.LayXau("Chú ý nhập dữ liệu bằng file Excel có thể sẽ bị mất các dữ liệu đã nhập ")%> </b>
                                            </div>
                                    </td>
                                    
                                </tr>
                               <tr>
                      <tr>
                                    <td class="td_form2_td1" style="width: 25%">
                                        <div>
                                            <%=NgonNgu.LayXau("Chọn file Excel ")%>
                                            <b style="color: Red">(*)</b></div>
                                    </td>
                                    <td class="td_form2_td5" style="width: 75%">
                                        <div>
                                            <%=MyHtmlHelper.Label("", "sFileName1", "")%>
                                            <%=MyHtmlHelper.Hidden(ParentID, "", "sFileName", "")%>
                                            <%=MyHtmlHelper.Hidden(ParentID, "", "sURL", "")%>
                                        </div>
                                        <div>
                                            <input type="file" name="uploadFile" id="uploadFile" style="width: 95%" onchange="checkFileExtension();" /><br />
                                             <%= Html.ValidationMessage(ParentID + "_" + "err_sFileName")%>
                                        </div>
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
                                <input type="submit" class="button"  value="Tiếp tục" id="btnTuChoi" />
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
            </div>
        </div>
    </div>
    <%}
       
         %>
</body>
</html>
