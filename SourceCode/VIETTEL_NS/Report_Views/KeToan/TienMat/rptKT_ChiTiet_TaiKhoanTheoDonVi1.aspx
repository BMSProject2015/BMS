<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienMat" %>
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
        String ParentID = "KeToanTH";
        String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);
        if (String.IsNullOrEmpty(iNamLamViec))
        {
            iNamLamViec = Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(User.Identity.Name, "iNamLamViec"));
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
        String iThang = DanhMucModels.ThangLamViec(UserID).ToString();

        //Chọn tháng
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (String.IsNullOrEmpty(iThang))
        {
            iThang = Convert.ToString(dtThang.Rows[1]["TenThang"]);
        }
        dtThang.Dispose();
        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String iTrangThai = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        String BackURL = Url.Action("Index", "KeToan_CucReport", new { sLoai = "1" });
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptKT_ChiTiet_TaiKhoanTheoDonVi1", new { iNamLamViec = iNamLamViec, iThang = iThang, iTrangThai = iTrangThai });

        using (Html.BeginForm("EditSubmit", "rptKT_ChiTiet_TaiKhoanTheoDonVi1", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width: 45%">
                        <span>CHI TIẾT TÀI KHOẢN THEO ĐƠN VỊ</span>
                    </td>
                    <%--<td><a style="width:150px; background:url('Content/Themes/images/btn_timkiem_le.png') repeat scroll 0px -23px transparent;padding:4px 4px; text-align:center; color:White; font-size:10pt; height:15px; line-height:15px; font-family:Tahoma Arial; font-weight:bold; border-radius:4px;-moz-border-radius:4px; -webkit-border-radius:4px;cursor:pointer;">Xem Báo Cáo</a></td>--%>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%--<div id="Div1">
            <div id="Div2">--%>
        <%--<table width="600px" style="margin:0 auto;" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn năm làm việc")%></div>
                        </td>
                        <td  style="width: 40%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                    </td>
                        <td  class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn Tháng")%></div>
                        </td>
                        <td  style="width: 40%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                    </tr> 
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td style="width: 40%;"> </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;">
                                <tr>
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
                </table>--%>
        <%--  </div>
        </div>--%>
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width: 750px; max-width: 850px; margin: 0px auto; padding: 0px 0px;
                overflow: visible; text-align: center">
                <ul class="inlineBlock">
                    <li style="line-height: 40px;">
                        <p>
                            <span style="padding: 2px; font-size: 14px; font-family: Tahoma Arial;">
                                <%=NgonNgu.LayXau("Trạng thái duyệt") %></span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100px; padding:2px;\" ")%>
                            <span>
                                <%=NgonNgu.LayXau("Tháng:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 80px; padding:2px;\"")%>
                            <span style="padding: 2px; font-size: 14px; font-family: Tahoma Arial;">
                                <%=NgonNgu.LayXau("Năm:") %>
                            <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 100px; \" disabled=\"disabled\"")%></span>
                        </p>
                    </li>
                </ul>
                <!--End .inlineBlock-->
                <p style="text-align: center; padding: 4px;">
                    <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>"
                        style="display: inline-block; margin-right: 5px;" /><input class="button" type="button"
                            value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" style="display: inline-block;
                            margin-left: 5px;" /></p>
            </div>
            <!--End #rptMain-->
        </div>
        <!--End #table_form2-->
    </div>
    <%} %>
    <div>
    </div>
    <%
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
            //  $("div#rptMain").hide();
            $('.title_tong a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                // $(this).parent('div').hide();
                return false;
            });
        });
    </script>
    <%-- <script type="text/javascript">
     function ChonNgay() {
         var NamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value;
         var iThang = document.getElementById("<%=ParentID %>_iThang").value;

         jQuery.ajaxSetup({ cache: false });
         var url = unescape('<%= Url.Action("Get_dtNgay?ParentID=#0&NamLamViec=#1&iThang=#2&iNgay1=#3", "rptKT_ChiTiet_TaiKhoanTheoDonVi1") %>');
         url = unescape(url.replace("#0", "<%= ParentID %>"));
         url = unescape(url.replace("#1", NamLamViec));
         url = unescape(url.replace("#2", iThang));
         url = unescape(url.replace("#3", "<%= iNgay1 %>"));
         $.getJSON(url, function (data) {
             document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
         });
     }                                            
      </script>--%>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKT_ChiTiet_TaiKhoanTheoDonVi1", new { iNamLamViec = iNamLamViec, iThang = iThang}), "Xuất ra file Excel")%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
