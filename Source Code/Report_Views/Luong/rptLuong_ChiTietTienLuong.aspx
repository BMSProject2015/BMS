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
       
        String ParentID = "CapPhatLuong";
        String NamBangLuong = Convert.ToString(ViewData["NamBangLuong"]);
        String UserID=User.Identity.Name;        
        if (String.IsNullOrEmpty(NamBangLuong))
        {
            NamBangLuong = CauHinhLuongModels.LayNamLamViec(UserID).ToString();
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

        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "2";
        }
        dtKhoGiay.Dispose();

        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "-100";
        }
        DataTable dtTrangThai = Luong_ReportModel.DachSachTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
        String PageLoad = Convert.ToString(ViewData["pageload"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptLuong_ChiTietTienLuong", new { NamBangLuong = NamBangLuong, ThangBangLuong = ThangBangLuong, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay });
        String URL = Url.Action("Index", "Luong_Report");
        using (Html.BeginForm("EditSubmit", "rptLuong_ChiTietTienLuong", new { ParentID = ParentID }))
        {
        %>
        
         <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo giải thích chi tiết</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
          <div id="Div2">
            <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">                
                        <tr>
                        <td width="5%"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Năm bảng lương:")%></div>                        </td>
                        <td width="10%">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamBangLuong, "NamBangLuong", "", "class=\"input1_2\" style=\"width: 100%\" ")%>   </div>
                            </td>                 
                         <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Tháng bảng lương:")%></div>                        </td>
                        <td width="10%">
                              <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangBangLuong, "ThangBangLuong", "", "class=\"input1_2\" style=\"width: 100%\"")%> </div>
                                 </td>                     
                        <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Tháng bảng lương:")%></div>
                        </td>
                        <td width="15%">
                              <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\"")%>  </div>
                         </td> 
                         <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Khổ giấy:")%></div>
                        </td>
                        <td width="10%">
                              <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\"")%>  </div>
                         </td> 
                         <td></td>
                    </tr>
					 <tr>
                      <td></td>
                      <td colspan="8"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
    </div>    
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
    </script>
        <%}%>
          <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
