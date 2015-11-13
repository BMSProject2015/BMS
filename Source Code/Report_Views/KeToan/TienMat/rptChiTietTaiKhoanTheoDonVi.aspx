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
    String iThang1 = Request.QueryString["iThang1"];
    String iThang2 = Request.QueryString["iThang2"]; 
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
    DataTable dtThang1 = DanhMucModels.DT_Thang();
    SelectOptionList slThang1 = new SelectOptionList(dtThang1, "MaThang", "TenThang");
    if (String.IsNullOrEmpty(iThang1))
    {
        iThang1 = Convert.ToString(dtThang1.Rows[1]["TenThang"]);
    }
    dtThang1.Dispose();
    //Chọn tháng
    DataTable dtThang2 = DanhMucModels.DT_Thang();
    SelectOptionList slThang2 = new SelectOptionList(dtThang2, "MaThang", "TenThang");
    if (String.IsNullOrEmpty(iThang2))
    {
        iThang2 = Convert.ToString(dtThang2.Rows[1]["TenThang"]);
    }
    dtThang2.Dispose();
    String BackURL = Url.Action("Index", "KeToan_ChiTiet_Report", new { sLoai = "1" });
    using (Html.BeginForm("EditSubmit", "rptChiTietTaiKhoanTheoDonVi", new { ParentID = ParentID }))
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
                                <%=NgonNgu.LayXau("Chọn đơn vị")%></div>
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
                        <td  class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Từ tháng")%></div>
                        </td>
                        <td  class="td_form2_td5" style="width: 40%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang1, iThang1, "iThang1", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                    </tr>
                   
                     <tr>
                        <td  class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Đến tháng")%></div>
                        </td>
                        <td  class="td_form2_td5" style="width: 40%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang2, iThang2, "iThang2", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                    </tr>
                 
                    <tr>
                        <td  class="td_form2_td1" style="width: 10%;"> </td>
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
        dtThang1.Dispose();
        //dtNgay1.Dispose();
        dtThang2.Dispose();
    %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
 </script>
 
  <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptChiTietTaiKhoanTheoDonVi", new { iID_MaDonVi = iID_MaDonVi, iNamLamViec = iNamLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, iThang1 = iThang1, iThang2 = iThang2 }), "Xuất ra file Excel")%>
     <iframe src="<%=Url.Action("ViewPDF","rptChiTietTaiKhoanTheoDonVi",new{ iID_MaDonVi =iID_MaDonVi, iNamLamViec=iNamLamViec, iID_MaTaiKhoan=iID_MaTaiKhoan,iThang1 = iThang1,  iThang2 = iThang2})%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>
