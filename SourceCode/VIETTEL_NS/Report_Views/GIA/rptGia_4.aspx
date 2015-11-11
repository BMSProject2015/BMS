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
    String NamLamViec = Request.QueryString["NamLamViec"];

    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];

    String iID_MaSanPham = Request.QueryString["iID_MaSanPham"];
    String iID_MaChiTietGia = Request.QueryString["iID_MaChiTietGia"];
        
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = "0";
    }

    DataTable dtTrangThai = rptGia_4Controller.tbTrangThai();
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
 
    using (Html.BeginForm("EditSubmit", "rptGia_4", new { ParentID = ParentID, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaSanPham = iID_MaSanPham, iID_MaChiTietGia = iID_MaChiTietGia }))
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
        
    <%}
        
    %>
<%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptGia_4", new { iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaSanPham = iID_MaSanPham, iID_MaChiTietGia = iID_MaChiTietGia }), "Export to Excel")%>
   <iframe src="<%=Url.Action("ViewPDF","rptGia_4",new{iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaSanPham = iID_MaSanPham, iID_MaChiTietGia = iID_MaChiTietGia })%>" height="600px" width="100%"></iframe>
</body>
</html>
