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
    <title>DANH SÁCH CÁ NHÂN NỘP THUẾ ĐẦU VÀO</title>
     <style type="text/css">
        .div-floatleft
        {
            float:left;    
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
        String iThangLuong = Convert.ToString(ViewData["iThangLuong"]);
        if (String.IsNullOrEmpty(iThangLuong))
        {
            iThangLuong = DanhMucModels.ThangLamViec(UserID).ToString();
        }
        //tháng     
        var dtThang = HamChung.getMonth(DateTime.Now, false, "", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        String iNamLuong = Convert.ToString(ViewData["iNamLuong"]);

        if (String.IsNullOrEmpty(iNamLuong))
        {
            iNamLuong = DanhMucModels.NamLamViec(UserID).ToString();
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
        //Đơn vị        
        //dtDonVi.Dispose();
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
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = rptLuong_DSCaNhan_NopThueDauVaoController.Get_DonVi_(iNamLuong, iThangLuong,iTrangThai);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            iID_MaDonVi = dtDonVi.Rows.Count > 0 ? Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]) : Guid.Empty.ToString();
        }  
        String URL = Url.Action("Index", "Luong_Report");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String urlReport = "";
        String iMaBL = Convert.ToString(Request.QueryString["iID_MaBangLuong"]);
        if (String.IsNullOrEmpty(iMaBL))
        {
            iMaBL = Guid.Empty.ToString();
        }
        urlReport = Url.Action("ViewPDF", "rptLuong_DSCaNhan_NopThueDauVao", new { iMaBL=iMaBL });
        using (Html.BeginForm("EditSubmit", "rptLuong_DSCaNhan_NopThueDauVao", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo danh sách cá nhân nộp thuế đầu vào</span>
                    </td>
                </tr>
            </table>
        </div>          
        <div id="table_form2" class="table_form2">
            <div id="" style="width:850px;margin:0px auto; height:35px; padding:5px; display:none;">                
                <div class="div-floatleft div-label" style="max-width:100px; text-align:center; float:left;">
                    <p class="p"><%=NgonNgu.LayXau("Chọn tháng")%></p>
                </div>
                <div class="div-floatleft div-txt" style="max-width:100px; float:left;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThangLuong, "iThangLuong", "", "class=\"input1_2\" style=\"width:100%;padding:2px;\" onchange=\"ChonDonVi()\"")%></p>                    
                </div>                
                <div class="div-floatleft div-label" style="float:left;max-width:100px; text-align:center; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Chọn năm")%></p>                                            
                </div>
                <div class="div-floatleft div-txt" style="max-width:100px;float:left;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamHienTai, "iNamLuong", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\" onchange=\"ChonDonVi()\"")%></p>                                        
                </div>               
                <div class="div-floatleft div-txt" style="float:right;max-width:100px;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\"")%></p>                                        
                </div>
                <div class="div-floatleft div-label" style="float:right;max-width:100px; text-align:center; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Chọn trạng thái")%></p>                                            
                </div>                
                <div class="div-floatleft div-txt" style="float:right;max-width:160px;">
                    <div id="<%=ParentID %>_divDonVi" class="p">
                        <%rptLuong_DSCaNhan_NopThueDauVaoController rptTB1 = new rptLuong_DSCaNhan_NopThueDauVaoController();%>                                    
                        <%=rptTB1.obj_DSDonVi(ParentID,iNamLuong,iThangLuong,iTrangThai,iID_MaDonVi)%>
                    </div>                                         
                </div>
                <div class="div-floatleft div-label" style="float:right;max-width:100px; text-align:center; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Chọn đơn vị")%></p>                                            
                </div>
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
         function ChonDonVi() {
             var NamLuong = document.getElementById("<%=ParentID %>_iNamLuong").value
             var ThangLuong = document.getElementById("<%=ParentID %>_iThangLuong").value
             var TrangThai = document.getElementById("<%=ParentID %>_iTrangThai").value
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&NamLuong=#1&ThangLuong=#2&TrangThai=#3&iID_MaDonVi=#4", "rptLuong_DSCaNhan_NopThueDauVao") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", NamLuong));
             url = unescape(url.replace("#2", ThangLuong));
             url = unescape(url.replace("#3", TrangThai));
             url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
             });
         }                                            
      </script>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
    </script>
    <%}
        dtDonVi.Dispose();
        dtTrangThai.Dispose();
         %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptLuong_DSCaNhan_NopThueDauVao", new { iMaBL=iMaBL }), "Xuất ra file Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>