<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<%
    
    String ParentID = "BaoCaoNganSachNam";
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
        DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND,iID_MaTrangThaiDuyet);
        if (dtDotPhanBo.Rows.Count > 1)
        {
            DotPhanBo = Convert.ToString(dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"]);
        }
        else
        {
            DotPhanBo = Guid.Empty.ToString();
        }
        dtDotPhanBo.Dispose();
    }
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {

        DataTable dtDonVi = PhanBo_ReportModels.DanhSachDonVi2(MaND,iID_MaTrangThaiDuyet, DotPhanBo,true);
        if (dtDonVi.Rows.Count > 1)
        {
            iID_MaDonVi = Convert.ToString(dtDonVi.Rows[1]["iID_MaDonVi"]);
        }
        else
        {
            iID_MaDonVi = Guid.Empty.ToString();
        }
       
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
    String BackURL = Url.Action("Index", "PhanBo_Report");
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
    if(PageLoad=="1")
        URLView = Url.Action("ViewPDF", "rptThongBaoBoSungNganSach", new { iID_MaDonVi = iID_MaDonVi, DotPhanBo = DotPhanBo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    
    using (Html.BeginForm("EditSubmit", "rptThongBaoBoSungNganSach", new { ParentID = ParentID}))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo thông báo ngân sách : Thông báo tổng hợp</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="13%"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Trạng Thái:")%></div>
                        </td>
                        <td width="10%">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNLV()\"")%>                           
                            </div>
                        </td>                   
                        <td class="td_form2_td1" style="width:10%">
                            <div><%=NgonNgu.LayXau("Chọn đợt phân bổ:")%></div>
                        </td >
                        <td width="15%">
                           <div id="<%=ParentID %>_divDotPhanBo">
                                       <% rptThongBaoBoSungNganSachController rpt = new rptThongBaoBoSungNganSachController();
                                          rptThongBaoBoSungNganSachController.Data _Data = new rptThongBaoBoSungNganSachController.Data();
                                          _Data = rpt.obj_DSDotPhanBo(ParentID, MaND, iID_MaTrangThaiDuyet, DotPhanBo, iID_MaDonVi);
                            %>
                            <%=_Data.iID_MaDotPhanBo%>
                            </div>    
                        </td>                   
                      <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Chọn đơn vi:")%></div>
                        </td>
                        <td width="15%">
                            <div id="<%=ParentID %>_divDonVi">                                    
                                     <%=_Data.iID_MaDonVi%>
                            </div>
                        </td> 
                        <td></td>
                        </tr>   
                        <tr>
                            <td>&nbsp;</td>
                            
                            <td colspan="6"> <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                                <tr>
                                                    <td width="49%" align="right"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                                    <td width="2%">&nbsp;</td>
                                                    <td width="49%"> <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                                </tr>
                                             </table></td>
                            <td>&nbsp;</td>
                          
                        </tr>           
                 </table>
            </div>
        </div>
    </div>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=BackURL%>';
         }
    </script>
        <script type="text/javascript">
            function ChonNLV() {
                var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                var DotPhanBo1 = document.getElementById("<%=ParentID %>_iID_MaDotPhanBo").value;
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Get_dsDotPhanBo?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDotPhanBo=#2&iID_MaDonVi=#3&MaND=#4", "rptThongBaoBoSungNganSach") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
                url = unescape(url.replace("#2", "<%= DotPhanBo%>"));
                url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
                url = unescape(url.replace("#4", "<%= MaND %>"));             
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_divDotPhanBo").innerHTML = data.iID_MaDotPhanBo;
                    document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data.iID_MaDonVi;
                });
            }                                            
        </script> 
           <script type="text/javascript">
               function ChonDPB() {
                   var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                   var DotPhanBo = document.getElementById("<%=ParentID %>_iID_MaDotPhanBo").value;              
                   jQuery.ajaxSetup({ cache: false });
                   var url = unescape('<%= Url.Action("Get_dsDotPhanBo?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDotPhanBo=#2&iID_MaDonVi=#3&MaND=#4", "rptThongBaoBoSungNganSach") %>');
                   url = unescape(url.replace("#0", "<%= ParentID %>"));
                   url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
                   url = unescape(url.replace("#2", DotPhanBo));
                   url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
                   url = unescape(url.replace("#4", "<%= MaND %>"));
                   $.getJSON(url, function (data) {                 
                       document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data.iID_MaDonVi;
                   });
               }                                            
        </script>   
    <%} %>
  <%
    dtNam.Dispose();    
     
%> <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThongBaoBoSungNganSach", new { iID_MaDonVi = iID_MaDonVi, DotPhanBo = DotPhanBo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Xuất ra Excel")%> 
   <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
