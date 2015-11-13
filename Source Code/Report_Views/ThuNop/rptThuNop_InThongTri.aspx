<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
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
            font-family:Tahoma Arial; color:Black; line-height:23px; cursor:pointer; background-color:#dedede;  
        }
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <% 
        String ParentID = "KeToan";
        String iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
          String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
           String sSoCT = Convert.ToString(ViewData["sSoCT"]);
          String iThang = Convert.ToString(ViewData["iThang"]);
          String iNam = Convert.ToString(ViewData["iNam"]);
          String iID_MaThongTri = Convert.ToString(ViewData["iID_MaThongTri"]);
          String iLoai = Convert.ToString(ViewData["iLoai"]);
        String URL = Url.Action("Index", "ThuNop_ChungTuChiTiet",new { iID_MaChungTu = iID_MaChungTu });
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptThuNopThongTri", new { iID_MaChungTu = iID_MaChungTu, iID_MaDonVi = iID_MaDonVi, iThang = iThang, iNam = iNam, iID_MaThongTri = iID_MaThongTri, sSoCT = sSoCT, iLoai = iLoai });
        
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Thông tri Thu nộp </span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
     <script type="text/javascript">
        function Huy() {
            window.parent.location.href = '<%=URL %>';
        }  </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThuNopThongTri", new { iID_MaChungTu = iID_MaChungTu, iID_MaDonVi = iID_MaDonVi, iThang = iThang, iNam = iNam, iID_MaThongTri = iID_MaThongTri, iLoai = iLoai }), "Xuất ra Excel")%>

    <iframe src="<%=UrlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
