<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
<%
    String MaND = User.Identity.Name;
    String ParentID = "BaoCaoNganSachNam";
   
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
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
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    DataTable dtTrangThai = rptDuToanChiNSQP_NganSachKhac_DonViController.tbTrangThai();
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

    DataTable dtNguonDonVi = rptDuToanChiNganSachSuDung_PhanNganSachNghiepVuController.DanhSachDonVi(iID_MaTrangThaiDuyet, "1020100",MaND);
    SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "TenHT");
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        if (dtNguonDonVi.Rows.Count > 0)
        {
            iID_MaDonVi = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDonVi"]);
        }
        else
        {
            iID_MaDonVi = Guid.Empty.ToString();
        }
    }
    dtNguonDonVi.Dispose();
    dtNam.Dispose();
   
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "1" });
    using (Html.BeginForm("EditSubmit", "rptDuToanChiNganSachSuDung_PhanNganSachNghiepVu", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo dự toán chi ngân sách sử dụng phần ngân sách nghiệp vụ</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2" style="margin-top:5px;">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Trạng Thái:")%></div>
                        </td>
                        <td style="width: 10%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 50%\" onchange=\"ChonNamLamViec()\"")%>                                
                            </div>
                        </td>                   
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Chọn đơn vị:")%></div>
                        </td>
                        <td style="width: 15%;">
                            <div id="<%=ParentID %>_divDonVi">
                                     <%rptDuToanChiNganSachSuDung_PhanNganSachNghiepVuController rptTB1 = new rptDuToanChiNganSachSuDung_PhanNganSachNghiepVuController();%>                                    
                                     <%=rptTB1.obj_DSDonVi(ParentID, iID_MaTrangThaiDuyet, iID_MaDonVi,MaND)%>
                            </div>
                        </td>
                    </tr>
                          <tr>
                         
                        <td colspan="4"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
                    </tr>
                 </table>
            </div>
        </div>
    </div>
    <%} %>
    
      <script type="text/javascript">
          function ChonNamLamViec() {
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value             

              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&MaND=#1&iID_MaTrangThaiDuyet=#2&iID_MaDonVi=#3", "rptDuToanChiNganSachSuDung_PhanNganSachNghiepVu") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", "<%= MaND %>"));
              url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));          
              url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
              });
          }                                            
      </script>
 <script type="text/javascript">
     function Huy() {
         window.location.href = '<%=BackURL %>';
     }
 </script>
   <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToanChiNganSachSuDung_PhanNganSachNghiepVu", new {MaND=MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi }), "Xuất ra file Excel")%>
   <iframe src="<%=Url.Action("ViewPDF","rptDuToanChiNganSachSuDung_PhanNganSachNghiepVu",new{MaND=MaND,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet,iID_MaDonVi=iID_MaDonVi})%>" height="600px" width="100%"></iframe>
</body>
</html>
