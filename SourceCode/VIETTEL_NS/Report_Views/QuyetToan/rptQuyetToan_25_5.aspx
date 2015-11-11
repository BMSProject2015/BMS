<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
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
     
        String MaND = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        String iNamLamViec = DateTime.Now.Year.ToString();
        if (dtCauHinh.Rows.Count > 0)
        {
            iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

        }
       
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtiThang = DanhMucModels.DT_Thang();
        String iThang = Convert.ToString(ViewData["iThang"]);
        {
            if (String.IsNullOrEmpty(iThang))
            {
                iThang = "1";
            }
        }
        SelectOptionList sliThang = new SelectOptionList(dtiThang, "MaThang", "TenThang");
        dtiThang.Dispose();

        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        //DataTable dtDonVi = QuyetToan_ReportModels.DanhSach_DonVi(iNamLamViec, iThang, "1010000");
        DataTable dtDonVi = rptQuyetToan_25_5Controller.DanhSach_DonVi(iThang, MaND,iID_MaTrangThaiDuyet, "1010000");    //.DanhSach_DonVi(iNamLamViec, iThang, "1010000");
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 1)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[1]["iID_MaDonVi"]);
            }
            else
             {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        DataTable dtTrangThai = rptQuyetToan_25_5Controller.tbTrangThai();
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
        String Cachin = Convert.ToString(ViewData["Cachin"]);
        if (String.IsNullOrEmpty(Cachin))
        {
            Cachin = "0";
        }
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });
        String urlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_25_5", new { iThang = iThang, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Cachin = Cachin });
        }
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_25_5", new { ParentID = ParentID}))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp quyết toán tiết mục</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                      <td width="20%"> </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn trạng thái:")%></div>
                        </td>
                       <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChoniThang()\" ")%>
                            </div>
                        </td>
                         <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn Tháng:")%></div>
                        </td>
                       <td width="10%">
                            <div>
                             <%=MyHtmlHelper.DropDownList(ParentID, sliThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChoniThang()\"")%>
                            </div>
                        </td>                              
                          <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Chọn Đơn Vị:")%></div>
                        </td>
                        <td style="width: 10%;">
                            <div id="<%=ParentID %>_divDonVi">
                                     <%rptQuyetToan_25_5Controller rptTB1 = new rptQuyetToan_25_5Controller();%>                                    
                                     <%=rptTB1.obj_DSDonVi(ParentID,iThang,MaND,iID_MaDonVi,iID_MaTrangThaiDuyet)%>
                            </div>
                        </td>
                        <td>
                        </td>
                                     
                    </tr>             
                   <tr>
                        <td>
                        </td>
                         
                        <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
<td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
   <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_25_5", new {iThang = iThang, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,Cachin = Cachin }), "Xuất ra Excels")%>
       <script type="text/javascript">
           function Huy() {
               window.location.href = '<%=URL %>';
           }
 </script>
      <script type="text/javascript">
          function ChoniThang() {
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
              var iThang = document.getElementById("<%=ParentID %>_iThang").value;
           
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&iThang=#1&MaND=#2&iID_MaDonVi=#3&iID_MaTrangThaiDuyet=#4", "rptQuyetToan_25_5") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", iThang));
             url = unescape(url.replace("#2", "<%= MaND %>"));
             url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
             url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));                
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
              });
          }                                            
      </script>
        
    <%} %>
    <%--<%dtNam.Dispose();
        %>--%>
    <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
