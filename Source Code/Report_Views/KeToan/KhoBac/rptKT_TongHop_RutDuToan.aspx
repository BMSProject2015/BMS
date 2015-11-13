<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.KhoBac" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <style type="text/css">
        ul.inlineBlock{
	        list-style: none inside;			
        }
        ul.inlineBlock li{			
	        /*-webkit-box-shadow: 2px 2px 0 #cecece;
	        box-shadow: 2px 2px 0 #cecece;	*/	
	        -webkit-box-shadow: rgba(200, 200, 200, 0.7) 0 4px 10px -1px;
            box-shadow: rgba(200, 200, 200, 0.7) 0 4px 10px -1px;
	        padding: 2px 5px;
	        display: inline-block;
	        vertical-align: middle; /*Mở comment để xem thuộc tính vertical-align*/
	        margin-right: 3px;
	        margin-left: 0px;
	        font-size: 13px;			
	        border-radius: 3px;
	        position: relative;
	    /*fix for IE 7*/
	        zoom:1;
	        *display: inline;		        
        }
        ul.inlineBlock li span
        {
            padding:2px 1px;   
        }
        ul.inlineBlock li p{
            padding:1px;    
        }        
        div.login1 {
            text-align : center;    
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }    
        div.login1 a {
            color: white;
            text-decoration: none;
            font: bold 16px "Museo 700";
            display: block;
            width: 50px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px 1px;
            -webkit-border-radius:2px;
            border-radius:2px;
        }    
        div.login1 a.active {
            background-position:  20px -29px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
             
        ul.inlineBlock li fieldset .div
        {
            width:90px; height:23px; display:inline-block; text-align:center; padding:1px; font-size:14px;-moz-border-radius:3px;-webkit-border-radius:3px; border-radius:3px;
            font-family:Tahoma Arial; color:Black; line-height:23px; cursor:pointer; background-color:#dedede;  
        }
        select
        {
            border:1px solid #dedede;
            }
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "KTKB";
        String Nam = Convert.ToString(ViewData["iNam"]);
        String UserID = User.Identity.Name;
        if (String.IsNullOrEmpty(Nam))
        {
            Nam = DanhMucModels.NamLamViec(UserID).ToString();
        }        
        String iDVT = Convert.ToString(ViewData["iDonViTinh"]);        
        DataTable dtDVT = new DataTable();
        dtDVT.Columns.Add("MaDVT", typeof(String));
        dtDVT.Columns.Add("TenDVT", typeof(String));
        DataRow dr = dtDVT.NewRow();
        dr["MaDVT"] = "rD";
        dr["TenDVT"] = "Đồng";
        dtDVT.Rows.Add(dr);
        DataRow dr1 = dtDVT.NewRow();
        dr1["MaDVT"] = "rND";
        dr1["TenDVT"] = "Nghìn đồng";
        dtDVT.Rows.Add(dr1);
        DataRow dr2 = dtDVT.NewRow();
        dr2["MaDVT"] = "rTrD";
        dr2["TenDVT"] = "Triệu đồng";
        dtDVT.Rows.Add(dr2);
        if (String.IsNullOrEmpty(iDVT))
        {
            iDVT = dtDVT.Rows[0]["MaDVT"].ToString();
        }
        SelectOptionList slDVT = new SelectOptionList(dtDVT, "MaDVT", "TenDVT");
        String PageLoad = Convert.ToString(ViewData["pageload"]);
        String urlReport = "";
        String URL = Url.Action("Index", "KeToan_ChiTiet_Report");
        String iPage = Convert.ToString(ViewData["iPage"]);
        if (String.IsNullOrEmpty(iPage))
            iPage = "A4Dung";
        DataTable dtThang = DanhMucModels.DT_Thang(false);
        String iTuNgay = Convert.ToString(ViewData["iTuNgay"]);
        String iDenNgay = Convert.ToString(ViewData["iDenNgay"]);
        String iTuThang = Convert.ToString(ViewData["iTuThang"]);
        String iDenThang = Convert.ToString(ViewData["iDenThang"]);
        if (String.IsNullOrEmpty(iTuNgay))
            iTuNgay = "1";
        if (String.IsNullOrEmpty(iTuThang))
            iTuThang = DanhMucModels.ThangLamViec(UserID).ToString();
        if (String.IsNullOrEmpty(iDenNgay))
            iDenNgay = "28";
        if (String.IsNullOrEmpty(iDenThang))
            iDenThang = DanhMucModels.ThangLamViec(UserID).ToString();           
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        String iSoTo = Convert.ToString(ViewData["iSoTo"]);
        if (String.IsNullOrEmpty(iSoTo))
            iSoTo = "1";
        String attr = iPage.Equals("A4Dung") ? "disabled" : "";
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);

        DataTable dtTrangThai = rptDoiChieuCTMTController.tbTrangThai();
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
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptKT_TongHop_RutDuToan", new { iTuNgay = iTuNgay, iDenNgay = iDenNgay, iTuThang = iTuThang, iDenThang = iDenThang, iNam = Nam, iDVTinh = iDVT, iSoTo = iSoTo, iReport = iPage, UserID = UserID, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        using (Html.BeginForm("EditSubmit", "rptKT_TongHop_RutDuToan", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span></span>
                    </td>
                    <td width="47.9%"><span>Báo cáo tổng hợp rút dự toán</span></td>
                    <td width="52%" style=" text-align:left;">
                        <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width:1200; max-width:1200; margin:0px auto; padding:0px 0px; overflow:visible; ">
                <ul class="inlineBlock">
               
                            <li> 
                                <fieldset style="border:1px solid #cecece;padding:3px 5px;border-radius:3px;-moz-border-radius:3px;-webkit-border-radius:3px; font-size:13px;">
                                    <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Từ ngày&nbsp;&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;Đến ngày&nbsp;&nbsp;Tháng")%></legend>
                                    <p style="padding:3px; margin-bottom:5px;">
                                        <label id="<%= ParentID %>_divTuNgay" style=" font-size:14px;padding:4px;"><% rptKT_TongHop_RutDuToanController rpt = new rptKT_TongHop_RutDuToanController(); %><%=rpt.obj_DSNgay(ParentID, iTuThang, Nam, iTuNgay, "iTuNgay")%></label>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, iTuThang, "iTuThang", "", "style=\"width:50px;padding:2px; border:1px solid #dedede;\" onchange=\"TuNgay()\"")%>                                            
                                        <label id="<%= ParentID %>_divDenNgay" style=" font-size:14px;padding:4px;"><%=rpt.obj_DSNgay(ParentID, iDenThang, Nam, iDenNgay, "iDenNgay")%></label>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, iDenThang, "iDenThang", "", "style=\"width:50px;padding:2px; border:1px solid #dedede;\" onchange=\"DenNgay()\"")%>
                                    </p>
                                </fieldset>               
                            </li> 
                     
                            <li>
                                <p style="text-align:right; padding:5px 3px 1px 3px; height:25px;"><span style="padding:2px 5px;"><%=NgonNgu.LayXau("Tờ số")%></span><span id="<%= ParentID %>_divSoTo"><%=rpt.obj_DSSoTo(ParentID, iTuNgay, iDenNgay, iTuNgay, iDenThang, Nam, iSoTo, attr, UserID, iID_MaTrangThaiDuyet)%></span></p>
                                <p style="text-align:right; padding:4px 3px 1px 4px; height:25px;"><span style="padding:2px 5px;"><%=NgonNgu.LayXau("Đơn vị tính")%></span><span><%=MyHtmlHelper.DropDownList(ParentID, slDVT, iDVT, "iDVT", "", "class=\"input1_2\" style=\"width:56px; padding:2px;\"")%></span></p>
                            </li>  
                                 
                            <li>
                                <fieldset style="padding:2px; border:1px solid #dedede; border-radius:5px; height:55px;">
                                    <legend><%=NgonNgu.LayXau("Chọn hướng in")%></legend>
                                    <span style="width:140px; line-height:30px;"><%=MyHtmlHelper.Option(ParentID, "A4Ngang", iPage, "Pages", "", "onchange=\"ChonPage()\"")%>&nbsp;A4 Ngang- 9 Loại NS</span>
                                    <span style="width:140px; line-height:30px;"><%=MyHtmlHelper.Option(ParentID, "A4Dung", iPage, "Pages", "", "onchange=\"ChonPage()\"")%>&nbsp;A4 Đứng - 4 Loại NS</span>
                                </fieldset>   
                                <div style="display:none;">
                                    <%=MyHtmlHelper.TextBox(ParentID, iPage, "divPages", "", "")%>
                                </div>
                                <%=MyHtmlHelper.Hidden(ParentID,Nam,"iNamLamViec","") %>                     
                            </li> 
                            
                            <li>
                                <p style="text-align:right;height:53px; padding:4px;"><span style="height:63px; vertical-align:top; padding:2px 5px;"><%=NgonNgu.LayXau("Trạng thái duyệt") %></span> <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width:100px; padding:2px; height:50px;\" onchange=\"ChonTo()\" size='2' tab-index='-1'")%></p>
                            </li>  
                                      
                </ul>
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>  
            </div>            
        </div>
    </div>
    <%} %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
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
            if ($("#<%=ParentID %>_divPages").val() == 'A4Dung') {
                $("#<%=ParentID %>_iSoTo").attr("disabled", true);
            }
            else
                $("#<%=ParentID %>_iSoTo").attr("disabled", false);
        }
        $(document).ready(function () {           
            $("ul.inlineBlock li:last-child span").css("line-height", "");
            if ($("#<%=ParentID %>_divPages").val() == 'A4Dung') {
                $("#<%=ParentID %>_iSoTo").attr("disabled", true);
            }           
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
            $("ul.inlineBlock li span:contains('Tháng')").width(40);
            $("ul.inlineBlock li:contains('Đơn vị tính') select").width(60).css({ 'text-align': 'left' });
            $("#<%=ParentID %>_iTuThang").change(function () {
                var iTuThang = parseInt($(this).val());
                var iDenThang = parseInt($("#<%=ParentID %>_iDenThang").val());
                if (iTuThang > iDenThang) {
                    $("#<%=ParentID %>_iDenThang option[value=" + (iTuThang) + "]").attr('selected', true);
                    DenNgay();
                }
            });
            $("#<%=ParentID %>_iDenThang").change(function () {
                var iDenThang = parseInt($(this).val());
                var iTuThang = parseInt($("#<%=ParentID %>_iTuThang").val());
                if (iDenThang < iTuThang) {
                    $("#<%=ParentID %>_iTuThang option[value=" + iDenThang + "]").attr('selected', true);
                    TuNgay();
                }
            });
        });

        function ChonNgay(idNgay, idThang, divNgay, FromOrTo) {
            var Nam = document.getElementById("<%=ParentID %>_iNamLamViec").value
            var Thang = document.getElementById("<%=ParentID %>_"+idThang).value
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsNgay?ParentID=#0&iThang=#1&iNam=#2&iNgay=#3&FromOrTo=#4", "rptKT_TongHop_RutDuToan") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", Thang));
            url = unescape(url.replace("#2", Nam));
            url = unescape(url.replace("#3", idNgay));
            url = unescape(url.replace("#4", FromOrTo));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_"+divNgay).innerHTML = data;
            });
        }
        function TuNgay() {
            ChonNgay("<%=iTuNgay %>", "iTuThang", "divTuNgay", "iTuNgay");
            ChonTo();
        }
        function DenNgay() {
            ChonNgay("<%=iDenNgay %>", "iDenThang", "divDenNgay","iDenNgay");
            ChonTo();
        }
        function ChonTo() {
            var iNam = document.getElementById("<%=ParentID %>_iNamLamViec").value
            var iTuNgay = document.getElementById("<%=ParentID %>_iTuThang").value
            var iTuThang = document.getElementById("<%=ParentID %>_iTuThang").value
            var iDenNgay = document.getElementById("<%=ParentID %>_iTuThang").value
            var iDenThang = document.getElementById("<%=ParentID %>_iTuThang").value
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value         
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsSoTo?ParentID=#0&iTuNgay=#1&iDenNgay=#2&iTuThang=#3&iDenThang=#4&iNam=#5&iSoTo=#6&attr=#7&UserID=#8&iID_MaTrangThaiDuyet=#9", "rptKT_TongHop_RutDuToan") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iTuNgay));
            url = unescape(url.replace("#2", iDenNgay));
            url = unescape(url.replace("#3", iTuNgay));
            url = unescape(url.replace("#4", iDenThang));
            url = unescape(url.replace("#5", iNam));
            url = unescape(url.replace("#6", "<%=iSoTo %>"));
            url = unescape(url.replace("#7", "<%=attr %>"));
            url = unescape(url.replace("#8", "<%=UserID %>"));
            url = unescape(url.replace("#9", iID_MaTrangThaiDuyet));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divSoTo").innerHTML = data;
            });
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKT_TongHop_RutDuToan", new { iTuNgay = iTuNgay, iDenNgay = iDenNgay, iTuThang = iTuThang, iDenThang = iDenThang, iNam = Nam, iDVTinh = iDVT, iSoTo = iSoTo, iReport = iPage, UserID = UserID, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Xuất ra excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>