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
<head runat="server">
    <title></title>
</head>
<body>
<%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoNganSachNam";
    String MaND = User.Identity.Name;
    String sLNS = Request.QueryString["sLNS"];
    DateTime dNgayHienTai = DateTime.Now;
    String NamHienTai = Convert.ToString(dNgayHienTai.Year);
    int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
    int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
    DataTable dtNam = new DataTable();
    dtNam.Columns.Add("MaNam", typeof(String));
    dtNam.Columns.Add("TenNam", typeof(String));
    DataRow R;
    for (int i = NamMin; i < NamMax; i++)
    {
        R = dtNam.NewRow();
        dtNam.Rows.Add(R);
        R[0] = Convert.ToString(i);
        R[1] = Convert.ToString(i);
    }
    dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
    dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    DataTable dtTrangThai = rptDuToanChiNganSachSuDungController.tbTrangThai();
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
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "0" });
    String urlExport = Url.Action("ExportToExcel", "rptDuToanChiNganSachSuDung", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    using (Html.BeginForm("EditSubmit", "rptDuToanChiNganSachSuDung", new { ParentID = ParentID, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }))
    {
    %>   
    <div class="box_tong" style="background-color:#F0F9FE;">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo Dự toàn ngân sách sử dụng</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">

<table width="1000" height="80" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="476" align="right"><b>Chọn trạng thái :</b> </td>
    <td width="508"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 60%\"")%></td>
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
        dtNam.Dispose();
         %>
    <div>    
    <iframe src="<%=Url.Action("ViewPDF","rptDuToanChiNganSachSuDung",new{MaND=MaND,iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet})%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
