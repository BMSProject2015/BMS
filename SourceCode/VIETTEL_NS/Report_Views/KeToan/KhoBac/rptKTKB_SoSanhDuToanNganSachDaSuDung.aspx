<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
        }
        ul.inlineBlock li p{
            padding:1px;    
        }
        ul.inlineBlock li fieldset{border:1px solid #cecece;border-radius:3px;-moz-border-radius:3px;-webkit-border-radius:3px;}
        ul.inlineBlock li fieldset span{
            padding:2px 3px 2px 1px;  
            margin-left:2px;  
        }
        ul.inlineBlock li fieldset legend{text-align:left;}
        ul.inlineBlock li fieldset div #divThang select{width:70px;}
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
            background-position:  20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
        select{border:1px solid #dedede;padding:2px;}
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
         String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    String ParentID = "RutDuToan";
    String UserID = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
    String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    
    dtCauHinh.Dispose();
    String MucIn = "DonVi";    
    String Quy = "1";        
    String LoaiThangQuy = "Thang";    
    String GiaTriThangQuy="1";
    
    MucIn = Convert.ToString(ViewData["MucIn"]);        
    LoaiThangQuy = Convert.ToString(ViewData["LoaiThangQuy"]);
    GiaTriThangQuy = Convert.ToString(ViewData["GiaTriThangQuy"]);
    Quy=Convert.ToString(ViewData["Quy"]);
    String Thang=Convert.ToString(ViewData["Thang"]); 
    if(String.IsNullOrEmpty(MucIn)) MucIn = "Muc";
    if (String.IsNullOrEmpty(Thang)) Thang = "1";
    if (String.IsNullOrEmpty(Quy)) Quy = "1";
    if (String.IsNullOrEmpty(LoaiThangQuy)) LoaiThangQuy = "Thang";
    if (String.IsNullOrEmpty(GiaTriThangQuy)) GiaTriThangQuy = "1";
    String BackURL = Url.Action("Index", "KeToan_ChiTiet_Report", new { sLoai = "1" });
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = Luong_ReportModel.DachSachTrangThai();
         String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "-100")
    {
        PageLoad = "0";
    }
    String UrlReport = "";
        if (PageLoad == "1")
        {
            UrlReport = Url.Action("ViewPDF", "rptKTKB_SoSanhDuToanNganSachDaSuDung", new { MucIn = MucIn, Thang = Thang, Quy = Quy, Nam = iNamLamViec, LoaiThangQuy = LoaiThangQuy, MaND = UserID, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        }
    using (Html.BeginForm("EditSubmit", "rptKTKB_SoSanhDuToanNganSachDaSuDung", new { ParentID = ParentID }))
   {
    %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTKB_SoSanhDuToanNganSachDaSuDung", new { MucIn = MucIn, Thang = GiaTriThangQuy, Quy = GiaTriThangQuy, Nam = iNamLamViec, LoaiThangQuy = LoaiThangQuy, MaND = UserID }), "Export To Excel")%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%"><span>Báo cáo so sánh dự toán ngân sách đã sử dụng</span></td>
                    <td width="52%" style=" text-align:left;">
                        <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                    </td>
                </tr>
            </table>
        </div>
        
        <%--<div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="10%" align="right" style="padding-right: 5px;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID,"LNS",MucIn,"MucIn","") %></div>
                        </td>
                        <td width="30%">
                            <div>
                                Loại ngân sách</div>
                        </td>
                        <td width="10%" align="right" style="padding-right: 5px;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID, "Thang", LoaiThangQuy, "ThangQuy", "","onchange=\"ChonThangQuy('Thang')\"")%></div>
                        </td>
                        <td width="10%">
                            <div>
                                Tháng</div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" align="right" style="padding-right: 5px;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID, "Muc", MucIn, "MucIn", "")%></div>
                        </td>
                        <td width="30%">
                            <div>
                                Mục</div>
                        </td>
                        <td width="10%" align="right" style="padding-right: 5px;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID, "Quy", LoaiThangQuy, "ThangQuy", "", "onchange=\"ChonThangQuy('Quy')\"")%></div>
                        </td>
                        <td width="10%">
                            <div>   
                                Quý</div>
                        </td>
                        <td width="10%" align="Left">
                            <div id="divThang">
                                <% VIETTEL.Report_Controllers.KeToan.KhoBac.rptKTKB_SoSanhDuToanNganSachDaSuDungController rpt = new VIETTEL.Report_Controllers.KeToan.KhoBac.rptKTKB_SoSanhDuToanNganSachDaSuDungController(); %>
                                <% =rpt.get_sThangQuy(ParentID, LoaiThangQuy, GiaTriThangQuy)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" align="right" style="padding-right: 5px;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID,"TieuMuc",MucIn,"MucIn","") %></div>
                        </td>
                        <td width="30%">
                            <div>
                                Tiểu mục</div>
                        </td>
                        <td align="right" style="padding-right: 5px;">
                            <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                        </td>
                        
                        <td>
                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                        </td>
                        <td>
                         &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="10%" align="right" style="padding-right: 5px;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID,"DonVi",MucIn,"MucIn","") %></div>
                        </td>
                        <td width="30%">
                            <div>
                                Đơn vị</div>
                        </td>
                        <td colspan="3">
                        </td>
                    </tr>                 
                </table>
            </div>
        </div>--%>
         <div class="box_tong">
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td width="5%"></td>
                    <td width="10%" class="td_form2_td1"><div> <%=NgonNgu.LayXau("Kiểu in  : ")%> </div> </td>
                    <td width="25%"> 
                     <%=MyHtmlHelper.Option(ParentID, "LNS", MucIn, "MucIn", "")%><span><%=NgonNgu.LayXau("Loại ngân sách") %></span>
                               <%=MyHtmlHelper.Option(ParentID, "Muc", MucIn, "MucIn", "")%><span><%=NgonNgu.LayXau("Mục") %></span>                               
                               <%=MyHtmlHelper.Option(ParentID, "TieuMuc", MucIn, "MucIn", "")%><span><%=NgonNgu.LayXau("Tiểu mục") %></span>
                               <%=MyHtmlHelper.Option(ParentID, "DonVi", MucIn, "MucIn", "")%><span><%=NgonNgu.LayXau("Đơn vị") %></span> 
                    </td>
                    <td width="10%" class="td_form2_td1"> Chọn thời gian:   </td>
                    <td width="20%">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                    	<tr>
                    		<td><%=MyHtmlHelper.Option(ParentID, "Thang", LoaiThangQuy, "ThangQuy", "","onchange=\"ChonThangQuy('Thang')\"")%><span><%=NgonNgu.LayXau("Tháng") %></span></td>
                            <td width="50%">  <div id="divThang">
                                    <% VIETTEL.Report_Controllers.KeToan.KhoBac.rptKTKB_SoSanhDuToanNganSachDaSuDungController rpt = new VIETTEL.Report_Controllers.KeToan.KhoBac.rptKTKB_SoSanhDuToanNganSachDaSuDungController(); %>
                                    <% =rpt.get_sThangQuy(ParentID, LoaiThangQuy, GiaTriThangQuy)%>
                            </div></td>
                    	</tr>
                        <tr><td><%=MyHtmlHelper.Option(ParentID, "Quy", LoaiThangQuy, "ThangQuy", "", "onchange=\"ChonThangQuy('Quy')\"")%><span><%=NgonNgu.LayXau("Quý") %></span></td></tr>
                    </table>
                            
                          
                    
                    </td>
                     <td width="10%" class="td_form2_td1">Trạng thái</td>
                     <td width="15%"> <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%; font-size:13px;\" size='3' tab-index='-1'")%></td>
                    <td></td>
                    </tr>
                    
                     <tr>
                        <td>
                        </td>
                         
                        <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
