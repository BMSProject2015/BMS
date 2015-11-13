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
     String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "QuyetToan";
        String UserID = User.Identity.Name;
        String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);  
        //String LoaiThang_Quy = Convert.ToString(Request.QueryString["LoaiThang_Quy"]);
        if (String.IsNullOrEmpty(LoaiThang_Quy))
        {
            LoaiThang_Quy = "0";
        }
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);  
        //String Thang_Quy = Convert.ToString(Request.QueryString["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String NamLamViec = Convert.ToString(ViewData["NamLamViec"]);  
       // String NamLamViec = Request.QueryString["NamLamViec"];
        if (String.IsNullOrEmpty(NamLamViec))
        {
            NamLamViec = DanhMucModels.NamLamViec(UserID).ToString();
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
        String iID_MaNguonNganSach = Convert.ToString(ViewData["iID_MaNguonNganSach"]);  
       // String iID_MaNguonNganSach = Request.QueryString["iID_MaNguonNganSach"];
        //Chọn ngân sách
        DataTable dtloai = rptDoiChieuSuDungKinhPhiController.Lay_LoaiNganSach();
        SelectOptionList slloai = new SelectOptionList(dtloai, "iID_MaNguonNganSach", "sTen");
        if (String.IsNullOrEmpty(iID_MaNguonNganSach))
        {
            if(dtloai.Rows.Count>0)
                iID_MaNguonNganSach = Convert.ToString(dtloai.Rows[0]["iID_MaNguonNganSach"]);
            iID_MaNguonNganSach = Guid.Empty.ToString();
        }
        dtloai.Dispose();
        String inmuc = Convert.ToString(ViewData["inmuc"]);  
        //String inmuc = Request.QueryString["inmuc"];
        if (String.IsNullOrEmpty(inmuc))
        {
            inmuc = "1";
        }
        DataTable dtinmuc = rptDoiChieuSuDungKinhPhiController.DanhSach_inmuc();
            SelectOptionList slinmuc = new SelectOptionList(dtinmuc, "MaMuc", "TenMuc");
        dtinmuc.Dispose();
            //chon nam ngan sach
        String iID_MaNamNganSach = Convert.ToString(ViewData["iID_MaNamNganSach"]);
        //String iID_MaNamNganSach = Request.QueryString["iID_MaNamNganSach"];
        DataTable dtNamNS = DanhMucModels.NS_NamNganSach();
        SelectOptionList slNamNS = new SelectOptionList(dtNamNS, "iID_MaNamNganSach", "sTen");
        if (String.IsNullOrEmpty(iID_MaNamNganSach))
        {
            if (dtNamNS.Rows.Count > 0)
                iID_MaNamNganSach = Convert.ToString(dtNamNS.Rows[0]["iID_MaNamNganSach"]);
            iID_MaNamNganSach = Guid.Empty.ToString();
        }
        dtNamNS.Dispose();
        String BackURL = Url.Action("Index", "KeToan_ChiTiet_Report", new { sLoai = "1" });

        DataTable dtTrangThai = rptDoiChieuSuDungKinhPhiController.tbTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
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
       
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptDoiChieuSuDungKinhPhi", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, inmuc = inmuc, iID_MaNguonNganSach = iID_MaNguonNganSach, iID_MaNamNganSach = iID_MaNamNganSach, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        using (Html.BeginForm("EditSubmit", "rptDoiChieuSuDungKinhPhi", new { ParentID = ParentID }))
        {
        %>  
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td width="47.9%"><span>Báo cáo đối chiếu sử dụng kinh phí</span></td>
                        <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                        </td>
                    </tr>
                </table>
            </div>
             <div id="table_form2" class="table_form2">
                <div id="rptMain" style="width:750px; max-width:800px; margin:0px auto; padding:0px 0px; overflow:visible;">
                    <ul class="inlineBlock">   
                    <Table width="100%;">
                        <tr>    
                             <td rowspan="3" style= " width:20%;">           
                                <li style= " width : 95%;">
                                    <p><%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "")%><span style="display:inline-block; width:50px; margin-left:5px;"><%=NgonNgu.LayXau("Tháng:") %></span> <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "", "class=\"input1_2\" style=\"width:60px; padding:2px;\"" )%></p>                            
                                    <p><%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "")%><span style="display:inline-block; width:50px; margin-right:4px; margin-left:5px;"><%=NgonNgu.LayXau("Quý:  ") %></span><%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "iQuy", "", "class=\"input1_2\" style=\"width:60px; padding:2px;\"" )%></p>
                                </li>
                                </td>    
                             <td rowspan="3" style= " width:42%;">
                                <li style= " width :100%;">
                                    <p style="text-align:right;"><%=NgonNgu.LayXau("Chọn ngân sách:") %><%=MyHtmlHelper.DropDownList(ParentID, slloai, iID_MaNguonNganSach, "iID_MaNguonNganSach", "", "class=\"input1_2\" style=\"width: 200px; padding:2px;\"")%></p>
                                    <p style="text-align:right;"><%=NgonNgu.LayXau("Năm ngân sách:")%><%=MyHtmlHelper.DropDownList(ParentID, slNamNS, iID_MaNamNganSach, "iID_MaNamNganSach", "", "class=\"input1_2\" style=\"width: 200px; padding:2px;\"")%></p>
                                </li>
                                </td>  
                             <td rowspan="3" style= " width:38%;">
                                <li style= " width : 95%;">
                                    <p style="text-align:right;"><%=NgonNgu.LayXau("Chọn mức in:") %><%=MyHtmlHelper.DropDownList(ParentID, slinmuc, inmuc, "inmuc", "", "class=\"input1_2\" style=\"width: 70px; padding:2px;\"")%></p>
                                    <p style="text-align:right;"><%=NgonNgu.LayXau("Chọn trạng thái duyệt:") %><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 120px; padding:2px;\"")%></p>

                                </li>
                            </td>
                            </tr> 
                        </Table>  
                    </ul><!--End .inlineBlock-->
                    <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 40%; display:none;\"")%>
                    <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
                </div><!--End #rptMain-->
            </div><!--End #table_form2-->            
        </div>
        <%} %>    
        <%
            dtQuy.Dispose();
            dtThang.Dispose();
            dtloai.Dispose();        
            dtinmuc.Dispose();
        %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        $(function () {
            $("div#rptMain").hide();
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
            $("#<%=ParentID %>_inmuc").css("margin-right","50px");
        });
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDoiChieuSuDungKinhPhi", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, inmuc = inmuc, iID_MaNguonNganSach = iID_MaNguonNganSach, iID_MaNamNganSach = iID_MaNamNganSach, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Xuất ra file Excel")%>
    <iframe src="<%=UrlReport %>" height="600px" width="100%">
    </iframe>
</body>
</html>