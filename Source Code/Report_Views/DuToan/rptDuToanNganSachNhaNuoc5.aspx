<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    
</head>
<body>
<%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoNganSachNam";
    String MaND = User.Identity.Name;
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    //Trang Thai Duyet
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    DataTable dtTrangThai = rptDuToanNganSachNhaNuoc5Controller.tbTrangThai();
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
    DataTable dtNguonDonVi = rptDuToanNganSachNhaNuoc5Controller.HienThiDonViTheoNam(MaND,iID_MaTrangThaiDuyet);
    SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "sTen");
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

    String urlExport = Url.Action("ExportToExcel", "rptDuToanNganSachNhaNuoc5", new { MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "1" });
    using (Html.BeginForm("EditSubmit", "rptDuToanNganSachNhaNuoc5", new { ParentID = ParentID, MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }))
    {
    %>   
    <div class="box_tong" style="background-color:#F0F9FE;">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo dự toán ngân sách nhà nước giao</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
  <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
 <tr>
    <td width="25%"></td>
    <td width="10%" class="td_form2_td1">Chọn trạng thái :</td>
    <td width="10%"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonDonVi()\"")%></td>
    <td width="10%" class="td_form2_td1">Chọn đơn vị :</td>
    <td width="10%"> <div id="<%=ParentID %>_tdDonVi">
                                     <%rptDuToanNganSachNhaNuoc5Controller rptTB1 = new rptDuToanNganSachNhaNuoc5Controller();%>                                    
                                     <%=rptTB1.obj_DonViTheoNam(ParentID,MaND,iID_MaDonVi,iID_MaTrangThaiDuyet)%>
                            </div></td>
    <td></td>
  </tr>
  
  <tr>
  <td></td>
       <td colspan="4"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
                           <td></td>
  </tr>
</table>
            </div>
        </div>
          <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Xuất ra Excel")%>
    </div>
          <script type="text/javascript">
              function Huy() {
                  window.location.href = '<%=BackURL%>';
              }
    </script>

    <script type="text/javascript">
        function ChonDonVi() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&iID_MaDonVi=#2&iID_MaTrangThaiDuyet=#3", "rptDuToanNganSachNhaNuoc5") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= MaND %>"));
            url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
            });
        }                                            
    </script>
    </div>
    <%} %>
    <div ></div>
    
  <%
    dtNam.Dispose();
   dtNguonDonVi.Dispose();
%>
   <iframe src="<%=Url.Action("ViewPDF","rptDuToanNganSachNhaNuoc5",new{MaND=MaND,iID_MaDonVi=iID_MaDonVi,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet})%>" height="600px" width="100%"></iframe>
</body>
</html>
