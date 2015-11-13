<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        ul.inlineBlock li fieldset legend{text-align:left; padding:3px 6px;font-size:13px;}  
        ul.inlineBlock li fieldset p{padding:2px 4px; text-align:left;}
    </style>
    <link rel="Stylesheet" type="text/css" href="../../../Content/Report_Style/ReportCSS.css" />
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%   String srcFile = Convert.ToString(ViewData["srcFile"]);
         String ParentID = "KeToan";
         String UserID = User.Identity.Name;
         String NamChungTu = Convert.ToString(ViewData["NamChungTu"]);
         if (String.IsNullOrEmpty(NamChungTu))
         {
             NamChungTu = Convert.ToString(NguoiDungCauHinhModels.iNamLamViec);
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
         dtNam.Rows[0]["TenNam"] = "-- Chọn năm --";
         SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        //Ngay Thang
        
        
        
         DataTable dtThang = DanhMucModels.DT_Thang(false);
         String TuNgay = Convert.ToString(ViewData["TuNgay"]);
         String DenNgay = Convert.ToString(ViewData["DenNgay"]);
         String TuThang = Convert.ToString(ViewData["TuThang"]);
         String DenThang = Convert.ToString(ViewData["DenThang"]);
         if (String.IsNullOrEmpty(TuNgay))
             TuNgay = "1";
         if (String.IsNullOrEmpty(TuThang))
             TuThang = DanhMucModels.ThangLamViec(UserID).ToString();
         if (String.IsNullOrEmpty(DenNgay))
             DenNgay = "28";
         if (String.IsNullOrEmpty(DenThang))
             DenThang = DanhMucModels.ThangLamViec(UserID).ToString();
         SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        
        
         // Tên tài khoản
         String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
         var dtiID_MaTaiKhoan = rptKT_SoQuyController.TenTaiKhoan(NamChungTu);
         SelectOptionList slTaiKhoan = new SelectOptionList(dtiID_MaTaiKhoan, "iID_MaTaiKhoan", "TenTK");
         if (String.IsNullOrEmpty(iID_MaTaiKhoan))
         {
             if (dtiID_MaTaiKhoan.Rows.Count > 0)
             {
                 iID_MaTaiKhoan = Convert.ToString(dtiID_MaTaiKhoan.Rows[0]["iID_MaTaiKhoan"]);
             }
             else
             {
                 iID_MaTaiKhoan = "";
             }
         }
         String BackURL = Url.Action("Index", "KeToan_Report");
         DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
         SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
         String iTrangThai = Convert.ToString(Request.QueryString["iID_MaTrangThaiDuyet"]);
         if (String.IsNullOrEmpty(iTrangThai))
             iTrangThai = dtTrangThai.Rows.Count > 0 ? Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]) : Guid.Empty.ToString();   
         String urlExport = Url.Action("ExportToExcel", "rptKT_SoQuy", new { NamChungTu = NamChungTu, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang, iID_MaTaiKhoan = iID_MaTaiKhoan });
         String URLView = "";
         String PageLoad = Convert.ToString(ViewData["PageLoad"]);
         if (PageLoad == "1")
             URLView = Url.Action("ViewPDF", "rptKT_SoQuy", new { NamChungTu = NamChungTu, TuNgay = TuNgay, DenNgay = DenNgay, TuThang = TuThang, DenThang = DenThang, iID_MaTaiKhoan = iID_MaTaiKhoan });
        using (Html.BeginForm("EditSubmit", "rptKT_SoQuy", new { ParentID = ParentID}))
         {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo sổ quỹ</span>
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
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Khoảng thời gian cần xem") %></legend>
                          <%--  <p><%=NgonNgu.LayXau("Từ ngày&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đến ngày&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tháng")%></p>
                            <p style="margin-bottom:10px;">
                                <span id="<%= ParentID %>_divTuNgay"><% rptKT_SoQuyController rpt = new rptKT_SoQuyController(); %><%=rpt.obj_DSNgay(ParentID, TuThang, NamChungTu, TuNgay, "TuNgay")%></span>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, TuThang, "TuThang", "", "class=\"input1_2\" style=\"width: 60px;\" onchange=\"TuNgay()\"")%>
                                 <span id="<%= ParentID %>_divDenNgay"><%=rpt.obj_DSNgay(ParentID, DenThang, NamChungTu, DenNgay, "DenNgay")%></span> 
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, DenThang, "DenThang", "", "class=\"input1_2\" style=\"width: 60px;\"onchange=\"DenNgay()\"")%>
                            </p>--%>
                            <table border="0" cellspacing="0" cellpadding="0" width="300px">
                            	<tr>
                            		<td width="70px">Từ ngày&nbsp;&nbsp;&nbsp;</td>
                                    <td width="70px">Đến ngày&nbsp;&nbsp;&nbsp;</td>
                                    <td width="70px">Từ tháng&nbsp;&nbsp;&nbsp;</td>
                                    <td width="70px">Đến tháng&nbsp;&nbsp;&nbsp;</td>
                            	</tr>
                                <tr>
                                <td>  <span id="<%= ParentID %>_divTuNgay"><% rptKT_SoQuyController rpt = new rptKT_SoQuyController(); %><%=rpt.obj_DSNgay(ParentID, TuThang, NamChungTu, TuNgay, "TuNgay")%></span></td>
                                <td><%=MyHtmlHelper.DropDownList(ParentID, slThang, TuThang, "TuThang", "", "class=\"input1_2\" style=\"width: 60px;\" onchange=\"TuNgay()\"")%></td>
                                <td>  <span id="<%= ParentID %>_divDenNgay"><%=rpt.obj_DSNgay(ParentID, DenThang, NamChungTu, DenNgay, "DenNgay")%></span> </td>
                                <td> <%=MyHtmlHelper.DropDownList(ParentID, slThang, DenThang, "DenThang", "", "class=\"input1_2\" style=\"width: 60px;\"onchange=\"DenNgay()\"")%></td>
                                </tr>
                            </table>
                        </fieldset>
                    </li>
                    <li>
                        <fieldset>
                            <legend><%=NgonNgu.LayXau("Chọn tài khoản:") %></legend>
                            <p>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 300px;\" size='3' tab-index='-1'")%>
                            </p>
                        </fieldset>
                    </li>
                    
                </ul><!--End .inlineBlock-->
                <%=MyHtmlHelper.Hidden(ParentID,NamChungTu,"NamChungTu","") %>
                <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
            </div><!--End #rptMain-->
        </div>
        <script type="text/javascript">
            $(function () {               
                $('div.login1 a').click(function () {
                    $('div#rptMain').slideToggle('Fast');
                    $(this).toggleClass('active');
                    return false;
                });
            });
            function Huy() {
                window.location.href = '<%=BackURL %>';
            }
        </script>
         <script type="text/javascript">
             function ChonNgay(idNgay, idThang, divNgay, FromOrTo) 
             {
                 var Nam = document.getElementById("<%=ParentID %>_NamChungTu").value
                 var Thang = document.getElementById("<%=ParentID %>_" + idThang).value
                 jQuery.ajaxSetup({ cache: false });
                 var url = unescape('<%= Url.Action("Get_dsNgay?ParentID=#0&iThang=#1&iNam=#2&iNgay=#3&FromOrTo=#4", "rptKT_SoQuy") %>');
                 url = unescape(url.replace("#0", "<%= ParentID %>"));
                 url = unescape(url.replace("#1", Thang));
                 url = unescape(url.replace("#2", Nam));
                 url = unescape(url.replace("#3", idNgay));
                 url = unescape(url.replace("#4", FromOrTo));
                 $.getJSON(url, function (data) {
                     document.getElementById("<%= ParentID %>_" + divNgay).innerHTML = data;
                 });
             }
             function TuNgay() {
                 ChonNgay("<%=TuNgay %>", "TuThang", "divTuNgay", "TuNgay");
             }
             function DenNgay() {
                 ChonNgay("<%=DenNgay %>", "DenThang", "divDenNgay", "DenNgay");
             }
    </script>
        <div>
            <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
        </div>
    </div>
    <%} %>
    <iframe src="<%=URLView%>"
        height="600px" width="100%"></iframe>
</body>
</html>