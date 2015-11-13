<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<% 
   
    String ParentID = "TongHopNganSach";
    String LuyKe = Convert.ToString(ViewData["LuyKe"]);
    String Nganh = Convert.ToString(ViewData["Nganh"]);
    if(String.IsNullOrEmpty(Nganh))
    {
        Nganh = Guid.Empty.ToString();
    }
    String MaND = User.Identity.Name;
    //dt Trạng thái duyệt
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    }
    DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHePhanBo);
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
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
    String DotPhanBo = Convert.ToString(ViewData["DotPhanBo"]);
    if (String.IsNullOrEmpty(DotPhanBo))
    {
        DataTable dtdotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
        if (dtdotPhanBo.Rows.Count > 1)
        {
            DotPhanBo = Convert.ToString(dtdotPhanBo.Rows[1]["iID_MaDotPhanBo"]);
        }
        else
        {
            DotPhanBo = Guid.Empty.ToString();
        }
        dtdotPhanBo.Dispose();
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
    dtNam.Dispose();
    DataTable dtNganh = Connection.GetDataTable("SELECT iID_MaDanhMuc,sTenKhoa, DC_DanhMuc.sTen FROM DC_LoaiDanhMuc INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc WHERE DC_DanhMuc.bHoatDong=1 AND DC_LoaiDanhMuc.sTenBang=N'Nganh' ORDER BY sTenKhoa");
    if (String.IsNullOrEmpty(Nganh))
    {
        if (dtNganh.Rows.Count > 0)
        {
            Nganh = dtNganh.Rows[0]["sTenKhoa"].ToString();
        }
        else
        {
            Nganh = "000";
        }
    }
    SelectOptionList slNganh = new SelectOptionList(dtNganh, "sTenKhoa", "sTen");
    dtNganh.Dispose();
    String opDonViNganh;
    opDonViNganh = Convert.ToString(ViewData["opDonViNganh"]);
    if (String.IsNullOrEmpty(opDonViNganh)) opDonViNganh = "Nganh";
    dtNganh.Dispose();
    String BackURL = Url.Action("Index", "PhanBo_Report");
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
    if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptPhanBo_18E_TheoNganh", new { DotPhanBo = DotPhanBo, Nganh = Nganh, LuyKe = LuyKe, opDonViNganh = opDonViNganh, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    using (Html.BeginForm("EditSubmit", "rptPhanBo_18E_TheoNganh", new { ParentID = ParentID, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }))
    {
%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp chỉ tiêu theo ngành</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">              
                     <tr>
                                    <td width="20%"></td>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div><%=NgonNgu.LayXau("Trạng Thái:")%></div>                                    </td>
                                    <td width="10%">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\"onchange=ChonNLV()")%></div></td>
                                      <td class="td_form2_td1" style="width: 10%;" >
                                        <div><%=NgonNgu.LayXau("Ngành:")%></div>                                    </td>
                                  <td width="10%" colspan="2">
                                    <div><%=MyHtmlHelper.DropDownList(ParentID, slNganh, Nganh, "sNG", "", "class=\"input1_2\" style=\"width: 100%\"")%></div></td>
                                    <td width="5%"></td>
                                   <td width="10%"></td>
                                   <td width="5%"></td>
                                    <td></td>
                    </tr>
                    <tr>
                    <td></td>
                            <td class="td_form2_td1">
                                        <div><%=NgonNgu.LayXau("Chọn đợt phân bổ:")%></div>                                    </td >
                                    <td>
                                        <div id="<%=ParentID %>_divDotPhanBo">
                                                 <%rptPhanBo_18E_TheoNganhController rptTB1 = new rptPhanBo_18E_TheoNganhController(); %>
                                                 <%=rptTB1.obj_DSDotPhanBo(ParentID, MaND, iID_MaTrangThaiDuyet, DotPhanBo)%>                                        </div>                                    </td>
                                      <td class="td_form2_td1">
                                        <div><%=NgonNgu.LayXau("Ngành MLNS:")%></div>                                    </td>
                                    <td >
                                        <div>
                                            <%=MyHtmlHelper.Option(ParentID, "Nganh", opDonViNganh, "opDonViNganh", "")%>                                            </div>                                    </td>
                                     <td class="td_form2_td1">
                                        <div><%=NgonNgu.LayXau("Đơn vị:")%></div>                                    </td>
                                    <td>
                                        <div>
                                            <%=MyHtmlHelper.Option(ParentID, "DonVi", opDonViNganh, "opDonViNganh", "")%>                                            </div>                                    </td>
                                     <td class="td_form2_td1">
                                        <div><%=NgonNgu.LayXau("Lũy kế đến nay")%></div>                                    </td>
                                    <td>
                                        <div><%=MyHtmlHelper.CheckBox(ParentID, LuyKe, "iLuyKe", "", "")%></div>                                    </td>
                    </tr>
                    <tr>                                               
                            <td>&nbsp;</td>
    					                                                                                   
                            <td colspan="8">
                       			<table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 10px; width: 100%;" >
                                <tr>
                                    <td style="width: 48%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="4%">
                                    </td>
                                  <td style="width: 48%;" align="left">
                                        <input class="button" type="button" value="Hủy" onclick="Huy()" />
                                    </td>
                                </tr>
                            	</table>
                            </td>                          
    					    <td>&nbsp;</td>
                        </tr>
                </table>
            </div>
        </div>
    </div>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptPhanBo_18E_TheoNganh", new { DotPhanBo = DotPhanBo, Nganh = Nganh, LuyKe = LuyKe, opDonViNganh = opDonViNganh, iID_MaTrangThaiDuyet =  iID_MaTrangThaiDuyet }), "Export To Excels")%>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=BackURL%>';
         }
    </script>
     <script type="text/javascript">
         function ChonNLV() {
             var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Get_dsDotPhanBo?ParentID=#0&MaND=#1&iID_MaTrangThaiDuyet=#2&iID_MaDotPhanBo=#3", "rptPhanBo_18E_TheoNganh") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", "<%=MaND %>"));
             url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#3", "<%= DotPhanBo %>"));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_divDotPhanBo").innerHTML = data;
             });
         }                                            
        </script>
    <%} %>
    <%dtNam.Dispose();%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
