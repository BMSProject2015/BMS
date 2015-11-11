<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.CapPhat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%         
        String MaND = User.Identity.Name;
        String ParentID = "CapPhat";
        String LuyKe = Convert.ToString(ViewData["LuyKe"]);
        if (String.IsNullOrEmpty(LuyKe))
        {
            LuyKe = "";
        }
        //dt Trạng thái duyệt
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptCapPhat_79_3Controller.tbTrangThai();
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
        DataTable dtNgayCapPhat = rptCapPhat_80_4Controller.DanhSach_Ngay_CapPhat(iID_MaTrangThaiDuyet,MaND);
        String dNgayCapPhat = Convert.ToString(ViewData["dNgayCapPhat"]);
        if (String.IsNullOrEmpty(dNgayCapPhat))
        {
            if (dtNgayCapPhat.Rows.Count >1)
            {
               
                dNgayCapPhat = Convert.ToString(dtNgayCapPhat.Rows[1]["dNgayCapPhat"]);
            }
            else
            {
                dNgayCapPhat = "01/01/2000";
            }
        }
        SelectOptionList slNgayCapPhat = new SelectOptionList(dtNgayCapPhat, "dNgayCapPhat", "dNgayCapPhat");
        dtNgayCapPhat.Dispose();
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            DataTable dtDonVi = rptCapPhat_80_4Controller.DanhSach_DonVi(iID_MaTrangThaiDuyet, dNgayCapPhat,MaND,LuyKe);
            if (dtDonVi.Rows.Count > 2)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[2]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = "-2";
            }
            dtDonVi.Dispose();
        }
        
       
        dtNgayCapPhat.Dispose();
      
        String URL = Url.Action("Index", "CapPhat_Report");
        String UrlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptCapPhat_PhanHoTheoLNS", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, dNgayCapPhat = dNgayCapPhat, LuyKe = LuyKe });
        using (Html.BeginForm("EditSubmit", "rptCapPhat_PhanHoTheoLNS", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo phân hộ theo loại ngân sách</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td width="15%"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Trạng Thái :")%></div>
                        </td>
                        <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNLV(this.value)\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="10%">
                            <div>
                                <%=NgonNgu.LayXau("Ngày cấp phát:")%></div>
                        </td>
                        <td id="<%= ParentID %>_tdNgayCapPhat" width="10%">
                            <%rptCapPhat_PhanHoTheoLNSController rptTB1 = new rptCapPhat_PhanHoTheoLNSController();
                              rptCapPhat_PhanHoTheoLNSController.NLVData _NLVData = new rptCapPhat_PhanHoTheoLNSController.NLVData();
                              _NLVData = rptTB1.obj_NgayCapPhat(ParentID, iID_MaTrangThaiDuyet, dNgayCapPhat, iID_MaDonVi,MaND,LuyKe);
                            %>
                            <%=_NLVData.NgayCapPhat%>
                        </td>
                          <td class="td_form2_td1" width="5%">
                          <div><%=NgonNgu.LayXau("Lũy kế:")%></div>
                          </td>
                            <td width="3%">
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, LuyKe, "LuyKe", "", "onchange=ChonNCP()")%>                        
                                    </div></td>
                        <td class="td_form2_td1" style="width: 6%;">
                            <div>
                                <%=NgonNgu.LayXau("Đơn vị: ")%></div>
                        </td>
                        <td id="<%= ParentID %>_tdDonVi" width="10%">
                            <%=_NLVData.iID_MaDonVi%>
                        </td>
                       
                        <td></td>
                    </tr>
                    <tr>
                   <td>&nbsp;</td>
                            
                            <td colspan="8"> <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                                <tr>
                                                    <td width="49%" align="right"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                                    <td width="2%">&nbsp;</td>
                                                    <td width="49%"> <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                                </tr>
                                             </table></td>
                          
                            <td>&nbsp;</td></tr>
                </table>
                
            </div>
        </div>
    </div>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=URL %>';
         }
    </script>
    <script type="text/javascript">
        function ChonNLV(iID_MaTrangThaiDuyet) {
            var bLuyKe = document.getElementById("<%=ParentID%>_LuyKe").checked;
            var LuyKe="";
            if (bLuyKe) LuyKe = "on";
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_NgayCapPhat?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDonVi=#2&dNgayCapPhat=#3&LuyKe=#4", "rptCapPhat_PhanHoTheoLNS") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#3", "<%= dNgayCapPhat %>"));
            url = unescape(url.replace("#4", LuyKe));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_tdNgayCapPhat").innerHTML = data.NgayCapPhat;
                document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
            });
        }                                            
    </script>
    <script type="text/javascript">
        function ChonNCP() {
            var NCP = document.getElementById("<%=ParentID%>_dNgayCapPhat").value;
            var bLuyKe = document.getElementById("<%=ParentID%>_LuyKe").checked;
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;
            var LuyKe = "";
            if (bLuyKe) LuyKe = "on";
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_NgayCapPhat?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDonVi=#2&dNgayCapPhat=#3&LuyKe=#4", "rptCapPhat_PhanHoTheoLNS") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#3", NCP));
            url = unescape(url.replace("#4", LuyKe));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
            });
        }                                            
    </script>
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptCapPhat_80_4", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, dNgayCapPhat = dNgayCapPhat, iID_MaDonVi = iID_MaDonVi }), "Xuất ra Excel")%> 
    <iframe src="<%=UrlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
