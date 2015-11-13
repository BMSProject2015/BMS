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
        ul.inlineBlock li fieldset{border:1px solid #cecece;border-radius:3px;-moz-border-radius:3px;-webkit-border-radius:3px;}
        ul.inlineBlock li fieldset span{
            padding:2px 3px 2px 1px;  
            margin-left:2px;  
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
        select{border:1px solid #dedede;padding:2px;}
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
     String srcFile = Convert.ToString(ViewData["srcFile"]);
            String ParentID = "QuyetToan";
            String UserID = User.Identity.Name;
            String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
           // String LoaiThang_Quy = Convert.ToString(Request.QueryString["LoaiThang_Quy"]);
            if (String.IsNullOrEmpty(LoaiThang_Quy))
            {
                LoaiThang_Quy = "0";
            }
            String Thang_Quy = Convert.ToString(ViewData["ThangQuy"]);
            //String Thang_Quy = Convert.ToString(Request.QueryString["Thang_Quy"]);
            if (String.IsNullOrEmpty(Thang_Quy))
            {
                Thang_Quy = "1";
            }
            String NamLamViec = Convert.ToString(ViewData["NamLamViec"]);
           // String NamLamViec = Request.QueryString["NamLamViec"];
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

            String iID_MaChuongTrinhMucTieu = Convert.ToString(ViewData["iID_MaChuongTrinhMucTieu"]);
   // String iID_MaChuongTrinhMucTieu = Request.QueryString["iID_MaChuongTrinhMucTieu"];
    //Chọn CTMT
        DataTable dtloai = rptDoiChieuCTMTController.Lay_CTMT();
        SelectOptionList slloai = new SelectOptionList(dtloai, "iID_MaChuongTrinhMucTieu", "sTenchuongTrinhMucTieu");
        if (String.IsNullOrEmpty(iID_MaChuongTrinhMucTieu))
        {
            if (dtloai.Rows.Count > 0)
                iID_MaChuongTrinhMucTieu = Convert.ToString(dtloai.Rows[0]["iID_MaChuongTrinhMucTieu"]);
            else iID_MaChuongTrinhMucTieu = Guid.Empty.ToString();
            
        }
        dtloai.Dispose();

    
         //chọn mức in
        String inmuc = Convert.ToString(ViewData["inmuc"]);
   // String inmuc = Request.QueryString["inmuc"];
    if (String.IsNullOrEmpty(inmuc))
    {
        inmuc = "1";
    }
        DataTable dtinmuc = rptDoiChieuCTMTController.DanhSach_inmuc();
        SelectOptionList slinmuc = new SelectOptionList(dtinmuc, "MaMuc", "TenMuc");
    dtinmuc.Dispose();
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
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String UrlReport = "";
    if (PageLoad == "1")
        UrlReport = Url.Action("ViewPDF", "rptDoiChieuCTMT", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, inmuc = inmuc, iID_MaChuongTrinhMucTieu = iID_MaChuongTrinhMucTieu, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
            
    String BackURL = Url.Action("Index", "KeToan_ChiTiet_Report", new { sLoai = "1" });        
    using (Html.BeginForm("EditSubmit", "rptDoiChieuCTMT", new { ParentID = ParentID }))
   {
    %>  
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>                    
                    <td width="47.9%"><span>Báo cáo đối chiếu CTMT</span></td>
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
                        <td style="width: 35%;" >
                            <table style="width: 100%;" >
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                            <%=NgonNgu.LayXau("Chọn năm làm việc")%>
                                        </div>
                                    </td>
                                    <td style="width: 40%;">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 40%\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                      <div>
                                            <%=NgonNgu.LayXau("Chương trình mục tiêu")%></div>
                                    </td>
                                    <td  style="width: 40%;">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slloai, iID_MaChuongTrinhMucTieu, "iID_MaChuongTrinhMucTieu", "", "class=\"input1_2\" style=\"width: 40%\"")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                                    
                        </td>
                            
                        <td style="width: 35%;" >
                            <table style="width: 100%;" >
                            <tr>
                                <td align="center">
                                    
                                    <div>
                                        <%=NgonNgu.LayXau("In đến mức")%>
                                    </div>
                                       
                                </td>
                                <td align="left">
                                    <table>
                                    <tr>
                                    <td align="left">
                                    <div>
                                    <%=MyHtmlHelper.Option(ParentID, "1", inmuc, "inmuc", "")%> Mục:&nbsp;&nbsp;
                                    </div> </td>
                                </tr>
                                    <tr><tdalign="left"> <div>
                                    <%=MyHtmlHelper.Option(ParentID, "2", inmuc, "inmuc", "")%> Tiểu mục:&nbsp;&nbsp;</div></td></tr>
                                    <tr><td align="left"> <div> <%=MyHtmlHelper.Option(ParentID, "3", inmuc, "inmuc", "")%> Đơn vị:&nbsp;&nbsp;</div></td>
                                </tr>
                                    </table>
                                </td>
                            </tr>
                              
                               
                            </table>
                        </td>
                         <td style="width: 30%;" >
                            <table style="width: 100%;" >
                                <tr style="height:23px; margin-bottom:3px;">
                                    <td class="td_form2_td1" style="width: 10%;">
                                        <div>
                                             <%=NgonNgu.LayXau(" ")%>
                                        </div>
                                    </td>
                                    <td style="width: 100px;">
                                        <div>
                                            <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "")%> Tháng:&nbsp;
                                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "", "class=\"input1_2\" style=\"width:30%; \"")%>
                                         </div>
                                    </td>
                                </tr>
                                <tr style="height:23px; margin-top:3px;">
                                    <td class="td_form2_td1" style="width: 10%;">
                                      <div>
                                       <%=NgonNgu.LayXau(" ")%>
                                      </div>
                              
                                    </td>
                                    <td style="width: 100px;">
                                        <div>
                                            <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "")%> Quý:&nbsp;&nbsp;&nbsp;&nbsp;
                                            <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "iQuy", "", "class=\"input1_2\" style=\"width:30%;\"")%>                           
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                   <tr>
                        <td colspan ="2">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;">
                                <tr> 
                                    <td style="width: 75%;"> </td>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>   
                </table>
            </div>
        </div>--%>
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width:1024px; max-width:1024px; margin:0px auto; padding:0px 0px; overflow:visible; text-align:center;">
                <ul class="inlineBlock">                       
                    <table width 100%>
                        <tr> 
                        <td rowspan="3" style= " width:30%;">
                            <li>
                                <fieldset style="padding:3px 5px;font-size:13px; height:70px;">
                                    <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Chọn thời gian:")%></legend>
                                    <p style="padding:10px 1px; margin-bottom:5px;">
                                       <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "")%><%=NgonNgu.LayXau("Tháng") %>
                                       <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "", "class=\"input1_2\" style=\"width:46px;\"")%>&nbsp;&nbsp;
                                       <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "")%><%=NgonNgu.LayXau("Quý") %>
                                       <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "iQuy", "", "class=\"input1_2\" style=\"width:46px;\"")%>
                                    </p>
                                </fieldset>
                            </li>
                        </td>
                        <td rowspan="3" style= " width:30%; text-align: left;">
                            <li style=" width:95%;">
                                <fieldset style="border:1px solid #cecece;padding:3px 5px; font-size:13px;height:70px; width:95%;">
                                    <legend style="padding:3px 3px 1px 5px; width:100%;"><%=NgonNgu.LayXau("In đến mức:")%></legend>
                                    <p style="padding:10px 1px; margin-bottom:5px;">
                                       <%=MyHtmlHelper.Option(ParentID, "1", inmuc, "inmuc", "")%><%=NgonNgu.LayXau("Mục") %> &nbsp;&nbsp;                         
                                      <%=MyHtmlHelper.Option(ParentID, "2", inmuc, "inmuc", "")%><%=NgonNgu.LayXau("Tiểu mục") %>&nbsp;&nbsp;
                                       <%=MyHtmlHelper.Option(ParentID, "3", inmuc, "inmuc", "")%><%=NgonNgu.LayXau("Đơn vị") %>
                                    </p>
                                </fieldset>
                            </li>
                        </td>
                        <td style=" width 2%"></td>
                        <td rowspan="3" style= " width:30%">
                            <li>
                                <fieldset style="padding:1px;font-size:13px;">
                                    <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Chương trình mục tiêu:")%></legend>
                                    <p style="text-align:right;">
                                        <%=MyHtmlHelper.DropDownList(ParentID, slloai, iID_MaChuongTrinhMucTieu, "iID_MaChuongTrinhMucTieu", "", "style=\"width:240px; font-size:13px;\" size='3' tab-index='-1' ")%>
                                    </p>
                                </fieldset>
                            </li>
                        </td>
                        <td rowspan="3" style= " width:18%">
                            <li>
                                <fieldset>
                                    <legend style="padding:3px 3px 1px 5px;"><%=NgonNgu.LayXau("Trạng thái duyệt:") %></legend>
                                    <p style="text-align:right;"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 150px; font-size:13px;\" size='3' tab-index='-1'")%></p>
                                </fieldset>
                            </li>
                        </td>
                        </tr>
                    </table>
                </ul><!--End .inlineBlock-->
                <%=MyHtmlHelper.Hidden(ParentID,NamLamViec,"iNamLamViec","")%>
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
            </div><!--End #rptMain-->
        </div>
    </div>
    <%} %>
    <div>
    </div>

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
            $('div.login1 a').click(function () {
            $('div#rptMain').slideToggle('normal');
            $(this).toggleClass('active');
            return false;
            });
        });
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDoiChieuCTMT", new { NamLamViec = NamLamViec, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, inmuc = inmuc, iID_MaChuongTrinhMucTieu = iID_MaChuongTrinhMucTieu,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet }), "Xuất ra file Excel")%>
     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>

</body>
</html>