<td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        </div>
       <%-- <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width:1024px; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:visible; text-align:center;">
                <ul class="inlineBlock">         
                    <li>
                        <fieldset style="border:1px solid #cecece;padding:3px 5px; font-size:13px; height:56px;">
                            <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("In đến mức:")%></legend>
                            <p style="padding:3px 1px; margin-bottom:5px;">
                               <%=MyHtmlHelper.Option(ParentID, "LNS", MucIn, "MucIn", "")%><span><%=NgonNgu.LayXau("Loại ngân sách") %></span>
                               <%=MyHtmlHelper.Option(ParentID, "Muc", MucIn, "MucIn", "")%><span><%=NgonNgu.LayXau("Mục") %></span>                               
                               <%=MyHtmlHelper.Option(ParentID, "TieuMuc", MucIn, "MucIn", "")%><span><%=NgonNgu.LayXau("Tiểu mục") %></span>
                               <%=MyHtmlHelper.Option(ParentID, "DonVi", MucIn, "MucIn", "")%><span><%=NgonNgu.LayXau("Đơn vị") %></span>                               
                            </p>
                        </fieldset>
                    </li>
                    <li>
                        <fieldset style="padding:1px;font-size:13px;">
                            <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Chọn thời gian:")%></legend>
                            <div style="float:left; width:80px; text-align:left;">
                                <p style="padding-left:5px;"><%=MyHtmlHelper.Option(ParentID, "Thang", LoaiThangQuy, "ThangQuy", "","onchange=\"ChonThangQuy('Thang')\"")%><span><%=NgonNgu.LayXau("Tháng") %></span></p>
                                <p style="padding-left:5px;"><%=MyHtmlHelper.Option(ParentID, "Quy", LoaiThangQuy, "ThangQuy", "", "onchange=\"ChonThangQuy('Quy')\"")%><span><%=NgonNgu.LayXau("Quý") %></span></p>
                            </div>
                            <div style="float:left; width:80px; line-height:36px;">
                                <p id="divThang" style="text-align:left;">
                                    <% VIETTEL.Report_Controllers.KeToan.KhoBac.rptKTKB_SoSanhDuToanNganSachDaSuDungController rpt = new VIETTEL.Report_Controllers.KeToan.KhoBac.rptKTKB_SoSanhDuToanNganSachDaSuDungController(); %>
                                    <% =rpt.get_sThangQuy(ParentID, LoaiThangQuy, GiaTriThangQuy)%>
                                </p>
                            </div>
                        </fieldset>
                    </li>
                    <li>
                        <fieldset>
                            <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Trạng thái duyệt:") %></legend>
                            <p style="text-align:right;"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 200px; font-size:13px;\" size='3' tab-index='-1'")%></p>
                        </fieldset>
                    </li>
                </ul><!--End .inlineBlock-->                
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
            </div><!--End #rptMain-->
        </div>--%>
    </div>
    <script type="text/javascript">
        function ChonThangQuy(Loai) {          
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsThangQuy?ParentID=#0&Loai=#1&GiaTri=#2","rptKTKB_SoSanhDuToanNganSachDaSuDung") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", Loai));
            url = unescape(url.replace("#2", "1"));
            $.getJSON(url, function (data) {
                document.getElementById('divThang').innerHTML = data;
            });
        }
        $(function () {
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }                            
    </script>
    <%}%>
    <iframe src="<%=UrlReport%>" height="600px" width="100%"></iframe>
</body>
</html>