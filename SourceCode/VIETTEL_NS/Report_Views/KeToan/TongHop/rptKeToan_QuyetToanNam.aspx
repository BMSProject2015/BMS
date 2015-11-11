<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
            height:50px;
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
            background-position: 20px 1px;
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
        String ParentID = "KeToan";
        String MaND = User.Identity.Name;
        DataTable dtND = NguoiDungCauHinhModels.LayCauHinh(MaND);
        String NamLamViec = Convert.ToString(ViewData["NamLamViec"]);
        String NamLamViec_LamViec = Convert.ToString(ViewData["NamLamViec_LamViec"]);
        if (String.IsNullOrEmpty(NamLamViec))
        {
            NamLamViec = Convert.ToString(Convert.ToInt32(dtND.Rows[0]["iNamLamViec"]) - 1);
        }
        if (String.IsNullOrEmpty(NamLamViec_LamViec))
        {
            NamLamViec_LamViec = Convert.ToString(dtND.Rows[0]["iNamLamViec"]);
        }

       
       
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();
        String ThangLamViec = Convert.ToString(ViewData["ThangLamViec"]);
        String iNgay = Convert.ToString(ViewData["iNgay"]);
        if (String.IsNullOrEmpty(ThangLamViec)) ThangLamViec = Convert.ToString(dtND.Rows[0]["iThangLamViec"]);

        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        ///
    
        DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToInt16(ThangLamViec), Convert.ToInt16(NamLamViec));
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        if (String.IsNullOrEmpty(iNgay))
        {
            iNgay = Convert.ToString(dtNgay.Rows[dtNgay.Rows.Count - 1]["MaNgay"]);
        }
        if (dtNgay != null)
        {
            dtNgay.Dispose();
        }
        String SQL = "SELECT sThamSo FROM KT_DanhMucThamSo WHERE sKyHieu='202' AND iNamLamViec='" + NamLamViec + "'";
        String dsTaiKhoan = Connection.GetValueString(SQL, "-1");
        DataTable dtTaiKhoan = new DataTable();

        SQL = "SELECT iID_MaTaiKhoan+'-'+sTen as TenHT,* FROM KT_TaiKhoan WHERE iTrangThai=1 AND iNam='" + NamLamViec + "' AND iID_MaTaiKhoan IN (" + dsTaiKhoan + ")";
        dtTaiKhoan = Connection.GetDataTable(SQL);
        String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
        if (String.IsNullOrEmpty(iID_MaTaiKhoan))
        {
            iID_MaTaiKhoan = "01";
        }
        SelectOptionList slTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "TenHT");
        dtTaiKhoan.Dispose();
        String TrangThai = Convert.ToString(ViewData["TrangThai"]);
        if (String.IsNullOrEmpty(TrangThai))
        {
            TrangThai = "0";
        }
        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "--Tất cả--");
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String URL = Url.Action("SoDoLuong", "KeToanTongHop");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptKeToan_QuyetToanNam", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, TrangThai = TrangThai, iNgay = iNgay, NamLamViec_LamViec = NamLamViec_LamViec });
        using (Html.BeginForm("EditSubmit", "rptKeToan_QuyetToanNam", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Báo cáo quyết toán năm KH 202 </span>
                    </td>
                    <td width="52%" style="text-align: left;">
                    </td>
                </tr>
            </table>
        </div>
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width: 100%; margin: 5px auto; padding: 0px 0px; overflow: visible;">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="5%">
                            &nbsp;
                        </td>
                        <td width="150px" align="right">
                            <b>Năm quyết toán:&nbsp;</b>
                        </td>
                        <td width="100px" align="right">
                            <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "NamLamViec", "", "class=\"input1_2\" style=\"width: 100%\" ")%>
                        </td>
                        <td align="right" width="150px">
                            <b>Số liệu tính đến ngày: </b>
                        </td>
                        <td width="80px" id="<%= ParentID %>_ngay1">
                            <% rptKeToan_QuyetToanNamController rpt = new rptKeToan_QuyetToanNamController();%>
                            <%= rpt.get_sNgayThang(ParentID, MaND, ThangLamViec, iNgay, NamLamViec)
                            %>
                        </td>
                        <td class="td_form2_td1" width="40px" align="right">
                            <b>Tháng:&nbsp;</b>
                        </td>
                        <td width="70px" align="right">
                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangLamViec, "ThangLamViec", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonThang(this.value)\"")%>
                        </td>
                         <td width="40px" align="right">
                            <b>Năm:&nbsp;</b>
                        </td>
                        <td width="100px" align="right">
                            <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec_LamViec, "NamLamViec_LamViec", "", "class=\"input1_2\" style=\"width: 100%\" ")%>
                        </td>
                        <td class="td_form2_td1" width="70px" align="right">
                            <b>Tài khoản</b>
                        </td>
                        <td width="350px" align="right">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="70px" align="right">
                            <b>Trạng thái:</b>
                        </td>
                        <td width="100px" align="right">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, TrangThai, "TrangThai", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                       <td width="5%">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="11">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        function ChonThang(value) {

            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&MaND=#1&iThang=#2&iNgay=#3&iNamLamViec=#4","rptKeToan_QuyetToanNam") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%= MaND %>"));
            url = unescape(url.replace("#2", value));
            url = unescape(url.replace("#3", "<%= iNgay %>"));

            url = unescape(url.replace("#4", "<%= NamLamViec_LamViec %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID%>_ngay1").innerHTML = data;
            });
        }        
       
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKeToan_QuyetToanNam", new { NamLamViec = NamLamViec, ThangLamViec = ThangLamViec, iID_MaTaiKhoan = iID_MaTaiKhoan, TrangThai = TrangThai, iNgay = iNgay, NamLamViec_LamViec = NamLamViec_LamViec }), "Xuất ra Excel")%>
    <%} %>
    <iframe src="<%=UrlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
