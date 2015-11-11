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
        /*
        ul.inlineBlock li span
        {
        width: 62px;
        float:left;
        text-align: right;
        margin-right: 0.5em;        
        }
         ul.inlineBlock li span.sp1
         {
             width:72px;
             }*/
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
        .errorafter
        {
           background-color:Yellow;
        }        
        ul.inlineBlock li fieldset .div
        {
            width:90px; height:23px; display:inline-block; text-align:center; padding:1px; font-size:14px;-moz-border-radius:3px;-webkit-border-radius:3px; border-radius:3px;
            font-family:Tahoma Arial; color:Black; line-height:23px; cursor:pointer; background-color:#dedede;  
        }
        select
        {
            border:1px solid #cecece;
        }
    </style>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "KTKB";        
        String UserID = User.Identity.Name;        
        String iLoaiThang_Quy = Convert.ToString(ViewData["iLoaiThang_Quy"]);        
        String iThang = Convert.ToString(ViewData["iThang"]);
        String iQuy = Convert.ToString(ViewData["iQuy"]);
        String iNguonNS = Convert.ToString(ViewData["iNguonNS"]);
        String iMuc_TieuMuc = Convert.ToString(ViewData["iMuc_TieuMuc"]);  
        String PageLoad = Convert.ToString(ViewData["pageload"]);
        if (String.IsNullOrEmpty(iLoaiThang_Quy))
            iLoaiThang_Quy = "1";
        if (String.IsNullOrEmpty(iThang))
            iThang = DanhMucModels.ThangLamViec(UserID).ToString();
        DataTable dtQuy = DanhMucModels.DT_Quy();
        DataTable dtThang=DanhMucModels.DT_Thang(false);
        dtQuy.Rows.RemoveAt(0);
        if(String.IsNullOrEmpty(iQuy))
            iQuy = dtQuy.Rows[0]["MaQuy"].ToString();       
        if (String.IsNullOrEmpty(iNguonNS)){            
            iNguonNS = Guid.Empty.ToString();
        }
        if (String.IsNullOrEmpty(iMuc_TieuMuc))
            iMuc_TieuMuc = "rM";
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");        
        String urlReport = "";
        String URL = Url.Action("Index", "KeToan_ChiTiet_Report");       
        dtQuy.Dispose();
        dtThang.Dispose();
        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String iTrangThai = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iTrangThai))
            iTrangThai = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();                 
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptKTKB_TheoDoiTamUng_S75H", new { iLoaiThang_Quy = iLoaiThang_Quy, iThang = iThang, iQuy = iQuy, iNguonNS = iNguonNS, iMuc_TieuMuc = iMuc_TieuMuc, MaND = UserID, iTrangThai =iTrangThai});
        using (Html.BeginForm("EditSubmit", "rptKTKB_TheoDoiTamUng_S75H", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%"><span>Báo cáo theo dõi tạm ứng tại kho bạc</span></td>
                    <td width="52%" style=" text-align:left;">
                        <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                    </td>                    
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width:1024px; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:visible; ">
                <ul class="inlineBlock">
                    <li>                        
                        <fieldset style="padding:3px; border:1px solid #dedede; border-radius:5px; width:250px">
                            <legend style="padding: 4px;"><%=NgonNgu.LayXau("Chọn tháng - quý:")%></legend>
                            <span style="width:60px; display:inline-block; line-height:23px;"><%=MyHtmlHelper.Option(ParentID, "1", iLoaiThang_Quy, "iLoaiThang_Quy", "", "")%>&nbsp;Quý</span><span><%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:46px; padding:2px;\"")%></span>&nbsp;&nbsp;
                            <span style="width:60px; display:inline-block; line-height:23px;"><%=MyHtmlHelper.Option(ParentID, "0", iLoaiThang_Quy, "iLoaiThang_Quy", "", "")%>&nbsp;Tháng</span><span><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width:46px; padding:2px;\"")%></span>
                        </fieldset>                               
                    </li>                               
                    <li>
                        <p style="text-align:right; padding:2px; margin-top:2px;"><span><%=NgonNgu.LayXau("Chọn ngân sách:") %></span><span id="<%= ParentID %>_divNguonNS" style="margin-left:2px"><% rptKTKB_TheoDoiTamUng_S75HController rpt = new rptKTKB_TheoDoiTamUng_S75HController(); %><%=rpt.obj_DSNguonNS(ParentID,UserID,iTrangThai,iNguonNS) %></span></p>
                        <p style="text-align:right; padding:2px;"><span><%=NgonNgu.LayXau("Trạng thái duyệt:") %></span><span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width:185px; padding:2px;\" onchange=\"ChonNguonNS()\"")%></span></p>
                    </li>
                    <li></li>
                    <li>
                        <fieldset style="padding:5px; border:1px solid #dedede; border-radius:5px; width:200px;">
                            <legend style="padding: 4px;"><%=NgonNgu.LayXau("In đến mức:")%></legend>                            
                            <span style="width:100px;line-height:23px; padding:3px 3px;"><%=MyHtmlHelper.Option(ParentID, "rM", iMuc_TieuMuc, "iMuc_TieuMuc", "", "")%>&nbsp;Mục</span>&nbsp;&nbsp;&nbsp;
                            <span style="width:100px;line-height:23px; padding:3px 3px;"><%=MyHtmlHelper.Option(ParentID, "rTM", iMuc_TieuMuc, "iMuc_TieuMuc", "", "")%>&nbsp;Tiểu mục</span>                            
                        </fieldset>  
                        <%=MyHtmlHelper.Hidden(ParentID,"","iNamLamViec","") %> 
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
        $(document).ready(function () {            
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });
        function ChonNguonNS() {
            var iTrangThai = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_DSNguonNS?ParentID=#0&MaND=#1&iTrangThai=#2&iNguonNS=#3", "rptKTKB_TheoDoiTamUng_S75H") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%=UserID %>"));
            url = unescape(url.replace("#2", iTrangThai));
            url = unescape(url.replace("#3", "<%=iNguonNS %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divNguonNS").innerHTML = data;
            });
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTKB_TheoDoiTamUng_S75H", new { iLoaiThang_Quy = iLoaiThang_Quy, iThang = iThang, iQuy = iQuy, iNguonNS = iNguonNS, iMuc_TieuMuc = iMuc_TieuMuc, MaND = UserID, iTrangThai=iTrangThai }), "Xuất ra excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
