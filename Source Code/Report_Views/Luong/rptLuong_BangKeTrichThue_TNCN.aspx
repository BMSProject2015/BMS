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
    <title>BẢNG TRÍCH KÊ THUẾ TNCN</title>
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
        String iThangLuong = Convert.ToString(ViewData["iThangLuong"]);
        String UserID = User.Identity.Name;
        if (String.IsNullOrEmpty(iThangLuong))
        {
            iThangLuong = CauHinhLuongModels.LayThangLamViec(UserID).ToString();
        }
        //tháng     
        var dtThang = HamChung.getMonth(DateTime.Now, false, "", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        String iNamLuong = Convert.ToString(ViewData["iNamLuong"]);

        if (String.IsNullOrEmpty(iNamLuong))
        {
            iNamLuong = CauHinhLuongModels.LayNamLamViec(UserID).ToString();
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
        String OnOrOff = Convert.ToString(ViewData["iOnOff"]);
        String isGroup = Convert.ToString(ViewData["iGroup"]);
        if (String.IsNullOrEmpty(isGroup))
        {
            isGroup = "rTheoTen";
        }
        String Pages = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(Pages))
        {
            Pages = "A3";
        }
        String _Checked="";
        if (String.IsNullOrEmpty(OnOrOff))
        {
            OnOrOff = "off";
        }
        if (OnOrOff == "on")
            _Checked = "checked=\"checked\"";
        else if (OnOrOff == "off")
            _Checked = "";
        
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptLuong_BangKeTrichThue_TNCN", new { Thang = iThangLuong, Nam = iNamLuong, TrangThai = iTrangThai, isALLDonVi = OnOrOff, KhoGiay = Pages, isGroup=isGroup });
        using (Html.BeginForm("EditSubmit", "rptLuong_BangKeTrichThue_TNCN", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo bảng kê trích thuế thu nhập cá nhân</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:700px; max-width:800px; margin:0px auto; height:55px; padding:5px;">                
                <div style="width:auto; float:left">
                    <div style="float:left; width:360px;">              
                        <div class="div-label" style="width:110px; text-align:right;float:left;">
                            <p class="p"><%=NgonNgu.LayXau("Chọn tháng")%></p>
                            <p class="p"><%=NgonNgu.LayXau("Chọn năm")%></p>
                            <p class="p"><%=NgonNgu.LayXau("Chọn trạng thái")%></p> 
                        </div>
                        <div class="div-label" style="width:225px; float:right; text-align:center; ">
                            <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThangLuong, "iThangLuong", "", "class=\"input1_2\" style=\"width:80%;padding:2px;\"")%></p>                    
                            <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamHienTai, "iNamLuong", "", "class=\"input1_2\" style=\"width: 80%;padding:2px;\"")%></p>
                            <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\" style=\"width: 80%;padding:2px;\"")%></p>                                        
                        </div>                       
                    </div>      
                    <div style="float:right; width:150px; " class="div-label">                  
                        <p class="p"><%=MyHtmlHelper.Option(ParentID, "rTheoTen", isGroup, "isGroup", "", "onchange=\"Chon()\"")%>&nbsp;&nbsp;Bảng kê theo tên</p>                  
                        <p class="p"><%=MyHtmlHelper.Option(ParentID, "rTheoDonVi", isGroup, "isGroup", "", "onchange=\"Chon()\"")%>&nbsp;&nbsp;Bảng kê theo đơn vị</p>                                
                        <p class="p"><input type="checkbox" value="rAllDonVi" <%=_Checked %>  id="rAllDonVi" style="cursor:pointer;" onclick="ChonDonVi(this.checked)" />&nbsp;&nbsp;In toàn bộ đơn vị</p>                                            
                        <div style="display:none;">
                            <%=MyHtmlHelper.TextBox(ParentID, OnOrOff, "divOnOrOff", "", "")%>
                        </div>    
                        <div style="display:none;">
                            <%=MyHtmlHelper.TextBox(ParentID, isGroup, "divGroup", "", "")%>
                        </div>
                    </div>  
                </div>  
                <div style="width:150px; float:right;" class="div-label">
                    <fieldset style="padding:3px; border:1px solid #cecece; border-radius:5px;">
                        <legend>&nbsp;Chọn cỡ giấy in &nbsp;</legend>
                        <p class="p"><%=MyHtmlHelper.Option(ParentID, "A3", Pages, "Pages", "", "onchange=\"ChonPage()\"")%>&nbsp;&nbsp;Khổ giấy A3</p>                  
                        <p class="p"><%=MyHtmlHelper.Option(ParentID, "A4", Pages, "Pages", "", "onchange=\"ChonPage()\"")%>&nbsp;&nbsp;Khổ giấy A4</p>
                    </fieldset>
                    <div style="display:none;">
                        <%=MyHtmlHelper.TextBox(ParentID, Pages, "divPages", "", "")%>
                    </div>
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
        function Huy() {
            window.location.href = '<%=URL %>';
        }               
        function Chon() {
            var TenLoaiThang_Quy = document.getElementsByName("<%=ParentID %>_isGroup");
            var LoaiThang_Quy;                 
            var i = 0;
            for (i = 0; i < TenLoaiThang_Quy.length; i++) {
                if (TenLoaiThang_Quy[i].checked) {
                    LoaiThang_Quy = TenLoaiThang_Quy[i].value;
                }
            }
            document.getElementById("<%= ParentID %>_divGroup").value = LoaiThang_Quy;                      
        }
        function ChonPage() {
            var TenPage = document.getElementsByName("<%=ParentID %>_Pages");
            var pages;
            var i = 0;
            for (i = 0; i < TenPage.length; i++) {
                if (TenPage[i].checked) {
                    pages = TenPage[i].value;
                }
            }
            document.getElementById("<%= ParentID %>_divPages").value = pages;  
        }
        function ChonDonVi(DonVi) {
            $("input:checkbox").each(function (i) {
                if (DonVi) {
                    document.getElementById("<%= ParentID %>_divOnOrOff").value = "on";
                }
                else {
                    document.getElementById("<%= ParentID %>_divOnOrOff").value = "off";
                }
            });
        }               
    </script>
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptLuong_BangKeTrichThue_TNCN", new { Thang = iThangLuong, Nam = iNamLuong, TrangThai = iTrangThai, isALLDonVi = OnOrOff, KhoGiay = Pages, isGroup = isGroup }), "Xuất ra file Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
