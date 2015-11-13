<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.Luong" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .div-floatleft
        {                
            max-height:80px;            
        }
        .div-label
        {           
            font-size:13px;  
            padding:5px 0px;                 
        }
        .div-txt
        {
            padding-top:5px;                  
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
        String iTuThang = Convert.ToString(ViewData["iTuThang"]);
        String UserID = User.Identity.Name;
        if (String.IsNullOrEmpty(iTuThang))
        {
            //iTuThang = CauHinhLuongModels.LayThangLamViec(UserID).ToString();
            iTuThang = "1";
        }
        //tháng     
        var dtThang = HamChung.getMonth(DateTime.Now, false, "", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        String iDenThang = Convert.ToString(ViewData["iDenThang"]);

        if (String.IsNullOrEmpty(iDenThang))
        {
            //iDenThang = (CauHinhLuongModels.LayThangLamViec(UserID)+1).ToString();
            iDenThang = "12";
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
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String urlReport = "";      
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptLuong_BangKeChiTiet", new { TuThang = iTuThang, DenThang = iDenThang,  Duyet = iTrangThai });
        using (Html.BeginForm("EditSubmit", "rptLuong_BangKeChiTiet", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Bảng kê chi tiết</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:850px; max-width:850px; margin:0px auto; padding:0px 0px; overflow:visible; ">
                <div id="main" style="width:850px; float:left; border:1px solid #cecece; border-radius:3px; height:40px; text-align:center;padding:4px 0px 2px 0px;">              
                        <div style="float:left; width:350px; ">                                         
                            <div class="div-label" style="width:200px; text-align:center;float:left;">
                                <p class="p"><span style="float:left; text-align:center; width:95px;"><%=NgonNgu.LayXau("Kỳ tính thuế")%></span><span style="float:right;text-align:center; width:95px;"><%=NgonNgu.LayXau("Từ tháng")%></span> </p>                      
                            </div>
                            <div class="div-label" style="width:150px; float:right; text-align:left; ">
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iTuThang, "iTuThang", "", "class=\"input1_2\" style=\"width:100%; padding:4px;\"")%></p>                                    
                            </div>                       
                        </div><!-----------End tháng,năm, trạng thái-----------> 
                        <div style="float:left; width:250px;">              
                            <div class="div-label" style="width:100px; text-align:center;float:left;">                                
                                <p class="p"><%=NgonNgu.LayXau("Đến tháng")%></p>                                
                            </div>
                            <div class="div-label" style="width:150px; float:right; text-align:left; ">                               
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iDenThang, "iDenThang", "", "class=\"input1_2\" style=\"width: 100%;padding:4px;\" ")%></p>                                
                            </div>                       
                        </div><!-----------End tháng,năm, trạng thái----------->      
                        <div style="float:right; width:250px;">              
                            <div class="div-label" style="width:100px; text-align:center;float:left;">                               
                                <p class="p"><%=NgonNgu.LayXau("Chọn trạng thái")%></p> 
                            </div>
                            <div class="div-label" style="width:150px; float:right; text-align:left; ">                               
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\" style=\"width: 100%;padding:4px;\" ")%></p>                                        
                            </div>                       
                        </div><!-----------End tháng,năm, trạng thái----------->                     
                </div>
                <div id="both" style="clear:both; min-height:30px; line-height:30px; margin-bottom:-5px; ">
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
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }        
    </script>
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptLuong_BangKeChiTiet", new { TuThang = iTuThang, DenThang = iDenThang, Duyet = iTrangThai }), "Xuất ra file Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
