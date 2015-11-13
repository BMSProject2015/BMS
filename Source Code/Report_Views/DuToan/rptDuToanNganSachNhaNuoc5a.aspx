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
  
    String ParentID = "BaoCaoNganSachNam";
    String NamLamViec = Request.QueryString["NamLamViec"];
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DateTime.Now.Year.ToString();
    }
    String sNG = Request.QueryString["sNG"];

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
    DataTable dtTrangThai = rptDuToanNganSachNhaNuoc5aController.tbTrangThai();
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
    DataTable dtNguonDonVi = rptDuToanNganSachNhaNuoc5aController.Get_sNG();
    SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "sTenKhoa", "sTen");

    if (String.IsNullOrEmpty(sNG))
    {
        if (dtNguonDonVi.Rows.Count > 0)
        {
            sNG = Convert.ToString(dtNguonDonVi.Rows[0]["sTenKhoa"]);
        }
        else
        {
            sNG = Guid.Empty.ToString();
        }
    }
    String sLNS = Request.QueryString["sLNS"];
    DataTable dtLNS = DanhMucModels.NS_LoaiNganSachNhaNuoc();
    SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
    if (String.IsNullOrEmpty(sLNS))
    {
        if (dtLNS.Rows.Count > 0)
        {
            sLNS = Convert.ToString(dtLNS.Rows[0]["sLNS"]);
        }
        else
        {
            sLNS = Guid.Empty.ToString();
        }
    }
    dtLNS.Dispose();
    String urlExport = Url.Action("ExportToExcel", "rptDuToanNganSachNhaNuoc5a", new { sNG = sNG, sLNS = sLNS, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "0" });
    using (Html.BeginForm("EditSubmit", "rptDuToanNganSachNhaNuoc5a", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong" style="background-color:#F0F9FE;">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo DỰ toán ngân sách nhà nước giao</span>
                    </td>
                </tr>
            </table>
        </div>
          <div id="Div1">
                <div id="Div2">   

 <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
  <tr>
    <td width="10%"></td>
    <td width="10%" class="td_form2_td1">Trạng Thái :</td>
    <td width="10%"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\"")%></td>
    <td width="10%" class="td_form2_td1">Chọn LNS :</td>
    <td width="20%"><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\" style=\"width: 100%\"")%></td>
    <td width="10%" class="td_form2_td1">Chọn Ngành :</td>
    <td width="10%"><%=MyHtmlHelper.DropDownList(ParentID, slTenDonVi, sNG, "sNG", "", "class=\"input1_2\" style=\"width: 100%\"")%></td>
    <td></td>
  </tr>
  
  <tr>
  <td></td>
       <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
    </div>
    <%} %>
    <div ></div>
    
  <%
    dtNam.Dispose();
   dtNguonDonVi.Dispose();
%>
   <iframe src="<%=Url.Action("ViewPDF","rptDuToanNganSachNhaNuoc5a",new{sNG=sNG,sLNS=sLNS,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet})%>" height="600px" width="100%"></iframe>
</body>
</html>
