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
    <title>TỔNG HỢP QUÂN SỐ QUYẾT TOÁN</title>
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
        String MaDV = Convert.ToString(ViewData["iMaDV"]);               
        String _Checked = "";
        DataTable dtDonVi = rptLuong_TongHopQuanSoController.GetDonVi(iNamLuong, iThangLuong, iTrangThai);        
        if (String.IsNullOrEmpty(MaDV))
        {
            MaDV = Guid.Empty.ToString();           
        }
        String iReport = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(iReport))
        {
            iReport = "A4";
        }
        String[] arrDonvi = MaDV.Split(',');                
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptLuong_TongHopQuanSo", new { Thang = iThangLuong, Nam = iNamLuong, TrangThai = iTrangThai, iMaDV = MaDV, KhoGiay = iReport });
        using (Html.BeginForm("EditSubmit", "rptLuong_TongHopQuanSo", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo danh sách chi trả tiền lương</span>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
                                                          
        </script>                  
        <div id="table_form2" class="table_form2">
            <div id="" style="width:900px; margin:0px auto; min-height:50px; height:auto;overflow:hidden; padding:5px 0px; ">
            
                <div style="float:left; width:550px; height:60px;">
                    <div id="DKleft" style="width:260px; float:left; ">
                        <div id="label" style="width:100px; float:left; height:60px; text-align:right;">
                            <p class="p"><span style="font-size:10pt; line-height:16px;"><%=NgonNgu.LayXau("Chọn tháng")%></span></p>
                            <p class="p"><span style="font-size:10pt; line-height:16px;"><%=NgonNgu.LayXau("Chọn năm")%></span></p>                      
                        </div>
                        <div id="txt" style="width:150px; float:right; height:60px; text-align:left;">
                            <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThangLuong, "iThangLuong", "", "class=\"input1_2\" style=\"width:100%;padding:2px;\" onchange=\"ChonDV()\"")%></p>                    
                            <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamHienTai, "iNamLuong", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\" onchange=\"ChonDV()\"")%></p>                        
                        </div>
                    </div><!--------End #DKLeft--------------->
                    <div id="DKMiddle" style="width:280px; height:60px; float:right">
                        <div id="Div1" style="width:100px; float:left; height:20px; text-align:right;">                        
                            <p class="p"><span style="font-size:10pt; line-height:16px;"><%=NgonNgu.LayXau("Chọn trạng thái")%></span></p> 
                        </div>
                        <div id="Div2" style="width:150px; float:right; height:20px; text-align:left;">                        
                            <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\" style=\"width: 100%; padding:2px;\" onchange=\"ChonDV()\"")%></p>                                        
                        </div>
                        <div style="clear:both; float:right;">
                            <fieldset style="border:1px dashed #cecece; border-radius:5px; padding:0px 2px 2px 2px; height:45px; float:right; width:270px;">
                                <legend class="p"><span style="font-size:10pt; line-height:16px;"><%=NgonNgu.LayXau("Chọn cơ giấy in")%>&nbsp;&nbsp;</span></legend>
                                <p class="p" style="line-height:18px; height:20px;"><%=MyHtmlHelper.Option(ParentID, "A4", iReport, "isReport", "", "onchange=\"Chon()\"")%><span style="font-size:10pt; line-height:16px; margin-right:5px;">&nbsp;&nbsp;Khổ giấy A4&nbsp;&nbsp;</span><span style="font-size:10pt; line-height:16px; float:right;">&nbsp;&nbsp;Khổ giấy A3&nbsp;&nbsp;</span>              
                                <%=MyHtmlHelper.Option(ParentID, "A3", iReport, "isReport", "", "style=\"float:right;\" onchange=\"Chon()\"")%>
                                </p>
                            </fieldset>
                            <div style="display:none;">
                                <%=MyHtmlHelper.TextBox(ParentID, iReport, "divReport", "", "")%>
                            </div>
                        </div>  
                    </div>
                </div>
                <div id="DKright" style="float:right; width:340px; height:70px; ">
                    <div id="donvi" style="float:left; width:340px; height:60px;">
                        <div id="labelDonvi" style="width:120px; float:left;">
                            <p class="p" style="text-align:center;"><%=NgonNgu.LayXau("Chọn đơn vị có dữ liệu")%></p>
                        </div>
                        <div class="div-label"style="width:196px;padding:1px 0px; float:right; text-align:left;border: 1px dashed #cecece;">
                            <div style="width: 100%; clear:both; min-height: 10px; max-height:60px; overflow: visible;">
                                <table class="mGrid" style="background-color:#ededed;">
                                    <tr style="height: 20px; font-size: 12px;">
                                        <td style="width: 20px; text-align:center; height:auto; line-height:7px;">
                                            <input type="checkbox" id="Checkbox1" onclick="ChonDonVi(this.checked)" style="cursor:pointer;" />
                                        </td>
                                        <td>&nbsp;&nbsp;Tất cả</td>
                                    </tr>
                                </table>                        
                            </div>
                            <div style="width: 100%; clear:both; min-height: 20px; max-height:50px; overflow: auto;" id="<%=ParentID %>_divDonVi"> 
                                <%rptLuong_TongHopQuanSoController rptTB1 = new rptLuong_TongHopQuanSoController();%>  
                                <%=rptTB1.obj_DSDonVi(iNamLuong, iThangLuong, iTrangThai,arrDonvi)%>
                            </div>     
                        </div>
                    </div>                    
                </div><!-------End #DKRight-------------->
                
                <div style="clear:both"></div>
                <div id="both" style="clear:both; min-height:25px; line-height:30px; margin-top:1px;">
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
    <%} %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        function Chon() {
            var Tens = document.getElementsByName("<%=ParentID %>_isReport");
            var idReport;
            var i = 0;
            for (i = 0; i < Tens.length; i++) {
                if (Tens[i].checked) {
                    idReport = Tens[i].value;
                }
            }
            document.getElementById("<%= ParentID %>_divReport").value = idReport;
        }
        function ChonDonVi(DonVi) {
            $("input:checkbox[check-group='iID_MaDonVi']").each(function (i) {
                if (DonVi) {
                    this.checked = true;
                }
                else {
                    this.checked = false;
                }
            });
        }
        function ChonDV() {
            var NamLuong = document.getElementById("<%=ParentID %>_iNamLuong").value
            var ThangLuong = document.getElementById("<%=ParentID %>_iThangLuong").value
            var TrangThai = document.getElementById("<%=ParentID %>_iTrangThai").value
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_DonVi?NamLuong=#0&ThangLuong=#1&DuyetLuong=#2&arrDV=#3", "rptLuong_TongHopQuanSo") %>');
            url = unescape(url.replace("#0", NamLuong));
            url = unescape(url.replace("#1", ThangLuong));
            url = unescape(url.replace("#2", TrangThai));
            url = unescape(url.replace("#3", "<%= arrDonvi %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
            });
        }        
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptLuong_TongHopQuanSo", new { Thang = iThangLuong, Nam = iNamLuong, TrangThai = iTrangThai, iMaDV = MaDV, KhoGiay = iReport }), "Xuất ra file Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
