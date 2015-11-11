<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
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
        String ParentID = "PhanTichDuToanNS";
        String LoaiThang_Quy = "";
        String NamLamViec = Convert.ToString(ViewData["NamLamViec"]);
        if (String.IsNullOrEmpty(NamLamViec))
        {
            NamLamViec = DateTime.Now.Year.ToString();
        }

        DataTable dtThangLamViec = DanhMucModels.DT_Thang(false);
        String ThangLamViec = Convert.ToString(ViewData["ThangLamViec"]);
        {
            if (String.IsNullOrEmpty(ThangLamViec))
            {
                ThangLamViec = "1";
            }
        }
        SelectOptionList slThangLamViec = new SelectOptionList(dtThangLamViec, "MaThang", "TenThang");
        dtThangLamViec.Dispose();

        DataTable dtQuy = DanhMucModels.DT_Quy(false);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
        if (String.IsNullOrEmpty(LoaiThang_Quy))
        {
            LoaiThang_Quy = "0";
        }
        String iQuy = Convert.ToString(ViewData["iQuy"]);
        if (String.IsNullOrEmpty(iQuy))
        {
            iQuy = "1";
        }
        String iThang = Convert.ToString(ViewData["iThang"]);
        if (String.IsNullOrEmpty(iThang))
        {
            iThang = "1";
        }

        String LoaiIn = Convert.ToString(ViewData["LoaiIn"]);
        if (String.IsNullOrEmpty(LoaiIn))
        {
            LoaiIn = "0";
        }

        String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptPhanTichDuToanNS_52", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaDonVi = iID_MaDonVi, LoaiThang_Quy = LoaiThang_Quy, LoaiIn = LoaiIn });

        String urlExport = Url.Action("ExportToExcel", "rptPhanTichDuToanNS_52", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaDonVi = iID_MaDonVi, LoaiThang_Quy = LoaiThang_Quy, LoaiIn = LoaiIn });
        String BackURL = Url.Action("Index", "KeToan_ChiTiet_Report", new { sLoai = "1" });
        using (Html.BeginForm("EditSubmit", "rptPhanTichDuToanNS_52", new { ParentID = ParentID }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, NamLamViec, "iNamLamViec", "")%>
    <%=MyHtmlHelper.Hidden(ParentID, iID_MaDonVi, "iID_MaDonVi", "")%>
    <%=MyHtmlHelper.Hidden(ParentID, ThangLamViec, "iThangLamViec", "")%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Phân tích dự toán ngân sách theo LNS</span>
                    </td>
                    <td width="52%" style=" text-align:left;">
                        <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("In đến mức:")%></div>
                        </td>
                        <td width="30%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID, "0", LoaiIn, "LoaiIn", "", "style=\"width:10%;\" ")%>Loại ngân sách 
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiIn, "LoaiIn", "", "style=\"width:10%;\" ")%>Mục
                                <%=MyHtmlHelper.Option(ParentID, "2", LoaiIn, "LoaiIn", "", "style=\"width:10%;\" ")%>Tiểu mục 
                                <%=MyHtmlHelper.Option(ParentID, "3", LoaiIn, "LoaiIn", "", "style=\"width:10%;\" ")%>Đơn vị
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn Tháng:")%></div>
                        </td>
                        <td width="25%" style="vertical-align: middle;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:10%;\" ")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThangLamViec, iThang, "iThang", "", "class=\"input1_2\" style=\"width:25%;\" ")%>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:10%;\" ")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:25%;\" ")%>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="4">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    <%} %>
    <iframe src="<%=URLView %>" height="600px" width="100%"></iframe>
    <script type="text/javascript">
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
</body>
</html>
