<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.KhoBac" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
            height:50px;
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
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px -29px;
            -webkit-border-radius:2px;
            border-radius:2px;
        }    
        div.login1 a.active {
            background-position: 20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
        .errorafter
        {
           background-color:Yellow;
        }        
        ul.inlineBlock li fieldset .div
        {
            width:90px; height:23px; display:inline-block; text-align:center; padding:1px; font-size:14px;-moz-border-radius:3px;-webkit-border-radius:3px; border-radius:3px;
            font-family:Tahoma Arial; color:Black; line-height:23px; cursor:pointer; background-color:#dddddd;  
        }
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>    
</head>
<body>
    <%
     String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "KTKB";
    String UserID=User.Identity.Name;
    String LoaiThang_Quy = Convert.ToString(ViewData["iLoaiThang_Quy"]);
    if (String.IsNullOrEmpty(LoaiThang_Quy))
    {
        LoaiThang_Quy = "0";
    }
    String Thang_Quy = Convert.ToString(ViewData["iThang_Quy"]);    
    //Chọn năm
    String NamLamViec = Convert.ToString(ViewData["iNamLamViec"]);
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DanhMucModels.NamLamViec(UserID).ToString();
    }
    String iID_MaLoaiNganSach = Convert.ToString(ViewData["iID_MaLoaiNganSach"]);
    //Chọn ngân sách   
    DataTable dtloai = rptKTKhoBac_DoiChieuDuToanController.Lay_LoaiNganSach(UserID);        
    SelectOptionList slloai = new SelectOptionList(dtloai, "iID_MaNguonNganSach", "sTen");
    if (String.IsNullOrEmpty(iID_MaLoaiNganSach))
    {
        iID_MaLoaiNganSach = dtloai.Rows.Count > 0 ? Convert.ToString(dtloai.Rows[0]["iID_MaNguonNganSach"]) : Guid.Empty.ToString();
    }
    dtloai.Dispose();
    //chọn loại báo cáo
    String LoaiBaoCao = Convert.ToString(ViewData["iLoaiBaoCao"]);    
    DataTable dtLoaiBaoCao = rptKTKhoBac_DoiChieuDuToanController.DanhSach_LoaiBaoCao();
    SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "MaLoai", "TenLoai");
    if (String.IsNullOrEmpty(LoaiBaoCao))
        LoaiBaoCao = dtLoaiBaoCao.Rows.Count > 0 ? Convert.ToString(dtLoaiBaoCao.Rows[0]["MaLoai"]) : Guid.Empty.ToString();
    //chọn mức in
    String inmuc = Convert.ToString(ViewData["inmuc"]);
    if (String.IsNullOrEmpty(inmuc))
    {
        inmuc = "rLK";
    }
    String checkGhep = Convert.ToString(ViewData["iGhep"]);
    String checkGom = Convert.ToString(ViewData["iGom"]);
    if (String.IsNullOrEmpty(checkGhep))
        checkGhep = "off";
    if (String.IsNullOrEmpty(checkGom))
        checkGom = "off";
    String attrGhep = checkGhep.Equals("on") ? "checked=\"checked\"" : "";
    String attrGom = checkGom.Equals("on") ? "checked=\"checked\"" : "";
    String BackURL = Url.Action("Index", "KeToan_ChiTiet_Report", new { sLoai = "1" });
    String urlReport = Convert.ToString(ViewData["pageload"]).Equals("1") ? Url.Action("ViewPDF", "rptKTKhoBac_DoiChieuDuToan", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, LoaiBaoCao = LoaiBaoCao, inmuc = inmuc, iID_MaNguonNganSach = iID_MaLoaiNganSach, iGom = checkGom,UserID=UserID }) : "";
    using (Html.BeginForm("EditSubmit", "rptKTKhoBac_DoiChieuDuToan", new { ParentID = ParentID}))
   {
    %>  
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo đối chiếu rút dự toán</span>
                    </td>
                    <td width="52%" style=" text-align:left;">
                        <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width:1024px; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:visible;">
                <ul class="inlineBlock">                       
                    <li>
                        <span style="float:left; padding:2px 1px; line-height:25px;">
                            <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "iLoaiThang_Quy", "", "onchange=\"ChonPage()\"")%>&nbsp;Quý
                            <br/>
                            <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "iLoaiThang_Quy", "", "onchange=\"ChonPage()\"")%>&nbsp;Tháng
                        </span>                    
                        <span id="<%= ParentID %>_ThangQuy" style="line-height:50px; float:left; margin-left:5px;">
                             <% rptKTKhoBac_DoiChieuDuToanController rpt = new rptKTKhoBac_DoiChieuDuToanController();%>
                             <%=rpt.obj_DSThangQuy(ParentID,LoaiThang_Quy,Thang_Quy,UserID) %>
                        </span>
                        <div style="display:none;">
                            <%=MyHtmlHelper.TextBox(ParentID, LoaiThang_Quy, "divThangQuy", "", "")%>
                        </div>
                    </li>     
                    <li>
                        <span style="text-align:right;padding:2px 1px; line-height:28px;" ><label class="label" style="min-width:132px; margin-right:7px;"><%=NgonNgu.LayXau("Chọn ngân sách") %>&nbsp;&nbsp;</label> <%=MyHtmlHelper.DropDownList(ParentID, slloai, iID_MaLoaiNganSach, "iID_MaLoaiNganSach", "", "class=\"input1_2\" style=\"width: 190px; padding:2px; border-radius:2px; -webkit-border-radius:2px;border-color:#dedede; cursor:pointer;\"")%></span>                        
                        <br/>
                        <span style="text-align:right; padding:2px 1px; line-height:28px;"><label class="label"><%=NgonNgu.LayXau("Chọn mẫu báo cáo") %>&nbsp;</label><%=MyHtmlHelper.DropDownList(ParentID, slLoaiBaoCao, LoaiBaoCao, "LoaiBaoCao", "", "class=\"input1_2\" style=\"width: 190px; padding:2px; border-radius:2px; -webkit-border-radius:2px;border-color:#dedede;cursor:pointer;\"")%></span>                        
                    </li>   
                    <li>
                        <fieldset style="padding:1px; border:1px dashed #cecece; border-radius:5px; width:300px; height:50px; text-align:center; line-height:20px;">
                            <legend style="text-align:left;"><%=NgonNgu.LayXau("&nbsp;In đến mục:")%></legend>                            
                            <div class="div"><%=MyHtmlHelper.Option(ParentID, "rLK", inmuc, "iReport", "", "style=\"display:none;\" onchange=\"ChonReport()\"")%>&nbsp;Loại - khoản&nbsp;</div>                                 
                            <div class="div"><%=MyHtmlHelper.Option(ParentID, "rM", inmuc, "iReport", "", "style=\"display:none;\" onchange=\"ChonReport()\"")%>&nbsp;Mục&nbsp;</div>                                
                            <div class="div"><%=MyHtmlHelper.Option(ParentID, "rTM", inmuc, "iReport", "", " style=\"display:none;\"onchange=\"ChonReport()\"")%>&nbsp;Tiểu mục&nbsp; </div>                                                      
                        </fieldset>
                        <div style="display:none;">
                            <%=MyHtmlHelper.TextBox(ParentID, inmuc, "divReport", "", "")%>
                        </div>     
                    </li> 
                    <li>
                        <span style="text-align:left;padding:2px 1px 0px 1px; line-height:28px;">
                            <input type="checkbox" value="rAllDonVi" id="rAllDonVi" <%=attrGhep %> style="cursor:pointer;" onclick="ChonGhep(this.checked)" />&nbsp;&nbsp;<label><%=NgonNgu.LayXau("Ghép trên 1 tờ (số TM <=10)") %></label>
                        </span>
                        <div style="display:none;">
                            <%=MyHtmlHelper.TextBox(ParentID, checkGhep, "divGhep", "", "")%>
                        </div>
                        <br />
                        <span style="text-align:left;padding:0px 1px 2px 1px; line-height:28px;">
                        <input type="checkbox" value="rAllDonVi1" id="Checkbox1" <%=attrGom %> style="cursor:pointer;" onclick="ChonGom(this.checked)" />&nbsp;&nbsp;<label><%=NgonNgu.LayXau("Gom phần 2 (số Loại-khoản <=5)") %></label>
                        </span>
                        <div style="display:none;">
                            <%=MyHtmlHelper.TextBox(ParentID, checkGom, "divGom", "", "")%>
                        </div>
                        <%=MyHtmlHelper.Hidden(ParentID,NamLamViec,"iNamLamViec","") %> 
                    </li>          
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
            <%--<div style="width:50px; margin:0 auto; height:20px; text-align:center;" class="login1"><a href="#"></a></div>--%>
        </div>
    </div>
    <%} %>
    <div>
    </div>
    <%       
        dtloai.Dispose();
        dtLoaiBaoCao.Dispose();       
    %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        $(function () {
            $("ul.inlineBlock li span #<%=ParentID %>_LoaiBaoCao").css({ 'border-color': '#cecece' });
            $("div#rptMain").hide();
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
            $("div.div").bind('click', function () {
                $(this).children().removeAttr("checked", "checked").attr("checked", "checked");
                ChonReport();
                $('.div').css('background','#dddddd');
                $(this).css("background", "#ffee66");
            });
            $("input:radio").each(function () {
                if ($(this).val() == '<%=inmuc %>') {
                    $(this).parent().css("background", "#ffee66");
                }
            });
        });
        function ChonPage() {
            var TenPage = document.getElementsByName("<%=ParentID %>_iLoaiThang_Quy");
            var pages;
            var i = 0;
            for (i = 0; i < TenPage.length; i++) {
                if (TenPage[i].checked) {
                    pages = TenPage[i].value;
                }
            }
            document.getElementById("<%= ParentID %>_divThangQuy").value = pages;
            ThangQuy();
        }
        function ChonReport() {
            var TenPage = document.getElementsByName("<%=ParentID %>_iReport");
            var pages;
            var i = 0;
            for (i = 0; i < TenPage.length; i++) {
                if (TenPage[i].checked) {
                    pages = TenPage[i].value;
                }
            }
            document.getElementById("<%= ParentID %>_divReport").value = pages;
        }
        function ThangQuy() {
            var iLoaiThangQuy = document.getElementById("<%=ParentID %>_divThangQuy").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsThangQuy?ParentID=#0&iLoaiThang_Quy=#1&iThangQuy=#2&UserID=#3", "rptKTKhoBac_DoiChieuDuToan") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iLoaiThangQuy));
            url = unescape(url.replace("#2", "<%=Thang_Quy %>"));
            url = unescape(url.replace("#2", "<%=UserID %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_ThangQuy").innerHTML = data;
            });
        }
        function ChonGom(DonVi) {
            $("input:checkbox").each(function (i) {
                if (DonVi) {
                    document.getElementById("<%= ParentID %>_divGom").value = "on";
                }
                else {
                    document.getElementById("<%= ParentID %>_divGom").value = "off";
                }
            });
        }
        function ChonGhep(DonVi) {
            $("input:checkbox").each(function (i) {
                if (DonVi) {
                    document.getElementById("<%= ParentID %>_divGhep").value = "on";
                }
                else {
                    document.getElementById("<%= ParentID %>_divGhep").value = "off";
                }
            });
        }     
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTKhoBac_DoiChieuDuToan", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, LoaiBaoCao = LoaiBaoCao, inmuc = inmuc, iID_MaNguonNganSach = iID_MaLoaiNganSach, iGom = checkGom,UserID=UserID }), "Xuất ra file Excel")%>
     <iframe src="<%=urlReport%>" height="600px" width="100%">
     </iframe>
</body>
</html>