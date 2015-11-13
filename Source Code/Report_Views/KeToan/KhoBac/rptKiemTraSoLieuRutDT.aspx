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
        div.login1
        {
            text-align: center;
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }
        div.login1 a
        {
            color: white;
            text-decoration: none;
            font: bold 16px "Museo 700";
            display: block;
            width: 50px;
            height: 20px;
            line-height: 20px;
            margin: 0px auto;
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px -29px;
            -webkit-border-radius: 2px;
            border-radius: 2px;
        }
        div.login1 a.active
        {
            background-position: 20px 1px;
        }
        div.login1 a:active, a:focus
        {
            outline: none;
        }
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
    </style>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
     String srcFile = Convert.ToString(ViewData["srcFile"]);
            String ParentID = "QuyetToan";
            String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
            if (String.IsNullOrEmpty(LoaiThang_Quy))
            {
                LoaiThang_Quy = "0";
            }
            String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
            if (String.IsNullOrEmpty(Thang_Quy))
            {
                Thang_Quy = "1";
            }
            String NamLamViec = Convert.ToString(ViewData["NamLamViec"]);
            if (String.IsNullOrEmpty(NamLamViec))
            {
                NamLamViec = DateTime.Now.Year.ToString();
            }
            DateTime dNgayHienTai = DateTime.Now;
            String NamHienTai = Convert.ToString(dNgayHienTai.Year);
            DataTable dtNam = DanhMucModels.DT_Nam();
            SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
            dtNam.Dispose();

            DataTable dtQuy = DanhMucModels.DT_Quy();
            SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
            dtQuy.Dispose();

            DataTable dtThang = DanhMucModels.DT_Thang();
            SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
            dtThang.Dispose();
            String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
            DataTable dtTrangThai = rptKiemTraSoLieuRutDTController.tbTrangThai();
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
    String UserID = User.Identity.Name;
          String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String UrlReport = "";
    if (PageLoad == "1")
        UrlReport = Url.Action("ViewPDF", "rptKiemTraSoLieuRutDT", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    String BackURL = Url.Action("Index", "KeToan_ChiTiet_Report", new { sLoai = "1" });
  
    using (Html.BeginForm("EditSubmit", "rptKiemTraSoLieuRutDT", new { ParentID = ParentID }))
   {
    %>  
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <%--<td width="40%">
                        <span>Báo cáo kiểm tra số liệu rút DT</span>
                    </td>
                    <td width="60%"; align =left>
                    <a style="width:150px; background:url('Content/Themes/images/btn_timkiem_le.png') repeat scroll 0px -23px transparent;padding:4px 4px; text-align:center; color:White; font-size:10pt; height:15px; line-height:15px; font-family:Tahoma Arial; font-weight:bold; border-radius:4px;-moz-border-radius:4px; -webkit-border-radius:4px;cursor:pointer;">Xem Báo Cáo</a>
                    </td>--%>
                    <td width="47.9%">
                        <span>Báo cáo kiểm tra số liệu rút DT</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>

            </table>
        </div>
    
        <div id="table_form2" class="table_form2">
                <div id="rptMain" style="width:750px; max-width:800px; margin:0px auto; padding:0px 0px; overflow:visible;">
                    <ul class="inlineBlock">  
                                       
                        <li >
                            <fieldset>
                                <legend>Chọn thời gian </legend>
                                <p><%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "")%>
                                <span style="display:inline-block; width:50px; margin-left:5px;"><%=NgonNgu.LayXau("Tháng:") %></span>
                                 <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "", "class=\"input1_2\" style=\"width:60px; padding:2px;\"" )%>
                                 <span style="display:inline-block; width:50px; margin-right:4px; margin-left:5px;"></span>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "")%>
                                <span style="display:inline-block; width:50px; margin-right:4px; margin-left:5px;"><%=NgonNgu.LayXau("Quý:  ") %>
                                </span><%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "iQuy", "", "class=\"input1_2\" style=\"width:60px; padding:2px;\"" )%></p>                            
                            </fieldset>
                        </li>
                       <li></li>
                        <li style=" line-height:40px;">
                            <p style="text-align:right;"><span><%=NgonNgu.LayXau("Chọn trạng thái duyệt:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 120px; padding:2px;\"")%></p>
                        </li>
                    </ul><!--End .inlineBlock-->
                    <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 40%; display:none;\"")%>
                    <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
                </div><!--End #rptMain-->
            </div><!--End #table_form2-->  
    </div>
    <%} %>
    <div>
    </div>
    

    <%
        dtQuy.Dispose();
        dtThang.Dispose();
        
    %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        $(document).ready(function () {
            //            $("ul.inlineBlock li").css({ 'height': '50px' });
            //            $("ul.inlineBlock li span").css({ 'line-height': '23px', 'margin': '1px' });
            //            $("ul.inlineBlock li:last-child span").css("line-height", "");
            //            if ($("#<%=ParentID %>_divPages").val() == 'A4Dung') {
            //                $("#<%=ParentID %>_iSoTo").attr("disabled", true);
            //            }
            $("div#Div2").hide();
            $('.title_tong a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
//                $(this).parent('div').hide();
                return false;
            });
        });
 </script>
 <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKiemTraSoLieuRutDT", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Xuất ra file Excel")%>
     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>

</body>
</html>

