<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienGui" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
        ul.inlineBlock li fieldset{border:1px solid #cecece;border-radius:3px;-moz-border-radius:3px;-webkit-border-radius:3px;}
        ul.inlineBlock li fieldset span{
            padding:2px 3px 2px 1px;  
            margin-left:2px;  
        }
        select{padding:2px; border:1px solid #dedede;}
        p.liFirst{text-align:right;}
        span.pFirst{float:left;}
        ul.inlineBlock li fieldset legend{text-align:left; padding:3px 5px;}
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <% 
        String ParentID = "KKTienGui_UNC";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        //load tai khoan
        DataTable dtDoiTuong = DanhMucModels.KT_GetDanhSachTaiKhoan("68");
        String DoiTuong = Convert.ToString(ViewData["DoiTuong"]);
        if (String.IsNullOrEmpty(DoiTuong))
        {
            DoiTuong = "0";
        }
        SelectOptionList slDoiTuong = new SelectOptionList(dtDoiTuong, "iID_MaTaiKhoan", "sTen");
        dtDoiTuong.Dispose();
        
        String dTuNgayGhiSo = Convert.ToString(ViewData["dTuNgayGhiSo"]);
        String dDenNgayGhiSo = Convert.ToString(ViewData["dDenNgayGhiSo"]);
        String dTuThangGhiSo = Convert.ToString(ViewData["dTuThangGhiSo"]);
        String dDenThangGhiSo = Convert.ToString(ViewData["dDenThangGhiSo"]);
        String dTuNgayPS = Convert.ToString(ViewData["dTuNgayPS"]);
        String dDenNgayPS = Convert.ToString(ViewData["dDenNgayPS"]);
        String dTuThangPS = Convert.ToString(ViewData["dTuThangPS"]);
        String dDenThangPS = Convert.ToString(ViewData["dDenThangPS"]);
        String sTKDoiUng = Convert.ToString(ViewData["sTKDoiUng"]);
        String sDVNo = Convert.ToString(ViewData["sDVNo"]);
        String sDVCo = Convert.ToString(ViewData["sDVCo"]);
        String sDVNhan = Convert.ToString(ViewData["sDVNhan"]);

        DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt32(dTuThangGhiSo), DateTime.Now.Year,false);
        DataTable dtNgayDen = DanhMucModels.DT_Ngay(Convert.ToInt32(dDenThangGhiSo), DateTime.Now.Year,false);
        DataTable dtThang = DanhMucModels.DT_Thang(false);

        SelectOptionList slTuNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        SelectOptionList slDenNgay = new SelectOptionList(dtNgayDen, "MaNgay", "TenNgay");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dTuThangPS == "")
        {
            dtNgay = DanhMucModels.DT_Ngay(1, DateTime.Now.Year, false);
        }
        else
        {
            dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt32(dTuThangPS), DateTime.Now.Year, false);
        }
        if (dTuThangPS == "")
        {
            dtNgayDen = DanhMucModels.DT_Ngay(DateTime.Now.Month, DateTime.Now.Year, false);
        }
        else
        {
            dtNgayDen = DanhMucModels.DT_Ngay(Convert.ToInt32(dDenThangPS), DateTime.Now.Year, false);
        }

        dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThangPS = new SelectOptionList(dtThang, "MaThang", "TenThang");
        SelectOptionList slTuNgayPS = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        SelectOptionList slDenNgayPS = new SelectOptionList(dtNgayDen, "MaNgay", "TenNgay");

        String iNamLamViec = Convert.ToString(DateTime.Now.Year);
        
        dtNgay.Dispose();
        dtNgayDen.Dispose();
        dtThang.Dispose();
        String URL = Url.Action("Index", "KeToan_ChiTiet_Report");
        String URLView = "";
        if (PageLoad == "1")
        {
            URLView = Url.Action("ViewPDF", "rptKTTienGui_UNC", new
            {
                DoiTuong = DoiTuong,
                dTuNgayGhiSo = dTuNgayGhiSo,
                dDenNgayGhiSo = dDenNgayGhiSo,
                dTuNgayPS = dTuNgayPS,
                dDenNgayPS = dDenNgayPS,
                sTKDoiUng = sTKDoiUng,
                sDVNo = sDVNo,
                sDVCo = sDVCo,
                sDVNhan = sDVNhan,
                dTuThangGhiSo = dTuThangGhiSo,
                dDenThangGhiSo = dDenThangGhiSo,
                dTuThangPS = dTuThangPS,
                dDenThangPS = dDenThangPS
            });
        }
        String urlExport = Url.Action("ExportToExcel", "rptKTTienGui_UNC", new
        {
            DoiTuong = DoiTuong,
            dTuNgayGhiSo = dTuNgayGhiSo,
            dDenNgayGhiSo = dDenNgayGhiSo,
            dTuNgayPS = dTuNgayPS,
            dDenNgayPS = dDenNgayPS,
            sTKDoiUng = sTKDoiUng,
            sDVNo = sDVNo,
            sDVCo = sDVCo,
            sDVNhan = sDVNhan,
            dTuThangGhiSo = dTuThangGhiSo,
            dDenThangGhiSo = dDenThangGhiSo,
            dTuThangPS = dTuThangPS,
            dDenThangPS = dDenThangPS
        });
        using (Html.BeginForm("EditSubmit", "rptKTTienGui_UNC", new
        {
            ParentID = ParentID,
            
        }))
        {
    %>
     <script type="text/javascript">
        function ChonDV(DV) {
            $("input:checkbox[check-group='iID_MaDonVi']").each(function (i) {
                this.checked = DV;
            });
        }    

        function ChonThang(Thang,TenTruong) {            
            var Ngay = document.getElementById("<%=ParentID %>_" + TenTruong).value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&TenTruong=#1&Ngay=#2&Thang=#3&iNam=#4", "rptKTTienGui_UNC") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", TenTruong));
            url = unescape(url.replace("#2", Ngay));
            url = unescape(url.replace("#3", Thang));
            url = unescape(url.replace("#4", <%=iNamLamViec %>));
                $.getJSON(url, function (data) {
                document.getElementById("td_"+TenTruong).innerHTML = data;    
            });            
        }                                     
     </script>
       <script type="text/javascript">
           $(function () {
               $('div.login1 a').click(function () {
                   $('div#rptMain').slideToggle('normal');
                   $(this).toggleClass('active');
                   return false;
               });
           });       
    </script>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Bảng kê chi tiết tài khoản tiền gửi</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td style="width: 1%;">
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn tài khoản:")%></div>
                        </td>
                        <td width="20%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDoiTuong, DoiTuong, "DoiTuong", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 3%;">
                            <div>
                                <%=NgonNgu.LayXau("Ghi sổ ngày:")%></div>
                        </td>
                        <td width="3%" style="vertical-align: middle;">
                            <div>
                                <label id="td_dTuNgayGhiSo">
                                    <%=MyHtmlHelper.DropDownList(ParentID, slTuNgay, dTuNgayGhiSo, "dTuNgayGhiSo", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                                </label>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 3%;">
                            <div>
                                <%=NgonNgu.LayXau("Tháng:")%></div>
                        </td>
                        <td width="3%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, dTuThangGhiSo, "dTuThangGhiSo", "", "class=\"input1_2\" style=\"width:100%;\" onchange=\"ChonThang(this.value,'dTuNgayGhiSo')\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 5%;">
                            <div>
                                <%=NgonNgu.LayXau("Đến ngày:")%></div>
                        </td>
                        <td width="3%" style="vertical-align: middle;">
                            <div>
                                <label id="td_dDenNgayGhiSo">
                                 <%=MyHtmlHelper.DropDownList(ParentID, slDenNgay, dDenNgayGhiSo, "dDenNgayGhiSo", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                                 </label>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 3%;">
                            <div>
                                <%=NgonNgu.LayXau("Tháng:")%></div>
                        </td>
                        <td width="3%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, dDenThangGhiSo, "dDenThangGhiSo", "", "class=\"input1_2\" style=\"width:100%;\"  onchange=\"ChonThang(this.value,'dDenNgayGhiSo')\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 5%;">
                            <div>
                                <%=NgonNgu.LayXau("Ch.Từ PS ngày:")%></div>
                        </td>
                        <td width="3%" style="vertical-align: middle;">
                            <div>
                                <label id="td_dTuNgayPS">
                                <%=MyHtmlHelper.DropDownList(ParentID, slTuNgayPS, dTuNgayPS, "dTuNgayPS", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                                </label>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 3%;">
                            <div>
                                <%=NgonNgu.LayXau("Tháng:")%></div>
                        </td>
                        <td width="3%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThangPS, dTuThangPS, "dTuThangPS", "", "class=\"input1_2\" style=\"width:100%;\"  onchange=\"ChonThang(this.value,'dTuNgayPS')\" ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 6%;">
                            <div>
                                <%=NgonNgu.LayXau("Đến ngày:")%></div>
                        </td>
                        <td width="3%" style="vertical-align: middle;">
                            <div>
                                <label id="td_dDenNgayPS">
                                 <%=MyHtmlHelper.DropDownList(ParentID, slDenNgayPS, dDenNgayPS, "dDenNgayPS", "", "class=\"input1_2\" style=\"width:100%;\" ")%>
                                 </label>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 3%;">
                            <div>
                                <%=NgonNgu.LayXau("Tháng:")%></div>
                        </td>
                        <td width="3%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThangPS, dDenThangPS, "dDenThangPS", "", "class=\"input1_2\" style=\"width:100%;\"  onchange=\"ChonThang(this.value,'dDenNgayPS')\"  ")%>
                            </div>
                        </td>
                        <td style="width: 1%;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1%;">
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("TK đối ứng:")%></div>
                        </td>
                        <td width="20%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTKDoiUng, "sTKDoiUng", "", "class=\"input1_2\" style=\"width:97%;\" maxlength='20'")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("Thuộc Đ.vị nợ:")%></div>
                        </td>
                        <td width="7%" style="vertical-align: middle;" colspan="3">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sDVNo, "sDVNo", "", "class=\"input1_2\" style=\"width:97%;\" maxlength='20'")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("Thuộc Đ.vị có:")%></div>
                        </td>
                        <td width="7%" style="vertical-align: middle;" colspan="3">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sDVCo, "sDVCo", "", "class=\"input1_2\" style=\"width:97%;\" maxlength='20'")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div>
                                <%=NgonNgu.LayXau("Thuộc Đ.vị nhận:")%></div>
                        </td>
                        <td width="7%" style="vertical-align: middle;" colspan="7">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sDVNhan, "sDVNhan", "", "class=\"input1_2\" style=\"width:97%;\" maxlength='20'")%>
                            </div>
                        </td>
                        <td style="width: 1%;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1%;">
                        </td>
                        <td colspan="18">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 47%;" align="right">
                                        <input type="submit" class="button" name="submitButton" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="1%">
                                    </td>
                                    <td style="width: 48%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 1%;">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    <%} %>
    <iframe src="<%=URLView %>" height="600px" width="100%"></iframe>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=URL %>';
         }
    </script>
</body>
</html>
