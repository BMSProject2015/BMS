<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>

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
        ul.inlineBlock li p span
        {
            display:inline-block;
            width:150px;
            }
    </style>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
     <%
    String ParentID = "QuyetToanThongTri";
    String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);
    if (String.IsNullOrEmpty(iNamLamViec))
    {
        iNamLamViec = DateTime.Now.Year.ToString();
    }
    DateTime dNgayHienTai = DateTime.Now;
    String NamHienTai = Convert.ToString(dNgayHienTai.Year);
    int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
    int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
    DataTable dtNam = new DataTable();
    dtNam.Columns.Add("MaNam", typeof(String));
    dtNam.Columns.Add("TenNam", typeof(String));
    DataRow R;
    for (int i = NamMin; i < NamMax; i++)
    {
        R = dtNam.NewRow();
        dtNam.Rows.Add(R);
        R[0] = Convert.ToString(i);
        R[1] = Convert.ToString(i);
    }
    dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
    dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    String UserID = User.Identity.Name;
    String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
    String iThang =Convert.ToString(ViewData["iThang"]);
   //Chọn tháng
    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    if (String.IsNullOrEmpty(iThang))
    {
        iThang = Convert.ToString(dtThang.Rows[1]["TenThang"]);
    }
    //chọn loại báo cáo
    String LoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
    if (String.IsNullOrEmpty(LoaiBaoCao))
    {
        LoaiBaoCao = "0";
    }
    DataTable dtLoaiBaoCao = rptKTTongHop_CanDoiNguonVaVonController.DanhSach_LoaiBaoCao();
    SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "MaLoai", "TenLoai");
    dtLoaiBaoCao.Dispose();
    dtThang.Dispose();
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
         String URL="";
         if (PageLoad == "1")
         {
             URL = Url.Action("ViewPDF","rptKTTongHop_CanDoiNguonVaVon",new{ iNamLamViec = iNamLamViec, iThang = iThang, LoaiBaoCao = LoaiBaoCao});
         }
             
    String BackURL = Url.Action("Index", "KeToan_Report", new { sLoai = "1" });
    using (Html.BeginForm("EditSubmit", "rptKTTongHop_CanDoiNguonVaVon", new { ParentID = ParentID }))
 
   {
    %>   
     <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo cân đối nguồn và vốn</span>
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
            <div id="rptMain" style="width: 100%; margin: 0px auto; padding: 0px 0px; overflow: visible; text-align:center;">
                <ul class="inlineBlock">
                    <li >
                        <p style=" height:45px;text-align:right;"><span><%=NgonNgu.LayXau("Chọn tháng:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\"")%></p>
                        
                    </li>
                    <li >
                        <fieldset style=" height:45px;border-radius:4px;-moz-border-radius:4px; -webkit-border-radius:4px;cursor:pointer;">
                            <legend>Bảng cân đối nguồn vốn cho </legend>
                            <p  ><%=MyHtmlHelper.Option(ParentID, "0", LoaiBaoCao, "LoaiBaoCao", "")%> 
                            <span style="display:inline-block; width:120px; margin-left:5px;"><%=NgonNgu.LayXau("Phần tiền:")%></span>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiBaoCao, "LoaiBaoCao", "")%> 
                                 
                            <span style="display:inline-block; width:120px; margin-right:4px; margin-left:5px;"><%=NgonNgu.LayXau("Hiện vật:  ")%>
                                                           
                        </fieldset>
                    </li>  
                       
                    <li >
                    <fieldset style="width:450px;border-radius:4px;-moz-border-radius:4px; -webkit-border-radius:4px;cursor:pointer;">
                        <p style="text-align:right;"><span style="width:450px;"><%=NgonNgu.LayXau("Phần tiền: Mã 94 - Các tài khoản nguồn, mã 95 - các tài khoản vốn")%></span></p>
                        <p style=" text-align:right;"><span style="width:450px;"><%=NgonNgu.LayXau("Phần hiện vật: Mã 96 - Các tài khoản nguồn, mã 97 - các tài khoản vốn")%></span></p>
                    </fieldset>
                    </li>
                </ul><!--End .inlineBlock-->
                <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 40%; display:none;\"")%>
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:20px;" />
                <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:0px;" /></p>
            </div>
        </div>
       
                    
                
    <%} %>
    <div>
    </div>
    

    <%
        dtThang.Dispose();
    %>
     <script type="text/javascript">
         $(function () {
            // $("div#rptMain").hide();
             $('div.login1 a').click(function () {
                 $('div#rptMain').slideToggle('normal');
                 $(this).toggleClass('active');
                 return false;
             });
         });
       
    </script>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=BackURL %>';
         }
 </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTongHop_CanDoiNguonVaVon", new { iNamLamViec = iNamLamViec, iThang = iThang, LoaiBaoCao = LoaiBaoCao }), "Xuất ra file Excel")%>
     <iframe src="<%=URL%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>
