<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
<%@ Import Namespace="FlexCel.Core" %>
<%@ Import Namespace="FlexCel.Render" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoNganSachNam";
    String MaND = User.Identity.Name;
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    DataTable dtTrangThai = rptDuToan_5d_CController.tbTrangThai();
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        if (dtTrangThai.Rows.Count > 0)
        {
            iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]);
        }
        else
        {
            iID_MaTrangThaiDuyet = Guid.Empty.ToString();
        }
    }
    dtTrangThai.Dispose();
            
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView="";
    if(PageLoad=="1")
        URLView = Url.Action("ViewPDF", "rptDuToan_5d_C", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    using (Html.BeginForm("EditSubmit", "rptDuToan_5d_C", new { ParentID = ParentID, MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo tổng hợp ngân sách người có công</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                <tr><td colspan="2" style="height:5px;"></td></tr>
                    <tr>
                        <td class="td_form2_td1" style="text-align:right; width:50%;">
                            <div><%=NgonNgu.LayXau("Chọn trạng thái")%></div>
                        </td>
                        <td class="td_form2_td1" style="text-align:left; width:50%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                                
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" colspan="2" style="text-align:center";>                       
                            <table cellpadding="0" cellspacing="0" border="0" style="width:250px; margin: 3px auto;">
                                <tr>
                                    <td style="text-align:right;"><input style="display:inline-block" type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="10px"></td>
                                    <td style="text-align:left;"><input style="display:inline-block; margin-left:5px;" class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                 </table>
            </div>
        </div>
    </div>
    <%}
         %>
    <div>    
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URLView %>';
        }
    </script>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
