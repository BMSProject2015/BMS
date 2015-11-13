<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.BaoHiem" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
 <style type="text/css">
     div.login1 {
            text-align : center;    
            background: transparent url(/Content/Report_Image/login.gif) no-repeat top center;
        }    
        div.login1 a {
            color: #545998;
            text-decoration: none;
            font: bold 10px "Museo 700";
            display: block;
            width: 250px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            background: transparent url(/Content/Report_Image/arrow.png) no-repeat 20px -29px;
            -webkit-border-radius:2px;
            border-radius:2px;
        }
        div.login1 a:hover
        {
            text-decoration:underline;
            color:#471083;
        }    
        div.login1 a.active {
            background-position:  20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
    
 </style>
</head>
<body>
    <%
         /// <summary>
         /// Hiển thị danh sách Mã + Ten DV
         /// <prame name="NamLamViec">lấy năm làm việc</prame>
         /// <prame name="UserID">kiểm tra user theo phòng ban</prame>
         /// <prame name="Thang_Quy">lấy tháng</prame>
         /// <prame name="LoaiThangQuy">lấy loại tháng quý</prame>
         /// <prame name="iID_MaDonVi">đơn vị</prame>
         /// </summary>
         /// <returns></returns>
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "BaoHiem";
       // String NamLamViec = Request.QueryString["NamLamViec"];
        String UserID = User.Identity.Name;

        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
        String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String LoaiThangQuy = Convert.ToString(ViewData["LoaiThangQuy"]);
        if (String.IsNullOrEmpty(LoaiThangQuy))
        {
            LoaiThangQuy = "0";
        }
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
       
        /// <summary>
        /// lấy dữ liệu cho combox tháng
        /// </summary>
        /// <returns></returns>
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        /// <summary>
        /// lấy dữ liệu cho combox Quý
        /// </summary>
        /// <returns></returns>
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        /// <summary>
        /// lấy dữ liệu cho combox đơn bị
        /// </summary>
        /// <returns></returns>
        DataTable dtDonVi =rptBH_Chi_ThongTri_72_2Controller.LayDSDonVi(NamLamViec,LoaiThangQuy,Thang_Quy,iID_MaTrangThaiDuyet);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
         //Kiểm tra dữ liệu lớn hơn 0 
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["sTen"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
       
         
        dtDonVi.Dispose();
        // dt Trạng Thái Duyệt
       
        DataTable dtTrangThai = rptBH_ThongTri64Controller.tbTrangThai();
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

        /// <summary>
        /// Kiểm tra radio đơn vị tính
        /// </summary>
        /// <returns></returns>

        String DonViTinh = Convert.ToString(ViewData["DonViTinh"]);
        if (String.IsNullOrEmpty(DonViTinh))
        {
            DonViTinh = Convert.ToString(ViewData["DonViTinh"]);
        }
        String[] arrLoai = { "1", "2" };
        if (String.IsNullOrEmpty(DonViTinh))
            DonViTinh = arrLoai[0];


        String Kieu = Request.QueryString["Kieu"];
        if (String.IsNullOrEmpty(Kieu))
        {
            Kieu = Convert.ToString(ViewData["Kieu"]);
        }
        String[] arrKieu = { "1", "2" };
        if (String.IsNullOrEmpty(Kieu))
            Kieu = arrKieu[0];
        String MaDonVi = "-111";
        int ChiSo = 0;
        //Neu chon chi tiet don vi
        if(DonViTinh=="1")
        {
            String[] arrMaDonVi;
          //= arrMaDonVi[ChiSo];
          
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
            {
                arrMaDonVi = iID_MaDonVi.Split(',');

                ChiSo = Convert.ToInt16(Request.QueryString["ChiSo"]);
                if (ChiSo == arrMaDonVi.Length)
                {
                    ChiSo = 0;
                }
                if (ChiSo <= arrMaDonVi.Length - 1)
                {
                    MaDonVi = arrMaDonVi[ChiSo];
                    ChiSo = ChiSo + 1;
                }
                else
                {
                    ChiSo = 0;
                }
            }
            else
            {
                iID_MaDonVi = "-111";
            } 
        }
        else
        {
            MaDonVi = iID_MaDonVi;
            ChiSo = 0;
        }
        
         // Url Action export ra file excel
        String BackURL = Url.Action("Index", "BaoHiem_Report", new { bChi = 1 });
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptBH_Chi_ThongTri_72_2", new { NamLamViec = NamLamViec, LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, iID_MaDonVi = MaDonVi, DonViTinh = DonViTinh, Kieu = Kieu, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        String urlExport = Url.Action("ExportToExcel", "rptBH_Chi_ThongTri_72_2", new { NamLamViec = NamLamViec, LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, iID_MaDonVi = MaDonVi, DonViTinh = DonViTinh, Kieu = Kieu, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
        using (Html.BeginForm("EditSubmit", "rptBH_Chi_ThongTri_72_2", new { ParentID = ParentID,ChiSo=ChiSo }))
        {
    %>
    <link href="../../../Content/Report_Style/ReportCSS.css" rel="StyleSheet" type="text/css" />
    <script src="../../../Scripts/Report/jquery-1.8.0.min.js" type="text/javascript"></script>    
    <script src="../../../Scripts/Report/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
     <style type="text/css">          
        fieldset{padding:3px;border:1px solid #dedede; border-radius:3px; -webkit-border-radius:3px; -moz-border-radius:3px; margin-bottom:3px;}
        fieldset legend{padding:3px; font-size:14px;font-family:Tahoma Arial;} 
    </style>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo thông tri</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;">
        <div id="rptMain" style="margin:0 auto;padding-top:5px;">
<table>
    <tr>

        <td  rowspan = "3" style =" width:35%; "> 
             <%=MyHtmlHelper.Option(ParentID, "0", LoaiThangQuy, "LoaiThangQuy", "", "onchange=ChonThang()")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "onchange=ChonThang()", "class=\"input1_2\" style=\"width:30%;\" onchange=ChonThang()")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThangQuy, "LoaiThangQuy", "", "onchange=ChonThang()")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "iQuy", "onchange=ChonThang()", "class=\"input1_2\" style=\"width:30%;\"onchange=ChonThang()")%><br />
                                <span><%=NgonNgu.LayXau("Trạng thái duyệt:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 40%;heigh:22px;\"onchange=ChonThang()")%>
        </td>
          <td rowspan = "3" style =" width:25%; ">
            <fieldset >
            <legend style="padding:3px 3px 1px 5px;  height:30px"> Tổng hợp theo</legend>
                <table>
                <tr>
                    <td><%=MyHtmlHelper.Option(ParentID, "1", DonViTinh, "DonViTinh", "")%> &nbsp;&nbsp;<b>Từng đơn vị</b> &nbsp;&nbsp;</td>
                    <td><%=MyHtmlHelper.Option(ParentID, "1", Kieu, "Kieu", "")%> &nbsp;&nbsp;<b>Chi Tiết </b> <br /></td>
                </tr>
                <tr>
                    <td><%=MyHtmlHelper.Option(ParentID, "2", DonViTinh, "DonViTinh", "")%> &nbsp;&nbsp;<b>Toàn bộ</b></td>
                    <td><%=MyHtmlHelper.Option(ParentID, "2", Kieu, "Kieu", "")%> &nbsp;&nbsp;<b>Tổng hợp </b></td>
                </tr>
                </table>
            </fieldset>
        </td>
            <%--<td rowspan = "3" style =" width:35%; ">
            <table><tr><td  align= "left">
            <%=NgonNgu.LayXau("Đơn vị") %></td><td width=" 5px;"></td><td>
            <div id="<%= ParentID %>_tdDonVi" style="width: 200px; height: 200px; overflow: scroll; border:1px solid black;">
                <%rptBH_Chi_ThongTri_72_2Controller rpt = new rptBH_Chi_ThongTri_72_2Controller();%> 
                                <%=rpt.obj_DonVi(ParentID,NamLamViec, LoaiThangQuy, Thang_Quy, iID_MaDonVi,iID_MaTrangThaiDuyet)%></div>
            </td></tr></table>
        </td>--%>
        </td><td width=" 10px;"></td>
        <td  align= " center" style =" width:5%; ">
            <%=NgonNgu.LayXau("Đơn vị") %></td>
        <td width="334" rowspan="4" ><div id="<%= ParentID %>_tdDonVi" style="height:100px;margin-top:5px; overflow:scroll;" >
           
        </div></td>
          
    </tr>

</table>
<table  cellpadding="0" cellspacing="0" border="0" align="center" style="margin:0 auto;>
     <tr>
    <td ><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin:0 auto;">
                                <tr>
                                    <td>
                                        <%--<input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />--%>
                                        <p style="text-align:center; padding:4px; clear:both;"><input type="submit" class="button" id="Submit2" onclick='clicks()' value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>
                                    </td>
                                   <%-- <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>--%>
                                </tr>
                            </table></td>
  </tr>
</table>

</div>
  <script type="text/javascript">
      $(function () {
          $("div#rptMain").show();
          $('div.login1 a').click(function () {
              $('div#rptMain').slideToggle('fast');
              $(this).toggleClass('active');
              return false;
          });
      });
    </script>
     <script type="text/javascript">
         function CheckAll(value) {
             $("input:checkbox[check-group='DonVi']").each(function (i) {
                 this.checked = value;
             });
         }                                            
     </script>
    <script type="text/javascript">
        ChonThang();
        function ChonThang() {
            
            var LoaiThang_QuyCheck = document.getElementById("<%= ParentID %>_LoaiThangQuy").checked;
            var LoaiThangQuy
            var Thang
            if (LoaiThang_QuyCheck == true) {
                Thang = document.getElementById("<%= ParentID %>_iThang").value;
                LoaiThangQuy = 0;
            }
            else {
                Thang = document.getElementById("<%= ParentID %>_iQuy").value;
                LoaiThangQuy = 1;
            }
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&NamLamViec=#1&LoaiThangQuy=#2&Thang_Quy=#3&iID_MaDonVi=#4&iID_MaTrangThaiDuyet=#5", "rptBH_Chi_ThongTri_72_2") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= NamLamViec %>"));
            url = unescape(url.replace("#2", LoaiThangQuy));
            url = unescape(url.replace("#3", Thang));
            url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#5", iID_MaTrangThaiDuyet));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
            });
        }                                            
     </script>
        </div>
        <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
    </div>
    <%} %>
    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
