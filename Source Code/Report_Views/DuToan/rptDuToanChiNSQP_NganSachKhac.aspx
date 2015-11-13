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
   
    String ParentID = "BaoCaoNganSachNam";
    String NamLamViec = Request.QueryString["NamLamViec"];
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DateTime.Now.Year.ToString();
    }
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
    DataTable dtTrangThai = rptDuToanChiNSQP_NganSachKhacController.tbTrangThai();
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
    
    String sLNS = Request.QueryString["sLNS"];
    DataTable dtLNS = DanhMucModels.NS_LoaiNganSachKhac();
    SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
    if (String.IsNullOrEmpty(sLNS))
    {
        if (dtLNS.Rows.Count > 0)
        {
            sLNS = Convert.ToString(dtLNS.Rows[0]["sLNS"]);
        }
        else
        {
            sLNS = Guid.Empty.ToString();
        }
    }
    dtLNS.Dispose();
    String URL = Url.Action("Index", "DuToan_Report", new { sLoai = "0" });
    using (Html.BeginForm("EditSubmit", "rptDuToanChiNSQP_NganSachKhac", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo dự toán chi ngân sách quốc phòng (Ngân sách khác)</span>
                    </td>
                </tr>
            </table>
        </div>
                <div id="Div1" style="background-color:#F0F9FE;">
            <div id="Div2">
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 62px;margin-left:100px;">
  <tr>
    <td align="right" style="width:350px;"><b>Trạng Thái : </b></td>
    <td><div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 80%;heigh:22px;\"")%>                              
                            </div></td>
    <td align="right"><b>Chọn LNS : </b></td>
    <td><div><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\" style=\"width: 50%\"")%>                              
                            </div></td>
  </tr>
  <tr>
    <td colspan="4"><table cellpadding="0" cellspacing="0" border="0" style="margin-left:450px;">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="5%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td>
  </tr>
</table>
            </div>
        </div>

    </div>
    <%}
        dtNam.Dispose();
         %>
        <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        </script>
        <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToanChiNSQP_NganSachKhac", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS }), "Xuất ra file Excel")%>
        <div>  
            <iframe src="<%=Url.Action("ViewPDF","rptDuToanChiNSQP_NganSachKhac",new{iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet,sLNS=sLNS})%>" height="600px" width="100%"></iframe>
        </div>
    </body>
</html>