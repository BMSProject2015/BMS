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
<head id="Head1" runat="server">
    <title></title>
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
    
     .style5
     {
         width: 27px;
     }
     .style6
     {
         width: 73px;
     }
     .style7
     {
         width: 24px;
     }
     .style8
     {
         width: 153px;
     }
     .style9
     {
         width: 235px;
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
        //  MaND, ThangQuy, LoaiThang_Quy, iID_MaDonVi, iID_MaNhomDonVi, LuyKe, iID_MaTrangThaiDuyet, KhoGiay, BaoHiem
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "BaoHiem";
        String MaND = User.Identity.Name;
        String LuyKe = Convert.ToString(ViewData["LuyKe"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String iID_MaNhomDonVi = Convert.ToString(ViewData["iID_MaNhomDonVi"]);
        String ThangQuy = Convert.ToString(ViewData["ThangQuy"]);
        
        if (String.IsNullOrEmpty(LuyKe))
        {
            LuyKe = "off";
        }
        if (String.IsNullOrEmpty(ThangQuy))
        {
            ThangQuy = "1";
        }
        String BaoHiem = Convert.ToString(ViewData["BaoHiem"]);
        if (String.IsNullOrEmpty(BaoHiem))
        {
            BaoHiem = "0";
        }
        String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
        if (String.IsNullOrEmpty(LoaiThang_Quy))
        {
            LoaiThang_Quy = "0";
        }
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        /// <summary>
        /// lấy dữ liệu cho combox đơn bị
        /// </summary>
        /// <returns></returns>
        DataTable dtDonVi = rptBH_TongQuyetToanController.HienThiDonViTheoNam(MaND, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
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
        dtDonVi.Dispose();
        DataTable dtNhomDV = rptBH_TongQuyetToanController.DS_NhomDonVi(MaND, ThangQuy, LoaiThang_Quy, iID_MaTrangThaiDuyet);
        SelectOptionList sliID_MaNhomDonVi = new SelectOptionList(dtNhomDV, "iID_MaNhomDonVi", "TenNhom");
        if (String.IsNullOrEmpty(iID_MaNhomDonVi))
        {
            if (dtNhomDV.Rows.Count > 0)
            {
                iID_MaNhomDonVi = Convert.ToString(dtNhomDV.Rows[0]["iID_MaNhomDonVi"]);
            }
            else
            {
                iID_MaNhomDonVi = Guid.Empty.ToString();
            }
        }  
        // dt Trạng Thái Duyệt
       
        DataTable dtTrangThai = rptBH_TongQuyetToanController.tbTrangThai();
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

        String iLoai = Convert.ToString(ViewData["iLoai"]);
        if (String.IsNullOrEmpty(iLoai))
        {
            iLoai = "0";
        }
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "0";
        }
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptBH_TongQuyetToan", new { MaND = MaND, ThangQuy = ThangQuy, LoaiThang_Quy = LoaiThang_Quy, iID_MaDonVi = iID_MaDonVi, iID_MaNhomDonVi = iID_MaNhomDonVi,LuyKe=LuyKe, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,  KhoGiay = KhoGiay,BaoHiem = BaoHiem ,iLoai = iLoai});
        
         // Url Action export ra file excel
        String BackURL = Url.Action("Index", "BaoHiem_Report", new { bChi = "0" });
        String urlExport = Url.Action("ExportToExcel", "rptBH_TongQuyetToan", new { MaND = MaND, ThangQuy = ThangQuy, LoaiThang_Quy = LoaiThang_Quy, iID_MaDonVi = iID_MaDonVi, iID_MaNhomDonVi = iID_MaNhomDonVi, LuyKe = LuyKe, ID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay, BaoHiem = BaoHiem, iLoai = iLoai });
        using (Html.BeginForm("EditSubmit", "rptBH_TongQuyetToan", new { ParentID = ParentID }))
        {
    %>
    
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng quyết toán</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                      <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                           <%--<div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>--%>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;">
   <div id="rptMain" style="margin:0 auto;background-color:#F0F9FE;">

    <table width="100%">
        <tr>
            <td>
                <li>
                    <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", " class=\"input1_2\" style=\"width:10%;\" onchange=chonDV()")%>Tháng
                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangQuy, "iThang", "", "class=\"input1_2\" style=\"width:17%;\"onchange=chonDV() ")%>
                    <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "class=\"input1_2\" style=\"width:10%;\" onchange=chonDV()")%>Quý
                    <%=MyHtmlHelper.DropDownList(ParentID, slQuy, ThangQuy, "iQuy", "", "class=\"input1_2\" style=\"width:17%;\" onchange=chonDV()")%><br />
                    <%=MyHtmlHelper.Option(ParentID, "2", LoaiThang_Quy, "LoaiThang_Quy", "", "class=\"input1_2\" style=\"width:10%;\" onchange=chonDV()")%>Năm<br />
                   <p style="text-align:left;"><span><%=NgonNgu.LayXau("Trạng thái duyệt:") %></span><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 150px; padding:2px;\" onchange=chonDV()")%></p>
                </li>
            </td>
            <td>
             <fieldset><legend >Loại báo cáo</legend>
                <table>
                    <tr>
                        <td>
                            <li>
                            <div class="div"><%=MyHtmlHelper.Option(ParentID, "0", BaoHiem, "BaoHiem", "", "class=\"input1_2\" style=\"width:10%;\"  ")%>BHXH</div>
                            <div class="div"><%=MyHtmlHelper.Option(ParentID, "1", BaoHiem, "BaoHiem", "", "class=\"input1_2\" style=\"width:10%;\"")%>BHYT</div>
                            <div class="div"><%=MyHtmlHelper.Option(ParentID, "2", BaoHiem, "BaoHiem", "", "class=\"input1_2\" style=\"width:10%;\"")%>BHTN</div>
                            </li>
                        </td>
                        <td >
                            <li>
                            <div class="div"><%=MyHtmlHelper.Option(ParentID, "3", BaoHiem, "BaoHiem", "", "class=\"input1_2\" style=\"width:10%;\" ")%>Tổng hợp cá nhân đóng</div>
                            <div class="div"><%=MyHtmlHelper.Option(ParentID, "4", BaoHiem, "BaoHiem", "", "class=\"input1_2\" style=\"width:10%;\" ")%>Tổng hợp đơn vị đóng</div>
                            <div class="div"><%=MyHtmlHelper.Option(ParentID, "5", BaoHiem, "BaoHiem", "", "class=\"input1_2\" style=\"width:10%;\" ")%>Tổng hợp cả 3 loại</div>
                         
                            </li>     
                        </td>
                    </tr>
                    <tr> <td> <div class="div"><%=MyHtmlHelper.CheckBox(ParentID, LuyKe, "LuyKe", "")%> &nbsp;&nbsp;&nbsp;Lũy kế đến kỳ</div></td></tr>
                </table>  
                </fieldset>         
            </td>
            <td width ="5px"></td>
            <td>
            <fieldset style=" height:75px;"><legend >Kiểu in</legend>
                <table><tr><td>
                <li>
                <%=MyHtmlHelper.Option(ParentID, "0", iLoai, "iLoai", "", "class=\"input1_2\" style=\"width:10%;\" ")%>Tổng DV<br />
                <%=MyHtmlHelper.Option(ParentID, "1", iLoai, "iLoai", "", "class=\"input1_2\" style=\"width:10%;\" ")%>Tất cả<br />
                <%=MyHtmlHelper.Option(ParentID, "2", iLoai, "iLoai", "", "class=\"input1_2\" style=\"width:10%;\"")%>Nhóm

                </li></td>
                <td width = "7px"></td>
                <td>
                <li>
                <%=MyHtmlHelper.Option(ParentID, "0", KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width:10%;\"")%>A4 - Giấy nhỏ<br />
                <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width:10%;\" ")%>A3 - Giấy to
                </li></td></tr></table>
                </fieldset>
            </td>
           
            <td>
                <table>
                    <tr>
                        <td>
                           <%=NgonNgu.LayXau("Đơn vị:") %> 
                        </td>
                        <td>
                            <li>
                            <div id="<%=ParentID%>_divDV" style="width:100%;padding-left:5px;"><%rptBH_TongQuyetToanController rpt = new rptBH_TongQuyetToanController();
                                                                                   rptBH_TongQuyetToanController.Data _Data = new rptBH_TongQuyetToanController.Data();
                                                                                    _Data=rpt.obj_DonViTheoLNS(ParentID,MaND,ThangQuy,LoaiThang_Quy, iID_MaDonVi,iID_MaNhomDonVi,iID_MaTrangThaiDuyet);
                                                                                     %> 
                                
                                <%=_Data.DV%></div>
                            </li>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <%=NgonNgu.LayXau("Nhóm đơn vị:") %> &nbsp;
                        </td>
                        <td>
                        <div id="<%=ParentID %>_divNDV" style="width:100%;padding-left:5px;"><%=_Data.NDV %></div>
                           <%-- <li><p id="divNDV">
                            <%=MyHtmlHelper.DropDownList(ParentID, sliID_MaNhomDonVi, iID_MaNhomDonVi, "iID_MaNhomDonVi", "", "class=\"input1_2\" style=\"width: 130px; padding:2px;\"")%>
                            </p></li>--%>
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
         <tr>
    <td height="27" colspan="6" align="center"><table cellpadding="0" cellspacing="0" border="0">
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

    <script type="text/javascript">
        function chonDV() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;
            var LoaiThang_QuyCheck = document.getElementById("<%= ParentID %>_LoaiThang_Quy").checked;
            var LoaiThang_Quy
            var Thang
            if (LoaiThang_QuyCheck == true) {
                Thang = document.getElementById("<%= ParentID %>_iThang").value;
                LoaiThang_Quy = 0;
            }
            else {
                Thang = document.getElementById("<%= ParentID %>_iQuy").value;
                LoaiThang_Quy = 1;
            }
            var iID_MaDonVi = document.getElementById("<%=ParentID %>_iID_MaDonVi").value;
            var iID_MaNhomDonVi = document.getElementById("<%=ParentID %>_iID_MaNhomDonVi").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&ThangQuy=#2&LoaiThang_Quy=#3&iID_MaDonVi=#4&iID_MaNhomDonVi=#5&iID_MaTrangThaiDuyet=#6", "rptBH_TongQuyetToan") %>');
            url = unescape(url.replace("#0", "<%=ParentID %>"));
            url = unescape(url.replace("#1", "<%=MaND %>"));
            url = unescape(url.replace("#2", Thang));
            url = unescape(url.replace("#3", LoaiThang_Quy));
            url = unescape(url.replace("#4", iID_MaDonVi));
            url = unescape(url.replace("#5", iID_MaNhomDonVi));
            url = unescape(url.replace("#6", iID_MaTrangThaiDuyet));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDV").innerHTML = data.DV;
                document.getElementById("<%= ParentID %>_divNDV").innerHTML = data.NDV;
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
    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
