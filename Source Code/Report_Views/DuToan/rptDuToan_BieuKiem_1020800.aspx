<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<%
  
    String ParentID = "BaoCaoNganSachNam";
    String MaND = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
    String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    DataTable dtTrangThai = rtprptDuToanChiNganSachQuocPhongXDCBCongTrinhPhoThongController.tbTrangThai();
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
    
    dtTrangThai.Dispose();
    String urlExport = Url.Action("ExportToExcel", "rptDuToan_BieuKiem_1020800", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "0" });
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
    if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptDuToan_BieuKiem_1020800", new {  iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    using (Html.BeginForm("EditSubmit", "rptDuToan_BieuKiem_1020800", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong" style="background-color:#F0F9FE;">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo chi ngân sách sử dụng năm</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="1000" height="80" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="476" align="right"><b>Chọn trạng thái :</b> </td>
    <td width="508"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%></td>
  </tr>
  <tr>
    <td colspan="2" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin-left:200px;">
                                <tr>
                                    <td><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table></td>
  </tr>
</table>
            </div>
        </div>
         <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
          <script type="text/javascript">
              function Huy() {
                  window.location.href = '<%=BackURL%>';
              }
    </script>
    </div>
    <%}
        //dtNam.Dispose();
         %>
    <div>
           <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
