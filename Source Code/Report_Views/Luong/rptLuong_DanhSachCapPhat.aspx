<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.Luong" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <% 
        String PageLoad = Convert.ToString(ViewData["pageload"]);
        String ParentID = "CapPhatLuong";
        String NamBangLuong = Convert.ToString(ViewData["NamBangLuong"]);
        String UserID=User.Identity.Name;
        if (String.IsNullOrEmpty(NamBangLuong))
        {
            NamBangLuong =CauHinhLuongModels.LayNamLamViec(UserID).ToString();
        }
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        String ThangBangLuong = Convert.ToString(ViewData["ThangBangLuong"]);
        if(String.IsNullOrEmpty(ThangBangLuong))
        {
            ThangBangLuong = CauHinhLuongModels.LayThangLamViec(UserID).ToString();
        }
        dtThang.Dispose();

        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
           
            PageLoad = "0";
        }
        DataTable dtTrangThai = Luong_ReportModel.DachSachTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();

        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = Luong_ReportModel.DanhSach_DonVi(NamBangLuong, ThangBangLuong, iID_MaTrangThaiDuyet);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            
            PageLoad = "0";
        }
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
        dtDonVi.Dispose();
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "2";
        }
        dtKhoGiay.Dispose();
        
       
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptLuong_DanhSachCapPhat", new { NamBangLuong = NamBangLuong, ThangBangLuong = ThangBangLuong,iID_MaDonVi=iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay });
        String URL = Url.Action("Index", "Luong_Report");
        String urlExport = Url.Action("ExportToExcel", "rptLuong_DanhSachCapPhat", new { NamBangLuong = NamBangLuong, ThangBangLuong = ThangBangLuong, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay });
        using (Html.BeginForm("EditSubmit", "rptLuong_DanhSachCapPhat", new { ParentID = ParentID }))
        {
        %>
        
         <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo bảng lương,Phụ cấp</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
          <div id="Div2">
            <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                   
                    <tr>
                        <td width="1%"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Năm bảng lương:")%></div>                        </td>
                        <td width="6%">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamBangLuong, "NamBangLuong", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\" onchange=LocDV()")%>                            </div>                        </td>                 
                         <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Tháng bảng lương:")%></div>                        </td>
                        <td width="6%">
                              <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangBangLuong, "ThangBangLuong", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\" onchange=LocDV()")%>                            </div>                            </td>
                                  <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Trạng thái:")%></div>                        </td>
                        <td width="14%">
                              <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\" onchange=LocDV()")%>                            </div>                            </td>
                                <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Khổ giấy:")%></div>                        </td>
                        <td width="10%">
                              <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\" onchange=LocDV()")%>                            </div>                            </td>
                         <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn Đơn Vị:")%></div>                        </td>
                        <td width="10%"> <div id="<%=ParentID %>_divDonVi">
                                <%rptLuong_DanhSachCapPhatController rptTB1 = new rptLuong_DanhSachCapPhatController();%>
                                <%=rptTB1.obj_DSDonVi(ParentID, NamBangLuong, ThangBangLuong, iID_MaDonVi,iID_MaTrangThaiDuyet)%>
                            </div> 
                        </td>
               
                         <td></td>
                    </tr>
					 <tr>
                      <td></td>
                      <td colspan="10"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
                      <td></td>
                    </tr>
                 </table>
            </div>
        </div>
    </div><%=MyHtmlHelper.ActionLink(urlExport, "Xuất ra Excel")%>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
    </script>
    <script type="text/javascript">
        function LocDV() {
            var NamBangLuong = document.getElementById("<%=ParentID %>_NamBangLuong").value;
            var ThangBangLuong = document.getElementById("<%=ParentID %>_ThangBangLuong").value;
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&NamBangLuong=#1&ThangBangLuong=#2&iID_MaDonVi=#3&iID_MaTrangThaiDuyet=#4", "rptLuong_DanhSachCapPhat") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", NamBangLuong));
            url = unescape(url.replace("#2", ThangBangLuong));
            url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
            });
        }                                            
     </script>
        <%}%>
         <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
