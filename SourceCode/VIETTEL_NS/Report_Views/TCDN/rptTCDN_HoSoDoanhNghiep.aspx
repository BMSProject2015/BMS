<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.TCDN" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>HỐ SƠ DOANH NGHIỆP</title>
    <style type="text/css">
        .div-floatleft
        {                
            max-height:80px;            
        }
        .div-label
        {           
            font-size:13px;  
            padding:5px 0px;                 
        }
        .div-txt
        {
            padding-top:5px;                  
        }    
        .p
        {
            height:23px;
            line-height:23px;
            padding:1px 2px;    
        }
    </style>
    <link type="text/css" rel="Stylesheet" href="../../Content/style.css" />
    <script type="text/javascript" src="../../Scripts/jquery-ui-1.8.21.custom.min.js"></script>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TCDN";
        String Nam = Convert.ToString(ViewData["iNam"]);
        String UserID = User.Identity.Name;
        if (String.IsNullOrEmpty(Nam))
        {
            Nam = DanhMucModels.NamLamViec(UserID).ToString();
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
        String iID_MaDoanhNghiep = Convert.ToString(ViewData["iMaDN"]);  
       
        String PageLoad = Convert.ToString(ViewData["pageload"]);
        String urlReport = "";
        String URL = Url.Action("Index", "TCDN_Report");
        DataTable dtQuy = DanhMucModels.DT_Quy();
        DataRow dr1 = dtQuy.NewRow();
        dr1["MaQuy"] = "5";
        dr1["TenQuy"] = "6 tháng";
        dtQuy.Rows.Add(dr1);
        DataRow dr2 = dtQuy.NewRow();
        dr2["MaQuy"] = "6";
        dr2["TenQuy"] = "9 tháng";
        dtQuy.Rows.Add(dr2);
        DataRow dr3 = dtQuy.NewRow();
        dr3["MaQuy"] = "7";
        dr3["TenQuy"] = "Cả năm";
        dtQuy.Rows.Add(dr3);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        String iQuy = Convert.ToString(ViewData["iQuy"]);
        if (String.IsNullOrEmpty(iQuy))
        {
            iQuy = "0";
        }        
        DataTable dtDoanhNghiep = rptTCDN_HoSoDoanhNghiepController.GetDoanhNghiep(iQuy, Nam);
        if (String.IsNullOrEmpty(iID_MaDoanhNghiep))
        {           
            iID_MaDoanhNghiep = Guid.Empty.ToString();
        }
        dtDoanhNghiep.Dispose(); 
        if (PageLoad == "1")
            urlReport = Url.Action("ViewPDF", "rptTCDN_HoSoDoanhNghiep", new { iQuy = iQuy, iNam = Nam, iMaDN = iID_MaDoanhNghiep });
        using (Html.BeginForm("EditSubmit", "rptTCDN_HoSoDoanhNghiep", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo hồ sơ doanh nghiệp</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:850px; max-width:850px; margin:0px auto; padding:0px 0px; overflow:visible;">                
                <ul class="inlineBlock">
                    <li>                        
                        <span style="float:left; text-align:center; width:100px;"><%=NgonNgu.LayXau("Chọn quý")%></span>
                        <span style="float:right; text-align:center; width:100px;"><%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\" onchange=\"ChonDoanhNghiep()\"")%></span>                        
                    </li>
                    <li>                       
                        <span style="float:left; text-align:center; width:100px;"><%=NgonNgu.LayXau("Chọn năm")%></span>
                        <span style="float:right; text-align:center; width:100px;"><%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "iNamLamViec", "", "class=\"input1_2\" style=\"width:100%; padding:2px;\" onchange=\"ChonDoanhNghiep()\"")%></span>                        
                    </li>
                    <li>                       
                        <span style="float:left; text-align:center; width:130px;"><%=NgonNgu.LayXau("Chọn đơn vị báo cáo")%></span>
                        <span style="float:right; text-align:center; width:140px;" id="<%=ParentID %>_divDoanhNghiep"><% rptTCDN_HoSoDoanhNghiepController rpt = new rptTCDN_HoSoDoanhNghiepController();%><%=rpt.obj_DSDoanhNghiep(ParentID,iQuy,Nam,iID_MaDoanhNghiep) %></span>                         
                    </li>                    
                </ul>
                <div id="both" style="clear:both; min-height:30px; line-height:30px; margin-bottom:-5px; ">
                    <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 5px auto; width:200px;">
                        <tr>
                            <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                            <td width="5px"></td>
                            <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                        </tr>
                    </table>   
                </div>
            </div>
        </div>
    </div>
    <%} %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        function ChonDoanhNghiep() {
            var Nam = document.getElementById("<%=ParentID %>_iNamLamViec").value
            var Quy = document.getElementById("<%=ParentID %>_iQuy").value            
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDoanhNghiep?ParentID=#0&Quy=#1&Nam=#2&iMaDN=#3", "rptTCDN_HoSoDoanhNghiep") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", Quy));
            url = unescape(url.replace("#2", Nam));            
            url = unescape(url.replace("#3", "<%= iID_MaDoanhNghiep %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDoanhNghiep").innerHTML = data;
            });
        }
        $(document).ready(function () {
            $("select option:contains('4')").css("border-bottom", "1px dashed #EB8A36");
            $("select option:contains('Cả năm')").css("border-top", "1px dashed #EB8A36");
            $("select#<%=ParentID %>_iQuy option").css("padding", "0px 2px");
        });    
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTCDN_HoSoDoanhNghiep", new { iQuy = iQuy, iNam = Nam, iMaDN = iID_MaDoanhNghiep }), "Xuất ra excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
