<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.GIA" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
 <style type="text/css">
    
     .style1
     {
         width: 423px;
     }
   
 </style>
</head>
<body>
    <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoGia";
    String UserID = User.Identity.Name;
    String NamLamViec = Request.QueryString["NamLamViec"];
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DateTime.Now.Year.ToString();
    }

    String iID_MaTrangThaiDuyet = "0";
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = "0";
    }

    DataTable dtTrangThai = rptGia_7bController.tbTrangThai();
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
    dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm --";
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");

    DataTable dtDonVi = GIA_ReportController.getDtDonVi(UserID, "2");
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();
    String URL = Url.Action("Index", "Gia_Report");
    using (Html.BeginForm("EditSubmit", "rptGia_7b", new { ParentID = ParentID, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet}))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Bảng Tổng hợp tính giá sản phẩm</span>
                    </td>
                    
                </tr>
            </table>
        </div>        
        <div id="Div1" style="background-color:#F0F9FE;">
          <div id="rptMain" style="background-color:#F0F9FE;padding-top:5px;">
  <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 68px">
   <tr>
    <td align="right" class="style1"><b>Năm làm việc : </b></td>
    <td width="494"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%></td>
  </tr>
  <tr>
    <td align="right" class="style1"><b>Đơn vị : </b></td>
    <td width="494"><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%></td>
  </tr>
  <tr>
    <td colspan="2" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
  </tr>
</table>
            </div>
        </div>
        
    <%}
        
    %>
<%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptGia_7b", new { iID_MaDonVi = iID_MaDonVi, NamLamViec = NamLamViec, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet}), "Export to Excel")%>
   <iframe src="<%=Url.Action("ViewPDF","rptGia_7b",new{iID_MaDonVi = iID_MaDonVi, NamLamViec = NamLamViec, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet})%>" height="600px" width="100%"></iframe>
</body>
</html>
<script type="text/javascript">
    function Huy() {
        window.location.href = '<%=URL %>';
    }
    </script>