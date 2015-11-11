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

        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if(String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "-100";
        }
        DataTable dtTrangThai=Luong_ReportModel.DachSachTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = Luong_ReportModel.DanhSach_DonVi(NamBangLuong, ThangBangLuong,iID_MaTrangThaiDuyet);
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
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTenDonVi");
        dtDonVi.Dispose();
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "2";
        }
        dtKhoGiay.Dispose();
        String TongHop=Convert.ToString(ViewData["TongHop"]);
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptLuong_TongHopLuongPhuCap", new { NamBangLuong = NamBangLuong, ThangBangLuong = ThangBangLuong, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, TongHop = TongHop, KhoGiay = KhoGiay });
        String URL = Url.Action("Index", "Luong_Report");
        using (Html.BeginForm("EditSubmit", "rptLuong_TongHopLuongPhuCap", new { ParentID = ParentID }))
        {
        %>
        
         <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo bảng lương, phụ cấp</span>
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
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamBangLuong, "NamBangLuong", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon()")%>                            </div>                        </td>                 
                         <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Tháng bảng lương:")%></div>  </td>
                        <td width="10%">
                              <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangBangLuong, "ThangBangLuong", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon()")%>
                                </div>               
                         </td>
                        
                        <td class="td_form2_td1" style="width: 7%;">
                            <div><%=NgonNgu.LayXau("Trạng thái:")%></div></td>
                        <td width="14%">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=Chon()")%>   </div>                        </td>                 
                         <td width="7%" class="td_form2_td1"><div><%=NgonNgu.LayXau("Đơn vị:")%></div>    </td>
                        <td width="10%" rowspan="5"> <div id="<%=ParentID %>_tdDonVi">
                                <%rptLuong_TongHopLuongPhuCapController rptTB1 = new rptLuong_TongHopLuongPhuCapController();%>
                                <%=rptTB1.obj_DSDonVi(ParentID, NamBangLuong, ThangBangLuong,iID_MaTrangThaiDuyet, iID_MaDonVi)%>
                            </div> 
                        </td>   
                                  
                         <td></td>
                    </tr>
                    <tr>
                    <td></td>
                     <td class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Khổ giấy:")%></div></td>
                        <td>
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\"")%>   </div> 
                        </td>
                        <td class="td_form2_td1">
                         <div><%=NgonNgu.LayXau("Tất cả đơn vị:")%></div>
                        </td> 
                        <td>
                        <div><%=MyHtmlHelper.CheckBox(ParentID, TongHop, "TongHop","","")%></div>
                        </td>
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
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
    </script>
    <script type="text/javascript">
        function Chonall(sLNS) {
            $("input:checkbox[check-group='MaDonVi']").each(function (i) {
                if (sLNS) {
                    this.checked = true;
                }
                else {
                    this.checked = false;
                }
            });

        }                                            
    </script>
     <script type="text/javascript">
         function Chon(){
             var ThangBangLuong = document.getElementById("<%= ParentID %>_ThangBangLuong").value;
             var NamBangLuong = document.getElementById("<%= ParentID %>_NamBangLuong").value;
             var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&NamBangLuong=#1&ThangBangLuong=#2&iID_MaTrangThaiDuyet=#3&iID_MaDonVi=#4", "rptLuong_TongHopLuongPhuCap") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", NamBangLuong));
             url = unescape(url.replace("#2", ThangBangLuong));
             url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
             });
         }                                            
    </script>
        <%}%>
         <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
