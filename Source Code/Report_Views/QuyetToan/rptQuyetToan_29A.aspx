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
        String MaND = User.Identity.Name;
        String ParentID = "QuyetToanNganSach";
        String TongHop = Request.QueryString["TongHop"];
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_30A1Controller.tbTrangThai();
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
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        String Quy = Convert.ToString(ViewData["Quy"]);
        if(String.IsNullOrEmpty(Quy))
        {
            Quy = "1";
        }
        dtQuy.Dispose();
        DataTable dtDonVi = QuyetToan_ReportModels.DanhSach_DonVi_Quy_TX(iID_MaTrangThaiDuyet, Quy,MaND);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {           
            if(dtDonVi.Rows.Count>1)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[1]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        dtDonVi.Dispose();
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = "0" });
        String URLView = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptQuyetToan_29A", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Quy = Quy, iID_MaDonVi = iID_MaDonVi });
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_29A", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp quyết toán thường xuyên Tháng-quý</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                         <td width="20%"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Trạng  Thái :")%></div>
                        </td>
                        <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonQuy()\"")%>
                            </div>
                        </td>
                    
                        <td class="td_form2_td1" style="width: 10%">
                            <div>
                                <%=NgonNgu.LayXau("Chọn Quý: ")%></div>
                        </td>
                       <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy,Quy, "MaQuy", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonQuy()\"")%>
                            </div>
                        </td>
                   
                      <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Chọn đơn vị:")%></div>
                        </td>
                         <td style="width: 10%;">
                            <div id="<%=ParentID %>_divDonVi">
                            <%rptQuyetToan_29AController rptTB1 = new rptQuyetToan_29AController();%>                                   
                                     <%=rptTB1.obj_DSDonVi(ParentID,iID_MaTrangThaiDuyet,Quy,iID_MaDonVi,MaND)%>
                            </div>
                        </td>
                         <td>
                           </td>
                   
                </tr>
                     <tr>
                        <td>
                        </td>
                         
                        <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
     <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_29A", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Quy = Quy, iID_MaDonVi = iID_MaDonVi }), "Xuất ra Excels")%>
      <script type="text/javascript">
          function ChonQuy() {
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
              var Quy = document.getElementById("<%=ParentID %>_MaQuy").value;
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Quy=#2&iID_MaDonVi=#3", "rptQuyetToan_29A") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
              url = unescape(url.replace("#2", Quy));
              url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
              });
          }                                            
      </script>
        
    <%} %>
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
    </script>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
