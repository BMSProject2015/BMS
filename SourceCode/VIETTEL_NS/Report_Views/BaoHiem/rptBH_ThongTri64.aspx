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
    <style type="text/css">
        .style1
        {
            width: 337px;
        }
    </style>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
 <style type="text/css">
     div.login1 {
            text-align : center;    
            
        }    
        div.login1 a {
            color: #545998;
            text-decoration: none;
            font: bold 10px "Museo 700";
            display: block;
            width: 250px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
          
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
        String MaND = User.Identity.Name;
       
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String Thang = "0", Quy = "0";
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
        if (LoaiThangQuy == "0")
        {
            Thang = Thang_Quy;
            Quy = "0";
        }
        else
        {
            Thang = "0";
            Quy = Thang_Quy;
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
        DataTable dtDonVi = rptBH_ThongTri64Controller.LayDSDonVi(MaND,LoaiThangQuy,Thang_Quy,iID_MaTrangThaiDuyet);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
         //Kiểm tra dữ liệu lớn hơn 0 
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        String[] arrDonVi = iID_MaDonVi.Split(',');     
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
         
        String iDonViDong = Convert.ToString(ViewData["iDonViDong"]);
        if (String.IsNullOrEmpty(iDonViDong))
        {
            iDonViDong = Convert.ToString(ViewData["iDonViDong"]);
        }
        String[] arriDonViDong = { "1", "2","3" };
        if (String.IsNullOrEmpty(iDonViDong))
            iDonViDong = arriDonViDong[0];

        /// <summary>
        /// Kiểm tra radio loại thông tri
        /// </summary>
        /// <returns></returns>
         
        String LoaiThongTri = Convert.ToString(ViewData["LoaiThongTri"]);
        if (String.IsNullOrEmpty(LoaiThongTri))
        {
            LoaiThongTri = Convert.ToString(ViewData["LoaiThongTri"]);
        }
        String[] arrLoaiThongTri = { "TongHopDonVi", "TongHop", };
        if (String.IsNullOrEmpty(LoaiThongTri))
            LoaiThongTri = arrLoaiThongTri[0];


        String[] arrMaDonVi;
        String MaDonVi = "-111";//= arrMaDonVi[ChiSo];
        int ChiSo = 0;
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
        if (LoaiThongTri == "TongHop")
        {
            ChiSo = 0;
            MaDonVi = iID_MaDonVi;
        }

        /// <summary>
        /// Kiểm tra radio loại bảo hiểm
        /// </summary>
        /// <returns></returns>
        String LoaiBaoHiem = Convert.ToString(ViewData["LoaiBaoHiem"]);
        if (String.IsNullOrEmpty(LoaiBaoHiem))
        {
            LoaiBaoHiem = Convert.ToString(ViewData["LoaiBaoHiem"]);
        }
        String[] arrLoaiBaoHiem = { "1", "2", "3","4" };
        if (String.IsNullOrEmpty(LoaiBaoHiem))
            LoaiBaoHiem = arrLoaiBaoHiem[0];
         
         
         //
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptBH_ThongTri64", new { MaND = MaND, iID_MaDonVi = iID_MaDonVi, LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, LoaiBaoHiem = LoaiBaoHiem, LoaiThongTri = LoaiThongTri, iDonViDong = iDonViDong, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, ChiSo = ChiSo, sMaDonVi = MaDonVi });
         // Url Action export ra file excel
        String BackURL = Url.Action("Index", "BaoHiem_Report",new {bChi=0});
        String urlExport = Url.Action("ExportToExcel", "rptBH_ThongTri64", new { MaND = MaND, iID_MaDonVi = iID_MaDonVi, LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, LoaiBaoHiem = LoaiBaoHiem, LoaiThongTri = LoaiThongTri, iDonViDong = iDonViDong, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, ChiSo = ChiSo, sMaDonVi = MaDonVi });
        using (Html.BeginForm("EditSubmit", "rptBH_ThongTri64", new { ParentID = ParentID, ChiSo = ChiSo }))
        {
    %>
    
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;">
        <div id="rptMain" style="margin:0 auto;background-color:#F0F9FE;">
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 191px">
  <tr>
    <td width="20">&nbsp;</td>
    <td width="100"><b>Trạng thái :</b></td>
    <td width="247"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 84%;heigh:22px;\"onchange=ChonThang()")%></td>
    <td width="14">&nbsp;</td>
    <td width="260" rowspan="2" valign="top"><fieldset style="height:70px;width:80%;text-align:left;margin-top:5px;padding-left:3px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
            <legend><b>Đơn vị đóng</b></legend>
                            &nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "1", iDonViDong, "iDonViDong", "")%> &nbsp;Cá nhân<br />
                           &nbsp; <%=MyHtmlHelper.Option(ParentID, "2", iDonViDong, "iDonViDong", "")%>&nbsp;&nbsp;Đơn vị<br />
                            &nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "3", iDonViDong, "iDonViDong", "")%>&nbsp;Tổng cộng
         
    </fieldset></td>
    <td width="334" rowspan="4"><div id="<%= ParentID %>_tdDonVi" style="height:130px;margin-top:5px; overflow:scroll;" >
            
        </div></td>
    <td width="9">&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td><b>Tháng / quý : </b></td>
    <td><div>                               
                               <%=MyHtmlHelper.Option(ParentID, "0", LoaiThangQuy, "LoaiThangQuy", "", "onchange=ChonThang()")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "onchange=ChonThang()", "class=\"input1_2\" style=\"width:26%;\" onchange=ChonThang()")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThangQuy, "LoaiThangQuy", "onchange=ChonThang()", "")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "onchange=ChonThang()", "class=\"input1_2\" style=\"width:26%;\"onchange=ChonThang()")%><br />
                            </div></td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td colspan="2" rowspan="2" valign="top"><fieldset style="height:40px;padding-left:13px;padding-top:5px;width:85%;text-align:left; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
            <legend><b>Chọn Loại bảo hiểm</b></legend>
                             &nbsp;<%=MyHtmlHelper.Option(ParentID, "1", LoaiBaoHiem, "LoaiBaoHiem", "")%> &nbspBHXH&nbsp
                           &nbsp; <%=MyHtmlHelper.Option(ParentID, "2", LoaiBaoHiem, "LoaiBaoHiem", "")%>&nbsp;&nbsp;BHYT&nbsp
                             &nbsp; <%=MyHtmlHelper.Option(ParentID, "3", LoaiBaoHiem, "LoaiBaoHiem", "")%>&nbsp;&nbsp;BHTN&nbsp
                            &nbsp;<%=MyHtmlHelper.Option(ParentID, "4", LoaiBaoHiem, "LoaiBaoHiem", "")%>&nbsp;&nbsp;BHXH-YN-TN
         
    </fieldset></td>
    <td>&nbsp;</td>
    <td rowspan="2" valign="top"><fieldset style="height:40px;padding-top:5px;width:80%;text-align:left;padding-left:3px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
            <legend><b>Loại Thông tri</b></legend>
              <%=MyHtmlHelper.Option(ParentID, "TongHopDonVi", LoaiThongTri, "LoaiThongTri", "", "")%>&nbsp;
                                      Từng đơn vị
                                       &nbsp;
                                    <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiThongTri, "LoaiThongTri", "", "")%>&nbsp;
                                        Tổng hợp
         
    </fieldset> </td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="7" align="center"><table cellpadding="0" cellspacing="0" border="0" style="margin: 10px;text-align:center;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
  </tr>
</table>

</div>
<%-- <script type="text/javascript">
     function Chonall(sLNS) {
         $("input:checkbox[check-group='MaDonVi']").each(function (i) {
             if (sLNS) {
                 this.checked = true;
             }
             else {
                 this.checked = false;
             }
         });

     }                                            
    </script>--%>

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
            var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&MaND=#1&LoaiThangQuy=#2&Thang_Quy=#3&iID_MaDonVi=#4&iID_MaTrangThaiDuyet=#5", "rptBH_ThongTri64") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= MaND %>"));
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
            window.location.href ='<%=BackURL%>';
        }
    </script>
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
    </div>
    <%} %>
    <iframe src="<%=URLView%>" height="600px" width="100%">
    </iframe>
</body>
</html>
