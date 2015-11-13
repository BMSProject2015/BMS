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
    <link type="text/css" rel="Stylesheet" href="../../Content/style.css" />
</head>
<body>
   <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoTongHop";
    String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);
    String iQuy = Convert.ToString(ViewData["iQuy"]);
    String iID_MaLoaiDoanhNghiep = Convert.ToString(ViewData["iID_MaLoaiDoanhNghiep"]);
    if (String.IsNullOrEmpty(iNamLamViec))
    {
        iNamLamViec = DateTime.Now.Year.ToString();
    }
    String To = Convert.ToString(ViewData["To"]);
    if (String.IsNullOrEmpty(To))
    {
        To = "";
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

    DataTable dtLoaiHinh = DanhMucModels.DT_DanhMuc("TCDN_LoaiHinhDN", true, "--- Chọn loại hình doanh nghiệp ---");
    SelectOptionList slLoaiHinh = new SelectOptionList(dtLoaiHinh, "iID_MaDanhMuc", "sTen");
    dtLoaiHinh.Dispose();

    DataTable dtQuy = DanhMucModels.DT_Quy();
    dtQuy.Rows[0]["TenQuy"] = "-- Chọn thời gian --";
    dtQuy.Rows[1]["TenQuy"] = "3 tháng";
    dtQuy.Rows[2]["TenQuy"] = "6 tháng";
    dtQuy.Rows[3]["TenQuy"] = "9 tháng";
    dtQuy.Rows[4]["TenQuy"] = "Cả năm";
       
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();
       
    String BackURL = Url.Action("Index", "TCDN_Report", new { sLoai="0"});
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
  //  if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptTCDN_BaoCaoTongHop", new { iNamLamViec = iNamLamViec,iQuy=iQuy,iID_MaLoaiDoanhNghiep=iID_MaLoaiDoanhNghiep});
    using (Html.BeginForm("EditSubmit", "rptTCDN_BaoCaoTongHop", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Tổng hợp báo cáo tài chính năm của công ty cổ phần</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="table_form2" class="table_form2">
            <div id="" style="width:1024px; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:visible; ">
                <ul class="inlineBlock">
                    <li>                        
                        <span style="float:left; text-align:center; width:100px;"><%=NgonNgu.LayXau("Chọn năm")%></span>
                        <span style="float:right; text-align:center; width:100px;"><%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\"")%></span>                        
                    </li><!-------------End năm---------------------->
                    <li>                       
                        <span style="float:left; text-align:center; width:150px;"><%=NgonNgu.LayXau("Loại hình doanh nghiệp")%></span>
                        <span style="float:right; text-align:center; width:250px;"><%=MyHtmlHelper.DropDownList(ParentID, slLoaiHinh, iID_MaLoaiDoanhNghiep, "iID_MaLoaiDoanhNghiep", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\"")%></span>                        
                    </li><!-------------End loại hình doanh nghiệp---------------------->
                    <li>                       
                        <span style="float:left; text-align:center; width:100px;"><%=NgonNgu.LayXau("Thời gian")%></span>
                        <span style="float:right; text-align:center; width:140px;"><%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\"")%></span>                         
                    </li><!-------------End quý--------------------->                    
                </ul>
                <div id="both" style="clear:both; min-height:30px; line-height:30px; margin-bottom:-5px; ">
                    <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 5px auto; width:200px;">
                        <tr>
                            <td><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                            <td width="5px"></td>
                            <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                        </tr>
                    </table>   
                </div>
            </div>
        </div>        
    </div>
    <%}
        dtNam.Dispose();    
    %> 
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script> 
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTCDN_BaoCaoTongHop", new { iNamLamViec = iNamLamViec, iQuy = iQuy, iID_MaLoaiDoanhNghiep = iID_MaLoaiDoanhNghiep }), "Export to Excel")%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
