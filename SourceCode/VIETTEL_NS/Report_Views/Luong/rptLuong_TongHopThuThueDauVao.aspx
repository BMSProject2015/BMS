<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>TỔNG HỢP THU THUẾ ĐẦU VÀO</title>
    <style type="text/css">
        .div-floatleft
        {                
            max-height:80px;            
        }
        .div-label
        {           
            font-size:13px;  
            padding:5px;                 
        }
        .div-txt
        {
            padding-top:5px;
            width:150px;            
        }    
        .p
        {
            height:23px;
            line-height:23px;
            padding:1px 2px;    
        }
    </style>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "Luong";
        String UserID = User.Identity.Name;
        String TuThang = Convert.ToString(ViewData["TuThang"]);
        if (String.IsNullOrEmpty(TuThang))
        {
            TuThang = CauHinhLuongModels.LayThangLamViec(UserID).ToString();
        }
        
        //tháng
        String DenThang = Convert.ToString(ViewData["DenThang"]);
        if (String.IsNullOrEmpty(DenThang))
        {
            DenThang = (CauHinhLuongModels.LayThangLamViec(UserID)+1).ToString();
        }         
        var dtThang = HamChung.getMonth(DateTime.Now, false, "", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();
        String URL = Url.Action("Index", "Luong_Report");
        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeLuong.ToString(), LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeLuong).ToString());
        DataRow dR = dtTrangThai.NewRow();
        dR["iID_MaTrangThaiDuyet"] = "0";
        dR["sTen"] = "--Tất cả--";
        dtTrangThai.Rows.InsertAt(dR, 0);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String iTrangThai = Convert.ToString(ViewData["iTrangThai"]);
        if (String.IsNullOrEmpty(iTrangThai))
        {
            if (dtTrangThai.Rows.Count > 0)
                iTrangThai = dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"].ToString();
            else
                iTrangThai = Guid.Empty.ToString();
        }
        String NamLuong = Convert.ToString(ViewData["iNam"]);
        if (String.IsNullOrEmpty(NamLuong))
        {
            NamLuong = CauHinhLuongModels.LayNamLamViec(UserID).ToString();
        }
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String urlReport = "";
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptLuong_TongHopThuThueDauVao", new { TuThang = TuThang, DenThang = DenThang, NamLuong=NamLuong, TrangThai = iTrangThai });
        using (Html.BeginForm("EditSubmit", "rptLuong_TongHopThuThueDauVao", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp thu thuế đầu vào</span>
                    </td>
                </tr>
            </table>
        </div>          
        <div id="table_form2" class="table_form2">
            <div id="" style="width:900px;margin:0px auto; height:35px; padding:5px;">   
                <div class="div-floatleft div-label" style="max-width:100px; text-align:center;float:left;">
                    <p class="p"><%=NgonNgu.LayXau("Kỳ tính thuế")%></p>
                </div>                             
                <div class="div-floatleft div-label" style="max-width:120px; text-align:center;float:left;">
                    <p class="p"><%=NgonNgu.LayXau("Từ tháng")%></p>
                </div>
                <div class="div-floatleft div-txt" style="max-width:120px; float:left;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slThang, TuThang, "iTuThang", "", "class=\"input1_2\" style=\"width:100%;padding:2px;\"")%></p>                    
                </div>                
                <div class="div-floatleft div-label" style="float:left;max-width:120px; text-align:center; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Đến tháng")%></p>                                            
                </div>
                <div class="div-floatleft div-txt" style="float:left;max-width:120px;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slThang, DenThang, "iDenThang", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\"")%></p>                                        
                </div>
                
                <div class="div-floatleft div-txt" style="max-width:130px; float:right;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\"")%></p>                                        
                </div>
                <div class="div-floatleft div-label" style="max-width:130px; text-align:center; padding-right:3px; padding-left:5px; margin-left:10px; float:right">
                    <p class="p"><%=NgonNgu.LayXau("Chọn trạng thái")%></p>                                            
                </div>       
                <div class="div-floatleft div-txt" style="max-width:100px; float:right;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLuong, "iNam", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\"")%></p>                                        
                </div>
                <div class="div-floatleft div-label" style="max-width:100px; text-align:center; padding-right:3px; padding-left:5px; margin-left:10px; float:right">
                    <p class="p"><%=NgonNgu.LayXau("Chọn năm")%></p></div>         
            </div>
            <div id="both" style="clear:both; min-height:30px; line-height:30px;">
                <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 5px auto; width:200px;">
                    <tr>
                        <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                        <td width="5px"></td>
                        <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
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
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptLuong_TongHopThuThueDauVao", new { TuThang = TuThang, DenThang = DenThang, NamLuong = NamLuong, TrangThai = iTrangThai }), "Xuất ra file Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>