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
   
    String ParentID = "BaoCaoNganSachNam";
    String NamLamViec = Request.QueryString["NamLamViec"];
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DateTime.Now.Year.ToString();
    }
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
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "0" });
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
    if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptDT_CNSNamLNS2", new { NamLamViec = NamLamViec });
    String urlExport = Url.Action("ExportToExcel", "rptDT_CNSNamLNS2", new { NamLamViec = NamLamViec, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeDuToan);
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = "0";
    }

    using (Html.BeginForm("EditSubmit", "rptDT_CNSNamLNS2", new { ParentID = ParentID, NamLamViec = NamLamViec, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo Phân bổ dự toán ngân sách năm</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="text-align:center;">
                            <div><b>Chọn trạng thái :</b>   <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                                 <%= Html.ValidationMessage(ParentID + "_" + "err_iNamLamViec")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="margin:0 auto;">
                            <table cellpadding="0" cellspacing="0" border="0" style="margin-left:500px;">
                                <tr>
                                    <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table>
                        </td>
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
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
