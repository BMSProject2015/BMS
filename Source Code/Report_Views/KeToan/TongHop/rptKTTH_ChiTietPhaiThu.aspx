<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
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
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "KeToan";
        String MaND = User.Identity.Name;
     
        Object objNam = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
        String iThangLamViec = "1";
        String iNamLamViec = DateTime.Now.Year.ToString();
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        {
            iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            iThangLamViec = dtCauHinh.Rows[0]["iThangLamViec"].ToString();
        }
        dtCauHinh.Dispose();

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
       
        String iThang = Convert.ToString(ViewData["iThang"]);
        if (String.IsNullOrEmpty(iThang)) iThang = iThangLamViec;
        if (iThang == "0")
        {
            iThang = "1";
        }
            
        String iNgay = Convert.ToString(ViewData["iNgay"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        int ThangHienTai = DateTime.Now.Month;
        //Chọn từ tháng
        DataTable dtThang = DanhMucModels.DT_Thang(false);

        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        //Chọn từ ngày

        DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(iThang), Convert.ToInt16(iNamLamViec), false);
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        if (String.IsNullOrEmpty(iNgay))
        {
            iNgay = Convert.ToString(dtNgay.Rows[dtNgay.Rows.Count - 1]["TenNgay"]);
        }
        dtNgay.Dispose();
        
      
        String DVT = Convert.ToString(ViewData["DVT"]);
        if (String.IsNullOrEmpty(DVT))
        {
            DVT = "0";
        }
        DataTable dtDVT = rptKTTH_TongHopCapVonController.DanhSach_LoaiBaoCao();
        SelectOptionList slLoaiBaoCao = new SelectOptionList(dtDVT, "MaLoai", "TenLoai");
        dtDVT.Dispose();
        String Mucin = Convert.ToString(ViewData["Mucin"]);
        DataTable dtMucin = rptKTTH_ChiTietPhaiThuController.DanhSach_Mucin();
        SelectOptionList slMucin = new SelectOptionList(dtMucin, "MucIn", "TenIn");
        if (String.IsNullOrEmpty(Mucin))
        {
            Mucin = "0";
        }
        dtMucin.Dispose();
        DataTable dtTrangThai = rptKTTH_TongHopCapVonController.tbTrangThai();
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
        dtTrangThai.Dispose();
        String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
       // VIETTEL.Report_Controllers.KeToan.TienMat.rptChiTietTheoDonVi_2Controller ctlCTDV = new VIETTEL.Report_Controllers.KeToan.TienMat.rptChiTietTheoDonVi_2Controller();

        String urlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptKTTH_ChiTietPhaiThu", new { iNamLamViec = iNamLamViec, iThang = iThang, iNgay = iNgay, DVT = DVT, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Mucin = Mucin });
        }
        using (Html.BeginForm("EditSubmit", "rptKTTH_ChiTietPhaiThu", new { ParentID = ParentID }))
        {
    %>
    
     <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style = "width:45%">
                        <span>Báo cáo chi tiết phải thu (Tài khoản 311)</span> 
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
                <div id="rptMain" style=";  margin:5px auto; padding:5px 5px; overflow:visible; text-align:center">
               <ul class="inlineBlock">
               
               <li>
               
                        <fieldset style="padding: 2px;  border-radius: 5px; width: 300px;
                        margin-right: 3px;height:80px">
                         
                    <legend style="font-weight: bold;"> 
                            <%=NgonNgu.LayXau("Đến ngày&nbsp;&nbsp;..Tháng&nbsp;&nbsp;..Năm")%> </legend>
                        <p>
                            <label id="<%= ParentID %>_ngay1">
                               <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay, "iNgay", "","style=\"width:70px\"")%>
                               <%--<% rptKTTH_ChiTietPhaiThuController rpt = new rptKTTH_ChiTietPhaiThuController();%>
                                    <%= rpt.get_sNgayThang(ParentID, MaND,iThang, iNgay) %>--%>
                               </label>
                            <label>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "style=\"width:30%\" onchange=\"ChonThang()\"")%></label>

                                <%=MyHtmlHelper.TextBox(ParentID, iNamLamViec, "", "", "class=\"input1_2\" style=\"width: 100px; \" disabled=\"disabled\"")%>
                        </p>
                    </fieldset>
               
               </li>
               
               <li>
               
                     <fieldset style="padding: 2px;  border-radius: 5px; width: 300px;
                        margin-right: 3px;height:80px">
                    <legend style="font-weight: bold;"> 
                            <%=NgonNgu.LayXau("Đơn vị tính &nbsp;&nbsp;&nbsp;Trạng thái duyệt")%></legend>
                          <p> <label>  <%=MyHtmlHelper.DropDownList(ParentID,slLoaiBaoCao,DVT,"DVT","style=\"width:40%\"") %></label>
                           <label>  <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "style=\"width:40%\"")%></label></p>
                    </fieldset>
               </li>
               <li>
                 <fieldset style="padding: 2px;  border-radius: 5px; width: 350px;
                        margin-right: 3px;height:80px;text-align:left">
                  
                        <legend style="font-weight: bold;">
                            <%=NgonNgu.LayXau("Kiểu in:") %></legend>
                        <div style="padding-bottom: 5px;">
                            <%=MyHtmlHelper.Option(ParentID, "0", Mucin, "Mucin", "")%><span style="font-size: 12px;
                                font-weight: bold;">1. In theo đơn vị - nội dung tài khoản chi tiết</span></div>
                        <div>
                            <%=MyHtmlHelper.Option(ParentID, "1", Mucin, "Mucin", "")%><span style="font-size: 12px;
                                font-weight: bold;">2. In theo các đơn vị - nội dung tài khoản chi tiết</span></div>
                                <div>
                            <%=MyHtmlHelper.Option(ParentID, "2", Mucin, "Mucin", "")%><span style="font-size: 12px;
                                font-weight: bold;">3. In theo nội dung tài khoản chi tiết - theo đơn vị</span></div>
                    </fieldset>
               </li>
               </ul>
                    <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 40%; display:none;\"")%>
                    <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
                </div><!--End #rptMain-->
            </div><!--End #table_form2-->  
    </div>
    <%} %>
    <div>
    </div>
   
    <script type="text/javascript">

        function ChonThang() {
            var iThang = document.getElementById("<%=ParentID %>_iThang").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&MaND=#1&iThang=#2&iNgay=#3","rptKTTongHop_ChiTietCacKhoanTamUng") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= MaND %>"));
            url = unescape(url.replace("#2", iThang));
            url = unescape(url.replace("#3", "<%= iNgay %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID%>_ngay1").innerHTML = data;
            });
        }       
      
function Huy() {
            window.location.href = '<%=BackURL %>';
        }
        $(document).ready(function () {
      
            $('.title_tong a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                  return false;
            });
        });           
    </script>
  
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTH_ChiTietPhaiThu", new { iNamLamViec = iNamLamViec, iThang = iThang, iNgay = iNgay, DVT = DVT, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Mucin = Mucin }), "Xuất ra file Excel")%>
    <div>
        <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>

