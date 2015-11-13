<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienMat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
     <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "QuyetToanThongTri";
    String iNamLamViec = Request.QueryString["iNamLamViec"];
   
    if (String.IsNullOrEmpty(iNamLamViec))
    {
        iNamLamViec = DateTime.Now.Year.ToString();
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
    String UserID = User.Identity.Name;
    String iID_MaTaiKhoan = Request.QueryString["iID_MaTaiKhoan"];
    String iThang = Request.QueryString["iThang"]; 
    String iNgay1 = Request.QueryString["iNgay1"];
    String iNgay2 = Request.QueryString["iNgay2"];
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    // Đơn Vị
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
    }
    dtDonVi.Dispose();     
    
     // Mã tài khoản
    DataTable dtTK = TaiKhoanModels.DT_DSTaiKhoan(false);
    SelectOptionList slTK = new SelectOptionList(dtTK, "iID_MaTaiKhoan", "iID_MaTaiKhoan");
    if (String.IsNullOrEmpty(iID_MaTaiKhoan))
    {
        iID_MaTaiKhoan = Convert.ToString(dtTK.Rows[0]["iID_MaTaiKhoan"]);
    }
    dtTK.Dispose();
         
   //Chọn tháng
    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    if (String.IsNullOrEmpty(iThang))
    {
        iThang = Convert.ToString(dtThang.Rows[1]["TenThang"]);
    }
    dtThang.Dispose();
    //Chọn ngày
    DataTable dtNgay2 = DanhMucModels.DT_Ngay();
    SelectOptionList slNgay2 = new SelectOptionList(dtNgay2, "MaNgay", "TenNgay");
    if (String.IsNullOrEmpty(iNgay2))
    {
        iNgay2 = Convert.ToString(dtNgay2.Rows[1]["TenNgay"]);
    }
    dtNgay2.Dispose();
    DataTable dtNgay1 = DanhMucModels.DT_Ngay();
    SelectOptionList slNgay1 = new SelectOptionList(dtNgay1, "MaNgay", "TenNgay");
    if (String.IsNullOrEmpty(iNgay1))
    {
        iNgay1 = Convert.ToString(dtNgay2.Rows[1]["TenNgay"]);
    }
    dtNgay1.Dispose();
    String BackURL = Url.Action("Index", "KeToan_ChiTiet_Report", new { sLoai = "1" });
    using (Html.BeginForm("EditSubmit", "rptChiTietTheoDonVi", new { ParentID = ParentID }))
   {
    %>   
     <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo</span>
                    </td>
                </tr>
            </table>
        </div>
         <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn năm làm việc")%></div>
                        </td>
                        <td class="td_form2_td5" style="width: 40%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn Phòng ban")%></div>
                        </td>
                        <td class="td_form2_td5" style="width: 40%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                    </tr> 
                    <tr>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn tài khoản")%></div>
                        </td>
                        <td class="td_form2_td5" style="width: 40%;">
                            <div>
                               
                                <%=MyHtmlHelper.DropDownList(ParentID, slTK, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Từ ngày")%></div>
                        </td>
                       <%-- <td class="td_form2_td5" style="width: 40%;">
                            <div id="<%=ParentID %>_divDonVi">
                                     <%rptChiTietTheoDonViController rptTB1 = new rptChiTietTheoDonViController();%>                                    
                                     <%=rptTB1.Get_dtNgay(ParentID,iNamLamViec,iThang,iNgay1)%>
                            </div>
                        </td>--%>
                         <td class="td_form2_td5" style="width: 40%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNgay1, iNgay1, "iNgay1", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                        
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn Tháng")%></div>
                        </td>
                        <td class="td_form2_td5" style="width: 40%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                        
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Đến ngày")%></div>
                        </td>
                        <td class="td_form2_td5" style="width: 40%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNgay2, iNgay2, "iNgay2", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                    </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td style="width: 40%;"> </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <div>
    </div>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo</span>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <%
        dtDonVi.Dispose();
        dtTK.Dispose();
        dtThang.Dispose();
        //dtNgay1.Dispose();
        dtNgay2.Dispose();
    %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
 </script>
 <script type="text/javascript">
     function ChonNgay() {
         var NamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value

         jQuery.ajaxSetup({ cache: false });
         var url = unescape('<%= Url.Action("Get_dtNgay?ParentID=#0&NamLamViec=#1&iThang=#2&iNgay1=#3", "rptChiTietTheoDonVi") %>');
         url = unescape(url.replace("#0", "<%= ParentID %>"));
         url = unescape(url.replace("#1", NamLamViec));
         url = unescape(url.replace("#2", "<%= iThang %>"));
         url = unescape(url.replace("#3", "<%= iNgay1 %>"));
         $.getJSON(url, function (data) {
             document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
         });
     }                                            
      </script>
  <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptChiTietTheoDonVi", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang = iThang, iNgay1 = iNgay1, iNgay2 = iNgay2 }), "Xuất ra file Excel")%>
     <iframe src="<%=Url.Action("ViewPDF","rptChiTietTheoDonVi",new{ iID_MaDonVi =iID_MaDonVi, iNamLamViec=iNamLamViec, iID_MaTaiKhoan=iID_MaTaiKhoan,iThang = iThang, iNgay1 = iNgay1, iNgay2 = iNgay2 })%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>
