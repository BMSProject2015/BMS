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
        String MaCB = Convert.ToString(ViewData["iMaCB"]);
        
        String Pages = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(Pages))
        {
            Pages = "A4";
        }
        String _Checked="";
        DataTable dtDonVi = rptLuong_Giay_CCTCController.GetDonVi(iNamLuong,iThangLuong,iTrangThai);
        DataTable dtCanBo = rptLuong_Giay_CCTCController.GetCanBo(iNamLuong, iThangLuong, iTrangThai, MaDV);
        if (String.IsNullOrEmpty(MaDV))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                MaDV = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
            }
            else
            {
                MaDV = Guid.Empty.ToString();
            }
        }
        if (String.IsNullOrEmpty(MaCB))
        {
            MaCB = Guid.Empty.ToString();            
        }
        String[] arrDonvi=MaDV.Split(',');
        String[] arrCanBo = MaCB.Split(',');
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptLuong_Giay_CCTC", new { NamLuong = iNamLuong, ThangLuong = iThangLuong, DonVi = MaDV, MaCB = MaCB, Duyet = iTrangThai, KhoGiay = Pages });
        using (Html.BeginForm("EditSubmit", "rptLuong_Giay_CCTC", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Giấy cung cấp tài chính</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:1024px; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:hidden; ">
                <div id="main" style="width:1024px; float:left; margin-bottom:10px;border:1px solid #cecece; border-radius:3px; height:140px;">                
                    <div style="float:left; height:130px; " id="tops">
                        <div style="float:left; width:250px; ">              
                            <div class="div-label" style="width:100px; text-align:right;float:left;">
                                <p class="p"><%=NgonNgu.LayXau("Chọn tháng")%></p>
                                <p class="p"><%=NgonNgu.LayXau("Chọn năm")%></p>
                                <p class="p"><%=NgonNgu.LayXau("Chọn trạng thái")%></p> 
                            </div>
                            <div class="div-label" style="width:150px; float:right; text-align:left; ">
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThangLuong, "iThangLuong", "", "class=\"input1_2\" style=\"width:100%;padding:2px;\" onchange=\"ChonDV()\"")%></p>                    
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamHienTai, "iNamLuong", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\" onchange=\"ChonDV()\"")%></p>
                                <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\" style=\"width: 100%;padding:2px;\" onchange=\"ChonDV()\"")%></p>                                        
                            </div>                       
                        </div><!-----------End tháng,năm, trạng thái----------->                    
                        <div id="donvi" style="float:left; height:85px; width:270px; max-width:270px;">
                            <div class="div-label" style="width:90px; text-align:center;float:left;">
                                <p class="p"><%=NgonNgu.LayXau("Chọn đơn vị")%></p>
                            </div>
                            <div class="div-label"style="width:180px; float:right; text-align:left; ">
                                <div style="width: 100%; clear:both; min-height: 10px; max-height:80px; overflow: visible;">
                                    <table class="mGrid" style="background-color:#ededed;">
                                        <tr style="height: 20px; font-size: 12px;">
                                            <td style="width: 20px; text-align:center; height:auto; line-height:7px;">
                                                <input type="checkbox" id="Checkbox1" onclick="ChonDonVi(this.checked)" style="cursor:pointer;" />
                                            </td>
                                            <td>&nbsp;&nbsp;Tất cả</td>
                                        </tr>
                                    </table>                        
                                </div>
                                <div style="width: 100%; clear:both; min-height: 20px; max-height:60px; overflow: auto;" id="<%=ParentID %>_divDonVi"> 
                                    <%rptLuong_Giay_CCTCController rptTB1 = new rptLuong_Giay_CCTCController();%>                                    
                                    <%=rptTB1.obj_DSDonVi(iNamLuong,iThangLuong,iTrangThai,arrDonvi)%>
                                </div>     
                            </div>
                        </div><!-----------End #donvi----------->
                        <div style="width:100%; clear:both; margin-bottom:10px;">
                            <fieldset style="padding:2px; border:1px dashed #cecece; border-radius:5px; height:45px; float:left; width:200px; margin-right:3px;">
                                <legend class="p"><span style="font-size:10pt; line-height:16px;">&nbsp;<%=NgonNgu.LayXau("Chọn cỡ giấy in")%>&nbsp;</span></legend>
                                <p class="p"><%=MyHtmlHelper.Option(ParentID, "A3", Pages, "Pages", "", "onchange=\"ChonPage()\" style=\"float:left;\"")%><span style="float:left;font-size:10pt; line-height:16px;">&nbsp;&nbsp;Khổ giấy A3</span>&nbsp;&nbsp;                  
                                <span style="float:right; font-size:10pt; line-height:16px;">&nbsp;&nbsp;Khổ giấy A4</span><%=MyHtmlHelper.Option(ParentID, "A4", Pages, "Pages", "", "onchange=\"ChonPage()\" style=\"float:right;\"")%></p>

                            </fieldset>
                            <div style="display:none;">
                                <%=MyHtmlHelper.TextBox(ParentID, Pages, "divPages", "", "")%>
                            </div>
                             <fieldset style="border:1px dashed #cecece; border-radius:5px; padding:2px; height:45px; float:right; width:280px; margin-left:3px;">
                                <legend class="p"><span style="font-size:10pt; line-height:16px;">&nbsp;&nbsp;<%=NgonNgu.LayXau("Loại biểu")%>&nbsp;&nbsp;</span></legend>
                                <p class="p"><%=MyHtmlHelper.Option(ParentID, "rTheoDV", "LoaiBieu", "isReport", "", "onchange=\"Chon()\"")%><span style="font-size:10pt; line-height:16px;">&nbsp;&nbsp;Theo đơn vị&nbsp;&nbsp;</span>                 
                                <%=MyHtmlHelper.Option(ParentID, "rLietKe", "LoaiBieu", "isReport", "", "onchange=\"Chon()\"")%><span style="font-size:10pt; line-height:16px;">&nbsp;&nbsp;Liệt kê&nbsp;&nbsp;</span>
                                <%=MyHtmlHelper.Option(ParentID, "rCoHoTen", "LoaiBieu", "isReport", "", "onchange=\"Chon()\"")%><span style="font-size:10pt; line-height:16px;">&nbsp;&nbsp;Theo họ tên</span></p>
                            </fieldset>
                            <div style="display:none;">
                                <%=MyHtmlHelper.TextBox(ParentID, "", "divReport", "", "")%>
                            </div>
                        </div> 
                    </div><!------------End #top----------->           
                    <div id="middle" style=" float:left; height:135px;  padding-right:0px; width:480px;">                    
                        <div id="canbo" style="float:left; width:100%; height:85px; overflow:visible;">
                            <div class="div-label" style="width:98px; text-align:center;float:left;">
                                <p class="p"><%=NgonNgu.LayXau("Chọn cán bộ")%></p>
                            </div>
                            <div class="div-label"style="width:380px; max-width:385px; float:left; text-align:center; overflow:auto;">
                                <div style="width: 100%; clear:both; min-height: 10px; max-height:90px; overflow: auto;">
                                    <table class="mGrid" style="background-color:#ededed;">
                                        <tr style="height: 20px; font-size: 12px;">
                                            <td style="width: 17px; text-align:center; height:auto; line-height:7px;">
                                                <input type="checkbox" id="Checkbox2" onclick="ChonCanBo(this.checked)" style="cursor:pointer;" />
                                            </td>
                                            <td style="width:26px;"><%=NgonNgu.LayXau("Mã đơn vị")%></td>
                                            <td style="width:28px;"><%=NgonNgu.LayXau("Cấp bậc")%></td>
                                            <td style="width:76px; "><%=NgonNgu.LayXau("Họ tên")%></td>
                                            <td style="width:57px; text-align:left;"><%=NgonNgu.LayXau("Số sổ lương")%>
                                            </td>
                                        </tr>
                                    </table>                        
                                </div>
                                <div style="width: 100%; clear:both; min-height: 20px; max-height:100px; overflow: auto;" id="<%=ParentID %>_divCanBo">                                                                     
                                    <%=rptTB1.obj_DSCanBo(iNamLuong,iThangLuong,iTrangThai,MaDV,arrCanBo)%>
                                </div>     
                            </div>
                        </div><!-----------End #canbo----------->
                    </div><!------------End #middle----------->
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
            $("input:checkbox[check-group='iID_MaDonVi']").each(function (i) {
                if (DonVi) {
                    this.checked = true;                    
                }
                else {
                    this.checked = false;
                }
                ChonCB();              
            });
        }        
        function ChonDV() {
            var NamLuong = document.getElementById("<%=ParentID %>_iNamLuong").value
            var ThangLuong = document.getElementById("<%=ParentID %>_iThangLuong").value
            var TrangThai = document.getElementById("<%=ParentID %>_iTrangThai").value
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_DonVi?NamLuong=#0&ThangLuong=#1&DuyetLuong=#2&arrDV=#3", "rptLuong_Giay_CCTC") %>');
            url = unescape(url.replace("#0", NamLuong));
            url = unescape(url.replace("#1", ThangLuong));
            url = unescape(url.replace("#2", TrangThai));
            url = unescape(url.replace("#3", "<%= arrDonvi %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
            });
            ChonCB();
        }
        function ChonCB() {
            var NamLuong = document.getElementById("<%=ParentID %>_iNamLuong").value
            var ThangLuong = document.getElementById("<%=ParentID %>_iThangLuong").value
            var TrangThai = document.getElementById("<%=ParentID %>_iTrangThai").value
            var MaDV = "";
            $("input:checkbox[check-group='iID_MaDonVi']").each(function (i) {
                if (this.checked) {
                    if (MaDV != "") MaDV += ",";
                    MaDV += this.value;
                }
            });
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_CanBo?NamLuong=#0&ThangLuong=#1&Duyet=#2&Donvi=#3&arrCB=#4", "rptLuong_Giay_CCTC") %>');
            url = unescape(url.replace("#0", NamLuong));
            url = unescape(url.replace("#1", ThangLuong));
            url = unescape(url.replace("#2", TrangThai));
            url = unescape(url.replace("#3", MaDV));
            url = unescape(url.replace("#4", "<%= arrCanBo %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divCanBo").innerHTML = data;
            });
        }
        function ChonCanBo(CB) {
            $("input:checkbox[check-group='iID_MaCanBo']").each(function (i) {
                if (CB) {
                    this.checked = true;
                }
                else {
                    this.checked = false;
                }
            });
        }    
    </script>
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptLuong_Giay_CCTC", new { NamLuong = iNamLuong, ThangLuong = iThangLuong, DonVi = MaDV, MaCB = MaCB, Duyet = iTrangThai, KhoGiay = Pages }), "Xuất ra file Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
